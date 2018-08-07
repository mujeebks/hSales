
'use strict';
MetronicApp.controller('FinanceController', ['$timeout', '$scope', 'dataService', 'NotificationService', '$controller', '$rootScope','HelperService',
function ($timeout, $scope, dataService, NotificationService, $controller, $rootScope, HelperService) {
  
    $controller('BaseController', { $scope: $scope });
    $scope.$on('$viewContentLoaded', function () { Metronic.initAjax(); });
    $scope.chartData = [];
    $scope.PriorMonthRanges = {};
    $scope.PriorYearRanges = {};


    $scope.refreshCharts = function () {
        Metronic.blockUI({ boxed: true });
        GetDashboardCollectionInfoData($rootScope.currentfilter);
    };




    $rootScope.$on('finance', function (event, args) {
        Metronic.blockUI({ boxed: true });
        GetDashboardCollectionInfoData( args.data);
    });

    $scope.caption_subjectfn = function (title) {
        $scope.caption_subject = (title == "initial") ? "TOTAL COLLECTION" : title;
    };

    $scope.caption_subjectfn('initial');

    function GetDashboardCollectionInfoData(filter) {
            $scope.currentfilter = filter;
            Metronic.blockUI({ boxed: true, message: 'LOADING...' });

            dataService.GetDashboardCollectionInfoData(filter).then(function (response) {
            try {
                $timeout(function () {
                    $scope.chartData = response.data;
                    $scope.PriorMonthRanges = PopulateGuageNonStacked(response.data[0], false, "Val");
                    $scope.PriorYearRanges = PopulateGuageNonStacked(response.data[0], true, "Val");
                    $scope.isyearlydata = (dataService.IsGlobal_Filter_Year()) ? true : false;
                }, 200);

            } catch (e) {
                NotificationService.Error();
            }
            finally {
                    dataService.PendingRequest();
            }
        }, function onError() {
            dataService.PendingRequest();
            NotificationService.Error();
        });
    };

    function PopulateGuageNonStacked(graphDataItem, Previousmonth, type) {

        var first = (Previousmonth) ? graphDataItem[type + "1"] : graphDataItem[type + "1"];
        var second = (Previousmonth) ? graphDataItem[type + "3"] : graphDataItem[type + "2"]
       
        var PriorMonth = first - second;
        if (PriorMonth !== 0) {
            PriorMonth = PriorMonth / second;
            PriorMonth = PriorMonth * 100;
            PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
        }
        else {
            PriorMonth = 0;
        }
        $scope.PriorRangeText = HelperService.getPriorRangeText($rootScope.currentfilter.Description);
        return {
            TotalDifference: Math.round(PriorMonth),
            FirstRange: first,
            SecondRange: second,
            RangeText: (Previousmonth) ? $scope.PriorRangeText : "PRIOR YEAR"
        }
    };
    GetDashboardCollectionInfoData($rootScope.currentfilter);
}]);
//197
