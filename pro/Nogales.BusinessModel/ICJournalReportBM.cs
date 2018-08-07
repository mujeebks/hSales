using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ICJournalReportBM
    {
        public string GLAccount { get; set; }

        public decimal TotalAmount { get; set; }

        //Added by praveen
        public string AccountName { get; set; }
    }
}
