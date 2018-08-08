
MetronicApp.factory('dataService', ['$http', 'ApiUrl', '$q', 'storageService', '$rootScope', function ($http, ApiUrl, $q, storageService, $rootScope) {
    var baseUrl = ApiUrl;

    function getCasesSold(filterModel, category, date) {
        var url = 'CasesSold';
        if (filterModel.type == 'month') {
            url += '/CategoryByMonth?filterDate=' + date + '&category=' + (category == null ? '' : category);
        }
        else if (filterModel.type == 'yearToDate') {
            url += '/CategoryByYear?filterDate=' + date;
        }
        else {
            url += '/CategoryByYearToMonth?filterDate=' + date + '&month=' + filterModel.month;
        }

        return $http.get(baseUrl + url);
    };
    function GetCasesSoldRevenueByMonth(filter) {

        return $http.get(baseUrl + 'Dashboard/GetCasesSoldRevenueByMonth?filterId=' + filter.Id);

    };
    
   
   
 
    function FilterOPEXcogsExpensesChart(filter) {

        return $http.get(baseUrl + 'Journal/GetOPEXCOGSJournalChart?filterId=' + filter.Id);

    };
   
    function getMainDashboardProfitabilityChartData(filter) {
        return $http.get(baseUrl + 'Profitability/GetProfitability?filterId=' + filter.Id);
    };
    function GetUserAccess(id) {
        return $http.get(baseUrl + 'AdminUserManagement/GetUserAccess?userId=' + id);
    };
   
   
    function loadcategory(filter) {
        return $http.get(baseUrl + 'Dashboard/GetCategories?filterId=' + filter.Id);
    };
   
    function GetSalesManDashBoardReport(filter) {
        return $http.get(baseUrl + 'Dashboard/GetSalesManDashBoardReport?filterId=' + filter.Id);
    };
   
   
    
  
    function GetExpensesStatistics(filter) {
        return $http.get(baseUrl + 'Expenses/GetExpensesStatistics?filterId=' + filter.Id);
    };
    function getLocalOotstackedColumnChart(filter) {
        return $http.get(baseUrl + 'Dashboard/GetCasesSoldByLocation?filterId=' + filter.Id);
    };
   
  
   
    function GetFiltersForSalesMapping() {
        return $http.get(baseUrl + 'SalesPersonMapping/GetFiltersForSalesMapping');
    };
   
   
    function GetCasesSoldRevenueByYear(dateFilter) {
        return $http.get(baseUrl + 'Dashboard/GetCasesSoldRevenueByYear?dateFilter=' + dateFilter);
    };
    function filterSalesChart(startDate, endDate) {
        return $http.get(baseUrl + 'Sales/SalesChart?startDate=' + startDate + '&endDate=' + endDate);
    };
    function GetAllEmployees() {
        return $http.get(baseUrl + 'Payroll/GetAllEmployees');
    };
    function GetDashboardDriverTripDrillDownReport(params) {
        return $http.post(baseUrl + 'Transportation/GetDashboardDriverTripDrillDownReport', params);
    };
  
    
    function GetPayRollData(data) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "PayRoll/GetAllEmployeePaymentDetails",data, { timeout: promise.promise })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    
    function loadSalesAndCustomerBySalesPerson(startDate, endDate) {
        return $http.get(baseUrl + 'Sales/GetSalesDashboardSalesAndCustomers?startDate=' + startDate + '&endDate=' + endDate);
    };
    function getTargets(isFirstTime) {
        return $http.get(baseUrl + 'Sales/GetTargets?isFirstTime=' + isFirstTime);
    };
    function updateTargets(params) {
        return $http.post(baseUrl + 'Sales/updateTargets', params);
    };
    function getRevenueDashboard(filterModel, category, date) {
        var url = 'Revenue';
        if (filterModel.type == 'month') {
            url += '/CategoryByMonth?filterDate=' + date + '&category=' + (category == null ? '' : category);
        }
        else if (filterModel.type == 'yearToDate') {
            url += '/CategoryByYear?filterDate=' + date;
        }
        else {
            url += '/CategoryByYearToMonth?filterDate=' + date + '&month=' + filterModel.month;
        }

        return $http.get(baseUrl + url);
    };
    function getMainDashboard(date, isSynced) {
        var url = baseUrl + 'Dashboard/Totals?dateFilter=' + date;
        if (isSynced != null)
            url += ('&isSynced=' + isSynced)
        return $http.get(url);
    };
    function getSales(filterModel, category, date, isSynced) {
        var url = 'Sales';
        if (filterModel.type == 'month') {
            url += '/SalesByMonth?dateFilter=' + date;
        }

        if (isSynced != null)
            url += ("&isSynced=" + isSynced);

        return $http.get(baseUrl + url);
    };
    function getApiVersion() {
        return $http.get(baseUrl + 'Dashboard/GetVersion');
    };
    function getRevenueReportFilters() {
        return $http.get(baseUrl + 'Revenue/ReportFilterData');
    };
    function GetGlobalFilters() {

        var promise = $q.defer();

        var request = $http.get(baseUrl + 'Global/GetFilters',

            {
                async:false,
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;
    };


    function GetAllCollectors() {

        var promise = $q.defer();

        var request = $http.get(baseUrl + 'Collector/GetAllCollectors',

            {
                async: false,
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;
    };

    function GetAllUnAssignedCustomerPrefixes() {

        var promise = $q.defer();

        var request = $http.get(baseUrl + 'Collector/GetAllUnAssignedCustomerPrefixes',

            {
                async: false,
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;
    };


    
    function getItemReportDrillDown(reportFilter)
    {

        var promise = $q.defer();

        var request = $http.post(baseUrl + "Item/GetItemReportByItem",
            reportFilter,
            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
   
    function GetCustomerReport(date) {
        var promise = $q.defer();

        var request = $http.get(baseUrl + "Customer/GetCustomerReport?startDate=" + date.currentStartDate,

            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function GetSalesAnalysisReport(data) {
        var promise = $q.defer();

        var request = $http.post(baseUrl + "Sales/GetSalesAnalysisReport",data,

            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function GetSalesAnalysisReportBySalesPerson(data) {
        var promise = $q.defer();

        var request = $http.post(baseUrl + "Sales/GetSalesAnalysisReportBySalesPerson", data,

            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function GetDumpAndDonation(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "Warehouse/GetDumpAndDonation?startDate=" + reportFilters.StartDate + "&endDate=" + reportFilters.EndDate,
            reportFilters,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;

    };
    function GetRevenueCustomerReport(reportFilters) {
        var promise = $q.defer();

        var request = $http.post(baseUrl + "Revenue/GetRevenueCustomerReport",
            reportFilters,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function getRevenueReportData(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "Revenue/GetRevenueReport",
            reportFilters,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function getMailIds() {
        return $http.get(baseUrl + 'Warehouse/GetSOShortReportNotificationEmails');

    };
    function GetItemComparisonReport(reportFilters) {

        var promise = $q.defer();

        var request = $http.post(baseUrl + "Item/GetItemComparisonReport",
            reportFilters,
            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function GetItemVendorReport(reportFilters) {

        var promise = $q.defer();

        var request = $http.post(baseUrl + "Item/GetItemVendorReport",
            reportFilters,
            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function sendNotification(params) {
        return $http.post(baseUrl + "Warehouse/SendShortReportNotification", params);
    };
    function GetCasesSoldReportPrerequisites() {
        return $http.get(baseUrl + 'CasesSoldSales/GetCasesSoldReportPrerequisites');
    };
    function GetAllSalesPersonsForFiltering() {
        return $http.get(baseUrl + 'Salespersonmapping/GetAllSalesPersonsForFiltering');
    };
  
    function getRegularTimeFormat(time) {
        // Return "hh:mm tt" formatted string from "h:mm tt"
        if (time.length < 8)
            time = "0" + time;
        return time;
    }
   
    function generateWarehouseWmsPickerProductivityReportDetails(filters) {

        var model = {
            EmployeID: filters.EmployeID,
            StartDate: filters.startWorkDate,
            EndDate: filters.endWorkDate,
            StartTime: getRegularTimeFormat(filters.startTime),
            EndTime: getRegularTimeFormat(filters.endTime)
        };

        var promise = $q.defer();
        var request = $http.post(baseUrl + 'Warehouse/PickerProductivityReportDetails', model,
        {
            timeout: promise.promise
        })
        .then(getReportDataCompleted)
        .catch(getReportDataFailed);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataCompleted(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    function generateWarehouseWmsPickerProductivityReportDetailsForWareHouse(filters) {

        var model = {
            EmployeID: filters.EmployeID,
            StartDate: filters.startWorkDate,
            EndDate: filters.endWorkDate,
            StartTime: getRegularTimeFormat(filters.startTime),
            EndTime: getRegularTimeFormat(filters.endTime)
        };

        var promise = $q.defer();
        var request = $http.post(baseUrl + 'Warehouse/GetPickerProductivityDayDetailedReport', model,
        {
            timeout: promise.promise
        })
        .then(getReportDataCompleted)
        .catch(getReportDataFailed);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataCompleted(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    function requestToken(username, pswd) {
        var data = "grant_type=password&username=" + username + "&password=" + encodeURIComponent(pswd);
        return $http.post(baseUrl + "token", data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } });
    }
    function getReleventItemsFilter(inputSearch) {
        return $http.get(baseUrl + 'Revenue/GetRevenueItemSearch?inputSearch=' + inputSearch);
    };
    function GetPickerProductivityDayReport(filter) {
        return $http.post(baseUrl + 'Warehouse/GetPickerProductivityDayReport', filter);
    };
    
    function filterWarehouseChart(startDate, endDate) {
        return $http.get(baseUrl + 'Warehouse/PickerProductivityChart?startDate=' + startDate + '&endDate=' + endDate);
    };
    function filterProfitabilityChart(startDate, endDate) {
        return $http.get(baseUrl + 'Profitability/GetProfitability?beginYear=' + startDate + '&endYear=' + endDate);
    };
   
    function sendIgnoredreason(params) {
        return $http.post(baseUrl + "Warehouse/SendShortReportIgnoredReason", params);
    };
    function filterMeterGaugeChart(startDate, endDate) {
        return $http.get(baseUrl + 'url?startDate=' + startDate + '&endDate=' + endDate);
    };
    function generateWarehouseNotifiedIgnoredShortReport(filters) {
        var promise = $q.defer();
        var request = $http.get(baseUrl + 'Warehouse/GetNotifiedIgnoredShortReports?routeNumber=' + filters.routeNumber + '&buyerId=' + filters.buyerId + '&shipDate=' + filters.shipDate,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    }
   
    function createUser(model) {
        return $http.post(baseUrl + 'Account/Register', model);
    };
    
   
    
   
    function changePassword(data) {
        return $http.post(baseUrl + "Account/ChangePassword", data);
    };
   
    function getCommonDashboardSalesData(date, isSynced) {
        var url = baseUrl + 'Sales/TotalByMonth?dateFilter=' + date;
        if (isSynced != null)
            url += ('&isSynced=' + isSynced)
        return $http.get(url);
    };
    function getCommonDashboardCasesSoldData(date) {
        var url = baseUrl + 'CasesSold/CategoryByMonth?filterDate=' + date + '&category=';
        return $http.get(url);
    };
    function getCommonDashboardRevenueData(date) {
        var url = baseUrl + 'Revenue/CategoryByMonth?filterDate=' + date + '&category=';
        return $http.get(url);
    };
    function getCommonDashboardExpensesData(date) {
        var url = baseUrl + 'Expenses/TotalByMonth?filterDate=' + date;
        return $http.get(url);
    };
    function getCommonDashboardStatiticsData(date) {
        var url = baseUrl + 'Dashboard/Totals?dateFilter=' + date;
        return $http.get(url);
    };
    function getCommonDashboardStatiticsByFilter(Id) {
        var url = baseUrl + 'Dashboard/Totals?filterId=' + Id;
        return $http.get(url);
    };
    function filterSalesBySalesPersonChart(startDate, endDate, isSynced) {
        var url = baseUrl + 'Sales/SalesBySalesPerson?startDate=' + startDate + '&endDate=' + endDate;
        if (isSynced != null)
            url += ("&isSynced=" + isSynced);
        return $http.get(url);
    };
    function filterCustomersBySalesPersonChart(startDate, endDate, isSynced) {
        var url = baseUrl + 'Sales/CustomerBySalesPerson?startDate=' + startDate + '&endDate=' + endDate;
        if (isSynced != null)
            url += ("&isSynced=" + isSynced);
        return $http.get(url);
    };
   
    function getItemReportFilters() {
        return $http.get(baseUrl + 'Item/GetAllItems');
    };
   
   
    function GetRevenueCustomerReport(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "Revenue/GetRevenueCustomerReport", reportFilters,
         {
             timeout: promise.promise
         })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;

    }
    function addSalesPersons(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "SalesPersonMapping/AddMappedSalesPersons", reportFilters,
         {
             timeout: promise.promise
         })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
           // promise.resolve();
            promise.resolve('cancelled!');
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function unAssignSalesPersons(params) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "SalesPersonMapping/UnAssignSalesPersons?salesPersonCode=" + params.salesPersonCode + "&assignSalesPersonCode=" + params.assignSalesPersonCode + "&endDate=" + params.unAssignLoadDate,
         {
             timeout: promise.promise
         })
        .then(getDataComplete)
        .catch(getDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getDataComplete(response) {
            return response;
        };

        function getDataFailed(error) {
            $q.reject();
        };

        return request;
    };
  
  
    function GetSalesPersonCustomersDataOfSalesReport(salesPerson, startDate, endDate) {
        var params = {
            SalesPerson: salesPerson,
            StartDateCurrent: startDate,
            EndDateCurrent: endDate
        };
        return $http.post(baseUrl + 'Sales/GetCustomerAndSales/', params);
    };
   
    function GetSalesPersonCustomersDataCategories(requestData) {
        return $http.post(baseUrl + 'Sales/GetCustomerAndSales/', requestData);
    };
    //function GetProfitByCustomerDetailAndCommodity(requestedData) {
    //    return $http.post(baseUrl + 'Profitability/GetProfitByCustomerDetailAndCommodity/', requestData);
    //}
   
  
   
    function GetCasesSoldDetails(requestData,isCM01) {
        if (isCM01) {
            return $http.post(baseUrl + 'Sales/GetInvoiceDetailsByCustomerForCustomerService/', requestData);
        }
        else {
            return $http.post(baseUrl + 'Sales/GetCasesSoldDetails/', requestData);
        }
    };
   
   
    function FilterJournalExpensesChart(startDate, endDate) {
        return $http.get(baseUrl + 'Journal/GetAPJournalChart?startDate=' + startDate + '&endDate=' + endDate);
    };
    function FilterOPEXJournalExpensesChart(startDate, endDate) {
        return $http.get(baseUrl + 'Journal/GetAPOPEXJournalChart?startDate=' + startDate + '&endDate=' + endDate);
    };
    function GetItems(itemname) {
        return $http.get(baseUrl + 'Item/GetItems?item=' + itemname);
    };
    function ForgetPassword(params) {
        return $http.post(baseUrl + "Account/ForgotPassword", params);
    };
    function GetSalesOrderWithNoBin(data) {

        var promise = $q.defer();
        var request = $http.post(baseUrl + "Warehouse/GetSalesOrderWithNoBin/",data,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;

    };
  
   
   
  
   
    function GetcasesSoldMap(data) {
        return $http.get(baseUrl + 'Dashboard/GetCasesSoldByMap?filterId=' + data.Id);
    };
    function GetRevenueMap(data) {
        return $http.get(baseUrl + 'Dashboard/GetRevenueByMap?filterId=' + data.Id);
    };
   
    function NogalesCurrentFilter() {
        var filter = storageService.getStorage("CurrentFilter");
        if(filter==false){

        }
        else{
            return filter.data.Id;
        }
    };
    function getWarehouseDashboardData() {
        //  return $http.get(baseUrl + '/api/Revenue/ReportFilterData');
    };
    function GetCustomerAndSalesReportWithoutFilter(requestData) {
        return $http.post(baseUrl + 'Sales/GetCustomerAndSalesReportWithoutFilter/', requestData);
    };
    function GetCustomerAndSalesReportForAnalysis(requestData) {
        return $http.post(baseUrl + 'Sales/GetCustomerAndSalesReportForAnalysis/', requestData);
    };
   
    function GetSalesMarginDetailedReport(requestData)
    {
        return $http.post(baseUrl + 'Sales/GetSalesMarginDetailedReport/', requestData);
    }
    function PendingRequest() {
        if (!window.gettotal) {

            $rootScope.total = $http.pendingRequests.length;
            window.gettotal = true;
            // console.log('%c Request Remaining "' + $http.pendingRequests.length + '" ', 'background: blue; color: #bada55');

        }
        else {

            var a = $http.pendingRequests.length;
     
            var c = a / $rootScope.total;
            c = c * 100;


            var statusloading = $rootScope.total - $http.pendingRequests.length;

            if (statusloading > 0) {
                Metronic.blockUI({ boxed: true, message: 'LOADING...' + ($rootScope.total - $http.pendingRequests.length) + " of " + $rootScope.total });
            }
            else {
                Metronic.blockUI({ boxed: true, message: 'LOADING...'});
            }
         
        
        }

        if ($http.pendingRequests.length == 0) {
          
            window.gettotal = false;
            //console.log('%c Loading Completed unblocking the UI Loader" ', 'background: gray; color: #bada55');
            Metronic.unblockUI();
        }



       };
    function IsGlobal_Filter_Year() {


       var globalfilter = $rootScope.currentfilter;

       var val = (globalfilter.Periods.Prior.Label == '') ? true : false;

       return val;
   };
  
   
   
   
   
    function GetCasesSoldAndRevenueData(filterId) {
       return $http.get(baseUrl + "Dashboard/GetCasesSoldAndRevenueData?filterId=" + filterId);
    };
    function GetCustomerWiseReportOfSalesPerson(requestData) {
        return $http.post(baseUrl + 'CasesSoldSales/GetCustomerWiseReportOfSalesPerson/', requestData);
    };

    //ok
    function GetDashboardStatistics(filter) {
       return $http.get(baseUrl + "Dashboard/GetDashboardStatistics?filterId=" + filter.Id);
    };
    function GetOpexTopBoxValues(filter) {
        return $http.get(baseUrl + "Dashboard/GetOpexTopBoxValues?filterId=" + filter.Id);
    };
    function GetProfitTopBoxValues(filter) {
        return $http.get(baseUrl + "Dashboard/GetProfitTopBoxValues?filterId=" + filter.Id);
    };
    function GetNonCommoditySalesAndCasesSoldTopBoxValues(filter) {
        return $http.get(baseUrl + "Dashboard/GetNonCommoditySalesAndCasesSoldTopBoxValues?filterId=" + filter.Id);
    };
    function GetCasesSoldAndSalesDasboardData(filter) {
        return $http.get(baseUrl + 'CasesSoldSales/GetCasesSoldAndSalesDasboardData?filterId=' + filter.Id);
    };
    function GetDashboardCollectionInfoData(filter) {
        return $http.get(baseUrl + 'Finance/GetDashboardCollectionInfoData?filterId=' + filter.Id);
    };
    function GetSalesCasesSoldMap(data) {
        return $http.get(baseUrl + 'CasesSoldSales/GetSalesCasesSoldMap?filterId=' + data.Id);
    };
    function GetCasesSoldAndGrowthBySalesPerson(filter) {
        return $http.get(baseUrl + 'CasesSold/GetCasesSoldAndGrowthBySalesPerson?filterId=' + filter.Id);
    };
    function GetCasesSoldAndGrowthByCustomer(filter) {
        return $http.get(baseUrl + 'CasesSold/GetCasesSoldAndGrowthByCustomer?filterId=' + filter.Id);
    };
    function GetSalesPersonCustomersDataTotal(requestData, iscm01) {
        if (iscm01) {
            return $http.post(baseUrl + 'CasesSoldSales/GetCustomerServiceReportBySalesman/', requestData);
        }
        else {
            return $http.post(baseUrl + 'CasesSold/GetCustomerAndCasessoldReport/', requestData);
        }
    };
    function GetCustomerServiceDetails(filterId, state) {
        return (state == "profitability") ? $http.get(baseUrl + 'Profitability/GetCustomerServiceDetails?filterId=' + filterId) :
               (state == "expenses") ? $http.get(baseUrl + 'Expenses/GetCustomerServiceExpenseChart?filterId=' + filterId) :
                                             $http.get(baseUrl + 'CasesSoldSales/GetCustomerServiceDetails?filterId=' + filterId)
    };
    function GetCategories(reportFilters) {
        var promise = $q.defer();
        var request = $http.get(baseUrl + "Masters/Categories"
            )
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;

    };


    function GetTransportationDriverReport(param) {
        var promise = $q.defer();
        var request = $http.get(baseUrl + "Transportation/GetDriverTripConsolidatedReport?startDate=" + param.StartDate + "&endDate=" + param.EndDate);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
       
    };

    function GetDriverDayAndDetailedReport(param) {
        var promise = $q.defer();
        var request = $http.get(baseUrl + "Transportation/GetDriverDayAndDetailedReport?startDate=" + param.StartDate + "&endDate=" + param.EndDate + "&driverCode=" + param.driverCode + "&route=" + param.route);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;

    };


    function GetRouteConsolidatedReport(param) {
        debugger;
        var promise = $q.defer();
        var request = $http.get(baseUrl + "Transportation/GetRouteConsolidatedReport?startDate=" + param.StartDate + "&endDate=" + param.EndDate );
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;

    };




    function GetSalesAndGrowthBySalesPerson(filter) {
        return $http.get(baseUrl + 'Sales/GetSalesAndGrowthBySalesPerson?filterId=' + filter.Id);
    };
    function GetSalesAndGrowthByCustomer(filter) {
        return $http.get(baseUrl + 'Sales/GetSalesAndGrowthByCustomer?filterId=' + filter.Id);
    };
    function GetCustomerAndCasessoldReport(requestData, ISCm01) {
        if (ISCm01 == "CM01") {
            return $http.post(baseUrl + 'CasesSoldSales/GetCustomerServiceReportBySalesman/', requestData);
        }
        else {
            return $http.post(baseUrl + 'CasesSold/GetCustomerAndCasessoldReport/', requestData);
        }
    };
    function getSalesReportData(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "CasesSoldSales/GetSalesReportofSalesPerson?startDate=" + reportFilters.StartDate +
            "&endDate=" + reportFilters.EndDate,
            reportFilters.SalesPerson,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    function GetSalesAnalysisReportofSalesPerson(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "Sales/GetSalesAnalysisReportofSalesPerson?startDate=" + reportFilters.StartDate +
            "&endDate=" + reportFilters.EndDate,
            reportFilters.SalesPerson,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function GetSalesPersonMarginReport(requestData) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + 'Sales/GetSalesMarginReport/', requestData,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }
        return request;
    }
    function GetProfitablityDashboardBarChartData(filter) {
        return $http.post(baseUrl + 'Profitability/GetProfitablityDashboardBarChartData?filterId=' + filter.Id);
    };
    function GetMargin(filter, filterCustomerData, filterItemData) {
        return $http.get(baseUrl + 'Profitability/GetMargin?filterCustomerData=' + filterCustomerData + '&filterItemData=' + filterItemData + '&filterId=' + filter.Id);
    };
    function GetItemMarginReport(data) {
        return $http.get(baseUrl + 'Profitability/GetItemMarginReport?filterId=' + data.filterId + '&period=' + data.Period + '&itemCode=' + data.itemCode);
    };
    function GetCustomerMarginReport(data) {
        return $http.get(baseUrl + 'Profitability/GetCustomerMarginReport?filterId=' + data.filterId + '&period=' + data.Period + '&customerCode=' + data.itemCode);
    };
    function GetMarginByDifferennce(filter, isCustomer, filterData) {
        return $http.get(baseUrl + 'Profitability/GetMarginByDifference?isCustomer=' + isCustomer + '&filterData=' + filterData + '&filterId=' + filter.Id);
    };
    function LoadGetProfitabilityByCustomer(id) {
        return $http.get(baseUrl + 'Profitability/GetProfitByCustomer?filterId=' + id);
    };
    function LoadGetProfitabilityByItem(id) {
        return $http.get(baseUrl + 'Profitability/GetProfitByItem?filterId=' + id);
    };
    function GetProfitByCustomerDetailAndCommodity(requestData) {
        return $http.post(baseUrl + 'Profitability/GetProfitByCustomerDetailAndCommodity/', requestData);
    };
    function GetCustomerWiseProfitByItem(filterId, itemCode) {
        return $http.get(baseUrl + "Profitability/GetCustomerWiseProfitByItem?filterId=" + filterId + "&itemCode=" + itemCode);
    };
    function getForcastResult(startDate, endDate) {
        return $http.get(baseUrl + 'Warehouse/PickerForcastReport?&startDate=' + startDate + '&endDate=' + endDate);
    };
    function getForcastReport(startDate, endDate) {
        return $http.get(baseUrl + 'Warehouse/PickerForcastQuantityPickedReport?startDate=' + startDate + '&endDate=' + endDate);
    };
    function getMainDashboardPickerProductivityChartData(filter) {
        return $http.get(baseUrl + 'Warehouse/PickerProductivityChart?filterId=' + filter.Id);
    };
    function generateWarehouseShortReport(filters) {
        var promise = $q.defer();
        var request = $http.get(baseUrl + 'Warehouse/GetShortReport?routeNumber=' + filters.routeNumber + '&buyerId=' + filters.buyerId + '&shipDate=' + filters.shipDate,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function generateWarehouseWmsPickerProductivityReport(filters) {
        var model = {
            EmployeID: filters.userId,
            StartDate: filters.startWorkDate,
            EndDate: filters.endWorkDate,
            StartTime: getRegularTimeFormat(filters.startTime),
            EndTime: getRegularTimeFormat(filters.endTime)
        };
        var promise = $q.defer();
        var request = $http.post(baseUrl + 'Warehouse/PickerProductivityReport', model,
        {
            timeout: promise.promise
        })
        .then(getReportDataCompleted)
        .catch(getReportDataFailed);
        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataCompleted(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    function getItemReportData(reportFilters) {
        var promise = $q.defer();

        var request = $http.post(baseUrl + "Item/GetItemReport",
            reportFilters,
            {

                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        }

        function getReportDataComplete(response) {
            return response;
        }

        function getReportDataFailed(error) {
            $q.reject();
        }

        return request;

    };
    function getAllUsers() {
        return $http.get(baseUrl + "Account/GetAllUsers");
    };
    function GetAllModules(filter) {
        return $http.get(baseUrl + 'AdminUserManagement/GetAllModules');
    };
    function GetAllSalesPersonCategories(filter) {
        return $http.get(baseUrl + 'AdminUserManagement/GetAllSalesPersonCategories');
    };
    function GetUserDetails(id) {
        return $http.get(baseUrl + 'AdminUserManagement/GetUserDetails?userId=' + id);
    };
    function deleteUser(userId) {
        var param = "userId=" + userId;
        return $http.post(baseUrl + "Account/DeleteUser/" + userId, null, {
            headers: {
                'Content-Type': "application/x-www-form-urlencoded"
            }
        });
    };
    function updateUser(model) {
        return $http.post(baseUrl + "Account/UpdateUser", model);
    };
    function resetPassword(data) {
        if (data.UserId && data.UserId.length > 0)
            return $http.post(baseUrl + "Account/ResetPasswordByAdmin", data);
        return $http.post(baseUrl + "Account/ResetPassword", data);
    };
    function loadAllSalesPerson() {
        var promise = $q.defer();
        var request = $http.get(baseUrl + "SalesPersonMapping/GetMappedSalesPersons",
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };

        function getReportDataComplete(response) {
            return response;
        };

        function getReportDataFailed(error) {
            $q.reject();
        };

        return request;
    };
    function getArchievedSalesPersonMapping(requestData) {
        return $http.post(baseUrl + 'SalesPersonMapping/ArchivedSalesPersons', requestData);
    };
    function getSalesReportFilters() {
        return $http.get(baseUrl + 'Sales/GetAllSalesPerson');
    };
    function AssignSalesPersons(paramS) {
        return $http.post(baseUrl + 'SalesPersonMapping/AssignSalesPersons', paramS);
    };
    function UpdateSalesPersonDescription(params) {
        return $http.post(baseUrl + 'SalesPersonMapping/UpdateSalesPersonDescription/', params);
    };
    function GetCasesSoldAndGrowthBySalesPersonCustomerService(filterId, state) {
        return $http.get(baseUrl + 'CasesSold/GetCasesSoldAndGrowthBySalesPersonCustomerService?filterId=' + filterId + "&isCasesSold=" + state);
    };
    function GetExpensesPersonCustomersData(requestData) {
        return $http.post(baseUrl + 'Expenses/GetOPEXCOGSExpenseReport/', requestData);
    };
    function filterAdminExpensesChart(filter) {
        return $http.get(baseUrl + 'Expenses/TopTenAdminExpenses?filterId=' + filter.Id);
    };
    function GetJournalReportData(reportFilters) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + "Journal/GetJournalReport",
            reportFilters,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);

        request.abortCall = function () {
            promise.resolve();
        };
        function getReportDataComplete(response) {
            return response;
        };
        function getReportDataFailed(error) {
            $q.reject();
        };
        return request;
    };
    function GetComodityExpenseReport(params) {
        return $http.get(baseUrl + 'Expenses/GetComodityExpenseReport?comodity=' + params.comodity + '&startDate=' + params.startDate + '&endDate=' + params.endDate);
    };
    function LoadExpenseChart(filter) {
        return $http.get(baseUrl + 'Expenses/LoadExpenseChart?filterId=' + filter.Id);
    };
    function AddCollector(collectorName) {
        return $http.post(baseUrl + 'Collector/AddCollector?collectorName=' + collectorName);
    };
    function DeleteCollector(collectorId) {
        return $http.delete(baseUrl + 'Collector/DeleteCollector?collectorId=' + collectorId);
    };
    
    function UpdateCollector(collector) {
         return $http.put(baseUrl + 'Collector/UpdateCollector/' , collector);
        };
    
    function GetExpenseReportBySalesman(requestData, iscm01) {
        requestData["IsCM01"] = iscm01;
        return $http.post(baseUrl + 'Expenses/GetExpenseReportBySalesman/', requestData);
    };
    function AssignUnAssignCustomerPrefix(requestData) {
        return $http.put(baseUrl + 'Collector/AssignUnAssignCustomerPrefix/', requestData);
    };
    
    function GetCollectorsReport(requestData) {
       
        return $http.post(baseUrl + 'Finance/GetCollectorsReport/', requestData);
    };

    
    function GetExpenseInvoiceDetailsByCustomer(requestData, isCM01) {
        requestData["IsCM01"] = isCM01;
        return $http.post(baseUrl + 'Expenses/GetExpenseInvoiceDetailsByCustomer/', requestData);
    };
    function GetDashboardDiverTripData(filter) {
        return $http.get(baseUrl + 'Transportation/GetDashboardDiverTripData?filterId=' + filter.Id);
    };
    function SaveCategories(params) {
        return  $http.post(baseUrl + "Masters/Categories/",params);
    };
    return {
        PendingRequest: PendingRequest,
        IsGlobal_Filter_Year:IsGlobal_Filter_Year,
        SaveCategories:SaveCategories,
        //loadcategory: loadcategory,
        //GetCasesSoldRevenueByMonth: GetCasesSoldRevenueByMonth,
        //FilterOPEXcogsExpensesChart: FilterOPEXcogsExpensesChart,
        //GetSalesManDashBoardReport: GetSalesManDashBoardReport, //sales dashboard api
       
      
        //getLocalOotstackedColumnChart: getLocalOotstackedColumnChart,
    
        getMainDashboardProfitabilityChartData: getMainDashboardProfitabilityChartData,
      
        //GetCommonDashboardStatiticsData: getCommonDashboardStatiticsData,
        //getCommonDashboardStatiticsByFilter:getCommonDashboardStatiticsByFilter,
        GetSalesOrderWithNoBin:GetSalesOrderWithNoBin,
        //GetcasesSoldMap: GetcasesSoldMap,
        //GetRevenueMap: GetRevenueMap,
      
        //GetCasesSold: getCasesSold,
        //getMainDashboardData: getMainDashboard,
        //GetRevenueDashboard: getRevenueDashboard,
        GetApiVersion: getApiVersion,
        //GetSales: getSales,
        //FilterSalesChart: filterSalesChart,
        //getRevenueReportFilters: getRevenueReportFilters,
        //getRevenueReportData: getRevenueReportData,
        //GetTargets: getTargets,
        //UpdateTargets: updateTargets,
        //getWarehouseDashboardData: getWarehouseDashboardData,
        GetMailIds: getMailIds,
        SendNotification: sendNotification,
       
      
        GenerateWarehouseWmsPickerProductivityReportDetails:generateWarehouseWmsPickerProductivityReportDetails,
      
        //GetReleventItemsFilter: getReleventItemsFilter,
        //FilterWarehouseChart: filterWarehouseChart,
        //FilterProfitabilityChart: filterProfitabilityChart,
      
        //SendIgnoredreason: sendIgnoredreason,
       
        //FilterMeterGaugeChart: filterMeterGaugeChart,
        GenerateWarehouseNotifiedIgnoredShortReport: generateWarehouseNotifiedIgnoredShortReport,
       
        RequestToken: requestToken,
        CreateUser: createUser,
      
     
     
      
     //   GetForcastReport: getForcastReport,
    
      
        ChangePassword: changePassword,
        GetCommonDashboardSalesData: getCommonDashboardSalesData,
        GetCommonDashboardCasesSoldData: getCommonDashboardCasesSoldData,
        GetCommonDashboardRevenueData: getCommonDashboardRevenueData,
        GetCommonDashboardExpensesData: getCommonDashboardExpensesData,
        FilterSalesBySalesPersonChart: filterSalesBySalesPersonChart,
        FilterCustomersBySalesPersonChart: filterCustomersBySalesPersonChart,
       
       
      
      
        GetCasesSoldDetails:GetCasesSoldDetails,
        GetSalesPersonCustomersDataCategories:GetSalesPersonCustomersDataCategories,
        FilterJournalExpensesChart: FilterJournalExpensesChart,
        FilterOPEXJournalExpensesChart: FilterOPEXJournalExpensesChart,
        ForgetPassword: ForgetPassword,
        GetSalesPersonCustomersDataOfSalesReport: GetSalesPersonCustomersDataOfSalesReport,
      
     
        addSalesPersons: addSalesPersons,
        unAssignSalesPersons: unAssignSalesPersons,
      
        GetCasesSoldReportPrerequisites: GetCasesSoldReportPrerequisites,
       
        loadSalesAndCustomerBySalesPerson: loadSalesAndCustomerBySalesPerson,
        GetDumpAndDonation:GetDumpAndDonation,
        GetCasesSoldRevenueByYear: GetCasesSoldRevenueByYear,
        GetCustomerAndSalesReportForAnalysis:GetCustomerAndSalesReportForAnalysis,
      
        GetRevenueCustomerReport: GetRevenueCustomerReport,
        GetAllSalesPersonsForFiltering: GetAllSalesPersonsForFiltering,
        GetExpensesStatistics: GetExpensesStatistics,
        GetFiltersForSalesMapping: GetFiltersForSalesMapping,
       
     
       
       
       
        GetRevenueCustomerReport: GetRevenueCustomerReport,
        GetGlobalFilters: GetGlobalFilters,
        GetAllCollectors: GetAllCollectors,
        GetAllUnAssignedCustomerPrefixes: GetAllUnAssignedCustomerPrefixes,
        AssignUnAssignCustomerPrefix:AssignUnAssignCustomerPrefix,
        NogalesCurrentFilter: NogalesCurrentFilter,
       
        getItemReportDrillDown:getItemReportDrillDown,
        getItemReportFilters: getItemReportFilters,
        GetCustomerAndSalesReportWithoutFilter: GetCustomerAndSalesReportWithoutFilter,
        GetItemComparisonReport: GetItemComparisonReport,
       
        GetItemVendorReport: GetItemVendorReport,
        GetItems: GetItems,
        GetCustomerReport: GetCustomerReport,
        GetSalesAnalysisReport: GetSalesAnalysisReport,
        GetSalesAnalysisReportBySalesPerson: GetSalesAnalysisReportBySalesPerson,
      
        GetSalesMarginDetailedReport: GetSalesMarginDetailedReport,
        
        
      
      
       
       
       
        GetCasesSoldAndRevenueData: GetCasesSoldAndRevenueData,
      
       
      
        GetCustomerWiseReportOfSalesPerson: GetCustomerWiseReportOfSalesPerson,
       
       
      
        GetUserAccess: GetUserAccess,
        GetPayRollData: GetPayRollData,
        GetAllEmployees: GetAllEmployees,
       
       
       
      
       


        //ok
        GetDashboardStatistics: GetDashboardStatistics,
        GetOpexTopBoxValues: GetOpexTopBoxValues,
        GetProfitTopBoxValues: GetProfitTopBoxValues,
        GetNonCommoditySalesAndCasesSoldTopBoxValues: GetNonCommoditySalesAndCasesSoldTopBoxValues,
        GetCasesSoldAndSalesDasboardData: GetCasesSoldAndSalesDasboardData,
        GetSalesCasesSoldMap: GetSalesCasesSoldMap,
        GetCasesSoldAndGrowthBySalesPerson: GetCasesSoldAndGrowthBySalesPerson,
        GetCasesSoldAndGrowthByCustomer: GetCasesSoldAndGrowthByCustomer,
        GetSalesPersonCustomersDataTotal: GetSalesPersonCustomersDataTotal,
        GetCustomerServiceDetails: GetCustomerServiceDetails,
        GetCategories: GetCategories,
        GetSalesAndGrowthBySalesPerson: GetSalesAndGrowthBySalesPerson,
        GetSalesAndGrowthByCustomer: GetSalesAndGrowthByCustomer,
        GetCustomerAndCasessoldReport: GetCustomerAndCasessoldReport,
        getSalesReportData: getSalesReportData,
        GetSalesAnalysisReportofSalesPerson: GetSalesAnalysisReportofSalesPerson,
        GetSalesPersonMarginReport: GetSalesPersonMarginReport,
        GetProfitablityDashboardBarChartData: GetProfitablityDashboardBarChartData,
        GetMargin: GetMargin,
        GetItemMarginReport: GetItemMarginReport,
        GetCustomerMarginReport: GetCustomerMarginReport,
        GetMarginByDifferennce: GetMarginByDifferennce,
        LoadGetProfitabilityByCustomer: LoadGetProfitabilityByCustomer,
        LoadGetProfitabilityByItem: LoadGetProfitabilityByItem,
        GetProfitByCustomerDetailAndCommodity: GetProfitByCustomerDetailAndCommodity,
        GetCustomerWiseProfitByItem: GetCustomerWiseProfitByItem,
        getForcastResult: getForcastResult,
        GetForcastReport: getForcastReport,
        getMainDashboardPickerProductivityChartData: getMainDashboardPickerProductivityChartData,
        GenerateWarehouseShortReport: generateWarehouseShortReport,
        GenerateWarehouseWmsPickerProductivityReport: generateWarehouseWmsPickerProductivityReport,
        getItemReportData: getItemReportData,
        GetAllUsers: getAllUsers,
        GetAllModules: GetAllModules,
        GetAllSalesPersonCategories: GetAllSalesPersonCategories,
        GetUserDetails: GetUserDetails,
        UpdateUser: updateUser,
        DeleteUser: deleteUser,
        ResetPassword: resetPassword,
        loadAllSalesPerson: loadAllSalesPerson,
        getArchievedSalesPersonMapping: getArchievedSalesPersonMapping,
        getSalesReportFilters: getSalesReportFilters,
        AssignSalesPersons: AssignSalesPersons,
        UpdateSalesPersonDescription: UpdateSalesPersonDescription,
        GetCasesSoldAndGrowthBySalesPersonCustomerService: GetCasesSoldAndGrowthBySalesPersonCustomerService,
        GetExpensesPersonCustomersData: GetExpensesPersonCustomersData,
        FilterAdminExpensesChart: filterAdminExpensesChart,
        GetJournalReportData: GetJournalReportData,
        GetComodityExpenseReport: GetComodityExpenseReport,
        LoadExpenseChart: LoadExpenseChart,
        GetExpenseReportBySalesman: GetExpenseReportBySalesman,
        GetExpenseInvoiceDetailsByCustomer: GetExpenseInvoiceDetailsByCustomer,
        FilterOPEXcogsExpensesChart: FilterOPEXcogsExpensesChart,
        GetDashboardDiverTripData: GetDashboardDiverTripData,
        GetDashboardDriverTripDrillDownReport: GetDashboardDriverTripDrillDownReport,
     
        GetTransportationDriverReport: GetTransportationDriverReport,
        GetDriverDayAndDetailedReport: GetDriverDayAndDetailedReport,
        GetRouteConsolidatedReport: GetRouteConsolidatedReport,
        GetPickerProductivityDayReport: GetPickerProductivityDayReport,
        generateWarehouseWmsPickerProductivityReportDetailsForWareHouse: generateWarehouseWmsPickerProductivityReportDetailsForWareHouse,
        GetDashboardCollectionInfoData: GetDashboardCollectionInfoData,
        GetCollectorsReport: GetCollectorsReport,
        AddCollector: AddCollector,
        UpdateCollector: UpdateCollector,
        DeleteCollector: DeleteCollector
    }
}]);
//1385