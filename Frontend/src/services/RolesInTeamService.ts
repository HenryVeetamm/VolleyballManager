
import type { IRolesInTeam } from "@/domain/IRolesInTeam";
import type { ITeam } from "@/domain/ITeam";
import httpClient from "@/http-client";

import { BaseService } from "./BaseService";

export class RolesInTeamService extends BaseService<IRolesInTeam>{
    /**
     *
     */
    constructor() {
        super("roleinteam");
        
    }
  
}