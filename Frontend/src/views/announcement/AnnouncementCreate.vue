<template>
    <h1>Create</h1>
    <h4>Announcement</h4>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <div>
                <div class="form-group">
                    <label class="control-label" for="TeamId">Team</label>
                    <select class="form-control" v-model=announcement.teamId id="TeamId" name="TeamId">
                        <option :value="null">No Team</option>>
                        <option v-for="item of teams" :value=item.id :key="item.id">{{ item.name }}</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="control-label" for="Title">Title</label>
                    <input v-model="announcement.title" class="form-control" type="text" id="Title" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="Content">Content</label>
                    <textarea v-model="announcement.content" class="form-control" type="text" id="Content">
                    </textarea>
                </div>
                <div class="form-group form-check">
                    <label class="form-check-label">
                        <input class="form-check-input" type="checkbox" v-model="announcement.pinned" id="Pinned"
                            name="Pinned" value="true" /> Pinned
                    </label>
                </div>
                <div class="form-group">
                    <input @click="submitClicked()" type="submit" value="Create" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <a>
            <RouterLink to="/announcements">Back to List
            </RouterLink>
        </a>
    </div>
</template>


<script lang="ts">
import { InitialAnnouncement, type IAnnouncement } from "@/domain/IAnnouncement";
import type { ITeam } from "@/domain/ITeam";
import isUserLoggedIn from "@/helper/loggedInCheck";
import { AnnouncementService } from "@/services/AnnouncementService";
import { TeamService } from "@/services/TeamService";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
    components: {
    },
    props: {},
    emits: [],
})
export default class AnnouncementCreate extends Vue {

    announcementService = new AnnouncementService();
    teamService = new TeamService();

    announcement: IAnnouncement = InitialAnnouncement;

    errorMsg: string[] = [];

    teams: ITeam[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            
            this.teams = await this.teamService.getOwnTeams();
        }
    }

    async submitClicked(): Promise<void> {

        this.checkAnnouncement();

        if (this.errorMsg.length === 0) {
            var res = await this.announcementService.add(this.announcement)

            if (res.status >= 300) {

                this.errorMsg = res.errorMsg.errors;
            } else {
                this.resetAnnouncement();
                this.$router.push("/announcements")
            }
        }
    }

    checkAnnouncement(): void {
        this.errorMsg = [];
        if (3 > this.announcement.title.length || this.announcement.title.length > 64) {
            this.errorMsg.push("Title must be between 3 and 64 characters long")
        }
        if (this.announcement.content.length < 3 || this.announcement.content.length > 512) {
            this.errorMsg.push("Content must be between 3 and 512 characters long")
        }
    }

    resetAnnouncement(): void {
        this.announcement.content = ""
        this.announcement.pinned = false
        this.announcement.title = ""
        this.announcement.teamId = ""
    }



}
</script>

