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
  <RouterLink :to="path" class="btn btn-primary gap-2">
    <i>
      <slot name="icon"></slot>
    </i>
    {{text}}
  </RouterLink>
</template>
