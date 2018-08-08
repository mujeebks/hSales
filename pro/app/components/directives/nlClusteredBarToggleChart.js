
MetronicApp.directive('nlClusteredBarToggleChart', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {
                 nlLocation: '=', valueAxesTitle: "=", useGraphCategoryAxisTitle: "=", topBottomToggleClusterCountLabel: "=",
                 hideChartTitle: "=",
                 callbackFn: '&', objectToInject: '=',nlChart:'='
             },


             template: '<div class="portlet-body" ng-init="showChart=true">' +
                      '<div ng-show="showChart">' +
                      '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                      '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="backChart()" style="cursor:pointer;"></i>' +

                      '</div>' +
                     '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                     '</br>' +
                      '</br>' +
                       '</br>' +
                        '<div style="text-align:right; position:relative; top:0px;" ng-show="backBtnVisible">' +
                        '<toggle-switch ng-model="topBottomToggle" ng-init="topBottomToggle=true" on-label="Top 10" off-label="Bottom 10" ng-click="loadDestinationFacilityChart()">' +
                         '<toggle-switch>' +
                         '</div>' +
                     '<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                     '</div>' +
                     '<div ng-show="showReport">' +
                     '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +

                           '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true;reportback()" style="cursor:pointer;"></i>' +
                      '</div>' +
                          '<div style="margin-top: -46px;">' +
                      '<div ng-include="\'app/pages/expenses/expenses-report-table.html\'"></div>' +
                     '</div>' +
                      '</div>' +
                      '</div>',
             link: function (scope, element, attrs, controller) {

              

                 scope.toggleClusterCountLabel = (scope.topBottomToggleClusterCountLabel) ? scope.topBottomToggleClusterCountLabel : 5;
                 var chart;
                 var data;
                 var subData = null;
                 var chartTitle = "";
                 var title;
                 var drillDownCount = 0;
                 var drillDownData = [];
                 scope.legendDiv = 'legenddiv_clustered_bar_toggle' + count;
                 scope.myId = 'chart_div_clustered' + (count++);
                 var clickTimeout = 0;
                 var lastClick = 0;
                 var doubleClickDuration = 200;
                 var drilldownCategoryAxistitle = "";
                 function formatNumber(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };
                 var initChart = function (chartData, title, labelRotation) {

                     element;
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
                             "title": (scope.valueAxesTitle) ? scope.valueAxesTitle : "Revenue"
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
                               "balloonText":"",
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
                                "valueField": "Val1",
                                "fillColorsField": "Color1",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                    var value = formatNumber(graphDataItem.dataContext.Val1);
                                    if (value == 0) {
                                        return "";
                                    }
                                    else {

                                        var percentagediff=graphDataItem.dataContext.Val1-graphDataItem.dataContext.Val2;
                                        percentagediff=percentagediff/graphDataItem.dataContext.Val2;
                                        percentagediff = percentagediff * 100;
                                        percentagediff = (percentagediff == Infinity) ? 100 : percentagediff;
                                        scope.PriorMonth =Math.round(percentagediff);
                                        var span = "";

                                        if (attrs.nlChart == "Opex-Cogs") {
                                            if (scope.PriorMonth > 0) {
                                                span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                                            }
                                            if (scope.PriorMonth < 0) {
                                                span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                                               
                                            }
                                            if (scope.PriorMonth == 0) {
                                                return span = "";
                                            }
                                        }
                                        else {
                                            if (scope.PriorMonth > 0) {
                                                span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                                            }
                                            if (scope.PriorMonth < 0) {
                                                span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                                            }
                                            if (scope.PriorMonth == 0) {
                                                return span = "";
                                            }
                                        }


                                        return graphDataItem.dataContext.Column1 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + value + span + "</b>";
                                    }

                                }

                            },



                             //this obj only for label val3 and val4 column
                            {
                                "id": "00",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "newStack":true,
                                "title": "",
                                "type": "column",
                                "valueField": "Label",
                                "balloonText": "",
                                "showAllValueLabels": true,
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column2;
                                    }
                                    else {
                                        return "";
                                    }
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
                                "valueField": "Val2",
                                "fillColorsField": "Color2",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                    var value = formatNumber(graphDataItem.dataContext.Val2);
                                    if (value == 0) {
                                        return "";
                                    }
                                    else {
                                        return graphDataItem.dataContext.Column2 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + value + " <br /></b>";
                                    }

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
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column3;
                                    }
                                    else {
                                        return "";
                                    }
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
                                "valueField": "Val3",
                                "fillColorsField": "Color3",

                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                    var value = formatNumber(graphDataItem.dataContext.Val3);
                                    if (value == 0) {
                                        return "";
                                    }
                                    else {
                                        var percentagediff = graphDataItem.dataContext.Val1 - graphDataItem.dataContext.Val3;
                                        percentagediff = percentagediff / graphDataItem.dataContext.Val3;
                                        percentagediff = percentagediff * 100;
                                        percentagediff = (percentagediff == Infinity) ? 100 : percentagediff;
                                        scope.PriorMonth = percentagediff;
                                        var span = "";

                                        if (attrs.nlChart == "Opex-Cogs") {
                                            if (scope.PriorMonth > 0) {
                                                span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                                            }
                                            if (scope.PriorMonth < 0) {
                                                span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";

                                            }
                                            if (scope.PriorMonth == 0) {
                                                return span = "";
                                            }
                                        }

                                        else {
                                            if (scope.PriorMonth >= 0) {
                                                span = "<span><br /><span class='fa fa-sort-up' style='color:forestgreen'>" + Math.abs(scope.PriorMonth) + "</span></span>";
                                            }
                                            if (scope.PriorMonth < 0) {
                                                span = "<span><br /><span class='fa fa-sort-down' style='color:red'>" + Math.abs(scope.PriorMonth) + "</span></span>";
                                            }
                                        }
                                        return graphDataItem.dataContext.Column3 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + value+ "</b>";
                                    }

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

                             "labelOffset":(drillDownCount==0)? 60:0,

                             "autoWrap": true
                         },
                         "legend": showlegend(chartData),
                         "columnSpacing": 12,
                     });

                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][4] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][5] : null;


                     chart.dataProvider = chartData;
                     chart.validateData();

                     chart.addListener('clickGraphItem', function (event) {
                         var cat;
                         var catTitle;
                         var graphid = event.graph.id;
                         if (graphid !== "00") {

                             var date = "";

                                 switch (graphid) {
                                     case "g1":
                                         date = event.item.dataContext.Column1;
                                         break;
                                     case "g2":
                                         date = event.item.dataContext.Column2;
                                         break;
                                     case "g3":
                                         date = event.item.dataContext.Column3;
                                         break;

                                 }

                                 cat = event.item.dataContext.Category;
                                 catTitle = event.item.dataContext.Category + " (" + date + ") ";
                             //}

                             if (event.item.dataContext.SubData != undefined) {

                                 subChartData = (scope.topBottomToggle) ? event.item.dataContext.SubData.Top : event.item.dataContext.SubData.Bottom;
                                 drillDownData[drillDownCount] = subChartData;

                                 if (drillDownCount == 0) {

                                     scope.categoriestitleld = "Expenses" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     scope.categoriesTitlePrevious = "Expenses" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     scope.callbackFn({ title: scope.categoriesTitlePrevious });
                                 }

                                 if (drillDownCount == 1) {


                                     scope.categoriesTitlesecond = scope.categoriestitleld + "<span class='small-text'> > " + event.item.category + " </span>";
                                     scope.callbackFn({ title: scope.categoriesTitlesecond });

                                 }

                                 drillDownCount += 1;


                                 subData = event.item.dataContext.SubData;
                                 event.chart.legend = null;
                                 chartTitle = title;
                                 drilldownCategoryAxistitle = (scope.useGraphCategoryAxisTitle) ? title : "Sales Person";
                                 initChart(subChartData, drilldownCategoryAxistitle, 35);
                                 chart.validateData();
                                 scope.$apply(function () {
                                     scope.backBtnVisible = true;
                                 });

                             }
                             else {

                                 if (attrs.nlChart == "Opex-Cogs") {

                                     scope.accountName = event.item.category;
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

                                         scope.$apply(function () {
                                             scope.showChart = false;
                                             scope.showReport = true;
                                         });


                                         var filterId = $rootScope.currentfilter.Id;

                                         var requestData = {
                                             Category: "",
                                             Commodity: "",
                                             SalesPerson: "",
                                             FilterId: filterId,
                                             Period: Period,
                                             AccountType: (scope.cat == "COGS") ? 1 : 0,
                                             AccountNumber: event.item.dataContext.AccountNumber
                                         };

                                         GetOPEXCOGSExpenseReport(requestData);
                                         var drill = drillDownCount;
                                         if (drillDownCount == 1) {
                                             scope.categoriesTitlesecond = scope.categoriestitleld + "<span class='small-text'> > " + scope.accountName + " </span>";
                                             scope.callbackFn({ title: scope.categoriesTitlesecond + "<span class='small-text'></span>" + "<span class='small-text'> > Report</span>" });
                                         }
                                         else {
                                             scope.callbackFn({ title: scope.categoriesTitlesecond + "<span class='small-text'> > " + event.item.category + "</span>" + "<span class='small-text'> > Report</span>" });
                                         }
                                     }
                                     else {
                                         scope.customersData = [];
                                         scope.customersDataSafe = [];
                                     }


                                 }

                                 if (attrs.nlChart == "Transportation") {
                                     //alert("efewffe");
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
                         function GetOPEXCOGSExpenseReport(requestData) {

                             scope.customersData = [];
                             scope.customersDataSafe = [];

                             dataService.GetExpensesPersonCustomersData(requestData).then(function (response) {
                                 if (response && response.data) {
                                     scope.customersData = response.data;
                                     scope.customersDataSafe = response.data;
                                 }
                                 Metronic.stopPageLoading();
                             }, function onError() {
                                 Metronic.stopPageLoading();
                                 NotificationService.Error("Error upon the API request");
                                 NotificationService.ConsoleLog('Error on the server');
                             });
                         }
                     });

                     function showlegend(chartData) {
                         if (chartData == undefined) {
                             return null;
                         }
                         else {
                             return {
                                 "position": "bottom",
                                 "data": generateLegend(chartData),
                                 "autoMargins": false,
                                 "equalWidths": false,
                                 "divId": scope.legendDiv
                             }
                         }


                     };
                     function generateLegend(legData) {

                         function getyear(date) {
                             var d = new Date(date);
                             return d.getFullYear();

                         };
                         var tempLegend = [];
                         var legendNames = [];

                         tempLegend.push({
                             title:(legData[0])? legData[0].Column1:"",
                             color: (legData[0])?legData[0].Color1:"",
                         })
                         tempLegend.push({
                             title: (legData[0])?legData[0].Column3:"",
                             color: (legData[0])?legData[0].Color1:"",
                         })
                         tempLegend.push({
                             title: (legData[0])?legData[0].Column2:"",
                             color:(legData[0])? legData[0].Color2:"",
                         })
                         return tempLegend;
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
                         //scope.callbackFn({ title: "initial" });



                         subData = null;
                     }
                 });

                 scope.$watch('topBottomToggle', function () {

                     var c = chart;
                     if (subData != null) {
                         subChartData = (scope.topBottomToggle) ? subData.Top : subData.Bottom;
                         initChart(subChartData, drilldownCategoryAxistitle, 35);
                     }
                 });

                 scope.backChart = function () {

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

                 scope.reportback = function () {
                     scope.callbackFn({ title: $rootScope.secondlast });
                 };
             }
         };
     }]);








