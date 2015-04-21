using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class Timesheet
    {
        public int TimesheetID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public String Information { get; set; }
        public bool LunchBreak { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<TimesheetRow> TimesheetRows { get; set; }

    }

}