<script>
import { RouterLink } from 'vue-router'

export default {
  props: {
    text: {
      type: String,
      required: true
    },
    path: {
      type: String,
      required: true
    }
  },
  components: {
    RouterLink
  }
}
</script>

<template>

  <RouterLink :to="path" class="flex flex-row items-center justify-center px-6 py-2 bg-blue-500 text-gray-100 mr-2 rounded-full focus:ring-2 focus:ring-gray-100">
    <i class="mr-2">
      <slot name="icon"></slot>
    </i>
    {{text}}
  </RouterLink>
</template>
