using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueReportBM
    {
        public string SalesPerson { get; set; }
        public string Buyer { get; set; }
        public string Vendor { get; set; }
        public string PONumber { get; set; }
        public bool IsConsignedPO { get; set; }
        public string SONumber { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal RevenueMargin { get; set; }
        public string Customer { get; set; }
        public string Class { get; set; }
        public string Item { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal Quantity { get; set; }
    }
}
