using System;
using System.Collections.Generic;

namespace Nogales.BusinessModel
{
    public class APJournalChartBM
    {
        public List<APJournalChartBM> Data { get; set; }
        public string GLAccount { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Color { get; set; }
        public APJournalChart SubData { get; set; }
    }

    public class APJournalChart
    {
        public List<APJournalChartBM> Top { get; set; }
        public List<APJournalChartBM> Bottom { get; set; }
    }

    public class OPEXCOGSExpenseJournalChartBM
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public decimal? Val1 { get; set; }
        public decimal? Val2 { get; set; }
        public decimal? Val3 { get; set; }
        public string Category { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string AccountNumber { get; set; }
        public OPEXCOGSExpenseJournalTopBottomBM SubData { get; set; }
        public int Label { get { return 0; } }

        public DateTime Period1 { get; set; }
        public DateTime Period2 { get; set; }
        public DateTime Period3 { get; set; }
    }

    public class OPEXCOGSExpenseJournalTopBottomBM
    {
        public List<OPEXCOGSExpenseJournalChartBM> Top { get; set; }
        public List<OPEXCOGSExpenseJournalChartBM> Bottom { get; set; }
    }

    public class OPEXCOGSExpenseJournalBM
    {
        public List<OPEXCOGSExpenseJournalChartBM> OPEXCOGSExpenseJournalChart { get; set; }
        public List<OPEXCOGSExpenseJournalChartBM> PayrollJournalChart { get; set; }
    }

    public class APJournalBM
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string CurrPrev { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentCode { get; set; }
        public decimal Amount { get; set; }
    }

}
