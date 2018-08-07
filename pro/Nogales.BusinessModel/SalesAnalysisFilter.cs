using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class SalesAnalysisFilter
    {
        public int? PurchaseOrderNumber { get; set; }
        public int? ItemNumber { get; set; }
        public string Buyer { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> item { get; set; }

        /// <summary>
        /// Possible values are 'EmployeeReport', 'CustomerReport', 'SalesOrderReport' and 'PurchaseOrderReport'
        /// </summary>
        public string ReportName { get; set; }
        public int LowerMarginLimit { get; set; }
        public int UpperMarginLimit { get; set; }

        public string FilterValue { get; set; }

        //public string EmployeeCode { get; set; }
        //public string CustomerNumber { get; set; }
        //public string SalesOrderNumber { get; set; }
        //public string PONumberString { get; set; }
    }
}
