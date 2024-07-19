import { defineStore } from 'pinia';
import ApiService from './authService';

export const useUserStore = defineStore('user', {
  state: () => ({
    isAuthenticated: false,
    userInfo: {},
  }),
  actions: {
    async fetchUser() {
      try {
        const response = await ApiService.get('/users/userinfo', '', true); // Assume /users/userinfo returns the user info
        this.isAuthenticated = true;
        this.userInfo = response.data;
      } catch (error) {
        console.error(error);
        this.clearUser();
      }
    },
    setToken(token) {
      window.localStorage.setItem('token', token);
      this.isAuthenticated = true;
    },
    clearUser() {
      window.localStorage.removeItem('token');
      this.isAuthenticated = false;
      this.userInfo = {};
    },
  },
});
