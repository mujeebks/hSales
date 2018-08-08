'use strict';
MetronicApp.controller('DashboardController', ['$scope', 'dataService', 'NotificationService', 'storageService', '$rootScope', 'HelperService',
    function ($scope, dataService, NotificationService, storageService, $rootScope, HelperService) {
        Metronic.blockUI({ boxed: true });
        $scope.$on('$viewContentLoaded', function () { Metronic.initAjax(); });
        $rootScope.$on('dashboard', function (event, args) {
            Metronic.blockUI({ boxed: true });
            var filter = args.data;
            loadStatiticsData(filter);
            $scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);
        });
        $scope.BlstatiticsData = {"casesSold": {name: "CASES SOLD",},"revenue": {name: "SALES"}};
        $scope.PriorRangeText = HelperService.getPriorRangeText($rootScope.currentfilter.Description);

        var GaugeValues = {
            casesSold: {},
            revenue: {}
        };
        function loadStatiticsData(filter) {
            
            dataService.GetOpexTopBoxValues(filter).then(function (response) {
                try {
                    
                    $scope.OpexTopBox = response.data.Result;
                    storageService.setStorage('CurrentFilter', filter);
                   
                    GaugeValues["filterid"] = filter.Id;
                    checkAccess();
                    if ($scope.ShowExpenseCanvas) {
                        GaugeValues["OpexTopBox"] = $scope.OpexTopBox;
                        storageService.setStorage('DashboardGaugeValues', GaugeValues);
                    }
                  
                 
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            });

            dataService.GetProfitTopBoxValues(filter).then(function (response) {
                try {
                    
                    $scope.GrossProfitTopBox = response.data.Result;
                  
                    checkAccess();
                    if ($scope.ShowProfitCanvas) {
                        GaugeValues["GrossProfitTopBox"] = $scope.GrossProfitTopBox;
                        storageService.setStorage('DashboardGaugeValues', GaugeValues)
                    }
                    
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            });

            dataService.GetNonCommoditySalesAndCasesSoldTopBoxValues(filter).then(function (response) {
                try {
                    $scope.NonCommoditySalesTopBox = response.data.Result[0];
                    $scope.NonCommodityCasesSoldTopBox = response.data.Result[1];
                    GaugeValues["NonCommoditySalesTopBox"] = $scope.NonCommoditySalesTopBox;
                    GaugeValues["NonCommodityCasesSoldTopBox"] = $scope.NonCommodityCasesSoldTopBox;
                    checkAccess();
                  
                   storageService.setStorage('DashboardGaugeValues', GaugeValues)
                   
                   
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            });

            //Cases sold and sales Data.
            dataService.GetDashboardStatistics(filter).then(function (response) {
                try {
                    $scope.BlstatiticsData.casesSold.CurrentYear = response.data.CasesSold.CurrentValue;
                    $scope.BlstatiticsData.casesSold.PreviousMonth = response.data.CasesSold.PriorValue;
                    $scope.BlstatiticsData.casesSold.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.casesSold.CurrentYear, $scope.BlstatiticsData.casesSold.PreviousMonth));

                    $scope.BlstatiticsData.revenue.CurrentYear = response.data.Sales.CurrentValue;;
                    $scope.BlstatiticsData.revenue.PreviousMonth = response.data.Sales.PriorValue;;
                    $scope.BlstatiticsData.revenue.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.revenue.CurrentYear, $scope.BlstatiticsData.revenue.PreviousMonth));

                   

                    checkAccess();
                    if ($scope.ShowcasesSoldCanvas) {
                        GaugeValues["casesSold"]["CurrentYear"] = $scope.BlstatiticsData.casesSold.CurrentYear;
                        GaugeValues["casesSold"]["PreviousMonth"] = $scope.BlstatiticsData.casesSold.PreviousMonth;
                        GaugeValues["casesSold"]["Change"] = $scope.BlstatiticsData.casesSold.Change;

                        GaugeValues["revenue"]["CurrentYear"] = $scope.BlstatiticsData.revenue.CurrentYear;
                        GaugeValues["revenue"]["PreviousMonth"] = $scope.BlstatiticsData.revenue.PreviousMonth;
                        GaugeValues["revenue"]["Change"] = $scope.BlstatiticsData.revenue.Change;

                        storageService.setStorage('DashboardGaugeValues', GaugeValues)
                    }
                    

                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }


            }, function onError() {
                Metronic.unblockUI();
                dataService.PendingRequest();
                NotificationService.Error("Error upon the API request");
            });
        };

        function getData(filter) {
            try {

                var storage = storageService.getStorage('DashboardGaugeValues');
                if (storage) {
                    if (storage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {
                        dataService.PendingRequest();
                        loadStatiticsData(filter);
                    }
                    else {
                        $scope.chartData = [];
                        if (storage.data.filterid == $rootScope.currentfilter.Id) {
                            $scope.OpexTopBox = storage.data.OpexTopBox;

                            $scope.BlstatiticsData.casesSold.CurrentYear = storage.data.casesSold.CurrentYear;
                            $scope.BlstatiticsData.casesSold.PreviousMonth = storage.data.casesSold.PreviousMonth;
                            $scope.BlstatiticsData.casesSold.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.casesSold.CurrentYear, $scope.BlstatiticsData.casesSold.PreviousMonth));

                            $scope.BlstatiticsData.revenue.CurrentYear = storage.data.revenue.CurrentYear;;
                            $scope.BlstatiticsData.revenue.PreviousMonth = storage.data.revenue.PreviousMonth;;
                            $scope.BlstatiticsData.revenue.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.revenue.CurrentYear, $scope.BlstatiticsData.revenue.PreviousMonth));

                            $scope.NonCommoditySalesTopBox = storage.data.NonCommoditySalesTopBox;
                            $scope.NonCommodityCasesSoldTopBox = storage.data.NonCommodityCasesSoldTopBox;

                            $scope.GrossProfitTopBox = storage.data.GrossProfitTopBox;
                            

                            checkAccess();
                           
                        }
                        else {
                            loadStatiticsData(filter);
                        }
                        Metronic.unblockUI();
                        //dataService.PendingRequest();
                    }
                }
                else {
                    loadStatiticsData(filter);
                }
            } catch (e) {

            }
        };

        function checkAccess() {

            var ModuleStorage = localStorage.getItem('ls.nogalesAuthAccess');
            ModuleStorage = (ModuleStorage) ? JSON.parse(ModuleStorage) : false;
            
            if (ModuleStorage) {
                ModuleStorage = ModuleStorage.Modules;
                for (var i = 0; i < ModuleStorage.length; i++) {
                    if (ModuleStorage[i].Name == "Cases Sold") {
                        $scope.ShowcasesSoldCanvas = ModuleStorage[i].IsAccess;
                    }
                    if (ModuleStorage[i].Name == "Sales") {
                        $scope.ShowSalesCanvas = ModuleStorage[i].IsAccess;
                    }
                    if (ModuleStorage[i].Name == "Expenses") {
                         $scope.ShowExpenseCanvas = ModuleStorage[i].IsAccess;
                    }
                    if (ModuleStorage[i].Name == "Profitablity") {
                        $scope.ShowProfitCanvas = ModuleStorage[i].IsAccess;
                    }
                }
            }
        };

        getData($rootScope.currentfilter);

        $scope.refreshCharts = function () {
            Metronic.blockUI({ boxed: true });
            loadStatiticsData($rootScope.currentfilter);
        };

    }]);