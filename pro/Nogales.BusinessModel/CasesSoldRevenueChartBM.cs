﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CasesSoldRevenueChartBM
    {
        public string Commodity { get; set; }
        public string Category { get; set; }
        public string AssignedSalesPersonCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonDescription { get; set; }
        public decimal CasesSold { get; set; }
        public decimal Sales { get; set; }
        public string Period { get; set; }
        public string AddUser { get; set; }
        public decimal Cost { get; set; }
    }
}
