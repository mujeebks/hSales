using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
   public class CustomerMapper
    {
        public string Commodity { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string SalesmanCode { get; set; }
        public string SalesmanDescription { get; set; }
        public string SalesOrderNumber { get; set; }
        public decimal? Sales { get; set; }
        public decimal? CasesSold { get; set; }

    }
}
