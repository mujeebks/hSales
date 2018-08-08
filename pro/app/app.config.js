
/* Configure ocLazyLoader(refer: https://github.com/ocombe/ocLazyLoad) */
//MetronicApp.config(['$ocLazyLoadProvider', '$httpProvider', function ($ocLazyLoadProvider, $httpProvider) {
//    $ocLazyLoadProvider.config({
//        cssFilesInsertBefore: 'ng_load_plugins_before' // load the above css files before a LINK element with this ID. Dynamic CSS files must be loaded between core and theme css files
//    });

//    // Adding intercepter to every http request
//    $httpProvider.interceptors.push("authInterceptor");
//}]);

/********************************************
 BEGIN: BREAKING CHANGE in AngularJS v1.3.x:
*********************************************/
/**
`$controller` will no longer look for controllers on `window`.
The old behavior of looking on `window` for controllers was originally intended
for use in examples, demos, and toy apps. We found that allowing global controller
functions encouraged poor practices, so we resolved to disable this behavior by
default.

To migrate, register your controllers with modules rather than exposing them
as globals:

Before:

```javascript
function MyController() {
  // ...
}
```

After:

```javascript
angular.module('myApp', []).controller('MyController', [function() {
  // ...
}]);

Although it's not recommended, you can re-enable the old behavior like this:

```javascript
angular.module('myModule').config(['$controllerProvider', function($controllerProvider) {
  // this option might be handy for migrating old apps, but please don't use it
  // in new ones!
  $controllerProvider.allowGlobals();
}]);
**/

//AngularJS v1.3.x workaround for old style controller declarition in HTML
MetronicApp.config(['$controllerProvider', '$httpProvider', '$compileProvider', function ($controllerProvider, $httpProvider, $compileProvider) {
    // this option might be handy for migrating old apps, but please don't use it
    // in new ones!
    $controllerProvider.allowGlobals();
    $httpProvider.interceptors.push("authInterceptor");
    $compileProvider.debugInfoEnabled(false);


  //  $httpProvider.useApplyAsync(true);
}]);

/********************************************
 END: BREAKING CHANGE in AngularJS v1.3.x:
*********************************************/

