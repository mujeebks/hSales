MetronicApp.directive('pieChart', ['$location', '$rootScope', '$filter', '$state', 'dataService', 'NotificationService','HelperService',
    function ($location, $rootScope, $filter, $state, dataService, NotificationService, HelperService) {
        return {
            restrict: "E",
            replace: true,
            require: '?ngModel',
            scope: { nlLocation: '=', nlDashboard: '=', filterType: '=', customMonth: '=', callbackFn: '&', objectToInject: '=', currentfilter: '=' },
            template:
                //' <div id="chartdiv" style="width: 100%;height: 500px;"></div>',
                 '<div class="portlet-body" style="text-align: center;">' +
                 '<span style="font-weight: bold;">{{text}}</span>' +
                       '<div id="chartdiv" style="width: 100%;height: 500px;"></div>' +
                      // '<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                       '</div>',
            link: function (scope, element, attrs, controller) {

                function formatCommaSeperate(num) {
                    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                };
                scope.legendDiv = "legenddiv_clustered_with_report" + count;
                scope.myId = 'chart_div_clustered_with_report1' + (count++);
                scope.chartLoaded = false;
                scope.breadcrumbtext = "";
                var chart;
                var data;
                var valueAxixTitle = getValueAxisTitle();
                var balloonTextPrefix = (attrs.charttype == "CasesSold") ? "" :
                                        (attrs.charttype == "expense") ? "$"
                                         : "$";
                scope.balloonTextPrefix = balloonTextPrefix;

                var drillDownCount = 0;
                var drillDownData = [];

                scope.sampledata = 0;
                scope.customersData = [];
                scope.customersDataSafe = [];
                scope.myTableFunctions = {};
                scope.groupProperty = '';

                var initChart = function (chartData, title, labelRotation, settings) {
                  
                    var itemData = [];
                    var totalField = 0;
                    for (var i = 0; i <= chartData.length - 1; i++) {
                        var item = chartData[i];
                        item.value = (attrs.charttype == "CasesSold") ? item.cValue1 + item.cValue2 :
                                      (attrs.charttype == "expense") ?  item.cValue1 + item.cValue2 :
                                                                        item.rValue1 + item.rValue2
                        itemData.push(item);
                        totalField = totalField + item.value;
                    }

                    var text = (attrs.charttype == "CasesSold") ? "Total Cases Sold" + " :  " + $filter('numberWithCommasRounded')(totalField) :
                                (attrs.charttype == "Revenue") ? "Total Revenue" + " :  $" + $filter('numberWithCommasRounded')(totalField) :
                                (attrs.charttype == "expense") ? "Total Expense" + " :  $" + $filter('numberWithCommasRounded')(totalField):"";

                    scope.text = text;
                 
                    totalField = totalField.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

                    var labelFunction = function (data,item) {
                        var label = (attrs.charttype == "CasesSold") ? data.dataContext.GroupName + " \n" + $filter('numberWithCommasRounded')((data.dataContext.cValue1 + data.dataContext.cValue2)) + " - " + Math.round(data.percents) + "%" :
                                    (attrs.charttype == "expense") ? data.dataContext.GroupName + " \n" + Math.round(data.percents) + "%"
                                                                     : data.dataContext.GroupName + " \n" + "$" + $filter('numberWithCommasRounded')((data.dataContext.rValue1 + data.dataContext.rValue2)) + " - " + Math.round(data.percents) + "%";
                        return label;
                     
                    };

                    var chart = AmCharts.makeChart("chartdiv", {
                        "type": "pie",
                        "theme": "light",              
                        "pullOutRadius": "5%",
                        "labelRadius": 27,
                        "startEffect": "easeOutSine",                  
                        "dataProvider": itemData,
                        "valueField": "value",
                        "titleField": "GroupName",
                        "colorField": "Color1",     
                        "labelText": " ",
                        "labelFunction": labelFunction,
                        "legend": showlegend(chartData, settings, chart),
                        "balloon": {
                            "fixedPosition": true
                        },
                        "listeners": [{
                            "event": "clickSlice",
                            "method": function (e) { e.chart.validateData(); }
                        }],
                        "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                            var total = 0;

                            var val1 = (attrs.charttype == "CasesSold") ? graphDataItem.dataContext.cValue1 :
                                         (attrs.charttype == "expense") ? graphDataItem.dataContext.cValue1 :
                                                                          graphDataItem.dataContext.rValue1;


                            var val2 = (attrs.charttype == "CasesSold") ? graphDataItem.dataContext.cValue2 :
                                       (attrs.charttype == "expense") ?   graphDataItem.dataContext.cValue2 :
                                                                          graphDataItem.dataContext.rValue2

                            var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.GroupName + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                            total = val1 + val2;
                            total = Math.round(((total) * 100) / 100);

                         
                            var tot;
                            if (attrs.charttype == "expense") {
                                 tot = val1 + val2;
                                 tot = Math.round(((tot) * 100) / totalField);
                                 total = tot;

                                 //var balloonTextPrefix = "$";
                                 return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>" + balloonTextPrefix + formatCommaSeperate(Math.round(graphDataItem.dataContext.cValue1)) + " </b>";
                            }
                           
                            return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + formatCommaSeperate(Math.round(val2)) + " <br />Grocery " + balloonTextPrefix + formatCommaSeperate(Math.round(val1)) + " <br />" + salespersonToolTip;
                          
                        },
                        "export": {
                            "enabled": true
                        }
                    });

                    if (attrs.charttype == "expense") {

                        chart.radius = "20%";
                        
                    }
                    else {
                      //  chart["legend"] = false;
                    }
                 
                    chart.validateData();
                   

                    function showlegend(chartData, settings) {
                       
                        if (chartData == undefined) {
                            return null;
                        }
                        else {
                            if (attrs.charttype != "expense" || settings) {
                                return null;
                            }
                            if (!settings) {
                                return {

                                    "markerType": "circle",
                                    "data": generateLegend(chartData),
                                    "position": "right",
                                    "marginRight": 80,
                                    "autoMargins": false,
                                    "truncateLabels": 25 // custom parameter
                                }
                            }
                            else {
                                return {

                                    "markerType": "circle",
                                    "position": "bottom",
                                    "data": generateLegend(chartData),
                                    "autoMargins": false,
                                    "maxColumns":1,
                                    "equalWidths": false,
                                    "divId": scope.legendDiv

                                }
                            }
                        }
                    };
                    function generateLegend(legData) {

                        if (legData) {
                            var tempLegend = [];
                            var legendNames = [];
                            for (var i = 0; i < legData.length; i++) {
                                tempLegend.push({
                                    title: legData[i].GroupName + ": " + "$" + HelperService.formatCommaSeperate(legData[i].cValue1),
                                    color: legData[i].Color1
                                })
                            }
                            return tempLegend;
                        }
                    }

                };
                scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                    debugger
                    scope.backBtnVisible = false;
                    data = newVal;

                    if (data == undefined || data.length == 0) {

                        if (data == undefined) {

                            Metronic.blockUI({ boxed: true });
                        }
                        else {
                            Metronic.unblockUI();
                        }

                    }
                    else {
                        initchartdata(data);

                        $(window).resize(function () {

                            initchartdata(data);

                        });

                        function initchartdata(data) {
                            var windowwidth = $(this).width();
                            if (windowwidth > 980) {
                                initChart(data, "", 0, false);
                            }
                            else {
                                initChart(data, "", 0, true);
                            }
                        }
                    }

                });

           

            

                function getValueAxisTitle() {
                    return (attrs.nlDashboard == "casesold") ? "No of Cases Sold"
                         : (attrs.nlDashboard == "revenue") ? "Revenue"
                        : (attrs.nlDashboard == "revenueDashboard") ? "Revenue"
                         : (attrs.nlDashboard == "expenses") ? "Expenses" : "";
                }

                function getBalloonTextPrefix() {
                    return (attrs.nlDashboard == "casesold") ? ""
                          : (attrs.nlDashboard == "revenue") ? "$"
                          : (attrs.nlDashboard == "revenueDashboard") ? "$"
                          : (attrs.nlDashboard == "expenses") ? "$" : "";
                }


               
            }



            //}

        };

    }]);