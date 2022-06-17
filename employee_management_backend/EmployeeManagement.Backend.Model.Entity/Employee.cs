using EmployeeManagement.Backend.Model.Entity.Base;

namespace EmployeeManagement.Backend.Model.Entity
{
    public class Employee : EntityModelBase
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? BirthDate { get; set; }

    }
}