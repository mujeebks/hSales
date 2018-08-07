using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CustomerBM
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? LastPayedDate { get; set; }
        public decimal LastPayedAmount { get; set; }
        public DateTime EnteredDate { get; set; }
        public string SalesPerson { get; set; }
        public decimal LastSalesAmount { get; set; }
        public DateTime? LastSaleDate { get; set; }
        
    }
}
