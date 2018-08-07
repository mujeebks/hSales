
MetronicApp.directive('nlTotalCategories', ['$rootScope', '$filter', '$state', 'dataService', 'NotificationService', '$timeout', 'HelperService',
    function ($rootScope, $filter, $state, dataService, NotificationService, $timeout, HelperService) {
        this.count = 0;
        return {
            restrict: 'E',
            replace: true,
            require: '?ngModel',
            scope: { filterType: '=', callbackFn: '&', callbackFullscreen: '&' },
            template: '<div  id=fulldiv{{::myId}}  class="portlet-body" ng-init="showChart=true" ng-cloak>' +
                 '<div class="chart-back-btn" ng-show="closebutton" style="float:right;">' +
                       '<i class="fa fa-close fa-2x" ng-click="closepanel()" style="cursor:pointer;"></i>' +
                       '</div>' +
                       '<div ng-show="showChart">' +
                       '<div class="chart-back-btn" ng-init="backBtnVisible = false" ng-show="backBtnVisible">' +
                       '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true; backChart()" style="cursor:pointer;"></i>' +
                       '</div>' +
                      '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div>' +
                      '<div ng-cloak ng-show="drillDownCount==0"  id="{{::legendDiv}}" class="clusteredBarChartLegenddiv2 fade-in-up"></div>' +
                      '</div>' +
                      '<div ng-show="showReport">' +
                      '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible">' +
                      '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="reportback()" style="cursor:pointer;"></i>' +
                       '</div>' +
                           '<div style="margin-top: -46px;">' +
                       '<div ng-if="!iscustomersItemReport" ng-include="\'app/pages/sales/sales-report-table.html\'"></div>' +
                         '<div ng-if="iscustomersItemReport" ng-include="\'app/pages/sales/customer-item-table.html\'"></div>' +
                        '</div>' +
                      '</div>' +
                       '<button  ng-if="sampledata !=0" type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#demo" ng-click="changebtnstatus()" ">{{btnstatus}} Sales Persons</button>' +
                       '</div>',

            link: function (scope, element, attrs, controller) {
                scope.closebutton = false;
                var state = $state.current.name;
                scope.legendDiv = "categories_legend" + count;
                scope.myId = 'categories_chart' + (count++);
                scope.salesPersonBtnID1 = false;
                var chart;
                var data;
                var balloonTextPrefix = (state == "cases-sold-categories") ? "" : "$";
                scope.casesold = (state == "cases-sold-categories") ? true : false;
                var drillDownCount = 0;
                scope.drillDownCount = drillDownCount;
                var drillDownData = [];
                scope.btnstatus = "Show";
                scope.currentcommodity = "All";
                scope.sampledata = 0;
                scope.customersData = [];
                scope.customersDataSafe = [];
                scope.iscustomersItemReport = false;
                scope.myTableFunctions = {};
                scope.groupProperty = '';
                scope.dashboard == attrs.nlDashboard;
                scope.isfullscreen = false;
                scope.ISCM01 = false;
                var initChart = function (chartData, title, labelRotation) {
                    chart = AmCharts.makeChart(scope.myId, {
                        "theme": "none",
                        "type": "serial",
                        "zoomOutOnDataUpdate": false,
                        "valueAxes": [{
                            "stackType": "regular",
                            "position": "left",
                            "title": (state == "cases-sold-categories") ? "Cases sold qty" : "Revenue"
                        }],
                        "graphs": [
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
                                  var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label1 : "";                             
                                  return label;
                              },
                              "labelRotation": (drillDownCount == 0) ? 0 : -35
                          },
                           {
                               "id": "1",
                               "fillAlphas": 1,
                               "lineAlpha": 0.2,
                               "title": "",
                               "type": "column",
                               "valueField": (scope.casesold == true) ? "cValue1" : "rValue1",
                               "fillColorsField": "Color1",
                               "balloonText": " ",
                               "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                   var total = 0;
                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue1 : graphDataItem.dataContext.rValue1;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue2 : graphDataItem.dataContext.rValue2;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.SalesPerson1 + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";
                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   var span = calcpercentage(graphDataItem);
                                   return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;
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
                                   var total = 0;
                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue1 : graphDataItem.dataContext.rValue1;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue2 : graphDataItem.dataContext.rValue2;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.SalesPerson1 + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";
                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   var span = calcpercentage(graphDataItem);
                                   return graphDataItem.dataContext.Label2 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + span + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;
                               }
                           },
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
                                   var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label3 : "";
                                   return label;
                               },
                               "labelRotation": (drillDownCount == 0) ? 0 : -35

                           },
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
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> "
                                                             + graphDataItem.dataContext.SalesPerson1
                                                             + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   return graphDataItem.dataContext.Label3
                                            + "<br /><b style='font-size: 130%'>Total "
                                            + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce "
                                            + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery "
                                            + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />"
                                            + salespersonToolTip;

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
                                   var total = 0;

                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue3 : graphDataItem.dataContext.rValue3;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue4 : graphDataItem.dataContext.rValue4;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.SalesPerson1 + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;

                                   total = Math.round(((total) * 100) / 100);

                                   return graphDataItem.dataContext.Label4 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;

                               }

                           },
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
                                   var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.Label5 : "";
                                   return label;
                               },
                               "labelRotation": (drillDownCount == 0) ? 0 : -35

                           },
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
                                   var total = 0;

                                   var val1 = (scope.casesold == true) ? graphDataItem.dataContext.cValue5 : graphDataItem.dataContext.rValue5;
                                   var val2 = (scope.casesold == true) ? graphDataItem.dataContext.cValue6 : graphDataItem.dataContext.rValue6;
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.SalesPerson1 + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   return graphDataItem.dataContext.Label5 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;

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
                                   var salespersonToolTip = (graphDataItem.dataContext.ActiveSalesPersonCode) ? "Sales Person <br /> " + graphDataItem.dataContext.SalesPerson1 + "(" + graphDataItem.dataContext.ActiveSalesPersonCode + ")" + "" : "";

                                   total = val1 + val2;
                                   total = Math.round(((total) * 100) / 100);
                                   return graphDataItem.dataContext.Label6 + "<br /><b style='font-size: 130%'>Total " + balloonTextPrefix + HelperService.formatCommaSeperate(total) + " </b><br />Produce " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val2)) + "<br /> Grocery " + balloonTextPrefix + HelperService.formatCommaSeperate(Math.round(val1)) + "<br />" + salespersonToolTip;

                               }
                           }
                        ],
                        "depth3D": 30,
                        "angle": 30,
                        "categoryField": "GroupName",
                        "categoryAxis": {
                            "ignoreAxisWidth": (drillDownCount == 0) ? true : false,
                            "gridPosition": "start",
                            "tickPosition": "start",
                            "labelRotation": (drillDownCount == 0) ? 0 : 45,
                            "autoWrap": true,
                            "labelOffset": 15,
                        },
                        "legend": {
                            "data":HelperService.generateLegend(chartData,drillDownCount),
                            "divId": scope.legendDiv
                        },
                        "columnSpacing": 12,
                    });
                    chart.dataProvider = chartData;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][6] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][7] : null;
                    (dataService.IsGlobal_Filter_Year()) ? delete chart["graphs"][8] : null;
                    chart.validateData();
                    chart.addListener('clickGraphItem', function (event) {
                        if (event.graph.id !== "000") {
                            scope.closebutton = true;
                            var selectedPeriod = "";
                            if (drillDownCount < 1) {
                                if (event.item.dataContext.SubData.length == 0) {
                                    scope.$apply(function () {
                                        scope.backBtnVisible = false;
                                        scope.closebutton = false;
                                    });
                                    return false;
                                }
                                if (drillDownCount == 0) {
                                    var selectedData = chartData[event.item.index];
                                    selectedPeriod = (event.graph.id == "1") ? selectedData.Label1 :
                                                     (event.graph.id == "2") ? selectedData.Label2 :
                                                     (event.graph.id == "3") ? selectedData.Label3 :
                                                     (event.graph.id == "4") ? selectedData.Label4 :
                                                     (event.graph.id == "5") ? selectedData.Label5 :
                                                     (event.graph.id == "6") ? selectedData.Label6 : "";

                                    scope.categoriestitleld = attrs.category.toUpperCase() + " <br/><span class='small-text'> " + selectedPeriod + "</span>";
                                    scope.categoriesTitlePrevious = attrs.category.toUpperCase() + " <br/><span class='small-text'> " + selectedPeriod + "</span>";
                                    scope.CurrentTitle = scope.categoriesTitlePrevious;
                                    scope.callbackFn({ title: scope.categoriesTitlePrevious, charttype: attrs.category });
                                }

                                if (drillDownCount == 1) {
                                    if (state == "revenue-categories") {
                                        scope.categoriesTitlePrevious = scope.categoriestitleld + "<span class='small-text'> > " + event.item.category.toUpperCase() + " </span>";
                                        scope.CurrentTitle = scope.categoriesTitlePrevious;
                                        scope.callbackFn({ title: scope.categoriesTitlePrevious, charttype: attrs.category });
                                    }
                                    else {
                                        scope.TitlePrevious = scope.titleold + "<span class='small-text'> > " + event.item.category.toUpperCase() + " </span>";
                                        scope.CurrentTitle = scope.TitlePrevious;
                                        scope.callbackFn({ title: scope.TitlePrevious, charttype: attrs.category });
                                    }
                                }

                                if (event.item.dataContext.SubData != undefined) {
                                    drillDownData[drillDownCount] = event.item.dataContext.SubData;
                                    drillDownCount += 1;
                                    scope.drillDownCount = drillDownCount;
                                    subChartData = event.item.dataContext.SubData;
                                    scope.eventCategory = (event.item.dataContext.GroupName).split("(");
                                    scope.isfullscreen = true;
                                    Metronic.blockUI({ boxed: true, message: 'Opening ' + event.item.category + ' ...' });
                                    $("#fulldiv" + scope.myId).hide();
                                    scope.callbackFullscreen({ category: attrs.xtitle });
                                    initChart(subChartData, "", 35);
                                    scope.$apply(function () {scope.backBtnVisible = true;scope.closebutton = true;});
                                    $timeout(function () {
                                        $("#fulldiv" + scope.myId).show();
                                        Metronic.unblockUI();
                                    }, 500)
                                }
                                else { return false; }
                            }
                            else {
                                if (event.item.category == "CUSTOMER SERVICE") {
                                    scope.ISCM01 = true;
                                    //here need to call the CM01 chart;
                                    var title = scope.categoriesTitlePrevious + "<span class='small-text'> > " + event.item.category + " </span>";
                                    scope.customerservicetitle = title;
                                    scope.callbackFn({ title: title, charttype: attrs.category });
                                    GetCustomerServiceDetails($rootScope.currentfilter.Id);
                                    scope.$apply(function () {scope.showChart = true; scope.showReport = false;});
                                }
                                else {
                                    scope.$apply(function () { scope.showChart = false; scope.showReport = true; });

                                    var selectedData = chartData[event.item.index];
                                    scope.GlobalFilterModel = {
                                        Category: attrs.xtitle,
                                        Commodity: scope.currentcommodity,
                                        SalesPerson: (selectedData.ActiveSalesPersonCode == null) ? selectedData.GroupName : selectedData.ActiveSalesPersonCode,
                                        FilterId: dataService.NogalesCurrentFilter(),
                                        Period: (event.graph.id == "1" || event.graph.id == "2") ? 0 :
                                                 (event.graph.id == "3" || event.graph.id == "4") ? 1 :
                                                 (event.graph.id == "5" || event.graph.id == "6") ? 2 : "",
                                        OrderBy: (scope.casesold == true) ? "casessold" : "sales"
                                    };
                                    scope.ActiveSalesPersonCode = selectedData.ActiveSalesPersonCode;
                                    var text = "";
                                    if (selectedData.ActiveSalesPersonCode == null) {
                                        //its cm01
                                        getCustomersSalesDataReport(scope.GlobalFilterModel, true);
                                        scope.breadcrumb = scope.customerservicetitle + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'> > Report </span>";
                                        scope.CurrentTitle = scope.breadcrumb;
                                        scope.callbackFn({ title: scope.breadcrumb, charttype: attrs.category });
                                        text = $filter('htmlToPlaintext')(scope.customerservicetitle + "<span class='small-text'> > " + event.item.dataContext.GroupName + " </span>" + "<span class='small-text'> > Report </span>");
                                    }
                                    else {
                                       
                                        getCustomersSalesDataReport(scope.GlobalFilterModel,false);
                                        scope.breadcrumb = scope.categoriesTitlePrevious + "<span class='small-text'> > " + event.item.dataContext.SalesPerson1.toUpperCase() + " </span>" + "<span class='small-text'> > Report </span>";
                                        scope.CurrentTitle = scope.breadcrumb;
                                        scope.callbackFn({ title: scope.breadcrumb, charttype: attrs.category });
                                        text = $filter('htmlToPlaintext')(scope.categoriesTitlePrevious + "<span class='small-text'> > " + event.item.dataContext.SalesPerson1.toUpperCase().trim() + " </span>" + "<span class='small-text'> > Report </span>");
                                    }
                                        text = text.replace('>', '-').toUpperCase() + ".csv".toUpperCase();
                                        scope.csvFileName = text;
                                        scope.csvFileNameOld = text;

                                }
                            }
                        }
                    });

                
                    function GetCustomerServiceDetails(filterId) {
                        scope.customersData = [];
                        scope.customersDataSafe = [];
                        drillDownCount = drillDownCount + 1;
                        Metronic.blockUI({ boxed: true });
                        dataService.GetCustomerServiceDetails(filterId, false).then(function (response) {
                            if (response && response.data) {
                                scope.showChart = true;
                                scope.showReport = false;
                                drillDownData[drillDownCount] = response.data.TotalCasesSold.All[0].SubData;
                                drillDownCount += 1;
                                scope.drillDownCount = drillDownCount;
                                subChartData = response.data.TotalCasesSold.All[0].SubData;
                                scope.isfullscreen = true;
                                $("#fulldiv" + scope.myId).hide();
                                scope.callbackFullscreen({ category: attrs.xtitle });
                                initChart(subChartData, "", 35);
                                scope.backBtnVisible = true;
                                scope.closebutton = true;
                                $timeout(function () {
                                    $("#fulldiv" + scope.myId).show();
                                }, 500)
                            }
                            Metronic.unblockUI();
                        }, function onError() {
                            Metronic.unblockUI();
                            NotificationService.Error("Error upon the API request");
                        });
                    };
                    var calcpercentage = function (graphDataItem) {
                        var first = (scope.casesold) ? (graphDataItem.dataContext.cValue1) + (graphDataItem.dataContext.cValue2) : (graphDataItem.dataContext.rValue1) + (graphDataItem.dataContext.rValue2);
                        var second = (scope.casesold) ? (graphDataItem.dataContext.cValue3) + (graphDataItem.dataContext.cValue4) : (graphDataItem.dataContext.rValue3) + (graphDataItem.dataContext.rValue4);
                        var PriorMonth = first - second;
                        if (PriorMonth !== 0) {
                            PriorMonth = PriorMonth / second;
                            PriorMonth = PriorMonth * 100;
                            PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
                        }
                        else {
                            PriorMonth = 0;
                        }
                        var PriorMonthSelected = Math.round(PriorMonth);
                        return (PriorMonthSelected > 0) ? "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(PriorMonthSelected) + " %" + "</span>)</span>" :
                               (PriorMonthSelected < 0) ? "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(PriorMonthSelected) + " %" + "</span>)</span>" : "";
                    };
                };

                scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                    var array = [];
                    array.push(newVal);
                    scope.backBtnVisible = false;
                    drillDownCount = 0;
                    scope.drillDownCount = drillDownCount;
                    scope.showChart = true;
                    scope.showReport = false;
                    data = array;
                    scope.initialdata = data;

                    if (data == undefined || data.length == 0) {
                        document.getElementById(scope.myId).innerText = (data == undefined) ? "" : "The chart contains no data !";
                    }
                    else {
                        initChart(data, "", 0);
                    }

                });


                scope.closepanel = function () {
                    drillDownCount = 0;
                    scope.backBtnVisible = false;
                    scope.closebutton = false;
                    scope.CurrentTitle = attrs.xtitle;
                    scope.callbackFn({ title: attrs.xtitle, charttype: attrs.category });
                    scope.callbackFullscreen({ category: "" });
                    scope.showReport = false;
                    scope.showChart = true;
                    scope.drillDownCount = 0;
                    initChart(scope.initialdata, "", 0);
                };
               

                scope.backChart = function () {
                    drillDownCount -= 1;
                    scope.drillDownCount = drillDownCount;
                    if (drillDownCount == 2) {
                        initChart(drillDownData[0], "", 35);
                        scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                        scope.CurrentTitle = scope.categoriestitleld;
                        scope.callbackFn({ title: scope.categoriestitleld, charttype: attrs.category });
                        scope.callbackFullscreen({ category: attrs.xtitle });
                        scope.closebutton = true;
                    } else if (drillDownCount == 1) {
                        scope.closepanel();
                    }
                    else {
                        initChart(drillDownCount == 0 ? data : drillDownData[drillDownCount - 1], "", drillDownCount == 0 ? 0 : 35);
                        scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                        scope.CurrentTitle = attrs.xtitle;
                        scope.callbackFn({ title: attrs.xtitle, charttype: attrs.category });
                        scope.callbackFullscreen({ category: "" });
                        scope.closebutton = false;
                    }
                };

                scope.reportback = function () {
                    if (scope.ActiveSalesPersonCode == null) {
                        //cm01
                        if (scope.iscustomersItemReport) {
                            scope.iscustomersItemReport = false;
                            scope.csvFileName = scope.csvFileNameOld;
                            scope.callbackFn({ title: scope.breadcrumb, charttype: attrs.category });
                        }
                        else {
                            scope.showReport = false;
                            scope.showChart = true;
                            scope.callbackFn({ title: scope.customerservicetitle, charttype: attrs.category });
                        }
                    } else {
                        if (scope.iscustomersItemReport) {
                            scope.iscustomersItemReport = false;
                            scope.csvFileName = scope.csvFileNameOld;
                            scope.callbackFn({ title: scope.breadcrumb, charttype: attrs.category });
                        }
                        else {
                            scope.showReport = false;
                            scope.showChart = true;
                            scope.callbackFn({ title: scope.categoriesTitlePrevious, charttype: attrs.category });
                        }
                    }
                }

                function getCustomersSalesDataReport(salesPerson,iscm01) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({ boxed: true});
                    dataService.GetSalesPersonCustomersDataTotal(salesPerson,iscm01).then(function (response) {
                        if (response && response.data) {
                            scope.customersData = response.data;
                            scope.customersDataSafe = angular.copy(response.data);
                            var TotalCasesSoldCurrent = 0;
                            var TotalCasesSoldPrior = 0;
                            var TotalDifference = 0;
                            var TotalDifferenceCasesSold = 0;
                            var TotalDifferenceExpense = 0;
                            var TotalExpenseCurrent = 0;
                            var TotalExpensePrior = 0;
                            var TotalNoOfCustomer = 0;
                            var TotalPercentage = 0;
                            var TotalPercentageCasesSold = 0;
                            var TotalPercentageExpense = 0;
                            var TotalSalesAmountCurrent = 0;
                            var TotalSalesAmountPrior = 0;
                            var TotalSalesQty = 0;
                            var length = scope.customersDataSafe.length;

                            for (var i = 0; i < length; i++) {
                                debugger

                                TotalCasesSoldCurrent = TotalCasesSoldCurrent + scope.customersDataSafe[i].CasesSoldCurrent;
                                TotalCasesSoldPrior = TotalCasesSoldPrior + scope.customersDataSafe[i].CasesSoldPrior;
                                TotalDifference = TotalDifference + scope.customersDataSafe[i].Difference;
                                TotalDifferenceCasesSold = TotalDifferenceCasesSold + scope.customersDataSafe[i].DifferenceCasesSold;
                                TotalDifferenceExpense = TotalDifferenceExpense + scope.customersDataSafe[i].DifferenceExpense;
                                TotalExpenseCurrent = TotalExpenseCurrent + scope.customersDataSafe[i].ExpenseCurrent;
                                TotalExpensePrior = TotalExpensePrior + scope.customersDataSafe[i].ExpensePrior;
                                TotalNoOfCustomer = TotalNoOfCustomer + scope.customersDataSafe[i].NoOfCustomer;
                                TotalPercentage = TotalPercentage + scope.customersDataSafe[i].Percentage
                                TotalPercentageCasesSold = TotalPercentageCasesSold + scope.customersDataSafe[i].PercentageCasesSold;
                                TotalPercentageExpense = TotalPercentageExpense + scope.customersDataSafe[i].PercentageExpense;
                                TotalSalesAmountCurrent = TotalSalesAmountCurrent + scope.customersDataSafe[i].SalesAmountCurrent;
                                TotalSalesAmountPrior = TotalSalesAmountPrior + scope.customersDataSafe[i].SalesAmountPrior;
                                TotalSalesQty = TotalSalesQty + scope.customersDataSafe[i].SalesQty
                               
                            }

                            scope.TotalCasesSoldCurrent = $filter('numberWithCommasRounded')(TotalCasesSoldCurrent);
                            scope.TotalCasesSoldPrior = $filter('numberWithCommasRounded')(TotalCasesSoldPrior);
                            scope.TotalDifference = $filter('numberWithCommasRounded')(TotalDifference);
                            scope.TotalDifferenceExpense = "$" + $filter('numberWithCommasRounded')(TotalDifferenceExpense);
                            scope.TotalExpenseCurrent = "$" + $filter('numberWithCommasRounded')(TotalExpenseCurrent);
                            scope.TotalExpensePrior = "$" + $filter('numberWithCommasRounded')(TotalExpensePrior);
                            scope.TotalNoOfCustomer = TotalNoOfCustomer;
                            scope.TotalPercentage = TotalPercentage;
                            scope.TotalPercentageCasesSold = $filter('numberWithCommasRounded')(TotalPercentageCasesSold)
                            scope.TotalPercentageExpense = $filter('numberWithCommasRounded')(TotalPercentageExpense)
                            scope.TotalSalesAmountCurrent = "$" + $filter('numberWithCommasRounded')(TotalSalesAmountCurrent);
                            scope.TotalSalesAmountPrior = "$" + $filter('numberWithCommasRounded')(TotalSalesAmountPrior);
                            scope.TotalSalesQty = $filter('numberWithCommasRounded')(TotalSalesQty);
                          
                            Metronic.unblockUI();
                        }
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                }

                scope.getDetailedReport = function (CustomerNumber, customer) {

                    Metronic.blockUI({ boxed: true});
                    scope.callbackFn({ title: scope.breadcrumb + "<span class='small-text'> > " + customer + " </span>", charttype: attrs.category });
                    scope.CurrentTitle = scope.breadcrumb + "<span class='small-text'> > " + customer + " </span>";
                    var splited = scope.csvFileName.split(".");
                    scope.csvFileName = splited[0] + customer.trim() + "-REPORT.csv";
                    scope.GlobalFilterModel["CustomerNumber"] = CustomerNumber;
                    scope.GlobalFilterModel["OrderBy"] = (scope.casesold == true) ? "casessold" : "sales"
                    var iscm01 = scope.ActiveSalesPersonCode == null ? true : false;
                    dataService.GetCasesSoldDetails(scope.GlobalFilterModel, iscm01).then(function (response) {
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
                                    }
                                }
                            }
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                }

                scope.getCsvHeader = function () {
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-').replace(/>/g, "-").replace("_", "-")
                    return (scope.casesold) ? ["Customer Name", "Sales Amt Current", "Cases Sold Prior", "Cases Sold Current", "Difference", "Difference (%)"] :
                                              ["Customer Name", "Cases Sold Current", "Sales Amt Prior", "Sales Amt Current", "Difference", "Difference (%)"]
                };

                scope.getCsvData = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var diff = 0;
                    var prior = 0;
                    var qty = 0;
                    predefinedHeader = (scope.casesold) ? ["Customer", "SalesAmountCurrent", "CasesSoldPrior", "CasesSoldCurrent", "DifferenceCasesSold", "PercentageCasesSold"] :
                                                          ["Customer", "CasesSoldCurrent", "SalesAmountPrior", "SalesAmountCurrent", "Difference", "Percentage"];

                    angular.forEach(scope.customersDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {

                                header[index] = (key == "SalesAmountCurrent" || key == "Difference" || key == "SalesAmountPrior") ? "$" + Math.round(value):
                                                (key == "CasesSoldPrior" || key == "CasesSoldCurrent" || key == "DifferenceCasesSold" || key == "Percentage" || key == "PercentageCasesSold") ? $filter('numberWithCommasRounded')(value):
                                                (key == "Customer") ? value : Math.round(value);
                            }
                        });
                        if (header != undefined) { array.push(header); }
                    });
                     var total = {};
                        total[0] = "Total";
                        total[1] = (scope.casesold) ? scope.TotalSalesAmountCurrent : scope.TotalCasesSoldCurrent;
                        total[2] = (scope.casesold) ? scope.TotalCasesSoldPrior : scope.TotalSalesAmountPrior;
                        total[3] = (scope.casesold) ? scope.TotalCasesSoldCurrent : scope.TotalSalesAmountCurrent;
                        total[4] = total[5] = "";
                        array.push(total);
                        return array;


                };

                scope.getCsvHeaderDrillDown = function () {
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-').replace(/>/g, "-").replace("_", "-");
                    return ["Commodity", "Invoice Date", "Invoice #", "Item", "Item Description", "Quantity", "Ext Price", "Sales Person", "SO #"];
                };

                scope.getCsvDataDrillDown = function () {
                    var array = [];
                    var predefinedHeader = [];
                    predefinedHeader = ["Comodity", "InvoiceDate", "InvoiceNumber", "Item", "ItemDesc", "Quantity", "ExtPrice", "SalesMan", "Sono"];
                    var ExtPriceTotal = 0;
                    var Quantity = 0;
                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                header[index] = (key == "ExtPrice") ? "$" + value :
                                                (key == "Quantity") ? $filter('numberWithCommas')(value) : Math.round(value);
                            }
                            if (key == "ExtPrice") {
                                ExtPriceTotal = ExtPriceTotal + value;
                            }
                            if (key == "Quantity") {
                                Quantity = Quantity + value;
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {};
                    total[0] = "Total";
                    total[1] = total[2] = total[3] = total[4] = ""
                    total[5] = $filter('numberWithCommas')(Quantity);
                    total[6] = $filter('numberWithCommas')(ExtPriceTotal);
                    total[7] = total[8] = "";
                    array.push(total)
                    return array;
                };

                scope.getCsvDataItemSoldToCustomerHeader = function () {
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-').replace(/>/g, "-").replace("_", "-")
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
                                header[index] =(key == "ExtPrice") ? "$" + Math.round(value):
                                               (key == "Quantity") ?  $filter('numberWithCommas')(Math.round(value)):
                                               (key == "InvoiceDate") ?  $filter('date')(value) :value;
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
                    var total = {};
                    total[0] = "Total";
                    total[1] = total[2] = total[3] = total[4] = total[5] = "";
                    total[6] = $filter('numberWithCommas')(Math.round(Quantity));
                    total[7] = $filter('currency')(Math.round(ExtPrice));
                    array.push(total);
                    return array;
                };
                scope.FilterCommodity = function (commodityFilter) {
                    scope.currentcommodity = commodityFilter;
                    scope.GlobalFilterModel["Commodity"] = commodityFilter;
                    var iscm01 = (scope.ActiveSalesPersonCode == null) ? true : false;
                    getCustomersSalesDataReport(scope.GlobalFilterModel,iscm01);
                };
            }
        };
    }]);
//1034