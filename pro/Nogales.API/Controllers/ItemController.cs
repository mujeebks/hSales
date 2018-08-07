using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.ENUM;
using System;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using Nogales.API.Utilities;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Item")]
    [AuthorizeModuleAccess(Modules = "Item,Buyers")]
    public class ItemController : ApiController
    {
        /// <summary>
        /// Get Item Report
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        [Route("GetItemReport")]
        [HttpPost]
        public IHttpActionResult GetItemReport(ItemReportFilterBM reportFilterBM)
        {
            var itemProvider = new ItemProvider();

            var currentStart = DateTime.Parse(reportFilterBM.currentStartDate);
            var currentEnd = DateTime.Parse(reportFilterBM.currentEndDate);

            var model = itemProvider.GetItemReport(currentStart.Date.ToString("yyyy/MM/dd"), currentEnd.Date.ToString("yyyy/MM/dd"), reportFilterBM.item);
            return Ok(model);
        }

        [Route("GetItemReportByItem")]
        [HttpPost]
        public IHttpActionResult GetItemReportByItem(ItemReportByItemFilterBM reportFilterBM)
        {
            var itemProvider = new ItemProvider();

            var currentStart = DateTime.Parse(reportFilterBM.currentStartDate);
            var currentEnd = DateTime.Parse(reportFilterBM.currentEndDate);
            var month = reportFilterBM.Month;
            var year = reportFilterBM.Year;
            var week = reportFilterBM.Week;
            var item = reportFilterBM.Item.Trim();
            var model = itemProvider.GetItemReportByItem(currentStart.Date.ToString("yyyy/MM/dd"), currentEnd.Date.ToString("yyyy/MM/dd"),item,week,month,year);
            return Ok(model);
        }
        [Route("GetAllItems")]
        [HttpGet]
        public ItemMapper GetAllItems()
        {
            var itemProvider = new ItemProvider();
            var revenueDataProvider = new SalesDataProvider();
            var model = itemProvider.GetAllItems();
            return model;
        }

        [Route("GetItems")]
        [HttpGet]
        public ItemMapper GetItems(string item)
        {
            var itemProvider = new ItemProvider();
            var revenueDataProvider = new SalesDataProvider();
            var model = itemProvider.GetItems(item);
            return model;
        }

        [Route("GetItemComparisonReport")]
        [HttpPost]
        public List<ItemComparisonReportBO> GetItemComparisonReport(CostcomparisionFilterBM costcomparisionFilterBM)
        {
            var itemProvider = new ItemProvider();
            var revenueDataProvider = new SalesDataProvider();
            var model = itemProvider.GetItemComparisonReport(Convert.ToInt16(costcomparisionFilterBM.filterId), costcomparisionFilterBM.Comodity, costcomparisionFilterBM.item);
            return model;
        }

        [Route("GetItemVendorReport")]
        [HttpPost]
        public List<ItemVendorComparisonReportBO> GetItemVendorReport(CostcomparisionFilterBM costcomparisionFilterBM)
        {
            var itemProvider = new ItemProvider();
            var revenueDataProvider = new SalesDataProvider();
            var model = itemProvider.GetItemVendorCostReport(Convert.ToInt16(costcomparisionFilterBM.filterId), costcomparisionFilterBM.Comodity, costcomparisionFilterBM.item);
            return model;
        }
    }
}
