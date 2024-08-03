<template>
    <Panel header="{{ article.title }}" toggleable="">
        <h1></h1>
        <div v-html="article.text"></div>
    </Panel>

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
        const response = await ApiService.get(`api/articles/${route.params.id}`);
        article.value = response.data
    } catch (error) {

    }
}
</script>
<style></style>