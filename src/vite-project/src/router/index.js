import { createRouter, createWebHistory } from 'vue-router'
import Main from '../page/main.vue'
import Articles from '../page/articles.vue'
import Jobs from '../page/jobs.vue'
import Project from '../page/project.vue'
import CreateArticle from '../page/createarticle.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/Main',
      name: 'Main',
      component: Articles
    },
    {
      path: '/Articles',
      name: 'Articles',
      component: Articles
    },
    {
      path: '/Jobs',
      name: 'Jobs',
      component: Jobs
    },
    {
      path: '/Project',
      name: 'Project',
      component: Project
    },
    {
      path: '/CreateArticle',
      name: 'CreateArticle',
      component: CreateArticle
    }
    
  ]
})

export default router
