<template>

    <h4>Search :</h4>

    <hr />
    <div class="row">
        <div class="row">
            <SearchBar @filtering="filter($event)" :navigationTo="'/savedcomparisons'"></SearchBar>
        </div>
        <div v-if="errorMsg.length != 0" class="text-danger">
            <ul>
                <li v-for="error in errorMsg">{{ error }}</li>
            </ul>
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
                                <th scope="col">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="(item, index) in searchPersons">
                                <UsersItem :user="item" @actionWithUser="submitClicked(item.id!)" :index="index"
                                    :isDisabled="personComparisons.some(x => x.comparableId == item.id)"
                                    :buttonClass="'primary'" :buttonMessage="'Save'"
                                    :role="useIdentity.$state.role.toString()"
                                    :buttonsActive="useIdentity.$state.role.toString() == 'Player'">
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
import type { ISearchItem } from "@/domain/ISearchItem";
import { Options, Vue } from "vue-class-component";
import { UserService } from "@/services/UserService"
import { useIdentityStore } from "@/stores/identityStore";
import type { ISavedComparison } from "@/domain/ISavedComparison";
import { SavedComparisonService } from "@/services/SavedComparisonService";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
        UsersItem,
        SearchBar
    },
    props: {},
    emits: [],
})
export default class SavedComparisonCreate extends Vue {
    userService = new UserService();
    savedComparisonService = new SavedComparisonService();
    useIdentity = useIdentityStore();

    players: IAppUser[] = []
    searchPersons: IAppUser[] = [];
    personComparisons: ISavedComparison[] = [];

    errorMsg: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.players = await this.userService.getPlayers();
            this.personComparisons = await this.savedComparisonService.getAll();
            this.searchPersons = this.players.map(x => x)
        }
    }
    async submitClicked(id: string): Promise<void> {
        let res = await this.savedComparisonService.add({
            comparerId: this.useIdentity.userId.toString(),
            comparableId: id
        });
        if (res.status >= 300) {
            this.errorMsg = [];
            this.errorMsg.push('Something went wrong while adding');
        } else {
            this.personComparisons = await this.savedComparisonService.getAll();
        }
    };


    filter(searchItem: ISearchItem) {
        this.searchPersons = this.players.map(x => x)
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