

MetronicApp.directive('canvasdonut', ['$location', '$rootScope', '$filter', 'dataService', 'NotificationService',
     function ($location, $rootScope, $filter, dataService, NotificationService) {
         this.count = 0;
         return {
             restrict: 'E',
             replace: true,
             require: '?ngModel',
             scope: {nlLocation: '='},
             template: '<div class="portlet-body">'+
                            '<div>'+
                                '<div id="{{::myId}}" class="chart" style="width:100%;min-height:500px;"></div> '+
                                //'<div id="{{::legendDiv}}" class="clusteredBarChartLegenddiv"></div> '+
                            '</div> '+
                       '</div>',
             link: function (scope, element, attrs, controller) {
               
                 var chart;
                 var data;
                 scope.legendDiv = 'CanvasDonut_Legend' + count;
                 scope.myId = 'CanvasDonut_Chart' + (count++);
                 var customDataPoints = [];
                 var initChart = function (chartData, title, labelRotation,datapoints) {
                     var balloontextPrefix = "$";
                     var chart = new CanvasJS.Chart(scope.myId, {
                         //theme: "dark2",
                         exportFileName: "Doughnut Chart",
                         exportEnabled: false,
                         animationEnabled: true,
                         //title: {
                         //    text: "Monthly Expense"
                         //},
                         //legend: {
                         //    cursor: "pointer",
                         //    itemclick: explodePie
                         //},
                         data: [{
                             type: "doughnut",
                             innerRadius: 90,
                             showInLegend: false,
                             toolTipContent: "<b>{name}</b>: ${y} (#percent%)",
                             indexLabel: "{name} - #percent%",
                             //dataPoints: [
                     		 //   { y: 0, name: "Food" },
                     		 //   { y: 0, name: "Insurance" },
                     		 //   { y: 0, name: "Travelling" },
                     		 //   { y: 800, name: "Housing" },
                     		 //   { y: 150, name: "Education" },
                     		 //   { y: 150, name: "Shopping" },
                     		 //   { y: 250, name: "Others" }
                             //]
                             dataPoints: datapoints
                         }]
                     });
                     chart.render();
                     $(".canvasjs-chart-credit").css("display", "none");
                     function explodePie(e) {
                         if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                             e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                         } else {
                             e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                         }
                         e.chart.render();
                     }

                 };

                 scope.$watch(function () { return controller.$viewValue }, function (newVal) {
                     data = newVal;
                     chartTitle = "";
                     if (data == undefined || data.length == 0) {

                     }
                     else {
                         debugger
                         for (var i = 0; i < data.length; i++) {
                             var data1 = {
                                 y: data[i].cValue1,
                                 name: data[i].GroupName.trim()
                             }
                             customDataPoints.push(data1);
                         }


                         initChart(data, "", 0, customDataPoints);
                     }

                 });
             }
         };
     }]);
















//var chart = new CanvasJS.Chart("chartContainer", {
//    theme: "dark2",
//    exportFileName: "Doughnut Chart",
//    exportEnabled: true,
//    animationEnabled: true,
//    title: {
//        text: "Monthly Expense"
//    },
//    legend: {
//        cursor: "pointer",
//        itemclick: explodePie
//    },
//    data: [{
//        type: "doughnut",
//        innerRadius: 90,
//        showInLegend: true,
//        toolTipContent: "<b>{name}</b>: ${y} (#percent%)",
//        indexLabel: "{name} - #percent%",
//        dataPoints: [
//			{ y: 0, name: "Food" },
//			{ y: 0, name: "Insurance" },
//			{ y: 0, name: "Travelling" },
//			{ y: 800, name: "Housing" },
//			{ y: 150, name: "Education" },
//			{ y: 150, name: "Shopping" },
//			{ y: 250, name: "Others" }
//        ]
//    }]
//});
//chart.render();

//function explodePie(e) {
//    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
//        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
//    } else {
//        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
//    }
//    e.chart.render();
//}
