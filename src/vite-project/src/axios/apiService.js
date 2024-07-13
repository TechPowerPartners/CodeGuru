import { apiClient,authApiClient } from "./authService";
export default {
  get(resource, slug = '', auth = false) {
    const client = auth ? authApiClient : apiClient;
    return client.get(`${resource}/${slug}`).catch((error) => {
      throw new Error(`ApiService ${error}`);
    });
  },
  post(resource, params, auth = false) {
    const client = auth ? authApiClient : apiClient;
    return client.post(`${resource}`, params);
  },
  update(resource, slug, params, auth = false) {
    const client = auth ? authApiClient : apiClient;
    return client.put(`${resource}/${slug}`, params);
  },
  delete(resource, slug, auth = false) {
    const client = auth ? authApiClient : apiClient;
    return client.delete(`${resource}/${slug}`).catch((error) => {
      throw new Error(`ApiService ${error}`);
    });
  },
};
