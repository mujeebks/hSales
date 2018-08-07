'use strict';
MetronicApp.controller('SalesOrderNoBinReportController', ['$scope', 'dataService', '$filter', 'NotificationService', 'ApiUrl', 'commonService', '$controller',

    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller) {


        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });

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

            $('#scheduledLoadStartDate1').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
            $('#scheduledLoadEndDate1').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
        }
        $scope.loadDatePickers();
        $scope.selectedSalesPerson = '';

        $scope.availableSalesPersons = [];
        function getSalesFilters() {
            dataService.GetAllSalesPersonsForFiltering()
           .then(function (response) {

               $scope.availableSalesPersons = [];
               angular.forEach(response.data, function (value, key) {

                   $scope.availableSalesPersons.push({ Key: value.SalesPersonCode, Value: value.SalesPersonDescription+' - ('+value.SalesPersonCode+')' });

               }, $scope.availableSalesPersons);
           });
        }
        getSalesFilters();

        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
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
        $scope.search = function () {
            $scope.searchClicked = true;
            if ($scope.beginDate) {

                $scope.btnSpinner.start();

                var param = {
                    Date: $scope.beginDate,
                    Sono: $scope.sonumber,
                    SalesPersons: getValueListFromKeyValueList(parsJasonFromArray($scope.selectedSalesPersons)),
                };
                $scope.SalesOrderNoBinReport = [];
                (httpRequest = dataService.GetSalesOrderWithNoBin(param))
                        .then(function (response) {
                            if (response) {

                                $scope.SalesOrderNoBinReportMasterExcel = response.data;

                                $scope.SalesOrderNoBinReportMaster = angular.copy($scope.SalesOrderNoBinReportMasterExcel);

                                for (var i = 0; i < $scope.SalesOrderNoBinReportMaster.length; i++) {
                                    var isdeleted = delete $scope.SalesOrderNoBinReportMaster[i]["Date"];
                                    if(isdeleted){
                                        $scope.SalesOrderNoBinReport.push(response.data[i]);
                                    }
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

            }
        };


        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            //var date = new Date();
            //$scope.beginDate = $filter('date')(date, 'MM/dd/yyyy');
            //$scope.sonumber = "";

            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {

                return (httpRequest && httpRequest.abortCall());

        };




        $scope.getCsvHeader = function () {
            return ['Sales Order Number ','Item','Quantity Shipped','PullerId','Puller','Sales Person Code','Sales Person Name', 'Date '];
        }
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];

            predefinedHeader = ["SalesOrderNumber", "Item", "QuantityShipped", "PullerId", "Puller", "SalesPerson", "SalesPersonName", "Date"];

            angular.forEach($scope.SalesOrderNoBinReportMasterExcel, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {

                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = value;

                        if (key == 'Date') {
                            header[index] = $filter('date')(value, 'MM/dd/yyyy');
                        }
                        if (key == 'QuantityShipped') {
                            header[index] = $filter('numberWithCommas')(value);
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
            $scope.beginDate = $filter('date')(date, 'MM/dd/yyyy');
            $scope.sonumber="";
            $scope.selectedSalesPersons = "";
            $scope.search();
        }
        generateDefaultReport();
}]);

