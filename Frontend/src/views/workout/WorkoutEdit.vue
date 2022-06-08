<template>
    <h1>Edit</h1>
    <h4>Workout</h4>
    <hr />
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
                <textarea class="form-control" v-model="workout.description" type="text" id="workoutDescription">
                </textarea>
            </div>
            <div class="form-group mb-3">
                <label class="control-label" for="workoutDate">Please select date</label>
                <input class="form-control" v-model="workout.date" type="date" id="workoutDate" />
            </div>
            <div class="form-group mb-3">
                <div class="input-group gap-1">
                    <input @click="submitClicked()" type="submit" value="Update" class="btn btn-primary btn-space" />
                </div>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/workouts'">Back to List</RouterLink>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { WorkoutService } from "@/services/WorkoutService";
import { WorkoutTypeService } from "@/services/WorkoutType";
import type { IWorkoutType } from "@/domain/IWorkoutType";
import { InitialWorkout, type IWorkout } from "@/domain/IWorkout";
import ParseDate from "@/helper/DateFromat"
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class PersonInWorkoutDetails extends Vue {

    id!: string

    workoutService = new WorkoutService();
    workoutTypeService = new WorkoutTypeService();

    workout: IWorkout = InitialWorkout;
    workoutTypes: IWorkoutType[] = [];

    errorMsg: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.workoutTypes = await this.workoutTypeService.getAll();
            this.workout = (await this.workoutService.getById(this.id)).data!;
            this.workout.date = ParseDate(this.workout.date)
        }
    }

    async submitClicked(): Promise<void> {

        this.checkWorkoutInfo();
        
        if (this.errorMsg.length === 0) {
            let res = await this.workoutService.update(this.workout, this.workout.id!)

            if (res.status >= 300) {
                this.errorMsg = res.errorMsg.errors
            }
            else
            {
                this.$router.push("/workouts");
            }
        }
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

