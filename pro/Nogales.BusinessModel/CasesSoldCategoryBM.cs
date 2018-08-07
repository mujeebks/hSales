using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CasesSoldCategoryBM
    {
        public IEnumerable<CasesSoldCategoryChartBM> Total { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> Buyer { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> FoodService { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> Carniceria { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> National { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> Retail { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> Wholesaler { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> WillCall { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> LossProd { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> AllOthers { get; set; }
        public IEnumerable<CasesSoldCategoryChartBM> Oot { get; set; }

        public IEnumerable<CasesSoldCategoryChartBM> SalesPerson { get; set; }
    }

    public class CaseSoldDashboardCategoryBM
    {
        //public IEnumerable<CaseSoldDashboardCategoryChartBM> Total { get; set; }
    }
}
