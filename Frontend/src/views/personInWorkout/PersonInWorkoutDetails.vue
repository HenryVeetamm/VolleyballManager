<template>
    <div>
        <div>
            <div class="media">
                <div class="media-body">
                    <h2 class="media-heading">Person in workout: {{ personInWorkout.appUser?.firstName }}
                        {{ personInWorkout.appUser?.lastName }}</h2>
                    <h3>Person's comment</h3>
                    <hr />
                    <div v-if="errorMsg.length != 0" class="text-danger">
                        <ul>
                            <li v-for="error in errorMsg">{{ error }}</li>
                        </ul>
                    </div>
                    <div class="row">
                        <template v-if="editBool">
                            <div class="form-group gap-1 mb-3">
                                <div class="row">
                                    <textarea v-model="updatedComment"></textarea>
                                </div>
                            </div>
                            <div class="form-inline mb-3">
                                <div class="row mb-3">
                                    <button @click="saveComment()" class="btn btn-success">Save comment</button>

                                </div>
                                <div class="row">
                                    <button @click="editBool = !editBool" class="btn btn-warning">Cancle</button>

                                </div>
                            </div>
                        </template>
                        <template v-else="!editbool">
                            <p class="alert alert-info" v-if="updatedComment">{{ personInWorkout.comment }}</p>
                            <div class="form-group">
                                <div class="row mb-3">
                                    <button @click="editBool = !editBool" v-if="updatedComment"
                                        class="btn btn-warning ">Edit comment</button>
                                    <button @click="editBool = !editBool" v-if="!updatedComment"
                                        class="btn btn-success">Add
                                        comment</button>
                                </div>
                            </div>
                        </template>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row mb-3">
                <RouterLink :to="{ name: 'WorkoutDetails', params: { id: personInWorkout.workOutId } }"
                    class="btn btn-info" type="button">Back to list</RouterLink>
            </div>
            <div class="row">
                <button @click="removeFromWorkout()" class="btn btn-danger">Remove from workout</button>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { PersonInWorkoutService } from "@/services/PersonInWorkoutService";
import { InitialPersonInWorkout, type IPersonInWorkout } from "@/domain/IPersonInWorkout";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        personInWorkoutId: String,
    },
    emits: [],
})


export default class PersonInWorkoutDetails extends Vue {

    personInWorkoutService = new PersonInWorkoutService();

    personInWorkoutId!: string;

    updatedComment: string = "";

    editBool: boolean = false;

    personInWorkout: IPersonInWorkout = InitialPersonInWorkout;

    errorMsg: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.personInWorkout = (await this.personInWorkoutService.getUserInfoInWorkout(this.personInWorkoutId)).data!;

            if (this.personInWorkout.comment) {
                this.updatedComment = this.personInWorkout.comment
            }
        }

    }

    async saveComment(): Promise<void> {
        this.errorMsg = []

        let res = await this.personInWorkoutService.update({
            id: this.personInWorkoutId,
            workOutId: this.personInWorkout.workOutId,
            appUserId: this.personInWorkout.appUserId,
            comment: this.updatedComment ? this.updatedComment : null
        }, this.personInWorkoutId)

        if (res.status >= 300) {
            this.errorMsg.push('Something went wrong while updating')
        } else {
            this.editBool = !this.editBool
            this.personInWorkout.comment = this.updatedComment
        }


    }

    async removeFromWorkout(): Promise<void> {
        this.errorMsg = []

        let res = await this.personInWorkoutService.delete(this.personInWorkout.id!)
        if (res.status >= 300) {
            this.errorMsg.push('Something went wrong while deleting')
        } else {
            this.$router.push({ name: 'WorkoutDetails', params: { id: this.personInWorkout.workOutId } })
        }
    }

}
</script>

