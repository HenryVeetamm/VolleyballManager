<template>

    <span class="text-center">
       
            <h1 v-if="identityStore.jwt != null">🏐Welcome to volleyball manager🏐</h1>
            <h1 v-if="identityStore.jwt == null">Please register or login to get started</h1>
        
    </span>


</template>


<script lang="ts">
import { Options, Vue } from "vue-class-component"
import { useIdentityStore } from "@/stores/identityStore";



export default class HomeView extends Vue {

    identityStore = useIdentityStore();


}
</script>

<style scoped>

</style>