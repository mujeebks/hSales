using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;

namespace Nogales.DataProvider
{
    public class SalesPersonMappingDataProvider : DataAccessADO
    {
        public List<SalesPersonsBO> GetMappedSalesPersonList()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetActiveMappedSalesPerson", parameterList.ToArray());
                var listDataResult = dataSetResult.Tables[0]
                 .AsEnumerable()
                 .Select(x => new SalesPersonsBO
                 {
                     Id = x.Field<int?>("Id").HasValue ? x.Field<int>("Id") : 0,
                     SalesPersonCode = x.Field<string>("SalesPersonCode"),
                     SalesPersonDescription = x.Field<string>("SalesPersonDescription"),
                     AssignedPersonCode = x.Field<string>("AssignedPersonCode"),
                     AssignedPersonDescription = x.Field<string>("AssignedPersonDescription"),
                     StartDate = x.Field<DateTime?>("StartDate").HasValue ? x.Field<DateTime>("StartDate").ToString("MM-dd-yyyy") : null,
                     Category = x.Field<string>("TrackingGroup")
                 })
                 .OrderBy(p=>p.AssignedPersonCode)
                 .ToList();

                //var result = listDataResult
                //                  .GroupBy(x => x.SalesPersonCode)
                //                  .Select(x => new SalesPersonMappingBM
                //                  {
                //                      SalesPersonCode = x.Select(y => y.SalesPersonCode).First(),
                //                      SalesPersonDescription = x.Select(y => y.SalesPersonDescription).First(),
                //                      AssignedPersonList = x.GroupBy(s => s.AssignedPersonCode)
                //                                              .Select(s => new AssignedPersonBM
                //                                              {
                //                                                  AssignedPersonCode = s.Select(p => p.AssignedPersonCode).First(),
                //                                                  AssignedDescription = s.Select(p => p.AssignedPersonDescription).First(),
                //                                                  StartDate = s.Select(p => { if (p.StartDate == null) return null; else return p.StartDate; }).FirstOrDefault(),
                //                                                  //StartDate = Convert.ToString(Convert.ToDateTime(s.Select(p => { if (p.StartDate == null)return p.StartDate; else return null; }).FirstOrDefault()).ToString("MM-dd-yyyy")),
                //                                                  //EndDate =Convert.ToString(Convert.ToDateTime(s.Select(p => { if (p.EndDate == null) return null; else return p.EndDate; }).FirstOrDefault()).ToString("MM-dd-yyyy"))
                //                                                  EndDate = null

                //                                              }).ToList(),
                //                      Category = x.Select(y => y.Category).First()

                //                  }).ToList();

                return listDataResult;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesPersonMappingBM> GetAllSalesPersonForFiltering()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetAllSalesPersonForFiltering", parameterList.ToArray());
                var listDataResult = dataSetResult.Tables[0]
                 .AsEnumerable()
                 .Select(x =>
                     new
                     {

                         SalesPersonCode = x.Field<string>("AssignedPersonCode"),
                         SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : ""

                     }).Distinct()
                     .ToList();

                var result = listDataResult.Select(x => new SalesPersonMappingBM
                {
                    SalesPersonCode = x.SalesPersonCode,
                    SalesPersonDescription = x.SalesPersonDescription
                })
                .OrderBy(p=>p.SalesPersonCode)
                .ToList();

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SalesPersonMappingFilterBO GetFiltersForSalesPersonMapping()
        {
            SalesPersonMappingFilterBO model = new SalesPersonMappingFilterBO();
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetFiltersForSalesPersonMapping", parameterList.ToArray());
                var listMasterSalesPersons = dataSetResult.Tables[0]
                 .AsEnumerable()
                 .Select(x =>
                     new SalesPersonsDetails
                     {

                         Code = x.Field<string>("MasterSalesperson"),
                         Description = x.Field<string>("Description"),
                         IsAssigned = x.Field<int>("Assigned")

                     }).ToList();

                var listSalesPersons = dataSetResult.Tables[1]
               .AsEnumerable()
               .Select(x =>
                   new SalesPersonsDetails
                   {

                       Code = x.Field<string>("code"),
                       Description = x.Field<string>("SalesPerson"),
                       IsAssigned = x.Field<int>("Assigned")

                   }).ToList();

                var listTrackingGroups = dataSetResult.Tables[2]
             .AsEnumerable()
             .Select(r => r.Field<string>("TrackingGroup")
                 ).ToList();

                model.MasterSalesPersons = listMasterSalesPersons;
                model.SalesPersons = listSalesPersons;
                model.TrackingGroups = listTrackingGroups;
                return model;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ArchivedSalesPersonMappingBM> GetArchivedSalesPersonList(ArchivedSalesPersonRequestBM archivedSalesPersonRequestBM)
        {
            try
            {
                string salesPersonCodeString = string.Empty;

                //if (archivedSalesPersonRequestBM.SalesPersonCode != null)
                //{
                //    if (archivedSalesPersonRequestBM.SalesPersonCode.Count > 0)
                //    {

                //        for (int count = 0; count < archivedSalesPersonRequestBM.SalesPersonCode.Count; count++)
                //        {
                //            salesPersonCodeString = salesPersonCodeString + archivedSalesPersonRequestBM.SalesPersonCode[count].SalesPersonCode + "','";
                //        }

                //        salesPersonCodeString = salesPersonCodeString.Remove(salesPersonCodeString.Length - 1);
                //        salesPersonCodeString = salesPersonCodeString.Remove(salesPersonCodeString.Length - 1);
                //        salesPersonCodeString = salesPersonCodeString.Remove(salesPersonCodeString.Length - 1);
                //    }
                //}
                DataSet dataSetResult = new DataSet();
                var codeString = string.Join("','", archivedSalesPersonRequestBM.SalesPersonCode);
                if (codeString != "")
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@salespersoncode", codeString));
                    parameterList.Add(new SqlParameter("@startDate", archivedSalesPersonRequestBM.startDate));
                    parameterList.Add(new SqlParameter("@enddate", archivedSalesPersonRequestBM.endDate));
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_GetArchivedSalesPersons", parameterList.ToArray());

                }
                else
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@startDate", archivedSalesPersonRequestBM.startDate));
                    parameterList.Add(new SqlParameter("@enddate", archivedSalesPersonRequestBM.endDate));
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_GetAllArchivedSalesPersons", parameterList.ToArray());
                }

                //var query = SQLQueries.GetArchivedSalesPersonListQuery(codeString, archivedSalesPersonRequestBM.startDate, archivedSalesPersonRequestBM.endDate);

                //dataSetResult = base.ReadToDataSet(query);
                var listDataResult = dataSetResult.Tables[0]
                              .AsEnumerable()
                              .Select(x =>
                                  new
                                  {
                                      Id = x.Field<int>("Id"),
                                      SalesPersonCode = x.Field<string>("SalesPersonCode"),
                                      SalesPersonDescription = x.Field<string>("SalesPersonDescription"),
                                      AssignedPersonCode = x.Field<string>("AssignedPersonCode"),
                                      AssignedPersonDescription = x.Field<string>("AssignedPersonDescription"),
                                      StartDate =x.Field<DateTime>("StartDate"),
                                      EndDate = x.Field<DateTime>("EndDate")
                                  }).ToList();

                var result = listDataResult
                              .Select(x => new ArchivedSalesPersonMappingBM
                              {
                                  Id = x.Id,
                                  SalesPersonCode = x.SalesPersonCode,
                                  SalesPersonDescription = x.SalesPersonDescription,
                                  AssignedPersonCode = x.AssignedPersonCode,
                                  AssignedDescription = x.AssignedPersonDescription,
                                  StartDate = x.StartDate,
                                  EndDate = x.EndDate
                              })
                              .OrderBy(p=>p.AssignedPersonCode)
                              .ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AssignSalesPersonCode(SalesPersonMappingBM salesPersonMappingBM)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        for (int count = 0; count < salesPersonMappingBM.AssignedPersonList.Count; count++)
                        {
                            List<SqlParameter> parameterList = new List<SqlParameter>();
                            parameterList.Add(new SqlParameter("@salespersoncode", salesPersonMappingBM.SalesPersonCode));
                            parameterList.Add(new SqlParameter("@salespersondesc", salesPersonMappingBM.SalesPersonDescription));
                            parameterList.Add(new SqlParameter("@assignedpersoncode", salesPersonMappingBM.AssignedPersonList[count].AssignedPersonCode));
                            parameterList.Add(new SqlParameter("@assignedpersondesc", salesPersonMappingBM.AssignedPersonList[count].AssignedDescription));
                            parameterList.Add(new SqlParameter("@startdate", salesPersonMappingBM.AssignedPersonList[count].StartDate));
                            parameterList.Add(new SqlParameter("@category", salesPersonMappingBM.Category));
                            var result = base.ExecuteNonQueryFromStoredProcedure("BI_AssignSalesPerson", parameterList.ToArray());

                            //var query = SQLQueries.AssignSalesPersonCodeQuery(salesPersonMappingBM, count);
                            //var result = base.ExecuteQuery(query);
                        }
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AssignSalesPerson(AssignSalesPersonsBO salesPersonMappingBM)
        {
            int success = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {

                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@salespersoncode", salesPersonMappingBM.SalesPersonCode));
                        parameterList.Add(new SqlParameter("@salespersondesc", salesPersonMappingBM.SalesPersonDescription));
                        parameterList.Add(new SqlParameter("@assignedpersoncode", salesPersonMappingBM.AssignedPersonCode));
                        parameterList.Add(new SqlParameter("@assignedpersondesc", salesPersonMappingBM.AssignedDescription));
                        parameterList.Add(new SqlParameter("@startdate", salesPersonMappingBM.StartDate));
                        parameterList.Add(new SqlParameter("@category", salesPersonMappingBM.TrackingGroup));
                        var result = base.ExecuteNonQueryFromStoredProcedure("BI_MapSalesPerson", parameterList.ToArray());

                        //var query = SQLQueries.AssignSalesPersonCodeQuery(salesPersonMappingBM, count);
                        //var result = base.ExecuteQuery(query);
                        success = result;
                        scope.Complete();

                    }
                    catch (Exception)
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateSalesPersonDescription(SalesPersonsBO salesPerson)
        {
            int success = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {

                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@Id", salesPerson.Id));
                        parameterList.Add(new SqlParameter("@SalesPerson", salesPerson.SalesPersonDescription));
                        parameterList.Add(new SqlParameter("@AssignedPerson", salesPerson.AssignedPersonDescription));
                        var result = base.ExecuteNonQueryFromStoredProcedure("BI_UpdateSalesPersonDescription", parameterList.ToArray());
                        success = result;
                        scope.Complete();

                    }
                    catch (Exception)
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UnAssignSalesPersonCode(string salesPersonCode, string assignSalesPersonCode, string endDate)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salespersoncode", salesPersonCode));
                parameterList.Add(new SqlParameter("@assignedpersoncode", assignSalesPersonCode));
                parameterList.Add(new SqlParameter("@enddate", endDate));

                var result = base.ExecuteNonQueryFromStoredProcedure("BI_UnAssignSalesPerson", parameterList.ToArray());
                //var query = SQLQueries.UnAssignSalesPersonCodeQuery(salesPersonCode, assignSalesPersonCode, endDate);
                //var result = base.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


    }
}
