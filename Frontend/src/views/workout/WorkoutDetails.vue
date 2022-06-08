<template>
    <div class="row">
        <div class="col-md-12">
            <SearchBar v-if="useIdentity.$state.role.toString() === 'Coach'" @filtering="filter($event)"
                :navigationTo="'/workouts'"></SearchBar>
            <div class="row" v-if="useIdentity.$state.role.toString() === 'Player' && personInWorkout.comment !== null">
                <h5>Workout type: {{ workout.workoutType?.description }}</h5>
                <p class="alert alert-warning">Workout description: {{ workout.description }}</p>
                <hr />
                <h5>Coach left you a comment: </h5>
                <p class="alert alert-info">{{ personInWorkout.comment }}</p>
            </div>
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
                            :isDisabled="false" :buttonClass="'info'" :buttonMessage="'View person in workout'"
                            :role="useIdentity.$state.role.toString()"
                            :buttonsActive="useIdentity.$state.role.toString() == 'Coach'">
                        </UsersItem>
                    </template>
                </tbody>
            </table>
        </div>
        <div>
            <a v-if="useIdentity.$state.role.toString() === 'Player'">
                <RouterLink to="/workouts">Back to List</RouterLink>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";
import type { ISearchItem } from "@/domain/ISearchItem";
import { PersonInWorkoutService } from "@/services/PersonInWorkoutService";
import { InitialPersonInWorkout, type IPersonInWorkout } from "@/domain/IPersonInWorkout";
import { InitialWorkout, type IWorkout } from "@/domain/IWorkout";
import { WorkoutService } from "@/services/WorkoutService";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class WorkoutDetails extends Vue {

    personInWorkoutService = new PersonInWorkoutService();
    workoutService = new WorkoutService()
    useIdentity = useIdentityStore();


    id!: string;
    workout: IWorkout = InitialWorkout
    personsInWorkout: IPersonInWorkout[] = []
    personInWorkout: IPersonInWorkout = InitialPersonInWorkout


    searchPersons: IPersonInWorkout[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.workout = (await this.workoutService.getById(this.id)).data!;
            this.personsInWorkout = await this.personInWorkoutService.getAllMembersOfWorkout(this.id)
            this.personInWorkout = this.personsInWorkout.find(x => x.appUserId === this.useIdentity.userId.toString())!
            this.searchPersons = this.personsInWorkout.map(x => x)
        }
    }


    filter(searchItem: ISearchItem) {
        this.searchPersons = this.personsInWorkout.map(x => x)
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

    async viewData(personInWorkoutId: string) {
        this.$router.push({ name: 'PersonInWorkoutDetails', params: { personInWorkoutId: personInWorkoutId } });
    }

}
</script>

