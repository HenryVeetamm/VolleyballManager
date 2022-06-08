import "./assets/index.css"

import { createApp } from "vue";
import { createPinia } from "pinia";

import App from "./App.vue";
import router from "./router";

import "jquery";
import "popper.js";
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";

import "font-awesome/css/font-awesome.min.css";
import UsersItem from "./components/UsersItem.vue";
import SearchBar from "./components/SearchBar.vue";

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.component('UsersItem', UsersItem)
app.component('SearchBar', SearchBar)

app.mount("#app");
