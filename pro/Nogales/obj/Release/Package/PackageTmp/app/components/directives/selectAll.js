MetronicApp.directive('selectAll', ['$rootScope', '$parse', function ($rootScope, $parse) {
    return {
        restrict: 'A',
        replace: false,
        require: '?ngModel',
        scope: { selectAll: '=', ngModel: '=' },
        link: function (scope, element, attrs, controller) {
            // if broadCastEvent is set to true, then after every change in the selected data, directive will broadcast an Event - 'select_all_changed'
            scope.broadCastEvent = attrs.broadcastEvent == null ? false : attrs.broadcastEvent;
            scope.fullOptions = [];

            scope.$watch('selectAll', function (newVal, oldVal) {
                if (newVal != null && newVal.length > 0) {

                    var selectAllOpt = {};
                    selectAllOpt["Value"] = controller.$viewValue != null && controller.$viewValue.length == newVal.length ?
                                            "Clear All" : "Select All";
                    selectAllOpt["Key"] = "all";

                    //var selectAllOpt = {CompanyName : "Select All", IngeniumLocationId : "all"};
                    newVal.unshift(selectAllOpt);
                    scope.fullOptions = newVal;
                }
            });

            scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                //if (newVal != null && newVal.indexOf("all") >= 0) {
                if (newVal != null &&
                    ((newVal.indexOf(angular.toJson({ "Value": "Select All", "Key": "all" })) >= 0) ||
                    (newVal.indexOf(angular.toJson({ "Value": "Clear All", "Key": "all" })) >= 0))) {
                    if (newVal.length >= scope.fullOptions.length) {
                        // Deselect all
                        controller.$viewValue = [];
                        //controller.$modelValue = [];
                    }
                    else {
                        //select all
                        var allOptions = scope.fullOptions;
                        allOptions.shift();
                        var optionValues = [];
                        
                        angular.forEach(allOptions, function (val, index) {
                            //optionValues.push(val["Value"]);
                            optionValues.push(angular.toJson(val));
                        });
                        controller.$viewValue = optionValues;
                        //controller.$viewValue = optionValues;
                        //controller.$modelValue = optionValues;
                        $('.select2-drop').css('display', 'none');
                    }
                }
                else if (newVal != null) {
                    if (newVal.length >= scope.fullOptions.length) {
                        if (scope.selectAll != null) {
                            var selectAllOpt = {};
                            selectAllOpt["Value"] = "Clear All";
                            selectAllOpt["Key"] = "all";
                            $('.select2-no-results').hide();
                            scope.selectAll.unshift(selectAllOpt);
                        }

                    }
                    else if (newVal.length == scope.fullOptions.length - 1) {
                        scope.selectAll[0]["Value"] = "Clear All";
                    }
                    else {
                        //scope.selectAll[0]["Value"] = "Select All";
                        (scope.selectAll.length>0)?scope.selectAll[0]["Value"] = "Select All" : void(0);
                    }

                }
                // Updating controller model
                scope.ngModel = controller.$viewValue;

                if (scope.broadCastEvent) {
                    $rootScope.$broadcast("select_all_changed", { newVal: controller.$viewValue });
                }
            });

        }
    }
}]
  );




