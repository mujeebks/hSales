
'use strict';

MetronicApp.controller('DumpdonationsReportController', ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', 'commonService', '$controller',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, commonService, $controller) {
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });

        $scope.iscanceldisabled = true;
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

        $controller('BaseController', { $scope: $scope });

        $scope.btnSpinner = commonService.InitBtnSpinner('#search');


        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
        $scope.searchClicked = false;

        $scope.selectedSalesPerson = '';




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

                    StartDate: $scope.beginDate,
                    EndDate: $scope.endDate,
                };



                (httpRequest = dataService.GetDumpAndDonation(param))
                .then(function onSuccess(response) {
                    if (response) {
                        $scope.iscanceldisabled = true;
                        $scope.reportData = [];
                        $scope.reportDataSafe = [];

                        for (var i = 0; i < response.data.length; i++) {
                            var data = response.data[i];
                            if (data.IsConsigned == 1) {
                                data["IsConsigned"] = "Yes";
                            }
                            else {
                                data["IsConsigned"] = "No";
                            }
                            $scope.reportData.push(data)

                        }

                        $scope.reportDataSafe = angular.copy($scope.reportData);


                    }
                    $scope.btnSpinner.stop();
                      })
                    .catch(function (exce) {
                        NotificationService.Error("An error occured while generating the report");
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
            //$scope.selectedSalesPersons = [];

           // $scope.beginDate = null;
          //  $scope.endDate = null;
            $scope.reportData = [];
            $scope.reportDataSafe = [];
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




        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }
        $scope.getCsvHeader = function () {
            //return ['Item Description', 'Customer', 'Quantity Shipped', 'Extended Cost', 'Type', 'Vendor Name', 'Buyer'];
            return ['Buyer Id', 'Item Description', 'Customer Description', 'Vendor/Supplier', 'Date', 'Quantity Shipped','Is Consigned', 'Type', 'Extended Cost'];

        };
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            //predefinedHeader = ['ItemDesc', 'Customer', 'QuantityShipped', 'ExtendedCost', 'Type', 'VendorName', 'BuyerId'];
            predefinedHeader = ['BuyerId', 'ItemDesc', 'CustomerDesc', 'VendorName', 'Date', 'QuantityShipped', 'IsConsigned', 'Type', 'ExtendedCost'];

            angular.forEach($scope.reportDataSafe, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                       // header[index] = (key == "ExtendedCost") ? "$" + value : value;

                        if (key == 'Date') {

                            header[index] = $filter('date')(value, 'MMM/dd/yyyy');
                        }

                        else if (key == 'ExtendedCost') {
                            header[index] = "$"+value;
                        }
                        else if (key == 'QuantityShipped') {
                            header[index] = $filter('numberWithCommas')(value);
                        }
                        else if (key == 'IsConsigned') {

                            header[index] = value;
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
        }
        generateDefaultReport();



    }]);
