
MetronicApp.directive("nlClusteredProfitablityTopBottom", ["$location", "$rootScope", "$filter", "$state", "dataService",
function ($location, $rootScope, $filter, $state, dataService) {
    this.count = 0;
    return {
        restrict: "E",
        require: "?ngModel",
        replace: true,
        scope: {
            nlLocation: "=", chartTitle: "=", nlDashboard: "=", hideLine: "=",
            columnValueTitle: "=", columnValueTargetTitle: "=", columnPointTitle: "=", columnPointTargetTitle: "=", callbackFn: '&', priorstatus: "=",filter:"=",
            valueAxisTile: "=", chartType: "=",
        },

        template: '<div class="portlet light">' +
          '<div class="portlet-title">' +
                  '<div class="caption caption-md">' +
                '<span class="caption-subject font-light-orange-haze bold uppercase text-mobile text-mobile">{{title}}</span>' +
                  '</div>' +
                  '</div>' +
                  '<div id="{{::chartId}}" class="chart" style="width:100%;min-height:607px;"  ></div>' +
                  '<div style="float:right;  text-align:right; margin-top:10px; top:-26px;">' +
                  '<toggle-switch ng-model="topBottomToggle" ng-init="topBottomToggle=true" on-label="{{toggleTopLabel}}" off-label="{{toggleBottomLabel}}" >' +
                  '</toggle-switch>' +
                  '</div>' +
           '</div>',

        link: function (scope, element, attrs, controller) {
            scope.filter = {
                label: 'Year over Year',
            };

            var Dollar = (attrs.isrevenue) ? "$" : "";
            scope.chartId = "chartId" + (count++);
            var charttitle = (scope.chartType == 'revenue') ? "SALES" : "TOTAL CUSTOMER BY SALES PERSON";
            scope.callbackFn({ title: "initial" });
            var currentChartData = [];
            var data;
            function formatCommaSeperate(num) {
                return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
            };
            var subChartData = null;
            var drillDownCount = 0;
            var level2Data = null;
            var drillDownChartTitle = "";
            var initChart = function (chartData) {
                if (drillDownCount == 0) {
                    scope.toggleTopLabel = "Top 10";
                    scope.toggleBottomLabel = "Bottom 10";
                }
                scope.currenttoggle = scope.toggleTopLabel;
                var sample = chartData;
                scope.chartdata = chartData;
                scope.title = (scope.toggleTopLabel==undefined)?"":scope.toggleTopLabel + "% Difference By " + attrs.types;
                chart = AmCharts.makeChart(scope.chartId, {
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
                    "dataProvider": chartData,
                    "valueAxes": [{
                        "stackType": "regular",
                        "position": "left",
                        "title": attrs.category
                    }],

                    "graphs": [

                        {
                            "fillAlphas": 0,
                            "lineAlpha": 0,
                            "title": "1200",
                            "type": "column",
                            "valueField": "ColumnMargin",
                            "showAllValueLabels": true,
                            "labelText": "",
                            "labelFunction": function label(graphDataItem) {
                                return graphDataItem.dataContext.Column1;
                            },
                            "columnWidth": .5,

                            "labelAnchor": "end",
                            "balloonText": "",
                        },
                        {
                            "fillAlphas": 0.9,
                            "lineAlpha": 0.2,
                            "title": "1200",
                            "type": "column",
                            "valueField": "Value1",
                            "fillColorsField": "Color1",
                            "columnWidth": .5,
                            "balloonText": " ",
                            "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                var value = graphDataItem.dataContext.Value1;
                                var title = graphDataItem.dataContext.Period;
                                var Percentage = "";
                                var span = "";
                                var positiveSpan = '<span class="fa fa-sort-up" style="color:forestgreen"></span>';
                                var negativeSpan = '<span class="fa fa-sort-down" style="color:red;"></span>';
                                var percentagediff = graphDataItem.dataContext.Tooltip;
                                if (percentagediff > 0) {
                                    span = positiveSpan;

                                }
                                if (percentagediff < 0) {
                                    span = negativeSpan;
                                    percentagediff = Math.abs(percentagediff);

                                }


                                return title + " <br /><b style='font-size: 130%'>"  +Dollar+formatCommaSeperate(value) +"("+span+percentagediff+")"+"</b>";

                            }
                        },
                    // right stack
                        {
                            "newStack": true,
                            "fillAlphas": 0,
                            "lineAlpha": 0,
                            "title": "0100",
                            "type": "column",
                            "valueField": "ColumnMargin",
                            "showAllValueLabels": true,
                            "labelText": "",
                            "labelFunction": function label(graphDataItem) {

                                return graphDataItem.category;
                            },
                            "columnWidth": .5,
                            //"labelRotation": 320,
                            //"labelOffset": -10,
                            "labelAnchor": "end",
                            "balloonText": "",
                        },
                        {
                            "fillAlphas": 0.9,
                            "lineAlpha": 0.2,
                            "title": "0100",
                            "type": "column",
                            "valueField": "Value2",
                            "fillColorsField": "Color2",

                            "columnWidth": .5,
                            "balloonText": " ",
                            "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                var value = graphDataItem.dataContext.Value2;
                                var title = graphDataItem.dataContext.Period2;
                                var Percentage = "Percentage Difference";
                                var span = "";
                                var positiveSpan = '<span class="fa fa-sort-up" style="color:forestgreen"></span>';
                                var negativeSpan = '<span class="fa fa-sort-down" style="color:red;"></span>';
                                var percentagediff = graphDataItem.dataContext.Tooltip;
                                if (percentagediff > 0) {
                                    span = positiveSpan;

                                }
                                if (percentagediff < 0) {
                                    span = negativeSpan;
                                    percentagediff = Math.abs(percentagediff);

                                }
                                return title + " <br /><b style='font-size: 130%'>" + Dollar + formatCommaSeperate(value) + "(" + span + percentagediff + ")" + "</b>";


                            }
                        },

                        {
                            "newStack": true,
                            "fillAlphas": 0,
                            "lineAlpha": 0,
                            "title": "0100",
                            "type": "column",
                            "valueField": "ColumnMargin",
                            "showAllValueLabels": true,
                            "labelText": " ",
                            "labelFunction": function label(graphDataItem) {
                                return graphDataItem.dataContext.Column3;
                            },
                            "columnWidth": .5,

                            "labelAnchor": "end",
                            "balloonText": "",
                        },
                        {
                            "fillAlphas": 0.9,
                            "lineAlpha": 0.2,
                            "title": "0100",
                            "type": "column",
                            "valueField": "Value3",
                            "fillColorsField": "Color3",
                            "columnWidth": .5,
                            "balloonText": " ",
                            "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                var value = graphDataItem.dataContext.Val3
                                return graphDataItem.dataContext.Column3 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>"  +formatCommaSeperate(value) + " <br /></b>";

                            }
                        }

                    ],
                    "plotAreaFillAlphas": 0.1,
                    "depth3D": 30,
                    "angle": 30,

                    "categoryField": "Category",
                    "categoryAxis": {
                        "labelOffset": 15,
                        "labelFunction": function (label, item, axis) {
                            var chart = axis.chart;
                            if ((chart.realWidth <= 300) && (label.length > 5))
                                return label.substr(0, 5) + '...';
                            if ((chart.realWidth <= 500) && (label.length > 10))
                                return label.substr(0, 10) + '...';
                            return label;
                        },
                        "gridPosition": "start",
                        "tickPosition": "start",
                        "labelOffset": 5,
                        "labelRotation": 60
                    },
                    "legend": {
                        "data": generateLegend(chartData),
                        "divId": scope.legendDiv,
                    },
                    "columnSpacing": 12,
                });
                scope.showReport = false;
                scope.breadcrumbtext = "";


            }

            scope.$watch(function () { return controller.$viewValue }, function (newVal, oldVal) {
                data = newVal;


                if (data == undefined || data.length == 0) {

                    if (data == undefined) {

                        scope.title = "Top Ten % Difference By " + attrs.types//+" "+ scope.filter;
                     //   Metronic.blockUI({ boxed: true });


                    }
                    scope.showToggleButton = false;
                }
                else {
                  //  Metronic.unblockUI();
                    scope.showToggleButton = true;

                    if (newVal.Top.length == 0) {
                        document.getElementById(scope.chartId).innerText = "The chart contains no data !";

                        scope.title = "Top Ten % Difference By " + attrs.types//+" "+ scope.filter;
                        scope.showToggleButton = false;

                    }
                    else{

                        initChart((scope.topBottomToggle) ? newVal.Top : newVal.Bottom);
                        scope.showToggleButton = true;
                    }
                    currentChartData = newVal;


                }


            });
            scope.$watch("topBottomToggle", function (newVal, oldVal) {


                //var drawchartData = (drillDownCount == 1) ? ((scope.topBottomToggle) ? currentChartData.Top : currentChartData.Bottom) :
                //                        ((drillDownCount == 2) ? ((scope.topBottomToggle) ? subChartData.Top : subChartData.Bottom) : ((scope.topBottomToggle) ? currentChartData.Top : currentChartData.Bottom));


                var drawchartData = (scope.topBottomToggle) ? currentChartData.Top : currentChartData.Bottom;

                    drawchartData ? initChart(drawchartData) : void (0);

                    if (newVal === false) {

                        scope.currenttoggle = scope.toggleBottomLabel;
                        setTimeout(function () {
                            scope.$apply(function () {
                                scope.title = scope.currenttoggle + "% Difference By " + attrs.types//+" "+ scope.filter;
                            });
                        }, 100);
                    }
                    else {
                        scope.currenttoggle = scope.toggleTopLabel;
                        setTimeout(function () {
                            scope.$apply(function () {
                                scope.title = scope.currenttoggle + "% Difference By " + attrs.types// + " " +scope.filter;
                            });
                        }, 100);
                    }

                //}
            });



            scope.displayDollar = function () {
                return scope.chartType == 'totalSalesBySalesPerson' ? "$" : "";
            }


            function generateLegend(legData) {
                if (legData) {

                    var tempLegend = [];
                    var legendNames = [];

                    if (legData.length > 0) {
                        tempLegend.push({
                            title:legData[0].Period,
                            color: legData[0].Color1
                        })
                        tempLegend.push({
                            title: legData[0].Period2,
                            color: legData[0].Color2
                        })
                    }

                    return tempLegend;
                }
            }
        }
    }
}]);