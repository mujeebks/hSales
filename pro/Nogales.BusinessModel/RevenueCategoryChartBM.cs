using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class RevenueCategoryChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public RenvenueSalesPesonBM SubData { get; set; }

        public int Label { get { return 0; } }
    }
}
