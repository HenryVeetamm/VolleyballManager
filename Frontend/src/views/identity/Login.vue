<template>
    <h1>Login</h1>

    <div class="row">
        <div class="col-md-12">

            <div v-if="errorMsg" class="text-danger">
                <ul>
                    <li>{{ errorMsg }}</li>
                </ul>
            </div>
            <div>
                <div class="form-group">
                    <label class="control-label" for="email">Email</label>
                    <input v-model="email" class="form-control" id="email" type="text" />
                </div>
                <div class="form-group mb-3">
                    <label class="control-label" for="password">Password</label>
                    <input v-model="password" class="form-control" id="password" type="password" />
                </div>
                <div class="form-group">
                    <input @click="loginClicked()" type="submit" value="Login" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">
import { IdentityService } from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identityStore";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class Login extends Vue {
    identityStore = useIdentityStore();

    email: string = '';
    password: string = '';
    errorMsg: string = "";


    identityService = new IdentityService();

    async loginClicked(): Promise<void> {

        var res = await this.identityService.login(this.email, this.password);

        if (res.status > 300) {
            this.errorMsg = res.errorMsg.errors.User[0]

        } else {
            this.identityStore.$state.jwt = res.data!;

            this.identityStore.decode();

            this.$router.push("/")

        }
    }

}
</script>

