
MetronicApp.directive('comingSoon', ['$interval', function ($interval) {

    return {
        replace: true,
        restrict: 'E',
        scope: {
            deadline: "=",
            heading: "=",
            content:"="
          
        },
        templateUrl: 'app/components/directives/coming-soon/coming-soon.html',
        link: function (scope, element, attrs) {

            var stop;
            //  var deadline = 'August 1 2018 23:59:59 GMT+0200';
            var deadline = scope.deadline;

            scope.stop = function () {
                $interval.cancel(stop);
            };

            function getTimeRemaining(endtime) {
                var t = Date.parse(endtime) - Date.parse(new Date());
                var seconds = Math.floor((t / 1000) % 60);
                var minutes = Math.floor((t / 1000 / 60) % 60);
                var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
                var days = Math.floor(t / (1000 * 60 * 60 * 24));
                // var week = (Math.floor(days / 7)==1 ? 0 : (Math.floor(days / 7)));
                return {
                    'total': t,
                    'days': days,
                    'hours': hours,
                    'minutes': minutes,
                    'seconds': seconds
                    // 'week': week
                };
            };
           
            function initializeClock(endtime) {
                scope.clock = "";
                stop = $interval(function () {
                    var t = getTimeRemaining(endtime);
                    scope.date = {
                        // weeks: t.week,
                        days: t.days,
                        hours: t.hours,
                        minutes: t.minutes,
                        seconds: t.seconds

                    };
                    debugger
                    if (t.total <= 0) {
                        $interval.cancel(stop);
                        scope.date = {
                            // weeks: t.week,
                            days: 0,
                            hours: 0,
                            minutes: 0,
                            seconds: 0

                        };
                    }
                }, 1000);
            };
            initializeClock(deadline);
         
        }
    }

}]);