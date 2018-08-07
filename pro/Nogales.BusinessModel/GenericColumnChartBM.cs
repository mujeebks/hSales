using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{

    public class GenericBaseColumnChartBM
    {
        public string GroupName { get; set; }
        public string Label1 { get; set; }
        public int cValue1 { get; set; }
        public double rValue1 { get; set; }
        public string Color1 { get; set; }
        public string SalesPerson1 { get; set; }
        public DateTime Period1 { get; set; }
        public string Label2 { get; set; }
        public int cValue2 { get; set; }
        public double rValue2 { get; set; }
        public string Color2 { get; set; }
        public string SalesPerson2 { get; set; }
        public DateTime Period2 { get; set; }

        public string Label3 { get; set; }
        public string Label4 { get; set; }
        public string Label5 { get; set; }
        public string Label6 { get; set; }
        public int cValue3 { get; set; }
        public int cValue4 { get; set; }
        public int cValue5 { get; set; }
        public int cValue6 { get; set; }
        public double rValue3 { get; set; }
        public double rValue4 { get; set; }
        public double rValue5 { get; set; }
        public double rValue6 { get; set; }

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

    public class GenericColumnChartBM : GenericBaseColumnChartBM
    {
        public List<GenericColumnChartBM> SubData { get; set; }
    }

    public class GenericMapChartMB
    {
        public string id { get; set; }
        public string value { get; set; }
        public string CasesSoldValue { get; set; }
        public string customData { get; set; }

    }


    public class GenericTopBottomSubDataChartBM : GenericBaseColumnChartBM
    {
        public GenericSubDataTopBottomChartBM SubData { get; set; }
    }

    public class GenericSubDataTopBottomChartBM
    {
        public List<GenericColumnChartBM> Top { get; set; }
        public List<GenericColumnChartBM> Bottom { get; set; }
    }

    public class GenericColumnChartCategoryBM
    {
        //Grocery
        public IEnumerable<GenericColumnChartBM> Grocery { get; set; }

        //Produce
        public IEnumerable<GenericColumnChartBM> Produce { get; set; }

        //All
        public IEnumerable<GenericColumnChartBM> All { get; set; }
    }

    public class GenericColumnChartSalesDashBoardBM
    {
        public List<GenericColumnChartBM> Local { get; set; }
        public List<GenericColumnChartBM> All { get; set; }
        public List<GenericColumnChartBM> OutSales { get; set; }
        public GenericSubDataTopBottomChartBM Sales { get; set; }

        public GenericSubDataTopBottomChartBM CasesSold { get; set; }
        public GenericSubDataTopBottomChartBM Customer { get; set; }

        public List<GenericMapChartMB> MapData { get; set; }
        public List<GenericMapChartMB> CaseSoldMapData { get; set; }

        public List<GenericMapChartMB> RevenueMapData { get; set; }

    }

    public class GenericColumnChartSalesManBM
    {
        //Revenue
        public IEnumerable<GenericTopBottomSubDataChartBM> Revenue { get; set; }

        //Case Sold
        public IEnumerable<GenericTopBottomSubDataChartBM> CaseSold { get; set; }

    }

    public class GenericColumnChartLocationBM
    {
        public GenericColumnChartCategoryBM OOT { get; set; }
        public GenericColumnChartCategoryBM Local { get; set; }
    }

    public class GenericDashboardChartsBO
    {
        public GenericColumnChartCategoryBM TotalCasesSold { get; set; }
        public GenericColumnChartCategoryBM TotalRevenue { get; set; }
        public GenericColumnChartCategoryBM BuyerCasesold { get; set; }
        public GenericColumnChartCategoryBM BuyerRevenue { get; set; }
        public GenericColumnChartCategoryBM FoodServiceCasesSold { get; set; }
        public GenericColumnChartCategoryBM FoodServiceRevenue { get; set; }
        public GenericColumnChartCategoryBM CarniceriaCasesSold { get; set; }
        public GenericColumnChartCategoryBM CarniceriaRevenue { get; set; }
        public GenericColumnChartCategoryBM LossProdCasesSold { get; set; }
        public GenericColumnChartCategoryBM LossProdRevenue { get; set; }

        public GenericColumnChartCategoryBM NationalCasesSold { get; set; }
        public GenericColumnChartCategoryBM NationalRevenue { get; set; }

        public GenericColumnChartCategoryBM WholesalerCasesSold { get; set; }
        public GenericColumnChartCategoryBM WholesalerRevenue { get; set; }

        public GenericColumnChartCategoryBM WillCallCasesSold { get; set; }
        public GenericColumnChartCategoryBM WillCallRevenue { get; set; }

        public GenericColumnChartCategoryBM RetailCasesSold { get; set; }
        public GenericColumnChartCategoryBM RetailRevenue { get; set; }

        public GenericColumnChartCategoryBM AllOthersCasesSold { get; set; }
        public GenericColumnChartCategoryBM AllOthersRevenue { get; set; }
        public GenericColumnChartSalesManBM SalesManReport { get; set; }
        public GenericColumnChartCategoryBM OOTCasesSold { get; set; }
        public GenericColumnChartCategoryBM OOTRevenue { get; set; }

        public double TotalMonthlyDifference { get; set; }
        public double TotalYearlyDifference { get; set; }
        public double GroceryMonthlyDifference { get; set; }
        public double GroceryYearlyDifference { get; set; }
        public double ProduceMonthlyDifference { get; set; }
        public double ProduceYearlyDifference { get; set; }
        public double revenueTotalMonthlyDifference { get; set; }
        public double revenueTotalYearlyDifference { get; set; }
        public double revenueGroceryMonthlyDifference { get; set; }
        public double revenueGroceryYearlyDifference { get; set; }
        public double revenueProduceMonthlyDifference { get; set; }
        public double revenueProduceYearlyDifference { get; set; }
    }

    public class MapCategoryQuantityRevenue
    {
        public string Invoice { get; set; }
        public string Comodity { get; set; }
        public DateTime InvDate { get; set; }
        public int MonthPart { get; set; }

        public int YearPart { get; set; }
        public string CustNo { get; set; }

        public string Company { get; set; }

        public int Quantity { get; set; }

        public decimal Revenue { get; set; }

        public string Category { get; set; }
        public string SalesMan { get; set; }
        public string ActiveEmployee { get; set; }


        public string ActualSalesPersonCodes { get; set; }
    }

    public class MapCategoryQuantityRevenueGlobal
    {
        public string Invoice { get; set; }
        public string Comodity { get; set; }
        public DateTime InvDate { get; set; }
        public string CustNo { get; set; }
        public string Company { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
        public string Category { get; set; }
        public string SalesMan { get; set; }
        public string ActiveEmployee { get; set; }
        public string ActualSalesPersonCodes { get; set; }
        public string Period { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("Month-{MonthPart},CCY-{CasesCurrentYear},CPY-{CasesPreviousYear},Slp-{SalesPersonCode}")]
    public class MapCasesSoldRevenue
    {
        public string Comodity { get; set; }
        public string SalesPersonCode { get; set; }
        public string Category { get; set; }
        public decimal RevenueCurrentYear { get; set; }
        public decimal RevenuePreviousYear { get; set; }

        public int CasesCurrentYear { get; set; }
        public int CasesPreviousYear { get; set; }
        public string CurrentYearEmployee { get; set; }
        public string PreviousYearEmployee { get; set; }
        public int WeekNo { get; set; }
        public int MonthPart { get; set; }

        public string ActiveEmployee { get; set; }
        public int YearPart { get; set; }
        public string Customers { get; set; }
        public int CasesCurrentMonth { get; set; }
        public int CasesPreviousMonth { get; set; }

        public string Location { get; set; }
        public string CurrentActualSalesPersonCodes { get; set; }
        public string PreviousActualSalesPersonCodes { get; set; }


    }

    public class MapCasesSoldRevenueGlobal
    {
        public int WeekNo { get; set; }
        public string Comodity { get; set; }
        public string SalesPersonCode { get; set; }
        public string Category { get; set; }
        public decimal RevenueCurrent { get; set; }

        public string Period { get; set; }

        public decimal RevenueHistorical { get; set; }
        public decimal RevenuePrior { get; set; }

        public int CasesCurrent { get; set; }
        public int CasesPrior { get; set; }
        public int CasesHistorical { get; set; }
        public string CurrentEmployee { get; set; }
        public string HistoricalEmployee { get; set; }
        public string PriorEmployee { get; set; }
        public string ActiveEmployee { get; set; }

        public string Customers { get; set; }
        public string State { get; set; }


    }


    public class GenericSalesPersonBO
    {
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
    }

    public class GenericReoprtMapperWithTopBottom
    {
        public List<GenericReportMapperBM> ReportData { get; set; }
        public GenericTopBottomTwoBarChartData ChartData { get; set; }
    }

    public class GenericReportMapperBM
    {
        public string Customer { get; set; }

        public double Previous { get; set; }

        public double Current { get; set; }

        public double Difference { get; set; }

        public double PercentageDifference { get; set; }

    }
    public class GenericTopBottomTwoBarChartData
    {
        public List<GenericTwoBarChartdata> Top { get; set; }
        public List<GenericTwoBarChartdata> Bottom { get; set; }
    }
    public class GenericTwoBarChartdata
    {
        public string Category { get; set; }
        public string Label { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public double Tooltip { get; set; }
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

    public class ItemReportByItemMapperBO
    {
        public string Item { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }

        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustNumber { get; set; }
        public string CustName { get; set; }
        public string BinNo { get; set; }
        public decimal QtyShipped { get; set; }
        public string UnitOfMeasure { get; set; }


    }

    public class ItemReportMapperBO
    {
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }

    }

    public class GenericItemMapperBM
    {
        public List<ItemReportMapperBO> ReportData { get; set; }
        public List<KeyValuePair<string, string>> ListItem { get; set; }


    }
    public class ItemMapper
    {
        public List<KeyValuePair<string, string>> ListItem { get; set; }
    }

    public class ItemComparisonReport
    {
        public string Item { get; set; }
        public string ItemName { get; set; }
        public string Period { get; set; }
        public string Comodity { get; set; }
        public decimal Revenue { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Margin { get; set; }
    }

    public class ItemVendorComparisonReport
    {
        public string Item { get; set; }
        public string ItemName { get; set; }
        public string Period { get; set; }
        public string Comodity { get; set; }
        public decimal Revenue { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public decimal Margin { get; set; }
    }

    public class ItemComparisonReportBO
    {
        public string Comodity { get; set; }

        public string Item { get; set; }
        public string ItemName { get; set; }

        public double rCurrent { get; set; }
        public double rHistorical { get; set; }
        public double rPrior { get; set; }
        //public double rYearHistorical { get; set; }
        //public double rYearCurrent { get; set; }

        //public int cCurrent { get; set; }
        //public int cHistorical { get; set; }
        //public int cPrior { get; set; }
        //public int cYearHistorical { get; set; }
        //public int cYearCurrent { get; set; }

        public double costCurrent { get; set; }
        public double costHistorical { get; set; }
        public double costPrior { get; set; }


    }

    public class ItemVendorComparisonReportBO
    {
        public string Comodity { get; set; }

        public string Item { get; set; }
        public string ItemName { get; set; }

        public string Vendor { get; set; }
        public string VendorName { get; set; }

        public int cCurrent { get; set; }
        public int cHistorical { get; set; }
        public int cPrior { get; set; }

        public double costCurrent { get; set; }
        public double costHistorical { get; set; }
        public double costPrior { get; set; }

        public double CostDifference { get; set; }

        public double Margin { get; set; }
    }


    public class BarChartDetails
    {
        public string Color1 { get; set; }
        public string salesman { get; set; }
        public string Code { get; set; }
        public string GroupName { get; set; }
        public string GroupName1 { get; set; }
        public decimal value { get; set; }
        public decimal prior { get; set; }
        public decimal growth { get; set; }
        public string Label1 { get; set; }
        public string LabelPrior { get; set; }
        public string Category { get; set; }
        public string AddUser { get; set; }
    }
    public class BarchartOrderBy
    {
        public List<BarChartDetails> Top { get; set; }
        public List<BarChartDetails> Bottom { get; set; }
    }
    public class BarChartTypes
    {
        public BarchartOrderBy SalesPerson { get; set; }
        public BarchartOrderBy Growth { get; set; }

    }

    public class GenericBarColumnChart
    {
        public string Category { get; set; }
        public int Label { get; set; }

        public string Column1 { get; set; }
        public decimal? Val1 { get; set; }
        public string Color1 { get; set; }
        public DateTime Period1 { get; set; }
       

        public string Column2 { get; set; }
        public decimal? Val2 { get; set; }
        public string Color2 { get; set; }
        public DateTime Period2 { get; set; }
      


        public string Column3 { get; set; }
        public decimal? Val3 { get; set; }
        public string Color3 { get; set; }
        public DateTime Period3 { get; set; }
    }

    public class GenericBarColumnTopBottomChart
    {
        public List<GenericBarColumnChart> Top { get; set; }
        public List<GenericBarColumnChart> Bottom { get; set; }
    }
}

