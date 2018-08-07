
'use strict';
MetronicApp.controller('CategoriesController',
      ['$http', '$timeout', '$scope', 'dataService', '$filter', 'NotificationService', '$controller', 'storageService', '$state', '$rootScope', 'HelperService',
function ($http, $timeout, $scope, dataService, $filter, NotificationService, $controller, storageService, $state, $rootScope, HelperService) {
  
    var $this = $("#load");
 
    $scope.IsFullScreen = "";

    $scope.$on('$viewContentLoaded', function () {

        Metronic.initAjax();
    });
 
    var state = $state.current.name;

    $rootScope.$on('cases-sold-categories', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        loadCasesSoldRevenueByMonth(filter);
    });

    $rootScope.$on('revenue-categories', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        loadCasesSoldRevenueByMonth(filter);
    });


    $scope.categories_subjectfn = function (title, charttype, chart) {
        debugger
       
        //if (chart.GroupName == "CUSTOMER SERVICE") {
           
        //    for (var i = 0; i < $scope.chartData.length; i++) {
        //        if ($scope.chartData[i].GroupName == "FOOD SERVICE") {
        //            $scope.chartData[i]["Title"] = title;
        //            break;
        //        }
        //    }
        //}
        //else {
            chart["Title"] = title;
        //}
 
      
       // chart.GroupName = title;
    };

    function loadCasesSoldRevenueByMonth(filter) {
        Metronic.blockUI({ boxed: true });
     
        dataService.GetCasesSoldAndSalesDasboardData(filter).then(function (response) {
            try {

                $timeout(function () {

                    //Added the Entire Values into the Local Storage
                 
                    response.data["filterid"] = filter.Id;
                    storageService.setStorage('CasesSoldRevenueByMonth', response.data);
                    storageService.setStorage('CurrentFilter', filter)




                    var Object = (state == "cases-sold-categories") ? "TotalCasesSold" : "TotalRevenue";
                 
                    var data = response.data[Object].All[0].SubData;
                    

                    data.sort(function (a, b) { return a.GroupName > b.GroupName ? 1 : 0; })
                        .forEach(function (item, index) { data[index]["xtitle"] = item.GroupName; data[index]["Title"] = item.GroupName; });

                    $scope.chartData = data;


                }, 2000);

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

    function FindindexofCategories(ChartData, Category) {
        function checkcategory(cat) { return cat.GroupName == Category;};
        return ChartData.findIndex(checkcategory);
    };

    //function fillchartdata(chartData) {
     
    //    var Object = (state == "cases-sold-categories") ? "TotalCasesSold" : "TotalRevenue";

    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "BUYER"); $scope.Categories["Buyer"]["All"] = [];
    //    if (index != -1) { $scope.Categories["Buyer"]["All"].push(chartData[Object].All[0].SubData[index]); }

    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "FOOD SERVICE"); $scope.Categories["FoodService"]["All"] = [];
    //    if (index != -1) { $scope.Categories["FoodService"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "CARNICERIA"); $scope.Categories["Carniceria"]["All"] = [];
    //    if (index != -1) { $scope.Categories["Carniceria"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "RETAIL"); $scope.Categories["Retail"]["All"] = [];
    //    if (index != -1) { $scope.Categories["Retail"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "NATIONAL"); $scope.Categories["National"]["All"] = [];
    //    if (index != -1) { $scope.Categories["National"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "OOT"); $scope.Categories["OOT"]["All"] = [];
    //    if (index != -1) { $scope.Categories["OOT"]["All"].push(chartData[Object].All[0].SubData[index]); }

    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "OS"); $scope.Categories["OSS"]["All"] = [];
    //    if (index != -1) { $scope.Categories["OSS"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "SALES"); $scope.Categories["SALES"]["All"] = [];
    //    if (index != -1) { $scope.Categories["SALES"]["All"].push(chartData[Object].All[0].SubData[index]); }

    //    var index = FindindexofCategories(chartData[Object].All[0].SubData, "WILL CALL"); $scope.Categories["WillCall"]["All"] = [];
    //    if (index != -1) { $scope.Categories["WillCall"]["All"].push(chartData[Object].All[0].SubData[index]); }


    //    //$scope.Categories["WholesalerCasesSold"]["All"] = []; $scope.Categories["WholesalerCasesSold"]["All"].push($scope.chartData.TotalCasesSold.All[0].SubData[0]);

    //    //$scope.Categories["AllOthersCasesSold"]["All"] = []; $scope.Categories["AllOthersCasesSold"]["All"].push($scope.chartData.TotalCasesSold.All[0].SubData[0]);
    //}

    function getData(filter) {
        try {
            Metronic.blockUI({ boxed: true });
            var storage = storageService.getStorage('CasesSoldRevenueByMonth');
          
            if (storage) {
                if (storage.date != new Date().toJSON().slice(0, 10).replace(/-/g, '/')) {
           
                    loadCasesSoldRevenueByMonth(filter);
                }
                else {
                    $scope.chartData = [];
                    if (storage.data.filterid == $rootScope.currentfilter.Id) {

                 
                        var Object = (state == "cases-sold-categories") ? "TotalCasesSold" : "TotalRevenue";
                      
                        var data = storage.data[Object].All[0].SubData;


                        data.sort(function (a, b) { return a.GroupName > b.GroupName ? 1 : 0; })
                            .forEach(function (item, index) { data[index]["xtitle"] = item.GroupName; data[index]["Title"] = item.GroupName; });

                            $scope.chartData = data;
                 
                        Metronic.unblockUI();
                    }
                    else {
                        loadCasesSoldRevenueByMonth(filter);
                    }
                   
                }
            }
            else {
                loadCasesSoldRevenueByMonth(filter);
            }
        } catch (e) {

        }
    };

    Metronic.blockUI({ boxed: true });
    getData($rootScope.currentfilter);

    $scope.refreshCharts = function () {
        loadCasesSoldRevenueByMonth($rootScope.currentfilter);
    };
  
    $scope.callbackfullscreenfn = function (category) {
        if (category) $("body").css("overflow", "hidden");
        else $("body").css("overflow", "inherit");
        $scope.IsFullScreen = category;
    };

}]);
