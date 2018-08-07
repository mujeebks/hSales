using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueReportFilterBM
    {
        public List<KeyValuePair<string, string>> SalesPerson{ get; set; }
        public List<KeyValuePair<string, string>> Buyer { get; set; }
        public List<KeyValuePair<string, string>> Vendor { get; set; }
        public List<KeyValuePair<string, string>> Item { get; set; }
        public List<KeyValuePair<string, string>> PurchaseOrder { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        

    }

    public class RevenueReportModel
    {
        public string Comodity { get; set; }

        public string Category { get; set; }

        public string currentStartDate { get; set; }

        public string currentEndDate { get; set; }

        public string priorStartDate { get; set; }

        public string priorEndDate { get; set; }

        public string MinSalesAmt { get; set; }

        public string SalesPersonCode { get; set; }


    }
}
