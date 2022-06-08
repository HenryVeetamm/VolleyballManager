<template>

  <table class="table table-success table-striped">
    <thead class="table-dark">
      <tr>
        <th scope="col">Name</th>
        <th scope="col">Total aces</th>
        <th scope="col">Total faults</th>
        <th scope="col">Total scored points</th>
        <th scope="col">Average reception</th>
        <th scope="col">Total games</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item of savedComparisonDetailed">
        <th scope="row">{{ item.comparer.firstName }} {{ item.comparer.lastName }} </th>
        <td>{{ item.aces }}</td>
        <td>{{ item.faults }}</td>
        <td>{{ item.points }}</td>
        <td>{{ item.reception }}</td>
        <td>{{ item.totalMatches }}</td>
      </tr>
    </tbody>
  </table>
  <RouterLink :to="'/savedcomparisons'" class="btn btn-info" type="button">Back to list</RouterLink>

</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import type { ISavedComparisonDetailed } from "@/domain/ISavedComparison";
import { SavedComparisonService } from "@/services/SavedComparisonService";
import isUserLoggedIn from "@/helper/loggedInCheck";

@Options({
  components: {
  },
  props: {
    id: String,
  },
  emits: [],
})


export default class PersonInWorkoutDetails extends Vue {
  id!: string;

  comparisonService = new SavedComparisonService()

  savedComparisonDetailed: ISavedComparisonDetailed[] = []

  async mounted(): Promise<void> {
    if (!isUserLoggedIn()) {
      this.$router.push('/')
    } else {
      this.savedComparisonDetailed = await this.comparisonService.getDetailedById(this.id);
    }
  }
}
</script>

