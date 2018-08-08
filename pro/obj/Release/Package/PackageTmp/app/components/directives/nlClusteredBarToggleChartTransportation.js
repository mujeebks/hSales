
MetronicApp.directive('nlClusteredBarToggleChartTransportation', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {
                valueAxesTitle: "=", topBottomToggleClusterCountLabel: "=", callbackFn: '&'
             },

             template: '<div class="portlet-body" ng-init="showChart=true">' +
                      '<div ng-show="showChart">' +
                      '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                      '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="backChart()" style="cursor:pointer;"></i>' +

                      '</div>' +
                     '<div id="{{::myId}}" class="chart" style="width:96%;min-height:500px;"></div>' +
                     '</br>' +
                      '</br>' +
                       '</br>' +
                        '<div style="text-align:right; position:relative; top:0px;" ng-show="backBtnVisible">' +
                        '<toggle-switch ng-model="topBottomToggle" ng-init="topBottomToggle=true;" on-label="Top 10" off-label="Bottom 10" ng-click="loadDestinationFacilityChart()">' +
                         '<toggle-switch>' +
                         '</div>' +
                     '<div id="{{::legendDiv}}" ng-show="showlegend" class="clusteredBarChartLegenddiv"></div>' +
                     '</div>' +
                     '<div ng-show="showReport">' +
                     '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +

                           '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true;reportback()" style="cursor:pointer;"></i>' +
                      '</div>' +
                          '<div style="margin-top: -46px;">' +
                      '<div ng-include="\'app/pages/transportation/Transport-report-table.html\'"></div>' +
                     '</div>' +
                      '</div>' +
                      '</div>',
             link: function (scope, element, attrs, controller) {

             //    scope.toggleClusterCountLabel = (scope.topBottomToggleClusterCountLabel) ? scope.topBottomToggleClusterCountLabel : 5;
                 var chart;
                 var data;
                 var subData = null;
                 var chartTitle = "";
                 var title;
                 var drillDownCount = 0;
                 var drillDownData = [];
                 scope.legendDiv = 'legenddiv_clustered_bar_toggle' + count;
                 scope.myId = 'chart_div_clustered' + (count++);
                 scope.showlegend = true;
                 scope.ReportData = {};
                 scope.groupPropertycustomer = '';
                 function formatNumber(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };
                 var initChart = function (chartData, title, labelRotation) {
                     var balloontextPrefix = "$";
                     chart = AmCharts.makeChart(scope.myId, {
                         "responsive": {
                             //"enabled": true
                             "minWidth": 200,
                             "maxWidth": 400,
                             "maxHeight": 400,
                             "minHeight": 200,
                             "overrides": {
                                 "precision": 2,
                                 "legend": {
                                     "enabled": false
                                 },
                                 "valueAxes": {
                                     "inside": true
                                 }
                             }
                         },
                         "processTimeout": 1,
                         "theme": "none",
                         "type": "serial",
                         //"dataProvider": chartData,
                         "valueAxes": [{
                             "stackType": "regular",
                             "position": "left",
                             "title": (scope.valueAxesTitle) ? scope.valueAxesTitle : "Transportation"
                         }],
                         "graphs": [

                            //this obj only for label val1 and val2 column
                            {
                                "id": "00",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "",
                                "type": "column",
                                "valueField": "Label",
                                "showAllValueLabels": true,
                                "balloonText": "",
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column1;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
                                "autoWrap": true
                            },
                            //val1 & val2
                            {
                                "id": "g1",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "title": "",
                                "type": "column",
                                "valueField": "NumberOfStops1",
                                "fillColorsField": "Color1",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                  
                                    var Tripspan = calcpercentage(graphDataItem, true, "Val");
                                    var Stopspan = calcpercentage(graphDataItem, true, "NumberOfStops");
                                    var CasesDeliveredspan = calcpercentage(graphDataItem, true, "CasesDelivered");

                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column1 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops1 + Stopspan + " </b> <br /> Trips : " + graphDataItem.dataContext.Val1 + Tripspan + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered1) + CasesDeliveredspan + "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column1 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.DriverName + " (" + graphDataItem.dataContext.Category + ") </b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops1 + Stopspan + " </b><br /> Trips : " + graphDataItem.dataContext.Val1 + Tripspan + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered1) + CasesDeliveredspan + "<br /><b style='font-size: 130%'></b>";
                                    };

                                    //if (drillDownCount == 0) {
                                    //    return graphDataItem.dataContext.Column1 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /> Trips : " + graphDataItem.dataContext.Val1 + Tripspan + "<br /> Stops : " + graphDataItem.dataContext.NumberOfStops1 +Stopspan+ "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered1)+CasesDeliveredspan + "<br /><b style='font-size: 130%'></b>";
                                    //} else {
                                    //    return graphDataItem.dataContext.Column1 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.DriverName + " (" + graphDataItem.dataContext.Category + ") </b><br /> Trips :" + graphDataItem.dataContext.Val1 + Tripspan + "<br /> Stops : " + graphDataItem.dataContext.NumberOfStops1 +Stopspan+ "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered1) +CasesDeliveredspan+ "<br /><b style='font-size: 130%'></b>";
                                    //};


                                }
                            },
                             //this obj only for label val3 and val4 column
                            {
                                "id": "00",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "newStack": true,
                                "title": "",
                                "type": "column",
                                "valueField": "Label",
                                "balloonText": "",
                                "showAllValueLabels": true,
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    return (drillDownCount == 0) ? graphDataItem.dataContext.Column2 : "";
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
                                "autoWrap": true
                            },
                              //val3 & val4
                            {
                                "id": "g2",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "NumberOfStops2",
                                "fillColorsField": "Color2",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column2 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops2 + " </b> <br /> Trips : " + graphDataItem.dataContext.Val2 + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered2) +  "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column2 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.DriverName + " (" + graphDataItem.dataContext.Category + ") </b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops2 + " </b><br /> Trips : " + graphDataItem.dataContext.Val2 +  "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered2) + "<br /><b style='font-size: 130%'></b>";
                                    };

                                    //if (drillDownCount == 0) {
                                    //    return graphDataItem.dataContext.Column2 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /> Trips  " + graphDataItem.dataContext.Val2 + "<br /> Stops  " + graphDataItem.dataContext.NumberOfStops2 + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered2) + "<br /><b style='font-size: 130%'></b>";
                                    //} else {
                                    //    return graphDataItem.dataContext.Column2 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.DriverName + " (" + graphDataItem.dataContext.Category + ") </b><br /> Trips  " + graphDataItem.dataContext.Val2 + "<br /> Stops  " + graphDataItem.dataContext.NumberOfStops2 + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered2) + "<br /><b style='font-size: 130%'></b>";
                                    //};
                                }
                            },
                           //this obj only for label val3 and val4 column
                            {
                                "id": "00",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "newStack": true,
                                "title": "",
                                "type": "column",
                                "valueField": "Label",
                                "balloonText": "",
                                "showAllValueLabels": true,
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    return (drillDownCount == 0) ? graphDataItem.dataContext.Column3 : "";
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
                                "autoWrap": true
                            },
                            //val 5 & val 6
                            {
                                "id": "g3",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "NumberOfStops3",
                                "fillColorsField": "Color3",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                    var Tripspan = calcpercentage(graphDataItem, false, "Val");
                                    var Stopspan = calcpercentage(graphDataItem, false, "NumberOfStops");
                                    var CasesDeliveredspan = calcpercentage(graphDataItem, false, "CasesDelivered");

                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column3 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops3 + Stopspan + " </b> <br /> Trips : " + graphDataItem.dataContext.Val3 + Tripspan + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered3) + CasesDeliveredspan + "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column3 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.DriverName + " (" + graphDataItem.dataContext.Category + ") </b>" + "<br /> <b> Stops : " + graphDataItem.dataContext.NumberOfStops3 + Stopspan + " </b><br /> Trips : " + graphDataItem.dataContext.Val3 + Tripspan + "<br />Cases : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.CasesDelivered3) + CasesDeliveredspan + "<br /><b style='font-size: 130%'></b>";
                                    };

                                }
                            },
                         ],
                         "plotAreaFillAlphas": 0.1,
                         "depth3D": 30,
                         "angle": 30,
                         "categoryField": "Category",
                         "categoryAxis": {
                             "gridPosition": "start",
                             "tickPosition": "start",
                             "labelOffset": (drillDownCount == 0) ? 44 : 0,
                             "boldLabels": (drillDownCount == 0) ? true : false,
                             "labelRotation": labelRotation,
                             "labelFunction": function (label, item, axis) {
                                 var chart = axis.chart;
                                 if ((chart.realWidth <= 300) && (label.length > 5))
                                     return label.substr(0, 5) + '...';
                                 if ((chart.realWidth <= 500) && (label.length > 10))
                                     return label.substr(0, 10) + '...';
                                 return label;
                             },

                             "labelOffset": (drillDownCount == 0) ? 60 : 0,
                             "autoWrap": true
                         },
                         "legend": {
                             "position": "bottom",
                             "data": generateLegend(chartData),
                             "autoMargins": false,
                             "equalWidths": false,
                             "divId": scope.legendDiv
                         },
                         "columnSpacing": 12,
                     });

                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][4] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][5] : null;


                     chart.dataProvider = chartData;
                     chart.validateData();
                     chart.addListener('clickGraphItem', function (event) {
                       
                         var catTitle;
                         var graphid = event.graph.id;
                         if (graphid !== "00") {

                             catTitle = event.item.dataContext.Category;

                             if (event.item.dataContext.SubData != undefined) {

                                 subChartData = (scope.topBottomToggle) ? event.item.dataContext.SubData.Top : event.item.dataContext.SubData.Bottom;
                                 drillDownData[drillDownCount] = subChartData;

                                 if (drillDownCount == 0) {

                                     scope.categoriestitleld = "Transportation" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     scope.categoriesTitlePrevious = "Transportation" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     scope.callbackFn({ title: scope.categoriesTitlePrevious });
                                 }

                                 drillDownCount += 1;

                                 scope.showlegend = (drillDownCount == 1) ? false : true;


                                 subData = event.item.dataContext.SubData;
                                 event.chart.legend = null;
                           
                                 initChart(subChartData, "", 35);
                                 chart.validateData();
                                 scope.$apply(function () {
                                     scope.backBtnVisible = true;
                                 });

                             }
                             else {

                                     var graphid = event.graph.id;
                                     if (graphid !== "00") {
                                         var clickedindex = event.item.index;
                                         var selectedData = chartData[clickedindex];
                                         var selectedPeriod = "";
                                         var Period;
                                         switch (graphid) {
                                             case "g1":
                                                 selectedPeriod = selectedData.Period1;
                                                 Period = 0;
                                                 break;
                                             case "g2":
                                                 selectedPeriod = selectedData.Period2;
                                                 Period = 1;
                                                 break;
                                             case "g3":
                                                 selectedPeriod = selectedData.Period3;
                                                 Period = 2;
                                                 break;
                                         }
                                         scope.categoriesTitlesecond = scope.categoriestitleld + "<span class='small-text'> > " + event.item.category + "> Report </span>";
                                         scope.callbackFn({ title: scope.categoriesTitlesecond });
                                         scope.$apply(function () {
                                             scope.showChart = false;
                                             scope.showReport = true;
                                         });
                                       
                                         var filterId = $rootScope.currentfilter.Id;
                                         scope.toggledateinvoice = true;

                                         var requestData = {
                                             IsCM01: "",
                                             OrderBy: "",
                                             CustomerNumber: "",
                                             FilterId: filterId,
                                             Period: Period,
                                             AccountType: "",
                                             AccountNumber: "",
                                             DriverCode: event.item.category
                                         };
                                         GetDashboardDriverTripDrillDownReport(requestData);
                                        
                                     }
                                     else {
                                         scope.customersData = [];
                                         scope.customersDataSafe = [];
                                     }



                              
                             }

                         }
                       
                         scope.reportback = function () {
                            
                             var drill = drillDownCount;
                             if (drillDownCount == 1) {
                                 scope.callbackFn({ title: scope.categoriestitleld });
                             }
                             else {
                                 scope.callbackFn({ title: scope.categoriesTitlesecond });
                             }

                         }
                         function GetDashboardDriverTripDrillDownReport(requestData) {
                             Metronic.blockUI({ boxed: true });
                             scope.customersData = [];
                             scope.customersDataSafe = [];

                             scope.customersDataDetailedReport = [];
                             scope.customersDataSafeDetailedReport = [];

                             scope.customersDataCustomerReport = [];
                             scope.customersDataSafeCustomerReport = [];

                             dataService.GetDashboardDriverTripDrillDownReport(requestData).then(function (response) {
                                 if (response && response.data) {
                                     Metronic.unblockUI();
                                     scope.ReportData = response.data;
                                     debugger;
                                     scope.customersData = response.data.DayReport;
                                     scope.customersDataSafe = response.data.DayReport;


                                     scope.customersDataDetailedReport = response.data.DetailedReport;
                                     scope.customersDataSafeDetailedReport = response.data.DetailedReport;

                                     scope.customersDataCustomerReport = response.data.CustomerReport;
                                     scope.customersDataSafeCustomerReport = response.data.CustomerReport;

                                     console.log(scope.ReportData)
                                     calcTotal(scope.customersDataSafe, scope.customersDataSafeDetailedReport, scope.customersDataSafeCustomerReport)
                                 }
                                 Metronic.stopPageLoading();
                             }, function onError() {
                                 Metronic.unblockUI();
                                 Metronic.stopPageLoading();
                                 NotificationService.Error("Error upon the API request");
                                 NotificationService.ConsoleLog('Error on the server');
                             });
                         }
                     });

                    
                     function generateLegend(legData) {
                         function getyear(date) {
                             var d = new Date(date);
                             return d.getFullYear();
                         };
                         var tempLegend = [];
                         var legendNames = [];

                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column1 : "",
                             color: (legData[0]) ? legData[0].Color1 : "",
                         })
                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column3 : "",
                             color: (legData[0]) ? legData[0].Color1 : "",
                         })
                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column2 : "",
                             color: (legData[0]) ? legData[0].Color2 : "",
                         })
                         return tempLegend;
                     }

                 };
                 var calcpercentage = function (graphDataItem, Previousmonth,type) {
                     var first;
                     var second;

                     if (Previousmonth) {
                         first = graphDataItem.dataContext[type+"1"];
                         second = graphDataItem.dataContext[type + "2"];
                     }
                     else {
                         first = graphDataItem.dataContext[type+"3"];
                         second = graphDataItem.dataContext[type+ "2"];
                     }


                     var PriorMonth = first - second;
                     if (PriorMonth !== 0) {
                         PriorMonth = PriorMonth / second;
                         PriorMonth = PriorMonth * 100;
                         PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
                     }
                     else {
                         PriorMonth = 0;
                     }
                     scope.PriorMonth = Math.round(PriorMonth);
                     var span = "";
                     if (scope.PriorMonth > 0) {
                         return span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                     }
                     if (scope.PriorMonth < 0) {
                         return span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                     }
                     if (scope.PriorMonth == 0) {
                         return span = "";
                     }
                 };
                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     scope.backBtnVisible = false;
                     drillDownCount = 0;
                     scope.showChart = true;
                     scope.showReport = false;
                     data = newVal;
                     chartTitle = "";
                     if (data == undefined || data.length == 0) {

                     }
                     else {
                         initChart(data, "", 0);
                     }

                 });

                 scope.$watch('backBtnVisible', function (newVal, oldVal) {
                     if (!newVal && oldVal) {
                         chartTitle = "";
                         initChart(data, "", 0);
                         subData = null;
                     }
                 });

                 scope.$watch('topBottomToggle', function () {
                     if (subData != null) {
                         subChartData = (scope.topBottomToggle) ? subData.Top : subData.Bottom;
                         initChart(subChartData, "" , 35);
                     }
                 });

             

                 //scope.loadDestinationFacilityChart = function (toggledateinvoice) {
                   
                 //    scope.toggledateinvoice = toggledateinvoice;
                  
                 //}
                 function calcTotal(data,data1,data2) {
                     scope.TotalNoofStops = 0;
                     scope.TotalNoofTrips = 0;
                     scope.TotalCasesDelivered = 0;
                     scope.TotalNoCasesDeliveredaDetailedReport = 0;
                     scope.TotalNumberOfInvoices = 0;
                     if (data.length > 0) {
                         for (var i = 0; i < data.length; i++) {
                             scope.TotalNoofStops = scope.TotalNoofStops + data[i].NumberOfStops;
                             scope.TotalNoofTrips = scope.TotalNoofTrips + data[i].NumberOfTrips;
                             scope.TotalCasesDelivered = scope.TotalCasesDelivered + data[i].CasesDelivered;
                         }
                     }

                     if (data2.length > 0) {
                         for (var i = 0; i < data2.length; i++) {
                             
                           //  scope.TotalNoCasesDeliveredaDetailedReport = scope.TotalNoCasesDeliveredaDetailedReport + data2[i].CasesDelivered;
                             scope.TotalNumberOfInvoices = scope.TotalNumberOfInvoices + data2[i].NumberOfInvoices;
                         }
                     }
                  
                    
                 }

                 scope.clickedButtonFn = function (data) {
                    
                     scope.CurrentData = data;
                 }

                 scope.backChart = function () {
                    
                     scope.showlegend = true;
                     var cData = (drillDownCount == 1) ? data : drillDownData[drillDownCount - 2];
                     drillDownCount -= 1;
                     initChart(cData, "", 35);
                     scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                     if (drillDownCount == 0) {
                         scope.callbackFn({ title: "initial" });
                     }
                     if (drillDownCount == 1) {
                         scope.callbackFn({ title: scope.categoriesTitlePrevious });
                     }
                 }
              
             }
         };
     }]);






