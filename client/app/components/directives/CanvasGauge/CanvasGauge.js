

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
                            "color": "#fa0000"
                        },
                        {
                            "from": scope.lowgreen.start,
                            "to": scope.lowgreen.end,
                            "color": "#66FF66"
                        },
                        {
                            "from": scope.mediumgreen.start,
                            "to": scope.mediumgreen.end,
                            "color": "#00CC00"
                        },
                         {
                             "from": scope.highgreen.start,
                             "to": scope.highgreen.end,
                             "color": "#006600"
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

                    

                    //Change between -10 to 100
                    if (newVal > -10 && newVal < 100) {
                        var listGaugeValues = generateMeterPlots(-10, 100, newVal);
                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0, 100, -5, 0, -10, -5, 0, 0);
                        else
                            setColorToRanges(-10, 0, 0, 5, 5, 15, 15, 100);
                    }
                        //Change between -20 to 200
                    else if (newVal > -20 && newVal < 200) {
                        generateMeterPlots(-20, 200, newVal);
                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0, 200,-15,0,-20,-15,0,0);
                        else
                            setColorToRanges(-20, 0, 0, 5, 5, 15, 15, 200);
                    }
                    //Change greater than 200
                    if (newVal >= 200) {
                        var listGaugeValues = generateMeterPlots(-20, newVal + 100, newVal);
                        scope.startvalue = listGaugeValues[0];
                        scope.endvalue = listGaugeValues[listGaugeValues.length - 1];

                        //Expense
                        if (scope.isrevert)
                            //setColorToRanges(listGaugeValues[1], listGaugeValues[listGaugeValues.length - 1]
                            //                , 0, 0
                            //                , 0, 0
                            //                , listGaugeValues[0], listGaugeValues[1]);
                            setColorToRanges(0, listGaugeValues[listGaugeValues.length - 1], -15, -15, -20, 0, 0);

                        else
                            setColorToRanges(-20, 0, 0, 5, 5, 15, 15,listGaugeValues[listGaugeValues.length - 1]);
                    }
                    //change between -20 and -100
                    if (newVal <= -20 && newVal > -100) {

                        var listGaugeValues = generateMeterPlots(-100, 10, newVal);
                        //Expense
                        if (scope.isrevert)
                            //setColorToRanges(0, listGaugeValues[listGaugeValues.length - 1], 0, 0, 0, 0, -100, 0);

                            setColorToRanges(0, 10, -5, 0, -15, -5, -100, -15);

                        else
                            setColorToRanges(-100, 0, 0, 5, 5, 10, 0, 0);

                    }
                        //change between -20 and -200
                    else if (newVal < -20 && newVal > -200) {

                        var listGaugeValues = generateMeterPlots(-200, 20, newVal);
                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0, 20, -5, 0, -15, -5, -200, -15);
                        else
                            setColorToRanges(-200,0,0,5,5,15,15,20);
                    }
                        //change less than -200
                    else if (newVal <= -200) {

                        var listGaugeValues = generateMeterPlots(newVal - 100, 20, newVal);
                        scope.startvalue = listGaugeValues[0];
                        scope.endvalue = listGaugeValues[listGaugeValues.length - 1];

                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0,listGaugeValues[listGaugeValues.length-1], -5, 0, -15, -5, listGaugeValues[0], -15);
                        else
                            setColorToRanges(listGaugeValues[0], 0, 0, 5, 5, 15, 15, listGaugeValues[listGaugeValues.length - 1]);
                    }
                    //change between -10 and 45
                    if (newVal > -10 && newVal < 45) {

                        generateMeterPlots(-10, 45, newVal, true);
                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0, 45, -5,0,-10,-5,0,0);
                        else
                            setColorToRanges(-10, 0, 0, 5, 5, 15, 15, 45);
                    }
                    //change between -30 to -10
                    if (newVal > -30 && newVal <= -10) {
                        var listGaugeValues = generateMeterPlots(-30, 25, newVal, true);
                        //Expense
                        if (scope.isrevert)
                            setColorToRanges(0, 25, -5, 0, -15, -5, -30, -15);
                        else
                            setColorToRanges(-30, 0, 0, 5, 5, 15, 15, 25);
                    }
                }

                scope.percentageDisplay = Math.round(scope.newVal) + '%';
                scope.percentage = Math.round(scope.newVal);
                //scope.GaugeCurrent = scope.current;
                //scope.GaugePrior = scope.prior;
                scope.GaugeName = scope.name;
                initchart(scope.startvalue, scope.endvalue);


            });

            function generateMeterPlots(startValue, endValue, newValue,isIncrement) {
                scope.startvalue = startValue;
                scope.endvalue = endValue;
                scope.gaugearrays = scope.getarrofguagevals(startValue, endValue, isIncrement);
                scope.gaugevalue = newValue;
                return scope.gaugearrays;
            };

            function setColorToRanges(redStart, redEnd, lightGreenStart, lightGreenEnd, mediumGreenStart, mediumGreenEnd, highGreenStart, highGreenEnd) {
                scope.red.start = redStart;
                scope.red.end = redEnd;

                scope.lowgreen.start = lightGreenStart;
                scope.lowgreen.end = lightGreenEnd;

                scope.mediumgreen.start = mediumGreenStart;
                scope.mediumgreen.end = mediumGreenEnd;

                scope.highgreen.start = highGreenStart;
                scope.highgreen.end = highGreenEnd;
            };
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

