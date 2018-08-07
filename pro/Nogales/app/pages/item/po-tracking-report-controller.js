
'use strict';

MetronicApp.controller('PotrackingReportController', ['$interval', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', 'commonService', '$controller',
    function ($interval, $scope, dataService, $stateParams, $filter, NotificationService, commonService, $controller) {
        $scope.$on('$viewContentLoaded', function () {

            Metronic.initAjax();
        });
      

        $controller('BaseController', { $scope: $scope });
     
    }]);
