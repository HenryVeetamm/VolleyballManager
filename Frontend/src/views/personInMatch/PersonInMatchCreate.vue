<template>
    <hr />
    <div class="row">
        <div class="row">
            <SearchBar @filtering="filter($event)" :navigationTo="'/matches'"></SearchBar>
        </div>
        <div v-if="errorMsg.length != 0" class="text-danger">
            <ul>
                <li v-for="error in errorMsg">{{ error }}</li>
            </ul>
        </div>
        <div>
            <div class="row">
                <div class="col-md-8">
                    <table class="table table-striped table-sm">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">#</th>
                                <th scope="col">First name</th>
                                <th scope="col">Last name</th>
                                <th scope="col">Nationcalcode</th>
                                <th scope="col">Add</th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="(item, index) in searchPersons">
                                <UsersItem :user="item" @actionWithUser="submitClicked(item.id!)" :index="index"
                                    :isDisabled="personsInMatches.some(x => x.appUserId == item.id)"
                                    :buttonClass="'primary'" :buttonMessage="'Add to match'"
                                    :role="useIdentity.$state.role.toString()"
                                    :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">
                                </UsersItem>
                            </template>
                        </tbody>
                    </table>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="totalPoints">How many points did they score?</label>
                        <input type="number" v-model="personInMatch.totalPoints" class="form-control" id="totalpoints">
                        <small class="form-text text-muted">You may leave this empty</small>
                    </div>
                    <div class="form-group">
                        <label for="aces">How many aces?</label>
                        <input type="number" v-model="personInMatch.aces" class="form-control" id="aces">
                        <small class="form-text text-muted">You may leave this empty</small>
                    </div>
                    <div class="form-group">
                        <label for="faults">How many faults?</label>
                        <input type="number" v-model="personInMatch.faults" class="form-control" id="faults">
                        <small class="form-text text-muted">You may leave this empty</small>
                    </div>
                    <div class="form-group">
                        <label for="reception">Reception</label>
                        <input type="number" v-model="personInMatch.reception" class="form-control" id="reception">
                        <small class="form-text text-muted">You may leave this empty</small>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>


<script lang="ts">
import SearchBar from "@/components/SearchBar.vue";
import UsersItem from "@/components/UsersItem.vue";
import type { IAppUser } from "@/domain/IPerson";
import type { ISearchItem } from "@/domain/ISearchItem";
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identityStore";
import { InitialMatch, type IMatch } from "@/domain/IMatch";
import { MatchService } from "@/services/MatchService";
import { PersonInMatchService } from "@/services/PersonInMatchService";
import { InitialPersonInMatch, type IPersonInMatch } from "@/domain/IPersonInMatch";
import { PersonInTeamService } from "@/services/PersonInTeamService";
import type { IPersonInTeam } from "@/domain/IPersonInTeam";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
        UsersItem,
        SearchBar
    },
    props: { id: String },
    emits: [],
})
export default class PersonInMatchCreate extends Vue {
    id!: string
    match: IMatch = InitialMatch;
    personInMatch: IPersonInMatch = InitialPersonInMatch

    matchService = new MatchService();
    personInMatchService = new PersonInMatchService();
    personInTeamService = new PersonInTeamService();


    useIdentity = useIdentityStore();

    personsInMatches: IPersonInMatch[] = []
    searchPersons: IAppUser[] = [];
    personsInTeam: IPersonInTeam[] = [];
    errorMsg: string[] = [];

    async mounted(): Promise<void> {

        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.resetPersonInMatch()
            this.match = (await this.matchService.getById(this.id)).data!;
            this.personsInMatches = await this.personInMatchService.getMembersOfMatch(this.id);
            this.personsInTeam = await this.personInTeamService.getMembersOfTeam(this.match.homeTeamId)
            this.searchPersons = this.personsInTeam.map(x => x.appUser!)
        }
    }

    async submitClicked(userId: string) {
        let res = await this.personInMatchService.add({
            appUserId: userId,
            matchId: this.id,
            totalPoints: this.personInMatch.totalPoints,
            aces: this.personInMatch.aces,
            faults: this.personInMatch.faults,
            reception: this.personInMatch.reception
        })

        if (res.status >= 300) {
            this.errorMsg = [];
            this.errorMsg.push('User already in match')
        } else {

            this.resetPersonInMatch()

            this.personsInMatches = await this.personInMatchService.getMembersOfMatch(this.id);
        }
    }

    resetPersonInMatch() {
        this.personInMatch.aces = null
        this.personInMatch.faults = null
        this.personInMatch.reception = null
        this.personInMatch.totalPoints = null
    }

    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInTeam.map(x => x.appUser!)
        if (searchItem.firstName) {

            this.searchPersons = this.searchPersons.filter(x => x.firstName.toLowerCase().startsWith(searchItem.firstName.toLowerCase()))
        }
        if (searchItem.lastName) {

            this.searchPersons = this.searchPersons.filter(x => x.lastName.toLowerCase().startsWith(searchItem.lastName.toLowerCase()))
        }
        if (searchItem.nationalCode) {

            this.searchPersons = this.searchPersons.filter(x => x.nationalCode!.startsWith(searchItem.nationalCode))
        }
    }
}
</script>

<style scoped>
</style>