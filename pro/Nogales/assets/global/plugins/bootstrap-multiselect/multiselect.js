/// -------------------------------------------------
/// Angular Multi Select Directive
/// Version : 1.0
///--------------------------------------------------
//angular.module("vocApp.directives")
MetronicApp
    .directive("vocMultiselect", function ($http) {
        return {
            restrict: 'A',
            require: "?ngModel",
            scope: {
                dropdownSrc: '=',
                toggleRefresh: '=',
                myData: '=ngModel'
            },
            link: function (scope, element, attr, controller) {

                /// Set select configs
                scope.isScalarData = attr.scalarData == null ? false : (attr.scalarData == 'true');
                scope.valueFeild = attr.valueFeild == null ? 'value' : attr.valueFeild;
                scope.textFeild = attr.textFeild == null ? 'text' : attr.textFeild;
                scope.maxHeight = attr.maxHeight == null ? 400 : parseInt(attr.maxHeight);
                scope.selectAllOption = attr.selectAllOption == null ? true : (attr.selectAllOption == 'true');
                scope.enableFiltering = attr.enableFiltering == null ? true : (attr.enableFiltering == 'true');
                scope.nonSelectedText = attr.nonSelectedText == null ? "Please select" : attr.nonSelectedText;
                scope.nSelectedText = attr.nSelectedText == null ? " Selected" : attr.nSelectedText;
                scope.allSelectedText = attr.allSelectedText == null ? "All Selected" : attr.allSelectedText;
                scope.buttonWidth = attr.buttonWidth == null ? "" : attr.buttonWidth;
                scope.lastSearchedText = '';

                // if the search in the dropdown is performed ar the server side, then set this as true
                scope.isServersideSearching = false;
                var onSearchChangeEvent = null;

                // if scope.isServersideSearching is true, then set scope.dropdownSrc as empty
                if (attr.dropdownSrcUrl != null && attr.dropdownSrcUrl != '') {
                    scope.isServersideSearching = true;
                    scope.dropdownSrc = [];
                    scope.minimumSearchLength = attr.minSearchLength == null ? 3 : attr.minSearchLength;

                    onSearchChangeEvent = function (searchText) {
                        if (searchText.length >= scope.minimumSearchLength && scope.lastSearchedText != searchText) {
                            scope.lastSearchedText = searchText;
                            $http.get(attr.dropdownSrcUrl + searchText).then(function (response) {
                                var options = [];
                                angular.forEach(scope.myData, function (vdn) {
                                    var option = { label: vdn, title: vdn, value: vdn, selected: true };
                                    options.push(option);
                                });

                                angular.forEach(response.data, function (vdn) {
                                    if (scope.myData && scope.myData.indexOf(vdn) > 0) {
                                    }
                                    else {
                                        var option = { label: vdn, title: vdn, value: vdn, selected: false };
                                        options.push(option);
                                    }
                                });

                                if (options.length > 0)
                                    element.multiselect('dataprovider', options, true);
                            },
                            function () {

                            });
                        }
                    }
                }
                /// End of select config

                ///   Helper Functions

                // initialize the dropdown
                function initializeSelect() {
                    // Destroy dropdown
                    element.multiselect('destroy');
                    // get option list for the dropdown
                    var options = generateOptionList();
                    // assign option list to the dropdown
                    element.html(options);

                    element.multiselect({
                        maxHeight: scope.maxHeight,
                        includeSelectAllOption: scope.selectAllOption,
                        enableFiltering: scope.enableFiltering,
                        numberDisplayed: 1,
                        nonSelectedText: scope.nonSelectedText,
                        nSelectedText: scope.nSelectedText,
                        allSelectedText: scope.allSelectedText,
                        buttonWidth: scope.buttonWidth,
                        onSearchTextChange: onSearchChangeEvent
                    });
                }


                // generate option list for the dropdown
                function generateOptionList() {
                    var options = '';

                    if (scope.isServersideSearching) {
                        options += ' <option disabled="true" > enter min ' + scope.minimumSearchLength + ' characters </option> '
                    }
                    if (scope.isScalarData) {
                        angular.forEach(scope.dropdownSrc, function (value, index) {
                            options += ' <option value="' + value + '"> ' + value + ' </option> '
                        });
                    }
                    else {
                        angular.forEach(scope.dropdownSrc, function (value, index) {
                            options += ' <option value="' + value[scope.valueFeild] + '"> ' + value[scope.textFeild] + ' </option> '
                        });
                    }

                    return options;
                }

                /// End Of Helper Functions

                /// Directive's watchers

                scope.$watchGroup(["dropdownSrc", 'toggleRefresh'], function (newVal, oldVal) {
                    initializeSelect();
                });

                /// End of Directive's watchers
            }
        }
    });
