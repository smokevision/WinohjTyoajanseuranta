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
        public TimeSpan EveningSum { get; set; }
        public TimeSpan NightSum { get; set; }
        public TimeSpan SundaySum { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<ReportRow> ReportRows { get; set; }
    }
}