using System;

namespace Nogales.BusinessModel
{
    public class CasesSoldMapper
    {
        public string Category { get; set; }
        public string Comodity { get; set; }
        public string SalesPerson { get; set; }
        public double? CurrentSold { get; set; }
        public double? PreviousSold { get; set; }
        public DateTime DateSold { get; set; }
        public string Customer { get; set; }
        public string Year { get; set; }
        public DateTime Date { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
    }
}
