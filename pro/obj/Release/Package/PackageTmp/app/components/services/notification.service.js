
    'use strict';

    MetronicApp.factory('NotificationService', ['Alertify', '$injector', '$state', function (Alertify, $injector, $state) {
       
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
        // Error notification
        function error(message) {
        
            var isautherized = getParameterByName('session');

            if (isautherized != "unauth") {
                if (message && message != '')
                    Alertify.error(message);
                else {
                    Alertify.error("An error occured while processing your request");
                }
            }
            else {
                console.log("The application is unauthorized stopped the Error Notification !!")
            }
           
        }

        // Success notification
        function success(message) {
            if (message && message != '')
                //Alertify.set({ delay: 10000000000 });
                Alertify.success(message.trim());
        }

        // Normal notification
        function log(message) {
            if (message && message != '')
                Alertify.log(message);
        }

        // Alert box with Ok button
        function alert(message) {
            if (message && message != '')
                Alertify.alert(message);
        }

        /// Confirm box with Ok and Cancel button
        function confirm(message, onOk, onCancel, onOkParameter) {
            if (message && message != '')
            {
                if (onOk && onCancel) {
                    Alertify.confirm('Are you sure?').then(onOk(onOkParameter), onCancel());
                }
                else if (onOk) {
                    Alertify.confirm('Are you sure?').then(onOk(onOkParameter), null);
                }
                else if (onCancel) {
                    Alertify.confirm('Are you sure?').then(null, onCancel());
                }

            }
        }

        // Write the message into the console only if the debugging mode is ON
        function consoleLog(message) {
            var isDebuggingMode = $injector.get('isDebugging');
            if (isDebuggingMode) {
                console.log(message);
            }
        }

        return {
            Error: error,
            Success: success,
            Log: log,
            Alert: alert,
            Confirm: confirm,
            ConsoleLog: consoleLog
        }

    }]);
