using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string NationalIdType { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }

        public virtual IList<Workday> Workdays { get; set; }

        public Employee()
        {
            IsActive = true;
        }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public string FullInfo()
        {
            return FullName() + " - " + Position;
        }
    }
}
