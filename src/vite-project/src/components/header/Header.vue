<template>
    <div class="card">
      <MegaMenu
        :model="items"
        class="p-4 bg-surface-0"
        style="border-radius: 3rem"
      >
        <template #start>
          <svg
            width="35"
            height="40"
            viewBox="0 0 35 40"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            class="h-8"
          >
            
          </svg>
        </template>
        <template #item="{ item }">
          <a
            v-if="item.root"
            v-ripple
            class="flex items-center cursor-pointer px-4 py-2 overflow-hidden relative font-semibold text-lg uppercase"
            style="border-radius: 2rem"
          >
            <span :class="item.icon" />
            <span class="ml-2">{{ item.label }}</span>
          </a>
          <a
            v-else-if="!item.image"
            class="flex items-center p-4 cursor-pointer mb-2 gap-3"
          >
            <span
              class="inline-flex items-center justify-center rounded-full bg-primary text-primary-contrast w-12 h-12"
            >
              <i :class="[item.icon, 'text-lg']"></i>
            </span>
            <span class="inline-flex flex-col gap-1">
              <span class="font-bold text-lg">{{ item.label }}</span>
              <span class="whitespace-nowrap">{{ item.subtext }}</span>
            </span>
          </a>
          <div v-else class="flex flex-col items-start gap-4 p-2">
            <img alt="megamenu-demo" :src="item.image" class="w-full" />
            <span>{{ item.subtext }}</span>
            <Button :label="item.label" outlined />
          </div>
        </template>
        <template #end>
          <Avatar
            image="https://primefaces.org/cdn/primevue/images/avatar/amyelsner.png"
            shape="circle"
          />
        </template>
      </MegaMenu>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  const items = ref([
    {
      label: 'Company',
      root: true,
      items: [
        [
          {
            items: [
              {
                label: 'Features',
                icon: 'pi pi-list',
                subtext: 'Subtext of item',
              },
              {
                label: 'Customers',
                icon: 'pi pi-users',
                subtext: 'Subtext of item',
              },
              {
                label: 'Case Studies',
                icon: 'pi pi-file',
                subtext: 'Subtext of item',
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                label: 'Solutions',
                icon: 'pi pi-shield',
                subtext: 'Subtext of item',
              },
              {
                label: 'Faq',
                icon: 'pi pi-question',
                subtext: 'Subtext of item',
              },
              {
                label: 'Library',
                icon: 'pi pi-search',
                subtext: 'Subtext of item',
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                label: 'Community',
                icon: 'pi pi-comments',
                subtext: 'Subtext of item',
              },
              {
                label: 'Rewards',
                icon: 'pi pi-star',
                subtext: 'Subtext of item',
              },
              {
                label: 'Investors',
                icon: 'pi pi-globe',
                subtext: 'Subtext of item',
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                image:
                  'https://primefaces.org/cdn/primevue/images/uikit/uikit-system.png',
                label: 'GET STARTED',
                subtext: 'Build spectacular apps in no time.',
              },
            ],
          },
        ],
      ],
    },
    {
      label: 'Resources',
      root: true,
    },
    {
      label: 'Contact',
      root: true,
    },
  ]);
  </script>
  