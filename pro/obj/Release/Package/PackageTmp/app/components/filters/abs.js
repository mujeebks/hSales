//to convert the number to its positive value



MetronicApp.filter('abs', function () {
    return function (num) {

        if (num)
        {

            return Math.abs(num);

        }

    }
})


.filter('startFrom', function () {
    return function (input, start) {
        start = +start; //parse to int
        if (input != undefined && input.length > 0)
            return input.slice(start);
    }
})
    .filter('formatMonth', function () {
        return function(num)
        {
            var monthNames = [ 'January', 'February', 'March', 'April', 'May', 'June',
            'July', 'August', 'September', 'October', 'November', 'December'];
            return num + ' - '+monthNames[num - 1];
        }
    })



.filter('htmlToPlaintext', function() {
    return function (text) {

        var text1 = text ? String(text).replace(/<\/?[^>]+(>|$)/g, "") : '';
        text1 = text1.replace(/_/g, '-');

        return text1;
    };
})


.filter('formatCommaSeperate', function () {

    return function (num) {

            //if (num == 0) {
            //    return num;
            //}
            //else {
            //    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        //}
        if (num) {
            num = (num == 0) ? num : num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
            return num;
        }
        else {
            return num;
        }
     
       





    };
})


.filter('numberWithCommas', function () {

    return function (x) {
        if (x) {
            var val = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            //console.log(val);

            const actualNumber = +val.replace(/,/g, '')
            const formatted = actualNumber.toLocaleString('en-US', { maximumFractionDigits: 2 })
            return formatted;
        }
        else {
            return 0;
        }
       

    };
})


.filter('numberWithCommasRounded', function () {

        return function (x) {
            if (x) {
                var val = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                //console.log(val);

                const actualNumber = +val.replace(/,/g, '')
                const formatted = Math.round(actualNumber).toLocaleString('en-US', { maximumFractionDigits: 2 })
                return formatted;
            }
            else {
                return 0;
            }


        };
    })


// start from filter for the inner group by
.filter('innerGroupPagination', function () {
    return function (input, start, limitTo) {
        var filtered = {};
        start = +start; //parse to int
        limitTo = +limitTo; // parse to int
        limitTo = start + limitTo;
        var count = 0;
        if (input != undefined) {
            for (var property in input) {
                if (input.hasOwnProperty(property)) {
                    if (count >= start && count < limitTo) {
                        filtered[property] = input[property];
                    }
                    count++;
                    if (count > limitTo) {
                        break;
                    }
                }
            }
        }
        //input = filtered;
        return filtered;
    }

})

.filter('secondsToDate', [
  function() {
      return function(seconds) {
          return new Date(1970, 0, 1).setSeconds(seconds);
      };
  }  
])

.filter('filterdateformat', ['$filter', function ($filter) {
 
      return function (date) {
          return $filter('date')(date,'dd-MM-yyyy');
      };
  }  
]);