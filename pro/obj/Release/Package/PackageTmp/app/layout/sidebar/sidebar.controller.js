
'use strict';

MetronicApp.controller('SidebarController', ['$rootScope', '$scope', '$http', '$state', 'authService', 'settings', 'NotificationService', 'commonService', 'SessionOutDisabledUsers',
function ($rootScope, $scope, $http, $state, authService, settings, NotificationService, commonService, SessionOutDisabledUsers) {

   // var IDLE_TIMEOUT = 10; //seconds
   var IDLE_TIMEOUT = 900; //seconds 15 Min
    var _idleSecondsTimer = null;
    var _idleSecondsCounter = 0;
    var Timer=null;

        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            Metronic.initAjax();


        });
        var resBreakpointMd = Metronic.getResponsiveBreakpoint('md');
        $scope.user = authService.user;

        $scope.initSidemenu = function () {
            //Layout.initSidebar();
        }
        //debugger;
        $scope.expansion = settings.layout.pageSidebarClosed;

        $scope.gotousermanager = function () {
            $state.go('user-management');
           // $state.reload()
        }

        $scope.gotosalespersonmapping = function () {
          
            $state.go('sales-person-mapping');
        };

        $scope.onMenuClick = function (event) {

            debugger
            var element = event.currentTarget;
            if (Metronic.getViewPort().width >= resBreakpointMd && $(element).parents('.page-sidebar-menu-hover-submenu').size() === 1) { // exit of hover sidebar menu
                return;
            }

            if ($(element).next().hasClass('sub-menu') === false) {
                if (Metronic.getViewPort().width < resBreakpointMd && $('.page-sidebar').hasClass("in")) { // close the menu on mobile view while laoding a page
                    $('.page-header .responsive-toggler').click();
                }
                return;
            }

            if ($(element).next().hasClass('sub-menu always-open')) {
                return;
            }

            var parent = $(element).parent().parent();
            var the = $(element);
            var menu = $('.page-sidebar-menu');
            var sub = $(element).next();

            var autoScroll = menu.data("auto-scroll");
            var slideSpeed = parseInt(menu.data("slide-speed"));
            var keepExpand = menu.data("keep-expanded");

            if (keepExpand !== true) {

                parent.children('li.open').children('a').children('.arrow').removeClass('open');
                parent.children('li.open').children('.sub-menu:not(.always-open)').slideUp(slideSpeed);
                parent.children('li.open').removeClass('open');
            }

            var slideOffeset = -200;

            var refElm = $(element).find('span').text().trim();

            //$('.m1').each(function (index, data) {

            //    var tex = $(this).text().trim();

            //    if(refElm != tex)
            //    {

            //       $(this).closest('li').removeClass('active open');
            //        $(this).next().removeClass('open');
            //    }

            //});






            if (sub.is(":visible")) {


                $('.arrow', $(element)).hide("open");
                $(element).parent().removeClass("open");
                sub.slideUp(slideSpeed, function () {
                    if (autoScroll === true && $('body').hasClass('page-sidebar-closed') === false) {
                        if ($('body').hasClass('page-sidebar-fixed')) {
                            menu.slimScroll({
                                'scrollTo': (the.position()).top
                            });
                        } else {
                            Metronic.scrollTo(the, slideOffeset);
                        }
                    }
                });
            } else {
                $('.arrow', $(element)).addClass("open");
                $(element).parent().addClass("open");

                sub.slideDown(slideSpeed, function () {
                    if (autoScroll === true && $('body').hasClass('page-sidebar-closed') === false) {
                        if ($('body').hasClass('page-sidebar-fixed')) {
                            menu.slimScroll({
                                'scrollTo': (the.position()).top
                            });
                        } else {
                            Metronic.scrollTo(the, slideOffeset);
                        }
                    }
                });
            }

            event.preventDefault();
        }


        $scope.$on('user_changed', function (event, param) {
            $scope.user = param.newUser;
        });

        $rootScope.continue = function () {

            window.clearInterval(_idleSecondsTimer);
            window.clearInterval(Timer);
            _idleSecondsTimer = window.setInterval(CheckIdleTime, 1000);
            _idleSecondsCounter = 0;
            $("#myModal").modal('hide');
        };

        $scope.cancel = function () {
           // localStorage.clear();
            //Alertify.success("session expired due to browser idle !");
            if (typeof localStorage === 'object') {
                try {
                    localStorage.removeItem('ls.nogalesAuth');
                    // location.reload(true);
                    localStorage.removeItem('CasesSoldRevenueByMonth');
                    localStorage.removeItem('DashboardGaugeValues');
                    localStorage.removeItem('HeatMapData');
                    localStorage.removeItem('OPEXCOGSExpenseJournalChart');
                    localStorage.removeItem('ls.nogalesAuthAccess');
                    //console.log(localStorage);
                } catch (e) {
                   Storage.prototype._setItem = Storage.prototype.setItem;
                   Storage.prototype.setItem = function () { };
                   localStorage.clear();
                    alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
                }
            }



            window.location.href = "#/login?session=screenIdle";
          

            window.clearInterval(Timer);


            $("#myModal").modal('hide');
            location.reload();
        };


    //var IDLE_TIMEOUT = 1800; //seconds


        document.onclick = function () {
            _idleSecondsCounter = 0;
        };

        document.onmousemove = function () {
            _idleSecondsCounter = 0;
        };

        document.onkeypress = function () {
            _idleSecondsCounter = 0;
        };

        _idleSecondsTimer = window.setInterval(CheckIdleTime, 1000);

        function CheckIdleTime() {
            _idleSecondsCounter++;
           // console.log("Session out When idle within " + (IDLE_TIMEOUT - _idleSecondsCounter)+" secs");
          //  console.log(localStorage.getItem('ls.nogalesAuth'));
            //var oPanel = document.getElementById("SecondsUntilExpire");
            //if (oPanel)
            //    oPanel.innerHTML = (IDLE_TIMEOUT - _idleSecondsCounter) + "";
            if (_idleSecondsCounter >= IDLE_TIMEOUT) {
                // window.clearInterval(_idleSecondsTimer);
                if (typeof localStorage === 'object') {
                    try {
                       
                        debugger
                  

                        function CheckIsAuth() {
                            var islogin = localStorage.getItem('ls.nogalesAuth');
                            if (islogin) {
                                if (SessionOutDisabledUsers.length > 0) {
                                    for (var i = 0; i < SessionOutDisabledUsers.length; i++) {
                                        if (!JSON.parse(islogin).userName == SessionOutDisabledUsers[i]) {
                                            
                                            break;
                                            return false;
                                        }
                                    }

                                }
                                else {
                                    return true;
                                }
                            }
                            else {
                                return false;
                            }
                           
                        }


                        //temperarly disabled the session timeout for the user 
                        if (CheckIsAuth()) {
                            window.clearInterval(_idleSecondsTimer);

                            $("#myModal").modal('show');
                            $scope.timersecs = 30;
                            Timer = window.setInterval(function () {

                                $scope.$apply(function () {
                                    $scope.timersecs = $scope.timersecs - 1;
                                });

                                if ($scope.timersecs == 0) {
                                    window.clearInterval(Timer);
                                    $scope.cancel();
                                }


                            }, 1000);
                        }
                    } catch (e) {
                      //  Storage.prototype._setItem = Storage.prototype.setItem;
                        Storage.prototype.setItem = function () { };
                        alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
                    }
                }


            }
        }

        $scope.Module = {
            CasesSold: { Name: "", IsShow: true },
            Sales: { Name: "", IsShow: true },
            Profitability: { Name: "", IsShow: true },
            Expenses: { Name: "", IsShow: true },
            WareHouse: { Name: "", IsShow: true },
            Transportation: { Name: "", IsShow: true},
            Item: { Name: "", IsShow: true },
            Customers: { Name: "", IsShow: true },
            Payroll:{Name:"",IsShow:true}
        }
        $scope.$on('ACCESSLOADED', function (event, param) {
          
            checkaccess(param.Access);
        });

        $scope.UserAccess = commonService.getuseraccesss();
    
        function checkaccess(accessarray) {
         

            function formatstate(word) {
                word = word.replace(/ +/g, "");
                word = word.toLowerCase();
                return word;
            };
           
        
            for (var i = 0; i < accessarray.Modules.length; i++) {
                var text = formatstate(accessarray.Modules[i].Name);
                if (text.includes('casessold')) {
                    $scope.Module.CasesSold = { Name: "Cases Sold", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('sales')) {
                    $scope.Module.Sales = { Name: "Sales", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('profitablity')) {
                    $scope.Module.Profitability = { Name: "profitablity", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('expenses')) {
                    $scope.Module.Expenses = { Name: "Expenses", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('warehouse')) {
                    $scope.Module.WareHouse = { Name: "Warehouse", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('transportation')) {
                    $scope.Module.Transportation = { Name: "Transportation", IsShow: accessarray.Modules[i].IsAccess };
                }
                if (text.includes('item')) {
                    $scope.Module.Item = {Name: "Item", IsShow: accessarray.Modules[i].IsAccess};
                }
                if (text.includes('customers')) {
                    $scope.Module.Customers = { Name: "Customers", IsShow: accessarray.Modules[i].IsAccess };
                }
                //if (text.includes('payroll')) {
                //    $scope.Module.Payroll = { Name: "Payroll", IsShow: accessarray.Modules[i].IsAccess };
                //}
            }
        };

        checkaccess($scope.UserAccess);
     
    }]);
