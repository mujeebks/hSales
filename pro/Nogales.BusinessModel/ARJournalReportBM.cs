using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ARJournalReportBM
    {
        public string GLAccount { get; set; }
        public string AccountName { get; set; }

        public string TransactionNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public string ARType { get; set; }

        public string Customer{ get; set; }

        public string ReferenceNumber { get; set; }

        public string Session { get; set; }

        public string GLBatch { get; set; }

        public decimal Amount { get; set; }
    }
}
