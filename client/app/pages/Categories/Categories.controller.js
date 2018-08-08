'use strict';
MetronicApp.controller('CategoryController', ['$scope', 'dataService', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller','HelperService','$mdDialog',
    function ($scope, dataService, $filter, NotificationService, ApiUrl, commonService, $controller, HelperService, $mdDialog) {
        
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initAjax();
        });
        //$scope.iscanceldisabled = true;
        $scope.pageSize = 20;
        $controller('BaseController', { $scope: $scope });
      //  $scope.btnSpinner = commonService.InitBtnSpinner('#search');
        $scope.casessoldcsv = "";
        var httpRequest = null;
        $scope.myTableFunctions = {};
        $scope.salesPersonTableFunctions = {};
        $scope.groupProperty = '';//in the smart table default value of group by property is ""
       // $scope.searchClicked = false;
       // $scope.selectedSalesPerson = '';
        //$scope.loadDatePickers = function () {
        //    $('#scheduledLoadStartDate').datepicker({
        //        rtl: Metronic.isRTL(),
        //        orientation: "left",
        //        autoclose: true,
        //        todayHighlight: true,
        //    });
        //    $('#scheduledLoadEndDate').datepicker({
        //        rtl: Metronic.isRTL(),
        //        orientation: "left",
        //        autoclose: true,
        //        todayHighlight: true,
        //    });

        //    $('#scheduledLoadStartDate1').datepicker({
        //        rtl: Metronic.isRTL(),
        //        orientation: "left",
        //        autoclose: true,
        //        todayHighlight: true,
        //    });
        //    $('#scheduledLoadEndDate1').datepicker({
        //        rtl: Metronic.isRTL(),
        //        orientation: "left",
        //        autoclose: true,
        //        todayHighlight: true,
        //    });
        //}
        //$scope.loadDatePickers();

      //function GetCasesSoldReportPrerequisites()  {
      //    dataService.GetCasesSoldReportPrerequisites().then(function (response) {
             
      //         $scope.Commodity = [];
      //         $scope.FoodService = [];
      //         $.each(response.data[0], function (key, value) {
      //             $scope.Commodity.push({
      //                 key: key,
      //                 value: value
      //             })
      //         });

      //         var categoriesData = localStorage.getItem('ls.nogalesAuthAccess');
      //         categoriesData = JSON.parse(categoriesData);
      //         categoriesData = categoriesData.Categories;

      //         $scope.FoodService.push({
      //             key: 'All',
      //             value: 'All'
      //         })

      //        $.each(categoriesData, function (key, value) {
      //             $scope.FoodService.push({
      //                 key: key,
      //                 value: value.Name
      //             })
      //        });
      //        if (categoriesData.length > 0) {
      //            for (var i = 0; i < $scope.FoodService.length; i++) {
      //                for (var z = 0; z < categoriesData.length; z++) {
      //                    if (categoriesData[z].Name == $scope.FoodService[i].value) {
      //                        if (categoriesData[z].IsAccess == false) {
      //                            $scope.FoodService.splice(i, 1);
      //                        }
      //                    }
      //                }
      //            }
      //        }
            

      //         $scope.Commodityselected = $scope.Commodity[0];
      //         $scope.FoodServiceselected = $scope.FoodService[0];
      //         loadsalesPersons();
      //     });
      //  }
       // GetCasesSoldReportPrerequisites();

     

      $scope.search = function (filter) {

          (httpRequest = dataService.GetCategories())
              .then(function (response) {
                  if (response) {
                      debugger
                      $scope.Data = response.data;
                      $scope.DataMaster =angular.copy(response.data);

                  }

              })
              .catch(function (error) {
              });


      };
     
        //$scope.resetSearch = function () {
        //    $scope.myTableFunctions.resetSearch();
        //    $scope.groupProperty = '';
        //    $scope.selectedSalesPersons = [];
        //    $scope.MinSalesAmt = 100;
        //    $scope.salesReport = [];
        //    $scope.salesReportMaster = [];
        //    $scope.Commodityselected = $scope.Commodity[0];
        //    $scope.FoodServiceselected = $scope.FoodService[0];
        //    loadsalesPersons();
        //    $scope.toggleyear = false;
        //    $scope.CurrentFilter = "";
        //};
        //$scope.abortExecutingApi = function () {
        //    $scope.iscanceldisabled = true;
        //    try { return (httpRequest && httpRequest.abortCall());}
        //    catch (e) {}
        //};


        $scope.search()

        $scope.getCsvHeader = function () {
            return ['Customer Name', 'Cases Sold Prior', 'Cases Sold (Current)','Difference', 'Difference(%)'];
        }

        $scope.getCsvData = function () {
            var array = [];
            var predefinedHeader = [];
            var current = 0;
            var previous = 0;
            predefinedHeader = ["Customer", "Previous", "Current", "Difference", "PercentageDifference"];

            angular.forEach($scope.salesReportMaster, function (value, key) {
                var header = {};
                angular.forEach(value, function (value, key) {
                    var index = predefinedHeader.indexOf(key);
                    if (index > -1) {
                        header[index] = (key == "Current" || key == "Previous") ? $filter('numberWithCommas')(value) : value;
                    }
                    //if (key == "Current") {
                    //    current = current + value;
                    //}
                    //if (key == "Previous") {
                    //    previous = previous + value;
                    //}
                });
                if (header != undefined)
                    array.push(header);
            });

            var total = {};
            total[0] = "Total";
            total[1] = $scope.TotalPrevious;
            total[2] = $scope.TotalCurrent;
            array.push(total);
            return array;
        };
        $scope.test = ""
        $scope.cancel = function () {
            $mdDialog.cancel();
        }
        $scope.showAdvanced = function (ev) {
            $mdDialog.show({
                controller: 'CategoryController',
                scope: $scope,        // use parent scope in template
                preserveScope: true,
                templateUrl: 'app/pages/Categories/dialog1.tmpl.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: false,
                fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
            })
            .then(function (answer) {
                $scope.status = 'You said the information was "' + answer + '".';
            }, function () {
                $scope.status = 'You cancelled the dialog.';
            });
        };


        $scope.submitted = false;

        // function to submit the form after all validation has occurred			
        $scope.submitForm = function () {

            $scope.submitted = true;

            // check to make sure the form is completely valid
            if ($scope.userForm.$valid) {
                alert('our form is amazing');
            }
            else {
                alert('ERROR');
            }

        };


        //function generateDefaultReport() {
        //    var date = new Date();
        //    $scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
        //    $scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
        //    $scope.search();
        //};


    
        //$scope.getCustomersSalesDataReport = function (salesPerson) {
        //    $scope.selectedSalesPerson = salesPerson;
        //    $scope.customersData = [];
        //    $scope.customersDataSafe = [];
        //    dataService.GetSalesPersonCustomersDataOfSalesReport(salesPerson, $scope.beginDate, $scope.endDate).then(function (response) {
        //        if (response && response.data) {
        //            $scope.customersData = response.data;
        //            $scope.customersDataSafe = angular.copy($scope.customersData);
        //        }
        //    }, function onError() {
        //        stopDivLoader("salesPersonReportPortletBody");
        //        NotificationService.Error("Error upon the API request");
        //    });
        //}

        //function loadsalesPersons() {
        //    dataService.GetAllSalesPersonsForFiltering().then(function (response) {
        //      $scope.salesPersonList = [];
        //      for (var i = 0; i < response.data.length; i++) {
        //          var defaultfilter = {
        //              "AssignedPersonList": response.data[i].AssignedPersonList,
        //              "Category": response.data[i].Category,
        //              "Id": i,
        //              "SalesPersonCode": response.data[i].SalesPersonCode,
        //              "SalesPersonDescription": response.data[i].SalesPersonDescription
        //          };
        //          $scope.salesPersonList.push(defaultfilter)
        //      }

        //      var defaultfilter = {
        //          "AssignedPersonList": "All",
        //          "Category": "All",
        //          "Id": $scope.salesPersonList.length,
        //          "SalesPersonCode": "All",
        //          "SalesPersonDescription": "All"
        //      };
        //      $scope.salesPersonList.push(defaultfilter);
        //      $scope.salesPersonList = $scope.salesPersonList.reverse();
        //      $scope.salespersonitem = $scope.salesPersonList[0];
        //      generateDefaultReport();
        //  });
        //};
    }]);
