//MetronicApp.directive("nlDonutRadialGradientChart", ['$location', '$rootScope', '$filter', 'HelperService',
//     function ($location, $rootScope, $filter, HelperService) {
//         this.count = 0;
//         return {
//             restrict: 'E',
//             replace: true,
//             require: '?ngModel',
//             //scope: { nlLocation: '=', nlDashboard: '=' },
//             scope: { },
//             template: '<div id="{{::myId}}"  style="width:100%;min-height:500px;"><div id="{{legenddiv}}"></div>',
//             link: function (scope, element, attrs, controller) {
//                 scope.myId = 'chart_div_donutradialgradient' + (count);
//                 scope.legenddiv = "legenddiv" + (count);

//                 count++;
//                 var chart;
//                 var data;

//                 var initChart = function (chartData) {

//                     AmCharts.addInitHandler(function (chart) {
//                         if (chart.legend === undefined || chart.legend.truncateLabels === undefined)
//                             return;

//                         // init fields
//                         var titleField = chart.titleField;
//                         var legendTitleField = chart.titleField + "Legend";

//                         // iterate through the data and create truncated label properties
//                         for (var i = 0; i < chart.dataProvider.length; i++) {

//                             var label = chart.dataProvider[i][chart.titleField];
//                             label = label.trim();
//                             //if (label.length > chart.legend.truncateLabels)
//                             //    label = label.substr(0, chart.legend.truncateLabels - 1) + '...'
//                             chart.dataProvider[i][legendTitleField] = label;
//                         }

//                         // replace chart.titleField to show our own truncated field
//                         chart.titleField = legendTitleField;

//                         // make the balloonText use full title instead
//                         chart.balloonText = chart.balloonText.replace(/\[\[title\]\]/, "[[" + titleField + "]]");

//                     }, ["pie"]);



//                     chart = AmCharts.makeChart(scope.myId, {
//                         "responsive": {
//                             "enabled": true,
//                             "minWidth": 200,
//                             "maxWidth": 400,
//                             "maxHeight": 400,
//                             "minHeight": 200,
//                             "overrides": {
//                                 "precision": 2,
//                                 "legend": {
//                                     "enabled": function () {

//                                         return true;
//                                     }
//                                 },
//                                 "valueAxes": {
//                                     "inside": false
//                                 }
//                             }
//                         },
//                         "minRadius":80,
//                         "startEffect": "elastic",
//                         "startDuration": 2,
//                         "labelRadius": 31,

//                         "innerRadius": "30%",
//                         //"marginTop": 82,
//                         "depth3D": 10,
//                         "angle": 15,
//                         "outlineAlpha": 1,
//                         "outlineThickness": 1.5,
//                         "legend": {
//                             "markerType": "circle",
//                             "position": "right",
//                             "marginRight": 80,
//                             "autoMargins": false,
//                             "truncateLabels": 25 // custom parameter
//                         },



//                         "type": "pie",

//                         "dataProvider": chartData,
//                         "balloonText": " ",
//                         "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                             
//                             var balloonTextPrefix = "$";
//                             return graphDataItem.dataContext.Description + "<br /><b style='font-size: 130%'>" + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.TotalExpense)) + " </b>";
//                         },
//                         "maxLabelWidth": 100,
//                         "valueField": "TotalExpense",
//                         "titleField": "Description",
                    
//                         "export": {
//                             "enabled": true
//                         }


//                     });
//                 };
//                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
//                   //  initChart(newVal);
//                     data = newVal;
//                     if (data == undefined || data.length == 0) {
//                         document.getElementById(scope.myId).innerText = "This chart contains no data !";
//                     }
//                     else {
//                         initChart(data, "", 0);
//                     }
//                 })
//             }
//         }
//}])

MetronicApp.directive('nlDonutRadialGradientChart', ['$location', '$rootScope', '$filter', '$state', 'dataService', 'NotificationService','HelperService',
    function ($location, $rootScope, $filter, $state, dataService, NotificationService, HelperService) {
        return {
            restrict: "E",
            replace: true,
            require: '?ngModel',
           // scope: { },
            template: '<div id="{{::myId}}" style="width: 100%;height: 500px;"></div>' ,
            link: function (scope, element, attrs, controller) {

             scope.myId = 'chart_div_clustered_with_report1' + (count++);            
                var chart;
                var data;
                var initChart = function (chartData, title, labelRotation, settings) {
                    
                    if (chartData) {
                        var total = 0;
                        for (var i = 0; i < chartData.length; i++) {
                            total = total + chartData[i].TotalExpense;
                        }
                    }
                   


                 
                    var labelFunction = function (data, item) {

                        var percentage = (data.dataContext.TotalExpense * 100) / total;

                        //return data.dataContext.Description +" : "+percentage.toFixed(2) +"%";
                        return data.dataContext.Description + " : " + Math.round(percentage) + "%";
                    };

                    var balloontextPrefix = "$";
                    var chart = AmCharts.makeChart(scope.myId, {
                        "type": "pie",
                        "theme": "light",
                        "minRadius": 80,
                        "startEffect": "easeOutSine",
                        "startDuration": 2,
                        "labelRadius": 31,
                        "innerRadius": "30%",
                        "dataProvider": chartData,
                        "valueField": "TotalExpense",
                        "titleField": "Description",
                        "colorField": "color",
                        "angle": 26.1,
                        "depth3D": 24,
                        "labelText": " ",
                        "labelFunction": labelFunction,
                        "balloonText": " ",
                        "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                            var balloonTextPrefix = "$";
                            return graphDataItem.dataContext.Description + "<br /><b style='font-size: 130%'>" + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.TotalExpense)) + " </b>";
                        },
                        "listeners": [{
                            "event": "clickSlice",
                            "method": function(e) { e.chart.validateData();}
                        }],
                        "legend": showlegend(chartData, settings, chart),                   
                        "export": {"enabled": true}
                    });
                    if (settings) {
                        chart.labelsEnabled = false;
                        chart.autoMargins = false;
                        chart.marginTop = 0;
                        chart.marginBottom = 0;
                        chart.marginLeft = 0;
                        chart.marginRight = 0;
                        chart.pullOutRadius = 0;  
                    }
                  
                    chart.validateData();
                  
                };
                function showlegend(chartData,settings) {

                    if (chartData == undefined) {
                        return null;
                    }
                    else {
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
                                //"equalWidths": false,
                                "divId": scope.legendDiv

                            }
                        }


                    }


                };
                function generateLegend(legData) {
                    
                    if (legData) {
                        var tempLegend = [];
                        var legendNames = [];
                        for (var i = 0; i <legData.length; i++) {
                            tempLegend.push({
                                title: legData[i].Description + ": " + "$" + HelperService.formatCommaSeperate(legData[i].TotalExpense),
                                color: legData[i].color
                            })
                        }
                        return tempLegend;
                    }
                }

                scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                    debugger
                    scope.backBtnVisible = false;
                        data = newVal;


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
                });

             
            }

        };

    }]);