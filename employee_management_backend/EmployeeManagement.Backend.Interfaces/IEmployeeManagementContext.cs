using EmployeeManagement.Backend.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployeeManagement.Backend.Interfaces
{
    public interface IEmployeeManagementContext : IDisposable
    {
        DatabaseFacade Database { get; }
        DbSet<Employee> Employees { get; set; }
    }
}