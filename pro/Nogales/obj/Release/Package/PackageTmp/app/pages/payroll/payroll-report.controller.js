'use strict';
MetronicApp.controller('PayrollReportController', ['$scope', 'dataService', '$filter', 'NotificationService', 'ApiUrl', 'commonService', '$controller', 'HelperService', function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller, HelperService) {

        $(function () {
            $("#datepicker").datepicker();
        });
        $scope.csvFileName = "PAYROLL-REPORT.CSV";
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        var httpRequest = null;
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
        $scope.searchClicked = false;
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
        };
        function GetAllEmployees() {
            dataService.GetAllEmployees()
           .then(function (response) {
               $scope.availableEmployees = response.data;
           });
        };
        GetAllEmployees();
       
     

        $scope.search = function (filter) {


            
            $scope.PayRollReport = [];
            $scope.PayRollReportMaster = [];



            $scope.iscanceldisabled = false;
            $scope.CurrentFilter = filter;
            $scope.searchClicked = true;
            var status = false;
            var date = {};
            if (filter) {
                date = HelperService.Getstartenddate(filter);
            }
            else {
                date = {
                    firstDay: $scope.beginDate,
                    lastDay: $scope.endDate
                }
            }
           
            $scope.beginDate = $filter('date')(date.firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(date.lastDay, 'MM/dd/yyyy');
            if ($scope.beginDate || date.lastDay) {
               
                status = (!$scope.beginDate || !$scope.endDate) ?
                        true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

                if (!status &&
                    ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                    $scope.btnSpinner.start();
                  
                    var param = {
                        Employees: getValueListFromKeyValueList(parsJasonFromArray($scope.selectedEmployees)),
                        StartDate: $scope.beginDate,
                        EndDate: $scope.endDate,
                    };

                    (httpRequest = dataService.GetPayRollData(param))
                        .then(function (response) {
                            debugger
                            if (response && response.data.PayRoll.length > 0) {
                           
                                $scope.iscanceldisabled = true;
                                $scope.PayRollReport = response.data.PayRoll;
                                $scope.PayRollReportMaster = angular.copy(response.data.PayRoll);

                                $scope.TotalOverTime = response.data.TotalOverTime;
                                $scope.TotalRegular = response.data.TotalRegular;
                                $scope.TotalTotal = response.data.TotalTotal;
                            }
                            $scope.btnSpinner.stop();
                        })
                        .catch(function (error) {
                            $scope.iscanceldisabled = true;
                            $scope.btnSpinner.stop();
                        });
                }
            }

        }
        function parsJasonFromArray(array) {
            var result = [];
            if (array && array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    result.push(JSON.parse(array[i]));
                }
            }
            return result;
        }
        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        }
        $scope.resetSearch = function () {

            $scope.PayRollReport = [];
            $scope.PayRollReportMaster = [];
  
           // $scope.clearInput();
            generateDefaultReport();

            $scope.search();
        };
        $scope.abortExecutingApi = function () {
            $scope.iscanceldisabled = true;
            $scope.btnSpinner.stop();
            try { return (httpRequest && httpRequest.abortCall());}
            catch (e) {}

        };
    //var csvheader = ["Employee", "IdNumber", "Department", "Status", "Supervisor", "Regular", "Overtime", "Orient", "Total"];
        
        $scope.getCsvHeader = function () {
            var csvheader = ["Employee", "ID Number", "Department", "Status", "Supervisor", "Regular", "Overtime", "Total"];
            return csvheader;
        };

        $scope.getCsvData = function () {
            debugger
            var array = [];
            var predefinedHeader = [];
            predefinedHeader = ["Employee", "Id", "Department", "Status", "Supervisor", "Regular", "Overtime", "Total"];
            angular.forEach($scope.PayRollReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] =  value;
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
         
            total[5] = $scope.TotalRegular;
            total[6] = $scope.TotalOverTime;
            total[7] = $scope.TotalTotal;
            array.push(total);
            return array;
        };
        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.search()
        };
        generateDefaultReport();
    }]);