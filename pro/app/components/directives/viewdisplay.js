

MetronicApp.directive('viewdisplay', ['$rootScope', '$filter', 'dataService', 'NotificationService', 'HelperService',
    function ($rootScope, $filter, dataService, NotificationService, HelperService) {
        this.count = 0;
        return {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: { nlDashboard: '=', filterType: '=', callbackFn: '&' },
            templateUrl: 'app/components/directives/viewdisplay.html',

            link: function (scope, element, attrs, controller) {

                scope.TotalMonthlyDifference = 10;
                scope.firstrange = 45;
                scope.secondrangeMonth = 54;
                scope.PriorRangeText = "wefewf";
                scope.showviewdisplay = false;
                scope.BlstatiticsData = { "casesSold": { name: "CASES SOLD" }, "revenue": { name: "SALES" } };
                function GoInFullscreen(element) {
                    if (element.requestFullscreen)
                        element.requestFullscreen();
                    else if (element.mozRequestFullScreen)
                        element.mozRequestFullScreen();
                    else if (element.webkitRequestFullscreen)
                        element.webkitRequestFullscreen();
                    else if (element.msRequestFullscreen)
                        element.msRequestFullscreen();
                }
                function GoOutFullscreen() {
                    if (document.exitFullscreen)
                        document.exitFullscreen();
                    else if (document.mozCancelFullScreen)
                        document.mozCancelFullScreen();
                    else if (document.webkitExitFullscreen)
                        document.webkitExitFullscreen();
                    else if (document.msExitFullscreen)
                        document.msExitFullscreen();
                }

                function populateCasesSoldTopBox(chartData, filter) {
                    var casesSoldCurrent = chartData.TotalCasesSold.All[0].cValue1 + chartData.TotalCasesSold.All[0].cValue2;
                    var casesSoldPrior = 0;
                    casesSoldPrior = chartData.TotalCasesSold.All[0].cValue3 + chartData.TotalCasesSold.All[0].cValue4;
                    scope.BlstatiticsData.casesSold.CurrentYear = casesSoldCurrent;
                    scope.BlstatiticsData.casesSold.PreviousMonth = casesSoldPrior;
                    scope.BlstatiticsData.casesSold.Change = Math.round(HelperService.calcPercentage(scope.BlstatiticsData.casesSold.CurrentYear, scope.BlstatiticsData.casesSold.PreviousMonth));
                };


                function populateMeterGauge(chartData) {

                    scope.meterparams = chartData.TotalCasesSold.All[0];
                    scope.firstrange = scope.meterparams.cValue1 + scope.meterparams.cValue2;
                    scope.secondrangeYear = scope.meterparams.cValue3 + scope.meterparams.cValue4;
                    scope.secondrangeMonth = scope.meterparams.cValue5 + scope.meterparams.cValue6;
                    scope.GroceryMonthlyDifference = chartData.GroceryMonthlyDifference;
                    scope.GroceryYearlyDifference = chartData.GroceryYearlyDifference;
                    scope.ProduceMonthlyDifference = chartData.ProduceMonthlyDifference;
                    scope.ProduceYearlyDifference = chartData.ProduceYearlyDifference;
                    scope.TotalMonthlyDifference = chartData.TotalMonthlyDifference;
                    scope.TotalYearlyDifference = chartData.TotalYearlyDifference;

                };
                function loadCasesSoldRevenueByMonth(filter) {
                    Metronic.blockUI({ boxed: true });
                 
                    dataService.GetCasesSoldAndSalesDasboardData(filter).then(function (response) {
                        try {
                            debugger
                           // scope.caption_subject = "Total Cases Sold";
                           // scope.PriorRangeText = HelperService.getPriorRangeText(filter.Description);

                       
                                //Populate the Total cases sold chart
                                scope.chartData = response.data;

                                //Populate the Pie Chart
                                scope.PiechartCasessold = scope.chartData.TotalCasesSold.All["0"].SubData;

                                //Populate the Caeses Sold Top Boxes
                                populateCasesSoldTopBox(scope.chartData, filter);

                                //Populate the Guage Values
                                populateMeterGauge(scope.chartData);


                                //Added the Entire Values into the Local Storage
                                //var storageid = 'CasesSoldRevenueByMonth';
                                //response.data["filterid"] = filter.Id;
                                //storageService.setStorage(storageid, response.data);
                                //storageService.setStorage('CurrentFilter', filter)



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
              

                scope.$watch(function () { return controller.$viewValue }, function (newVal,oldval) {
                
                   // scope.currentfilter = $rootScope.currentfilter
                    if (scope.currentfilter) {
                        loadCasesSoldRevenueByMonth(scope.currentfilter)
                    }
                    
                    scope.showviewdisplay = newVal;
                    debugger
                    var a = $("#fullscreendiv").get(0)
                    if (newVal) {
                        GoInFullscreen(a)
                    }
                    else {
                        GoOutFullscreen()
                    }
                });
                scope.exit = function () {
              
                    scope.callbackFn({
                        title: false
                    });
                }

              
            }
        };
    }]);

