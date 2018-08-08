'use strict';
MetronicApp.controller('RevenueController', ['$http','$timeout', '$scope', 'dataService', '$filter', 'NotificationService', '$controller', 'storageService', '$state','$rootScope',
function ($http, $timeout, $scope, dataService, $filter, NotificationService, $controller, storageService, $state, $rootScope) {


    $controller('BaseController', { $scope: $scope });
    Metronic.unblockUI();
    var $this = $("#load");
    $scope.PendingRequest = function () {

        //console.log("Request Remaining " + $http.pendingRequests.length);
        if ($http.pendingRequests.length == 0) {
            Metronic.unblockUI();
        }

    };


    $rootScope.$on('revenue', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        loadCasesSoldRevenueByMonth(filter);
        loadGetSalesManDashBoardReport(filter);

    });


    function initTitles() {
        $scope.buyertitle = "BUYER";
        $scope.foodservicetitle = "FOOD SERVICE";
        $scope.carniceriatitle = "CARNICERIA";
        $scope.retailtitle = "RETAIL";
        $scope.nationaltitle = "National";
        $scope.Willcalltitle = "Will call";
        $scope.Wholesalertitle = "Wholesaler";
        $scope.AllOtherstitle = "All Others";
    };
    initTitles();
    $controller('BaseController', { $scope: $scope });
    $scope.caption_subject = "Total";
    $scope.chartData = [];
    $scope.meterparams = [];
    $scope.page = ($state.current.data == undefined) ? "" : $state.current.data.pageTitle;
    $scope.GroceryMonthlyDifference = "";
    $scope.GroceryYearlyDifference = "";
    $scope.ProduceMonthlyDifference = "";
    $scope.ProduceYearlyDifference = "";
    $scope.TotalMonthlyDifference = "";
    $scope.TotalYearlyDifference = "";
    $scope.caption_subjectfn = function (title) {
        if (title == "initial") {
            $scope.caption_subject = "Total";
            $scope.backBtnShow = !$scope.totalTabAll;
        }
        else {
            $scope.backBtnShow = false;
            $scope.caption_subject = title;
        }
    };
    $scope.$on('$viewContentLoaded', function () {

        Metronic.initAjax();
    });


    $scope.caption_subjectfn('initial');

    function loadCasesSoldRevenueByMonth(filter) {

        dataService.GetCasesSoldRevenueByMonth(filter).then(function (response) {
            try {
                $this.button('reset');
                $scope.caption_subject = "Total";
                //var msg = 'Syncing Data...';
                //Metronic.startPageLoading({ message: msg });

                $timeout(function () {

                    Metronic.stopPageLoading();

                    $scope.chartData = response.data;
                    $scope.meterparams = $scope.chartData.TotalRevenue.All[0];
                    $scope.PiechartRevenue = $scope.chartData.TotalRevenue.All["0"].SubData;

                    var storageid = 'CasesSoldRevenueByMonth';
                    response.data["filterid"] = filter.Id;
                    storageService.setStorage(storageid, response.data);
                    storageService.setStorage('CurrentFilter', filter)



                    //response.data["filterid"] = filter.Id;
                    //storageService.setStorage(storageid, response.data);


                    var OPEXCOGSExpenseJournalChart = storageService.getStorage('OPEXCOGSExpenseJournalChart');

                    var totalSales = storageService.getStorage('totalSales');


                    if (OPEXCOGSExpenseJournalChart) {

                        OPEXCOGSExpenseJournalChart.data["filterid"] = filter.Id;

                        if (totalSales) {
                            totalSales.data["filterid"] = filter.Id;
                        }
                    }


                }, 2000);
                if (response.data != null) {
                    $scope.GroceryMonthlyDifference = response.data.GroceryMonthlyDifference;
                    $scope.GroceryYearlyDifference = response.data.GroceryYearlyDifference;
                    $scope.ProduceMonthlyDifference = response.data.ProduceMonthlyDifference;
                    $scope.ProduceYearlyDifference = response.data.ProduceYearlyDifference;
                    $scope.TotalMonthlyDifference = response.data.TotalMonthlyDifference;
                    $scope.TotalYearlyDifference = response.data.TotalYearlyDifference;
                }
            } catch (e) {
                NotificationService.Error();
                $this.button('reset');
                NotificationService.ConsoleLog('Error while assigning the data from the API to the chart.');
            }
            finally {

                $scope.PendingRequest();
            }

        }, function onError() {
            Metronic.stopPageLoading();
            $scope.PendingRequest();
            NotificationService.Error("Error upon the API request");
            NotificationService.ConsoleLog('Error on the server');
        });
    };
    function loadGetSalesManDashBoardReport(filter) {
        dataService.GetSalesManDashBoardReport(filter).then(function (response) {
            try {
                //$this.button('reset');
                //debugger;
                $scope.totlaCasesSold = response.data;
                var storageid = 'categorystorage';
                response.data["filterid"] = filter.Id;
                storageService.setStorage('CurrentFilter', filter)
                storageService.setStorage(storageid, response.data);


            } catch (e) {
                NotificationService.Error();
                NotificationService.ConsoleLog('Error while assigning the data from the API to the chart.');
            }
            finally {
                Metronic.unblockUI();
                dataService.PendingRequest();
                NotificationService.Error("Error upon the API request");
                NotificationService.ConsoleLog('Error on the server');
            }
        });

        //dataService.loadcategory(filter).then(function (response) {
        //    try {



        //        $scope.Categories = response.data;
        //        var storageid = 'categorystorage';
        //        response.data["filterid"] = filter.Id;
        //        storageService.setStorage(storageid, response.data);
        //        storageService.setStorage('CurrentFilter', filter)


        //    } catch (e) {
        //        NotificationService.Error();
        //        NotificationService.ConsoleLog('Error while assigning the data from the API to the chart.');
        //    }
        //    finally {
        //        $scope.PendingRequest();
        //    }

        //}, function onError() {
        //    Metronic.unblockUI();
        //    $scope.PendingRequest();
        //    NotificationService.Error("Error upon the API request");
        //    NotificationService.ConsoleLog('Error on the server');

        //});
    }
    function getmonthData(filter) {

        try {
            var storage = storageService.getStorage('CasesSoldRevenueByMonth');
            var catetogoresstorage = storageService.getStorage('categorystorage');

            if (storage) {
                if (storage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {
                    loadCasesSoldRevenueByMonth(filter);
                    loadGetSalesManDashBoardReport(filter);
                }
                else {
                    $scope.chartData = [];


                    if (storage.data.filterid == $rootScope.currentfilter.Id) {
                        $scope.chartData = storage.data;

                        $scope.PiechartRevenue = $scope.chartData.TotalRevenue.All["0"].SubData;

                        $scope.GroceryMonthlyDifference = storage.data.GroceryMonthlyDifference;
                        $scope.GroceryYearlyDifference = storage.data.GroceryYearlyDifference;
                        $scope.ProduceMonthlyDifference = storage.data.ProduceMonthlyDifference;
                        $scope.ProduceYearlyDifference = storage.data.ProduceYearlyDifference;
                        $scope.TotalMonthlyDifference = storage.data.TotalMonthlyDifference;
                        $scope.TotalYearlyDifference = storage.data.TotalYearlyDifference;

                        $scope.meterparams = $scope.chartData.TotalRevenue.All[0];
                    }
                    else {
                        loadCasesSoldRevenueByMonth(filter);
                        loadGetSalesManDashBoardReport(filter);
                    }


                    $scope.PendingRequest();
                }
            }
            else {
                loadCasesSoldRevenueByMonth(filter);
            }

            if (catetogoresstorage) {
                if (catetogoresstorage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {

                    loadGetSalesManDashBoardReport(filter);
                }
                else {
                    $scope.totlaCasesSold = [];

                    if (storage.data.filterid == $rootScope.currentfilter.Id) {
                        $scope.totlaCasesSold = catetogoresstorage.data;
                    }
                    else {
                        loadGetSalesManDashBoardReport(filter);
                    }



                }
            }
            else {
                loadGetSalesManDashBoardReport(filter);
            }


        } catch (e) {
            $scope.PendingRequest();

        }

    };


    getmonthData($rootScope.currentfilter);
    $scope.saleschartcaption_subject = "Sales";
    $scope.saleschartcaption_subject = "TOTAL CUSTOMER BY SALES PERSON";
    $scope.saleschartcaption_subjectfn = function (title) {

        if (title == "initial") {
            $scope.saleschartcaption_subject = "Revenue by Salesperson";
        }
        else {
            $scope.saleschartcaption_subject = title;
        }

    };
    $scope.categories_subjectfn = function (title, charttype) {
        //

        function setCategoriesTitle(type) {
            switch (type) {
                case "BUYER":
                    $scope.buyertitle = (title == "initial") ? "BUYER" : title;
                    break;
                case "FOOD SERVICE":
                    $scope.foodservicetitle = (title == "initial") ? "FOOD SERVICE" : title;
                    break;
                case "CARNICERIA":
                    $scope.carniceriatitle = (title == "initial") ? "CARNICERIA" : title;
                    break;
                case "RETAIL":
                    $scope.retailtitle = (title == "initial") ? "RETAIL" : title;
                    break;
                case "National":
                    $scope.nationaltitle = (title == "initial") ? "National" : title;
                    break;
                case "Will Call":
                    $scope.Willcalltitle = (title == "initial") ? "Will call" : title;
                    break;
                case "Wholesaler":
                    $scope.Wholesalertitle = (title == "initial") ? "Wholesaler" : title;
                    break;
                case "All Others":
                    $scope.AllOtherstitle = (title == "initial") ? "All Others" : title;
                    break;
            }
        };
        setCategoriesTitle(charttype);

    };
    $scope.PercentageCallbackfn = function (PriorYear, PriorMonth, meterparams) {

        $scope.TotalMonthlyDifference = PriorMonth;
        $scope.TotalYearlyDifference = PriorYear;
        $scope.meterparams = meterparams;

    };

    function getheatmap(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.GetRevenueMap(filter).then(function (response) {
            try {

                $scope.HeatMapData = response.data;

                dataService.PendingRequest();

            } catch (e) {
                //debugger
                NotificationService.Error();

                dataService.PendingRequest();
            }
            finally {
                dataService.PendingRequest();
            }

        }, function onError() {
            dataService.PendingRequest();
            NotificationService.Error("Error upon the API request");
        });
    };

    getheatmap($rootScope.currentfilter);

}]);
