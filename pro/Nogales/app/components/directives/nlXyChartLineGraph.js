
MetronicApp.directive('nlXyChartLineGraph', ['$location', '$rootScope', '$filter', '$state', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, $state, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',     
             scope: { nlLocation: '=', nlDashboard: '=', filterType: '=', },

             template: '<div class="portlet-body">' +
                        '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                        '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click=" backBtnVisible  = false " style="cursor:pointer;"></i>' +
                    
                        '</div>' +
                       '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                       '<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                        '</div>',

             link: function (scope, element, attrs, controller) {
                 scope.legendDiv = "nlXyChartLineGraph_legend" + count;
                 scope.myId = 'nlXyChartLineGraph_chart' + (count++);

                 var chart;
                 var data;
                 var valueAxixTitle = getValueAxisTitle();
                 var balloonTextPrefix = "$";
                 var showLegend = true;

                 var initChart = function (chartData, title, labelRotation) {
                     element;
                     chart = AmCharts.makeChart(scope.myId, {
                         "type": "xy",
                         "theme": "light",
                         "marginRight": 80,
                         "dataDateFormat": "YYYY-MM-DD",
                         "startDuration": 1.5,
                         "trendLines": [],
                         "balloon": {
                             "adjustBorderColor": false,
                             "shadowAlpha": 0,
                             "fixedPosition": true
                         },
                         "graphs": [{
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "diamond",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "yellow",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "ay",
                             "valueField": "aValue",
                             "title": "virginia"
                         }, {
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "round",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "blue",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "by",
                             "valueField": "bValue",
                             "title": "National"
                         },
                         {
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "round",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "green",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "cy",
                             "valueField": "cValue",
                             "title": "south dakota"
                         },
                         {
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "round",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "red",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "dy",
                             "valueField": "dValue",
                             "title": "north carolina"
                         },
                         {
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "round",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "gray",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "ey",
                             "valueField": "eValue",
                             "title": "tennessee"
                         },
                         {
                             "balloonText": "<div style='margin:5px;'><b>[[x]]</b><br>y:<b>[[y]]</b><br>value:<b>[[value]]</b></div>",
                             "bullet": "round",
                             "maxBulletSize": 25,
                             "lineAlpha": 0.8,
                             "lineThickness": 2,
                             "lineColor": "black",
                             "fillAlphas": 0,
                             "xField": "date",
                             "yField": "fy",
                             "valueField": "fValue",
                             "title": "maryland"
                         }
                         ],
                         "valueAxes": [{
                             "id": "ValueAxis-1",
                             "axisAlpha": 0
                         }, {
                             "id": "ValueAxis-2",
                             "axisAlpha": 0,
                             "position": "bottom"
                         }],
                         "allLabels": [],
                         "titles": [],
                         "dataProvider": [{
                             "date": 1,
                             "ay": 6.5,
                             "by": 2.2,
                             "aValue": 15,
                             "bValue": 10
                         }, {
                             "date": 2,
                             "ay": 12.3,
                             "by": 29,
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 3.9,
                             "fy": 8.9,
                             "aValue": 8,
                             "bValue": 3,
                             "cValue": 3,
                             "dValue": 5,
                             "eValue": 7,
                             "fValue": 8
                         }, {
                             "date": 3,
                             "ay": 12.3,
                             "by": 5.1,
                             "cy": 4.9,
                             "dy": 7.9,
                             "ey": 6.9,
                             "fy": 4.9,
                             "aValue": 16,
                             "bValue": 4,
                             "cValue": 4,
                             "dValue": 6,
                             "eValue": 7,
                             "fValue": 9
                         }, {
                             "date": 5,
                             "ay": 2.9,
                             "by": 4.1,
                             "cy": 2.9,
                             "dy": 7.9,
                             "ey": 8.9,
                             "fy": 4.9,
                             "aValue": 9,
                             "cValue": 6,
                             "dValue": 4,
                             "eValue": 2,
                             "fValue": 7
                         }, {
                             "date": 7,
                             "by": 8.3,
                             "cy": 5.9,
                             "dy": 3.9,
                             "ey": 9.9,
                             "fy": 6.9,
                             "bValue": 13,
                             "cValue": 6,
                             "dValue": 5,
                             "eValue": 4,
                             "fValue": 3
                         }, {
                             "date": 10,
                             "ay": 4.8,
                             "by": 13.3,
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 7.9,
                             "fy": 4.9,
                             "aValue": 9,
                             "bValue": 13,
                             "cValue": 43,
                             "dValue": 2,
                             "eValue": 6,
                             "fValue": 7
                         }, {
                             "date": 12,
                             "ay": 5.5,
                             "by": 6.1,
                             "cy": 9.9,
                             "dy": 3.9,
                             "ey": 7.9,
                             "fy": 2.9,
                             "aValue": 5,
                             "bValue": 2,
                             "cValue": 6,
                             "dValue": 5,
                             "eValue": 4,
                             "fValue": 3
                         }, {
                             "date": 13,
                             "ay": 5.1,
                             "by": 6.1,
                             "cy": 7.9,
                             "dy": 4.9,
                             "ey": 9.9,
                             "fy": 4.9,
                             "aValue": 10,
                             "cValue": 6,
                             "dValue": 4,
                             "eValue": 3,
                             "fValue": 3
                         }, {
                             "date": 15,
                             "ay": 6.7,
                             "by": 10.5,
                            
                             "cy": 7.9,
                             "dy": 3.9,
                             "ey": 5.9,
                             "fy": 6.9,
                             "aValue": 3,
                             "bValue": 10,
                             "cValue": 6,
                             "dValue": 5,
                             "eValue": 7,
                             "fValue": 8
                         }, {
                             "date": 16,
                             "ay": 8,
                             "by": 12.3,
                            
                             "cy":7.9,
                             "dy": 8.9,
                             "ey": 3.9,
                             "fy": 2.9,
                             "aValue": 5,
                             "bValue": 13,
                             "cValue": 6,
                             "dValue": 4,
                             "eValue": 3,
                             "fValue": 5
                         }, {
                             "date": 20,
                             "by": 4.5,
                           
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 3.9,
                             "fy": 8.9,
                             "bValue": 11,
                             "cValue": 6,
                             "dValue": 2,
                             "eValue": 3,
                             "fValue": 4
                         }, {
                             "date": 22,
                             "ay": 9.7,
                           
                             "by": 6.1,
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 3.9,
                             "fy": 8.9,
                             "aValue": 15,
                             "bValue": 10,
                             "cValue": 4,
                             "dValue": 8,
                             "eValue": 7,
                             "fValue": 6
                         }, {
                             "date": 23,
                             "ay": 10.4,
                             "by": 10.8,
                           
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 3.9,
                             "fy": 8.9,
                             "aValue": 1,
                             "bValue": 11,
                             "cValue": 7,
                             "dValue": 6,
                             "eValue": 2,
                             "fValue": 4
                         }, {
                             "date": 24,
                             "ay": 1.7,
                             "by": 19,
                            
                             "cy": 3.9,
                             "dy": 6.9,
                             "ey": 3.9,
                             "fy": 8.9,
                             "aValue": 12,
                             "bValue": 2,
                             "cValue": 3,
                             "dValue": 4,
                             "eValue": 6,
                             "fValue": 8
                         }],

                         "export": {
                             "enabled": true
                         },

                         "chartScrollbar": {
                             "offset": 15,
                             "scrollbarHeight": 5
                         },

                         "chartCursor": {
                             "pan": true,
                             "cursorAlpha": 0,
                             "valueLineAlpha": 0
                         },
                         "legend": {
                             "enabled": true,
                             "useGraphSettings": true,
                             "position":"right"
                         },
                     });

                     chart.addListener("drawn", function (event) {
                         chart.legend = null;
                         //console.log("drawn");
                     });

                     chart.addListener('clickGraphItem', function (event) {
                         if (attrs.nlLocation == undefined) {
                             if (event.item.dataContext.SubData != undefined) {
                                 showLegend = false;
                                 subChartData = event.item.dataContext.SubData;
                                 var title = event.item.dataContext.Category;
                                 chart.legend = null;
                                 //  event.chart.validateNow();

                                 initChart(subChartData, title, 35);
                                 scope.$apply(function () {
                                     scope.backBtnVisible = true;
                                 });
                             }
                             else {
                                 chart.legend = null;
                                 // event.chart.validateNow();
                                 showLegend = true;

                                 initChart(data, "", 0);
                                 scope.$apply(function () {
                                     scope.backBtnVisible = false;
                                 });
                             }
                         }
                         else {
                             if (event.item.dataContext.Category) {
                                 $state.go(attrs.nlLocation, { category: event.item.dataContext.Category });
                             }
                         }
                     });

                     function generateLegend(legData) {
                         if (legData) {
                             var tempLegend = [];
                             var legendNames = [];
                             for (i = 0; i < legData.length; i++) {
                                 for (var x in legData[i]) {
                                     if ((x == "Column1" || x == "Column2") && legendNames.indexOf(legData[i][x]) < 0) {
                                         var item = {};
                                         item.color = (x == "Column1") ? legData[i].Color1 : legData[i].Color2;
                                         //item.title = legData[i].Category + " in " + legData[i][x];
                                         var date = isNaN(Date.parse(legData[i][x])) ? legData[i][x] + " 01" : legData[i][x];
                                         var columnName = (scope.filterType == "month") ?
                                                            new Date(date + "T24:00:00.000Z").toLocaleString("en-us", { month: "short" }) :
                                                            new Date(date + "T24:00:00.000Z").getFullYear();

                                         item.title = isNaN(legData[i].Category) ? legData[i].Category + " in " + columnName : columnName + " " + legData[i].Category;
                                         tempLegend.push(item);
                                         legendNames.push(legData[i][x]);
                                     }
                                 }
                             }

                             return tempLegend;
                         }
                     }
                 };

                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     scope.backBtnVisible = false;
                     data = newVal;
                     initChart(data, "", 0);
                 });

                 scope.$watch('backBtnVisible', function (newVal, oldVal) {
                     if (!newVal && oldVal) {
                         showLegend = true;
                         initChart(data, "", 0);
                     }
                 });

                 function getValueAxisTitle() {
                  
                     return (attrs.nlDashboard == "casesold") ? "No oF Cases Sold"
                          : (attrs.nlDashboard == "revenue") ? "Revenue"
                          : (attrs.nlDashboard == "expenses") ? "Expenses" : "";
                 }

                 function getBalloonTextPrefix() {
                     return (attrs.nlDashboard == "casesold") ? ""
                           : (attrs.nlDashboard == "revenue") ? "$"
                           : (attrs.nlDashboard == "expenses") ? "$" : "";
                 }
             }
         };
     }]);