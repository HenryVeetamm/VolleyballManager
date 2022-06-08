export interface IRegistration{
    email : string,
    nationalcode : string,
    firstname: string,
    lastname: string,
    birthday: string,
    gender: number | null,
    password: string,
    role: string | null,
}

export const InitialRegistration : IRegistration = {
    email: "",
    nationalcode: "",
    firstname: "",
    lastname: "",
    birthday: "",
    password: "",
    role: null,
    gender: null
}