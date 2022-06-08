import type { IClub } from "./IClub"

export interface ITeam{
    id? : string
    name : string,
    code : string,
    clubId : string
    club? : IClub
    ownTeam : boolean
}

export const InitialTeam : ITeam = {
    name: "",
    code: "",
    clubId: "",
    ownTeam: false
}