import './main.css'
import PrimeVue from 'primevue/config'
import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Noir from './Noir'
import MegaMenu from 'primevue/megamenu'
const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Noir,
        options: {
            prefix: 'p',
            darkModeSelector: '.p-dark',
            cssLayer: false,
        }
    }
});
app.use(createPinia())
app.use(router)
app.component('Button', Button)
app.component('Dialog', Dialog)
app.component('InputText',InputText)
app.component('MegaMenu',MegaMenu)
app.mount('#app')
