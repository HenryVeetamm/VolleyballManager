import httpClient from "@/http-client";
import type { IServiceResult } from "./IServiceResult";
import type { AxiosError } from "axios";
import { useIdentityStore } from "@/stores/identityStore";
import { IdentityService } from "./IdentityService";

export class BaseService<TEntity>{

    identityStore = useIdentityStore();

    constructor(private path: string) {

    }
    async getAll(): Promise<TEntity[]> {
        console.log("getAll");
        try {

            let response = await httpClient.get(`/${this.path}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            });
           
            let res = response.data as TEntity[];
            return res;
            
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) return [];


                let response = await httpClient.get(`/${this.path}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });
                

                let res = response.data as TEntity[];
                return res;

            }
        }
        return [];
    }

    async getById(id: string): Promise<IServiceResult<TEntity>> {
        console.log("getById")
        let response;
        try {
            response = await httpClient.get(`/${this.path}/${id}`, {
                headers: {
                    "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                }
            });

            return {
                status: response.status,
                data: response.data,

            };
        } catch (e) {
            response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) {
                    return { status: response.status };
                }


                response = await httpClient.get(`/${this.path}/${id}`, {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                });

                return {
                    status: response.status,
                    data: response.data,

                };
            }
        }
        return { status: response.status };
    }

    async add(entity: TEntity): Promise<IServiceResult<TEntity>> {
        console.log("add");

        let response;
        try {
            response = await httpClient.post(`/${this.path}`, entity,
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                }
            );
        } catch (e) {

            response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt.token) {
                    return { status: response.status };
                }
                response = await httpClient.post(`/${this.path}`, entity,
                    {
                        headers: {
                            "Authorization": "bearer " + this.identityStore.$state.jwt!.token
                        }
                    }
                );

                if (response.status < 300) {
                    return { status: response.status }
                }

                let res = {
                    status: (e as AxiosError).response!.status,
                    errorMsg: (e as AxiosError).response!.data,
                }
                return res;
            }

            let res = {
                status: (e as AxiosError).response!.status,
                errorMsg: (e as AxiosError).response!.data,
            }
            return res;
        }
        return response;
    }

    async delete(id: string): Promise<IServiceResult<void>> {
        console.log("delete")
        let response;
        try {
            response = await httpClient.delete(`/${this.path}/${id}`,
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                })

            return { status: response.status };
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                console.log("sees")
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt.token) {

                    return { status: response.status };
                }

                response = await httpClient.delete(`/${this.path}/${id}`,
                    {
                        headers: {
                            "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                        }
                    })

                if (response.status < 300) {
                    return { status: response.status }
                }

                let res = {
                    status: (e as AxiosError).response!.status,
                    errorMsg: (e as AxiosError).response!.data,
                }
                return res;

            }
            let res = {
                status: (e as AxiosError).response!.status,
                errorMsg: (e as AxiosError).response!.data,
            }
            return res;
        }
    }

    async update(entity: TEntity, id: string): Promise<IServiceResult<void>> {
        console.log("update");

        let response;
        try {
            response = await httpClient.put(`/${this.path}/${id}`, entity,
                {
                    headers: {
                        "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                    }
                }
            );
        } catch (e) {
            let response = (e as AxiosError).response!;
            if (response.status == 401 && this.identityStore.jwt) {
                let identityService = new IdentityService();
                let refreshResponse = await identityService.refreshIdentity();
                this.identityStore.$state.jwt = refreshResponse.data!;

                if (!this.identityStore.$state.jwt) {
                    return { status: response.status };
                }


                response = await httpClient.put(`/${this.path}/${id}`, entity,
                    {
                        headers: {
                            "Authorization": "bearer " + this.identityStore.$state.jwt?.token
                        }
                    });
                if (response.status < 300) {
                    return { status: response.status }
                }

                let res = {
                    status: (e as AxiosError).response!.status,
                    errorMsg: (e as AxiosError).response!.data,
                }
                return res;
            }

            return {
                status: response.status,
                errorMsg: (e as AxiosError).response!.data
            };
        }

        return response
    }
}