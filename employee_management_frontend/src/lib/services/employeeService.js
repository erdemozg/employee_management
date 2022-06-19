import { apiClient } from '../client/apiClient'

export const employeeService = {
  getAll,
  get,
  add,
  update,
  delete: _delete,
  deleteAll
};

function getAll(searchTerm, skip, take = 100) {
  return apiClient.get(`/employee?searchTerm=${searchTerm}&skip=${skip}&take=${take}`)
    .then((response) => {
        return response;
    });
}

function get(id) {
  return apiClient.get(`/employee/${id}`)
    .then((response) => {
        return response;
    });
}

function add(employee){
  return apiClient.post(`/employee`, employee)
    .then((response) => {
        return response;
    });
}

function update(employee){
  return apiClient.put(`/employee`, employee)
    .then((response) => {
        return response;
    });
}

function _delete(id){
  return apiClient.delete(`/employee/${id}`)
    .then((response) => {
        return response;
    });
}

function deleteAll(){
  return apiClient.delete(`/employee/all`)
    .then((response) => {
        return response;
    });
}
