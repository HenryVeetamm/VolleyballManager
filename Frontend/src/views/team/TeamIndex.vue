<template>
    <h1>Teams</h1>

    <p>
        <RouterLink to="/team/create" v-if="useIdentity.$state.role.toString() === 'Coach'">Create New</RouterLink>

    </p>
    <div class="row">
        <div class="col-md-12">
            <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                <button @click="changeTeams()" v-if="showOpponents" class="btn btn-info">Show your teams</button>
                <button @click="changeTeams()" v-if="!showOpponents" class="btn btn-info">Show opponent teams</button>
            </template>
            <table class="table table-striped table">

                <thead class="alert-warning alert">
                    <tr>
                        <th>
                            Club
                        </th>
                        <th>
                            Name
                        </th>
                        <th>
                            Code
                        </th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in showingTeams">
                        <td>
                            {{ item.club?.name }}
                        </td>
                        <td>
                            {{ item.name }}
                        </td>
                        <td>
                            {{ item.code }}
                        </td>
                        <td>

                            <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                                <div class="input-group gap-2">
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'TeamEdit', params: { id: item.id } }"
                                            class="btn btn-warning" v-if="!showOpponents" type="button">Edit team name
                                        </RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'TeamDetails', params: { id: item.id } }"
                                            class="btn btn-info" v-if="!showOpponents" type="button">Details
                                        </RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'TeamDelete', params: { id: item.id } }"
                                            class="btn btn-danger" type="button">Delete</RouterLink>
                                    </div>
                                    <div class="form-group">
                                        <RouterLink :to="{ name: 'PersonInTeamCreate', params: { id: item.id } }"
                                            v-if="!showOpponents" class="btn btn-success" type="button">Add person
                                        </RouterLink>
                                    </div>
                                </div>
                            </template>
                            <template v-if="useIdentity.$state.role.toString() === 'Player'">
                                <RouterLink :to="{ name: 'TeamDetails', params: { id: item.id } }" class="btn btn-info"
                                    type="button">View team members</RouterLink>
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

import { TeamService } from "@/services/TeamService"
import type { ITeam } from "@/domain/ITeam";
import { useIdentityStore } from "@/stores/identityStore";
import { PersonInTeamService } from "@/services/PersonInTeamService";
import isUserLoggedIn from "@/helper/loggedInCheck";


export default class TeamIndex extends Vue {

    teamService = new TeamService();
    personInTeamService = new PersonInTeamService();
    useIdentity = useIdentityStore();

    showingteams: ITeam[] = []

    showingTeams: ITeam[] = [];
    teamList: ITeam[] = [];
    opponentsTeam: ITeam[] = [];
    showOpponents: boolean = false;


    async mounted(): Promise<void> {
        if (!isUserLoggedIn) {
            this.$router.push('/')
        } else {
            if (this.useIdentity.role.toString() === 'Coach') {
                this.teamList = await this.teamService.getOwnTeams();
                this.opponentsTeam = await this.teamService.getOpponentTeams();
    
                this.showingTeams = this.teamList
            }
            if (this.useIdentity.role.toString() === 'Player') {
                const playerTeams = await this.personInTeamService.getAll()
                this.showingTeams = playerTeams.map(x => x.team!)
            }
        }

    }

    changeTeams() {
        this.showOpponents = !this.showOpponents
        if (this.showOpponents) {
            this.showingTeams = this.opponentsTeam
        }
        if (!this.showOpponents) {
            this.showingTeams = this.teamList
        }
    }

}
</script>

