
import type { ITeam } from "@/domain/ITeam";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";

import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class TeamService extends BaseService<ITeam>{
    /**
     *
     */
    constructor() {
        super("team");

    }
    async getOwnTeams(): Promise<ITeam[]> {
        let response;
        try {
            response = await httpClient.get("/team/ownTeams",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })

            let res = response.data as ITeam[]
            return res;
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];

                response = await httpClient.get("/team/ownTeams",
                    {
                        headers: {
                            "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                        }
                    })

                let res = response.data as ITeam[]
                return res;

            }
        }
        return [];
    }

    async getOpponentTeams(): Promise<ITeam[]> {
        let response;
        try {
            response = await httpClient.get("/team/opponentTeams",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
            
            let res = response.data as ITeam[]
            return res;
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];

                response = await httpClient.get("/team/opponentTeams",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
            
            let res = response.data as ITeam[]
            return res;

            }
        }
        return[]
    }
}