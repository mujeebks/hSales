'use strict';
MetronicApp.controller('CasesSoldReportController', ['$scope', 'dataService', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller','HelperService',
    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller, HelperService) {

        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.casessoldcsv = "";
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

      function GetCasesSoldReportPrerequisites()  {
          dataService.GetCasesSoldReportPrerequisites().then(function (response) {
             
               $scope.Commodity = [];
               $scope.FoodService = [];
               $.each(response.data[0], function (key, value) {
                   $scope.Commodity.push({
                       key: key,
                       value: value
                   })
               });

               var categoriesData = localStorage.getItem('ls.nogalesAuthAccess');
               categoriesData = JSON.parse(categoriesData);
               categoriesData = categoriesData.Categories;

               $scope.FoodService.push({
                   key: 'All',
                   value: 'All'
               })

              $.each(categoriesData, function (key, value) {
                   $scope.FoodService.push({
                       key: key,
                       value: value.Name
                   })
              });
              if (categoriesData.length > 0) {
                  for (var i = 0; i < $scope.FoodService.length; i++) {
                      for (var z = 0; z < categoriesData.length; z++) {
                          if (categoriesData[z].Name == $scope.FoodService[i].value) {
                              if (categoriesData[z].IsAccess == false) {
                                  $scope.FoodService.splice(i, 1);
                              }
                          }
                      }
                  }
              }
            

               $scope.Commodityselected = $scope.Commodity[0];
               $scope.FoodServiceselected = $scope.FoodService[0];
               loadsalesPersons();
           });
        }
        GetCasesSoldReportPrerequisites();

     

        $scope.search = function (filter) {

            $scope.CurrentFilter = filter;
            $scope.iscanceldisabled = false;
            $scope.searchClicked = true;
            $scope.Prior = "";

            var date = {};
            var status = false;
            if (filter) {
                date = HelperService.Getstartenddate(filter);
            } else {
                date = {
                    firstDay: $scope.beginDate,
                    lastDay: $scope.endDate
                }
            }
        

          
            $scope.beginDate = $filter('date')(date.firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(date.lastDay, 'MM/dd/yyyy');

            $scope.Prior = ($scope.toggleyear) ? "LastYear" :"LastMonth";
            $scope.priorstatus = ($scope.toggleyear) ? "Prior Year" : "Prior Month";

            $scope.MinSalesAmt = ($scope.MinSalesAmt == undefined) ? 100 : $scope.MinSalesAmt;

            if ($scope.beginDate || date.lastDay) {

                function getPriorDate(startEndDate, Prior) {
                    var date = new Date(startEndDate);
                    return (Prior == 'LastMonth') ? moment(date).subtract(1, 'month').format('MM/DD/YYYY') :
                           (Prior == 'LastYear') ? moment(date).subtract(1, 'year').format('MM/DD/YYYY') : "";
                };

            $scope.StartPrior = $filter('date')(getPriorDate($scope.beginDate, $scope.Prior), 'MM/dd/yyyy');;
            $scope.EndPrior = $filter('date')(getPriorDate($scope.endDate, $scope.Prior), 'MM/dd/yyyy');;
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


                (httpRequest = dataService.getCasesSoldReportData(param))
                    .then(function (response) {
                        if (response) {
                            $scope.iscanceldisabled = true;
                            $scope.casessoldcsv = "CASES-SOLD-REPORT ( " + $scope.priorstatus + " ).CSV";
                            $scope.casessoldcsv = $scope.casessoldcsv.toUpperCase();
                            $scope.salesReport = response.data.ReportData;
                            $scope.salesReportMaster =angular.copy(response.data.ReportData);
                            $scope.ChartData = response.data.ChartData;
                            debugger
                            var TotalPrevious = 0;
                            var TotalCurrent = 0;
                            for (var i = 0; i < $scope.salesReportMaster.length; i++) {
                                TotalPrevious = TotalPrevious + $scope.salesReport[i].Previous;
                                TotalCurrent = TotalCurrent + $scope.salesReport[i].Current;
                            }
                            $scope.TotalPrevious = $filter('numberWithCommasRounded')(TotalPrevious);
                            $scope.TotalCurrent = $filter('numberWithCommasRounded')(TotalCurrent);
                          
                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        $scope.btnSpinner.stop();
                    });
                }
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
            try { return (httpRequest && httpRequest.abortCall());}
            catch (e) {}
        };


        //function parsJasonFromArray(array) {
        //    var result = [];
        //    if (array && array.length > 0) {
        //        for (var i = 0; i < array.length; i++) {
        //            result.push(JSON.parse(array[i]));
        //        }
        //    }
        //    return result;
        //}



        $scope.getCsvHeader = function () {
            return ['Customer Name', 'Cases Sold Prior', 'Cases Sold (Current)','Difference', 'Difference(%)'];
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
                        header[index] = (key == "Current" || key == "Previous") ? $filter('numberWithCommas')(value) : value;
                    }
                    //if (key == "Current") {
                    //    current = current + value;
                    //}
                    //if (key == "Previous") {
                    //    previous = previous + value;
                    //}
                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] = $scope.TotalPrevious;
            total[2] = $scope.TotalCurrent;
            array.push(total);
            return array;
        };

        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.search();
        };


    
        $scope.getCustomersSalesDataReport = function (salesPerson) {
            $scope.selectedSalesPerson = salesPerson;
            $scope.customersData = [];
            $scope.customersDataSafe = [];
            dataService.GetSalesPersonCustomersDataOfSalesReport(salesPerson, $scope.beginDate, $scope.endDate).then(function (response) {
                if (response && response.data) {
                    $scope.customersData = response.data;
                    $scope.customersDataSafe = angular.copy($scope.customersData);
                }
            }, function onError() {
                stopDivLoader("salesPersonReportPortletBody");
                NotificationService.Error("Error upon the API request");
            });
        }

        function loadsalesPersons() {
            dataService.GetAllSalesPersonsForFiltering().then(function (response) {
              $scope.salesPersonList = [];
              for (var i = 0; i < response.data.length; i++) {
                  var defaultfilter = {
                      "AssignedPersonList": response.data[i].AssignedPersonList,
                      "Category": response.data[i].Category,
                      "Id": i,
                      "SalesPersonCode": response.data[i].SalesPersonCode,
                      "SalesPersonDescription": response.data[i].SalesPersonDescription
                  };
                  $scope.salesPersonList.push(defaultfilter)
              }

              var defaultfilter = {
                  "AssignedPersonList": "All",
                  "Category": "All",
                  "Id": $scope.salesPersonList.length,
                  "SalesPersonCode": "All",
                  "SalesPersonDescription": "All"
              };
              $scope.salesPersonList.push(defaultfilter);
              $scope.salesPersonList = $scope.salesPersonList.reverse();
              $scope.salespersonitem = $scope.salesPersonList[0];
              generateDefaultReport();
          });
        };
    }]);
