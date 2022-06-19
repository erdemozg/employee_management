<script>
import { importQueueService } from '../lib/services/importQueueService';
import EmployeeForm from '../components/EmployeeForm.vue'
import ImportIcon from '@/components/icons/IconAddDocument.vue'
import TrashIcon from '../components/icons/IconTrash.vue'
import EditIcon from '../components/icons/IconEdit.vue'
import LoadingIcon from '../components/icons/IconLoading.vue'

export default {
  created () {
    window.addEventListener('scroll', this.handleScroll);
  },
  destroyed () {
    window.removeEventListener('scroll', this.handleScroll);
  },
  components: {
    EmployeeForm,
    TrashIcon,
    EditIcon,
    LoadingIcon,
    ImportIcon
  },
  data() {
    return {
      queryInProgress: false,
      selectedFile: null,
      employeeImportQueueItems: [],
      currentQueueItemId: null,
      isModalOpen: false,
      selectedQueueItemIds: [],
      searchTerm: null,
      queryTimer: undefined
    }
  },
  computed: {
    selectAll: {
      get: function () {
          return this.employeeImportQueueItems ? this.selectedQueueItemIds.length == this.employeeImportQueueItems.length : false
      },
      set: function (value) {
          var selected = []

          if (value) {
            this.employeeImportQueueItems.forEach(function (queueItem) {
                selected.push(queueItem.id)
            })
          }

          this.selectedQueueItemIds = selected
      }
    }
  },
  methods: {
    fetchData() {
      this.queryInProgress = true;
      const skip = this.employeeImportQueueItems.length
      importQueueService.getAll(this.searchTerm || '', skip).then(result => {
        this.queryInProgress = false
        this.employeeImportQueueItems = [...this.employeeImportQueueItems, ...result]
      }).catch(err => this.queryInProgress = false)
    },
    uploadFile() {
      this.selectedFile = this.$refs.fileupload.files[0]
    },
    submitFile() {
      if (!this.selectedFile) {
        alert("Please choose a file to upload!")
      }
      else {
        if(!this.selectedFile.name.endsWith(".csv")){
          alert("Only .csv files are allowed")
          return
        }
        const formData = new FormData()
        formData.append('file', this.selectedFile)
        this.queryInProgress = true
        importQueueService.submitFile(formData).then(res => {
          this.queryInProgress = false
          this.$refs.fileupload.value = null
          if (res.isOK) {
            this.refreshTable()  
          }
          else {
            alert(res.message);
          }
        })
        .catch(error => {
          this.queryInProgress = false
          console.log(error)
        })
      }
    },
    deleteQueueItem(employee){
      const res = confirm(`Are you sure you want to delete employee: ${employee.firstname} ${employee.lastname}?`)
      if (res) {
        importQueueService.delete(employee.id).then(result => {
          if (result.isOK) {
            this.employeeImportQueueItems = this.employeeImportQueueItems.filter(item => item.id != employee.id)  
          }
          else {
            alert(result.message)
          }
        });
      }
    },
    async deleteQueueItems(){
      if (this.selectedQueueItemIds.length == 0) {
        alert("Please select at least one record to delete!")
      }
      else {
        const res = confirm(`Are you sure you want to delete ${this.selectedQueueItemIds.length} employees?`)
        if (res) {
          this.queryInProgress = true;
          for (let index = 0; index < this.selectedQueueItemIds.length; index++) {
            const id = this.selectedQueueItemIds[index]
            await importQueueService.delete(id)
            this.employeeImportQueueItems = this.employeeImportQueueItems.filter(item => item.id != id)
          }
          this.queryInProgress = false;
        }
      }
    },
    deleteAllQueueItems(){
      const res = confirm(`Are you sure you want to delete all data?`)
      if (res) {
        this.queryInProgress = true;
        importQueueService.deleteAll().then(result => {
          this.queryInProgress = false;
          if (result.isOK) {
            this.refreshTable()
          }
          else {
            alert(result.message)
          }
        });
      }
    },
    openModal(employeeId){
      this.currentQueueItemId = employeeId
      this.isModalOpen = true
    },
    closeModal(){
      this.isModalOpen = false
    },
    handleScroll(){
      let bottomOfWindow = document.documentElement.scrollTop + window.innerHeight === document.documentElement.offsetHeight;
      if (bottomOfWindow) {
        if (!this.queryInProgress) {
          this.fetchData()  
        }
      }
    },
    searchTermChanged(){
      clearTimeout(this.queryTimer)
      this.queryTimer = setTimeout(() => {
          this.refreshTable()
      }, 1000)
    },
    refreshTable(){
      this.employeeImportQueueItems = []
      this.selectedQueueItemIds = []
      this.currentQueueItemId = null
      this.fetchData()
    },
    importAll(){
      const res = confirm(`All the data will imported to the employees table?`)
      if (res) {
        this.queryInProgress = true;
        importQueueService.importAll().then(result => {
          this.queryInProgress = false;
          if (result.isOK) {
            this.refreshTable()
            alert("Successfully imported!") 
          }
          else {
            alert(result.message)
          }
        });
      }
    }
  },
  mounted() {
    this.fetchData()
  },

}

</script>

<template>
  <div>
    <!-- Header -->
    <div class="flex flex-row justify-between pb-8 mb-8 border-b-2 border-gray-500">
      <h1 class="text-3xl font-bold">
        Import Data
      </h1>
      <LoadingIcon v-if="queryInProgress" />
    </div>
    <!-- Content -->
    <div>
      <div class="mb-4">
        <input type="file" @change="uploadFile" ref="fileupload" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"/>
      </div>
      <div class="mb-4 flex flex-row justify-between items-center">
        <div class="flex flex-row space-x-2">  
          <a class="btn btn-sm btn-outline btn-success gap-2" :class="{'btn-disabled': queryInProgress}" @click="submitFile">
            <ImportIcon/>
            Upload File
          </a>
          <button class="btn btn-sm btn-outline gap-2" :class="{'btn-disabled': queryInProgress}" @click="refreshTable">
            <TrashIcon/>
            Refresh
          </button>
          <button class="btn btn-sm btn-outline btn-error gap-2" :class="{'btn-disabled': queryInProgress}" @click="deleteQueueItems">
            <TrashIcon/>
            Delete Selected Rows
          </button>
          <button class="btn btn-sm btn-outline btn-error gap-2" :class="{'btn-disabled': queryInProgress}" @click="deleteAllQueueItems">
            <TrashIcon/>
            Delete All
          </button> 
          <button class="btn btn-sm btn-outline btn-primary gap-2" :class="{'btn-disabled': queryInProgress}" @click="importAll">
            <TrashIcon/>
            Import All
          </button>          
        </div>
        <div>
          <input type="text" placeholder="filter..." v-model="searchTerm" @keyup="searchTermChanged" class="input input-sm input-bordered" />
        </div>
      </div>
      <table class="border-collapse w-full border border-slate-400 text-sm shadow-sm">
        <thead>
          <tr>
            <th class="border border-slate-300 p-4 text-slate-500">
              <label>
                <input type="checkbox" class="checkbox" v-model="selectAll"/>
              </label>
            </th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Employee Id</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Firstname</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Lastname</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Date of Birth</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Import File</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left">Created At</th>
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!employeeImportQueueItems || employeeImportQueueItems.length == 0">
            <td class="border border-slate-300 p-4 text-slate-500" colspan="8">
              <div class="p-4 text-center">
                No records
              </div>
            </td>
          </tr>
          <tr v-for="queueItem in employeeImportQueueItems" :key="queueItem.id" class="border border-slate-300 p-4 text-slate-500">
            <th class="border border-slate-300 p-4 text-slate-500">
              <label>
                <input type="checkbox" class="checkbox" v-model="selectedQueueItemIds" :value="queueItem.id" number/>
              </label>
            </th>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ queueItem.employeeId }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ queueItem.firstname }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ queueItem.lastname }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ new Date(queueItem.birthDate).toLocaleDateString() }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ queueItem.importFileName }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ new Date(queueItem.createdAt).toLocaleString() }}
            </td>
            <th class="border border-slate-300 p-4 text-slate-500">
              <div class="flex flex-row justify-end">
                <a href="#my-modal" class="btn btn-sm btn-square btn-outline mr-2" :class="{'btn-disabled': queryInProgress}" @click="openModal(queueItem.id)">
                  <EditIcon/>
                </a>
                <button class="btn btn-sm btn-error btn-square btn-outline" :class="{'btn-disabled': queryInProgress}" @click="deleteQueueItem(queueItem)">
                  <TrashIcon/>
                </button>
              </div>
              
            </th>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <!-- Modal -->
  <input type="checkbox" id="my-modal" class="modal-toggle" :checked="isModalOpen">
  <div class="modal">
    <div class="modal-box">
      <EmployeeForm :id="currentQueueItemId" :isModal="true" mode="IMPORT" @refresh-needed="refreshTable" @close-modal="closeModal" />
    </div>
  </div>
</template>
