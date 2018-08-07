using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ItemReportFilterBM
    {
        public string currentStartDate { get; set; }

        public string currentEndDate { get; set; }

        public List<string> item { get; set; }
    }
    public class ItemReportByItemFilterBM
    {
        public string currentStartDate { get; set; }

        public string currentEndDate { get; set; }

        public string Item { get; set; }

        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }


    }
    public class CostcomparisionFilterBM
    {
        public string filterId { get; set; }

        public string Comodity { get; set; }

        public List<string> item { get; set; }
    }
}
