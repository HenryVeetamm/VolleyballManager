import type { IWorkoutType } from "./IWorkoutType"

export interface IWorkout{
    id? : string
    workoutTypeId : string
    workoutType?: IWorkoutType | null
    description: string,
    date : string 
}

export const InitialWorkout : IWorkout = {
    workoutTypeId: "",
    workoutType: null,
    description: "",
    date: ""
}