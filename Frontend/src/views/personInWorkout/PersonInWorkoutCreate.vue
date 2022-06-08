<template>

    <h4>You are adding a person to workout: {{ workout.workoutType?.description }} with date : {{ workout.date }}</h4>

    <hr />
    <div class="row">
        <div class="row">
            <SearchBar @filtering="filter($event)" :navigationTo="'/workouts'"></SearchBar>
            <div class="form-group alert">
                <label class="control-label" for="commentId">Leave a comment to your participant.</label>
                <input v-model="personsWorkoutComment" class="form-control" type="text" id="commentId"
                    name="commentId" />
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
                                    :isDisabled="personsInWorkout.some(x => x.appUser?.firstName == item.firstName
                                    && x.appUser?.lastName == item.lastName && x.appUser?.nationalCode == item.nationalCode)"
                                    :buttonClass="'primary'" :buttonMessage="'Add to workout'"
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
import type { ISearchItem } from "@/domain/ISearchItem";
import { Options, Vue } from "vue-class-component";
import { InitialWorkout, type IWorkout } from "@/domain/IWorkout";
import { WorkoutService } from "@/services/WorkoutService";
import { PersonInWorkoutService } from "@/services/PersonInWorkoutService"
import { UserService } from "@/services/UserService"
import type { IPersonInWorkout } from "@/domain/IPersonInWorkout";
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
export default class PersonInWorkoutCreate extends Vue {
    id!: string
    workout: IWorkout = InitialWorkout;

    personsWorkoutComment: string | null = null

    workoutService = new WorkoutService();
    personInWorkoutService = new PersonInWorkoutService();
    clubsMembersSerivce = new UserService();

    useIdentity = useIdentityStore();

    personsInClubs: IAppUser[] = []
    searchPersons: IAppUser[] = [];
    personsInWorkout: IPersonInWorkout[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn) {
            this.$router.push('/')
        } else {
            this.workout = (await this.workoutService.getById(this.id)).data!;
            this.personsInClubs = await this.clubsMembersSerivce.getAllClubsMembers();
            this.personsInWorkout = await this.personInWorkoutService.getAllMembersOfWorkout(this.id)
            this.searchPersons = this.personsInClubs.map(x => x)
        }

    }
    async submitClicked(id: string): Promise<void> {

        if (!this.checkPersonInWorkout(id)) {
            let res = await this.personInWorkoutService.add({
                workOutId: this.id,
                appUserId: id,
                comment: this.personsWorkoutComment
            });
            this.personsWorkoutComment = null

            this.personsInWorkout = await this.personInWorkoutService.getAllMembersOfWorkout(this.id);
        }
    }

    checkPersonInWorkout(id: string): boolean {
        return this.personsInWorkout.some(person => {
            if (person.appUserId == id) {
                return true;
            }
            return false;
        });
    };

    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInClubs.map(x => x)
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