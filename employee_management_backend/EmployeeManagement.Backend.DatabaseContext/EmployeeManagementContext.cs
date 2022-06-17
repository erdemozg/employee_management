using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Backend.DatabaseContext
{
    public class EmployeeManagementContext : DbContext, IEmployeeManagementContext
    {
        public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
           : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("employee");
        }
    }
}