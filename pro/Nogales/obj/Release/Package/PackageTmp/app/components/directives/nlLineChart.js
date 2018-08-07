MetronicApp.directive("nlLineChart", ['$location', '$state', function ($location, $state) {
    this.count = 0;
    return {
        restrict: 'E',
        replace: true,
        require: '?ngModel',
        scope: { nlDashboard: '=', },
        template: '<div class="portlet-body">' +
                       '<div id="{{::chartId}}" class="chart" style="width:100%;min-height:600px;"></div>' +
                  '</div>',
        link: function (scope, element, attrs, controller) {
            scope.chartId = "div_line_Chart" + (count++);
            var chart;
            var valueAxisTitle = getValueAxisTitle();
            var initChart = function (chartData) {
                chart = AmCharts.makeChart(scope.chartId, {
                    "panEventsEnabled": false,
                    "pan": true,//if it is false we can not  zoom chart
                    //"responsive": {
                    //    "enabled": true
                    //},

                    "type": "serial",
                    "theme": "light",
                    "marginRight": 40,
                    "marginLeft": 40,
                    "autoMarginOffset": 20,
                    "mouseWheelZoomEnabled": true,
                    "dataDateFormat": "YYYY-MM-DD",
                    "valueAxes": [{
                        "id": "v1",
                        "axisAlpha": 0,
                        "position": "left",
                        "ignoreAxisWidth": true,
                        "title": valueAxisTitle
                    }],
                    "balloon": {
                        "borderThickness": 1,
                        "shadowAlpha": 0,
                    },
                    "graphs": [{
                        "id": "g1",
                        "balloon": {
                            //"drop": true,
                            //"adjustBorderColor": true,
                            "adjustBorderColor": true,
                            //"color": "#ffffff",
                            "fillColor": "#FFFFFF",//it will only applicable if "adjustBorderColor": true,
                            "borderThickness": 2
                        },
                        "bullet": "round",
                        "bulletBorderAlpha": 1,
                        "bulletColor": "#FFFFFF",
                        "bulletSize": 5,
                        "hideBulletsCount": 50,
                        "lineThickness": 2,
                        "title": "red line",
                        "useLineColorForBulletBorder": true,
                        //"valueField": "value",
                        //"valueField": "Amount",
                        "valueField": getValueField(),
                        //"balloonText": "<span style='font-size:18px;'>[[value]]</span>"
                        "balloonText": "<span>[[value]]</span>"
                    }],
                    "chartScrollbar": {
                        "graph": "g1",
                        "oppositeAxis": false,
                        "offset": 30,
                        "scrollbarHeight": 80,
                        "backgroundAlpha": 0,
                        "selectedBackgroundAlpha": 0.1,
                        "selectedBackgroundColor": "#888888",
                        "graphFillAlpha": 0,
                        "graphLineAlpha": 0.5,
                        "selectedGraphFillAlpha": 0,
                        "selectedGraphLineAlpha": 1,
                        "autoGridCount": true,
                        "color": "#AAAAAA"
                    },
                    "chartCursor": {
                        "pan": true,
                        "valueLineEnabled": true,
                        "valueLineBalloonEnabled": true,
                        "cursorAlpha": 1,
                        "cursorColor": "#258cbb",
                        "limitToGraph": "g1",
                        "valueLineAlpha": 0.2,
                        "valueZoomable": true
                    },
                    "valueScrollbar": {
                        "oppositeAxis": false,
                        "offset": 50,
                        "scrollbarHeight": 10
                    },
                    //"categoryField": "date",
                    "categoryField": "Date",
                    "categoryAxis": {
                        "parseDates": true,
                        "dashLength": 1,
                        "minorGridEnabled": true
                    },
                    "export": {
                        "enabled": true
                    },
                    "dataProvider": chartData,
                });

                chart.addListener("rendered", zoomChart);
                zoomChart();
                function zoomChart() {
                    chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
                }

                chart.addListener("clickGraphItem", function (event) {
                    $state.go('warehouse-wms-picker-productivity-report', { date: event.item.category });
                });
            }

            scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                //if (newVal) { initChart(newVal); }

                if (newVal == undefined || newVal.length == 0) {
                    document.getElementById(scope.chartId).innerText = (newVal == undefined) ? "" : "The chart contains no data !";
                    if (newVal == undefined) {
                        //startDivLoader(scope.myId);
                      //  Metronic.blockUI({ boxed: true });
                    }
                    else {
                        //stopDivLoader(scope.myId);
                      //  Metronic.unblockUI();
                    }

                }
                else {
                    initChart(newVal, "", 0);
                }





            });

            function getValueAxisTitle() {
                return (attrs.nlDashboard == "profitability") ? "Margin in $"
                            : (attrs.nlDashboard == "warehouse") ? "Avg cases Picked/Hour"
                            : "";
            }
            function getValueField() {
                return (attrs.nlDashboard == "warehouse") ? "AvgProductivity"
                        : (attrs.nlDashboard == "profitability") ? "Amount" : "Amount";
            }
        }

    }
}])