<template>
    <h1>Edit</h1>
    <h4>Announcement</h4>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div v-if="errorMsg.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errorMsg">{{ error }}</li>
                </ul>
            </div>
            <div class="form-group">
                <label class="control-label" for="TeamId">TeamId</label>

                <select class="form-control" v-model="announcement.teamId" id="TeamId" name="TeamId">
                    <option :value="null">No Team</option>
                    <option v-for="item in teams" :value="item.id"> {{ item.name }} </option>
                </select>
            </div>
            <div class="form-group">
                <label class="control-label" for="Title">Title</label>
                <input class="form-control" type="text" v-model="announcement.title" />
            </div>
            <div class="form-group">
                <label class="control-label" for="Content">Content</label>
                <textarea class="form-control" type="text" v-model="announcement.content">
                </textarea>
            </div>
            <div class="form-group form-check">
                <label class="form-check-label">
                    <input class="form-check-input" v-model="announcement.pinned" type="checkbox" id="Pinned"
                        name="Pinned" />
                    Important
                </label>
            </div>
            <div class="form-group">
                <input @click="UpdateAnn()" type="submit" value="Save" class="btn btn-primary" />
            </div>
        </div>
    </div>
    <div>
        <a>
            <RouterLink to="/announcements">Back to List</RouterLink>
        </a>
    </div>
</template>

<script lang="ts">
import { type IAnnouncement, InitialAnnouncement } from "@/domain/IAnnouncement";
import type { ITeam } from "@/domain/ITeam";
import isUserLoggedIn from "@/helper/loggedInCheck";
import { AnnouncementService } from "@/services/AnnouncementService";
import { TeamService } from "@/services/TeamService";
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
export default class AnnouncementEdit extends Vue {
    id!: string;

    announcement: IAnnouncement = InitialAnnouncement
    teams: ITeam[] = [];

    announcementService = new AnnouncementService();
    teamService = new TeamService();

    errorMsg: string[] = [];


    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {

            this.teams = await this.teamService.getOwnTeams();

            this.announcement = (await this.announcementService.getById(this.id)).data!;
        }
    }

    async UpdateAnn(): Promise<void> {

        this.checkAnnouncement()

        if (this.errorMsg.length === 0) {
            
            let res = await this.announcementService.update(this.announcement,
                this.announcement.id!)
    
            if (res.status >= 300) {
                this.errorMsg = res.errorMsg.errors;
            } else {
                this.$router.push('/announcements');
            }
        }
    }

    checkAnnouncement(): void {
        this.errorMsg = []

        if (3 > this.announcement.title.length || this.announcement.title.length > 64) {
            this.errorMsg.push("Title must be between 3 and 64 characters long")
        }
        if (this.announcement.content.length < 3 || this.announcement.content.length > 512) {
            this.errorMsg.push("Content must be between 3 and 512 characters long")
        }
    }
}
</script>

