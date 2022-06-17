
using EmployeeManagement.Backend.Interfaces;
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

        public List<EmployeeModel> GetEmployees()
        {
            return _context.Employees.Select(p => new EmployeeModel() { 
                BirthDate = p.BirthDate,
                CreatedAt = p.CreatedAt,
                EmployeeId = p.EmployeeId,
                Firstname = p.Firstname,
                Id = p.Id,  
                Lastname = p.Lastname,  
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}