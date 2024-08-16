<template>
    <div class="container">
        <div class="form-preview-container">
            <div class="form-section">
                <h1>Создать статью</h1>
                <div class="form-group">
                    <InputText 
                        type="text" 
                        placeholder="Название статьи" 
                        size="large" 
                        class="input-title" 
                        v-model="params.Title" 
                    />
                </div>
                <div class="form-group">
                    <InputText 
                        type="text" 
                        placeholder="Описание статьи" 
                        size="large" 
                        class="input-description" 
                        v-model="params.Description" 
                    />
                </div>
                <div class="editor-container">
                    <h2>Содержание</h2>
                    <Editor 
                        editor-style="height: 400px" 
                        v-model="params.Content" 
                        class="editor" 
                    />
                </div>
                <div class="keywords-container">
                    <InputGroup class="keyword-input">
                        <InputText 
                            placeholder="Добавьте ключевые слова" 
                            v-model="params.Keywords" 
                        />
                        <Button 
                            icon="pi pi-search" 
                            severity="warn" 
                        />
                    </InputGroup>
                </div>
                <Button 
                    label="Создать" 
                    icon="pi pi-check" 
                    iconPos="left" 
                    class="submit-button"
                    @click="publishPost" 
                />
            </div>
            <div class="preview-section">
                <h2>Предварительный просмотр</h2>
                <div class="preview-content-container">
                    <div class="preview-title">{{ params.Title }}</div>
                    <div class="preview-description">{{ params.Description }}</div>
                    <div v-html="params.Content" class="preview-content"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import ApiService from '@/axios/authService';
import { ref } from 'vue';

const params = ref({
    Title: "",
    Content: "",
    Description: "",
    Keywords: ""
});

const publishPost = async () => {
    try {
        await ApiService.post('api/users/me/articles', params.value, true);
        window.refresh(); // Refresh or redirect after success
    } catch (error) {
        console.error("Error creating article:", error);
    }
}
</script>

<style lang="sass" scoped>
.container
    display: flex
    justify-content: center
    width: 100%
    padding: 20px
    background-color: #f9f9f9

.form-preview-container
    display: flex
    width: 100%
    max-width: 1200px
    gap: 20px

.form-section
    flex: 1
    display: flex
    flex-direction: column
    background-color: #fff
    border-radius: 8px
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1)
    padding: 20px
    overflow: hidden

.preview-section
    flex: 1
    display: flex
    flex-direction: column
    background-color: #fff
    border-radius: 8px
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1)
    padding: 20px
    max-height: 835px
    overflow-y: auto

h1, h2
    color: #333

.input-title,
.input-description,
.editor-container,
.keywords-container,
.submit-button
    width: 100%

.input-title
    padding: 10px
    border-radius: 4px
    border: 1px solid #ddd
    font-size: 16px
    margin-bottom: 16px

.input-description
    height: 100px
    padding: 10px
    border-radius: 4px
    border: 1px solid #ddd
    font-size: 16px
    margin-bottom: 16px

.editor-container
    margin-bottom: 20px

.editor
    border: 1px solid #ddd
    border-radius: 4px

.keywords-container
    margin-bottom: 20px

.keyword-input
    display: flex
    align-items: center

.submit-button
    background-color: #007bff
    color: white
    border: none
    border-radius: 4px
    padding: 12px 24px
    cursor: pointer
    transition: background-color 0.3s ease
    align-self: flex-start

.submit-button:hover
    background-color: #0056b3

.preview-content-container
    display: flex
    flex-direction: column
    height: 100%
    overflow-y: auto

.preview-title
    font-size: 2em
    font-weight: bold
    margin-bottom: 10px
    color: #333

.preview-description
    font-size: 1.2em
    margin-bottom: 20px
    color: #555

.preview-content
    font-size: 1em
    line-height: 1.6
    color: #444
    overflow-wrap: break-word
</style>
