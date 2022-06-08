<template>
    <h1>Delete</h1>
    <h3>Are you sure you want to delete this?</h3>
    <div>
        <h4>Announcement</h4>
        <hr />
        <dl class="row">
            <div v-if="errors.length != 0" class="text-danger">
                <ul>
                    <li v-for="error in errors">{{ error }}</li>
                </ul>
            </div>
            <dt class="col-sm-2">
                Announcer
            </dt>
            <dd class="col-sm-10">
                {{ announcement.appUser?.firstName + " " + announcement.appUser?.lastName }}
            </dd>
            <dt v-if=(announcement.team) class="col-sm-2">
                Team
            </dt>
            <dd v-if=(announcement.team) class="col-sm-10">
                {{ announcement.team?.name }}
            </dd>
            <dt class="col-sm-2">
                Title
            </dt>
            <dd class="col-sm-10">
                {{ announcement.title }}
            </dd>
            <dt class="col-sm-2">
                Content
            </dt>
            <dd class="col-sm-10">
                <p><span style="white-space: pre-line">
                        {{ announcement.content }}
                    </span></p>
            </dd>
        </dl>
        <input @click="deleteAnnouncement(announcement.id!)" value="Delete" class="btn btn-danger" /> |
        <a>
            <RouterLink to="/announcements">Back to List</RouterLink>
        </a>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { AnnouncementService } from "@/services/AnnouncementService"
import { InitialAnnouncement, type IAnnouncement } from "@/domain/IAnnouncement";
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
export default class AnnouncementDelete extends Vue {

    id!: string;
    announcement: IAnnouncement = InitialAnnouncement;
    announcemenetService = new AnnouncementService();
    errors: string[] = [];

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.announcement = (await this.announcemenetService.getById(this.id)).data!;
        }
    }

    async deleteAnnouncement(id: string): Promise<void> {
        this.errors = []

        let res = await this.announcemenetService.delete(id);

        if (res.status > 300) {
            this.errors.push(res.errorMsg.title)
        } else {
            this.$router.push("/announcements")
        }

    }
}



</script>

