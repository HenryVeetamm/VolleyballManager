<template>

    <h1>Comparisons</h1>

    <p>
        <RouterLink :to="'/savedcomparisons/create'">Create new </RouterLink>
    </p>
    <div v-if="errorMsg.length != 0" class="text-danger">
        <ul>
            <li v-for="error in errorMsg">{{ error }}</li>
        </ul>
    </div>
    <table class="table table-striped">
        <thead class="alert-warning">
            <tr>
                <td>#</td>

                <th>
                    Comparable
                </th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) of savedComparisons">
                <th>{{ index + 1 }}</th>

                <td>
                    {{ item.comparable?.firstName }} {{ item.comparable?.lastName }}
                </td>
                <td>
                    <div class="input-group gap-1">
                        <div class="form-group">
                            <RouterLink :to="{ name: 'SavedComparisonDetails', params: { id: item.id } }"
                                class="btn btn-info" type="button">Details</RouterLink>
                        </div>
                        <div class="form-group">
                            <a @click="deleteComparison(item.id!)" class="btn btn-danger" type="button">Delete</a>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>


</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";
import { SavedComparisonService } from "@/services/SavedComparisonService";
import type { ISavedComparison } from "@/domain/ISavedComparison";
import isUserLoggedIn from "@/helper/loggedInCheck";

export default class MatchIndex extends Vue {

    useIdentity = useIdentityStore();
    savedComparisonService = new SavedComparisonService();

    savedComparisons: ISavedComparison[] = [];

    errorMsg: string[] = [];



    async mounted(): Promise<void> {
        if (!isUserLoggedIn()) {
            this.$router.push('/')
        } else {
            if (this.useIdentity.role.toString() === 'Player') {
                this.savedComparisons = await this.savedComparisonService.getAll()
            }
        }
    }

    async deleteComparison(id: string): Promise<void> {
        let res = await this.savedComparisonService.delete(id);
        if (res.status >= 300) {
            this.errorMsg = []
            this.errorMsg = res.errorMsg.errors
        } else {
            this.savedComparisons = await this.savedComparisonService.getAll();
        }
    }
}
</script>

