using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.Entity;
using EmployeeManagement.Backend.Model.View;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Backend.Tests.DatabaseContext
{
    public class MockEmployeeManagementDatabaseContext : IEmployeeManagementContext
    {
        public MockEmployeeManagementDatabaseContext()
        {

        }

        public DatabaseFacade Database => throw new NotImplementedException();

        public GenericResult AddEmployee(EmployeeModel employeeModel)
        {
            throw new NotImplementedException();
        }

        public GenericResult BulkInsertEmployeeImportQueueItems(List<EmployeeImportQueueItemModel> items)
        {
            return new GenericResult()
            {
                IsOK = true
            };
        }

        public GenericResult DeleteAllEmployeeImportQueueItems()
        {
            throw new NotImplementedException();
        }

        public GenericResult DeleteAllEmployees()
        {
            throw new NotImplementedException();
        }

        public GenericResult DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public GenericResult DeleteEmployeeImportQueueItem(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public EmployeeImportQueueItem GetEmployeeImportQueueItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeImportQueueItem> GetEmployeeImportQueueItems(string searchTerm, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetEmployees(string searchTerm, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public GenericResult ImportDataFromQueue()
        {
            throw new NotImplementedException();
        }

        public GenericResult UpdateEmployee(EmployeeModel employeeModel)
        {
            throw new NotImplementedException();
        }

        public GenericResult UpdateEmployeeImportQueueItem(EmployeeImportQueueItemModel model)
        {
            throw new NotImplementedException();
        }
    }
}
