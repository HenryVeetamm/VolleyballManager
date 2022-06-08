<template>
    <h1>Register</h1>

    <div class="row">
        <div class="col-md-12">

            <div v-if="errorMsg.length != 0" class="text-danger " data-valmsg-summary="true">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>

            <div>
                <div class="form-group">
                    <label class="control-label" for="email">Email</label>
                    <input v-model="registration.email" class="form-control" id="email" type="text" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="firstname">Firstname</label>
                    <input v-model="registration.firstname" class="form-control" id="firstname" type="text" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="lastname">Lastname</label>
                    <input v-model="registration.lastname" class="form-control" id="lastname" type="text" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="nationalcode">Nationalcode</label>
                    <input v-model="registration.nationalcode" class="form-control" id="nationalcode" type="text" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="gender">Gender</label>
                    <select class="form-control" v-model=registration.gender id="gender">
                        <option :value="null" disabled>Choose gender</option>
                        <option v-for="(item, value) in genders" :value="value">{{ item }}</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="control-label" for="role">Role</label>
                    <select class="form-control" v-model=registration.role id="role">
                        <option :value="null" disabled>Choose role</option>
                        <option v-for="item in roles" :value="item">{{ item }}</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="control-label" for="bir">Birthday</label>
                    <input class="form-control" v-model="registration.birthday" type="date" id="bir" name="bir">
                    <span class="text-danger field-validation-valid" data-valmsg-for="Input.Birthday"
                        data-valmsg-replace="true"></span>
                </div>
                <div class="form-group">
                    <label class="control-label" for="password">Password</label>
                    <input v-model="registration.password" class="form-control" id="password" type="password" />
                </div>
                <div class="form-group">
                    <div v-if="passwordError" class="text-danger validation-summary-errors" data-valmsg-summary="true">
                        {{ passwordError }}
                    </div>
                    <label class="control-label" for="confirmPassword">Password</label>
                    <input v-model="confirmPassWord" v-on:input="passwordConfirmation()" class="form-control" id="confirmPassword"
                        type="password" />
                </div>
                <div class="form-group">
                    <input @click="registerClicked()" type="submit" value="Register" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">
import { IdentityService } from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identityStore";
import { Options, Vue } from "vue-class-component";
import { type IRegistration, InitialRegistration } from "@/domain/Identity/IRegistration";
import { EGender } from "@/domain/Enum/EGender";

import { RouterLink } from "vue-router";
import { ERole } from "@/domain/Enum/ERole";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class Register extends Vue {

    identityStore = useIdentityStore();
    identityService = new IdentityService();

    registration: IRegistration = InitialRegistration;
    confirmPassWord: string = "";

    errorMsg: string[] = [];
    passwordError: string = "";

    roles: ERole[] = [ERole.Coach, ERole.Player]
    genders: EGender[] = [EGender.Male, EGender.Female]


    mounted() : void {
        this.resetFields()
    }

    async registerClicked(): Promise<void> {
      
        
        this.checkRegisterInfo()
        if (!this.passwordError && this.errorMsg.length == 0) {
            console.log(this.registration)
            let res = await this.identityService.register(this.registration);

            console.log(res)
            if (res.status > 300){
                this.errorMsg = res.errorMsg.errors
            }else{
                this.identityStore.$state.jwt = res.data!;
                this.identityStore.decode();
                this.registration = InitialRegistration
                this.$router.push("/")
            }
        }
    }

    checkRegisterInfo(){
        this.errorMsg = [];
        if (!this.registration.email){
            this.errorMsg.push("Email too short")
        }
        if(!this.registration.birthday){
            this.errorMsg.push("Please Select birthday")
        }
        if(!this.registration.firstname){
            this.errorMsg.push("Please enter your firstname")
        }
        if(!this.registration.lastname){
            this.errorMsg.push("Please enter your lastname")
        }
         if(!this.registration.nationalcode || this.registration.nationalcode.length < 3 || this.registration.nationalcode.length > 16){
            this.errorMsg.push("Please enter correct nationalcode")
        }
        if(this.registration.gender == null){
            this.errorMsg.push("Please select your gender")
        }
        if(!this.registration.role){
            this.errorMsg.push("Please select your role")
        }
        if(!this.registration.password){
            this.errorMsg.push("Please enter password")
        }
        if(!this.confirmPassWord){
            this.errorMsg.push("Please confirm your password")
        }
    }

    resetFields() : void {
        this.registration.email = ""
        this.registration.birthday = ""
        this.registration.firstname = ""
        this.registration.gender = null
        this.registration.lastname = ""
        this.registration.nationalcode = ""
        this.registration.password = ""
        this.registration.role = null
        this.passwordError = ""
    }

    passwordConfirmation() {
        if (this.confirmPassWord != this.registration.password) {
            this.passwordError = "Passwords do not match"
        }
        else {
            this.passwordError = ""
        }
    }

}
</script>

