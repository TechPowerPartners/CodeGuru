import './main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Button from 'primevue/button';
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import "primeicons/primeicons.css";

const app = createApp(App)

app.use(PrimeVue, {
  theme: {
      preset: Aura,
      options: {
          prefix: 'p',
          darkModeSelector: 'dark',
          cssLayer: false
      }
  }
});

app.use(createPinia())
app.component('Button', Button);
app.use(router)
app.mount('#app')