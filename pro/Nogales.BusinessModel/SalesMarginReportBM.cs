using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class SalesMarginReportBM
    {
        public string Employee { get; set; }
        public int InvoicesNumber { get; set; }
        public int Lines { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal? MarginPercentage { get; set; }
        public string Customer { get; set; }
        public decimal Weight { get; set; }
        public string SalesOrder { get; set; }
        public string PurchaseOrder { get; set; }
        public string Buyer { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal QuantityReceived { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string CustomerNumber { get; set; }
    }
}
