using EFCore.BulkExtensions;
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.Entity;
using EmployeeManagement.Backend.Model.View;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Backend.DatabaseContext
{
    public class EmployeeManagementContext : DbContext, IEmployeeManagementContext
    {
        public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeImportQueueItem> EmployeeImportQueueItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("employee");
            modelBuilder.Entity<EmployeeImportQueueItem>().ToTable("employee_import_queue_item");
        }

        #region employee

        public List<Employee> GetEmployees(string searchTerm, int skip, int take)
        {
            return this.Employees
                .Where(p =>
                    p.Firstname.Contains(searchTerm)
                    || p.Lastname.Contains(searchTerm)
                    || p.EmployeeId.Contains(searchTerm))
                .OrderBy(p => p.Firstname)
                .ThenBy(p => p.Lastname)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public Employee GetEmployee(int id)
        {
            return this.Employees.Where(p => p.Id == id).FirstOrDefault();
        }

        public GenericResult AddEmployee(EmployeeModel employeeModel)
        {
            var res = new GenericResult();

            try
            {
                var employeeEntity = new Employee()
                {
                    BirthDate = employeeModel.BirthDate,
                    CreatedAt = DateTime.UtcNow,
                    EmployeeId = employeeModel.EmployeeId,
                    Firstname = employeeModel.Firstname,
                    Lastname = employeeModel.Lastname
                };

                this.Employees.Add(employeeEntity);

                this.SaveChanges();

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResult UpdateEmployee(EmployeeModel employeeModel)
        {
            var res = new GenericResult();

            try
            {
                var employeeEntity = this.GetEmployee(employeeModel.Id.Value);

                employeeEntity.BirthDate = employeeModel.BirthDate;
                employeeEntity.UpdatedAt = DateTime.UtcNow;
                employeeEntity.EmployeeId = employeeModel.EmployeeId;
                employeeEntity.Firstname = employeeModel.Firstname;
                employeeEntity.Lastname = employeeModel.Lastname;

                this.SaveChanges();

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResult DeleteEmployee(int id)
        {
            var res = new GenericResult();

            try
            {
                var employee = this.GetEmployee(id);

                if (employee != null)
                {
                    this.Employees.Remove(employee);
                    
                    this.SaveChanges();
                    
                    res.IsOK = true;
                }

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        #endregion


        #region employee import queue

        public GenericResult BulkInsertEmployeeImportQueueItems(List<EmployeeImportQueueItemModel> items)
        {
            var res = new GenericResult();

            try
            {
                this.BulkInsert(items.Select(p => new EmployeeImportQueueItem()
                {
                    BirthDate = p.BirthDate,
                    CreatedAt = DateTime.UtcNow,
                    EmployeeId = p.EmployeeId,
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    ImportFileName = p.ImportFileName
                }).ToList());

                this.BulkSaveChanges();

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public List<EmployeeImportQueueItem> GetEmployeeImportQueueItems(string searchTerm, int skip, int take)
        {
            return this.EmployeeImportQueueItems
                .Where(p => 
                    p.Firstname.Contains(searchTerm) 
                    || p.Lastname.Contains(searchTerm) 
                    || p.EmployeeId.Contains(searchTerm)
                    || p.ImportFileName.Contains(searchTerm))   
                .OrderBy(p => p.Firstname)
                .ThenBy(p => p.Lastname)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public EmployeeImportQueueItem GetEmployeeImportQueueItem(int id)
        {
            return this.EmployeeImportQueueItems.Where(p => p.Id == id).FirstOrDefault();
        }

        public GenericResult UpdateEmployeeImportQueueItem(EmployeeImportQueueItemModel model)
        {
            var res = new GenericResult();

            try
            {
                var employeeImportQueueItemEntity = this.GetEmployeeImportQueueItem(model.Id.Value);

                employeeImportQueueItemEntity.BirthDate = model.BirthDate;
                employeeImportQueueItemEntity.UpdatedAt = DateTime.UtcNow;
                employeeImportQueueItemEntity.EmployeeId = model.EmployeeId;
                employeeImportQueueItemEntity.Firstname = model.Firstname;
                employeeImportQueueItemEntity.Lastname = model.Lastname;

                this.SaveChanges();

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResult DeleteEmployeeImportQueueItem(int id)
        {
            var res = new GenericResult();

            try
            {
                var employeeImportQueueItemEntity = this.GetEmployeeImportQueueItem(id);

                if (employeeImportQueueItemEntity != null)
                {
                    this.EmployeeImportQueueItems.Remove(employeeImportQueueItemEntity);

                    this.SaveChanges();

                    res.IsOK = true;
                }

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResult DeleteAllEmployeeImportQueueItems()
        {
            var res = new GenericResult();

            try
            {
                this.Database.ExecuteSqlRaw("TRUNCATE TABLE employee_import_queue_item");

                res.IsOK = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResult ImportDataFromQueue() 
        {
            var res = new GenericResult();

            try
            {
                if (this.EmployeeImportQueueItems.Any())
                {
                    while (this.EmployeeImportQueueItems.Any())
                    {
                        var batch = this.EmployeeImportQueueItems.Take(50_000).ToList();

                        this.BulkInsert(batch.Select(p => new Employee()
                        {
                            BirthDate = p.BirthDate,
                            CreatedAt = DateTime.UtcNow,
                            EmployeeId = p.EmployeeId,
                            Firstname = p.Firstname,
                            Lastname = p.Lastname
                        }).ToList());

                        this.BulkDelete(batch);
                    }

                    res.IsOK = true;
                }
                else {
                    res.Message = "No data to import!";   
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }

            return res;
        }

        #endregion
    }
}