'use strict';
MetronicApp.controller('SalesPersonMapController', ['$scope', 'dataService', '$stateParams', '$filter',
    'NotificationService', 'ApiUrl', 'commonService', '$controller',
    function ($scope, dataService, $stateParams, $filter, NotificationService, ApiUrl, commonService, $controller) {

    	$scope.$on('$viewContentLoaded', function () {

    		Metronic.initAjax();
    	});

    	$scope.iscanceldisabled = true;
    	$controller('BaseController', { $scope: $scope });

    	$scope.isnewMasterSalesPersons = false;
    	$scope.isnewSalesPersons = false;
    	//$scope.isnewMasterSalesPerson = false;
    	$scope.btnSpinner = commonService.InitBtnSpinner('#search');
    	$scope.btnSpinner.start();

    	var httpRequest = null;
    	$scope.myTableFunctions = {};
    	$scope.salesPersonTableFunctions = {};
    	$scope.groupProperty = '';//in the smart table default value of group by property is ""
    	$scope.searchClicked = false;
    	$scope.AllsalesPersons = [];
    	$scope.isEdit = false;
    	$scope.editDis = function(index)
    	{
            $scope.index = index
    	    $scope.isEdit = true;
    	}

    	$scope.selectedSalesPerson = '';
    	$scope.editsDescription = {};
    	$scope.UpdateSalesPersonDescription = function (editsDescription, id, index, isEdit)
    	{



    	    $scope.index = index;
    	    $scope.isEdit = false;
    	    editsDescription["Id"] = id;
    	    dataService.UpdateSalesPersonDescription(editsDescription).success(function (data) {

    	        $scope.LoadAllSalesPerson();
    	        //$scope.AllsalesPersons = data;

    	    }).error(function (e) {

    	        NotificationService.Error(e);
    	    });

    	}

    	$scope.editCancel = function(index,isEdit)
    	{
    	    $scope.index = index;
    	    $scope.isEdit = false;

    	}

    	$scope.loadDatePickers = function () {
    		$('#scheduledLoadStartDate').datepicker({
    			rtl: Metronic.isRTL(),
    			orientation: "left",
    			autoclose: true,
    			todayHighlight: true,
    		});
    		$('#scheduledLoadEndDate').datepicker({
    			rtl: Metronic.isRTL(),
    			orientation: "left",
    			autoclose: true,
    			todayHighlight: true,
    		});
    		$('#assignNewScheduledLoadStartDate').datepicker({
    			rtl: Metronic.isRTL(),
    			orientation: "left",
    			autoclose: true,
    			todayHighlight: true,
    		});
    		$('#assignNewScheduledLoadEndDate').datepicker({
    			rtl: Metronic.isRTL(),
    			orientation: "left",
    			autoclose: true,
    			todayHighlight: true,
    		});
    		$('#unAssignLoadDate').datepicker({
    			rtl: Metronic.isRTL(),
    			orientation: "left",
    			autoclose: true,
    			todayHighlight: true,
    		});

    	}
    	$scope.loadDatePickers();

    	$scope.getSalesFilters = function () {
    		dataService.getSalesReportFilters().success(function (response) {

    			$scope.availableSalesPersons = response.ListSalesPerson;
    			$scope.availableSalesPersons1 = angular.copy(response.ListSalesPerson);
    			$scope.available_Sales_Persons = angular.copy(response.ListSalesPerson);

    		}).error(function (err) {
    			console.log(err)
    		})
    	}
    	$scope.getSalesFilters();

    	$scope.LoadAllSalesPerson = function () {
    		(httpRequest = dataService.loadAllSalesPerson()).then(function (response) {
    			if (response) {
    				$scope.AllsalesPersons = response.data;
    				$scope.AllsalesPersonsSafe =angular.copy(response.data);

    			}
    		}).catch(function (error) {
    			var a = error;
    		});

    	};
    	$scope.LoadAllSalesPerson();


    	function GetFiltersForSalesMapping() {
    		dataService.GetFiltersForSalesMapping().success(function (response) {

    			
    			//$scope.FiltersForSalesMapping = response.ListSalesPerson;
    			//$scope.FiltersForSalesMapping = angular.copy(response.ListSalesPerson);
    			//$scope.FiltersForSalesMapping = angular.copy(response.ListSalesPerson);

    			$scope.MasterSalesPersons =angular.copy(response.MasterSalesPersons);
    			$scope.SalesPersons = response.SalesPersons;
    			//console.log(response);
    			$scope.TrackingGroups = response.TrackingGroups;




    		}).error(function (err) {
    			//console.log(err)
    		})
    	};

    	GetFiltersForSalesMapping();



    	$scope.Isnew = function (status, type) {
    		
    		if (status == "0") {
    			


    			switch (type) {
    				case "MasterSalesPersons":
    					$scope.isnewMasterSalesPersons = true;


    					break;
    				case "SalesPersons":

    					$scope.isnewSalesPersons = true;

    					break;

    			}



    		}
    		else {


    			switch (type) {
    				case "MasterSalesPersons":
    					$scope.isnewMasterSalesPersons = false;




    					break;
    				case "SalesPersons":

    					$scope.isnewSalesPersons = false;
    					$scope.SalesPersonsDescriptionScope = status.Description;
    					break;

    			}
    		}

    	};


    	$scope.checkExcisting = function (modelScope,type) {

    		if (type == "MasterSalesPersons") {

    			for (var i = 0; i < $scope.MasterSalesPersons.length; i++) {

    				if (modelScope == $scope.MasterSalesPersons[i].Code) {
    					$scope.MasterSalesPersonsDescriptionScope = $scope.MasterSalesPersons[i].Description;
    				}

    			}



    		}
    		if (type == "SalesPersons") {
    			for (var i = 0; i < $scope.SalesPersons.length; i++) {
    				if (modelScope == $scope.SalesPersons[i].Code) {
    					$scope.SalesPersonsDescriptionScope = $scope.SalesPersons[i].Description;
    				}

    			}
    		}

    	};

    	function getQuarter(d) {
    		d = d || new Date();
    		var m = Math.floor(d.getMonth() / 3) + 1;
    		return m > 4 ? m - 4 : m;
    	}

    	$scope.search = function (filter, selectedSales) {
    	    $scope.searchClicked = true;
    	    $scope.iscanceldisabled = false;
    	    $scope.btnSpinner.start();
    		var status = false;
    		var date = new Date();
    		var firstDay;
    		var lastDay;
    		if (filter == 'ThisMonth') {
    			firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    			lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    		}
    		else if (filter == 'LastMonth') {
    			var month = date.getMonth();
    			var year;
    			if (month < 12)
    				year = date.getFullYear();
    			else
    				year = date.getFullYear() - 1;
    			firstDay = new Date(year, month - 1, 1);
    			lastDay = new Date(year, month, 0);
    		}
    		else if (filter == 'ThisQuarter') {
    			var currQuarter = getQuarter(date);
    			firstDay = new Date(date.getFullYear(), 3 * currQuarter - 3, 1);
    			lastDay = new Date(date.getFullYear(), 3 * currQuarter, 0);
    		}
    		else if (filter == 'LastQuarter') {
    			var year;
    			var quarter = getQuarter(date) - 1;
    			//The quarter is zero means, when current quarter is first , then prevous quarter is the last quarter of previous year
    			if (quarter > 0) {
    				year = date.getFullYear();
    				firstDay = new Date(year, 3 * quarter - 3, 1);
    				lastDay = new Date(year, 3 * quarter, 0);
    			}
    			else {
    				year = date.getFullYear() - 1;
    				firstDay = new Date(year, 9, 1);
    				lastDay = new Date(year, 12, 0);
    			}
    		}
    		else if (filter == 'YTD') {
    			firstDay = new Date(date.getFullYear(), 0, 1);
    			lastDay = new Date();
    		}
    		else if (filter == 'LastYear') {
    			firstDay = new Date(date.getFullYear() - 1, 0, 1);
    			lastDay = new Date(date.getFullYear(), 0, 0);
    		}
    		else {
    			firstDay = $scope.beginDate;
    			lastDay = $scope.endDate;
    		}
    		$scope.beginDate = $filter('date')(firstDay, 'MM/dd/yyyy');
    		$scope.endDate = $filter('date')(lastDay, 'MM/dd/yyyy');

    		status = (!$scope.beginDate || !$scope.endDate) ?
                    true : ((($scope.beginDate.length + $scope.endDate.length) < 20) ? true : status);

    		if (!status &&
                ($scope.isDate($scope.beginDate) && $scope.isDate($scope.endDate) && !$scope.isPreviouseDate($scope.beginDate, $scope.endDate))) {

    			var param = {
    				SalesPersonCode: getValueListFromKeyValueList(parsJasonFromArray(selectedSales)),
    				startDate: $scope.beginDate,
    				endDate: $scope.endDate,
    			};

    			(httpRequest = dataService.getArchievedSalesPersonMapping(param))
                    .then(function (response) {
                        $scope.btnSpinner.stop();
                        if (response) {
                            $scope.iscanceldisabled = true;
                    		$scope.ArchievedSalesPersonMapping = response.data;
                    		$scope.ArchievedSalesPersonMappingSafe = angular.copy(response.data);


                    		//for (var i = 0; i < $scope.ArchievedSalesPersonMappingSafe.length; i++) {
                    		//    $scope.ArchievedSalesPersonMappingSafe[i].StartDate = $filter('date')($scope.ArchievedSalesPersonMappingSafe[i].StartDate);
                    		//    $scope.ArchievedSalesPersonMappingSafe[i].EndDate = $filter('date')($scope.ArchievedSalesPersonMappingSafe[i].EndDate);
                    		//    $scope.ArchievedSalesPersonMapping[i].StartDate = $filter('date')($scope.ArchievedSalesPersonMapping[i].StartDate);
                    		//    $scope.ArchievedSalesPersonMapping[i].EndDate = $filter('date')($scope.ArchievedSalesPersonMapping[i].EndDate);
                    		//}



                    	}

                    })
                    .catch(function (error) {
                    	var a = error;

                    });

    		}
    		else {
    			//   Metronic.stopPageLoading();
    		}

    	}
    
    	$scope.resetSearch = function () {
    	// $scope.myTableFunctions.resetSearch();
    		$scope.groupProperty = '';
    		$scope.selectedSalesPersons = [];
    		$scope.selectedSales = "";
    		//$scope.beginDate = null;
    		//$scope.endDate = null;
    		$scope.ArchievedSalesPersonMapping = [];
    		$scope.ArchievedSalesPersonMappingSafe = [];
    		generateDefaultReport();
    	};
    	$scope.abortExecutingApi = function () {


    		try {
    			(httpRequest && httpRequest.abortCall());
    		}
    		catch (e) {


    			//console.log(e.constructor.name); // SyntaxError
    		}

    	};

    	// Return the array of objects from array of stringified objects
    	function parsJasonFromArray(array) {
    		var result = [];
    		if (array && array.length > 0) {
    			for (var i = 0; i < array.length; i++) {
    				result.push(JSON.parse(array[i]));
    			}
    		}
    		return result;
    	}



    	function generateDefaultReport() {
    		var date = new Date();
    		$scope.beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
    		$scope.endDate = $filter('date')(new Date(), 'MM/dd/yyyy');
    		$scope.search();
    	}
    	generateDefaultReport();


    	//

    	function getValueListFromKeyValueList(list) {
    		var result = [];
    		for (var i = 0; i < list.length; i++) {
    			result.push(list[i].Key);
    		}
    		return result;
    	}


    	$scope.openModal = function (elementId, modelData) {
    	    $scope.selectedMasterSalesPersons = "";
    	    $scope.selectedSalesPersons = "";
    	    $scope.isnewMasterSalesPersons = false;
    	    $scope.isnewSalesPersons = false;
    		// $scope.getSalesFilters();
    		if (elementId && elementId != '') {
    			$('#' + elementId).modal({ backdrop: 'static', keyboard: false });
    		}
    		//$scope.assignNewBeginDate = $filter('date')(new Date(), 'MM/dd/yyyy');;
    		//$scope.selectedSalesPersonsList = null;
    		//$scope.modelData = modelData;
    		////$scope.availableSalesPersons1.splice(0, 1);
    		//$scope.availableSalesPersons1 = angular.copy($scope.available_Sales_Persons);
    		//var list = modelData.AssignedPersonList;
    		//for (var i = 0; i < $scope.availableSalesPersons1.length; i++) {
    		//	for (var j = 0; j < list.length; j++) {
    		//		if ($scope.availableSalesPersons1[i].Key == list[j].AssignedPersonCode) {
    		//			$scope.availableSalesPersons1.splice(i, 1);
    		//		}
    		//	}
    		//}


    	}
    	$scope.closeModal = function (elementId) {
    		if (elementId && elementId != '') {
    			$('#' + elementId).modal('hide');
    		}
    		$scope.modelData = "";
    		$scope.availableSalesPersons1 = angular.copy($scope.available_Sales_Persons);


    		$scope.isnewMasterSalesPersons = false;
    		$scope.isnewSalesPersons = false;
    		$scope.sales = {};
    		$scope.selectedMasterSalesPersons = "";
    		$scope.selectedSalesPersons = "";
    		$scope.selectedTrackingGroups = "";


    		// $scope.getSalesFilters();
    	}


    	$scope.sales = {};
    	$scope.salesPersonAdd = function (data) {




    		
    		var SalesPersonCode = ($scope.selectedSalesPersons == 0) ? $scope.SalesPersonsCodeScope : $scope.selectedSalesPersons;
    		for (var i = 0; i < $scope.SalesPersons.length; i++) {
    			if ($scope.SalesPersons[i].Code == SalesPersonCode) {
    				$scope.SalesPersonsDescriptionScope = $scope.SalesPersons[i].Description;
    				break;
    			}
    		}



    		var AssignedPersonCode=($scope.selectedMasterSalesPersons==0) ? $scope.MasterSalesPersonsCodeScope : $scope.selectedMasterSalesPersons

    		for (var i = 0; i <$scope.MasterSalesPersons.length; i++) {
    			if ($scope.MasterSalesPersons[i].Code == AssignedPersonCode) {
    				$scope.MasterSalesPersonsDescriptionScope = $scope.MasterSalesPersons[i].Description;
    				break;
    			}
    		}

    		var MasterSalesPersonsCodeScope;
    		var MasterSalesPersonsDescriptionScope;
    		var SalesPersonsCodeScope;
    		var SalesPersonsDescriptionScope;
    		if ($scope.isnewMasterSalesPersons) {
    			
    			 MasterSalesPersonsCodeScope = data.MasterSalesPersonsCodeScope;
    			 MasterSalesPersonsDescriptionScope = data.MasterSalesPersonsDescriptionScope;

    		}
    		else {
    			MasterSalesPersonsCodeScope = AssignedPersonCode;
    			MasterSalesPersonsDescriptionScope = $scope.MasterSalesPersonsDescriptionScope;
    		}


    		if ($scope.isnewSalesPersons) {
    			 SalesPersonsCodeScope = data.SalesPersonsCodeScope;
    			 SalesPersonsDescriptionScope = data.SalesPersonsDescriptionScope;
    		}
    		else {
    			SalesPersonsCodeScope = SalesPersonCode
    			SalesPersonsDescriptionScope =$scope.SalesPersonsDescriptionScope;
    		}



    		var param = {
    			SalesPersonCode:SalesPersonsCodeScope,
    			SalesPersonDescription: SalesPersonsDescriptionScope,
    			AssignedPersonCode:MasterSalesPersonsCodeScope,
    			AssignedDescription: MasterSalesPersonsDescriptionScope,
    			StartDate: $filter('date')(new Date(), 'MM/dd/yyyy'),
    		    TrackingGroup:$scope.selectedTrackingGroups

    		};

    	(httpRequest = dataService.AssignSalesPersons(param))
                .then(function (response) {
                	if (response) {

                		
                		//  $scope.salesPersons = response.data;
                		$scope.AllsalesPersons = response.data;


                		$scope.AllsalesPersonsSafe =angular.copy(response.data);
                		$scope.closeModal('salesReportOfSalesPerson');
                		//$scope.salesReportMaster = response.data;
                		$scope.getSalesFilters();
                	}

                	// $scope.btnSpinner.stop();
                })
                .catch(function (error) {
                	var a = error;
                	// $scope.btnSpinner.stop();
                });
    	};

    	$scope.unassignconfirm = function (rowData, AssignedPerson, $index) {
    		$scope.CurrentIndex = rowData.$$hashKey;
    		$scope.AssignedCodeIndex = AssignedPerson.$$hashKey;
    		$scope.rowData = rowData.AssignedPersonCode;
    		$scope.AssignedPerson = AssignedPerson.SalesPersonCode;
    		$scope.unAssignLoadDate = $filter('date')(new Date(), 'MM/dd/yyyy');;
    		$('#unassignConfirmation').modal({ backdrop: 'static', keyboard: false });

    	};

    	$scope.unassign = function (unAssignLoadDate) {
    		//console.log(unAssignLoadDate);
    		var param = {
    			salesPersonCode: $scope.AssignedPerson,
    			assignSalesPersonCode: $scope.rowData,
    			unAssignLoadDate: unAssignLoadDate
    		};

    		(httpRequest = dataService.unAssignSalesPersons(param))
				.then(function (response) {
					if (response.statusText == "OK") {

						function checkindexsales(item) {
							return item.$$hashKey == $scope.AssignedCodeIndex;
						}
						function checkindexcode(item) {
							return item.$$hashKey == $scope.CurrentIndex;
						}
						var sales_index = $scope.AllsalesPersons.findIndex(checkindexsales);

						var sales_code_index = $scope.AllsalesPersons[sales_index].AssignedPersonList.findIndex(checkindexcode);

						$scope.AllsalesPersons[sales_index].AssignedPersonList.splice(sales_code_index, 1);

						$scope.search();
						$('#unassignConfirmation').modal('hide');
						$scope.getSalesFilters();
					}

				})
				.catch(function (error) {
					var a = error;

				});

    	};
    }]);
