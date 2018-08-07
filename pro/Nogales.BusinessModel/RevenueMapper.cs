using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueMapper
    {
        public string Category { get; set; }
        public string Comodity { get; set; }
        public string SalesPerson { get; set; }
        public double? CurrentValue { get; set; }
        public double? PreviousValue { get; set; }
        //public DateTime DateSold { get; set; }
        public string Customer { get; set; }
        public string Year { get; set; }
        public string SalesPersonCode { get; set; }
        public DateTime Date { get; set; }
    }
}
