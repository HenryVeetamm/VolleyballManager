<template>
    <h1>Edit</h1>

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
                <label class="control-label" for="Name">Name</label>
                <input class="form-control" type="text" v-model="team.name">
            </div>
            <div class="form-group">
                <label class="control-label" for="Code">Code</label>
                <input class="form-control" type="text" v-model="team.code">

            </div>
            <div class="form-check mb-3">
                <input class="form-check-input" type="checkbox" name="ownTeam" id="ownTeam" v-model="team.ownTeam"
                    value="true" />
                <label class="control-label" for="ownTeam">Is it your team?</label>
            </div>
            <div class="form-group">
                <input @click="UpdateTeam()" type="submit" value="Save" class="btn btn-primary" />
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
import { type ITeam, InitialTeam } from "@/domain/ITeam";
import type { IClub } from "@/domain/IClub";
import { TeamService } from "@/services/TeamService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";
import { ClubService } from "@/services/ClubService";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String
    },
    emits: [],
})
export default class TeamEdit extends Vue {
    id!: string;

    team: ITeam = InitialTeam
    clubs: IClub[] = []

    teamService = new TeamService();
    clubService = new ClubService();

    errorMsg: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.team = (await this.teamService.getById(this.id)).data!;
            this.clubs = await this.clubService.getAll();
        }
    }

    async UpdateTeam(): Promise<void> {

        this.checkTeamInfo();

        if (this.errorMsg.length === 0) {
            let res = await this.teamService.update(this.team,
                this.team.id!)

            if (res.status >= 300) {
                this.errorMsg= (res.errorMsg.errors);
            }
            else {
                this.$router.push('/teams');
            }
        }


    }

    checkTeamInfo() {

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


}
</script>

