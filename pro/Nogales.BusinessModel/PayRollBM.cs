using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
   public class PayRollBM
    {

        public int Id { get; set; }
        public string Employee { get; set; }
        public string IdNumber { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Supervisor { get; set; }
        public string Regular { get; set; }
        public string Overtime { get; set; }
        public string Orient { get; set; }

        public double RegularSortValue { get; set; }
        public double OvertimeSortValue { get; set; }
        public double OrientSortValue { get; set; }
        public double TotalSortValue { get; set; }

        public string Total { get; set; }
        public string PaymentDescription { get; set; }
        public double PaymentValue { get; set; }
        public double Rate { get; set; }

    }

    public class PayrollResult
    {
        public List<PayRollBM> PayRoll { get; set; }
        public string TotalRegular { get; set; }
        public string TotalOverTime { get; set; }
        public string TotalTotal { get; set; }
    }

    public class PayrollFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> Employees { get; set; }
    }
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
