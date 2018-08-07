using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    class ExpensesBM
    {
    }
    public class ExpensesGlobalCategoryBM
    {
        public IEnumerable<ExpensesCategoryChartGlobalBM> Total { get; set; }
    }

    /// <summary>
    /// Accomodates the expenses of all the different Categories
    /// </summary>
    public class ExpensesCategoryBM
    {
        public IEnumerable<ExpensesCategoryChartBM> Total { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> Buyer { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> FoodService { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> Carniceria { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> National { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> Retail { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> Wholesaler { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> WillCall { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> LossProd { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> AllOthers { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> Oot { get; set; }
        public IEnumerable<ExpensesCategoryChartBM> SalesPerson { get; set; }
    }


    /// <summary>
    /// The Expense detail of a category
    /// </summary>
    public class ExpensesCategoryChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public List<ExpensesCategoryChartBM> SubData { get; set; }

        public int Label { get { return 0; } }
    }

    /// <summary>
    /// The Expense detail of a category
    /// </summary>
    public class ExpensesCategoryChartGlobalBM
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
        public List<ExpensesCategoryChartGlobalBM> SubData { get; set; }

        public int Label { get { return 0; } }
    }

    /// <summary>
    /// Class used to map the data from the data table generated from the Cube
    /// </summary>
    public class ExpensesDataMapper
    {

        public string Category { get; set; }
        public string SalesPerson { get; set; }
        public string Customer { get; set; }
        public double? SalesAmount { get; set; }
        public string Type { get; set; }
        public string Comodity { get; set; }
        public double? CurrentSold { get; set; }
        public double? PreviousSold { get; set; }
        public int Year { get; set; }

    }

    public class ExpensesDataMapperGlobal
    {

        public string Category { get; set; }
        public string SalesPerson { get; set; }
        public string Customer { get; set; }
        public double? SalesAmount { get; set; }
        public string Type { get; set; }
        public string Comodity { get; set; }
        public double? CurrentSold { get; set; }
        public double? HistoricalSold { get; set; }
        public double? PriorSold { get; set; }
        public int Year { get; set; }

    }

    public class ExpensesAmountChartBM
    {
        public string Description { get; set; }
        public int TotalExpense { get; set; }
    }

    public class ExpenseStatistics
    {
        public double Wages { get; set; }
        public double OtWages { get; set; }
        public double AdminExp { get; set; }
        public double WarehouseExp { get; set; }
        public double TransExp { get; set; }
    }

    public class MapAdminExpensesData
    {
        public string Description { get; set; }
        public decimal Expense { get; set; }
        public string Period { get; set; }
        public DateTime Date { get; set; }
    }

    public class ExpensesData
    {
        public decimal CurrentExpense { get; set; }
        public decimal PriorExpense { get; set; }
        public decimal CurrentRevenue { get; set; }
        public decimal PriorRevenue { get; set; }
        public string CurrentLabel { get; set; }
        public string PriorLabel { get; set; }
        public string Category { get; set; }
        public string Comodity { get; set; }
        public decimal Percentage { get; set; }
        public string Period { get; set; }
        public decimal Cost { get; set; }

    }

    public class MapEmployeePayrollData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
        public string EmployeeId { get; set; }
        public string Level { get; set; }
        public string Position { get; set; }
        public decimal Regular { get; set; }
        public decimal OverTime { get; set; }
        public decimal EarningsTotal { get; set; }
        public decimal DeductionTotal { get; set; }
        public decimal LiabilityTotal { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Period { get; set; }
    }
    public class OPEXCOGSExpenseJournalReport
    {
        public string InvoiceNumber { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExpenseCategory
    {
        public string Commodity { get; set; }
        public string Category { get; set; }
        public string AssignedSalesPersonCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string Period { get; set; }
        public decimal Cost { get; set; }
    }
}
