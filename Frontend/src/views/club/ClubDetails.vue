<template>
    <h4>You are viewing club: {{ club.name }}</h4>
    <hr />

    <div class="row">
        <div class="col-md-12">
            <SearchBar v-if="useIdentity.$state.role.toString() === 'Coach'" @filtering="filter($event)"
                :navigationTo="'/clubs'"></SearchBar>
            <div v-if="errors.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errors">{{ error }}</li>
                </ul>
            </div>
            <table class="table table-striped table-sm">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">First name</th>
                        <th scope="col">Last name</th>
                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Nationcalcode</th>
                        <th v-if="useIdentity.$state.role.toString() === 'Coach'" scope="col">Remove</th>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="(item, index) in searchPersons">
                        <UsersItem :user="item.appUser" @actionWithUser="removePerson(item.id!)" :index="index"
                            :isDisabled="false" :buttonClass="'danger'" :buttonMessage="'Remove from club'"
                            :role="useIdentity.$state.role.toString()"
                            :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">

                        </UsersItem>
                    </template>
                </tbody>
            </table>
        </div>
        <div>
            <a v-if="useIdentity.$state.role.toString() === 'Player'">
                <RouterLink to="/clubs">Back to List</RouterLink>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { ClubService } from "@/services/ClubService"
import { InitialClub, type IClub } from "@/domain/IClub";
import type { IPersonInClub } from "@/domain/IPersonInClub";
import { PersonInClubService } from "@/services/PersonInClubService";
import UsersItem from "@/components/UsersItem.vue";
import type { ISearchItem } from "@/domain/ISearchItem";
import { useIdentityStore } from "@/stores/identityStore";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
        UsersItem
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class ClubDetails extends Vue {

    clubService = new ClubService();
    personInclubservice = new PersonInClubService();

    id!: string;

    club: IClub = InitialClub
    useIdentity = useIdentityStore()

    personsInclub: IPersonInClub[] = [];
    searchPersons: IPersonInClub[] = [];

    errors: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.club = (await this.clubService.getById(this.id)).data!;
            this.personsInclub = await this.personInclubservice.GetMembersOfClub(this.id);
            this.searchPersons = this.personsInclub.map(x => x)
        }
    }


    async removePerson(personId: string) {
        let res = await this.personInclubservice.delete(personId);
        if (res.status >= 300) {
            this.errors.push(res.errorMsg)
        }
        else {
            this.personsInclub.forEach((item, index) => {
                if (item.id == personId) {
                    this.personsInclub.splice(index, 1)
                    this.searchPersons = this.personsInclub.map(x => x)
                }
            })
        }
    }



    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInclub.map(x => x)
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

}
</script>

