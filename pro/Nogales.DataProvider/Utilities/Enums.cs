using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
   public class Enums
    {
        public enum PayrollStatus
        {
            Progress=0,
            Success=1,
            Failed=2
        }
        public enum PaymentDesciption
        {
            Regular = 0,
            Overtime = 1,
            Orient = 2
        }
    }
}
