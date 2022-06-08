import type { IRegistration } from "@/domain/Identity/IRegistration";
import type { IJWTResponse } from "@/domain/IJWTResponse";
import httpClient from "@/http-client";
import { useIdentityStore } from "@/stores/identityStore";
import type { AxiosError } from "axios";
import type { IServiceResult } from "./IServiceResult";

export class IdentityService {
    identityStore = useIdentityStore();


    async login(email: string, password: string): Promise<IServiceResult<IJWTResponse>> {
        try {
            let loginInfo = {
                email,
                password
            }
            let response = await httpClient.post("/identity/account/login", loginInfo);
            return {
                status: response.status,
                data: response.data as IJWTResponse
            };

        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                errorMsg: (e as AxiosError).response!.data,
            }


            return response;
        }



    }

    async refreshIdentity(): Promise<IServiceResult<IJWTResponse>> {
        try {
            

            let response = await httpClient.post("/identity/account/refreshtoken",
                {
                    jwt: this.identityStore.$state.jwt?.token,
                    refreshToken: this.identityStore.$state.jwt?.refreshToken
                }
            );
            return {
                status: response.status,
                data: response.data as IJWTResponse
            };

        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                errorMsg: (e as AxiosError).response!.data,
            }

            return response;
        }
    }

    async register(register : IRegistration): Promise<IServiceResult<IJWTResponse>> {
        try {
            
            console.log("Register");
            let response = await httpClient.post("/identity/account/register", register
            );
            return {
                status: response.status,
                data: response.data as IJWTResponse
            };

        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                errorMsg: (e as AxiosError).response!.data,
            }

            return response;
        }
    }
}