import type { IAppUser } from "./IPerson";
import type { IRolesInTeam } from "./IRolesInTeam";
import type { ITeam } from "./ITeam";

export interface IPersonInTeam {
    id? : string
    appUserId : string
    appUser? : IAppUser
    teamId : string
    team? : ITeam
    rolesInTeamId : string
    rolesInTeam? : IRolesInTeam
}