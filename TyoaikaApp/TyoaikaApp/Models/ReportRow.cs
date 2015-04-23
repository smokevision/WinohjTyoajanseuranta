using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class ReportRow
    {
        public DateTime Date;
        public Timesheet Timesheet;

        public virtual Report Report { get; set; }

    }
}