
'use strict';

MetronicApp.controller('LoginController', ['$rootScope', '$scope', '$http', '$state', 'authService', 'commonService', 'dataService', 'NotificationService', '$location',
    function ($rootScope,$scope, $http, $state, authService, commonService, dataService, NotificationService, $location) {
    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        Metronic.initAjax();
        $rootScope.continue();
    });

    alert("sf")

    var searchObject = $location.search();
    //if (searchObject.session == "screenIdle") {

    //    showInfoMessage("screen idle");
    //}

    Metronic.unblockUI();
    $scope.btnSpinner = commonService.InitBtnSpinner('#login');
    $scope.forgotPasswordBtnSpinner = commonService.InitBtnSpinner('#forgotPassword');

    initBackgroundSlider();

    $scope.isErrorMessage = true;
    $scope.message = '';
    $scope.loginData = {
        userName: 'riyas',
        password: 'riyas'
    }

    // TO DO: Implement login functionality
    $scope.login = function () {
      //  $scope.btnSpinner.start();
        //  removeErrorMessage();
        debugger

                    //if (typeof localStorage === 'object') {
                    //    try {
                    //        var isexpanded = localStorage.setItem('test',"test");
                    //        window.isPrivate = false;
                    //        startAuth();
                    //    } catch (e) {

                    //        Storage.prototype.setItem = function () { };
                    //        console.log("PrivateBrowsedddsdsfrrrrr");
                    //        startAuth();
                    //        window.isPrivate = true;
                    //    }
                    //}

        startAuth();


    }

    function startAuth() {
        showInfoMessage("Verifying Credentials...");
        authService.login($scope.loginData.userName, $scope.loginData.password)
            .then(function onSuccess(data) {
                removeErrorMessage();
                if (data.forceReset == "True") {
                    $state.go('resetPassword');
                }
                else if (authService.user.role == "Tool Admin")
                    $state.go('user-management');
                else
                   debugger
                    $state.go('Categories');

                $scope.btnSpinner.stop();
            })
            .catch(function onError() {
                $scope.btnSpinner.stop();
                showErrorMessage('Either the username was not recognized or the password was incorrect');
            });
    }

    function showErrorMessage(msg) {
        $scope.isErrorMessage = true;
        $scope.message = msg;
    }

    function removeErrorMessage() {
        $scope.isErrorMessage = true;
        $scope.message = '';
    }

    function showInfoMessage(msg) {
        $scope.isErrorMessage = false;
        $scope.message = msg;
    }

    function initBackgroundSlider() {
        $(".login-bg").backstretch(["assets/global/img/login/1.jpg", "assets/global/img/login/2.jpg",  "assets/global/img/login/4.jpg", "assets/global/img/login/5.jpg", "assets/global/img/login/6.jpg"], { fade: 1e3, duration: 8e3 });
    }


    $scope.openModal = function (elementId) {
        
        $scope.resetEmail = "";
        if (elementId && elementId != '') {
            $('#' + elementId).modal({ backdrop: 'static', keyboard: false });
        }
    }
    $scope.closeModal = function (elementId) {
        if (elementId && elementId != '') {
            $('#' + elementId).modal('hide');
        }
    }

    $scope.forgotPassword = function () {
        $scope.forgotPasswordBtnSpinner.start();
        var params = {
            "Email": $scope.resetEmail,
        };

        //startDivLoader("resetUserPasswordModal");
        dataService.ForgetPassword(params).then(function (response) {

            //stopDivLoader("resetUserPasswordModal");
            $scope.forgotPasswordBtnSpinner.stop();
            $scope.closeModal('resetUserPasswordModal');
            NotificationService.Success("Successfully sent mail");
            $scope.resetEmail = "";

        }, function onError() {
            //Metronic.stopPageLoading();
            //stopDivLoader("resetUserPasswordModal");
            $scope.forgotPasswordBtnSpinner.stop();
           // $scope.closeModal('resetUserPasswordModal');
            //NotificationService.Error("Error upon the send change password mail. The email may not be exists");
            NotificationService.Error("No matching account was found!\n Please fill correct email address for password reset link");
            NotificationService.ConsoleLog('Error on the server');
           // $scope.resetEmail = "";

        });
    }

    //$scope.showResetPopup = function () {
    //    //$('#email-error').remove();
    //    //$scope.passwordresetmail = "";
    //    $('#resetUserPasswordModal').modal('show');
    //    //$scope.selectedUserForReset = "";
    //}
}]);
