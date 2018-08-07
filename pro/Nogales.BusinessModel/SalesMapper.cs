using System;
using System.Collections.Generic;

namespace Nogales.BusinessModel
{
    //public class SalesMapper
    //{
    //    public List<KeyValuePair<string, string>> ListSalesPerson { get; set; }
    //    public string SalesPerson { get; set; }
    //    public string Customer { get; set; }
    //    public double? SalesAmount { get; set; }
    //    public string Type { get; set; }
    //    public int NoOfCustomer { get; set; }
    //}
    public class SalesMapperReportWithTopBottom
    {
        public List<SalesMapper> ReportData { get; set; }
        public GenericTopBottomTwoBarChartData ChartData { get; set; }
    }

   
    public class SalesMapper
    {
        public List<KeyValuePair<string, string>> ListSalesPerson { get; set; }
        public string SalesPerson { get; set; }
        public string AssignedPersonCode { get; set; }
        public string Customer { get; set; }
        public string  CustomerNumber { get; set; }
        public double? SalesAmount { get; set; }
        public string Type { get; set; }
        public int NoOfCustomer { get; set; }
        public decimal SalesQty { get; set; }
        public decimal SalesAmountCurrent { get; set; }
        public decimal SalesAmountPrior { get; set; }
        public decimal Percentage { get; set; }
        public string SalesPersonDescription { get; set; }
        public decimal Difference { get; set; }
        public string Binno { get; set; }
        public string Picker { get; set; }
        public string Commodity { get; set; }
        public decimal CasesSoldPrior { get; set; }
        public decimal CasesSoldCurrent { get; set; }
        public decimal DifferenceCasesSold { get; set; }
        public decimal PercentageCasesSold { get; set; }

        public decimal DifferenceExpense { get; set; }
        public decimal PercentageExpense { get; set; }
        public decimal ExpenseCurrent { get; set; }
        public decimal ExpensePrior { get; set; }

    }
    public class CasesSoldDetailsMapper
    {
        public string Comodity { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public  decimal Quantity { get; set; }
        public decimal ExtPrice { get; set; }
        public string SalesMan { get; set; }
        public string Sono { get; set; }
        public string Transeq { get; set; }
        public string Category { get; set; }
        public decimal Expense { get; set; }

    }

    public class SalesAnalysisMapper
    {
        public string SalesPerson { get; set; }
        public int NoOfCustomer { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCasesSold { get; set; }
        public decimal GroceryRevenue { get; set; }
        public decimal GroceryCasesSold { get; set; }

        public decimal ProduceRevenue { get; set; }
        public decimal ProduceCasesSold { get; set; }
        public decimal ManualRevenue { get; set; }
        public decimal ManualCasesSold { get; set; }


    }
}

