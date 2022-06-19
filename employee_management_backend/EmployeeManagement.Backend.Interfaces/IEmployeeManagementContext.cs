using EmployeeManagement.Backend.Model.Entity;
using EmployeeManagement.Backend.Model.View;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployeeManagement.Backend.Interfaces
{
    public interface IEmployeeManagementContext : IDisposable
    {
        DatabaseFacade Database { get; }

        #region employee

        List<Employee> GetEmployees(string searchTerm, int skip, int take);

        Employee GetEmployee(int id);

        GenericResult AddEmployee(EmployeeModel employeeModel);

        GenericResult UpdateEmployee(EmployeeModel employeeModel);

        GenericResult DeleteEmployee(int id);

        #endregion


        #region employee import queue

        GenericResult BulkInsertEmployeeImportQueueItems(List<EmployeeImportQueueItemModel> items);

        List<EmployeeImportQueueItem> GetEmployeeImportQueueItems(string searchTerm, int skip, int take);

        EmployeeImportQueueItem GetEmployeeImportQueueItem(int id);

        GenericResult UpdateEmployeeImportQueueItem(EmployeeImportQueueItemModel model);

        GenericResult DeleteEmployeeImportQueueItem(int id);

        GenericResult DeleteAllEmployeeImportQueueItems();

        GenericResult ImportDataFromQueue();

        #endregion

    }
}