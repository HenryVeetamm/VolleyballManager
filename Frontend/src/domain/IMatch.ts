import type { IAppUser } from "./IPerson";
import type { ITeam } from "./ITeam";

export interface IMatch{
    id? : string,
    homeTeamId: string,
    homeTeam?: ITeam,
    awayTeamId: string,
    awayTeam?: ITeam,
    matchDate: string,
    matchScore: string,
    victory?: boolean,
    appUserId?: string,
    appUser?: IAppUser
}

export const InitialMatch: IMatch = {
    homeTeamId: "",
    awayTeamId: "",
    matchDate: "",
    matchScore: ""
}