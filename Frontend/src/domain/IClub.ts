export interface IClub{
    id? : string;
    name : string
    ownClub: boolean
}

export const InitialClub : IClub = {
    name: "",
    ownClub: false
}