<script>
  import { employeeService } from '../lib/services/employeeService'
  import { importQueueService } from '../lib/services/importQueueService'
  import { dateHelper } from '../lib/helpers/dateHelper'
  import LoadingIcon from '../components/icons/IconLoading.vue'
  import Datepicker from '@vuepic/vue-datepicker'
  import '@vuepic/vue-datepicker/dist/main.css'

  export default {
    props: {
      id: {
        type: Number,
        required: false
      },
      mode: {
        type: String,
        required: true
      },
      isModal: {
        type: Boolean,
        required: false
      }
    },
    watch: { 
      id: function(newVal, oldVal) {
        this.employeeDatabaseId = newVal
        this.fetchEmployee()
      }
    },
    components: {
      LoadingIcon,
      Datepicker 
    },
    data() {
      return {
        serviceClient: this.mode == "IMPORT" ? importQueueService : employeeService,
        queryInProgress: false,
        employeeDatabaseId: this.id,
        employeeId: null,
        firstname: null,
        lastname: null,
        birthDate: null,
        errorMessage: null,
        successMessage: null
      }
    },
    computed: {
      isNewRecord(){
        return !this.employeeDatabaseId
      }
    },
    methods: {
      fetchEmployee() {
        this.queryInProgress = true
        this.serviceClient.get(this.employeeDatabaseId).then(result => {
          this.queryInProgress = false

          this.employeeId = result.employeeId
          this.firstname = result.firstname
          this.lastname = result.lastname
          this.birthDate = new Date(result.birthDate)
        }).catch(err => {
          this.queryInProgress = false

          console.log(err)
        })
      },
      saveEmployee(){
        const queryParam = {
          id: this.employeeDatabaseId,
          employeeId: this.employeeId,
          firstname: this.firstname,
          lastname: this.lastname,
          birthDate: dateHelper.dateToRFCDateString(this.birthDate)
        }

        this.queryInProgress = true
        if (this.isNewRecord) {
          this.serviceClient.add(queryParam).then(result => {
            this.queryInProgress = false

            if (result.isOK) {
              this.success()
            }
            else {
              this.flashError(result.message)
            }
          }).catch(err => {
            this.queryInProgress = false

            this.flashError("Error")
            console.log(err)
          })
        }
        else {
          this.serviceClient.update(queryParam).then(result => {
            this.queryInProgress = false
            if (result.isOK) {
              this.success()
            }
            else {
              this.flashError(result.message)
            }
          }).catch(err => {
            this.queryInProgress = false

            this.flashError('Error')
            console.log(err)
          })
        }
      },
      flashError(msg){
        this.errorMessage = msg
        setTimeout(() => {
          this.errorMessage = null
        }, 5000)
      },
      flashSuccess(msg){
        this.successMessage = msg
        setTimeout(() => {
          this.successMessage = null
        }, 5000)
      },
      format(date){
        if (date) {
          return date.toLocaleDateString()  
        }
        return null
      },
      success(){
        this.flashSuccess('Success!')
        this.$emit('refreshNeeded')
        if (this.isModal) {
          setTimeout(() => {
            this.closeModal(true)
          }, 1000);
        }
      },
      closeModal(){
        this.clearForm()
        this.$emit('closeModal')
      },
      clearForm(){
        this.queryInProgress = false
        this.employeeDatabaseId = this.id
        this.employeeId = null
        this.firstname = null
        this.lastname = null
        this.birthDate = null
        this.errorMessage = null
      }
    },
    mounted() {
      if (!this.isNewRecord) {
        this.fetchEmployee()
      }
    }
  }
</script>

<template>
  <div>
    <div class="flex flex-row justify-between pb-4 mb-4 border-b-2 border-gray-500">
      <h3 class="text-lg font-bold">Employee ({{ isNewRecord ? 'New' : 'Edit' }})</h3>
      <LoadingIcon v-if="queryInProgress" class="h-6"/>
    </div>
    <div class="form-control w-full mb-2">
      <label class="label">
        <span class="label-text">Employee Id</span>
      </label>
      <input type="text" v-model="employeeId" placeholder="Employee Id" class="input input-bordered w-full" />
    </div>
    <div class="form-control w-full mb-2">
      <label class="label">
        <span class="label-text">Firstname</span>
      </label>
      <input type="text" v-model="firstname" placeholder="Firstname" class="input input-bordered w-full" />
    </div>
    <div class="form-control w-full mb-2">
      <label class="label">
        <span class="label-text">Lastname</span>
      </label>
      <input type="text" v-model="lastname" placeholder="Lastname" class="input input-bordered w-full" />
    </div>
    <div class="form-control w-full mb-2">
      <label class="label">
        <span class="label-text">Date Of Birth</span>
      </label>
      <Datepicker
        inputClassName="dp-custom-input"
        v-model="birthDate" 
        :enableTimePicker="false" 
        :format="format" 
        :autoApply="true" />
    </div>
    <div class="h-4"></div>
    <button class="mb-4 btn btn-primary w-full" :class="{ 'btn-disabled': queryInProgress }" @click="saveEmployee">
      Save
    </button>

    <div v-if="!!errorMessage" class="mb-4 alert alert-error shadow-lg">
      <div>
        <svg xmlns="http://www.w3.org/2000/svg" class="stroke-current flex-shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
        <span>{{ errorMessage }}</span>
      </div>
    </div>

    <div v-if="!!successMessage" class="mb-4 alert alert-success shadow-lg">
      <div>
        <svg xmlns="http://www.w3.org/2000/svg" class="stroke-current flex-shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
        <span>{{ successMessage }}</span>
      </div>
    </div>

    <div v-if="isModal">
      <a href="#" class="btn w-full" @click="closeModal" >Close</a>
    </div>

  </div>
</template>

<style>
  .dp-custom-input {
    padding: 12px 12px 12px 36px;
    border-color: rgba(31, 41, 55, 0.2);
    border-radius: 8px;
  }
</style>