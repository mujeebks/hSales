'use strict';
MetronicApp.controller('ChangePasswordController', ['$scope', '$http', '$state', 'authService', '$interval',
function ($scope, $http, $state, authService, $interval) {
    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        Metronic.initAjax();
    });

    $scope.startRedirect = false;
    $scope.time = 5;
    $scope.message = '';
    $scope.formData = {
        CurrentPassword: '',
        NewPassword: '',
        ConfirmPassword: ''
    }

    $scope.changePassword = function () {
        removeErrorMessage();
        if (validPassword()) {
            Metronic.startPageLoading({ message: 'Changing your password...' });
            authService.updatePassword($scope.formData)
            .then(function onSuccess() {
                authService.logout();
                redirectedToLogin();
                Metronic.stopPageLoading();
            })
            .catch(function onError() {
                showErrorMessage('An error occured while changing your password');
                Metronic.stopPageLoading();
            });
        }
    }

    $scope.clearForm = function () {
        $scope.formData = {
            CurrentPassword: '',
            NewPassword: '',
            ConfirmPassword: ''
        }
        $scope.passwordUpdateForm.$setPristine();
    }

    function showErrorMessage(msg) {
        $scope.message = msg;
    }

    function removeErrorMessage() {
        $scope.message = '';
    }

    function validPassword() {
        var isValid = true;

        if ($scope.formData.NewPassword.length < 6) {
            showErrorMessage('Password must have minimum 6 characters');
            isValid = false;
        }
        else if ($scope.formData.NewPassword != $scope.formData.ConfirmPassword) {
            showErrorMessage('Your password and confirm password are not matching');
            isValid = false;
        }

        return isValid;
    }

    function redirectedToLogin() {
        $scope.startRedirect = true;
        $scope.time = 5;
        $interval(function () {
            $scope.time -= 1;
            if ($scope.time <= 0) {
                $state.go("login");
            }
        }, 1000, $scope.time);
    }

}]);


