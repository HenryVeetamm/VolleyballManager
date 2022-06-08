import type { IAppUser } from "./IPerson"
import type { IWorkout } from "./IWorkout"

export interface IPersonInWorkout{
    id? : string
    workOutId : string
    workout?: IWorkout
    appUserId : string
    appUser? : IAppUser
    comment: string | null
}

export const InitialPersonInWorkout : IPersonInWorkout = {
    workOutId: "",
    appUserId: "",
    comment: null
}