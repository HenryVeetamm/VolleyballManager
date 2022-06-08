
import type { IAppUser } from "@/domain/IPerson";


import { BaseService } from "./BaseService";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";
import { IdentityService } from "./IdentityService";

export class UserService extends BaseService<IAppUser>{
    /**
     *
     */
    constructor() {
        super("team");

    }
    // async getAll(): Promise<IAnnouncement[]> {
    //     console.log("Servuce enne")
    //     let response = await httpClient.get("/Announcement")
    //     console.log("Servuce")
    //     console.log(response)
    //     let res = response.data as IAnnouncement[]
    //     return res;
    // }

    async getPlayers(): Promise<IAppUser[]> {
        let response;
        try {
            response = await httpClient.get("/identity/account/getplayers/getplayers",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

            let res = response.data as IAppUser[]
            return res;
        } catch (e) {
            response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];

                response = await httpClient.get("/identity/account/getplayers/getplayers",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

                let res = response.data as IAppUser[]
                return res;

            }
        }
        return [];
    }

    async getAllClubsMembers(): Promise<IAppUser[]> {
        let response;
        try {
            response = await httpClient.get("/user/getallclubsmembers",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

            let res = response.data as IAppUser[]
            return res;
        } catch (e) {
            response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];

                response = await httpClient.get("/user/getallclubsmembers",
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

                let res = response.data as IAppUser[]
                return res;

            }
        }
        return [];
    }
}