'use strict';
MetronicApp.controller('RevenueReportController', ['$scope', 'dataService', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller) {

        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();
        });
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
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

        function GetCasesSoldReportPrerequisites() {
            dataService.GetCasesSoldReportPrerequisites()
           .then(function (response) {

               $scope.Commodity = [];
               $scope.FoodService = [];


               $.each(response.data[0], function (key, value) {

                   $scope.Commodity.push({
                       key: key,
                       value: value
                   })
               });


               $.each(response.data[1], function (key, value) {
                   $scope.FoodService.push({
                       key: key,
                       value: value
                   })
               });



               $scope.Commodityselected = $scope.Commodity[0];
               $scope.FoodServiceselected = $scope.FoodService[0];

               loadsalesPersons();



           });
        }
        GetCasesSoldReportPrerequisites();


        function loadsalesPersons() {
            dataService.GetAllSalesPersonsForFiltering()
          .then(function (response) {

              $scope.salesPersonList = [];

              for (var i = 0; i < response.data.length; i++) {

                  var defaultfilter = {
                      "AssignedPersonList": response.data[i].AssignedPersonList,
                      "Category": response.data[i].Category,
                      "Id": i,
                      "SalesPersonCode": response.data[i].SalesPersonCode,
                      "SalesPersonDescription": response.data[i].SalesPersonDescription
                  }


                  $scope.salesPersonList.push(defaultfilter)
              }



              var defaultfilter = {
                  "AssignedPersonList": "All",
                  "Category": "All",
                  "Id": $scope.salesPersonList.length,
                  "SalesPersonCode": "All",
                  "SalesPersonDescription": "All"
              }
              $scope.salesPersonList.push(defaultfilter);

              $scope.salesPersonList = $scope.salesPersonList.reverse();
              $scope.salespersonitem = $scope.salesPersonList[0];

              generateDefaultReport();

          });



        };

     //   loadsalesPersons();

        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }

        $scope.search = function (filter) {
            $scope.CurrentFilter = filter;
            $scope.iscanceldisabled = false;
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
                else {
                    firstDay = $scope.beginDate;
                    lastDay = $scope.endDate;
                }

                $scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
                $scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');



                if ($scope.toggleyear) {
                    $scope.Prior = "LastYear";
                    $scope.priorstatus = "Prior Year";
                }
                else {
                    $scope.Prior = 'LastMonth';
                    $scope.priorstatus = "Prior Month";
                }

                if ($scope.MinSalesAmt == undefined) {
                    $scope.MinSalesAmt = 100;
                }
                if ($scope.MinSalesAmt == "") {
                    $scope.MinSalesAmt = 0;
                }


                if ($scope.beginDate || lastDay) {

                    function getPriorDate(startEndDate, Prior) {

                        var date = new Date(startEndDate);
                        var returndate = "";
                        if (Prior == 'LastMonth') {

                            returndate = moment(date).subtract(1, 'month').format('MM/DD/YYYY');

                        }
                        else {
                            returndate = moment(date).subtract(1, 'year').format('MM/DD/YYYY');

                        }

                        return returndate

                    };


                $scope.StartPrior = $filter('date')(getPriorDate($scope.beginDate, $scope.Prior), 'MM/dd/yyyy');;
                $scope.EndPrior = $filter('date')(getPriorDate(lastDay, $scope.Prior), 'MM/dd/yyyy');;


                status = (!$scope.beginDate || !$scope.endDate) ?
                        true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

                if (!status &&
                    ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                    $scope.btnSpinner.start();



                    var param = {
                        Comodity: ($scope.Commodityselected.value == "All") ? "" : $scope.Commodityselected.value,
                        Category: ($scope.FoodServiceselected.value == "All") ? "" : $scope.FoodServiceselected.value,
                        currentStartDate: $scope.beginDate,
                        currentEndDate: $scope.endDate,
                        priorStartDate: $scope.StartPrior,
                        priorEndDate: $scope.EndPrior,
                        MinSalesAmt: $scope.MinSalesAmt,
                        SalesPersonCode: ($scope.salespersonitem.SalesPersonCode == "All") ? "" : $scope.salespersonitem.SalesPersonCode
                    };
                    //console.log(param);

                    (httpRequest = dataService.GetRevenueCustomerReport(param))
                        .then(function (response) {
                            if (response) {
                                $scope.iscanceldisabled = true;
                                $scope.revenuecsv = "Revenue-Report ( " + $scope.priorstatus + " ).csv";
                                $scope.revenuecsv = $scope.revenuecsv.toUpperCase();

                                $scope.salesReport = response.data.ReportData;
                                $scope.salesReportMaster = response.data.ReportData;

                                $scope.ChartData = response.data.ChartData;


                                $scope.TotalPrevious = 0;
                                $scope.TotalCurrent = 0;




                                var length = $scope.salesReport.length;

                                for (var i = 0; i < length; i++) {

                                    $scope.TotalPrevious = $scope.TotalPrevious + $scope.salesReport[i].Previous;
                                    $scope.TotalCurrent = $scope.TotalCurrent + $scope.salesReport[i].Current;

                                }

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
                    //   Metronic.stopPageLoading();
                }



        }
        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.selectedSalesPersons = [];
            $scope.MinSalesAmt = 100;

            $scope.salesReport = [];
            $scope.salesReportMaster = [];

            $scope.Commodityselected = $scope.Commodity[0];
            $scope.FoodServiceselected = $scope.FoodService[0];

            loadsalesPersons();
            $scope.toggleyear = false;
            $scope.CurrentFilter = "";

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
            return ['Customer Name', 'Revenue Prior', 'Revenue (Current)', 'Difference', 'Difference(%) '];
        }
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var current = 0;
            var previous = 0;
            predefinedHeader = ["Customer", "Previous", "Current", "Difference", "PercentageDifference"];

            angular.forEach($scope.salesReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = (key == "Current" || key == "Previous") ? "$" + value : value;
                    }

                    if (key == "Current") {
                        current = current + value;
                    }
                    if (key == "Previous") {
                        previous = previous + value;
                    }

                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] ="$"+previous;
            total[2] = "$"+current;

            array.push(total);

            return array;
        };





        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.search();
        }
      //  generateDefaultReport();




        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        }
        $scope.getCustomersSalesDataReport = function (salesPerson) {
            // $('#salesReportOfSalesPerson').modal('show');
            $scope.selectedSalesPerson = salesPerson;
            $scope.customersData = [];
            $scope.customersDataSafe = [];
            startDivLoader("salesPersonReportPortletBody");
            dataService.GetSalesPersonCustomersDataOfSalesReport(salesPerson, $scope.beginDate, $scope.endDate).then(function (response) {
                if (response && response.data) {

                    $scope.customersData = response.data;
                    $scope.customersDataSafe = angular.copy($scope.customersData);
                    //$scope.openModal('responsive');
                }
                stopDivLoader("salesPersonReportPortletBody");
            }, function onError() {
                stopDivLoader("salesPersonReportPortletBody");
                NotificationService.Error("Error upon the API request");
                NotificationService.ConsoleLog('Error on the server');
            });
        }





    }]);

