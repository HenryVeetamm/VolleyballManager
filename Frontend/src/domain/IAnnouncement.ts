import type { IAppUser } from "./IPerson";
import type { ITeam } from "./ITeam";
export interface IAnnouncement{
    id? : string;
    title: string;
    content: string
    pinned: boolean
    teamId: string | null
    appUser? : IAppUser 
    team? : ITeam 
}

export const InitialAnnouncement : IAnnouncement = {
    title: "",
    content: "",
    pinned: false,
    teamId: null,
}