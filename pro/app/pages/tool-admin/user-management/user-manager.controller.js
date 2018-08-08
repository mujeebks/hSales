
MetronicApp.controller('UserManagement', ['$scope', 'dataService', 'NotificationService', 'authService','$state',
    function ($scope, dataService, NotificationService, authService, $state) {
        $scope.ErrorMessage = "";
    // Begin: Declaration
        $scope.groupProperty = '';
        $scope.formTitle = '';
        $scope.user = {};
        $scope.resetModel = {};
        $scope.isResetPswd = false;
        // End: Declaration


       // $('.fullscreen user-manager-tools on').tooltip('disable');

    // Begin: Public Functions

        // Fn. to create an user
        $scope.moduleacceschange = function (mode) {
            if ($scope.Modules.length > 0)
                if (mode) {
                 
                    for (var i = 0; i < $scope.Modules.length; i++) {
                        $scope.Modules[i].IsAccess = true;
                    }
                }
            

        };

        $scope.categoryacceschange = function (mode) {
          
            if ($scope.Categories.length > 0)
                if (mode) {

                    for (var i = 0; i < $scope.Categories.length; i++) {
                        $scope.Categories[i].IsAccess = true;
                    }
                }
                //else {

                //    for (var i = 0; i < $scope.Categories.length; i++) {
                //        $scope.Categories[i].Isaccesable = false;
                //    }
                //}

        };


        $scope.createuserbtnfn = function () {
            $scope.ErrorMessage = "";
         
           $scope.isUnrestricted = false;
           $scope.isUnrestrictedcat = false;

           for (var i = 0; i < $scope.Categories.length; i++) {
               $scope.Categories[i].IsAccess = false;
           }

                for (var i = 0; i < $scope.Modules.length; i++) {
                    $scope.Modules[i].IsAccess = false;
                }
            //$scope.moduleacceschange(false);
            //$scope.categoryacceschange(false);

        };
        function GetAdminUserManagement() {
            dataService.GetAllModules()
                 .then(function onSuccess(response) {
              
                     $scope.Modules = [];
                     $scope.ModulesForDisplay = [];
                     if (response.data.length > 0) {
                         for (var i = 0; i < response.data.length; i++) {
                             $scope.Modules.push({
                                 Name: response.data[i].Name,
                                 IsAccess: false,
                                 Id:response.data[i].Id

                             })

                             $scope.ModulesForDisplay.push({
                                 Name: response.data[i].Name,
                                 IsAccess: false,
                                 Id: response.data[i].Id

                             })
                         }
                     }
                 })
                 .catch(function onError(error) {
                     NotificationService.Error("An error occured while updating the user");
                 });

            dataService.GetAllSalesPersonCategories()
                 .then(function onSuccess(response) {
            
                     $scope.Categories = [];
                     if (response.data.length > 0) {
                         for (var i = 0; i < response.data.length; i++) {
                             $scope.Categories.push({
                                 Name: response.data[i].Name,
                                 IsAccess: false,
                                 Id:response.data[i].Id
                                
                             })
                         }
                     }
                     
                 })
                 .catch(function onError(error) {
                     NotificationService.Error("An error occured while updating the user");
                 });
        };

        GetAdminUserManagement();
        function checkunrestrict(data) {
            return data.IsAccess == true;
        };

        $scope.CheckSelectAnyAccess=function() {
            if ($scope.Categories && $scope.Modules && $scope.ModulesForDisplay) {
                $scope.AnyCategories = $scope.Categories.some(function (item) {
                    return item.IsAccess;
                });
                $scope.AnyModules = $scope.Modules.some(function (item) {
                    return item.IsAccess;
                });
                //$scope.AnyDisplay = $scope.ModulesForDisplay.some(function (item) {
                //    return item;
                //});
                if ($scope.AnyCategories && $scope.AnyModules && $scope.currentDisplay || $scope.isUnrestrictedDisplay) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
         
        
        };


        $scope.createUser = function () {
            Metronic.blockUI({ boxed: true });
            $scope.ErrorMessage = "";
            if ($scope.userForm.$valid) {
                $scope.user["Categories"] = $scope.Categories;
                $scope.user["Modules"] = $scope.Modules;
                $scope.user.IsRestrictedModuleAccess = $scope.isUnrestricted;
                $scope.user.IsRestrictedCategoryAccess = $scope.isUnrestrictedcat;
              dataService.CreateUser($scope.user)
                    .then(function onSuccess(response) {
                        dataService.PendingRequest();
                        if (response.data && response.data && response.data.Error == "Mail Not sent") {
                            // User has been created but the mail is not sent to the user
                            NotificationService.Log("User has been created successfully but mail is not sent to the user");
                        }
                        else {
                            // Mail has been sent to the user
                            NotificationService.Success("User has been created successfully");
                        }
                        $scope.user = {};
                        addUserToList(response.data.User);

                        $state.reload();
                    })
                    .catch(function onError(error) {
                        dataService.PendingRequest();

                        $scope.ErrorMessage = error.data.ModelState[""][1];
                    
                    });
        }
    }

        $scope.onEditUser = function (data) {
          
            $scope.createuserbtnfn();
        $scope.user = angular.copy(data);
       
        Metronic.blockUI({ boxed: true });
        dataService.GetUserDetails(data.Id)
              .then(function onSuccess(response) {
                
                  Metronic.unblockUI();

                  $scope.currentDisplay = (response.data.DisplayModule) ? response.data.DisplayModule : "";

                  $scope.UserByIDData = response.data;

                  $scope.isUnrestricted = !response.data.IsRestrictedModuleAccess;


                  $scope.isUnrestrictedcat = !response.data.IsRestrictedCategoryAccess;

                  $scope.moduleacceschange($scope.isUnrestricted);
                  $scope.categoryacceschange($scope.isUnrestrictedcat);

                  if (response.data.Modules) {
                      if ($scope.Modules) {
                          for (var i = 0; i < $scope.Modules.length; i++) {
                              for (var z = 0; z < response.data.Modules.length; z++) {

                                  if (response.data.Modules[z].Name == $scope.Modules[i].Name) {

                                      $scope.Modules[i].IsAccess = true;

                                   


                                  }

                              }
                          }
                      }

                  }

                  if (response.data.Categories) {
                      if ($scope.Categories) {
                          for (var i = 0; i < $scope.Categories.length; i++) {
                              for (var z = 0; z < response.data.Categories.length; z++) {

                                  if (response.data.Categories[z].Name == $scope.Categories[i].Name) {

                                      $scope.Categories[i].IsAccess = true;

                                  }

                              }
                          }
                      }

                  }
               

                
                  if (response.data.DisplayModule) {
                      if ($scope.ModulesForDisplay) {
                          for (var i = 0; i < $scope.ModulesForDisplay.length; i++) {
                             

                                  if (response.data.DisplayModule.Name == $scope.ModulesForDisplay[i].Name) {

                                      $scope.ModulesForDisplay[i].IsAccess = true;
                                      $scope.isUnrestrictedDisplay = false;

                                  }

                          }
                      }

                  }
                  else{
                      $scope.isUnrestrictedDisplay = true;
                      $scope.currentDisplay = "";
                      for (var i = 0; i < $scope.ModulesForDisplay.length; i++) {
                      

                              $scope.ModulesForDisplay[i].IsAccess = false;

                      }

                  }








              })
              .catch(function onError(error) {
                  Metronic.unblockUI();
                  NotificationService.Error("An error occured while updating the user");
              });

        $scope.formTitle = 'Edit User';
    }

        $scope.moduleacceschangeDisplay = function (isUnrestrictedDisplay) {

            if (isUnrestrictedDisplay) {
                $scope.currentDisplay = "";
                for (var i = 0; i < $scope.ModulesForDisplay.length; i++) {


                    $scope.ModulesForDisplay[i].IsAccess = false;

                }
            }
        
        }

        $scope.Setdisplaymodule = function (mod) {
            for (var i = 0; i < $scope.ModulesForDisplay.length; i++) {
                if ($scope.ModulesForDisplay[i].Name == mod.Name) {
                    if ($scope.ModulesForDisplay[i].IsAccess == true) {

                    }
                    else {
                        $scope.ModulesForDisplay[i].IsAccess = true;
                    }
                }
                else {
                    $scope.ModulesForDisplay[i].IsAccess = false;
                }
            }

            $scope.currentDisplay = mod;

        };
    $scope.onDeleteUser = function (data) {
        alertify.confirm('Are you sure you want to delete this user?', function (result) {
            if (result)
                deleteUser(data.Id);
        });
    }

    $scope.onResetPswd = function (user) {
        
        $scope.isResetPswd = true;
        $scope.formTitle = "Change Password of " + user.FirstName + " " + user.LastName;
        $scope.resetModel = {
            UserId: user.Id,
            NewPassword: "",
            ConfirmPassword:""
        };
    }

    //$rootScope.test = function () {
    //    $scope.formTitle = ''; $scope.isResetPswd = false;
    //}

    $scope.updateUser = function () {
       
        if ($scope.userForm.$valid) {
            Metronic.blockUI({ boxed: true });
            $scope.user["Categories"] = $scope.Categories;
            $scope.user["Modules"] = $scope.Modules;
            $scope.user["DisplayModules"] = $scope.currentDisplay;
            $scope.user.IsRestrictedModuleAccess = $scope.isUnrestricted;
            $scope.user.IsRestrictedCategoryAccess = $scope.isUnrestrictedcat;
            dataService.UpdateUser($scope.user)
                    .then(function onSuccess(response) {
                        Metronic.unblockUI();
                        updateUsersList($scope.user);
                        NotificationService.Success("User has been updated successfully");
                        $state.reload();
                    })
                    .catch(function onError(error) {
                        Metronic.unblockUI();
                        NotificationService.Error("An error occured while updating the user");
                        //console.log("Error : " + error);
                    });
        }
    }

    $scope.resetPassword = function () {
      
        if ($scope.passwordResetForm.$valid && $scope.resetModel.NewPassword == $scope.resetModel.ConfirmPassword) {
            Metronic.blockUI({ boxed: true });
            authService.resetPassword($scope.resetModel)
                .then(function onSuccess(response) {
                    dataService.PendingRequest();
                    $scope.isResetPswd = false;
                    $scope.formTitle = '';
                    NotificationService.Success("Password has been updated successfully");
                    //NotificationService.Error("An error occured while sending the email to user.");
                })
                .catch(function onError() {
                    dataService.PendingRequest();
                    NotificationService.Error("An error occured while updating the password");
                });
        }
    }

    // End: Public Functions

    // Begin: Private Functions

    // Get all users in the system and populate it
    function loadUsers() {
        Metronic.blockUI({ boxed: true });
        dataService.GetAllUsers()
                .then(function (response) {
                   // dataService.PendingRequest();
                    if (response && response.data) {
                        Metronic.unblockUI();
                        $scope.userList = response.data;
                        $scope.safeUserList = response.data;
                    }
                })
                .catch(function () {
                    Metronic.unblockUI();
                    //dataService.PendingRequest();
                });
    }

    function updateUsersList(updatedUser) {
        for (var i = 0; i < $scope.userList.length; i++) {
            if ($scope.userList[i].Id == updatedUser.Id) {
                $scope.userList[i].FirstName = updatedUser.FirstName;
                $scope.userList[i].LastName = updatedUser.LastName;
                $scope.userList[i].Email = updatedUser.Email;
            }
        }
    }

    function removeUserFromList(removedUser) {
        var tempList = [];
        for (var i = 0; i < $scope.userList.length; i++) {
            if ($scope.userList[i].Id != removedUser) {
                tempList.push($scope.userList[i]);
            }
        }
        $scope.userList = angular.copy(tempList);
    }

    function addUserToList(user) {
        if ($scope.userList && $scope.userList.length >= 0)
            $scope.userList.push(user);
        else {
            $scope.userList = [];
            $scope.userList.push(user);
        }
    }

    function deleteUser(data) {
                 Metronic.blockUI({ boxed: true });
        dataService.DeleteUser(data)
                    .then(function onSuccess(response) {
                        dataService.PendingRequest();
                        removeUserFromList(data);
                        NotificationService.Success("You have successfully deleted the user");
                    })
                    .catch(function onError(error) {
                         dataService.PendingRequest();
                        console.error("Nogales Error : " + error);
                        NotificationService.Error("An error occured while deleting the user");
                    });
    }

    // End: Private Functions

    // Begin: Page Initialisation
    loadUsers();
    // End: Page Initialisation
}]);
