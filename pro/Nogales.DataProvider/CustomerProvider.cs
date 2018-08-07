using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider
{
    public class CustomerProvider : DataAccessADO
    {
        #region Get Customer Report
        public List<CustomerBM> GetCustomerReport(string startDate)
        {
            string query = string.Empty;
            var dataSetResult = new DataSet();
            DataTable dtIds = new DataTable();
            dtIds.Columns.Add("Id", typeof(int));
            DateTime? nullableDateTime = null;

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", startDate));
            //parameterList.Add(new SqlParameter("@endDate", endDate ?? ""));
            //parameterList.Add(new SqlParameter("@customer", customer ?? ""));
            dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCustomerReport", parameterList.ToArray());

            var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new CustomerBM
                            {
                                Address = x.Field<string>("address1"),
                                ContactPerson = x.Field<string>("POC"),
                                CustomerName = x.Field<string>("Customer"),
                                CustomerNumber = x.Field<string>("custno"),
                                EnteredDate = x.Field<DateTime>("entered"),
                                LastPayedAmount = x.Field<decimal>("LastPayment"),
                                LastPayedDate = x.Field<DateTime>("LastPayDate") == new DateTime(1900, 1, 1) ? nullableDateTime : x.Field<DateTime>("LastPayDate").Date,
                                LastSaleDate = x.Field<DateTime>("LastSaleDate") == new DateTime(1900, 1, 1) ? nullableDateTime : x.Field<DateTime>("LastSaleDate").Date,
                                LastSalesAmount = x.Field<decimal>("LastSale"),
                                Phone = x.Field<string>("phone"),
                                SalesPerson = x.Field<string>("salesmn")
                            })
                            .OrderByDescending(x=>x.LastSaleDate).ThenByDescending(x=>x.CustomerName)
                            .ToList();

            return result;
        }
        #endregion
    }
}
