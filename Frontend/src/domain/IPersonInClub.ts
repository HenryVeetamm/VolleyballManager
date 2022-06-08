import type { IClub } from "./IClub";
import type { IAppUser } from "./IPerson";

export interface IPersonInClub{
    id? : string,
    clubId : string;
    club? : IClub,
    appUserId : string
    appUser? : IAppUser
}