


import type { IPersonInClub } from "@/domain/IPersonInClub";
import type { IPersonInMatch } from "@/domain/IPersonInMatch";
import type { IPersonInTeam } from "@/domain/IPersonInTeam";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";
import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class PersonInMatchService extends BaseService<IPersonInMatch>{
    /**
     *
     */
    constructor() {
        super("personinmatch");

    }

    async getMembersOfMatch(id: string): Promise<IPersonInMatch[]> {
        let response;

        try {
            let response = await httpClient.get(`personinmatch/matchmembers/${id}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            })
            let res = response.data as IPersonInMatch[]
            return res;
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                response = await httpClient.get(`personinmatch/matchmembers/${id}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
                let res = response.data as IPersonInMatch[]
                return res;
            }
        }
        return [];

    }


}