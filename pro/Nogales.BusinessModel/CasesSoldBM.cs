using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ClusteredBarChartCategoryBM
    {
        public CasesSoldComodityBM Total { get; set; }
        public CasesSoldComodityBM Buyer { get; set; }
        public CasesSoldComodityBM FoodService { get; set; }
        public CasesSoldComodityBM Carniceria { get; set; }
        public CasesSoldComodityBM National { get; set; }
        public CasesSoldComodityBM Retail { get; set; }
        public CasesSoldComodityBM Wholesaler { get; set; }
        public CasesSoldComodityBM WillCall { get; set; }
        public CasesSoldComodityBM LossProd { get; set; }
        public CasesSoldComodityBM AllOthers { get; set; }
        public CasesSoldComodityBM Oot { get; set; }

        public IEnumerable<CasesSoldSalesPersonChartBM> SalesPerson { get; set; }
    }

    public class ClusteredBarChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Code { get; set; }
        public string SalesPersonDescription { get; set; }
        public int Label { get { return 0; } }
        public List<ClusteredBarChartBM> SubData { get; set; }
    }


    public class CasesSoldComodityBM
    {
        //Grocery
        public IEnumerable<ClusteredBarChartBM> Grocery { get; set; }

        //Produce
        public IEnumerable<ClusteredBarChartBM> Produce { get; set; }

        //All
        public List<ClusteredStackChartBM> All { get; set; }
    }


    public class ClusteredStackChartBM
    {
        public string Year { get; set; }
        public string  Grocery { get; set; }
        public string  Produce { get; set; }
        public string Color1 { get; set; }
        public string  Color2 { get; set; }
    }

    public class CasesSoldReportFilterBM
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

    public class CasesSoldReportMapperBM
    {
        public string Customer { get; set; }

        public double PreviousCasesSold { get; set; }

        public double CurrentCasesSold { get; set; }

        public double Difference { get; set; }

        public double PercentageDifference { get; set; }

    }
}
