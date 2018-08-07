using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CasesSoldCategoryChartBM    
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public List<CasesSoldCategoryChartBM> SubData { get; set; }

        public int Label { get { return 0; } }
    }

    public class CasesSoldSalesPersonChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public CasesSoldSalesPersonBM SubData { get; set; }

        public int Label { get { return 0; } }
    }

    public class CasesSoldSalesPersonBM
    {
        public List<CasesSoldSalesPersonChartBM> Top { get; set; }
        public List<CasesSoldSalesPersonChartBM> Bottom { get; set; }
    }

    //public class CaseSoldDashboardCategoryChartBM
    //{
    //    public List<CasesSoldCategoryBM> Grocery { get; set; }
    //    public List<CasesSoldCategoryBM> Produce { get; set; }
    //}

    //public class CasesSoldCategoryBM
    //{
    //    public string Column1 { get; set; }
    //    public string Column2 { get; set; }
    //    public double? Val1 { get; set; }
    //    public double? Val2 { get; set; }
    //    public string Column3 { get; set; }
    //    public string Column4 { get; set; }
    //    public double? Val3 { get; set; }
    //    public double? Val4 { get; set; }
    //    public string Category { get; set; }
    //    public string Color1 { get; set; }
    //    public string Color2 { get; set; }
    //    public List<CasesSoldCategoryBM> SubData { get; set; }

    //    public int Label { get { return 0; } }
    //}
}
