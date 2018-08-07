using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    class FinacnceBM
    {
    }

    public class FinanceCollectionDashboardDTO
    {
        public string Period { get; set; }
        public string Collector { get; set; }
        public decimal PNet { get; set; }
        public string PTerms { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int PaymentOnTimePercentage { get; set; }
        public decimal TotalCollectionPercentage { get; set; }
        public int? Ordinance { get; set; }
    }

    public class FinanceCollectionDashboardBarColumnChartBO : GenericBarColumnChart
    {
        public decimal InvoiceAmount1 { get; set; }
        public double PaymentOnTimePercentage1 { get; set; }
        public decimal PaymentCollectionPercentage1 { get; set; }


        public decimal InvoiceAmount2 { get; set; }
        public double PaymentOnTimePercentage2 { get; set; }
        public decimal PaymentCollectionPercentage2 { get; set; }


        public decimal InvoiceAmount3 { get; set; }
        public double PaymentOnTimePercentage3 { get; set; }
        public decimal PaymentCollectionPercentage3 { get; set; }

        public List<FinanceCollectionDashboardBarColumnChartTermsSubDataBO> SubData { get; set; }
    }

    public class FinanceCollectionDashboardBarColumnChartTermsSubDataBO : GenericBarColumnChart
    {
        public decimal InvoiceAmount1 { get; set; }
        public double PaymentOnTimePercentage1 { get; set; }
        public decimal PaymentCollectionPercentage1 { get; set; }


        public decimal InvoiceAmount2 { get; set; }
        public double PaymentOnTimePercentage2 { get; set; }
        public decimal PaymentCollectionPercentage2 { get; set; }


        public decimal InvoiceAmount3 { get; set; }
        public double PaymentOnTimePercentage3 { get; set; }
        public decimal PaymentCollectionPercentage3 { get; set; }

        public FinanceCollectionDashboardBarColumnChartCollectorSubDataBO SubData { get; set; }
    }

    public class FinanceCollectionDashboardBarColumnChartCollectorSubDataBO
    {
        public List<FinanceCollectionDashboardBarColumnChartBO> Top { get; set; }
        public List<FinanceCollectionDashboardBarColumnChartBO> Bottom { get; set; }
    }

    public class FinanceCollectorReportDTO
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string SalesManCode { get; set; }
        public string SalesManName { get; set; }
        public decimal PNet { get; set; }
        public string PTerms { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DatePaid { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal AmountCollected { get; set; }
        public decimal BalanceAmount { get; set; }
        public int DaysOverDue { get; set; }
        public int PaymentOnTimePercentage { get; set; }
        public decimal CollectionOnTimePercentage { get; set; }
        public int Collector { get; set; }
        public string CollectorName { get; set; }
    }

    public class FinanceFilterBO : BaseFilter
    {
        public string PTerms { get; set; }

        public string Collector { get; set; }
    }
}
