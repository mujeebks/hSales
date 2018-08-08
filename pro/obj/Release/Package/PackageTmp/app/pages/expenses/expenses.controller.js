'use strict';
MetronicApp.controller('ExpenseController', ['$scope', 'dataService', 'NotificationService', '$controller', 'storageService', '$rootScope', 'HelperService',
    function ($scope, dataService, NotificationService, $controller, storageService, $rootScope, HelperService) {

        $controller('BaseController', { $scope: $scope });
        $scope.caption_subjectOPEXCOGSExpenseJournalChart = "OPEX/COGS Journal Expenses";
        $scope.caption_subjectPayrollJournalChart = "Wages";
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        $rootScope.$on('expenses', function (event, args) {
            Metronic.blockUI({ boxed: true });
            var filter = args.data;
            LoadExpenseChart(filter);
            FilterOPEXcogsExpensesChart(filter);
            filterMeterGaugeChart(filter);
            filterAdminExpensesChart(filter);
        });

        function LoadExpenseChart(filter) {
            dataService.LoadExpenseChart(filter).then(function (response) {
                try {
                    debugger
                    $scope.totalChartData = response.data;
                    storageService.setStorage('CurrentFilter', filter)
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
                NotificationService.Error("Error upon the API request");
            });
        }
        function FilterOPEXcogsExpensesChart(filter) {

            dataService.FilterOPEXcogsExpensesChart(filter).then(function (response) {
                try {
                    var storageid = 'OPEXCOGSExpenseJournalChart';
                    storageService.setStorage(storageid, response.data.OPEXCOGSExpenseJournalChart);
                    $scope.expensesData = response.data;
                    storageService.setStorage('CurrentFilter', filter)
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
                dataService.PendingRequest();
                NotificationService.Error("Error upon the API request");
            });
        }
        function filterMeterGaugeChart(filter) {
            dataService.GetExpensesStatistics(filter).then(function (response) {
                if (response && response.data) {
                    $scope.Data = response.data;
                    Metronic.unblockUI();
                    $scope.metergauge = {
                        wages: $scope.Data.Wages,
                        otWages: $scope.Data.OtWages,
                        adminExp: $scope.Data.AdminExp,
                        warehouseExp: $scope.Data.WarehouseExp,
                        transExp: $scope.Data.TransExp
                    };
                    dataService.PendingRequest();
                }
            }, function onError() {
                Metronic.unblockUI();
                NotificationService.Error("Error upon the API request");
            });

        };

        filterMeterGaugeChart($rootScope.currentfilter);
        FilterOPEXcogsExpensesChart($rootScope.currentfilter);
        LoadExpenseChart($rootScope.currentfilter);

        function filterAdminExpensesChart(filter) {
            dataService.FilterAdminExpensesChart(filter).then(function (response) {
                try {
                    response.data.forEach(function (item, index) {
                        response.data[index]["color"] = HelperService.getRandomColor();
                    });
                    $scope.topTenAdminExpenses = response.data;
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
                NotificationService.Error("Error upon the API request");
            });
        };

        filterAdminExpensesChart($rootScope.currentfilter);

        $scope.caption_subjectfnOPEXCOGSExpenseJournalChart = function (title) {
            $scope.caption_subjectOPEXCOGSExpenseJournalChart = (title == "initial") ? "OPEX/COGS Journal Expenses" : title;
        };
        $scope.caption_subjectfnPayrollJournalChart = function (title) {
            $scope.caption_subjectPayrollJournalChart = (title == "initial") ? "Wages" : title;
        };

        $scope.caption_subjectfn = function (title) {
            
            $scope.commoditycharttitle = (title == "initial") ? "Commodity Expenses" : title;
        };
        $scope.caption_subjectfn("initial");


    }]);
