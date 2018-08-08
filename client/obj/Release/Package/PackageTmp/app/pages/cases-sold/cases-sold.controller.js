
'use strict';
MetronicApp.controller('CaseSoldController', ['$timeout', '$scope', 'dataService', 'NotificationService', '$controller', 'storageService', '$state', '$rootScope', 'HelperService','commonService',
function ($timeout, $scope, dataService, NotificationService, $controller, storageService, $state, $rootScope, HelperService, commonService) {
  
    $controller('BaseController', { $scope: $scope });
    $scope.$on('$viewContentLoaded', function () { Metronic.initAjax();});
    $scope.caption_subject = "Total Cases Sold";
    $scope.chartData = $scope.meterparams=[];
    $scope.page = ($state.current.data == undefined) ? "" : $state.current.data.pageTitle;
    $scope.GroceryMonthlyDifference = $scope.GroceryYearlyDifference=$scope.ProduceMonthlyDifference=$scope.ProduceYearlyDifference=$scope.TotalMonthlyDifference=$scope.TotalYearlyDifference="";
   
    $scope.PriorRangeText = HelperService.getPriorRangeText($rootScope.currentfilter.Description);

    $rootScope.$on('casessold', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        loadCasesSoldRevenueByMonth(filter);
        GetCasesSoldAndGrowthBySalesPerson(filter);
        LoadCasesSoldAndGrowthByCustomer(filter);
        getheatmap(filter);
        $scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);
    });



    $scope.caption_subjectfn = function (title) {
        if (title == "initial") {
            $scope.caption_subject = "Total Cases Sold";
            $scope.backBtnShow = !$scope.totalTabAll;
        }
        else {
            $scope.backBtnShow = false;
            $scope.caption_subject = title;
        }
    };

    $scope.caption_subjectfn('initial');

    $scope.BlstatiticsData = {"casesSold": {name: "CASES SOLD"},"revenue": {name: "SALES"}};

    function loadCasesSoldRevenueByMonth(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.PendingRequest();
        dataService.GetCasesSoldAndSalesDasboardData(filter).then(function (response) {
            try {
            
                $scope.caption_subject = "Total Cases Sold";
                $scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);

                $timeout(function () {
                    //Populate the Total cases sold chart
                    $scope.chartData = response.data;

                    //Populate the Pie Chart
                    $scope.PiechartCasessold = $scope.chartData.TotalCasesSold.All["0"].SubData;

                    //Populate the Caeses Sold Top Boxes
                    populateCasesSoldTopBox($scope.chartData, filter);

                    //Populate the Guage Values
                    populateMeterGauge($scope.chartData);


                    //Added the Entire Values into the Local Storage
                    var storageid = 'CasesSoldRevenueByMonth';
                    response.data["filterid"] = filter.Id;
                    storageService.setStorage(storageid, response.data);
                    storageService.setStorage('CurrentFilter', filter)


                }, 200);
             
            } catch (e) {
                NotificationService.Error();
             
            }
            finally {
                dataService.PendingRequest();
            }

        }, function onError() {  
            dataService.PendingRequest();
            NotificationService.Error();
         
        });
    };
     

    function populateCasesSoldTopBox(chartData, filter) {
        var casesSoldCurrent = chartData.TotalCasesSold.All[0].cValue1 + chartData.TotalCasesSold.All[0].cValue2;
        var casesSoldPrior = 0;
        casesSoldPrior = chartData.TotalCasesSold.All[0].cValue3 + chartData.TotalCasesSold.All[0].cValue4;
        $scope.BlstatiticsData.casesSold.CurrentYear = casesSoldCurrent;
        $scope.BlstatiticsData.casesSold.PreviousMonth = casesSoldPrior;
        $scope.BlstatiticsData.casesSold.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.casesSold.CurrentYear, $scope.BlstatiticsData.casesSold.PreviousMonth));
    };


    function populateMeterGauge(chartData) {

        $scope.meterparams = chartData.TotalCasesSold.All[0];
        $scope.firstrange = $scope.meterparams.cValue1 + $scope.meterparams.cValue2;
        $scope.secondrangeYear = $scope.meterparams.cValue3 + $scope.meterparams.cValue4;
        $scope.secondrangeMonth = $scope.meterparams.cValue5 + $scope.meterparams.cValue6;
        $scope.GroceryMonthlyDifference = chartData.GroceryMonthlyDifference;
        $scope.GroceryYearlyDifference = chartData.GroceryYearlyDifference;
        $scope.ProduceMonthlyDifference = chartData.ProduceMonthlyDifference;
        $scope.ProduceYearlyDifference = chartData.ProduceYearlyDifference;
        $scope.TotalMonthlyDifference = chartData.TotalMonthlyDifference;
        $scope.TotalYearlyDifference = chartData.TotalYearlyDifference;

    };


    function GetCasesSoldAndGrowthBySalesPerson(filter) {
        Metronic.blockUI({ boxed: true });
  
        dataService.GetCasesSoldAndGrowthBySalesPerson(filter).then(function (response) {
            try {
             
                $scope.totlaCasesSold = response.data;
             
            } catch (e) {
            
                NotificationService.Error();
            
            }
            finally {
            
                dataService.PendingRequest();
            }
        });
       
    }

    function LoadCasesSoldAndGrowthByCustomer(filter) {
        dataService.GetCasesSoldAndGrowthByCustomer(filter).then(function (response) {
            try {
                $scope.customerGraph = response.data;
            } catch (e) {
                NotificationService.Error();
            }
            finally {
                dataService.PendingRequest();
            }
        });

    }

    function getData(filter) {
        try {
            var storage = storageService.getStorage('CasesSoldRevenueByMonth');
            if (storage) {
                if (storage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {
                    dataService.PendingRequest();
                    loadCasesSoldRevenueByMonth(filter);
                }
                else {
                    $scope.chartData = [];
                    if (storage.data.filterid == $rootScope.currentfilter.Id) {
                        $scope.chartData = storage.data;
                        populateCasesSoldTopBox($scope.chartData, filter);
                        $scope.PiechartCasessold = $scope.chartData.TotalCasesSold.All["0"].SubData;
                        populateMeterGauge($scope.chartData);
                    }
                    else {
                        loadCasesSoldRevenueByMonth(filter);
                    }
                    dataService.PendingRequest();
                }
            }
            else {
                loadCasesSoldRevenueByMonth(filter);
            }
            var HeatMapDatastorage = storageService.getStorage('HeatMapData');
            if (HeatMapDatastorage) {
                if (HeatMapDatastorage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {
                    dataService.PendingRequest();
                    getheatmap($rootScope.currentfilter);
                }
                else {
                    $scope.HeatMapData = HeatMapDatastorage.data;
                }
            }
            else {
                getheatmap($rootScope.currentfilter);
            }

        } catch (e) {
         
        }
    };



    $scope.saleschartcaption_subject = "Cases Sold (By Sales Person)";
    $scope.saleschartcaption_subjectfn = function (title) {$scope.saleschartcaption_subject =(title == "initial") ? "Cases Sold (By Sales Person)" : title;};

    $scope.casesoldtocustomer_subject = "Cases Sold (By Customer)";
    $scope.casesoldtocustomer_subjectfn = function (title) {$scope.casesoldtocustomer_subject = (title == "initial") ? "Cases Sold (By Customer)": title;};

    $scope.casessoldgrowthbysalesperson_subject = "Cases Sold (Growth By Sales Person)";
    $scope.casessoldgrowthbysalesperson_subjectfn = function (title) {$scope.casessoldgrowthbysalesperson_subject = (title == "initial") ? "Cases Sold (Growth By Sales Person)" :title;};

    $scope.casesoldgrowthtocustomer_subject = "Cases Sold (Growth By Customer)";
    $scope.casesoldgrowthtocustomer_subjectfn = function (title) {$scope.casesoldgrowthtocustomer_subject = (title == "initial") ? "Cases Sold (Growth By Customer)" : title;};
    


 
    function getheatmap(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.GetSalesCasesSoldMap(filter).then(function (response) {
            try {

                $scope.HeatMapData = response.data;
                var storageid = 'HeatMapData';
                response.data["filterid"] = filter.Id;
                storageService.setStorage(storageid, response.data);
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
    };



  //  getheatmap($rootScope.currentfilter);
    GetCasesSoldAndGrowthBySalesPerson($rootScope.currentfilter);
    LoadCasesSoldAndGrowthByCustomer($rootScope.currentfilter);
    getData($rootScope.currentfilter);

   

    $scope.getTopBoxEmptyStylePositive = function (statiticsData) { return HelperService.getTopBoxEmptyStylePositive(statiticsData); }
    $scope.getTopBoxEmptyStyleNegative = function (statiticsData) { return HelperService.getTopBoxEmptyStyleNegative(statiticsData); }
    $scope.getTopBoxSuccessStyle = function (statiticsData) { return HelperService.getTopBoxSuccessStyle(statiticsData); }
    $scope.getTopBoxFailStyle = function (statiticsData) { return HelperService.getTopBoxFailStyle(statiticsData); }

    $scope.refreshCharts = function () {
        Metronic.blockUI({ boxed: true });
        loadCasesSoldRevenueByMonth($rootScope.currentfilter);
        GetCasesSoldAndGrowthBySalesPerson($rootScope.currentfilter);
        LoadCasesSoldAndGrowthByCustomer($rootScope.currentfilter);
        getheatmap($rootScope.currentfilter);
    };

}]);
