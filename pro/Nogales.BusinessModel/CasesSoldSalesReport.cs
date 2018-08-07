using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CasesSoldSalesReport
    {
        public string Customer { get; set; }

        public decimal Previous { get; set; }

        public decimal Current { get; set; }

        public decimal Difference { get; set; }

        public decimal PercentageDifference { get; set; }
    }

    public class CasesSoldSalesReoprtMapperWithTopBottom
    {
        public List<CasesSoldSalesReport> ReportData { get; set; }
        public CasesSoldSalesTopBottomTwoBarChartData ChartData { get; set; }
    }

    public class CasesSoldSalesReportMapperBM
    {
        public string Customer { get; set; }

        public double Previous { get; set; }

        public double Current { get; set; }

        public double Difference { get; set; }

        public double PercentageDifference { get; set; }

    }
    public class CasesSoldSalesTopBottomTwoBarChartData
    {
        public List<CasesSoldSalesTwoBarChartdata> Top { get; set; }
        public List<CasesSoldSalesTwoBarChartdata> Bottom { get; set; }
    }
    public class CasesSoldSalesTwoBarChartdata
    {
        public string Category { get; set; }
        public string Label { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public decimal? Tooltip { get; set; }
        public double Tooltip2 { get; set; }
        public string Period { get; set; }
        public string Period2 { get; set; }

        public string ColumnMargin
        {
            get
            {
                return "0";
            }

        }


    }
}
