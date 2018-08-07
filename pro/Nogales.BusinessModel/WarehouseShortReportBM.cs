using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class WarehouseShortReportBM
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Customer { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Buyer { get; set; }
        public string UOM { get; set; }
        public decimal QuantityNeeded { get; set; }
        public decimal TransactionCost { get; set; }
        public decimal MarketPrice { get; set; }
        public string SalesOrderNumber { get; set; }
        public string Date { get; set; }
        public double? AvgProductivity { get; set; }

        public string Email { get; set; }

        public bool NotifiedOrIgnored { get; set; }

        public decimal QuantityLeft { get; set; }
        public decimal QuantityOrd { get; set; }

        public string Type { get; set; }

        public decimal QuantityOnHand { get; set; }
        public decimal QtyAvailable { get; set; }
        public string Notes { get; set; }
        public string BinNo { get; set; }
        public string Picker { get; set; }
        public string PickerName { get; set; }
    }
}
