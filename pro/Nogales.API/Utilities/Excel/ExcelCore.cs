using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nogales.API.Utilities.Excel
{
    /// <summary>
    /// Core functions for excel service
    /// </summary>
    public class ExcelCore
    {
        /// <summary>
        /// Create a cell and return the object of the cell
        /// </summary>
        /// <param name="value"> Value to add in the cell. Note all types of the values should be passed as string and specify it's actual type in the parameter dataType</param>
        /// <param name="dataType"> Data type of the Value</param>
        /// <returns></returns>
        internal Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
    }
}