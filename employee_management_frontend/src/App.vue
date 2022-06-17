<script setup>
import { RouterView } from 'vue-router'
import NavigationMenuItem from '@/components/form/NavigationMenuItem.vue'
import CommunityIcon from '@/components/icons/IconCommunity.vue'
import ImportIcon from '@/components/icons/IconAddDocument.vue'
</script>

<template>
  <!-- main wrapper -->
  <div class="flex flex-row">
    <!-- left menu -->
    <div class="h-screen p-4 bg-gray-900 w-72">
      <!-- brand -->
      <a href="/" class="p-4 flex flex-col mb-8 border-b-2">
        <span class="text-gray-100 text-2xl">
          Employee
        </span>
        <span class="text-gray-100 text-sm">
          Management
        </span>
      </a>
      <!-- navigation menu -->
      <div class="flex flex-col space-y-4">
        <NavigationMenuItem text="Employees" path="/">
          <template #icon>
            <CommunityIcon />
          </template>
        </NavigationMenuItem>
        <NavigationMenuItem text="Import Data" path="/import">
          <template #icon>
            <ImportIcon />
          </template>
        </NavigationMenuItem>
      </div>
    </div>
    <!-- content -->
    <div class="p-8 bg-gray-200 w-full">
      <RouterView />
    </div>
  </div>
</template>
