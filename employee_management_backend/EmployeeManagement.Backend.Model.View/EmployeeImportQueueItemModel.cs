using EmployeeManagement.Backend.Model.View.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Backend.Model.View
{
    public  class EmployeeImportQueueItemModel : ViewModelBase
    {
        public int? Id { get; set; }

        public string EmployeeId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ImportFileName { get; set; }

    }
}
