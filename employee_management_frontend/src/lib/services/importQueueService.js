import { apiClient } from '../client/apiClient'

export const importQueueService = {
  submitFile,
  getAll,
  get,
  update,
  delete: _delete,
  deleteAll,
  importAll
};

function submitFile(formData){
  console.log(formData)
  return apiClient.post(`/employeeImportQueue/fileUpload`, formData, false)
    .then((response) => {
        return response;
    })
}

function getAll(searchTerm, skip, take = 100) {
  return apiClient.get(`/employeeImportQueue?searchTerm=${searchTerm}&skip=${skip}&take=${take}`)
    .then((response) => {
        return response;
    });
}

function get(id) {
  return apiClient.get(`/employeeImportQueue/${id}`)
    .then((response) => {
        return response;
    });
}

function update(employee){
  return apiClient.put(`/employeeImportQueue`, employee)
    .then((response) => {
        return response;
    });
}

function _delete(id){
  return apiClient.delete(`/employeeImportQueue/${id}`)
    .then((response) => {
        return response;
    });
}

function deleteAll(){
  return apiClient.delete(`/employeeImportQueue/all`)
    .then((response) => {
        return response;
    });
}

function importAll(){
  return apiClient.post(`/employeeImportQueue/import`)
    .then((response) => {
        return response;
    });
}
