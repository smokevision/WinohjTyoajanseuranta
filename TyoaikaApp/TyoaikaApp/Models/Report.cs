using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class Report
    {
        public String ApplicationUserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan TimeSum { get; set; }
        public TimeSpan TimeTotal { get; set; }
        public int LunchBreaks { get; set; }
        public TimeSpan LunchBreakTime { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<ReportRow> ReportRows { get; set; }
    }
}