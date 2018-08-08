
// ---------- Define application constants ----------

// Set true for development other wise set false
MetronicApp.value("isDebugging", true);

MetronicApp.constant('SessionOutDisabledUsers', ['npi@nogalesproduce.com']);


// display chart timeout in milli seconds
MetronicApp.constant('DisplayChartTimeout', 300000); // 5 minute

//MetronicApp.constant('DisplayChartTimeout', 60000); // 5 minute

if (window.location.href.indexOf('localhost') > 0) {
  
    MetronicApp.constant('ApiUrl', 'http://192.168.10.165:4001/api/');

   // MetronicApp.constant('ApiUrl', 'http://dev-dashboard.nogalesproduce.com/api/');

    
   

}
else {
    MetronicApp.constant('ApiUrl', window.location.origin + '/api/');

}