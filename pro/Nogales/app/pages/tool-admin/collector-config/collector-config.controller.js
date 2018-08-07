
MetronicApp.controller('CollectorConfigController', ['$scope', 'dataService', 'NotificationService', 'authService', '$state','Alertify',
    function ($scope, dataService, NotificationService, authService, $state, Alertify) {
        $scope.groupProperty = '';
        $scope.slide = false;
      
        $(document).ready(function () {

            var start;
            $("#tblReport tbody").sortable({
                disableSelection: true,
                placeholder: "ui-state-highlight",
                axis: "y",
             
                stop: function (e, ui) {
                    var end = ui.item.index();

                    var from = start <= end ? start : end,
                      to = start <= end ? end + 1 : start + 1;
                    $("#tblReport tbody > tr").slice(from, to).find('td:first').text(function (i) {
                        return from + i + 1;
                    })
                }
            });
        });


        $scope.sortableOptions = {
            'ui-floating': true,
            'ui-preserve-size': true,
            handle: '> .myHandle',
            start: function (e, ui) {
                start = ui.item.index();
                debugger
                if (ui.item.sortable.span) {
                    ui.item.sortable.cancel();
                }
            },
            update: function (e, ui) {
             
                var PrimaryOrdinance = ui.item.sortable.index;
                var PrimaryData = $scope.CollectData[PrimaryOrdinance];
                PrimaryData.Ordinance = ui.item.sortable.dropindex + 1

                //var SecondaryyOrdinance = ui.item.sortable.dropindex + 1;
                //var SecondaryyData = $scope.CollectData.filter(function (item) {
                //    return item.Ordinance == SecondaryyOrdinance;
                //})

                //PrimaryData[0].Ordinance = SecondaryyOrdinance;
                //SecondaryyData[0].Ordinance = PrimaryOrdinance;

                //UpdateCollector(PrimaryData[0]);
                //UpdateCollector(SecondaryyData[0])

                UpdateCollector(PrimaryData, "Ordinance Updated Successfully")
             
        
            }
        };
        $scope.onEditUser = function (user) {
            for (var i = 0; i < $scope.CollectData.length; i++) {
                if ($scope.CollectData[i].CollectorId == user.CollectorId) {
                    $scope.CollectData[i].isedit = true;
                    $scope.IndexData =angular.copy($scope.CollectData[i]);
                   
                }
                else {
                    $scope.CollectData[i].isedit = false;
                }
               
            }

        };
        $scope.callBackFunction1 = function (data) {
            
        };
        $scope.canceledit = function (user) {
          
            for (var i = 0; i < $scope.CollectData.length; i++) {
                if ($scope.CollectData[i].CollectorId == user.CollectorId) {

                    $scope.CollectData[i].isedit = false;
                    $scope.CollectData[i].CollectorName = $scope.IndexData.CollectorName;
                    
                    break;
                }
            }
        };
        $scope.setStyle = function (item) {
          
            var Item = !isNaN(item.LetterName);
           
            if (Item) {
                return {
                    //"color": "white",
                    "background": "#5bc0de",
                }
            }
            else {
                return {
                    //"color": "white",
                    "background": "#068fb8",
                }
            }
        };



        $scope.onUpdateUser = function (user,status) {
            debugger
            if (user.CollectorName == "") {
                NotificationService.Error("Collector name is required");
                return false;
            }
            if (status == "Add") {
                dataService.AddCollector(user.CollectorName)
               .then(function (response) {
                   if (response && response.data) {
                       debugger
                       Metronic.unblockUI();
                       GetAllCollectors();
                       GetAllUnAssignedCustomerPrefixes();
                       NotificationService.Success(response.data);
                   }
               })
               .catch(function () {
                   Metronic.unblockUI();
                   NotificationService.Error("Something Wrong!");
               });
            }
            else {
                debugger
                UpdateCollector(user);
            }
         
            //done later
        };

        function UpdateCollector(user,message) {
            dataService.UpdateCollector(user)
                 .then(function (response) {
                     if (response && response.data) {
                         debugger
                         Metronic.unblockUI();
                         GetAllCollectors();
                         GetAllUnAssignedCustomerPrefixes();
                         if (message) {
                             NotificationService.Success(message);
                         }
                         else {
                             NotificationService.Success(response.data);
                         }
                     }
                 })
         .catch(function (e) {
             Metronic.unblockUI();
             NotificationService.Error(e.data);
         });
        };

        $scope.onDeleteUser = function (user) {
            //done later
            $scope.slide = true;
            Alertify.confirm('Are you sure you want to delete '+user.CollectorName+'?', 'Default value').then(function (onOkParameter) {

                
                dataService.DeleteCollector(user.CollectorId)
              .then(function (response) {
                  if (response && response.data) {
                      debugger
                      Metronic.unblockUI();

                      for (var i = 0; i < $scope.CollectData.length; i++) {
                          if ($scope.CollectData[i].CollectorId == user.CollectorId) {
                              $scope.CollectData.splice(i, 1);

                              break;
                          }

                      }
                      GetAllCollectors();
                      GetAllUnAssignedCustomerPrefixes();
                      NotificationService.Success("Deleted Successfully");
                  }
              })
              .catch(function () {
                  Metronic.unblockUI();
                  NotificationService.Error("Something Wrong!");
              });


            }, function (can) {


            });


          
        }

        $scope.CreateCollector = function () {
            debugger
            var IsnewFound = false;
            for (var i = 0; i < $scope.CollectData.length; i++) {
                if ($scope.CollectData[i].isnew == true) {

                    IsnewFound = true;
                    break;
                }
             
            }

            if (!IsnewFound) {
                $scope.CollectData.push({
                    isedit: false,
                    CollectorId: null,
                    CollectorName: '',
                    isnew: true,
                    Ordinance: $scope.CollectData.length+1
                })
            }
        };

        $scope.cancelNewUser = function (user) {
            for (var i = 0; i < $scope.CollectData.length; i++) {
                if ($scope.CollectData[i].CollectorId == null) {
                    $scope.CollectData.splice(i, 1);
                    break;
                }
            }
        };
  
        // Limit items to be dropped in list1
        $scope.optionsList1 = {
            accept: function (dragEl) {
                if ($scope.list1.length >= 2) {
                    return false;
                } else {
                    return true;
                }
            }
        };



        function GetAllCollectors() {
            Metronic.blockUI({ boxed: true });
            dataService.GetAllCollectors()
                    .then(function (response) {
                        if (response && response.data) {
                          debugger
                            Metronic.unblockUI();
                            $scope.CollectData = response.data;
                            $scope.CollectDataSafe = angular.copy(response.data);

                            for (var i = 0; i < $scope.CollectData.length; i++) {
                                $scope.CollectData[i].isnew = false;
                                $scope.CollectDataSafe[i].isnew = false;

                                $scope.CollectData[i].isedit = false;
                                $scope.CollectDataSafe[i].isedit = false;
                            }


                           
                        }
                    })
                    .catch(function () {
                        Metronic.unblockUI();
                    });
        };
       
        function GetAllUnAssignedCustomerPrefixes() {
            Metronic.blockUI({ boxed: true });
            dataService.GetAllUnAssignedCustomerPrefixes()
                    .then(function (response) {
                        if (response && response.data) {
                            Metronic.unblockUI();

                            debugger

                            $scope.UnAssignedCustomerPrefixes = response.data;
                            $scope.UnAssignedCustomerPrefixesSafe =angular.copy(response.data);
                        }
                    })
                    .catch(function () {
                        Metronic.unblockUI();
                    });
        };



        GetAllCollectors();
        GetAllUnAssignedCustomerPrefixes();



        $scope.startCallback = function (event, ui, title) {
       
            $scope.draggedItem = title;
        };

        $scope.stopCallback = function (event, ui,title) {
   
        };

        $scope.dragCallback = function (event, ui,title) {
         
            $scope.CurrentFlyingData = title;
        };
        $scope.UnAssignCallback = function (event, ui, title) {
         
            // here the unassign (api) takes place
            var model = {
                CollectorAssignmentId: $scope.draggedItem.CollectorAssignmentId,
                LetterName: $scope.draggedItem.LetterName,
                CollectorId: null

            }
            AssignUnAssign(model);



        };
        function AssignUnAssign(model) {
            Metronic.blockUI({ boxed: true });
          
            dataService.AssignUnAssignCustomerPrefix(model)
                    .then(function (response) {
                        if (response && response.data) {

                            Metronic.unblockUI();
                            // NotificationService.Success(response.data);
                            $scope.slide = false;
                            //GetAllCollectors();
                            //GetAllUnAssignedCustomerPrefixes();
                        }
                    })
                    .catch(function () {
                        Metronic.unblockUI();
                        NotificationService.Error("Something Wrong!");
                    });
        }

        $scope.dropCallback = function (event, ui, data, call) {
            debugger
            var a = $scope.CollectData;
           
            if (ui.draggable["0"].className == "btn-draggable prettybutton repeat-item ng-pristine ng-untouched ng-valid ui-draggable ui-draggable-handle" || ui.draggable["0"].className == "btn-draggable prettybutton1 ng-pristine ng-untouched ng-valid ui-draggable ui-draggable-handle"
) {

                //here the Assign takes place (api)
                debugger
                if ($scope.draggedItem) {
                    var model = {
                        CollectorAssignmentId: $scope.draggedItem.CollectorAssignmentId,
                        LetterName: $scope.draggedItem.LetterName,
                        CollectorId: data.CollectorId
                    };
                    AssignUnAssign(model);

                } else {
                   

                }
            }
            else {
                debugger
                //for (var i = 0; i < $scope.CollectData.length; i++) {
                //    var Ordinance = i + 1;
                //    $scope.CollectData[i].Ordinance = Ordinance;
                //}
                //audinance update
          
                //UpdateCollector(PrimaryData[0]);
                //UpdateCollector(SecondaryyData[0])


            }


        };

        $scope.overCallback = function (event, ui,title) {
            console.log('Look, I`m over you');
        };

        $scope.outCallback = function (event, ui,title) {
            console.log('I`m not, hehe');
        };
    }]);
