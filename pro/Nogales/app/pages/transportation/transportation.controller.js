
'use strict';

MetronicApp.controller('transportationController', ['$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', '$controller','$rootScope','storageService',
    function ($scope, dataService, $stateParams, $filter, NotificationService, $controller, $rootScope, storageService) {

   
        // Inheritis from Base Controller
        $controller('BaseController', { $scope: $scope });

        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();

        });

        $rootScope.$on('transportation', function (event, args) {
            Metronic.blockUI({ boxed: true });
            var filter = args.data;
            LoadTransportation(filter);
          
        });
        Metronic.blockUI({ boxed: true });
        $scope.tableFunctions = {};

        $scope.caption_subjectfnTransportationChart = function (Title) {
           
            $scope.caption_subjecttransportationChart=(Title=="initial") ? "Transportation" : Title
        };

        function LoadTransportation(filter) {
            Metronic.blockUI({ boxed: true });
         
            dataService.GetDashboardDiverTripData(filter).then(function (response) {
                try {
                    debugger
                    $scope.TripData = response.data;

                } catch (e) {
                    NotificationService.Error();

                }
                finally {
                    storageService.setStorage('CurrentFilter', filter)
                    dataService.PendingRequest();
                }

            }, function onError() {
                dataService.PendingRequest();
                NotificationService.Error();

            });
        };
        LoadTransportation($rootScope.currentfilter);
    }]);
