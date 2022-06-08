<template>
    <h1>Create</h1>

    <h4>Team</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">

            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <div class="form-group">
                <label class="control-label" for="clubId">Club</label>
                <select class="form-control" data-val="true" v-model="team.clubId" id="clubId" name="clubId">
                    <option v-for="item in clubs" :value="item.id"> {{ item.name }} </option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="name">Name</label>
                <input class="form-control" v-model="team.name" type="text" data-val="true" name="name" />

            </div>
            <div class="form-group mb-3">
                <label class="control-label" for="code">Code</label>
                <input class="form-control" type="text" data-val="true" name="code" v-model="team.code" />
            </div>
            <div class="form-check mb-3">
                <input class="form-check-input" type="checkbox" name="ownTeam" id="ownTeam" v-model="team.ownTeam"
                    data-val="true" value="true" />
                <label class="control-label" for="ownTeam">Is it your team?</label>
            </div>
            <div class="form-group">
                <input @click="submitClicked('Team')" value="Create" class="btn btn-primary" />
            </div>
            <div class="form-group" v-if="team.ownTeam">
                <input @click="submitClicked('PersonInTeamCreate')" value="Add persons to team"
                    class="btn btn-primary" />
            </div>
        </div>
    </div>

    <div>
        <a>
            <RouterLink to="/teams">Back to List</RouterLink>
        </a>
    </div>
</template>


<script lang="ts">
import type { IClub } from "@/domain/IClub";
import { InitialTeam, type ITeam } from "@/domain/ITeam";
import isUserLoggedIn from "@/helper/loggedInCheck";
import { ClubService } from "@/services/ClubService";
import type { IServiceResult } from "@/services/IServiceResult";
import { TeamService } from "@/services/TeamService";

import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class TeamCreate extends Vue {

    clubService = new ClubService();
    teamService = new TeamService();

    clubs: IClub[] = []

    team: ITeam = InitialTeam

    errorMsg: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.resetTeam()
            this.clubs = await this.clubService.getAll();
        }
    }

    async submitClicked(location: string): Promise<void> {

        this.checkTeamInfo();
    
        if (this.errorMsg.length === 0) {

            let res = await this.teamService.add(
                this.team
            );

            if (res.status >= 300) {
                this.errorMsg = (res.errorMsg.errors);
            }
            else {
                this.resetTeam()
                this.$router.push({ name: location, params: { id: res.data?.id } });
            }
        }
    }

    checkTeamInfo(): void {
        this.errorMsg = [];

        if (!this.team.name) {
            this.errorMsg.push("The Name field is required")
        }
        if (!this.team.code) {
            this.errorMsg.push("The Code field is required")
        }
        if (!this.team.clubId) {
            this.errorMsg.push("The Club is required")
        }
    }

    resetTeam(): void {
        this.team.name = ""
        this.team.code = ""
        this.team.clubId = ""
        this.team.ownTeam = false
    }

}
</script>

<style scoped>
</style>