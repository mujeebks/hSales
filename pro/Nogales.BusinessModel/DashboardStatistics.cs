using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class DashboardStatistics
    {
        public double Current { get; set; }
        public double Prior { get; set; }
        public string Name { get; set; }
        public double? Change { get; set; }
    }

    public class Statistics
    {
        public decimal CurrentValue { get; set; }
        public decimal PriorValue { get; set; }
    }
    public class CasesSoldSalesStatistics
    {
        public Statistics Sales { get; set; }
        public Statistics CasesSold { get; set; }
    }

    public class DashboardStatisticsData
    {
        public decimal CasesSold { get; set; }
        public decimal Sales { get; set; }
        public string Period { get; set; }
    }
}
