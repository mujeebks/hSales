
'use strict';

MetronicApp.controller('WarehouseWmsPickerProductivityReportController', ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', 'commonService', '$controller',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, commonService, $controller) {
        $scope.reportByName = "EmployeeReport";
        $scope.reportTitle = '';
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.Currentshift = "Day";

        //$scope.isValidEndTime = true;

        $controller('BaseController', { $scope: $scope });


        $scope.settime = function (shift) {

            var options1 = {};
            var options2 = {};

            var picker1 = $('#timepicker-one').wickedpicker({ timeSeparator: ':', title: 'Set Time' });
            var picker2 = $('#timepicker-two').wickedpicker({ timeSeparator: ':', title: 'Set Time' });
            options1 = {
                now: ""
            };
            options2 = {
                now: ""
            };
            if (shift == "Night") {

                $scope.Currentshift = "Night";

                options1 = {
                    now: "16:00"
                };
                options2 = {
                    now: "2:00"
                };
                //  $scope.iscustomtime = false;

            } else if (shift == "Day") {
                $scope.Currentshift = "Day";

                options1 = {
                    now: "5:00"
                };
                options2 = {
                    now: "16:00"
                };
                $scope.iscustomtime = false;
            }
            else {

                $scope.Currentshift = "custom";
                options1 = {
                    now: "16:00"
                };
                options2 = {
                    now: "2:00"
                };
                // $scope.iscustomtime = true;
            }


            picker1.wickedpicker('setTime', 0, options1.now);
            picker2.wickedpicker('setTime', 0, options2.now);

        };

        $scope.Starttime = "5:00 AM";
        $scope.Endtime = "4:00 PM";
        $scope.settime("Day");


        $scope.loadDatePickers = function () {
            $('#startWorkDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
            $('#endWorkDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });



        }
        $scope.loadDatePickers();



        var httpRequest = null;
        var date = new Date();
        $scope.datafilter = {

            userId: "",
            startWorkDate: $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy'),
            endWorkDate: $filter('date')(date, 'MM/dd/yyyy'),
            startTime: '5:00 AM',
            endTime: '4:00 PM'

        };

        $scope.reportData = [];
        $scope.reportDataSafe = [];
        $scope.groupProperty = '';
        $scope.searchClicked = false;
        $scope.isDrillDown = false;
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.backToReport = function () {
            $scope.isDrillDown = false;
            $scope.reportByName = "EmployeeReport";
            var reportTitle = $scope.reportTitle.replace(" (" + $scope.selectedFiltervalue + ")", "");
            $scope.reportTitle = '';


        };
        $scope.getDetailedReport = function (item) {
            $scope.isDrillDown = true;
            // alert(item);
            //$scope.selectedFiltervalue = filterItem;
            $scope.reportTitle = " ( " + item.EmployeeId + " - " + item.Name.trim() + " - " + $filter('date')(item.StartTime, 'hh:mm a') + " - " + $filter('date')(item.EndTime, 'hh:mm a') + ")";
            $scope.reportByName = "SalesDetailedReport";

            $scope.salesDetailedReport = [];
            $scope.salesDetailedReportMaster = [];

            var totalDetailedExtendedPrice = 0;
            var totalDetailedMargin = 0;
            var totalDetailedQtyShipped = 0;
            //Search Parameters
            var searchFilter = {

                startWorkDate: $scope.datafilter.startWorkDate,
                endWorkDate: $scope.datafilter.endWorkDate,
                startTime: $scope.datafilter.startTime,
                endTime: $scope.datafilter.endTime,
                EmployeID: item.EmployeeId

            };

            Metronic.blockUI({ boxed: true });
            (httpRequest = dataService.GenerateWarehouseWmsPickerProductivityReportDetails(searchFilter))
                    .then(function (response) {
                        if (response) {

                            $scope.salesDetailedReport = response.data;
                            $scope.salesDetailedReportMaster = angular.copy(response.data);

                            $scope.totalPicked = item.PiecesPicked;


                        }
                        Metronic.unblockUI();
                        //$scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        var a = error;
                        Metronic.unblockUI();
                        //$scope.btnSpinner.stop();
                    });
        }
        //// Currently not using.
        //$scope.loadCharts = function () {

        //    Metronic.startPageLoading({ message: 'Loading Dashboard charts...' });
        //    dataService.getWarehouseDashboardData($filter('date')(new Date(), 'yyyy-MM-dd')).then(function (response) {
        //        try {
        //            //$scope.casesSoldData = response.data.CasesSoldTotals;
        //        } catch (e) {
        //            NotificationService.Error();
        //            NotificationService.ConsoleLog('Error while assigning the data from the API to the charts.');
        //        }
        //        finally {
        //            Metronic.stopPageLoading();
        //        }
        //    }, function onError() {
        //        Metronic.stopPageLoading();
        //        NotificationService.Error("Error upon the API request");
        //        NotificationService.ConsoleLog('Error on the server');
        //    });
        //}

        // Generate report
        $scope.search = function () {
            if ($scope.isDrillDown) {
                $scope.backToReport();
            }
            $scope.iscanceldisabled = false;
            $scope.searchClicked = true;

            if ($scope.reportBy == 1) {
                $scope.datafilter.endWorkDate = $scope.datafilter.startWorkDate;
            }

            //// Format the time
            //if ($scope.datafilter && $scope.datafilter.startTime && $scope.datafilter.startTime.length < 8) {
            //    $scope.datafilter.startTime = "0" + $scope.datafilter.startTime;
            //}
            //if ($scope.datafilter && $scope.datafilter.endTime && $scope.datafilter.endTime.length < 8) {
            //    $scope.datafilter.endTime = "0" + $scope.datafilter.endTime;
            //}
            //Metronic.startPageLoading({ animate: true });
            if (($scope.isDate($scope.datafilter.startWorkDate) && $scope.isDate($scope.datafilter.endWorkDate) &&
                !$scope.isPreviouseDate($scope.datafilter.startWorkDate, $scope.datafilter.endWorkDate)
                && $scope.isValidTime()
                )) {
                $scope.btnSpinner.start();
                (httpRequest = dataService.GenerateWarehouseWmsPickerProductivityReport($scope.datafilter))
                    .then(function onSuccess(response) {
                        if (response) {
                            $scope.iscanceldisabled = true;
                            $scope.reportData = response.data;
                            $scope.reportDataSafe = response.data;
                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        $scope.iscanceldisabled = true;
                        NotificationService.Error("An error occured while generating the report");

                        $scope.btnSpinner.stop();
                    });
            }
        }

        $scope.resetSearch = function () {
            $scope.datafilter = {

                userId: "",
                startWorkDate: $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy'),
                endWorkDate: $filter('date')(date, 'MM/dd/yyyy'),
                startTime: '5:00 AM',
                endTime: '4:00 PM'

            };
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.backToReport();
            $scope.datafilter.userId = "";

            $scope.reportData = [];
            $scope.reportDataSafe = [];
            //generateDefaultReport();
        };

        $scope.abortExecutingApi = function () {
            $scope.iscanceldisabled = true;

            return (httpRequest && httpRequest.abortCall());
        };

        $scope.isValidTime = function () {
            if ($scope.datafilter.endTime == '' && $scope.datafilter.startTime != '')
                return false;
            if ($scope.datafilter.endTime != '' && $scope.datafilter.startTime == '')
                return false;
            return true;

            //if ($scope.datafilter.endTime == "")
            //    $scope.isValidEndTime = true;

            //var index = $scope.datafilter.endTime.indexOf(":");
            //var endTime = parseFloat(parseInt($scope.datafilter.endTime.substr(0, index)) + "." + parseInt($scope.datafilter.endTime.substr(index + 1, 3)));
            //if ($scope.datafilter.endTime.substr($scope.datafilter.endTime.length - 2, 2) == "PM")
            //    endTime += 12;

            //var index = $scope.datafilter.startTime.indexOf(":");
            //var startTime = parseFloat(parseInt($scope.datafilter.startTime.substr(0, index)) + "." +parseInt($scope.datafilter.startTime.substr(index +1, 3)));
            //if($scope.datafilter.startTime.substr($scope.datafilter.startTime.length - 2, 2) == "PM")
            //    startTime += 12;

            //if (endTime < startTime)
            //    $scope.isValidEndTime = false;
            //$scope.isValidEndTime = true;
        }
        $scope.csvFileName = '';
        $scope.getCsvHeader = function () {
            $scope.csvFileName = 'WMS PICKER PRODUCTIVITY REPORT.CSV';
            if ($scope.isDrillDown) {

                $scope.csvFileName = 'WMS PICKER PRODUCTIVITY REPORT' + $scope.reportTitle + '.csv';
                $scope.csvFileName = $scope.csvFileName.toUpperCase();
                if ($scope.reportBy == 2)
                    return ['Item #', 'Item Description', 'Pieces Picked', 'Hours Worked', 'Pcs Per Hour'];
                else
                    return ['Item #', 'Item Description', 'Start Time', 'End Time', 'Pieces Picked', 'Hours Worked', 'Pcs Per Hour'];

            }
            else if ($scope.reportBy == 2) {
                return ['Emp Id', 'Name', 'Pieces Picked', 'Hours Worked', 'Pcs Per Hour'];
            }
            else
                return ['Emp Id', 'Name', 'Start Time', 'End Time', 'Pieces Picked', 'Hours Worked', 'Pcs Per Hour'];
        }
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            predefinedHeader = ['EmployeeId', 'Name', 'StartTime', 'EndTime', 'PiecesPicked', 'HoursWorked', 'PiecesPerHour'];
            if ($scope.isDrillDown) {
                predefinedHeader = ['Item', 'ItemDesc', 'StartTime', 'EndTime', 'PiecesPicked', 'HoursWorked', 'PiecesPerHour'];
                if ($scope.reportBy == 2) {
                    predefinedHeader = ['Item', 'ItemDesc', 'PiecesPicked', 'HoursWorked', 'PiecesPerHour'];
                }
            }
            else if ($scope.reportBy == 2) {

                //remove the start time and end time column
                predefinedHeader = ['EmployeeId', 'Name', 'PiecesPicked', 'HoursWorked', 'PiecesPerHour'];

            }

            var totalPicked = 0;
            angular.forEach($scope.isDrillDown ? $scope.salesDetailedReportMaster : $scope.reportDataSafe, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1 && key.toLowerCase().indexOf("time") >= 0)
                        header[index] = $filter('date')(value, 'hh:mm a');
                    else if (index > -1) {
                        header[index] = value;
                    }
                    if (key == "PiecesPicked") {
                        totalPicked = totalPicked + value;
                    }

                });
                if (header != undefined)
                    array.push(header);
            });
            var total = {};
            total[0] = "Total";
            total[1] = "";
            total[2] = $scope.reportBy == 1 ? "" : totalPicked;
            total[3] = "";
            total[4] = $scope.reportBy == 2 ? "" : totalPicked;
            total[5] = "";
            if ($scope.isDrillDown)
                array.push(total);
            return array;
        };


        //function generateDefaultReport() {
        //    if ($stateParams.date != undefined && $stateParams.date != null && $stateParams.date !="") {
        //        var date = new Date($stateParams.date);
        //        $scope.datafilter.startWorkDate = ("0" + (date.getMonth() + 1)).slice(-2) + "/" + ("0" + date.getDate()).slice(-2) + "/" + date.getFullYear();
        //        $scope.datafilter.endWorkDate = ("0" + (date.getMonth() + 1)).slice(-2) + "/" + ("0" + date.getDate()).slice(-2) + "/" + date.getFullYear();
        //    }
        //    else {
        //        $scope.datafilter.startWorkDate = getMonthStartday();
        //        $scope.datafilter.endWorkDate = getToday();
        //    }

        //    $scope.search();
        //}

        //function getToday() {
        //    var today = new Date();
        //    var dd = today.getDate();
        //    var mm = today.getMonth() + 1; //January is 0!

        //    var yyyy = today.getFullYear();
        //    if (dd < 10) {
        //        dd = '0' + dd
        //    }
        //    if (mm < 10) {
        //        mm = '0' + mm
        //    }

        //    var today = mm + '/' + dd + '/' + yyyy;
        //    return today;
        //}


        //function getMonthStartday() {
        //    var today = new Date();
        //    var mm = today.getMonth() + 1; //January is 0!
        //    var yyyy = today.getFullYear();

        //    if (mm < 10) {
        //        mm = '0' + mm
        //    }

        //    var today = mm + '/' + '01' + '/' + yyyy;
        //    return today;
        //}

        //generateDefaultReport();

    }]);
