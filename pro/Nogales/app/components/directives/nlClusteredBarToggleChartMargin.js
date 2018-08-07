MetronicApp.directive('nlClusteredBarToggleChartMargin', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {
                 nlLocation: '=', valueAxesTitle: "=", useGraphCategoryAxisTitle: "=", topBottomToggleClusterCountLabel: "=",
                 hideChartTitle: "=", nlChart: "=",
                 callbackFn: '&', objectToInject: '=',
             },


             template: '<div class="portlet-body" ng-init="showChart=true">' +
                      '<div ng-show="showChart">' +
                      '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                      '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="backChart()" style="cursor:pointer;"></i>' +
                      //'<button style="top:0px;height:25.6px" ng-click="backChart()" class="btn btn-sm orange chart-back-btn">' +
                      //'<i class="fa fa-arrow-circle-left"></i>' +
                      //      'Back' +
                      //'</button>' +
                      '</div>' +
                     '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                        '<div  style="text-align:right;     margin-top: 20px; " ng-show="backBtnVisible">' +
                        '<toggle-switch ng-model="topBottomToggle"  ng-init="topBottomToggle=true" on-label="Top 10" off-label="Bottom 10" ng-click="loadDestinationFacilityChart()">' +
                         '<toggle-switch>' +
                         '</div>' +
                     '<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div>' +
                     '</div>' +
                     '<div ng-show="showReport">' +
                     '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +
                           //'<button style="top:0px;height:25.6px" ng-click="showReport=false;showChart=true" class="btn btn-sm orange chart-back-btn">' +
                           //    '<i class="fa fa-arrow-circle-left"></i>' +
                           //           '<span >Back</span>' +
                           //'</button>' +
                           '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true;reportback()" style="cursor:pointer;"></i>' +
                      '</div>' +
                          '<div style="margin-top: -46px;">' +
                      '<div ng-include="\'app/pages/profitability/profitability-report-table.html?v=' + window.version + '\'"></div>' +
                     '</div>' +
                      '</div>' +
                      '</div>',
             link: function (scope, element, attrs, controller) {

                 scope.toggleClusterCountLabel = (scope.topBottomToggleClusterCountLabel) ? scope.topBottomToggleClusterCountLabel : 5;
                 var chart;
                 var data;
                 var subData = null;
                 var chartTitle = "";
                 var title;
                 function formatCommaSeperate(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };
                 var drillDownCount = 0;
                 var drillDownData = [];
                 scope.legendDiv = 'legenddiv_clustered_bar_toggle' + count;
                 scope.myId = 'chart_div_clustered' + (count++);
                 var clickTimeout = 0; // this will hold setTimeout reference
                 var lastClick = 0; // last click timestamp
                 var doubleClickDuration = 200; // distance between clicks in ms - if it's less than that -
                 var drilldownCategoryAxistitle = "";
                 var initChart = function (chartData, categoryAxisTitle, labelRotation) {
                     //console.log(chartData);


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
                         //"rotate": true,
                         "titles": [
		                            {
		                                "id": "Title-1",
		                                "size": 15,
		                                "text": (scope.hideChartTitle) ? "" : chartTitle
		                            }
                         ],
                         "theme": "none",
                         "type": "serial",
                         // "dataProvider": chartData,
                         "valueAxes": [{
                             "stackType": "regular",
                             "position": "left",
                             "title": (scope.valueAxesTitle) ? scope.valueAxesTitle : "Revenue"
                         }],
                         //  "startDuration": 1,
                         "graphs": [
                          // left stack
                             {
                                 "id": "000",

                                 "fillAlphas": 0,
                                 "lineAlpha": 0,
                                 "title": "1200",
                                 "type": "column",
                                 "valueField": "Label",
                                 "showAllValueLabels": true,
                                 //"labelText": "\n[[Column1]]2",
                                 "labelText": "", //"[[Column1]]", // for label rotation
                                 // "fixedColumnWidth": 55,
                                 "columnWidth": .5,
                                 "labelRotation": 320,// for label rotation
                                 "labelOffset": -10,// for label rotation
                                 "labelAnchor": "end",// for label rotation
                                 "balloonText": "",
                             },
                             {
                                 "id": "0",
                                 "fillAlphas": 0.9,
                                 "lineAlpha": 0.2,
                                 "title": "1200",
                                 "type": "column",
                                 "valueField": "Val1",
                                 "fillColorsField": "Color1",
                                 // "fixedColumnWidth": 55,
                                 "columnWidth": .5,
                                 //"balloonText": "$[[value]]"
                                 "balloonText": " ",
                                 "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                     //
                                     var value = graphDataItem.dataContext.Val1
                                     var span = calcpercentage(graphDataItem);
                                     return graphDataItem.dataContext.Column1 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + formatCommaSeperate(value) +span+ " <br /></b>";

                                 }
                             },
                         // right stack
                             {
                                 "id": "000",
                                 "newStack": true,
                                 "fillAlphas": 0,
                                 "lineAlpha": 0,
                                 "title": "0100",
                                 "type": "column",
                                 "valueField": "Label",
                                 "showAllValueLabels": true,
                                 "labelText": "", //"[[Column2]]", // for label rotation
                                 "columnWidth": .5,
                                 "labelRotation": 320,// for label rotation
                                 "labelOffset": -10,// for label rotation
                                 "labelAnchor": "end",// for label rotation
                                 "balloonText": "",
                             },
                             {
                                 "id": "1",
                                 "fillAlphas": 0.9,
                                 "lineAlpha": 0.2,
                                 "title": "0100",
                                 "type": "column",
                                 "valueField": "Val2",
                                 "fillColorsField": "Color2",
                                 //  "fixedColumnWidth": 55
                                 "columnWidth": .5,
                                 "balloonText": " ",
                                 "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                     //
                                     var value = graphDataItem.dataContext.Val2
                                     return graphDataItem.dataContext.Column2 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + formatCommaSeperate(value) + " <br /></b>";

                                 }
                             },

                             {
                                 "id": "000",
                                 "newStack": true,
                                 "fillAlphas": 0,
                                 "lineAlpha": 0,
                                 "title": "0100",
                                 "type": "column",
                                 "valueField": "Label",
                                 "showAllValueLabels": true,
                                 "labelText": "", //"[[Column2]]", // for label rotation
                                 "columnWidth": .5,
                                 "labelRotation": 320,// for label rotation
                                 "labelOffset": -10,// for label rotation
                                 "labelAnchor": "end",// for label rotation
                                 "balloonText": "",
                             },
                             {
                                 "id": "2",
                                 "fillAlphas": 0.9,
                                 "lineAlpha": 0.2,
                                 "title": "0100",
                                 "type": "column",
                                 "valueField": "Val3",
                                 "fillColorsField": "Color3",
                                 //  "fixedColumnWidth": 55
                                 "columnWidth": .5,
                                 "balloonText": " ",
                                 "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {

                                     //
                                     var value = graphDataItem.dataContext.Val3
                                     return graphDataItem.dataContext.Column3 + " <br />" + graphDataItem.category + " <br /><b style='font-size: 130%'>" + "$" + formatCommaSeperate(value) + " <br /></b>";

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
                             "labelRotation": (drillDownCount == 0) ? 0 : 60
                         },
                         "legend": {
                             "data": generateLegend(chartData),
                             "divId": scope.legendDiv,
                         },
                         "columnSpacing": 12,
                     });

                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][4] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][5] : null;


                     chart.dataProvider = chartData;
                     chart.validateData();
                     chart.addListener('clickGraphItem', function (event) {

                         if (event.graph.id !== "000") {


                         function doClickEvent() {

                                 scope.marginData = [];
                                 scope.marginDataSafe = [];

                                 if (drillDownCount < 1) {

                                     scope.showReport = false;
                                     if (event.item.dataContext.SubData != undefined) {


                                         subChartData = (scope.topBottomToggle) ? event.item.dataContext.SubData.Top : event.item.dataContext.SubData.Bottom;
                                         drillDownData[drillDownCount] = subChartData;
                                         drillDownCount += 1;
                                         title = event.item.dataContext.Category;

                                         $rootScope.titleold = "Profit Margin" + "</br> " + "<span class='small-text'> " + title + "</span>";
                                         $rootScope.TitlePrevious = "Profit Margin" + "</br> " + "<span class='small-text'> " + title + "</span>";
                                         scope.callbackFn({ title: $rootScope.titleold });
                                         subData = event.item.dataContext.SubData;
                                         event.chart.legend = null;
                                         chartTitle = title;
                                         drilldownCategoryAxistitle = (scope.useGraphCategoryAxisTitle) ? title : "Sales Person";
                                         initChart(subChartData, drilldownCategoryAxistitle, 35);
                                         chart.validateData();
                                         scope.$apply(function () {
                                             scope.backBtnVisible = true;
                                         });
                                     }
                                     else {
                                         chart.legend = null;
                                         chart.destroy();
                                         event.chart = null;
                                         chartTitle = "";
                                         initChart(data, "", 0);
                                         subData = null;
                                         scope.$apply(function () {
                                             scope.backBtnVisible = false;
                                         });
                                     }
                                 }
                                 else {

                                     scope.$apply(function () {
                                         scope.showChart = false;
                                         scope.showReport = true;
                                     });

                                     var filterId = $rootScope.currentfilter.Id;

                                     var requestData = {
                                         filterId: filterId,
                                         Period: event.graph.id,
                                         itemCode: event.item.dataContext.Id,
                                     }

                                     var key = event.item.dataContext.Key;

                                     //debugger;

                                     GetOPEXCOGSExpenseReport(requestData, key);
                                     $rootScope.secondlast = $rootScope.TitlePrevious;
                                     scope.callbackFn({ title: $rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.category.trim() + "</span>" + "<span class='small-text'> > Report</span>" });

                                     var text = $filter('htmlToPlaintext')($rootScope.TitlePrevious + "<span class='small-text'> > " + event.item.category.trim() + "</span>" + "<span class='small-text'> > Report</span>");

                                     scope.csvFileName = text+".csv";
                                     //scope.csvFileName = scope.csvFileName.replace(/ /g, '');

                                 }


                         }


                         //doClickEvent();
                         function doDoubleClick() {
                             doClickEvent();
                         }

                         var ts = (new Date()).getTime();
                         if ((ts - lastClick) < doubleClickDuration) {
                             if (clickTimeout) {
                                 clearTimeout(clickTimeout);
                             }

                             lastClick = 0;
                             doDoubleClick(event);
                         }
                         else {
                             clickTimeout = setTimeout(function () {
                                 doClickEvent();
                             }, doubleClickDuration);
                         }

                         scope.reportback = function () {
                             if (drillDownCount == 1) {
                                 scope.callbackFn({ title: $rootScope.secondlast });
                             }
                         }
                         lastClick = ts;





                         function GetOPEXCOGSExpenseReport(requestData, key) {

                             scope.customersData = [];
                             scope.customersDataSafe = [];
                             Metronic.blockUI({ boxed: true });

                             scope.RevenueTotal = 0;
                             scope.ExtCostTotal = 0;
                             scope.MarginTotal = 0;
                             scope.QuantityTotal = 0;
                             //debugger;
                             if (key == "Customer") {
                                 dataService.GetCustomerMarginReport(requestData).then(function (response) {
                                     if (response && response.data) {
                                         //console.log(response);

                                         scope.marginData = response.data;
                                         scope.marginDataSafe = response.data;

                                         for (var i = 0; i < scope.marginData.length; i++) {

                                             scope.RevenueTotal = scope.RevenueTotal + scope.marginData[i].Revenue;
                                             scope.ExtCostTotal = scope.ExtCostTotal + scope.marginData[i].ExtCost;
                                             scope.MarginTotal = scope.MarginTotal + scope.marginData[i].Margin;
                                             scope.QuantityTotal = scope.QuantityTotal + scope.marginData[i].QuantityShipped;
                                         }



                                     }
                                     Metronic.unblockUI();
                                     scope.IsProcessing = false;

                                 }, function onError() {
                                     Metronic.unblockUI();
                                     scope.IsProcessing = false;

                                     NotificationService.Error("Error upon the API request");
                                     NotificationService.ConsoleLog('Error on the server');
                                 });

                             }

                             if (key == "Item") {
                                 dataService.GetItemMarginReport(requestData).then(function (response) {
                                     if (response && response.data) {
                                         //console.log(response);
                                         scope.marginData = response.data;
                                         scope.marginDataSafe = response.data;

                                         for (var i = 0; i < scope.marginData.length; i++) {
//debugger
                                             scope.RevenueTotal = scope.RevenueTotal + scope.marginData[i].Revenue;
                                             scope.ExtCostTotal = scope.ExtCostTotal + scope.marginData[i].ExtCost;
                                             scope.MarginTotal = scope.MarginTotal + scope.marginData[i].Margin;
                                             scope.QuantityTotal = scope.QuantityTotal + scope.marginData[i].QuantityShipped;
                                         }
                                     }
                                     Metronic.unblockUI();
                                 }, function onError() {
                                     Metronic.unblockUI();
                                     NotificationService.Error("Error upon the API request");
                                     NotificationService.ConsoleLog('Error on the server');
                                 });

                             }


                         }
                         }
                     });


                     function generateLegend(legData) {

                         function getyear(date) {
                             var datestring = date.split(" ");

                             return datestring[1];

                         };
                         var tempLegend = [];
                         var legendNames = [];

                         if (legData.length > 0) {

                             tempLegend.push({
                                 title: legData[0].Column1,
                                 color: legData[0].Color1,
                             })
                             tempLegend.push({
                                 title: legData[0].Column2,
                                 color: legData[0].Color2,
                             })
                             tempLegend.push({
                                 title: legData[0].Column3,
                                 color: legData[0].Color3,
                             })
                         }

                         return tempLegend;
                     }

                 };

                 var calcpercentage = function (graphDataItem) {

                     var first = graphDataItem.dataContext.Val1;
                     var second = graphDataItem.dataContext.Val3;

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
                     if (scope.PriorMonth >= 0) {
                         return span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'>" + Math.abs(scope.PriorMonth) + "</span>)</span>";
                     }
                     if (scope.PriorMonth < 0) {
                         return span = "<span>(<span class='fa fa-sort-down' style='color:red'>" + Math.abs(scope.PriorMonth) + "</span>)</span>";
                     }
                 }

                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     scope.backBtnVisible = false;
                     drillDownCount = 0;
                     scope.showChart = true;
                     scope.showReport = false;
                     data = newVal;
                     chartTitle = "";
                     if (data == undefined || data.length == 0) {
                         if (data == undefined) {
                             scope.chart_data = false;
                         }
                         else {
                             scope.chart_data = true;
                         }

                     }
                     else {
                         initChart(data, "", 0);
                         scope.chart_data = true;
                     }

                 });

                 scope.$watch('backBtnVisible', function (newVal, oldVal) {
                     if (!newVal && oldVal) {
                         chartTitle = "";
                         scope.marginData = [];
                         scope.marginDataSafe = [];
                         initChart(data, "", 0);
                         scope.callbackFn({ title: "initial" });
                         subData = null;
                     }
                 });

                 //scope.$watch('topBottomToggle', function () {
                 //    var c = chart;
                 //    if (subData != null) {
                 //        subChartData = (scope.topBottomToggle) ? subData.Top : subData.Bottom;
                 //        initChart(subChartData, drilldownCategoryAxistitle, 35);
                 //    }
                 //});

                 scope.getCsvHeader = function () {

                     scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                     scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                     scope.csvFileName = scope.csvFileName.replace("_", "-");

                     scope.csvFileName = scope.csvFileName.toUpperCase();
                     if (scope.type != 'Customer') {
                         return ["Invoice Number", "Item", "Credit Code", "Date", "Customer Name", "Sales Person", "Quantity Shipped", "Revenue", "Ext Cost", "Margin $", "Margin %"];
                     }
                     else {
                         return ["Invoice Number", "Date", "Credit Code", "Item", "Item Description", "Quantity Shipped", "Revenue", "Margin $", "Margin %"];
                     }
                     //debugger;


                 }
                 scope.getCsvData = function () {

                     var array = [];
                     var predefinedHeader = [];
                     var Revenue = 0;
                     var ExtCost = 0;
                     var Margin = 0;
                     var RevenueTotal = 0;
                     var MarginTotal = 0;
                 //    var Quantity = 0;

                     if (scope.type != 'Customer') {
                         predefinedHeader = ["InvNo", "Item", "CreditCode", "Date", "Company", "SalesPerson", "QuantityShipped", "Revenue", "ExtCost", "Margin", "MarginPercentage"];
                     }
                     else {
                         predefinedHeader = ["InvNo", "Date", "CreditCode", "Item", "Descrip", "QuantityShipped", "Revenue", "Margin", "MarginPercentage"];
                     }


                     angular.forEach(scope.marginData, function (value, key) {
                         var header = {};
                         angular.forEach(value, function (value, key) {
                             var index = predefinedHeader.indexOf(key);
                             if (index > -1) {

                                 if (key == "Revenue") {
                                     header[index] = $filter('currency')(value);
                                 }
                                 else if (key == "ExtCost") {
                                     header[index] = $filter('currency')(value);
                                 }
                                 else if (key == "Margin") {
                                     header[index] = $filter('currency')(value);
                                 }
                                 else if (key == "Date") {
                                     header[index] = $filter('date')(value);

                                 }
                                 else {
                                     header[index] = value;
                                 }

                             }

                             if (key == "Revenue") {
                                 Revenue = Revenue + value;
                             }
                             if (key == "ExtCost") {
                                 ExtCost = ExtCost + value;
                             }
                             if (key == "Margin") {
                                 Margin = Margin + value;
                             }
                             if (key == "RevenueTotal") {
                                 RevenueTotal = RevenueTotal + value;
                             }
                             if (key == "MarginTotal") {
                                 MarginTotal = MarginTotal + value;
                             }


                         });
                         if (header != undefined)
                             array.push(header);
                     });

                     if (scope.type == "Customer") {
                         var total = {};
                         total[0] = "Total";
                         total[1] = "";
                         total[2] = "";
                         total[3] = "";
                         total[4] = "";
                         total[5] = "";
                         total[6] = $filter('currency')(RevenueTotal);

                         total[7] = $filter('currency')(MarginTotal);
                         total[8] = "";


                         array.push(total);
                     }
                     else {
                         var total = {};
                         total[0] = "Total";
                         total[1] = "";
                         total[2] = "";
                         total[3] = "";
                         total[4] = "";
                         total[5] = "";
                         total[6] = $filter('numberWithCommas')(scope.QuantityTotal);

                         total[7] = $filter('currency')(Revenue);
                         total[8] = $filter('currency')(ExtCost);
                         total[9] = $filter('currency')(Margin);

                         total[10] = "";

                         array.push(total);
                     }


                     return array;
                 };

                 scope.backChart = function () {


                     var cData = (drillDownCount == 1) ? data : drillDownData[drillDownCount - 2];

                     drillDownCount -= 1;
                     initChart(cData, "", 35);
                     //console.log(cData);
                     scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                 }
                 scope.backChartLink = function (item) {
                     //
                     drillDownCount = item;
                     var cData = (drillDownCount > 2) ? drillDownData[0]
                        : (drillDownCount == 1) ? drillDownData[0]
                        : (drillDownCount == 0) ? data : data;
                     if (drillDownCount > 1) {
                         scope.showChart = false;
                         scope.showReport = true;
                     }
                     else {
                         scope.callbackFn({ title: $rootScope.secondlast });

                         scope.showChart = true;
                         scope.showReport = false;

                     }
                     if (drillDownCount == 0) {

                         initChart(cData, "", 0);
                         scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                         scope.callbackFn({ title: "OPEX/COGS Journal Expenses" });
                     }
                     drillDownCount = drillDownCount - 1;
                     scope.marginData = [];
                     scope.marginDataSafe = [];

                 }
                 scope.reportback = function () {
                     scope.callbackFn({ title: $rootScope.secondlast });
                 };
             }
         };
     }]);