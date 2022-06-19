using EmployeeManagement.Backend.API.Controllers.Base;
using EmployeeManagement.Backend.Business;
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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

    [HttpGet]
    public IEnumerable<EmployeeModel> GetAll(string? searchTerm, int skip, int take)
    {
        searchTerm = searchTerm ?? string.Empty;

        return _business.GetEmployees(searchTerm, skip, take);
    }

    [HttpGet("{id}")]
    public EmployeeModel GetById(int id)
    {
        return _business.GetEmployee(id);
    }

    [HttpPost]
    public GenericResult Post(EmployeeModel employee)
    {
        return _business.AddOrUpdateEmployee(employee);
    }

    [HttpPut]
    public GenericResult Put(EmployeeModel employee)
    {
        return _business.AddOrUpdateEmployee(employee);
    }

    [HttpDelete("{id}")]
    public GenericResult Delete(int id)
    {
        return _business.DeleteEmployee(id);
    }

    [HttpDelete("all")]
    public GenericResult DeleteAll()
    {
        return _business.DeleteAllEmployees();
    }
}
