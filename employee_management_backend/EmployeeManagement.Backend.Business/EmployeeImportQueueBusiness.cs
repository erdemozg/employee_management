using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Backend.Business
{
    public class EmployeeImportQueueBusiness
    {
        private IEmployeeManagementContext _context;

        public EmployeeImportQueueBusiness(IEmployeeManagementContext context)
        {
            _context = context;
        }

        public GenericResult ProcessCsvFile(string filePath, string originalFileName)
        {
            var ret = new GenericResult();

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    NewLine = Environment.NewLine,
                    HasHeaderRecord = true
                };

                using (var reader = new StreamReader(filePath))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        var csvEmployees = csv.GetRecords<CsvEmployee>();

                        var itemsBatch = new List<EmployeeImportQueueItemModel>();

                        foreach (var csvEmployee in csvEmployees)
                        {
                            var queueItem = new EmployeeImportQueueItemModel()
                            {
                                BirthDate = csvEmployee.DateOfBirth,
                                CreatedAt = DateTime.UtcNow,
                                EmployeeId = csvEmployee.EmpId,
                                Firstname = csvEmployee.Firstname,
                                Lastname = csvEmployee.Lastname,
                                ImportFileName = originalFileName
                            };

                            itemsBatch.Add(queueItem);

                            if (itemsBatch.Count == 50_000)
                            {
                                _context.BulkInsertEmployeeImportQueueItems(itemsBatch);

                                itemsBatch = new List<EmployeeImportQueueItemModel>();
                            }
                        }

                        _context.BulkInsertEmployeeImportQueueItems(itemsBatch);
                    }
                }

                ret.IsOK = true;
            }
            catch (Exception ex)
            {
                ret.Message = ex.Message;
            }

            return ret;
        }

        public List<EmployeeImportQueueItemModel> GetEmployeeImportQueueItems(string searchTerm, int skip, int take)
        {
            var res = new List<EmployeeImportQueueItemModel>();

            var employeeImportQueueItemRecords = _context.GetEmployeeImportQueueItems(searchTerm, skip, take);

            if (employeeImportQueueItemRecords != null)
            {
                res = employeeImportQueueItemRecords.Select(p => new EmployeeImportQueueItemModel()
                {
                    BirthDate = p.BirthDate,
                    CreatedAt = p.CreatedAt,
                    EmployeeId = p.EmployeeId,
                    Firstname = p.Firstname,
                    Id = p.Id,
                    Lastname = p.Lastname,
                    UpdatedAt = p.UpdatedAt,
                    ImportFileName = p.ImportFileName
                }).ToList();
            }

            return res;
        }

        public EmployeeImportQueueItemModel GetEmployeeImportQueueItem(int id)
        {
            EmployeeImportQueueItemModel res = null;

            var employeeImportQueueItemRecord = _context.GetEmployeeImportQueueItem(id);

            if (employeeImportQueueItemRecord != null)
            {
                res = new EmployeeImportQueueItemModel()
                {
                    BirthDate = employeeImportQueueItemRecord.BirthDate,
                    CreatedAt = employeeImportQueueItemRecord.CreatedAt,
                    EmployeeId = employeeImportQueueItemRecord.EmployeeId,
                    Firstname = employeeImportQueueItemRecord.Firstname,
                    Id = employeeImportQueueItemRecord.Id,
                    Lastname = employeeImportQueueItemRecord.Lastname,
                    UpdatedAt = employeeImportQueueItemRecord.UpdatedAt,
                    ImportFileName = employeeImportQueueItemRecord.ImportFileName
                };
            }

            return res;
        }

        public GenericResult UpdateEmployeeImportQueueItem(EmployeeImportQueueItemModel employee)
        {
            var ret = new GenericResult();

            if (employee != null)
            {
                ret = _context.UpdateEmployeeImportQueueItem(employee);
            }
            else
            {
                ret.Message = "ERROR_PARAM";
            }

            return ret;
        }

        public GenericResult DeleteEmployeeImportQueueItem(int id)
        {
            return _context.DeleteEmployeeImportQueueItem(id);
        }

        public GenericResult DeleteAllEmployeeImportQueueItems()
        {
            return _context.DeleteAllEmployeeImportQueueItems();
        }

        public GenericResult ImportDataFromQueue()
        {
            return _context.ImportDataFromQueue();
        }

    }

    public class CsvEmployee
    {
        [Name("Emp ID")]
        public string EmpId { get; set; }

        [Name("First Name")]
        public string Firstname { get; set; }

        [Name("Last Name")]
        public string Lastname { get; set; }

        [Name("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
