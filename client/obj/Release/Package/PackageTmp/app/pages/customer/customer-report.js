
'use strict';
MetronicApp.controller('CustomerReportController', ['$scope', 'dataService', '$filter', 'NotificationService', 'ApiUrl', 'commonService', '$controller',


    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller) {

     
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });
        $scope.customerReport = [];
        $scope.selectedData = null;
        $scope.iscanceldisabled = true;

        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });

        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.getmomnetdate = function (lastpaiddate) {
            //console.log(lastpaiddate);
            if (lastpaiddate) {
                var a = moment(lastpaiddate, "YYYYMMDD").fromNow();
                return a;
            }


        };
        var httpRequest = null;

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

            else if (filter == "twomonthsago") {

                firstDay = moment().subtract(2, 'months').calendar();
                lastDay = moment().subtract(2, 'months').calendar();
            }
            else if (filter == "sixmonthsago") {
                firstDay = moment().subtract(6, 'months').calendar();
                lastDay = moment().subtract(6, 'months').calendar();
            }
            else if (filter == "ninemonthsago") {
                firstDay = moment().subtract(9, 'months').calendar();
                lastDay = moment().subtract(9, 'months').calendar();
            }
            else if (filter == "oneyearago") {
                firstDay = moment().subtract(1, 'year').calendar();
                lastDay = moment().subtract(1, 'year').calendar();
            }
            else {
                firstDay = $scope.beginDate;
                lastDay = $scope.endDate;
            }

            $scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');


            if ($scope.beginDate || lastDay) {
                function getPriorDate(startEndDate, Prior) {

                    var date = new Date(startEndDate);
                    var newday = "";
                    var newdayend = "";
                    if (Prior == 'LastMonth') {

                        var month = date.getMonth();
                        var year;
                        if (month < 12)
                            year = date.getFullYear();
                        else
                            year = date.getFullYear() - 1;
                        newday = new Date(year, month - 1, 1);
                        newdayend = new Date(year, month, 0);

                    }
                    else {
                        newday = new Date(date.getFullYear() - 1, 0, 1);
                        newdayend = new Date(date.getFullYear(), 0, 0);
                    }

                    return newdayend;
                };

                $scope.StartPrior = $filter('date')(getPriorDate($scope.beginDate, $scope.Prior), 'MM/dd/yyyy');;
                $scope.EndPrior = $filter('date')(getPriorDate(lastDay, $scope.Prior), 'MM/dd/yyyy');;
                status = (!$scope.beginDate || !$scope.endDate) ?
                        true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

                if (!status &&
                    ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                    $scope.btnSpinner.start();

                    var param = {
                        currentStartDate: $scope.beginDate,
                        currentEndDate: $scope.endDate,


                    };

                    (httpRequest = dataService.GetCustomerReport(param))
                        .then(function (response) {
                            if (response) {

                                $scope.iscanceldisabled = true;

                                //for (var i = 0; i <response.data.length; i++) {
                                //    response.data[i].LastPayedDate = (response.data[i].LastPayedDate) ? $filter('date')(response.data[i].LastPayedDate, "MM/dd/yyyy") : '';
                                //    response.data[i].EnteredDate = (response.data[i].EnteredDate) ? $filter('date')(response.data[i].EnteredDate, "MM/dd/yyyy") : '';


                                //    response.data[i].LastSaleDate = (response.data[i].LastSaleDate) ? $filter('date')(response.data[i].LastSaleDate, "MM/dd/yyyy") : '';

                                //    if (response.data[i].EnteredDate == "01/01/1900") {
                                //        response.data[i].EnteredDate = "";
                                //    }
                                //    if (response.data[i].LastPayedDate == "01/01/1900") {
                                //        response.data[i].LastPayedDate = "";
                                //    }
                                //    if (response.data[i].LastSaleDate == "01/01/1900") {
                                //        response.data[i].LastSaleDate = "";
                                //    }
                                //}

                                $scope.customerReport = response.data;


                                $scope.customerReportMaster = angular.copy($scope.customerReport);
                            }

                            $scope.btnSpinner.stop();
                        })
                        .catch(function (error) {
                            $scope.iscanceldisabled = true;
                            var a = error;
                            $scope.btnSpinner.stop();
                        });
                }
            }
            else {

            }

        }


        $scope.resetSearch = function () {

            $scope.itemReport = [];
            $scope.itemReportMaster = [];
            $scope.CurrentFilter = "";
            $scope.selectedItems = [];
            //   $scope.clearInput();

             generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {

            $scope.iscanceldisabled = true;
            try {
                return (httpRequest && httpRequest.abortCall());
            }
            catch (e) {
                //console.log(e.constructor.name);
            }

        };




        $scope.getCsvHeader = function () {
            return ["Customer#", "Customer Name", "Contact Person", "Address", "Phone", "Entered Date","Last Payed Date", "Last Payed Amount", "Sales Person", "Last Sales Amount", "Last Sale Date" ];
        }

        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var Cost = 0;
            var Price = 0;
            predefinedHeader = ["CustomerNumber", "CustomerName", "ContactPerson", "Address", "Phone","EnteredDate","LastPayedDate", "LastPayedAmount", "SalesPerson", "LastSalesAmount", "LastSaleDate"];

            angular.forEach($scope.customerReportMaster, function (value, key) {

                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        if (key == "LastPayedDate" || key == "EnteredDate" || key == "LastSaleDate") {


                            if (key == "EnteredDate" && value == "01/01/1900") {
                                header[index] = "";
                            }
                           else if (key == "LastPayedDate" && value == "01/01/1900") {
                                header[index] = "";
                            }
                           else if (key == "LastSaleDate" && value == "01/01/1900") {
                               header[index] = "";
                           }
                           else {
                               header[index] = $filter('date')(value, 'dd/MM/yyyy');
                           }
                        }
                        else if (key == "LastPayedAmount" || key == "LastSalesAmount")
                        {
                            header[index] = "$"+value;
                        }
                        else {
                            header[index] = value;
                        }

                    }
                });
                if (header != undefined)
                    array.push(header);
            });
            return array;
        };

        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');

            $scope.search();

        };

        generateDefaultReport();

    }]);