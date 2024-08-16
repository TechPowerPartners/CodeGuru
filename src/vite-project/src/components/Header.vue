<template>
  <div>
    <Menubar class="card" :model="items">
      <template #start>
        <h1><router-link class="logo" to="/main">CodeGuru</router-link></h1>
      </template>
      <template #item="{ item, props, root }">
        <a v-ripple class="flex items-center" v-bind="props.action">
          <span :class="item.icon" />
          <span class="ml-2">
            <router-link class="nav-link" v-if="item.link" :to="item.link">{{ item.label }}</router-link>
          </span>
          <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
          <span v-if="item.shortcut"
            class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1">{{ item.shortcut
            }}</span>
        </a>
      </template>
      <template #end>
        <div class="card flex justify-center">
          <template v-if="!userStore.isAuthenticated">
            <Button label="Login" @click="handleLoginClick" />
          </template>
          <template v-else>
            <PanelMenu :model="loggedInOptions" class="w-full md:w-80" />

          </template>
        </div>
        <Dialog v-model:visible="visible" modal header="Логин" :style="{ width: '25rem' }">
          <div class="mb-6">
            <label for="username" class="font-semibold text-lg">Логин</label>
            <InputText class="mt-2 border rounded-md p-2" autocomplete="off" v-model="loginParams.name" />
          </div>

          <div class="mb-6">
            <label for="password" class="font-semibold text-lg">Пароль</label>
            <Password class="mt-2 border rounded-md p-2" autocomplete="off" v-model="loginParams.password"
              :feedback="false" />
          </div>

          <div class="flex justify-end gap-4">
            <Button type="button" label="Login"
              class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" @click="login"></Button>
          </div>
        </Dialog>
      </template>
    </Menubar>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
import { useUserStore } from '@/axios/userStore';

import Password from 'primevue/password';
import ApiService from '../axios/authService';


const visible = ref(false);
const userStore = useUserStore();
const items = ref([
  {
    label: "Cтатьи",
    icon: "pi pi-home",
    link: "/Articles",
  },
  {
    label: "Вакансии",
    icon: "pi pi-star",
    link: "/Jobs",
  },
  {
    label: "Проекты",
    icon: "pi pi-search",
    link: "/Project",
  },
  {
    label: "Создать статью",
    icon: "pi pi-pencil",
    link: '/CreateArticle'
  },
  {
    label: "Ресурсы/Книги",
    icon: "pi pi-book",
    link: "/Resources"
  }
]);
const loggedInOptions = ref([
  {
    label: 'Профиль',
    icon: 'pi pi-user',
    items: [
      {
        label: 'Личные настройки',
        icon: 'pi pi-cog',

      },
      {
        label: 'Выйти',
        icon: 'pi logout',
        command: () => logout()
      }
    ]
  }
])
let loginParams = {
  name: "",
  password: ""
};

const login = async () => {
  try {
    const response = await ApiService.post('/users/login', loginParams);
    userStore.setToken(response.data);
    await userStore.fetchUser();
    visible.value = false;
  } catch (error) {
    
  }
};

const logout = () => {
  userStore.clearUser();
};

const handleLoginClick = () => {
  console.log('Login button clicked');
  visible.value = true;
};

onMounted(async () => {
  const token = window.localStorage.getItem('token');
  if (token) {
    userStore.setToken(token);
    await userStore.fetchUser();
  }
});
</script>
<style lang="sass">
.nav-link
  text-decoration: none
  color: #000
  font-size: 1rem
  

.logo
  text-decoration: none
  color: #000
  font-size: 1.5rem
  margin-right: 50px
</style>
