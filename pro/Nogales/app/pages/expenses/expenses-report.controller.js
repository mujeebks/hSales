'use strict';
MetronicApp.controller('ExpensesReportController', ['$scope', 'dataService', '$stateParams', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope, dataService, $stateParams, $filter, NotificationService, ApiUrl, commonService, $controller) {

        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $controller('BaseController', { $scope: $scope });
        $scope.searchClicked = false;

        $scope.filter = {};
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');

        var apChartTitles = ["Operational Expenses", "Costs to Good Sold", "OPEX And COGS"];

        $scope.glAccounts = [
                     {
                         Key: 0,
                         Value: "OPEX- Operational Expenses"
                     },
                     {
                         Key: 1,
                         Value: "COGS- Costs to Good Sold"
                     },
                     {
                         Key: 2,
                         Value: "All"
                     }
        ];
        $scope.filter.glAccount = $scope.glAccounts[0].Key;


        $scope.apChartTitle = apChartTitles[0];


        $scope.shipmentsMaster = [];

        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.profiles = [];
        $scope.profilesMaster = [];
        $scope.groupProperty = '';

        $scope.loadDatePickers = function () {
            $('#scheduledLoadStartDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
            $('#scheduledLoadEndDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
        }
        $scope.loadDatePickers();

        initFilter();
        $scope.showDefaultReport = true;

        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }
        $scope.search = function (filter) {
            $scope.searchClicked = true;
            $scope.iscanceldisabled = false;
            var status = false;
            var date = new Date();
            var firstDay;
            var lastDay;
            if (filter == 'ThisMonth') {
                firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            }
            else if (filter == 'LastMonth') {
                var month = date.getMonth();
                var year;
                if (month < 12)
                    year = date.getFullYear();
                else
                    year = date.getFullYear() - 1;
                firstDay = new Date(year, month - 1, 1);
                lastDay = new Date(year, month, 0);
            }
            else if (filter == 'ThisQuarter') {
                var currQuarter = getQuarter(date);
                firstDay = new Date(date.getFullYear(), 3 * currQuarter - 3, 1);
                lastDay = new Date(date.getFullYear(), 3 * currQuarter, 0);
            }
            else if (filter == 'LastQuarter') {
                var year;
                var quarter = getQuarter(date) - 1;
                if (quarter > 0) {
                    year = date.getFullYear();
                    firstDay = new Date(year, 3 * quarter - 3, 1);
                    lastDay = new Date(year, 3 * quarter, 0);
                }
                else {
                    year = date.getFullYear() - 1;
                    firstDay = new Date(year, 9, 1);
                    lastDay = new Date(year, 12, 0);
                }
            }
            else if (filter == 'YTD') {
                firstDay = new Date(date.getFullYear(), 0, 1);
                lastDay = new Date();
            }
            else if (filter == 'LastYear') {
                firstDay = new Date(date.getFullYear() - 1, 0, 1);
                lastDay = new Date(date.getFullYear(), 0, 0);
            }
            else {
                firstDay = $scope.beginDate;
                lastDay = $scope.endDate;
            }

            $scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');

            status = (!$scope.beginDate || !$scope.endDate) ?
                   true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);
            if (!status && ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {

                $scope.apChartTitle = apChartTitles[$scope.filter.glAccount];

                $scope.btnSpinner.start();
                var param = {
                    "StartDate"         : $scope.beginDate,
                    "EndDate"           : $scope.endDate,
                    "AccountNumberStart": $scope.filter.accountNumberStart,
                    "AccountNumberEnd"  : $scope.filter.accountNumberEnd,
                    "GLAccounTtype"     : $scope.filter.glAccount,
                    "VendorNumber"      : $scope.filter.vendor,
                    "InvoiceNumber"     : $scope.filter.invoiceNumber,
                    "StartSession"      : $scope.filter.stratSession,
                    "EndSession"        : $scope.filter.endSession,
                    "StartBatch"        : $scope.filter.startBatch,
                    "EndBatch"          : $scope.filter.endBatch,
                    "TransactionNumber" : $scope.filter.transactionNo,
                    "ARType"            : ($scope.selectedArTypes)?getArTypeAsCommaSeparatedString($scope.selectedArTypes):"",
                    "CustomerNumber"    : $scope.filter.customerNo,
                    //"JournalType"       : ($scope.showAr) ? "AR" : ($scope.showAp) ? "AP" : "IC",
                    "JournalType": "AP",
                };


                (httpRequest = dataService.GetJournalReportData(param))
                    .then(function (response) {
                        if (response) {
                            $scope.iscanceldisabled = true;
                            $scope.TotalAmount = 0;
                            if (response.data == 204) {
                                $scope.ApJournalReport = [];
                                $scope.ApJournalReportMaster = [];
                            }
                            else {
                                $scope.ApJournalReport = response.data;
                                $scope.ApJournalReportMaster = response.data;

                                //for (var i = 0; i < response.data.length; i++) {
                                //    $scope.TotalAmount = $scope.TotalAmount + response.data[i].Amount;
                                //}

                                debugger
                                $scope.TotalAmount = response.data.reduce(function (item1, item2) { return { Amount: item1.Amount + item2.Amount } }).Amount;
                              

                            }


                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        var a = error;
                        $scope.btnSpinner.stop();
                    });

            }
            $scope.showDefaultReport = false;
        }
        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.selectedSalesPersons = [];

            $scope.ApJournalReport          =[];
            $scope.ApJournalReportMaster = [];

           // $scope.beginDate = null;
           // $scope.endDate = null;

            $scope.filter.glAccount = 0;

            $scope.filter.stratSession = "";
            $scope.filter.endSession   = "";
            $scope.filter.startBatch = "";
            $scope.filter.endBatch = "";
            $scope.filter.vendor = "";
            $scope.filter.invoiceNumber = "";
            $scope.searchClicked = false;

            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {
            $scope.iscanceldisabled = true;
            return (httpRequest && httpRequest.abortCall());
        };

        // Return the array of objects from array of stringified objects
        function parsJasonFromArray(array) {
            var result = [];
            if (array && array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    result.push(JSON.parse(array[i]));
                }
            }
            return result;
        }
        function processJsonToKeyValueObject(data) {
            var result = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                result.push({ Key: item.id, Value: item.text });
            }
            return result;
        }
        function prossJsonIfSelectAll(selected) {
            return (selected[0] && selected[0].id == '-1' && selected[0].text == 'Select All') ? [] : selected;
        }

        function clearAllReportTables() {
            $scope.myTableFunctions.resetSearch();

            $scope.groupProperty = '';
            $scope.gridSearchText = "";

            $scope.ApJournalReport       = [];
            $scope.ApJournalReportMaster = [];
        }

       function getArTypeAsCommaSeparatedString(data) {
           var arTypes = "";
           for (i = 0; i < data.length; i++) {
               var obj = JSON.parse(data[i]);
               arTypes += obj.Key + ",";
           }
           arTypes = (arTypes.length>1) ? arTypes.substring(0, arTypes.length - 1) : arTypes;
           return arTypes;
       }
       function initFilter() {
           $scope.beginDate = "";
           $scope.endDate = "";
           $scope.filter.accountNumberStart = "";
           $scope.filter.accountNumberEnd = "";
           $scope.filter.vendor = "";
           $scope.filter.invoiceNumber = "";
           $scope.filter.stratSession = "";
           $scope.filter.endSession = "";
           $scope.filter.startBatch = "";
           $scope.filter.endBatch = "";
           $scope.filter.transactionNo = "";

           $scope.selectedArTypes = "";
           $scope.filter.transactionNo = "";
       }

       $scope.getCsvData = function (header, ReportMaster) {
           var array = [];
           var predefinedHeader = [];
           var TotalAmount = 0;
           predefinedHeader = header

           angular.forEach(ReportMaster, function (value, key) {
               var header = {};
               angular.forEach(value, function (value, key) {
                   var index = predefinedHeader.indexOf(key);
                   if (index > -1) {

                       if (key == "Amount") {
                           header[index] = "$" + value;
                           TotalAmount = TotalAmount + value;
                       }

                       else {
                           header[index] = value;
                       }

                   }

               });
               if (header != undefined)
                   array.push(header);
           });
           var total = {};
               total[0] = "Total";
               total[1] = "";
               total[2] = "";
               total[3] = "";
                total[4] = "";
                total[5] = "";
                total[6] = "";
                total[7] = "";
                total[8] = "";
               total[9] = "$" + TotalAmount;


           array.push(total);
           return array;
       };

       $scope.numberOfPages = function (length, pageSize) {
           return Math.ceil(length / pageSize);
       }

       $scope.getSum = function (list, property) {
           var sum = 0;
           if (list) {
               for (var i = 0; i < list.length; i++) {
                   sum += list[i][property];
               }
           }
           return sum;
       }

       $scope.getApCsvHeader = function () {
           return ["GL Account", "Account Name", "Transaction Date", "Vendor", "Invoice Number", "Reference Number", "Session", "GL Batch",
               "Description", "Amount"];
       }
       $scope.getApCsvData = function () {
           var header = ["GLAccount", "AccountName", "TransactionDate", "Vendor", "InvoiceNumber", "ReferenceNumber", "Session", "GLBatch",
                              "Description", "Amount"];
           return $scope.getCsvData(header, $scope.ApJournalReportMaster)
       }


     function generateDefaultReport() {
           var date = new Date();
           $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
           $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
           $scope.search();
       }
       generateDefaultReport();

    }])
