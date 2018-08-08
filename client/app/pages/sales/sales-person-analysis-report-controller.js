'use strict';
MetronicApp.controller('SalesPersonAnalysisReportController', ['$scope', '$timeout', 'dataService', '$stateParams', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope,$timeout, dataService, $stateParams, $filter, NotificationService, ApiUrl, commonService, $controller) {

        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
        $scope.iscanceldisabled = true;

        $scope.btnSpinner = commonService.InitBtnSpinner('#search');


        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';
        $scope.searchClicked = false;

        $scope.selectedSalesPerson = '';

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

        $scope.getSalesFilters = function () {
            dataService.getSalesReportFilters()
           .then(function (response) {
               $scope.availableSalesPersons = response.data.ListSalesPerson;

           });
        }
        $scope.getSalesFilters();

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
                //The quarter is zero means, when current quarter is first , then prevous quarter is the last quarter of previous year
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
            //$scope.dfDateFilter = "Custom";

            // if any of the dates are blank status is true
            //else if both of the dates are not null then check for beginDate.length + endDate.length <20 status is true
            // otherwise status is false
            status = (!$scope.beginDate || !$scope.endDate) ?
                    true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

            if (!status &&
                ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                $scope.btnSpinner.start();
                var param = {
                    SalesPerson: getValueListFromKeyValueList(parsJasonFromArray($scope.selectedSalesPersons)),
                    StartDate: $scope.beginDate,
                    EndDate: $scope.endDate,
                };

                (httpRequest = dataService.GetSalesAnalysisReportofSalesPerson(param))
                    .then(function (response) {
                        if (response) {
                            //
                            $scope.iscanceldisabled = true;
                            $scope.salesReport = response.data;
                            $scope.salesReportMaster = response.data;

                            $scope.TotalNoOfCustomer = 0;
                            $scope.TotalGroceryCasesSold = 0;
                            $scope.TotalProduceCasesSold = 0;
                            $scope.TotalManualCasesSold = 0;
                            $scope.TotalTotalCasesSold = 0;
                            $scope.TotalGroceryRevenue = 0;
                            $scope.TotalProduceRevenue = 0;
                            $scope.TotalManualRevenue = 0;
                            $scope.TotalTotalRevenue = 0;

                            var length=$scope.salesReport.length;

                            for (var i = 0; i <length; i++) {

                                $scope.TotalNoOfCustomer = $scope.TotalNoOfCustomer + $scope.salesReport[i].NoOfCustomer;
                                $scope.TotalGroceryCasesSold = $scope.TotalGroceryCasesSold + $scope.salesReport[i].GroceryCasesSold;
                                $scope.TotalProduceCasesSold = $scope.TotalProduceCasesSold + $scope.salesReport[i].ProduceCasesSold;
                                $scope.TotalManualCasesSold = $scope.TotalManualCasesSold + $scope.salesReport[i].ManualCasesSold;
                                $scope.TotalTotalCasesSold = $scope.TotalTotalCasesSold + $scope.salesReport[i].TotalCasesSold;
                                $scope.TotalGroceryRevenue = $scope.TotalGroceryRevenue + $scope.salesReport[i].GroceryRevenue;
                                $scope.TotalProduceRevenue = $scope.TotalProduceRevenue + $scope.salesReport[i].ProduceRevenue;
                                $scope.TotalManualRevenue = $scope.TotalManualRevenue + $scope.salesReport[i].ManualRevenue;
                                $scope.TotalTotalRevenue = $scope.TotalTotalRevenue + $scope.salesReport[i].TotalRevenue;

                            }

                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        var a = error;
                        $scope.btnSpinner.stop();
                    });

            }
            else {
                //   Metronic.stopPageLoading();
            }

        }
        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.selectedSalesPersons = [];

            //$scope.beginDate = null;
            //$scope.endDate = null;
            $scope.salesReport = [];
            $scope.salesReportMaster = [];
            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {
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

        $scope.getCsvHeader = function () {
            return ['Sales Person', 'No of Customers', 'Grocery Cases Sold', 'Grocery Revenue', 'Produce Cases sold', 'Produce Revenue', 'Manual Cases sold', 'Manual Revenue', 'Total Cases Sold', 'Total Revenue'];
        };
        $scope.getCsvDatacustomerreportHeader = function () {
            
            return ['Customer Name', 'Sales Amount Prior', 'Sales Amount Current', 'Difference', 'Difference(%)'];
        };
        $scope.setscroll = function () {

            $("body").css("overflow", "inherit");
        };
        $scope.getCsvDatacustomerreport = function () {
         
            var array = [];
            var predefinedHeader = [];
            $scope.filename = ("Customer Report Of " + $scope.selectedSalesPerson + ".csv").toUpperCase();
            predefinedHeader = ["Customer", "SalesAmountPrior", "SalesAmountCurrent", "Difference", "Percentage"];
            angular.forEach($scope.customersDataSafe, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = (key == "SalesAmountPrior" || key == "SalesAmountCurrent" || key == "Difference") ? "$" + value : value;
                    }
                });
                if (header != undefined)
                    array.push(header);
            });
            var total = {};
            total[0] = "Total";
            total[1] = $scope.SalesAmountPriorTotal;
            total[2] = $scope.SalesAmountCurrentTotal;
            total[3] = total[4] = "";
            array.push(total);

            return array;
        };

        $scope.getCsvData = function () {
        
                var array = [];
                var predefinedHeader = [];
                var NoOfCustomer = 0;
                var TotalRevenue = 0;
                var TotalCasesSold = 0;
                var GroceryRevenue = 0;
                var GroceryCasesSold = 0;
                var ProduceRevenue = 0;
                var ProduceCasesSold = 0;
                var ManualRevenue = 0;
                var ManualCasesSold = 0;

                predefinedHeader = ["SalesPerson", "NoOfCustomer", "GroceryCasesSold", "GroceryRevenue", "ProduceCasesSold", "ProduceRevenue", "ManualCasesSold", "ManualRevenue", "TotalCasesSold", "TotalRevenue"];


            angular.forEach($scope.salesReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        //   header[index] = (key == "SalesAmountCurrent" || key == "SalesAmountPrior" || key == "Difference") ? "$" + value : value;
                        header[index] = (key == "TotalRevenue" || key == "GroceryRevenue" || key == "ProduceRevenue" || key == "ManualRevenue") ? "$" + value : value;

                        header[index] = (key == "TotalCasesSold" || key == "GroceryCasesSold" || key == "ProduceCasesSold" || key == "ManualCasesSold") ? $filter('numberWithCommas')(value) : value;
                        if (key == "NoOfCustomer") {
                            header[index] = $filter('numberWithCommas')(value);
                        }
                    }
                    if (key=="NoOfCustomer") {

                                    NoOfCustomer = NoOfCustomer + value;
                                }
                                if (key=="TotalRevenue") {

                                    TotalRevenue = TotalRevenue + value;
                                }

                                if (key=="TotalCasesSold") {

                                    TotalCasesSold = TotalCasesSold + value;
                                }

                                if (key=="GroceryRevenue") {

                                    GroceryRevenue = GroceryRevenue + value;
                                }

                                if (key=="GroceryCasesSold") {

                                    GroceryCasesSold = GroceryCasesSold + value;
                                }

                                if (key=="ProduceRevenue") {

                                    ProduceRevenue = ProduceRevenue + value;
                                }


                                if (key=="ProduceCasesSold") {

                                    ProduceCasesSold = ProduceCasesSold + value;
                                }

                                if (key=="ManualRevenue") {

                                    ManualRevenue = ManualRevenue + value;
                                }
                                if (key=="ManualCasesSold") {

                                    ManualCasesSold = ManualCasesSold + value;
                                }

                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] =$filter('numberWithCommas')(NoOfCustomer) ;
            total[2] = $filter('numberWithCommas')(GroceryCasesSold);
            total[3] = "$" + GroceryRevenue;
            total[4] = $filter('numberWithCommas')(ProduceCasesSold);
            total[5] = "$" + ProduceRevenue;
            total[6] = $filter('numberWithCommas')(ManualCasesSold);
            total[7] = "$" + ManualRevenue;
            total[8] = $filter('numberWithCommas')(TotalCasesSold);
            total[9] = "$" + TotalRevenue;










            array.push(total);

            return array;
        };

        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.search();
        }
        generateDefaultReport();




        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        };

        function toJSONLocal(date) {
            var local = new Date(date);
            local.setMinutes(date.getMinutes() - date.getTimezoneOffset());
            return local.toJSON().slice(0, 10);
        };
        $scope.getCustomersSalesDataReport = function (salesPerson) {
          

            $scope.selectedSalesPerson = salesPerson;
            $scope.customersData = [];
            $scope.customersDataSafe = [];

            
            var params = {

                //date today moth from date year from date
                SalesPerson: salesPerson,
                StartDateCurrent: $scope.beginDate,
                EndDateCurrent: $scope.endDate,
                StartDatePrevious: "",
                EndDatePrevious: "",
                Commodity:"All",
                OrderBy:"sales"
            };

            startDivLoader("salesPersonReportPortletBody");
            dataService.GetCustomerWiseReportOfSalesPerson(params).then(function (response) {
                if (response && response.data) {
                    $scope.customersData = response.data;
                    $scope.customersDataSafe = angular.copy($scope.customersData);
                    var SalesAmountCurrentTotal = 0;
                    var SalesAmountPriorTotal = 0;
                    for (var i = 0; i < $scope.customersDataSafe.length; i++) {
                        SalesAmountCurrentTotal = SalesAmountCurrentTotal +$scope.customersDataSafe[i].SalesAmountCurrent;
                        SalesAmountPriorTotal = SalesAmountPriorTotal + $scope.customersDataSafe[i].SalesAmountPrior;
                    }
                    $scope.SalesAmountCurrentTotal = "$" + $filter('numberWithCommas')(Math.round(SalesAmountCurrentTotal));
                    $scope.SalesAmountPriorTotal = "$" + $filter('numberWithCommas')(Math.round(SalesAmountPriorTotal));
                   

                }
                stopDivLoader("salesPersonReportPortletBody");
            }, function onError() {
                stopDivLoader("salesPersonReportPortletBody");
                NotificationService.Error("Error upon the API request");
                NotificationService.ConsoleLog('Error on the server');
            });
        }
        $scope.openModal = function (elementId) {
            debugger
            $("body").css("overflow", "hidden");

            $scope.CustomerSearch = "";
            if (elementId && elementId != '') {
                $('#' + elementId).modal({ backdrop: 'static', keyboard: false });
            }

        };
        $scope.closeModal = function (elementId) {
            $("body").css("overflow", "inherit");

            if (elementId && elementId != '') {
                $('#' + elementId).modal('hide');
            }

        };
        $scope.expand = false;
        $scope.action = function () {
         
            $scope.expand = !$scope.expand;
            
           
        };

    }]);

