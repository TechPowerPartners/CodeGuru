<template>
  <div>
    <Menubar class="nav p-menubar" :model="items">
      <template #start>
        <h1><router-link class="logo" to="/main">CodeGuru</router-link></h1>
      </template>
      <template #item="{ item, props, root }">
        <a class="flex items-center" v-bind="props.action">
          <span :class="item.icon" />
            <router-link class="nav-link" v-if="item.link" :to="item.link">{{item.label}}</router-link>
          <Badge
            v-if="item.badge"
            :class="{ 'ml-auto': !root, 'ml-2': root }"
            :value="item.badge"
          />
          <span
            v-if="item.shortcut"
            class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1"
            >
            {{ item.shortcut }}
          </span>
        </a>
      </template>
    </Menubar>
  </div>
</template>

<script setup>
import Menubar from "primevue/menubar";
import Badge from "primevue/badge";

import { ref } from "vue";

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
.p-menubar
  display: flex
  justify-content: space-between
.nav-link
  text-decoration: none
  color: #000
  font-size: 1rem
  width: 100%
.logo
  text-decoration: none
  color: #000
  font-size: 2rem
  margin-right: 50px
  margin-left: 3rem
</style>