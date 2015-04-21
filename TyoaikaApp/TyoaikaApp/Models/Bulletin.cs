using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyoaikaApp.Models
{
    public class Bulletin
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public String Header { get; set; }
        public String Content { get; set; }
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }
    }
}