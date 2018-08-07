using System.Collections.Generic;

namespace Nogales.BusinessModel
{
    public class SalesChartBM
    {
        public List<SalesSubDataBM> Top { get; set; }
        public List<SalesSubDataBM> Bottom { get; set; }
    }
   
    public class SalesSubDataBM
    {
        public string ColumnName { get; set; }
        public double? ColumnValue { get; set; }
        public int? ColumnPoint { get; set; }
        public double? ColumnValueTarget { get; set; }
        public int? ColumnPointTarget { get; set; }
        public string Color { get; set; }
        public string ColumnValueToolTip { get; set; }

        public string ColumnValueTargetToolTip { get; set; }

        public SalesChartBM SubData { get; set; }
    }

    // Sales by slaes person
    public class SalesCustomerBySalesPersonChartBM
    {
        public List<SalesCustomerBySalesPersonSubDataBM> Top { get; set; }
        public List<SalesCustomerBySalesPersonSubDataBM> Bottom { get; set; }
    }

    public class SalesCustomerBySalesPersonSubDataBM
    {
        public string ColumnName { get; set; }
        public double? ColumnValue { get; set; }
        public int? ColumnPoint { get; set; }
        public double? ColumnValueTarget { get; set; }
        public double? ColumnPointTarget { get; set; } // property data type change
        public string Color { get; set; }

        public SalesCustomerBySalesPersonChartBM SubData { get; set; }
    }

    //// Customer by sales person
    //public class CustomerBySalesPersonChartBM
    //{
    //    public List<CustomerBySalesPersonSubDataBM> Top { get; set; }
    //    public List<CustomerBySalesPersonSubDataBM> Bottom { get; set; }
    //}

    //public class CustomerBySalesPersonSubDataBM
    //{
    //    public string ColumnName { get; set; }
    //    public double? ColumnValue { get; set; }
    //    public int? ColumnPoint { get; set; }
    //    public double? ColumnValueTarget { get; set; }
    //    public double? ColumnPointTarget { get; set; } // property data type change
    //    public string Color { get; set; }

    //    public CustomerBySalesPersonChartBM SubData { get; set; }
    //}
   
}
