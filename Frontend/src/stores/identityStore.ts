import type { IJWTResponse } from "@/domain/IJWTResponse";
import { defineStore } from "pinia";
import jwt_decode from 'jwt-decode';

export const useIdentityStore = defineStore({
  id: "identity",
  state: () => ({
    jwt: null as IJWTResponse | null,
    role: String,
    name: String,
    userId: String
  }),
  getters: {
  },
  actions: {
    decode() : void{
      let decoded : any= jwt_decode(this.jwt?.token!)
      this.role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      this.name = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
      this.userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
    },

  },
});