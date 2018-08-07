
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
  public  class GenericDrillDownChartsChartsBO
    {
        public GenericDrillDownColumnChartCategoryBM TotalCasesSold { get; set; }
        public GenericDrillDownColumnChartCategoryBM TotalRevenue { get; set; }


        //public GenericDrillDownColumnChartCategoryBM BuyerCasesold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM BuyerRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM FoodServiceCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM FoodServiceRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM CarniceriaCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM CarniceriaRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM LossProdCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM LossProdRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM NationalCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM NationalRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM WholesalerCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM WholesalerRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM WillCallCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM WillCallRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM RetailCasesSold { get; set; }
        //public GenericDrillDownColumnChartCategoryBM RetailRevenue { get; set; }
        //public GenericDrillDownColumnChartCategoryBM AllOthersCasesSold { get; set; }
        //public GenericColumnChartCategoryBM AllOthersRevenue { get; set; }
        //public GenericColumnChartSalesManBM SalesManReport { get; set; }
        //public GenericColumnChartCategoryBM OOTCasesSold { get; set; }
        //public GenericColumnChartCategoryBM OOTRevenue { get; set; }
        //public double GroceryMonthlyDifference { get; set; }
        //public double GroceryYearlyDifference { get; set; }
        //public double ProduceMonthlyDifference { get; set; }
        //public double revenueGroceryMonthlyDifference { get; set; }
        //public double revenueGroceryYearlyDifference { get; set; }
        //public double revenueProduceMonthlyDifference { get; set; }
        //public double revenueProduceYearlyDifference { get; set; }
        //public double ProduceYearlyDifference { get; set; }

        public double TotalMonthlyDifference { get; set; }
        public double TotalYearlyDifference { get; set; }
        public double revenueTotalMonthlyDifference { get; set; }
        public double revenueTotalYearlyDifference { get; set; }

    }

    public class GenericDrillDownColumnChartBM : GenericDrillDownBaseColumnChartBM
    {
        public List<GenericDrillDownColumnChartBM> SubData { get; set; }
    }

   
}

public class GenericDrillDownBaseColumnChartBM
{
    public string GroupName { get; set; }
    public string Label1 { get; set; }
    public decimal cValue1 { get; set; }
    public decimal rValue1 { get; set; }
    public string Color1 { get; set; }
    public string SalesPerson1 { get; set; }
    public DateTime Period1 { get; set; }
    public string Label2 { get; set; }
    public decimal cValue2 { get; set; }
    public decimal rValue2 { get; set; }
    public string Color2 { get; set; }
    public string SalesPerson2 { get; set; }
    public DateTime Period2 { get; set; }

    public string Label3 { get; set; }
    public string Label4 { get; set; }
    public string Label5 { get; set; }
    public string Label6 { get; set; }
    public decimal cValue3 { get; set; }
    public decimal cValue4 { get; set; }
    public decimal cValue5 { get; set; }
    public decimal cValue6 { get; set; }
    public decimal rValue3 { get; set; }
    public decimal rValue4 { get; set; }
    public decimal rValue5 { get; set; }
    public decimal rValue6 { get; set; }

    public string Color3 { get; set; }
    public string Color4 { get; set; }
    public string Color5 { get; set; }
    public string Color6 { get; set; }

    public DateTime Period3 { get; set; }
    public DateTime Period4 { get; set; }
    public DateTime Period5 { get; set; }
    public DateTime Period6 { get; set; }

    public string SalesPerson4 { get; set; }
    public string SalesPerson3 { get; set; }
    public string SalesPerson5 { get; set; }
    public string SalesPerson6 { get; set; }

    public string ActiveSalesPersonCode { get; set; }




    public string ColumnBottomMargin
    {
        get
        {
            return "0";
        }

    }
}

public class GenericDrillDownColumnChartCategoryBM
{
    //Grocery
    public IEnumerable<GenericDrillDownBaseColumnChartBM> Grocery { get; set; }

    //Produce
    public IEnumerable<GenericDrillDownBaseColumnChartBM> Produce { get; set; }

    //All
    public IEnumerable<GenericDrillDownBaseColumnChartBM> All { get; set; }
}
