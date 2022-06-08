export interface IAppUser{
    id? : string,
    firstName : string,
    lastName : string,
    nationalCode?: string
}

export const InitialAppUser : IAppUser = {
    id : "",
    firstName: "",
    lastName: ""
}

export interface IAppUserSimple {
    firstName: string,
    lastName: string
}

export const InitialAppUserSimple : IAppUserSimple = {
    firstName: "",
    lastName: ""
}