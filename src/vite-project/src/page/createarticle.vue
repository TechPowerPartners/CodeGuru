<template>
    <div class="container">
        
        <div class="title">
            
            <InputText type="text" placeholder="Название статьи" size="large" class="input-title" v-model="params.Title" />
        </div>
        <div class="quill-container">
            <h1>Статья</h1>
            <Editor editor-style="height: 400px" v-model="params.Text" class="editr" />
        </div>
        <div class="card flex flex-col md:flex-row gap-4">
            <InputGroup class="keyword-input">
                <InputText placeholder="Keyword" />
                <Button icon="pi pi-search" severity="warn" />
            </InputGroup>
        </div>
    </div>
    
    <Button label="Создать" icon="pi pi-check" iconPos="left" @click="publishPost" />

</template>
<script setup>
import ApiService from '@/axios/authService';

var params = {
    Title: "",
    Text: "",
}

const publishPost = async () => {
    try {
        const response = await ApiService.post('api/articles/create',
            params,true
        )
        window.refresh()
    } catch (error) {
        
    }
}
</script>
<style lang="sass" scoped>
.container
    display: flex
    flex-direction: column
    align-items: center
    width: 100%
    padding: 16px

.title 
    width: 100%
    max-width: 600px
    margin-bottom: 16px
    display: flex
    justify-content: center

.quill-container
    width: 100%
    max-width: 800px
    margin-bottom: 16px
    @media (min-width: 768px)
        width: 80%
        margin: 0 auto

.card
    width: 100%
    max-width: 800px
    padding: 16px
    @media (min-width: 768px)
        padding: 24px
        margin: 0 auto
.input-title
    width: 100%
    max-width: 800px
    box-sizing: border-box

</style>
