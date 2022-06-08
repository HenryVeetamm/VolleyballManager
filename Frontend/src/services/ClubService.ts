
import type { IClub } from "@/domain/IClub";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";

import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";

export class ClubService extends BaseService<IClub>{

    constructor() {
        super("club");
    }

   

    async getUserOpponentClubs(): Promise<IClub[]> {

        let response;
        try {
            response = await httpClient.get("/club/opponentClubs",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
            let res = response.data as IClub[]
            return res;

        }
        catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                response = await httpClient.get("/club/opponentClubs",
                    {
                        headers: {
                            "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                        }
                    })

                let res = response.data as IClub[];
                return res;

            }
        }
        return [];
    }

    async getUserClubs(): Promise<IClub[]> {

        let response;
        try {
            response = await httpClient.get("/club/ownClubs",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
            let res = response.data as IClub[]
            return res;

        }
        catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                response = await httpClient.get("/club/ownClubs",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })

                let res = response.data as IClub[];
                return res;

            }
        }
        return [];
    }

}