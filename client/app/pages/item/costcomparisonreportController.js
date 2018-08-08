
'use strict';

MetronicApp.controller('CostcomparisonController', ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', 'commonService', '$controller',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, commonService, $controller) {
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $scope.commoditylist = [];
        $scope.commoditylist = [
            { Id: "", Description: "All" },
            { Id: "Produce", Description: "Produce" },
             { Id: "Grocery", Description: "Grocery" }
        ];

        $scope.reportData = [];

        //$scope.selectedcommodityitem = $scope.commoditylist[0];

        $controller('BaseController', { $scope: $scope });

           $scope.btnSpinner = commonService.InitBtnSpinner('#search');


        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
        $scope.searchClicked = false;

        $scope.selectedSalesPerson = '';

        $scope.costComparisonFilter = [];
        $scope.selectedItems = [];

        function makefilter() {
            for (var i = 0; i < $rootScope.GlobalFilter.length; i++) {
                if ($rootScope.GlobalFilter[i].Id == 2 || $rootScope.GlobalFilter[i].Id == 4 || $rootScope.GlobalFilter[i].Id == 6 || $rootScope.GlobalFilter[i].Id == 8) {
                    $scope.costComparisonFilter.push($rootScope.GlobalFilter[i]);
                }
            }


        }
        makefilter();

        $scope.getItemFilters = function () {
            dataService.getItemReportFilters()
           .then(function (response) {
               $scope.availableItems = response.data.ListItem;
               $scope.datas = response.data.ListItem;
           });
        }
        $scope.getItemFilters();


        $scope.search = function (filter, commodity) {
            $scope.iscanceldisabled = false;
            $scope.btnSpinner.start();

            var params = {
                filterId: filter.Id,
                Comodity: commodity.Id,
                item: ($scope.selectedItems == null || $scope.selectedItems == '') ? [] : getValueListFromKeyValueList(parsJasonFromArray($scope.selectedItems))
            };

            (httpRequest = dataService.GetItemComparisonReport(params))
            .then(function onSuccess(response) {
                if (response) {
                    $scope.iscanceldisabled = true;
                    $scope.Current = "Cost Current";
                    $scope.Historical = "Cost Historical";
                    $scope.Prior = "Cost Prior";

                    var id = filter.Id;
                    if (id == 2) {
                        $scope.Current = "This Week";
                        $scope.Historical = "This Week LY";
                        $scope.Prior = "Last Week";
                    }
                    if (id == 4) {
                        $scope.Current = "This Month";
                        $scope.Historical = "This Month LY";
                        $scope.Prior = "Last Month";
                    }
                    if (id == 6) {
                        $scope.Current = "This Quarter";
                        $scope.Historical = "This Quarter LY";
                        $scope.Prior = "Last Quarter";
                    }

                    $scope.reportData = response.data;
                    $scope.reportDataSafe = response.data;
                    Metronic.unblockUI();
                }
                $scope.btnSpinner.stop();
            })
                .catch(function (exce) {
                    $scope.iscanceldisabled = true;
                    NotificationService.Error("An error occured while generating the report");
                    $scope.btnSpinner.stop();

                });


        }
        $scope.resetSearch = function () {
          //  $scope.myTableFunctions.resetSearch();
            $scope.selectedItems = '';
            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {
            $scope.iscanceldisabled = true;
            try {
                return (httpRequest && httpRequest.abortCall());
            }
            catch (e) {
                //console.log(e.constructor.name);
            }
        };

        $scope.getCsvHeader = function () {
            return ['Commodity', 'Item', 'Item Name', $scope.Current, $scope.Historical, $scope.Prior];
        }
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            predefinedHeader = ['Commodity', 'Item', 'ItemName', 'costCurrent', 'costHistorical', 'costPrior'];

            angular.forEach($scope.reportDataSafe, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {

                        header[index] = (key == "costCurrent" || key == "costHistorical" || key == "costPrior" || key == "costYearHistorical" || key == "costYearCurrent") ? "$" + value : value;

                    }

                });
                if (header != undefined)
                    array.push(header);
            });
            return array;
        };

        function generateDefaultReport() {

            $scope.selectedfilter = $scope.costComparisonFilter[0];
            // $scope.selectedcommodityitem = "";
            $scope.selectedcommodityitem = $scope.commoditylist[0];


           // $scope.search($scope.selectedfilter, "");
        }
        generateDefaultReport();
        var initializing = true
        $scope.$watch('selectedItems', function (newVal, oldVal) {

            if (newVal != oldVal) {
                if (initializing) {
                    initializing = false;
                } else {
                   // $scope.search($scope.selectedfilter, $scope.selectedcommodityitem.Id);
                    // do whatever you were going to do
                }


            }
        });

        function getValueListFromKeyValueList(list) {
            var result = [];
            for (var i = 0; i < list.length; i++) {
                result.push(list[i].Key);
            }
            return result;
        }
        function parsJasonFromArray(array) {
            var result = [];
            if (array && array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    result.push(JSON.parse(array[i]));
                }
            }
            return result;
        }

    }]);
