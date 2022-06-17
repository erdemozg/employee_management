using EmployeeManagement.Backend.API.Controllers.Base;
using EmployeeManagement.Backend.Business;
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.View;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Backend.API.Controllers;

public class EmployeeController : BaseController
{
    private EmployeeManagementBusiness _business;

    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger, IEmployeeManagementContext context)
    {
        _logger = logger;
        _business = new EmployeeManagementBusiness(context);
    }

    [HttpGet(Name = "GetEmployees")]
    public IEnumerable<EmployeeModel> Get()
    {
        return _business.GetEmployees();
    }
}
