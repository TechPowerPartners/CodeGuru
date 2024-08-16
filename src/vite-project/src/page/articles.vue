<template>
    <div class="container">
      <ProgressSpinner
      v-if="loading"
      class="spinner"
      :style="{ 'position': 'absolute', 'top': '50%', 'left': '50%', 'transform': 'translate(-50%, -50%)' }"
    />
    <div class="cards" @click="">
        <Card style="width: 100%; height: 10rem; overflow: hidden" class="card" v-for="card in cards" key="card" @click="selectedCard(card.id)">
          <template #title><p class="cards__title">{{ card.title }}</p></template>
          <template #subtitle><p class="cards__subtitle">{{ card.description }}</p></template>
          <template #id><p>{{ card.id }}</p></template>
          <template #content>
            <div v-html="card.text" class="text-container"></div>
            <i class="pi pi-thumbs-up">{{ card.likes }}</i>
          </template>        
        </Card>
    </div>
    <Paginator class="paginator" :rows="10" :totalRecords="totalRec" :rowsPerPageOptions="[10, 20, 30]" @page="onPageChange" />
  </div>
</template>


<script setup>

import ApiService from '../axios/authService'
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";


let pageNumber = 1;
let size = 10;
const totalPages = ref(0);
const totalRec = ref([]);
const router = useRouter();
const loading = ref(false);
onMounted(() => {
    fetchCards();
})
const cards = ref([]);
const selectedCardId = ref(null);

var pagination = {
  number: pageNumber,
  size: size,
}

const fetchCards = async () => {
  loading.value = true;
    try {
        const response = await ApiService.post('/api/articles/page',pagination);
        cards.value = response.data.items;
        totalRec.value = response.data.pageIndex;
        totalPages.value = response.data.totalPages;
    } catch (error) {
        console.error("something went wrong")
    }
    finally {
      loading.value = false;
    }
}
const onPageChange = (event) => {
  pageNumber = event.page + 1;
  pagination.number = pageNumber;
  fetchCards().then(() => {
    window.scrollTo({ top: 0, behavior: 'smooth'})
  });
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
.container
  position: relative
  display: flex
  flex-direction: column
  min-height: 100vh
  padding: 20px

.spinner
  position: absolute
  top: 50%
  left: 50%
  transform: translate(-50%, -50%)

.cards
  display: flex
  flex-wrap: wrap
  gap: 20px
  margin-top: 20px

.card
  background: #fff
  border-radius: 8px
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1)
  transition: transform 0.2s, box-shadow 0.2s
  cursor: pointer
  overflow: hidden
  width: calc(33.333% - 20px) 
  height: 300px

.card:hover
  transform: translateY(-5px)
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2)

.cards__title
  font-size: 18px
  font-weight: bold
  margin: 10px

.cards__subtitle
  font-size: 14px
  color: #666
  margin: 0 10px 10px

.text-container
  padding: 10px
  font-size: 14px
  color: #333
</style>
