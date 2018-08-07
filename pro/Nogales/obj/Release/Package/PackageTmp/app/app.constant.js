
// ---------- Define application constants ----------

// Set true for development other wise set false
MetronicApp.value("isDebugging", true);

MetronicApp.constant('SessionOutDisabledUsers', ['npi@nogalesproduce.com']);


if (window.location.href.indexOf('localhost') > 0) {

    MetronicApp.constant('ApiUrl', 'http://localhost:55335/');

   // MetronicApp.constant('ApiUrl', 'http://dev-dashboard.nogalesproduce.com/api/');

    
   

}
else {
    MetronicApp.constant('ApiUrl', window.location.origin + '/api/');

}