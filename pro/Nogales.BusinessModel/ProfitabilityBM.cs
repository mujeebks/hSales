using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ProfitChartBM
    {
        public double? Amount { get; set; }
        public string Date { get; set; }
    }

    public class MarginChartBM
    {

        public string InvNo { get; set; }
        public string SalesPerson { get; set; }

        public string Company { get; set; }
        public string CreditCode { get; set; }
        public string CustCode { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public int QuantityShipped { get; set; }
        public double Revenue { get; set; }

        public double ExtCost { get; set; }
        public double Cost { get; set; }
        public double BinCost { get; set; }
        public string Period { get; set; }
        public DateTime Date { get; set; }

        public decimal Margin { get; set; }
        public double MarginPercentage { get; set; }
    }

    public class GenericMarginChartData
    {
        public List<GenericProfitablityChart> ProfitabiltyChart { get; set; }
        public GenericTwoBarChartData Customer { get; set; }
        public GenericTwoBarChartData Item { get; set; }
    }

    public class GenericTwoBarChartData
    {
        public GenericTopBottomTwoBarChartData TopBottomChartByMonth { get; set; }
        public GenericTopBottomTwoBarChartData TopBottomChartByYear { get; set; }
    }

    public class GenericProfitablityChart
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public decimal? Val1 { get; set; }
        public decimal? Val2 { get; set; }
        public decimal? Val3 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public GenericProfitablityTopBottomBM SubData { get; set; }
        public int Label { get { return 0; } }

        public string Id { get; set; }
        public string Key { get; set; }



    }
    public class GenericProfitablityTopBottomBM
    {
        public List<GenericProfitablityChart> Top { get; set; }
        public List<GenericProfitablityChart> Bottom { get; set; }
    }
}
