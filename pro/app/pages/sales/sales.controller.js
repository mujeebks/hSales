
'use strict';
MetronicApp.controller('SalesController', ['$timeout', 'storageService', '$scope', 'dataService', '$filter', 'NotificationService', '$rootScope', 'HelperService', '$controller', '$interval', '$state', function ($timeout, storageService, $scope, dataService, $filter, NotificationService, $rootScope, HelperService, $controller, $interval, $state) {
   

    var state = $state.current.name;

    document.addEventListener('fullscreenchange', exitHandler);
    document.addEventListener('webkitfullscreenchange', exitHandler);
    document.addEventListener('mozfullscreenchange', exitHandler);
    document.addEventListener('MSFullscreenChange', exitHandler);

    function exitHandler() {

        if (!document.fullscreenElement && !document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {

            $scope.$apply(function () {
             
                $scope.isfullscreen = (document.fullscreenElement) ? document.fullscreenElement :
                                   (document.webkitIsFullScreen) ? document.webkitIsFullScreen :
                                   (document.mozFullScreen) ? document.mozFullScreen :
                                   (document.msFullscreenElement) ? document.msFullscreenElement : false;
            });

        }
    }
    $scope.isfullscreen = false;
   // $scope.btnnamefullscreen = "Full screen";
    $scope.setfullscreen = function () {
        if ($scope.isfullscreen == false) {
            $scope.isfullscreen = true;
        //    $scope.btnnamefullscreen = "Exit Full screen";

            HelperService.GoInFullscreen($("#fullscreendiv").get(0))
        }
        else {
            $scope.isfullscreen = false;
         //   $scope.btnnamefullscreen = "Full screen";
            HelperService.GoOutFullscreen()
        }

    }
    $scope.callbackapifn = function (event) {
        if (event) {
          
            loadCasesSoldRevenueByMonth($rootScope.currentfilter, event);

            
            dataService.GetGlobalFilters().then(function (response) {
                try {

                   
                    $rootScope.defaultfilter = response.data[3];
                    $rootScope.GlobalFilter = response.data;
                    $rootScope.currentfilter = $rootScope.defaultfilter;
                    $scope.currentfilter = $rootScope.currentfilter;

                    if (storageService.getStorage('CurrentFilter') == false) {

                        storageService.setStorage('CurrentFilter', $rootScope.defaultfilter);

                        $rootScope.currentfilter = $rootScope.defaultfilter;

                    }
                    else {

                        $rootScope.currentfilter = storageService.getStorage('CurrentFilter').data;

                    }
                  
                } catch (e) {


                }
                finally {

                }
            })
            }
            
    };
    $scope.callbackapifnguagevalues = function (values) {
        $scope.guagevals = values;
    };
    function populateguagevaluesfordisplay() {

        var first = $scope.chartData.TotalRevenue.All[0].rValue1 + $scope.chartData.TotalRevenue.All[0].rValue2;
        var secondyear = $scope.chartData.TotalRevenue.All[0].rValue3 + $scope.chartData.TotalRevenue.All[0].rValue4;
        var secondmonth = $scope.chartData.TotalRevenue.All[0].rValue5 + $scope.chartData.TotalRevenue.All[0].rValue6;

        $scope.first = first;
        $scope.secondyear = secondyear;
        $scope.secondmonth = secondmonth;

        var secondyearPercentage = first - secondyear;
        if (secondyearPercentage !== 0) {
            secondyearPercentage = secondyearPercentage / secondyear;
            secondyearPercentage = secondyearPercentage * 100;
            secondyearPercentage = (secondyearPercentage == Infinity) ? 0 : secondyearPercentage;
        }
        else {
            secondyearPercentage = 0;
        }
        $scope.secondyearPercentage = Math.round(secondyearPercentage);


        var secondmonthPercentage = first - secondmonth;
        if (secondmonthPercentage !== 0) {
            secondmonthPercentage = secondmonthPercentage / secondmonth;
            secondmonthPercentage = secondmonthPercentage * 100;
            secondmonthPercentage = (secondmonthPercentage == Infinity) ? 0 : secondmonthPercentage;
        }
        else {
            secondmonthPercentage = 0;
        }
        $scope.secondmonthPercentage = Math.round(secondmonthPercentage);


    };
    $scope.stoptimerval = false;
    $scope.stoptimer = function () {
        $scope.stoptimerval = true;
    };





    $scope.TotalMonthlyDifference = $scope.TotalYearlyDifference = "";
    $scope.meterparams = [];
    $scope.PriorRangeText = HelperService.getPriorRangeText($rootScope.currentfilter.Description);

    $scope.saleschartcaption_subject = "Sales";
    $scope.saleschartcaption_subject = "TOTAL CUSTOMER BY SALES PERSON";
    $scope.caption_subjectfn = function (title) { $scope.caption_subject = (title == "initial") ? "" : title; };
    $scope.saleschartcaption_subjectfn = function (title) { $scope.saleschartcaption_subject = (title == "initial") ? "SALES (BY SALES PERSON)" : title; };
    $scope.nonrevenuecaption_subjectfn = function (title) { $scope.nonrevenuecaption_subject = (title == "initial") ? "SALES (GROWTH BY SALES PERSON)" : title; };
    $scope.salestocustomer_subjectfn = function (title) { $scope.salestocustomer_subject = (title == "initial") ? "SALES (BY CUSTOMER)" : title; };
    $scope.salesgrowthtocustomer_subjectfn = function (title) { $scope.salesgrowthtocustomer_subject = (title == "initial") ? "SALES (GROWTH BY CUSTOMER)" : title; };
    $scope.BlstatiticsData = { "casesSold": { name: "CASES SOLD" }, "revenue": { name: "SALES" } };


    $scope.caption_subjectfn = function (title) {
        if (title == "initial") {
            $scope.caption_subject = "Total sales";
            $scope.backBtnShow = !$scope.totalTabAll;
        }
        else {
            $scope.backBtnShow = false;
            $scope.caption_subject = title;
        }
    };

    $scope.caption_subjectfn('initial');


    $scope.$on('$viewContentLoaded', function () {
        Metronic.initAjax();
    });
    $rootScope.$on('sales', function (event, args) {
        Metronic.blockUI({ boxed: true });
        var filter = args.data;
        loadCasesSoldRevenueByMonth(filter);
        loadSalesAndGrowthBySalesPerson(filter);
        LoadSalesAndGrowthByCustomer(filter)
        $scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);
        getheatmap(filter);
    });

    

    function loadCasesSoldRevenueByMonth(filter, isdisplayloading) {
        if (state == "sales") {
            Metronic.blockUI({ boxed: true });
            dataService.PendingRequest();
            $scope.currentfilter = filter;
        }
        else {
            $scope.currentfilter = filter;
            Metronic.blockUI({ boxed: true, message: 'LOADING...' });
        }// Metronic.unblockUI(); }
        dataService.GetCasesSoldAndSalesDasboardData(filter).then(function (response) {
            try {
                $scope.caption_subject = "SALES (BY CATEGORY)";
                $scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);

                $timeout(function () {
                    //Populate the Total cases sold chart
                    $scope.chartData = response.data;
                    dataService.PendingRequest();
                   // demomode();
                    populateguagevaluesfordisplay()
                    //Populate the Pie Chart
                    $scope.PiechartSales = $scope.chartData.TotalRevenue.All["0"].SubData;

                    //Populate the Caeses Sold Top Boxes
                    populateTopBox($scope.chartData, filter);
                    debugger
                    //Populate the Guage Values
                    populateMeterGauge($scope.chartData);

                 
                        //Added the Entire Values into the Local Storage
                        var storageid = 'CasesSoldRevenueByMonth';
                        response.data["filterid"] = filter.Id;
                        storageService.setStorage(storageid, response.data);
                        storageService.setStorage('CurrentFilter', filter)
                   
                   
                }, 200);

            } catch (e) { NotificationService.Error(); }
            finally {

                if (state == "sales") {
                   
                    dataService.PendingRequest();
                }
                else{
                    Metronic.unblockUI();
}

            }

        }, function onError() {
            dataService.PendingRequest();
            NotificationService.Error();

        });
    };

   
    function populateMeterGauge(chartData) {
     debugger
        $scope.meterparams = $scope.chartData.TotalRevenue.All[0];
        $scope.firstrange = $scope.meterparams.rValue1 + $scope.meterparams.rValue2;
       
        $scope.secondrangeYear = $scope.meterparams.rValue3 + $scope.meterparams.rValue4;
        $scope.secondrangeMonth = $scope.meterparams.rValue5 + $scope.meterparams.rValue6;
        //$scope.GroceryMonthlyDifference = $scope.GroceryMonthlyDifference;
        //$scope.GroceryYearlyDifference = $scope.GroceryYearlyDifference;
        //$scope.ProduceMonthlyDifference = $scope.ProduceMonthlyDifference;
        //$scope.ProduceYearlyDifference = $scope.ProduceYearlyDifference;
        $scope.TotalMonthlyDifference = $scope.chartData.revenueTotalMonthlyDifference;
        $scope.TotalYearlyDifference = $scope.chartData.revenueTotalYearlyDifference;





    };
    function populateTopBox(chartData, filter) {
        var Current = chartData.TotalRevenue.All[0].rValue1 + chartData.TotalRevenue.All[0].rValue2;
      
        var Prior = 0;
        Prior = chartData.TotalRevenue.All[0].rValue3 + chartData.TotalRevenue.All[0].rValue4;
        $scope.BlstatiticsData.revenue.CurrentYear = Current;
        $scope.BlstatiticsData.revenue.PreviousMonth = Prior;
        $scope.BlstatiticsData.revenue.Change = Math.round(HelperService.calcPercentage($scope.BlstatiticsData.revenue.CurrentYear, $scope.BlstatiticsData.revenue.PreviousMonth));
    };
    function getheatmap(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.GetSalesCasesSoldMap(filter).then(function (response) {
            try {
                $scope.HeatMapData = response.data;
                var storageid = 'HeatMapData';
                response.data["filterid"] = filter.Id;
                storageService.setStorage(storageid, response.data);
                storageService.setStorage('CurrentFilter', filter)
                dataService.PendingRequest();
            } catch (e) {
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
    function LoadSalesAndGrowthByCustomer(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.GetSalesAndGrowthByCustomer(filter).then(function (response) {
            try {
                $scope.customerGraph = response.data;
            } catch (e) {
                NotificationService.Error();
            }
            finally {
                dataService.PendingRequest();
            }
        });
    };
    function loadSalesAndGrowthBySalesPerson(filter) {
        Metronic.blockUI({ boxed: true });
        dataService.GetSalesAndGrowthBySalesPerson(filter).then(function (response) {
            try {
                $scope.TotalSalesAndGrowth = response.data;
                dataService.PendingRequest();


            } catch (e) {
                dataService.PendingRequest();
                NotificationService.Error();
            }
            finally {
                dataService.PendingRequest();
            }
        });

    };
    function getData(filter) {
debugger
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
                        populateTopBox($scope.chartData, filter);
                        $scope.PiechartSales = $scope.chartData.TotalRevenue.All["0"].SubData;
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



    if (state == "sales") {

        loadSalesAndGrowthBySalesPerson($rootScope.currentfilter);
        LoadSalesAndGrowthByCustomer($rootScope.currentfilter);
        getData($rootScope.currentfilter);
    }
    else {
        loadCasesSoldRevenueByMonth($rootScope.currentfilter);
    }




    //$scope.getTopBoxEmptyStylePositive = function (statiticsData) { return HelperService.getTopBoxEmptyStylePositive(statiticsData); }
    //$scope.getTopBoxEmptyStyleNegative = function (statiticsData) { return HelperService.getTopBoxEmptyStyleNegative(statiticsData); }
    //$scope.getTopBoxSuccessStyle = function (statiticsData) { return HelperService.getTopBoxSuccessStyle(statiticsData); }
    //$scope.getTopBoxFailStyle = function (statiticsData) { return HelperService.getTopBoxFailStyle(statiticsData); }

    //$scope.PercentageCallbackfn = function (PriorYear, PriorMonth, meterparams) {
    //    //no need to reflect the percentage change on clicking the total chart
    //    $scope.TotalMonthlyDifference = PriorMonth;
    //    $scope.TotalYearlyDifference = PriorYear;
    //    $scope.meterparams = meterparams;
    //};

    $scope.refreshCharts = function () {
        Metronic.blockUI({ boxed: true });
        loadCasesSoldRevenueByMonth($rootScope.currentfilter);
        loadSalesAndGrowthBySalesPerson($rootScope.currentfilter);
        LoadSalesAndGrowthByCustomer($rootScope.currentfilter)
        $scope.PriorRangeText = HelperService.getPriorRangeText($rootScope.currentfilter.Description);
        getheatmap($rootScope.currentfilter);
    };
}]);


