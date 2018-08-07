using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueCategroryBM
    {
        public IEnumerable<RevenueCategoryChartBM> Total { get; set; }
        public IEnumerable<RevenueCategoryChartBM> Buyer { get; set; }
        public IEnumerable<RevenueCategoryChartBM> FoodService { get; set; }
        public IEnumerable<RevenueCategoryChartBM> Carniceria { get; set; }
        public IEnumerable<RevenueCategoryChartBM> National { get; set; }
        public IEnumerable<RevenueCategoryChartBM> Retail { get; set; }
        public IEnumerable<RevenueCategoryChartBM> Wholesaler { get; set; }
        public IEnumerable<RevenueCategoryChartBM> WillCall { get; set; }
        public IEnumerable<RevenueCategoryChartBM> LossProd { get; set; }
        public IEnumerable<RevenueCategoryChartBM> AllOthers { get; set; }
        public IEnumerable<RevenueCategoryChartBM> Oot { get; set; }

        public IEnumerable<RevenueCategoryChartBM> SalesPerson { get; set; }
    }
}
