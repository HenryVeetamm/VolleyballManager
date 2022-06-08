<template>
    <h1>Delete</h1>

    <h3>Are you sure you want to delete this?</h3>
    <div>
        <h4>Club</h4>
        <hr />
        <div v-if="errors.length != 0" class="text-danger">
            <ul>
                <li v-for="error in errors">{{ error }}</li>
            </ul>
        </div>
        <dl class="row">
            <dt class="col-sm-2">
                Name
            </dt>
            <dd class="col-sm-10">
                {{ club.name }}
            </dd>
        </dl>
        <input @click="deleteClub(club.id!)" value="Delete" class="btn btn-danger" /> |
        <a>
            <RouterLink to="/clubs">Back to List</RouterLink>
        </a>

    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { ClubService } from "@/services/ClubService"
import { InitialClub, type IClub } from "@/domain/IClub";
import { RouterLink } from "vue-router";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String
    },
    emits: [],
})
export default class ClubDelete extends Vue {

    id!: string;
    club: IClub = InitialClub;
    clubService = new ClubService();
    errors: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.club = (await this.clubService.getById(this.id)).data!;
        }
    }

    async deleteClub(id: string): Promise<void> {
        let res = await this.clubService.delete(id);
        if (res.status > 300) {
            this.errors.push("Club is still in use. Remove members from club or from team")
        } else {
            this.$router.push("/clubs")
        }

    }
}



</script>

