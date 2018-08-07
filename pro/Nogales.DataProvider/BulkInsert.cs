using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Nogales.DataProvider
{
    public class BulkInsert
    {
        public static void Insert(DataTable dataTable,string tableName="")
        {
            try
            {

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IdentityConnectionString"].ConnectionString;


                #region MyRegion  SqlConnection

                using (TransactionScope ts = new TransactionScope())
                {
                    //Open a connection to the database.
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                       
                        #region comment
                        // Create the SqlBulkCopy object. 
                        // Note that the column positions in the source DataTable 
                        // match the column positions in the destination table so 
                        // there is no need to map columns. 
                        #endregion

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "dbo." + tableName;

                            try
                            {
                                // Write from the source to the destination.
                                bulkCopy.WriteToServer(dataTable);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        ts.Complete();
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Truncate(string tableName = "")
        {
            try
            {

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IdentityConnectionString"].ConnectionString;


                #region MyRegion  SqlConnection

                using (TransactionScope ts = new TransactionScope())
                {
                    //Open a connection to the database.
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        //Truncate table 
                        if (tableName != string.Empty)
                        {
                            var commandRowCount = new SqlCommand("Truncate Table dbo." + tableName + ";", connection);
                            long count = Convert.ToInt32(commandRowCount.ExecuteScalar());
                        }
                        ts.Complete();
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
