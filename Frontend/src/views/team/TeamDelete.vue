<template>
    <h1>Delete</h1>

    <h3>Are you sure you want to delete this?</h3>
    <div>
        <h4>Team</h4>
        <hr />
        <div v-if="errorMsg.length != 0" class="text-danger">
            <ul>
                <li v-for="error in errorMsg">{{ error }}</li>
            </ul>
        </div>
        <dl class="row">
            <dt class="col-sm-2">
                Club
            </dt>
            <dd class="col-sm-10">
                {{ team.club?.name }}
            </dd>
            <dt class="col-sm-2">
                Name
            </dt>
            <dd class="col-sm-10">
                {{ team.name }}
            </dd>
            <dt class="col-sm-2">
                Code
            </dt>
            <dd class="col-sm-10">
                {{ team.code }}
            </dd>
        </dl>
        <input @click="deleteTeam(team.id!)" value="Delete" class="btn btn-danger" /> |
        <a>
            <RouterLink to="/teams">Back to List</RouterLink>
        </a>

    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { TeamService } from "@/services/TeamService"
import { InitialTeam, type ITeam } from "@/domain/ITeam";
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
export default class TeamDelete extends Vue {

    id!: string;
    team: ITeam = InitialTeam;
    teamService = new TeamService();

    errorMsg: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.team = (await this.teamService.getById(this.id)).data!;
        }
    }

    async deleteTeam(id: string): Promise<void> {
        this.errorMsg = []
        let res = await this.teamService.delete(id);
        if (res.status >= 300) {

            this.errorMsg.push('Remove all persons from team, matches with this team and announcments linked to this team')
        }
        else {
            this.$router.push("/teams")
        }
    }
}



</script>

