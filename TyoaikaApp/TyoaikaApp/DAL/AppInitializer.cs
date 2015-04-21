using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TyoaikaApp.Models;

namespace TyoaikaApp.DAL
{
    public class AppInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            var employees = new List<Employee>
            {
                new Employee{FirstName="Ville", LastName="Lindsberg", JobTitle="Esimies"},
                new Employee{FirstName="Matti", LastName="Virtanen", JobTitle="Työntekijä"},
                new Employee{FirstName="Antton", LastName="Lappalainen", JobTitle="Työntekijä"},
                new Employee{FirstName="Jesse", LastName="Koli", JobTitle="Työntekijä"}
            };
            
            employees.ForEach(e => context.Employees.Add(e));
            context.SaveChanges();

            var bulletins = new List<Bulletin>
            {
                new Bulletin{EmployeeID=1, Header="Ensimmäinen tiedote", Content="Heippa maailma ja sitä rataa.", Date=DateTime.Parse("21.04.2015 14:24")},
                new Bulletin{EmployeeID=1, Header="Toinen tiedote", Content="No jopas.", Date=DateTime.Parse("22.04.2015 09:11")}
            };

            bulletins.ForEach(b => context.Bulletins.Add(b));
            context.SaveChanges();
        }
    }
}