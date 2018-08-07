'use strict';

MetronicApp.controller('HeaderController', ['$rootScope', '$scope', '$state', 'authService', function ($rootScope, $scope, $state, authService) {

    $scope.$on('$viewContentLoaded', function () {

        Metronic.initAjax();

    });

    var BrowserNotSupportError = 'Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.';

    $scope.IsInreportpage = function () {
        var state = $state.current.name;
      
        switch (state) {
            case "dashboard":
                return false;
                break;
            case "expenses":
                return false;
                break;
            case "sales":
                return false;
                break;
            case "casessold":
                return false;
                break;
            case "cases-sold-categories":
                return false;
                break;

            case "revenue":
                return false;
                break;
            case "revenue-categories":
                return false;
                break;
            case "Profitablity":
                return false;
                break;
            case "transportation":
                return false;
                break;
            default:
                return true;

        }
    };




    $scope.setFilter = function (filter) {

        $scope.currentfilter = filter;
        $rootScope.currentfilter = filter;
        var state = $state.current.name;
        switch (state) {
            case "dashboard":

                $rootScope.$broadcast('dashboard', {
                    data: filter
                });
                break;
            case "expenses":
                $rootScope.$broadcast('expenses', {
                    data: filter
                });
                break;
            case "sales":
                $rootScope.$broadcast('sales', {
                    data: filter
                });
                break;
            case "casessold":
                $rootScope.$broadcast('casessold', {
                    data: filter
                });
                break;
            case "cases-sold-categories":
                $rootScope.$broadcast('cases-sold-categories', {
                    data: filter
                });
                break;

            case "revenue":
                $rootScope.$broadcast('revenue', {
                    data: filter
                });
                break;
            case "revenue-categories":
                $rootScope.$broadcast('revenue-categories', {
                    data: filter
                });
                break;
            case "Profitablity":
                $rootScope.$broadcast('profitability', {
                    data: filter
                });
            case "transportation":
                $rootScope.$broadcast('transportation', {
                    data: filter
                });
                

                break;

        }


    };
    $scope.changeexpansion = function () {

        $scope.showimage = !$scope.showimage;

        if (typeof localStorage === 'object') {
            try {
                var isexpanded = localStorage.getItem('isexpanded');
                if (isexpanded == "true") {
                    localStorage.removeItem('isexpanded');
                    localStorage.setItem('isexpanded', false);
                }
                if (isexpanded == "false") {
                    localStorage.removeItem('isexpanded');
                    localStorage.setItem('isexpanded', true);
                }
            } catch (e) {
                Storage.prototype._setItem = Storage.prototype.setItem;
                Storage.prototype.setItem = function () { };
                alert(BrowserNotSupportError);
            }
        }


    };
    $scope.logout = function () {
        authService.logout();
        if (typeof localStorage === 'object') {
            try {
                $scope.currentfilter.Description = "This Month to Date";
                $rootScope.currentfilter.Description = "This Month to Date";
                Metronic.unblockUI();
                localStorage.clear();
            } catch (e) {
                localStorage.clear();
                Storage.prototype._setItem = Storage.prototype.setItem;
                Storage.prototype.setItem = function () { };

              //  alert(BrowserNotSupportError);
                window.location.reload();
            }
        }

        Metronic.unblockUI();
        $state.go('login');
    }

    $scope.user = authService.user;

    $scope.$on('user_changed', function (event, param) {
        $scope.user = param.newUser;
    });

    $rootScope.$state = $state; // state to be accessed from view

}]);