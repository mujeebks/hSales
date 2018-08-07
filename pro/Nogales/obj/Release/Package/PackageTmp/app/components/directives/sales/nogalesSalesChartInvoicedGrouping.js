MetronicApp.directive('nogalesSalesChartInvoicedGrouping', ['$location', '$rootScope', '$filter', '$state', 'dataService', 'NotificationService','HelperService',
function ($location, $rootScope, $filter, $state, dataService, NotificationService,HelperService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: { nlLocation: '=', nlDashboard: '=', filterType: '=', customMonth: '=', callbackFn: '&', objectToInject: '=', currentfilter: '=', percentagecallbackFn: '&' },
             template:

           ' <div class="portlet-body" id="sales">' +
                 '<div class="portlet-body" ng-init="showChart=true">' +
                        '<div ng-show="showChart">' +
                        '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                        '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true; backChart()" style="cursor:pointer;"></i>' +

                        '</div>' +
                       '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                       '<div ng-show="drillDownCount==0" id="{{::legendDiv}}"  class="clusteredBarChartLegenddiv1"></div>' +
                       '</div>' +
                       '<div ng-show="showReport">' +
                       '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +
                       '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="reportback()" style="cursor:pointer;"></i>' +

                        '</div>' +
                            '<div style="margin-top: -46px;">' +
                           '<div ng-if="!iscustomersItemReport" ng-include="\'app/pages/sales/sales-report-table.html\'"></div>' +
                         '<div ng-if="iscustomersItemReport" ng-include="\'app/pages/sales/customer-item-table.html\'"></div>' +
                       '</div>' +
                         '</div>' +
                    '</div>' +
               ' </div>' +

                        '</div>',


             link: function (scope, element, attrs, controller) {

                 var Percentage_Data = [];
                 //function formatCommaSeperate(num) {
                 //    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 //};
                 scope.legendDiv = "legenddiv_clustered_with_report" + count;
                 scope.myId = 'chart_div_clustered_with_report1' + (count++);
                 scope.chartLoaded = false;
                 scope.callbackFn({ title: 'initial' });
                 var chart;
                 var data;
                 var valueAxixTitle = getValueAxisTitle();
                 var balloonTextPrefix = "$";
                 scope.isCM01 = false;
                 var drillDownCount = 0;
                 scope.drillDownCount = drillDownCount;
                 var drillDownData = [];

                 scope.sampledata = 0;
                 scope.customersData = [];
                 scope.customersDataSafe = [];
                 scope.myTableFunctions = {};
                 scope.groupProperty = '';

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

                         "valueAxes": [{
                             "stackType": "regular",
                             "position": "left",
                             "title": valueAxixTitle
                         }],
                         "graphs": [

                            //this obj only for label val1 and val2 column
                            {
                                "id": "000",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "",
                                "type": "column",
                                "valueField": "ColumnBottomMargin",
                                "showAllValueLabels": true,
                                "labelText": " ",
                                "balloonText": "",
                                "labelFunction": function (graphDataItem, graph) {
                                    var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label1 : "";

                                    return "";
                                },
                                "labelRotation": -35

                            },
                            //val1 & val2
                            {

                                "id": "1",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue1",
                                "fillColorsField": "Color1",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                 
                                 
                                    var total = 0;

                                    var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue1 : graphDataItem.dataContext.rValue1;
                                    var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue2 : graphDataItem.dataContext.rValue2;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);
                                    var span = calcpercentage(graphDataItem);

                                    return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1)) + "<br/>" + salespersonToolTip;
                                }


                            },
                            {

                                "id": "2",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue2",
                                "fillColorsField": "Color2",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                    //debugger;
                                    var total = 0;

                                    var val1 = graphDataItem.dataContext.rValue1;
                                    var val2 = graphDataItem.dataContext.rValue2;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);
                                    var span = calcpercentage(graphDataItem);

                                    return graphDataItem.dataContext.Label2 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;
                                   
                                }

                            },


                             //this obj only for label val3 and val4 column
                            {
                                "id": "000",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "",
                                "type": "column",
                                "newStack": true,
                                "valueField": "ColumnBottomMargin",
                                "showAllValueLabels": true,
                                "labelText": " ",
                                "balloonText": "",
                                "labelFunction": function (graphDataItem, graph) {
                                    var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label2 : "";

                                    return "";

                                },
                                "labelRotation": -35

                            },
                              //val3 & val4
                            {

                                "id": "3",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue3",
                                "fillColorsField": "Color3",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                    //debugger;
                                    var total = 0;

                                    var val1 = graphDataItem.dataContext.rValue3;
                                    var val2 = graphDataItem.dataContext.rValue4;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);

                                    return graphDataItem.dataContext.Label3 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />"
                                    salespersonToolTip;

                                    
                                }

                            },
                            {

                                "id": "4",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue4",
                                "fillColorsField": "Color4",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                    //debugger;
                                    var total = 0;

                                    var val1 = graphDataItem.dataContext.rValue3;
                                    var val2 = graphDataItem.dataContext.rValue4;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);

                                    return graphDataItem.dataContext.Label4 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />"
                                    salespersonToolTip;

                                }

                            },


                           //this obj only for label val3 and val4 column
                            {
                                "id": "000",
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "",
                                "type": "column",
                                "newStack": true,
                                "valueField": "ColumnBottomMargin",
                                "showAllValueLabels": true,
                                "labelText": " ",
                                "balloonText": "",
                                "labelFunction": function (graphDataItem, graph) {
                                    var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label3 : "";
                                    return "";
                                },
                                "labelRotation": -35

                            },

                            //val 5 & val 6
                            {

                                "id": "5",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue5",
                                "fillColorsField": "Color5",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                    var total = 0;

                                    var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue5 : graphDataItem.dataContext.rValue5;
                                    var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue6 : graphDataItem.dataContext.rValue6;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);
                                    var span = calcpercentage(graphDataItem, true);

                                    return graphDataItem.dataContext.Label5 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1)) + "<br/>"
                                    salespersonToolTip;

                                    
                                }

                            },
                            {

                                "id": "6",
                                "fillAlphas": 1,
                                "lineAlpha": 0.2,
                                "newStack": false,
                                "title": "",
                                "type": "column",
                                "valueField": "rValue6",
                                "fillColorsField": "Color6",
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                    var total = 0;
                                    var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue5 : graphDataItem.dataContext.rValue5;
                                    var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue6 : graphDataItem.dataContext.rValue6;
                                    var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";
                                    total = val1 + val2;
                                    total = Math.round(((total) * 100) / 100);
                                    var span = calcpercentage(graphDataItem, true);
                                    return graphDataItem.dataContext.Label6 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1)) + "<br/>"
                                    salespersonToolTip;
                                }
                            }
                         ],
                         "plotAreaFillAlphas": 0.1,
                         "depth3D": 30,
                         "angle": 30,
                         "categoryField": "GroupName",
                         "categoryAxis": {
                             "title": title,
                             "labelOffset": 15,
                             "gridPosition": "start",
                             "tickPosition": "start",
                             "labelOffset": 34,
                             "labelRotation": 50,
                             "ignoreAxisWidth": false,
                             "autoWrap": true,
                             "labelFunction": function (label, item, axis) {
                                 var chart = axis.chart;
                                 if ((chart.realWidth <= 300) && (label.length > 5))
                                     return label.substr(0, 5) + '...';
                                 if ((chart.realWidth <= 500) && (label.length > 10))
                                     return label.substr(0, 10) + '...';
                                 return label;
                             },

                         },
                         "legend": showlegend(chartData),
                         "columnSpacing": 12,
                     });

                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][6] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][7] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][8] : null;
                     chart.dataProvider = chartData;
                     chart.validateData();

                     chart.addListener('clickGraphItem', function (event) {
                         //debugger
                         if (event.graph.id !== "000") {

                             if (drillDownCount < 2) {
                                 if (event.item.dataContext.SubData.length == 0) { return false;}

                                 var selectedData = event.item;
                                 calcpercentage(selectedData);

                                     scope.$apply(function () {
                                         scope.showChart = true;
                                         scope.showReport = false;
                                         scope.backBtnVisible = true;
                                     });


                                     if (event.item.dataContext.SubData != undefined) {
                                         drillDownData[drillDownCount] = event.item.dataContext.SubData;
                                         drillDownCount += 1;
                                         scope.drillDownCount = drillDownCount;
                                         subChartData = event.item.dataContext.SubData;
                                         var title = event.item.dataContext.GroupName;
                                         scope.GroupName = event.item.dataContext.GroupName;
                                         scope.breadcrumbtext = event.item.category;
                                         $rootScope.breadcrumbtextPrevious = event.item.category;
                                         scope.callbackFn({ title: scope.breadcrumbtext });
                                         scope.chart_data = subChartData;
                                         initChart(subChartData, title, 0);
                                         scope.$apply(function () {
                                             scope.backBtnVisible = true;
                                         });
                                     }

                                 }
                                 else {


                                     scope.breadcrumbtext = scope.breadcrumbtext + ">" + event.item.category + "> Report";
                                     scope.breadcrumbtextold = scope.breadcrumbtext;
                                     scope.breadcrumbback = scope.breadcrumbtext;
                                     scope.callbackFn({ title: scope.breadcrumbtext });

                                     var graphid = event.graph.id;
                                     var clickedindex = event.item.index;
                                     var selectedData = chartData[clickedindex];
                                     var selectedPeriod = "";
                                     var Period;
                                     switch (graphid) {
                                         case "1":
                                             selectedPeriod = selectedData.Period1;
                                             Period = 0;
                                             break;
                                         case "2":
                                             selectedPeriod = selectedData.Period2;
                                             Period = 0;
                                             break;
                                         case "3":
                                             selectedPeriod = selectedData.Period3;
                                             Period = 1;
                                             break;
                                         case "4":
                                             selectedPeriod = selectedData.Period4;
                                             Period = 1;
                                             break;
                                         case "5":
                                             selectedPeriod = selectedData.Period5;
                                             Period = 2;
                                             break;
                                         case "6":
                                             selectedPeriod = selectedData.Period6;
                                             Period = 2;
                                             break;

                                     }

                                     var selectedData = event.item;

                                     calcpercentage(selectedData);

                                     var requestData = {

                                         SalesPerson: (scope.isCM01) ? event.item.dataContext.GroupName : event.item.dataContext.ActiveSalesPersonCode,
                                         FilterId: $rootScope.currentfilter.Id,
                                         Period: Period,
                                         Commodity: scope.currentcommodity,
                                         OrderBy:"sales"

                                     };
                                     scope.GlobalFilterModel = requestData;

                                     if (graphid !== "000") {
                                        
                                         if (event.item.dataContext.ActiveSalesPersonCode == "CM01") {

                                             getCustomersSalesDataReport(requestData, scope.isCM01);
                                         }
                                         else {
                                             getCustomersSalesDataReport(requestData, scope.isCM01);
                                         }


                                         //var CM01text = (event.item.dataContext.ActiveSalesPersonCode == "CM01") ? "" : " > Sales Report ";
                                         //scope.breadcrumbold = $rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'>" + CM01text + "</span>";
                                         //$rootScope.secondlast = $rootScope.TitlePrevious;

                                         //if (scope.isCM01) {

                                         //    if (drillDownCount == 3 && event.item.dataContext.ActiveSalesPersonCode == "CM01") {
                                         //        scope.cm01breadcrumb = $rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'>" + CM01text + "</span>"
                                         //        scope.callbackFn({
                                         //            title: scope.cm01breadcrumb
                                         //        });
                                         //        var text = $filter('htmlToPlaintext')(scope.cm01breadcrumb);
                                         //    }
                                         //    else {
                                         //        scope.callbackFn({
                                         //            title: scope.cm01breadcrumb + "> " + event.item.dataContext.GroupName + " > Report"
                                         //        });
                                         //        var text = $filter('htmlToPlaintext')(scope.cm01breadcrumb + "> " + event.item.dataContext.GroupName + " > Report");
                                         //    }

                                         //    text = text.replace('>', '-');
                                         //    scope.csvFileName = text + ".csv";
                                         //    scope.csvFileNameOld = text + ".csv";

                                         //}
                                         //else {
                                         //    scope.callbackFn({
                                         //        title: $rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'>" + CM01text + "</span>"
                                         //    });

                                         //    var text = $filter('htmlToPlaintext')($rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'>" + CM01text + "</span>");

                                         //    text = text.replace('>', '-');
                                         //    scope.csvFileName = text + ".csv";
                                         //    scope.csvFileNameOld = text + ".csv";
                                         //}
                                     }

                                 }


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
                         if (legData) {
                             var tempLegend = [];
                             var legendNames = [];

                             function getyear(date) {
                                 var d = new Date(date);
                                 return d.getFullYear();

                             };

                            
                             tempLegend.push({
                                 title: getyear(legData[0].Period1) + " " + "Produce",
                                 color: legData[0].Color2
                             })

                             tempLegend.push({
                                 title: getyear(legData[0].Period1) + " " + "Grocery",
                                 color: legData[0].Color1
                             })

                             tempLegend.push({
                                 title: getyear(legData[0].Period3) + " " + "Produce",
                                 color: legData[0].Color4
                             })

                             tempLegend.push({
                                 title: getyear(legData[0].Period3) + " " + "Grocery",
                                 color: legData[0].Color3
                             })
                           
                            

                             if (drillDownCount > 0) {
                                 tempLegend = [];
                                 tempLegend.push({
                                     title: "",
                                     color: "white"
                                 })
                             }

                             return tempLegend;
                         }
                     }

                 };
                 var calcpercentage = function (graphDataItem, Previousmonth) {
                     var first;
                     var second;
               
                     if (Previousmonth) {
                         first = (scope.casesold) ? (graphDataItem.dataContext.cValue5) + (graphDataItem.dataContext.cValue6) : (graphDataItem.dataContext.rValue5) + (graphDataItem.dataContext.rValue6);
                         second = (scope.casesold) ? (graphDataItem.dataContext.cValue3) + (graphDataItem.dataContext.cValue4) : (graphDataItem.dataContext.rValue3) + (graphDataItem.dataContext.rValue4);
                     }
                     else {
                         first = (scope.casesold) ? (graphDataItem.dataContext.cValue1) + (graphDataItem.dataContext.cValue2) : (graphDataItem.dataContext.rValue1) + (graphDataItem.dataContext.rValue2);
                         second = (scope.casesold) ? (graphDataItem.dataContext.cValue3) + (graphDataItem.dataContext.cValue4) : (graphDataItem.dataContext.rValue3) + (graphDataItem.dataContext.rValue4);
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
                 }
                 function formatCommaSeperate(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };

                 //var calcpercentage = function (graphdata) {
                 //    var first, second;
                 //    Percentage_Data[drillDownCount] = graphdata.dataContext;

                 //    first = graphdata.dataContext.rValue1 + graphdata.dataContext.rValue2;
                 //    second = graphdata.dataContext.rValue3 + graphdata.dataContext.rValue4;
                 //    var PriorYear = first - second;
                 //    if (PriorYear !== 0) {
                 //        PriorYear = PriorYear / second;
                 //        PriorYear = PriorYear * 100;

                 //        PriorYear = (PriorYear == Infinity) ? 0 : PriorYear;

                 //    }
                 //    else {
                 //        PriorYear = 0;
                 //    }


                 //     first = graphdata.dataContext.rValue1 + graphdata.dataContext.rValue2;
                 //     second = graphdata.dataContext.rValue5 + graphdata.dataContext.rValue6;

                 //     var PriorMonth = first - second;
                 //     if (PriorMonth == 0) { PriorMonth = 100; }
                 //     else if (PriorMonth !== 0) {
                 //         PriorMonth = PriorMonth / second;
                 //         PriorMonth = PriorMonth * 100;
                 //         PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
                 //     }
                 //     else {
                 //         PriorMonth = 0;
                 //     }



                 //    scope.percentagecallbackFn({
                 //        PriorYear: Math.round(PriorYear), //
                 //        PriorMonth: Math.round(PriorMonth),
                 //        meterparams: graphdata.dataContext

                 //    });
                 //    scope.PriorMonth = Math.round(PriorMonth);
                 //    scope.PriorYear = Math.round(PriorYear);
                 //    var span = "";
                 //    //if (scope.PriorMonth > 0) {
                 //    //    return span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                 //    //}
                 //    //if (scope.PriorMonth < 0) {
                 //    //    return span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>";
                 //    //}

                 //    span = (scope.PriorMonth > 0) ? "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>" :
                 //          (scope.PriorMonth < 0) ? "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorMonth) + " %" + "</span>)</span>" : "";


                 //    if (dataService.IsGlobal_Filter_Year()) {
                 //        return span = scope.PriorYear > 0 ? "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(scope.PriorYear) + " %" + "</span>)</span>" :
                 //                      scope.PriorYear < 0 ? "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(scope.PriorYear) + " %" + "</span>)</span>" : "";
                 //    }

                 //    return span;
                 //    //if (scope.PriorMonth == 0) {
                 //    //    return span = "";
                 //    //}

                 //}

                 scope.$watch(function () {
                     return controller.$viewValue
                 }, function (newVal) {
                     scope.backBtnVisible = false;
                     data = newVal;

                     if (data == undefined || data.length == 0) {
                        // document.getElementById(scope.myId).innerText = (data == undefined) ? "" : "The chart contains no data !";
                         if (data == undefined) {

                             Metronic.blockUI({ boxed: true });
                         }
                         else {
                             Metronic.unblockUI();
                         }

                     }
                     else {
                         initChart(data, "", 0);
                     }

                     drillDownCount = 0;
                     scope.drillDownCount = drillDownCount;

                     scope.showChart = true;
                     scope.showReport = false;

                 });


                 scope.backChart = function () {
                     drillDownCount -= 1;
                     scope.drillDownCount = drillDownCount;


                     //debugger
                     initChart(drillDownCount == 0 ? data : drillDownData[drillDownCount - 1], "", drillDownCount == 0 ? 0 : 35);
                    // var obj = {};
                   //  obj["dataContext"] = (drillDownCount == 0) ? data[0] : Percentage_Data[drillDownCount - 1];

                    // calcpercentage(obj);
                     scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                     scope.breadcrumbtext = "";
                     scope.callbackFn({ title: 'initial' });

                 }

                 scope.reportback = function () {
                     if (scope.isCM01) {
                         scope.callbackFn({
                             title: scope.cm01breadcrumb
                         });
                     }

                     scope.isCM01 = false;
                     scope.drillDownCount = drillDownCount;

                     if (scope.iscustomersItemReport) {
                         scope.customersItemData = [];
                         scope.customersItemDataSafe = [];
                         scope.iscustomersItemReport = false;
                         scope.breadcrumbtext = scope.breadcrumbtextold;
                         scope.callbackFn({ title: scope.breadcrumbtext });
                     }
                     else {

                         scope.showChart = true;
                         scope.showReport = false;
                         scope.breadcrumbtext = $rootScope.breadcrumbtextPrevious;
                         scope.callbackFn({ title: scope.breadcrumbtext });
                     }

                     scope.csvFileName = scope.csvFileNameDuplicate
                  

                 }
                 scope.csvFileName = 'TotalSales';
                 scope.getDetailedReport = function (CustomerNumber, customerName) {
                     //debugger;
                     scope.breadcrumbtext = scope.breadcrumbtext + ">" + customerName;
                     scope.customer = customerName;
                     //scope.csvFileName = scope.csvFileName + '_' + scope.breadcrumbtext + '_' + customerName + '.csv';
                     //alert(scope.csvFileName);
                     scope.callbackFn({ title: scope.breadcrumbtext });
                     scope.GlobalFilterModel["CustomerNumber"] = CustomerNumber;
                     Metronic.blockUI({ boxed: true });



                     dataService.GetCasesSoldDetails(scope.GlobalFilterModel).then(function (response) {
                         if (response && response.data) {
                             scope.TotalQuantity = 0;
                             scope.TotalExtPrice = 0;

                             scope.customersItemData = response.data;
                             scope.customersItemDataSafe = response.data;
                             scope.iscustomersItemReport = true;




                             scope.bk_customersItemData = angular.copy(response.data);
                             scope.bk_customersItemDataSafe = angular.copy(response.data);

                             if (scope.iscustomersItemReport) {
                                 if (scope.currentcommodity == 'All') {
                                     scope.customersItemData = scope.bk_customersItemData;
                                     scope.customersItemDataSafe = scope.bk_customersItemDataSafe;

                                     for (var i = 0; i < scope.customersItemData.length; i++) {
                                         scope.TotalQuantity = scope.TotalQuantity + scope.customersItemData[i].Quantity;
                                         scope.TotalExtPrice = scope.TotalExtPrice + scope.customersItemData[i].ExtPrice;
                                     }

                                 } else {

                                     scope.customersItemData = [];
                                     scope.customersItemDataSafe = [];

                                     for (var i = 0; i < scope.bk_customersItemData.length; i++) {

                                         if (scope.bk_customersItemData[i].Comodity.trim() == scope.currentcommodity) {
                                             scope.customersItemData.push(scope.bk_customersItemData[i]);
                                             scope.customersItemDataSafe.push(scope.bk_customersItemData[i]);
                                             scope.TotalQuantity = scope.TotalQuantity + scope.customersItemData[i].Quantity;
                                             scope.TotalExtPrice = scope.TotalExtPrice + scope.customersItemData[i].ExtPrice;
                                         }
                                         else {

                                         }
                                     }
                                 }

                             }





                         }
                         Metronic.unblockUI();

                     }, function onError() {

                         Metronic.unblockUI();
                         NotificationService.Error("Error upon the API request");
                         NotificationService.ConsoleLog('Error on the server');
                     });

                 }
                 scope.currentcommodity = "All";
                 scope.FilterCommodity = function (commodity) {

                     scope.currentcommodity = commodity;

                     scope.GlobalFilterModel["Commodity"] = commodity;
                     getCustomersSalesDataReport(scope.GlobalFilterModel, scope.isCM01);


                 };

                 scope.getCsvHeader = function () {
                     //debugger;
                     scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                     scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                     scope.csvFileName = scope.csvFileName.replace("_", "-");

                     scope.csvFileName = scope.csvFileName.toUpperCase();

                     return ["Customer Name", "Sales Amt Prior", "Sales Qty", "Sales Amt Current", "Difference", "Difference (%)"];
                 }
                 scope.getCsvData = function () {

                     var array = [];
                     var predefinedHeader = [];
                     var current = 0;
                     var diff = 0;
                     var prior = 0;
                     var qty = 0;
                     predefinedHeader = ["Customer", "SalesAmountPrior", "SalesQty", "SalesAmountCurrent", "Difference", "Percentage"];

                     angular.forEach(scope.customersDataSafe, function (value, key) {
                         var header = {};
                         angular.forEach(value, function (value, key) {
                             var index = predefinedHeader.indexOf(key);
                             if (index > -1) {

                                 if (key == "SalesAmountCurrent") {
                                     header[index] = "$" + value;
                                 }
                                 else if (key == "Difference") {
                                     header[index] = "$" + value;
                                 }
                                 else if (key == "SalesAmountPrior") {
                                     header[index] = "$" + value;
                                 }
                                 else if (key == "SalesQty") {
                                     header[index] = $filter('numberWithCommas')(value);
                                 }
                                 else {
                                     header[index] = value;
                                 }

                             }

                             if (key == "SalesAmountCurrent") {
                                 current = current + value;
                             }
                             if (key == "Difference") {
                                 diff = diff + value;
                             }
                             if (key == "SalesAmountPrior") {
                                 prior = prior + value;
                             }
                             if (key == "SalesQty") {
                                 qty = qty + value;
                             }

                         });
                         if (header != undefined)
                             array.push(header);
                     });

                     var total = {};
                     total[0] = "Total";
                     total[1] = "$" + Math.round(prior);
                     total[2] = $filter('numberWithCommas')(Math.round(qty));
                     total[3] = "$" + Math.round(current);
                     total[4] = "$" + Math.round(diff);
                     total[5] = "";
                     //total[6] = "";


                     array.push(total);

                     return array;
                 };

                 scope.getCsvHeaderDrillDown = function () {

                     //debugger;
                     scope.csvFileName = scope.breadcrumbtext.replace('>Report', '') + "-Report.csv";
                     scope.csvFileName = "Total Sales " + scope.csvFileName.replace(/\s/g, '');

                     scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                     scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                     scope.csvFileName = scope.csvFileName.replace("_", "-");

                     scope.csvFileName = scope.csvFileName.toUpperCase();


                     return ["Commodity", "Invoice Date", "Invoice #", "Item", "Item Description", "Quantity", "Ext Price", "Sales Person", "SO #"];
                 };
                 scope.getCsvDataDrillDown = function () {
                     var array = [];
                     var predefinedHeader = [];
                     predefinedHeader = ["Comodity", "InvoiceDate", "InvoiceNumber", "Item", "ItemDesc", "Quantity", "ExtPrice", "SalesMan", "Sono"];

                     angular.forEach(scope.customersItemDataSafe, function (value, key) {
                         var header = {};
                         angular.forEach(value, function (value, key) {
                             var index = predefinedHeader.indexOf(key);
                             if (index > -1) {
                                 if (key == "ExtPrice") {
                                     header[index] = "$" + value;
                                 } else if (key == "Quantity") {
                                     header[index] = $filter('numberWithCommas')(value);

                                 }
                                 else {
                                     header[index] = value;
                                 }
                             }
                         });
                         if (header != undefined)
                             array.push(header);
                     });

                     var total = {};
                     total[0] = "Total";
                     total[1] = "";
                     total[2] = "";
                     total[3] = ""
                     total[4] = ""
                     total[5] = $filter('numberWithCommas')(scope.TotalQuantity);
                     total[6] = "$" + scope.TotalExtPrice;
                     total[7] = "";
                     total[8] = "";
                     array.push(total)
                     return array;
                 };

                 function getValueAxisTitle() {
                     return (attrs.nlDashboard == "casesold") ? "No of Cases Sold"
                          : (attrs.nlDashboard == "revenue") ? "Revenue"
                         : (attrs.nlDashboard == "revenueDashboard") ? "Revenue"
                          : (attrs.nlDashboard == "expenses") ? "Expenses" : "";
                 };

                 function getBalloonTextPrefix() {
                     return (attrs.nlDashboard == "casesold") ? ""
                           : (attrs.nlDashboard == "revenue") ? "$"
                           : (attrs.nlDashboard == "revenueDashboard") ? "$"
                           : (attrs.nlDashboard == "expenses") ? "$" : "";
                 };
                 function GetCustomerServiceDetails(filterId) {
                 
                     drillDownCount = drillDownCount + 1;
                     Metronic.blockUI({ boxed: true });

                     dataService.GetCustomerServiceDetails(filterId).then(function (response) {

                         if (response && response.data) {
                             scope.showChart = true;
                             scope.showReport = false;
                             debugger
                             // scope.cm01data = response.data.TotalCasesSold.All[0].SubData;
                             initChart(response.data.TotalCasesSold.All[0].SubData, "", 50);

                         }
                         Metronic.unblockUI();

                     }, function onError() {

                         Metronic.unblockUI();
                         NotificationService.Error("Error upon the API request");
                     });
                 };

                 function getCustomersSalesDataReport(salesPerson, iscm01) {

                
                     var text = $filter('htmlToPlaintext')("Total Sales " + scope.breadcrumbtext);

                     scope.csvFileName = text + ".csv";
                     scope.csvFileNameDuplicate = scope.csvFileName;
                 
                     scope.customersData = [];
                     scope.customersDataSafe = [];
                     Metronic.blockUI({ boxed: true });
                     dataService.GetSalesPersonCustomersDataTotal(salesPerson, iscm01).then(function (response) {
                         if (response && response.data) {

                                 scope.showChart = false;
                                 scope.showReport = true;

                             scope.customersData = response.data;
                             scope.customersDataSafe = response.data;


                             scope.TotalSalesAmtPrior = 0;
                             scope.TotalSalesQuantity = 0;
                             scope.TotalSalesQuantityPrior = 0;
                             scope.TotalSalesAmtCurrent = 0;
                             scope.TotalDifference = 0;


                             var length = scope.customersData.length;

                             for (var i = 0; i < length; i++) {

                                 scope.TotalSalesAmtPrior = scope.TotalSalesAmtPrior + scope.customersData[i].SalesAmountPrior;
                                 scope.TotalSalesQuantity = scope.TotalSalesQuantity + scope.customersData[i].SalesQty;
                                 scope.TotalSalesQuantityPrior = scope.TotalSalesQuantityPrior + scope.customersData[i].CasesSoldPrior;
                                 scope.TotalSalesAmtCurrent = scope.TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                 scope.TotalDifference = scope.TotalDifference + scope.customersData[i].Difference;


                             }

                         }
                         Metronic.unblockUI();
                     }, function onError() {
                         //stopDivLoader("caseSoldDrilldownSalesreport");
                         NotificationService.Error("Error upon the API request");
                         NotificationService.ConsoleLog('Error on the server');
                         Metronic.unblockUI();
                     });
                 }


                 scope.getCsvDataItemSoldToCustomerHeader = function () {
                     return ["Commodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "SO #", "Cases Sold", "Sales Amount", ];
                 };

                 scope.getCsvDataItemSoldToCustomer = function () {
                     var array = [];
                     var predefinedHeader = [];
                     var Quantity = 0;
                     var ExtPrice = 0;
                     predefinedHeader = ["Comodity", "InvoiceDate", "InvoiceNumber", "Item", "ItemDesc", "Sono", "Quantity", "ExtPrice"];

                     angular.forEach(scope.customersItemDataSafe, function (value, key) {
                         var header = {};
                         angular.forEach(value, function (value, key) {
                             var index = predefinedHeader.indexOf(key);
                             if (index > -1) {
                                 if (key == "ExtPrice") {
                                     header[index] = "$" + Math.round(value);
                                 }
                                 else if (key == "Quantity") {
                                     header[index] = $filter('numberWithCommas')(Math.round(value));
                                 }
                                 else {
                                     header[index] = value;
                                 }
                             }
                             if (key == "Quantity") {
                                 Quantity = Quantity + value;
                             }
                             if (key == "ExtPrice") {
                                 ExtPrice = ExtPrice + value;
                             }
                         });
                         if (header != undefined)
                             array.push(header);
                     });

                     var total = {};
                     total[0] = "Total";
                     total[1] = "";
                     total[2] = "";
                     total[3] = "";
                     total[4] = "";

                     total[5] = "";

                     total[6] = $filter('numberWithCommas')(Math.round(Quantity));
                     total[7] = $filter('currency')(Math.round(ExtPrice));



                     array.push(total);
                     return array;
                 };
             }
         };
     }]);

