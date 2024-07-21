<template>
    <h1>{{ article.title }}</h1>
    <div v-html="article.text"></div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import ApiService from '@/axios/authService';

const route = useRoute();
const article = ref({})

onMounted(() => {
    fetchArticle();
})

const fetchArticle = async () => {
    try {
        console.log(route.params.id)
        const response = await ApiService.post(`api/articles/getarticle?guid=${route.params.id}`);
         article.value = response.data
    } catch (error) {
        console.log(error)
    }
}
</script>
<style>

</style>