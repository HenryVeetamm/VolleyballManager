<template>
    <div class="row">
        <div class="col-md-12">
            <SearchBar v-if="useIdentity.$state.role.toString() === 'Coach'" @filtering="filter($event)"
                :navigationTo="'/matches'"></SearchBar>
            <template v-if="useIdentity.role.toString() == 'Player'">
                <p class="alert alert-info col-md-4">
                <div class="alert alert-warning">
                    Your statistics
                </div>
                <div>
                    Aces: {{ personInMatch.aces }}
                </div>
                <div>
                    Faults: {{ personInMatch.faults }}
                </div>
                <div>
                    Reception: {{ personInMatch.reception }}
                </div>
                <div>
                    Scored points: {{ personInMatch.totalPoints }}
                </div>
                </p>
            </template>
            <table class="table table-striped table-sm">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">First name</th>
                        <th scope="col">Last name</th>
                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Nationcalcode</th>

                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="(item, index) in searchPersons">
                        <UsersItem :user="item.appUser" @actionWithUser="viewData(item.id!)" :index="index"
                            :isDisabled="false" :buttonClass="'info'" :buttonMessage="'View person in match'"
                            :role="useIdentity.$state.role.toString()"
                            :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">
                        </UsersItem>
                    </template>
                </tbody>
            </table>
        </div>
        <div>
            <a v-if="useIdentity.$state.role.toString() === 'Player'">
                <RouterLink to="/matches">Back to List</RouterLink>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";
import type { ISearchItem } from "@/domain/ISearchItem";
import { PersonInMatchService } from "@/services/PersonInMatchService";
import { MatchService } from "@/services/MatchService";
import { InitialMatch, type IMatch } from "@/domain/IMatch";
import { InitialPersonInMatch, type IPersonInMatch } from "@/domain/IPersonInMatch";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class MatchDetails extends Vue {

    personInMatchService = new PersonInMatchService();
    matchService = new MatchService()
    useIdentity = useIdentityStore();

    id!: string;
    match: IMatch = InitialMatch
    personsInMatch: IPersonInMatch[] = []
    personInMatch: IPersonInMatch = InitialPersonInMatch

    searchPersons: IPersonInMatch[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {

            this.match = (await this.matchService.getById(this.id)).data!;
            this.personsInMatch = await this.personInMatchService.getMembersOfMatch(this.id)

            if (this.useIdentity.role.toString() == 'Player') {
                this.personInMatch = this.personsInMatch.find(x => x.appUserId === this.useIdentity.userId.toString())!
            }
            this.searchPersons = this.personsInMatch.map(x => x)
        }
    }


    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInMatch.map(x => x)
        if (searchItem.firstName) {

            this.searchPersons = this.searchPersons.filter(x => x.appUser!.firstName.toLowerCase().startsWith(searchItem.firstName.toLowerCase()))
        }
        if (searchItem.lastName) {

            this.searchPersons = this.searchPersons.filter(x => x.appUser!.lastName.toLowerCase().startsWith(searchItem.lastName.toLowerCase()))
        }
        if (searchItem.nationalCode) {

            this.searchPersons = this.searchPersons.filter(x => x.appUser!.nationalCode!.startsWith(searchItem.nationalCode))
        }
    }

    async viewData(personInMatchId: string) {
        this.$router.push({ name: 'PersonInMatchDetails', params: { personInMatchId: personInMatchId } });
    }

}
</script>

