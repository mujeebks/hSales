
MetronicApp.directive('nlUsaHeatMap', ['$location', '$filter', '$state',
     function ($location, $filter, $state) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: { },

             template: '<div class="portlet-body" style="text-align: center;">' +
                 '<span style="font-weight: bold;">Total {{titlechart}} : {{dollar}}{{TotalSales}}</span>' +
                       '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                       //'<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                       '</div>',

             link: function (scope, element, attrs, controller) {
                 scope.titlechart = attrs.titlechart;
                 scope.dollar = (scope.titlechart == "Cases Sold") ? "" :
                                (scope.titlechart == "Sales") ? "$" :
                                (scope.titlechart == "Revenue") ? "$" : "";
                 scope.legendDiv = "legenddiv_nlUsaHeatMap" + count;
                 scope.myId = 'chart_div_nlUsaHeatMap' + (count++);

                 var drillDownCount = 0;
                 var chart;
                 var data;
                 var showLegend = true;

                 var initChart = function (chartData, title, labelRotation) {
                     element;
                     chart = AmCharts.makeChart(scope.myId, {
                         "responsive": {
                             "enabled": true,

                         },
                         "processTimeout": 1,
                         "type": "map",
                         "theme": "light",
                         "colorSteps": 5,

                         "dataProvider": {
                             "map": "usaLow",
                             areas: chartData
                         },

                         areasSettings: {

                             "autoZoom": true,
                             "selectedColor": "#CC0000",
                             "balloonText": "[[title]] </br>[[customData]]",


                         },

                         valueLegend: {
                             right: 10,
                             minValue: "little",
                             maxValue: "a lot!"
                         }

                     });
                     //chart.dataProvider = chartData;
                     //chart.validateData();



                     //chart.addListener('clickGraphItem', function (event) {

                     //   debugger
                     //});


                 };

                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     scope.TotalSales = 0;
                     data = newVal;
                    
                     if (data != undefined) {
                         for (var i = 0; i < data.length; i++) {
                             scope.TotalSales = scope.TotalSales + data[i].value;
                         }

                         scope.TotalSales = formatCommaSeperate(Math.round(scope.TotalSales));



                         initChart(data, "", 0);
                     }
                 });
                 function formatCommaSeperate(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };
                 //scope.$watch('backBtnVisible', function (newVal, oldVal) {
                 //    if (!newVal && oldVal) {
                 //        showLegend = true;
                 //        drillDownCount = 0;
                 //        initChart(data, "", 0);
                 //    }
                 //});


                // initChart("", "", 0);

             }
         };
     }]);