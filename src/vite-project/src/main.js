import "./main.css";

import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";

import App from "./App.vue";
import router from "./router";
import Aura from "@primevue/themes/aura";

const app = createApp(App);

app.use(PrimeVue, {
  theme: {
      preset: Aura,
      options: {
          darkModeSelector: '.dark',
          cssLayer: false
      }
  }
});
app.use(createPinia());
app.use(router);
app.mount("#app");
