'use strict';
MetronicApp.controller('CommodityReportController', ['$scope', 'dataService', '$filter', 'NotificationService', 'ApiUrl', 'commonService', '$controller',

    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller) {
      
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.commoditylist = [
            { id: 0, com: "All" },
            { id: 1, com: "Produce" },
             { id: 2, com: "Grocery" }
        ];

        $scope.Commodityselected = 0;
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
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

        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }

        $scope.search = function (filter) {
            $scope.iscanceldisabled = false;
            $scope.CurrentFilter = filter;
            $scope.searchClicked = true;
            $scope.Prior = "";
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
            else if (filter == "MTD") {
                var date = new Date();

                firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                lastDay = new Date();
            }
            else {
                firstDay = $scope.beginDate;
                lastDay = $scope.endDate;
            }

            $scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');

            if ($scope.beginDate || lastDay) {

                status = (!$scope.beginDate || !$scope.endDate) ?
                        true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

                if (!status &&
                    ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                    $scope.btnSpinner.start();

                    var commodity = $scope.Commodityselected;

                    commodity = ($scope.Commodityselected == 0 || $scope.Commodityselected == undefined) ? "" :
                                ($scope.Commodityselected == 1) ? "01" :
                                ($scope.Commodityselected == 2) ? "02" : "";


                    var param = {
                        comodity: commodity,
                        startDate: $scope.beginDate,
                        endDate: $scope.endDate
                    };

                    (httpRequest = dataService.GetComodityExpenseReport(param))
                        .then(function (response) {
                            if (response.data.length > 0) {

                                $scope.iscanceldisabled = true;
                                $scope.CurrentLabel = 'Current Expense (' + response.data[0].CurrentLabel + ')';
                                $scope.PriorLabel = 'Prior Expense (' + response.data[0].PriorLabel + ')';
                                $scope.salesReport = response.data;
                                $scope.salesReportMaster =angular.copy(response.data);
                                var Total = $scope.salesReport.reduce(function (item1, item2) {
                                    return {
                                        PriorExpense: Number.parseInt(item1.PriorExpense + '') + Number.parseInt(item2.PriorExpense + ''),
                                        PriorRevenue: Number.parseInt(item1.PriorRevenue + '') + Number.parseInt(item2.PriorRevenue + ''),
                                        CurrentExpense: Number.parseInt(item1.CurrentExpense + '') + Number.parseInt(item2.CurrentExpense + '')
                                    };
                                });
                                $scope.TotalCurrentExpense = Total.CurrentExpense;
                                $scope.TotalPriorExpense = Total.PriorExpense;
                                $scope.TotalPriorRevenue = Total.PriorRevenue;
                            }
                            else {
                                $scope.iscanceldisabled = true;
                                $scope.CurrentLabel = "Current Expense";
                                $scope.PriorLabel = "Prior Expense";
                                $scope.salesReport = [];
                                $scope.salesReportMaster = [];

                            }
                            $scope.btnSpinner.stop();
                        })
                        .catch(function (error) {
                            $scope.btnSpinner.stop();
                        });

                }

            }

        };

        $scope.resetSearch = function () {

            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.selectedSalesPersons = [];

            $scope.salesReport = [];
            $scope.salesReportMaster = [];
            $scope.Commodityselected = 0;

            $scope.CurrentFilter = 'MTD';
            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {
            $scope.iscanceldisabled = true;
            try {return (httpRequest && httpRequest.abortCall());}
            catch (e) {}
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
            return ['Category', 'Commodity', $scope.CurrentLabel, $scope.PriorLabel, "Prior Revenue"];
        }

        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var CurrentExpenseTotal = 0;
            var PriorExpenseTotal = 0;
            var TotalPriorRevenue = 0;

            predefinedHeader = ["Category", "Commodity", "CurrentExpense", "PriorExpense", "PriorRevenue"];

            angular.forEach($scope.salesReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        header[index] = (key == "CurrentExpense" || key == "PriorExpense" || key == "PriorRevenue") ? "$" + value : value;
                    }

                    if (key == "CurrentExpense") {
                        CurrentExpenseTotal = CurrentExpenseTotal + value;
                    }
                    if (key == "PriorExpense") {
                        PriorExpenseTotal = PriorExpenseTotal + value;
                    }
                    if (key == "PriorRevenue") {
                        TotalPriorRevenue = TotalPriorRevenue + value;
                    }

                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] = "";
            total[2] = "$" + CurrentExpenseTotal;
            total[3] = "$" + PriorExpenseTotal;
            total[4] = "$" + TotalPriorRevenue;

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

        //function getValueListFromKeyValueList(list) {
        //    var result = [];
        //    for (var i = 0; i < list.length; i++) {
        //        result.push(list[i].Key);
        //    }
        //    return result;
        //}

        $scope.checksamemonthdate = function (startDate,EndDate) {
            var start = new Date(startDate);
            var endDate = new Date(EndDate);
            return (start.getMonth() == endDate.getMonth()) ? true : false;
        }
    }]);

