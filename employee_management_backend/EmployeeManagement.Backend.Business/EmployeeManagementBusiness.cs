
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.Entity;
using EmployeeManagement.Backend.Model.View;

namespace EmployeeManagement.Backend.Business
{
    public class EmployeeManagementBusiness
    {
        private IEmployeeManagementContext _context;

        public EmployeeManagementBusiness(IEmployeeManagementContext context)
        {
            _context = context;
        }

        public List<EmployeeModel> GetEmployees(string searchTerm, int skip, int take)
        {
            var res = new List<EmployeeModel>();

            var employeeRecords = _context.GetEmployees(searchTerm, skip, take);

            if (employeeRecords != null)
            {
                res = employeeRecords.Select(p => new EmployeeModel()
                {
                    BirthDate = p.BirthDate,
                    CreatedAt = p.CreatedAt,
                    EmployeeId = p.EmployeeId,
                    Firstname = p.Firstname,
                    Id = p.Id,
                    Lastname = p.Lastname,
                    UpdatedAt = p.UpdatedAt
                }).ToList();
            }

            return res;
        }

        public EmployeeModel GetEmployee(int id)
        {
            EmployeeModel res = null;

            var employeeRecord = _context.GetEmployee(id);

            if (employeeRecord != null)
            {
                res = new EmployeeModel()
                {
                    BirthDate = employeeRecord.BirthDate,
                    CreatedAt = employeeRecord.CreatedAt,
                    EmployeeId = employeeRecord.EmployeeId,
                    Firstname = employeeRecord.Firstname,
                    Id = employeeRecord.Id,
                    Lastname = employeeRecord.Lastname,
                    UpdatedAt = employeeRecord.UpdatedAt
                };
            }

            return res;
        }

        public GenericResult AddOrUpdateEmployee(EmployeeModel employee)
        {
            var ret = new GenericResult();

            if (employee != null)
            {
                var newRecord = !employee.Id.HasValue;

                if (newRecord)
                {
                    ret = _context.AddEmployee(employee);
                }
                else
                {
                    ret = _context.UpdateEmployee(employee);
                }
            }
            else 
            {
                ret.Message = "ERROR_PARAM";
            }

            return ret;
        }

        public GenericResult DeleteEmployee(int id)
        {
            var ret = _context.DeleteEmployee(id);

            return ret;
        }
        
    }
}