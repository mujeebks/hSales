using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class SalesPersonMappingBM
    {
        public Int32 Id { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
        public List<AssignedPersonBM> AssignedPersonList { get; set; }
        public string Category { get; set; }

    }

    public class AssignedPersonBM
    {
        public string AssignedPersonCode { get; set; }
        public string AssignedDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ArchivedSalesPersonMappingBM
    {
        public Int32 Id { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
        public string AssignedPersonCode { get; set; }
        public string AssignedDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SalesPersonCodes
    {
        public string SalesPersonCode { get; set; }
    }

    public struct ArchivedSalesPersonRequestBM
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> SalesPersonCode { get; set; }
    }

    public class SalesPersonsDetails
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int IsAssigned { get; set; }

    }

    public class SalesPersonMappingFilterBO
    {
        public List<SalesPersonsDetails> MasterSalesPersons { get; set; }
        public List<SalesPersonsDetails> SalesPersons { get; set; }
        public List<string> TrackingGroups { get; set; }
    }

    public class AssignSalesPersonsBO
    {
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
        public string AssignedPersonCode { get; set; }
        public string AssignedDescription { get; set; }
        public string StartDate { get; set; }
        public string TrackingGroup { get; set; }
    }

    public class SalesPersonsBO
    {
        public Int32 Id { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
        public string AssignedPersonCode { get; set; }
        public string AssignedPersonDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Category { get; set; }
    }
}
