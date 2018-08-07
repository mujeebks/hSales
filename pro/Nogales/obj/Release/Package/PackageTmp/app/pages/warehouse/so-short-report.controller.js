
'use strict';

MetronicApp.controller('WarehouseSoShortReportController', ['$rootScope', '$scope', 'dataService', '$stateParams', '$filter', 'NotificationService', 'commonService', '$controller',
    function ($rootScope, $scope, dataService, $stateParams, $filter, NotificationService, commonService, $controller) {
        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();
        });
        $scope.iscanceldisabled = true;
        $controller('BaseController', { $scope: $scope });
        $scope.EMAIL_REGX = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

        // Begin: Public variables
        var httpRequest = null;
        $scope.filter = {
            routeNumber: "",
            buyerId: "",
            shipDate: "",
        };
        $scope.searchClicked = false;

        //$scope.fullReportData = []; //which include notified as well as unnotified data
        $scope.reportData = [];
        $scope.reportDataSafe = [];
        //  $scope.reportGroupBy = "";
        $scope.groupProperty = '';

        //sendNotification models
        //$scope.datatable_selected_documents = [];
        $scope.datatable_selected_ids = [];
        $scope.datatable_functions = {};

        $scope.datatable_selected_mail_ids = [];

        $scope.datatable_showNotified = false;

        $scope.ignoredReason = "";
        //  End: Public variables

        $scope.unNotifiedReportData = null;

        $scope.selectedMailIdsToSendNotification = [];


        //initializing button spinner  $scope.initBtnSpinner().start(); to start and
        //$scope.initBtnSpinner().stop() to stop the spinner
        $scope.btnSpinner = commonService.InitBtnSpinner('#search-so-short-report');

        // Currently not using.
        $scope.loadCharts = function () {
            Metronic.startPageLoading({ message: 'Loading Dashboard charts...' });
            dataService.getWarehouseDashboardData($filter('date')(new Date(), 'yyyy-MM-dd')).then(function (response) {
                try {
                    //$scope.casesSoldData = response.data.CasesSoldTotals;
                } catch (e) {
                    NotificationService.Error();
                    NotificationService.ConsoleLog('Error while assigning the data from the API to the charts.');
                }
                finally {
                    Metronic.stopPageLoading();
                }
            }, function onError() {
                Metronic.stopPageLoading();
                NotificationService.Error("Error upon the API request");
                NotificationService.ConsoleLog('Error on the server');
            });
        }


        $scope.resetSearch = function () {
            $scope.myTableFunctions.resetSearch();
            $scope.groupProperty = '';

            $scope.filter.routeNumber = "";
            $scope.filter.buyerId = "";
         //   $scope.filter.shipDate = null;

            $scope.reportData = [];
            $scope.reportDataSafe = [];

            $scope.datatable_selected_ids = [];

            $scope.datatable_selected_mail_ids = [];

            //$scope.fullReportData = [];
            $scope.datatable_showNotified = false;

            generateDefaultReport();
        };
        $scope.abortExecutingApi = function () {
            //$scope.searchStatus.isSearched = true;
            $scope.iscanceldisabled = true;
            Metronic.stopPageLoading();
            return (httpRequest && httpRequest.abortCall());
        };
        $('#shipDate').datepicker();

        $scope.getCsvHeader = function () {
            return ['Route', 'Customer', 'Item', 'Description', 'Buyer', 'UoM', 'Qty Ordered', 'Qty Available', 'Qty On Hand', 'Trans. Cost', 'Mkt. Price',
               'SO Number'];
        }
        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            predefinedHeader = ["Route", "Customer", "Item", "Description", "Buyer", "UOM", "QuantityOrd", "QtyAvailable", "QuantityOnHand",
              "TransactionCost", "MarketPrice", "SalesOrderNumber"];

            angular.forEach($scope.reportDataSafe, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = value;
                    }

                });
                if (header != undefined)
                    array.push(header);
            });
            return array;
        };

        $scope.openModal = function (elementId) {
            if (elementId && elementId != '') {
                $('#' + elementId).modal('show');
            }
        }
        $scope.closeModal = function (elementId) {
            if (elementId && elementId != '') {
                $('#' + elementId).modal('hide');
            }
        }
        $scope.getMailIds = function () {
            Metronic.startPageLoading();
            $('#mailIds').tagsinput('removeAll');//removing all tags before new tags are come in
            dataService.GetMailIds().then(function (response) {
                Metronic.stopPageLoading();
                if (response && response.data) {
                    //$scope.mailIds = response.data;
                    $scope.datatable_selected_mail_ids = $scope.getSelectedMailIdsWrtSelectedIds();
                    $scope.mailIds = response.data.concat(getUniqueMailIds($scope.datatable_selected_mail_ids));
                    $scope.selectedMailIdsToSendNotification = $scope.mailIds;

                    applyStyleToCheckBox();

                    //var mailIds = response.data;
                    //if (mailIds) {
                    //    $('#mailIds').tagsinput();
                    //    for (var i = 0; i < mailIds.length; i++) {
                    //        $('#mailIds').tagsinput('add', mailIds[i]);
                    //    }
                    //}

                }
            }, function onError() {
                Metronic.stopPageLoading();
                NotificationService.Error("Error upon the API request");
                NotificationService.ConsoleLog('Error on the server');
            });

            //$scope.datatable_selected_mail_ids = $scope.getSelectedMailIdsWrtSelectedIds();

            //$scope.setSelectedCustomerMailIdsToTagsInput($scope.datatable_selected_mail_ids);
        }
        $scope.getSelectedMailIdsWrtSelectedIds = function(){
            var mailIds = [];
            for (var i = 0; i < $scope.datatable_selected_ids.length;i++){
                angular.forEach($scope.reportDataSafe,function(value,index){
                    if (value.Id == $scope.datatable_selected_ids[i]){
                        mailIds.push(value.Email);
                    }
                })
            }
            return mailIds;
        }
        //$scope.setSelectedCustomerMailIdsToTagsInput = function (mailIds) {
        //    angular.forEach(mailIds, function (value, index) {
        //        if ($scope.EMAIL_REGX.test(value)) {
        //            $('#mailIds').tagsinput('add', value);
        //        }
        //    })
        //}
        $scope.sendNotification = function () {
            Metronic.startPageLoading();
            var params = {
                routeNumber: $scope.filter.routeNumber,
                buyerId: $scope.filter.buyerId,
                shipDate: $scope.filter.shipDate,
                //EmailTos: $("#mailIds").tagsinput('items'),
                EmailTos: $scope.selectedMailIdsToSendNotification,
                Ids: $scope.datatable_selected_ids
            };
            dataService.SendNotification(params).then(function (response) {
                $scope.unNotifiedReportData = $scope.removeNotifiedRowsFromUnNotifieddata($scope.unNotifiedReportData, $scope.datatable_selected_ids);

                $scope.reportData = $scope.unNotifiedReportData;
                $scope.reportDataSafe = $scope.unNotifiedReportData;
                $scope.totalNumberOfRecords = $scope.unNotifiedReportData.length;

                $scope.numberOfSelectedRows = 0;
                $scope.datatable_selected_ids = [];

                Metronic.stopPageLoading();
                $scope.closeModal('sendNotification');
                NotificationService.Success("Successfully sent notification");

            }, function onError() {
                Metronic.stopPageLoading();
                $scope.closeModal('sendNotification');
                NotificationService.Error("Error upon the send notification");
                NotificationService.ConsoleLog('Error on the server');
            });

        }
        $scope.datatable_functions.updateSelectedIds = function () {
            $scope.datatable_selected_ids = [];
            if ($scope.datatable_select_all) {
                //$scope.numberOfSelectedRows = $scope.totalNumberOfRecords;
                angular.forEach($scope.reportDataSafe, function (value, index) {
                    $scope.datatable_selected_ids.push(value.Id);
                });
                $scope.numberOfSelectedRows = $scope.totalNumberOfRecords;
            }
            else {
                $scope.numberOfSelectedRows = 0;
            }

         //   $scope.updateSelectedMailIds();
        }
        $scope.datatable_functions.changeSelectedIds = function (id, itemNo) {
            var index = $scope.datatable_selected_ids.indexOf(id);
            if (index == -1) {
                $scope.datatable_selected_ids.push(id);
                $scope.numberOfSelectedRows++;

                $scope.selectRemainingIdsWithSameItemNo(itemNo);
            }
            else {
                $scope.datatable_selected_ids.splice(index, 1);
                $scope.numberOfSelectedRows--;
            }
            //to check or uncheck header checkbox(check/uncheck all check box)
            ($scope.numberOfSelectedRows == $scope.totalNumberOfRecords) ?
              $('#selectAllCheckBox').parent().addClass('checked') : $('#selectAllCheckBox').parent().removeClass('checked');
            $scope.datatable_select_all = false;

           // $scope.changeSelectedMailIds(mailId);

        }
        $scope.selectRemainingIdsWithSameItemNo = function (itemNo) {
            var object_ids = $scope.findIdsWithSameItemNumber(itemNo);
            for (var i = 0; i < object_ids.length; i++) {
                if ($scope.datatable_selected_ids.indexOf(object_ids[i].Id) == -1) {
                    $scope.datatable_functions.changeSelectedIds(object_ids[i].Id, object_ids[i].Mail, object_ids[i].Item);
                }
            }
        }
        $scope.findIdsWithSameItemNumber = function (itemNo) {
            var object_ids = [];
            angular.forEach($scope.reportDataSafe, function (value, index) {
                if (value.Item == itemNo) {
                    object_ids.push(value);
                }
            });
            return object_ids;
        }



        //$scope.getUnNotifiedData = function (data) {
        //    var unNotifiedData = [];
        //    angular.forEach(data, function (value, index) {
        //        if (value.Flag == false) {
        //            unNotifiedData.push(value)
        //        }
        //    })
        //    return unNotifiedData;
        //}
        //$scope.getNotifiedData = function(data){
        //    var notifiedData = [];
        //    angular.forEach(data,function(value,index){
        //        if (value.Flag == true) {
        //            notifiedData.push(value);
        //        };
        //    })
        //    return notifiedData;
        //}
        //$scope.setNotifiedObjectsInfullreportData = function (fullreportdata, selectedIds) {
        //    angular.forEach(selectedIds, function (id, inIndex) {
        //        angular.forEach(fullreportdata, function (value, index) {
        //            if (value.Id == id) {
        //                value.Flag = true;
        //            }
        //        });
        //    })
        //    return fullreportdata;
        //}
        $scope.showNotifiedOrUnNotifiedData = function (fullReportData) {
            $scope.reportData = [];
            $scope.reportDataSafe = [];
            $scope.datatable_selected_ids = [];



            if ($scope.datatable_showNotified) {
                $scope.showNotifiedData();
            } else {
                if ($scope.unNotifiedReportData) {
                    $scope.reportData = $scope.unNotifiedReportData;
                    $scope.reportDataSafe = $scope.unNotifiedReportData;
                    $scope.totalNumberOfRecords = $scope.unNotifiedReportData.length;

                    $scope.numberOfSelectedRows = 0;
                    $scope.datatable_selected_ids = [];
                }
                else {
                    $scope.showUnNotifiedData();
                }
            }
            $('#selectAllCheckBox').parent().removeClass('checked');
            $scope.datatable_select_all = false;
        }

        $scope.sendIgnoredreason = function () {
            Metronic.startPageLoading();
            var params = {
                Ids: $scope.datatable_selected_ids,
                shipDate: $scope.filter.shipDate,
                IgnoredReason: $scope.ignoredReason,
            };
            dataService.SendIgnoredreason(params).then(function (response) {
                //$scope.fullReportData = $scope.setNotifiedObjectsInfullreportData($scope.fullReportData, $scope.datatable_selected_ids);
                //$scope.showNotifiedOrUnNotifiedData($scope.fullReportData);
                //$scope.datatable_selected_ids = [];
                //$scope.ignoredReason = "";

                //Metronic.stopPageLoading();
                //$scope.closeModal('ignoredReason');
                //NotificationService.Success("Successfully sent Ignored Reason");

                $scope.unNotifiedReportData = $scope.removeNotifiedRowsFromUnNotifieddata($scope.unNotifiedReportData, $scope.datatable_selected_ids);
                $scope.reportData = $scope.unNotifiedReportData;
                $scope.reportDataSafe = $scope.unNotifiedReportData;
                $scope.totalNumberOfRecords = $scope.unNotifiedReportData.length;

                $scope.numberOfSelectedRows = 0;
                $scope.datatable_selected_ids = [];
                $scope.ignoredReason = "";

                Metronic.stopPageLoading();
                $scope.closeModal('ignoredReason');
                NotificationService.Success("Successfully sent Ignored Reason");

            }, function onError() {
                Metronic.stopPageLoading();
                $scope.closeModal('ignoredReason');
                NotificationService.Error("Error upon the send Ignored Reason");
                NotificationService.ConsoleLog('Error on the server');
            });
        }
        $scope.showNotifiedData = function () {

            $scope.btnSpinner.start();
            (httpRequest = dataService.GenerateWarehouseNotifiedIgnoredShortReport($scope.filter))
                .then(function onSuccess(response) {
                    //$scope.unNotifiedReportData = response.data;
                    //response.data = $scope.getDummyNotifiedData();
                    if (response) {
                        $scope.iscanceldisabled = true;
                        $scope.reportData = response.data;
                        $scope.reportDataSafe = response.data;
                        $scope.totalNumberOfRecords = response.data.length;

                        $scope.numberOfSelectedRows = 0;
                        $scope.datatable_selected_ids = [];
                    }
                    $scope.btnSpinner.stop();
                })
                .catch(function (exce) {
                    //if (exce && exce.data && exce.data.ExceptionMessage) {
                    //    NotificationService.Error(exce.data.ExceptionMessage);
                    //    NotificationService.ConsoleLog(exce.data.ExceptionMessage);
                    //}
                    //else {
                    //    NotificationService.Error("An error occured while generating the report");
                    //    NotificationService.ConsoleLog('API Error upon dataService.generateWarehouseReport()');
                    //}
                    NotificationService.Error("An error occured while generating the report");
                    NotificationService.ConsoleLog('API Error upon dataService.generateWarehouseWmsPickerProductivityReport()');
                    $scope.btnSpinner.stop();

                    $scope.reportData = [];
                    $scope.reportDataSafe = [];
                    $scope.datatable_selected_ids = [];


                    $scope.btnSpinner.stop();
                });
        }
        $scope.showUnNotifiedData = function () {

            $scope.btnSpinner.start();
            (httpRequest = dataService.GenerateWarehouseShortReport($scope.filter))
                .then(function onSuccess(response) {
                    //response.data = $scope.getDummyData();
                    //$scope.unNotifiedReportData = response.data;
                    if (response) {
                        $scope.iscanceldisabled = true;
                        $scope.unNotifiedReportData = (response.data.length > 0) ? response.data : null;

                        $scope.reportData = $scope.unNotifiedReportData;
                        $scope.reportDataSafe = $scope.unNotifiedReportData;
                        $scope.totalNumberOfRecords = ($scope.unNotifiedReportData) ? $scope.unNotifiedReportData.length : 0;

                        $scope.numberOfSelectedRows = 0;
                        $scope.datatable_selected_ids = [];
                    }
                    $scope.btnSpinner.stop();
                })
            //.catch(function onError(exce) {
            .catch(function (exce) {
                    //if (exce && exce.data && exce.data.ExceptionMessage) {
                    //    NotificationService.Error(exce.data.ExceptionMessage);
                    //    NotificationService.ConsoleLog(exce.data.ExceptionMessage);
                    //}
                    //else {
                    //    NotificationService.Error("An error occured while generating the report");
                    //    NotificationService.ConsoleLog('API Error upon dataService.generateWarehouseReport()');
                    //}
                //$scope.btnSpinner.stop();
                NotificationService.Error("An error occured while generating the report");
                NotificationService.ConsoleLog('API Error upon dataService.generateWarehouseWmsPickerProductivityReport()');
                $scope.btnSpinner.stop();
                });
        }
        $scope.removeNotifiedRowsFromUnNotifieddata = function (data, selectedIds) {
            var newData = [];
            angular.forEach(selectedIds, function (id, inIndex) {
                angular.forEach(data, function (value, index) {
                    if (value.Id != id) {
                        newData.push(value);
                    }
                });
            })
            return newData;
        }

        // Generate report
        $scope.search = function () {
            $scope.iscanceldisabled = false;
            $scope.searchClicked = true;
            if ($scope.isDate($scope.filter.shipDate)) {
                if ($scope.datatable_showNotified) {
                    $scope.showNotifiedData();
                }
                else {
                    $scope.showUnNotifiedData();
                }
            }

        }

        // Fn. to generate the report upon page load
        function generateDefaultReport() {
            $scope.filter.shipDate = getToday();
            $scope.search();
        }

        // Return today's date in MM/dd/yyyy format
        function getToday() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var today = mm + '/' + dd + '/' + yyyy;

            return today;
        }

        // Call the fn. to populate the report with today's date
        generateDefaultReport();

        function applyStyleToCheckBox() {
            setTimeout(function () {
                $('.mcheck').iCheck({
                    checkboxClass: 'icheckbox_square-blue',

                });


                $('input').on('ifChecked', function (event) {
                    //alert($(this).val());
                    addMailIdToSelectedMailIdsToSendNotificationIfNot($(this).val());
                });
                $('input').on('ifUnchecked', function (event) {
                    //alert(123);
                    removeMailIdFromSelectedMailIdsToSendNotification($(this).val());
                });


            })
        }
        function getUniqueMailIds(mailIds) {
            var tempIds = [];
            for (var i = 0; i < mailIds.length; i++) {
                if (tempIds.indexOf(mailIds[i]) < 0 && ($scope.EMAIL_REGX.test(mailIds[i]))) {
                    tempIds.push(mailIds[i]);
                }
            }
            return tempIds;
        }
        function addMailIdToSelectedMailIdsToSendNotificationIfNot(mailId) {
            if ($scope.selectedMailIdsToSendNotification.indexOf(mailId) < 0) {
                $scope.selectedMailIdsToSendNotification.push(mailId);
            }
        }
        function removeMailIdFromSelectedMailIdsToSendNotification(mailId) {
            if ($scope.selectedMailIdsToSendNotification.indexOf(mailId) > 0) {
                $scope.selectedMailIdsToSendNotification.splice($scope.selectedMailIdsToSendNotification.indexOf(mailId), 1);
            }
        }




        //$scope.mailIds = ["amal@vofoxsolutions.com", "amale@vofoxsolutions.com", "amalp@vofoxsolutions.com",
        //    "amalp@vofoxsolutions1.com"
        //];
        $scope.abc = function () {
           // alert(1);
        }

        $('.icheck').on('click', function () {

           // alert($(this).val());


        });

    }]);
