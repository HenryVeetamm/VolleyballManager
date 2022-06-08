<template>
    <h1>Create</h1>

    <h4>Club</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">
            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <div class="form-group">
                <label class="control-label" for="Name">Name</label>
                <input class="form-control" v-model="club.name" type="text"  />
            </div>
            <div class="form-check mb-3">
                <input class="form-check-input" type="checkbox" name="ownClub" id="ownClub" v-model="club.ownClub" value="true" />
                <label class="control-label" for="ownClub">Is it your club?</label>
            </div>
            <div class="form-group">
                <input @click="submitClicked()" value="Create" class="btn btn-primary" />
            </div>
        </div>
    </div>

    <div>
        <a>
            <RouterLink to="/clubs">Back to List</RouterLink>
        </a>
    </div>
</template>


<script lang="ts">
import { InitialClub, type IClub } from "@/domain/IClub";
import isUserLoggedIn from "@/helper/loggedInCheck";
import { ClubService } from "@/services/ClubService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class ClubCreate extends Vue {

    clubService = new ClubService();

    club: IClub = InitialClub;

    errorMsg: string[] = [];


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.club.name = ""
            this.club.ownClub = false;
        }
    }

    async submitClicked(): Promise<void> {

        this.checkClubInfo()

        if (this.errorMsg.length === 0) {
            var res = await this.clubService.add(this.club);
            if (res.status >= 300) {
                this.errorMsg = (res.errorMsg.errors);
            } else {
                this.$router.push('/clubs');
            }
        }
    }

    checkClubInfo(): void {
        this.errorMsg = [];

        if (this.club.name.length < 3) {
            this.errorMsg.push("The name should be between 3 and 64 characters")
        }
    }

}
</script>

<style scoped>
</style>