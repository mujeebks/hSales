
MetronicApp.factory("authService", ["dataService", "localStorageService", "$q", "$rootScope", function (dataService, localStorageService, $q, $rootScope) {
    var userData = {
        userName: "",
        name: "",
        role: ""
    };



    var login = function (username, paswd) {
        var q = $q.defer();
        dataService.RequestToken(username, paswd)
                .then(function (result) {
                    var response = result.data;
                    

                debugger
                    localStorageService.set("nogalesAuth", { token: response.access_token, userName: response.userName, role: response.role, name: response.name,UserId:response.userId });
                    window.tocken = response.access_token;
                    if (response.forceReset == "True") {
                       
                    }
                    else if (response.role == "Tool Admin")
                    {

                    }

                    else {
                        getaccess(response.userId);
                    }
                       
                        


                    factory.user.userName = response.userName;
                    factory.user.role = response.role;
                    factory.user.name = response.name;
                    $rootScope.$broadcast('user_changed', {newUser : factory.user});
                    q.resolve(result.data);
                })
                .catch(function () {
                    q.reject();
                });

        return q.promise;
    }

    var logout = function () {
        factory.user = {
            userName: "",
            name: "",
            role: ""
        };
        //
        localStorageService.set("nogalesAuth", null);
        $rootScope.$broadcast('user_changed', { newUser: factory.user });
       // localStorage.clear();
    }

    var resetPassword = function (data) {
        var q = $q.defer();
        dataService.ResetPassword(data)
                .then(function (result) {
                    q.resolve(result);
                 
                })
                .catch(function () {
                    q.reject();
                });

        return q.promise;
    }

    var getaccess = function (userid) {
        var q = $q.defer();
        dataService.GetUserAccess(userid)
                .then(function (result) {
                    q.resolve(result);
                   
                    localStorageService.set("nogalesAuthAccess", result.data);

                    $rootScope.$broadcast('ACCESSLOADED', { Access: result.data });
                   

                })
                .catch(function () {
                    q.reject();
                });

        return q.promise;
    }

    var updatePassword = function (data) {
        var q = $q.defer();
        dataService.ChangePassword(data)
                .then(function (result) {
                    q.resolve(result);
                })
                .catch(function () {
                    q.reject();
                });

        return q.promise;
    }

    var populateAuthDataFromLocalstorage = function () {
        var data = localStorageService.get("nogalesAuth");
        if (data != null && data != "") {
            factory.user.userName = data.userName;
            factory.user.role = data.role;
            factory.user.name = data.name;
        }
        else {
            factory.user.userName = "";
            factory.user.role = "";
            factory.user.name = "";
        }
    }

    var checkUserData = function () {
        if (factory.user.userName == "")
            return false;
        else
            return true;
    }

    var factory = {
        user: userData,
        login: login,
        logout: logout,
        resetPassword: resetPassword,
        updatePassword: updatePassword,
        fillAuthData: populateAuthDataFromLocalstorage,
        isAuthenticated: checkUserData
    };

    return factory;
}]);
