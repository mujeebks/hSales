/***
GLobal Directives
***/
MetronicApp.directive('refresh',
    function () {
        return {
            restrict: 'E',
            scope: {callbackFn: '&'},
            template: '<div class="row page-filter"><div class="col-md-6 col-lg-6 col-sm-12 col-xs-12 filter-action pull-right-lg-md"><div class="btn-group pull-right-lg-md"><button ng-click="refreshCharts()" class="btn btn-success"><i class="fa fa-refresh"></i> Refresh</button></div></div></div>',
            link: function (scope, elem, attrs) {
                scope.refreshCharts = function () {
                   
                    scope.callbackFn();
                };
            }
        };
    });



MetronicApp.directive("fullscreen", function () {
    return function (scope, element, attrs) {
        scope.$watch(attrs.fullscreen, function (newVal) {
            if (newVal) {
                //$(element).removeClass('portlet light', { duration: 500 });
                $(element).addClass('portlet-fullscreen', { duration: 50 });
              //  $(element).addClass('fade-in-up', { duration: 1500 });

             
               
            } else {
              // $(element).addClass('fade-in-up', { duration: 300 });
                $(element).removeClass('portlet-fullscreen', { duration: 0 });
                 
               // $(element).addClass('fade-in-up', { duration: 100 });
               
              //  $(element).addClass('portlet light portlet-fullscreen', { duration: 500 });
            }
        })
    }
})
// Route State Load Spinner(used on page or content load)
MetronicApp.directive('ngSpinnerBar', ['$rootScope',
    function ($rootScope) {
        return {
            link: function (scope, element, attrs) {
                // by defult hide the spinner bar
                element.addClass('hide'); // hide spinner bar by default

                // display the spinner bar whenever the route changes(the content part started loading)
                $rootScope.$on('$stateChangeStart', function () {
                    element.removeClass('hide'); // show spinner bar
                });

                // hide the spinner bar on rounte change success(after the content loaded)
                $rootScope.$on('$stateChangeSuccess', function () {
                    element.addClass('hide'); // hide spinner bar
                    $('body').removeClass('page-on-load'); // remove page loading indicator
                    Layout.setSidebarMenuActiveLink('match'); // activate selected link in the sidebar menu

                    // auto scorll to page top
                    setTimeout(function () {
                        Metronic.scrollTop(); // scroll to the top on content load
                    }, $rootScope.settings.layout.pageAutoScrollOnLoad);
                });

                // handle errors
                $rootScope.$on('$stateNotFound', function () {
                    element.addClass('hide'); // hide spinner bar
                });

                // handle errors
                $rootScope.$on('$stateChangeError', function () {
                    element.addClass('hide'); // hide spinner bar
                });
            }
        };
    }
])


MetronicApp.directive('icheck', ['$timeout', '$parse', function ($timeout, $parse) {
    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attr, ngModel) {
            $timeout(function () {
                var value = attr.value;

                function update(checked) {
                    if (attr.type === 'radio') {
                        ngModel.$setViewValue(value);
                    } else {
                        ngModel.$setViewValue(checked);
                    }
                }

                $(element).iCheck({
                    checkboxClass: attr.checkboxClass || 'icheckbox_square-green',
                    radioClass: attr.radioClass || 'iradio_square-green'
                }).on('ifChanged', function (e) {
                    scope.$apply(function () {
                        update(e.target.checked);
                    });
                });

                scope.$watch(attr.ngChecked, function (checked) {
                    if (typeof checked === 'undefined') checked = !!ngModel.$viewValue;
                    update(checked)
                }, true);

                scope.$watch(attr.ngModel, function (model) {
                    $(element).iCheck('update');
                }, true);

            })
        }
    }
}])

// Handle global LINK click
MetronicApp.directive('a',
    function () {
        return {
            restrict: 'E',
            link: function (scope, elem, attrs) {
                if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                    elem.on('click', function (e) {
                        e.preventDefault(); // prevent link click for above criteria
                    });
                }
            }
        };
    });

// Handle Dropdown Hover Plugin Integration
MetronicApp.directive('dropdownMenuHover', function () {
    return {
        link: function (scope, elem) {
            elem.dropdownHover();
        }
    };
});

MetronicApp.directive('compile', ['$compile', function ($compile) {
    return function (scope, element, attrs) {
        scope.$watch(
          function (scope) {
              // watch the 'compile' expression for changes
              return scope.$eval(attrs.compile);
          },
          function (value) {
              // when the 'compile' expression changes
              // assign it into the current DOM
              element.html(value);

              // compile the new DOM and link it to the current
              // scope.
              // NOTE: we only compile .childNodes so that
              // we don't get into infinite loop compiling ourselves
              $compile(element.contents())(scope);
          }
      );
    };
}]);


MetronicApp.directive('restictletter', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element[0].onkeypress = function (evt) {

                var theEvent = evt || window.event;
                var key = theEvent.keyCode || theEvent.which;
                var keys = theEvent.keyCode || theEvent.which;

                key = String.fromCharCode(key);
                var regex = /[0-9]|\./;
                if (!regex.test(key)) {
                    theEvent.returnValue = false;
                    if (keys == 8) {

                    }
                    else {
                        if (theEvent.preventDefault) theEvent.preventDefault();
                    }

                }

                if (attrs.restictpoint) {
                    //restrict the points also
                    if (keys == 46) {
                        theEvent.preventDefault();
                    }

                }
            }
        }
    };
}]);

MetronicApp.directive('restrictcutcopypaste', function () {
    return {
        scope: {},
        link: function (scope, element) {
            element.on('cut copy paste', function (event) {
                event.preventDefault();
            });
        }
    };
});


MetronicApp.directive('autocompletedropdown', ['dataService', function (dataService) {

    return {
        replace: true,
        restrict: 'E',
        scope: {
            search: "=",
            callback: '&',
            minSearch: "=",
            list: "=",
            onSelect: "=",
            clearvalue:"="
        },
        template:
            '<div><style>' +

            '.typeahead {' +
   ' z-index: 1051;' +
    'margin-top: 2px;' +
    '-webkit-border-radius: 4px;' +
    '-moz-border-radius: 4px;' +
   ' border-radius: 4px;' +
'}' +

'.dropdown-menu {' +
    '-webkit-transition: all 0.9s;' +
    '-moz-transition: all 0.9s;' +
    '-ms-transition: all 0.9s;' +
    '-o-transition: all 0.9s;' +
    'transition: all 0.9s;' +
    //' opacity: inherit;' +
    ' max-height: none;' +
    'display: block;' +
    'overflow: hidden;' +
'}' +

'.opac{ opacity: inherit;}' +



                '</style>' +
            '<div class ="input-group" style="width: 100%;"> ' +
        //'<input data-ng-model="UserSearch"   ng-change="selectSearchType(UserSearch)" ng-blur="blurinput()" ng-focus="inputFocused()"  type="text" class="form-control" placeholder="Item to search">' +
       ' <div ng-blur="blurinput()" class="select2-container select2-container-multi form-control select-md tooltips ng-untouched ng-valid-parse ng-dirty ng-valid ng-valid-required" id="s2id_autogen1">' +
       '<ul class ="select2-choices">' +
       '<li ng-if="selectedlist.length>0" class ="select2-search-choice" ng-repeat="selected in selectedlist">' +
       '<div  >{{selected.Value}}</div>' +
       '<a href="#" ng-click="removeselected(selected)" class ="select2-search-choice-close" tabindex="-1"></a>' +
       '</li>' +
       '<li class ="select2-search-field">' +
       '<label for="s2id_autogen2" class ="select2-offscreen"></label>' +
       '<input data-ng-model="UserSearch" style="min-width: 274%;"     ng-change="selectSearchType(UserSearch)" ng-blur="blurinput()" ng-focus="inputFocused()"  type="text" class ="form-control" placeholder="{{placeholder}}">' +
       '</li>' +
       '</ul>' +


        '<ul  ng-if="items && !nodata" class="typeahead dropdown-menu opac select2-results" style="width: 100%;overflow: scroll;height:250px">' +

                '<li  ng-click="selectvalue(item)" role="presentation" class="select2-results-dept-0 select2-result select2-result-selectable"  ng-repeat="item in items">' +
                    '<a tabindex="-1"  class="ng-binding" >{{item.Value}}</a>' +
                '</li>' +


            '</ul>' +



            '<ul ng-if="!items" class="typeahead dropdown-menu opac" ng-show="defaultul" style="width: 100%;">' +

                '<li  class="ng-scope active">' +
                    '<a tabindex="-1"   >Search items by enter {{minSearch}} letters</a>' +
                '</li>' +


            '</ul>' +
            '<ul class="typeahead dropdown-menu opac" ng-show="nodata" style="width: 100%;">' +

                '<li class="ng-scope active">' +
                    '<a tabindex="-1"  class="ng-binding">No result found!!</a>' +
                '</li>' +


            '</ul>' +
            // '<ul class="typeahead dropdown-menu opac"  ng-if="!items && selectedlist.length>0 && needclear" style="width: 100%;">' +

            //    '<li ng-click="test()" class="ng-scope active">' +
            //        '<a tabindex="-1"  class="ng-binding">Remove All</a>' +
            //    '</li>' +


            //'</ul>' +

    '</div></div>',
        link: function (scope, element, attrs) {

            scope.ulshow = false;
            scope.defaultul = false;

            scope.items = null;
            scope.selectedlist = [];
            scope.placeholder = "Item to search";
            scope.selectSearchType = function (UserSearch) {
                scope.needclear = false;
                scope.nodata = false;
                scope.isselected = true;
                if (UserSearch.length >= scope.minSearch) {

                    scope.ulshow = true;
                    scope.defaultul = false;



                    dataService.GetItems(UserSearch).then(function (response) {

                        if (response.data.ListItem) {
                            scope.items = response.data.ListItem;
                            scope.itemlist = response.data.ListItem;

                            if (scope.selectedlist.length > 0) {
                                for (var i = 0; i < scope.items.length; i++) {
                                    for (var z = 0; z < scope.selectedlist.length; z++) {
                                        if (scope.selectedlist[z].Key == scope.items[i].Key) {
                                            scope.items.splice(i, 1);
                                        }
                                    }
                                }
                            }

                        }
                        else {
                            scope.nodata = true;

                        }

                    }, function onError() {
                        
                    });



                }
                else {
                    scope.ulshow = false;


                        scope.defaultul = true;


                   // scope.defaultul = true;
                    scope.items = null;
                }

            };
            scope.clearAll = function () {
                scope.defaultul = true;
                scope.needclear = false;
                scope.selectedlist = [];
            }
            scope.inputFocused = function () {


                if (scope.selectedlist.length > 0) {
                    scope.defaultul = false;
                    scope.needclear = true;
                }
                else {
                    scope.defaultul = true;
                    scope.needclear = false;
                }

                scope.ulshow = false;
               // scope.defaultul = true;

            }
            scope.blurinput = function () {

                scope.ulshow = false;
                scope.defaultul = false;
                scope.nodata = false;
                scope.needclear=true
               // scope.needclear=false
              //  scope.needclear = false;
               // scope.items = null;

            };
            scope.selectvalue = function (item) {

                scope.UserSearch = "";
                scope.items = null;
                scope.selectedlist.push({
                    Key: item.Key,
                    Value: item.Value
                })
                scope.callback({ item: scope.selectedlist });
                scope.placeholder = "";
                scope.needclear=false
            }
            scope.$watch('clearvalue', function (newvalue, oldvalue) {

                scope.selectedlist = [];

            });
            scope.removeselected = function (selected) {
                for (var i = 0; i < scope.selectedlist.length; i++) {
                    if (scope.selectedlist[i].Key == selected.Key) {

                        scope.selectedlist.splice(i, 1)

                    }

                }

                if (scope.selectedlist.length == 0) {
                    scope.placeholder = "Item to search";
                }
                scope.callback({ item: scope.selectedlist });
            }

            scope.test = function () {

                scope.needclear = false;
                scope.selectedlist = [];
            }


        }
    }

}]);




MetronicApp.directive('autocompletedropdownbuyer', ['dataService', function (dataService) {

    return {
        replace: true,
        restrict: 'E',
        scope: {
            search: "=",
            callback: '&',
            minSearch: "=",
            list: "=",
            onSelect: "="
        },
        template:
            '<div><style>' +

                        '.typeahead {' +
               ' z-index: 1051;' +
                'margin-top: 2px;' +
                '-webkit-border-radius: 4px;' +
                '-moz-border-radius: 4px;' +
               ' border-radius: 4px;' +
            '}' +

'.dropdown-menu {' +
    '-webkit-transition: all 0.9s;' +
    '-moz-transition: all 0.9s;' +
    '-ms-transition: all 0.9s;' +
    '-o-transition: all 0.9s;' +
    'transition: all 0.9s;' +
    //' opacity: inherit;' +
    ' max-height: none;' +
    'display: block;' +
    'overflow: hidden;' +
'}' +

'.opac{ opacity: inherit;}' +



                '</style>' +
            '<div class ="input-group" style="width: 100%;"> ' +
        //'<input data-ng-model="UserSearch"   ng-change="selectSearchType(UserSearch)" ng-blur="blurinput()" ng-focus="inputFocused()"  type="text" class="form-control" placeholder="Item to search">' +
       ' <div ng-blur="blurinput()" class="select2-container select2-container-multi form-control select-md tooltips ng-untouched ng-valid-parse ng-dirty ng-valid ng-valid-required" id="s2id_autogen1">' +
       '<ul class ="select2-choices">' +
       '<li ng-if="selectedlist.length>0" class ="select2-search-choice" ng-repeat="selected in selectedlist">' +
       '<div  >{{selected.Value}}</div>' +
       '<a href="#" ng-click="removeselected(selected)" class ="select2-search-choice-close" tabindex="-1"></a>' +
       '</li>' +
       '<li class ="select2-search-field">' +
       '<label for="s2id_autogen2" class ="select2-offscreen"></label>' +
       '<input data-ng-model="UserSearch" style="min-width: 274%;"     ng-change="selectSearchType(UserSearch)" ng-blur="blurinput()" ng-focus="inputFocused()"  type="text" class ="form-control" placeholder="{{placeholder}}">' +
       '</li>' +
       '</ul>' +


        '<ul  ng-if="items && !nodata" class="typeahead dropdown-menu opac select2-results" style="width: 100%;overflow: scroll;height:250px">' +

                '<li  ng-click="selectvalue(item)" role="presentation" class="select2-results-dept-0 select2-result select2-result-selectable"  ng-repeat="item in items">' +
                    '<a tabindex="-1"  class="ng-binding" >{{item.Value}}</a>' +
                '</li>' +


            '</ul>' +



            '<ul ng-if="!items" class="typeahead dropdown-menu opac" ng-show="defaultul" style="width: 100%;">' +

                '<li  class="ng-scope active">' +
                    '<a tabindex="-1"   >Search items by enter {{minSearch}} letters</a>' +
                '</li>' +


            '</ul>' +
            '<ul class="typeahead dropdown-menu opac" ng-show="nodata" style="width: 100%;">' +

                '<li class="ng-scope active">' +
                    '<a tabindex="-1"  class="ng-binding">No result found!!</a>' +
                '</li>' +


            '</ul>' +


    '</div></div>',
        link: function (scope, element, attrs) {

            scope.ulshow = false;
            scope.defaultul = false;

            scope.items = null;
            scope.selectedlist = [];
            scope.placeholder = "Item to search";
            scope.selectSearchType = function (UserSearch) {
                scope.needclear = false;
                scope.nodata = false;
                scope.isselected = true;
                if (UserSearch.length >= scope.minSearch) {

                    scope.ulshow = true;
                    scope.defaultul = false;



                    dataService.GetItems(UserSearch).then(function (response) {

                        if (response.data.ListItem) {
                            scope.items = response.data.ListItem;
                            scope.itemlist = response.data.ListItem;

                            if (scope.selectedlist.length > 0) {
                                for (var i = 0; i < scope.items.length; i++) {
                                    for (var z = 0; z < scope.selectedlist.length; z++) {
                                        if (scope.selectedlist[z].Key == scope.items[i].Key) {
                                            scope.items.splice(i, 1);
                                        }
                                    }
                                }
                            }

                        }
                        else {
                            scope.nodata = true;

                        }

                    }, function onError() {
                        
                    });



                }
                else {
                    scope.ulshow = false;


                    scope.defaultul = true;



                    scope.items = null;
                }

            };
            scope.clearAll = function () {
                scope.defaultul = true;
                scope.needclear = false;
                scope.selectedlist = [];
            }
            scope.inputFocused = function () {


                if (scope.selectedlist.length > 0) {
                    scope.defaultul = false;
                    scope.needclear = true;
                }
                else {
                    scope.defaultul = true;
                    scope.needclear = false;
                }

                scope.ulshow = false;


            }
            scope.blurinput = function () {

                scope.ulshow = false;
                scope.defaultul = false;
                scope.nodata = false;
                scope.needclear = true


            };
            scope.selectvalue = function (item) {

                scope.UserSearch = "";
                scope.items = null;
                scope.selectedlist.push({
                    Key: item.Key,
                    Value: item.Value
                })
                scope.callback({ item: scope.selectedlist });
                scope.placeholder = "";
                scope.needclear = false
            }

            scope.removeselected = function (selected) {
                for (var i = 0; i < scope.selectedlist.length; i++) {
                    if (scope.selectedlist[i].Key == selected.Key) {

                        scope.selectedlist.splice(i, 1)

                    }

                }

                if (scope.selectedlist.length == 0) {
                    scope.placeholder = "Item to search";
                }
                scope.callback({ item: scope.selectedlist });
            }

            scope.test = function () {

                scope.needclear = false;
                scope.selectedlist = [];
            }


        }
    }

}]);


MetronicApp.directive('filterDate',
    function () {
        this.count = 0;
        return {
            restrict: 'E',
            scope: { compiletitle: '=',filter:'=' ,needexpand:'='},
            template: '<div class="portlet-title  tabbable-line">'+
                '<div class="caption caption-md" style="width:46% !important;">' +
                '<div ng-attr-title="{{message}}"  ng-if="needexpand" id={{::expandID}}  class="fa fa-expand" style="position: absolute; float: right; margin-top: 36px; right: 57px;z-index: 9181;cursor: pointer;" ng-click="action()"></div>' +
                    '<div class="time-frame" style="float: right;position: absolute;right: 0;margin-right: 27px;">'+
                        '<span class="ng-binding">'+
                            '{{currentfilter.Periods.Current.Start | date}} - {{currentfilter.Periods.Current.End | date}}'+
                        '</span>'+
                    '</div>'+
                    '<span class="caption-subject font-light-orange-haze bold uppercase text-mobile text-mobile" compile="title"></span>'+
                '</div>'+
            '</div>',
            link: function (scope, elem, attrs) {
                scope.expandID = 'Expand' + (count);
                count++;
                var expandicon = "fa fa-expand";
                var restoreicon = "fa fa-compress";
                scope.message = "Click to Expand";
                scope.$watch('compiletitle', function (newValue, oldValue) {
                    if (newValue)
                    {
                        scope.title = scope.compiletitle;
                        scope.currentfilter = scope.filter;
                    }
                });
                scope.$watch('filter', function (newValue, oldValue) {
                    if (newValue){
                        scope.title = scope.compiletitle;
                        scope.currentfilter = scope.filter;
                    }
                       
                });

                scope.action = function () {
                    var Elementcontainclass = elem.parent().attr('class');
                    if (Elementcontainclass == "portlet light bordered") {
                        elem.parent().addClass('portlet-fullscreen fade-in-up');
                        $("body").css("overflow", "hidden");
                        elem.parent().css("overflow", "scroll");
                        $('#' + scope.expandID).removeClass();
                        $('#' + scope.expandID).addClass(restoreicon).fadeIn(1000);
                        scope.message = "Click to restore";
                    }
                    else {
                        elem.parent().removeClass('portlet-fullscreen fade-in-up');
                        $("body").css("overflow", "inherit");
                        elem.parent().css("overflow", "hidden");
                        $('#' + scope.expandID).removeClass();
                        $('#' + scope.expandID).addClass(expandicon).fadeIn(1000);
                        scope.message = "Click to expand";
                    }
                };
              
                $(document).ready(function () {
                    $(document).keydown(function (e) {
                        
                        var Elementcontainclass = elem.parent().attr('class');
                        if (Elementcontainclass == "portlet light bordered portlet-fullscreen fade-in-up") {
                            if (e.keyCode == 27) {
                                elem.parent().removeClass();
                                elem.parent().addClass('portlet light bordered');
                                $("body").css("overflow", "inherit");
                                elem.parent().css("overflow", "hidden");
                                $('#' + scope.expandID).removeClass();
                                $('#' + scope.expandID).addClass(expandicon).fadeIn(1000);
                            }
                        }
                    
                    });

                   
                });


            }
        };
    });
MetronicApp.directive("reportfullscreen", function () {
    this.count = 0;
    return {
        restrict: 'E',
        scope: { needexpand: '=' },
        template: 
           
            '<div ng-attr-title="{{message}}"  ng-if="needexpand" id={{::expandID}}  class="fa fa-expand" style="float: right; margin-top: 15px;z-index: 9181;cursor: pointer;" ng-click="action()"></div>',
        link: function (scope, elem, attrs) {
            scope.expandID = 'Expand' + (count);
            count++;
            var expandicon = "fa fa-expand";
            var restoreicon = "fa fa-compress";
            scope.message = "Click to Expand";
        

            scope.action = function () {
               
                var Elementcontainclass = elem.parent().parent().attr('class');
                if (Elementcontainclass == "portlet light" || Elementcontainclass == "portlet box blue-hoki") {
                    elem.parent().parent().addClass('portlet-fullscreen fade-in-up');
                    elem.parent().parent().css("overflow-x", "scroll");
                   $("body").css("overflow", "hidden");
                   // elem.parent().css("overflow", "scroll");
                    $('#' + scope.expandID).removeClass();
                    $('#' + scope.expandID).addClass(restoreicon).fadeIn(1000);
                    scope.message = "Click to restore";
                }
                else {
                    elem.parent().parent().removeClass('portlet-fullscreen fade-in-up');
                   // elem.parent().parent().css("overflow", "scroll");
                   $("body").css("overflow", "inherit");
                   elem.parent().parent().css("overflow-x", "inherit");
                    $('#' + scope.expandID).removeClass();
                    $('#' + scope.expandID).addClass(expandicon).fadeIn(1000);
                    scope.message = "Click to expand";
                }
            };

            //$(document).ready(function () {
            //    $(document).keydown(function (e) {

            //        var Elementcontainclass = elem.parent().attr('class');
            //        if (Elementcontainclass == "portlet light bordered portlet-fullscreen fade-in-up") {
            //            if (e.keyCode == 27) {
            //                elem.parent().removeClass();
            //                elem.parent().addClass('portlet light bordered');
            //                $("body").css("overflow", "inherit");
            //                elem.parent().css("overflow", "hidden");
            //                $('#' + scope.expandID).removeClass();
            //                $('#' + scope.expandID).addClass(expandicon).fadeIn(1000);
            //            }
            //        }

            //    });


            //});


        }
    };
});

MetronicApp.directive("ariaremove", function () {
    return function (scope, element, attrs) {
        
        element.on('mouseenter', function () {
          
            //setTimeout(function () {
            //    $('.tool-tip fade top in').remove();
            //}, 100)
           
        });
        element.on('mouseleave', function () {
         
            //setTimeout(function () {
            //    $('.tool-tip fade top in').remove();
            //}, 100)
        });
    }
})














MetronicApp.directive('filterbuttons',function () {
    return {
        restrict: 'E',
        replace: true,
        require: '?ngModel',
        scope: {
            callbackFn: '&',

        },
        templateUrl: 'app/components/directives/filterbuttons.html',
      
        link: function (scope, element, attr, ngModel) {
            debugger
            scope.InitalButton = attr.initial;
          
            scope.ButtonClicked = function (dataselected) {

                scope.selectedbutton = dataselected;
                
                if (dataselected == "By Date") {
                    scope.bydatestyle = { "background-color": "#e43a45", "color": "white" };
                    scope.byinvoicestyle = { "background-color": "white", "color": "#67809F" };
                    scope.bycustomerstyle = { "background-color": "white", "color": "#44b6ae" };
                }
                if (dataselected == "By Invoice") {

                    scope.bydatestyle = { "background-color": "white", "color": "#e43a45" };
                    scope.byinvoicestyle = { "background-color": "#67809F", "color": "white" };
                    scope.bycustomerstyle = { "background-color": "white", "color": "#44b6ae" };
                }
                if (dataselected == "By Customer") {

                    scope.bydatestyle = { "background-color": "white", "color": "#e43a45" };
                    scope.byinvoicestyle = { "background-color": "white", "color": "#67809F" };
                    scope.bycustomerstyle = { "background-color": "#44b6ae", "color": "white" };
                }
           


                scope.callbackFn({ data: dataselected });

            };
            //scope.setstyle = function (filter) {

            //}
            scope.ButtonClicked(scope.InitalButton)
        }
    }
})


