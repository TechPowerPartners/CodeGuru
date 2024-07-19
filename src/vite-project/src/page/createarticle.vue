<template>

    <div class="quill-container">
        <h1>Статья</h1>
        <Editor editor-style="height: 400px"></Editor>
        <QuillEditor theme="snow" content-type="html" toolbar="full" />
    </div>
    <div class="card flex flex-col md:flex-row gap-4">
        <InputGroup>
            <Button label="Search" />
            <InputText placeholder="Keyword" />
        </InputGroup>

        <InputGroup>
            <InputText placeholder="Keyword" />
            <Button icon="pi pi-search" severity="warn" />
        </InputGroup>

        <InputGroup>
            <Button icon="pi pi-check" severity="success" />
            <InputText placeholder="Vote" />
            <Button icon="pi pi-times" severity="danger" />
        </InputGroup>
    </div>
</template>


<style lang="sass" scoped>

.quill-container
    width: 100%
    @media(min-width: 768px)
        width: 80%
        margin: 0 auto
        

</style>