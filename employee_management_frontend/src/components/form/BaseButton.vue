<script>
export default {
  props: {
    text: {
      type: String,
      required: false
    }
  }
}
</script>

<template>
  <button class="btn btn-outline btn-sm btn-success gap-2">
    <i>
      <slot name="icon"></slot>
    </i>
    {{text}}
  </button>
</template>
