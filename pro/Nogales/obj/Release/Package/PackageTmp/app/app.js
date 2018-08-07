
var MetronicApp = angular.module("MetronicApp", [
    "ui.router",
    "ui.bootstrap",
    //"oc.lazyLoad",
    "ngSanitize",
    "ngAnimate",
    "toggle-switch",
    "Alertify",
    //"smart-table",
    "ui.select2",
    "daterangepicker",
    "smart-table",
    "angular.filter",
    "ngCsv",
    "toggle-sidemenu",
    "LocalStorageModule",
    "simple-autocomplete",
    "rzModule",
    "angularjs-gauge"
]);


MetronicApp.factory('settings', ['$rootScope', function ($rootScope)
{

    if (typeof localStorage === 'object') {
        try {
            var isexpanded = localStorage.getItem('isexpanded');
            if (isexpanded == "true") {
                isexpanded = true;
            }
            if (isexpanded == "false") {
                isexpanded = false;
            }
            if (isexpanded == null) {
                localStorage.setItem('isexpanded', false);
                isexpanded = false;
            }

            $rootScope.isPrivate = false;
        } catch (e) {
            Storage.prototype.setItem = function () { };
            console.log("PrivateBrowser");
            window.isPrivate = true;
            alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
        }
    }
    var settings = {
        layout: {
            pageSidebarClosed: isexpanded, // sidebar state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        layoutImgPath: Metronic.getAssetsPath() + 'admin/layout/img/',
        layoutCssPath: Metronic.getAssetsPath() + 'admin/layout/css/'
    };

    $rootScope.settings = settings;
    return settings;
}]);

MetronicApp.controller('AppController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    $scope.$on('$viewContentLoaded', function () {
        Metronic.initComponents(); // init core components
        //Layout.init(); //  Init entire layout(header, footer, sidebar, etc) on page load if the partials included in server side instead of loading with ng-include directive
    });
    $scope.version = window.version;

    $rootScope.$on('$stateChangeStart',
        function (event, toState, toParams, fromState, fromParams, options) {

            $('ul.sub-menu').each(function () {
                if ($(this).find('[ui-sref="' + toState.name + '"]') && $(this).find('[ui-sref="' + toState.name + '"]').length > 0 && !$(this).parent().hasClass('open')) { //
                    $(this).parent().addClass('open');
                    $(this).show();
                }
            });
        })
}]);

MetronicApp.controller('HeaderController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initHeader(); // init header
    });
}]);

MetronicApp.controller('SidebarController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initSidebar(); // init sidebar
    });
}]);

MetronicApp.controller('PageHeadController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Demo.init(); // init theme panel
    });
}]);

MetronicApp.controller('FooterController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initFooter(); // init footer
    });
}]);

