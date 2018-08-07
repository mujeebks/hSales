using Nogales.BusinessModel;
using Nogales.DataProvider.AOD_WS;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using static Nogales.DataProvider.Utilities.Enums;
using System.Web.Script.Serialization;
namespace Nogales.DataProvider
{

    public class PayrollDataProvider : DataAccessADO
    {
        IAeXMLBridgeservice webServices = new IAeXMLBridgeservice();
        PayrollDataProvider _payrollDataProvider;

        #region Constructor
        public PayrollDataProvider()
        {
            NetworkCredential creds = new NetworkCredential()
            {
                UserName = ConfigurationManager.AppSettings["PayrollServiceUsername"],                    // replace with your username/access account
                Password = ConfigurationManager.AppSettings["PayrollServicePassword"]                          // replace with your password
            };

            webServices.Credentials = creds;
            webServices.PreAuthenticate = true;
            webServices.Timeout = -1;                      // -1 = no timeout, else 1 sec = 1000ms
            webServices.Url = ConfigurationManager.AppSettings["PayrollServiceUrl"];
        }
        #endregion

        #region PayRoll ETL Process

        #region Import Data from Service
        public bool ImportPayRollData(string minDate, string maxDate, string invokedBy)
        {
            _payrollDataProvider = new PayrollDataProvider();

            EmailServices emailService = new EmailServices();

            var importMaster = InitializeImportMaster("Employee", invokedBy);

            try
            {
                #region Employeee

                importMaster.Id = _payrollDataProvider.AddToImportMaster(importMaster);  //Insert to import master table

                var activeEmployees = webServices.getActiveEmployeesList().ToList();  // Select all employees from web service

                DataTable employeeDataTable = AddImportMasterIdToDataTable(activeEmployees.ToDataTable(), importMaster.Id);

                BulkInsert.Truncate("StagingEmployee");                         // Truncate and bulk insert to db.
                BulkInsert.Insert(employeeDataTable, "StagingEmployee");

                importMaster.RecordCount = activeEmployees.Count;
                importMaster.Notes = "Bulk Import Completed";

                _payrollDataProvider.UpdateImportMaster(importMaster);  //update master table 


                _payrollDataProvider.MergeEmployee();

                importMaster.Finish = DateTime.Now;
                importMaster.Status = (int)PayrollStatus.Success;
                importMaster.Notes = "Merging Employee Completed.";
                _payrollDataProvider.UpdateImportMaster(importMaster);


                #endregion

                #region Employee Payment Master

                try
                {
                    BulkInsert.Truncate("StagingEmployeePaymentDetails");


                    importMaster = InitializeImportMaster("Employee Payment Details", invokedBy);


                    importMaster.Id = _payrollDataProvider.AddToImportMaster(importMaster);

                    int count = 0;
                    foreach (var employee in activeEmployees)   //Loop each employeed to get their payment details
                    {
                        var empPaymentDetails = webServices.extractEmployeeSummsByIDNum(employee.EmpID, TDateRangeEnum.drCustom, minDate, maxDate).ToList();
                        if (empPaymentDetails.Count > 0)
                        {
                            count++;
                            DataTable employeePaymentDataTable = AddImportMasterIdToDataTable(empPaymentDetails.ToDataTable(), importMaster.Id);

                            BulkInsert.Insert(employeePaymentDataTable, "StagingEmployeePaymentDetails");
                            importMaster.RecordCount = count;
                            _payrollDataProvider.UpdateImportMaster(importMaster);
                        }
                    }

                    _payrollDataProvider.MergeEmployeePaymentDetails();

                    importMaster.Status = (int)PayrollStatus.Success;
                    importMaster.Notes = "Merging Employee Payment Details Completed";
                    importMaster.Finish = DateTime.Now;
                    _payrollDataProvider.UpdateImportMaster(importMaster);
                }
                catch (Exception ex)
                {
                    emailService.Payroll(minDate, maxDate, "ImportPayrollData => extractEmployeeSummsByIDNum", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), ex.Message);
                    UpdateImportMasterOnException(importMaster, ex);
                    return false;
                }

                #endregion

            }
            catch (Exception ex)
            {
                emailService.Payroll(minDate, maxDate, "ImportPayrollData => getActiveEmployeesList", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"), ex.Message);
                UpdateImportMasterOnException(importMaster, ex);
                return false;
            }

            return true;
        }
        #endregion

        #region Import Master
        public int AddToImportMaster(ImportMasterBM master)
        {
            int id = 0;
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Method", master.Method));
                parameterList.Add(new SqlParameter("@API", master.API));
                parameterList.Add(new SqlParameter("@InvokedBy", master.InvokedBy));
                parameterList.Add(new SqlParameter("@RecordCount", master.RecordCount));
                parameterList.Add(new SqlParameter("@Commence", master.Commence));
                parameterList.Add(new SqlParameter("@Status", master.Status));
                parameterList.Add(new SqlParameter("@Notes", master.Notes));
                var result = base.ReadToDataSetViaProcedure("BI_PYR_AddImportMaster", parameterList.ToArray());
                id = Convert.ToInt32(result.Tables[0].Rows[0][0]);
                return id;

            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int UpdateImportMaster(ImportMasterBM master)
        {
            int result = 0;
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", master.Id));
                parameterList.Add(new SqlParameter("@Method", master.Method));
                parameterList.Add(new SqlParameter("@API", master.API));
                parameterList.Add(new SqlParameter("@InvokedBy", master.InvokedBy));
                if (master.Finish != null)
                {
                    parameterList.Add(new SqlParameter("@Finish", master.Finish));
                }
                else
                {
                    parameterList.Add(new SqlParameter("@Finish", DBNull.Value));
                }
                parameterList.Add(new SqlParameter("@RecordCount", master.RecordCount));
                parameterList.Add(new SqlParameter("@Status", master.Status));
                parameterList.Add(new SqlParameter("@Notes", master.Notes));
                result = base.ExecuteNonQueryFromStoredProcedure("BI_PYR_UpdateImportMaster", parameterList.ToArray());
                return result;

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        #endregion

        #region MERGE Process

        public int MergeEmployee()
        {
            int result = 0;
            try
            {
                result = base.ExecuteNonQueryFromStoredProcedure("BI_PYR_MergeEmployee");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int MergeEmployeePaymentDetails()
        {
            int result = 0;
            try
            {
                result = base.ExecuteNonQueryFromStoredProcedure("BI_PYR_MergeEmployeePaymentDetails");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #endregion

        #region Get Payroll Data

        public PayrollResult GetEmployeePaymentDetails(PayrollFilter filter)
        {
            PayrollResult payrollResult = new PayrollResult();
            try
            {
                DataTable dtEmployee = new DataTable();
                dtEmployee.Columns.Add("Code");
                if (filter.Employees != null)
                {
                    foreach (int id in filter.Employees)
                    {
                        dtEmployee.Rows.Add(id);
                    }
                }

                List<SqlParameter> parameterList = new List<SqlParameter>();


                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtEmployee;
                param.ParameterName = "@Employees";

                parameterList.Add(param);
                parameterList.Add(new SqlParameter("@StartDate", filter.StartDate));
                parameterList.Add(new SqlParameter("@EndDate", filter.EndDate));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_PYR_GetEmployeesPaymentDetails", parameterList.ToArray());

                #region get Data From DB
                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new PayRollBM
                                            {
                                                Department = !string.IsNullOrEmpty(x.Field<string>("Department")) ? x.Field<string>("Department").Trim() : "",
                                                IdNumber = !string.IsNullOrEmpty(x.Field<string>("EmpId")) ? x.Field<string>("EmpId").Trim() : "",
                                                Id = x.Field<int>("Id"),
                                                Status = !string.IsNullOrEmpty(x.Field<string>("Status")) ? x.Field<string>("Status").Trim() : "",
                                                Supervisor = !string.IsNullOrEmpty(x.Field<string>("Supervisor")) ? x.Field<string>("Supervisor").Trim() : "",
                                                PaymentDescription = x.Field<string>("PaymentDescription"),
                                                PaymentValue = x.Field<double>("PaymentValue"),
                                                Employee = x.Field<string>("Name"),
                                                Rate = x.Field<double>("Rate"),
                                            }).ToList();

                #endregion


                var newlist = result.GroupBy(x => x.IdNumber).Select(p => new PayRollBM
                {
                    Id = p.First().Id,
                    Department = p.FirstOrDefault().Department,
                    Employee = p.First().Employee,
                    IdNumber = p.First().IdNumber,
                    Status = p.First().Status,
                    Supervisor = p.First().Supervisor,
                    Rate = p.First().Rate,

                    Regular = CalculatePaymentStringValues(p, "Regular"),
                    Orient = CalculatePaymentStringValues(p, "Orient"),
                    Overtime = CalculatePaymentStringValues(p, "Overtime"),

                    RegularSortValue = CalculatePaymentValues(p, "Regular"),
                    OrientSortValue = CalculatePaymentValues(p, "Orient"),
                    OvertimeSortValue = CalculatePaymentValues(p, "Overtime"),

                    Total = ConvertToPayrollTimeFormat(CalculatePaymentValues(p, "Regular")
                            + CalculatePaymentValues(p, "Orient")
                            + CalculatePaymentValues(p, "Overtime")),

                    TotalSortValue = CalculatePaymentValues(p, "Regular")
                            + CalculatePaymentValues(p, "Orient")
                            + CalculatePaymentValues(p, "Overtime")

                }).ToList();

                newlist = newlist.Where(x => x.TotalSortValue > 0).ToList();

                payrollResult.PayRoll = newlist;

                payrollResult.TotalOverTime = ConvertToPayrollTimeFormat(newlist.Sum(x => x.OvertimeSortValue));
                payrollResult.TotalRegular = ConvertToPayrollTimeFormat(newlist.Sum(x => x.RegularSortValue));
                payrollResult.TotalTotal = ConvertToPayrollTimeFormat(newlist.Sum(x => x.TotalSortValue));

                return payrollResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<KeyValuePair<int, string>> GetAllEmployees()
        {

            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                var dataTableResult = base.ReadToDataSetViaProcedure("BI_PYR_GetAllEmployees");


                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                             .Select(x => new KeyValuePair<int, string>(

                                                 x.Field<int>("Id"),
                                                   x.Field<string>("Name")
                                                 )).ToList();



                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string CalculatePaymentStringValues(IGrouping<string, PayRollBM> val, string type)
        {
            if (val.Where(x => x.PaymentDescription.Trim() == type).Count() > 0)
            {
                var paymentDetails = val.Where(x => x.PaymentDescription.Trim() == type).ToList();
                double sum = 0;
                sum = val.Where(x => x.PaymentDescription.Trim() == type).Sum(p => p.PaymentValue);

                TimeSpan timespan = TimeSpan.FromHours(sum);
                int total24 = (int)(sum / 24);
                string output = timespan.ToString("h\\:mm");
                var value = total24 * 24 + Convert.ToDouble(output.Split(':')[0]) + ":" + output.Split(':')[1];


                string hrArray = (total24 * 24 + Convert.ToDouble(output.Split(':')[0])).ToString();
                string minAray = (output.Split(':')[1]);

                //if (hrArray.Length < 2)
                //{
                //    hrArray = "0" + hrArray;
                //}

                if (minAray.Length < 2)
                {
                    minAray = "0" + minAray;
                }

                var result = hrArray + ":" + minAray;


                return result;

            }
            else
            {
                return "0:00";
            }
        }
        private string ConvertToPayrollTimeFormat(double value)
        {

            TimeSpan timespan = TimeSpan.FromHours(value);
            int total24 = (int)(value / 24);
            string output = timespan.ToString("h\\:mm");

            string hrArray = (total24 * 24 + Convert.ToDouble(output.Split(':')[0])).ToString();
            string minAray = (output.Split(':')[1]);

            //if (hrArray.Length < 2) {
            //    hrArray = "0" + hrArray;
            //}

            if (minAray.Length < 2)
            {
                minAray = "0" + minAray;
            }

            var result = hrArray + ":" + minAray;

            return result;
        }
        //private string ConvertToPayrollTimeFormat(string value)
        //{

        //}


        private double CalculatePaymentValues(IGrouping<string, PayRollBM> val, string type)
        {
            if (val.Where(x => x.PaymentDescription.Trim() == type).Count() > 0)
            {
                var paymentDetails = val.Where(x => x.PaymentDescription.Trim() == type).ToList();
                double sum = 0;
                sum = val.Where(x => x.PaymentDescription.Trim() == type).Sum(p => p.PaymentValue);
                return sum;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region private methods

        private ImportMasterBM InitializeImportMaster(string method, string invokedBy)
        {
            var importMaster = new ImportMasterBM
            {
                API = "Payroll/ImportPayRollData",
                Commence = DateTime.Now,
                Finish = null,
                InvokedBy = invokedBy,
                Method = method,
                Notes = "",
                RecordCount = 0,
                Status = (int)PayrollStatus.Progress

            };
            return importMaster;
        }

        private int UpdateImportMasterOnException(ImportMasterBM importMaster, Exception ex)
        {
            _payrollDataProvider = new PayrollDataProvider();
            importMaster.Finish = DateTime.Now;
            importMaster.Status = (int)PayrollStatus.Failed;
            importMaster.Notes = ex.Message;
            return _payrollDataProvider.UpdateImportMaster(importMaster);
        }

        private DataTable AddImportMasterIdToDataTable(DataTable dt, int id)
        {
            dt.Columns.Add("ImportMasterId", typeof(System.Int32));
            foreach (DataRow row in dt.Rows) { row["ImportMasterId"] = id; }
            return dt;
        }
        #endregion

        public void GetData()
        {
            var data = webServices.getActiveEmployeesList().ToList();
            var PayClasses = webServices.getPayClassesSimple();
            //var listEmpId = new List<string> { "502196", "502197", "502198", "502199", "502200", "502201", "502202", "502195", "502193", "502194", "502192", "502186", "502188" };

            //data = data.Where(x => listEmpId.Contains(x.EmpID)).ToList();

            var listEmp = new List<Emp>();
            data.ForEach(employee =>
            {

                var empPaymentDetails = webServices.extractEmployeeSummsByIDNum(employee.EmpID, TDateRangeEnum.drCustom, "2017-04-20", "2018-04-30").ToList();

                var sabu = webServices.getEmployeeRateHistoryByIDNum(employee.EmpID, "2015/03/01", "2018/04/20");
                var employeeData = webServices.getEmployeeDetail2ByIDNum(employee.EmpID);

                listEmp.Add(new Emp
                {
                    //ActiveStatus = employeeData.ActiveStatus,
                    //ActiveStatusConditionEffDate = employeeData.ActiveStatusConditionEffDate,
                    //ActiveStatusConditionID = employeeData.ActiveStatusConditionID,
                    //Address1 = employeeData.Address1,
                    //AddressCity = employeeData.AddressCity,
                    //AddressStateProv = employeeData.AddressStateProv,
                    //AddressZIPPC = employeeData.AddressZIPPC,
                    Badge = employeeData.Badge,
                    BirthDate = employeeData.BirthDate,
                    //ClockGroupID = employeeData.ClockGroupID,
                    CurrentRate = employeeData.CurrentRate,
                    CurrentRateEffDate = employeeData.CurrentRateEffDate,
                    DateOfHire = employeeData.DateOfHire,
                    //EMAIL = employeeData.EMAIL,
                    //EmergencyContact = employeeData.EmergencyContact,
                    EmpId = employeeData.EmpID,
                    EmpName = employeeData.EmpName,
                    //ESSPIN = employeeData.ESSPIN,
                    FileKey = sabu.First().Filekey,
                    //FirstName = employeeData.FirstName,
                    HourlyStatusEffDate = employeeData.HourlyStatusEffDate,
                    //IsCurrent = sabu.First().IsCurrent,
                    //LastName = employeeData.LastName,
                    PayClassEffDate = employeeData.PayClassEffDate,
                    //PayClassID = employeeData.PayClassID,
                    //PayType = sabu.First().PayType,
                    PayClass = PayClasses.First(x => x.Num == employeeData.PayClassID).NameLabel,
                    PayTypeEffDate = employeeData.PayTypeEffDate,
                    Rate = sabu.Last().Rate,
                    RateEffDate = sabu.Last().RateEffDate,
                    SchPatternEffDate = employeeData.SchPatternEffDate,
                    //UniqueId = sabu.First().UniqueID,
                    WG1 = employeeData.WG1,
                    WG2 = employeeData.WG2,
                    WG3 = employeeData.WG3,
                    WG4 = employeeData.WG4,
                    WG5 = employeeData.WG5,
                    WG6 = employeeData.WG6,
                    WG7 = employeeData.WG7,
                    WGDescr = employeeData.WGDescr,
                    WGEffDate = employeeData.WGEffDate
                });

                //var empFileKey = webServices.getEmployeeDetailByFilekey(employee.Filekey);

                //var empBadge = webServices.getEmployeeDetailByBadge(employee.Badge);

            });

            var json = new JavaScriptSerializer().Serialize(listEmp);
         }

    }

    public class Emp
    {
        public int FileKey { get; set; }
        //public int IsCurrent { get; set; }
        //public int PayType { get; set; }
        public double Rate { get; set; }
        public string RateEffDate { get; set; }
        //public int UniqueId { get; set; }
        //public int ActiveStatus { get; set; }
        //public string ActiveStatusConditionEffDate { get; set; }
        //public int ActiveStatusConditionID { get; set; }
        //public string Address1 { get; set; }
        //public string AddressCity { get; set; }
        //public string AddressStateProv { get; set; }
        //public string AddressZIPPC { get; set; }
        public int Badge { get; set; }
        public string BirthDate { get; set; }
        //public int ClockGroupID { get; set; }
        public double CurrentRate { get; set; }
        public string CurrentRateEffDate { get; set; }
        public string DateOfHire { get; set; }
        //public string EMAIL { get; set; }
        //public string ESSPIN { get; set; }
        //public string EmergencyContact { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        //public string FirstName { get; set; }
        public string HourlyStatusEffDate { get; set; }
        //public string LastName { get; set; }
        public string PayClassEffDate { get; set; }
        //public int PayClassID { get; set; }
        public string PayClass { get; set; }
        public string PayTypeEffDate { get; set; }
        public string SchPatternEffDate { get; set; }
        public int WG1 { get; set; }
        public int WG2 { get; set; }
        public int WG3 { get; set; }
        public int WG4 { get; set; }
        public int WG5 { get; set; }
        public int WG6 { get; set; }
        public int WG7 { get; set; }
        public string WGDescr { get; set; }
        public string WGEffDate { get; set; }

    }
}
