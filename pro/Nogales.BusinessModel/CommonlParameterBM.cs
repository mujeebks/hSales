using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CommonlParameterBM
    {
    }
    public class GlobalFilterModel : FilterModel
    {
        public int FilterId { get; set; }
        public int Period { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string OrderBy { get; set; }
        public bool IsCM01 { get; set; }
    }
    public class FilterModel
    {
        public string SalesPerson { get; set; }
        public string StartDateCurrent { get; set; }
        public string EndDateCurrent { get; set; }
        public string StartDatePrevious { get; set; }
        public string EndDatePrevious { get; set; }

        public string Category { get; set; }
        public string Commodity { get; set; }

    }


    public class BaseFilter
    {
        public int FilterId { get; set; }
        public int Period { get; set; }

    }

}
