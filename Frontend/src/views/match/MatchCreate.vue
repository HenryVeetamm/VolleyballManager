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
                    <option v-for="item of teams" :value=item.id :key="item.id">{{ item.name }} {{item.code}}</option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="awayTeamId">Select your opponent team</label>
                <select class="form-control" v-model="match.awayTeamId" id="awayTeamId" name="awayTeamId">
                    <option v-for="item of opponentTeams" :value=item.id :key="item.id">{{ item.name }} {{item.code}}</option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="MatchDate">Match date</label>
                <input class="form-control" type="date" data-val="true" v-model="match.matchDate" id="MatchDate" />
                <span class="text-danger " data-valmsg-for="MatchDate"></span>
            </div>
            <div class="form-group mb-2">
                <label class="control-label" for="MatchScore">Match score : example</label>
                <input class="form-control" type="text" data-val="true" id="MatchScore" name="MatchScore"
                    v-model="match.matchScore" placeholder="25:23,25:20,25:15" />
            </div>
            <div class="input-group gap-2">
                <div class="form-group">
                    <input @click="submitClicked('list')" type="submit" value="Create" class="btn btn-primary" />
                </div>
                <div class="form-group">
                    <input @click="submitClicked('add')" type="submit" value="Create and add persons to workout"
                        class="btn btn-success" />
                </div>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/matches'">Back to List</RouterLink>
    </div>
</template>

<script lang="ts">
import { InitialMatch, type IMatch } from "@/domain/IMatch";
import type { ITeam } from "@/domain/ITeam";
import isUserLoggedIn from "@/helper/loggedInCheck";
import checkMatchScore from "@/helper/MatchScore";
import { MatchService } from "@/services/MatchService";
import { TeamService } from "@/services/TeamService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class MatchCreate extends Vue {

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
            this.teams = await this.teamService.getOwnTeams();
            this.opponentTeams = await this.teamService.getOpponentTeams();
        }
    }



    async submitClicked(location: string): Promise<void> {
        this.checkMatchInfo();

        if (this.errorMsg.length == 0) {
            let res = await this.matchService.add(this.match)

            if (res.status >= 300) {
                this.errorMsg.push(res.errorMsg.title);
            } else {
                this.resetMatch()
                if (location == 'list') {
                    this.$router.push('/matches');
                }
                if (location == 'add') {
                    this.$router.push({ name: 'PersonInMatchCreate', params: { id: res.data?.id } });
                }
            }
        }
    }

    checkMatchInfo(): void {
        this.errorMsg = [];
        if(!checkMatchScore(this.match.matchScore)){
            this.errorMsg.push("Please check match score input")
        }
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

    resetMatch(): void {
        this.match.awayTeamId = "";
        this.match.homeTeamId = "";
        this.match.matchDate = "";
        this.match.matchScore = "";
    }

}
</script>
