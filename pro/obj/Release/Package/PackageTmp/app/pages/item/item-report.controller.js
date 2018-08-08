
'use strict';


MetronicApp.controller('ItemReportController', ['$scope', 'dataService', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller) {
        $scope.reportBy = "EmployeeReport";
        $scope.isDrillDown = false;
        $scope.reportTitle = '';
        $scope.csvFileName = "AVG-COST-REPORT.CSV";
        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();
        });
        $scope.arrayobj = [];
        $scope.selectedcallback = function (item) {



            $scope.selectedItems = item;
            $scope.arrayobj = [];

            for (var i = 0; i < item.length; i++) {
                $scope.arrayobj.push(item[i].Key)
            }

            $scope.search();
        };





        $scope.selectedData = null;
        $scope.iscanceldisabled = true;

        $scope.onSelect = function (selection) {
            $scope.selectedData = selection;
            // $scope.search();

        };


        $scope.clearInput = function () {
            $scope.$broadcast('simple-autocomplete:clearInput');
            $scope.selectedData = '';
            // $scope.search();
        };

        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });

        $scope.btnSpinner = commonService.InitBtnSpinner('#search');

        $scope.getDetailedReport = function (item) {
            $scope.isDrillDown = true;
            $scope.csvFileName = "Avg-Cost-Report-" + item.ItemDesc.trim() + ".csv";
            $scope.csvFileName = $scope.csvFileName.toUpperCase();
            // alert(item);
            //$scope.selectedFiltervalue = filterItem;
            //console.log(item);
            $scope.reportTitle = " (" + item.Item + " - " + item.ItemDesc + " - " + $scope.beginDate + " " + $scope.endDate + ")";
            $scope.reportBy = "SalesDetailedReport";

            $scope.salesDetailedReport = [];
            $scope.salesDetailedReportMaster = [];

            var totalDetailedExtendedPrice = 0;
            var totalDetailedMargin = 0;
            var totalDetailedQtyShipped = 0;
            //Search Parameters
            var searchFilter = {

                currentStartDate: $scope.beginDate,
                currentEndDate: $scope.endDate,
                Item: item.Item,
                Week: item.Week,
                Month: item.Month,
                Year: item.Year

            };

            Metronic.blockUI({ boxed: true });
            (httpRequest = dataService.getItemReportDrillDown(searchFilter))
                    .then(function (response) {
                        if (response) {

                            $scope.salesDetailedReport = response.data;
                            $scope.salesDetailedReportMaster = angular.copy(response.data);

                            $scope.avgCost = item.Cost;
                            $scope.avgPrize = item.Price;

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

        $scope.backToReport = function () {
            $scope.csvFileName = "Avg-Cost-Report.csv";
            $scope.csvFileName = $scope.csvFileName.toUpperCase();
            $scope.isDrillDown = false;
            $scope.reportBy = "EmployeeReport";
            var reportTitle = $scope.reportTitle.replace(" (" + $scope.selectedFiltervalue + ")", "");
            $scope.reportTitle = '';


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

        //$scope.getItemFilters = function () {
        //    dataService.getItemReportFilters()
        //   .then(function (response) {
        //       $scope.availableItems = response.data.ListItem;
        //       $scope.datas = response.data.ListItem;
        //
        //       //$scope.availableSalesPersons = getDummyAvailableSalesPersons();
        //   });
        //}
        //$scope.getItemFilters();
        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
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
                        // item:( $scope.selectedData == null || $scope.selectedData =='')? '' : $scope.selectedData.Key
                        item: $scope.arrayobj
                        //  item: ($scope.selectedItems == null || $scope.selectedItems == '') ? [] :getValueListFromKeyValueList( parsJasonFromArray($scope.selectedItems))

                    };

                    (httpRequest = dataService.getItemReportData(param))
                        .then(function (response) {
                            if (response) {
                                $scope.iscanceldisabled = true;
                                $scope.itemReport = response.data.ReportData;
                                $scope.availableItems = response.data.ListItem;
                                $scope.itemReportMaster = response.data.ReportData;

                                $scope.TotalCost = 0;
                                $scope.TotalPrice = 0;

                                var length = $scope.itemReport.length;

                                for (var i = 0; i < length; i++) {

                                    $scope.TotalCost = $scope.TotalCost + $scope.itemReport[i].Cost;
                                    $scope.TotalPrice = $scope.TotalPrice + $scope.itemReport[i].Price;
                                    var diff = (($scope.itemReport[i].Cost - $scope.itemReport[i].Price) / $scope.itemReport[i].Cost) * 100;
                                    $scope.itemReport[i].Difference = diff >= 50 ? "color:red" : false;

                                }

                                $scope.TotalCost = $scope.TotalCost / length;
                                $scope.TotalPrice = $scope.TotalPrice / length;
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
        //$scope.$watch('selectedItems', function (newVal, oldVal) {

        //    if (newVal != oldVal) {
        //      //  $scope.search();
        //    }
        //});

        function parsJasonFromArray(array) {
            var result = [];
            if (array && array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    result.push(JSON.parse(array[i]));
                }
            }
            return result;
        }
        $scope.resetSearch = function () {

            $scope.MinSalesAmt = 100;
            // $scope.beginDate = null;
            // $scope.endDate = null;
            $scope.itemReport = [];
            $scope.itemReportMaster = [];
            $scope.CurrentFilter = "";
            $scope.selectedItems = [];
            $scope.clearInput();
            // generateDefaultReport();
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

            if ($scope.isDrillDown) {
                return ["Invoice #", "Invoice Date", "Customer Number", "Customer Name", "Quantity Shipped", "Bin Number", "Unit Of Measure", "Cost", "Price"];
            }
            else {
                return ["Item#", "Item Name", "Week", "Month", "Year", "Avg Cost", "Price"];
            }
        }

        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            //var Cost = 0;
            //var Price = 0;
            predefinedHeader = ["Item", "ItemDesc", "Week", "Month", "Year", "Cost", "Price"];
            if ($scope.isDrillDown)
                predefinedHeader = ["InvoiceNumber", "InvoiceDate", "CustNumber", "CustName", "QtyShipped", "BinNo", "UnitOfMeasure", "Cost", "Price"];
            angular.forEach($scope.isDrillDown ? $scope.salesDetailedReportMaster : $scope.itemReportMaster, function (value, key) {

                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        if (key == "Cost") {
                            header[index] = "$" + value;
                        }
                        else if (key == "Price") {
                            header[index] = "$" + value;
                        }
                        else if(key=="Month")
                        {
                            header[index]=formatMonth(value);
                        }
                        else if (key == "InvoiceDate") {
                            header[index]= $filter('date')(value, 'MM/dd/yyyy');
                        }
                        else {
                            header[index] = value;
                        }

                    }

                    //if (key == "Cost") {
                    //    Cost = Cost + value;
                    //}
                    //if (key == "Price") {
                    //    Price = Price + value;
                    //}

                });
                if (header != undefined)
                    array.push(header);
            });
          
            var total = {};
            total[0] = "Average";
            total[1] = "";
            total[2] = "";
            total[3] = "";
            total[4] = "";
            total[5] = "$" + $scope.TotalCost;
            total[6] = "$" + $scope.TotalPrice;

            if ($scope.isDrillDown == false)
                array.push(total);
            else {
                var total = {};
                total[0] = "Average";
                total[1] = "";
                total[2] = "";
                total[3] = "";
                total[4] = "";
                total[5] = "";
                total[6] = "";
                total[7] = "$" + $scope.avgCost;
                total[8] = "$" + $scope.avgPrize;
                array.push(total);
            }

            return array;
        };
        function formatMonth(num)
        {
            var monthNames = [ 'January', 'February', 'March', 'April', 'May', 'June',
            'July', 'August', 'September', 'October', 'November', 'December'];
            return num + ' - '+monthNames[num - 1];
        }
        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');

            // $scope.search();

        };
        generateDefaultReport();

        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        };



    }]);