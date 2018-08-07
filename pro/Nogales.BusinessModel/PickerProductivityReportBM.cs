using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class PickerProductivityReportBM
    {

        public string EmployeeId { get; set; }
        // public DateTime WorkDate{ get; set; }
        public string Name { get; set; }
        public decimal PiecesPicked { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal PiecesPerHour { get; set; }
        public DateTime ShiftTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
    public class PickerProductivityReportDetailsBM
    {

        public string EmployeeId { get; set; }
        // public DateTime WorkDate{ get; set; }
        public string Name { get; set; }
        public decimal PiecesPicked { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal PiecesPerHour { get; set; }
        public DateTime ShiftTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } 
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public string SalesOrderNo { get; set; }


    }
}
