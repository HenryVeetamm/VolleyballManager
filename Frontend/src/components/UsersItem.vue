<template>

    <tr :key="user.id">
        <th scope="row">{{ index + 1 }}</th>
        <td>{{ user.firstName }}</td>
        <td>{{ user.lastName }}</td>
        <td v-if="role === 'Coach'">{{ user.nationalCode }}</td>
        <td v-if="roleInTeam">
            {{roleInTeam}}
        </td>
        <td v-if="buttonsActive">
            <input @click="$emit('actionWithUser', user.id!)" :value="`${buttonMessage}`" type="button" :class="['btn', `btn-${buttonClass}`]"
                :disabled="isDisabled" />
        </td>
       
    </tr>

</template>


<script lang="ts">
import type { IAppUser } from "@/domain/IPerson";
import { Options, Vue } from "vue-class-component";

@Options({
    components: {
    },
    props: {
        user: Object,
        index: Number,
        isDisabled: Boolean,
        buttonClass: String,
        buttonMessage: String,
        role: String,
        roleInTeam: String,
        buttonsActive: Boolean
    },
    emits: ['actionWithUser'],
})
export default class UsersItem extends Vue {

    user!: IAppUser
    index!: number
    isDisabled!: boolean;
    buttonClass!: string;
    buttonMessage!: string;
    role!: string;
    roleInTeam! : string;
    buttonsActive!: boolean

}
</script>

<style scoped>
</style>