<template>
    <h1>Edit</h1>

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
                <input class="form-control" type="text" v-model="club.name" />
            </div>
            <div class="form-check mb-3">
                <input class="form-check-input" type="checkbox" name="ownClub" id="ownClub" v-model="club.ownClub"
                    data-val="true" value="true" />
                <label class="control-label" for="ownClub">Is it your club?</label>
            </div>
            <div class="form-group">
                <input @click="UpdateClub()" type="submit" value="Save" class="btn btn-primary" />
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
import { type IClub, InitialClub } from "@/domain/IClub";
import isUserLoggedIn from "@/helper/loggedInCheck";
import { ClubService } from "@/services/ClubService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {
        id: String
    },
    emits: [],
})
export default class ClubEdit extends Vue {
    id!: string;

    club: IClub = InitialClub

    clubService = new ClubService();
    errorMsg: string[] = [];

    async mounted() {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.club = (await this.clubService.getById(this.id)).data!;
        }
    }

    async UpdateClub(): Promise<void> {

        this.checkClubInfo()

        if (this.errorMsg.length === 0) {
            let res = await this.clubService.update(this.club,
                this.club.id!)
            if (res.status >= 300) {
                this.errorMsg = (res.errorMsg.errors);
            } else {
                this.$router.push('/clubs');
            }
        }

    }

    checkClubInfo() {
        this.errorMsg = []

        if (!this.club.name) {
            this.errorMsg.push("The Name field is required")
        }
    }


}
</script>

