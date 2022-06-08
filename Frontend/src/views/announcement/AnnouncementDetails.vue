<template>
    <div>
        <hr />
        <div>
            <div class="media">
                <div class="media-body">
                    <h2 class="media-heading">{{ announcement.title }}</h2>
                    <h4 class="">By {{ announcement.appUser?.firstName + " " + announcement.appUser?.lastName }}</h4>
                    <p><template v-if=(announcement.team)>
                            <h5>To team {{ announcement.team?.name }}</h5>
                        </template>
                        <template v-else>
                            <h5>To club</h5>
                        </template>
                    </p>
                    <hr />
                    <p><span style="white-space: pre-line">
                            {{ announcement.content }}
                        </span></p>
                </div>
            </div>
        </div>
        <div>
            <a>
                <RouterLink to="/announcements" type="button" class="btn btn-info">Back to List</RouterLink>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { AnnouncementService } from "@/services/AnnouncementService"
import { InitialAnnouncement, type IAnnouncement } from "@/domain/IAnnouncement";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
    components: {
    },
    props: {
        id: String,
    },
    emits: [],
})


export default class AnnouncementDetails extends Vue {

    id!: string;

    announcementService = new AnnouncementService();
    announcement: IAnnouncement = InitialAnnouncement

    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.announcement = (await this.announcementService.getById(this.id)).data!;
        }
    }

}
</script>

