using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    public class Constants
    {
        public const string OpexAccountStart = "80000";
        public const string OpexAccountEnd = "89999";

        public const string CogsAccountStart = "50000";
        public const string CogsAccountEnd = "69999";

        public const string OpexGLAccounTtype = "0";
        public const string COGSGLAccounTtype = "1";
        public const string ALLGLAccounTtype = "2";

        public const string APJournalType = "AP";
        public const string ARJournalType = "AR";
        public const string ICJournalType = "IC";

        public const string StartSession = "000000";
        public const string EndSession = "999999";

        public const string CaseSoldByMonth = "CategoryByMonth";
        public const string CaseSoldByYear = "CategoryByYear";
        public const string CaseSoldByYearToMonth = "CategoryByYearToMonth";

        public const string RevenueByMonth = "CategoryByMonth";
        public const string RevenueByYear = "CategoryByYear";
        public const string RevenueByYearToMonth = "CategoryByYearToMonth";


        public const string COGSOPEXJournalByMonth = "CategoryByMonth";
        public const string COGSOPEXJournalByYear = "CategoryByYear";
        public const string COGSOPEXJournalByYearToMonth = "CategoryByYearToMonth";


        public const string ByMonth = "CategoryByMonth";
        public const string ByYear = "CategoryByYear";
        public const string ByYearToMonth = "CategoryByYearToMonth";

        public const string CategorySchools = "SCHOOLS";
        public const string CategoryHealthcare = "HEALTHCARE";
        public const string CategoryInstitute = "INSTITUTE";
        // public const IReadOnlyCollection<string> osSalesPersons=new const IReadOnlyCollection<string> { "DN01", "DS01", "FW01", "MC01", "MN01", "NE01", "SE01" };
        public static readonly IList<String> OutSalesPersons = new ReadOnlyCollection<string>
        (new List<String> { "DN01", "DS01", "FW01", "MC01", "MN01", "NE01", "SE01" });
    }
}
