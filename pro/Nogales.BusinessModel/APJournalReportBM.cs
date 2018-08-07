using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class APJournalReportBM
    {
        public string GLAccount { get; set; }
        public string AccountName { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Vendor { get; set; }

        public string InvoiceNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string Session { get; set; }

        public string GLBatch { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

    }
}
