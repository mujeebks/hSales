using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class SalesAnalysisBM
    {
        public string BinNumber { get; set; }
        public string PurchaseOrder { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Buyer { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal ExtendedPrice { get; set; }
        public string SalesPerson { get; set; }
        public int NumberOfInvoices { get; set; }
        public int Lines { get; set; }
        public decimal Margin { get; set; }

        /// <summary>
        /// (Margin/ ExtendedPrice) * 100
        /// </summary>
        public decimal? MarginPercentage {
            get {
                if (this.ExtendedPrice == 0)
                    return 0;
                return (this.Margin / this.ExtendedPrice) * 100;
            }
        }
    }
}
