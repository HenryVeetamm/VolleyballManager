


import type { IPersonInClub } from "@/domain/IPersonInClub";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";
import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class PersonInClubService extends BaseService<IPersonInClub>{
    /**
     *
     */
    constructor() {
        super("personinclub");
        
    }
    
    async GetMembersOfClub(id : string) : Promise<IPersonInClub[]>{

        let response;
        try{
            let response = await httpClient.get(`/PersonInClub/getclubmembers/${id}`,
        {headers: {
            "Authorization": "bearer " + this.identityStore.$state.jwt?.token
        } })
        let res = response.data as IPersonInClub[]
        return res
        }
        catch(e){
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                response = await httpClient.get(`/PersonInClub/getclubmembers/${id}`,
                {headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                } })
                let res = response.data as IPersonInClub[]
                return res

            }
        }
        return [];

    }
}