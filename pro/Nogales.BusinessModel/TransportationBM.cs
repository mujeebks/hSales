using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class TransportationBM
    {
    }

    public class TransportationTripDashboardBarColumnChartBO : GenericBarColumnChart
    {
        public string DriverName { get; set; }
        public int NumberOfStops1 { get; set; }
        public decimal CasesDelivered1 { get; set; }
        public int NumberOfStops2 { get; set; }
        public decimal CasesDelivered2 { get; set; }
        public int NumberOfStops3 { get; set; }
        public decimal CasesDelivered3 { get; set; }

        public TransportationTripDashboardBarColumnChartSubDataBO SubData { get; set; }


    }

    public class TransportationTripDashboardBarColumnChartSubDataBO
    {
        public List<TransportationTripDashboardBarColumnChartBO> Top { get; set; }
        public List<TransportationTripDashboardBarColumnChartBO> Bottom { get; set; }
    }

    public class TransportationDashboardTripDTO
    {
        public string Period { get; set; }
        public string DriverCode { get; set; }
        public string DriverName { get; set; }
        public string RouteType { get; set; }
        public int NumberOfTrips { get; set; }
        public int NumberOfStops { get; set; }
        public decimal CasesDelivered { get; set; }
    }

    public class TransportaionDriverTripConsolidatedReportBO : TransportationDashboardTripDTO
    {
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateString { get; set; }

        public string Route { get; set; }  
    }

    public class TransportationRouteConsolidatedReportBO
    {
        public string Route { get; set; }
        public string RouteType { get; set; }
        public int NumberOfDrivers { get; set; }
        public int NumberOfStops { get; set; }
        public decimal CasesDelivered { get; set; }

    }

    public class TransportationDriverTripDetailedReportBO
    {
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateString { get; set; }
        public string DriverCode { get; set; }
        public string TruckCode { get; set; }
        public string DriverName { get; set; }
        public string Route { get; set; }
        public string RouteType { get; set; }
        public string RouteRun { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Reference { get; set; }
        public decimal CasesDelivered { get; set; }
        public int NumberOfInvoices { get; set; }
    }

    public class TransportaionDriverTripDayAndDetailedReportBO
    {
        public List<TransportaionDriverTripConsolidatedReportBO> DayReport { get; set; }
        public List<TransportationDriverTripDetailedReportBO> DetailedReport { get; set; }
        public List<TransportationDriverTripDetailedReportBO> CustomerReport { get; set; }
    }

    public class TransportationFilterBO : BaseFilter
    {
        public string DriverCode { get; set; }
    }


}
