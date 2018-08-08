
MetronicApp.factory("authInterceptor", ['localStorageService','$q', '$injector',
function (localStorageService,$q, $injector) {
    var authInterceptorServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    var canceller = $q.defer();


    var _request = function (config) {
        config.headers = config.headers || {};
       // config.timeout = canceller.promise;
        if (window.isPrivate == true) {
            config.headers.Authorization = 'Bearer ' + window.tocken;
        }
        else {
            var authData = localStorageService.get('nogalesAuth');
            if (authData && config.url.indexOf(".html") < 0) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }
        }

        //try {
        //    var authData = localStorageService.get('nogalesAuth');
        //    if (authData && config.url.indexOf(".html") < 0) {
        //        config.headers.Authorization = 'Bearer ' + authData.token;
        //    }
        //} catch (e) {
        //    config.headers.Authorization = 'Bearer ' + window.tocken;
        //}



        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            // when user not authorized
          //  localStorage.removeItem('')
            // localStorage.clear();
            //angular.forEach($http.pendingRequests, function (request) {
            //    $http.abort(request);
            //    console.log("aborting all call due to unauth user")
            //});
            // $injector.get('$state').transitionTo('login');
            canceller.resolve('Unauthorized');
            Metronic.unblockUI();
            window.location.href = "#/login?session=unauth";
          //  $state.go("login");
        }

        if (rejection.status === 403) {
            canceller.resolve('Forbidden');
            window.location.href = "#/login?session=Forbidden";
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);