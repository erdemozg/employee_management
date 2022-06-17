using EmployeeManagement.Backend.Model.View.Base;

namespace EmployeeManagement.Backend.Model.View
{
    public class EmployeeModel : ViewModelBase
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? BirthDate { get; set; }

    }
}