import { InitialAppUserSimple, type IAppUser, type IAppUserSimple } from "./IPerson";

export interface ISavedComparison {
    id?: string,
    comparerId: string,
    comparer?: IAppUser,
    comparableId: string,
    comparable?: IAppUser
}


export interface ISavedComparisonDetailed {
    comparer: IAppUserSimple
    totalMatches: number
    points: number
    aces: number
    faults: number
    reception: number
}

export const InitialSavedComparisonDetailed : ISavedComparisonDetailed = {
    comparer: InitialAppUserSimple,
    totalMatches: 0,
    points: 0,
    aces: 0,
    faults: 0,
    reception: 0
}