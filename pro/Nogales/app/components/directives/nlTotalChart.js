

MetronicApp.directive('nlTotalChart', ['$rootScope', '$filter', 'dataService', 'NotificationService','HelperService',
    function ($rootScope, $filter, dataService, NotificationService,HelperService) {
        this.count = 0;
        return {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: { nlDashboard: '=', filterType: '=', callbackFn: '&' },
            templateUrl: 'app/components/directives/nlTotalChart.html',

            link: function (scope, element, attrs, controller) {

                scope.legendDiv = "Total_Chart_Legend" + count;
                scope.myId = 'Total_Chart' + (count++);
                var chart;
                var data;
                var valueAxixTitle = (attrs.nlDashboard == "casesold") ? "Cases sold qty"
                                    : (attrs.nlDashboard == "sales") ? "sales qty"
                                    : (attrs.nlDashboard == "profitability") ? "sales qty"
                                    : (attrs.nlDashboard == "expenses") ? "Commodity Expenses" : "";
                var balloonTextPrefix = (attrs.nlDashboard == "casesold") ? "" : "$";
                var drillDownCount = 0;
                scope.drillDownCount = drillDownCount;
                var Percentage_Data = [];
                var drillDownData = [];
                scope.nlDashboard = attrs.nlDashboard;
                scope.customersItemData = [];
                scope.customersItemDataSafe = [];
                scope.iscustomersItemReport = false;
                scope.isCM01 = false;
                scope.sampledata = 0;
                scope.customersData = [];
                scope.customersDataSafe = [];
                scope.myTableFunctions = {};
                scope.groupProperty = '';

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
                                   var xlabel = graphDataItem.dataContext.Label1;
                                   var label = (drillDownCount == 0) ? "\n" + xlabel : "";
                                   return label;
                               },
                               "labelRotation": (drillDownCount == 0) ? 0 : -35
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
                                   // xlabel=xlabel.replace("Jan Jan", "Jan")


                                   //return "\n" + label;
                                   var label = (drillDownCount == 0) ? "\n" + xlabel : "";
                                   //return "\n" + label;

                                   return label;
                               },
                               "labelRotation": (drillDownCount == 0) ? 0 : -35

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
                                   //debugger;
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
                                   // xlabel = xlabel.replace("Jan Jan", "Jan")
                                   var label = (drillDownCount == 0) ? "\n" + xlabel : "";
                                   //return "\n" + label;
                                   return label;
                               },
                               "labelRotation": (drillDownCount == 0) ? 0 : -35

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
                                   //debugger;
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

                            "ignoreAxisWidth": (drillDownCount == 0) ? true : false,
                            "title": title,
                            "gridPosition": "start",
                            "tickPosition": "start",
                            "labelRotation": labelRotation,
                            "autoWrap": true
                        },
                        "legend": {
                            "position": "bottom",
                            "data":HelperService.generateLegend(chartData,drillDownCount),
                            "autoMargins": false,
                            "equalWidths": false,
                            "divId": scope.legendDiv
                        },
                        "columnSpacing": 12,
                    });

                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][6] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][7] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][8] : null;
                    chart.dataProvider = chartData;
                    chart.validateData();

                    chart.addListener('clickGraphItem', function (event) {
                        var selectedperiod = "";
                        var id = event.graph.id;
                        if (id !== "000") {
                            var selectedPeriod = "";
                            if (drillDownCount < 2) {
                                if (event.item.dataContext.SubData.length == 0) { return false; }
                                scope.$apply(function () { scope.showChart = true; scope.showReport = false; scope.backBtnVisible = true;});
                                var titledata;
                                if (drillDownCount == 0) {
                                    var module = (attrs.nlDashboard == "casesold") ? "CASES SOLD (BY CATEGORY)<br/>" :
                                                 (attrs.nlDashboard == "sales") ? "SALES (BY CATEGORY)<br/>" :
                                                 (attrs.nlDashboard == "profitability") ? "PROFITABILITY (BY CATEGORY)<br/>" :
                                                 (attrs.nlDashboard == "expenses") ? "COMMODITY EXPENSES (BY CATEGORY)<br/>" : "Total <br/>"

                                    $rootScope.titleold = module + "<span class='small-text'></span>";
                                    $rootScope.TitlePrevious = module + "<span class='small-text'></span>";
                                    scope.callbackFn({ title: $rootScope.TitlePrevious });
                                }

                                if (drillDownCount == 1) {

                                    $rootScope.TitlePrevious = $rootScope.titleold.replace('CATEGORY', 'SALES PERSON') + "<span class='small-text'> " + event.item.category + " </span>"
                                    scope.callbackFn({ title: $rootScope.TitlePrevious });
                                }

                                if (event.item.dataContext.SubData != undefined) {
                                    drillDownData[drillDownCount] = event.item.dataContext.SubData;
                                    drillDownCount += 1;
                                    subChartData = event.item.dataContext.SubData;
                                    scope.eventCategory = (event.item.dataContext.GroupName).split("(");
                                    scope.chart_data = subChartData;

                                    initChart(subChartData, "", 50);
                                    scope.$apply(function () {
                                        scope.backBtnVisible = true;
                                    });
                                }
                            }
                            else {
                                var text = "";
                                if (id != "000") {

                                    var graphid = event.graph.id;

                                    var Period = (graphid == "1" || graphid == "2") ? 0 :
                                                 (graphid == "3" || graphid == "4") ? 1 :
                                                 (graphid == "5" || graphid == "6") ? 2 : "";


                                    var requestData = {};
                                    if (attrs.nlDashboard == "profitability") {
                                        requestData = {
                                            FilterId: $rootScope.currentfilter.Id,
                                            ItemCode: "",
                                            Commodity: scope.currentcommodity,
                                            CustomerCode: "",
                                            Period: Period,
                                            Salesman: (scope.isCM01) ? event.item.dataContext.GroupName : event.item.dataContext.ActiveSalesPersonCode,
                                        };

                                    } else {
                                        requestData = {
                                            SalesPerson: (scope.isCM01) ? event.item.dataContext.GroupName : event.item.dataContext.ActiveSalesPersonCode,
                                            FilterId: $rootScope.currentfilter.Id,
                                            Period: Period,
                                            Commodity: scope.currentcommodity,
                                            OrderBy: "casessold"
                                        };
                                    }
                                  
                                    scope.GlobalFilterModel = requestData;
                                    if (event.item.dataContext.ActiveSalesPersonCode == "CM01") {
                                        scope.isCM01 = true;
                                        GetCustomerServiceDetails($rootScope.currentfilter.Id, attrs.nlDashboard);
                                    }
                                    else {

                                        (attrs.nlDashboard == "profitability") ? GetProfitByCustomerDetailAndCommodity(requestData, scope.isCM01) :
                                        (attrs.nlDashboard == "expenses") ? GetExpenseReportBySalesman(requestData, scope.isCM01): getCustomersSalesDataReport(requestData, scope.isCM01);
                                    }
                                    var CM01text = (event.item.dataContext.ActiveSalesPersonCode == "CM01") ? "" : " > Report ";
                                    scope.breadcrumbold = $rootScope.TitlePrevious + "<span class='small-text'> > " + (event.item.dataContext.GroupName).toLowerCase(); + " </span>" + "<span class='small-text'>" + CM01text + "</span>";
                                    $rootScope.secondlast = $rootScope.TitlePrevious;
                                   
                                    if (scope.isCM01) {
                                        if (drillDownCount == 3 && event.item.dataContext.ActiveSalesPersonCode == "CM01") {
                                            scope.cm01breadcrumb = $rootScope.TitlePrevious + "<span class='small-text'> > " + (event.item.dataContext.GroupName).toLowerCase(); + " </span>" + "<span class='small-text'>" + CM01text + "</span>"
                                            scope.callbackFn({title: scope.cm01breadcrumb});
                                             text = $filter('htmlToPlaintext')(scope.cm01breadcrumb);
                                        }
                                        else {
                                            scope.cm011stdrilldowntext = scope.cm01breadcrumb + "> " + event.item.dataContext.GroupName + " > Report";
                                            scope.callbackFn({title: scope.cm011stdrilldowntext});
                                             text = $filter('htmlToPlaintext')(scope.cm01breadcrumb + "> " + (event.item.dataContext.GroupName).toLowerCase() + " > Report");
                                        }
                                        text = text.replace('>', '-');
                                        scope.csvFileName = text + ".csv";
                                        scope.csvFileNameOld = text + ".csv";
                                    }
                                    else {
                                        scope.callbackFn({
                                            title: $rootScope.TitlePrevious + "<span class='small-text'> > " + (event.item.dataContext.GroupName).toLowerCase() + " </span>" + "<span class='small-text'>" + CM01text + "</span>"
                                        });

                                        var text = $filter('htmlToPlaintext')($rootScope.TitlePrevious + "<span class='small-text'> > " + (event.item.dataContext.GroupName).toLowerCase() + " </span>" + "<span class='small-text'>" + CM01text + "</span>");

                                        text = text.replace('>', '-');
                                        scope.csvFileName = text + ".csv";
                                        scope.csvFileNameOld = text + ".csv";
                                    }
                                }
                                else {
                                    scope.customersData = [];
                                    scope.customersDataSafe = [];
                                }
                            }
                        }
                    });
                };

                scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                    scope.backBtnVisible = false;
                    drillDownCount = 0;
                    scope.drillDownCount = drillDownCount;
                    scope.showChart = true;
                    scope.showReport = false;
                    data = newVal;
                    if (data == undefined || data.length == 0) {
                        document.getElementById(scope.myId).innerText = (data == undefined) ? "" : "The chart contains no data !";
                    }
                    else {
                        if (data != undefined) {
                            data.splice(3, 1);
                        }
                        initChart(data, "", 0);
                    }
                });
                scope.backChart = function () {
                    drillDownCount -= 1;
                    scope.drillDownCount = drillDownCount;
                    initChart(drillDownCount == 0 ? data : drillDownData[drillDownCount - 1], "", drillDownCount == 0 ? 0 : 35);
                    var obj = (drillDownCount == 0) ? data[0] : Percentage_Data[drillDownCount - 1];
                    scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                    if (drillDownCount == 0) {
                        scope.callbackFn({ title: "initial" });
                        scope.sampledata = 0;
                    }
                    if (drillDownCount == 1) {
                        scope.isCM01 = false;
                        if (attrs.nlDashboard == "revenueDashboard")
                            scope.callbackFn({ title: $rootScope.revenuetitleold });
                        else
                            scope.callbackFn({ title: $rootScope.titleold });
                        scope.sampledata = 0;
                    }
                    if (drillDownCount == 2) {
                        scope.callbackFn({ title: $rootScope.secondlast
                    });
                    }
                };

             
                function GetCustomerServiceDetails(filterId, state) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    drillDownCount = drillDownCount + 1;
                    Metronic.blockUI({ boxed: true });

                    dataService.GetCustomerServiceDetails(filterId, state).then(function (response) {
                       
                        if (response && response.data) {
                            scope.showChart = true;
                            scope.showReport = false;

                            //(state == "profitability" || state =="expenses") ? initChart(response.data.TotalRevenue.All[0].SubData, "", 50):
                            //                                                     initChart(response.data.TotalCasesSold.All[0].SubData, "", 50);
                            if (state == "profitability" || state =="expenses") {
                                initChart(response.data.TotalRevenue.All[0].SubData, "", 50);
                            }
                            else {
                                initChart(response.data.TotalCasesSold.All[0].SubData, "", 50);
                            }

                        }
                        Metronic.unblockUI();

                    }, function onError() {

                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };


                function getCustomersSalesDataReport(salesPerson, iscm01) {

                    scope.customersData = [];
                    scope.customersDataSafe = [];

                    Metronic.blockUI({ boxed: true });

                    dataService.GetSalesPersonCustomersDataTotal(salesPerson, iscm01).then(function (response) {
                        scope.showChart = false;
                        scope.showReport = true;
                        if (response && response.data) {
                            scope.customersData = response.data;
                            scope.customersDataSafe = response.data;
                            scope.bk_customersData = angular.copy(response.data);
                            debugger;
                            if (!scope.casesold) {

                                var TotalSalesAmountPrior = 0;
                                var TotalCasesSoldCurrent = 0;
                                var TotalSalesAmtCurrent = 0;
                                var SalesAmountPrior = 0;
                                var length = scope.customersData.length;
                                for (var i = 0; i < length; i++) {
                                    TotalCasesSoldCurrent = TotalCasesSoldCurrent + scope.customersData[i].CasesSoldCurrent;
                                    SalesAmountPrior = SalesAmountPrior + scope.customersData[i].SalesAmountPrior;
                                    TotalSalesAmtCurrent = TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                }
                                debugger;
                                scope.TotalCasesSoldQty = Math.round(TotalCasesSoldCurrent);
                                scope.TotalCasesSoldPrior = Math.round(SalesAmountPrior);
                                scope.TotalSalesAmtCurrent = Math.round(TotalSalesAmtCurrent);
                            }
                            else {
                                scope.TotalCasesSoldPrior = 0;
                                scope.TotalCasesSoldQty = 0;
                                scope.TotalSalesAmtCurrent = 0;
                                scope.DifferenceCasesSold = 0;
                                scope.TotalSalesAmtPrior = 0;
                                scope.TotalSalesQuantity = 0;
                                scope.TotalDifference = 0;
                                var length = scope.customersData.length;
                                for (var i = 0; i < length; i++) {
                                    scope.TotalCasesSoldQty = scope.TotalCasesSoldQty + scope.customersData[i].CasesSoldCurrent;
                                    scope.TotalCasesSoldPrior = scope.TotalCasesSoldPrior + scope.customersData[i].CasesSoldPrior;
                                    scope.TotalSalesAmtCurrent = scope.TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                }
                                scope.TotalCasesSoldQty = Math.round(scope.TotalCasesSoldQty);
                                scope.TotalCasesSoldPrior = Math.round(scope.TotalCasesSoldPrior);
                                scope.TotalSalesAmtCurrent = Math.round(scope.TotalSalesAmtCurrent);
                            }

                        }
                        Metronic.unblockUI();

                    }, function onError() {

                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                }

                function GetProfitByCustomerDetailAndCommodity(salesPerson, iscm01) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({ boxed: true });
                    salesPerson["IsCM01"] = iscm01;
                    dataService.GetProfitByCustomerDetailAndCommodity(salesPerson, iscm01).then(function (response) {
                        scope.showChart = false;
                        scope.showReport = true;
                        if (response && response.data) {
                            scope.customersData = response.data;
                            scope.customersDataSafe = response.data;
                            scope.bk_customersData = angular.copy(response.data);
                            scope.Cost = 0;
                            scope.ExtPrice = 0;
                            scope.Profit = 0;
                            scope.QtyShip = 0;
                            var length = scope.customersData.length;
                            for (var i = 0; i < length; i++) {
                                scope.Cost = scope.Cost + scope.customersData[i].Cost;
                                scope.ExtPrice = scope.ExtPrice + scope.customersData[i].ExtPrice;
                                scope.Profit = scope.Profit + scope.customersData[i].Profit;
                                scope.QtyShip = scope.QtyShip + scope.customersData[i].QtyShip;
                            }
                            scope.Cost = Math.round(scope.Cost);
                            scope.ExtPrice = Math.round(scope.ExtPrice);
                            scope.Profit = Math.round(scope.Profit);
                            scope.QtyShip = Math.round(scope.QtyShip);
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };
                function GetExpenseReportBySalesman(salesPerson, iscm01) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({ boxed: true });
                    salesPerson["IsCM01"] = iscm01;
                    dataService.GetExpenseReportBySalesman(salesPerson, iscm01).then(function (response) {
                        debugger
                        scope.showChart = false;
                        scope.showReport = true;
                        if (response && response.data) {
                            scope.customersData = response.data;
                            scope.customersDataSafe = response.data;
                            scope.bk_customersData = angular.copy(response.data);
                            var DifferenceExpense = 0;
                            var ExpenseCurrent = 0;
                            var ExpensePrior = 0;
                            var PercentageExpense = 0;

                            var length = scope.customersData.length;
                            for (var i = 0; i < length; i++) {
                                DifferenceExpense = DifferenceExpense + scope.customersData[i].DifferenceExpense;
                                ExpenseCurrent = ExpenseCurrent + scope.customersData[i].ExpenseCurrent;
                                ExpensePrior = ExpensePrior + scope.customersData[i].ExpensePrior;
                                PercentageExpense = PercentageExpense + scope.customersData[i].PercentageExpense;
                            }
                            scope.TotalDifferenceExpense = $filter('currency')(DifferenceExpense, "$",0);
                            scope.TotalExpenseCurrent = $filter('currency')(ExpenseCurrent, "$", 0);
                            scope.TotalExpensePrior = $filter('currency')(ExpensePrior, "$", 0);
                            scope.TotalPercentageExpense =$filter('numberWithCommas')(PercentageExpense);
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };
              

                scope.reportback = function () {
                    scope.drillDownCount = drillDownCount;
                    if (scope.iscustomersItemReport) {
                        scope.customersItemData =[];
                        scope.customersItemDataSafe =[];
                        scope.iscustomersItemReport = false;
                        scope.csvFileName = scope.csvFileNameOld;
                        scope.callbackFn({
                            title:(scope.isCM01)?scope.cm011stdrilldowntext: scope.breadcrumbold+"> Report"
                        });
                    }
                    else {
                        scope.showReport = false;
                        scope.showChart = true;
                        if (drillDownCount == 2) {
                            scope.callbackFn({ title: (scope.isCM01) ? scope.cm01breadcrumb : $rootScope.secondlast });
                            scope.showChart = true;
                            scope.showReport = false;
                        }
                        else {
                            scope.callbackFn({ title: (scope.isCM01) ? scope.cm01breadcrumb : $rootScope.secondlast });
                            scope.showChart = true;
                            scope.showReport = false;
                        }
                        
                    }

                    var obj = (drillDownCount == 0) ? data[0] : Percentage_Data[drillDownCount - 1];
                    scope.sampledata = $rootScope.data;
                };

                scope.getDetailedReport = function (CustomerNumber, customer) {

                    var text = "";
                    if (scope.isCM01) {
                        scope.callbackFn({
                            title: scope.cm011stdrilldowntext + "<span class='small-text'> > " + customer.trim() + " > Report</span>"
                        });
                        text = $filter('htmlToPlaintext')(scope.cm011stdrilldowntext + "<span class='small-text'> > " + customer.trim() + " > Report</span>");
                    }
                    else {
                        scope.callbackFn({
                            title: scope.breadcrumbold + ">Report<span class='small-text'> > " + customer.trim() + " >Report </span>"
                        });
                        text = $filter('htmlToPlaintext')(scope.breadcrumbold + ">Report <span class='small-text'> > " + customer.trim() + "> Report</span>");
                    }
                    scope.csvFileName = text.replace(">", "-").replace("_", "-").toUpperCase()+".csv";
                    scope.GlobalFilterModel["CustomerNumber"] = CustomerNumber;

                    Metronic.blockUI({ boxed: true });

                    dataService.GetCasesSoldDetails(scope.GlobalFilterModel, scope.isCM01).then(function (response) {
                        if (response && response.data) {
                            scope.TotalQuantity = 0;
                            scope.TotalExtPrice = 0;
                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            scope.iscustomersItemReport = true;

                            scope.customersItemDataSafe = scope.customersItemData;
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
                scope.GetExpenseInvoiceDetailsByCustomer = function (CustomerNumber, customer) {
                    debugger
                    var text = "";
                    if (scope.isCM01) {
                        scope.callbackFn({
                            title: scope.cm011stdrilldowntext + "<span class='small-text'> > " + customer.trim() + " > Report</span>"
                        });
                        text = $filter('htmlToPlaintext')(scope.cm011stdrilldowntext + "<span class='small-text'> > " + customer.trim() + " > Report</span>");
                    }
                    else {
                        scope.callbackFn({
                            title: scope.breadcrumbold + ">Report<span class='small-text'> > " + customer.trim() + " >Report </span>"
                        });
                        text = $filter('htmlToPlaintext')(scope.breadcrumbold + ">Report <span class='small-text'> > " + customer.trim() + "> Report</span>");
                    }
                    scope.csvFileName = text.replace(">", "-").replace("_", "-").toUpperCase() + ".csv";
                    scope.GlobalFilterModel["CustomerNumber"] = CustomerNumber;

                    Metronic.blockUI({
                        boxed: true
                    });

                    dataService.GetExpenseInvoiceDetailsByCustomer(scope.GlobalFilterModel, scope.isCM01).then(function (response) {
                        if (response && response.data) {
                            var TotalExpense = 0;
                          
                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            scope.iscustomersItemReport = true;

                            scope.customersItemDataSafe = scope.customersItemData;
                            scope.bk_customersItemData = angular.copy(response.data);
                            scope.bk_customersItemDataSafe = angular.copy(response.data);
                            if (scope.iscustomersItemReport) {
                                if (scope.currentcommodity == 'All') {
                                    scope.customersItemData = scope.bk_customersItemData;
                                    scope.customersItemDataSafe = scope.bk_customersItemDataSafe;
                                    for (var i = 0; i < scope.customersItemData.length; i++) {
                                        TotalExpense = TotalExpense + scope.customersItemData[i].Expense;
                                     
                                    }
                                } else {
                                    scope.customersItemData = [];
                                    scope.customersItemDataSafe = [];
                                    for (var i = 0; i < scope.bk_customersItemData.length; i++) {
                                        if (scope.bk_customersItemData[i].Comodity.trim() == scope.currentcommodity) {
                                            scope.customersItemData.push(scope.bk_customersItemData[i]);
                                            scope.customersItemDataSafe.push(scope.bk_customersItemData[i]);
                                              TotalExpense = TotalExpense +scope.customersItemData[i].Expense;
                                        }
                                        else {

                                        }
                                    }
                                }

                                scope.TotalExpense = $filter('currency')(TotalExpense, "$", 0);
                            }
                        }
                        Metronic.unblockUI();

                    }, function onError() {

                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                        NotificationService.ConsoleLog('Error on the server');
                    });

                };
                //GetExpenseInvoiceDetailsByCustomer
                scope.currentcommodity = "All";

                scope.FilterCommodity = function (commodity) {
                    scope.currentcommodity = commodity;
                    scope.GlobalFilterModel["Commodity"] = commodity;
                    (attrs.nlDashboard == "profitability") ? GetProfitByCustomerDetailAndCommodity(scope.GlobalFilterModel, scope.isCM01):
                    (attrs.nlDashboard == "expenses") ? GetExpenseReportBySalesman(scope.GlobalFilterModel, scope.isCM01) : getCustomersSalesDataReport(scope.GlobalFilterModel, scope.isCM01);
                };

                scope.getCsvHeader = function () {
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName.toUpperCase();

                    if (!scope.casesold) {
                        return ["Customer Name", "CasesSold Current", "Sales Amount Prior", "Sales Amount Current", "Difference", "Percentage"];
                    } else {
                        return ["Customer Name", "Sales Amount Prior", "Sales Qty", "Sales Amount Current", "Difference", "Percentage"];
                    }
                };
                scope.getCsvDataGetProfitByCustomerDetailAndCommodityrHeader = function () {
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-').replace(/>/g, "-").replace("_", "-").toUpperCase() +".csv";
                    //scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    //scope.csvFileName = scope.csvFileName.replace("_", "-");
                    //scope.csvFileName = scope.csvFileName.toUpperCase()+".csv";
                    return ["Commodity", "Invoice Date", "Invoice Number", "Item Code", "Item Description", "SalesMan Code", "SalesMan Description", "Sales Order", "Cost", "Qty Ship", "Ext Price", "Profit"];
                };
                scope.getCsvDataProfitByCustomerDetailAndCommodity = function () {
                    var array = [];
                    var predefinedHeader = [];
                    predefinedHeader = ["Commodity", "InvoiceDate", "InvoiceNumber", "ItemCode", "ItemDescription", "SalesManCode", "SalesManDescription", "SalesOrder", "Cost", "QtyShip", "ExtPrice", "Profit"];
                    angular.forEach(scope.customersDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                if (key == "Cost" || key == "ExtPrice" || key == "Profit") {
                                    header[index] = "$" + value;
                                }
                                else if (key == "QtyShip") {
                                    header[index] = $filter('numberWithCommas')(value);
                                }
                                else if (key == "InvoiceDate") {
                                    header[index] = $filter('date')(value, 'yyyy-MM-dd');
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
                    total[1] = total[2] = total[3] = total[4] = total[5] = total[6] = total[7] = "";
                    total[8] = "$" + scope.Cost;
                    total[9] = $filter('numberWithCommas')(scope.QtyShip);
                    total[10] = "$" + scope.ExtPrice;
                    total[11] = "$" + scope.Profit;

                    array.push(total);

                    return array;

                };

                scope.getCsvData = function () {
                    if (!scope.casesold) {
                        var array = [];
                        var predefinedHeader = [];
                        predefinedHeader = ["Customer", "CasesSoldCurrent", "SalesAmountPrior", "SalesAmountCurrent", "Difference", "Percentage"];

                        angular.forEach(scope.customersDataSafe, function (value, key) {
                            var header = {};
                            angular.forEach(value, function (value, key) {
                                var index = predefinedHeader.indexOf(key);
                                if (index > -1) {

                                    if (key == "CasesSoldCurrent") {
                                        header[index] = $filter('numberWithCommasRounded')(value);
                                    }
                                    else if (key == "SalesAmountPrior") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
                                    }
                                    else if (key == "Difference") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
                                    }
                                    else if (key == "SalesAmountCurrent") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
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
                        total[1] = $filter('numberWithCommas')(scope.TotalCasesSoldQty);
                        total[2] = $filter('currency')(scope.TotalCasesSoldPrior);
                        total[3] = $filter('currency')(scope.TotalSalesAmtCurrent); //"$" + scope.TotalSalesAmtCurrent;
                        total[4] = "";

                        array.push(total);

                        return array;
                    }
                    else {
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
                                        header[index] = "$" + $filter('numberWithCommas')(value);
                                    }
                                    else if (key == "Difference") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
                                    }
                                    else if (key == "SalesAmountPrior") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
                                    }
                                    else if (key == "SalesQty") {
                                        header[index] = "$" + $filter('numberWithCommas')(value);
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
                        total[1] = "$" + $filter('numberWithCommas')(prior);
                        total[2] = $filter('numberWithCommas')(qty);
                        total[3] = "$" + $filter('numberWithCommas')(current);
                        total[4] = "$" + $filter('numberWithCommas')(diff);
                        total[5] = "";

                        array.push(total);

                        return array;
                    }

                };

                scope.getCsvHeaderDrillDown = function () {
                  
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    return ["Commodity", "Invoice Date", "Invoice #", "Item", "Item Description", "Quantity", "Ext Price", "Sales Person", "SO #"];
                }

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
                                    header[index] = "$" + $filter('numberWithCommas')(value);
                                }
                                else if (key == "Quantity") {
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

                scope.getCasesSoldBySalesPersonCsvDataHeader = function () {
                    return ["Customer Name", "Sales Amt Current", "Cases Sold Prior", "Cases Sold Current", "Difference", "Difference (%)"];
                };

                scope.getCasesSoldBySalesPersonCsvData = function () {

                    var array = [];
                    var predefinedHeader = [];

                    var salesAmtCurrent = 0;
                    var casesSoldCurrent = 0;
                    var casesSoldPrior = 0;
                    predefinedHeader = ["Customer", "SalesAmountCurrent", "CasesSoldPrior", "CasesSoldCurrent", "DifferenceCasesSold", "PercentageCasesSold"];

                    angular.forEach(scope.customersDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {

                                if (key == "CasesSoldCurrent" || key == "CasesSoldPrior") {
                                    header[index] = $filter('numberWithCommas')(value);
                                }
                                else if (key == "DifferenceCasesSold" || key == "PercentageCasesSold") {
                                    header[index] = Math.round(value);
                                }
                                else {
                                    header[index] = value;
                                }

                            }

                            if (key == "SalesAmountCurrent") {
                                salesAmtCurrent = salesAmtCurrent + value;
                            }
                            if (key == "CasesSoldPrior") {
                                casesSoldPrior = casesSoldPrior + value;
                            }
                            if (key == "CasesSoldCurrent") {
                                casesSoldCurrent = casesSoldCurrent + value;
                            }


                        });
                        if (header != undefined)
                            array.push(header);
                    });

                    var total = {};
                    total[0] = "Total";
                    total[1] = "$" + $filter('numberWithCommas')(Math.round(salesAmtCurrent));
                    total[2] = $filter('numberWithCommas')(Math.round(casesSoldPrior));
                    total[3] = $filter('numberWithCommas')(Math.round(casesSoldCurrent));
                    array.push(total);
                    return array;
                };

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
                                else if (key == "InvoiceDate") {
                                    header[index] = $filter('date')(value);
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
                    var total = {}; total[0] = "Total"; total[1] = ""; total[2] = ""; total[3] = ""; total[4] = ""; total[5] = "";
                    total[6] = $filter('numberWithCommas')(Math.round(Quantity));
                    total[7] = $filter('currency')(Math.round(ExtPrice));
                    array.push(total);
                    return array;
                };



                scope.getCsvDataExpenseReportBySalesmanHeader = function () {

                    return ["Customer", "Customer Number", "Expense Current", "Expense Prior", "Difference Expense", "Percentage Expense"];
                };

                scope.getCsvDataExpenseReportBySalesman = function () {
                    debugger
                    var array = [];
                    var predefinedHeader = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    predefinedHeader = ["Customer", "CustomerNumber", "ExpenseCurrent", "ExpensePrior", "DifferenceExpense", "PercentageExpense"];

                    angular.forEach(scope.customersDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                header[index] = (key == "ExpenseCurrent" || key == "ExpensePrior" || key == "DifferenceExpense") ? "$" + $filter('numberWithCommas')(value) : value
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {}; total[0] = "Total"; total[1] = ""; total[5] = "";
                    total[2] = scope.TotalExpenseCurrent;
                    total[3] = scope.TotalExpensePrior;
                    total[4] = scope.TotalDifferenceExpense;
                    array.push(total);
                    return array;
                };

                scope.getCsvDataExpenseInvoiceDetailsByCustomerHeader = function () {

                    return ["Commodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "So #", "Expense"];
                };

                scope.getCsvDataExpenseInvoiceDetailsByCustomer = function () {
                    debugger
                    var array = [];
                    var predefinedHeader = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    predefinedHeader = ["Comodity", "InvoiceDate", "InvoiceNumber", "Item", "ItemDesc", "Sono", "Expense"];

                    angular.forEach(scope.customersItemData, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                header[index] = (key == "Expense") ? $filter('currency')(value, "$", 0) :
                                                (key == "InvoiceDate") ?$filter('date')(value): value
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {}; total[0] = "Total"; total[1] = ""; total[2] = ""; total[3] = ""; total[4] = ""; total[5] = "";
                    total[6] = scope.TotalExpense;
                    array.push(total);
                    return array;
                };


                function toJSONLocal(date) {
                    var local = new Date(date);
                    local.setMinutes(date.getMinutes() - date.getTimezoneOffset());
                    return local.toJSON().slice(0, 10);
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
                };
            }
        };
    }]);

//1266