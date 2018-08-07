using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
   public class ProfitabilityChartBM
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public decimal CurrentProfit { get; set; }
        public decimal PriorProfit { get; set; }
        public decimal GrowthProfit { get; set; }
        public decimal DifferenceProfit { get; set; }

    }


    public class ProfitByCustomer
    {
        public string Commodity { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string SalesManCode { get; set; }
        public string SalesManDescription { get; set; }
        public string SalesOrder { get; set; }
        public decimal Cost { get; set; }
        public decimal QtyShip { get; set; }
        public decimal ExtPrice { get; set; }
        public decimal Profit { get; set; }

    }

    public class ProfitByCustomerRequest
    {
        public int FilterId { get; set; }
        public string ItemCode { get; set; }
        public string Commodity { get; set; }
        public string CustomerCode { get; set; }
        public string Salesman { get; set; }
        public int Period { get; set; }
        public bool IsCM01 { get; set; }
    }
}
