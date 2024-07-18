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
            <router-link class="nav-link" v-if="item.link" :to="item.link">{{item.label}}</router-link></span>
          <Badge
            v-if="item.badge"
            :class="{ 'ml-auto': !root, 'ml-2': root }"
            :value="item.badge"
          />
          <span
            v-if="item.shortcut"
            class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1"
            >{{ item.shortcut }}</span
          >
        </a>
      </template>
      <template #end>
        
        <div class="flex justify-between items-center gap-2">
            
            <Button label="" @click="isClicked = true" text raised>Login</Button>
            <Dialog v-model:visible="isClicked" modal header="Edit Profile" :style="{ width: '25rem' }">
            <template #header>
                <div class="inline-flex items-center justify-center gap-2">
                    <Avatar image="https://primefaces.org/cdn/primevue/images/avatar/amyelsner.png" shape="circle" />
                    <span class="font-bold whitespace-nowrap">Amy Elsner</span>
                </div>
            </template>
            <span class="text-surface-500 dark:text-surface-400 block mb-8">Update your information.</span>
            <div class="flex items-center gap-4 mb-4">
                <label for="username" class="font-semibold w-24">Username</label>
                <InputText id="username" class="flex-auto" autocomplete="off" />
            </div>
            <div class="flex items-center gap-4 mb-2">
                <label for="email" class="font-semibold w-24">Email</label>
                <InputText id="email" class="flex-auto" autocomplete="off" />
            </div>
            <template #footer>
                <Button label="Cancel" text severity="secondary" @click="visible = false" autofocus />
                <Button label="Save" outlined severity="secondary" @click="visible = false" autofocus />
            </template>
        </Dialog>
            <InputText placeholder="Search" type="text" class="w-32 sm:w-auto" />
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
let articlesBuf;
const isClicked = ref(false);
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