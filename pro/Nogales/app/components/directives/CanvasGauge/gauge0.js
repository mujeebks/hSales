
MetronicApp.directive("canvasgauge", ['$location', '$state', 'commonService', function ($location, $state, commonService) {
    this.count = 0;
    return {
        restrict: 'E',
        replace: false,

        require: '?ngModel',
        scope: { nlLocation: '=', current: '=', prior: '=', name: '=', revert: '=', showdollar: '=', currenttext: '=', priortext: '=', navigate: '=', icon: '=' },
        templateUrl: 'app/components/directives/CanvasGauge/CanvasGauge.html',

        link: function (scope, element, attrs, controller) {
            scope.showchart = false;
            scope.state = $state.current.name;
            scope.Icon = scope.icon;
            scope.ShowDollar = scope.showdollar;
            scope.NavigateTo = scope.navigate;
            scope.CurrentText = scope.currenttext;
            scope.PriorText = scope.priortext;
            scope.gaugearrays = [];
            scope.red = {};
            //  scope.yellow = {};
            scope.lowgreen = {};
            scope.mediumgreen = {};
            scope.highgreen = {};

            scope.chartid = 'CanvasGauge' + (count++)
            scope.isrevert = scope.revert;
            var initchart = function (start, end) {
                var gauge = new RadialGauge({
                    renderTo: scope.chartid,
                    width: 290,
                    height: 300,
                    //units: "%",
                    title: false,
                    value: scope.gaugevalue,
                    minValue: scope.gaugearrays[0],
                    maxValue: scope.gaugearrays[scope.gaugearrays.length - 1],
                    startAngle: 90,
                    ticksAngle: 180,
                    majorTicks: scope.gaugearrays,

                    minorTicks: 2,
                    valueBox: false,
                    border: false,
                    strokeTicks: true,
                    highlights: [
                        {
                            "from": scope.red.start,
                            "to": scope.red.end,
                            "color": (scope.isrevert) ? "#006600" : "#fa0000"
                        },
                        {
                            "from": scope.lowgreen.start,
                            "to": scope.lowgreen.end,
                            "color": (scope.isrevert) ? "#00CC00" : "#66FF66"
                        },
                        {
                            "from": scope.mediumgreen.start,
                            "to": scope.mediumgreen.end,
                            "color": (scope.isrevert) ? "#fa0000" : "#00CC00"
                        },
                         {
                             "from": scope.highgreen.start,
                             "to": scope.highgreen.end,
                             "color": (scope.isrevert) ? "#fa0000" : "#006600"
                         }

                    ],

                    colorPlate: "#fff",
                    borderShadowWidth: 0,
                    borders: false,
                    needleType: "arrow",
                    colorBarEnd: false,
                    colorMajorTicks: ["white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white", "white"],
                    minorTicks: false,
                    colorUnits: "#3CA7DB",
                    colorNumbers: "#534638",
                    colorNeedle: "black",
                    colorNeedleEnd: "black",
                    colorNeedleCircleOuter: "white",
                    colorNeedleCircleOuterEnd: "white",

                    colorNeedleShadowUp: "rgba(255,255,255,0.3)",
                    colorNeedleShadowDown: "#8E7860",

                    needleStart: 0,
                    needleEnd: 60,
                    needleWidth: 6,
                    fontNumbersSize: setfontwidth(scope.gaugearrays),

                    needleCircleSize: 25,
                    valueBox: false,
                    needleCircleOuter: true,
                    needleCircleInner: false,
                    animationDuration: 1500,
                    animationRule: "linear",

                }).draw();

            }

            var setfontwidth = function (array) {
                var width = 18;

                for (var i = 0; i < array.length; i++) {
                    if (Math.abs(array[i]).toString().length >= 4) {
                        width = 15;
                        break;
                    };
                }

                return width;
            };

            scope.$watch(function () { return controller.$viewValue }, function (newVal) {

                if (newVal !== undefined) {
                    scope.newVal = newVal;
                    scope.guageobject = newVal;

                    debugger


                    if (newVal > -10 && newVal < 100) {
                        scope.startvalue = -10;
                        scope.endvalue = 100;
                        scope.gaugearrays = scope.getarrofguagevals(-10, 100);

                        scope.red.start = -10;
                        scope.red.end = 0;

                        scope.lowgreen.start = 0;
                        scope.lowgreen.end = 10;

                        scope.mediumgreen.start = 10;
                        scope.mediumgreen.end = 20;

                        //scope.yellow.start = 0;
                        //scope.yellow.end = 20;

                        scope.highgreen.start = 20
                        scope.highgreen.end = 100;

                        scope.gaugevalue = newVal;

                    }
                    else if (newVal > -20 && newVal < 200) {
                        scope.startvalue = -20;
                        scope.endvalue = 200;
                        scope.gaugearrays = scope.getarrofguagevals(-20, 200);

                        scope.red.start = -20;
                        scope.red.end = 0;

                        //scope.yellow.start = 0;
                        //scope.yellow.end = 20;

                        scope.lowgreen.start = 0;
                        scope.lowgreen.end = 10;

                        scope.mediumgreen.start = 10;
                        scope.mediumgreen.end = 20;

                        scope.highgreen.start = 20
                        scope.highgreen.end = 200;

                        scope.gaugevalue = newVal;

                    }
                    debugger
                    if (newVal > 200) {
                        scope.gaugearrays = scope.getarrofguagevals(-20, newVal + 100);

                        scope.startvalue = scope.gaugearrays[0];
                        scope.endvalue = scope.gaugearrays[scope.gaugearrays.length - 1];

                        scope.red.start = -20;
                        scope.red.end = scope.gaugearrays[1];

                        //scope.yellow.start = scope.gaugearrays[1];
                        //scope.yellow.end = scope.gaugearrays[2];

                        scope.lowgreen.start = scope.gaugearrays[1];
                        scope.lowgreen.end = scope.lowgreen.start / 2;

                        scope.mediumgreen.start = scope.lowgreen.start / 2;
                        scope.mediumgreen.end = scope.gaugearrays[2];




                        scope.highgreen.start = scope.gaugearrays[2];
                        scope.highgreen.end = scope.gaugearrays[scope.gaugearrays.length - 1];

                        scope.gaugevalue = newVal;

                    }
                    if (newVal <= -20 && newVal > -100) {

                        scope.startvalue = -100;
                        scope.endvalue = 10;
                        scope.gaugearrays = scope.getarrofguagevals(-100, 10);

                        scope.red.start = -100;
                        scope.red.end = scope.gaugearrays[scope.gaugearrays.length - 4];

                        //scope.yellow.start = scope.gaugearrays[scope.gaugearrays.length - 4];
                        //scope.yellow.end = scope.gaugearrays[scope.gaugearrays.length - 2];


                        scope.lowgreen.start = scope.gaugearrays[scope.gaugearrays.length - 4];
                        scope.lowgreen.end = scope.lowgreen.start / 2;

                        scope.mediumgreen.start = scope.lowgreen.start / 2;
                        scope.mediumgreen.end = scope.gaugearrays[scope.gaugearrays.length - 2];

                        scope.highgreen.start = scope.gaugearrays[scope.gaugearrays.length - 2];
                        scope.highgreen.end = scope.gaugearrays[scope.gaugearrays.length - 1];

                        scope.gaugevalue = newVal;

                    } else if (newVal < -20 && newVal > -200) {

                        scope.startvalue = -200;
                        scope.endvalue = 20;
                        scope.gaugearrays = scope.getarrofguagevals(-200, 20);

                        scope.red.start = -200;
                        scope.red.end = scope.gaugearrays[scope.gaugearrays.length - 3];

                        //scope.yellow.start = scope.gaugearrays[scope.gaugearrays.length - 3];
                        //scope.yellow.end = scope.gaugearrays[scope.gaugearrays.length - 2];

                        scope.lowgreen.start = scope.gaugearrays[scope.gaugearrays.length - 3];
                        scope.lowgreen.end = scope.lowgreen.start / 2;

                        scope.mediumgreen.start = scope.lowgreen.start / 2;
                        scope.mediumgreen.end = scope.gaugearrays[scope.gaugearrays.length - 2];

                        scope.highgreen.start = scope.gaugearrays[scope.gaugearrays.length - 2];
                        scope.highgreen.end = scope.gaugearrays[scope.gaugearrays.length - 1];

                        scope.gaugevalue = newVal;

                    }

                    else if (newVal < -200) {

                        scope.gaugearrays = scope.getarrofguagevals(newVal - 100, 20);
                        scope.startvalue = scope.gaugearrays[0];
                        scope.endvalue = scope.gaugearrays[scope.gaugearrays.length - 1];


                        scope.red.start = scope.gaugearrays[0];
                        scope.red.end = scope.gaugearrays[scope.gaugearrays.length - 3];

                        //scope.yellow.start = scope.gaugearrays[scope.gaugearrays.length - 3];
                        //scope.yellow.end = scope.gaugearrays[scope.gaugearrays.length - 2];

                        scope.lowgreen.start = scope.gaugearrays[scope.gaugearrays.length - 3] + 1;
                        scope.lowgreen.end = scope.lowgreen.start / 2;

                        scope.mediumgreen.start = scope.lowgreen.end + 1;
                        scope.mediumgreen.end = scope.gaugearrays[scope.gaugearrays.length - 2];

                        scope.highgreen.start = scope.gaugearrays[scope.gaugearrays.length - 2] + 1;
                        scope.highgreen.end = scope.gaugearrays[scope.gaugearrays.length - 1];

                        scope.gaugevalue = newVal;

                    }


                    if (newVal > -10 && newVal < 45) {

                        scope.startvalue = -10;
                        scope.endvalue = 45;
                        scope.gaugearrays = scope.getarrofguagevals(-10, 45, true);

                        scope.red.start = -10;
                        scope.red.end = 0;

                        //scope.yellow.start = 0;
                        //scope.yellow.end = 20;

                        scope.lowgreen.start = 0;
                        scope.lowgreen.end = 5;

                        scope.mediumgreen.start = 5;
                        scope.mediumgreen.end = 15;

                        scope.highgreen.start = 15
                        scope.highgreen.end = 45;

                        scope.gaugevalue = newVal;

                    }
                    if (newVal > -30 && newVal <= -10) {

                        scope.startvalue = -30;
                        scope.endvalue = 25;
                        scope.gaugearrays = scope.getarrofguagevals(-30, 25, true);

                        scope.red.start = -30;
                        scope.red.end = 0;

                        //scope.yellow.start = 0;
                        //scope.yellow.end = 10;

                        scope.lowgreen.start = 0;
                        scope.lowgreen.end = 5;

                        scope.mediumgreen.start = 5;
                        scope.mediumgreen.end = 10;

                        scope.highgreen.start = 10
                        scope.highgreen.end = 25;

                        scope.gaugevalue = newVal;

                    }
                }

                scope.percentageDisplay = Math.round(scope.newVal) + '%';
                scope.percentage = Math.round(scope.newVal);
                //scope.GaugeCurrent = scope.current;
                //scope.GaugePrior = scope.prior;
                scope.GaugeName = scope.name;
                initchart(scope.startvalue, scope.endvalue);


            });
            function formatstate(word) {
                word = word.replace(/ +/g, "");
                word = word.toLowerCase();
                return word;
            };
            scope.navigatepage = function () {

                var access = commonService.getuseraccesss().Modules;

                if (access.length > 0) {

                    var accessobj = access.filter(function (item) {
                        return formatstate(item.Name) == formatstate(scope.NavigateTo);
                    });

                    if (accessobj.length > 0) {
                        if (accessobj[0].IsAccess) {
                            $state.go(scope.NavigateTo);
                        }
                        else {

                        }
                    }

                }

            };
            scope.$watch(function () { return scope.current }, function (newVal) {
                scope.GaugeCurrent = newVal;

                if (Math.abs(newVal) >= 0) {
                    scope.showchart = true;
                }
                //else {
                //    scope.showchart = false;
                //}
            });
            scope.$watch(function () { return scope.prior }, function (newVal) { scope.GaugePrior = newVal; });
            scope.getarrofguagevals = function (a, b, isincremnet) {
                if (a < -50)
                    a = Math.ceil(a / 50) * 50;
                else
                    b = Math.ceil(b / 50) * 50


                var incremenentvalue = Math.round((Math.abs(a) + Math.abs(b)) / 11);

                incremenentvalue = (isincremnet) ? 5 : Math.ceil(incremenentvalue / 10) * 10
                var arr = [];
                for (var i = 0; i < 12 ; i++) {
                    if (i == 0) {
                        arr.push(a)
                    }
                    else {
                        arr.push(arr[arr.length - 1] + incremenentvalue);
                    }
                }

                return arr.sort(function (a, b) { return a - b });

            }



        }
    };
}]);

