/**
 open-sub-menu
 to solve the issue with side menu bar when there is a 
 single layer menu comes insted of multi layer menu
 have to add a property parentMenu(e.g. parentMenu='dashboard')
 in data object of the ui-routing state.
 also have to assign attribute value to directive in ui 
 e.g. open-sub-menu="dashboard"
*/

//MetronicApp.directive("openSubMenu", ["$location", "$rootScope", "$filter", "$state",
angular.module("toggle-sidemenu", []).directive("openSubMenu", ["$location", "$rootScope", "$filter", "$state",
function ($location, $rootScope, $filter, $state) {
    this.count = 0;
    return {
        restrict: "A",
        //require: "?ngModel",
        // replace: true,
        // scope: { nlLocation: "=", chartTitle: "=" },
        // template: '<div>',

        link: function (scope, element, attrs, controller) {
            $rootScope.$on('$stateChangeSuccess',
            function (event, unfoundState, fromState, fromParams) {
                var a = scope;
                var b = attrs;
                var el = element;

                if (attrs.openSubMenu == 'dashboard') {
                    if ($state.current.data.parentMenu == 'dashboard') {
                        element.addClass("active open");
                        element.find('.sub-menu').show();
                        element.find('.arrow').addClass("open");
                    }
                    else {
                        element.removeClass("active open");
                        element.find('.sub-menu').hide();
                        element.find('.arrow').removeClass("open");
                    }
                }
                else if (attrs.openSubMenu == 'report') {
                    //for all the childs of the menu item(report) which having no sub menus
                    if ($state.current.data.parentMenu == 'report') {
                        element.addClass("active open");
                        element.find('.sub-menu').show();
                        element.find('.arrow').addClass("open");

                        //to hide all the child menus which having SUB_MENU
                        element.find('ul .sub-menu').hide();
                        element.find('li .arrow').removeClass("open");
                        element.find('li').removeClass("open active");
                    }
                        //if the report parent menu having child which having sub menu
                        // also no need to provide directive open-sub-menu  on its ui.
                    else if ($state.current.data.parentMenu == 'warehouse') {
                        //element.closest().addClass("active open");
                        //element.closest('.sub-menu').show();
                        //element.closest('.arrow').addClass("open");
                        element.addClass("active open");
                        element.find('.sub-menu').show();
                        element.find('.arrow').addClass("open");
                    }
                    else {
                        element.removeClass("active open");
                        element.find('.sub-menu').hide();
                        element.find('.arrow').removeClass("open");
                    }
                }
            })
        }

    }


}]);