using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class ImportMasterBM
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string API { get; set; }
        public string InvokedBy { get; set; }
        public int RecordCount { get; set; }
        public DateTime Commence { get; set; }
        public DateTime? Finish { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
    }
}
