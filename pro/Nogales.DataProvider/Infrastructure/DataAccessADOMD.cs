using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Infrastructure
{
    public class DataAccessADOMD
    {
        //private readonly string _connectionString;

        //public DataAccessADOMD()
        //{
        //    _connectionString = ConfigurationManager.ConnectionStrings["CubeConnectionString"].ConnectionString;

        //}
        //protected DataTable GetDataTable(string sqlString)
        //{
        //    try
        //    {
        //        using (var connection = new AdomdConnection(_connectionString))
        //        {
        //            var dataSet = new DataSet();
        //            var sqlCommand = new AdomdCommand
        //            {
        //                CommandText = sqlString,
        //                Connection = connection
        //            };

        //            var dataAdapter = new AdomdDataAdapter(sqlCommand);
        //            var datatable = new DataTable();
        //            dataAdapter.Fill(datatable);

        //            return datatable;
        //        }
        //    }
        //    catch (AdomdException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //internal object ReadToDataSet(string query)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
