
    'use strict';

    MetronicApp.factory('$exceptionHandler', ['$injector', function ($injector) {

        var NotificationService;
        return function exceptionHandler(exception, cause) {
            NotificationService = NotificationService || $injector.get('NotificationService');
            var isDebugging = $injector.get('isDebugging');
           // alert(isDebugging);
            if (isDebugging) {
                //console.log(exception)
                NotificationService.Error("Debugging: " + exception);
                console.error(exception, "Debugging Mode");
            }
            else {
               // NotificationService.Error(" Server Error.");
            }

        };

    }]);
