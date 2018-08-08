
MetronicApp.directive('nlTotalChartFinance', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {
                 valueAxesTitle: "=", topBottomToggleClusterCountLabel: "=", callbackFn: '&'
             },

             template: '<div class="portlet-body" ng-init="showChart=true">' +
                      '<div ng-show="showChart">' +
                      '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                      '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="backChart()" style="cursor:pointer;"></i>' +

                      '</div>' +
                     '<div id="{{::myId}}" class="chart" style="width:96%;min-height:500px;"></div>' +
                     '</br>' +
                      '</br>' +
                       '</br>' +
                        '<div style="text-align:right; position:relative; top:0px;" ng-show="backBtnVisible">' +
                        '<toggle-switch ng-show="showtoggle" ng-model="topBottomToggle" ng-init="topBottomToggle=true;" on-label="Top 10" off-label="Bottom 10" ng-click="loadDestinationFacilityChart()">' +
                         '<toggle-switch>' +
                         '</div>' +
                     '<div id="{{::legendDiv}}" ng-show="showlegend" class="clusteredBarChartLegenddiv"></div>' +
                     '</div>' +
                     '<div ng-show="showReport">' +
                     '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +

                           '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true;reportback()" style="cursor:pointer;"></i>' +
                      '</div>' +
                          '<div style="margin-top: -46px;">' +
                      '<div ng-include="\'app/pages/finance/financial-report-table.html\'"></div>' +
                     '</div>' +
                      '</div>' +
                      '</div>',
             link: function (scope, element, attrs, controller) {
                 scope.showtoggle = false;

                 var chart;
                 var data;
                 var subData = null;
                 var chartTitle = "";
                 var title;
                 var drillDownCount = 0;
                 var drillDownData = [];
                 scope.legendDiv = 'legenddiv_clustered_bar_toggle' + count;
                 scope.myId = 'chart_div_clustered' + (count++);
                 scope.showlegend = true;
                 scope.ReportData = {};
                 scope.groupProperty = '';
                 function formatNumber(num) {
                     return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                 };
                 var initChart = function (chartData, title, labelRotation) {
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
                         //"dataProvider": chartData,
                         "valueAxes": [{
                             "stackType": "regular",
                             "position": "left",
                             "title": (scope.valueAxesTitle) ? scope.valueAxesTitle : "Transportation"
                         }],
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
                                "balloonText": "",
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column1;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
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
                               
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {


                                    return SetBalloonFunction(true, graphDataItem, 1, true);
                                 

                                }
                               
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
                                "balloonText": "",
                                "showAllValueLabels": true,
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    return (drillDownCount == 0) ? graphDataItem.dataContext.Column2 : "";
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
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
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                 
                                    return SetBalloonFunction(true, graphDataItem,2,false);

                                }
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
                                "balloonText": "",
                                "showAllValueLabels": true,
                                "labelText": " ", //"[[Column1]]", // for label rotation
                                "labelFunction": function label(graphDataItem) {
                                    return (drillDownCount == 0) ? graphDataItem.dataContext.Column3 : "";
                                },
                                "labelOffset": -15,
                                "labelRotation": -30,
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
                                "balloonFunction": function BalloonTextCurrent(graphDataItem, graph) {
                                  
                                    return SetBalloonFunction(false, graphDataItem, 3, true);

                                }
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
                             "boldLabels": (drillDownCount == 0) ? true : false,
                             "labelRotation": labelRotation,
                             "labelFunction": function (label, item, axis) {
                                 var chart = axis.chart;
                                 if ((chart.realWidth <= 300) && (label.length > 5))
                                     return label.substr(0, 5) + '...';
                                 if ((chart.realWidth <= 500) && (label.length > 10))
                                     return label.substr(0, 10) + '...';
                                 return label;
                             },

                             "labelOffset": (drillDownCount == 0) ? 60 : 0,
                             "autoWrap": true
                         },
                         "legend": {
                             "position": "bottom",
                             "data": generateLegend(chartData),
                             "autoMargins": false,
                             "equalWidths": false,
                             "divId": scope.legendDiv
                         },
                         "columnSpacing": 12,
                     });

                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][4] : null;
                     (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][5] : null;


                     chart.dataProvider = chartData;
                     chart.validateData();
                     chart.addListener('clickGraphItem', function (event) {
                         var catTitle;
                         var graphid = event.graph.id;
                         if (graphid !== "00") {

                             catTitle = event.item.dataContext.Category;

                             if (event.item.dataContext.SubData != undefined) {

                                 if (drillDownCount == 0) {
                                 
                                     scope.TitleFirst = "TOTAL COLLECTION" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     scope.callbackFn({ title: scope.TitleFirst });

                                     subChartData = event.item.dataContext.SubData;
                                     drillDownData[drillDownCount] = subChartData;

                                     scope.showtoggle = false;
                                 }
                                 if (drillDownCount == 1) {
                                     subChartData = (scope.topBottomToggle) ? event.item.dataContext.SubData.Top : event.item.dataContext.SubData.Bottom;
                                     drillDownData[drillDownCount] = subChartData;
                                     scope.Pterm = catTitle;
                                     scope.TitleSecond = scope.TitleFirst + " > " + "<span class='small-text'>" + catTitle + "</span>"
                                     scope.callbackFn({ title: scope.TitleSecond });


                                     scope.showtoggle = true;
                                 }
                                 if (drillDownCount == 2) {

                                 }
                                 drillDownCount += 1;

                                 scope.showlegend = (drillDownCount > 0) ? false : true;

                                 subData = event.item.dataContext.SubData;
                                 event.chart.legend = null;

                                 initChart(subChartData, "", 35);
                                 chart.validateData();
                                 scope.$apply(function () {
                                     scope.backBtnVisible = true;
                                 });

                             }
                             else {

                                 var graphid = event.graph.id;
                                 if (graphid !== "00") {
                             
                                     var Period = (graphid == "g1") ? 0 :
                                                  (graphid == "g2") ? 1 :
                                                  (graphid == "g3") ? 2 : "";
                                  
                                     scope.TitleReport = scope.TitleSecond + "<span class='small-text'> > " + event.item.category + "> Report </span>";
                                     scope.callbackFn({ title: scope.TitleReport });



                                     
                                     var text = $filter('htmlToPlaintext')(scope.TitleReport);
                                     text = text.replace(">", "-").replace("_", "-").toUpperCase() + ".csv"

                                     scope.csvFileName = text;
                                     scope.$apply(function () { scope.showChart = false;scope.showReport = true;});

                                     var filterId = $rootScope.currentfilter.Id;
                              
                                     var requestData = {
                                         PTerms: scope.Pterm,
                                         Collector: event.item.category,
                                         FilterId: filterId,
                                         Period: Period
                                     };
                                     GetCollectorsReport(requestData);
                                 }
                                 else {
                                     scope.customersData = [];
                                     scope.customersDataSafe = [];
                                 }
                             }
                         };

                         scope.reportback = function () {
                             scope.showlegend = false;
                             scope.callbackFn({ title: scope.TitleSecond });

                         }
                        
                     });
                     function GetCollectorsReport(requestData) {
                         Metronic.blockUI({ boxed: true });
                         scope.customersData = [];
                         scope.customersDataSafe = [];

                         dataService.GetCollectorsReport(requestData).then(function (response) {
                             if (response && response.data) {
                                 Metronic.unblockUI();
                                 scope.customersData = response.data;
                                 scope.customersDataSafe = angular.copy(response.data);

                                 var TotalInvoiceAmount = 0;
                                 var TotalAmountCollected = 0;
                                 var TotalBalanceAmount = 0;



                                 var PaymentOnTimePercentage=0;
                                 var CollectionOnTimePercentage = 0;

                                 for (var i = 0; i < scope.customersDataSafe.length; i++) {
                                     TotalInvoiceAmount = TotalInvoiceAmount + scope.customersDataSafe[i].InvoiceAmount;
                                     TotalAmountCollected = TotalAmountCollected + scope.customersDataSafe[i].AmountCollected;
                                     TotalBalanceAmount = TotalBalanceAmount + scope.customersDataSafe[i].BalanceAmount;
                                
                                 
                                     scope.customersData[i].InvoiceAmount = $filter('currency')(scope.customersData[i].InvoiceAmount,"$","0");
                                     scope.customersData[i].AmountCollected = $filter('currency')(scope.customersData[i].AmountCollected, "$", "0");
                                     scope.customersData[i].BalanceAmount = $filter('currency')(scope.customersData[i].BalanceAmount, "$", "0");

                                     scope.customersDataSafe[i].InvoiceAmount = $filter('currency')(scope.customersDataSafe[i].InvoiceAmount, "$", "0");
                                     scope.customersDataSafe[i].AmountCollected = $filter('currency')(scope.customersDataSafe[i].AmountCollected, "$", "0");
                                     scope.customersDataSafe[i].BalanceAmount = $filter('currency')(scope.customersDataSafe[i].BalanceAmount, "$", "0");



                                     scope.customersData[i].DueDate =(scope.customersData[i].DueDate) ?  $filter('date')(scope.customersData[i].DueDate) : "";
                                     scope.customersData[i].DatePaid =(scope.customersData[i].DatePaid) ?  $filter('date')(scope.customersData[i].DatePaid):"";


                                     scope.customersDataSafe[i].DueDate =(scope.customersDataSafe[i].DueDate) ?  $filter('date')(scope.customersDataSafe[i].DueDate):"";
                                     scope.customersDataSafe[i].DatePaid = (scope.customersDataSafe[i].DatePaid) ? $filter('date')(scope.customersDataSafe[i].DatePaid) : "";

                                     PaymentOnTimePercentage = PaymentOnTimePercentage + scope.customersDataSafe[i].PaymentOnTimePercentage;
                                     CollectionOnTimePercentage = CollectionOnTimePercentage + scope.customersDataSafe[i].CollectionOnTimePercentage;

                                    



                                 }
                                 debugger;
                                 PaymentOnTimePercentage = PaymentOnTimePercentage / scope.customersDataSafe.length;
                                 //PaymentOnTimePercentage = PaymentOnTimePercentage * 100;
                                 CollectionOnTimePercentage = CollectionOnTimePercentage / scope.customersDataSafe.length;
                              //   CollectionOnTimePercentage = CollectionOnTimePercentage * 100;
                                 scope.TotalInvoiceAmount = TotalInvoiceAmount;
                                 scope.TotalAmountCollected = TotalAmountCollected;
                                 scope.TotalBalanceAmount = TotalBalanceAmount;

                                 scope.PaymentOnTimePercentage =Math.round(PaymentOnTimePercentage)+"%";
                                 scope.CollectionOnTimePercentage = Math.round(CollectionOnTimePercentage) + "%";

                                 var PecentageAmountCollected = TotalAmountCollected / TotalInvoiceAmount;
                                 PecentageAmountCollected = PecentageAmountCollected * 100;
                                 scope.PecentageAmountCollected =Math.round(PecentageAmountCollected)+"%";
                             }
                             Metronic.stopPageLoading();
                         }, function onError() {
                             Metronic.unblockUI();
                            NotificationService.Error("Error upon the API request");
                         });
                     };
                     function SetBalloonFunction(isprevious, graphDataItem, i, showpercentagespan) {

                         var CollectedSpan = "";
                         var InvoiceAmountSpan = "";
                         var PaymentCollectionPercentagespan = "";
                         var PaymentOnTimePercentageSpan = "";

                         if (showpercentagespan) {
                             var CollectedSpan = calcpercentage(graphDataItem, isprevious, "Val");
                             var InvoiceAmountSpan = calcpercentage(graphDataItem, isprevious, "InvoiceAmount");
                             var PaymentCollectionPercentagespan = calcpercentage(graphDataItem, isprevious, "PaymentCollectionPercentage");
                             var PaymentOnTimePercentageSpan = calcpercentage(graphDataItem, isprevious, "PaymentOnTimePercentage");
                         }
                        
                         return graphDataItem.dataContext["Column" + [i.toString()]] + "<br /> <b style='font-size: 120%'>" + graphDataItem.dataContext.Category + "</b>" + "<br/><b style=''> Collected : " + "$" + $filter('numberWithCommasRounded')(graphDataItem.dataContext["Val" + [i.toString()]]) + CollectedSpan + " </b><br />Invoice Amount : " +"$"+ $filter('numberWithCommasRounded')(graphDataItem.dataContext["InvoiceAmount" + [i.toString()]]) + InvoiceAmountSpan + "<br />Payment Collection : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext["PaymentCollectionPercentage" + [i.toString()]]) + "%" + PaymentCollectionPercentagespan + "<br />Payment OnTime : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext["PaymentOnTimePercentage" + [i.toString()]]) + "%" + PaymentOnTimePercentageSpan;
                     }
                     function generateLegend(legData) {
                         function getyear(date) { var d = new Date(date); return d.getFullYear();};
                         var tempLegend = [];
                         for (var i = 1; i <= 3; i++) {
                            tempLegend.push({ title: (legData[0]) ? legData[0]["Column" + [i.toString()]] : "", color: (legData[0]) ? legData[0]["Color" + [i.toString()]] : "" });
                         }
                         return tempLegend;
                     }

                 };
                 var calcpercentage = function (graphDataItem, Previousmonth, type) {
                     var first;
                     var second;

                     if (Previousmonth) {
                         first = graphDataItem.dataContext[type + "1"];
                         second = graphDataItem.dataContext[type + "3"];
                     }
                     else {
                         first = graphDataItem.dataContext[type + "1"];
                         second = graphDataItem.dataContext[type + "2"];
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
                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     debugger
                     scope.backBtnVisible = false;
                     drillDownCount = 0;
                     scope.showChart = true;
                     scope.showReport = false;
                     data = newVal;
                     chartTitle = "";
                     if (data == undefined || data.length == 0) {

                     }
                     else {
                         initChart(data, "", 0);
                     }

                 });

                 scope.$watch('backBtnVisible', function (newVal, oldVal) {
                     if (!newVal && oldVal) {
                         chartTitle = "";
                         initChart(data, "", 0);
                         subData = null;
                     }
                 });

                 scope.$watch('topBottomToggle', function () {
                     if (subData != null) {
                         subChartData = (scope.topBottomToggle) ? subData.Top : subData.Bottom;
                         initChart(subChartData, "", 35);
                     }
                 });
                 scope.getCsv = function () {
                     var array = [];
                     var predefinedHeader = [];
                   
                     predefinedHeader = ['InvoiceNumber', 'InvoiceDate', 'CustomerCode', 'CustomerName', 'SalesManCode', 'SalesManName', 'PNet', 'PTerms', 'InvoiceAmount', 'AmountCollected', 'BalanceAmount', 'DueDate', 'DatePaid','DaysOverDue', 'PaymentOnTimePercentage', 'CollectionOnTimePercentage', 'CollectorName'];

                     angular.forEach(scope.customersDataSafe, function (value, key) {
                         var header = {};
                         angular.forEach(value, function (value, key) {
                             var index = predefinedHeader.indexOf(key);
                             if (index > -1) {
                                 header[index] = (key == "DueDate" || key == "DatePaid" || key == "InvoiceDate") ? $filter('date')(value) : (key == "PaymentOnTimePercentage" || key == "CollectionOnTimePercentage") ? value+"%" : value;
                             }
                       
                         });
                         if (header != undefined)
                             array.push(header);
                     });

                     var total = {};
                     total[0] = "Total";
                     total[1] = total[2] = total[3] = total[4] = total[5] = total[6] = total[7]= total[11]= total[12]=total[13] = "";
                  
                    total[8] = "$" + scope.TotalInvoiceAmount;
                    total[9] = "$" + scope.TotalAmountCollected;
                    total[10] = "$" + scope.TotalBalanceAmount;
                     //total[14] = scope.PaymentOnTimePercentage;
                     //total[15] = scope.CollectionOnTimePercentage;
                    total[14] = "";
                    total[15] = "";
                     array.push(total);



                     var total1 = {};
                    
                     total1[0] = total1[1] = total1[2] = total1[3] = total1[4] = total1[5] = total1[6] = total1[7] = total1[8] = total1[11] =total1[10]=total1[12]= total1[13] = "";
                     total1[14] = scope.PaymentOnTimePercentage;
                     total1[15] = scope.CollectionOnTimePercentage;
                     total1[9] = scope.PecentageAmountCollected;
                     array.push(total1);
                     return array;
                 };

                 scope.getCsvHeader = function () {
                     return ['Invoice #', 'Invoice Date', 'Customer Code', 'Customer Name', 'Salesman Code', 'Salesman Name', 'Net', 'Terms', 'Invoice Amount', 'Amount Collected', 'Balance','Due Date','Date Paid','Days Overdue', 'PaymentOnTime(%)', 'Collection OnTime(%)', 'Collector Name'];
                 };

                 scope.backChart = function () {
                     
                     scope.showlegend = true;
                     var cData = (drillDownCount == 1) ? data : drillDownData[drillDownCount - 2];
                     drillDownCount -= 1;
                     initChart(cData, "", 35);
                     scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                     if (drillDownCount == 0) {
                         scope.showlegend = true;
                         scope.callbackFn({ title: "initial" });
                     }
                     if (drillDownCount == 1) {
                         scope.callbackFn({ title: scope.TitleFirst });
                         scope.showtoggle = false;
                         scope.showlegend = false;
                     }

                     if (drillDownCount == 2) {
                        // scope.showtoggle = false;
                         scope.callbackFn({ title: scope.TitleSecond });
                         scope.showlegend = false;
                     }
                 }

             }
         };
     }]);






