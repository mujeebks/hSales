using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    class WarehouseBM
    {
    }

    public class WarehouseDashboardPickerProductivityDTO
    {
        public string Period { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int MinutesWorked { get; set; }
        public decimal QtyPicked { get; set; }
    }

    public class WarehousePickerProductivityDashboardBarColumnChartBO : GenericBarColumnChart
    {
        public string PickerId { get; set; }

        public int HoursWorked1 { get; set; }
        public decimal PiecesPicked1 { get; set; }

        public int HoursWorked2 { get; set; }
        public decimal PiecesPicked2 { get; set; }

        public int HoursWorked3 { get; set; }
        public decimal PiecesPicked3 { get; set; }

        public WarehousePickerProductivityDashboardBarColumnChartSubDataBO SubData { get; set; }

    }

    public class WarehousePickerProductivityDashboardBarColumnChartSubDataBO
    {
        public List<WarehousePickerProductivityDashboardBarColumnChartBO> Top { get; set; }
        public List<WarehousePickerProductivityDashboardBarColumnChartBO> Bottom { get; set; }
    }

    public class WarehousePickerProductivityDayReportBO
    {
        public string UserId { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskDateString { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public decimal PiecesPicked { get; set; }
        public decimal AveragePiecesPicked { get; set; }
        public decimal HoursWorked { get; set; }

    }

}
