<template>


    <div class="col-md-12">
        <div class="input-group gap-1">

            <div class="form-group">

                <input class="form-control" v-model="firstName"
                    v-on:input="$emit('filtering', { firstName: firstName, lastName: lastName, nationalCode: nationalCode })"
                    type="text" placeholder="Search by first name" />
            </div>
            <div class="form-group mb-3">
                <input class="form-control" v-model="lastName"
                    v-on:input="$emit('filtering', { firstName: firstName, lastName: lastName, nationalCode: nationalCode })"
                    type="text" placeholder="Search by last name" />
            </div>
            <div class="form-group">

                <input class="form-control" v-model="nationalCode"
                    v-on:input="$emit('filtering', { firstName: firstName, lastName: lastName, nationalCode: nationalCode })"
                    type="text" placeholder="Search by nationalcode" />
            </div>
            <div class="form-group">
                <RouterLink :to="navigationTo">
                    <input value="Back to list" class="btn btn-success" type="button" />
                </RouterLink>
            </div>

        </div>
    </div>




</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component";
import UsersItem from "@/components/UsersItem.vue";

@Options({
    components: {
        UsersItem
    },
    props: {
        navigationTo: String
    },
    emits: ['filtering'],
})
export default class SearchBar extends Vue {

    navigationTo!: string;
    firstName: string = "";
    lastName: string = "";
    nationalCode: string = "";
}
</script>

<style scoped>
</style>