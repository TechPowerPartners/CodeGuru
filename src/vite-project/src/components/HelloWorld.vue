<script setup>
import Test from './test.vue';
</script>

<template>
  <div class="greetings">
    <h1></h1>
    <Test />
  </div>
</template>

<style scoped>
</style>
<script>
  import apiService from './axios/apiService';
  export default {
  name: 'Temp',
  methods: {
    async fetchData() {
      try {
        // Unauthenticated request
        const response = await apiService.get('/tests/getlanguageslist');
        console.log(response);

        // Authenticated request
        //const authResponse = await apiService.get('private/resource', '', true);
        //console.log(authResponse);
      } catch (error) {
        console.error(error);
      }
    },
  },
  created() {
    this.fetchData();
  },
};
</script>
