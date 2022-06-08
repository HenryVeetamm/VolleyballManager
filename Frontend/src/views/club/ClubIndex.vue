<template>
    <h1>Clubs</h1>

    <p>
        <a>
            <RouterLink to="/club/create" v-if="useIdentity.$state.role.toString() === 'Coach'">Create New</RouterLink>
        </a>
    </p>
    <div class="row">
        <div class="col-md-12">
            <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                <button @click="changeClubs()" v-if="showOpponents" class="btn btn-info">Show your clubs</button>
                <button @click="changeClubs()" v-if="!showOpponents" class="btn btn-info">Show opponent clubs</button>
            </template>
            <table class="table table-striped table">
                <thead class="alert-warning alert">
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item of showingClubs" :key="item.id">
                        <td>
                            {{ item.name }}
                        </td>
                        <td>
                            <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                                <div class="input-group gap-2">
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'ClubEdit', params: { id: item.id } }"
                                            class="btn btn-warning" v-if="!showOpponents" type="button">Edit club name
                                        </RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'ClubDetails', params: { id: item.id } }"
                                            class="btn btn-info" v-if="!showOpponents" type="button">Details
                                        </RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'ClubDelete', params: { id: item.id } }"
                                            class="btn btn-danger" type="button">Delete</RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'PersonInClubCreate', params: { id: item.id } }"
                                            v-if="!showOpponents" class="btn btn-success" type="button">Add person
                                        </RouterLink>
                                    </div>
                                </div>

                            </template>
                            <template v-if="useIdentity.$state.role.toString() === 'Player'">

                                <RouterLink :to="{ name: 'ClubDetails', params: { id: item.id } }" class="btn btn-info"
                                    type="button">View club members</RouterLink>

                            </template>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { ClubService } from "@/services/ClubService"
import type { IClub } from "@/domain/IClub";
import { PersonInClubService } from "@/services/PersonInClubService";
import { useIdentityStore } from "@/stores/identityStore";
import isUserLoggedIn from "@/helper/loggedInCheck";

export default class ClubIndex extends Vue {

    clubService = new ClubService();
    personInClubService = new PersonInClubService();
    useIdentity = useIdentityStore();

    clubList: IClub[] = [];
    opponentsClubs: IClub[] = [];
    showingClubs: IClub[] = [];

    showOpponents: boolean = false;


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {

            if (this.useIdentity.role.toString() === 'Coach') {
                this.clubList = await this.clubService.getUserClubs();
                this.opponentsClubs = await this.clubService.getUserOpponentClubs();
                this.showingClubs = this.clubList
            }
            if (this.useIdentity.role.toString() === 'Player') {
                const playerClubs = await this.personInClubService.getAll()
                this.showingClubs = playerClubs.map(x => x.club!)
            }
        }
    }

    changeClubs() {
        this.showOpponents = !this.showOpponents
        if (this.showOpponents) {
            this.showingClubs = this.opponentsClubs
        }
        if (!this.showOpponents) {
            this.showingClubs = this.clubList
        }

    }



}
</script>

