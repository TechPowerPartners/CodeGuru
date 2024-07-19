<template>
  <div>
    <Menubar class="card" :model="items">
      <template #start>
        <h1><router-link class="logo" to="/main">CodeGuru</router-link></h1>
      </template>
      <template #item="{ item, props, root }">
        <a v-ripple class="flex items-center" v-bind="props.action">
          <span :class="item.icon" />
          <span class="ml-2">
            <router-link class="nav-link" v-if="item.link" :to="item.link">{{ item.label }}</router-link></span>
          <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
          <span v-if="item.shortcut"
            class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1">{{ item.shortcut
            }}</span>
        </a>
      </template>
      <template #end>
        <div class="card flex justify-center">
          <div class="card flex justify-center">
            <Button label="Login" @click="visible = true" />
            <Dialog v-model:visible="visible" modal header="Логин" :style="{ width: '25rem' }">
              <span class="text-surface-500 dark:text-surface-400 block mb-8"></span>
              <div class="flex items-center gap-4 mb-4">
                <label for="username" class="font-semibold w-24">Логин</label>
                <InputText class="flex-auto" autocomplete="off" v-model="loginParams.name" />
              </div>
              <div class="flex items-center gap-4 mb-8">
                <label for="password" class="font-semibold w-24">Пароль</label>
                <Password class="flex-auto" autocomplete="off" v-model="loginParams.password" :feedback="false" />
              </div>
              <div class="flex justify-end gap-2">
                <Button type="button" label="Cancel" severity="secondary" @click="visible = false"></Button>
                <Button type="button" label="Save" @click="login"></Button>
              </div>
            </Dialog>
            <InputText placeholder="Search" type="text" class="w-32 sm:w-auto" />
          </div>
          
        </div>
      </template>
    </Menubar>
  </div>
</template>

<script setup>
import Menubar from "primevue/menubar";
import InputText from "primevue/inputtext";
import Badge from "primevue/badge";

import { ref } from "vue";
import ApiService from '../axios/authService'
const visible = ref(false);
const items = ref([
  {
    label: "Cтатьи",
    icon: "pi pi-home",
    link: "/Articles",
  },
  {
    label: "Вакансии",
    icon: "pi pi-star",
    link: "/Jobs",
  },
  {
    label: "Проекты",
    icon: "pi pi-search",
    link: "/Project",
  },

]);
let loginParams = {
  name: "",
  password: ""
}
const login = async () => {
    try {
        const response = await ApiService.post('/users/login',
          loginParams
        );
        window.localStorage.setItem('token',response.data);
        
    } catch (error) {

        console.error(error);
    }
}
</script>

<style lang="sass">
.nav-link
  text-decoration: none
  color: #000
  font-size: 1rem
  margin-right: 1rem
.logo
  text-decoration: none
  color: #000
  font-size: 1.5rem
  margin-right: 50px
</style>