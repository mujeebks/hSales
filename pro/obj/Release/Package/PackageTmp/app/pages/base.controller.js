
// Base controller or Common controller
MetronicApp.controller('BaseController', ['$scope', 'commonService','$state', function ($scope, commonService,$state) {
    
    
    //$scope.UserAccess = commonService.getuseraccesss();
   
    // Fn. to check whether the sales person targets synced or not
    $scope.isSalesPersonTargetsSynced = function () {
        var isSynedToday = false;
        var cookie = $.cookie("Nogales-LastDate_SalesPersonTarget");
        var today = new Date();
        if (cookie) {
            var cookieDate = new Date(cookie);
            if (today.getDate() == cookieDate.getDate() && today.getMonth() == cookieDate.getMonth() && today.getFullYear() == cookieDate.getFullYear()) {
                isSynedToday = true;
            }
        }
        return isSynedToday;
    }

    // Fn. to set sales person target synced as True
    $scope.setSalesPersonTargetsSynced = function()
    {
        var today = new Date();
        // Update the cookie with today
        $.cookie("Nogales-LastDate_SalesPersonTarget", today.toDateString());
    }

    // Fn. to stop propagation
    $scope.preventClosing = function (event) {
        event.stopPropagation();
    }


    $scope.clearDate = function (date) {
        var newDate = "";
        if (date && new Date(date) != "Invalid Date") {
            var parts = date.split("/");
            if (parts.length == 3) {
                parts[0] = (parts[0].length == 1) ? ("0" + parts[0]) : parts[0];
                parts[1] = (parts[1].length == 1) ? ("0" + parts[1]) : parts[1];
                newDate = parts[0] + "/" + parts[1] + "/" + parts[2];
            }
        }
        return newDate;
    }
    $scope.isDate = function (date) {
        return ((date) && (date.length == 10) && (new Date(date) != "Invalid Date"));
    }

    $scope.isNotaNumber = function (a) {
        var flag = isNaN(a) ? true : false;
        return flag;
    }

    $scope.isPreviouseDate = function (fromDate, toDate) {
        var status = false;
        if ($scope.isDate(fromDate) && $scope.isDate(toDate)) {
            var startDate = new Date(fromDate);
            var endDate = new Date(toDate);
            status = (startDate > endDate) ? true : status;
        } else {
            status = false;
        }
        return status;

    }

    //debugger;
    //if (!$scope.UserAccess.IsModuleAccess) {
     
    //    $state.go('dashboard');

    //    setTimeout(function () {
          
    //        Metronic.unblockUI();
    //    },100);
     
    //    return false;
    //}
    
}]);