<template>
    <h1>Matches</h1>

    <p>
        <RouterLink :to="'/match/create'" v-if="useIdentity.$state.role.toString() === 'Coach'">Create new match
        </RouterLink>
    </p>
    <div v-if="errorMsg.length != 0" class="text-danger">
        <ul>
            <li v-for="error in errorMsg">{{ error }}</li>
        </ul>
    </div>

    <table class="table table-striped">
        <thead class="table-dark">
            <tr>
                <th>#</th>
                <th>
                    Home team
                </th>
                <th>
                    Away team
                </th>
                <th>
                    Match date
                </th>
                <th>Match Score</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) of matches" :class="item.victory ? 'alert alert-success' : 'alert alert-danger'">
                <th>{{ index + 1 }}</th>
                <td>
                    {{ item.homeTeam?.name }}
                </td>
                <td>
                    {{ item.awayTeam?.name }}
                </td>
                <td>
                    {{ item.matchDate }}
                </td>
                <td>
                    {{ item.matchScore }}
                </td>

                <td>
                    <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                        <div class="input-group gap-2">
                            <div class="form-group">
                                <RouterLink :to="{ name: 'MatchEdit', params: { id: item.id } }" class="btn btn-warning"
                                    type="button">Match info</RouterLink>
                            </div>
                            <div class="form-group">
                                <RouterLink :to="{ name: 'MatchDetails', params: { id: item.id } }" class="btn btn-info"
                                    type="button">View you players</RouterLink>
                            </div>
                            <div class="form-group">
                                <input @click="deleteMatch(item.id!)" value="Delete" type="button"
                                    class="btn btn-danger" />
                            </div>
                            <div class="form-group">
                                <RouterLink :to="{ name: 'PersonInMatchCreate', params: { id: item.id } }"
                                    class="btn btn-success" type="button">Add person</RouterLink>
                            </div>
                        </div>
                    </template>
                    <template v-if="useIdentity.$state.role.toString() === 'Player'">
                        <RouterLink :to="{ name: 'MatchDetails', params: { id: item.id } }" class="btn btn-info"
                            type="button">View info</RouterLink>
                    </template>
                </td>
            </tr>

        </tbody>
    </table>
</template>


<script lang="ts">
import { Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";
import { MatchService } from "@/services/MatchService"
import type { IMatch } from "@/domain/IMatch";
import { PersonInMatchService } from "@/services/PersonInMatchService";
import ParseDate from "@/helper/DateFromat";
import isUserLoggedIn from "@/helper/loggedInCheck";

export default class MatchIndex extends Vue {

    useIdentity = useIdentityStore();
    matchService = new MatchService();
    personInMatchService = new PersonInMatchService();

    matches: IMatch[] = []
    errorMsg: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        }
        else {
            if (this.useIdentity.role.toString() === 'Coach') {
                this.matches = await this.matchService.getAll();
            }
            if (this.useIdentity.role.toString() === 'Player') {
                const playerClubs = await this.personInMatchService.getAll()
                this.matches = playerClubs.map(x => x.match!)
            }
            this.parseMatchDate()
        }
    }

    parseMatchDate(): void {
         this.matches.forEach(item => {
                item.matchDate = ParseDate(item.matchDate)
            })
    }

    async deleteMatch(id: string): Promise<void> {
        let res = await this.matchService.delete(id);
        if (res.status > 300) {
            this.errorMsg = [];
            this.errorMsg.push("Remove everybody from match")
        } else {
            this.matches = await this.matchService.getAll();
            this.parseMatchDate()
        }
    }




}
</script>

