<template>
    <h1>Workouts</h1>

    <p>
        <RouterLink :to="'/workout/create'" v-if="useIdentity.$state.role.toString() === 'Coach'">Create new workout
        </RouterLink>
    </p>
    <div v-if="errors.length != 0" class="text-danger">
        <ul>
            <li v-for="error in errors">{{ error }}</li>
        </ul>
    </div>

    <table class="table table-striped">
        <thead class="alert-warning">
            <tr>
                <th>#</th>
                <th>
                    WorkoutType
                </th>
                <th>
                    Description
                </th>
                <th>
                    Date
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) of workoutStore">
                <td>{{ index + 1 }}</td>
                <td>
                    {{ item.workoutType?.description }}
                </td>
                <td>
                    {{ item.description }}
                </td>
                <td>
                    {{ item.date }}
                </td>
                <td>
                    <template v-if="useIdentity.$state.role.toString() === 'Coach'">
                        <div class="input-group gap-2">
                            <div class="form-group">
                                <RouterLink :to="{ name: 'WorkoutEdit', params: { id: item.id } }"
                                    class="btn btn-warning" type="button">Workout info</RouterLink>
                            </div>
                            <div class="form-group">
                                <RouterLink :to="{ name: 'WorkoutDetails', params: { id: item.id } }"
                                    class="btn btn-info" type="button">View members</RouterLink>
                            </div>
                            <input @click="deleteWorkout(item.id!)" value="Delete" type="button"
                                class="btn btn-danger" />
                            <div class="form-group">
                                <RouterLink :to="{ name: 'PersonInWorkoutCreate', params: { id: item.id } }"
                                    class="btn btn-success" type="button">Add person</RouterLink>
                            </div>
                        </div>
                    </template>
                    <template v-if="useIdentity.$state.role.toString() === 'Player'">
                        <RouterLink :to="{ name: 'WorkoutDetails', params: { id: item.id } }" class="btn btn-info"
                            type="button">View members</RouterLink>
                    </template>
                </td>
            </tr>

        </tbody>
    </table>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";
import { WorkoutService } from "@/services/WorkoutService";
import type { IWorkout } from "@/domain/IWorkout";
import { PersonInWorkoutService } from "@/services/PersonInWorkoutService";
import ParseDate from "@/helper/DateFromat";
import isUserLoggedIn from "@/helper/loggedInCheck";

export default class WorkoutIndex extends Vue {

    workoutService = new WorkoutService();
    personInWorkoutService = new PersonInWorkoutService();
    useIdentity = useIdentityStore();

    workoutStore: IWorkout[] = [];
    errors: string[] = [];


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            if (this.useIdentity.role.toString() === 'Coach') {
                this.workoutStore = await this.workoutService.getAll();
            }
            if (this.useIdentity.role.toString() === 'Player') {
                const playerClubs = await this.personInWorkoutService.getAll()
                this.workoutStore = playerClubs.map(x => x.workout!)
            }
            this.parseWorkoutDate();
        }
    }

    parseWorkoutDate() {
        this.workoutStore.forEach(item => {
            item.date = ParseDate(item.date)
        })
    }

    async deleteWorkout(workoutId: string): Promise<void> {
        this.errors = [];
        let res = await this.workoutService.delete(workoutId);
        if (res.status >= 300) {
            this.errors.push("Please remove all persons from workout");
        } else {
            this.workoutStore = await this.workoutService.getAll();
            this.parseWorkoutDate();
        }
    }
}
</script>

