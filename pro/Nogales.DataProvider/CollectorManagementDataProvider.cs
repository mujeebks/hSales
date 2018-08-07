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
    public class CollectorManagementDataProvider : DataAccessADO
    {
        public List<CollectorBM> GetAllCollectors()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetAllCollectors", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                 .AsEnumerable()
                       .Select(x => new
                       {
                           CollectorId = x.Field<int>("CollectorId"),
                           CollectorName = x.Field<string>("CollectorName"),
                           Ordinance = x.Field<int>("Ordinance"),
                           CollectorAssignmentId = x.Field<int?>("CollectorAssignmentId"),
                           LetterName = x.Field<string>("LetterName")
                       }).OrderBy(v=>v.Ordinance)
                 .ToList();

                var collectors = result.GroupBy(x => x.CollectorId)
                                    .Select(y => new CollectorBM
                                    {
                                        CollectorId = y.Key,
                                        CollectorName = y.First().CollectorName,
                                        Ordinance = y.First().Ordinance,
                                        CollectorAssignments = y.Any() ?
                                        y.Select(z => new CollectorAssignmentBM
                                        {
                                            CollectorAssignmentId = z.CollectorAssignmentId,
                                            LetterName = z.LetterName
                                        }).ToList()
                                        : new List<CollectorAssignmentBM> { }
                                    }).ToList();
                return collectors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UnAssignedCustomerPrefix> GetAllUnAssignedCustomerPrefixes()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetAllUnAssignedCustomerPrefixes", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                 .AsEnumerable()
                       .Select(x => new UnAssignedCustomerPrefix
                       {
                           CollectorAssignmentId = x.Field<int>("CollectorAssignmentId"),
                           LetterName = x.Field<string>("LetterName")
                       })
                 .ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertCollector(string collectorName)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@collectorName", collectorName));

                var result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_InsertCollector", parameterList.ToArray());

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistCollector(int collectorId, string collectorName)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@collectorName", collectorName));
                parameterList.Add(new SqlParameter("@collectorId", collectorId));

                var result = base.ExecuteScalarFromStoredProcedure("BI_USR_IsExistCollector", parameterList.ToArray());

                if (result == null)
                    return false;
                else
                    return true;

                //return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCollector(CollectorBM collector)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@collectorId", collector.CollectorId));
                parameterList.Add(new SqlParameter("@collectorName", collector.CollectorName));
                parameterList.Add(new SqlParameter("@newOrder", collector.Ordinance));

                var result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_UpdateCollector", parameterList.ToArray());

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteCollector(int collectorId)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@collectorId", collectorId));

                var result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_DeleteCollector", parameterList.ToArray());

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AssignUnAssignCustomerPrefix(UnAssignedCustomerPrefix collectorAssignment)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@collectorAssignmentId", collectorAssignment.CollectorAssignmentId));
                parameterList.Add(new SqlParameter("@collectorId", (object)collectorAssignment.CollectorId ?? DBNull.Value));

                var result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_AssignCustomerPrefix", parameterList.ToArray());

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
