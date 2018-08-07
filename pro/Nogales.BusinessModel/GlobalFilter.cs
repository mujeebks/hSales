using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class GlobalFilter
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Period Periods { get; set; }
      
    }

    public class Period
    {
        public DateRange Current { get; set; }
        public DateRange Prior { get; set; }
        public DateRange Historical { get; set; }
    }

    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Label { get; set; }
    }


}
