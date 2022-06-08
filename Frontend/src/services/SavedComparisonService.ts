

import type { ISavedComparison, ISavedComparisonDetailed } from "@/domain/ISavedComparison";

import httpClient from "@/http-client";
import type { AxiosError } from "axios";

import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class SavedComparisonService extends BaseService<ISavedComparison>{
    /**
     *
     */
    constructor() {
        super("savedcomparison");
    }

    async getDetailedById(id: string): Promise<ISavedComparisonDetailed[]> {
        let response;
        try{
            response = await httpClient.get(`savedcomparison/${id}`,  {headers: {
                "Authorization": "bearer " + this.identityStore.$state.jwt?.token
            } })
            
            let res = response.data as ISavedComparisonDetailed[]
            return res;
        }catch(e){
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                response = await httpClient.get(`savedcomparison/${id}`,  {headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                } })
                
                let res = response.data as ISavedComparisonDetailed[]
                return res;

            }
        }
        return [];
       
    }
  
}