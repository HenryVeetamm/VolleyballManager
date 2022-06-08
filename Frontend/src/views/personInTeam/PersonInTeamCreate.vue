<template>


    <h4>You are adding a new person to team: {{ team.name }} with code : {{ team.code }}</h4>
    <hr />
    <div class="row">
        <div class="row">
            <div v-if="errorMsg != ''" class="text-danger ">
                {{ errorMsg }}
            </div>
            <SearchBar @filtering="filter($event)" :navigationTo="'/teams'"></SearchBar>
            <div class="form-group alert">
                <label class="control-label" for="RoleId">Choose role for player</label>
                <select class="form-control" v-model=selectedRolesInTeam.id id="RoleId" name="RoleId">
                    <option v-for="item of rolesInTeam" :value=item.id :key="item.id">{{ item.roleDescription }}
                    </option>
                </select>
            </div>
        </div>
        <div>
            <div class="row">
                <div class="col-md-12">
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
                                    :isDisabled="personsInTeam.some(x => x.appUserId == item.id)"
                                    :buttonClass="'primary'" :buttonMessage="'Add to team'"
                                    :role="useIdentity.$state.role.toString()"
                                    :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">
                                </UsersItem>
                            </template>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

</template>


<script lang="ts">
import SearchBar from "@/components/SearchBar.vue";
import UsersItem from "@/components/UsersItem.vue";
import type { IAppUser } from "@/domain/IPerson";
import type { IPersonInClub } from "@/domain/IPersonInClub";
import type { IPersonInTeam } from "@/domain/IPersonInTeam";
import type { ISearchItem } from "@/domain/ISearchItem";
import { InitialTeam, type ITeam } from "@/domain/ITeam";
import { PersonInClubService } from "@/services/PersonInClubService";
import { PersonInTeamService } from "@/services/PersonInTeamService";
import { TeamService } from "@/services/TeamService";
import { RolesInTeamService } from "@/services/RolesInTeamService"
import { useIdentityStore } from "@/stores/identityStore";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";
import { InitialRolesInTeam, type IRolesInTeam } from "@/domain/IRolesInTeam";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
        UsersItem,
        SearchBar
    },
    props: { id: String },
    emits: [],
})
export default class PersonInTeamCreate extends Vue {
    id!: string
    errorMsg: string = "";
    selectedRolesInTeam: IRolesInTeam = InitialRolesInTeam;

    teamService = new TeamService();
    personInClubService = new PersonInClubService();
    personInTeamService = new PersonInTeamService();
    rolesInTeamService = new RolesInTeamService();
    useIdentity = useIdentityStore();

    team: ITeam = InitialTeam
    personsInClub: IPersonInClub[] = []
    personsInTeam: IPersonInTeam[] = []
    searchPersons: IAppUser[] = [];
    rolesInTeam: IRolesInTeam[] = []

    async mounted(): Promise<void> {

        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.team = (await this.teamService.getById(this.id)).data!;
            this.personsInClub = await this.personInClubService.GetMembersOfClub(this.team.clubId)
            this.personsInTeam = await this.personInTeamService.getMembersOfTeam(this.id);
            this.searchPersons = this.personsInClub.map(x => x.appUser!)
            this.rolesInTeam = await this.rolesInTeamService.getAll();
        }
    }

    async submitClicked(id: string): Promise<void> {
        this.errorMsg = "";
        if (!this.checkPersonInClub(id) && this.selectedRolesInTeam.id) {
            let res = await this.personInTeamService.add({
                appUserId: id,
                teamId: this.id,
                rolesInTeamId: this.selectedRolesInTeam.id
            });

            this.personsInTeam = await this.personInTeamService.getMembersOfTeam(this.id);
        } else {
            this.errorMsg = "Person already in team or you havent picked role for player"
        }
    }
    checkPersonInClub(id: string): boolean {
        return this.personsInTeam.some(person => {
            if (person.appUserId == id) {
                return true;
            }
            return false;
        });
    };


    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInClub.map(x => x.appUser!)
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