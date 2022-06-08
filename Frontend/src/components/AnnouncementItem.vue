<template>
    <tr :key="announcement.id"
        v-bind:class="{ 'alert-danger': announcement.pinned, 'alert-info': !announcement.pinned }">
        <td>
            {{ announcement.appUser!.firstName }}
        </td>
        <td>
            <template v-if="announcement.team">
                Team: {{ announcement.team?.name }}
            </template>
            <template v-else>
                Club
            </template>
        </td>
        <td>
            {{ announcement.title }}
        </td>


        <td>
            <template v-if="identityStore.$state.role.toString() === 'Coach'">

                <div class="input-group gap-2">
                    <div class="form-group">
                        <RouterLink v-if="identityStore.role.toString() == 'Coach'"
                            :to="{ name: 'AnnouncementEdit', params: { id: announcement.id } }" class="btn btn-warning"
                            type="button">Edit</RouterLink>
                    </div>
                    <div class="form-group">
                        <RouterLink :to="{ name: 'AnnouncementDetails', params: { id: announcement.id } }"
                            class="btn btn-info" type="button">Details</RouterLink>
                    </div>
                    <div class="form-group">
                        <RouterLink v-if="identityStore.role.toString() == 'Coach'"
                            :to="{ name: 'AnnouncementDelete', params: { id: announcement.id } }" class="btn btn-danger"
                            type="button">Delete</RouterLink>
                    </div>
                </div>
            </template>
            <template v-if="identityStore.$state.role.toString() === 'Player'">
                <RouterLink :to="{ name: 'AnnouncementDetails', params: { id: announcement.id } }" class="btn btn-info"
                    type="button">Details</RouterLink>

            </template>
        </td>
    </tr>
</template>

<script  lang="ts">
import type { IAnnouncement } from '@/domain/IAnnouncement';
import { Options, Vue } from 'vue-class-component';
import { useIdentityStore } from '@/stores/identityStore';

@Options({
    components: {

    },

    props: {
        announcement: Object
    },
    emits: []
})


export default class AnnouncementItem extends Vue {

    announcement!: IAnnouncement;
    identityStore = useIdentityStore();

}
</script>

<style scoped>
@import "@/assets/index.css"
</style>