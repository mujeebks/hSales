
MetronicApp.directive('nlClusteredBarToggleChartWarehouse', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {valueAxesTitle: "=", topBottomToggleClusterCountLabel: "=", callbackFn: '&'},

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
                        '<toggle-switch ng-model="topBottomToggle" ng-init="topBottomToggle=true;" on-label="Top 10" off-label="Bottom 10" ng-click="loadDestinationFacilityChart()">' +
                         '<toggle-switch>' +
                         '</div>' +
                     '<div id="{{::legendDiv}}" ng-show="showlegend" class="clusteredBarChartLegenddiv"></div>' +
                     '</div>' +
                     '<div ng-show="showReport">' +
                     '<div class="chart-back-btn" ng-init="chartBackBtnVisible = true" ng-show="chartBackBtnVisible && !isDrillDown">' +
                           '<i class="fa-arrow-circle-o-left fa fa-2x" ng-click="showReport=false;showChart=true;reportback()" style="cursor:pointer;"></i>' +
                      '</div>' +
                          '<div style="margin-top: -26px;">' +
                      '<div ng-if="showReport" ng-include="\'app/pages/warehouse/warehouse-report-table.html\'"></div>' +
                     '</div>' +
                      '</div>' +
                      '</div>',
             link: function (scope, element, attrs, controller) {

                 scope.isDrillDown = false;
                 var chart;
              //   var data;
                 var subData = null;
                // var chartTitle = "";
                 var title;
                 var drillDownCount = 0;
                 var drillDownData = [];
                 scope.legendDiv = 'legenddiv_clustered_bar_toggle' + count;
                 scope.myId = 'chart_div_clustered' + (count++);
                 scope.showlegend = true;
                 scope.ReportData = {};

                 scope.groupPropertycustomer = '';
                 scope.reportByName = 'EmployeeReport';
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
                             "title": scope.valueAxesTitle
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
                                "labelText": " ", 
                                "labelFunction": function label(graphDataItem) {
                                    return (drillDownCount == 0) ? graphDataItem.dataContext.Column1 : "";
                                },
                               "labelOffset": -5,
                                //"labelRotation": 0,
                                //"autoWrap": true
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
                                    var AveragePiecesspan = calcpercentage(graphDataItem, true, "Val");
                                    var HoursWorkedspan = calcpercentage(graphDataItem, true, "HoursWorked");
                                    var PiecesPickedspan = calcpercentage(graphDataItem, true, "PiecesPicked");

                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column1 + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val1) + AveragePiecesspan + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked1) + PiecesPickedspan + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked1 + HoursWorkedspan + "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column1 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val1) + AveragePiecesspan + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked1) + PiecesPickedspan + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked1 + HoursWorkedspan + "<br /><b style='font-size: 130%'></b>";
                                    };
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
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column2;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                                "labelOffset": -5,
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
                                 
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column2 + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val2)  + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked2)  + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked2  + "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column2 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val2)  + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked2)  + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked2  + "<br /><b style='font-size: 130%'></b>";
                                    };
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
                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column3;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                                "labelOffset": -5,
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
                                    
                                    var AveragePiecesspan = calcpercentage(graphDataItem, false, "Val");
                                    var HoursWorkedspan = calcpercentage(graphDataItem, false, "HoursWorked");
                                    var PiecesPickedspan = calcpercentage(graphDataItem, false, "PiecesPicked");

                                    if (drillDownCount == 0) {
                                        return graphDataItem.dataContext.Column3 + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val3) + AveragePiecesspan + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked3) + PiecesPickedspan + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked3 + HoursWorkedspan + "<br /><b style='font-size: 130%'></b>";
                                    } else {
                                        return graphDataItem.dataContext.Column3 + "<br /> <b style='font-size: 130%'>" + graphDataItem.dataContext.Category + "</b>" + "<br /><b>Average Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.Val3) + AveragePiecesspan + " </b> <br /> Total Picked : " + $filter('numberWithCommasRounded')(graphDataItem.dataContext.PiecesPicked3) + PiecesPickedspan + "<br /> Hours Worked : " + graphDataItem.dataContext.HoursWorked3 + HoursWorkedspan + "<br /><b style='font-size: 130%'></b>";
                                    };
                                }
                            },
                         ],
                         "plotAreaFillAlphas": 0.1,
                         "depth3D": 30,
                         "angle": 30,
                         "categoryField": "Category",
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
                         debugger
                       //  var catTitle;
                         var graphid = event.graph.id;
                         if (graphid !== "00") {

                         //    catTitle = event.item.dataContext.Category;

                             if (event.item.dataContext.SubData != undefined) {

                                 subChartData = (scope.topBottomToggle) ? event.item.dataContext.SubData.Top : event.item.dataContext.SubData.Bottom;
                                 drillDownData[drillDownCount] = subChartData;
                              //   if (drillDownCount == 0) {
                                     //scope.categoriestitleld = "PICKER PRODUCTIVITY CHART" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     //scope.categoriesTitlePrevious = "PICKER PRODUCTIVITY CHART" + " </br> " + "<span class='small-text'>" + catTitle + "</span>";
                                     //scope.callbackFn({ title: scope.categoriesTitlePrevious });
                                // }
                                 drillDownCount += 1;
                                 scope.showlegend = (drillDownCount == 1) ? false : true;
                                 subData = event.item.dataContext.SubData;
                                 event.chart.legend = null;
                                 initChart(subChartData, "", 35);
                                 chart.validateData();
                                 scope.$apply(function () {scope.backBtnVisible = true;});
                             }
                             else {
                                                                
                                     var selectedData = chartData[event.item.index];
                                     var Period = (graphid == "g1") ? 0 :
                                                  (graphid == "g2") ? 1 :
                                                  (graphid == "g3") ? 2 : "";

                                   scope.categoriesTitlesecond = "PICKER PRODUCTIVITY CHART" + " </br> " + "<span class='small-text'>" + event.item.dataContext.Category + " > Report</span>";
                               
                                     scope.callbackFn({ title: scope.categoriesTitlesecond });
                                     scope.$apply(function () { scope.showChart = false;scope.showReport = true;});


                                     var requestData = {
                                         IsCM01: "",
                                         OrderBy: "",
                                         CustomerNumber: selectedData.PickerId,
                                         FilterId: $rootScope.currentfilter.Id,
                                         Period: Period,
                                         AccountType: "",
                                         AccountNumber: "",
                                         DriverCode:""
                                     };
                                    
                                     GetWareHouseReport(requestData);
                                 // generateWarehouseWmsPickerProductivityReportDetails
                             }

                         }
                         scope.reportback = function () {
                             scope.callbackFn({ title: "PICKER PRODUCTIVITY CHART" });
                         };
                         function GetWareHouseReport(requestData) {
                             Metronic.blockUI({ boxed: true });
                             scope.customersData = [];
                             scope.customersDataSafe = [];
                           
                           
                             dataService.GetPickerProductivityDayReport(requestData).then(function (response) {
                                 if (response && response.data) {
                                     Metronic.unblockUI();
                                     scope.customersData = response.data;
                                     scope.customersDataSafe = angular.copy(response.data);

                                     var TotalPiecesPicked = 0;
                                     var TotalHours = 0;

                                     for (var i = 0; i < scope.customersDataSafe.length; i++) {
                                         TotalPiecesPicked = TotalPiecesPicked + scope.customersDataSafe[i].PiecesPicked;
                                         TotalHours = TotalHours + scope.customersDataSafe[i].HoursWorked;
                                     }
                                     

                                     scope.TotalPiecesPicked = TotalPiecesPicked;
                                     scope.TotalHours =TotalHours;
                           
                                 }
                                 Metronic.stopPageLoading();
                             }, function onError() {
                                 Metronic.unblockUI();
                                 Metronic.stopPageLoading();
                         
                             });
                         }
                     });
                     function generateLegend(legData) {
                         function getyear(date) {
                             var d = new Date(date);
                             return d.getFullYear();
                         };
                         var tempLegend = [];
                         var legendNames = [];

                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column1 : "",
                             color: (legData[0]) ? legData[0].Color1 : "",
                         })
                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column3 : "",
                             color: (legData[0]) ? legData[0].Color1 : "",
                         })
                         tempLegend.push({
                             title: (legData[0]) ? legData[0].Column2 : "",
                             color: (legData[0]) ? legData[0].Color2 : "",
                         })
                         return tempLegend;
                     }

                 };
                 var calcpercentage = function (graphDataItem, Previousmonth, type) {
                     var first;
                     var second;

                     if (Previousmonth) {
                         first = graphDataItem.dataContext[type + "1"];
                         second = graphDataItem.dataContext[type + "2"];
                     }
                     else {
                         first = graphDataItem.dataContext[type + "3"];
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
                 //    chartTitle = "";
                     if (data == undefined || data.length == 0) {

                     }
                     else {
                         initChart(data, "", 0);
                     }

                 });

                 scope.$watch('showReport', function (newVal) {
                     debugger
                     if (newVal) {

                     } else {

                     }
                   

                 });
                 
                 scope.backToReport = function () {
                     scope.isDrillDown = false;
                     scope.reportByName = "EmployeeReport";
                  
                     scope.callbackFn({ title: scope.categoriesTitlesecond });

                 };
                 var date = new Date();
                 scope.datafilter = {

                     userId: "",
                     startWorkDate: $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy'),
                     endWorkDate: $filter('date')(date, 'MM/dd/yyyy'),
                     startTime: '5:00 AM',
                     endTime: '4:00 PM'

                 };
                 scope.getDetailedReport = function (item) {
                  
                     scope.callbackFn({ title: scope.categoriesTitlesecond.replace('Report', item.TaskDateString +"> Report")});
                     scope.isDrillDown = true;
                    
                     scope.reportByName = "SalesDetailedReport";
                     scope.salesDetailedReport = [];
                     scope.salesDetailedReportMaster = [];
                     var totalDetailedExtendedPrice = 0;
                     var totalDetailedMargin = 0;
                     var totalDetailedQtyShipped = 0;
                     //Search Parameters
                     debugger
                     var searchFilter = {

                         startWorkDate: $filter('date')(item.StartTime, 'MM/dd/yyyy'),
                         endWorkDate: $filter('date')(item.StartTime, 'MM/dd/yyyy'),
                         startTime: item.StartTimeString,
                         endTime: item.EndTimeString,
                         EmployeID: item.UserId

                     };

                     Metronic.blockUI({ boxed: true });
                     (httpRequest = dataService.generateWarehouseWmsPickerProductivityReportDetailsForWareHouse(searchFilter))
                             .then(function (response) {
                                 if (response) {

                                     scope.salesDetailedReport = response.data;
                                     scope.salesDetailedReportMaster = angular.copy(response.data);
                                     scope.totalPicked = item.PiecesPicked;
                                     var TotalDetailedHour = 0;
                                     for (var i = 0; i < scope.salesDetailedReportMaster.length; i++) {
                                         TotalDetailedHour = TotalDetailedHour + scope.salesDetailedReportMaster[i].HoursWorked
                                     }

                                     scope.TotalDetailedHour = TotalDetailedHour;
                                 }
                                 Metronic.unblockUI();
                                 //$scope.btnSpinner.stop();
                             })
                             .catch(function (error) {
                                 var a = error;
                                 Metronic.unblockUI();
                                 //$scope.btnSpinner.stop();
                             });
                 }
                 scope.$watch('backBtnVisible', function (newVal, oldVal) {
                     if (!newVal && oldVal) {
                       //  chartTitle = "";
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

              

                 scope.clickedButtonFn = function (data) {

                     scope.CurrentData = data;
                 }

                 scope.backChart = function () {

                     scope.showlegend = true;
                     var cData = (drillDownCount == 1) ? data : drillDownData[drillDownCount - 2];
                     drillDownCount -= 1;
                     initChart(cData, "", 35);
                     scope.backBtnVisible = (drillDownCount < 1) ? false : scope.backBtnVisible;
                     if (drillDownCount == 0) {
                         scope.callbackFn({ title: "initial" });
                     }
                     if (drillDownCount == 1) {
                         scope.callbackFn({ title: scope.categoriesTitlePrevious });
                     }
                 }

             }
         };
     }]);






