
import type { IMatch } from "@/domain/IMatch";

import httpClient from "@/http-client";

import { BaseService } from "./BaseService";

export class MatchService extends BaseService<IMatch>{
    
    constructor() {
        super("match");
        
    }
}