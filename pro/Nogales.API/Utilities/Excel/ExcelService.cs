using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Nogales.API.Utilities.Excel
{
    /// <summary>
    /// Service to handle excel files
    /// </summary>
    public class ExcelService : ExcelCore
    {
        /// <summary>
        /// Generate excel report of Warehouse shortage and return it as MemoryStream
        /// </summary>
        /// <param name="data"> List of warehouse shortage </param>
        /// <returns>Report as memory stream</returns>
        public MemoryStream GenerateWarehouseShortExcel(List<WarehouseShortReportBM> data, string date)
        {
            try
            {
                MemoryStream stream = new MemoryStream();

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Warehouse Shortage" };

                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    // Constructing header
                    Row row = new Row();

                    row.Append(
                        ConstructCell("Nogales - Warehouse Shortage Report for " + date, CellValues.String)
                        );

                    sheetData.AppendChild(row);

                    row = new Row();

                    row.Append(
                        base.ConstructCell("Route", CellValues.String),
                        base.ConstructCell("Customer", CellValues.String),
                        base.ConstructCell("Item", CellValues.String),
                        base.ConstructCell("Description", CellValues.String),
                        base.ConstructCell("Buyer", CellValues.String),
                        base.ConstructCell("UOM", CellValues.String),
                        base.ConstructCell("Quantity Needed", CellValues.String),
                        base.ConstructCell("Transaction Cost", CellValues.String),
                        base.ConstructCell("Market Price", CellValues.String),
                        base.ConstructCell("SalesOrder Number", CellValues.String)
                    );

                    // Insert the header row to the Sheet Data
                    sheetData.AppendChild(row);

                    // Inserting tbody
                    foreach (var item in data)
                    {
                        row = new Row();

                        row.Append(
                                ConstructCell(item.Route, CellValues.String),
                                ConstructCell(item.Customer, CellValues.String),
                                ConstructCell(item.Item, CellValues.String),
                                ConstructCell(item.Description, CellValues.String),
                                ConstructCell(item.Buyer, CellValues.String),
                                ConstructCell(item.UOM, CellValues.String),
                                ConstructCell(item.QuantityNeeded.ToString(), CellValues.Number),
                                ConstructCell(item.TransactionCost.ToString(), CellValues.Number),
                                ConstructCell(item.MarketPrice.ToString(), CellValues.Number),
                                ConstructCell(item.SalesOrderNumber, CellValues.String)
                            );

                        sheetData.AppendChild(row);
                    }

                    worksheetPart.Worksheet.Save();
                    document.Close();
                    stream.Seek(0, SeekOrigin.Begin);
                    
                    return stream;
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while creating excel report - " + e.Message);
            }
        }
    }
}