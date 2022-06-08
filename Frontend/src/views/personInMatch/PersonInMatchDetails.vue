<template>
    <div>
        <div>
            <div class="media">
                <div class="media-body">
                    <h2 class="media-heading">Person in match: {{ personInMatch.appUser?.firstName }}
                        {{ personInMatch.appUser?.lastName }}</h2>
                    <hr />
                    <div v-if="errorMsg.length != 0" class="text-danger">
                        <ul>
                            <li v-for="error in errorMsg">{{ error }}</li>
                        </ul>
                    </div>
                    <div class="row">
                        <template v-if="editBool">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="totalPoints">How many points did they score?</label>
                                    <input type="number" v-model="initialPersonInMatch.totalPoints" class="form-control"
                                        id="totalpoints">
                                    <small class="form-text text-muted">You may leave this empty</small>
                                </div>
                                <div class="form-group">
                                    <label for="aces">How many aces?</label>
                                    <input type="number" v-model="initialPersonInMatch.aces" class="form-control"
                                        id="aces">
                                    <small class="form-text text-muted">You may leave this empty</small>
                                </div>
                                <div class="form-group">
                                    <label for="faults">How many faults?</label>
                                    <input type="number" v-model="initialPersonInMatch.faults" class="form-control"
                                        id="faults">
                                    <small class="form-text text-muted">You may leave this empty</small>
                                </div>
                                <div class="form-group">
                                    <label for="reception">Reception</label>
                                    <input type="number" v-model="initialPersonInMatch.reception" class="form-control"
                                        id="reception">
                                    <small class="form-text text-muted">You may leave this empty</small>
                                </div>
                            </div>

                            <div class="form-inline mb-3">
                                <div class="row mb-3">
                                    <button @click="saveEdit()" class="btn btn-success">Save</button>

                                </div>
                                <div class="row">
                                    <button @click="editBool = !editBool" class="btn btn-warning">Cancle</button>

                                </div>
                            </div>
                        </template>


                        <template v-else="!editbool">
                            <p class="alert alert-info col-md-4">
                            <div>
                                Aces: {{ personInMatch.aces }}
                            </div>
                            <div>
                                Faults: {{ personInMatch.faults }}
                            </div>
                            <div>
                                Reception: {{ personInMatch.reception }}
                            </div>
                            <div>
                                Scored points: {{ personInMatch.totalPoints }}
                            </div>
                            </p>

                            <div class="form-group">
                                <div class="row mb-3">
                                    <button @click="editBool = !editBool" class="btn btn-warning ">Edit person
                                        perfomance</button>
                                </div>
                            </div>

                        </template>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row mb-3">
                <RouterLink :to="{ name: 'MatchDetails', params: { id: personInMatch.matchId } }" class="btn btn-info"
                    type="button">Back to list</RouterLink>
            </div>
            <div class="row">
                <button @click="removeFromMatch()" class="btn btn-danger">Remove from
                    match</button>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { PersonInMatchService } from "@/services/PersonInMatchService";
import { InitialPersonInMatch, type IPersonInMatch } from "@/domain/IPersonInMatch";
import { offset } from "@popperjs/core";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        personInMatchId: String,
    },
    emits: [],
})


export default class PersonInMatchDetails extends Vue {

    personInMatchService = new PersonInMatchService();

    personInMatchId!: string;

    editBool: boolean = false;

    personInMatch: IPersonInMatch = InitialPersonInMatch
    initialPersonInMatch: IPersonInMatch = InitialPersonInMatch;

    errorMsg: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.personInMatch = (await this.personInMatchService.getById(this.personInMatchId)).data!;
            this.initialPersonInMatch = { ...this.personInMatch }
        }
    }


    async removeFromMatch(): Promise<void> {
        this.errorMsg = []
        let res = await this.personInMatchService.delete(this.personInMatch.id!)
        if (res.status >= 300) {
            this.errorMsg.push('Something went wrong')
        } else {
            this.$router.push({ name: 'MatchDetails', params: { id: this.personInMatch.matchId } })
        }
    }

    async saveEdit(): Promise<void> {
        this.personInMatch = { ...this.initialPersonInMatch }
        let res = await this.personInMatchService.update({
            id: this.personInMatch.id,
            appUserId: this.personInMatch.appUserId,
            matchId: this.personInMatch.matchId,
            totalPoints: this.personInMatch.totalPoints,
            aces: this.personInMatch.aces,
            faults: this.personInMatch.faults,
            reception: this.personInMatch.reception
        }, this.personInMatch.id!)

        if (res.status >= 300) {
            this.errorMsg = [];
            this.errorMsg.push('Update failed')
        }

        this.editBool = !this.editBool
    }

}
</script>

