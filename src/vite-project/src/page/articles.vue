<template>
    <h1 class="title">Статьи</h1>
    <div class="cards" @click="">
        <Card style="width: 25rem; overflow: hidden" class="card" v-for="card in cards" key="card" @click="selectedCard(card.id)">
          <!-- <div class="selectedCard" @click="selectedCard(card.id)"> -->
          <!-- <template #header>
            <img
              alt="user header"
              src="https://primefaces.org/cdn/primevue/images/usercard.png"
            />
          </template> -->
          <template #title><p class="cards__title">{{ card.title }}</p></template>
          <template #subtitle><p class="cards__subtitle">{{ card.description }}</p></template>
          <template #id><p>{{ card.id }}</p></template>
          <template #content>
            <div v-html="card.text" class="text-container"></div>
          </template>
        <!-- </div> -->
        </Card>
    </div>
</template>

<script setup>
import Card from "primevue/card";
import Button from "primevue/button";
import ApiService from '../axios/authService'
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
onMounted(() => {
    fetchCards();
})
const cards = ref([]);
const selectedCardId = ref(null);

var pagination = {
  number: 1,
  size: 10,
}

const fetchCards = async () => {
    try {
        const response = await ApiService.post('/api/articles/page',pagination);
        cards.value = response.data.items;
        console.log(response.data.items.id)
        console.log(response.data.items)
    } catch (error) {

        console.error("shit");
    }
}

const selectedCard = (id) => {
    selectedCardId.value = id;
    
    router.push({
      name: 'ReadArticle',
      params: {
        id
      }
    })
    
};

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