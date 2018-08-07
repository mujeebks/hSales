

MetronicApp.directive('displayTvChart', ['$filter', 'dataService', 'NotificationService', 'HelperService', 'DisplayChartTimeout', '$interval','$state',
    function ($filter, dataService, NotificationService, HelperService, DisplayChartTimeout, $interval, $state) {
        this.count = 0;
        return {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: { nlDashboard: '=', filterType: '=', callbackFnapi: '&', timerevent: '=' },
            template: '<div class="portlet-body">' +
                            '<div >' +
                          
                                 '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>'+
                         '</div>'+
                        '</div>',

            link: function (scope, element, attrs, controller) {
                var promise;

                scope.legendDiv = "Total_Chart_Legend" + count;
                scope.myId = 'Total_Chart' + (count++);
                var chart;
                var data;
                var valueAxixTitle = (attrs.nlDashboard == "casesold") ? "Cases sold qty"
                                    : (attrs.nlDashboard == "sales") ? "sales qty"
                                    : (attrs.nlDashboard == "profitability") ? "sales qty"
                                    : (attrs.nlDashboard == "expenses") ? "Commodity Expenses" : "";
                var balloonTextPrefix = (attrs.nlDashboard == "casesold") ? "" : "$";
                scope.nlDashboard = attrs.nlDashboard;

                scope.dashboard == attrs.nlDashboard;
                scope.casesold = (attrs.nlDashboard == "casesold") ? true : false;
                var formatCommaSeperate = function (num) { return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") };
                var initChart = function (chartData, title, labelRotation) {
                    chart = AmCharts.makeChart(scope.myId, {
                        "responsive": {
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
                        "processTimeout": 0,
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
                                   var xlabel = graphDataItem.dataContext.Label1;
                                   var label = "";
                                   return label;
                               },
                               "labelRotation": 0
                           },
                           //val1 & val2
                           {
                               "id": "1",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue1" : "rValue1",
                               "fillColorsField": "Color1",
                               "balloonText": " ",
                               "balloonFunction": function (graphDataItem, graph) {
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
                               "valueField": (scope.casesold == true) ? "cValue2" : "rValue2",
                               "fillColorsField": "Color2",
                               "balloonText": " ",
                               "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                   //debugger;
                                   var total = 0;

                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue1 : graphDataItem.dataContext.rValue1;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue2 : graphDataItem.dataContext.rValue2;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   var span = calcpercentage(graphDataItem);

                                   return graphDataItem.dataContext.Label2 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1)) + "<br/>" + salespersonToolTip;

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
                                   var xlabel = graphDataItem.dataContext.Label3;
                                   var label = "";
                                   return label;
                               },
                               "labelRotation": 0

                           },
                             //val3 & val4
                           {

                               "id": "3",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "newStack": false,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue3" : "rValue3",
                               "fillColorsField": "Color3",
                               "balloonText": " ",
                               "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                   var total = 0;
                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue3 : graphDataItem.dataContext.rValue3;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue4 : graphDataItem.dataContext.rValue4;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";
                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);

                                   return graphDataItem.dataContext.Label3 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1))
                                   "<br/>" + salespersonToolTip;
                               }
                           },
                           {

                               "id": "4",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "newStack": false,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue4" : "rValue4",
                               "fillColorsField": "Color4",
                               "balloonText": " ",
                               "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                   //debugger;
                                   var total = 0;

                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue3 : graphDataItem.dataContext.rValue3;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue4 : graphDataItem.dataContext.rValue4;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);

                                   return graphDataItem.dataContext.Label4 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1))
                                   "<br/>" + salespersonToolTip;
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
                                   var xlabel = graphDataItem.dataContext.Label5;
                                   var label =  "";
                                   return label;
                               },
                               "labelRotation": 0

                           },

                           //val 5 & val 6
                           {

                               "id": "5",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "newStack": false,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue5" : "rValue5",
                               "fillColorsField": "Color5",
                               "balloonText": " ",
                               "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                   //debugger;
                                   var total = 0;

                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue5 : graphDataItem.dataContext.rValue5;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue6 : graphDataItem.dataContext.rValue6;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   var span = calcpercentage(graphDataItem, true);

                                   return graphDataItem.dataContext.Label5 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1))
                                   "<br/>" + salespersonToolTip;

                               }

                           },
                           {

                               "id": "6",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "newStack": false,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue6" : "rValue6",
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


                                   return graphDataItem.dataContext.Label6 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1))
                                   "<br/>" + salespersonToolTip;
                               }
                           }
                        ],
                        "plotAreaFillAlphas": 0.1,
                        "depth3D": 30,
                        "angle": 30,
                        "categoryField": "GroupName",
                        "categoryAxis": {

                            "ignoreAxisWidth": false,
                            "title": title,
                            "gridPosition": "start",
                            "tickPosition": "start",
                            "labelRotation": labelRotation,
                            "autoWrap": true
                        },
                    
                        "columnSpacing": 12,
                    });

                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][6] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][7] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][8] : null;
                    chart.dataProvider = chartData;
                    chart.validateData();
                  
                    chart.addListener('clickGraphItem', function (event) {
                       
                    });
                };
               
                initChart(controller.$viewValue, "", 0);

                //controller.$render = function () {
                //    //alert(JSON.stringify(controller.$modelValue));
                //    debugger;
                //    initChart(controller.$modelValue[0].SubData, "", 0);
                //};
              
                
                setTimeout(function () { scope.callbackFnapi({ event: true }); }, 8000);

                scope.redirecttodashboard = function (timer) {
                  
                    $state.go('dashboard');
                };
                function callAtInterval() {
                   
                    scope.callbackFnapi({ event: true });
                }
                $interval.cancel(promise);
                promise = $interval(callAtInterval, DisplayChartTimeout);
              
                scope.$watch(function () { return controller.$viewValue }, function (newVal) {
       
                    if (newVal) {
                        initChart(newVal[0].SubData, "", 0);
                    }
                 


                });
                scope.$watch('timerevent', function (newVal,oldval) {

                    
                    if (newVal) {
                        $interval.cancel(promise);
                        scope.redirecttodashboard();
                    }


                });
                
              
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
                };
            }
        };
    }]);

//1266