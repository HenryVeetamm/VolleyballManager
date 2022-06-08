
import type { ITeam } from "@/domain/ITeam";
import type { IWorkout } from "@/domain/IWorkout";
import httpClient from "@/http-client";

import { BaseService } from "./BaseService";

export class WorkoutService extends BaseService<IWorkout>{
    /**
     *
     */
    constructor() {
        super("workout");
        
    }
 
}