
'use strict';

MetronicApp.controller("WarehouseController", ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', '$controller',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, $controller) {
    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        Metronic.initAjax();
    });
    $controller('BaseController', { $scope: $scope });
    $scope.filter = {
        //type: 'month',
        label: 'Year To Date',
        //month: '',
        //availableMonths: ['January ', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        //currentMonth: new Date().getMonth()
    };

    //startdivloader('abcd');
    //startdivloader('xyz');

    $scope.showForecastReport = false;

    $scope.reportData = [];
    $scope.reportDataSafe = [];
    $scope.groupProperty = '';

    //$scope.onDate = ((new Date().getDay()) < 6) ? ((new Date().getDay()) + 2) : 1

    //$scope.foreCast = {
    //    on: $scope.onDate,
    //    label: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][$scope.onDate - 1],
    //    foreCastFrom:"lastWeek",
    //    fromDate: "",
    //    toDate:"",
    //    value: "",
    //    fromLabel: "Last Week"
    //};
    //$scope.forCastOnlabel = function (labelName, on) {
    //    $scope.foreCast.label = labelName;
    //    $scope.foreCast.on = (on<7)?on+1:1;
    //}
    //$scope.foreCastFromLabel = function (labelName) {
    //    $scope.foreCast.fromLabel = labelName;
    //}

    $scope.flabel = function (labelName) {

        $scope.filter.label = labelName;
    }
    $scope.totalChartData = [];
    $scope.resetFilter = function () {
        $scope.filter.type = 'month';
        $scope.filter.label = 'Year To Date';
        // Update the chart
        loadDefaultChart();
    }
    function getQuarter(d) {
        d = d || new Date();
        var m = Math.floor(d.getMonth() / 3) + 1;
        return m > 4 ? m - 4 : m;
    }
    $scope.search = function (filter, monthIndex) {
        var status = false;
        var date = new Date();
        var firstDay;
        var lastDay;

        if (filter == 'YTD') {
            firstDay = new Date(date.getFullYear(), 0, 1);
            lastDay = new Date();
        }
        else if (filter == 'LastYear') {
            firstDay = new Date(date.getFullYear() - 1, 0, 1);
            lastDay = new Date(date.getFullYear(), 0, 0);
        }
        else if (filter == 'LastTwoYear') {
            firstDay = new Date(date.getFullYear() - 2, 0, 1);
            lastDay = new Date();
        }
        else if (filter == 'LastThreeYear') {
            firstDay = new Date(date.getFullYear() - 3, 0, 1);
            lastDay = new Date();
        }
        $scope.startDate = $filter('date')(firstDay, 'yyyy-MM-dd');
        $scope.endDate = $filter('date')(lastDay, 'yyyy-MM-dd');

        $scope.dfDateFilter = "Custom";
        var filter = {
            Description : "This Month",
            Id:3
        }

        filterChart(filter);

    }
    function filterChart(filter) {
        //var msg = loadingMessage ? loadingMessage : 'Fetching warehouse charts...';
        //Metronic.startPageLoading({ message: msg });
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
                Metronic.stopPageLoading();
            }

        }, function onError() {
            Metronic.unblockUI();
            Metronic.stopPageLoading();
            NotificationService.Error("Error upon the API request");
            NotificationService.ConsoleLog('Error on the server');
        });
    }
    function loadDefaultChart() {
        // Load charts
        var date = new Date();
        var sDate = $scope.startDate = $filter('date')(new Date(date.getFullYear(), 0, 1), 'yyyy-MM-dd');
        var eDate = $scope.endDate = $filter('date')(date, 'yyyy-MM-dd');

        var filter = {
            Description: "This Month",
            Id: 3
        }

        filterChart(filter);
    }
    loadDefaultChart();

    //$scope.searchForeCast = function () {
    //    //Metronic.startPageLoading();
    //    var date = new Date();
    //    var firstDay;
    //    var lastDay;
    //    if ($scope.foreCast.foreCastFrom == "lastWeek")
    //    {
    //        var fd = new Date(date.setDate(date.getDate() - date.getDay() - 7));

    //        firstDay = fd;
    //        lastDay = new Date(date.setDate(date.getDate() - date.getDay() + 6));
    //    }
    //    else if ($scope.foreCast.foreCastFrom == "lastMonth") {
    //        var month = date.getMonth();
    //        var year;
    //        if (month < 12)
    //            year = date.getFullYear();
    //        else
    //            year = date.getFullYear() - 1;
    //        firstDay = new Date(year, month - 1, 1);
    //        lastDay = new Date(year, month, 0);
    //    }
    //    else if ($scope.foreCast.foreCastFrom == "lastQuarter") {
    //        var currQuarter = getQuarter(date);
    //        firstDay = new Date(date.getFullYear(), 3 * currQuarter - 3, 1);
    //        lastDay = new Date(date.getFullYear(), 3 * currQuarter, 0);
    //    }
    //    else if ($scope.foreCast.foreCastFrom == 'YTD') {
    //        firstDay = new Date(date.getFullYear(), 0, 1);
    //        lastDay = new Date();
    //    }


    //    $scope.foreCast.fromDate = $filter('date')(firstDay, 'yyyy-MM-dd');
    //    $scope.foreCast.toDate = $filter('date')(lastDay, 'yyyy-MM-dd');

    //    dataService.getForcastResult($scope.foreCast.on, $scope.foreCast.fromDate, $scope.foreCast.toDate).then(function (response) {
    //        try {
    //            $scope.foreCast.value = response.data;
    //        } catch (e) {
    //            NotificationService.Error();
    //            NotificationService.ConsoleLog('Error while assigning the data from the API to the forecast data.');
    //        }
    //        finally {
    //            //Metronic.stopPageLoading();
    //        }

    //    }, function onError() {
    //        //Metronic.stopPageLoading();
    //        NotificationService.Error("Error upon the API request");
    //        NotificationService.ConsoleLog('Error on the server');
    //    });
    //}
    //$scope.searchForeCast();


    $scope.monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
    ];
    $scope.foreCastDates = [];
    $scope.foreCastDates =  getNextSixMonthDates(new Date());
    function getNextSixMonthDates(date) {
        var nextSixDates = [];
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var yearFlag = false;
        for (var i = 0; i < 6; i++) {
            var monthMM = (month < 10) ? ("0" + month) : month;
            nextSixDates.push(year + "-" + monthMM + "-" + "01");

            yearFlag = ((month == 12 && yearFlag == false) ? true : false)
            year = (yearFlag) ? year + 1 : year;
            yearFlag = (yearFlag) ? false : yearFlag;
            month = (month < 12) ? month + 1 : 1;
        }
        return nextSixDates;
    }
    $scope.foreCast = {
         value:0,
         fromDate: getPreviousYearDate($scope.foreCastDates[0]),
         onDate:$scope.foreCastDates[0],
         OnLabel: $scope.monthNames[($scope.foreCastDates[0].split("-")[1]) - 1] + "  " + $scope.foreCastDates[0].split("-")[0],
     };
     $scope.setForeCastOn = function (date) {
         $scope.foreCast.OnLabel = $scope.monthNames[(date.split("-")[1]) - 1] + "  " + date.split("-")[0];
         $scope.foreCast.onDate = date;
         $scope.foreCast.fromDate = getPreviousYearDate(date);
     }
     function getPreviousYearDate(date) {
         var cDate = new Date(date.split("-")[0], date.split("-")[1] - 1, date.split("-")[2]);
         var previousYeardate = new Date(cDate.getFullYear() - 1, cDate.getMonth(), cDate.getDate());
         var month = previousYeardate.getMonth() + 1;
         month = (month < 10) ? "0" + month : month;
         return previousYeardate.getFullYear() + "-" + month + "-" + previousYeardate.getDate();
     }

     $scope.searchForeCast = function () {
         //$scope.showForecastReport = false;
         $scope.reportData = [];
         $scope.reportDataSafe = [];
         Metronic.startPageLoading({ message: "Fetching Forecast Data" });

         var date = new Date();
         date.setMonth($scope.foreCast.fromDate.split("-")[1] - 1);
         var firstDay = new Date($scope.foreCast.fromDate.split("-")[0], date.getMonth(), 1);
         var lastDay = new Date($scope.foreCast.fromDate.split("-")[0], date.getMonth() + 1, 0);
         var startDate =  $filter('date')(firstDay, 'yyyy-MM-dd');
         var endDate = $filter('date')(lastDay, 'yyyy-MM-dd');

         dataService.getForcastResult(startDate, endDate).then(function (response) {
             try {
                 Metronic.stopPageLoading();
                 $scope.foreCast.value = response.data;
                 if ($scope.showForecastReport) {
                    $scope.getForeCastReport();
                 }
                 //$scope.showForecastReport = true;
             } catch (e) {
                 Metronic.stopPageLoading();
                 NotificationService.Error();
                 NotificationService.ConsoleLog('Error while assigning the data from the API to the forecast data.');
             }
             finally {
                 //Metronic.stopPageLoading();
             }

         }, function onError() {
             Metronic.stopPageLoading();
             NotificationService.Error("Error upon the API request");
             NotificationService.ConsoleLog('Error on the server');
         });

     }
     $scope.searchForeCast();

     $scope.getForeCastReport = function () {
         if ($scope.showForecastReport) {
             Metronic.startPageLoading({ message: "Fetching ForeCast Report Data" });



             var date = new Date();
             date.setMonth($scope.foreCast.fromDate.split("-")[1] - 1);
             var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
             var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
             var startDate = $filter('date')(firstDay, 'yyyy-MM-dd');
             var endDate = $filter('date')(lastDay, 'yyyy-MM-dd');
             dataService.GetForcastReport(startDate, endDate).then(function (response) {
                 try {

                     $scope.reportData = response.data;
                     $scope.reportDataSafe = response.data;
                 } catch (e) {
                     NotificationService.Error();
                     NotificationService.ConsoleLog('Error while assigning the data from the API to the forecast report.');
                 }
                 finally {
                     Metronic.stopPageLoading();
                 }

             }, function onError() {
                 Metronic.stopPageLoading();
                 NotificationService.Error("Error upon the API request");
                 NotificationService.ConsoleLog('Error on the server');
             });
         }


     }

     $scope.resetForecast = function(){
         $scope.foreCast = {
             value: 0,
             fromDate: getPreviousYearDate($scope.foreCastDates[0]),
             onDate: $scope.foreCastDates[0],
             OnLabel: $scope.monthNames[($scope.foreCastDates[0].split("-")[1]) - 1] + "  " + $scope.foreCastDates[0].split("-")[0],
         };
         $scope.reportData = [];
         $scope.reportDataSafe = [];
    }

     $scope.getCsvHeader = function () {
         return ['Name', 'Pieces Picked', 'Hours Worked', 'Pieces Per Hour'];
     }
     $scope.getCsvData = function () {
         var array = [];
         var predefinedHeader = [];
         predefinedHeader = ["Name", "PiecesPicked", 'HoursWorked', 'PiecesPerHour'];

         angular.forEach($scope.reportDataSafe, function (value, key) {
             var header = {};
             angular.forEach(value, function (value, key) {
                 var index = predefinedHeader.indexOf(key);
                 if (index > -1) {
                     header[index] = value;
                 }

             });
             if (header != undefined)
                 array.push(header);
         });
         return array;
     };


}])