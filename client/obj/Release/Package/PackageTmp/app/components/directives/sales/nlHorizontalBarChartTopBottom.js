MetronicApp.directive("nlHorizontalBarChartTopBottom", ["$location", "$rootScope", "$filter", "$state", "dataService", "HelperService", "NotificationService",
    function ($location, $rootScope, $filter, $state, dataService, HelperService, NotificationService) {
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
                valueAxisTile: "=",
                chartType: "=",
                isGrowth: "=",
                xtitle:"="
            },
            templateUrl: 'app/components/directives/sales/nlHorizontalBarChartTopBottom.html',
          

            link: function (scope, element, attrs, controller) {
                var data;
                scope.gulpindex = 14;
                scope.nlDashboard=attrs.nlDashboard
                scope.state = $state.current.name;
                scope.chartId = "chartId" + (count++);
                //var charttitle = (scope.chartType == 'revenue') ? "SALES" : "TOTAL CUSTOMER BY SALES PERSON";
                scope.title=scope.xtitle;
                var charttitle = scope.title;
                scope.isCasessoldReport = false;
                scope.IsProfitablity = false;
                scope.IsProfitablityByCustomerReport = false;
                scope.IsProfitablityByCustomerReportDetails = false;
                if (scope.valueAxisTile == "Cases Sold") {
                    scope.isCasessoldReport = true;
                }
                scope.callbackFn({
                    title: "initial"
                });
               
                scope.backBtnVisible = false;
                var subChartData = null;
                var drillDownCount = 0;
                var level2Data = null;
                var drillDownChartTitle = "";
                var initChart = function (chartData) {
                    if (drillDownCount == 0) {
                        scope.toggleTopLabel = "Top";
                        scope.toggleBottomLabel = "Bottom";
                    }
                    var sample = chartData;
                    var chart = AmCharts.makeChart(scope.chartId, {
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
                        "rotate": true,
                        "valueAxes": [{
                            "stackType": "regular",
                            "position": "left",
                            "title": (scope.valueAxisTile) ? scope.valueAxisTile : attrs.nlDashboard,
                        },

                        {
                            "id": "v2",
                            "title": "Customer Count",
                            "gridAlpha": 0,
                            "position": "right",
                            "autoGridCount": false
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
                                  var label = (drillDownCount == 0) ? "\n" + graphDataItem.dataContext.salesman : "";
                                  //return "\n" + label;
                                  return "";
                              },
                              "labelRotation": -35

                          },
                          //val1 & val2
                          {

                              "id": "g1",
                              "fillAlphas": 1,
                              "lineAlpha": 0.2,
                              "title": "",
                              "type": "column",
                              "valueField": "value",
                              "fillColorsField": "Color1",
                              "balloonText": " ",
                              "balloonFunction": function BalloonTextPrevious(graphDataItem, graph) {
                                  
                                  var Dollar = "";

                                  if (attrs.chartType.startsWith("sales") || scope.state == "Profitablity") { Dollar = "$" }

                                  Dollar = scope.valueAxisTile == "Cases Sold" ? "" : Dollar;
                                  var value = graphDataItem.dataContext.value;
                                  var salespersonToolTip = "<hr style='padding:0;margin:0'>" + graphDataItem.dataContext.LabelPrior + "<br/>" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.prior)) + " <br /> " + scope.chartTitle + " <br /> " + graphDataItem.dataContext.GroupName.toUpperCase() + "(" + graphDataItem.dataContext.Code + ")" + "";

                                  if (attrs.chartType == "salesCustomer" || attrs.chartType == "casesoldCustomer") {
                                      salespersonToolTip = "<hr style='padding:0;margin:0' >" + graphDataItem.dataContext.LabelPrior + "<br/>" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.prior)) + " <br /> " + scope.chartTitle + " <br /> " + graphDataItem.dataContext.GroupName1.toUpperCase() + "(" + graphDataItem.dataContext.Code + ")" + "";
                                  }
                                  if (scope.title == "Profitablilty By Customer") {
                                      salespersonToolTip = "<hr style='padding:0;margin:0' >" + graphDataItem.dataContext.LabelPrior + "<br/>" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.prior)) + " <br /> " + scope.chartTitle + " <br /> " + graphDataItem.dataContext.GroupName1.toUpperCase() + "(" + graphDataItem.dataContext.Code + ")" + "";
                                  }
                                  if (scope.title == "Profitability By Item") {
                                      var item = "Item"
                                      salespersonToolTip = "<hr style='padding:0;margin:0' >" + graphDataItem.dataContext.LabelPrior + "<br/>" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.prior)) + " <br /> " + item + " <br /> " + graphDataItem.dataContext.GroupName1.toUpperCase() + "(" + graphDataItem.dataContext.Code + ")" + "";
                                  }
                                  scope.catDate = graphDataItem.category;

                                  if (scope.isGrowth) {
                                      if (scope.IsCm01) {
                                          var span = calcpercentage(Math.round(graphDataItem.dataContext.growth));
                                          return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>" + "<span>" + span + "</span>" + " (" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.value)) + ")" + "</b>" + "<br />" + salespersonToolTip;

                                      }
                                      else {

                                      }
                                      var span = calcpercentage(Math.round(graphDataItem.dataContext.value));
                                      return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>" + "<span>" + span + "</span>" + " (" + Dollar + HelperService.formatCommaSeperate(Math.round(graphDataItem.dataContext.growth)) + ")" + "</b>" + "<br />" + salespersonToolTip;
                                  }
                                  else {
                                      var span = calcpercentage(Math.round(graphDataItem.dataContext.growth));
                                      return graphDataItem.dataContext.Label1 + "<br /><b style='font-size: 130%'>" + Dollar + HelperService.formatCommaSeperate(Math.round(value)) + " <span>(" + span + ")</span>" + "</b>" + "<br />" + salespersonToolTip;
                                  }
                              }
                          },



                        ],

                        "plotAreaFillAlphas": 0.1,
                        "depth3D": 10,
                        "angle": 10,
                        "categoryField": (scope.state == "Profitablity") ? "GroupName" : "GroupName",
                        "categoryAxis": {

                            "gridPosition": "start",
                            //"tickPosition": "start",
                            "labelFunction": function (label, item, axis) {
                                var chart = axis.chart;
                                if ((chart.realWidth <= 300) && (label.length > 5))
                                    return label.substr(0, 5) + '...';
                                if ((chart.realWidth <= 500) && (label.length > 10))
                                    return label.substr(0, 10) + '...';
                                return label;
                            },
                            "labelRotation": 50,
                            "ignoreAxisWidth": false,
                            "autoWrap": true
                        },

                        "columnSpacing": 12,
                        "balloon": {
                            "borderThickness": 1,
                            "shadowAlpha": 0
                        },
                        "export": {
                            "enabled": true
                        },

                    });
                    scope.showReport = false;
                    scope.breadcrumbtext = "";
                    debugger;
                    chart.dataProvider = chartData;
                    chart.validateData();
                    scope.cm01drilldownreport = false;
                    chart.addListener('clickGraphItem', function (event) {
                        debugger
                        scope.ItemCode = "";
                        scope.IsProfitablityByCustomerReportDetailsByCustomer = false;
                        if (scope.state == "Profitablity") {

                            if (scope.title == "Profitability By Item") {


                                charttitle = "Profitability By Item" + " (" + event.item.dataContext.Code + ")";
                                $rootScope.TITLEPRE = scope.title + "(" + event.item.dataContext.Code + ")" + " <br/>" + "<span class='small-text'>" + event.item.category;
                                $rootScope.TITLEOLD = $rootScope.TITLEPRE + "</span>" + "<span class='small-text'> >REPORT </span>";
                                if (event.item.dataContext.Code == "CM01") {

                                }
                                else {

                                }
                                scope.callbackFn({
                                    title: $rootScope.TITLEOLD
                                });


                                scope.csvFileName = "PROFITABILITY (BY ITEM).csv";
                                scope.ItemCode = event.item.dataContext.Code;
                                GetCustomerWiseProfitByItem($rootScope.currentfilter.Id, event.item.dataContext.Code);


                                scope.$apply(function () {
                                    scope.showChart = false;
                                    scope.showReport = true;
                                    scope.showToggleButton = false;
                                });
                            }
                            else {

                                charttitle = "Profitability By Customer" + " (" + event.item.dataContext.Code + ")";
                                $rootScope.TITLEPRE = scope.title + "(" + event.item.dataContext.Code + ")" + " <br/>" + "<span class='small-text'>" + event.item.category;
                                $rootScope.TITLEOLD = $rootScope.TITLEPRE + "</span>" + "<span class='small-text'> >REPORT </span>";
                                scope.callbackFn({
                                    title: $rootScope.TITLEOLD
                                });

                                var requestData = {

                                    ItemCode: '',
                                    FilterId: $rootScope.currentfilter.Id,
                                    Commodity: 'All',//scope.currentcommodity,
                                    CustomerCode: event.item.dataContext.Code,

                                }
                                scope.IsProfitablityByCustomerReportDetailsByCustomer = true;
                                scope.CustomerCode = event.item.dataContext.Code;
                                GetProfitByCustomerDetailAndCommodity(requestData);
                                scope.$apply(function () {
                                    scope.showChart = false;
                                    scope.showReport = true;
                                    scope.showToggleButton = false;
                                });
                            }

                            var text = $filter('htmlToPlaintext')($rootScope.TITLEOLD);
                            scope.csvFileName = text;

                            scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                            scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                            scope.csvFileName = scope.csvFileName.replace("_", "-");

                            scope.csvFileName = scope.csvFileName + ".csv";

                            scope.csvFileNameDup = scope.csvFileName;

                        }
                        else {
                            var orderBy = "";
                            if (scope.valueAxisTile == "Cases Sold") {
                                scope.isCasessoldReport = true;
                                orderBy = "casessold";
                            }
                            else {
                                scope.isCasessoldReport = false;
                                orderBy = "sales";
                            }
                            var requestData = {

                                SalesPerson:(event.item.dataContext.Code == "CM01") ?event.item.dataContext.GroupName:event.item.dataContext.Code,
                                FilterId: $rootScope.currentfilter.Id,
                                Period: 0,
                                Commodity: scope.currentcommodity,
                                Category: scope.chartTitle,
                                OrderBy: orderBy
                            }
                            scope.GlobalFilterModel = requestData;
                            if (requestData.Category == "Customer") {
                                charttitle = "Items Sold to Customer" + " (" + event.item.dataContext.Code + ")";
                                $rootScope.TITLEPRE = scope.title + "(" + event.item.dataContext.Code + ")" + " <br/>" + "<span class='small-text'>" + event.item.category;
                                $rootScope.TITLEOLD = $rootScope.TITLEPRE + "</span>" + "<span class='small-text'> >REPORT </span>";
                                scope.callbackFn({
                                    title: charttitle
                                });
                            }
                            else {
                                var isreport = (event.item.dataContext.Code == "CM01") ? "" : ">REPORT";
                                charttitle = scope.title + "(" + event.item.dataContext.Code + ")";
                              
                                
                                if (scope.drillDownData) {
                                    scope.cm01drilldownreport = true;
                                    scope.sc01drilldown = $rootScope.TITLEOLD+ "</span>" + "<span class='small-text'> >"+event.item.category+ ">Report</span>";
                                    scope.callbackFn({
                                        title: scope.sc01drilldown
                                    });
                                }
                                else {
                                    $rootScope.TITLEPRE = charttitle + " <br/>" + "<span class='small-text'>" + event.item.category;
                                    $rootScope.TITLEOLD = $rootScope.TITLEPRE + "</span>" + "<span class='small-text'> " + isreport + " </span>";
                                    scope.callbackFn({
                                        title: $rootScope.TITLEOLD
                                    });
                                }
                                
                            }
                            //var text = $filter('htmlToPlaintext')($rootScope.TITLEOLD);
                            //scope.csvFileName = text;
                            //scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                            //scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                            //scope.csvFileName = scope.csvFileName.replace("_", "-");
                            //scope.csvFileName = scope.csvFileName + ".csv";
                            //scope.csvFileNameDup = scope.csvFileName;
                            if (requestData.Category == "Customer") {
                                GetInvoiceDetailsByCustomer(requestData);
                                scope.IsCustomerWiseInvoiceReport = true;
                                scope.$apply(function () {
                                    scope.showChart = false;
                                    scope.showReport = true;
                                });
                                scope.$apply(function () {
                                    scope.showToggleButton = false;
                                });
                            }
                            else {
                                debugger
                                if (event.item.dataContext.Code == "CM01") {
                                    scope.IsCm01 = true;
                                    if (scope.drillDownData) {
                                        getCustomersCaseSoldDataReport(requestData,"CM01")
                                    }
                                    else {
                                        var state = (attrs.nlDashboard == "sales") ? false : true;
 
                                        GetCasesSoldAndGrowthBySalesPersonCustomerService(requestData.FilterId, state);
                                    }
                                   
                                }
                                else {
                                    getCustomersCaseSoldDataReport(requestData,"")
                                }


                                //(event.item.dataContext.Code == "CM01") ? GetCasesSoldAndGrowthBySalesPersonCustomerService(requestData.FilterId, attrs.nlDashboard) :
                                //                  (scope.drillDownData) ? getCustomersCaseSoldDataReport(requestData) :
                                //getCustomersCaseSoldDataReport(requestData);
                                scope.$apply(function () {
                                    scope.showChart = (event.item.dataContext.Code == "CM01") ? true : false;
                                    scope.showReport = (event.item.dataContext.Code == "CM01") ? false : true;
                                });
                                scope.$apply(function () {
                                    scope.showToggleButton = (event.item.dataContext.Code == "CM01") ? true : false;
                                });
                            }
                        }
                    });
                };

                function GetCasesSoldAndGrowthBySalesPersonCustomerService(filterId, state) {
                    
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    drillDownCount = drillDownCount + 1;
                    Metronic.blockUI({ boxed: true });
                    dataService.GetCasesSoldAndGrowthBySalesPersonCustomerService(filterId, state).then(function (response) {
                      
                        if (response && response.data) {
                            scope.drillDownData = response.data.SalesPerson;
                           
                            initChart((scope.topBottomToggle) ? scope.drillDownData.Top : scope.drillDownData.Bottom);
                            //scope.showToggleButton = true;
                            //scope.showChart = true;
                            //scope.showReport = false;

                            //scope.$apply(function () {
                                scope.showChart = true;
                                scope.showReport = false;
                            //});
                            //scope.$apply(function () {
                                scope.showToggleButton = true;
                            //});



                            //(attrs.nlDashboard == "profitability") ? initChart(response.data.TotalRevenue.All[0].SubData, "", 50) :
                            //(attrs.nlDashboard == "sales") ? initChart(response.data.TotalCasesSold.All[0].SubData, "", 50) :
                            //                                 initChart(response.data.TotalCasesSold.All[0].SubData, "", 50);

                        }
                        Metronic.unblockUI();

                    }, function onError() {

                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };

                scope.GetProfitByCustomerDetails = function (CustomerNumber) {

                    charttitle = "Items Sold to Customer" + "<span class=''> (" + CustomerNumber + ") </span>"
                 //   scope.chartitemtitle = "Items Sold to Customer" + "<span class=''> (" + CustomerNumber + ") </span>";

                    scope.callbackFn({
                        title: "Items Sold to Customer" + "<span class=''> (" + CustomerNumber + ") </span>"
                    });

                    var text = $filter('htmlToPlaintext')(charttitle);
                    scope.csvFileName = text;
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");

                    scope.csvFileName = scope.csvFileName + ".csv";

                    scope.csvFileNameDup = scope.csvFileName;
                    //var a = scope.csvFileName.split('.');
                  //  scope.csvFileName = a[0] + " >" + CustomerNumber.trim() + ".csv";

                    scope.CustomerCode = CustomerNumber
                    scope.IsProfitablityByCustomerReportDetails = true;
                    scope.IsProfitablityByCustomerReport = false;

                  //  scope.showChart = false;
                   // scope.showReport = true;
                   // scope.showToggleButton = false;


                    Metronic.blockUI({ boxed: true });
                    var itemCode = "";
                    if (scope.ItemCode == undefined || scope.ItemCode != "")
                    {
                        itemCode = scope.ItemCode;


                    }

                    var requestData = {

                        ItemCode: itemCode,
                        FilterId: $rootScope.currentfilter.Id,
                        Commodity: scope.currentcommodity,
                        CustomerCode: CustomerNumber
                       
                    }

                    

                    GetProfitByCustomerDetailAndCommodity(requestData);


                

                }
                    
                    

                scope.getDetailedReport = function (CustomerNumber, customerName) {
                    
                    //scope.callbackFn({
                    //    title: $rootScope.TITLEOLD + "<span class='small-text'> >" + customerName + " </span>"
                    //});

                    scope.callbackFn({
                        title: "Items Sold to Customer" + "<span class=''> (" + CustomerNumber + ") </span>"
                    });
                    scope.chartitemtitle = "Items Sold to Customer" + "<span class=''> (" + CustomerNumber + ") </span>";
                    
                    scope.TotalQuantity = 0;
                    scope.TotalExtPrice = 0;
                    scope.customerName = customerName;
                    scope.GlobalFilterModel["CustomerNumber"] = CustomerNumber;
                    if (scope.state == "sales") { scope.GlobalFilterModel["OrderBy"] = "sales"; }
                    else if (scope.state == "casessold") { scope.GlobalFilterModel["OrderBy"] = "casessold"; }
                    
                    var a = scope.csvFileName.split('.');



                    scope.csvFileName = a[0] + " >" + customerName.trim() + ".csv";

                    Metronic.blockUI({ boxed: true });
                    dataService.GetCasesSoldDetails(scope.GlobalFilterModel,scope.IsCm01).then(function (response) {
                        if (response && response.data) {

                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            scope.iscustomersItemReport = true;
                            scope.IsCustomerWiseInvoiceReport = false;


                            for (var i = 0; i < response.data.length; i++) {
                                scope.TotalQuantity = scope.TotalQuantity + response.data[i].Quantity;
                                scope.TotalExtPrice = scope.TotalExtPrice + response.data[i].ExtPrice;
                            }

                            scope.bk_customersItemData = angular.copy(response.data);
                            scope.bk_customersItemDataSafe = angular.copy(response.data);

                            if (scope.iscustomersItemReport) {
                                if (scope.currentcommodity == 'All') {
                                    scope.customersItemData = scope.bk_customersItemData;
                                    scope.customersItemDataSafe = scope.bk_customersItemDataSafe;

                                } else {
                                    scope.TotalQuantity = 0;
                                    scope.TotalExtPrice = 0;
                                    scope.customersItemData = [];
                                    scope.customersItemDataSafe = [];

                                    for (var i = 0; i < scope.bk_customersItemData.length; i++) {

                                        if (scope.bk_customersItemData[i].Comodity.trim() == scope.currentcommodity) {
                                            scope.customersItemData.push(scope.bk_customersItemData[i]);
                                            scope.customersItemDataSafe.push(scope.bk_customersItemData[i]);
                                            scope.TotalQuantity = scope.TotalQuantity + scope.bk_customersItemData[i].Quantity;
                                            scope.TotalExtPrice = scope.TotalExtPrice + scope.bk_customersItemData[i].ExtPrice;
                                        }
                                        else {

                                        }
                                    }

                                }

                            }

                            scope.TotalQuantity = Math.round(scope.TotalQuantity);
                            scope.TotalExtPrice = Math.round(scope.TotalExtPrice);
                        }
                        Metronic.unblockUI();

                    }, function onError() {

                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                        NotificationService.ConsoleLog('Error on the server');
                    });

                }

                scope.getCsvHeader = function () {

                   
                    //if (scope.isCasessoldReport) {
                    //    return ["Customer Name", "Sales Amt Current", "Cases Sold Prior", "Cases Sold Current", "Difference", "Difference (%)"];
                    //}
                    //else 
                    if (scope.valueAxisTile == 'Revenue') {
                        return ["Customer Name", "Revenue Amt Prior", "Sales Qty", "Revenue Amt Current", "Difference", "Difference (%)"];
                    }
                    else
                        return ["Customer Name", "Sales Amt Prior", "Sales Qty", "Sales Amt Current", "Difference", "Difference (%)"];
                }


                //csv download - Headers.
                scope.getCasesSoldBySalesPersonCsvDataHeader = function () {

                    var text = $filter('htmlToPlaintext')($rootScope.TITLEOLD);
                    scope.csvFileName = text;
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName + ".csv";

                    return ["Customer Name", "Sales Amt Current", "Cases Sold Prior", "Cases Sold Current", "Difference", "Difference (%)"];
                }

                scope.getCasesSoldToCustomerCsvHeader = function () {
                    return ["Commodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "Sales Person", "SO #", "Cases Sold", "Sales Amount", ];
                }

                scope.getCsvDataForSalesBySalesPersonHeader = function () {
                    var text = $filter('htmlToPlaintext')($rootScope.TITLEOLD);
                    scope.csvFileName = text;
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName + ".csv";

                    return ["Customer Name", "Cases Sold Current", "Sales Amt Prior", "Sales Amt Current", "Difference", "Difference (%)"];
                };

                scope.getCsvDataItemSoldToCustomerHeader = function () {
                    debugger;
                    var text = $filter('htmlToPlaintext')(scope.chartitemtitle);
                    scope.csvFileName = text;
                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName + ".csv";

                    return ["Comodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "SO #", "Cases Sold", "Sales Amount", ];
                };


                scope.getProfitabilityByItemCsvDataHeader = function () {


                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName.toUpperCase();

                    return ["Company Code", "Company Name", "Historical Profit", "Current Pofit", "Difference", "Difference (%)"];
                };

                scope.getProfitabilityByCustomerDetailsCsvDataHeader = function () {


                    scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                    scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                    scope.csvFileName = scope.csvFileName.replace("_", "-");
                    scope.csvFileName = scope.csvFileName.toUpperCase();

                    return ["Commodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "Salesman Code", "Salesman Description", "SO #", "Cost", "Cases Sold", "Ext Price", "Profit"];
                };


                scope.getProfitabilityByCustomerDetailsCsvData = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    var ProfitTotal = 0;

                    predefinedHeader = ["Commodity", "InvoiceDate", "InvoiceNumber", "ItemCode", "ItemDescription", "SalesManCode", "SalesManDescription", "SalesOrder", "Cost", "QtyShip", "ExtPrice", "Profit"];

                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                if (key == "ExtPrice" || key == "Cost" || key == "Profit") {
                                    header[index] = "$" + Math.round(value);
                                }
                                else if (key == "QtyShip" || key == "Profit") {
                                    header[index] = $filter('numberWithCommas')(Math.round(value));
                                }
                                else {
                                    header[index] = value;
                                }
                            }
                            if (key == "QtyShip") {
                                Quantity = Quantity + value;
                            }
                            if (key == "ExtPrice") {
                                ExtPrice = ExtPrice + value;
                            }
                            if (key == "Profit") {
                                ProfitTotal = ProfitTotal + value;
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });

                    var total = {};
                    total[0] = "Total";
                    total[1] = "";
                    total[2] = "";
                    total[3] = "";
                    total[4] = "";

                    total[5] = "";
                    total[6] = "";
                    total[7] = "";
                    total[8] = "";
                    total[9] =  $filter('numberWithCommas')(Math.round(Quantity));
                    total[10] = $filter('currency')(Math.round(ExtPrice));
                    total[11] = $filter('currency')(Math.round(ProfitTotal));


                    array.push(total);
                    return array;
                };


                
                scope.getProfitabilityByItemCsvData = function () {

                    var array = [];
                    var predefinedHeader = [];

                    var salesAmtCurrent = 0;
                    var casesSoldCurrent = 0;
                    var salesAmtPrior = 0;
                    predefinedHeader = ["CompanyCode", "CompanyName", "PriorProfit", "CurrentProfit", "DifferenceProfit", "GrowthProfit"];

                    angular.forEach(scope.profitItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {

                                if (key == "PriorProfit" || key == "CurrentProfit") {
                                    header[index] = "$" + $filter('numberWithCommas')(Math.round(value));
                                }
                                else if (key == "DifferenceProfit" || key == "GrowthProfit") {
                                    header[index] = Math.round(value);
                                }
                                else {
                                    header[index] = value;
                                }

                            }

                            if (key == "CurrentProfit") {
                                salesAmtCurrent = salesAmtCurrent + value;
                            }
                            if (key == "PriorProfit") {
                                salesAmtPrior = salesAmtPrior + value;
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {};
                    total[0] = "Total";
                    total[1] = "";
                    total[2] = "$" + $filter('numberWithCommas')(Math.round(salesAmtPrior));
                    total[3] = $filter('numberWithCommas')(Math.round(salesAmtCurrent));
                    array.push(total);
                    return array;
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

                scope.getCasesSoldToCustomerCsv = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    predefinedHeader = ["Commodity", "InvoiceDate", "InvoiceNumber", "ItemCode", "ItemDescription", "SalesmanDescription", "SalesOrderNumber", "CasesSold", "Sales"];
                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                if (key == "Sales") {
                                    header[index] = "$" + Math.round(value);
                                }
                                else if (key == "CasesSold") {
                                    header[index] = $filter('numberWithCommas')(Math.round(value));
                                }
                                else {
                                    header[index] = value;
                                }
                            }
                            if (key == "CasesSold") {
                                Quantity = Quantity + value;
                            }
                            if (key == "Sales") {
                                ExtPrice = ExtPrice + value;
                            }
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {};
                    total[0] = "Total";
                    total[1] = "";
                    total[2] = "";
                    total[3] = "";
                    total[4] = "";
                    total[5] = "";
                    total[6] = "";

                    total[7] = $filter('numberWithCommas')(round(Quantity));
                    total[8] = $filter('currency')(round(ExtPrice));



                    array.push(total);
                    return array;
                };
               
                scope.getCsvDataItemSoldToCustomer = function () {
                  //  return ["Commodity", "Invoice Date", "Invoice #", "Item Code", "Item Description", "SO #", "Cases Sold", "Sales Amount", ];
                    debugger;
                    var array = [];
                    var predefinedHeader1 = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    predefinedHeader1 = ["Comodity", "InvoiceDate", "InvoiceNumber", "Item", "ItemDesc", "Sono", "Quantity", "ExtPrice"];
                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader1.indexOf(key);
                            if (index > -1) {
                              
                                if (key == "ExtPrice") {
                                    header[index] = "$" +$filter('numberWithCommas')(value);
                                }
                                else if (key == "InvoiceDate") {
                                    header[index] = $filter('date')(value);
                                }
                               else if (key == "Quantity") {
                                    header[index] = $filter('numberWithCommas')(value);
                                }
                                else {
                                    header[index] = value;
                                }
                            }
                            //if (key == "Quantity") {
                            //    Quantity = Quantity + value;
                            //}
                            //if (key == "ExtPrice") {
                            //    ExtPrice = ExtPrice + value;
                            //}
                        });
                        if (header != undefined)
                            array.push(header);
                    });
                    var total = {};
                    total[0] = "Total";
                    total[1] = "";
                    total[2] = "";
                    total[3] = "";
                    total[4] = "";
                    total[5] = "";
                    total[6] = $filter('numberWithCommas')(Math.round(scope.TotalQuantity));
                    total[7] = "$" + $filter('numberWithCommas')(Math.round(scope.TotalExtPrice));
                    array.push(total);
                    return array;
                };

                scope.getCsvDataForSalesBySalesPerson = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var salesAmtCurrent = 0;
                    var casesSoldCurrent = 0;
                    var salesAmtPrior = 0;
                    predefinedHeader = ["Customer", "CasesSoldCurrent", "SalesAmountPrior", "SalesAmountCurrent", "Difference", "Percentage"];

                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                if (key == "CasesSoldCurrent") {
                                    header[index] = $filter('numberWithCommas')(Math.round(value));
                                }
                                else if (key == "SalesAmountPrior" || key == "SalesAmountCurrent")
                                {
                                    header[index] = "$" + $filter('numberWithCommas')(Math.round(value));
                                }
                                else if (key == "Difference" || key == "Percentage") {
                                    header[index] = Math.round(value);
                                }
                                else {
                                    header[index] = value;
                                }

                            }

                            if (key == "SalesAmountCurrent") {
                                salesAmtCurrent = salesAmtCurrent + value;
                            }
                            if (key == "SalesAmountPrior") {
                                salesAmtPrior = salesAmtPrior + value;
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
                    total[1] = $filter('numberWithCommas')(Math.round(casesSoldCurrent));
                    total[2] = "$" + $filter('numberWithCommas')(Math.round(salesAmtPrior));
                    total[3] = "$" + $filter('numberWithCommas')(Math.round(salesAmtCurrent));
                    array.push(total);
                    return array;
                };


                scope.getCsvData = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var value = 0;
                    var diff = 0;
                    var prior = 0;
                    var qty = 0;
                    predefinedHeader = ["Customer", "SalesAmountPrior", "SalesQty", "SalesAmountCurrent", "Difference", "Percentage"];

                    angular.forEach(scope.customersDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {

                                if (key == "SalesAmountCurrent" && !scope.isCasessoldReport) {
                                    header[index] = "$" + value;
                                }
                                else if (key == "Difference" && !scope.isCasessoldReport) {
                                    header[index] = "$" + value;
                                }
                                else if (key == "SalesAmountPrior" && !scope.isCasessoldReport) {
                                    header[index] = "$" + value;
                                }
                                else if (key == "SalesQty") {
                                    header[index] = $filter('numberWithCommas')(value);
                                }
                                else {
                                    header[index] = value;
                                }

                            }

                            if (key == "SalesAmountCurrent") {
                                value = value + value;
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
                    total[1] = scope.isCasessoldReport == true ? prior : "$" + prior;
                    total[2] = $filter('numberWithCommas')(qty);
                    total[3] = scope.isCasessoldReport == true ? value : "$" + value;
                    total[4] = '';// isCasessoldReport == true ? diff : "$" + diff;
                    total[5] = "";
                    array.push(total);
                    return array;
                };
                scope.getCsvHeaderDrillDown = function () {
                    return ["Commodity", "Invoice Date", "Invoice #", "Item", "Item Description", "Quantity", "Ext Price", "Sales Person", "SO #"];
                };
                scope.getCsvDataDrillDown = function () {
                    var array = [];
                    var predefinedHeader = [];
                    var Quantity = 0;
                    var ExtPrice = 0;
                    predefinedHeader = ["Commodity", "InvoiceDate", "InvoiceNumber", "ItemCode", "ItemDescription", "CasesSold", "Sales", "SalesmanDescription", "SalesOrderNumber"];

                    angular.forEach(scope.customersItemDataSafe, function (value, key) {
                        var header = {};
                        angular.forEach(value, function (value, key) {
                            var index = predefinedHeader.indexOf(key);
                            if (index > -1) {
                                if (key == "ExtPrice") {
                                    header[index] = "$" + value;
                                }
                                else if (key == "Quantity") {
                                    header[index] = $filter('numberWithCommas')(value);
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
                    var total = {};
                    total[0] = "Total";
                    total[1] = "";
                    total[2] = "";
                    total[3] = "";
                    total[4] = "";
                    total[5] = $filter('numberWithCommas')(Quantity);
                    total[6] = $filter('currency')(ExtPrice);
                    total[7] = "";
                    total[8] = "";
                    array.push(total);
                    return array;
                };
                scope.currentcommodity = "All";
                scope.FilterCommodity = function (commodity) {

                    scope.currentcommodity = commodity;
                    scope.GlobalFilterModel["Commodity"] = commodity;
debugger

                    if (scope.IsCm01) {
                        //  GetCasesSoldAndGrowthBySalesPersonCustomerService(requestData)
                        getCustomersCaseSoldDataReport(scope.GlobalFilterModel, "CM01");
                    }
                    else {
                    if (scope.isCasessoldReport) {
                        getCustomersCaseSoldDataReport(scope.GlobalFilterModel,"");
                    }
                    else
                        getCustomersSalesDataReport(scope.GlobalFilterModel,"");

                    }





                  

                };


                scope.FilterProfitCustomerCommodity = function (commodity) {

                    scope.currentcommodity = commodity;

                 //   scope.GlobalFilterModel["Commodity"] = commodity;
                    var itemCode = (scope.ItemCode != undefined || scope.ItemCode != "") ?  scope.ItemCode : "";
                    //if (scope.ItemCode != undefined || scope.ItemCode != "")
                    //{
                    //    itemCode =
                    //}
                    var requestData = {
                        ItemCode: itemCode,
                        FilterId: $rootScope.currentfilter.Id,
                        Commodity: scope.currentcommodity,
                        CustomerCode: scope.CustomerCode
                    };
                        GetProfitByCustomerDetailAndCommodity(requestData);
                };

                scope.FilterCustomerCommodity = function (commodity) {
                    scope.currentcommodity = commodity;
                    scope.GlobalFilterModel["Commodity"] = commodity;
                    GetInvoiceDetailsByCustomer(scope.GlobalFilterModel)
                };
                scope.back = function () {
                    debugger;
                    initChart((scope.topBottomToggle) ? scope.initvalues.Top : scope.initvalues.Bottom);
                    scope.drillDownData = null;
                    scope.callbackFn({
                        title: scope.xtitle
                    });

                };
                scope.$watch(function () { return controller.$viewValue }, function (newVal, oldVal) {
                    debugger;
                    data = newVal;
                   
                    if (scope.showReport) {
                        scope.showChart = true;
                        scope.showReport = false;
                        scope.callbackFn({title: "initial"});
                    }

                    if (data == undefined || data.length == 0) {
                        if (data == undefined) {
                          //  Metronic.blockUI({boxed: true});
                            scope.showToggleButton = false;
                        } else {
                          //  Metronic.unblockUI();
                            scope.showToggleButton = true;
                        }
                    } else {
                        scope.initvalues = newVal;
                        initChart((scope.topBottomToggle) ? newVal.Top : newVal.Bottom);
                        scope.showToggleButton = true;
                    }
                });
                scope.$watch("topBottomToggle", function (newVal, oldVal) {
                     
                    if (scope.initvalues) {
                        //var drawchartData = (drillDownCount == 1) ? ((scope.topBottomToggle) ? level2Data.Top : level2Data.Bottom) :
                        //    ((drillDownCount == 2) ? ((scope.topBottomToggle) ? subChartData.Top : subChartData.Bottom) :
                        //        ((scope.topBottomToggle) ? data.Top : data.Bottom));
                        //drawchartData ? initChart(drawchartData) : void (0);
                        var drawchartData = null;
                        if (drillDownCount == 0) {
                            drawchartData = (scope.topBottomToggle) ? scope.initvalues.Top : scope.initvalues.Bottom;
                            scope.drawchartData = drawchartData;
                            
                        }
                        if (drillDownCount == 1) {
                            var data = (scope.drillDownData) ? scope.drillDownData : scope.initvalues;
                            drawchartData = (scope.topBottomToggle) ? data.Top : data.Bottom;
                        }

                        initChart(drawchartData)

                    }
                });

                scope.$watch("backBtnVisible", function (newVal, oldVal) {
                    debugger
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

                    }
                });
                scope.displayDollar = function () {
                    return scope.chartType == 'totalSalesBySalesPerson' ? "$" : "";
                };
                var calcpercentage = function (growth) {

                    return     (growth > 0) ? "<span class='fa fa-sort-up' style='color:forestgreen'> " + (growth) + " %" + "</span>" :
                               (growth < 0) ? "<span class='fa fa-sort-down' style='color:red'> " + (growth) + " %" + "</span>" :
                               (growth == 0) ? "<span class='fa' style='color:blue'> " + (growth) + "%" + "</span>" : "";
                    //   if (growth > 0) {
                    //       return span = "<span class='fa fa-sort-up' style='color:forestgreen'> " + (growth) + " %" + "</span>";
                    //}
                    //   if (growth < 0) {
                    //       return span = "<span class='fa fa-sort-down' style='color:red'> " +(growth) + " %" + "</span>";
                    //}
                    //   if (growth == 0) {
                    //       return span = "<span class='fa' style='color:blue'> " + (growth) + "%" + "</span>";
                    //}
                }
                function getCustomersCaseSoldDataReport(salesPerson,IsCm01) {

                   
                   
                    var text = $filter('htmlToPlaintext')(scope.sc01drilldown);
                    scope.csvFileName = (text + ".csv").toUpperCase();


                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({boxed: true});
                    dataService.GetCustomerAndCasessoldReport(salesPerson, IsCm01).then(function (response) {
                        if (response && response.data) {
                            scope.groupProperty = '';
                            scope.customersData = response.data;
                            scope.customersDataSafe = response.data;
                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            scope.TotalCasesSoldCurrent = 0;
                            scope.TotalSalesAmtPrior = 0;
                            scope.TotalSalesQuantity = 0;
                            scope.TotalSalesAmtCurrent = 0;
                            scope.TotalDifference = 0;
                            scope.TotalCasesSoldPrior = 0;
                            scope.TotalCasesSoldQty = 0;

                            //scope.$apply(function () {
                                scope.showChart =  false;
                                scope.showReport =  true;
                            //});
                            //scope.$apply(function () {
                                scope.showToggleButton =  false;
                            //});


                            if (scope.valueAxisTile == "Cases Sold") {
                                var length = scope.customersData.length;
                                for (var i = 0; i < length; i++) {
                                    scope.TotalCasesSoldPrior = scope.TotalCasesSoldPrior + scope.customersData[i].CasesSoldPrior;
                                    scope.TotalCasesSoldQty = scope.TotalCasesSoldQty + scope.customersData[i].CasesSoldCurrent;
                                    scope.TotalSalesAmtCurrent = scope.TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                    scope.TotalDifference = scope.TotalDifference + scope.customersData[i].DifferenceCasesSold;
                                }
                                scope.TotalCasesSoldPrior = Math.round(scope.TotalCasesSoldPrior);
                                scope.TotalCasesSoldQty = Math.round(scope.TotalCasesSoldQty);
                                scope.TotalSalesAmtCurrent = Math.round(scope.TotalSalesAmtCurrent);
                                scope.TotalDifference = Math.round(scope.TotalDifference);
                            }
                            else {
                                var length = scope.customersData.length;
                                for (var i = 0; i < length; i++) {
                                    scope.TotalCasesSoldPrior = scope.TotalCasesSoldPrior + scope.customersData[i].CasesSoldPrior;
                                    scope.TotalCasesSoldQty = scope.TotalCasesSoldQty + scope.customersData[i].CasesSoldCurrent;
                                    scope.TotalSalesAmtCurrent = scope.TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                    scope.TotalDifference = scope.TotalDifference + scope.customersData[i].DifferenceCasesSold;
                                    scope.TotalSalesAmtPrior = scope.TotalSalesAmtPrior + scope.customersData[i].SalesAmountPrior;
                                }
                                scope.TotalCasesSoldPrior = Math.round(scope.TotalCasesSoldPrior);
                                scope.TotalCasesSoldQty = Math.round(scope.TotalCasesSoldQty);
                                scope.TotalSalesAmtCurrent = Math.round(scope.TotalSalesAmtCurrent);
                                scope.TotalDifference = Math.round(scope.TotalDifference);
                                scope.TotalSalesAmtPrior = Math.round(scope.TotalSalesAmtPrior);
                            }
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                }

                function getCustomersSalesDataReport(salesPerson) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({ boxed: true });
                    dataService.GetSalesPersonCustomersDataTotal(salesPerson).then(function (response) {
                        if (response && response.data) {
                            scope.groupProperty = '';
                            scope.customersData = response.data;
                            scope.customersDataSafe = response.data;
                            scope.TotalCasesSoldPrior = 0;
                            scope.TotalCasesSoldQty = 0;
                            scope.TotalSalesAmtPrior = 0;
                            scope.TotalSalesQuantity = 0;
                            scope.TotalSalesAmtCurrent = 0;
                            scope.TotalDifference = 0;
                            var length = scope.customersData.length;
                            for (var i = 0; i < length; i++) {
                                scope.TotalCasesSoldPrior = scope.TotalCasesSoldPrior + scope.customersData[i].CasesSoldPrior;
                                scope.TotalCasesSoldQty = scope.TotalCasesSoldQty + scope.customersData[i].CasesSoldCurrent;
                                scope.TotalSalesAmtCurrent = scope.TotalSalesAmtCurrent + scope.customersData[i].SalesAmountCurrent;
                                scope.TotalDifference = scope.TotalDifference + scope.customersData[i].DifferenceCasesSold;
                            }
                            scope.TotalCasesSoldPrior = Math.round(scope.TotalCasesSoldPrior);
                            scope.TotalCasesSoldQty = Math.round(scope.TotalCasesSoldQty);
                            scope.TotalSalesAmtCurrent = Math.round(scope.TotalSalesAmtCurrent);
                            scope.TotalDifference = Math.round(scope.TotalDifference);
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };

                function round(v) {
                    return (v >= 0 || -1) * Math.round(Math.abs(v));
                }

                function GetInvoiceDetailsByCustomer(salesPerson) {
                    scope.customersData = [];
                    scope.customersDataSafe = [];
                    Metronic.blockUI({ boxed: true });
                    dataService.GetCustomerAndCasessoldReport(salesPerson).then(function (response) {
                        if (response && response.data) {
                            scope.groupProperty = '';
                            scope.IsCustomerWiseInvoiceReport = true;
                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            scope.TotalQuantity = 0;
                            scope.TotalExtPrice = 0;
                            var length = scope.customersItemData.length;
                            for (var i = 0; i < length; i++) {
                                scope.TotalQuantity = scope.TotalQuantity + scope.customersItemData[i].CasesSold;
                                scope.TotalExtPrice = scope.TotalExtPrice + scope.customersItemData[i].Sales;
                            }

                            scope.TotalQuantity = round(scope.TotalQuantity);
                            scope.TotalExtPrice = round(scope.TotalExtPrice);
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };

                function GetCustomerWiseProfitByItem(filterId, itemCode) {
                    scope.profitItemData = [];
                    scope.profitItemDataSafe = [];
                    Metronic.blockUI({ boxed: true });
                    dataService.GetCustomerWiseProfitByItem(filterId, itemCode).then(function (response) {
                        if (response && response.data) {
                            scope.groupProperty = '';
                            scope.IsProfitablity = true;
                            scope.IsProfitablityByCustomerReport = true;
                            scope.IsProfitablityByCustomerReportDetails = false;
                            scope.profitItemData = response.data;
                            scope.profitItemDataSafe = response.data;
                            scope.TotalPriorProfit = 0;
                            scope.TotalCurrentProfit = 0;
                            var length = scope.profitItemData.length;
                            for (var i = 0; i < length; i++) {
                                scope.TotalPriorProfit = scope.TotalPriorProfit + scope.profitItemDataSafe[i].PriorProfit;
                                scope.TotalCurrentProfit = scope.TotalCurrentProfit + scope.profitItemDataSafe[i].CurrentProfit;
                            }
                            scope.TotalPriorProfit = Math.round(scope.TotalPriorProfit);
                            scope.TotalCurrentProfit = Math.round(scope.TotalCurrentProfit);
                        }
                        Metronic.unblockUI();
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };

                scope.reportback = function () {
                    debugger;
                    var title = "";
                    if (scope.IsProfitablityByCustomerReportDetailsByCustomer) {
                        scope.showChart = true;
                        scope.showReport = false;
                        scope.IsProfitablityByCustomerReport = false;
                        scope.IsProfitablityByCustomerReportDetails = false;
                        scope.IsProfitablityByCustomerReportDetailsByCustomer = false;
                        scope.showToggleButton = true;
                        scope.callbackFn({ title: "PROFITABLILTY BY CUSTOMER" });
                        scope.csvFileName = scope.csvFileNameDup; //= scope.csvFileName;
                    }
                    else if (scope.IsProfitablityByCustomerReportDetails) {
                        scope.showChart = false;
                        scope.showReport = true;
                        scope.IsProfitablityByCustomerReport = true;
                        scope.IsProfitablityByCustomerReportDetails = false;
                        scope.IsProfitablityByCustomerReportDetailsByCustomer = false;
                        scope.callbackFn({ title: $rootScope.TITLEOLD });
                        var text = $filter('htmlToPlaintext')($rootScope.TITLEOLD);
                        scope.csvFileName = text;
                        scope.csvFileName = scope.csvFileName.replace(/_/g, '-');
                        scope.csvFileName = scope.csvFileName.replace(/>/g, "-");
                        scope.csvFileName = scope.csvFileName.replace("_", "-");
                        scope.csvFileName = scope.csvFileName + ".csv";
                        scope.csvFileNameDup = scope.csvFileName;
                    }
                    else if (scope.iscustomersItemReport) {
                        scope.iscustomersItemReport = false;
                        if (!scope.isCasessoldReport)
                            scope.showReport = true;
                        else
                            scope.isCasessoldReport = true;
                        scope.showChart = false;
                        scope.callbackFn({ title: $rootScope.TITLEOLD });
                       // scope.csvFileName = scope.csvFileNameDup; //= scope.csvFileName;
                    }
                    else {
                        if (scope.IsCm01) {
                            scope.callbackFn({ title: $rootScope.TITLEOLD });
                        }
                        else {
                            scope.callbackFn({ title:"initial" });
                        }
                        
                        function timerComplete() { scope.$apply(function () { scope.showChart = true; scope.showReport = false; scope.showToggleButton = true; }); };
                        setTimeout(timerComplete, 200);
                    }
                };
                function getChartTitle(drillDownCountNo, drilldownChartTitle) {
                    var chartTitle = "";
                    if (scope.chartType == 'totalSalesBySalesPerson') {
                        chartTitle = (drillDownCountNo == 0) ? "sales person" :
                            ((drillDownCountNo == 1) ? drilldownChartTitle + " per customer" : drilldownChartTitle + " per customer per type");
                    } else if (scope.chartType == 'totalCustomersBySalesPerson') {
                        chartTitle = (drillDownCountNo == 0) ? "sales person" :
                            ((drillDownCountNo == 1) ? drilldownChartTitle + "" : drilldownChartTitle + "");
                    } else {
                        chartTitle = (drillDownCountNo == 0) ? "sales person" :
                            ((drillDownCountNo == 1) ? drilldownChartTitle + " per customer" : drilldownChartTitle + " per type");
                    }
                    return chartTitle;
                };

                function GetProfitByCustomerDetailAndCommodity(requestData) {
                    Metronic.blockUI({ boxed: true });
                    scope.IsProfitablity = true;
                    scope.IsProfitablityByCustomerReportDetails = true;
                    scope.IsProfitablityByCustomerReport = false;
                    scope.TotalCost = 0;
                    scope.TotalCasesSold = 0;
                    scope.TotalExtPrice = 0;
                    scope.TotalProfit = 0;
                    dataService.GetProfitByCustomerDetailAndCommodity(requestData).then(function (response) {
                        if (response && response.data) {
                            scope.groupProperty = '';
                            scope.customersItemData = response.data;
                            scope.customersItemDataSafe = response.data;
                            for (var i = 0; i < response.data.length; i++) {
                                scope.TotalCost = scope.TotalCost + response.data[i].Cost;
                                scope.TotalCasesSold = scope.TotalCasesSold + response.data[i].QtyShip;
                                scope.TotalExtPrice = scope.TotalExtPrice + response.data[i].ExtPrice;
                                scope.TotalProfit = scope.TotalProfit + response.data[i].Profit;
                            }
                            scope.TotalCost = Math.round(scope.TotalCost);
                            scope.TotalCasesSold = Math.round(scope.TotalCasesSold);
                            scope.TotalExtPrice = Math.round(scope.TotalExtPrice);
                            scope.TotalProfit = Math.round(scope.TotalProfit);
                            Metronic.unblockUI();
                        }
                    }, function onError() {
                        Metronic.unblockUI();
                        NotificationService.Error("Error upon the API request");
                    });
                };
            }
        }
    }
]);