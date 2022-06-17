import { apiClient } from '../client/apiClient'

export const employeeService = {
  getEmployees
};

function getEmployees(skip, take, count = 100) {
  return apiClient.get(`/employee?skip=${skip}&take=${take}&count=${count}`)
    .then((response) => {
        return response;
    });
}
