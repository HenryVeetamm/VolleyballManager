using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<WorkoutType> WorkoutTypes { get; set; } = default!;
    public DbSet<RolesInTeam> RolesInTeams { get; set; } = default!;
    public DbSet<Club> Clubs { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
    public DbSet<SavedComparison> SavedComparisons { get; set; } = default!;
    public DbSet<PersonInClub> PersonInClubs { get; set; } = default!;
    public DbSet<PersonInTeam> PersonInTeams { get; set; } = default!;
    public DbSet<Workout> Workouts { get; set; } = default!;
    public DbSet<PersonInWorkout> PersonInWorkouts { get; set; } = default!;
    public DbSet<Match> Matches { get; set; } = default!;
    public DbSet<PersonInMatch> PersonInMatches { get; set; } = default!;
    public DbSet<Announcement> Announcements { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            builder
                .Entity<RolesInTeam>()
                .Property(e => e.RoleDescription)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v));
            builder
                .Entity<WorkoutType>()
                .Property(e => e.Description)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v));
        }
        //Remove cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
    
    private static string SerialiseLangStr(LangStr lStr) => System.Text.Json.JsonSerializer.Serialize(lStr);

    private static LangStr DeserializeLangStr(string jsonStr) =>
        System.Text.Json.JsonSerializer.Deserialize<LangStr>(jsonStr) ?? new LangStr();
    

    public override int SaveChanges()
    {
        FixEntities(this);
        
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);

        return base.SaveChangesAsync(cancellationToken);
    }


    private void FixEntities(AppDbContext context)
    {
        
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);
        

        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
    }

}