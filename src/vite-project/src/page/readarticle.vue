<template>
    <Panel>
        <div class="article-container">
            <h1 class="article-title">{{ article.title }}</h1>
            <div v-html="article.content" class="article-content"></div>
            <div class="article-actions">
                <button @click="likeArticle" class="action-button">
                    <i class="pi pi-thumbs-up"></i> {{ likes }}
                </button>
                <button @click="toggleComments" class="action-button">
                    <i :class="showComments ? 'pi pi-chevron-up' : 'pi pi-chevron-down'"></i>
                    {{ showComments ? 'Скрыть комментарии' : 'Показать комментарии' }}
                </button>
            </div>
            <div v-if="showComments" class="comments-section">
                <h2>Комментарии</h2>
                <div v-for="comment in comments" :key="comment.id" class="comment">
                    <div class="comment-header">
                        <span class="comment-author">{{ comment.name }}</span>
                        <div v-html="comment.comment" class="comment-body"></div>
                    </div>
                    <div class="comment-body" v-html="comment.text"></div>
                </div>
                <div class="comment-form">
                    <Editor 
                        editor-style="height:100px" 
                        v-model="newComment" 
                        class="editor" 
                    />
                    <button @click="submitComment" class="submit-button">Добавить</button>
                </div>
            </div>
        </div>
    </Panel>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import ApiService from '@/axios/authService';

const route = useRoute();
const article = ref({});
const likes = ref(0);
const showComments = ref(false);
const comments = ref([]);
const newComment = ref('');

onMounted(() => {
    fetchArticle();
    fetchComments();
})

const fetchArticle = async () => {
    try {
        const response = await ApiService.get(`/api/articles/${route.params.id}`);
        article.value = response.data;
        likes.value = article.value.likes;
    } catch (error) {
        console.error(error);
    }
}

const fetchComments = async () => {
    try {
        const response = await ApiService.post(`api/articles/getcomments`,{article: route.params.id}, true);
        comments.value = response.data
        // comments.value = response.data.comment;
    } catch (error) {
        console.error(error);
    }
}

const likeArticle = async () => {
    try {
        await ApiService.post(`/api/articles/like`, {
            articleId: route.params.id
        },
        true
    );
        fetchArticle()
    } catch (error) {
        console.error(error);
    }
}

const submitComment = async () => {
    try {
        await ApiService.post(`/api/articles/addcomment`, { articleId:route.params.id, comment: newComment.value },true);
        fetchComments();
        newComment.value = '';
    } catch (error) {
        console.error(error);
    }
}

const toggleComments = () => {
    showComments.value = !showComments.value;
}
</script>

<style>
.article-container {
    max-width: 800px;
    margin: auto;
    padding: 20px;
    background: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.article-title {
    font-size: 2.5em; 
    font-weight: bold; 
    text-align: center;
    margin-bottom: 20px;
    color: #333;
}

.article-content {
    margin-bottom: 20px;
    line-height: 1.6;
}

.article-actions {
    display: flex;
    align-items: center;
    gap: 20px;
    margin-bottom: 20px;
}

.action-button {
    background: none;
    border: none;
    cursor: pointer;
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 16px;
    color: #007bff;
    transition: color 0.3s;
}

.action-button:hover {
    color: #0056b3;
}

.comments-section {
    border-top: 1px solid #ddd;
    padding-top: 10px;
    background: #fff;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.comment {
    border-bottom: 1px solid #ddd;
    padding: 15px 0;
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.comment-header {
    display: flex;
    align-items: center;
    gap: 10px;
}

.comment-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
}

.comment-author {
    font-weight: bold;
    color: #333;
}

.comment-body {
    font-size: 0.875em;
    color: #888;
}

.comment-body {
    padding-left: 50px;
}

/* Quill editor styles */
.ql-container {
    border: 1px solid #ddd;
    border-radius: 5px;
}

.ql-editor {
    min-height: 100px;
    line-height: 1.6;
}

.ql-editor p {
    margin: 0;
}

.comment-form {
    margin-top: 20px;
}

.submit-button {
    background-color: #007bff;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 5px;
    font-size: 16px;
    transition: background-color 0.3s;
}

.submit-button:hover {
    background-color: #0056b3;
}

</style>