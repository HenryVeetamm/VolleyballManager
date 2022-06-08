<template>
    <h4>You are viewing team: {{ team.name }}</h4>
    <hr />

    <div class="row">
        <div class="col-md-12">
            <SearchBar v-if="useIdentity.$state.role.toString() === 'Coach'" @filtering="filter($event)"
                :navigationTo="'/teams'"></SearchBar>
            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <table class="table table-striped table-sm">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">First name</th>
                        <th scope="col">Last name</th>
                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Nationcalcode</th>
                        <th scope="col">Role in team</th>
                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Remove</th>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="(item, index) in searchPersons">
                        <UsersItem :user="item.appUser" @actionWithUser="removePerson(item.id!)" :index="index"
                            :isDisabled="false" :buttonClass="'danger'" :buttonMessage="'Remove from team'"
                            :role="useIdentity.$state.role.toString()" :roleInTeam="item.rolesInTeam!.roleDescription"
                            :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">
                        </UsersItem>
                    </template>
                </tbody>
            </table>
        </div>
        <div>
            <a v-if="useIdentity.$state.role.toString() === 'Player'">
                <RouterLink to="/teams">Back to List</RouterLink>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { TeamService } from "@/services/TeamService"
import { InitialTeam, type ITeam } from "@/domain/ITeam";
import type { IPersonInTeam } from "@/domain/IPersonInTeam";
import { PersonInTeamService } from "@/services/PersonInTeamService";
import { useIdentityStore } from "@/stores/identityStore";
import type { ISearchItem } from "@/domain/ISearchItem";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class TeamDetails extends Vue {

    teamService = new TeamService();
    personInTeamService = new PersonInTeamService();
    useIdentity = useIdentityStore();

    id!: string;
    team: ITeam = InitialTeam;
    personsInTeam: IPersonInTeam[] = [];
    searchPersons: IPersonInTeam[] = [];
    errorMsg: string[] = [];


    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        }
        else {
            this.team = (await this.teamService.getById(this.id)).data!;
            this.personsInTeam = await this.personInTeamService.getMembersOfTeam(this.id)
            this.searchPersons = this.personsInTeam.map(x => x)
        }
    }


    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInTeam.map(x => x)
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

    async removePerson(personId: string) {
        this.errorMsg = []
        let res = await this.personInTeamService.delete(personId);

        if (res.status >= 300) {
            this.errorMsg = res.errorMsg.errors
        } else {
            this.personsInTeam.forEach((item, index) => {
                if (item.id == personId) {
                    this.personsInTeam.splice(index, 1)
                    this.searchPersons = this.personsInTeam.map(x => x)
                }
            })
        }
    }

}
</script>

