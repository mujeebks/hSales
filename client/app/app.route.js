﻿


/* Setup Rounting For All Pages */
MetronicApp.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var version = (window.version) ? window.version : 0;
  
    //make it true during gulp
    //make it false during development
    var production = false;

    version = (production) ? "?v=" + version : "";  
    //version = "?v=" + version;
    //version = "";

    $urlRouterProvider.otherwise("/Categories");

    //function resolveGlobalFilter(dataService, $rootScope, storageService) {
    

      

    //    return dataService.GetGlobalFilters().then(function (response) {
    //        $rootScope.defaultfilter = response.data[3];
    //        $rootScope.GlobalFilter = response.data;
    //        $rootScope.currentfilter = $rootScope.defaultfilter;

    //        if (storageService.getStorage('CurrentFilter') == false) {

    //            storageService.setStorage('CurrentFilter', $rootScope.defaultfilter);

    //            $rootScope.currentfilter = $rootScope.defaultfilter;

    //        }
    //        else {

    //            $rootScope.currentfilter = storageService.getStorage('CurrentFilter').data;

    //        }
         

    //        return response.data;
    //    })
    //};

    //function resolveaccess($rootScope, commonService, $state) {
    //    var access = commonService.getuseraccesss();

    //    if (!access.IsModuleAccess) {
       
    //        setTimeout(function () {
    //            var lastPart = window.location.href.split("#/").pop();
    //            lastPart = lastPart.replace(/\\/g, '')
            
    //            var state = lastPart;
    //            if (state !== "dashboard") {
    //                $state.go('dashboard');
    //            }
             

    //            Metronic.unblockUI();
    //        });
    //    }
    //};



    $stateProvider
        // Login page
        //.state('login', {
        //    url: "/login",
        //    templateUrl: "app/pages/login/login.html" + version,
        //    data: {},
        //    controller: "",
        //    //resolve: {
        //    //    GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter]
        //    //}

        //})

        // Reset password
        //.state('resetPassword', {
        //    url: "/reset-password",
        //    templateUrl: "app/pages/reset-password/reset-password.html"+ version,
        //    data: {},
        //    controller: "ResetController",

        //})

        //// Change password
        //.state('changePassword', {
        //    url: "/change-password" + version,
        //    templateUrl: "app/pages/change-password/change-password.html" + version,
        //    data: {
        //        pageTitle: 'Change Password', pageSubTitle: '', roles: ['User', 'Tool Admin']
        //    },
        //    controller: "ChangePasswordController",

        //})


     //       .state('dashboard', {
     //           url: "/dashboard",
     //           templateUrl: "app/pages/dashboard/dashboard.html"+version,
     //           data: {
     //               pageTitle: 'Dashboard', pageSubTitle: 'statistics & reports', roles: ['User'], permissions: false
     //           },
     //           controller: "DashboardController",
     //           resolve: {
     //               GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter]
                  
     //           }

     //       })

     //.state('sales', {
     //    url: "/sales/:sales"+version,
     //    templateUrl: "app/pages/sales/sales.html"+version,
     //    data: { pageTitle: 'Sales', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
     //    controller: "SalesController",
     //    resolve: {
     //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
     //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
     //    }

     //})

    //.state('casessold', {
    //    url: "/casessold" + version,
    //    templateUrl: "app/pages/cases-sold/cases-sold.html"+version,

    //    data: { pageTitle: 'Cases Sold', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "CaseSoldController",
       

    //})

    //  .state('finance', {
    //      url: "/finance" + version,
    //      templateUrl: "app/pages/finance/finance.html" + version,

    //      data: { pageTitle: 'Finance', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //      controller: "FinanceController",
    //      resolve: {
    //          GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //          AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //      }

    //  })
    

   //.state('casessolddisplay', {
   //    url: "/casessolddisplay" + version,
   //    templateUrl: "app/pages/cases-sold/CasesSold_Display.html" + version,

   //    data: { pageTitle: 'Cases Sold', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
   //    controller: "CaseSoldController",
   //    resolve: {
   //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
   //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
   //    }

   //})
   //  .state('salesdisplay', {
   //      url: "/salesdisplay" + version,
   //      templateUrl: "app/pages/sales/Sales_Display.html" + version,

   //      data: { pageTitle: 'Sales', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
   //      controller: "SalesController",
   //      resolve: {
   //          GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
   //          AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
   //      }

   //  })

    

       

    //.state('expenses', {
    //    url: "/expenses/:category" + version,
    //    templateUrl: "app/pages/expenses/expenses.html"+version,
    //    data: { pageTitle: 'Expenses', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "ExpenseController",
    //    resolve: {
    //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }
    //})
    //.state('Profitablity', {
    //    url: "/Profitablity/:category" + version,
    //    templateUrl: "app/pages/profitability/profitability.html"+version,
    //    data: { pageTitle: 'Profitability', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "ProfitabiltyController",
    //    resolve: {
    //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }

    //})

    //.state('revenue', {
    //    url: "/revenue/:category" + version,
    //    templateUrl: "app/pages/revenue/revenue.html"+version,
    //    data: { pageTitle: 'Revenue', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "RevenueController",
    //    resolve: {
    //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }

    //})
      //    .state('cases-sold-categories', {
      //        url: "/cases-sold-categories" + version,
      //        templateUrl: "app/pages/cases-sold-Revenue-categories/CasesSoldRevenueCategories.html" + version,

      //        data: { pageTitle: 'Cases Sold', pageSubTitle: 'by categories', parentMenu: 'dashboard', roles: ['User'] },
      //        controller: "CategoriesController",
      //        resolve: {
      //            GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
      //            AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
      //        }

      //    })
      //.state('revenue-categories', {
      //    url: "/revenue-categories/:category" + version,
      //    templateUrl: "app/pages/cases-sold-Revenue-categories/CasesSoldRevenueCategories.html" + version,
      //    data: { pageTitle: 'Sales', pageSubTitle: 'by Categories', parentMenu: 'dashboard', roles: ['User'] },
      //    controller: "CategoriesController",
      //    resolve: {
      //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
      //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
      //    }

      //})

        //.state('payroll-report', {
        //    url: "/payroll-report"+version,
        //    templateUrl: "app/pages/payroll/payroll-report.html"+version,
        //    data: { pageTitle: 'Payroll', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
        //    controller: "PayrollReportController",
        //    resolve: {
        //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
        //    }

        //})

        //   .state('potracking-report', {
        //       url: "/po-tracking-report" + version,
        //       templateUrl: "app/pages/item/po-tracking-report.html" + version,
        //       data: { pageTitle: 'Po Tracking', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
        //       controller: "PotrackingReportController",
        //       resolve: {
        //           AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
        //       }

        //   })
   

    
    //Sales Order No Bin

    // .state('sales-order-no-bin-report', {
    //     url: "/sales-order-no-bin-report"+version,
    //     templateUrl: "app/pages/warehouse/sales-order-no-bin-report.html"+version,
    //     data: { pageTitle: 'Sales Order No Bin Report', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //     controller: "SalesOrderNoBinReportController",
    //     resolve: {
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })


    // .state('dumpdonations-report', {
    //     url: "/dumpdonations-report"+version,
    //     templateUrl: "app/pages/warehouse/dumpdonations-report.html"+version,
    //     data: { pageTitle: 'Dump & Donation Report', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //     controller: "DumpdonationsReportController",
    //     resolve: {
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    //.state('costcomparison-report', {
    //    url: "/costcomparison-report"+version,
    //    templateUrl: "app/pages/item/costcomparison-report.html"+version,
    //    data: { pageTitle: 'Cost Comparison Report', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "CostcomparisonController",
    //    resolve: {
    //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }

    //})


    //.state('itemvendor-report', {
    //    url: "/itemvendor-report"+version,
    //    templateUrl: "app/pages/item/Item-vendor-report.html"+version,
    //    data: { pageTitle: 'Item Vendor Report', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "ItemVendorReportController",
    //    resolve: {
    //        GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }

    //})






    //    .state('revenue-report', {
    //        url: "/revenue-report"+version,
    //        templateUrl: "app/pages/revenue/revenue-report.html"+version,
    //        data: { pageTitle: 'Revenue Report', pageSubTitle: 'statistics & reports', parentMenu: 'report', roles: ['User'] },
    //        controller: "RevenueReportController",
    //        resolve: {
                
    //            AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //        }

    //    })

    // .state('warehouse-so-short-report', {

    //     url: "/warehouse-so-short-report" + version,
    //     templateUrl: "app/pages/warehouse/so-short-report.html"+version,
    //     data: { pageTitle: 'SO Short Report', pageSubTitle: 'statistics & reports', parentMenu: 'warehouse', roles: ['User'] },
    //     controller: "WarehouseSoShortReportController",
    //     resolve: {

    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    // .state('warehouse-wms-picker-productivity-report', {
    //     //url: "/warehouse/:wmsPickerProductivityreport",
    //     url: "/warehouse-wms-picker-productivity-report/:date" + version,
    //     templateUrl: "app/pages/warehouse/wms-picker-productivity-report.html"+version,
    //     data: { pageTitle: 'WMS Picker Productivity Report', pageSubTitle: 'statistics & reports', parentMenu: 'warehouse', roles: ['User'] },
    //     controller: "WarehouseWmsPickerProductivityReportController",
    //     resolve: {

    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    // .state('warehouse', {
    //     url: "/warehouse/:warehouse" + version,
    //     templateUrl: "app/pages/warehouse/warehouse.html"+version,
    //     data: { pageTitle: 'Warehouse', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //     controller: "WarehouseController",
    //     resolve: {
    //         GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })
    //.state('user-management', {
    //    url: '/user-management'+ version,
    //    templateUrl: 'app/pages/tool-admin/user-management/user-manager.html?v=' + version,
    //    data: { pageTitle: 'User Management', roles: ['Tool Admin'] },
    //    controller: 'UserManagement',

    //})

    // .state('collector-config', {
    //     url: '/collector-config' + version,
    //     templateUrl: 'app/pages/tool-admin/collector-config/collector-config.html?v=' + version,
    //     data: { pageTitle: 'Collector Customer Mapping', roles: ['Tool Admin'] },
    //     controller: 'CollectorConfigController',

    // })

    


    // .state('expenses-report', {
    //     url: "/expenses-report"+version,
    //     templateUrl: "app/pages/expenses/expenses-report.html"+version,
    //     data: { pageTitle: 'Expenses Report', pageSubTitle: 'statistics & reports', parentMenu: 'report' },
    //     controller: "ExpensesReportController",
    //     resolve: {

    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    // .state('sales-report', {
    //     url: "/sales-report"+version,
    //     templateUrl: "app/pages/sales/sales-report.html?" + version,
    //     data: { pageTitle: 'Sales Report', pageSubTitle: 'statistics & reports', parentMenu: 'report', roles: ['User'] },
    //     controller: "SalesReportController",
    //     resolve: {

    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    .state('Categories', {
        url: "/Categories" + version,
        templateUrl: "app/pages/Categories/Categories.html" + version,
        data: { pageTitle: 'Categories List', pageSubTitle: '', parentMenu: 'report', roles: ['User'] },
        controller: "CategoryController",
      

    })

    //    .state('Commodity-report', {
    //        url: "/Commodity-report"+version,
    //        templateUrl: "app/pages/expenses/Commodity-report.html"+version,
    //        data: { pageTitle: 'Commodity Report', pageSubTitle: 'statistics & reports', parentMenu: 'report', roles: ['User'] },
    //        controller: "CommodityReportController",
    //        resolve: {
    //            AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //        }

    //    })

    // .state('transportation', {
    //     url: "/transportation"+version,
    //     templateUrl: "app/pages/transportation/transportation.html"+version,
    //     data: { pageTitle: 'Transportation', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //     controller: "transportationController",
    //     resolve: {
    //         GlobalFilter: ['dataService', '$rootScope', 'storageService', resolveGlobalFilter],
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }



    // })
    //       .state('transportation-driver-report', {
    //           url: "/transportation-driver-report" + version,
    //           templateUrl: "app/pages/transportation/transportation-driver-report.html" + version,
    //           data: { pageTitle: 'Transportation', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //           controller: "TransportationReportController",
    //           resolve: {
    //               AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //           }

    //    })

    

    //    .state('transportation-route-report', {
    //        url: "/transportation-route-report" + version,
    //        templateUrl: "app/pages/transportation/transportation-route-report.html" + version,
    //        data: { pageTitle: 'Transportation', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //        controller: "TransportationRouteReportController",
    //        resolve: {
    //            AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //        }

    //    })


    //  .state('sales-person-mapping', {
    //      url: "/sales-person-mapping"+version,
    //      templateUrl: "app/pages/sales/sales-person-mapping.html" + version,
       
    //      data: { pageTitle: 'Sales Person Mapping', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['Tool Admin'] },
    //      controller: "SalesPersonMapController",
    //      resolve: {
    //          AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //      }

    //  })


    //.state('sales-person-analysis-report', {
    //    url: "/sales-person-analysis-report"+version,
    //    templateUrl: "app/pages/sales/sales-person-analysis-report.html"+version,
    //    data: { pageTitle: 'Sales Person Analysis Report', pageSubTitle: 'statistics & reports', parentMenu: 'dashboard', roles: ['User'] },
    //    controller: "SalesPersonAnalysisReportController",
    //    resolve: {
    //        AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //    }

    //})
    // .state('item-report', {
    //     url: "/item-report"+version,
    //     templateUrl: "app/pages/item/item-report.html"+version,
    //     data: { pageTitle: 'Average Cost Report', pageSubTitle: 'Reports', parentMenu: 'report', roles: ['User'] },
    //     controller: "ItemReportController",
    //     resolve: {
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })
    // .state('customer-report', {
    //     url: "/customer-report"+version,
    //     templateUrl: "app/pages/customer/customer-report.html"+version,
    //     data: { pageTitle: 'Customer Report', pageSubTitle: 'Reports', parentMenu: 'report', roles: ['User'] },
    //     controller: "CustomerReportController",
    //     resolve: {
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })
    //  .state('lost-order-report', {
    //      url: "/lost-order-report"+version,
    //      templateUrl: "app/pages/customer/lost-order-report.html"+version,
    //      data: { pageTitle: 'Lost Order Report', pageSubTitle: 'Reports', parentMenu: 'report', roles: ['User'] },
    //      controller: "LostOrderReportController",
    //      resolve: {
    //          AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //      }

    //  })

    // .state('sales-analysis-report', {
    //     url: "/sales-analysis-report"+version,
    //     templateUrl: "app/pages/sales/sales-analysis-report.html"+version,
    //     data: { pageTitle: 'Sales Analysis', pageSubTitle: 'Reports', parentMenu: 'report', roles: ['User'] },
    //     controller: "SalesAnalysisReportController",
    //     resolve: {
    //         AccessCheck: ['$rootScope', 'commonService', '$state', resolveaccess]
    //     }

    // })

    //.state('sales-person-margin-report', {
    //    url: "/sales-person-margin-report",
    //    templateUrl: "app/pages/sales/sales-person-margin-report.html?v=" + version,
    //    data: { pageTitle: 'Sales Person Margin', pageSubTitle: 'Reports', parentMenu: 'report', roles: ['User'] },
    //    controller: "SalesPersonMarginReportController",

    //})






}])


MetronicApp.run(['$state', 'authService', '$rootScope', function ($state, authService, $rootScope) {
    // TO DO: use correct logic to check whether the user is logged in or not while reloading the page
    // $rootScope.GlobalFilterID = 2;

    authService.fillAuthData();
    if (authService.isAuthenticated()) {
        // go to currently request state directly
    }
    else {
        window.location.href = '#login'
        //$state.go('login');
    }

    window.gettotal = false;

    //$rootScope.$on('$stateChangeStart',
    //    function (event, toState, toParams, fromState, fromParams, options) {
    //        //console.log("state change initiated - From state " + fromState.name + ", to state " + toState.name);

    //        if (toState.name == "login" || toState.name == "resetPassword") {
    //            // do nothing
    //            //console.log("do nothing");
    //        }
    //        else if (!authService.isAuthenticated()) {
    //            // go to login page
    //            event.preventDefault();
    //            //console.log("not authenticated");
    //            $state.go('login');
    //        }
    //        else if (toState.data && toState.data.roles && toState.data.roles.length > 0) {
    //            if (toState.data.roles.indexOf(authService.user.role) < 0) {
    //                //console.log("not authorized");
    //                // User not authorized to access the page
    //                event.preventDefault();
    //                if (fromState.name == "")
    //                    $state.go('login');
    //            }
    //        }
    //    });
}]);

