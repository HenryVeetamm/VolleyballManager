import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import AnnouncementIndex from "@/views/announcement/AnnouncementIndex.vue";
import AnnouncementCreate from "@/views/announcement/AnnouncementCreate.vue";
import Login from "@/views/identity/Login.vue";
import Register from "@/views/identity/Register.vue";
import ClubIndex from "@/views/club/ClubIndex.vue";
import AnnouncementDetails from "@/views/announcement/AnnouncementDetails.vue";
import AnnouncementDelete from "@/views/announcement/AnnouncementDelete.vue";
import AnnouncementEdit from "@/views/announcement/AnnouncementEdit.vue";
import ClubCreate from "@/views/club/ClubCreate.vue";
import ClubDelete from "@/views/club/ClubDelete.vue";
import ClubDetails from "@/views/club/ClubDetails.vue";
import ClubEdit from "@/views/club/ClubEdit.vue";
import TeamIndex from "@/views/team/TeamIndex.vue";
import TeamCreate from "@/views/team/TeamCreate.vue";
import TeamDelete from "@/views/team/TeamDelete.vue";
import TeamDetails from "@/views/team/TeamDetails.vue";
import TeamEdit from "@/views/team/TeamEdit.vue";
import PersonInTeamCreate from "@/views/personInTeam/PersonInTeamCreate.vue";
import PersonInClubCreate from "@/views/personInClub/PersonInClubCreate.vue";
import WorkoutIndex from "@/views/workout/WorkoutIndex.vue"
import WorkoutDetails from "@/views/workout/WorkoutDetails.vue"
import WorkoutEdit from "@/views/workout/WorkoutEdit.vue"
import WorkoutCreate from "@/views/workout/WorkoutCreate.vue"
import PersonInWorkoutCreate from "@/views/personInWorkout/PersonInWorkoutCreate.vue";
import PersonInWorkoutDetails from "@/views/personInWorkout/PersonInWorkoutDetails.vue";
import MatchIndex from "@/views/match/MatchIndex.vue"
import MatchCreate from "@/views/match/MatchCreate.vue";
import MatchEdit from "@/views/match/MatchEdit.vue";
import MatchDetails from "@/views/match/MatchDetails.vue";
import PersonInMatchCreate from "@/views/personInMatch/PersonInMatchCreate.vue";
import PersonInMatchDetails from "@/views/personInMatch/PersonInMatchDetails.vue";
import SavedComparisonIndex from "@/views/savedComparison/SavedComparisonIndex.vue";
import SavedComparisonCreate from "@/views/savedComparison/SavedComparisonCreate.vue";
import SavedComparisonDetail from "@/views/savedComparison/SavedComparisonDetail.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: "/", name: "home", component: HomeView },
    { path: "/identity/account/login", name: "login", component: Login },
    { path: "/identity/account/register", name: "register", component: Register },

    { path: "/announcements", name: "announcement", component: AnnouncementIndex },
    { path: "/announcement/create", name: "AnnouncementCreate", component: AnnouncementCreate },
    { path: "/announcement/details", name: "AnnouncementDetails", component: AnnouncementDetails, props: true },
    { path: "/announcement/delete", name: "AnnouncementDelete", component: AnnouncementDelete, props: true },
    { path: "/announcement/edit", name: "AnnouncementEdit", component: AnnouncementEdit, props: true },

    { path: "/clubs", name: "club", component: ClubIndex },
    { path: "/club/create", name: "ClubCreate", component: ClubCreate },
    { path: "/club/delete", name: "ClubDelete", component: ClubDelete, props: true },
    { path: "/club/details", name: "ClubDetails", component: ClubDetails, props: true },
    { path: "/club/edit", name: "ClubEdit", component: ClubEdit, props: true },
    
    { path: "/teams", name: "Team", component: TeamIndex },
    { path: "/team/create", name: "TeamCreate", component: TeamCreate },
    { path: "/team/delete", name: "TeamDelete", component: TeamDelete, props: true },
    { path: "/team/details", name: "TeamDetails", component: TeamDetails, props: true },
    { path: "/team/edit", name: "TeamEdit", component: TeamEdit, props: true },

    { path: "/personinteam/create", name: "PersonInTeamCreate", component: PersonInTeamCreate, props: true },

    { path: "/personinclub/create", name: "PersonInClubCreate", component: PersonInClubCreate, props: true },

    { path: "/workouts", name: "WorkoutIndex", component: WorkoutIndex, props: true },
    { path: "/workout/create", name: "WorkoutCreate", component: WorkoutCreate },
    { path: "/workout/details", name: "WorkoutDetails", component: WorkoutDetails, props: true },
    { path: "/workout/edit", name: "WorkoutEdit", component: WorkoutEdit, props: true },

    { path: "/personinworkout/create", name: "PersonInWorkoutCreate", component: PersonInWorkoutCreate, props: true },
    { path: "/personinworkout/details", name: "PersonInWorkoutDetails", component: PersonInWorkoutDetails, props: true },

    { path: "/matches", name: "MatchIndex", component: MatchIndex, props: true },
    { path: "/match/create", name: "MatchCreate", component: MatchCreate, props: true },
    { path: "/match/edit", name: "MatchEdit", component: MatchEdit, props: true },
    { path: "/match/details", name: "MatchDetails", component: MatchDetails, props: true },

    { path: "/personinmatch/create", name: "PersonInMatchCreate", component: PersonInMatchCreate, props: true },
    { path: "/personinmatch/details", name: "PersonInMatchDetails", component: PersonInMatchDetails, props: true },

    { path: "/savedcomparisons", name: "SavedComparisonIndex", component: SavedComparisonIndex},
    { path: "/savedcomparisons/create", name: "SavedCompairsonCreate", component: SavedComparisonCreate, props: true },
    { path: "/savedcomparisons/details", name: "SavedComparisonDetails", component: SavedComparisonDetail, props: true },
    
  ]
});

export default router;
