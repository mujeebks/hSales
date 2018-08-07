using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ShortReportNotificationBM
    {
        public string routeNumber { get; set; }
        public string buyerId { get; set; }
        public string shipDate { get; set; }
        public List<string> EmailTos { get; set; }
        public List<int> Ids { get; set; }
        public string type { get; set; }

        public string Date { get; set; }
        public string IgnoredReason { get; set; }
    }

    public class ReportWithoutBinBo
    {
        public DateTime Date { get; set; }
        public string Sono { get; set; }
        public List<string> SalesPersons { get; set; }
    }

    public class ReportWithoutBINBM
    {
        public string SalesOrderNumber { get; set; }
        public string Item { get; set; }
        public int QuantityShipped { get; set; }
        public DateTime Date { get; set; }
        public string PullerId { get; set; }
        public string Puller { get; set; }
        public string SalesPerson { get; set; }
        public string SalesPersonName { get; set; }

    }

    public class DumpAndDonaReportBM
    {
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public string Customer { get; set; }
        public string CustomerDesc { get; set; }
        public int QuantityShipped { get; set; }
        public decimal Revenue { get; set; }

        public decimal ExtendedCost { get; set; }
        public string Type { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public string BuyerId { get; set; }
        public DateTime Date { get; set; }
        public int IsConsigned { get; set; }
    }
}
