
import type { ITeam } from "@/domain/ITeam";
import type { IWorkout } from "@/domain/IWorkout";
import type { IWorkoutType } from "@/domain/IWorkoutType";
import httpClient from "@/http-client";

import { BaseService } from "./BaseService";

export class WorkoutTypeService extends BaseService<IWorkoutType>{
    /**
     *
     */
    constructor() {
        super("workouttype");
        
    }
}