import type { IMatch } from "./IMatch";
import type { IAppUser } from "./IPerson";

export interface IPersonInMatch{
    id? : string;
    appUserId: string;
    appUser?: IAppUser
    matchId: string;
    match?: IMatch;
    totalPoints: number | null;
    aces: number | null;
    faults: number | null;
    reception: number | null;
}

export const InitialPersonInMatch : IPersonInMatch = {
    appUserId: "",
    matchId: "",
    totalPoints: null,
    aces: null,
    faults: null,
    reception: null
}