using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class DashboardChartBM
    {
        public List<RevenueCategoryChartBM> RevenueTotals { get; set; }
        public List<CasesSoldCategoryChartBM> CasesSoldTotals { get; set; }
        public SalesChartBM SalesTotals { get; set; }
        public List<ExpensesCategoryChartBM> ExpensesTotals { get; set; }
        public List<DashboardStatisticsBM> Statistics { get; set; }
    }
    public class DashboardStatisticsBM
    {
        public string Name { get; set; }
        public double? Amount { get; set; }
        public double? PreviousMonthAmount { get; set; }
        public double? Change { get; set; }
        public string Month { get; set; }
        public string PreviousMonth { get; set; }
    }
}
