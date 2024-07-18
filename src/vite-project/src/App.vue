<script setup>
import Header from "@/components/Header.vue";
import Footer from "@/components/Footer.vue";
</script>

<template>
  <Header />
  <RouterView />
  <Footer class="footer"/>
</template>

<style lang="sass" scoped>
.footer 
  position: fixed
  bottom:0

</style>
