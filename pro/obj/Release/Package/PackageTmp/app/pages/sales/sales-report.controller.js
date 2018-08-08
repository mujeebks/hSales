'use strict';
MetronicApp.controller('SalesReportController', ['$scope', 'dataService', '$stateParams', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller','$rootScope',
    function ($scope, dataService, $stateParams, $filter, NotificationService, ApiUrl, commonService, $controller, $rootScope) {

        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });

        $controller('BaseController', { $scope: $scope });
        $scope.priorstatus = "";
        $scope.iscanceldisabled = true;
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');

        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
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
            $scope.iscanceldisabled = false;
            $scope.searchClicked = true;

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

                (httpRequest = dataService.getSalesReportData(param))
                    .then(function (response) {
                        if (response.data) {
                            $scope.iscanceldisabled = true;
                            $scope.salesReport = response.data.ReportData;
                            $scope.salesReportMaster =angular.copy(response.data.ReportData);
                            $scope.ChartData = response.data.ChartData;
                          
                            var Total = $scope.salesReport.reduce(function (item1, item2) {
                                return {
                                    NoOfCustomer: (item1.NoOfCustomer) + (item2.NoOfCustomer),
                                    SalesAmountPrior: (item1.SalesAmountPrior) + (item2.SalesAmountPrior),
                                    SalesAmountCurrent: (item1.SalesAmountCurrent) + (item2.SalesAmountCurrent)
                                };
                            });

                            $scope.TotalNoOfCustomer = Math.round(Total.NoOfCustomer);
                            $scope.TotalSalesAmountPrior = Math.round(Total.SalesAmountPrior);
                            $scope.TotalSalesAmountCurrent = Math.round(Total.SalesAmountCurrent);
                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        $scope.btnSpinner.stop();
                    });

            }
        }
        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.selectedSalesPersons = [];
            generateDefaultReport();
            $scope.salesReport = [];
            $scope.salesReportMaster = [];
        };
        $scope.abortExecutingApi = function () {
                return (httpRequest && httpRequest.abortCall());
        };

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
            return ['Sales Person', 'Sales Person Description', 'Number Of Customers', 'Sales Amount Prior', 'Sales Amount Current', 'Difference', 'Difference (%)'];//,'Bin #','Picker'
        }


        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var NoOfCustomer = 0;
            var SalesAmountCurrent = 0;
            var SalesAmountPrior = 0;
            predefinedHeader = ["SalesPerson", "SalesPersonDescription", "NoOfCustomer", "SalesAmountPrior", "SalesAmountCurrent", "Difference", "Percentage"];//,"Binno","Picker"


            angular.forEach($scope.salesReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = (key == "SalesAmountCurrent" || key == "SalesAmountPrior" || key == "Difference") ? "$" + Math.round(value) : value;

                        if (key == "NoOfCustomer") {
                            header[index] = $filter('numberWithCommas')(value);
                        }
                        if (key == "Percentage") {
                            header[index] = Math.round(value);
                        }
                        if (key == "NoOfCustomer") {
                            NoOfCustomer = NoOfCustomer + value;
                        }
                        if (key == "SalesAmountCurrent") {
                            SalesAmountCurrent = SalesAmountCurrent + value;
                        }
                        if (key == "SalesAmountPrior") {
                            SalesAmountPrior = SalesAmountPrior + value;
                        }
                    }
                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] = "";
            //total[2] = "";
            total[2] = $filter('numberWithCommasRounded')(NoOfCustomer);
            total[3] = "$" + Math.round(SalesAmountPrior);
            total[4] = "$" + Math.round(SalesAmountCurrent);

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
        }
        $scope.getCustomersSalesDataReport = function (salesPerson) {

            $scope.CustomerSearch = "";

            $scope.selectedSalesPerson = salesPerson;
            $scope.customersData     = [];
            $scope.customersDataSafe = [];
            startDivLoader("salesPersonReportPortletBody");

            var params = {
                SalesPerson: salesPerson,
                StartDateCurrent: $scope.beginDate,
                EndDateCurrent: $scope.endDate,
                StartDatePrevious: "",
                EndDatePrevious: "",
                Commodity: "All",
                OrderBy:"sales"
            };

            dataService.GetCustomerWiseReportOfSalesPerson(params).then(function (response) {
                if (response && response.data) {
                   
                    $scope.customersData = response.data;
                    $scope.customersDataSafe = angular.copy($scope.customersData);

                    var Total = $scope.customersData.reduce(function (item1, item2) {
                        return {
                            SalesAmountPrior: Number.parseInt(item1.SalesAmountPrior + '') + Number.parseInt(item2.SalesAmountPrior + ''),
                            TotalSalesAmtCurrent: Number.parseInt(item1.TotalSalesAmtCurrent + '') + Number.parseInt(item2.TotalSalesAmtCurrent + ''),
                            TotalSalesQuantity: Number.parseInt(item1.TotalSalesQuantity + '') + Number.parseInt(item2.TotalSalesQuantity + ''),
                            Difference: Number.parseInt(item1.Difference + '') + Number.parseInt(item2.Difference + '')
                        };
                    });

                    $scope.TotalSalesAmtPrior = Total.SalesAmountPrior;
                    $scope.TotalSalesQuantity = Total.TotalSalesQuantity;
                    $scope.TotalSalesAmtCurrent = Total.TotalSalesAmtCurrent;
                    $scope.TotalDifference = Total.Difference;
                    stopDivLoader("salesPersonReportPortletBody");
                }
            }, function onError() {
                Metronic.unblockUI();
                NotificationService.Error("Error upon the API request");
            });
           
        }
        $scope.openModal = function (elementId) {
            if (elementId && elementId != '') {
                $('#' + elementId).modal({ backdrop: 'static', keyboard: false });
            }
        }
        $scope.closeModal = function (elementId) {
            if (elementId && elementId != '') {
                $('#' + elementId).modal('hide');
            }
        }

    }]);

