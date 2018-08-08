
MetronicApp.directive('nlCommodityBarChart', ['$location', '$rootScope', '$filter', '$state',
     function ($location, $rootScope, $filter, $state) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: { nlLocation: '=', nlDashboard: '=', callbackFn: '&' ,xtitle:'='},
             template: '<div class="portlet-body">' +
                        '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                        '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click=" backBtnVisible  = false; " style="cursor:pointer;"></i>' +
                        '</div>' +
                        '<div ng-if="!showReport">' +
                       '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                       '<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                       '</div>' +
                       '<div ng-if="showReport">' +
                          '<div ng-include="\'app/pages/expenses/expenses-commodity-report-table.html\'"></div>' +
                       '</div>'+
                       '</div>',

             link: function (scope, element, attrs, controller) {
                 scope.legendDiv = "CommodityBarChart_legend" + count;
                 scope.myId = 'CommodityBarChart_chart' + (count++);
                 function formatCommaSeperate(num) { return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")};
                 var drillDownCount = 0;
                 var chart;
                 var data;
                 var valueAxixTitle = (attrs.nlDashboard == "casesold") ? "No oF Cases Sold"
                                    : (attrs.nlDashboard == "revenue") ? "Revenue"
                                    : (attrs.nlDashboard == "expenses") ? "Expenses" : "";
                 var balloonTextPrefix = (attrs.nlDashboard == "casesold") ? ""
                                    : (attrs.nlDashboard == "revenue") ? "$"
                                    : (attrs.nlDashboard == "expenses") ? "$" : "";
                 var showLegend = true;
                 scope.showReport = false;
                 var initChart = function (chartData, title, labelRotation) {
                   
                     var BallonFunction=   function(graphDataItem,index) {
                         var value = graphDataItem.dataContext["Val" + index];
                         var span = calcpercentage(graphDataItem);
                            return graphDataItem.category + "<br /><b style='font-size: 130%'> " + graphDataItem.dataContext["Column" + index] + " </b> <br />" + formatCommaSeperate(Math.round(graphDataItem.dataContext["Val" + index])) + span;
                     };
                     chart = AmCharts.makeChart(scope.myId, {
                         "responsive": {
                             "minWidth": 200,
                             "maxWidth": 400,
                             "maxHeight": 400,
                             "minHeight": 200,
                             "overrides": {
                                 "precision": 2,
                                 "legend": {"enabled": false},
                                 "valueAxes": {"inside": true}
                             }
                         },
                         "processTimeout": 1,
                         "theme": "none",
                         "type": "serial",
                         "valueAxes": [{"stackType": "regular","position": "left","title": valueAxixTitle}],
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
                                "labelText": " ",
                                "labelFunction": function label(graphDataItem) { return (drillDownCount == 0) ? graphDataItem.dataContext.Column1 : "";},
                                "labelOffset": -10,
                                "labelRotation": 0,
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
                                "balloonFunction": function (graphDataItem, graph) {return BallonFunction(graphDataItem, 1) }
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
                                "showAllValueLabels": true,
                                "labelText": " ", 
                                "labelFunction": function label(graphDataItem) {
                                    return  (drillDownCount == 0) ? graphDataItem.dataContext.Column2:"";
                                },

                                "labelOffset": -10,
                                "labelRotation": 0,
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
                                "balloonFunction": function (graphDataItem, graph) { return BallonFunction(graphDataItem, 2) }

                            },
                           //this obj only for label val3 and val4 column
                            {
                                "id": "00",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "newStack": true,
                                "title": "1200",
                                "type": "column",
                                "valueField": "Label",
                                "showAllValueLabels": true,
                                "labelText": " ", 
                                "labelFunction": function label(graphDataItem) {
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column3;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                                "labelOffset": -10,
                                "labelRotation": 0,
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
                                "balloonText": " ",
                                "balloonFunction": function (graphDataItem, graph) { return BallonFunction(graphDataItem, 3) }
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
                             "labelRotation": labelRotation,
                             "labelFunction": function (label, item, axis) {
                                 var chart = axis.chart;
                                 if ((chart.realWidth <= 300) && (label.length > 5))
                                     return label.substr(0, 5) + '...';
                                 if ((chart.realWidth <= 500) && (label.length > 10))
                                     return label.substr(0, 10) + '...';
                                 return label;
                             },
                             "autoWrap": true
                         },
                         "legend": showlegend(chartData),
                         "columnSpacing": 12,
                     });
                     chart.dataProvider = chartData;
                     chart.validateData();
                     var DrilldownData = [];
                     chart.addListener('clickGraphItem', function (event) {
                         debugger;
                       
                             if (event.item.dataContext.SubData != undefined) {
                                 chart.legend = null;
                                 showLegend = (event.item.dataContext.SubData.length == 0) ? false : true;
                                 subChartData = event.item.dataContext.SubData;
                                 var title = event.item.dataContext.Category;
                                 drillDownCount += 1;
                                 initChart(subChartData, title, 35);
                                 DrilldownData[drillDownCount] = subChartData;
                                 scope.showReport = false;

                                 if (drillDownCount == 1) {
                                     scope.firstdrilldowntext = attrs.xtitle.toUpperCase() + " <br/><span class='small-text'> " + title + "</span>";
                                     scope.callbackFn({ title: scope.firstdrilldowntext });
                                 }
                                 scope.$apply(function () {scope.backBtnVisible = true;});
                             }
                             else {
                                 scope.showReport = true;
                                 chart.legend = null;
                                 showLegend = true;
                                 //   initChart(data, "", 0);
                                 scope.firstdrilldownreporttext = scope.firstdrilldowntext + "> <span class='small-text'> " + event.item.category + "> Report</span>";
                              
                                 scope.$apply(function () { scope.backBtnVisible = false; });

                                 scope.callbackFn({ title: scope.firstdrilldownreporttext });
                             }
                     });
                     function showlegend(chartData) {
                         if (chartData == undefined) {
                             return null;
                         }
                         else {
                             return  {
                                  "enabled": showLegend,
                                 "position": "bottom",
                                 "data": generateLegend(chartData),
                                 "autoMargins": false,
                                 "equalWidths": false,
                                 "divId": scope.legendDiv
                             }
                         }
                     };
                     function generateLegend(legData) {
                        if (legData.length>0) {
                            var tempLegend = [];
                            var legendNames = [];
                            if (drillDownCount == 1) {
                                tempLegend.push({
                                    title: legData[0].Column1,
                                    color: legData[0].Color1
                                })
                                tempLegend.push({
                                    title: legData[0].Column2,
                                    color: legData[0].Color2
                                })
                            }
                            else {
                                tempLegend.push({
                                    title: legData[0].Category + " " + legData[0].Column1,
                                    color: legData[0].Color1
                                })
                                tempLegend.push({
                                    title: legData[0].Category + " " + legData[0].Column2,
                                    color: legData[0].Color2
                                })

                                tempLegend.push({
                                    title: legData[1].Category + " " + legData[1].Column1,
                                    color: legData[1].Color1
                                })
                                tempLegend.push({
                                    title: legData[1].Category + " " + legData[1].Column2,
                                    color: legData[1].Color2
                                })
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
                         drillDownCount = 0;
                         initChart(data, "", 0);
                         scope.callbackFn({ title: "initial" });
                     }
                 });

                 var calcpercentage = function (graphDataItem) {
                     var first = graphDataItem.dataContext.Val1;
                     var second = graphDataItem.dataContext.Val2;
                     var PriorMonth = first - second;
                     if (PriorMonth !== 0) {
                         PriorMonth = PriorMonth / second;
                         PriorMonth = PriorMonth * 100;
                         PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
                     }
                     else {
                         PriorMonth = 0;
                     }
                    var PriorMonthselected = Math.round(PriorMonth);
                    if (attrs.nlDashboard == "expenses") {
                        return (PriorMonthselected > 0) ? "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(PriorMonthselected) + " %" + "</span>)</span>" :
                               (PriorMonthselected < 0) ? "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(PriorMonthselected) + " %" + "</span>)</span>" :
                               (PriorMonthselected == 0) ? "" : "";
                    } else {
                        return (PriorMonthselected > 0) ? "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(PriorMonthselected) + " %" + "</span>)</span>" :
                               (PriorMonthselected < 0) ? "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(PriorMonthselected) + " %" + "</span>)</span>" :
                               (PriorMonthselected == 0) ? "" : "";
                     }
                 }
             }
         };
     }]);