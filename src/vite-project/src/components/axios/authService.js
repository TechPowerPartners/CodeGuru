import axios from 'axios';

const development = 'http://localhost:8080';
// const deployed = 'http://127.0.0.1:8080';

const apiClient = axios.create({
  baseURL: development,
  headers: {
    'Content-Type': 'application/json',
  },
});

const authApiClient = axios.create({
  baseURL: development,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json;charset=UTF-8',
  },
});
//TODO завтра добавить логин и сохранение токена
authApiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

authApiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response.status === 401) {
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export { apiClient, authApiClient };
