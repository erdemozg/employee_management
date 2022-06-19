using EmployeeManagement.Backend.API.Controllers.Base;
using EmployeeManagement.Backend.Business;
using EmployeeManagement.Backend.Interfaces;
using EmployeeManagement.Backend.Model.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeManagement.Backend.API.Controllers
{
    public class EmployeeImportQueueController : BaseController
    {
        private EmployeeImportQueueBusiness _business;

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeImportQueueController(ILogger<EmployeeController> logger, IEmployeeManagementContext context)
        {
            _logger = logger;
            _business = new EmployeeImportQueueBusiness(context);
        }

        [HttpPost("fileUpload")]
        [RequestSizeLimit(2_000_000_000)]
        public async Task<GenericResult> FileUpload([FromQuery] IFormFile file)
        {
            var res = new GenericResult();

            if (file.Length > 0 && file.FileName.ToLower().EndsWith(".csv"))
            {
                var filesFolderPath = System.Environment.GetEnvironmentVariable("FILES_PATH");

                if (string.IsNullOrWhiteSpace(filesFolderPath))
                {
                    throw new ArgumentException("FILES_PATH env var not found!");
                }
                
                string filePath =  System.IO.Path.Join(filesFolderPath, $"{Guid.NewGuid().ToString()}.csv");

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                res = _business.ProcessCsvFile(filePath, file.FileName);
            }
            else
            {
                res.Message = "Param error!";
            }

            return res;
        }

        [HttpGet]
        public IEnumerable<EmployeeImportQueueItemModel> GetAll(string? searchTerm, int skip, int take)
        {
            searchTerm = searchTerm ?? string.Empty;

            return _business.GetEmployeeImportQueueItems(searchTerm, skip, take);
        }

        [HttpGet("{id}")]
        public EmployeeImportQueueItemModel GetById(int id)
        {
            return _business.GetEmployeeImportQueueItem(id);
        }

        [HttpPut]
        public GenericResult Put(EmployeeImportQueueItemModel employee)
        {
            return _business.UpdateEmployeeImportQueueItem(employee);
        }

        [HttpDelete("{id}")]
        public GenericResult Delete(int id)
        {
            return _business.DeleteEmployeeImportQueueItem(id);
        }

        [HttpDelete("all")]
        public GenericResult DeleteAll()
        {
            return _business.DeleteAllEmployeeImportQueueItems();
        }

        [HttpPost("import")]
        public GenericResult ImportDataFromQueue()
        {
            return _business.ImportDataFromQueue();
        }
    }
}
