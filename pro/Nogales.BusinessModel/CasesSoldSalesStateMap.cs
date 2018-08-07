using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CasesSoldSalesStateMap
    {
        public List<MapData> CasesSold { get; set; }
        public List<MapData> Sales { get; set; }
    }

    public class CasesSoldAndSalesStateData
    {
        public string State { get; set; }
        public decimal CasesSoldCurrent { get; set; }
        public decimal CasesSoldPrior { get; set; }
        public decimal CasesSoldHistorical { get; set; }
        public decimal SalesCurrent { get; set; }
        public decimal SalesPrior { get; set; }
        public decimal SalesHistorical { get; set; }
        public string Commodity { get; set; }
        public string Category { get; set; }
    }
    public class MapData
    {
        public string id { get; set; }
        public decimal value { get; set; }
        public string customData { get; set; }
    }
}
