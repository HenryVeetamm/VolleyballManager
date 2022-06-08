<template>
    <header class="animated-background">
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3 animated-background">
            <div class=" container container-fluid">
                <RouterLink class="navbar-brand" to="/">Home</RouterLink>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse"
                    aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <template v-if="identityStore.$state.jwt">
                            <li class="nav-item ">
                                <RouterLink to="/announcements" class="nav-link text-dark" active-class="active">
                                    Announcements</RouterLink>
                            </li>
                            <li class="nav-item">
                                <RouterLink to="/workouts" class="nav-link text-dark" active-class="active">
                                    Workouts</RouterLink>
                            </li>
                            <li class="nav-item">
                                <RouterLink to="/matches" class="nav-link text-dark" active-class="active">
                                    Matches</RouterLink>
                            </li>

                            <template v-if="identityStore.$state.role.toString() === 'Player'">
                                <li class="nav-item">
                                    <RouterLink to="/clubs" class="nav-link text-dark" active-class="active">
                                        My clubs</RouterLink>
                                </li>
                                <li class="nav-item">
                                    <RouterLink to="/teams" class="nav-link text-dark" active-class="active">
                                        My teams</RouterLink>
                                </li>

                                <li class="nav-item">
                                    <RouterLink to="/savedcomparisons" class="nav-link text-dark" active-class="active">
                                        My comparisons</RouterLink>
                                </li>
                            </template>
                        </template>
                    </ul>

                    <ul class="navbar-nav">
                        <li v-if="identityStore.$state.role.toString() === 'Coach'" class="nav-item dropdown">
                            <a b-7882z672yd class="nav-link dropdown-toggle text-dark" href="#" id="navbarDropdown"
                                role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Management
                            </a>
                            <ul class="dropdown-menu">
                                <li class="nav-item">
                                    <RouterLink to="/clubs" class="nav-link text-dark" active-class="active">
                                        Manage clubs</RouterLink>
                                </li>
                                <li class="nav-item">
                                    <RouterLink to="/teams" class="nav-link text-dark" active-class="active">
                                        Manage teams</RouterLink>
                                </li>
                            </ul>
                        </li>
                        <template v-if="identityStore.$state.jwt == null">
                            <li class="nav-item">
                                <RouterLink class="nav-link text-dark" to="/identity/account/register">Register
                                </RouterLink>
                            </li>
                            <li class="nav-item">
                                <RouterLink to="/identity/account/login" class="nav-link text-dark"
                                    active-class="active">
                                    Login</RouterLink>
                            </li>
                        </template>
                        <template v-else>
                            <li class="nav-item">
                                <div class="nav-link text-dark">Hello {{ identityStore.$state.name }} </div>
                            </li>
                            <li class="nav-item">
                                <a @click="logOut()" class="nav-link text-dark">Logout</a>
                            </li>

                        </template>
                    </ul>

                </div>
            </div>
        </nav>
    </header>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identityStore";
import { RouterLink } from "vue-router";

@Options({
    components: {
    }
})
export default class Header extends Vue {

    identityStore = useIdentityStore();

    mounted() {
        console.log(this.identityStore.role)
    }

    logOut(): void {
        this.identityStore.$reset();
        this.$router.push("/")
    }
}
</script>
<style scoped>
</style>



