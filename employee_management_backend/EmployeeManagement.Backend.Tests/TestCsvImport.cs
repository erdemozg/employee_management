using EmployeeManagement.Backend.Business;
using EmployeeManagement.Backend.Tests.DatabaseContext;

namespace EmployeeManagement.Backend.Tests
{
    [TestClass]
    public class TestCsvImport
    {
        EmployeeImportQueueBusiness _business = null;

        public TestCsvImport() 
        {
            var context = new MockEmployeeManagementDatabaseContext();

            _business = new EmployeeImportQueueBusiness(context);
        }

        [TestMethod]
        public void Test_Valid_Csv_File_Pass()
        {
            var filePath = System.IO.Path.Join("Resources", "valid.csv");

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Valid test csv file not found!");
            }

            var res = _business.ProcessCsvFile(filePath, Path.GetFileName(filePath));

            Assert.IsNotNull(res);

            Assert.AreEqual(res.IsOK, true);
        }

        [TestMethod]
        public void Test_Invalid_Csv_File_Fail()
        {
            var filePath = System.IO.Path.Join("Resources", "invalid.csv");

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Inalid test csv file not found!");
            }

            var res = _business.ProcessCsvFile(filePath, Path.GetFileName(filePath));

            Assert.IsNotNull(res);

            Assert.AreEqual(res.IsOK, false);
        }
    }
}