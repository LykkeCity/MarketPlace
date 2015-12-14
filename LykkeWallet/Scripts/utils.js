var Utils = (function () {
    function Utils() {
    }
    Utils.round = function (value, accuracy) {
        var multilpier = Math.pow(10, accuracy);
        return Math.round(value * multilpier) / multilpier;
    };
    Utils.getTime = function () {
        var time = new Date();
        return this.dateTimeToIso(time);
    };
    Utils.numberToString = function (value, digits) {
        var result = value.toString();
        while (result.length < digits)
            result = '0' + result;
        return result;
    };
    Utils.daysInMonth = function (date) {
        return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    };
    Utils.roundFloat = function (value, digits) {
        return value.toFixed(digits);
    };
    Utils.dateToIso = function (date) {
        var mn = (date.getMonth() + 1);
        var m = mn < 10 ? "0" + mn.toString() : mn.toString();
        var dt = date.getDate();
        var d = dt < 10 ? "0" + dt.toString() : dt.toString();
        return (date.getFullYear()).toString() + '-' + m + '-' + d;
    };
    Utils.dateTimeToIso = function (dt) {
        return this.dateToIso(dt) + ' ' + this.timeToString(dt);
    };
    Utils.timeToString = function (time) {
        var h = time.getHours() < 10 ? "0" + time.getHours().toString() : time.getHours().toString();
        var m = time.getMinutes() < 10 ? "0" + time.getMinutes().toString() : time.getMinutes().toString();
        var s = time.getSeconds() < 10 ? "0" + time.getSeconds().toString() : time.getSeconds().toString();
        return h + ':' + m + ":" + s;
    };
    Utils.formatDateTime = function (dateTime) {
        var mn = (dateTime.getMonth() + 1);
        var m = mn < 10 ? "0" + mn.toString() : mn.toString();
        var dt = dateTime.getDate();
        var d = dt < 10 ? "0" + dt.toString() : dt.toString();
        return d + '.' + m + '.' + dateTime.getFullYear().toString() + " " + this.timeToString(dateTime);
    };
    Utils.formatDate = function (dateTime) {
        var mn = (dateTime.getMonth() + 1);
        var m = mn < 10 ? "0" + mn.toString() : mn.toString();
        var dt = dateTime.getDate();
        var d = dt < 10 ? "0" + dt.toString() : dt.toString();
        return d + '.' + m + '.' + dateTime.getFullYear().toString();
    };
    Utils.parseIsoDate = function (isoDate) {
        isoDate = isoDate.replace('T', ' ');
        var dt = isoDate.split(' ');
        var yyyy = 0, mm = 0, dd = 0, hh = 0, min = 0, sec = 0;
        if (dt.length > 0) {
            var ydm = dt[0].split('-');
            yyyy = parseInt(ydm[0]);
            mm = parseInt(ydm[1]);
            dd = parseInt(ydm[2]);
        }
        if (dt.length > 1) {
            var hms = dt[1].split(':');
            hh = parseInt(hms[0]);
            min = parseInt(hms[1]);
            sec = parseInt(hms[2]);
        }
        return new Date(yyyy, mm - 1, dd, hh, min, sec, 0);
    };
    Utils.differenceInSeconds = function (after, before) {
        return this.trunc(Math.floor(after - before) * 0.001);
    };
    Utils.timeBetween = function (after, before) {
        var seconds = this.differenceInSeconds(after, before);
        var hours = truncate(seconds / 3600);
        seconds -= hours * 3600;
        var minutes = truncate(seconds / 60);
        seconds -= minutes * 60;
        seconds = truncate(seconds);
        return this.numberToString(hours, 2) + ":" + this.numberToString(minutes, 2) + ":" + this.numberToString(seconds, 2);
    };
    Utils.formatString = function (src, data) {
        if (!data)
            return src;
        for (var i = 0; i < data.length; i++) {
            var s = data[i];
            src = src.replace('{' + i + '}', s);
        }
        return src;
    };
    Utils.timeToSeconds = function (time) {
        var d = time.split(":");
        return parseInt(d[0]) * 3600 + parseInt(d[1]) * 60 + parseInt(d[2]);
    };
    Utils.trunc = function (src) {
        return parseInt(src);
    };
    Utils.secondsToTime = function (seconds) {
        var h = this.trunc(seconds / 3600);
        seconds -= h * 3600;
        var m = this.trunc(seconds / 60);
        seconds -= m * 60;
        return this.numberToString(h, 2) + ":" + this.numberToString(m, 2) + ":" + this.numberToString(seconds, 2);
    };
    Utils.datesAreSame = function (d1, d2) {
        return d1.getFullYear() === d2.getFullYear() &&
            d1.getMonth() === d2.getMonth() &&
            d1.getDate() === d2.getDate() &&
            d1.getHours() === d2.getHours() &&
            d1.getMinutes() === d2.getMinutes() &&
            d1.getSeconds() === d2.getSeconds();
    };
    return Utils;
})();
function filterKeyNumbers(event) {
    // Allow: backspace, delete, tab, escape, and enter
    if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 || event.keyCode === 13 ||
        // Allow: Ctrl+A
        (event.keyCode === 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
        (event.keyCode >= 35 && event.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
        }
    }
}
function truncate(value) {
    return value - (value % 1);
}
//# sourceMappingURL=utils.js.map