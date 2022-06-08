using App.Domain;
using App.Domain.Enums;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.EF.AppData;

public class InitialData
{
    public static string[] Roles { get; } = { "Coach", "Admin", "Player" };

    public static readonly (string email, string FirstName, string LastName,
        string Birthday, EGender Gender, string nationalCode, string password, string? role)[] Users =
        {
            ("coach1@ttu.ee", "coach1", "coach1", "2022-03-18", 0, "39904280843", "Admin,1", "Coach"),
            ("coach2@ttu.ee", "coach2", "coach2", "2022-03-18", 0, "39904280843", "Admin,1", "Coach"),
            ("player1@ttu.ee", "player1", "player1", "2022-03-18", 0, "39904280843", "Admin,1", "Player"),
            ("player2@ttu.ee", "player2", "player2", "2022-03-18", 0, "39904280843", "Admin,1", "Player"),
            ("admin@admin.ee", "admin", "admin", "2022-03-18", 0, "00000000000", "Admin,1", "Admin"),
            ("egon@ttu.ee", "Egon", "Vaiksaar", "2021-03-18", 0, "39602301753", "Admin,1", "Player"),
        };

    public static readonly (string en, string et)[] WorkoutTypes =
    {
        ("Cardio", "Vastupidavus"),
        ("Gym", "Jõusaal"),
        ("Yoga", "Jooga"),
        ("Ball training", "Treening pallidega"),
        ("Other", "Muu")
    };
    

    public static readonly (string en, string et)[] RolesInTeams =
    { 
        ("Libero", "Libero"),
        ("Middle blocker", "Tempo"),
        ("Outside hitter", "Nurgaründaja"),
        ("Rightside hitter", "Diagonaalründaja"),
        ("Setter", "Sidemängija"),
        ("General", "Üldine mängija"),
        ("Coach", "Treener")
    };
}