using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String JobTitle { get; set; }

        public virtual ICollection<Timesheet> Timesheets { get; set; }
    }
}