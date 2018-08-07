
'use strict';

MetronicApp.controller("WarehouseController", ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', '$controller','storageService',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, $controller, storageService) {
    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        Metronic.initAjax();
    });

    $rootScope.$on('warehouse', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        filterChart(filter);
    });
    $controller('BaseController', { $scope: $scope });

    //$scope.showForecastReport = false;

    $scope.reportData = [];
    $scope.reportDataSafe = [];
    $scope.groupProperty = '';
    $scope.caption_subjectpickerproductivityChartName = "Picker Productivity Chart";
    $scope.caption_subjectfnpickerproductivityChart = function (title) {
        $scope.caption_subjectpickerproductivityChartName = (title == "initial") ? "Picker Productivity Chart" : title;
    }

    $scope.totalChartData = [];
   
    function filterChart(filter) {

        Metronic.blockUI({ boxed: true });
        dataService.getMainDashboardPickerProductivityChartData(filter).then(function (response) {
            try {
                $scope.totalChartData = response.data;
                Metronic.unblockUI();
            } catch (e) {
                NotificationService.Error();
                Metronic.unblockUI();
                NotificationService.ConsoleLog('Error while assigning the data from the API to the chart.');
            }
            finally {
                storageService.setStorage('CurrentFilter', filter)
                Metronic.stopPageLoading();
            }

        }, function onError() {
            Metronic.unblockUI();
            Metronic.stopPageLoading();
            NotificationService.Error("Error upon the API request");
            NotificationService.ConsoleLog('Error on the server');
        });
    }

    filterChart($rootScope.currentfilter)
   

    //$scope.monthNames = ["January", "February", "March", "April", "May", "June",
    //        "July", "August", "September", "October", "November", "December"
    //];
    //$scope.foreCastDates = [];
    //$scope.foreCastDates =  getNextSixMonthDates(new Date());
    //function getNextSixMonthDates(date) {
    //    var nextSixDates = [];
    //    var month = date.getMonth() + 1;
    //    var year = date.getFullYear();
    //    var yearFlag = false;
    //    for (var i = 0; i < 6; i++) {
    //        var monthMM = (month < 10) ? ("0" + month) : month;
    //        nextSixDates.push(year + "-" + monthMM + "-" + "01");

    //        yearFlag = ((month == 12 && yearFlag == false) ? true : false)
    //        year = (yearFlag) ? year + 1 : year;
    //        yearFlag = (yearFlag) ? false : yearFlag;
    //        month = (month < 12) ? month + 1 : 1;
    //    }
    //    return nextSixDates;
    //}
    //$scope.foreCast = {
    //     value:0,
    //     fromDate: getPreviousYearDate($scope.foreCastDates[0]),
    //     onDate:$scope.foreCastDates[0],
    //     OnLabel: $scope.monthNames[($scope.foreCastDates[0].split("-")[1]) - 1] + "  " + $scope.foreCastDates[0].split("-")[0],
    // };
    // $scope.setForeCastOn = function (date) {
    //     $scope.foreCast.OnLabel = $scope.monthNames[(date.split("-")[1]) - 1] + "  " + date.split("-")[0];
    //     $scope.foreCast.onDate = date;
    //     $scope.foreCast.fromDate = getPreviousYearDate(date);
    // }
    // function getPreviousYearDate(date) {
    //     var cDate = new Date(date.split("-")[0], date.split("-")[1] - 1, date.split("-")[2]);
    //     var previousYeardate = new Date(cDate.getFullYear() - 1, cDate.getMonth(), cDate.getDate());
    //     var month = previousYeardate.getMonth() + 1;
    //     month = (month < 10) ? "0" + month : month;
    //     return previousYeardate.getFullYear() + "-" + month + "-" + previousYeardate.getDate();
    // }

    // $scope.searchForeCast = function () {
    //     //$scope.showForecastReport = false;
    //     $scope.reportData = [];
    //     $scope.reportDataSafe = [];
    //     Metronic.startPageLoading({ message: "Fetching Forecast Data" });

    //     var date = new Date();
    //     date.setMonth($scope.foreCast.fromDate.split("-")[1] - 1);
    //     var firstDay = new Date($scope.foreCast.fromDate.split("-")[0], date.getMonth(), 1);
    //     var lastDay = new Date($scope.foreCast.fromDate.split("-")[0], date.getMonth() + 1, 0);
    //     var startDate =  $filter('date')(firstDay, 'yyyy-MM-dd');
    //     var endDate = $filter('date')(lastDay, 'yyyy-MM-dd');

    //     dataService.getForcastResult(startDate, endDate).then(function (response) {
    //         try {
    //             Metronic.stopPageLoading();
    //             $scope.foreCast.value = response.data;
    //             if ($scope.showForecastReport) {
    //                $scope.getForeCastReport();
    //             }
    //             //$scope.showForecastReport = true;
    //         } catch (e) {
    //             Metronic.stopPageLoading();
    //             NotificationService.Error();
    //             NotificationService.ConsoleLog('Error while assigning the data from the API to the forecast data.');
    //         }
    //         finally {
    //             //Metronic.stopPageLoading();
    //         }

    //     }, function onError() {
    //         Metronic.stopPageLoading();
    //         NotificationService.Error("Error upon the API request");
    //         NotificationService.ConsoleLog('Error on the server');
    //     });
    // }
    // $scope.searchForeCast();

     //$scope.getForeCastReport = function () {
     //    if ($scope.showForecastReport) {
     //        //Metronic.startPageLoading({ message: "Fetching ForeCast Report Data" });
     //                     var date = new Date();
     //        date.setMonth($scope.foreCast.fromDate.split("-")[1] - 1);
     //        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
     //        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
     //        var startDate = $filter('date')(firstDay, 'yyyy-MM-dd');
     //        var endDate = $filter('date')(lastDay, 'yyyy-MM-dd');
     //        dataService.GetForcastReport(startDate, endDate).then(function (response) {
     //            try {
     //                $scope.reportData = response.data;
     //                $scope.reportDataSafe = response.data;
     //            } catch (e) {
     //                NotificationService.Error();
     //                NotificationService.ConsoleLog('Error while assigning the data from the API to the forecast report.');
     //            }
     //            finally {
     //                Metronic.stopPageLoading();
     //            }
     //        }, function onError() {
     //            Metronic.stopPageLoading();
     //            NotificationService.Error("Error upon the API request");
     //            NotificationService.ConsoleLog('Error on the server');
     //        });
     //    }
     //}

    // $scope.resetForecast = function(){
    //     $scope.foreCast = {
    //         value: 0,
    //         fromDate: getPreviousYearDate($scope.foreCastDates[0]),
    //         onDate: $scope.foreCastDates[0],
    //         OnLabel: $scope.monthNames[($scope.foreCastDates[0].split("-")[1]) - 1] + "  " + $scope.foreCastDates[0].split("-")[0],
    //     };
    //     $scope.reportData = [];
    //     $scope.reportDataSafe = [];
    //}
}])