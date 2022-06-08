<template>

    <h4>You are adding a new person to club: {{ club.name }}</h4>
    <hr />
    <div class="row">
        <div class="row">
            <div v-if="errorMsg != ''" class="text-danger ">
                {{ errorMsg }}
            </div>
            <SearchBar @filtering="filter($event)" :navigationTo="'/clubs'"></SearchBar>
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
                                    :isDisabled="personsInclub.some(x => x.appUser?.firstName == item.firstName
                                    && x.appUser?.lastName == item.lastName && x.appUser?.nationalCode == item.nationalCode)"
                                    :buttonClass="'primary'" :buttonMessage="'Add to club'"
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
import { InitialClub, type IClub } from "@/domain/IClub";
import type { IAppUser } from "@/domain/IPerson";
import { ClubService } from "@/services/ClubService";
import { UserService } from "@/services/UserService";
import { PersonInClubService } from "@/services/PersonInClubService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";
import type { IPersonInClub } from "@/domain/IPersonInClub";
import UsersItem from "@/components/UsersItem.vue";
import SearchBar from "../../components/SearchBar.vue";
import type { ISearchItem } from "@/domain/ISearchItem";
import { useIdentityStore } from "@/stores/identityStore";
import isUserLoggedIn from "@/helper/loggedInCheck";


@Options({
    components: {
        UsersItem,
        SearchBar
    },
    props: { id: String },
    emits: [],
})
export default class PersonInClubCreate extends Vue {

    id!: string;

    clubService = new ClubService();
    userService = new UserService();
    personInclubservice = new PersonInClubService();
    useIdentity = useIdentityStore();

    errorMsg: string = "";

    club: IClub = InitialClub;

    users: IAppUser[] = [];
    selectedUserId!: string;
    personsInclub: IPersonInClub[] = [];
    searchPersons: IAppUser[] = [];



    async mounted(): Promise<void> {

        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.club = (await this.clubService.getById(this.id)).data!;
            this.users = await this.userService.getPlayers();
            this.personsInclub = await this.personInclubservice.GetMembersOfClub(this.id);
            this.searchPersons = [...this.users];
        }
    }

    async submitClicked(id: string): Promise<void> {
        this.errorMsg = "";
        if (!this.checkPersonInClub(id)) {

            let res = await this.personInclubservice.add({
                clubId: this.id,
                appUserId: id
            });

            this.personsInclub = await this.personInclubservice.GetMembersOfClub(this.id);
        } else {
            this.errorMsg = "Person already in club"
        }
    }
    checkPersonInClub(id: string): boolean {
        return this.personsInclub.some(person => {
            if (person.appUserId == id) {
                return true;
            }
            return false;
        });
    };

    filter(searchItem: ISearchItem) {
        this.searchPersons = [...this.users]
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