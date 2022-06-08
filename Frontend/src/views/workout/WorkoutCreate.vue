<template>

    <h1>Create</h1>
    <h4>Workout</h4>
    <hr />
    <div v-if="errorMsg.length != 0" class="text-danger">
        <ul>
            <li v-for="error in errorMsg">{{ error }}</li>
        </ul>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="control-label" for="workoutTypeId">Select workout type</label>
                <select class="form-control" v-model="workout.workoutTypeId" id="workoutTypeId" name="workoutTypeId">
                    <option v-for="item of workoutTypes" :value=item.id :key="item.id">{{ item.description }}</option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="workoutDescription">Workout description</label>
                <input class="form-control" v-model="workout.description" type="text" id="workoutDescription" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label" for="workoutDate">Please select date</label>
                <input class="form-control" v-model="workout.date" type="date" id="workoutDate" />
            </div>
            <div class="form-group mb-3">
                <div class="input-group gap-1">
                    <input @click="submitClicked('list')" type="submit" value="Create"
                        class="btn btn-primary btn-space" />
                    <input @click="submitClicked('add')" type="submit" value="Create and add persons to workout"
                        class="btn btn-success" />
                </div>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/workouts'">Back to List</RouterLink>
    </div>
</template>

<script lang="ts">
import { InitialWorkout, type IWorkout } from "@/domain/IWorkout";
import type { IWorkoutType } from "@/domain/IWorkoutType";
import isUserLoggedIn from "@/helper/loggedInCheck";
import type { IServiceResult } from "@/services/IServiceResult";
import { WorkoutService } from "@/services/WorkoutService";
import { WorkoutTypeService } from "@/services/WorkoutType";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class ClubCreate extends Vue {

    workoutService = new WorkoutService();
    workoutTypeService = new WorkoutTypeService();

    workoutTypes: IWorkoutType[] = []

    workout: IWorkout = InitialWorkout

    errorMsg: string[] = [];


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.workoutTypes = await this.workoutTypeService.getAll();
        }
    }

    async submitClicked(location: string): Promise<void> {
        this.checkWorkoutInfo();

        if (this.errorMsg.length === 0) {
            let res = await this.workoutService.add(this.workout);
            if (res.status >= 300) {
                this.errorMsg = res.errorMsg.errors
            } else {
                this.resetWorkout();
                if (location == 'list') {
                    this.$router.push("/workouts")
                }
                if(location == 'add'){
                    this.$router.push({ name: 'PersonInWorkoutCreate', params: { id: res.data?.id } });
                }
            }
        }
    }

    resetWorkout(): void {
        this.workout.workoutTypeId = "";
        this.workout.description = "";
        this.workout.date = "";
    }

    checkWorkoutInfo() {
        this.errorMsg = []
        if (!this.workout.date) {
            this.errorMsg.push('Please select date')
        }
        if (!this.workout.workoutTypeId) {
            this.errorMsg.push('Please select workout type')
        }
    }

}
</script>
