
'use strict';

MetronicApp.controller('ResetController', ['$scope', '$http', '$state', 'authService', '$interval',
    function ($scope, $http, $state, authService, $interval) {
        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();
        });

        initBackgroundSlider();

        $scope.startRedirect = false;
        $scope.time = 5;
        $scope.message = '';
        $scope.loginData = {
            NewPassword: '',
            ConfirmPassword: ''
        }

        $scope.resetPassword = function () {
            removeErrorMessage();
            if (validPassword()) {
                authService.resetPassword($scope.loginData)
                .then(function onSuccess() {
                    authService.logout();
                    redirectedToLogin();
                })
                .catch(function onError() {
                    showErrorMessage('An error occured while resetting your password');
                });
            }
        }

        function showErrorMessage(msg) {
            $scope.message = msg;
        }

        function removeErrorMessage() {
            $scope.message = '';
        }

        function validPassword() {
            var isValid = true;

            if ($scope.loginData.NewPassword.length < 6) {
                showErrorMessage('Password must have minimum 6 characters');
                isValid = false;
            }
            else if ($scope.loginData.NewPassword.length > 15) {
                showErrorMessage('Password length exceed more than 15 characters');
                isValid = false;
            }
            else if ($scope.loginData.NewPassword != $scope.loginData.ConfirmPassword) {
                showErrorMessage('Your password and confirm password are not matching');
                isValid = false;
            }
             if ($scope.loginData.NewPassword.length ==0) {
                showErrorMessage('You must enter a password!');
                isValid = false;
            }
            return isValid;
        }

        $scope.gottologin = function () {
            $state.go("login");
        };

        function redirectedToLogin() {
            $scope.startRedirect = true;
            $scope.time = 5;
            //$scope.time = 55;
            $interval(function () {
                $scope.time -= 1;
                if ($scope.time <= 0) {
                    $state.go("login");
                }
            }, 1000, $scope.time);
        }

        function initBackgroundSlider() {
            $(".login-bg").backstretch(["assets/global/img/login/1.jpg", "assets/global/img/login/2.jpg", "assets/global/img/login/3.jpg", "assets/global/img/login/4.jpg"], { fade: 1e3, duration: 8e3 });
        }
    }]);
