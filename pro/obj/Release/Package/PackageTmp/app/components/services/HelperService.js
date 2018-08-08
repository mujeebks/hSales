MetronicApp.factory('HelperService', ['$filter',
function ($filter) {


    function initBtnSpinner(id) {
        return Ladda.create(document.querySelector(id));
    };
    function getBalloonTextPrefix(attrs) {
        return (attrs.nlDashboard == "casesold") ? ""
              : (attrs.nlDashboard == "revenue") ? "$"
              : (attrs.nlDashboard == "revenueDashboard") ? "$"
              : (attrs.nlDashboard == "expenses") ? "$" : "";
    };
    function getValueAxisTitle(attrs) {
        return (attrs.nlDashboard == "casesold") ? "Cases sold qty"
             : (attrs.nlDashboard == "revenue") ? "Revenue"
            : (attrs.nlDashboard == "revenueDashboard") ? "Revenue"
             : (attrs.nlDashboard == "expenses") ? "Expenses" : "";
    };
    function formatCommaSeperate(num) {
        if (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        }
        else {
            return 0;
        }
    };
    function calcPercentage(CurrentYear, PreviousMonth) {
        var diff = CurrentYear - PreviousMonth;
        if (PreviousMonth == 0) { return 100; }
        if (diff !== 0) {
            diff = diff / PreviousMonth;
            diff = diff * 100;
            //console.log((PreviousMonth < 0) ? 0 : diff);
            return (PreviousMonth < 0) ? 0 : diff;
        }
        else {
            return 0;
        }


    };
    function calcpercentagespan (graphDataItem, iscasesold, Previousmonth) {
        var first;
        var second;

        if (Previousmonth) {
            first = (iscasesold) ? (graphDataItem.dataContext.cValue5) + (graphDataItem.dataContext.cValue6) : (graphDataItem.dataContext.rValue5) + (graphDataItem.dataContext.rValue6);
            second = (iscasesold) ? (graphDataItem.dataContext.cValue3) + (graphDataItem.dataContext.cValue4) : (graphDataItem.dataContext.rValue3) + (graphDataItem.dataContext.rValue4);
        }
        else {
            first = (iscasesold) ? (graphDataItem.dataContext.cValue1) + (graphDataItem.dataContext.cValue2) : (graphDataItem.dataContext.rValue1) + (graphDataItem.dataContext.rValue2);
            second = (iscasesold) ? (graphDataItem.dataContext.cValue3) + (graphDataItem.dataContext.cValue4) : (graphDataItem.dataContext.rValue3) + (graphDataItem.dataContext.rValue4);
        }


        var PriorMonth = first - second;
        if (PriorMonth !== 0) {
            PriorMonth = PriorMonth / second;
            PriorMonth = PriorMonth * 100;
            PriorMonth = (PriorMonth == Infinity) ? 0 : PriorMonth;
        }
        else {
            PriorMonth = 0;
        }
       var Prior = Math.round(PriorMonth);
        var span = "";
        if (Prior > 0) {
            return span = "<span>(<span class='fa fa-sort-up' style='color:forestgreen'> " + Math.abs(Prior) + " %" + "</span>)</span>";
        }
        if (Prior < 0) {
            return span = "<span>(<span class='fa fa-sort-down' style='color:red'> " + Math.abs(Prior) + " %" + "</span>)</span>";
        }
        if (Prior == 0) {
            return span = "";
        }
    }
 
    //function getTopBoxEmptyStylePositive(statiticsData) {

    //    if (statiticsData != undefined) {
    //        var val = statiticsData.Change > 0 ? 50 - ($filter('abs')(statiticsData.Change)) / 2 : 50;
    //        return {
    //            width: val + "%"
    //        }
    //    }
    //};
    //function getTopBoxEmptyStyleNegative(statiticsData) {

    //    if (statiticsData != undefined) {

    //        var val = statiticsData.Change < 0 ? 50 - ($filter('abs')(statiticsData.Change)) / 2 : 50;
    //        return {
    //            width: val + "%"
    //        }
    //    }

    //};
    //function getTopBoxSuccessStyle(data) {

    //    if (data != undefined) {
    //        var da = $filter('abs')(data);
    //        var val = data > 0 ? (da) / 2 : 0
    //        return {
    //            width: val + "%"

    //        }
    //    }
    //};
    //function getTopBoxFailStyle(data) {
        
    //    if (data != undefined) {
    //        var da = $filter('abs')(data);
    //        var val = data < 0 ? (da) / 2 : 0
    //        return {
    //            width: val + "%"

    //        }

    //    }
    //};

    function getPriorRangeText(filterName) {
        switch (filterName) {
            case "This Week": return "Prior Week";
            case "This Week To Date": return "Prior Week To Date";
            case "This Month": return "Prior Month";
            case "This Month To Date": return "Prior Month To Date";
            case "This Quarter": return "Prior Quarter";
            case "This Quarter To Date": return "Prior Quarter To Date";
            case "This Year": return "Prior Year";
            case "This Year To Date": return "Prior Year To Date";
            case "This Year To Last Month": return "Prior Year To Last Month";
            case "Last Week To Date": return "Prior Week To Date";
            case "Last Month": return "Prior Month";
            case "Last Month To Date": return "Prior Month To Date";
            case "Last Quarter": return "Prior Quarter";
            case "Last Quarter To Date": return "Prior Quarter To Date";
            case "Since 30 Days Ago": return "Prior 30 Days";
            case "Since 60 Days Ago": return "Prior 60 Days";
            default: return "Prior Month";
        }
    };

    function getRandomColor() {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    };

    function Getstartenddate(filter) {

        var date = new Date();
        var beginDate = $filter('date')(new Date(date.getFullYear(), date.getMonth(), 1), 'MM/dd/yyyy');
        var endDate = $filter('date')(new Date(), 'MM/dd/yyyy');


        function getQuarter(d) {
            d = d || new Date();
            var m = Math.floor(d.getMonth() / 3) + 1;
            return m > 4 ? m - 4 : m;
        };

        var date = new Date();
        var firstDay;
        var lastDay;
        if (filter == 'ThisMonth') {
            firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        }
        else if (filter == 'LastMonth') {
            var month = date.getMonth();
            var year;
            if (month < 12)
                year = date.getFullYear();
            else
                year = date.getFullYear() - 1;
            firstDay = new Date(year, month - 1, 1);
            lastDay = new Date(year, month, 0);
        }
        else if (filter == 'ThisQuarter') {
            var currQuarter = getQuarter(date);
            firstDay = new Date(date.getFullYear(), 3 * currQuarter - 3, 1);
            lastDay = new Date(date.getFullYear(), 3 * currQuarter, 0);
        }
        else if (filter == 'LastQuarter') {
            var year;
            var quarter = getQuarter(date) - 1;
            //The quarter is zero means, when current quarter is first , then prevous quarter is the last quarter of previous year
            if (quarter > 0) {
                year = date.getFullYear();
                firstDay = new Date(year, 3 * quarter - 3, 1);
                lastDay = new Date(year, 3 * quarter, 0);
            }
            else {
                year = date.getFullYear() - 1;
                firstDay = new Date(year, 9, 1);
                lastDay = new Date(year, 12, 0);
            }
        }
        else if (filter == 'YTD') {
            firstDay = new Date(date.getFullYear(), 0, 1);
            lastDay = new Date();
        }
        else if (filter == 'LastYear') {
            firstDay = new Date(date.getFullYear() - 1, 0, 1);
            lastDay = new Date(date.getFullYear(), 0, 0);
        }
        else {
            firstDay = beginDate;
            lastDay = endDate;
        }

        return { firstDay: firstDay, lastDay: lastDay };
    }

    function generateLegend(legData, drilldowncount) {
        
        if (legData) {
            var tempLegend = [];
            var legendNames = [];
            function getyear(date) {
                var d = new Date(date);
                return d.getFullYear();
            };
            tempLegend.push({
                title: getyear(legData[0].Period1) + " " + "Produce",
                color: legData[0].Color2
            })
            tempLegend.push({
                title: getyear(legData[0].Period1) + " " + "Grocery",
                color: legData[0].Color1
            })
            tempLegend.push({
                title: getyear(legData[0].Period3) + " " + "Produce",
                color: legData[0].Color4
            })
            tempLegend.push({
                title: getyear(legData[0].Period3) + " " + "Grocery",
                color: legData[0].Color3
            })
            if (drilldowncount > 0) {
                tempLegend = [];
                tempLegend.push({
                    title: "",
                    color: "white"
                })
            }
            return tempLegend;
        }
    };


    return {
        InitBtnSpinner: initBtnSpinner,
        getBalloonTextPrefix: getBalloonTextPrefix,
        getValueAxisTitle: getValueAxisTitle,
        formatCommaSeperate: formatCommaSeperate,
        calcPercentage: calcPercentage,
        //getTopBoxEmptyStylePositive: getTopBoxEmptyStylePositive,
        //getTopBoxEmptyStyleNegative: getTopBoxEmptyStyleNegative,
        //getTopBoxSuccessStyle: getTopBoxSuccessStyle,
        //getTopBoxFailStyle: getTopBoxFailStyle,
        getPriorRangeText: getPriorRangeText,
        getRandomColor: getRandomColor,
        Getstartenddate: Getstartenddate,
        generateLegend: generateLegend,
        calcpercentagespan: calcpercentagespan

    }
}]);

