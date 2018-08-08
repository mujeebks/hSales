//TransportationRouteReportController

'use strict';
MetronicApp.controller('TransportationRouteReportController', ['$scope', 'dataService', '$filter', 'NotificationService', 'ApiUrl', 'commonService', '$controller', 'HelperService',

    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller, HelperService) {

        $scope.Daycsv = "";
        $scope.ReportHeading = "Route Report";
        $scope.ReportheadAttachment = "";
        $scope.DetailedDayReport = true;
        $scope.detailtoggle = false;
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.transportationdrivercsv = "";
        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.transportationDriverTableFunctions = {};
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
        $scope.searchClicked = false;
        $scope.selecteditem = '';
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

        $scope.toggledetailreport = true;
        $scope.TotalNoofTripDriver = 0;
        $scope.TotalNoofStops = 0;
        $scope.TotalNoofCases = 0;


        $scope.TotalNoofTripsDetail = 0;
        $scope.TotalNoofStopsDetail = 0;
        $scope.TotalNoofCasesDetail = 0;
        $scope.GridtotalCasedDelivered = 0;
        $scope.GridTotalforCasesDeliveredCustomerGrid = 0;
        $scope.GridnoofinvoicestotalforCustomergrid = 0;
        $scope.groupPropertycustomer = '';


        $scope.GetDriverDayAndDetailedReport = function (Route, RouteType) {
            Metronic.blockUI({ boxed: true });
            $scope.dayReport = "DRIVER_ROUTE_BY_DAY_REPORT_" + Route + ".CSV";
            $scope.detailReport = "DRIVER_ROUTE_BY_DETAIL_REPORT_" + Route + ".CSV";
            $scope.DetailedDayReport = true;
            $scope.ReportheadAttachment = "(" + Route  + ")";
            $scope.detailtoggle = true;
            var param = {

                StartDate: $scope.beginDate,
                EndDate: $scope.endDate,
                driverCode: " ",
                route: Route
            };



            (httpRequest = dataService.GetDriverDayAndDetailedReport(param))
                .then(function (response) {
                    if (response) {
                        $scope.iscanceldisabled = true;
                        $scope.transportationdriverdetailcsv = "TRANSPORTATION-ROUTE-REPORT.CSV";
                        $scope.transportationdriverdetailcsv = $scope.transportationdrivercsv.toUpperCase();

                        $scope.DriverDayReportList = response.data.DayReport;
                        $scope.DriverDayReportListCopy = angular.copy(response.data.DayReport);

                        $scope.DriverDetailedReportList = response.data.DetailedReport;
                        $scope.DriverDetailedReportListCopy = angular.copy(response.data.DetailedReport);


                        $scope.CustomerReport = response.data.CustomerReport;
                        $scope.CustomerReportCopy = response.data.CustomerReport;


                        var TotalPrevious = 0;
                        var TotalCurrent = 0;
                        var TotalPreviousDetail = 0;
                        var TotalCurrentDetail = 0;
                        var TotalPreviousDay = 0;
                        var TotalCurrentDay = 0;
                        var totaltrip = 0;
                        var totalstop = 0;
                        var totalcases = 0;
                        var casesdeliveredtotalfordetailgrid = 0;
                        var casesdeliveredtotalforCustomergrid = 0;
                        var casesnoofinvoicesCustomergrid = 0;
                        var noofinvoicestotalforCustomergrid = 0;

                        for (var i = 0; i < $scope.DriverDetailedReportList.length; i++) {

                            TotalPreviousDetail = TotalPrevious + $scope.DriverDetailedReportList[i].Previous;
                            TotalCurrentDetail = TotalCurrent + $scope.DriverDetailedReportList[i].Current;
                            casesdeliveredtotalfordetailgrid = casesdeliveredtotalfordetailgrid + Number.parseFloat($scope.DriverDetailedReportList[i].CasesDelivered + '');
                        }

                        for (var i = 0; i < $scope.CustomerReport.length; i++) {

                            TotalPreviousDetail = TotalPrevious + $scope.CustomerReport[i].Previous;
                            TotalCurrentDetail = TotalCurrent + $scope.CustomerReport[i].Current;
                            casesdeliveredtotalforCustomergrid = casesdeliveredtotalforCustomergrid + Number.parseFloat($scope.CustomerReport[i].CasesDelivered + '');
                            noofinvoicestotalforCustomergrid = noofinvoicestotalforCustomergrid + Number.parseFloat($scope.CustomerReport[i].NumberOfInvoices + '');
                        }


                        for (var i = 0; i < $scope.DriverDayReportList.length; i++) {
                            debugger;
                            TotalPreviousDay = TotalPrevious + $scope.DriverDayReportList[i].Previous;
                            TotalCurrentDay = TotalCurrent + $scope.DriverDayReportList[i].Current;
                            totaltrip = totaltrip + $scope.DriverDayReportList[i].NumberOfTrips;
                            totalstop = totalstop + $scope.DriverDayReportList[i].NumberOfStops;
                            totalcases = totalcases + Number.parseFloat($scope.DriverDayReportList[i].CasesDelivered + '');
                        }

                        $scope.TotalPrevious = $filter('numberWithCommasRounded')(TotalPreviousDetail);
                        $scope.TotalCurrent = $filter('numberWithCommasRounded')(TotalCurrentDetail);
                        $scope.TotalNoofTripsDetail = $filter('numberWithCommasRounded')(totaltrip);
                        $scope.TotalNoofStopsDetail = $filter('numberWithCommasRounded')(totalstop); debugger;
                        $scope.TotalNoofCasesDetail = Number.parseInt(totalcases);
                        $scope.GridtotalCasedDelivered = Number.parseInt(casesdeliveredtotalfordetailgrid);
                        $scope.GridTotalforCasesDeliveredCustomerGrid = Number.parseInt(casesdeliveredtotalforCustomergrid);
                        $scope.GridnoofinvoicestotalforCustomergrid = Number.parseInt(noofinvoicestotalforCustomergrid);

                    }
                    Metronic.unblockUI();
                    $scope.btnSpinner.stop();
                })
                .catch(function (error) {
                    Metronic.unblockUI();
                    $scope.btnSpinner.stop();
                });
        }


        $scope.backtoMainList = function () {
            $scope.ReportheadAttachment = "";
            $scope.detailtoggle = false;
        }

        $scope.$watch($scope.toggledetailreport, function () {

            $scope.DetailedDayReport = ($scope.toggledetailreport) ? true : false;


        });

        $scope.toggledetailReport = function (data) {

            $scope.DetailedDayReport = data;
        }

        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }


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
                debugger;
                (httpRequest = dataService.GetRouteConsolidatedReport(param))
                    .then(function (response) {
                        if (response) {
                            $scope.iscanceldisabled = true;
                            $scope.transportationdrivercsv = "TRANSPORTATION-ROUTE-REPORT.CSV";
                            $scope.transportationdrivercsv = $scope.transportationdrivercsv.toUpperCase();
                            $scope.RouteConsolidatedReportList = response.data;
                            $scope.RouteConsolidatedReportListCopy = angular.copy(response.data);


                            var TotalPrevious = 0;
                            var TotalCurrent = 0;
                            var totaldrivers = 0;
                            var totalstop = 0;
                            var totalcases = 0;
                            for (var i = 0; i < $scope.RouteConsolidatedReportList.length; i++) {

                                TotalPrevious = TotalPrevious + $scope.RouteConsolidatedReportList[i].Previous;
                                TotalCurrent = TotalCurrent + $scope.RouteConsolidatedReportList[i].Current;
                                totaldrivers = totaldrivers + $scope.RouteConsolidatedReportList[i].NumberOfDrivers;
                                totalstop = totalstop + $scope.RouteConsolidatedReportList[i].NumberOfStops;
                                totalcases = totalcases + Number.parseFloat($scope.RouteConsolidatedReportList[i].CasesDelivered + '');

                            }
                            $scope.TotalPrevious = $filter('numberWithCommasRounded')(TotalPrevious);
                            $scope.TotalCurrent = $filter('numberWithCommasRounded')(TotalCurrent);
                            $scope.TotalNoofTripDriver = $filter('numberWithCommasRounded')(totaldrivers);
                            $scope.TotalNoofStops = $filter('numberWithCommasRounded')(totalstop); debugger;
                            $scope.TotalNoofCases = totalcases;

                        }
                        $scope.detailtoggle = false;
                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                        $scope.btnSpinner.stop();
                    });

            }
        }


        $scope.abortExecutingApi = function () {
            $scope.searchClicked = false;
            return (httpRequest && httpRequest.abortCall());
        };


        $scope.resetSearch = function () {
          //  $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.RouteConsolidatedReportList = [];
            $scope.RouteConsolidatedReportListCopy = [];
            generateDefaultReport();
        };


        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.search();

        }

        generateDefaultReport();




        $scope.getCsvHeader = function () {
            return ['Route', 'Route Type', 'Number Of Drivers', 'Number Of Stops', 'Cases Delivered'];
        }

        $scope.getCsvHeaderDay = function () {
            return ['Driver Code', 'Driver Name', ' Date','Route Type',   'Number Of Stops', 'Cases Delivered'];
        }

        $scope.getCsvHeaderDetail = function () {
            return ['Driver Code', 'Driver Name', 'Truck Code', 'Route', 'Route Type',  'Date',
                'Invoice Number', 'Customer Name', 'Customer Code', 'Address', 'City', 'State', 'Zip', 'Reference', 'Cases Delivered'];
        }


        $scope.getCsvHeaderCustomer = function () {
            return ['Driver Code', 'Driver Name', 'Truck Code', 'Route', 'Route Type',  'Date',
                'Customer Name', 'Customer Code', 'Address', 'City', 'State', 'Zip', 'Reference', 'Number Of Invoices', 'Cases Delivered'];
        }


        $scope.clickedButtonFn = function (data) {

            $scope.CurrentData = data;
        }


        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var totaltripsDrivers = 0;
            var totalstops = 0;
            var totalcases = 0;
            var TotalPriorRevenue = 0;

            predefinedHeader = ['Route', 'RouteType', 'NumberOfDrivers', 'NumberOfStops', 'CasesDelivered'];

            angular.forEach($scope.RouteConsolidatedReportListCopy, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {

                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        if (key == "CasesDelivered")
                            header[index] = $filter('numberWithCommasRounded')(value);
                        else
                            header[index] = value;

                    }

                    if (key == "NumberOfStops") {
                        totalstops = totalstops + value;
                    }
                    if (key == "NumberOfDrivers") {
                        totaltripsDrivers = totaltripsDrivers + value;
                    }
                    if (key == "CasesDelivered") {
                        totalcases = totalcases + Number.parseFloat(value + '');
                    }


                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
           
            total[1] = "";
            total[2] = totaltripsDrivers;
            total[3] = totalstops;
            total[4] = $filter('numberWithCommasRounded')(Number.parseInt(totalcases));

            array.push(total);

            return array;
        };


        $scope.getCsvDataDay = function () {
            var array = [];
            var predefinedHeader = [];
            var totaltrips = 0;
            var totalstops = 0;
            var TotalPriorRevenue = 0;
            var totalcases = 0;

            predefinedHeader = ['DriverCode', 'DriverName', 'InvoiceDateString', 'RouteType',  'NumberOfStops', 'CasesDelivered'];

            angular.forEach($scope.DriverDayReportListCopy, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {

                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        header[index] = value;
                    }

                    if (key == "NumberOfStops") {
                        totalstops = totalstops + value;
                    }
                    if (key == "NumberOfTrips") {
                        totaltrips = totaltrips + value;
                    }
                    if (key == "CasesDelivered") {
                        totalcases = totalcases + Number.parseFloat(value + '');
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
          
            total[4] = totalstops;
            total[5] = $filter('numberWithCommasRounded')(Number.parseInt(totalcases));

            array.push(total);

            return array;
        };

        $scope.getCsvDataDetail = function () {
            var array = [];
            var predefinedHeader = [];
            var totaltrips = 0;
            var totalstops = 0;
            var TotalPriorRevenue = 0;
            var totalcases = 0;

            predefinedHeader = ['DriverCode', 'DriverName', 'TruckCode', 'Route', 'RouteType',  'InvoiceDateString',
                'InvoiceNumber', 'CustomerName', 'CustomerCode', 'Address', 'City', 'State', 'Zip', 'Reference', 'CasesDelivered'];

            angular.forEach($scope.DriverDetailedReportListCopy, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {

                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        header[index] = value;
                    }


                    if (key == "CasesDelivered") {
                        totalcases = totalcases + Number.parseFloat(value + '');
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
            total[5] = "";
            total[6] = "";
            total[7] = "";
            total[8] = "";
            total[9] = "";
            total[10] = "";
            total[11] = "";
            total[12] = "";
            total[13] = "";
           
            total[14] = $filter('numberWithCommasRounded')(Number.parseInt(totalcases));

            array.push(total);

            return array;
        };

        $scope.getCsvDataDetailCustomer = function () {
            var array = [];
            var predefinedHeader = [];
            var totaltrips = 0;
            var totalstops = 0;
            var TotalPriorRevenue = 0;
            var totalcases = 0;
            var invoice = 0;

            predefinedHeader = ['DriverCode', 'DriverName', 'TruckCode', 'Route', 'RouteType',  'InvoiceDateString',
                'CustomerName', 'CustomerCode', 'Address', 'City', 'State', 'Zip', 'Reference',  'NumberOfInvoices', 'CasesDelivered'];

            angular.forEach($scope.CustomerReportCopy, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {

                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        header[index] = value;
                    }


                    if (key == "CasesDelivered") {
                        totalcases = totalcases + Number.parseFloat(value + '');
                    }

                        if (key == "NumberOfInvoices") {
                            invoice = invoice + Number.parseFloat(value + '');
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
            total[5] = "";
            total[6] = "";
            total[7] = "";
            total[8] = "";
            total[9] = "";
            total[10] = "";
            total[11] = "";
            total[12] = "";
          
            total[13] = invoice;
            total[14] = $filter('numberWithCommasRounded')(Number.parseInt(totalcases));

            array.push(total);

            return array;
        };

    }]);
