using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class TimesheetRow
    {
        public int TimesheetRowID { get; set; }
        public int TimesheetID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }

        public virtual Timesheet Timesheet { get; set; }
    }
}