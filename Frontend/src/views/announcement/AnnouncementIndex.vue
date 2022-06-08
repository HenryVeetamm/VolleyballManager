<template>
    <h1>Announcements</h1>
    <p>
        <RouterLink v-if="'Coach' === useIdentity.$state.role.toString()" to="/announcement/create">Create announcement
        </RouterLink>
    </p>
    <div class="row">
        <div class="col-md-10">
            <table class="table table-striped table">
                <thead class="table-dark">
                    <tr>
                        <th scope="col">
                            Author
                        </th>
                        <th scope="col">
                            Announcement to
                        </th>
                        <th scope="col">
                            Title
                        </th>
                        <th scope="col"> Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <AnnouncementItem v-for="item of announcements" :announcement="item"></AnnouncementItem>
                </tbody>
            </table>
        </div>
    </div>

</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"

import { AnnouncementService } from "@/services/AnnouncementService"
import type { IAnnouncement } from "@/domain/IAnnouncement";
import { useIdentityStore } from "@/stores/identityStore";
import AnnouncementItem from "../../components/AnnouncementItem.vue"
import isUserLoggedIn from "@/helper/loggedInCheck";


@Options({
    components: {
        AnnouncementItem
    },
})

export default class AnnouncementIndex extends Vue {

    announcementService = new AnnouncementService();
    announcements: IAnnouncement[] = [];
    useIdentity = useIdentityStore();




    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            this.announcements = await this.announcementService.getAll();
        
            this.announcements = this.announcements.sort((a: IAnnouncement, b: IAnnouncement) => Number(b.pinned) - Number(a.pinned))
        }
    }




}
</script>

