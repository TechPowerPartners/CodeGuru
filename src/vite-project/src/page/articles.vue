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
          <template #subtitle><p class="cards__subtitle">{{ card.id }}</p></template>
          
          <template #content>
            <p class="m-0">
             {{ card.description }}
            </p>
          </template>
          
        </Card>
    </div>
</template>

<script setup>
import Card from "primevue/card";
import Button from "primevue/button";
import ApiService from '../axios/authService'
import { ref, onMounted } from "vue";
onMounted(() => {
    fetchCards();
})
const cards = ref([]);

const fetchCards = async () => {
    try {
        const response = await ApiService.post('api/articles/getall');
        cards.value = response.data;
        
    } catch (error) {

        console.error("shit");
    }
}

const GetArticle = async () => {
  try {
    
  } catch (error) {
    
  }
}
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