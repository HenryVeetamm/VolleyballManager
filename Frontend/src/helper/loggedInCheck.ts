import { useIdentityStore } from "@/stores/identityStore";

export default function isUserLoggedIn() : boolean{

    let useIdentity = useIdentityStore();

    if(useIdentity.jwt){
        return true
    }
    return false;
}