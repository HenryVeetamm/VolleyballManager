<template>

    <h1>Create</h1>

    <h4>Match</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">
            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <div class="form-group">
                <label class="control-label" for="homeTeamId">Select your team</label>
                <select class="form-control" v-model="match.homeTeamId" id="homeTeamId" name="homeTeamId">
                    <option v-for="item of teams" :value=item.id :key="item.id">{{ item.name }}</option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="awayTeamId">Select your opponent team</label>
                <select class="form-control" v-model="match.awayTeamId" id="awayTeamId" name="awayTeamId">
                    <option v-for="item of opponentTeams" :value=item.id :key="item.id">{{ item.name }}</option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="MatchDate">Match date</label>
                <input class="form-control" type="date" data-val="true" v-model="match.matchDate" id="MatchDate" />
                <span class="text-danger field-validation-valid" data-valmsg-for="MatchDate"
                    data-valmsg-replace="true"></span>
            </div>
            <div class="form-group">
                <label class="control-label" for="MatchScore">Match score : example</label>
                <input class="form-control" type="text" data-val="true" id="MatchScore" name="MatchScore"
                    v-model="match.matchScore" placeholder="25:23,25:20,25:15" />
            </div>
            <div class="form-group">
                <input @click="submitClicked()" type="submit" value="Edit" class="btn btn-primary" />
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/matches'">Back to List</RouterLink>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { type IMatch, InitialMatch } from "@/domain/IMatch";
import type { ITeam } from "@/domain/ITeam";
import { MatchService } from "@/services/MatchService";
import { TeamService } from "@/services/TeamService";
import ParseDate from "@/helper/DateFromat";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class MatchEdit extends Vue {

    id!: string

    teamService = new TeamService();
    matchService = new MatchService();

    match: IMatch = InitialMatch

    teams: ITeam[] = []
    opponentTeams: ITeam[] = []

    errorMsg: string[] = [];


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.match = (await this.matchService.getById(this.id)).data!;
            this.match.matchDate = ParseDate(this.match.matchDate)
            this.teams = await this.teamService.getOwnTeams();
            this.opponentTeams = await this.teamService.getOpponentTeams();
        }
    }


    async submitClicked(): Promise<void> {
        this.checkMatchInfo()
        if (this.errorMsg.length === 0) {
            var res = await this.matchService.update(this.match, this.id)
        
            if (res.status >= 300) {

                this.errorMsg.push(res.errorMsg.title);
            } else {

                this.$router.push("/matches")
            }
        }
    }

    checkMatchInfo() {
        this.errorMsg = [];
        if (!this.match.homeTeamId) {
            this.errorMsg.push("Please select home team")
        }
        if (!this.match.awayTeamId) {
            this.errorMsg.push("Please select opponent team")
        }
        if (!this.match.matchDate) {
            this.errorMsg.push("Please select match date")
        }
        if (!this.match.matchScore) {
            this.errorMsg.push("Please add score")
        }
    }

}
</script>

