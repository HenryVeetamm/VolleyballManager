<script lang="ts">
import { RouterLink, RouterView } from "vue-router";
import Header from "./components/Header.vue";
import Footer from "./components/Footer.vue";
import { Options, Vue } from "vue-class-component";


@Options({
  components: {
    Header, Footer
  }
})

export default class App extends Vue {

}

</script>

<template>
  <Header/>
    
    <div class="container">
      <main role="main" class="pb-3">
        <RouterView />
      </main>
      
    </div>
 

  <Footer>
  </Footer>
</template>

<style scoped>

</style>
