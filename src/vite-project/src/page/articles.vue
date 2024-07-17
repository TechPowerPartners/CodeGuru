<template>
    <h1 class="title">Статьи</h1>
    <div class="cards">
        <Card style="width: 25rem; overflow: hidden" class="card" v-for="card in cards" key="card">
          <template #header>
            <img
              alt="user header"
              src="https://primefaces.org/cdn/primevue/images/usercard.png"
            />
          </template>
          <template #title><p class="cards__title">{{ card.title }}</p></template>
          <template #subtitle><p class="cards__subtitle">{{ card.subtitle }}</p></template>
          <template #content>
            <p class="m-0">
             {{ card.description }}
            </p>
          </template>
          <template #footer>
            <div class="flex gap-4 mt-1">
              <Button label="Cancel" severity="secondary" outlined class="w-full cards__btn" />
              <Button label="Save" class="w-full" />
            </div>
          </template>
        </Card>
    </div>
</template>

<script setup>
import Card from "primevue/card";
import Button from "primevue/button";

import { ref } from "vue";

const cards = ref([
  {
    title: "Абобус на занянге 1",
    subtitle: "Card subtitle 1",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Абобус на занянге 2",
    subtitle: "Card subtitle 2",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Абобус на занянге 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Абобус на занянге 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Advanced Card 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Advanced Card 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Advanced Card 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
  {
    title: "Advanced Card 3",
    subtitle: "Card subtitle 3",
    description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. ", 
  },
]);
</script>

<style lang="sass" scoped>

.title
    font-size: 2rem
    margin: 2rem 0 2rem 4rem
.cards
    display: grid
    grid-template-columns: repeat(auto-fill, minmax(400px, 1fr))
    grid-row-gap: 3.5rem
    margin: 0 2rem
    & .card
        margin: 2rem
    &__title
        margin: 1rem 0
    &__btn
        margin-right: 1rem
.p-card-title
    margin: 2rem 0

</style>