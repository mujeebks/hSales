using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueClusteredBarChartCategoryBM
    {
        public RevenueComodityBM Total { get; set; }
        public RevenueComodityBM Buyer { get; set; }
        public RevenueComodityBM FoodService { get; set; }
        public RevenueComodityBM Carniceria { get; set; }
        public RevenueComodityBM National { get; set; }
        public RevenueComodityBM Retail { get; set; }
        public RevenueComodityBM Wholesaler { get; set; }
        public RevenueComodityBM WillCall { get; set; }
        public RevenueComodityBM LossProd { get; set; }
        public RevenueComodityBM AllOthers { get; set; }
        public RevenueComodityBM Oot { get; set; }

        public List<RevenueCategoryChartBM> SalesPerson { get; set; }
    }

    public class RevenueClusteredBarChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Code { get; set; }
        public int Label { get { return 0; } }
        public List<RevenueClusteredBarChartBM> SubData { get; set; }
    }

    public class RevenueComodityBM
    {
        //Grocery
        public IEnumerable<RevenueClusteredBarChartBM> Grocery { get; set; }

        //Produce
        public IEnumerable<RevenueClusteredBarChartBM> Produce { get; set; }

        //All
        public List<ClusteredStackChartBM> All { get; set; }
    }
}
