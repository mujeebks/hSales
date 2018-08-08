'use strict';
MetronicApp.controller('SalesAnalysisReportController', ['$scope', 'dataService', '$stateParams', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope, dataService, $stateParams, $filter, NotificationService, ApiUrl, commonService, $controller) {
    
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });

        $controller('BaseController', { $scope: $scope });
        $scope.iscanceldisabled = true;
        $scope.pageSize = 20;

        $scope.ischecked = false;
        $scope.checked = function (ischecked) {
            if (ischecked) {
                $scope.slider = {
                    minValue: 0,
                    maxValue: 100,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 5,

                    }
                };
            } else {
                $scope.slider = {
                    minValue: 30,
                    maxValue: 50,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 5,

                    }
                };
            }
        }

        $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.ischecked = true;
        $scope.slider = {
            minValue: 30,
            maxValue: 50,
            options: {
                floor: 0,
                ceil: 100,
                step: 5
            }
        };

        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';
        $scope.searchClicked = false;
        $scope.selectedSalesPerson = '';

        $scope.loadDatePickers = function () {
            $('#invoiceStartDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
            $('#invoiceEndDate').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                todayHighlight: true,
            });
        }
        $scope.loadDatePickers();

        $scope.selectedReportType = 'EmployeeReport';
        $scope.reportTitle = '';

        //used to hide the report table w.r.t to the selected Report Type.
        $scope.reportBy = '';

        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        }
        $scope.arrayobj = [];
        $scope.selectedcallback = function (item) {
            $scope.selectedItems = item;
            $scope.arrayobj = [];

            for (var i = 0; i < item.length; i++) {
                $scope.arrayobj.push(item[i].Key)
            }

            $scope.search($scope.selectedfilter, $scope.selectedcommodityitem);
        };

        $scope.search = function (filter) {

            $scope.reportBy = '';
            var TotalExtendedPrice = 0;
            var TotalMargin = 0;
            var TotalQtyShipped = 0;
            var TotalInvoicesNumber = 0;
            $scope.CurrentFilter = filter;
            $scope.searchClicked = true;
            $scope.iscanceldisabled = false;
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
                
            else {
                firstDay = $scope.beginDate;
                lastDay = $scope.endDate;
            }
            $scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');

            status = (!$scope.beginDate || !$scope.endDate) ?
                    true :
                    (($scope.beginDate.length + $scope.endDate.length) < 20) ?
                        true :
                        status;

            if (!status &&
                ($scope.isDate($scope.beginDate)
                && $scope.isDate($scope.endDate)
                && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {
                $scope.btnSpinner.start();
                var param = {
                    StartDate: $scope.beginDate,
                    EndDate: $scope.endDate,
                    ReportName: $scope.selectedReportType,
                    LowerMarginLimit:($scope.ischecked)? -100000:$scope.slider.minValue,
                    UpperMarginLimit: ($scope.ischecked) ? 100000 : $scope.slider.maxValue,
                    item: $scope.arrayobj
                };


                (httpRequest = dataService.GetSalesPersonMarginReport(param))
                    .then(function (response) {
                        $scope.reportBy = $scope.selectedReportType;
                        var reportType = $scope.selectedReportType.replace("Report", "").replace("Employee", "Sales Person").replace("Order", " Order")
                        $scope.reportTitle = ' BY ' + reportType;
                        if (response) {

                            $scope.iscanceldisabled = true;

                            $scope.salesReport = response.data;
                            $scope.salesReportMaster = angular.copy(response.data);

                            for (var i = 0; i < $scope.salesReport.length; i++) {
                                TotalExtendedPrice = TotalExtendedPrice + $scope.salesReport[i].ExtendedPrice;
                                TotalMargin = TotalMargin + $scope.salesReport[i].Margin;
                                TotalQtyShipped = TotalQtyShipped + $scope.salesReport[i].QuantityShipped;
                                TotalInvoicesNumber = TotalInvoicesNumber + $scope.salesReport[i].InvoicesNumber;
                            }

                            $scope.TotalExtendedPrice = Math.round(TotalExtendedPrice);
                            $scope.TotalMargin = Math.round(TotalMargin);
                            $scope.TotalQtyShipped = Math.round(TotalQtyShipped);
                            $scope.TotalInvoicesNumber = Math.round(TotalInvoicesNumber);



                        }

                        $scope.btnSpinner.stop();
                    })
                    .catch(function (error) {
                      
                        $scope.btnSpinner.stop();
                    });

            }


        }

        $scope.resetAutoComplete = false;
        $scope.resetSearch = function () {
            $scope.resetAutoComplete = (!$scope.resetAutoComplete);

            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';
            $scope.CurrentFilter = "";

            //clearing main report
            $scope.salesReport = [];
            $scope.salesReportMaster = [];

            //clearing detailed report
            $scope.salesDetailedReport = [];
            $scope.salesDetailedReportMaster = [];

            //clearing dates
            generateDefaultReport();
            $scope.selectedReportType = 'EmployeeReport';

            //resetting slider
            $scope.slider = {
                minValue: 30,
                maxValue: 50,
                options: {

                    floor: 0,
                    ceil: 100,
                    step: 5
                
                }
            };

            if ($scope.reportBy = "SalesDetailedReport")
            {
                $scope.backToReport();
            }
        };
        $scope.abortExecutingApi = function () {
            return (httpRequest && httpRequest.abortCall());
        };

        // Return the array of objects from array of stringified objects
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

            //debugger;
            $scope.csvfilename = "SALES ANALYSIS REPORT" + $scope.reportTitle + ".csv";
            $scope.csvfilename = $scope.csvfilename.toUpperCase();


            switch ($scope.reportBy) {
                case 'EmployeeReport':
                    return ["Sales Person", "No of Invoices", "Quantity Shipped", "Extended Price", "Margin", "Margin Percentage"];
                case 'CustomerReport':
                    return ["Customer Code", "Customer Name", "Sales Person", "Quantity Shipped", "Extended Price", "Margin", "Margin Percentage"];
                case 'SalesOrderReport':
                    return ["SO #", "Sales Person", "Customer", "Quantity Shipped", "Extended Price", "Margin", "Margin Percentage"];
                case 'PurchaseOrderReport':
                    return ["PO #", "Buyer", "Item Number", "Description", "Sales Person", "Customer", "Quantity Shipped", "Extended Price", "Margin", "Margin Percentage"]
                case 'SalesDetailedReport':
                    return ["Invoice #", "Invoice Date", "Item", "Description", "Sales Person", "Customer", "Quantity Shipped", "Margin", "Margin Percentage"]
            }




        }
        $scope.getCsvData = function () {

            var array = [];
            var predefinedHeader = [];
            var total = {};

            switch ($scope.reportBy) {
                case 'EmployeeReport':
                    predefinedHeader = ["Employee", "InvoicesNumber", "QuantityShipped", "ExtendedPrice", "Margin", "MarginPercentage"];
                    total[0] = total[1] = $filter('numberWithCommas')($scope.TotalInvoicesNumber); total[2] = $filter('numberWithCommas')($scope.TotalQtyShipped); total[3] = "$" + $filter('number')($scope.TotalExtendedPrice, "2"); total[4] = "$" + $filter('number')($scope.TotalMargin, "2");
                    break;
                case 'CustomerReport':
                    predefinedHeader = ["Customer", "Description", "Employee", "QuantityShipped", "ExtendedPrice", "Margin", "MarginPercentage"];
                    total[0] = total[1] = total[2] = ""; total[3] = $filter('numberWithCommas')($scope.TotalQtyShipped); total[4] = "$" + $filter('number')($scope.TotalExtendedPrice, "2"); total[5] = "$" + $filter('number')($scope.TotalMargin, "2");
                    break;
                case 'SalesOrderReport':
                    predefinedHeader = ["SalesOrder", "Employee", "Customer", "QuantityShipped", "ExtendedPrice", "Margin", "MarginPercentage"];
                    total[0] = total[1] = total[2] = ""; total[3] = $filter('numberWithCommas')($scope.TotalQtyShipped); total[4] = "$" + $filter('number')($scope.TotalExtendedPrice, "2"); total[5] = "$" + $filter('number')($scope.TotalMargin, "2");
                    break;
                case 'PurchaseOrderReport':
                    predefinedHeader = ["PurchaseOrder", "Buyer", "Item", "Description", "Employee", "Customer", "QuantityShipped", "ExtendedPrice", "Margin", "MarginPercentage"]
                    total[0] = total[1] = total[2] = total[3] = total[4] = total[5] = ""; total[6] = $filter('numberWithCommas')($scope.TotalQtyShipped); total[7] = "$" + $filter('number')($scope.TotalExtendedPrice, "2"); total[8] = "$" + $filter('number')($scope.TotalMargin, "2");
                    break;
                case 'SalesDetailedReport':
                    predefinedHeader = ["InvoiceNumber", "InvoiceDate", "Item", "Description", "Employee", "Customer",  "QuantityShipped", "Margin", "MarginPercentage"]
                    total[0] = total[1] = total[2] = total[3] = total[4] = total[5] = ""; total[6] = $filter('numberWithCommas')($scope.TotalDetailedQtyShipped); total[7] = "$" + $filter('number')($scope.TotalDetailedExtendedPrice, "2"); total[8] = "$" + $filter('number')($scope.TotalDetailedMargin, "2");
                    break;
            }

            angular.forEach((($scope.reportBy == "SalesDetailedReport") ? $scope.salesDetailedReportMaster : $scope.salesReportMaster)
                , function (value, key) {
                    var header = {};
                    angular.forEach(value, function (value, key) {
                        var index = predefinedHeader.indexOf(key);
                        if (index > -1) {
                            header[index] = (key == "ExtendedPrice" || key == "Margin") ? "$" + $filter('number')(value, "2")
                                            : (key == "InvoiceDate") ? $filter('date')(value, 'MM/dd/yyyy')
                                            : (key == "MarginPercentage") ? $filter('number')(value, "2") + "%"
                                            : (key == "QuantityShipped") ? $filter('numberWithCommas')(value)
                                            : (key == "InvoicesNumber") ? $filter('numberWithCommas')(value)
                                            : value;
                        }
                    });
                    if (header != undefined)
                        array.push(header);
                });

            array.push(total);

            return array;
        };

        function generateDefaultReport() {
            var date = new Date();
            $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
            $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
            $scope.CurrentFilter = "ThisMonth";
            $scope.search();
        }
        generateDefaultReport();

        $scope.TotalDetailedExtendedPrice = 0;
        $scope.TotalDetailedMargin = 0;
        $scope.TotalDetailedQtyShipped = 0;

        $scope.selectedFiltervalue = "";

        $scope.getDetailedReport = function (filterItem) {
            $scope.selectedFiltervalue = filterItem;
            $scope.reportTitle = $scope.reportTitle + " (" + filterItem + ")";
            $scope.reportBy = "SalesDetailedReport";

            $scope.salesDetailedReport = [];
            $scope.salesDetailedReportMaster = [];

            var totalDetailedExtendedPrice,
                totalDetailedMargin,
                totalDetailedQtyShipped = 0;

            //Search Parameters
            var searchFilter = {
                StartDate: $scope.beginDate,
                EndDate: $scope.endDate,
                ReportName: $scope.selectedReportType,
                LowerMarginLimit: ($scope.ischecked)? -100000:$scope.slider.minValue,
                UpperMarginLimit: ($scope.ischecked) ? 100000 : $scope.slider.maxValue,
                FilterValue: filterItem,
                item: $scope.arrayobj
            };

            Metronic.blockUI({ boxed: true });
            (httpRequest = dataService.GetSalesMarginDetailedReport(searchFilter))
                    .then(function (response) {
                        if (response) {

                            $scope.salesDetailedReport = response.data;
                            $scope.salesDetailedReportMaster = angular.copy(response.data);

                            for (var i = 0; i < $scope.salesDetailedReport.length; i++) {
                                totalDetailedExtendedPrice = totalDetailedExtendedPrice + $scope.salesDetailedReport[i].ExtendedPrice;
                                totalDetailedMargin = totalDetailedMargin + $scope.salesDetailedReport[i].Margin;
                                totalDetailedQtyShipped = totalDetailedQtyShipped + $scope.salesDetailedReport[i].QuantityShipped;
                            }

                            $scope.TotalDetailedExtendedPrice = totalDetailedExtendedPrice;
                            $scope.TotalDetailedMargin = totalDetailedMargin;
                            $scope.TotalDetailedQtyShipped = totalDetailedQtyShipped;
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

            $scope.reportBy = $scope.selectedReportType;
            var reportTitle = $scope.reportTitle.replace(" (" + $scope.selectedFiltervalue + ")", "");
            $scope.reportTitle = reportTitle;


        };

        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        }
        function toJSONLocal(date) {
            var local = new Date(date);
            local.setMinutes(date.getMinutes() - date.getTimezoneOffset());
            return local.toJSON().slice(0, 10);
        }
    }]);

