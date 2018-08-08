
MetronicApp.directive("nlClusteredReportChart", ["$location", "$rootScope", "$filter", "$state", "dataService",
    function ($location, $rootScope, $filter, $state, dataService) {
        this.count = 0;
        return {
            restrict: "E",
            require: "?ngModel",
            replace: true,
            scope: {
                nlLocation: "=",
                chartTitle: "=",
                nlDashboard: "=",
                hideLine: "=",
                columnValueTitle: "=",
                columnValueTargetTitle: "=",
                columnPointTitle: "=",
                columnPointTargetTitle: "=",
                callbackFn: '&',
                priorstatus: "=",
                valueAxisTile: "=",
                chartType: "=",
            },
            template: '<div class="portlet light">' +
                '<div class="portlet-title">' +
                '<div class="caption caption-md">' +
                '<span class="caption-subject font-light-orange-haze bold uppercase text-mobile">{{title}}</span>' +
                '</div>' +
                '</div>' +
                '<div id="{{::chartId}}" class="chart" style="width:100%;min-height:607px;"  ></div>' +
                '<div style="text-align:right; position:relative; top:0px;">' +
                '<toggle-switch ng-model="topBottomToggle" ng-init="topBottomToggle=true" on-label="{{toggleTopLabel}}" off-label="{{toggleBottomLabel}}" >' +
                '</toggle-switch>' +
                '</div>' +
                '</div>',

            link: function (scope, element, attrs, controller) {
                scope.chartId = "chartId" + (count++);
                var charttitle = (scope.chartType == 'revenue') ? "SALES" : "TOTAL CUSTOMER BY SALES PERSON";
                function formatCommaSeperate(num) {
                    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                };
                scope.callbackFn({
                    title: "initial"
                });

                var data;
                var subChartData = null;
                var drillDownCount = 0;
                var level2Data = null;
                var drillDownChartTitle = "";
                var dollar = (attrs.category == "Cases sold") ? "" : "$";
                var initChart = function (chartData) {
                    if (drillDownCount == 0) {
                        scope.toggleTopLabel = "Top 10";
                        scope.toggleBottomLabel = "Bottom 10";
                    }
                    scope.currenttoggle = scope.toggleTopLabel;
                    var sample = chartData;


                    if (scope.priorstatus == "") {

                        scope.title = scope.currenttoggle + " Increase in " + attrs.category + "";
                    } else {
                        if (scope.priorstatus == "Prior Month") {
                            scope.title = scope.currenttoggle + " Increase in " + attrs.category + " Month Over Month"
                        } else {
                            scope.title = scope.currenttoggle + " Increase in " + attrs.category + " Year Over Year"
                        }
                    }

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
                            // left stack
                            {
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "1200",
                                "type": "column",
                                "valueField": "Label",
                                "showAllValueLabels": true,
                                //"labelText": "\n[[Column1]]2",
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    // var date1 = new Date(graphDataItem.dataContext.Column1);
                                    return graphDataItem.dataContext.Column1;
                                },
                                // "fixedColumnWidth": 55,
                                "columnWidth": .5,
                                //"labelRotation": 320,// for label rotation
                                "labelRotation": 270,
                                "labelOffset": -10, // for label rotation
                                "labelAnchor": "end", // for label rotation
                                "balloonText": " ",
                            },
                            {
                                "fillAlphas": 0.9,
                                "lineAlpha": 0.2,
                                "title": "1200",
                                "type": "column",
                                "valueField": "Value1",
                                "fillColorsField": "Color1",
                                // "fixedColumnWidth": 55,
                                "columnWidth": .5,
                                //"balloonText": "$[[value]]"
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                    var value = graphDataItem.dataContext.Value1;
                                    var title = "Current " + attrs.category;
                                    var Percentage = "Percentage Difference";
                                    var span = "";
                                    var positiveSpan = '<span class="fa fa-sort-up" style="color:forestgreen"></span>';
                                    var negativeSpan = '<span class="fa fa-sort-down" style="color:red;" ng-if="shipment.PercentageDifference<0"></span>';
                                    var percentagediff = graphDataItem.dataContext.Tooltip;
                                    if (percentagediff > 0) {
                                        span = positiveSpan;

                                    }
                                    if (percentagediff < 0) {
                                        span = negativeSpan;
                                       // percentagediff = Math.abs(percentagediff);

                                    }


                                    return title + " <br /><b style='font-size: 130%'>" + dollar +formatCommaSeperate(value) + " <br /></b>" + Percentage + " <br /><b style='font-size: 130%'>" + span + " " + percentagediff + "% <br /></b>";

                                }
                            },
                            // right stack
                            {
                                "newStack": true,
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "0100",
                                "type": "column",
                                "valueField": "Label",
                                "showAllValueLabels": true,
                                "labelText": " ",
                                "labelFunction": function label(graphDataItem) {

                                    return graphDataItem.category;
                                },
                                "columnWidth": .5,
                                // "labelRotation": 320,
                                "labelRotation": 270,
                                "labelOffset": -10,
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
                                //  "fixedColumnWidth": 55
                                "columnWidth": .5,
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                    var value = graphDataItem.dataContext.Value2;
                                    var title = "Previous " + attrs.category;
                                    var Percentage = "Percentage Difference";
                                    var span = "";
                                    var positiveSpan = '<span class="fa fa-sort-up" style="color:forestgreen"></span>';
                                    var negativeSpan = '<span class="fa fa-sort-down" style="color:red;" ng-if="shipment.PercentageDifference<0"></span>';
                                    var percentagediff = graphDataItem.dataContext.Tooltip;
                                    if (percentagediff > 0) {
                                        span = positiveSpan;

                                    }
                                    if (percentagediff < 0) {
                                        span = negativeSpan;
                                      //  percentagediff = Math.abs(percentagediff);

                                    }


                                    return title + " <br /><b style='font-size: 130%'>" + dollar +formatCommaSeperate(value) + "</b>";

                                }
                            },

                            {
                                "newStack": true,
                                "fillAlphas": 0,
                                "lineAlpha": 0,
                                "title": "0100",
                                "type": "column",
                                "valueField": "Label",
                                "showAllValueLabels": true,
                                "labelText": " ",
                                "labelFunction": function label(graphDataItem) {
                                    //var date1 = new Date(graphDataItem.dataContext.Column3);
                                    return graphDataItem.dataContext.Column3;
                                }, //"[[Column2]]", // for label rotation
                                "columnWidth": .5,
                                //"labelRotation": 320,// for label rotation
                                "labelRotation": 270,
                                "labelOffset": -10, // for label rotation
                                "labelAnchor": "end", // for label rotation
                                "balloonText": "",
                            },
                            {
                                "fillAlphas": 0.9,
                                "lineAlpha": 0.2,
                                "title": "0100",
                                "type": "column",
                                "valueField": "Value3",
                                "fillColorsField": "Color3",
                                //  "fixedColumnWidth": 55
                                "columnWidth": .5,
                                "balloonText": " ",
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {


                                    var value = graphDataItem.dataContext.Val3
                                    return graphDataItem.dataContext.Column3 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + value + " <br /></b>";

                                }
                            }

                        ],
                        "plotAreaFillAlphas": 0.1,
                        "depth3D": 30,
                        "angle": 30,

                        "categoryField": "Label",
                        "categoryAxis": {
                            //"title": title,
                            //"labelOffset": 15,
                            "gridPosition": "start",
                            "tickPosition": "start",
                            //"labelOffset": 34,
                            "labelRotation": 60,
                            "ignoreAxisWidth": false,
                            "autoWrap": true,
                                      "labelFunction": function(label, item, axis) {
                                                var chart = axis.chart;
                                                if ( (chart.realWidth <= 300 ) && ( label.length > 5 ) )
                                                    return label.substr(0, 5) + '...';
                                                if ( (chart.realWidth <= 500 ) && ( label.length > 10 ) )
                                                    return label.substr(0, 10) + '...';
                                                return label;
                                      },

                            //"labelOffset": 15,
                            //"gridPosition": "start",
                            //"tickPosition": "start",
                            //"labelOffset": 34,
                            //"labelRotation": 50,
                            //"autoWrap": true
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

                scope.$watch(function () {
                    return controller.$viewValue
                }, function (newVal, oldVal) {
                    data = newVal;


                    if (data == undefined || data.length == 0) {

                        if (data == undefined) {

                            Metronic.blockUI({
                                boxed: true
                            });
                            scope.showToggleButton = false;
                        } else {

                            Metronic.unblockUI();
                            scope.showToggleButton = true;
                        }

                    } else {

                        initChart((scope.topBottomToggle) ? newVal.Top : newVal.Bottom);
                        Metronic.unblockUI();
                        scope.showToggleButton = true;
                        scope.topBottomToggle = true
                    }


                });
                scope.$watch("topBottomToggle", function (newVal, oldVal) {

                    if (data) {

                        if (newVal === false) {
                            setTimeout(function () {
                                scope.$apply(function () {
                                    scope.currenttoggle = "Bottom 10";
                                    if (scope.priorstatus == "") {
                                        scope.title = scope.currenttoggle + " Decrease in " + attrs.category;
                                    } else {
                                        if (scope.priorstatus == "Prior Month") {
                                            scope.title = scope.currenttoggle + " Decrease in " + attrs.category + " Month Over Month"
                                        } else {
                                            scope.title = scope.currenttoggle + " Decrease in " + attrs.category + " Year Over Year"
                                        }
                                    }
                                });
                            }, 100);


                            // scope.$apply();

                        } else {
                            setTimeout(function () {
                                scope.$apply(function () {
                                    scope.currenttoggle = "Top 10";
                                    if (scope.priorstatus == "") {
                                        scope.title = scope.currenttoggle + " Increase in " + attrs.category;
                                    } else {
                                        if (scope.priorstatus == "Prior Month") {
                                            scope.title = scope.currenttoggle + " Increase in " + attrs.category + " Month Over Month"
                                        } else {
                                            scope.title = scope.currenttoggle + " Increase in " + attrs.category + " Year Over Year"
                                        }
                                    }
                                });
                            }, 100);


                            // scope.$apply();

                        }


                        var drawchartData = (drillDownCount == 1) ? ((scope.topBottomToggle) ? level2Data.Top : level2Data.Bottom) :
                            ((drillDownCount == 2) ? ((scope.topBottomToggle) ? subChartData.Top : subChartData.Bottom) : ((scope.topBottomToggle) ? data.Top : data.Bottom));

                        drawchartData ? initChart(drawchartData) : void (0);



                    }
                });

                scope.$watch("backBtnVisible", function (newVal, oldVal) {
                    if (!newVal && oldVal) {
                        scope.topBottomToggle = true;
                        var drawchartData = (drillDownCount == 2) ? (level2Data) : (data);
                        initChart((scope.topBottomToggle) ? drawchartData.Top : drawchartData.Bottom);
                        scope.backBtnVisible = (drillDownCount <= 1) ? false : true;
                        drillDownCount--;
                        scope.callbackFn({
                            title: ""
                        });
                        scope.showToggleButton = (drillDownCount == 2) ? false : true;
                        scope.chartTitle = getChartTitle(drillDownCount, drillDownChartTitle);
                        if (drillDownCount == 0) {
                            scope.toggleTopLabel = "Top 5";
                            scope.toggleBottomLabel = "Bottom 5";
                        }
                        //else if (drillDownCount == 1) {
                        //    scope.toggleTopLabel = "Top 10";
                        //    scope.toggleBottomLabel = "Bottom 10";
                        //}


                    }
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
                                title: "Current " + attrs.category,
                                color: legData[0].Color1
                            })
                            tempLegend.push({
                                title: "Previous " + attrs.category,
                                color: legData[0].Color2
                            })
                        }

                        return tempLegend;
                    }
                }
            }
        }
    }
]);