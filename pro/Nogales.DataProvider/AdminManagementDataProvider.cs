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
    public class AdminManagementDataProvider : DataAccessADO
    {
        #region Sales person Category
        public List<string> GetAllSalesPersonCategories()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_SL_GetAllSalesPersonCategory", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                 .AsEnumerable()
                 .Select(r => r.Field<string>("Speciality"))
                 .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetAllCategories", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                 .AsEnumerable()
                       .Select(x => new Category
                       {
                           Id = x.Field<int>("Id"),
                           Name = x.Field<string>("Name"),
                           IsAccess = true
                       })
                 .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Module> GetAllModules()
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetAllModules", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                 .AsEnumerable()
                 .Select(x => new Module {
                     Id = x.Field<int>("Id"),
                     Name = x.Field<string>("Name"),
                     IsAccess = true
                 })
                 .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region User Module Access
        public int AddUserModuleAccess(List<Module> userModuleAccess,string userId)
        {
            int result = 0;
            try
            {
                foreach (var item in userModuleAccess)
                {
                    if (item.IsAccess)
                    {
                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@UserId", userId));
                        parameterList.Add(new SqlParameter("@ModuleId", item.Id));
                        result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_AddUserModuleAccess", parameterList.ToArray());
                    }
                    else
                    {
                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@UserId", userId));
                        parameterList.Add(new SqlParameter("@ModuleId", item.Id));
                        result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_RemoveUserModuleAccess", parameterList.ToArray());

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Module Access
        public int AddUserDisplayModuleAccess(Module userDisplayModuleAccess, string userId)
        {
            int result = 0;
            try
            {
                if (userDisplayModuleAccess != null) {
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@UserId", userId));
                    parameterList.Add(new SqlParameter("@ModuleId", userDisplayModuleAccess.Id));
                    result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_AddRemoveUserDisplayModuleAccess", parameterList.ToArray());
                } else
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@UserId", userId));
                    result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_RemoveUserDisplayModuleAccess", parameterList.ToArray());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Category Access
        public int AddUserCategoryAccess(List<Category> userCategoryAccess,string userId)
        {
            int result = 0;
            try
            {
                foreach (var item in userCategoryAccess)
                {
                    if (item.IsAccess)
                    {
                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@UserId", userId));
                        parameterList.Add(new SqlParameter("@CategoryId", item.Id));
                        result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_AddUserCategoryAccess", parameterList.ToArray());
                    }
                    else
                    {
                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        parameterList.Add(new SqlParameter("@UserId", userId));
                        parameterList.Add(new SqlParameter("@CategoryId", item.Id));
                        result = base.ExecuteNonQueryFromStoredProcedure("BI_USR_RemoveUserCategoryAccess", parameterList.ToArray());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get User Details
        public UserDetails GetUserDetails(string userId)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                var userDataSet = base.ReadToDataSetViaProcedure("BI_USR_GetUserDetails", parameterList.ToArray());
                var userDetails = userDataSet.Tables[0]
                 .AsEnumerable()
                 .Select(x => new UserDetails
                 {
                     FirstName = x.Field<string>("FirstName"),
                     LastName = x.Field<string>("LastName"),
                     UserId = x.Field<string>("Id"),
                     Email = x.Field<string>("Email"),
                     IsRestrictedCategoryAccess = x.Field<bool?>("IsRestrictedCategoryAccess").Value,
                     IsRestrictedModuleAccess = x.Field<bool?>("IsRestrictedModuleAccess").Value,
                 })
                 .FirstOrDefault();


                if (userDetails.IsRestrictedCategoryAccess)
                {
                    userDetails.Categories = GetAllUserAssignedCategoris(userId);
                }


                if (userDetails.IsRestrictedModuleAccess)
                {
                    userDetails.Modules = GetAllUserAssignedModules(userId);
                }

                userDetails.DisplayModule = GetUserAssignedDisplayModule(userId);

                return userDetails;
            }
            catch (Exception ex)
            {
                Utilities.ErrorLog.ErrorLogging(ex);
                throw ex;
            }
        }

        #endregion

        private List<Category> GetAllUserAssignedCategoris(string userId)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@UserId", userId));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetUserAssignedCategories", parameterList
                .ToArray());
            var result = dataSetResult.Tables[0]
             .AsEnumerable()
             .Select(x => new Category
             {
                 Id = x.Field<int>("CategoryId"),
                 Name = x.Field<string>("CategoryName"),
                 IsAccess = true
             })
             .ToList();
            return result;
        }
        private List<Module> GetAllUserAssignedModules(string userId)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@UserId", userId));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetUserAssignedModules", parameterList.ToArray());
            var result = dataSetResult.Tables[0]
             .AsEnumerable()
             .Select(x => new Module
             {
                 Id = x.Field<int>("ModuleId"),  
                 Name = x.Field<string>("ModuleName"),
                 IsAccess = true
             })
             .ToList();

            return result;
        }
        private Module GetUserAssignedDisplayModule(string userId)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@UserId", userId));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_USR_GetUserAssignedDisplayModules", parameterList.ToArray());
            var result = dataSetResult.Tables[0]
             .AsEnumerable()
             .Select(x => new Module
             {
                 Id = x.Field<int>("ModuleId"),
                 Name = x.Field<string>("ModuleName"),
             })
             .FirstOrDefault();
            return result;
        }

        #region Get User Details
        public UserAccessModuleAndCategory GetUserAccess(string userId)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                var userDataSet = base.ReadToDataSetViaProcedure("BI_USR_GetUserDetails", parameterList.ToArray());
                var userDetails = userDataSet.Tables[0]
                 .AsEnumerable()
                 .Select(x => new UserAccessModuleAndCategory
                 {
                     IsRestrictedCategoryAccess = x.Field<bool?>("IsRestrictedCategoryAccess").Value,
                     IsRestrictedModuleAccess = x.Field<bool?>("IsRestrictedModuleAccess").Value,
                 })
                 .FirstOrDefault();
                var allCategories= GetAllCategories();
                var allModules = GetAllModules();
                if (userDetails.IsRestrictedCategoryAccess)
                {
                    var assignedCategores = GetAllUserAssignedCategoris(userId);
                    foreach (var item in allCategories) {

                        if (assignedCategores.Any(x => x.Id == item.Id))
                        {
                            item.IsAccess = true;
                        }
                        else
                        {
                            item.IsAccess = false;
                        }
                    }
                    userDetails.Categories = allCategories;
                }
                else
                {
                    userDetails.Categories = allCategories;
                }

                if (userDetails.IsRestrictedModuleAccess)
                {
                    var assignedModules = GetAllUserAssignedModules(userId);
                    foreach (var item in allModules)
                    {

                        if (assignedModules.Any(x => x.Id == item.Id))
                        {
                            item.IsAccess = true;
                        }
                        else
                        {
                            item.IsAccess = false;
                        }
                    }

                    userDetails.Modules = allModules;
                }
                else
                {
                    userDetails.Modules = GetAllModules();
                }

                Module result = GetUserAssignedDisplayModule(userId);

                if (result == null)
                  userDetails.DisplayModule = "";
                else
                  userDetails.DisplayModule = result.Name;

                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
    }
}
