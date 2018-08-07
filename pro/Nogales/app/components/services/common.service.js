
MetronicApp.factory('commonService', ['$state', function ($state) {

    function initBtnSpinner(id) {
        return Ladda.create(document.querySelector(id));
    };
    function getuseraccesss() {
        function formatstate(word) {
            word = word.replace(/ +/g, "");
            word = word.toLowerCase();
            return word;
        };

        function FindParentmodule(state) {
        
            var parent;
            switch (state) {
                case "cases-sold-categories":
                    parent = "casessold";
                    break;
                case "revenue-categories":
                    parent = "sales";
                    break;
                case "sales-order-no-bin-report":
                    parent = "warehouse";
                    break;
                case "dumpdonations-report":
                    parent = "Buyers";
                    break;
                case "costcomparison-report":
                    parent = "Buyers";
                    break;
                case "itemvendor-report":
                    parent = "Buyers";
                    break;
                case "revenue-report":
                    parent = "sales";
                    break;
                case "warehouse-so-short-report":
                    parent = "warehouse";
                    break;
                case "expenses-report":
                    parent = "expenses";
                    break;
                case "sales-report":
                    parent = "sales";
                    break;
                case "casesold-report":
                    parent = "casessold";
                    break;
                case "Commodity-report":
                    parent = "expenses";
                    break;
                case "sales-person-mapping":
                    parent = "sales";
                    break;
                case "sales-person-analysis-report":
                    parent = "sales";
                    break;
                case "item-report":
                    parent = "Buyers";
                    break;
                case "customer-report":
                    parent = "customer";
                    break;
                case "sales-analysis-report":
                    parent = "sales";
                    break;
                case "payroll-report":
                    parent = "expenses";
                    break;
                case "transportation-driver-report":
                    parent = "transportation";
                    break;
                case "transportation-route-report":
                    parent = "transportation";
                    break;

                    
                //case "payroll-report":
                //    parent = "Payroll";
                //    break;
                default:
                    parent = state;

                   
            }

            return parent;
        };
        debugger
        var obj = {"state":"","Categories":[],"Modules":[],"IsModuleAccess":true};
        if (typeof localStorage === 'object') {
            try {
               
                var lastPart = window.location.href.split("#/").pop();
                lastPart = lastPart.replace(/\\/g, '')

                var state = lastPart;
                obj["state"] = state;
                state = FindParentmodule(state);
                var nogalesAuthAccess = localStorage.getItem('ls.nogalesAuthAccess');
                nogalesAuthAccess = JSON.parse(nogalesAuthAccess);
              
                if (nogalesAuthAccess && nogalesAuthAccess.Categories) {
                 
                    obj["Categories"] = nogalesAuthAccess.Categories;
                    obj["Modules"] = nogalesAuthAccess.Modules;

                    for (var i = 0; i < nogalesAuthAccess.Modules.length; i++) {
                        var stat = formatstate(state);
                       
                        var Name = formatstate(nogalesAuthAccess.Modules[i].Name);
                     
                        var matches = stat.indexOf(Name);
                        if (matches>-1) {
                            var isaccess = nogalesAuthAccess.Modules[i].IsAccess;
                                            
                            obj["IsModuleAccess"] = isaccess;
                            break;
                        }

                    }
                }
             
              
            } catch (e) {
                return null;
                Storage.prototype.setItem = function () { };
                alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
            }
        }
        return obj;
         
    };

    return {
        InitBtnSpinner: initBtnSpinner,
        getuseraccesss: getuseraccesss
    }
}]);


MetronicApp.factory('storageService', ['NotificationService', '$http', '$rootScope', function (NotificationService, $http, $rootScope) {


    function setStorage(key, data) {

        if (typeof localStorage === 'object') {
            try {
                localStorage.removeItem(key);
                var time = new Date().getTime()
                
                var JsonData = {

                    date: new Date().toJSON().slice(0, 10).replace(/-/g, '/'),
                    data: data,
                    time: time
                }
                localStorage.setItem(key, JSON.stringify(JsonData));
            } catch (e) {
                Storage.prototype.setItem = function () { };
                alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
            }
        }

    };

    function getStorage(key) {

        if (typeof localStorage === 'object') {
            try {
                var expiryMinute = 60;
                var item = localStorage.getItem(key);
                if (item == null) {
                    return false;
                }
                else {

                    var data = localStorage.getItem(key);
                    var json = JSON.parse(data);
                    var timediff = new Date().getTime() - json.time;
                    var mindiff = Math.floor(timediff / 1000 / 60);

                    if (mindiff >= expiryMinute) {
                        if (key == 'CurrentFilter') {
                          
                        }
                        else {
                              localStorage.removeItem(key);
                        }
                        //console.log('%c Session Expired Fetched New Data" ', 'background: white; color: red');

                        json = false;

                    }
                    else {
                        //console.log('%c Fetched Data From Session Expires in "' + (expiryMinute - mindiff) + " Minutes", 'background: white; color: #1663DE ');

                    }


                    return json
                }
            } catch (e) {

                Storage.prototype.setItem = function () { };
                alert('Your web browser does not support storing settings locally. In Safari, the most common cause of this is using "Private Browsing Mode". Some settings may not save or some features may not work properly for you.');
            }
        }

    }

    return {
        setStorage: setStorage,
        getStorage: getStorage,

    }
}]);