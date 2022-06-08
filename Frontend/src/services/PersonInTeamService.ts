


import type { IPersonInClub } from "@/domain/IPersonInClub";
import type { IPersonInTeam } from "@/domain/IPersonInTeam";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";
import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class PersonInTeamService extends BaseService<IPersonInTeam>{
    /**
     *
     */
    constructor() {
        super("personinteam");

    }

    async getMembersOfTeam(id: string): Promise<IPersonInTeam[]> {
        let response;
        try {
            let response = await httpClient.get(`personinteam/${id}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            })
            let res = response.data as IPersonInTeam[]
            return res;

        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                let response = await httpClient.get(`personinteam/${id}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
                let res = response.data as IPersonInTeam[]
                return res;

            }

        }
        return [];
    }


}