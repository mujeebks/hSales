
'use strict';

MetronicApp.controller('ProfitabiltyController', ['$scope', 'dataService', '$filter', 'NotificationService', 'storageService', '$rootScope', '$controller',
    function ($scope, dataService, $filter, NotificationService, storageService, $rootScope, $controller) {

        $controller('BaseController', { $scope: $scope });
        Metronic.blockUI({ boxed: true });
        $scope.CustomerFilter = 50000;
        $scope.ItemFilter = 2500;
        $scope.checkClickIndex = function (isclicked, event, data, key) {
            if (isclicked == true) {
                event.stopPropagation();
            }
            else {
                if (key == "Customer") {
                    $scope.CustomerFilter = 50000;
                }
                else {
                    $scope.ItemFilter = data;
                }
            }
        };

        $scope.tableFunctions = {};
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });

        $scope.CurrentRevenuePointList = [
            { Key: "25K", value: 25000, clickedinput: false },
            { Key: "50K", value: 50000, clickedinput: false },
            { Key: "100K", value: 100000, clickedinput: false },
            { Key: "250K", value: 250000, clickedinput: false }];


        $scope.CurrentRevenuePointItemList = [
            { Key: "2.5K", value: 2500, clickedinput: false },
            { Key: "5K", value: 5000, clickedinput: false },
            { Key: "10K", value: 10000, clickedinput: false }];

        $scope.profitmargincaption = "PROFIT MARGIN";
        $scope.clickedinput = false;
        $scope.profitabilityChartTitle = "";
        $scope.currentfilterlabel = "50K";
        $scope.currentfilterlabelItem = "2.5K";
        $rootScope.$on('profitability', function (event, args) {
            Metronic.blockUI({ boxed: true });
            var filter = args.data;
            $rootScope.currentfilter = filter;
            $scope.GetMarginByDifference(true, $scope.CustomerFilter);
            $scope.GetMarginByDifference(false, $scope.ItemFilter);
            $scope.GetMargin($rootScope.currentfilter, $scope.CustomerFilter, $scope.ItemFilter);
            loadDefaultChart();
            GetProfitablityDashboardBarChartData($rootScope.currentfilter);
        });

        $scope.profitmargincaptionfn = function (title) {
            $scope.profitmargincaption = (title == "initial") ? "Profit Margin" : title;
        };

        $scope.search = function (filter, monthIndex) {
            var status = false;
            var date = new Date();
            var firstDay;
            var lastDay;
            if (filter == 'YTD') {
                firstDay = new Date(date.getFullYear(), 0, 1);
                lastDay = new Date();
            }
            else if (filter == 'LastYear') {
                firstDay = new Date(date.getFullYear() - 1, 0, 1);
                lastDay = new Date(date.getFullYear(), 0, 0);
            }
            else if (filter == 'LastTwoYear') {
                firstDay = new Date(date.getFullYear() - 2, 0, 1);
                lastDay = new Date();
            }
            else if (filter == 'LastThreeYear') {
                firstDay = new Date(date.getFullYear() - 3, 0, 1);
                lastDay = new Date();
            }

            $scope.startDate = $filter('date')(firstDay, 'yyyy-MM-dd');
            $scope.endDate = $filter('date')(lastDay, 'yyyy-MM-dd');
            $scope.dfDateFilter = "Custom";
            filterChart($rootScope.currentfilter);
        };

        function filterChart(filter) {
            Metronic.blockUI({ boxed: true });
            dataService.getMainDashboardProfitabilityChartData(filter).then(function (response) {
                try {
                    $scope.totalChartData = response.data;
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    //   Metronic.stopPageLoading();
                    dataService.PendingRequest();
                }

            }, function onError() {
               // Metronic.stopPageLoading();
                NotificationService.Error("Error upon the API request");
            });
        };

        function kFormatter(num) {
            if (num >= 1000000000000000000) {
                return (num / 1000000000000000000).toFixed(1).replace(/\.0$/, '') + 'E';
            }
            if (num >= 1000000000000000) {
                return (num / 1000000000000000).toFixed(1).replace(/\.0$/, '') + 'P';
            }
            if (num >= 1000000000000) {
                return (num / 1000000000000).toFixed(1).replace(/\.0$/, '') + 'T';
            }
            if (num >= 1000000000) {
                return (num / 1000000000).toFixed(1).replace(/\.0$/, '') + 'G';
            }
            if (num >= 1000000) {
                return (num / 1000000).toFixed(1).replace(/\.0$/, '') + 'M';
            }
            if (num >= 1000) {
                return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
            }
            return num;
        };

        $scope.addrevenue = function (revenue, type) {
            
            function ltrim(str, chars) {
                chars = chars || "\\s";
                return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
            };

            revenue = ltrim(revenue, "0");
            if (revenue !== "") {
                var KFormated = kFormatter(revenue);
                var res = {
                    Key: KFormated,
                    value: revenue,
                    clickedinput: false
                };

                if (type == "Customer") {
                    function checkexcist(item) {
                        return item.value == revenue;
                    };
                    var isexcist = $scope.CurrentRevenuePointList.findIndex(checkexcist);
                    if (isexcist == -1) {
                        $scope.CurrentRevenuePointList.push(res);
                        $scope.CustomReveue = "";
                    }
                    else {
                        $scope.CustomReveue = "";
                       // NotificationService.Error("The value already exist.");
                    }
                }
                else {

                    function checkexcist(item) {
                        return item.value == revenue;
                    };
                    var isexcist = $scope.CurrentRevenuePointItemList.findIndex(checkexcist);
                    if (isexcist == -1) {
                        $scope.CurrentRevenuePointItemList.push(res);
                        $scope.CustomReveue = "";
                    }
                    else {
                        $scope.CustomReveue = "";
                        NotificationService.Error("The value already exist.");
                    }
                }

            }
            else {
                $scope.CustomReveue = "";
            }
        };
        $scope.GetMargin = function (filter, filterCustomerData, filterItemData) {
           Metronic.blockUI({ boxed: true });
            var Kformated = kFormatter(filterCustomerData);
            $scope.currentfilterlabel = Kformated;
            var KformatedItem = kFormatter(filterItemData);
            $scope.currentfilterlabelItem = KformatedItem;
           // Metronic.blockUI({ boxed: true });
            dataService.GetMargin(filter, filterCustomerData, filterItemData).then(function (response) {
                try {
                    $scope.marginData = response.data.ProfitabiltyChart;
                    $scope.TopBottomChartByMonthCustomer = response.data.Customer.TopBottomChartByMonth;
                    $scope.CustomerChart = $scope.TopBottomChartByMonthCustomer;
                    $scope.TopBottomChartByMonthItem = response.data.Item.TopBottomChartByMonth;
                    $scope.ItemChart = $scope.TopBottomChartByMonthItem;
                    storageService.setStorage('CurrentFilter', $rootScope.currentfilter);
                    dataService.PendingRequest();

                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
               // stopDivLoader("CustomerChart");
               // NotificationService.Error("Error upon the API request");
            });
        };

        function GetProfitablityDashboardBarChartData(filter) {
           Metronic.blockUI({ boxed: true });
            dataService.GetProfitablityDashboardBarChartData(filter).then(function (response) {
                try {
                    
                    dataService.PendingRequest();
                    $scope.ProfitablityDashboardBarChartData = response.data.TotalRevenue.All;
                 

                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                    //dataService.PendingRequest();
                }
            }, function onError() {
              
                NotificationService.Error("Error upon the API request");
            });
        };

      
        
        $scope.GetMargin($rootScope.currentfilter, $scope.CustomerFilter, $scope.ItemFilter);
        $scope.GetMarginByDifference = function (isCustomer, filterData) {
            if (isCustomer) {
                $scope.currentfilterlabel = kFormatter(filterData);
                $scope.CustomerFilter = filterData;
            }
            else {
                $scope.currentfilterlabelItem = kFormatter(filterData);
                $scope.ItemFilter = filterData;
            }
           // Metronic.blockUI({ boxed: true });
        
            dataService.GetMarginByDifferennce($rootScope.currentfilter, isCustomer, filterData).then(function (response)
            {
                try {
                    
                    if (isCustomer) {
                        $scope.TopBottomChartByMonthCustomer = response.data.Customer.TopBottomChartByMonth;
                        $scope.CustomerChart = $scope.TopBottomChartByMonthCustomer;
                    }
                    else {
                        $scope.TopBottomChartByMonthItem = response.data.Item.TopBottomChartByMonth;
                        $scope.ItemChart = $scope.TopBottomChartByMonthItem;
                    }
                    storageService.setStorage('CurrentFilter', $rootScope.currentfilter);

                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
                NotificationService.Error("Error upon the API request");
            });
        }

        function loadDefaultChart() {
            Metronic.blockUI({ boxed: true });
            var date = new Date();
            var sDate = $scope.startDate = $filter('date')(new Date(date.getFullYear(), 0, 1), 'yyyy-MM-dd');
            var eDate = $scope.endDate = $filter('date')(date, 'yyyy-MM-dd');
            filterChart($rootScope.currentfilter);
            LoadGetProfitabilityByItem();
            LoadGetProfitabilityByCustomer();
            GetProfitablityDashboardBarChartData($rootScope.currentfilter);
        };

        function LoadGetProfitabilityByItem() {
          //  Metronic.blockUI({ boxed: true });
            dataService.LoadGetProfitabilityByItem($rootScope.currentfilter.Id).then(function (response) {
                try {
                    $scope.ProfitabilityByItem = response.data;
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }

            }, function onError() {
               // dataService.PendingRequest();
                NotificationService.Error("Error upon the API request");
            });
        };

        function LoadGetProfitabilityByCustomer() {
          //  Metronic.blockUI({ boxed: true });
            dataService.LoadGetProfitabilityByCustomer($rootScope.currentfilter.Id).then(function (response) {
                try {
                    $scope.ProfitabilityByCustomer = response.data;
                } catch (e) {
                    NotificationService.Error();
                }
                finally {
                    dataService.PendingRequest();
                }
            }, function onError() {
              //  dataService.PendingRequest();
                NotificationService.Error("Error upon the API request");
            });
        };

        loadDefaultChart();

        $scope.ProfitabilityByItem_subjectfn = function (title) {
            $scope.ProfitabilityByItem_subject = (title == "initial") ? "Profit By Item" : title;
        };
        $scope.ProfitabilityByCustomer_subjectfn = function (title) {
            $scope.ProfitabilityByCustomer_subject = (title == "initial") ? "Profit By Customer" : title;
        };
        $scope.BreadCrumbTextFnTotalChart = function (title) {
         
            $scope.BreadCrumbTextTotalChart = title;
        };
        $scope.caption_subjectfn = function (title) {
            if (title == "initial") {
                $scope.caption_subject = "Total Profitability";
                $scope.backBtnShow = !$scope.totalTabAll;
            }
            else {
                $scope.backBtnShow = false;
                $scope.caption_subject = title;
            }
        };

        $scope.caption_subjectfn('initial');
    }]);
