
import type { IPersonInWorkout } from "@/domain/IPersonInWorkout";
import type { IRolesInTeam } from "@/domain/IRolesInTeam";
import type { ITeam } from "@/domain/ITeam";
import httpClient from "@/http-client";
import type { AxiosError } from "axios";

import { BaseService } from "./BaseService";
import { IdentityService } from "./IdentityService";
import type { IServiceResult } from "./IServiceResult";

export class PersonInWorkoutService extends BaseService<IPersonInWorkout>{
    /**
     *
     */
    constructor() {
        super("personinworkout");
    }

    async getAllMembersOfWorkout(workoutId: string): Promise<IPersonInWorkout[]> {
        let response;
        try {
            response = await httpClient.get(`personinworkout/${workoutId}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            })
            console.log("Servuce")
            console.log(response)
            let res = response.data as IPersonInWorkout[]
            return res;
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];

                response = await httpClient.get(`personinworkout/${workoutId}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })
                let res = response.data as IPersonInWorkout[]
                return res;

            }
        }
        return [];

    }

    async getUserInfoInWorkout(personInWorkoutId: string): Promise<IServiceResult<IPersonInWorkout>> {
        let response;
        try {
            response = await httpClient.get(`personinworkout/userinfo/${personInWorkoutId}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            })
            return {
                status: response.status,
                data: response.data
            };
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return {status: response.status};

                response = await httpClient.get(`personinworkout/userinfo/${personInWorkoutId}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

                return {
                    status: response.status,
                    data: (response.data as IPersonInWorkout)
                };

            }
        }
        return {status : response?.status!}
    }

}
  