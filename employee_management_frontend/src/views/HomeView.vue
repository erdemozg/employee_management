<script>
import { employeeService } from '../lib/services/employeeService';
import EmployeeForm from '../components/EmployeeForm.vue'
import PlusIcon from '../components/icons/IconPlus.vue'
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
    PlusIcon,
    TrashIcon,
    EditIcon,
    LoadingIcon
  },
  data() {
    return {
      queryInProgress: false,
      employeeList: [],
      currentEmployeeId: null,
      isModalOpen: false,
      selectedEmployeeIds: [],
      searchTerm: null,
      queryTimer: undefined
    }
  },
  computed: {
    selectAll: {
      get: function () {
          return this.employeeList ? this.selectedEmployeeIds.length == this.employeeList.length : false
      },
      set: function (value) {
          var selected = []

          if (value) {
            this.employeeList.forEach(function (employee) {
                selected.push(employee.id);
            })
          }

          this.selectedEmployeeIds = selected
      }
    }
  },
  methods: {
    fetchData() {
      this.queryInProgress = true;
      const skip = this.employeeList.length
      employeeService.getAll(this.searchTerm || '', skip).then(result => {
        this.queryInProgress = false
        this.employeeList = [...this.employeeList, ...result]
      }).catch(err => this.queryInProgress = false)
    },
    openModal(employeeId){
      this.currentEmployeeId = employeeId
      this.isModalOpen = true
    },
    setCurrentEmployeeId(id){
      this.currentEmployeeId = id
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
      this.employeeList = []
      this.selectedEmployeeIds = []
      this.currentEmployeeId = null
      this.fetchData()
    },
    deleteEmployee(employee){
      const res = confirm(`Are you sure you want to delete employee: ${employee.firstname} ${employee.lastname}?`)
      if (res) {
        employeeService.delete(employee.id).then(result => {
          if (result.isOK) {
            this.employeeList = this.employeeList.filter(item => item.id != employee.id)  
          }
          else {
            alert(result.message)
          }
        });
      }
    },
    async deleteEmployees(){
      if (this.selectedEmployeeIds.length == 0) {
        alert("Please select at least one record to delete!")
      }
      else {
        const res = confirm(`Are you sure you want to delete ${this.selectedEmployeeIds.length} employees?`)
        if (res) {
          this.queryInProgress = true;
          for (let index = 0; index < this.selectedEmployeeIds.length; index++) {
            const id = this.selectedEmployeeIds[index]
            await employeeService.delete(id)
            this.employeeList = this.employeeList.filter(item => item.id != id)
          }
          this.queryInProgress = false;
        }
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
        Employees
      </h1>
      <LoadingIcon v-if="queryInProgress" />
    </div>
    <!-- Content -->
    <div>
      <div class="mb-4 flex flex-row justify-between items-center">
        <div class="flex flex-row space-x-2">
          <a class="btn btn-sm btn-outline btn-success gap-2" @click="openModal(null)">
            <PlusIcon/>
            New
          </a>
          <button class="btn btn-sm btn-outline gap-2" :class="{'btn-disabled': queryInProgress}" @click="refreshTable">
            <TrashIcon/>
            Refresh
          </button>
          <button class="btn btn-sm btn-outline btn-error gap-2" @click="deleteEmployees">
            <TrashIcon/>
            Delete Selected Rows
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
            <th class="border border-slate-300 font-semibold p-4 text-slate-900 text-left"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!employeeList || employeeList.length == 0">
            <td class="border border-slate-300 p-4 text-slate-500" colspan="6">
              <div class="p-4 text-center">
                No records
              </div>
            </td>
          </tr>
          <tr v-for="employee in employeeList" :key="employee.id" class="border border-slate-300 p-4 text-slate-500">
            <th class="border border-slate-300 p-4 text-slate-500">
              <label>
                <input type="checkbox" class="checkbox" v-model="selectedEmployeeIds" :value="employee.id" number/>
              </label>
            </th>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ employee.employeeId }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ employee.firstname }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ employee.lastname }}
            </td>
            <td class="border border-slate-300 p-4 text-slate-500">
              {{ new Date(employee.birthDate).toLocaleDateString() }}
            </td>
            <th class="border border-slate-300 p-4 text-slate-500">
              <div class="flex flex-row justify-end">
                <a href="#my-modal" class="btn btn-sm btn-square btn-outline mr-2" :class="{'btn-disabled': queryInProgress}" @click="openModal(employee.id)">
                  <EditIcon/>
                </a>
                <button class="btn btn-sm btn-error btn-square btn-outline" :class="{'btn-disabled': queryInProgress}" @click="deleteEmployee(employee)">
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
      <EmployeeForm :id="currentEmployeeId" :isModal="true" mode="EMPLOYEE" @refresh-needed="refreshTable" @close-modal="closeModal" />
    </div>
  </div>
</template>
