/*!
 * DevExpress Gantt (dx-gantt)
 * Version: 0.0.23
 * Build date: Thu Oct 17 2019
 * 
 * Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
 * Read about DevExpress licensing here: https://www.devexpress.com/Support/EULAs
 */
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define([], factory);
	else if(typeof exports === 'object')
		exports["Gantt"] = factory();
	else
		root["DevExpress"] = root["DevExpress"] || {}, root["DevExpress"]["Gantt"] = factory();
})(window, function() {
return /******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 31);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var JsonUtils = (function () {
    function JsonUtils() {
    }
    JsonUtils.isExists = function (obj) {
        return (typeof (obj) != "undefined") && (obj != null);
    };
    JsonUtils.isValidJson = function (json) {
        return !(/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(json.replace(/"(\\.|[^"\\])*"/g, '')));
    };
    JsonUtils.evalJson = function (json) {
        return JsonUtils.isValidJson(json) ? eval("(" + json + ")") : null;
    };
    return JsonUtils;
}());
exports.JsonUtils = JsonUtils;


/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Time_1 = __webpack_require__(22);
var TimeRange_1 = __webpack_require__(23);
var Utils_1 = __webpack_require__(0);
var DateRange_1 = __webpack_require__(10);
var DayOfWeekMonthlyOccurrence_1 = __webpack_require__(24);
var DateTimeUtils = (function () {
    function DateTimeUtils() {
    }
    DateTimeUtils.compareDates = function (date1, date2) {
        if (!date1 || !date2)
            return -1;
        return date2.getTime() - date1.getTime();
    };
    DateTimeUtils.areDatesEqual = function (date1, date2) {
        return this.compareDates(date1, date2) == 0;
    };
    DateTimeUtils.getMaxDate = function (date1, date2) {
        if (!date1 && !date2)
            return null;
        if (!date1)
            return date2;
        if (!date2)
            return date1;
        var diff = this.compareDates(date1, date2);
        return diff > 0 ? date2 : date1;
    };
    DateTimeUtils.getMinDate = function (date1, date2) {
        if (!date1 && !date2)
            return null;
        if (!date1)
            return date2;
        if (!date2)
            return date1;
        var diff = this.compareDates(date1, date2);
        return diff > 0 ? date1 : date2;
    };
    DateTimeUtils.getDaysBetween = function (start, end) {
        var diff = Math.abs(end.getTime() - start.getTime());
        return Math.ceil(diff / this.msInDay);
    };
    DateTimeUtils.getWeeksBetween = function (start, end) {
        var daysBetween = this.getDaysBetween(start, end);
        var numWeeks = Math.floor(daysBetween / 7);
        if (start.getDay() > end.getDay())
            numWeeks++;
        return numWeeks;
    };
    DateTimeUtils.getMonthsDifference = function (start, end) {
        var dateDiff = this.compareDates(start, end);
        var from = dateDiff >= 0 ? start : end;
        var to = dateDiff >= 0 ? end : start;
        var yearsDiff = to.getFullYear() - from.getFullYear();
        var monthDiff = yearsDiff * 12 + (to.getMonth() - from.getMonth());
        return monthDiff;
    };
    DateTimeUtils.getYearsDifference = function (start, end) {
        return Math.abs(end.getFullYear() - start.getFullYear());
    };
    DateTimeUtils.getDayNumber = function (date) {
        return Math.ceil(date.getTime() / this.msInDay);
    };
    DateTimeUtils.getDateByDayNumber = function (num) {
        var date = new Date(num * this.msInDay);
        date.setHours(0);
        date.setMinutes(0);
        date.setSeconds(0);
        return date;
    };
    DateTimeUtils.addDays = function (date, days) {
        return new Date(date.getTime() + days * this.msInDay);
    };
    DateTimeUtils.checkDayOfMonth = function (day, date) {
        return day == date.getDate();
    };
    DateTimeUtils.checkDayOfWeek = function (day, date) {
        return day == date.getDay();
    };
    DateTimeUtils.checkMonth = function (month, date) {
        return month == date.getMonth();
    };
    DateTimeUtils.checkYear = function (year, date) {
        return year == date.getFullYear();
    };
    DateTimeUtils.checkDayOfWeekOccurrenceInMonth = function (date, dayOfWeek, occurrence) {
        var dayOfWeekInMonthDates = this.getSpecificDayOfWeekInMonthDates(dayOfWeek, date.getFullYear(), date.getMonth());
        if (occurrence == DayOfWeekMonthlyOccurrence_1.DayOfWeekMonthlyOccurrence.Last)
            return this.areDatesEqual(date, dayOfWeekInMonthDates[dayOfWeekInMonthDates.length - 1]);
        return this.areDatesEqual(date, dayOfWeekInMonthDates[occurrence]);
    };
    DateTimeUtils.getFirstDayOfWeekInMonth = function (year, month) {
        var date = new Date(year, month, 1);
        return date.getDay();
    };
    DateTimeUtils.getSpecificDayOfWeekInMonthDates = function (dayOfWeek, year, month) {
        var firstDayOfWeekInMonth = this.getFirstDayOfWeekInMonth(year, month);
        var diffDays = dayOfWeek >= firstDayOfWeekInMonth ? dayOfWeek - firstDayOfWeekInMonth : dayOfWeek + 7 - firstDayOfWeekInMonth;
        var res = new Array();
        var specificDayOfWeekDate = new Date(year, month, diffDays + 1);
        while (specificDayOfWeekDate.getMonth() == month) {
            res.push(specificDayOfWeekDate);
            specificDayOfWeekDate = this.addDays(specificDayOfWeekDate, 7);
        }
        return res;
    };
    DateTimeUtils.getSpecificDayOfWeekInMonthDate = function (dayOfWeek, year, month, occurrence) {
        var dates = this.getSpecificDayOfWeekInMonthDates(dayOfWeek, year, month);
        if (occurrence == DayOfWeekMonthlyOccurrence_1.DayOfWeekMonthlyOccurrence.Last)
            return dates[dates.length - 1];
        return dates[occurrence];
    };
    DateTimeUtils.checkValidDayInMonth = function (year, month, day) {
        if (day < 1 || day > 31 || (new Date(year, month, day)).getMonth() != month)
            return false;
        return true;
    };
    DateTimeUtils.getNextMonth = function (month, inc) {
        if (inc === void 0) { inc = 1; }
        return (month + inc) % 12;
    };
    DateTimeUtils.convertToDate = function (src) {
        if (src instanceof Date)
            return new Date(src);
        var ms = Date.parse(src);
        if (!isNaN(ms))
            return new Date(ms);
        return null;
    };
    DateTimeUtils.convertTimeRangeToDateRange = function (timeRange, dayNumber) {
        var date = this.getDateByDayNumber(dayNumber);
        var year = date.getFullYear();
        var month = date.getMonth();
        var day = date.getDate();
        var startT = timeRange.start;
        var start = new Date(year, month, day, startT.hour, startT.min, startT.sec, startT.msec);
        var endT = timeRange.end;
        var end = new Date(year, month, day, endT.hour, endT.min, endT.sec, endT.msec);
        return new DateRange_1.DateRange(start, end);
    };
    DateTimeUtils.convertToTimeRanges = function (src) {
        var _this = this;
        if (src instanceof Array)
            return src.map(function (x) { return _this.convertToTimeRange(x); });
        return this.parseTimeRanges(src);
    };
    DateTimeUtils.convertToTimeRange = function (src) {
        if (!src)
            return null;
        if (src instanceof TimeRange_1.TimeRange)
            return src;
        if (Utils_1.JsonUtils.isExists(src.start) && Utils_1.JsonUtils.isExists(src.end))
            return new TimeRange_1.TimeRange(this.convertToTime(src.start), this.convertToTime(src.end));
        return this.parseTimeRange(src);
    };
    DateTimeUtils.convertToTime = function (src) {
        if (!src)
            return null;
        if (src instanceof Time_1.Time)
            return src;
        if (src instanceof Date)
            return this.getTimeGromJsDate(src);
        return this.parseTime(src);
    };
    DateTimeUtils.parseTimeRanges = function (src) {
        var _this = this;
        if (!src)
            return null;
        var parts = src.split(/;|,/);
        return parts.map(function (p) { return _this.parseTimeRange(p); }).filter(function (r) { return !!r; });
    };
    DateTimeUtils.parseTimeRange = function (src) {
        if (!src)
            return null;
        var parts = src.split("-");
        var start = parts[0];
        var end = parts[1];
        if (Utils_1.JsonUtils.isExists(start) && Utils_1.JsonUtils.isExists(end))
            return new TimeRange_1.TimeRange(this.parseTime(start), this.parseTime(end));
        return null;
    };
    DateTimeUtils.parseTime = function (src) {
        if (!src)
            return null;
        var parts = src.split(":");
        var h = parseInt(parts[0]) || 0;
        var m = parseInt(parts[1]) || 0;
        var s = parseInt(parts[2]) || 0;
        var ms = parseInt(parts[3]) || 0;
        return new Time_1.Time(h, m, s, ms);
    };
    DateTimeUtils.getTimeGromJsDate = function (date) {
        if (!date)
            return null;
        var h = date.getHours();
        var m = date.getMinutes();
        var s = date.getSeconds();
        var ms = date.getMilliseconds();
        return new Time_1.Time(h, m, s, ms);
    };
    DateTimeUtils.caclTimeDifference = function (time1, time2) {
        return time2.getTimeInMilleconds() - time1.getTimeInMilleconds();
    };
    DateTimeUtils.areTimesEqual = function (time1, time2) {
        return this.caclTimeDifference(time1, time2) == 0;
    };
    DateTimeUtils.getMaxTime = function (time1, time2) {
        if (!time1 && !time2)
            return null;
        if (!time1)
            return time2;
        if (!time2)
            return time1;
        var diff = this.caclTimeDifference(time1, time2);
        return diff > 0 ? time2 : time1;
    };
    DateTimeUtils.getMinTime = function (time1, time2) {
        if (!time1 && !time2)
            return null;
        if (!time1)
            return time2;
        if (!time2)
            return time1;
        var diff = this.caclTimeDifference(time1, time2);
        return diff > 0 ? time1 : time2;
    };
    DateTimeUtils.getLastTimeOfDay = function () {
        return new Time_1.Time(23, 59, 59, 999);
    };
    DateTimeUtils.msInDay = 24 * 3600 * 1000;
    return DateTimeUtils;
}());
exports.DateTimeUtils = DateTimeUtils;


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Browser_1 = __webpack_require__(14);
var DomUtils = (function () {
    function DomUtils() {
    }
    DomUtils.clientEventRequiresDocScrollCorrection = function () {
        var isSafariVerLess3 = Browser_1.Browser.Safari && Browser_1.Browser.Version < 3, isMacOSMobileVerLess51 = Browser_1.Browser.MacOSMobilePlatform && Browser_1.Browser.Version < 5.1;
        return Browser_1.Browser.AndroidDefaultBrowser || Browser_1.Browser.AndroidChromeBrowser || !(isSafariVerLess3 || isMacOSMobileVerLess51);
    };
    DomUtils.getEventX = function (evt) {
        return evt.clientX + (this.clientEventRequiresDocScrollCorrection() ? this.getDocumentScrollLeft() : 0);
    };
    DomUtils.getEventY = function (evt) {
        return evt.clientY + (this.clientEventRequiresDocScrollCorrection() ? this.getDocumentScrollTop() : 0);
    };
    DomUtils.getEventSource = function (evt) {
        return evt.srcElement ? evt.srcElement : evt.target;
    };
    DomUtils.GetKeyCode = function (evt) {
        return Browser_1.Browser.NetscapeFamily || Browser_1.Browser.Opera ? evt.which : evt.keyCode;
    };
    DomUtils.GetIsParent = function (parentElement, element) {
        if (!parentElement || !element)
            return false;
        while (element) {
            if (element === parentElement)
                return true;
            if (element.tagName === "BODY")
                return false;
            element = element.parentNode;
        }
        return false;
    };
    DomUtils.getCurrentStyle = function (element) {
        if (document.defaultView && document.defaultView.getComputedStyle)
            return document.defaultView.getComputedStyle(element, null);
        return window.getComputedStyle(element, null);
    };
    DomUtils.getTopPaddings = function (element) {
        var currentStyle = this.getCurrentStyle(element);
        return this.pxToInt(currentStyle.paddingTop);
    };
    DomUtils.getVerticalBordersWidth = function (element) {
        var style = window.getComputedStyle ? window.getComputedStyle(element) : this.getCurrentStyle(element);
        var res = 0;
        if (style.borderTopStyle != "none")
            res += this.pxToFloat(style.borderTopWidth);
        if (style.borderBottomStyle != "none")
            res += this.pxToFloat(style.borderBottomWidth);
        return res;
    };
    DomUtils.getHorizontalBordersWidth = function (element) {
        var style = window.getComputedStyle ? window.getComputedStyle(element) : this.getCurrentStyle(element);
        var res = 0;
        if (style.borderLeftStyle != "none")
            res += this.pxToFloat(style.borderLeftWidth);
        if (style.borderRightStyle != "none")
            res += this.pxToFloat(style.borderRightWidth);
        return res;
    };
    DomUtils.pxToInt = function (px) {
        return this.pxToNumber(px, parseInt);
    };
    DomUtils.pxToFloat = function (px) {
        return this.pxToNumber(px, parseFloat);
    };
    ;
    DomUtils.pxToNumber = function (px, parseFunction) {
        var result = 0;
        if (px != null && px != "") {
            try {
                var indexOfPx = px.indexOf("px");
                if (indexOfPx > -1)
                    result = parseFunction(px.substr(0, indexOfPx));
            }
            catch (e) { }
        }
        return result;
    };
    DomUtils.getDocumentScrollTop = function () {
        var isScrollBodyIE = Browser_1.Browser.IE && this.getCurrentStyle(document.body).overflow == "hidden" && document.body.scrollTop > 0;
        if (Browser_1.Browser.WebKitFamily || Browser_1.Browser.Edge || isScrollBodyIE) {
            if (Browser_1.Browser.MacOSMobilePlatform)
                return window.pageYOffset;
            if (Browser_1.Browser.WebKitFamily)
                return document.documentElement.scrollTop || document.body.scrollTop;
            return document.body.scrollTop;
        }
        else
            return document.documentElement.scrollTop;
    };
    DomUtils.getDocumentScrollLeft = function () {
        var isScrollBodyIE = Browser_1.Browser.IE && this.getCurrentStyle(document.body).overflow == "hidden" && document.body.scrollLeft > 0;
        if (Browser_1.Browser.Edge || isScrollBodyIE)
            return document.body ? document.body.scrollLeft : document.documentElement.scrollLeft;
        if (Browser_1.Browser.WebKitFamily)
            return document.documentElement.scrollLeft || document.body.scrollLeft;
        return document.documentElement.scrollLeft;
    };
    DomUtils.getAbsolutePositionY = function (element) {
        if (Browser_1.Browser.IE)
            return this.getAbsolutePositionY_IE(element);
        else if (Browser_1.Browser.Firefox && Browser_1.Browser.Version >= 3)
            return this.getAbsolutePositionY_FF3(element);
        else if (Browser_1.Browser.Opera)
            return this.getAbsolutePositionY_Opera(element);
        else if (Browser_1.Browser.NetscapeFamily && (!Browser_1.Browser.Firefox || Browser_1.Browser.Version < 3))
            return this.getAbsolutePositionY_NS(element);
        else if (Browser_1.Browser.WebKitFamily || Browser_1.Browser.Edge)
            return this.getAbsolutePositionY_FF3(element);
        else
            return this.getAbsolutePositionY_Other(element);
    };
    DomUtils.getAbsolutePositionY_Opera = function (curEl) {
        var isFirstCycle = true;
        if (curEl && curEl.tagName == "TR" && curEl.cells.length > 0)
            curEl = curEl.cells[0];
        var pos = this.getAbsoluteScrollOffset_OperaFF(curEl, false);
        while (curEl != null) {
            pos += curEl.offsetTop;
            if (!isFirstCycle)
                pos -= curEl.scrollTop;
            curEl = curEl.offsetParent;
            isFirstCycle = false;
        }
        pos += document.body.scrollTop;
        return pos;
    };
    DomUtils.getAbsolutePositionY_IE = function (element) {
        if (element == null || Browser_1.Browser.IE && element.parentNode == null)
            return 0;
        return element.getBoundingClientRect().top + this.getDocumentScrollTop();
    };
    DomUtils.getAbsolutePositionY_FF3 = function (element) {
        if (element == null)
            return 0;
        var y = element.getBoundingClientRect().top + this.getDocumentScrollTop();
        return Math.round(y);
    };
    DomUtils.getAbsolutePositionY_NS = function (curEl) {
        var pos = this.getAbsoluteScrollOffset_OperaFF(curEl, false);
        var isFirstCycle = true;
        while (curEl != null) {
            pos += curEl.offsetTop;
            if (!isFirstCycle && curEl.offsetParent != null)
                pos -= curEl.scrollTop;
            if (!isFirstCycle && Browser_1.Browser.Firefox) {
                var style = this.getCurrentStyle(curEl);
                if (curEl.tagName == "DIV" && style.overflow != "visible")
                    pos += this.pxToInt(style.borderTopWidth);
            }
            isFirstCycle = false;
            curEl = curEl.offsetParent;
        }
        return pos;
    };
    DomUtils.getAbsolutePositionY_Other = function (curEl) {
        var pos = 0;
        var isFirstCycle = true;
        while (curEl != null) {
            pos += curEl.offsetTop;
            if (!isFirstCycle && curEl.offsetParent != null)
                pos -= curEl.scrollTop;
            isFirstCycle = false;
            curEl = curEl.offsetParent;
        }
        return pos;
    };
    DomUtils.getAbsoluteScrollOffset_OperaFF = function (curEl, isX) {
        var pos = 0;
        var isFirstCycle = true;
        while (curEl != null) {
            if (curEl.tagName == "BODY")
                break;
            var style = this.getCurrentStyle(curEl);
            if (style.position == "absolute")
                break;
            if (!isFirstCycle && curEl.tagName == "DIV" && (style.position == "" || style.position == "static"))
                pos -= isX ? curEl.scrollLeft : curEl.scrollTop;
            curEl = curEl.parentNode;
            isFirstCycle = false;
        }
        return pos;
    };
    DomUtils.getAbsolutePositionX = function (element) {
        if (Browser_1.Browser.IE)
            return this.getAbsolutePositionX_IE(element);
        else if (Browser_1.Browser.Firefox && Browser_1.Browser.Version >= 3)
            return this.getAbsolutePositionX_FF3(element);
        else if (Browser_1.Browser.Opera)
            return this.getAbsolutePositionX_Opera(element);
        else if (Browser_1.Browser.NetscapeFamily && (!Browser_1.Browser.Firefox || Browser_1.Browser.Version < 3))
            return this.getAbsolutePositionX_NS(element);
        else if (Browser_1.Browser.WebKitFamily || Browser_1.Browser.Edge)
            return this.getAbsolutePositionX_FF3(element);
        else
            return this.getAbsolutePositionX_Other(element);
    };
    DomUtils.getAbsolutePositionX_Opera = function (curEl) {
        var isFirstCycle = true;
        var pos = this.getAbsoluteScrollOffset_OperaFF(curEl, true);
        while (curEl != null) {
            pos += curEl.offsetLeft;
            if (!isFirstCycle)
                pos -= curEl.scrollLeft;
            curEl = curEl.offsetParent;
            isFirstCycle = false;
        }
        pos += document.body.scrollLeft;
        return pos;
    };
    DomUtils.getAbsolutePositionX_IE = function (element) {
        if (element == null || Browser_1.Browser.IE && element.parentNode == null)
            return 0;
        return element.getBoundingClientRect().left + this.getDocumentScrollLeft();
    };
    DomUtils.getAbsolutePositionX_FF3 = function (element) {
        if (element == null)
            return 0;
        var x = element.getBoundingClientRect().left + this.getDocumentScrollLeft();
        return Math.round(x);
    };
    DomUtils.getAbsolutePositionX_NS = function (curEl) {
        var pos = this.getAbsoluteScrollOffset_OperaFF(curEl, true);
        var isFirstCycle = true;
        while (curEl != null) {
            pos += curEl.offsetLeft;
            if (!isFirstCycle && curEl.offsetParent != null)
                pos -= curEl.scrollLeft;
            if (!isFirstCycle && Browser_1.Browser.Firefox) {
                var style = this.getCurrentStyle(curEl);
                if (curEl.tagName == "DIV" && style.overflow != "visible")
                    pos += this.pxToInt(style.borderLeftWidth);
            }
            isFirstCycle = false;
            curEl = curEl.offsetParent;
        }
        return pos;
    };
    DomUtils.getAbsolutePositionX_Other = function (curEl) {
        var pos = 0;
        var isFirstCycle = true;
        while (curEl != null) {
            pos += curEl.offsetLeft;
            if (!isFirstCycle && curEl.offsetParent != null)
                pos -= curEl.scrollLeft;
            isFirstCycle = false;
            curEl = curEl.offsetParent;
        }
        return pos;
    };
    DomUtils.GetEvent = function (evt) {
        return (typeof (event) != "undefined" && event != null && Browser_1.Browser.IE) ? event : evt;
    };
    DomUtils.isExists = function (obj) {
        return (typeof (obj) != "undefined") && (obj != null);
    };
    DomUtils.isTouchEvent = function (evt) {
        if (!evt)
            return false;
        return Browser_1.Browser.WebKitTouchUI && DomUtils.isExists(evt.changedTouches);
    };
    DomUtils.IsLeftButtonPressed = function (evt) {
        if (DomUtils.isTouchEvent(evt))
            return true;
        evt = DomUtils.GetEvent(evt);
        if (!evt)
            return false;
        if (Browser_1.Browser.IE && Browser_1.Browser.Version < 11) {
            if (Browser_1.Browser.MSTouchUI)
                return true;
            return evt.button % 2 == 1;
        }
        else if (Browser_1.Browser.WebKitFamily) {
            if (evt.type === "pointermove")
                return evt.buttons === 1;
            return evt.which == 1;
        }
        else if (Browser_1.Browser.NetscapeFamily || Browser_1.Browser.Edge || (Browser_1.Browser.IE && Browser_1.Browser.Version >= 11)) {
            if (evt.type === DomUtils.touchMouseMoveEventName)
                return evt.buttons === 1;
            return evt.which == 1;
        }
        else if (Browser_1.Browser.Opera)
            return evt.button == 0;
        return true;
    };
    DomUtils.touchMouseMoveEventName = Browser_1.Browser.WebKitTouchUI ? "touchmove" : (Browser_1.Browser.Edge && Browser_1.Browser.MSTouchUI && window.PointerEvent ? "pointermove" : "mousemove");
    DomUtils.getMouseWheelEventName = function () {
        if (Browser_1.Browser.Safari)
            return "mousewheel";
        if (Browser_1.Browser.NetscapeFamily && Browser_1.Browser.MajorVersion < 17)
            return "DOMMouseScroll";
        return "wheel";
    };
    DomUtils.getWheelDelta = function (evt) {
        var ret;
        if (Browser_1.Browser.NetscapeFamily && Browser_1.Browser.MajorVersion < 17)
            ret = -evt.detail;
        else if (Browser_1.Browser.Safari)
            ret = evt.wheelDelta;
        else
            ret = -evt.deltaY;
        if (Browser_1.Browser.Opera && Browser_1.Browser.Version < 9)
            ret = -ret;
        return ret;
    };
    DomUtils.IsRightButtonPressed = function (evt) {
        evt = DomUtils.GetEvent(evt);
        if (!DomUtils.isExists(evt))
            return false;
        if (Browser_1.Browser.IE || Browser_1.Browser.Edge) {
            if (evt.type === "pointermove")
                return evt.buttons === 2;
            return evt.button == 2;
        }
        else if (Browser_1.Browser.NetscapeFamily || Browser_1.Browser.WebKitFamily)
            return evt.which == 3;
        else if (Browser_1.Browser.Opera)
            return evt.button == 1;
        return true;
    };
    return DomUtils;
}());
exports.DomUtils = DomUtils;


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Point = (function () {
    function Point(x, y) {
        this.x = null;
        this.y = null;
        if (x !== undefined)
            this.x = x;
        if (y !== undefined)
            this.y = y;
    }
    return Point;
}());
exports.Point = Point;


/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var DataObject = (function () {
    function DataObject() {
        this.internalId = this.generateGuid();
    }
    DataObject.prototype.assignFromObject = function (sourceObj) {
        if (!Utils_1.JsonUtils.isExists(sourceObj))
            return;
        if (Utils_1.JsonUtils.isExists(sourceObj.id)) {
            this.id = sourceObj.id;
            this.internalId = String(sourceObj.id);
        }
    };
    DataObject.prototype.generateGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };
    return DataObject;
}());
exports.DataObject = DataObject;


/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase = (function () {
    function CommandBase(control) {
        this.control = control;
    }
    Object.defineProperty(CommandBase.prototype, "modelManipulator", {
        get: function () { return this.control.modelManipulator; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandBase.prototype, "history", {
        get: function () { return this.control.history; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandBase.prototype, "state", {
        get: function () {
            if (!this._state)
                this._state = this.getState();
            return this._state;
        },
        enumerable: true,
        configurable: true
    });
    CommandBase.prototype.execute = function () {
        var parameters = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            parameters[_i] = arguments[_i];
        }
        if (!this.state.enabled)
            return false;
        return this.executeInternal.apply(this, parameters);
    };
    CommandBase.prototype.isEnabled = function () {
        return this.control.settings.editing.enabled;
    };
    CommandBase.prototype.executeInternal = function () {
        var parameters = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            parameters[_i] = arguments[_i];
        }
        throw new Error("Not implemented");
    };
    ;
    return CommandBase;
}());
exports.CommandBase = CommandBase;
var SimpleCommandState = (function () {
    function SimpleCommandState(enabled, value) {
        this.visible = true;
        this.enabled = enabled;
        this.value = value;
    }
    return SimpleCommandState;
}());
exports.SimpleCommandState = SimpleCommandState;


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var CollectionBase = (function () {
    function CollectionBase() {
        this._items = new Array();
        this._isGanttCollection = true;
    }
    CollectionBase.prototype.add = function (element) {
        if (!Utils_1.JsonUtils.isExists(element))
            return;
        if (!!this.getItemById(element.internalId))
            throw "The collection item with id ='" + element.internalId + "' already exists.";
        this._items.push(element);
    };
    CollectionBase.prototype.addRange = function (range) {
        for (var i = 0; i < range.length; i++)
            this.add(range[i]);
    };
    CollectionBase.prototype.remove = function (element) {
        var index = this._items.indexOf(element);
        if (index > -1 && index < this._items.length)
            this._items.splice(index, 1);
    };
    CollectionBase.prototype.clear = function () {
        this._items.splice(0, this._items.length);
    };
    Object.defineProperty(CollectionBase.prototype, "items", {
        get: function () {
            return this._items.slice();
        },
        set: function (value) {
            if (value)
                this._items = value.slice();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CollectionBase.prototype, "length", {
        get: function () {
            return this._items.length;
        },
        enumerable: true,
        configurable: true
    });
    CollectionBase.prototype.getItem = function (index) {
        if (index > -1 && index < this._items.length)
            return this._items[index];
        return null;
    };
    CollectionBase.prototype.getItemById = function (id) {
        return this._items.filter(function (val) { return val.internalId === id; })[0];
    };
    CollectionBase.prototype.getItemByPublicId = function (id) {
        return this._items.filter(function (val) { return val.id === id || val.id.toString() === id; })[0];
    };
    CollectionBase.prototype.assign = function (sourceCollection) {
        if (!Utils_1.JsonUtils.isExists(sourceCollection))
            return;
        this.items = sourceCollection.items;
    };
    CollectionBase.prototype.importFromObject = function (source) {
        if (!Utils_1.JsonUtils.isExists(source))
            return;
        this.clear();
        if (source._isGanttCollection)
            this.assign(source);
        else if (source instanceof Array) {
            this.importFromArray(source);
        }
        else {
            this.createItemFromObjectAndAdd(source);
        }
    };
    CollectionBase.prototype.createItemFromObjectAndAdd = function (source) {
        if (Utils_1.JsonUtils.isExists(source)) {
            var item = this.createItem();
            item.assignFromObject(source);
            this.add(item);
        }
    };
    CollectionBase.prototype.importFromArray = function (values) {
        for (var i = 0; i < values.length; i++)
            this.createItemFromObjectAndAdd(values[i]);
    };
    CollectionBase.prototype.importFromJSON = function (json) {
        this.importFromObject(Utils_1.JsonUtils.evalJson(json));
    };
    return CollectionBase;
}());
exports.CollectionBase = CollectionBase;


/***/ }),
/* 7 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ViewType;
(function (ViewType) {
    ViewType[ViewType["TenMinutes"] = 0] = "TenMinutes";
    ViewType[ViewType["Hours"] = 1] = "Hours";
    ViewType[ViewType["SixHours"] = 2] = "SixHours";
    ViewType[ViewType["Days"] = 3] = "Days";
    ViewType[ViewType["Weeks"] = 4] = "Weeks";
    ViewType[ViewType["Months"] = 5] = "Months";
    ViewType[ViewType["Quarter"] = 6] = "Quarter";
    ViewType[ViewType["Years"] = 7] = "Years";
})(ViewType = exports.ViewType || (exports.ViewType = {}));
var Position;
(function (Position) {
    Position[Position["Left"] = 0] = "Left";
    Position[Position["Top"] = 1] = "Top";
    Position[Position["Right"] = 2] = "Right";
    Position[Position["Bottom"] = 3] = "Bottom";
})(Position = exports.Position || (exports.Position = {}));
var TaskTitlePosition;
(function (TaskTitlePosition) {
    TaskTitlePosition[TaskTitlePosition["Inside"] = 0] = "Inside";
    TaskTitlePosition[TaskTitlePosition["Outside"] = 1] = "Outside";
    TaskTitlePosition[TaskTitlePosition["None"] = 2] = "None";
})(TaskTitlePosition = exports.TaskTitlePosition || (exports.TaskTitlePosition = {}));


/***/ }),
/* 8 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DomUtils_1 = __webpack_require__(2);
var Point_1 = __webpack_require__(3);
var Browser_1 = __webpack_require__(14);
var MouseEventSource;
(function (MouseEventSource) {
    MouseEventSource[MouseEventSource["TaskArea"] = 0] = "TaskArea";
    MouseEventSource[MouseEventSource["TaskEdit_Frame"] = 1] = "TaskEdit_Frame";
    MouseEventSource[MouseEventSource["TaskEdit_Progress"] = 2] = "TaskEdit_Progress";
    MouseEventSource[MouseEventSource["TaskEdit_Start"] = 3] = "TaskEdit_Start";
    MouseEventSource[MouseEventSource["TaskEdit_End"] = 4] = "TaskEdit_End";
    MouseEventSource[MouseEventSource["TaskEdit_DependencyStart"] = 5] = "TaskEdit_DependencyStart";
    MouseEventSource[MouseEventSource["TaskEdit_DependencyFinish"] = 6] = "TaskEdit_DependencyFinish";
    MouseEventSource[MouseEventSource["Successor_Wrapper"] = 7] = "Successor_Wrapper";
    MouseEventSource[MouseEventSource["Successor_DependencyStart"] = 8] = "Successor_DependencyStart";
    MouseEventSource[MouseEventSource["Successor_DependencyFinish"] = 9] = "Successor_DependencyFinish";
})(MouseEventSource = exports.MouseEventSource || (exports.MouseEventSource = {}));
var TaskAreaManager = (function () {
    function TaskAreaManager(ganttView) {
        this.ganttView = ganttView;
        this.eventManager = ganttView.eventManager;
        this.mousePosition = new Point_1.Point(-1, -1);
        this.initMouseEvents();
    }
    TaskAreaManager.prototype.initMouseEvents = function () {
        var _this = this;
        this.ganttView.taskArea.addEventListener("click", function (evt) { _this.onTaskAreaClick(evt); });
        this.ganttView.taskArea.addEventListener("scroll", this.ganttView.updateView.bind(this.ganttView));
        this.ganttView.taskArea.addEventListener("mousedown", function (evt) { _this.onMouseDown(evt); });
        this.ganttView.taskArea.addEventListener("contextmenu", function (evt) { _this.onContextMenu(evt); });
        this.ganttView.taskArea.addEventListener(DomUtils_1.DomUtils.getMouseWheelEventName(), function (evt) { _this.onMouseWheel(evt); });
        document.addEventListener("mousemove", function (evt) { _this.onDocumentMouseMove(evt); });
        document.addEventListener("mouseup", function (evt) { _this.onDocumentMouseUp(evt); });
        document.addEventListener("keydown", function (evt) { _this.onDocumentKeyDown(evt); });
    };
    TaskAreaManager.prototype.onMouseDown = function (evt) {
        this.eventManager.onMouseDown(evt);
        this.preventSelect = false;
        this.mousePosition = new Point_1.Point(evt.clientX, evt.clientY);
    };
    TaskAreaManager.prototype.onDocumentMouseUp = function (evt) {
        var _this = this;
        this.ganttView.isFocus = DomUtils_1.DomUtils.GetIsParent(this.ganttView.taskArea, DomUtils_1.DomUtils.getEventSource(evt)) ? true : false;
        if (this.ganttView.isFocus && !this.preventSelect && this.ganttView.settings.allowSelectTask && !this.isConnectorLine(evt))
            setTimeout(function () { _this.changeTaskSelection(_this.getClickedTaskIndex(evt)); }, 0);
        this.eventManager.onMouseUp(evt);
    };
    TaskAreaManager.prototype.onDocumentMouseMove = function (evt) {
        if (this.mousePosition.x != evt.clientX || this.mousePosition.y != evt.clientY) {
            this.eventManager.onMouseMove(evt);
            this.preventSelect = true;
        }
    };
    TaskAreaManager.prototype.onMouseWheel = function (evt) {
        this.eventManager.onMouseWheel(evt);
    };
    TaskAreaManager.prototype.onDocumentKeyDown = function (evt) {
        this.eventManager.onKeyDown(evt);
    };
    TaskAreaManager.prototype.onContextMenu = function (evt) {
        if (evt.stopPropagation)
            evt.stopPropagation();
        if (evt.preventDefault)
            evt.preventDefault();
        if (Browser_1.Browser.WebKitFamily)
            evt.returnValue = false;
        this.ganttView.ganttOwner.showPopupMenu(new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt)));
    };
    TaskAreaManager.prototype.getClickedTaskIndex = function (evt) {
        var y = DomUtils_1.DomUtils.getEventY(evt);
        var taskAreaY = DomUtils_1.DomUtils.getAbsolutePositionY(this.ganttView.taskArea);
        var relativeY = y - taskAreaY;
        return Math.floor(relativeY / this.ganttView.tickSize.height);
    };
    TaskAreaManager.prototype.onTaskElementHover = function (evt) {
        var hoveredTaskIndex = this.getClickedTaskIndex(evt);
        this.ganttView.taskEditController.show(hoveredTaskIndex);
    };
    TaskAreaManager.prototype.changeTaskSelection = function (index) {
        var clickedTask = this.ganttView.viewModel.items[index];
        if (clickedTask) {
            this.ganttView.unselectCurrentSelectedTask();
            this.ganttView.ganttOwner.changeGanttTaskSelection(clickedTask.task.id, true);
            this.ganttView.selectTask(clickedTask.task.internalId);
        }
    };
    TaskAreaManager.prototype.onTaskAreaClick = function (evt) {
        var now = new Date(Date.now());
        var clickedTaskIndex = this.getClickedTaskIndex(evt);
        if (this.time && now.getTime() - this.time.getTime() < TaskAreaManager.DBLCLICK_INTERVAL) {
            var clickedTask = this.ganttView.viewModel.items[clickedTaskIndex];
            this.ganttView.commandManager.showTaskEditDialog.execute(clickedTask.task);
        }
        this.time = now;
    };
    TaskAreaManager.prototype.isConnectorLine = function (evt) {
        var source = DomUtils_1.DomUtils.getEventSource(evt);
        return source.classList.contains(this.ganttView.gridLayoutCalculator.getConnectorClassName(true)) ||
            source.classList.contains(this.ganttView.gridLayoutCalculator.getConnectorClassName(false));
    };
    TaskAreaManager.DBLCLICK_INTERVAL = 300;
    return TaskAreaManager;
}());
exports.TaskAreaManager = TaskAreaManager;


/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem = (function () {
    function HistoryItem(modelManipulator) {
        this.modelManipulator = modelManipulator;
    }
    return HistoryItem;
}());
exports.HistoryItem = HistoryItem;
var CompositionHistoryItem = (function (_super) {
    __extends(CompositionHistoryItem, _super);
    function CompositionHistoryItem() {
        var _this = _super.call(this, null) || this;
        _this.historyItems = [];
        return _this;
    }
    CompositionHistoryItem.prototype.redo = function () {
        var item;
        for (var i = 0; item = this.historyItems[i]; i++)
            item.redo();
    };
    CompositionHistoryItem.prototype.undo = function () {
        var item;
        for (var i = this.historyItems.length - 1; item = this.historyItems[i]; i--)
            item.undo();
    };
    CompositionHistoryItem.prototype.add = function (historyItem) {
        if (historyItem == null)
            throw new Error("Can't add null HistoryItem");
        this.historyItems.push(historyItem);
    };
    return CompositionHistoryItem;
}(HistoryItem));
exports.CompositionHistoryItem = CompositionHistoryItem;


/***/ }),
/* 10 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DateRange = (function () {
    function DateRange(start, end) {
        this.start = start;
        this.end = end;
    }
    return DateRange;
}());
exports.DateRange = DateRange;


/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DataObject_1 = __webpack_require__(4);
var DayOfWeek_1 = __webpack_require__(46);
var DayOfWeekMonthlyOccurrence_1 = __webpack_require__(24);
var Month_1 = __webpack_require__(47);
var Utils_1 = __webpack_require__(0);
var DateTimeUtils_1 = __webpack_require__(1);
var RecurrenceFactory_1 = __webpack_require__(25);
var RecurrenceBase = (function (_super) {
    __extends(RecurrenceBase, _super);
    function RecurrenceBase(start, end, interval, occurrenceCount) {
        if (start === void 0) { start = null; }
        if (end === void 0) { end = null; }
        if (interval === void 0) { interval = 1; }
        if (occurrenceCount === void 0) { occurrenceCount = 0; }
        var _this = _super.call(this) || this;
        _this._start = null;
        _this._end = null;
        _this._interval = 1;
        _this._occurrenceCount = 0;
        _this._dayOfWeek = 0;
        _this._day = 1;
        _this._dayOfWeekOccurrence = 0;
        _this._month = 0;
        _this._calculateByDayOfWeek = false;
        _this.start = start;
        _this.end = end;
        _this.interval = interval;
        _this.occurrenceCount = occurrenceCount;
        return _this;
    }
    RecurrenceBase.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.start = DateTimeUtils_1.DateTimeUtils.convertToDate(sourceObj.start);
            this.end = DateTimeUtils_1.DateTimeUtils.convertToDate(sourceObj.end);
            if (Utils_1.JsonUtils.isExists(sourceObj.interval))
                this.interval = sourceObj.interval;
            if (Utils_1.JsonUtils.isExists(sourceObj.occurrenceCount))
                this.occurrenceCount = sourceObj.occurrenceCount;
            if (Utils_1.JsonUtils.isExists(sourceObj.dayOfWeek))
                this.dayOfWeekInternal = RecurrenceFactory_1.RecurrenceFactory.getEnumValue(DayOfWeek_1.DayOfWeek, sourceObj.dayOfWeek);
            if (Utils_1.JsonUtils.isExists(sourceObj.day))
                this.dayInternal = sourceObj.day;
            if (Utils_1.JsonUtils.isExists(sourceObj.dayOfWeekOccurrence))
                this.dayOfWeekOccurrenceInternal = RecurrenceFactory_1.RecurrenceFactory.getEnumValue(DayOfWeekMonthlyOccurrence_1.DayOfWeekMonthlyOccurrence, sourceObj.dayOfWeekOccurrence);
            if (Utils_1.JsonUtils.isExists(sourceObj.month))
                this.monthInternal = RecurrenceFactory_1.RecurrenceFactory.getEnumValue(Month_1.Month, sourceObj.month);
            if (Utils_1.JsonUtils.isExists(sourceObj.calculateByDayOfWeek))
                this._calculateByDayOfWeek = !!sourceObj.calculateByDayOfWeek;
        }
    };
    RecurrenceBase.prototype.calculatePoints = function (start, end) {
        if (!start || !end)
            return new Array();
        var from = DateTimeUtils_1.DateTimeUtils.getMaxDate(start, this._start);
        var to = DateTimeUtils_1.DateTimeUtils.getMinDate(end, this._end);
        if (this._occurrenceCount > 0)
            return this.calculatePointsByOccurrenceCount(from, to);
        return this.calculatePointsByDateRange(from, to);
    };
    RecurrenceBase.prototype.calculatePointsByOccurrenceCount = function (start, end) {
        var points = new Array();
        var point = this.getFirstPoint(start);
        while (!!point && points.length < this._occurrenceCount && DateTimeUtils_1.DateTimeUtils.compareDates(point, end) >= 0) {
            if (this.isRecurrencePoint(point))
                points.push(point);
            point = this.getNextPoint(point);
        }
        return points;
    };
    RecurrenceBase.prototype.calculatePointsByDateRange = function (start, end) {
        var points = new Array();
        var point = this.getFirstPoint(start);
        while (!!point && DateTimeUtils_1.DateTimeUtils.compareDates(point, end) >= 0) {
            if (this.isRecurrencePoint(point))
                points.push(point);
            point = this.getNextPoint(point);
        }
        return points;
    };
    RecurrenceBase.prototype.getFirstPoint = function (start) {
        if (this.isRecurrencePoint(start))
            return start;
        return this.getNextPoint(start);
    };
    ;
    RecurrenceBase.prototype.isRecurrencePoint = function (date) {
        return this.isDateInRange(date) && this.checkDate(date) && (!this.useIntervalInCalc() || this.checkInterval(date));
    };
    RecurrenceBase.prototype.isDateInRange = function (date) {
        if (!date)
            return false;
        if (this._start && DateTimeUtils_1.DateTimeUtils.compareDates(this.start, date) < 0)
            return false;
        if (this._occurrenceCount == 0 && this.end && DateTimeUtils_1.DateTimeUtils.compareDates(date, this.end) < 0)
            return false;
        return true;
    };
    RecurrenceBase.prototype.useIntervalInCalc = function () {
        return this.interval > 1 && !!this._start;
    };
    RecurrenceBase.prototype.getNextPoint = function (date) {
        if (!this.isDateInRange(date))
            return null;
        if (this.useIntervalInCalc())
            return this.calculatePointByInterval(date);
        return this.calculateNearestPoint(date);
    };
    ;
    RecurrenceBase.prototype.getSpecDayInMonth = function (year, month) {
        var date;
        if (this._calculateByDayOfWeek)
            date = DateTimeUtils_1.DateTimeUtils.getSpecificDayOfWeekInMonthDate(this.dayOfWeekInternal, year, month, this.dayOfWeekOccurrenceInternal);
        else
            date = new Date(year, month, this.dayInternal);
        return date;
    };
    Object.defineProperty(RecurrenceBase.prototype, "dayInternal", {
        get: function () { return this._day; },
        set: function (value) {
            if (value > 0 && value <= 31)
                this._day = value;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "dayOfWeekInternal", {
        get: function () { return this._dayOfWeek; },
        set: function (dayOfWeek) {
            if (dayOfWeek >= DayOfWeek_1.DayOfWeek.Sunday && dayOfWeek <= DayOfWeek_1.DayOfWeek.Saturday)
                this._dayOfWeek = dayOfWeek;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "dayOfWeekOccurrenceInternal", {
        get: function () {
            return this._dayOfWeekOccurrence;
        },
        set: function (value) {
            if (value >= DayOfWeekMonthlyOccurrence_1.DayOfWeekMonthlyOccurrence.First && value <= DayOfWeekMonthlyOccurrence_1.DayOfWeekMonthlyOccurrence.Last)
                this._dayOfWeekOccurrence = value;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "monthInternal", {
        get: function () { return this._month; },
        set: function (value) {
            if (value >= Month_1.Month.January && value <= Month_1.Month.December)
                this._month = value;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "start", {
        get: function () { return this._start; },
        set: function (date) {
            if (!date)
                return;
            this._start = date;
            if (!!this._end && date > this._end)
                this._end = date;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "end", {
        get: function () { return this._end; },
        set: function (date) {
            if (!date)
                return;
            this._end = date;
            if (!!this._start && date < this._start)
                this._start = date;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "occurrenceCount", {
        get: function () { return this._occurrenceCount; },
        set: function (value) {
            if (value < 0)
                value = 0;
            this._occurrenceCount = value;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(RecurrenceBase.prototype, "interval", {
        get: function () { return this._interval; },
        set: function (value) {
            if (value > 0)
                this._interval = value;
        },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    return RecurrenceBase;
}(DataObject_1.DataObject));
exports.RecurrenceBase = RecurrenceBase;


/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CollectionBase_1 = __webpack_require__(6);
var Resource_1 = __webpack_require__(35);
var ResourceCollection = (function (_super) {
    __extends(ResourceCollection, _super);
    function ResourceCollection() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourceCollection.prototype.createItem = function () { return new Resource_1.Resource(); };
    return ResourceCollection;
}(CollectionBase_1.CollectionBase));
exports.ResourceCollection = ResourceCollection;


/***/ }),
/* 13 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DataObject_1 = __webpack_require__(4);
var Utils_1 = __webpack_require__(0);
var DependencyType;
(function (DependencyType) {
    DependencyType[DependencyType["FS"] = 0] = "FS";
    DependencyType[DependencyType["SS"] = 1] = "SS";
    DependencyType[DependencyType["FF"] = 2] = "FF";
    DependencyType[DependencyType["SF"] = 3] = "SF";
})(DependencyType = exports.DependencyType || (exports.DependencyType = {}));
var Dependency = (function (_super) {
    __extends(Dependency, _super);
    function Dependency() {
        var _this = _super.call(this) || this;
        _this.predecessorId = "";
        _this.successorId = "";
        _this.type = null;
        return _this;
    }
    Dependency.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.internalId = String(sourceObj.id);
            this.predecessorId = String(sourceObj.predecessorId);
            this.successorId = String(sourceObj.successorId);
            this.type = sourceObj.type;
        }
    };
    return Dependency;
}(DataObject_1.DataObject));
exports.Dependency = Dependency;


/***/ }),
/* 14 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Browser = (function () {
    function Browser() {
    }
    Browser.IdentUserAgent = function (userAgent, ignoreDocumentMode) {
        if (ignoreDocumentMode === void 0) { ignoreDocumentMode = false; }
        var browserTypesOrderedList = ["Mozilla", "IE", "Firefox", "Netscape", "Safari", "Chrome", "Opera", "Opera10", "Edge"];
        var defaultBrowserType = "IE";
        var defaultPlatform = "Win";
        var defaultVersions = { Safari: 2, Chrome: 0.1, Mozilla: 1.9, Netscape: 8, Firefox: 2, Opera: 9, IE: 6, Edge: 12 };
        if (!userAgent || userAgent.length == 0) {
            Browser.fillUserAgentInfo(browserTypesOrderedList, defaultBrowserType, defaultVersions[defaultBrowserType], defaultPlatform);
            return;
        }
        userAgent = userAgent.toLowerCase();
        Browser.indentPlatformMajorVersion(userAgent);
        try {
            var platformIdentStrings = {
                "Windows": "Win",
                "Macintosh": "Mac",
                "Mac OS": "Mac",
                "Mac_PowerPC": "Mac",
                "cpu os": "MacMobile",
                "cpu iphone os": "MacMobile",
                "Android": "Android",
                "!Windows Phone": "WinPhone",
                "!WPDesktop": "WinPhone",
                "!ZuneWP": "WinPhone"
            };
            var optSlashOrSpace = "(?:/|\\s*)?";
            var versionString = "(\\d+)(?:\\.((?:\\d+?[1-9])|\\d)0*?)?";
            var optVersion = "(?:" + versionString + ")?";
            var patterns = {
                Safari: "applewebkit(?:.*?(?:version/" + versionString + "[\\.\\w\\d]*?(?:\\s+mobile\/\\S*)?\\s+safari))?",
                Chrome: "(?:chrome|crios)(?!frame)" + optSlashOrSpace + optVersion,
                Mozilla: "mozilla(?:.*rv:" + optVersion + ".*Gecko)?",
                Netscape: "(?:netscape|navigator)\\d*/?\\s*" + optVersion,
                Firefox: "firefox" + optSlashOrSpace + optVersion,
                Opera: "(?:opera|\sopr)" + optSlashOrSpace + optVersion,
                Opera10: "opera.*\\s*version" + optSlashOrSpace + optVersion,
                IE: "msie\\s*" + optVersion,
                Edge: "edge" + optSlashOrSpace + optVersion
            };
            var browserType;
            var version = -1;
            for (var i = 0; i < browserTypesOrderedList.length; i++) {
                var browserTypeCandidate = browserTypesOrderedList[i];
                var regExp = new RegExp(patterns[browserTypeCandidate], "i");
                if (regExp.compile)
                    regExp.compile(patterns[browserTypeCandidate], "i");
                var matches = regExp.exec(userAgent);
                if (matches && matches.index >= 0) {
                    if (browserType == "IE" && version >= 11 && browserTypeCandidate == "Safari")
                        continue;
                    browserType = browserTypeCandidate;
                    if (browserType == "Opera10")
                        browserType = "Opera";
                    var tridentPattern = "trident" + optSlashOrSpace + optVersion;
                    version = Browser.GetBrowserVersion(userAgent, matches, tridentPattern, Browser.getIECompatibleVersionString());
                    if (browserType == "Mozilla" && version >= 11)
                        browserType = "IE";
                }
            }
            if (!browserType)
                browserType = defaultBrowserType;
            var browserVersionDetected = version != -1;
            if (!browserVersionDetected)
                version = defaultVersions[browserType];
            var platform;
            var minOccurenceIndex = Number.MAX_VALUE;
            for (var identStr in platformIdentStrings) {
                if (!platformIdentStrings.hasOwnProperty(identStr))
                    continue;
                var importantIdent = identStr.substr(0, 1) == "!";
                var occurenceIndex = userAgent.indexOf((importantIdent ? identStr.substr(1) : identStr).toLowerCase());
                if (occurenceIndex >= 0 && (occurenceIndex < minOccurenceIndex || importantIdent)) {
                    minOccurenceIndex = importantIdent ? 0 : occurenceIndex;
                    platform = platformIdentStrings[identStr];
                }
            }
            var samsungPattern = "SM-[A-Z]";
            var m = userAgent.toUpperCase().match(samsungPattern);
            var isSamsungAndroidDevice = m && m.length > 0;
            if (platform == "WinPhone" && version < 9)
                version = Math.floor(Browser.getVersionFromTrident(userAgent, "trident" + optSlashOrSpace + optVersion));
            if (!ignoreDocumentMode && browserType == "IE" && version > 7 && document.documentMode < version)
                version = document.documentMode;
            if (platform == "WinPhone")
                version = Math.max(9, version);
            if (!platform)
                platform = defaultPlatform;
            if (platform == platformIdentStrings["cpu os"] && !browserVersionDetected)
                version = 4;
            Browser.fillUserAgentInfo(browserTypesOrderedList, browserType, version, platform, isSamsungAndroidDevice);
        }
        catch (e) {
            Browser.fillUserAgentInfo(browserTypesOrderedList, defaultBrowserType, defaultVersions[defaultBrowserType], defaultPlatform);
        }
    };
    Browser.GetBrowserVersion = function (userAgent, matches, tridentPattern, ieCompatibleVersionString) {
        var version = Browser.getVersionFromMatches(matches);
        if (ieCompatibleVersionString) {
            var versionFromTrident = Browser.getVersionFromTrident(userAgent, tridentPattern);
            if (ieCompatibleVersionString === "edge" || parseInt(ieCompatibleVersionString) === versionFromTrident)
                return versionFromTrident;
        }
        return version;
    };
    Browser.getIECompatibleVersionString = function () {
        if (document.compatible) {
            for (var i = 0; i < document.compatible.length; i++)
                if (document.compatible[i].userAgent === "IE" && document.compatible[i].version)
                    return document.compatible[i].version.toLowerCase();
        }
        return "";
    };
    Browser.fillUserAgentInfo = function (browserTypesOrderedList, browserType, version, platform, isSamsungAndroidDevice) {
        if (isSamsungAndroidDevice === void 0) { isSamsungAndroidDevice = false; }
        for (var i = 0; i < browserTypesOrderedList.length; i++) {
            var type = browserTypesOrderedList[i];
            Browser[type] = type == browserType;
        }
        Browser.Version = Math.floor(10.0 * version) / 10.0;
        Browser.MajorVersion = Math.floor(Browser.Version);
        Browser.WindowsPlatform = platform == "Win" || platform == "WinPhone";
        Browser.MacOSPlatform = platform == "Mac";
        Browser.MacOSMobilePlatform = platform == "MacMobile";
        Browser.AndroidMobilePlatform = platform == "Android";
        Browser.WindowsPhonePlatform = platform == "WinPhone";
        Browser.WebKitFamily = Browser.Safari || Browser.Chrome || Browser.Opera && Browser.MajorVersion >= 15;
        Browser.NetscapeFamily = Browser.Netscape || Browser.Mozilla || Browser.Firefox;
        Browser.HardwareAcceleration = (Browser.IE && Browser.MajorVersion >= 9) || (Browser.Firefox && Browser.MajorVersion >= 4) ||
            (Browser.AndroidMobilePlatform && Browser.Chrome) || (Browser.Chrome && Browser.MajorVersion >= 37) ||
            (Browser.Safari && !Browser.WindowsPlatform) || Browser.Edge || (Browser.Opera && Browser.MajorVersion >= 46);
        Browser.WebKitTouchUI = Browser.MacOSMobilePlatform || Browser.AndroidMobilePlatform;
        var isIETouchUI = Browser.IE && Browser.MajorVersion > 9 && Browser.WindowsPlatform && Browser.UserAgent.toLowerCase().indexOf("touch") >= 0;
        Browser.MSTouchUI = isIETouchUI || (Browser.Edge && !!window.navigator.maxTouchPoints);
        Browser.TouchUI = Browser.WebKitTouchUI || Browser.MSTouchUI;
        Browser.MobileUI = Browser.WebKitTouchUI || Browser.WindowsPhonePlatform;
        Browser.AndroidDefaultBrowser = Browser.AndroidMobilePlatform && !Browser.Chrome;
        Browser.AndroidChromeBrowser = Browser.AndroidMobilePlatform && Browser.Chrome;
        if (isSamsungAndroidDevice)
            Browser.SamsungAndroidDevice = isSamsungAndroidDevice;
        if (Browser.MSTouchUI) {
            var isARMArchitecture = Browser.UserAgent.toLowerCase().indexOf("arm;") > -1;
            Browser.VirtualKeyboardSupported = isARMArchitecture || Browser.WindowsPhonePlatform;
        }
        else {
            Browser.VirtualKeyboardSupported = Browser.WebKitTouchUI;
        }
        Browser.fillDocumentElementBrowserTypeClassNames(browserTypesOrderedList);
    };
    Browser.indentPlatformMajorVersion = function (userAgent) {
        var regex = /(?:(?:windows nt|macintosh|mac os|cpu os|cpu iphone os|android|windows phone|linux) )(\d+)(?:[-0-9_.])*/;
        var matches = regex.exec(userAgent);
        if (matches)
            Browser.PlaformMajorVersion = matches[1];
    };
    Browser.prototype.GetBrowserVersion = function (userAgent, matches, tridentPattern, ieCompatibleVersionString) {
        var version = Browser.getVersionFromMatches(matches);
        if (ieCompatibleVersionString) {
            var versionFromTrident = Browser.getVersionFromTrident(userAgent, tridentPattern);
            if (ieCompatibleVersionString === "edge" || parseInt(ieCompatibleVersionString) === versionFromTrident)
                return versionFromTrident;
        }
        return version;
    };
    Browser.getVersionFromMatches = function (matches) {
        var result = -1;
        var versionStr = "";
        if (matches[1]) {
            versionStr += matches[1];
            if (matches[2])
                versionStr += "." + matches[2];
        }
        if (versionStr != "") {
            result = parseFloat(versionStr);
            if (isNaN(result))
                result = -1;
        }
        return result;
    };
    Browser.getVersionFromTrident = function (userAgent, tridentPattern) {
        var tridentDiffFromVersion = 4;
        var matches = new RegExp(tridentPattern, "i").exec(userAgent);
        return Browser.getVersionFromMatches(matches) + tridentDiffFromVersion;
    };
    Browser.fillDocumentElementBrowserTypeClassNames = function (browserTypesOrderedList) {
        var documentElementClassName = "";
        var browserTypeslist = browserTypesOrderedList.concat(["WindowsPlatform", "MacOSPlatform", "MacOSMobilePlatform", "AndroidMobilePlatform",
            "WindowsPhonePlatform", "WebKitFamily", "WebKitTouchUI", "MSTouchUI", "TouchUI", "AndroidDefaultBrowser"]);
        for (var i = 0; i < browserTypeslist.length; i++) {
            var type = browserTypeslist[i];
            if (Browser[type])
                documentElementClassName += "dx" + type + " ";
        }
        documentElementClassName += "dxBrowserVersion-" + Browser.MajorVersion;
        if (document && document.documentElement) {
            if (document.documentElement.className != "")
                documentElementClassName = " " + documentElementClassName;
            document.documentElement.className += documentElementClassName;
            Browser.Info = documentElementClassName;
        }
    };
    Browser.UserAgent = window.navigator.userAgent.toLowerCase();
    Browser._foo = Browser.IdentUserAgent(Browser.UserAgent);
    return Browser;
}());
exports.Browser = Browser;


/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Size = (function () {
    function Size(width, height) {
        this.width = null;
        this.height = null;
        if (width !== undefined)
            this.width = width;
        if (height !== undefined)
            this.height = height;
    }
    return Size;
}());
exports.Size = Size;


/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItemState_1 = __webpack_require__(58);
var BaseManipulator = (function () {
    function BaseManipulator(viewModel, dispatcher) {
        this.viewModel = viewModel;
        this.dispatcher = dispatcher;
    }
    return BaseManipulator;
}());
exports.BaseManipulator = BaseManipulator;
var TaskPropertiesManipulator = (function (_super) {
    __extends(TaskPropertiesManipulator, _super);
    function TaskPropertiesManipulator(viewModel, dispatcher) {
        var _this = _super.call(this, viewModel, dispatcher) || this;
        _this.title = new TaskTitleManipulator(viewModel, dispatcher);
        _this.description = new TaskDescriptionManipulator(viewModel, dispatcher);
        _this.progress = new TaskProgressManipulator(viewModel, dispatcher);
        _this.start = new TaskStartDateManipulator(viewModel, dispatcher);
        _this.end = new TaskEndDateManipulator(viewModel, dispatcher);
        return _this;
    }
    return TaskPropertiesManipulator;
}(BaseManipulator));
exports.TaskPropertiesManipulator = TaskPropertiesManipulator;
var TaskPropertyManipulator = (function (_super) {
    __extends(TaskPropertyManipulator, _super);
    function TaskPropertyManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskPropertyManipulator.prototype.setValue = function (id, newValue) {
        var task = this.viewModel.tasks.getItemById(id);
        var oldState = new HistoryItemState_1.HistoryItemState(id, this.getPropertyValue(task));
        this.setPropertyValue(task, newValue);
        this.viewModel.owner.resetAndUpdate();
        return oldState;
    };
    TaskPropertyManipulator.prototype.restoreValue = function (state) {
        if (!state)
            return;
        var stateValue = state.value;
        var task = this.viewModel.tasks.getItemById(state.taskId);
        this.setPropertyValue(task, stateValue);
        this.viewModel.owner.resetAndUpdate();
    };
    return TaskPropertyManipulator;
}(BaseManipulator));
exports.TaskPropertyManipulator = TaskPropertyManipulator;
var TaskTitleManipulator = (function (_super) {
    __extends(TaskTitleManipulator, _super);
    function TaskTitleManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskTitleManipulator.prototype.getPropertyValue = function (task) {
        return task.title;
    };
    TaskTitleManipulator.prototype.setPropertyValue = function (task, value) {
        task.title = value;
        this.dispatcher.notifyTaskTitleChanged(task.id, value);
    };
    return TaskTitleManipulator;
}(TaskPropertyManipulator));
var TaskDescriptionManipulator = (function (_super) {
    __extends(TaskDescriptionManipulator, _super);
    function TaskDescriptionManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskDescriptionManipulator.prototype.getPropertyValue = function (task) {
        return task.description;
    };
    TaskDescriptionManipulator.prototype.setPropertyValue = function (task, value) {
        task.description = value;
        this.dispatcher.notifyTaskDescriptionChanged(task.id, value);
    };
    return TaskDescriptionManipulator;
}(TaskPropertyManipulator));
var TaskProgressManipulator = (function (_super) {
    __extends(TaskProgressManipulator, _super);
    function TaskProgressManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskProgressManipulator.prototype.getPropertyValue = function (task) {
        return task.progress;
    };
    TaskProgressManipulator.prototype.setPropertyValue = function (task, value) {
        task.progress = value;
        this.dispatcher.notifyTaskProgressChanged(task.id, value);
    };
    return TaskProgressManipulator;
}(TaskPropertyManipulator));
var TaskStartDateManipulator = (function (_super) {
    __extends(TaskStartDateManipulator, _super);
    function TaskStartDateManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskStartDateManipulator.prototype.getPropertyValue = function (task) {
        return task.start;
    };
    TaskStartDateManipulator.prototype.setPropertyValue = function (task, value) {
        task.start = value;
        this.dispatcher.notifyTaskStartChanged(task.id, value);
    };
    return TaskStartDateManipulator;
}(TaskPropertyManipulator));
var TaskEndDateManipulator = (function (_super) {
    __extends(TaskEndDateManipulator, _super);
    function TaskEndDateManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskEndDateManipulator.prototype.getPropertyValue = function (task) {
        return task.end;
    };
    TaskEndDateManipulator.prototype.setPropertyValue = function (task, value) {
        task.end = value;
        this.dispatcher.notifyTaskEndChanged(task.id, value);
    };
    return TaskEndDateManipulator;
}(TaskPropertyManipulator));


/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HandlerStateBase_1 = __webpack_require__(66);
var Point_1 = __webpack_require__(3);
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerStateBase = (function (_super) {
    __extends(MouseHandlerStateBase, _super);
    function MouseHandlerStateBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerStateBase.prototype.onMouseDoubleClick = function (_evt) { };
    MouseHandlerStateBase.prototype.onMouseDown = function (_evt) { };
    MouseHandlerStateBase.prototype.onMouseUp = function (_evt) { };
    MouseHandlerStateBase.prototype.onMouseMove = function (_evt) { };
    MouseHandlerStateBase.prototype.onMouseWheel = function (_evt) { };
    MouseHandlerStateBase.prototype.getRelativePos = function (absolutePos) {
        var taskAreaX = DomUtils_1.DomUtils.getAbsolutePositionX(this.handler.control.taskArea);
        var taskAreaY = DomUtils_1.DomUtils.getAbsolutePositionY(this.handler.control.taskArea);
        return new Point_1.Point(absolutePos.x - taskAreaX, absolutePos.y - taskAreaY);
    };
    return MouseHandlerStateBase;
}(HandlerStateBase_1.HandlerStateBase));
exports.MouseHandlerStateBase = MouseHandlerStateBase;


/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Point_1 = __webpack_require__(3);
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerStateBase_1 = __webpack_require__(17);
var MouseHandlerDragBaseState = (function (_super) {
    __extends(MouseHandlerDragBaseState, _super);
    function MouseHandlerDragBaseState() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerDragBaseState.prototype.onMouseDown = function (evt) {
        this.currentPosition = new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt));
    };
    MouseHandlerDragBaseState.prototype.onMouseUp = function (evt) {
        this.onMouseUpInternal(evt);
        this.handler.switchToDefaultState();
        this.handler.onMouseUp(evt);
    };
    MouseHandlerDragBaseState.prototype.onMouseMove = function (evt) {
        var position = new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt));
        this.onMouseMoveInternal(position);
        this.currentPosition = position;
    };
    MouseHandlerDragBaseState.prototype.onMouseUpInternal = function (_evt) { };
    MouseHandlerDragBaseState.prototype.onMouseMoveInternal = function (_position) { };
    return MouseHandlerDragBaseState;
}(MouseHandlerStateBase_1.MouseHandlerStateBase));
exports.MouseHandlerDragBaseState = MouseHandlerDragBaseState;


/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem_1 = __webpack_require__(9);
var InsertDependencyHistoryItem = (function (_super) {
    __extends(InsertDependencyHistoryItem, _super);
    function InsertDependencyHistoryItem(modelManipulator, predecessorId, successorId, type) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.predecessorId = predecessorId;
        _this.successorId = successorId;
        _this.type = type;
        return _this;
    }
    InsertDependencyHistoryItem.prototype.redo = function () {
        this.dependency = this.modelManipulator.dependency.insertDependency(this.predecessorId, this.successorId, this.type);
    };
    InsertDependencyHistoryItem.prototype.undo = function () {
        this.modelManipulator.dependency.removeDependency(this.dependency.internalId);
    };
    return InsertDependencyHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.InsertDependencyHistoryItem = InsertDependencyHistoryItem;
var RemoveDependencyHistoryItem = (function (_super) {
    __extends(RemoveDependencyHistoryItem, _super);
    function RemoveDependencyHistoryItem(modelManipulator, dependencyId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.dependencyId = dependencyId;
        return _this;
    }
    RemoveDependencyHistoryItem.prototype.redo = function () {
        this.dependency = this.modelManipulator.dependency.removeDependency(this.dependencyId);
    };
    RemoveDependencyHistoryItem.prototype.undo = function () {
        this.dependencyId = this.modelManipulator.dependency.insertDependency(this.dependency.predecessorId, this.dependency.successorId, this.dependency.type).internalId;
    };
    return RemoveDependencyHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.RemoveDependencyHistoryItem = RemoveDependencyHistoryItem;


/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem_1 = __webpack_require__(9);
var CreateResourceHistoryItem = (function (_super) {
    __extends(CreateResourceHistoryItem, _super);
    function CreateResourceHistoryItem(modelManipulator, text) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.text = text;
        return _this;
    }
    CreateResourceHistoryItem.prototype.redo = function () {
        this.resource = this.modelManipulator.resource.create(this.text, this.resource ? this.resource.internalId : null);
    };
    CreateResourceHistoryItem.prototype.undo = function () {
        this.modelManipulator.resource.remove(this.resource.internalId);
    };
    return CreateResourceHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.CreateResourceHistoryItem = CreateResourceHistoryItem;
var RemoveResourceHistoryItem = (function (_super) {
    __extends(RemoveResourceHistoryItem, _super);
    function RemoveResourceHistoryItem(modelManipulator, resourceId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.resourceId = resourceId;
        return _this;
    }
    RemoveResourceHistoryItem.prototype.redo = function () {
        this.resource = this.modelManipulator.resource.remove(this.resourceId);
    };
    RemoveResourceHistoryItem.prototype.undo = function () {
        this.modelManipulator.resource.create(this.resource.text, this.resourceId);
    };
    return RemoveResourceHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.RemoveResourceHistoryItem = RemoveResourceHistoryItem;
var AssignResourceHistoryItem = (function (_super) {
    __extends(AssignResourceHistoryItem, _super);
    function AssignResourceHistoryItem(modelManipulator, resourceId, taskId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.resourceId = resourceId;
        _this.taskId = taskId;
        return _this;
    }
    AssignResourceHistoryItem.prototype.redo = function () {
        this.assignment = this.modelManipulator.resource.assign(this.resourceId, this.taskId);
    };
    AssignResourceHistoryItem.prototype.undo = function () {
        this.modelManipulator.resource.deassig(this.assignment.internalId);
    };
    return AssignResourceHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.AssignResourceHistoryItem = AssignResourceHistoryItem;
var DeassignResourceHistoryItem = (function (_super) {
    __extends(DeassignResourceHistoryItem, _super);
    function DeassignResourceHistoryItem(modelManipulator, assignmentId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.assignmentId = assignmentId;
        return _this;
    }
    DeassignResourceHistoryItem.prototype.redo = function () {
        this.assignment = this.modelManipulator.resource.deassig(this.assignmentId);
    };
    DeassignResourceHistoryItem.prototype.undo = function () {
        this.modelManipulator.resource.assign(this.assignment.resourceId, this.assignment.taskId);
    };
    return DeassignResourceHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.DeassignResourceHistoryItem = DeassignResourceHistoryItem;


/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase_1 = __webpack_require__(5);
var DialogBase = (function (_super) {
    __extends(DialogBase, _super);
    function DialogBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DialogBase.prototype.executeInternal = function (options) {
        var _this = this;
        var params = this.createParameters(options);
        var initParams = params.clone();
        this.control.ganttOwner.showDialog(this.getDialogName(), params, function (result) {
            if (result)
                _this.applyParameters(result, initParams);
        }, function () {
            _this.afterClosing();
        });
        return true;
    };
    DialogBase.prototype.applyParameters = function (_newParameters, _oldParameters) {
        return false;
    };
    DialogBase.prototype.afterClosing = function () { };
    DialogBase.prototype.getState = function () {
        return new CommandBase_1.SimpleCommandState(this.isEnabled());
    };
    return DialogBase;
}(CommandBase_1.CommandBase));
exports.DialogBase = DialogBase;
var DialogParametersBase = (function () {
    function DialogParametersBase() {
    }
    return DialogParametersBase;
}());
exports.DialogParametersBase = DialogParametersBase;


/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Time = (function () {
    function Time(h, min, sec, msec) {
        if (h === void 0) { h = 0; }
        if (min === void 0) { min = 0; }
        if (sec === void 0) { sec = 0; }
        if (msec === void 0) { msec = 0; }
        this._hour = 0;
        this._min = 0;
        this._sec = 0;
        this._msec = 0;
        this._fullmsec = 0;
        this.hour = h;
        this.min = min;
        this.sec = sec;
        this.msec = msec;
    }
    Object.defineProperty(Time.prototype, "hour", {
        get: function () { return this._hour; },
        set: function (h) {
            if (h >= 0 && h < 24) {
                this._hour = h;
                this.updateFullMilleconds();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Time.prototype, "min", {
        get: function () { return this._min; },
        set: function (m) {
            if (m >= 0 && m < 60) {
                this._min = m;
                this.updateFullMilleconds();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Time.prototype, "sec", {
        get: function () { return this._sec; },
        set: function (s) {
            if (s >= 0 && s < 60) {
                this._sec = s;
                this.updateFullMilleconds();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Time.prototype, "msec", {
        get: function () { return this._msec; },
        set: function (ms) {
            if (ms >= 0 && ms < 1000) {
                this._msec = ms;
                this.updateFullMilleconds();
            }
        },
        enumerable: true,
        configurable: true
    });
    Time.prototype.updateFullMilleconds = function () {
        var minutes = this._hour * 60 + this._min;
        var sec = minutes * 60 + this._sec;
        this._fullmsec = sec * 1000 + this._msec;
    };
    Time.prototype.getTimeInMilleconds = function () {
        return this._fullmsec;
    };
    return Time;
}());
exports.Time = Time;


/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DateTimeUtils_1 = __webpack_require__(1);
var TimeRange = (function () {
    function TimeRange(start, end) {
        var diff = DateTimeUtils_1.DateTimeUtils.caclTimeDifference(start, end);
        if (diff >= 0) {
            this._start = start;
            this._end = end;
        }
        else {
            this._start = end;
            this._end = start;
        }
    }
    ;
    Object.defineProperty(TimeRange.prototype, "start", {
        get: function () { return this._start; },
        set: function (time) {
            if (time && DateTimeUtils_1.DateTimeUtils.caclTimeDifference(time, this._end) >= 0)
                this._start = time;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeRange.prototype, "end", {
        get: function () { return this._end; },
        set: function (time) {
            if (time && DateTimeUtils_1.DateTimeUtils.caclTimeDifference(this._start, time) >= 0)
                this._end = time;
        },
        enumerable: true,
        configurable: true
    });
    TimeRange.prototype.isTimeInRange = function (time) {
        return DateTimeUtils_1.DateTimeUtils.caclTimeDifference(this._start, time) >= 0 && DateTimeUtils_1.DateTimeUtils.caclTimeDifference(time, this._end) >= 0;
    };
    TimeRange.prototype.hasIntersect = function (range) {
        return this.isTimeInRange(range.start) || this.isTimeInRange(range.end) || range.isTimeInRange(this.start) || range.isTimeInRange(this.end);
    };
    TimeRange.prototype.concatWith = function (range) {
        if (!this.hasIntersect(range))
            return false;
        ;
        this.start = DateTimeUtils_1.DateTimeUtils.getMinTime(this.start, range.start);
        this.end = DateTimeUtils_1.DateTimeUtils.getMaxTime(this.end, range.end);
        return true;
    };
    return TimeRange;
}());
exports.TimeRange = TimeRange;


/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DayOfWeekMonthlyOccurrence;
(function (DayOfWeekMonthlyOccurrence) {
    DayOfWeekMonthlyOccurrence[DayOfWeekMonthlyOccurrence["First"] = 0] = "First";
    DayOfWeekMonthlyOccurrence[DayOfWeekMonthlyOccurrence["Second"] = 1] = "Second";
    DayOfWeekMonthlyOccurrence[DayOfWeekMonthlyOccurrence["Third"] = 2] = "Third";
    DayOfWeekMonthlyOccurrence[DayOfWeekMonthlyOccurrence["Forth"] = 3] = "Forth";
    DayOfWeekMonthlyOccurrence[DayOfWeekMonthlyOccurrence["Last"] = 4] = "Last";
})(DayOfWeekMonthlyOccurrence = exports.DayOfWeekMonthlyOccurrence || (exports.DayOfWeekMonthlyOccurrence = {}));


/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var Daily_1 = __webpack_require__(26);
var Weekly_1 = __webpack_require__(48);
var Monthly_1 = __webpack_require__(49);
var Yearly_1 = __webpack_require__(51);
var RecurrenceFactory = (function () {
    function RecurrenceFactory() {
    }
    RecurrenceFactory.createRecurrenceByType = function (type) {
        if (!type)
            return null;
        var correctedType = type.toLowerCase();
        switch (correctedType) {
            case "daily":
                return new Daily_1.Daily();
            case "weekly":
                return new Weekly_1.Weekly();
            case "monthly":
                return new Monthly_1.Monthly();
            case "yearly":
                return new Yearly_1.Yearly();
        }
        return null;
    };
    RecurrenceFactory.createRecurrenceFromObject = function (sourceObj) {
        if (!sourceObj)
            return null;
        var recurrence = this.createRecurrenceByType(sourceObj.type);
        if (recurrence)
            recurrence.assignFromObject(sourceObj);
        return recurrence;
    };
    RecurrenceFactory.getEnumValue = function (type, value) {
        if (!Utils_1.JsonUtils.isExists(type[value]))
            return null;
        var num = parseInt(value);
        if (!isNaN(num))
            return num;
        return type[value];
    };
    return RecurrenceFactory;
}());
exports.RecurrenceFactory = RecurrenceFactory;


/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var RecurrenceBase_1 = __webpack_require__(11);
var DateTimeUtils_1 = __webpack_require__(1);
var Daily = (function (_super) {
    __extends(Daily, _super);
    function Daily() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Daily.prototype.checkDate = function (date) { return true; };
    Daily.prototype.checkInterval = function (date) {
        return DateTimeUtils_1.DateTimeUtils.getDaysBetween(this.start, date) % this.interval == 0;
    };
    Daily.prototype.calculatePointByInterval = function (date) {
        var daysToAdd = this.interval;
        if (!this.isRecurrencePoint(date))
            daysToAdd -= DateTimeUtils_1.DateTimeUtils.getDaysBetween(this.start, date) % this.interval;
        return DateTimeUtils_1.DateTimeUtils.addDays(date, daysToAdd);
    };
    Daily.prototype.calculateNearestPoint = function (date) {
        return DateTimeUtils_1.DateTimeUtils.addDays(date, 1);
    };
    return Daily;
}(RecurrenceBase_1.RecurrenceBase));
exports.Daily = Daily;


/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Enums_1 = __webpack_require__(7);
var DomUtils_1 = __webpack_require__(2);
var DateUtils = (function () {
    function DateUtils() {
    }
    DateUtils.getDaysInMonth = function (month, year) {
        var d = new Date(year, month + 1, 0);
        return d.getDate();
    };
    DateUtils.getOffsetInMonths = function (start, end) {
        return (end.getFullYear() - start.getFullYear()) * 12 + end.getMonth() - start.getMonth();
    };
    DateUtils.getOffsetInQuarters = function (start, end) {
        return (end.getFullYear() - start.getFullYear()) * 4 + Math.floor(end.getMonth() / 3) - Math.floor(start.getMonth() / 3);
    };
    DateUtils.getNearestScaleTickDate = function (date, range, tickTimeSpan, viewType) {
        var result = new Date();
        var rangeStartTime = range.start.getTime();
        var rangeEndTime = range.end.getTime();
        result.setTime(date.getTime());
        if (date.getTime() < rangeStartTime)
            result.setTime(rangeStartTime);
        else if (date.getTime() > rangeEndTime)
            result.setTime(rangeEndTime);
        else if (this.needCorrectDate(date, rangeStartTime, tickTimeSpan, viewType)) {
            var nearestLeftTickTime = this.getNearestLeftTickTime(date, rangeStartTime, tickTimeSpan, viewType);
            var nearestRightTickTime = this.getNextTickTime(nearestLeftTickTime, tickTimeSpan, viewType);
            if (Math.abs(date.getTime() - nearestLeftTickTime) > Math.abs(date.getTime() - nearestRightTickTime))
                result.setTime(nearestRightTickTime);
            else
                result.setTime(nearestLeftTickTime);
        }
        return result;
    };
    DateUtils.needCorrectDate = function (date, rangeStartTime, tickTimeSpan, viewType) {
        if (viewType == Enums_1.ViewType.Months)
            return date.getTime() !== new Date(date.getFullYear(), date.getMonth(), 1).getTime();
        return (date.getTime() - rangeStartTime) % tickTimeSpan !== 0;
    };
    DateUtils.getNearestLeftTickTime = function (date, rangeStartTime, tickTimeSpan, viewType) {
        if (viewType == Enums_1.ViewType.Months)
            return new Date(date.getFullYear(), date.getMonth(), 1).getTime();
        var tickCountAtLeft = Math.floor((date.getTime() - rangeStartTime) / tickTimeSpan);
        return rangeStartTime + tickCountAtLeft * tickTimeSpan;
    };
    DateUtils.getNextTickTime = function (currentTickTime, tickTimeSpan, viewType) {
        if (viewType == Enums_1.ViewType.Months) {
            var nextTickDate = new Date();
            nextTickDate.setTime(currentTickTime);
            nextTickDate.setMonth(nextTickDate.getMonth() + 1);
            return nextTickDate.getTime();
        }
        return currentTickTime + tickTimeSpan;
    };
    DateUtils.adjustStartEndDateByViewType = function (date, isStart, viewType) {
        var result = new Date();
        switch (viewType) {
            case Enums_1.ViewType.TenMinutes:
                result = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours());
                if (date.getTime() !== result.getTime() && !isStart)
                    result = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 1);
                break;
            case Enums_1.ViewType.SixHours:
            case Enums_1.ViewType.Hours:
                result = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                if (date.getTime() !== result.getTime() && !isStart)
                    result = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
                break;
            case Enums_1.ViewType.Days:
            case Enums_1.ViewType.Weeks:
                result = new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay());
                if (date.getTime() !== result.getTime() && !isStart)
                    result = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 7 - date.getDay());
                break;
            case Enums_1.ViewType.Months:
            case Enums_1.ViewType.Quarter:
                result = new Date(date.getFullYear(), 0, 1);
                if (date.getTime() !== result.getTime() && !isStart)
                    result = new Date(date.getFullYear() + 1, 0, 1);
                break;
        }
        return result;
    };
    DateUtils.getTickTimeSpan = function (viewType) {
        switch (viewType) {
            case Enums_1.ViewType.TenMinutes:
                return DateUtils.msPerHour / 6;
            case Enums_1.ViewType.Hours:
                return DateUtils.msPerHour;
            case Enums_1.ViewType.SixHours:
                return DateUtils.msPerHour * 6;
            case Enums_1.ViewType.Days:
                return DateUtils.msPerDay;
            case Enums_1.ViewType.Weeks:
                return DateUtils.msPerWeek;
            case Enums_1.ViewType.Months:
                return DateUtils.msPerMonth;
            case Enums_1.ViewType.Quarter:
                return DateUtils.msPerMonth * 3;
        }
    };
    DateUtils.msPerHour = 3600000;
    DateUtils.msPerDay = 24 * DateUtils.msPerHour;
    DateUtils.msPerWeek = 7 * DateUtils.msPerDay;
    DateUtils.msPerMonth = 30 * DateUtils.msPerDay;
    DateUtils.ViewTypeToScaleMap = createViewTypeToScaleMap();
    return DateUtils;
}());
exports.DateUtils = DateUtils;
var ElementTextHelper = (function () {
    function ElementTextHelper() {
        this.longestAbbrMonthName = null;
        this.longestMonthName = null;
        this.longestAbbrDayName = null;
        var canvas = document.createElement("canvas");
        this.textMeasureContext = canvas.getContext("2d");
    }
    ElementTextHelper.prototype.setFont = function (fontHolder) {
        var computedStyle = DomUtils_1.DomUtils.getCurrentStyle(fontHolder);
        var font = computedStyle.font ? computedStyle.font :
            computedStyle.fontStyle + " " + computedStyle.fontVariant + " " + computedStyle.fontWeight + " "
                + computedStyle.fontSize + " / " + computedStyle.lineHeight + " " + computedStyle.fontFamily;
        this.textMeasureContext.font = font;
    };
    ElementTextHelper.prototype.setSettings = function (startTime, viewType, modelItems) {
        this.startTime = startTime;
        this.viewType = viewType;
        this.modelItems = modelItems;
    };
    ElementTextHelper.prototype.getScaleItemStartDate = function (index, scaleType) {
        var result = new Date(this.startTime);
        switch (scaleType) {
            case Enums_1.ViewType.TenMinutes:
                result.setTime(this.startTime + index * DateUtils.msPerHour / 6);
                break;
            case Enums_1.ViewType.Hours:
                result.setTime(this.startTime + index * DateUtils.msPerHour);
                break;
            case Enums_1.ViewType.SixHours:
                result.setTime(this.startTime + index * DateUtils.msPerHour * 6);
                break;
            case Enums_1.ViewType.Days:
                result.setTime(this.startTime + index * DateUtils.msPerDay);
                break;
            case Enums_1.ViewType.Weeks:
                result.setTime(this.startTime + index * DateUtils.msPerWeek);
                break;
            case Enums_1.ViewType.Months:
                result.setMonth(result.getMonth() + index);
                break;
            case Enums_1.ViewType.Quarter:
                result.setMonth(result.getMonth() + index * 3);
                break;
            case Enums_1.ViewType.Years:
                result.setFullYear(result.getFullYear() + index);
                break;
        }
        return result;
    };
    ElementTextHelper.prototype.getScaleItemText = function (index, scaleType) {
        var scaleItemStartDate = this.getScaleItemStartDate(index, scaleType);
        var isViewTypeScale = this.viewType.valueOf() == scaleType.valueOf();
        switch (scaleType) {
            case Enums_1.ViewType.TenMinutes:
                return this.getTenMinutesScaleItemText(scaleItemStartDate);
            case Enums_1.ViewType.Hours:
            case Enums_1.ViewType.SixHours:
                return this.getHoursScaleItemText(scaleItemStartDate);
            case Enums_1.ViewType.Days:
                return this.getDaysScaleItemText(scaleItemStartDate, isViewTypeScale);
            case Enums_1.ViewType.Weeks:
                return this.getWeeksScaleItemText(scaleItemStartDate, isViewTypeScale);
            case Enums_1.ViewType.Months:
                return this.getMonthsScaleItemText(scaleItemStartDate, isViewTypeScale);
            case Enums_1.ViewType.Quarter:
                return this.getQuarterScaleItemText(scaleItemStartDate, isViewTypeScale);
            case Enums_1.ViewType.Years:
                return this.getYearsScaleItemText(scaleItemStartDate);
        }
    };
    ElementTextHelper.prototype.getTenMinutesScaleItemText = function (scaleItemDate) {
        var minutes = scaleItemDate.getMinutes() + 1;
        return (Math.ceil(minutes / 10) * 10).toString();
    };
    ElementTextHelper.prototype.getThirtyMinutesScaleItemText = function (scaleItemDate) {
        var minutes = scaleItemDate.getMinutes();
        return minutes < 30 ? "30" : "60";
    };
    ElementTextHelper.prototype.getHoursScaleItemText = function (scaleItemDate) {
        var hours = scaleItemDate.getHours();
        var hourDisplayText = this.getHourDisplayText(hours);
        var amPmText = hours < 12 ? this.getAmText() : this.getPmText();
        return this.getHoursScaleItemTextCore(hourDisplayText, amPmText);
    };
    ElementTextHelper.prototype.getDaysScaleItemText = function (scaleItemDate, isViewTypeScale) {
        return this.getDayTotalText(scaleItemDate, true, isViewTypeScale, isViewTypeScale, !isViewTypeScale);
    };
    ElementTextHelper.prototype.getWeeksScaleItemText = function (scaleItemDate, isViewTypeScale) {
        var weekLastDayDate = new Date(scaleItemDate.getTime() + DateUtils.msPerWeek - DateUtils.msPerDay);
        return this.getWeeksScaleItemTextCore(this.getDayTotalText(scaleItemDate, isViewTypeScale, true, isViewTypeScale, !isViewTypeScale), this.getDayTotalText(weekLastDayDate, isViewTypeScale, true, isViewTypeScale, !isViewTypeScale));
    };
    ElementTextHelper.prototype.getMonthsScaleItemText = function (scaleItemDate, isViewTypeScale) {
        var monthNames = this.getMonthNames();
        var yearDisplayText = !isViewTypeScale ? scaleItemDate.getFullYear().toString() : "";
        return this.getMonthsScaleItemTextCore(monthNames[scaleItemDate.getMonth()], yearDisplayText);
    };
    ElementTextHelper.prototype.getQuarterScaleItemText = function (scaleItemDate, isViewTypeScale) {
        var quarterNames = this.getQuarterNames();
        var yearDisplayText = !isViewTypeScale ? scaleItemDate.getFullYear().toString() : "";
        return this.getMonthsScaleItemTextCore(quarterNames[Math.floor(scaleItemDate.getMonth() / 3)], yearDisplayText);
    };
    ElementTextHelper.prototype.getYearsScaleItemText = function (scaleItemDate) {
        return scaleItemDate.getFullYear().toString();
    };
    ElementTextHelper.prototype.getHourDisplayText = function (hours) {
        if (this.hasAmPm())
            return (hours == 0 ? 12 : (hours <= 12 ? hours : hours - 12)).toString();
        return hours < 10 ? "0" + hours : hours.toString();
    };
    ElementTextHelper.prototype.getDayTotalText = function (scaleItemDate, displayDayName, useAbbrDayNames, useAbbrMonthNames, displayYear) {
        var monthNames = useAbbrMonthNames ? this.getAbbrMonthNames() : this.getMonthNames();
        var dayNames = useAbbrDayNames ? this.getAbbrDayNames() : this.getDayNames();
        var dayNameDisplayText = displayDayName ? dayNames[scaleItemDate.getDay()] : "";
        var day = scaleItemDate.getDate();
        var monthName = monthNames[scaleItemDate.getMonth()];
        var yearDisplayText = displayYear ? scaleItemDate.getFullYear().toString() : "";
        return this.getDayTotalTextCore(dayNameDisplayText, day.toString(), monthName, yearDisplayText);
    };
    ElementTextHelper.prototype.getTaskText = function (index) {
        var item = this.modelItems[index];
        return item ? item.task.title : "";
    };
    ElementTextHelper.prototype.getTaskVisibility = function (index) {
        var item = this.modelItems[index];
        return !!item && item.getVisible();
    };
    ElementTextHelper.prototype.hasAmPm = function () {
        return this.getAmText().length > 0 || this.getPmText().length > 0;
    };
    ElementTextHelper.prototype.getScaleItemTextTemplate = function (viewType) {
        switch (viewType) {
            case Enums_1.ViewType.TenMinutes:
                return "00";
            case Enums_1.ViewType.Hours:
            case Enums_1.ViewType.SixHours:
                return this.getHoursScaleItemTextCore("00", this.getAmText());
            case Enums_1.ViewType.Days:
                return this.getDayTextTemplate();
            case Enums_1.ViewType.Weeks:
                return this.getWeekTextTemplate();
            case Enums_1.ViewType.Months:
                return this.getMonthsScaleItemTextCore(this.getLongestMonthName(), "");
            case Enums_1.ViewType.Quarter:
                return "Q4";
        }
    };
    ElementTextHelper.prototype.getDayTextTemplate = function () {
        return this.getDayTotalTextCore(this.getLongestAbbrDayName(), "00", this.getLongestAbbrMonthName(), "");
    };
    ElementTextHelper.prototype.getWeekTextTemplate = function () {
        var dayTextTemplate = this.getDayTextTemplate();
        return this.getWeeksScaleItemTextCore(dayTextTemplate, dayTextTemplate);
    };
    ElementTextHelper.prototype.getHoursScaleItemTextCore = function (hourDisplayText, amPmText) {
        return hourDisplayText + ":00" + (this.hasAmPm() ? " " + amPmText : "");
    };
    ElementTextHelper.prototype.getDayTotalTextCore = function (dayName, dayValueString, monthName, yearValueString) {
        var result = dayName.length > 0 ? dayName + ", " : "";
        result += dayValueString + " " + monthName;
        result += (yearValueString.length > 0 ? " " + yearValueString : "");
        return result;
    };
    ElementTextHelper.prototype.getWeeksScaleItemTextCore = function (firstDayOfWeekString, lastDayOfWeekString) {
        return firstDayOfWeekString + " - " + lastDayOfWeekString;
    };
    ElementTextHelper.prototype.getMonthsScaleItemTextCore = function (monthName, yearValueString) {
        var result = monthName;
        if (yearValueString.length > 0)
            result += " " + yearValueString;
        return result;
    };
    ElementTextHelper.prototype.getLongestAbbrMonthName = function () {
        if (this.longestAbbrMonthName == null)
            this.longestAbbrMonthName = this.getLongestText(this.getAbbrMonthNames());
        return this.longestAbbrMonthName;
    };
    ElementTextHelper.prototype.getLongestMonthName = function () {
        if (this.longestMonthName == null)
            this.longestMonthName = this.getLongestText(this.getMonthNames());
        return this.longestMonthName;
    };
    ElementTextHelper.prototype.getLongestAbbrDayName = function () {
        if (this.longestAbbrDayName == null)
            this.longestAbbrDayName = this.getLongestText(this.getAbbrDayNames());
        return this.longestAbbrDayName;
    };
    ElementTextHelper.prototype.getLongestText = function (texts) {
        var _this = this;
        var result = "";
        var longestTextWidth = 0;
        texts.forEach(function (text) {
            var textWidth = _this.getTextWidth(text);
            if (textWidth > longestTextWidth) {
                longestTextWidth = textWidth;
                result = text;
            }
        });
        return result;
    };
    ElementTextHelper.prototype.getTextWidth = function (text) {
        return Math.round(this.textMeasureContext.measureText(text).width);
    };
    ElementTextHelper.prototype.getAmText = function () {
        return "AM";
    };
    ElementTextHelper.prototype.getPmText = function () {
        return "PM";
    };
    ElementTextHelper.prototype.getQuarterNames = function () {
        return ["Q1", "Q2", "Q3", "Q4"];
    };
    ElementTextHelper.prototype.getMonthNames = function () {
        return ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""];
    };
    ElementTextHelper.prototype.getDayNames = function () {
        return ["Sunday", "Monday", "ASP", "Wednesday", "Thursday", "Friday", "Saturday"];
    };
    ElementTextHelper.prototype.getAbbrMonthNames = function () {
        return ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""];
    };
    ElementTextHelper.prototype.getAbbrDayNames = function () {
        return ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    };
    return ElementTextHelper;
}());
exports.ElementTextHelper = ElementTextHelper;
function createViewTypeToScaleMap() {
    var result = new Map();
    result.set(Enums_1.ViewType.TenMinutes, Enums_1.ViewType.Hours);
    result.set(Enums_1.ViewType.Hours, Enums_1.ViewType.Days);
    result.set(Enums_1.ViewType.SixHours, Enums_1.ViewType.Days);
    result.set(Enums_1.ViewType.Days, Enums_1.ViewType.Weeks);
    result.set(Enums_1.ViewType.Weeks, Enums_1.ViewType.Months);
    result.set(Enums_1.ViewType.Months, Enums_1.ViewType.Years);
    result.set(Enums_1.ViewType.Quarter, Enums_1.ViewType.Years);
    result.set(Enums_1.ViewType.Years, null);
    return result;
}


/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Point_1 = __webpack_require__(3);
var Size_1 = __webpack_require__(15);
var Margins_1 = __webpack_require__(53);
var GridElementInfo = (function () {
    function GridElementInfo(className, position, size) {
        this.position = new Point_1.Point();
        this.size = new Size_1.Size();
        this.margins = new Margins_1.Margins();
        this.attr = new Map();
        if (className)
            this.className = className;
        if (position)
            this.setPosition(position);
        if (size)
            this.setSize(size);
    }
    GridElementInfo.prototype.setSize = function (size) {
        this.size.width = size.width;
        this.size.height = size.height;
    };
    GridElementInfo.prototype.setPosition = function (position) {
        this.position.x = position.x;
        this.position.y = position.y;
    };
    GridElementInfo.prototype.assignToElement = function (element) {
        this.assignPosition(element);
        this.assignSize(element);
        this.assignMargins(element);
        if (this.className)
            element.className = this.className;
    };
    GridElementInfo.prototype.assignPosition = function (element) {
        if (this.position.x != null)
            element.style.left = this.position.x + "px";
        if (this.position.y != null)
            element.style.top = this.position.y + "px";
    };
    GridElementInfo.prototype.assignSize = function (element) {
        if (this.size.width)
            element.style.width = this.size.width + "px";
        if (this.size.height)
            element.style.height = this.size.height + "px";
    };
    GridElementInfo.prototype.assignMargins = function (element) {
        if (this.margins.marginLeft)
            element.style.marginLeft = this.margins.marginLeft + "px";
        if (this.margins.marginTop)
            element.style.marginTop = this.margins.marginTop + "px";
        if (this.margins.marginRight)
            element.style.marginRight = this.margins.marginRight + "px";
        if (this.margins.marginBottom)
            element.style.marginBottom = this.margins.marginBottom + "px";
    };
    GridElementInfo.prototype.setAttribute = function (name, value) {
        this.attr.set(name, value);
    };
    return GridElementInfo;
}());
exports.GridElementInfo = GridElementInfo;


/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var TaskAreaManager_1 = __webpack_require__(8);
var Enums_1 = __webpack_require__(7);
var DateRange_1 = __webpack_require__(10);
var TaskEditController = (function () {
    function TaskEditController(gantt) {
        this.taskIndex = -1;
        this.successorIndex = -1;
        this.editing = false;
        this.gantt = gantt;
        this.createElements();
        this.createclassToSourceMap();
        this.attachEvents();
    }
    Object.defineProperty(TaskEditController.prototype, "taskId", {
        get: function () {
            return this.gantt.viewModel.items[this.taskIndex].task.internalId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskEditController.prototype, "successorId", {
        get: function () {
            return this.gantt.viewModel.items[this.successorIndex].task.internalId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskEditController.prototype, "task", {
        get: function () {
            return this.gantt.viewModel.items[this.taskIndex].task;
        },
        enumerable: true,
        configurable: true
    });
    TaskEditController.prototype.show = function (taskIndex) {
        if (!this.editing && this.gantt.settings.editing.enabled) {
            this.taskIndex = taskIndex;
            this.updateWrapInfo();
            this.wrapInfo.assignPosition(this.baseElement);
            this.wrapInfo.assignSize(this.baseElement);
            this.gantt.taskArea.appendChild(this.baseElement);
            this.baseElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_BOX;
            if (this.task.isMilestone())
                this.baseElement.className = this.baseElement.className + " milestone";
            else {
                if (!this.gantt.settings.editing.allowDependencyAdding)
                    this.baseElement.className = this.baseElement.className + " hide-dependency";
                if (!this.gantt.settings.editing.allowTaskUpdating)
                    this.baseElement.className = this.baseElement.className + " hide-updating";
                this.progressEdit.style.left = ((this.task.progress / 100) *
                    this.wrapInfo.size.width - (this.progressEdit.offsetWidth / 2)) + "px";
            }
            this.taskDateRange = new DateRange_1.DateRange(this.task.start, this.task.end);
        }
    };
    TaskEditController.prototype.updateWrapInfo = function () {
        this.wrapInfo = this.gantt.gridLayoutCalculator.getTaskWrapperElementInfo(this.taskIndex);
        this.wrapInfo.size.width = this.gantt.gridLayoutCalculator.getTaskWidth(this.taskIndex);
        this.wrapInfo.position.x--;
    };
    TaskEditController.prototype.hide = function () {
        var parentNode = this.baseElement.parentNode;
        if (parentNode)
            parentNode.removeChild(this.baseElement);
    };
    TaskEditController.prototype.showDependencySuccessor = function (taskIndex) {
        if (this.successorIndex != taskIndex && this.taskIndex != taskIndex) {
            this.successorIndex = taskIndex;
            var wrapInfo = this.gantt.gridLayoutCalculator.getTaskWrapperElementInfo(taskIndex);
            wrapInfo.size.width = this.gantt.gridLayoutCalculator.getTaskWidth(taskIndex) + 1;
            wrapInfo.assignPosition(this.dependencySuccessorBaseElement);
            wrapInfo.assignSize(this.dependencySuccessorBaseElement);
            wrapInfo.assignSize(this.dependencySuccessorFrame);
            this.gantt.taskArea.appendChild(this.dependencySuccessorBaseElement);
        }
    };
    TaskEditController.prototype.hideDependencySuccessor = function () {
        var parentNode = this.dependencySuccessorBaseElement.parentNode;
        if (parentNode)
            parentNode.removeChild(this.dependencySuccessorBaseElement);
        this.successorIndex = -1;
    };
    TaskEditController.prototype.processProgress = function (position) {
        var _this = this;
        this.editing = true;
        var progressOffset = position.x - this.wrapInfo.position.x;
        var progress = 0;
        if (position.x > this.wrapInfo.position.x)
            if (position.x < this.wrapInfo.position.x + this.wrapInfo.size.width)
                progress = Math.round((progressOffset) / this.baseElement.clientWidth * 100);
            else
                progress = 100;
        this.progressEdit.style.left = ((progress / 100) *
            this.wrapInfo.size.width - (this.progressEdit.offsetWidth / 2)) + "px";
        this.statusMsg.style.display = "block";
        this.statusMsg.style.right = "";
        this.statusMsg.style.left = this.progressEdit.offsetLeft - this.statusMsg.clientWidth / 2 + "px";
        this.statusMsg.innerHTML = progress + "%";
        if (this.statusTimerId)
            clearTimeout(this.statusTimerId);
        this.statusTimerId = setTimeout(function () {
            _this.statusMsg.style.display = "none";
        }, 1500);
    };
    TaskEditController.prototype.confirmProgress = function () {
        this.editing = false;
        var progress = Math.round((this.progressEdit.offsetLeft + (this.progressEdit.offsetWidth / 2)) / this.wrapInfo.size.width * 100);
        this.gantt.commandManager.changeTaskProgressCommand.execute(this.taskId, progress);
    };
    TaskEditController.prototype.isShouldProcessEnd = function (positionX) {
        return positionX > this.wrapInfo.position.x - this.endEdit.clientWidth;
    };
    TaskEditController.prototype.processEnd = function (position) {
        this.baseElement.className = this.baseElement.className + " move";
        this.editing = true;
        var positionX = position.x > this.wrapInfo.position.x ? position.x : this.wrapInfo.position.x;
        var width = positionX - this.wrapInfo.position.x;
        this.baseElement.style.width = (width < 1 ? 0 : width) + "px";
        var startDate = this.task.start;
        var date = this.gantt.gridLayoutCalculator.getDateByPos(positionX);
        date.setSeconds(0);
        if (date < startDate || width < 1)
            this.taskDateRange.end.setTime(startDate.getTime());
        else
            this.taskDateRange.end = this.getNewDate(this.task.end, date);
        this.statusMsg.style.left = "";
        this.statusMsg.style.right = "0px";
        this.showTimeMessage(startDate, this.taskDateRange.end);
    };
    TaskEditController.prototype.confirmEnd = function () {
        this.baseElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_BOX;
        this.editing = false;
        this.gantt.commandManager.changeTaskEndCommand.execute(this.taskId, this.taskDateRange.end);
        this.updateWrapInfo();
    };
    TaskEditController.prototype.processStart = function (position) {
        this.baseElement.className = this.baseElement.className + " move";
        this.editing = true;
        var positionX = position.x < this.wrapInfo.position.x + this.wrapInfo.size.width ? position.x : this.wrapInfo.position.x + this.wrapInfo.size.width;
        var width = this.wrapInfo.size.width - (positionX - this.wrapInfo.position.x);
        this.baseElement.style.left = positionX + "px";
        this.baseElement.style.width = (width < 1 ? 0 : width) + "px";
        var endDate = this.task.end;
        var date = this.gantt.gridLayoutCalculator.getDateByPos(positionX);
        date.setSeconds(0);
        if (date > endDate || width < 1)
            this.taskDateRange.start.setTime(endDate.getTime());
        else
            this.taskDateRange.start = this.getNewDate(this.task.start, date);
        this.statusMsg.style.right = "";
        this.statusMsg.style.left = "0px";
        this.showTimeMessage(this.taskDateRange.start, endDate);
    };
    TaskEditController.prototype.confirmStart = function () {
        this.baseElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_BOX;
        this.editing = false;
        this.gantt.commandManager.changeTaskStartCommand.execute(this.taskId, this.taskDateRange.start);
        this.updateWrapInfo();
    };
    TaskEditController.prototype.processMove = function (delta) {
        if (this.gantt.settings.editing.allowTaskUpdating) {
            this.baseElement.className = this.baseElement.className + " move";
            this.editing = true;
            var left = this.baseElement.offsetLeft + delta;
            this.baseElement.style.left = left + "px";
            var date = this.gantt.gridLayoutCalculator.getDateByPos(left);
            this.taskDateRange.start = this.getNewDate(this.task.start, date);
            var dateDiff = this.task.start.getTime() - date.getTime();
            var endDate = new Date(this.task.end.getTime() - dateDiff);
            this.taskDateRange.end = this.getNewDate(this.task.end, endDate);
            this.showTimeMessage(this.taskDateRange.start, this.taskDateRange.end);
        }
    };
    TaskEditController.prototype.confirmMove = function () {
        this.baseElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_BOX;
        if (this.editing) {
            this.gantt.history.beginTransaction();
            this.gantt.commandManager.changeTaskStartCommand.execute(this.taskId, this.taskDateRange.start);
            this.gantt.commandManager.changeTaskEndCommand.execute(this.taskId, this.taskDateRange.end);
            this.gantt.history.endTransaction();
        }
        this.updateWrapInfo();
        this.editing = false;
    };
    TaskEditController.prototype.getNewDate = function (referenceDate, newDate) {
        if (this.gantt.settings.viewType > Enums_1.ViewType.SixHours) {
            var date = new Date(referenceDate.getTime());
            date.setDate(newDate.getDate());
            date.setMonth(newDate.getMonth());
            date.setFullYear(newDate.getFullYear());
            if (this.gantt.settings.viewType == Enums_1.ViewType.Days)
                date.setHours(newDate.getHours());
            return date;
        }
        else
            return newDate;
    };
    TaskEditController.prototype.startDependency = function (pos) {
        this.dependencyLine = document.createElement("DIV");
        this.dependencyLine.className = TaskEditController.CLASSNAMES.TASK_EDIT_DEPENDENCY_LINE;
        this.gantt.taskArea.appendChild(this.dependencyLine);
        this.startPosition = pos;
    };
    TaskEditController.prototype.processDependency = function (pos) {
        this.editing = true;
        this.drawline(this.startPosition, pos);
    };
    TaskEditController.prototype.endDependency = function (type) {
        this.editing = false;
        if (type != null)
            this.gantt.commandManager.createDependencyCommand.execute(this.task.internalId, this.successorId, type);
        var parentNode = this.dependencyLine.parentNode;
        if (parentNode)
            parentNode.removeChild(this.dependencyLine);
        this.dependencyLine = null;
        this.hideDependencySuccessor();
        this.hide();
    };
    TaskEditController.prototype.selectDependency = function (id) {
        if (this.gantt.settings.editing.allowDependencyDeleting)
            this.dependencyId = id;
    };
    TaskEditController.prototype.isDependencySelected = function (id) {
        return this.dependencyId && this.dependencyId == id;
    };
    TaskEditController.prototype.deleteSelectedDependency = function () {
        if (this.dependencyId)
            this.gantt.commandManager.removeDependencyCommand.execute(this.dependencyId);
    };
    TaskEditController.prototype.showTimeMessage = function (start, end) {
        var _this = this;
        this.statusMsg.innerHTML = "";
        var timeElement = document.createElement("DIV");
        timeElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_PROGRESS_STATUS_TIME;
        var startEl = document.createElement("DIV");
        var startTitle = document.createElement("SPAN");
        startTitle.innerHTML = "Start: ";
        var startValue = document.createElement("SPAN");
        startValue.innerHTML = this.formatDate(start);
        startEl.appendChild(startTitle);
        startEl.appendChild(startValue);
        var endEl = document.createElement("DIV");
        var endTitle = document.createElement("SPAN");
        endTitle.innerHTML = "End: ";
        var endValue = document.createElement("SPAN");
        endValue.innerHTML = this.formatDate(end);
        endEl.appendChild(endTitle);
        endEl.appendChild(endValue);
        timeElement.appendChild(startEl);
        timeElement.appendChild(endEl);
        this.statusMsg.appendChild(timeElement);
        this.statusMsg.style.display = "block";
        if (this.statusTimerId)
            clearTimeout(this.statusTimerId);
        this.statusTimerId = setTimeout(function () {
            _this.statusMsg.style.display = "none";
        }, 1500);
    };
    TaskEditController.prototype.formatDate = function (date) {
        return ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + "/" +
            date.getFullYear() + " " + ('0' + date.getHours()).slice(-2) + ":" + ('0' + date.getMinutes()).slice(-2);
    };
    TaskEditController.prototype.createElements = function () {
        this.baseElement = document.createElement("DIV");
        this.baseFrame = document.createElement("DIV");
        this.baseFrame.className = TaskEditController.CLASSNAMES.TASK_EDIT_FRAME;
        this.baseElement.appendChild(this.baseFrame);
        this.progressEdit = document.createElement("DIV");
        this.progressEdit.className = TaskEditController.CLASSNAMES.TASK_EDIT_PROGRESS;
        this.baseFrame.appendChild(this.progressEdit);
        this.statusMsg = document.createElement("DIV");
        this.statusMsg.className = TaskEditController.CLASSNAMES.TASK_EDIT_PROGRESS_STATUS;
        this.baseFrame.appendChild(this.statusMsg);
        this.dependencyFinish = document.createElement("DIV");
        this.dependencyFinish.className = TaskEditController.CLASSNAMES.TASK_EDIT_DEPENDENCY_RIGTH;
        this.baseFrame.appendChild(this.dependencyFinish);
        this.dependencyStart = document.createElement("DIV");
        this.dependencyStart.className = TaskEditController.CLASSNAMES.TASK_EDIT_DEPENDENCY_LEFT;
        this.baseFrame.appendChild(this.dependencyStart);
        this.startEdit = document.createElement("DIV");
        this.startEdit.className = TaskEditController.CLASSNAMES.TASK_EDIT_START;
        this.baseFrame.appendChild(this.startEdit);
        this.endEdit = document.createElement("DIV");
        this.endEdit.className = TaskEditController.CLASSNAMES.TASK_EDIT_END;
        this.baseFrame.appendChild(this.endEdit);
        this.dependencySuccessorBaseElement = document.createElement("DIV");
        this.dependencySuccessorBaseElement.className = TaskEditController.CLASSNAMES.TASK_EDIT_BOX_SUCCESSOR;
        this.dependencySuccessorFrame = document.createElement("DIV");
        this.dependencySuccessorFrame.className = TaskEditController.CLASSNAMES.TASK_EDIT_FRAME_SUCCESSOR;
        this.dependencySuccessorBaseElement.appendChild(this.dependencySuccessorFrame);
        this.dependencySuccessorStart = document.createElement("DIV");
        this.dependencySuccessorStart.className = TaskEditController.CLASSNAMES.TASK_EDIT_SUCCESSOR_DEPENDENCY_RIGTH;
        this.dependencySuccessorFrame.appendChild(this.dependencySuccessorStart);
        this.dependencySuccessorFinish = document.createElement("DIV");
        this.dependencySuccessorFinish.className = TaskEditController.CLASSNAMES.TASK_EDIT_SUCCESSOR_DEPENDENCY_LEFT;
        this.dependencySuccessorFrame.appendChild(this.dependencySuccessorFinish);
    };
    TaskEditController.prototype.attachEvents = function () {
        this.baseElement.addEventListener("mouseleave", function (evt) {
            if (!this.editing)
                this.hide();
        }.bind(this));
    };
    TaskEditController.prototype.drawline = function (start, end) {
        if (start.x > end.x) {
            var temp = end;
            end = start;
            start = temp;
        }
        var angle = Math.atan((start.y - end.y) / (end.x - start.x));
        angle = (angle * 180 / Math.PI);
        angle = -angle;
        var length = Math.sqrt((start.x - end.x) * (start.x - end.x) + (start.y - end.y) * (start.y - end.y));
        this.dependencyLine.style.left = start.x + "px";
        this.dependencyLine.style.top = start.y + "px";
        this.dependencyLine.style.width = length + "px";
        this.dependencyLine.style.transform = "rotate(" + angle + "deg)";
    };
    TaskEditController.prototype.createclassToSourceMap = function () {
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_PROGRESS, TaskAreaManager_1.MouseEventSource.TaskEdit_Progress);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_START, TaskAreaManager_1.MouseEventSource.TaskEdit_Start);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_END, TaskAreaManager_1.MouseEventSource.TaskEdit_End);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_FRAME, TaskAreaManager_1.MouseEventSource.TaskEdit_Frame);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_DEPENDENCY_RIGTH, TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyStart);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_DEPENDENCY_LEFT, TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyFinish);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_SUCCESSOR_DEPENDENCY_RIGTH, TaskAreaManager_1.MouseEventSource.Successor_DependencyStart);
        TaskEditController.classToSource.set(TaskEditController.CLASSNAMES.TASK_EDIT_SUCCESSOR_DEPENDENCY_LEFT, TaskAreaManager_1.MouseEventSource.Successor_DependencyFinish);
    };
    TaskEditController.CLASSNAMES = {
        TASK_EDIT_BOX: "dx-gantt-task-edit-wrapper",
        TASK_EDIT_FRAME: "dx-gantt-task-edit-frame",
        TASK_EDIT_PROGRESS: "dx-gantt-task-edit-progress",
        TASK_EDIT_PROGRESS_STATUS: "dx-gantt-task-edit-progress-status",
        TASK_EDIT_PROGRESS_STATUS_TIME: "dx-gantt-status-time",
        TASK_EDIT_DEPENDENCY_RIGTH: "dx-gantt-task-edit-dependency-r",
        TASK_EDIT_DEPENDENCY_LEFT: "dx-gantt-task-edit-dependency-l",
        TASK_EDIT_START: "dx-gantt-task-edit-start",
        TASK_EDIT_END: "dx-gantt-task-edit-end",
        TASK_EDIT_DEPENDENCY_LINE: "dx-gantt-task-edit-dependency-line",
        TASK_EDIT_BOX_SUCCESSOR: "dx-gantt-task-edit-wrapper-successor",
        TASK_EDIT_FRAME_SUCCESSOR: "dx-gantt-task-edit-frame-successor",
        TASK_EDIT_SUCCESSOR_DEPENDENCY_RIGTH: "dx-gantt-task-edit-successor-dependency-r",
        TASK_EDIT_SUCCESSOR_DEPENDENCY_LEFT: "dx-gantt-task-edit-successor-dependency-l"
    };
    TaskEditController.classToSource = new Map();
    return TaskEditController;
}());
exports.TaskEditController = TaskEditController;


/***/ }),
/* 30 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem_1 = __webpack_require__(9);
var TaskPropertiesHistoryItemBase = (function (_super) {
    __extends(TaskPropertiesHistoryItemBase, _super);
    function TaskPropertiesHistoryItemBase(modelManipulator, taskId, newValue) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.taskId = taskId;
        _this.newValue = newValue;
        return _this;
    }
    TaskPropertiesHistoryItemBase.prototype.redo = function () {
        this.oldState = this.getPropertiesManipulator().setValue(this.taskId, this.newValue);
    };
    TaskPropertiesHistoryItemBase.prototype.undo = function () {
        this.getPropertiesManipulator().restoreValue(this.oldState);
    };
    TaskPropertiesHistoryItemBase.prototype.getPropertiesManipulator = function () {
        throw new Error("Not Implemented");
    };
    return TaskPropertiesHistoryItemBase;
}(HistoryItem_1.HistoryItem));
exports.TaskPropertiesHistoryItemBase = TaskPropertiesHistoryItemBase;
var TaskTitleHistoryItem = (function (_super) {
    __extends(TaskTitleHistoryItem, _super);
    function TaskTitleHistoryItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskTitleHistoryItem.prototype.getPropertiesManipulator = function () {
        return this.modelManipulator.task.properties.title;
    };
    return TaskTitleHistoryItem;
}(TaskPropertiesHistoryItemBase));
exports.TaskTitleHistoryItem = TaskTitleHistoryItem;
var TaskDesriptionHistoryItem = (function (_super) {
    __extends(TaskDesriptionHistoryItem, _super);
    function TaskDesriptionHistoryItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskDesriptionHistoryItem.prototype.getPropertiesManipulator = function () {
        return this.modelManipulator.task.properties.description;
    };
    return TaskDesriptionHistoryItem;
}(TaskPropertiesHistoryItemBase));
exports.TaskDesriptionHistoryItem = TaskDesriptionHistoryItem;
var TaskProgressHistoryItem = (function (_super) {
    __extends(TaskProgressHistoryItem, _super);
    function TaskProgressHistoryItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskProgressHistoryItem.prototype.getPropertiesManipulator = function () {
        return this.modelManipulator.task.properties.progress;
    };
    return TaskProgressHistoryItem;
}(TaskPropertiesHistoryItemBase));
exports.TaskProgressHistoryItem = TaskProgressHistoryItem;
var TaskStartHistoryItem = (function (_super) {
    __extends(TaskStartHistoryItem, _super);
    function TaskStartHistoryItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskStartHistoryItem.prototype.getPropertiesManipulator = function () {
        return this.modelManipulator.task.properties.start;
    };
    return TaskStartHistoryItem;
}(TaskPropertiesHistoryItemBase));
exports.TaskStartHistoryItem = TaskStartHistoryItem;
var TaskEndHistoryItem = (function (_super) {
    __extends(TaskEndHistoryItem, _super);
    function TaskEndHistoryItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskEndHistoryItem.prototype.getPropertiesManipulator = function () {
        return this.modelManipulator.task.properties.end;
    };
    return TaskEndHistoryItem;
}(TaskPropertiesHistoryItemBase));
exports.TaskEndHistoryItem = TaskEndHistoryItem;


/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
__webpack_require__(32);
var GanttView_1 = __webpack_require__(33);
exports.GanttView = GanttView_1.GanttView;


/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

// extracted by mini-css-extract-plugin

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var VisualModel_1 = __webpack_require__(34);
var Enums_1 = __webpack_require__(7);
var Utils_1 = __webpack_require__(27);
var DomUtils_1 = __webpack_require__(2);
var DateRange_1 = __webpack_require__(10);
var Size_1 = __webpack_require__(15);
var Point_1 = __webpack_require__(3);
var GridElementInfo_1 = __webpack_require__(28);
var GridLayoutCalculator_1 = __webpack_require__(54);
var EtalonSizeValues_1 = __webpack_require__(55);
var TaskEditController_1 = __webpack_require__(29);
var TaskAreaManager_1 = __webpack_require__(8);
var ModelManipulator_1 = __webpack_require__(56);
var History_1 = __webpack_require__(61);
var EventManager_1 = __webpack_require__(62);
var TaskAreaContainer_1 = __webpack_require__(72);
var Settings_1 = __webpack_require__(73);
var ModelChangesDispatcher_1 = __webpack_require__(74);
var CommandManager_1 = __webpack_require__(76);
var BarManager_1 = __webpack_require__(86);
var GanttView = (function () {
    function GanttView(element, ganttOwner, settings) {
        this.timeScaleAreas = new Array();
        this.horTaskAreaBorders = [];
        this.vertTaskAreaBorders = [];
        this.scaleBorders = [];
        this.scaleElements = [];
        this.taskElements = [];
        this.selectionElements = [];
        this.hlRowElements = [];
        this.renderedRowIndices = [];
        this.renderedColIndices = [];
        this.renderedScaleItemIndices = [];
        this.dependencyMap = [];
        this.renderedConnectorLines = [];
        this.connectorLinesToElementsMap = new Map();
        this.noWorkingIntervalsToElementsMap = new Map();
        this.renderedNoWorkingIntervals = [];
        this.gridLayoutCalculator = new GridLayoutCalculator_1.GridLayoutCalculator();
        this.elementTextHelper = new Utils_1.ElementTextHelper();
        this.etalonSizeValues = new EtalonSizeValues_1.EtalonSizeValues();
        this.tickSize = new Size_1.Size();
        this.scaleCount = 2;
        this.isFocus = false;
        this.zoom = 1;
        this.currentSelectedTaskID = "";
        this.ganttOwner = ganttOwner;
        this.settings = Settings_1.Settings.parse(settings);
        this.createMainElement(element);
        this.createHeader();
        this.createTaskAreaContainer();
        this.calculateEtalonSizeValues();
        this.loadOptionsFromGanttOwner();
        this.elementTextHelper.setFont(this.mainElement);
        this.setupHelpers();
        this.setSizeForTaskArea();
        this.createTimeScaleContainer();
        this.createTimeScaleAreas();
        this.commandManager = new CommandManager_1.CommandManager(this);
        this.barManager = new BarManager_1.BarManager(this, ganttOwner.bars);
        this.eventManager = new EventManager_1.EventManager(this);
        this.taskEditController = new TaskEditController_1.TaskEditController(this);
        this.history = new History_1.History();
        this.taskAreaManager = new TaskAreaManager_1.TaskAreaManager(this);
        this.updateView();
    }
    GanttView.prototype.reset = function () {
        this.timeScaleContainer.innerHTML = "";
        this.taskArea.innerHTML = "";
        this.horTaskAreaBorders = [];
        this.vertTaskAreaBorders = [];
        this.scaleBorders = [];
        this.scaleElements = [];
        this.taskElements = [];
        this.selectionElements = [];
        this.hlRowElements = [];
        this.renderedRowIndices = [];
        this.renderedColIndices = [];
        this.renderedConnectorLines = [];
        this.timeScaleAreas = [];
        this.renderedScaleItemIndices = [];
        this.connectorLinesToElementsMap.clear();
        this.renderedNoWorkingIntervals = [];
        this.noWorkingIntervalsToElementsMap.clear();
    };
    GanttView.prototype.setupHelpers = function () {
        var size = new Size_1.Size(this.taskAreaContainer.getWidth(), this.taskAreaContainer.getHeight());
        this.gridLayoutCalculator.setSettings(size, this.tickSize, this.etalonSizeValues, this.range, this.viewModel, this.settings.viewType);
        this.elementTextHelper.setSettings(this.range.start.getTime(), this.settings.viewType, this.viewModel.items);
    };
    GanttView.prototype.getDateRange = function (modelStartDate, modelEndDate) {
        var adjustedStartDate = Utils_1.DateUtils.adjustStartEndDateByViewType(modelStartDate, true, this.settings.viewType);
        var adjustedEndDate = this.adjustEndDateByVisibleAreaWidth(adjustedStartDate, modelEndDate);
        adjustedEndDate = Utils_1.DateUtils.adjustStartEndDateByViewType(adjustedEndDate, false, this.settings.viewType);
        return new DateRange_1.DateRange(adjustedStartDate, adjustedEndDate);
    };
    GanttView.prototype.adjustEndDateByVisibleAreaWidth = function (adjustedStartDate, modelEndDate) {
        var result = new Date();
        var endTime = Math.max(modelEndDate.getTime(), this.getVisibleAreaEndDate(adjustedStartDate).getTime());
        result.setTime(endTime);
        return result;
    };
    GanttView.prototype.getVisibleAreaEndDate = function (visibleAreaStartDate) {
        var result = new Date();
        var visibleTickCount = Math.ceil(this.taskAreaContainer.getWidth() / this.tickSize.width);
        if (this.settings.viewType == Enums_1.ViewType.Months) {
            result.setTime(visibleAreaStartDate.getTime());
            result.setMonth(result.getMonth() + visibleTickCount);
        }
        else
            result.setTime(visibleAreaStartDate.getTime() + visibleTickCount * Utils_1.DateUtils.getTickTimeSpan(this.settings.viewType));
        return result;
    };
    GanttView.prototype.calculateEtalonSizeValues = function () {
        var etalonElementsContainer = this.createEtalonElementsContainer();
        var etalonElements = this.createEtalonElements(etalonElementsContainer);
        this.calculateEtalonSizeValuesCore(etalonElements);
        this.mainElement.removeChild(etalonElementsContainer);
    };
    GanttView.prototype.calculateEtalonSizeValuesCore = function (etalonElements) {
        this.etalonSizeValues.taskHeight = etalonElements[0].firstChild.offsetHeight;
        this.etalonSizeValues.milestoneWidth = etalonElements[1].firstChild.offsetWidth;
        this.etalonSizeValues.taskWrapperTopPadding = DomUtils_1.DomUtils.getTopPaddings(etalonElements[0]);
        this.etalonSizeValues.connectorLineThickness = DomUtils_1.DomUtils.getVerticalBordersWidth(etalonElements[2]);
        this.etalonSizeValues.connectorArrowWidth = DomUtils_1.DomUtils.getHorizontalBordersWidth(etalonElements[3]);
        this.etalonSizeValues.scaleItemHeight = etalonElements[4].offsetHeight;
        for (var i = 0; i <= Enums_1.ViewType.Quarter; i++) {
            etalonElements[4].innerText = this.elementTextHelper.getScaleItemTextTemplate(i);
            this.etalonSizeValues.scaleItemWidths.set(i, etalonElements[4].offsetWidth);
        }
        var taskElement = etalonElements[0];
        var taskTitleElement = taskElement.childNodes[0];
        taskElement.style.width = "0px";
        while (taskTitleElement.scrollWidth > taskTitleElement.clientWidth)
            taskElement.style.width = (DomUtils_1.DomUtils.pxToFloat(taskElement.style.width) + 1) + "px";
        this.etalonSizeValues.smallTaskWidth = taskElement.offsetWidth;
        this.etalonSizeValues.outsideTaskTextDefaultWidth = DomUtils_1.DomUtils.pxToFloat(DomUtils_1.DomUtils.getCurrentStyle(etalonElements[5]).width);
    };
    GanttView.prototype.createEtalonElementsContainer = function () {
        var result = document.createElement("DIV");
        result.style.visibility = "hidden";
        result.style.position = "absolute";
        result.style.left = "-1000px";
        this.mainElement.appendChild(result);
        return result;
    };
    GanttView.prototype.createEtalonElements = function (parent) {
        var etalonElements = [];
        var wrapper = this.createElement(new GridElementInfo_1.GridElementInfo("dx-gantt-taskWrapper"), null, parent);
        var task = this.createElement(new GridElementInfo_1.GridElementInfo("dx-gantt-task"), null, wrapper);
        var taskTitle = this.createElement(new GridElementInfo_1.GridElementInfo("dx-gantt-taskTitle dx-gantt-titleIn"), null, task);
        taskTitle.innerText = "WWW";
        etalonElements.push(wrapper);
        var milestoneWrapper = this.createElement(new GridElementInfo_1.GridElementInfo("dx-gantt-taskWrapper"), null, parent);
        this.createElement(new GridElementInfo_1.GridElementInfo("dx-gantt-task dx-gantt-milestone"), null, milestoneWrapper);
        etalonElements.push(milestoneWrapper);
        var etalonElementClassNames = ["dx-gantt-conn-h", "dx-gantt-arrow", "dx-gantt-si", "dx-gantt-taskTitle dx-gantt-titleOut"];
        for (var i = 0; i < etalonElementClassNames.length; i++) {
            var etalonElementInfo = new GridElementInfo_1.GridElementInfo(etalonElementClassNames[i]);
            etalonElements.push(this.createElement(etalonElementInfo, null, parent));
        }
        return etalonElements;
    };
    GanttView.prototype.zoomIn = function () {
        if (this.zoom == 1) {
            this.zoom++;
            this.updateTickSizeWidth();
            this.resetAndUpdate();
        }
        else if (this.settings.viewType > Enums_1.ViewType.TenMinutes) {
            this.zoom--;
            this.setViewType(this.settings.viewType - 1);
        }
    };
    GanttView.prototype.zoomOut = function () {
        if (this.zoom > 1) {
            this.zoom--;
            this.updateTickSizeWidth();
            this.resetAndUpdate();
        }
        else if (this.settings.viewType < Enums_1.ViewType.Quarter) {
            this.zoom++;
            this.setViewType(this.settings.viewType + 1);
        }
    };
    GanttView.prototype.onVisualModelChanged = function () {
        this.resetAndUpdate();
    };
    GanttView.prototype.getGanttViewStartDate = function (tasks) {
        if (!tasks)
            return new Date();
        var dates = tasks.map(function (t) { return typeof t.start == "string" ? new Date(t.start) : t.start; });
        return dates.length > 0 ? dates.reduce(function (min, d) { return d < min ? d : min; }, dates[0]) : new Date();
    };
    GanttView.prototype.getGanttViewEndDate = function (tasks) {
        if (!tasks)
            return new Date();
        var dates = tasks.map(function (t) { return typeof t.end == "string" ? new Date(t.end) : t.end; });
        return dates.length > 0 ? dates.reduce(function (max, d) { return d > max ? d : max; }, dates[0]) : new Date();
    };
    GanttView.prototype.getTaskAreaWidth = function () {
        return this.gridLayoutCalculator.horizontalTickCount * this.tickSize.width;
    };
    GanttView.prototype.getTaskAreaHeight = function () {
        return this.getVisibleTaskCount() * this.tickSize.height;
    };
    GanttView.prototype.getVisibleTaskCount = function () { return this.viewModel.itemCount; };
    GanttView.prototype.getTask = function (index) {
        var item = this.viewModel.items[index];
        return item.task;
    };
    GanttView.prototype.createMainElement = function (parent) {
        this.mainElement = document.createElement("DIV");
        this.mainElement.style.width = parent.offsetWidth + "px";
        this.mainElement.style.height = parent.offsetHeight + "px";
        parent.appendChild(this.mainElement);
    };
    GanttView.prototype.createHeader = function () {
        this.header = document.createElement("DIV");
        this.header.className = "dx-gantt-header";
        this.mainElement.appendChild(this.header);
    };
    GanttView.prototype.createTimeScaleContainer = function () {
        this.timeScaleContainer = document.createElement("DIV");
        this.timeScaleContainer.className = "dx-gantt-tsac";
        this.timeScaleContainer.style.height = this.etalonSizeValues.scaleItemHeight * this.scaleCount + "px";
        this.header.appendChild(this.timeScaleContainer);
    };
    GanttView.prototype.createTimeScaleArea = function () {
        var timeScaleArea = document.createElement("DIV");
        timeScaleArea.className = "dx-gantt-tsa";
        timeScaleArea.style.width = this.getTaskAreaWidth() + "px";
        timeScaleArea.style.height = this.etalonSizeValues.scaleItemHeight + "px";
        this.timeScaleContainer.appendChild(timeScaleArea);
        this.timeScaleAreas.unshift(timeScaleArea);
        return timeScaleArea;
    };
    GanttView.prototype.createTimeScaleAreas = function () {
        for (var i = 0; i < this.scaleCount; i++) {
            var timeScaleArea = this.createTimeScaleArea();
            if (i == 0 && this.settings.viewType == Enums_1.ViewType.Weeks)
                this.createMonthsScale(timeScaleArea);
        }
    };
    GanttView.prototype.createTaskAreaContainer = function () {
        var element = document.createElement("DIV");
        element.className = "dx-gantt-tac";
        this.mainElement.appendChild(element);
        this.createTaskArea(element);
        this.taskAreaContainer = this.ganttOwner.getExternalTaskAreaContainer(element);
        if (this.taskAreaContainer == null)
            this.taskAreaContainer = new TaskAreaContainer_1.TaskAreaContainer(element, this);
        this.setClassesToTaskAreaContainer();
    };
    GanttView.prototype.setClassesToTaskAreaContainer = function () {
        var className = "dx-gantt-tac-hb";
        var element = this.taskAreaContainer.getElement();
        this.settings.areHorizontalBordersEnabled ? element.classList.add(className) : element.classList.remove(className);
    };
    GanttView.prototype.createTaskArea = function (parent) {
        this.taskArea = document.createElement("DIV");
        this.taskArea.id = "dx-gantt-ta";
        parent.appendChild(this.taskArea);
    };
    GanttView.prototype.setSizeForTaskArea = function () {
        this.taskArea.style.width = this.getTaskAreaWidth() + "px";
        this.taskArea.style.height = this.getTaskAreaHeight() + "px";
    };
    GanttView.prototype.updateTickSizeWidth = function () {
        this.tickSize.width = this.etalonSizeValues.scaleItemWidths.get(this.settings.viewType) * this.zoom;
    };
    GanttView.prototype.createMonthsScale = function (parent) {
        var currentDate = new Date();
        currentDate.setTime(this.range.start.getTime());
        var x = 0;
        var index = 0;
        while (currentDate.getTime() < this.range.end.getTime()) {
            var dayInMonthCount = Utils_1.DateUtils.getDaysInMonth(currentDate.getMonth(), currentDate.getFullYear());
            var dayCount = dayInMonthCount - currentDate.getDate() + 1;
            var nextDate = new Date();
            nextDate.setTime(currentDate.getTime() + Math.min(dayCount * Utils_1.DateUtils.msPerDay, this.range.end.getTime() - currentDate.getTime()));
            var width = this.gridLayoutCalculator.getWidthByDateRange(currentDate, nextDate);
            var scaleItemInfo = new GridElementInfo_1.GridElementInfo("dx-gantt-si", new Point_1.Point(x), new Size_1.Size(width));
            var scaleItemElement = this.createElement(scaleItemInfo, null, parent);
            scaleItemElement.innerText = this.elementTextHelper.getScaleItemText(index, Enums_1.ViewType.Months);
            var scaleBorderInfo = new GridElementInfo_1.GridElementInfo("dx-gantt-vb", new Point_1.Point(x + width), new Size_1.Size(0, this.etalonSizeValues.scaleItemHeight));
            this.createElement(scaleBorderInfo, null, parent);
            x += width;
            index++;
            currentDate = nextDate;
        }
    };
    GanttView.prototype.updateView = function () {
        this.timeScaleContainer.scrollLeft = this.taskAreaContainer.scrollLeft;
        this.processScroll(false);
        this.processScroll(true);
        this.ganttOwner.onGanttScroll(this.taskAreaContainer.scrollTop);
    };
    GanttView.prototype.processScroll = function (isVertical) {
        this.recreateTaskAreaBordersAndTaskElements(isVertical);
        if (isVertical)
            this.recreateConnectorLineElements();
        else {
            this.recreateNoWorkingIntervalElements();
            this.recreateScalesElements();
        }
    };
    GanttView.prototype.recreateTaskAreaBordersAndTaskElements = function (isVertical) {
        var _this = this;
        var scrollPos = isVertical ? this.taskAreaContainer.scrollTop : this.taskAreaContainer.scrollLeft;
        var newRenderedIndices = this.gridLayoutCalculator.getRenderedRowColumnIndices(scrollPos, isVertical);
        var renderedIndices = isVertical ? this.renderedRowIndices : this.renderedColIndices;
        this.recreateElements(renderedIndices, newRenderedIndices, function (index) { _this.removeTaskAreaBorderAndTaskElement(index, isVertical); }, function (index) { _this.createTaskAreaBorderAndTaskElement(index, isVertical); });
        if (isVertical)
            this.renderedRowIndices = newRenderedIndices;
        else
            this.renderedColIndices = newRenderedIndices;
    };
    GanttView.prototype.recreateNoWorkingIntervalElements = function () {
        var _this = this;
        var newRenderedNoWorkingIntervals = this.gridLayoutCalculator.getRenderedNoWorkingIntervals(this.taskAreaContainer.scrollLeft);
        this.recreateElements(this.renderedNoWorkingIntervals, newRenderedNoWorkingIntervals, function (info) { _this.removeNoWorkingIntervalElement(info); }, function (info) { _this.createNoWorkingIntervalElement(info); });
        this.renderedNoWorkingIntervals = newRenderedNoWorkingIntervals;
    };
    GanttView.prototype.recreateConnectorLineElements = function () {
        var _this = this;
        var newRenderedConnectorLines = this.gridLayoutCalculator.getRenderedConnectorLines(this.taskAreaContainer.scrollTop);
        this.recreateElements(this.renderedConnectorLines, newRenderedConnectorLines, function (info) { _this.removeConnectorLineElement(info); }, function (info) { _this.createConnectorLineElement(info); });
        this.renderedConnectorLines = newRenderedConnectorLines;
    };
    GanttView.prototype.recreateScalesElements = function () {
        this.recreateScaleElements(this.settings.viewType, 0);
        if (this.settings.viewType != Enums_1.ViewType.Weeks)
            this.recreateScaleElements(Utils_1.DateUtils.ViewTypeToScaleMap.get(this.settings.viewType), 1);
    };
    GanttView.prototype.recreateScaleElements = function (scaleType, scaleIndex) {
        var _this = this;
        var newRenderedIndices = this.gridLayoutCalculator.getRenderedScaleItemIndices(scaleType, this.renderedColIndices);
        var renderedIndices = this.renderedScaleItemIndices[scaleType - this.settings.viewType] || [];
        this.recreateElements(renderedIndices, newRenderedIndices, function (index) { _this.removeScaleElementAndBorder(index, scaleIndex); }, function (index) { _this.createScaleElementAndBorder(index, scaleIndex, scaleType); });
        this.renderedScaleItemIndices[scaleType - this.settings.viewType] = newRenderedIndices;
    };
    GanttView.prototype.recreateElements = function (oldRenderedElementsInfo, newRenderedelementsInfo, removeAction, createAction) {
        oldRenderedElementsInfo
            .filter(function (info) { return newRenderedelementsInfo.indexOf(info) === -1; })
            .forEach(function (info) { removeAction(info); });
        newRenderedelementsInfo
            .filter(function (info) { return oldRenderedElementsInfo.indexOf(info) === -1; })
            .forEach(function (info) { createAction(info); });
    };
    GanttView.prototype.allowTaskAreaBorders = function (isVerticalScroll) {
        return isVerticalScroll ? this.settings.areHorizontalBordersEnabled : this.settings.areVerticalBordersEnabled;
    };
    GanttView.prototype.createTaskAreaBorderAndTaskElement = function (index, isVerticalScroll) {
        if (this.allowTaskAreaBorders(isVerticalScroll))
            this.createTaskAreaBorder(index, !isVerticalScroll);
        if (isVerticalScroll)
            this.createTaskElement(index);
    };
    GanttView.prototype.removeTaskAreaBorderAndTaskElement = function (index, isVerticalScroll) {
        if (this.allowTaskAreaBorders(isVerticalScroll))
            this.removeTaskAreaBorder(index, !isVerticalScroll);
        if (isVerticalScroll)
            this.removeTaskElement(index);
    };
    GanttView.prototype.getTaskAreaBordersDictionary = function (isVertical) {
        return isVertical ? this.vertTaskAreaBorders : this.horTaskAreaBorders;
    };
    GanttView.prototype.createTaskAreaBorder = function (index, isVertical) {
        var info = this.gridLayoutCalculator.getTaskAreaBorderInfo(index, isVertical);
        this.createElement(info, index, this.taskArea, this.getTaskAreaBordersDictionary(isVertical));
    };
    GanttView.prototype.removeTaskAreaBorder = function (index, isVertical) {
        this.removeElement(null, index, this.taskArea, this.getTaskAreaBordersDictionary(isVertical));
    };
    GanttView.prototype.createScaleElementAndBorder = function (index, scaleIndex, scaleType) {
        this.createScaleElement(index, scaleIndex, scaleType);
        this.createScaleBorder(index, scaleIndex, scaleType);
    };
    GanttView.prototype.createScaleElement = function (index, scaleIndex, scaleType) {
        var info = this.gridLayoutCalculator.getScaleElementInfo(index, scaleType);
        var scaleElement = this.createScaleElementCore(index, info, scaleIndex, this.scaleElements);
        scaleElement.innerText = this.elementTextHelper.getScaleItemText(index, scaleType);
    };
    GanttView.prototype.createScaleBorder = function (index, scaleIndex, scaleType) {
        var info = this.gridLayoutCalculator.getScaleBorderInfo(index, scaleType);
        this.createScaleElementCore(index, info, scaleIndex, this.scaleBorders);
    };
    GanttView.prototype.createScaleElementCore = function (index, info, scaleIndex, dictionary) {
        if (!dictionary[scaleIndex])
            dictionary[scaleIndex] = [];
        return this.createElement(info, index, this.timeScaleAreas[scaleIndex], dictionary[scaleIndex]);
    };
    GanttView.prototype.removeScaleElementAndBorder = function (index, scaleIndex) {
        this.removeElement(null, index, this.timeScaleAreas[scaleIndex], this.scaleElements[scaleIndex]);
        this.removeElement(null, index, this.timeScaleAreas[scaleIndex], this.scaleBorders[scaleIndex]);
    };
    GanttView.prototype.createTaskElement = function (index) {
        this.createTaskWrapperElement(index);
        if (this.settings.taskTitlePosition == Enums_1.TaskTitlePosition.Outside)
            this.createTaskTextElement(index, this.taskElements[index]);
        var taskVisualElement = this.createTaskVisualElement(index);
        if (!this.viewModel.items[index].task.isMilestone()) {
            if (this.settings.taskTitlePosition == Enums_1.TaskTitlePosition.Inside)
                this.createTaskTextElement(index, taskVisualElement);
            this.createTaskProgressElement(index, taskVisualElement);
        }
        if (this.settings.showResources)
            this.createResources(index);
        if (this.viewModel.items[index].selected)
            this.createTaskSelectionElement(index);
        if (this.isHighlightRowElementAllowed(index))
            this.createHighlightRowElement(index);
    };
    GanttView.prototype.isHighlightRowElementAllowed = function (index) {
        return index % 2 !== 0 && this.settings.areAlternateRowsEnabled || this.viewModel.items[index].children.length > 0;
    };
    GanttView.prototype.createResources = function (index) {
        var resources = this.viewModel.items[index].resources.items;
        for (var i = 0; i < resources.length; i++)
            this.createResourceElement(index, resources[i]);
    };
    GanttView.prototype.createTaskWrapperElement = function (index) {
        var taskWrapperInfo = this.gridLayoutCalculator.getTaskWrapperElementInfo(index);
        this.createElement(taskWrapperInfo, index, this.taskArea, this.taskElements);
        this.taskElements[index].style.display = this.elementTextHelper.getTaskVisibility(index) ? "" : "none";
    };
    GanttView.prototype.createTaskVisualElement = function (index) {
        var taskElementInfo = this.gridLayoutCalculator.getTaskElementInfo(index);
        var taskElement = this.createElement(taskElementInfo, index, this.taskElements[index]);
        taskElement.addEventListener("mouseenter", function (evt) { this.taskAreaManager.onTaskElementHover(evt); }.bind(this));
        return taskElement;
    };
    GanttView.prototype.createTaskProgressElement = function (index, parent) {
        var taskProgressInfo = this.gridLayoutCalculator.getTaskProgressElementInfo(index);
        this.createElement(taskProgressInfo, index, parent);
    };
    GanttView.prototype.createTaskTextElement = function (index, parent) {
        var taskTextInfo = this.gridLayoutCalculator.getTaskTextElementInfo(index, this.settings.taskTitlePosition == Enums_1.TaskTitlePosition.Inside);
        var taskTextElement = this.createElement(taskTextInfo, index, parent);
        taskTextElement.innerText = this.elementTextHelper.getTaskText(index);
    };
    GanttView.prototype.createResourceElement = function (index, resource) {
        var resourceElementInfo = this.gridLayoutCalculator.getTaskResourceElementInfo();
        var resElement = this.createElement(resourceElementInfo, index, this.taskElements[index]);
        resElement.innerText = resource.text;
    };
    GanttView.prototype.createTaskSelectionElement = function (index) {
        var selectionInfo = this.gridLayoutCalculator.getSelectionElementInfo(index);
        if (this.taskAreaContainer.isExternal && !this.settings.areHorizontalBordersEnabled)
            selectionInfo.size.height++;
        this.createElement(selectionInfo, index, this.taskArea, this.selectionElements);
    };
    GanttView.prototype.createHighlightRowElement = function (index) {
        var hlRowInfo = this.gridLayoutCalculator.getHighlightRowInfo(index);
        this.createElement(hlRowInfo, index, this.taskArea, this.hlRowElements);
    };
    GanttView.prototype.removeTaskElement = function (index) {
        this.removeElement(null, index, this.taskArea, this.taskElements);
        this.removeElement(null, index, this.taskArea, this.selectionElements);
        if (this.isHighlightRowElementAllowed(index))
            this.removeElement(null, index, this.taskArea, this.hlRowElements);
    };
    GanttView.prototype.createConnectorLineElement = function (info) {
        if (this.taskEditController.isDependencySelected(info.attr.get("dependency-id")))
            info.className = info.className + " active";
        return this.createElement(info, null, this.taskArea, this.connectorLinesToElementsMap);
    };
    GanttView.prototype.removeConnectorLineElement = function (info) {
        this.removeElement(info, null, this.taskArea, this.connectorLinesToElementsMap);
    };
    GanttView.prototype.createNoWorkingIntervalElement = function (info) {
        return this.createElement(info, null, this.taskArea, this.noWorkingIntervalsToElementsMap);
    };
    GanttView.prototype.removeNoWorkingIntervalElement = function (info) {
        this.removeElement(info, null, this.taskArea, this.noWorkingIntervalsToElementsMap);
    };
    GanttView.prototype.createElement = function (info, index, parent, dictionary) {
        var element = document.createElement("DIV");
        info.assignToElement(element);
        parent.appendChild(element);
        if (dictionary && dictionary instanceof Array && index !== null)
            dictionary[index] = element;
        else if (dictionary && dictionary instanceof Map)
            dictionary.set(info, element);
        info.attr.forEach(function (value, key) {
            element.setAttribute(key, value);
        });
        return element;
    };
    GanttView.prototype.removeElement = function (info, index, parent, dictionary) {
        var element;
        if (dictionary instanceof Array && index !== null) {
            element = dictionary[index];
            delete dictionary[index];
        }
        else if (dictionary instanceof Map) {
            element = dictionary.get(info);
            dictionary.delete(info);
        }
        if (element)
            parent.removeChild(element);
    };
    GanttView.prototype.calculateAutoViewType = function (startDate, endDate) {
        var diffInHours = (endDate.getTime() - startDate.getTime()) / (1000 * 3600);
        if (diffInHours > 24 * 365)
            return Enums_1.ViewType.Years;
        if (diffInHours > 24 * 30)
            return Enums_1.ViewType.Months;
        if (diffInHours > 24 * 7)
            return Enums_1.ViewType.Weeks;
        if (diffInHours > 24)
            return Enums_1.ViewType.Days;
        if (diffInHours > 6)
            return Enums_1.ViewType.SixHours;
        if (diffInHours > 1)
            return Enums_1.ViewType.Hours;
        return Enums_1.ViewType.TenMinutes;
    };
    GanttView.prototype.changeTaskExpanded = function (publicId, expanded) {
        var task = this.getTaskByPublicId(publicId);
        if (task)
            this.viewModel.changeTaskExpanded(task.internalId, expanded);
    };
    GanttView.prototype.expandTask = function (id) { this.viewModel.changeTaskExpanded(id, true); };
    GanttView.prototype.collapseTask = function (id) { this.viewModel.changeTaskExpanded(id, false); };
    GanttView.prototype.showTask = function (id) { this.viewModel.changeTaskVisibility(id, true); };
    GanttView.prototype.hideTask = function (id) { this.viewModel.changeTaskVisibility(id, false); };
    GanttView.prototype.getTaskVisibility = function (id) { return this.viewModel.getTaskVisibility(id); };
    GanttView.prototype.unselectCurrentSelectedTask = function () { this.unselectTask(this.currentSelectedTaskID); };
    GanttView.prototype.getTaskSelected = function (id) { return this.viewModel.getTaskSelected(id); };
    GanttView.prototype.setViewType = function (viewType) {
        if (viewType == undefined) {
            viewType = this.calculateAutoViewType(this.dataRange.start, this.dataRange.end);
        }
        if (this.settings.viewType !== viewType) {
            this.settings.viewType = viewType;
            this.updateTickSizeWidth();
            this.resetAndUpdate();
        }
    };
    GanttView.prototype.setTaskTitlePosition = function (taskTitlePosition) {
        if (this.settings.taskTitlePosition !== taskTitlePosition) {
            this.settings.taskTitlePosition = taskTitlePosition;
            this.resetAndUpdate();
        }
    };
    GanttView.prototype.setShowResources = function (showResources) {
        if (this.settings.showResources !== showResources) {
            this.settings.showResources = showResources;
            this.resetAndUpdate();
        }
    };
    GanttView.prototype.loadOptionsFromGanttOwner = function () {
        this.tickSize.height = this.ganttOwner.getRowHeight();
        var tasksData = this.ganttOwner.getGanttTasksData();
        this.dataRange = new DateRange_1.DateRange(this.getGanttViewStartDate(tasksData), this.getGanttViewEndDate(tasksData));
        if (this.settings.viewType == undefined)
            this.settings.viewType = this.calculateAutoViewType(this.dataRange.start, this.dataRange.end);
        this.updateTickSizeWidth();
        this.range = this.getDateRange(this.dataRange.start, this.dataRange.end);
        this.viewModel = new VisualModel_1.ViewVisualModel(this, tasksData, this.ganttOwner.getGanttDependenciesData(), this.ganttOwner.getGanttResourcesData(), this.ganttOwner.getGanttResourceAssignmentsData(), this.range, this.ganttOwner.getGanttWorkTimeRules());
        var dispatcher = new ModelChangesDispatcher_1.ModelChangesDispatcher();
        var modelChangesListener = this.ganttOwner.getModelChangesListener();
        if (modelChangesListener)
            dispatcher.onModelChanged.add(modelChangesListener);
        this.modelManipulator = new ModelManipulator_1.ModelManipulator(this.viewModel, dispatcher);
    };
    GanttView.prototype.resetAndUpdate = function () {
        this.reset();
        this.range = this.getDateRange(this.dataRange.start, this.dataRange.end);
        this.setupHelpers();
        this.createTimeScaleAreas();
        this.setSizeForTaskArea();
        this.updateView();
    };
    GanttView.prototype.selectTask = function (id) {
        this.viewModel.changeTaskSelected(id, true);
        this.currentSelectedTaskID = id;
    };
    GanttView.prototype.unselectTask = function (id) {
        this.viewModel.changeTaskSelected(id, false);
    };
    GanttView.prototype.selectTaskById = function (publicId) {
        this.unselectCurrentSelectedTask();
        var task = this.getTaskByPublicId(publicId);
        if (task)
            this.selectTask(task.internalId);
    };
    GanttView.prototype.getTaskAreaContainer = function () {
        return this.taskAreaContainer;
    };
    GanttView.prototype.setWidth = function (value) {
        this.mainElement.style.width = value + "px";
    };
    GanttView.prototype.setAllowSelection = function (value) {
        this.settings.allowSelectTask = value;
    };
    GanttView.prototype.setEditingSettings = function (value) {
        this.settings.editing = value;
    };
    GanttView.prototype.setRowLinesVisible = function (value) {
        this.settings.areHorizontalBordersEnabled = value;
        this.setClassesToTaskAreaContainer();
        this.resetAndUpdate();
    };
    GanttView.prototype.setTaskTitle = function (taskId, newValue) {
        var task = this.getTaskByPublicId(taskId);
        if (task)
            this.commandManager.changeTaskTitleCommand.execute(task.internalId, newValue);
    };
    GanttView.prototype.setTaskProgress = function (taskId, newValue) {
        var task = this.getTaskByPublicId(taskId);
        if (task)
            this.commandManager.changeTaskProgressCommand.execute(task.internalId, newValue);
    };
    GanttView.prototype.setTaskStart = function (taskId, newValue) {
        var task = this.getTaskByPublicId(taskId);
        if (task)
            this.commandManager.changeTaskStartCommand.execute(task.internalId, newValue);
    };
    GanttView.prototype.setTaskEnd = function (taskId, newValue) {
        var task = this.getTaskByPublicId(taskId);
        if (task)
            this.commandManager.changeTaskEndCommand.execute(task.internalId, newValue);
    };
    GanttView.prototype.removeTask = function (taskId) {
        var task = this.getTaskByPublicId(taskId);
        if (task)
            this.commandManager.removeTaskCommand.execute(task.internalId);
    };
    GanttView.prototype.createTask = function (parentId) {
        var modelItem = this.viewModel.findItem(parentId) || this.viewModel.items[0];
        var parent = modelItem.task;
        var start = new Date(parent.start.getFullYear(), parent.start.getMonth(), parent.start.getDate());
        var end = new Date(parent.start.getFullYear(), parent.start.getMonth(), parent.start.getDate() + 1);
        this.commandManager.createTaskCommand.execute(start, end, "", parentId);
    };
    GanttView.prototype.getTaskByPublicId = function (id) {
        return this.viewModel.tasks.getItemByPublicId(id);
    };
    GanttView.prototype.getPrevTask = function (taskId) {
        var item = this.viewModel.findItem(taskId);
        var index = item.parent.children.indexOf(item) - 1;
        return index > -1 ? item.parent.children[index].task : item.parent.task;
    };
    GanttView.prototype.updateCreatedTaskIdAfterServerUpdate = function (internalId, id) {
        var item = this.viewModel.findItem(internalId);
        var task = item && item.task;
        if (task)
            task.id = id;
    };
    GanttView.prototype.getTaskIdByInternalId = function (internalId) {
        var item = this.viewModel.findItem(internalId);
        var task = item && item.task;
        return task ? task.id : null;
    };
    GanttView.prototype.isTaskHasChildren = function (taskId) {
        var item = this.viewModel.findItem(taskId);
        return item && item.children.length > 0;
    };
    return GanttView;
}());
exports.GanttView = GanttView;


/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ResourceCollection_1 = __webpack_require__(12);
var TaskCollection_1 = __webpack_require__(36);
var DependencyCollection_1 = __webpack_require__(38);
var ResourceAssignmentCollection_1 = __webpack_require__(39);
var ViewVisualModelItem_1 = __webpack_require__(41);
var ViewVisualModelDependencyInfo_1 = __webpack_require__(42);
var WorkingTimeCalculator_1 = __webpack_require__(43);
var ViewVisualModel = (function () {
    function ViewVisualModel(owner, tasks, dependencies, resources, assignments, dateRange, workTimeRules) {
        this._fLockCount = 0;
        this.owner = owner;
        this.tasks = new TaskCollection_1.TaskCollection();
        this.tasks.importFromObject(tasks);
        this.dependencies = new DependencyCollection_1.DependencyCollection();
        this.dependencies.importFromObject(dependencies);
        this.resources = new ResourceCollection_1.ResourceCollection();
        this.resources.importFromObject(resources);
        this.assignments = new ResourceAssignmentCollection_1.ResourceAssignmentCollection();
        this.assignments.importFromObject(assignments);
        this._itemList = new Array();
        this._viewItemList = new Array();
        this._workTimeCalculator = new WorkingTimeCalculator_1.WorkingTimeCalculator(dateRange, workTimeRules);
        this.updateModel();
    }
    ViewVisualModel.prototype.updateModel = function () {
        this._itemList.splice(0, this._itemList.length);
        var tasks = this.tasks.items;
        for (var i = 0; i < tasks.length; i++) {
            var task = tasks[i];
            if (task)
                this._itemList.push(new ViewVisualModelItem_1.ViewVisualModelItem(task, this.getAssignedResources(task)));
        }
        this.createHierarchy();
        this.populateItemsForView();
        if (this.owner && this.owner.currentSelectedTaskID)
            this.changeTaskSelected(this.owner.currentSelectedTaskID, true);
    };
    ViewVisualModel.prototype.createHierarchy = function () {
        this.root = new ViewVisualModelItem_1.ViewVisualModelItem(null, null);
        var list = this._itemList;
        var _loop_1 = function () {
            var item = list[i];
            var parentId = item.task.parentId;
            var parentItem = list.filter(function (value) { return value.task && value.task.internalId === parentId || value.task.internalId.toString() === parentId; })[0] || this_1.root;
            item.parent = parentItem;
            parentItem.addChild(item);
        };
        var this_1 = this;
        for (var i = 0; i < list.length; i++) {
            _loop_1();
        }
    };
    ViewVisualModel.prototype.populateItemsForView = function () {
        this._viewItemList.splice(0, this._viewItemList.length);
        this.populateVisibleItems(this.root);
        this.updateVisibleItemDependencies();
    };
    ViewVisualModel.prototype.populateVisibleItems = function (item) {
        var _this = this;
        var isRoot = item === this.root;
        if (!item || (!item.task && !isRoot))
            return;
        if (!isRoot) {
            this._viewItemList.push(item);
            item.visibleIndex = this._viewItemList.length - 1;
        }
        if (item.getExpanded() || item === this.root)
            item.children.forEach(function (n) { return _this.populateVisibleItems(n); });
    };
    ViewVisualModel.prototype.updateVisibleItemDependencies = function () {
        var list = this._viewItemList;
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var visibleDependencies = this.getTasVisibleDependencies(item.task);
            item.setDependencies(visibleDependencies);
        }
    };
    ViewVisualModel.prototype.getAssignedResources = function (task) {
        var _this = this;
        var res = new ResourceCollection_1.ResourceCollection();
        var assignments = this.assignments.items.filter(function (value) { return value.taskId == task.internalId; });
        assignments.forEach(function (assignment) { res.add(_this.resources.getItemById(assignment.resourceId)); });
        return res;
    };
    ViewVisualModel.prototype.getTasVisibleDependencies = function (task) {
        var res = new Array();
        var id = task.internalId;
        var dependencies = this.dependencies.items.filter(function (value) { return value.successorId == id; });
        for (var i = 0; i < dependencies.length; i++) {
            var dependency = dependencies[i];
            var item = this.findItem(dependency.predecessorId);
            if (item && item.getVisible())
                res.push(new ViewVisualModelDependencyInfo_1.ViewVisualModelDependencyInfo(dependency.internalId, item, dependency.type));
        }
        return res;
    };
    ViewVisualModel.prototype.changeTaskExpanded = function (id, expanded) {
        var task = this.tasks.getItemById(String(id));
        if (task) {
            task.expanded = expanded;
            this.changed();
        }
    };
    ViewVisualModel.prototype.changeTaskVisibility = function (id, visible) {
        var item = this.findItem(id);
        if (item) {
            item.visible = visible;
            this.changed();
        }
    };
    ViewVisualModel.prototype.changeTaskSelected = function (id, selected) {
        var item = this.findItem(String(id));
        if (item) {
            item.selected = selected;
            this.changed();
        }
    };
    ViewVisualModel.prototype.beginUpdate = function () {
        this._fLockCount++;
    };
    ViewVisualModel.prototype.endUpdate = function () {
        this._fLockCount--;
        if (this._fLockCount == 0)
            this.changed();
    };
    ViewVisualModel.prototype.changed = function () {
        if (this._fLockCount !== 0)
            return;
        this.populateItemsForView();
        if (this.owner && this.owner.onVisualModelChanged)
            this.owner.onVisualModelChanged();
    };
    ViewVisualModel.prototype.findItem = function (taskId) { return this._viewItemList.filter(function (value) { return value.task && value.task.internalId === taskId; })[0]; };
    Object.defineProperty(ViewVisualModel.prototype, "items", {
        get: function () { return this._viewItemList.slice(); ; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ViewVisualModel.prototype, "itemCount", {
        get: function () { return this.items.length; },
        enumerable: true,
        configurable: true
    });
    ViewVisualModel.prototype.getTaskVisibility = function (id) {
        var item = this.findItem(id);
        return !!item && item.getVisible();
    };
    ViewVisualModel.prototype.getTaskSelected = function (id) {
        var item = this.findItem(id);
        return !!item && item.selected;
    };
    Object.defineProperty(ViewVisualModel.prototype, "noWorkingIntervals", {
        get: function () { return this._workTimeCalculator.noWorkingIntervals; },
        enumerable: true,
        configurable: true
    });
    ;
    ViewVisualModel.prototype.updateRange = function (range) { this._workTimeCalculator.updateRange(range); };
    ;
    return ViewVisualModel;
}());
exports.ViewVisualModel = ViewVisualModel;


/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var DataObject_1 = __webpack_require__(4);
var Resource = (function (_super) {
    __extends(Resource, _super);
    function Resource() {
        var _this = _super.call(this) || this;
        _this.text = "";
        return _this;
    }
    Resource.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.text = sourceObj.text;
        }
    };
    return Resource;
}(DataObject_1.DataObject));
exports.Resource = Resource;


/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Task_1 = __webpack_require__(37);
var CollectionBase_1 = __webpack_require__(6);
var TaskCollection = (function (_super) {
    __extends(TaskCollection, _super);
    function TaskCollection() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskCollection.prototype.createItem = function () { return new Task_1.Task(); };
    return TaskCollection;
}(CollectionBase_1.CollectionBase));
exports.TaskCollection = TaskCollection;


/***/ }),
/* 37 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var DataObject_1 = __webpack_require__(4);
var TaskType;
(function (TaskType) {
    TaskType[TaskType["Regular"] = 0] = "Regular";
    TaskType[TaskType["Summary"] = 1] = "Summary";
    TaskType[TaskType["Milestone"] = 2] = "Milestone";
})(TaskType || (TaskType = {}));
var Task = (function (_super) {
    __extends(Task, _super);
    function Task() {
        var _this = _super.call(this) || this;
        _this.start = null;
        _this.end = null;
        _this.duration = null;
        _this.description = "";
        _this.parentId = "";
        _this.title = "";
        _this.owner = null;
        _this.progress = 0;
        _this.taskType = null;
        _this.customFields = {};
        _this.expanded = true;
        return _this;
    }
    Task.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.owner = sourceObj.owner;
            this.parentId = sourceObj.parentId != null ? String(sourceObj.parentId) : null;
            this.description = sourceObj.description;
            this.title = sourceObj.title;
            this.start = sourceObj.start;
            this.end = sourceObj.end;
            this.start = typeof sourceObj.start == "string" ? new Date(sourceObj.start) : sourceObj.start;
            this.end = typeof sourceObj.end == "string" ? new Date(sourceObj.end) : sourceObj.end;
            this.duration = sourceObj.duration;
            this.progress = sourceObj.progress;
            this.taskType = sourceObj.taskType;
            if (Utils_1.JsonUtils.isExists(sourceObj.expanded))
                this.expanded = !!sourceObj.expanded;
            this.assignCustomFields(sourceObj.customFields);
        }
    };
    Task.prototype.assignCustomFields = function (sourceObj) {
        if (!sourceObj)
            return;
        for (var property in sourceObj) {
            if (!sourceObj.hasOwnProperty(property))
                continue;
            this.customFields[property] = sourceObj[property];
        }
    };
    Task.prototype.isMilestone = function () {
        return this.start.getTime() == this.end.getTime();
    };
    return Task;
}(DataObject_1.DataObject));
exports.Task = Task;


/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CollectionBase_1 = __webpack_require__(6);
var Dependency_1 = __webpack_require__(13);
var DependencyCollection = (function (_super) {
    __extends(DependencyCollection, _super);
    function DependencyCollection() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DependencyCollection.prototype.createItem = function () { return new Dependency_1.Dependency(); };
    return DependencyCollection;
}(CollectionBase_1.CollectionBase));
exports.DependencyCollection = DependencyCollection;


/***/ }),
/* 39 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CollectionBase_1 = __webpack_require__(6);
var ResourceAssignment_1 = __webpack_require__(40);
var ResourceAssignmentCollection = (function (_super) {
    __extends(ResourceAssignmentCollection, _super);
    function ResourceAssignmentCollection() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourceAssignmentCollection.prototype.createItem = function () { return new ResourceAssignment_1.ResourceAssignment(); };
    return ResourceAssignmentCollection;
}(CollectionBase_1.CollectionBase));
exports.ResourceAssignmentCollection = ResourceAssignmentCollection;


/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DataObject_1 = __webpack_require__(4);
var Utils_1 = __webpack_require__(0);
var ResourceAssignment = (function (_super) {
    __extends(ResourceAssignment, _super);
    function ResourceAssignment() {
        var _this = _super.call(this) || this;
        _this.taskId = "";
        _this.resourceId = "";
        return _this;
    }
    ResourceAssignment.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.taskId = String(sourceObj.taskId);
            this.resourceId = String(sourceObj.resourceId);
        }
    };
    return ResourceAssignment;
}(DataObject_1.DataObject));
exports.ResourceAssignment = ResourceAssignment;


/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Utils_1 = __webpack_require__(0);
var ViewVisualModelItem = (function () {
    function ViewVisualModelItem(task, resources) {
        this.dependencies = new Array();
        this.parent = null;
        this.visible = true;
        this.selected = false;
        this.visibleIndex = -1;
        this.task = task;
        this.resources = resources;
        this.children = new Array();
    }
    Object.defineProperty(ViewVisualModelItem.prototype, "resourceText", {
        get: function () {
            var text = "";
            this.resources.items.forEach(function (r) { return text += r.text + " "; });
            return text;
        },
        enumerable: true,
        configurable: true
    });
    ViewVisualModelItem.prototype.addChild = function (child) {
        if (Utils_1.JsonUtils.isExists(child) && this.children.indexOf(child) < 0)
            this.children.push(child);
    };
    ViewVisualModelItem.prototype.removeChild = function (child) {
        var index = this.children.indexOf(child);
        if (index > -1)
            this.children.splice(index, 1);
    };
    ViewVisualModelItem.prototype.getExpanded = function () {
        return !!this.task && this.task.expanded;
    };
    ViewVisualModelItem.prototype.getVisible = function () {
        if (!this.visible)
            return false;
        var parentItem = this.parent;
        while (parentItem) {
            if (!parentItem.visible)
                return false;
            parentItem = parentItem.parent;
        }
        return true;
    };
    ViewVisualModelItem.prototype.changeVisibility = function (visible) {
        this.visible = visible;
    };
    ViewVisualModelItem.prototype.changeSelection = function (selected) {
        this.selected = selected;
    };
    ViewVisualModelItem.prototype.setDependencies = function (dependencies) {
        if (dependencies)
            this.dependencies = dependencies.slice();
    };
    return ViewVisualModelItem;
}());
exports.ViewVisualModelItem = ViewVisualModelItem;


/***/ }),
/* 42 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ViewVisualModelDependencyInfo = (function () {
    function ViewVisualModelDependencyInfo(id, predecessor, type) {
        this.id = id;
        this.predecessor = predecessor;
        this.type = type;
    }
    return ViewVisualModelDependencyInfo;
}());
exports.ViewVisualModelDependencyInfo = ViewVisualModelDependencyInfo;


/***/ }),
/* 43 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var GanttWorkingDayRuleCollection_1 = __webpack_require__(44);
var DayWorkingTimeInfo_1 = __webpack_require__(52);
var DateTimeUtils_1 = __webpack_require__(1);
var WorkingTimeCalculator = (function () {
    function WorkingTimeCalculator(range, rules) {
        this._workingRules = new GanttWorkingDayRuleCollection_1.WorkingDayRuleCollection();
        this._workDayList = new Array();
        this._calculationRange = range;
        this._workingRules.importFromObject(rules);
    }
    WorkingTimeCalculator.prototype.calculateWorkDayList = function () {
        if (!this._calculationRange)
            return;
        this.clearList();
        var rules = this._workingRules.items;
        for (var i = 0; i < rules.length; i++)
            this.processRule(rules[i]);
        this.sortList();
    };
    WorkingTimeCalculator.prototype.processRule = function (rule) {
        var points = rule.recurrence.calculatePoints(this._calculationRange.start, this._calculationRange.end);
        var _loop_1 = function (i) {
            var point = points[i];
            var dayNum = DateTimeUtils_1.DateTimeUtils.getDayNumber(point);
            var dayInfo = this_1._workDayList.filter(function (i) { return i.dayNumber == dayNum; })[0];
            if (dayInfo) {
                dayInfo.isWorkDay = dayInfo.isWorkDay && rule.isWorkDay;
                dayInfo.addWorkingIntervals(rule.workTimeRanges);
            }
            else
                this_1._workDayList.push(new DayWorkingTimeInfo_1.DayWorkingTimeInfo(dayNum, rule.isWorkDay, rule.workTimeRanges));
        };
        var this_1 = this;
        for (var i = 0; i < points.length; i++) {
            _loop_1(i);
        }
    };
    WorkingTimeCalculator.prototype.sortList = function () {
        this._workDayList.sort(function (d1, d2) { return d1.dayNumber - d2.dayNumber; });
    };
    WorkingTimeCalculator.prototype.clearList = function () {
        this._workDayList.splice(0, this._workDayList.length);
    };
    WorkingTimeCalculator.prototype.calculateNoWorkTimeIntervals = function () {
        var _this = this;
        var res = new Array();
        if (this._workDayList.length == 0)
            this.calculateWorkDayList();
        this._workDayList.forEach(function (d) { return res = res.concat(_this.getNoWorkTimeRangesFromDay(d)); });
        return this.concatJointedRanges(res);
    };
    WorkingTimeCalculator.prototype.concatJointedRanges = function (list) {
        var res = new Array();
        for (var i = 0; i < list.length; i++) {
            var interval = list[i];
            var needExpandPrevInterval = res.length > 0 && DateTimeUtils_1.DateTimeUtils.compareDates(res[res.length - 1].end, interval.start) < 2;
            if (needExpandPrevInterval)
                res[res.length - 1].end = interval.end;
            else
                res.push(interval);
        }
        return res;
    };
    WorkingTimeCalculator.prototype.getNoWorkTimeRangesFromDay = function (day) {
        return day.noWorkingIntervals.map(function (i) { return DateTimeUtils_1.DateTimeUtils.convertTimeRangeToDateRange(i, day.dayNumber); });
    };
    Object.defineProperty(WorkingTimeCalculator.prototype, "noWorkingIntervals", {
        get: function () {
            if (!this._noWorkingIntervals)
                this._noWorkingIntervals = this.calculateNoWorkTimeIntervals();
            return this._noWorkingIntervals.slice();
        },
        enumerable: true,
        configurable: true
    });
    WorkingTimeCalculator.prototype.updateRange = function (range) {
        this._calculationRange = range;
        this.invalidate();
    };
    WorkingTimeCalculator.prototype.invalidate = function () {
        this._noWorkingIntervals = null;
        this.clearList();
    };
    return WorkingTimeCalculator;
}());
exports.WorkingTimeCalculator = WorkingTimeCalculator;


/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CollectionBase_1 = __webpack_require__(6);
var WorkingTimeRule_1 = __webpack_require__(45);
var WorkingDayRuleCollection = (function (_super) {
    __extends(WorkingDayRuleCollection, _super);
    function WorkingDayRuleCollection() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    WorkingDayRuleCollection.prototype.createItem = function () { return new WorkingTimeRule_1.WorkingTimeRule(); };
    return WorkingDayRuleCollection;
}(CollectionBase_1.CollectionBase));
exports.WorkingDayRuleCollection = WorkingDayRuleCollection;


/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DataObject_1 = __webpack_require__(4);
var Utils_1 = __webpack_require__(0);
var DateTimeUtils_1 = __webpack_require__(1);
var RecurrenceFactory_1 = __webpack_require__(25);
var Daily_1 = __webpack_require__(26);
var WorkingTimeRule = (function (_super) {
    __extends(WorkingTimeRule, _super);
    function WorkingTimeRule(recurrence, isWorkDay, workTimeRanges) {
        if (recurrence === void 0) { recurrence = null; }
        if (isWorkDay === void 0) { isWorkDay = true; }
        if (workTimeRanges === void 0) { workTimeRanges = null; }
        var _this = _super.call(this) || this;
        _this.isWorkDay = true;
        _this.workTimeRanges = new Array();
        _this.recurrence = recurrence;
        _this.isWorkDay = isWorkDay;
        if (workTimeRanges)
            _this.workTimeRanges.concat(workTimeRanges);
        return _this;
    }
    WorkingTimeRule.prototype.assignFromObject = function (sourceObj) {
        if (Utils_1.JsonUtils.isExists(sourceObj)) {
            _super.prototype.assignFromObject.call(this, sourceObj);
            this.recurrence = RecurrenceFactory_1.RecurrenceFactory.createRecurrenceByType(sourceObj.recurrenceType) || new Daily_1.Daily();
            if (Utils_1.JsonUtils.isExists(sourceObj.recurrence))
                this.recurrence.assignFromObject(sourceObj.recurrence);
            if (Utils_1.JsonUtils.isExists(sourceObj.isWorkDay))
                this.isWorkDay = !!sourceObj.isWorkDay;
            var ranges = DateTimeUtils_1.DateTimeUtils.convertToTimeRanges(sourceObj.workTimeRanges);
            if (ranges)
                this.workTimeRanges = ranges;
        }
    };
    return WorkingTimeRule;
}(DataObject_1.DataObject));
exports.WorkingTimeRule = WorkingTimeRule;


/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DayOfWeek;
(function (DayOfWeek) {
    DayOfWeek[DayOfWeek["Sunday"] = 0] = "Sunday";
    DayOfWeek[DayOfWeek["Monday"] = 1] = "Monday";
    DayOfWeek[DayOfWeek["Tuesday"] = 2] = "Tuesday";
    DayOfWeek[DayOfWeek["Wednesday"] = 3] = "Wednesday";
    DayOfWeek[DayOfWeek["Thursday"] = 4] = "Thursday";
    DayOfWeek[DayOfWeek["Friday"] = 5] = "Friday";
    DayOfWeek[DayOfWeek["Saturday"] = 6] = "Saturday";
})(DayOfWeek = exports.DayOfWeek || (exports.DayOfWeek = {}));


/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Month;
(function (Month) {
    Month[Month["January"] = 0] = "January";
    Month[Month["February"] = 1] = "February";
    Month[Month["March"] = 2] = "March";
    Month[Month["April"] = 3] = "April";
    Month[Month["May"] = 4] = "May";
    Month[Month["June"] = 5] = "June";
    Month[Month["July"] = 6] = "July";
    Month[Month["August"] = 7] = "August";
    Month[Month["September"] = 8] = "September";
    Month[Month["October"] = 9] = "October";
    Month[Month["November"] = 10] = "November";
    Month[Month["December"] = 11] = "December";
})(Month = exports.Month || (exports.Month = {}));


/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var RecurrenceBase_1 = __webpack_require__(11);
var DateTimeUtils_1 = __webpack_require__(1);
var Weekly = (function (_super) {
    __extends(Weekly, _super);
    function Weekly() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Weekly.prototype.checkDate = function (date) {
        return DateTimeUtils_1.DateTimeUtils.checkDayOfWeek(this.dayOfWeekInternal, date);
    };
    Weekly.prototype.checkInterval = function (date) {
        return DateTimeUtils_1.DateTimeUtils.getWeeksBetween(this.start, date) % this.interval == 0;
    };
    Weekly.prototype.calculatePointByInterval = function (date) {
        var weeksFromStart = DateTimeUtils_1.DateTimeUtils.getWeeksBetween(this.start, date);
        var intervalCount = Math.floor(weeksFromStart / this.interval);
        var remainder = weeksFromStart % this.interval;
        var isPointOnNewWeek = remainder > 0 || date.getDay() >= this.dayOfWeekInternal;
        if (isPointOnNewWeek)
            intervalCount++;
        var weeksToAdd = intervalCount * this.interval;
        return this.calcNextPointWithWeekCount(this.start, weeksToAdd);
    };
    Weekly.prototype.calculateNearestPoint = function (date) {
        var diff = this.dayOfWeekInternal - date.getDay();
        if (diff > 0)
            return DateTimeUtils_1.DateTimeUtils.addDays(new Date(date), diff);
        return this.calcNextPointWithWeekCount(date, 1);
    };
    Weekly.prototype.calcNextPointWithWeekCount = function (date, weekCount) {
        if (weekCount === void 0) { weekCount = 1; }
        var daysToAdd = weekCount * 7 + this.dayOfWeekInternal - date.getDay();
        return DateTimeUtils_1.DateTimeUtils.addDays(new Date(date), daysToAdd);
    };
    Object.defineProperty(Weekly.prototype, "dayOfWeek", {
        get: function () { return this.dayOfWeekInternal; },
        set: function (value) { this.dayOfWeekInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    return Weekly;
}(RecurrenceBase_1.RecurrenceBase));
exports.Weekly = Weekly;


/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var RecurrenceBase_1 = __webpack_require__(11);
var DateTimeUtils_1 = __webpack_require__(1);
var MonthInfo_1 = __webpack_require__(50);
var Monthly = (function (_super) {
    __extends(Monthly, _super);
    function Monthly() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Monthly.prototype.checkDate = function (date) {
        if (this._calculateByDayOfWeek)
            return DateTimeUtils_1.DateTimeUtils.checkDayOfWeekOccurrenceInMonth(date, this.dayOfWeekInternal, this.dayOfWeekOccurrenceInternal);
        return DateTimeUtils_1.DateTimeUtils.checkDayOfMonth(this.dayInternal, date);
    };
    Monthly.prototype.checkInterval = function (date) {
        return DateTimeUtils_1.DateTimeUtils.getMonthsDifference(this.start, date) % this.interval == 0;
    };
    Monthly.prototype.calculatePointByInterval = function (date) {
        var start = this.start;
        var monthFromStart = DateTimeUtils_1.DateTimeUtils.getMonthsDifference(start, date);
        var monthToAdd = Math.floor(monthFromStart / this.interval) * this.interval;
        var info = new MonthInfo_1.MonthInfo(start.getMonth(), start.getFullYear());
        info.addMonths(monthToAdd);
        var point = this.getSpecDayInMonth(info.year, info.month);
        if (DateTimeUtils_1.DateTimeUtils.compareDates(point, date) >= 0) {
            info.addMonths(this.interval);
            point = this.getSpecDayInMonth(info.year, info.month);
        }
        return point;
    };
    Monthly.prototype.calculateNearestPoint = function (date) {
        var month = date.getMonth();
        var year = date.getFullYear();
        var point = this.getSpecDayInMonth(year, month);
        if (DateTimeUtils_1.DateTimeUtils.compareDates(point, date) >= 0) {
            var info = new MonthInfo_1.MonthInfo(month, year);
            info.addMonths(1);
            point = this.getSpecDayInMonth(info.year, info.month);
        }
        return point;
    };
    Object.defineProperty(Monthly.prototype, "day", {
        get: function () { return this.dayInternal; },
        set: function (value) { this.dayInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Monthly.prototype, "dayOfWeek", {
        get: function () { return this.dayOfWeekInternal; },
        set: function (value) { this.dayOfWeekInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Monthly.prototype, "dayOfWeekOccurrence", {
        get: function () { return this.dayOfWeekOccurrenceInternal; },
        set: function (value) { this.dayOfWeekOccurrenceInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Monthly.prototype, "calculateByDayOfWeek", {
        get: function () { return this._calculateByDayOfWeek; },
        set: function (value) { this._calculateByDayOfWeek = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    return Monthly;
}(RecurrenceBase_1.RecurrenceBase));
exports.Monthly = Monthly;


/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DateTimeUtils_1 = __webpack_require__(1);
var MonthInfo = (function () {
    function MonthInfo(month, year) {
        this.month = month;
        this.year = year;
    }
    MonthInfo.prototype.addMonths = function (months) {
        var nextMonth = DateTimeUtils_1.DateTimeUtils.getNextMonth(this.month, months);
        var yearInc = Math.floor(months / 12);
        if (nextMonth < this.month)
            ++yearInc;
        this.month = nextMonth;
        this.year += yearInc;
    };
    return MonthInfo;
}());
exports.MonthInfo = MonthInfo;


/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var RecurrenceBase_1 = __webpack_require__(11);
var DateTimeUtils_1 = __webpack_require__(1);
var Yearly = (function (_super) {
    __extends(Yearly, _super);
    function Yearly() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Yearly.prototype.checkDate = function (date) {
        if (!DateTimeUtils_1.DateTimeUtils.checkMonth(this.month, date))
            return false;
        if (this._calculateByDayOfWeek)
            return DateTimeUtils_1.DateTimeUtils.checkDayOfWeekOccurrenceInMonth(date, this.dayOfWeekInternal, this.dayOfWeekOccurrenceInternal);
        return DateTimeUtils_1.DateTimeUtils.checkDayOfMonth(this.dayInternal, date);
    };
    Yearly.prototype.checkInterval = function (date) {
        return DateTimeUtils_1.DateTimeUtils.getYearsDifference(this.start, date) % this.interval == 0;
    };
    Yearly.prototype.calculatePointByInterval = function (date) {
        var yearFromStart = DateTimeUtils_1.DateTimeUtils.getYearsDifference(this.start, date);
        var yearInc = Math.floor(yearFromStart / this.interval) * this.interval;
        var newYear = this.start.getFullYear() + yearInc;
        var point = this.getSpecDayInMonth(newYear, this.monthInternal);
        if (DateTimeUtils_1.DateTimeUtils.compareDates(point, date) >= 0) {
            newYear += this.interval;
            point = this.getSpecDayInMonth(newYear, this.monthInternal);
        }
        return point;
    };
    Yearly.prototype.calculateNearestPoint = function (date) {
        var year = date.getFullYear();
        var point = this.getSpecDayInMonth(year, this.monthInternal);
        if (DateTimeUtils_1.DateTimeUtils.compareDates(point, date) >= 0)
            point = this.getSpecDayInMonth(++year, this.monthInternal);
        return point;
    };
    Object.defineProperty(Yearly.prototype, "month", {
        get: function () { return this.monthInternal; },
        set: function (value) { this.monthInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Yearly.prototype, "day", {
        get: function () { return this.dayInternal; },
        set: function (value) { this.dayInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Yearly.prototype, "dayOfWeek", {
        get: function () { return this.dayOfWeekInternal; },
        set: function (value) { this.dayOfWeekInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Yearly.prototype, "dayOfWeekOccurrence", {
        get: function () { return this.dayOfWeekOccurrenceInternal; },
        set: function (value) { this.dayOfWeekOccurrenceInternal = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(Yearly.prototype, "calculateByDayOfWeek", {
        get: function () { return this._calculateByDayOfWeek; },
        set: function (value) { this._calculateByDayOfWeek = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    return Yearly;
}(RecurrenceBase_1.RecurrenceBase));
exports.Yearly = Yearly;


/***/ }),
/* 52 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var TimeRange_1 = __webpack_require__(23);
var DateTimeUtils_1 = __webpack_require__(1);
var Time_1 = __webpack_require__(22);
var DayWorkingTimeInfo = (function () {
    function DayWorkingTimeInfo(dayNumber, isWorkDay, intervals) {
        if (dayNumber === void 0) { dayNumber = 0; }
        if (isWorkDay === void 0) { isWorkDay = true; }
        if (intervals === void 0) { intervals = null; }
        this._workingIntervals = new Array();
        this.dayNumber = dayNumber;
        this.isWorkDay = isWorkDay;
        this.addWorkingIntervals(intervals);
    }
    DayWorkingTimeInfo.prototype.addWorkingIntervals = function (intervals) {
        if (!intervals)
            return;
        this._workingIntervals = this._workingIntervals.concat(intervals.filter(function (r) { return !!r; }));
        this.rearrangeWorkingIntervals();
    };
    DayWorkingTimeInfo.prototype.rearrangeWorkingIntervals = function () {
        for (var i = 0; i < this._workingIntervals.length; i++) {
            this.concatWithIntersectedRanges(this._workingIntervals[i]);
        }
        this.sortIntervals();
    };
    ;
    DayWorkingTimeInfo.prototype.concatWithIntersectedRanges = function (range) {
        var _this = this;
        var intersectedRanges = this.getIntersectedIntervals(range);
        intersectedRanges.forEach(function (r) {
            range.concatWith(r);
            _this.removeInterval(r);
        });
    };
    ;
    DayWorkingTimeInfo.prototype.getIntersectedIntervals = function (range) {
        return this._workingIntervals.filter(function (r) { return r.hasIntersect(range) && r !== range; });
    };
    ;
    DayWorkingTimeInfo.prototype.sortIntervals = function () {
        this._workingIntervals.sort(function (r1, r2) { return DateTimeUtils_1.DateTimeUtils.caclTimeDifference(r2.start, r1.start); });
    };
    ;
    DayWorkingTimeInfo.prototype.removeInterval = function (element) {
        var index = this._workingIntervals.indexOf(element);
        if (index > -1 && index < this._workingIntervals.length)
            this._workingIntervals.splice(index, 1);
    };
    ;
    DayWorkingTimeInfo.prototype.clearIntervals = function () {
        this._workingIntervals.splice(0, this._workingIntervals.length);
    };
    Object.defineProperty(DayWorkingTimeInfo.prototype, "workingIntervals", {
        get: function () { return this._workingIntervals.slice(); },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(DayWorkingTimeInfo.prototype, "noWorkingIntervals", {
        get: function () {
            var res = new Array();
            if (this.isWorkDay && this._workingIntervals.length == 0)
                return res;
            var starts = this._workingIntervals.map(function (r) { return r.end; });
            starts.splice(0, 0, new Time_1.Time());
            var ends = this._workingIntervals.map(function (r) { return r.start; });
            ends.push(DateTimeUtils_1.DateTimeUtils.getLastTimeOfDay());
            for (var i = 0; i < starts.length; i++) {
                var start = starts[i];
                var end = ends[i];
                if (!DateTimeUtils_1.DateTimeUtils.areTimesEqual(start, end))
                    res.push(new TimeRange_1.TimeRange(start, end));
            }
            return res;
        },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(DayWorkingTimeInfo.prototype, "isWorkDay", {
        get: function () { return this._isWorkDay; },
        set: function (value) {
            this._isWorkDay = value;
            if (!value)
                this.clearIntervals();
        },
        enumerable: true,
        configurable: true
    });
    ;
    return DayWorkingTimeInfo;
}());
exports.DayWorkingTimeInfo = DayWorkingTimeInfo;


/***/ }),
/* 53 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Margins = (function () {
    function Margins() {
        this.marginLeft = null;
        this.marginTop = null;
        this.marginRight = null;
        this.marginBottom = null;
    }
    return Margins;
}());
exports.Margins = Margins;


/***/ }),
/* 54 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Size_1 = __webpack_require__(15);
var DateRange_1 = __webpack_require__(10);
var Enums_1 = __webpack_require__(7);
var GridElementInfo_1 = __webpack_require__(28);
var Utils_1 = __webpack_require__(27);
var Point_1 = __webpack_require__(3);
var Dependency_1 = __webpack_require__(13);
var GridLayoutCalculator = (function () {
    function GridLayoutCalculator() {
        this.tileToDependencyMap = [];
        this.tileToNoWorkingIntervalsMap = [];
        this.minLineLength = 10;
        this.resourceMaxWidth = 500;
    }
    GridLayoutCalculator.prototype.setSettings = function (visibleTaskAreaSize, tickSize, elementSizeValues, range, viewModel, viewType) {
        this.visibleTaskAreaSize = visibleTaskAreaSize;
        this.tickSize = tickSize;
        this.viewType = viewType;
        this.range = range;
        this.verticalTickCount = viewModel.itemCount;
        this.viewModel = viewModel;
        this.elementSizeValues = elementSizeValues;
        this.taskHeight = elementSizeValues.taskHeight;
        this.milestoneWidth = elementSizeValues.milestoneWidth;
        this.scaleHeight = elementSizeValues.scaleItemHeight;
        this.arrowSize = new Size_1.Size(elementSizeValues.connectorArrowWidth, elementSizeValues.connectorArrowWidth);
        this.lineThickness = elementSizeValues.connectorLineThickness;
        this.minConnectorSpaceFromTask = (this.tickSize.height - this.taskHeight) / 2;
        this.tickTimeSpan = Utils_1.DateUtils.getTickTimeSpan(viewType);
        this.horizontalTickCount = this.getTotalTickCount();
        this.createTileToConnectorLinesMap();
        this.createTileToNonWorkingIntervalsMap();
    };
    GridLayoutCalculator.prototype.getTaskAreaBorderInfo = function (index, isVertical) {
        var sizeValue = isVertical ?
            this.tickSize.height * this.verticalTickCount :
            this.tickSize.width * this.horizontalTickCount;
        return this.getGridBorderInfo(index, isVertical, sizeValue);
    };
    GridLayoutCalculator.prototype.getScaleBorderInfo = function (index, scaleType) {
        var result = this.getGridBorderInfo(index, true);
        result.position.x *= this.getScaleItemColSpan(scaleType);
        return result;
    };
    GridLayoutCalculator.prototype.getGridBorderInfo = function (index, isVertical, size) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.setPosition(this.getGridBorderPosition(index, isVertical));
        if (size)
            result.setSize(this.getGridBorderSize(isVertical, size));
        result.className = isVertical ? "dx-gantt-vb" : "dx-gantt-hb";
        return result;
    };
    GridLayoutCalculator.prototype.getGridBorderPosition = function (index, isVertical) {
        var result = new Point_1.Point();
        var posValue = (index + 1) * (isVertical ? this.tickSize.width : this.tickSize.height);
        if (isVertical)
            result.x = posValue;
        else
            result.y = posValue;
        return result;
    };
    GridLayoutCalculator.prototype.getGridBorderSize = function (isVertical, sizeValue) {
        var result = new Size_1.Size();
        if (isVertical)
            result.height = sizeValue;
        else
            result.width = sizeValue;
        return result;
    };
    GridLayoutCalculator.prototype.getScaleElementInfo = function (index, scaleType) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.setPosition(this.getScaleItemPosition(index, scaleType));
        result.setSize(this.getScaleItemSize(scaleType));
        result.className = this.getScaleItemClassName(scaleType, result, this.getRenderedNoWorkingIntervals(result.position.x));
        return result;
    };
    GridLayoutCalculator.prototype.getScaleItemClassName = function (scaleType, scaleItemInfo, noWorkingIntervals) {
        var result = "dx-gantt-si";
        if (scaleType.valueOf() == this.viewType.valueOf() && this.isScaleItemInsideNoWorkingInterval(scaleItemInfo, noWorkingIntervals))
            result += " dx-gantt-holiday-scaleItem";
        return result;
    };
    GridLayoutCalculator.prototype.isScaleItemInsideNoWorkingInterval = function (scaleItemInfo, noWorkingIntervals) {
        var scaleItemLeft = scaleItemInfo.position.x;
        var scaleItemRight = scaleItemInfo.position.x + scaleItemInfo.size.width;
        for (var i = 0; i < noWorkingIntervals.length; i++) {
            var noWorkingIntervalLeft = noWorkingIntervals[i].position.x;
            var noWorkingIntervalRight = noWorkingIntervals[i].position.x + noWorkingIntervals[i].size.width;
            if (scaleItemLeft >= noWorkingIntervalLeft && scaleItemRight <= noWorkingIntervalRight)
                return true;
        }
        return false;
    };
    GridLayoutCalculator.prototype.getScaleItemPosition = function (index, scaleType) {
        return new Point_1.Point(index * this.tickSize.width * this.getScaleItemColSpan(scaleType));
    };
    GridLayoutCalculator.prototype.getScaleItemSize = function (scaleType) {
        return new Size_1.Size(this.tickSize.width * this.getScaleItemColSpan(scaleType));
    };
    GridLayoutCalculator.prototype.getScaleItemColSpan = function (scaleType) {
        if (scaleType.valueOf() == this.viewType.valueOf())
            return 1;
        if (this.viewType == Enums_1.ViewType.TenMinutes)
            return 6;
        if (this.viewType == Enums_1.ViewType.Hours)
            return 24;
        if (this.viewType == Enums_1.ViewType.SixHours)
            return 4;
        if (this.viewType == Enums_1.ViewType.Days)
            return 7;
        if (this.viewType == Enums_1.ViewType.Weeks)
            return 4.29;
        if (this.viewType == Enums_1.ViewType.Months)
            return 12;
        if (this.viewType == Enums_1.ViewType.Quarter)
            return 4;
        return 1;
    };
    GridLayoutCalculator.prototype.getTaskWrapperElementInfo = function (index) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = this.getTaskWrapperClassName(index);
        result.setPosition(this.getTaskWrapperPoint(index));
        result.setSize(this.getTaskWrapperSize(index));
        return result;
    };
    GridLayoutCalculator.prototype.getTaskWrapperClassName = function (index) {
        var result = "dx-gantt-taskWrapper";
        if (this.viewModel.items[index].selected)
            result += " dx-gantt-selectedTask";
        return result;
    };
    GridLayoutCalculator.prototype.getTaskWrapperSize = function (index) {
        var resourceCount = this.viewModel.items[index].resources.items.length;
        var resourceWidth = this.resourceMaxWidth * resourceCount;
        var taskWidth = this.getTaskWidth(index);
        var taskPoint = this.getTaskPoint(index);
        var totalWidth = this.tickSize.width * this.horizontalTickCount;
        var resultWidth = Math.min(taskWidth + resourceWidth, totalWidth - taskPoint.x);
        return new Size_1.Size(resultWidth);
    };
    GridLayoutCalculator.prototype.getTaskWrapperPoint = function (index) {
        var result = new Point_1.Point(this.getPosByDate(this.getTask(index).start), index * this.tickSize.height);
        if (this.getTask(index).isMilestone())
            result.x -= this.getTaskHeight(index) / 2;
        return result;
    };
    GridLayoutCalculator.prototype.getTaskElementInfo = function (index) {
        var result = new GridElementInfo_1.GridElementInfo();
        if (!this.getTask(index).isMilestone())
            result.size.width = this.getTaskWidth(index);
        result.className = this.getTaskClassName(index, result.size.width);
        return result;
    };
    GridLayoutCalculator.prototype.getTaskClassName = function (index, taskWidth) {
        var result = "dx-gantt-task";
        if (this.getTask(index).isMilestone())
            result += " dx-gantt-milestone";
        else if (taskWidth <= this.elementSizeValues.smallTaskWidth)
            result += " dx-gantt-smallTask";
        return result;
    };
    GridLayoutCalculator.prototype.getTaskPoint = function (index) {
        var result = this.getTaskWrapperPoint(index);
        result.y += this.elementSizeValues.taskWrapperTopPadding;
        return result;
    };
    GridLayoutCalculator.prototype.getTaskSize = function (index) {
        return new Size_1.Size(this.getTaskWidth(index), this.getTaskHeight(index));
    };
    GridLayoutCalculator.prototype.getTaskWidth = function (index) {
        var task = this.getTask(index);
        return task.isMilestone() ? this.getTaskHeight(index) : this.getWidthByDateRange(task.start, task.end);
    };
    GridLayoutCalculator.prototype.getTaskHeight = function (index) {
        var task = this.getTask(index);
        return task.isMilestone() ? this.milestoneWidth : this.taskHeight;
    };
    GridLayoutCalculator.prototype.getTask = function (index) {
        return this.viewModel.items[index].task;
    };
    GridLayoutCalculator.prototype.getTaskProgressElementInfo = function (index) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = "dx-gantt-tPrg";
        result.setSize(this.getTaskProgressSize(index));
        return result;
    };
    GridLayoutCalculator.prototype.getTaskProgressSize = function (index) {
        return new Size_1.Size(this.getTaskProgressWidth(index), null);
    };
    GridLayoutCalculator.prototype.getTaskProgressWidth = function (index) {
        return this.getTaskWidth(index) * this.getTask(index).progress / 100;
    };
    GridLayoutCalculator.prototype.getTaskTextElementInfo = function (index, isInsideText) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = this.getTaskTextElementClassName(isInsideText);
        if (!isInsideText) {
            var taskX = this.getTaskPoint(index).x;
            if (taskX < this.elementSizeValues.outsideTaskTextDefaultWidth) {
                result.size.width = taskX;
                result.margins.marginLeft = -taskX;
            }
        }
        return result;
    };
    GridLayoutCalculator.prototype.getTaskTextElementClassName = function (isInsideText) {
        var result = "dx-gantt-taskTitle";
        result += (isInsideText ? " dx-gantt-titleIn" : " dx-gantt-titleOut");
        return result;
    };
    GridLayoutCalculator.prototype.getTaskResourceElementInfo = function () {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = "dx-gantt-taskRes";
        return result;
    };
    GridLayoutCalculator.prototype.getSelectionElementInfo = function (index) {
        return this.getRowElementInfo(index, "dx-gantt-sel");
    };
    GridLayoutCalculator.prototype.getSelectionPosition = function (index) {
        var result = new Point_1.Point();
        result.y = index * this.tickSize.height;
        return result;
    };
    GridLayoutCalculator.prototype.getSelectionSize = function () {
        return new Size_1.Size(this.tickSize.width * this.horizontalTickCount, this.tickSize.height);
    };
    GridLayoutCalculator.prototype.getHighlightRowInfo = function (index) {
        return this.getRowElementInfo(index, "dx-gantt-altRow");
    };
    GridLayoutCalculator.prototype.getRowElementInfo = function (index, className) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = className;
        result.setPosition(this.getSelectionPosition(index));
        result.setSize(this.getSelectionSize());
        return result;
    };
    GridLayoutCalculator.prototype.getNoWorkingIntervalInfo = function (noWorkingDateRange) {
        var result = new GridElementInfo_1.GridElementInfo();
        result.className = "dx-gantt-nwi";
        result.setPosition(this.getNoWorkingIntervalPosition(noWorkingDateRange.start));
        result.setSize(this.getNoWorkingIntervalSize(noWorkingDateRange));
        return result;
    };
    GridLayoutCalculator.prototype.getNoWorkingIntervalPosition = function (intervalStart) {
        var result = new Point_1.Point();
        result.x = this.getPosByDate(intervalStart);
        return result;
    };
    GridLayoutCalculator.prototype.getNoWorkingIntervalSize = function (noWorkingInterval) {
        return new Size_1.Size(this.getWidthByDateRange(noWorkingInterval.start, noWorkingInterval.end), this.tickSize.height * this.verticalTickCount);
    };
    GridLayoutCalculator.prototype.getConnectorInfo = function (id, predessorIndex, successorIndex, connectorType) {
        var result = new Array();
        var connectorPoints = this.getConnectorPoints(predessorIndex, successorIndex, connectorType);
        for (var i = 0; i < connectorPoints.length - 1; i++)
            result.push(this.getConnectorLineInfo(id, connectorPoints[i], connectorPoints[i + 1], i == 0 || i == connectorPoints.length - 2));
        result.push(this.getArrowInfo(connectorPoints, result, predessorIndex, successorIndex));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorLineInfo = function (id, startPoint, endPoint, isEdgeLine) {
        var result = new GridElementInfo_1.GridElementInfo();
        var isVertical = startPoint.x == endPoint.x;
        result.className = this.getConnectorClassName(isVertical);
        result.setPosition(this.getConnectorPosition(startPoint, endPoint));
        result.setSize(this.getConnectorSize(startPoint, endPoint, isVertical, isEdgeLine));
        result.setAttribute("dependency-id", id);
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorClassName = function (isVertical) {
        return isVertical ? "dx-gantt-conn-v" : "dx-gantt-conn-h";
    };
    GridLayoutCalculator.prototype.getConnectorPosition = function (startPoint, endPoint) {
        return new Point_1.Point(Math.min(startPoint.x, endPoint.x), Math.min(startPoint.y, endPoint.y));
    };
    GridLayoutCalculator.prototype.getConnectorSize = function (startPoint, endPoint, isVertical, isEdgeLine) {
        var result = new Size_1.Size();
        var sizeCorrection = isEdgeLine ? 0 : 0;
        if (isVertical)
            result.height = Math.abs(endPoint.y - startPoint.y + sizeCorrection);
        else
            result.width = Math.abs(endPoint.x - startPoint.x + sizeCorrection);
        return result;
    };
    GridLayoutCalculator.prototype.getArrowInfo = function (connectorPoints, connectorLines, predessorIndex, successorIndex) {
        var result = new GridElementInfo_1.GridElementInfo();
        var lineInfo = this.findArrowLineInfo(connectorLines, predessorIndex, successorIndex);
        var arrowPosition = this.getArrowPosition(connectorPoints, predessorIndex, successorIndex);
        result.className = this.getArrowClassName(arrowPosition);
        result.setPosition(this.getArrowPoint(lineInfo, arrowPosition));
        return result;
    };
    GridLayoutCalculator.prototype.findArrowLineInfo = function (connectorLines, predessorIndex, successorIndex) {
        var arrowLineIndex = predessorIndex < successorIndex ? connectorLines.length - 1 : 0;
        return connectorLines[arrowLineIndex];
    };
    GridLayoutCalculator.prototype.getArrowPosition = function (connectorPoints, predessorIndex, successorIndex) {
        var prevLastPoint = connectorPoints[predessorIndex < successorIndex ? connectorPoints.length - 2 : 1];
        var lastPoint = connectorPoints[predessorIndex < successorIndex ? connectorPoints.length - 1 : 0];
        if (prevLastPoint.x == lastPoint.x)
            return prevLastPoint.y > lastPoint.y ? Enums_1.Position.Top : Enums_1.Position.Bottom;
        return prevLastPoint.x > lastPoint.x ? Enums_1.Position.Left : Enums_1.Position.Right;
    };
    GridLayoutCalculator.prototype.getArrowClassName = function (arrowPosition) {
        var result = "dx-gantt-arrow";
        switch (arrowPosition) {
            case Enums_1.Position.Left:
                result += " dx-gantt-LA";
                break;
            case Enums_1.Position.Top:
                result += " dx-gantt-TA";
                break;
            case Enums_1.Position.Right:
                result += " dx-gantt-RA";
                break;
            case Enums_1.Position.Bottom:
                result += " dx-gantt-BA";
                break;
        }
        return result;
    };
    GridLayoutCalculator.prototype.getArrowPoint = function (lineInfo, arrowPosition) {
        return new Point_1.Point(this.getArrowX(lineInfo, arrowPosition), this.getArrowY(lineInfo, arrowPosition));
    };
    GridLayoutCalculator.prototype.getArrowX = function (lineInfo, arrowPosition) {
        switch (arrowPosition) {
            case Enums_1.Position.Left:
                return lineInfo.position.x - this.arrowSize.width / 2;
            case Enums_1.Position.Right:
                return lineInfo.position.x + lineInfo.size.width - this.arrowSize.width / 2;
            case Enums_1.Position.Top:
            case Enums_1.Position.Bottom:
                return lineInfo.position.x - (this.arrowSize.width - this.lineThickness) / 2;
        }
    };
    GridLayoutCalculator.prototype.getArrowY = function (lineInfo, arrowPosition) {
        switch (arrowPosition) {
            case Enums_1.Position.Top:
                return lineInfo.position.y - this.arrowSize.height / 2;
            case Enums_1.Position.Bottom:
                return lineInfo.position.y + lineInfo.size.height - this.arrowSize.height / 2;
            case Enums_1.Position.Left:
            case Enums_1.Position.Right:
                return lineInfo.position.y - (this.arrowSize.height - this.lineThickness) / 2;
        }
    };
    GridLayoutCalculator.prototype.getPosByDate = function (date) {
        return this.getWidthByDateRange(this.range.start, date);
    };
    GridLayoutCalculator.prototype.getWidthByDateRange = function (start, end) {
        return this.getRangeTickCount(start, end) * this.tickSize.width;
    };
    GridLayoutCalculator.prototype.getRangeTickCount = function (start, end) {
        if (this.viewType == Enums_1.ViewType.Months)
            return this.getRangeTickCountInMonthsViewType(start, end);
        if (this.viewType == Enums_1.ViewType.Quarter)
            return this.getRangeTickCountInQuarterViewType(start, end);
        return (end.getTime() - start.getTime()) / this.tickTimeSpan;
    };
    GridLayoutCalculator.prototype.getRangeTickCountInMonthsViewType = function (start, end) {
        var startMonthStartDate = new Date(start.getFullYear(), start.getMonth(), 1);
        var endMonthStartDate = new Date(end.getFullYear(), end.getMonth(), 1);
        var monthOffset = Utils_1.DateUtils.getOffsetInMonths(startMonthStartDate, endMonthStartDate);
        var endFromMonthStartDateOffset = end.getTime() - endMonthStartDate.getTime();
        var startFromMonthStartDateOffset = start.getTime() - startMonthStartDate.getTime();
        return monthOffset + (endFromMonthStartDateOffset - startFromMonthStartDateOffset) / Utils_1.DateUtils.msPerMonth;
    };
    GridLayoutCalculator.prototype.getRangeTickCountInQuarterViewType = function (start, end) {
        var startQuarterStartDate = new Date(start.getFullYear(), Math.floor(start.getMonth() / 3) * 3, 1);
        var endQuarterStartDate = new Date(end.getFullYear(), Math.floor(end.getMonth() / 3) * 3, 1);
        var quarterOffset = Utils_1.DateUtils.getOffsetInQuarters(startQuarterStartDate, endQuarterStartDate);
        var endFromQuarterStartDateOffset = end.getTime() - endQuarterStartDate.getTime();
        var startFromQuarterStartDateOffset = start.getTime() - startQuarterStartDate.getTime();
        return quarterOffset + (endFromQuarterStartDateOffset - startFromQuarterStartDateOffset) / (Utils_1.DateUtils.msPerMonth * 3);
    };
    GridLayoutCalculator.prototype.getDateByPos = function (position) {
        var preResult = position / this.tickSize.width;
        var start = new Date(this.range.start);
        if (this.viewType == Enums_1.ViewType.Months || this.viewType == Enums_1.ViewType.Quarter) {
            var monthFromStart = Math.floor(preResult);
            start = new Date(start.setMonth(start.getMonth() + (this.viewType == Enums_1.ViewType.Months ? monthFromStart : monthFromStart * 3)));
            preResult = (position - this.getPosByDate(start)) / this.tickSize.width;
        }
        var time = preResult * this.tickTimeSpan + start.getTime();
        var result = new Date();
        result.setTime(time);
        return result;
    };
    GridLayoutCalculator.prototype.getTotalTickCount = function () {
        return this.getRangeTickCount(this.range.start, this.range.end);
    };
    GridLayoutCalculator.prototype.getConnectorPoints = function (predessorIndex, successorIndex, connectorType) {
        switch (connectorType) {
            case (Dependency_1.DependencyType.FS):
                return this.getFinishToStartConnectorPoints(predessorIndex, successorIndex);
            case (Dependency_1.DependencyType.SF):
                return this.getStartToFinishConnectorPoints(predessorIndex, successorIndex);
            case (Dependency_1.DependencyType.SS):
                return this.getStartToStartConnectorPoints(predessorIndex, successorIndex);
            case (Dependency_1.DependencyType.FF):
                return this.getFinishToFinishConnectorPoints(predessorIndex, successorIndex);
            default:
                return new Array();
        }
    };
    GridLayoutCalculator.prototype.getFinishToStartConnectorPoints = function (predessorIndex, successorIndex) {
        if (predessorIndex < successorIndex) {
            if (this.getTask(predessorIndex).end <= this.getTask(successorIndex).start)
                return this.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskTopSide(predessorIndex, successorIndex, false);
            return this.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskLeftSide(predessorIndex, successorIndex, false);
        }
        else {
            if (this.getTask(predessorIndex).end <= this.getTask(successorIndex).start)
                return this.getConnectorPoints_FromTopTaskBottomSide_ToBottomTaskRightSide(successorIndex, predessorIndex, false);
            return this.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskRightSide(successorIndex, predessorIndex, true);
        }
    };
    GridLayoutCalculator.prototype.getFinishToFinishConnectorPoints = function (predessorIndex, successorIndex) {
        if (predessorIndex < successorIndex) {
            return this.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskRightSide(predessorIndex, successorIndex);
        }
        else {
            return this.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskRightSide(successorIndex, predessorIndex);
        }
    };
    GridLayoutCalculator.prototype.getStartToStartConnectorPoints = function (predessorIndex, successorIndex) {
        if (predessorIndex < successorIndex) {
            return this.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskLeftSide(predessorIndex, successorIndex);
        }
        else {
            return this.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskLeftSide(successorIndex, predessorIndex);
        }
    };
    GridLayoutCalculator.prototype.getStartToFinishConnectorPoints = function (predessorIndex, successorIndex) {
        if (predessorIndex < successorIndex) {
            if (this.getTask(predessorIndex).start >= this.getTask(successorIndex).end)
                return this.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskTopSide(predessorIndex, successorIndex, true);
            return this.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskRightSide(predessorIndex, successorIndex, false);
        }
        else {
            if (this.getTask(predessorIndex).start >= this.getTask(successorIndex).end)
                return this.getConnectorPoints_FromTopTaskBottomSide_ToBottomTaskLeftSide(successorIndex, predessorIndex, true);
            return this.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskLeftSide(successorIndex, predessorIndex, true);
        }
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskTopSide = function (topTaskIndex, bottomTaskIndex, shiftEndPointToRight) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskRightCenter = this.getTaskRightCenter(topTaskPoint, topTaskIndex);
        var isBottomMilestone = this.getTask(bottomTaskIndex).isMilestone();
        var bottomTaskTopCenter = this.getTaskTopCenter(bottomTaskPoint, bottomTaskIndex);
        var endPointIndent = shiftEndPointToRight ? this.getTaskWidth(bottomTaskIndex) - this.minLineLength : this.minLineLength;
        result.push(new Point_1.Point(topTaskRightCenter.x, topTaskRightCenter.y));
        result.push(new Point_1.Point(isBottomMilestone ? bottomTaskTopCenter.x : bottomTaskPoint.x + endPointIndent, result[0].y));
        result.push(new Point_1.Point(result[1].x, bottomTaskTopCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskRightSide = function (topTaskIndex, bottomTaskIndex) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskRightCenter = this.getTaskRightCenter(topTaskPoint, topTaskIndex);
        var bottomTaskRightCenter = this.getTaskRightCenter(bottomTaskPoint, bottomTaskIndex);
        result.push(new Point_1.Point(topTaskRightCenter.x, topTaskRightCenter.y));
        result.push(new Point_1.Point(Math.max(topTaskRightCenter.x, bottomTaskRightCenter.x) + this.minLineLength, result[0].y));
        result.push(new Point_1.Point(result[1].x, bottomTaskRightCenter.y));
        result.push(new Point_1.Point(bottomTaskRightCenter.x, bottomTaskRightCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskRightSide_ToBottomTaskLeftSide = function (topTaskIndex, bottomTaskIndex, shiftToTop) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskRightCenter = this.getTaskRightCenter(topTaskPoint, topTaskIndex);
        var topTaskBottomCenter = this.getTaskBottomCenter(topTaskPoint, topTaskIndex);
        var bottomTaskLeftCenter = this.getTaskLeftCenter(bottomTaskPoint, bottomTaskIndex);
        var bottomTaskTopCenter = this.getTaskTopCenter(bottomTaskPoint, bottomTaskIndex);
        result.push(new Point_1.Point(topTaskRightCenter.x, topTaskRightCenter.y));
        result.push(new Point_1.Point(result[0].x + this.minLineLength, result[0].y));
        result.push(new Point_1.Point(result[1].x, shiftToTop ?
            topTaskBottomCenter.y + this.minConnectorSpaceFromTask
            : bottomTaskTopCenter.y - this.minConnectorSpaceFromTask));
        result.push(new Point_1.Point(bottomTaskLeftCenter.x - this.minLineLength, result[2].y));
        result.push(new Point_1.Point(result[3].x, bottomTaskLeftCenter.y));
        result.push(new Point_1.Point(bottomTaskLeftCenter.x, bottomTaskLeftCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskBottomSide_ToBottomTaskRightSide = function (topTaskIndex, bottomTaskIndex, shiftStartPointToRight) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskBottomCenter = this.getTaskBottomCenter(topTaskPoint, topTaskIndex);
        var isTopMilestone = this.getTask(topTaskIndex).isMilestone();
        var bottomTaskRightCenter = this.getTaskRightCenter(bottomTaskPoint, bottomTaskIndex);
        var startPointIndent = shiftStartPointToRight ? this.getTaskWidth(topTaskIndex) - this.minLineLength : this.minLineLength;
        result.push(new Point_1.Point(isTopMilestone ? topTaskBottomCenter.x : topTaskPoint.x + startPointIndent, topTaskBottomCenter.y));
        result.push(new Point_1.Point(result[0].x, bottomTaskRightCenter.y));
        result.push(new Point_1.Point(bottomTaskRightCenter.x, bottomTaskRightCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskBottomSide_ToBottomTaskLeftSide = function (topTaskIndex, bottomTaskIndex, shiftStartPointToRight) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskBottomCenter = this.getTaskBottomCenter(topTaskPoint, topTaskIndex);
        var isTopMilestone = this.getTask(topTaskIndex).isMilestone();
        var bottomTaskLeftCenter = this.getTaskLeftCenter(bottomTaskPoint, bottomTaskIndex);
        var startPointIndent = shiftStartPointToRight ? this.getTaskWidth(topTaskIndex) - this.minLineLength : this.minLineLength;
        result.push(new Point_1.Point(isTopMilestone ? topTaskBottomCenter.x : topTaskPoint.x + startPointIndent, topTaskBottomCenter.y));
        result.push(new Point_1.Point(result[0].x, bottomTaskLeftCenter.y));
        result.push(new Point_1.Point(bottomTaskLeftCenter.x, bottomTaskLeftCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskTopSide = function (topTaskIndex, bottomTaskIndex, shiftEndPointToRight) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskLeftCenter = this.getTaskLeftCenter(topTaskPoint, topTaskIndex);
        var bottomTaskTopCenter = this.getTaskTopCenter(bottomTaskPoint, bottomTaskIndex);
        var isBottomMilestone = this.getTask(bottomTaskIndex).isMilestone();
        var endPointIndent = shiftEndPointToRight ? this.getTaskWidth(bottomTaskIndex) - this.minLineLength : this.minLineLength;
        result.push(new Point_1.Point(topTaskLeftCenter.x, topTaskLeftCenter.y));
        result.push(new Point_1.Point(isBottomMilestone ? bottomTaskTopCenter.x : bottomTaskPoint.x + endPointIndent, result[0].y));
        result.push(new Point_1.Point(result[1].x, bottomTaskTopCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskRightSide = function (topTaskIndex, bottomTaskIndex, shiftToTop) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskLeftCenter = this.getTaskLeftCenter(topTaskPoint, topTaskIndex);
        var topTaskBottomCenter = this.getTaskBottomCenter(topTaskPoint, topTaskIndex);
        var bottomTaskRightCenter = this.getTaskRightCenter(bottomTaskPoint, bottomTaskIndex);
        var bottomTaskTopCenter = this.getTaskTopCenter(bottomTaskPoint, bottomTaskIndex);
        result.push(new Point_1.Point(topTaskLeftCenter.x, topTaskLeftCenter.y));
        result.push(new Point_1.Point(result[0].x - this.minLineLength, result[0].y));
        result.push(new Point_1.Point(result[1].x, shiftToTop ?
            topTaskBottomCenter.y + this.minConnectorSpaceFromTask
            : bottomTaskTopCenter.y - this.minConnectorSpaceFromTask));
        result.push(new Point_1.Point(bottomTaskRightCenter.x + this.minLineLength, result[2].y));
        result.push(new Point_1.Point(result[3].x, bottomTaskRightCenter.y));
        result.push(new Point_1.Point(bottomTaskRightCenter.x, bottomTaskRightCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getConnectorPoints_FromTopTaskLeftSide_ToBottomTaskLeftSide = function (topTaskIndex, bottomTaskIndex) {
        var result = new Array();
        var topTaskPoint = this.getTaskPoint(topTaskIndex);
        var bottomTaskPoint = this.getTaskPoint(bottomTaskIndex);
        var topTaskLeftCenter = this.getTaskLeftCenter(topTaskPoint, topTaskIndex);
        var bottomTaskLeftCenter = this.getTaskLeftCenter(bottomTaskPoint, bottomTaskIndex);
        result.push(new Point_1.Point(topTaskLeftCenter.x, topTaskLeftCenter.y));
        result.push(new Point_1.Point(Math.min(topTaskLeftCenter.x, bottomTaskLeftCenter.x) - this.minLineLength, result[0].y));
        result.push(new Point_1.Point(result[1].x, bottomTaskLeftCenter.y));
        result.push(new Point_1.Point(bottomTaskLeftCenter.x, bottomTaskLeftCenter.y));
        return result;
    };
    GridLayoutCalculator.prototype.getTaskLeftCenter = function (taskPoint, index) {
        return new Point_1.Point(taskPoint.x - this.getTaskEdgeCorrection(index), taskPoint.y + this.getTaskHeight(index) / 2);
    };
    GridLayoutCalculator.prototype.getTaskRightCenter = function (taskPoint, index) {
        return new Point_1.Point(taskPoint.x + this.getTaskWidth(index) + this.getTaskEdgeCorrection(index), taskPoint.y + this.getTaskHeight(index) / 2);
    };
    GridLayoutCalculator.prototype.getTaskTopCenter = function (taskPoint, index) {
        return new Point_1.Point(taskPoint.x + this.getTaskWidth(index) / 2, taskPoint.y - this.getTaskEdgeCorrection(index));
    };
    GridLayoutCalculator.prototype.getTaskBottomCenter = function (taskPoint, index) {
        return new Point_1.Point(taskPoint.x + this.getTaskWidth(index) / 2, taskPoint.y + this.getTaskHeight(index) + this.getTaskEdgeCorrection(index));
    };
    GridLayoutCalculator.prototype.getTaskEdgeCorrection = function (index) {
        var isMilestone = this.viewModel.items[index].task.isMilestone();
        return isMilestone ? this.getTaskHeight(index) * (Math.sqrt(2) - 1) / 2 : 0;
    };
    GridLayoutCalculator.prototype.getRenderedRowColumnIndices = function (scrollPos, isVertical) {
        var visibleAreaSizeValue = isVertical ? this.visibleTaskAreaSize.height : this.visibleTaskAreaSize.width;
        var tickSizeValue = isVertical ? this.tickSize.height : this.tickSize.width;
        var tickCount = isVertical ? this.verticalTickCount : this.horizontalTickCount;
        var firstVisibleIndex = this.getFirstVisibleGridCellIndex(scrollPos, tickSizeValue);
        var lastVisibleIndex = this.getLastVisibleGridCellIndex(scrollPos, tickSizeValue, visibleAreaSizeValue, tickCount);
        var result = new Array();
        for (var i = firstVisibleIndex; i <= lastVisibleIndex; i++)
            result.push(i);
        return result;
    };
    GridLayoutCalculator.prototype.getRenderedScaleItemIndices = function (scaleType, renderedColIndices) {
        var scaleItemColSpan = this.getScaleItemColSpan(scaleType);
        var firstVisibleIndex = Math.floor(renderedColIndices[0] / scaleItemColSpan);
        var lastVisibleIndex = Math.floor(renderedColIndices[renderedColIndices.length - 1] / scaleItemColSpan);
        var result = new Array();
        for (var i = firstVisibleIndex; i <= lastVisibleIndex; i++)
            result.push(i);
        return result;
    };
    GridLayoutCalculator.prototype.getFirstVisibleGridCellIndex = function (scrollPos, tickSizeValue) {
        var result = Math.floor(scrollPos / tickSizeValue);
        result = Math.max(result - 10, 0);
        return result;
    };
    GridLayoutCalculator.prototype.getLastVisibleGridCellIndex = function (scrollPos, tickSizeValue, visibleAreaSizeValue, tickCount) {
        var result = Math.floor((scrollPos + visibleAreaSizeValue) / tickSizeValue);
        result = Math.min(result + 10, tickCount - 1);
        return result;
    };
    GridLayoutCalculator.prototype.createTileToConnectorLinesMap = function () {
        var _this = this;
        this.tileToDependencyMap = [];
        for (var i = 0; i < this.viewModel.items.length; i++) {
            for (var j = 0; j < this.viewModel.items[i].dependencies.length; j++) {
                var predessorIndex = this.viewModel.items[i].dependencies[j].predecessor.visibleIndex;
                var successorIndex = this.viewModel.items[i].visibleIndex;
                var type = this.viewModel.items[i].dependencies[j].type;
                var id = this.viewModel.items[i].dependencies[j].id;
                var connectorInfo = this.getConnectorInfo(id, predessorIndex, successorIndex, type);
                connectorInfo.forEach(function (connectorLine) {
                    _this.addElementInfoToTileMap(connectorLine, _this.tileToDependencyMap, true);
                });
            }
        }
    };
    GridLayoutCalculator.prototype.createTileToNonWorkingIntervalsMap = function () {
        this.tileToNoWorkingIntervalsMap = [];
        for (var i = 0; i < this.viewModel.noWorkingIntervals.length; i++) {
            var noWorkingDateRange = this.getAdjustedNoWorkingInterval(this.viewModel.noWorkingIntervals[i]);
            if (!noWorkingDateRange)
                continue;
            var noWorkingIntervalInfo = this.getNoWorkingIntervalInfo(noWorkingDateRange);
            this.addElementInfoToTileMap(noWorkingIntervalInfo, this.tileToNoWorkingIntervalsMap, false);
        }
    };
    GridLayoutCalculator.prototype.getAdjustedNoWorkingInterval = function (modelInterval) {
        if (modelInterval.end.getTime() - modelInterval.start.getTime() < this.tickTimeSpan - 1)
            return null;
        return new DateRange_1.DateRange(Utils_1.DateUtils.getNearestScaleTickDate(modelInterval.start, this.range, this.tickTimeSpan, this.viewType), Utils_1.DateUtils.getNearestScaleTickDate(modelInterval.end, this.range, this.tickTimeSpan, this.viewType));
    };
    GridLayoutCalculator.prototype.addElementInfoToTileMap = function (info, map, isVerticalTile) {
        var infoPointValue = isVerticalTile ? info.position.y : info.position.x;
        var infoSizeValue = isVerticalTile ? info.size.height : info.size.width;
        var tileSizeValue = (isVerticalTile ? this.visibleTaskAreaSize.height : this.visibleTaskAreaSize.width) * 2;
        var firstTileIndex = Math.floor(infoPointValue / tileSizeValue);
        var lastTileIndex = Math.floor((infoPointValue + infoSizeValue) / tileSizeValue);
        for (var i = firstTileIndex; i <= lastTileIndex; i++) {
            if (!map[i])
                map[i] = new Array();
            map[i].push(info);
        }
    };
    GridLayoutCalculator.prototype.getRenderedConnectorLines = function (scrollPos) {
        return this.getElementsInRenderedTiles(this.tileToDependencyMap, true, scrollPos);
    };
    GridLayoutCalculator.prototype.getRenderedNoWorkingIntervals = function (scrollPos) {
        return this.getElementsInRenderedTiles(this.tileToNoWorkingIntervalsMap, false, scrollPos);
    };
    GridLayoutCalculator.prototype.getElementsInRenderedTiles = function (map, isVerticalTile, scrollPos) {
        var visibleAreaSizeValue = isVerticalTile ? this.visibleTaskAreaSize.height : this.visibleTaskAreaSize.width;
        var firstVisibleTileIndex = Math.floor(scrollPos / (visibleAreaSizeValue * 2));
        var lastVisibleTileIndex = Math.floor((scrollPos + visibleAreaSizeValue) / (visibleAreaSizeValue * 2));
        var result = new Array();
        for (var i = firstVisibleTileIndex; i <= lastVisibleTileIndex; i++) {
            if (!map[i])
                continue;
            map[i].forEach(function (info) {
                if (result.indexOf(info) == -1)
                    result.push(info);
            });
        }
        return result;
    };
    return GridLayoutCalculator;
}());
exports.GridLayoutCalculator = GridLayoutCalculator;


/***/ }),
/* 55 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var EtalonSizeValues = (function () {
    function EtalonSizeValues() {
        this.scaleItemWidths = new Map();
    }
    return EtalonSizeValues;
}());
exports.EtalonSizeValues = EtalonSizeValues;


/***/ }),
/* 56 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var DependencyManipulator_1 = __webpack_require__(57);
var ResourcesManipulator_1 = __webpack_require__(59);
var TaskManipulator_1 = __webpack_require__(60);
var ModelManipulator = (function () {
    function ModelManipulator(viewModel, dispatcher) {
        this.task = new TaskManipulator_1.TaskManipulator(viewModel, dispatcher);
        this.dependency = new DependencyManipulator_1.TaskDependencyManipulator(viewModel, dispatcher);
        this.resource = new ResourcesManipulator_1.ResourcesManipulator(viewModel, dispatcher);
    }
    return ModelManipulator;
}());
exports.ModelManipulator = ModelManipulator;


/***/ }),
/* 57 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var TaskPropertiesManipulator_1 = __webpack_require__(16);
var TaskDependencyManipulator = (function (_super) {
    __extends(TaskDependencyManipulator, _super);
    function TaskDependencyManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskDependencyManipulator.prototype.insertDependency = function (predecessorId, successorId, type) {
        var dependency = this.viewModel.dependencies.createItem();
        dependency.predecessorId = predecessorId;
        dependency.successorId = successorId;
        dependency.type = type;
        this.viewModel.dependencies.add(dependency);
        this.dispatcher.notifyDependencyInserted(this.getObjectForDataSource(dependency), function (id) { return dependency.id = id; });
        this.viewModel.updateVisibleItemDependencies();
        this.viewModel.owner.resetAndUpdate();
        return dependency;
    };
    TaskDependencyManipulator.prototype.removeDependency = function (dependencyId) {
        var dependency = this.viewModel.dependencies.getItemById(dependencyId);
        this.viewModel.dependencies.remove(dependency);
        this.dispatcher.notifyDependencyRemoved(dependency.id);
        this.viewModel.updateVisibleItemDependencies();
        this.viewModel.owner.resetAndUpdate();
        return dependency;
    };
    TaskDependencyManipulator.prototype.getObjectForDataSource = function (dependency) {
        return {
            id: dependency.id,
            predecessorId: this.viewModel.tasks.getItemById(dependency.predecessorId).id,
            successorId: this.viewModel.tasks.getItemById(dependency.successorId).id,
            type: dependency.type
        };
    };
    return TaskDependencyManipulator;
}(TaskPropertiesManipulator_1.BaseManipulator));
exports.TaskDependencyManipulator = TaskDependencyManipulator;


/***/ }),
/* 58 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItemState = (function () {
    function HistoryItemState(taskId, value) {
        this.taskId = taskId;
        this.value = value;
    }
    return HistoryItemState;
}());
exports.HistoryItemState = HistoryItemState;


/***/ }),
/* 59 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var TaskPropertiesManipulator_1 = __webpack_require__(16);
var ResourcesManipulator = (function (_super) {
    __extends(ResourcesManipulator, _super);
    function ResourcesManipulator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourcesManipulator.prototype.create = function (text, id) {
        var resource = this.viewModel.resources.createItem();
        resource.text = text;
        if (id)
            resource.internalId = id;
        this.viewModel.resources.add(resource);
        this.dispatcher.notifyResourceCreated(this.getResourceObjectForDataSource(resource), function (id) { return resource.id = id; });
        return resource;
    };
    ResourcesManipulator.prototype.remove = function (resourceId) {
        var resource = this.viewModel.resources.getItemById(resourceId);
        if (!resource)
            throw new Error("Invalid resource id");
        var assignments = this.viewModel.assignments.items.filter(function (a) { return a.resourceId == resourceId; });
        if (assignments.length)
            throw new Error("Can't delete assigned resource");
        this.viewModel.resources.remove(resource);
        this.dispatcher.notifyResourceRemoved(resource.id);
        return resource;
    };
    ResourcesManipulator.prototype.assign = function (resourceID, taskId) {
        var assignment = this.viewModel.assignments.createItem();
        assignment.resourceId = resourceID;
        assignment.taskId = taskId;
        this.viewModel.assignments.add(assignment);
        this.dispatcher.notifyResourceAssigned(this.getResourceAssignmentObjectForDataSource(assignment), function (id) { return assignment.id = id; });
        this.viewModel.updateModel();
        this.viewModel.owner.resetAndUpdate();
        return assignment;
    };
    ResourcesManipulator.prototype.deassig = function (assignmentId) {
        var assignment = this.viewModel.assignments.getItemById(assignmentId);
        this.viewModel.assignments.remove(assignment);
        this.dispatcher.notifyResourceUnassigned(assignment);
        this.viewModel.updateModel();
        this.viewModel.owner.resetAndUpdate();
        return assignment;
    };
    ResourcesManipulator.prototype.getResourceObjectForDataSource = function (resource) {
        return {
            id: resource.id,
            text: resource.text
        };
    };
    ResourcesManipulator.prototype.getResourceAssignmentObjectForDataSource = function (resourceAssignment) {
        return {
            id: resourceAssignment,
            taskId: this.viewModel.tasks.getItemById(resourceAssignment.taskId).id,
            resourceId: this.viewModel.resources.getItemById(resourceAssignment.resourceId).id
        };
    };
    return ResourcesManipulator;
}(TaskPropertiesManipulator_1.BaseManipulator));
exports.ResourcesManipulator = ResourcesManipulator;


/***/ }),
/* 60 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var TaskPropertiesManipulator_1 = __webpack_require__(16);
var TaskManipulator = (function (_super) {
    __extends(TaskManipulator, _super);
    function TaskManipulator(viewModel, dispatcher) {
        var _this = _super.call(this, viewModel, dispatcher) || this;
        _this.properties = new TaskPropertiesManipulator_1.TaskPropertiesManipulator(viewModel, dispatcher);
        return _this;
    }
    TaskManipulator.prototype.create = function (start, end, title, parentId, id) {
        var task = this.viewModel.tasks.createItem();
        task.start = start;
        task.end = end;
        task.title = title;
        if (parentId) {
            var parentItem = this.viewModel.tasks.getItemById(parentId);
            parentItem.expanded = true;
            task.parentId = parentId;
        }
        if (id) {
            task.internalId = id;
        }
        task.id = task.internalId;
        this.viewModel.tasks.add(task);
        this.viewModel.updateModel();
        this.dispatcher.notifyTaskCreated(this.getObjectForDataSource(task), function (id) { return task.id = id; });
        this.viewModel.owner.resetAndUpdate();
        return task;
    };
    TaskManipulator.prototype.remove = function (taskId) {
        var task = this.viewModel.tasks.getItemById(taskId);
        if (!task)
            throw new Error("Invalid task id");
        var dependencies = this.viewModel.dependencies.items.filter(function (d) { return d.predecessorId == taskId || d.successorId == taskId; });
        if (dependencies.length)
            throw new Error("Can't delete task with dependency");
        var assignments = this.viewModel.assignments.items.filter(function (a) { return a.taskId == taskId; });
        if (assignments.length)
            throw new Error("Can't delete task with assigned resource");
        this.viewModel.tasks.remove(task);
        this.dispatcher.notifyTaskRemoved(task.id);
        this.viewModel.updateModel();
        this.viewModel.owner.resetAndUpdate();
        return task;
    };
    TaskManipulator.prototype.getObjectForDataSource = function (task) {
        return {
            id: task.id,
            start: task.start,
            end: task.end,
            duration: task.duration,
            description: task.description,
            parentId: this.viewModel.tasks.getItemById(task.parentId).id,
            progress: task.progress,
            taskType: task.taskType,
            title: task.title,
            customFields: task.customFields,
            expanded: task.expanded
        };
    };
    return TaskManipulator;
}(TaskPropertiesManipulator_1.BaseManipulator));
exports.TaskManipulator = TaskManipulator;


/***/ }),
/* 61 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem_1 = __webpack_require__(9);
var History = (function () {
    function History() {
        this.historyItems = [];
        this.currentIndex = -1;
        this.transaction = null;
        this.transactionLevel = -1;
    }
    History.prototype.undo = function () {
        if (this.currentIndex < 0)
            return;
        this.historyItems[this.currentIndex].undo();
        this.currentIndex--;
    };
    History.prototype.redo = function () {
        if (this.currentIndex >= this.historyItems.length - 1)
            return;
        this.currentIndex++;
        this.historyItems[this.currentIndex].redo();
    };
    History.prototype.beginTransaction = function () {
        this.transactionLevel++;
        if (this.transactionLevel == 0)
            this.transaction = new HistoryItem_1.CompositionHistoryItem();
    };
    History.prototype.endTransaction = function () {
        if (--this.transactionLevel >= 0)
            return;
        var transactionLength = this.transaction.historyItems.length;
        if (transactionLength > 1)
            this.addInternal(this.transaction);
        else if (transactionLength == 1)
            this.addInternal(this.transaction.historyItems.pop());
        this.transaction = null;
    };
    History.prototype.addAndRedo = function (historyItem) {
        this.add(historyItem);
        historyItem.redo();
    };
    History.prototype.add = function (historyItem) {
        if (this.transactionLevel >= 0)
            this.transaction.add(historyItem);
        else
            this.addInternal(historyItem);
    };
    History.prototype.addInternal = function (historyItem) {
        if (this.currentIndex < this.historyItems.length - 1)
            this.historyItems.splice(this.currentIndex + 1);
        this.historyItems.push(historyItem);
        this.currentIndex++;
        this.deleteOldItems();
    };
    History.prototype.deleteOldItems = function () {
        var exceedItemsCount = this.historyItems.length - History.MAX_HISTORY_ITEM_COUNT;
        if (exceedItemsCount > 0 && this.currentIndex > exceedItemsCount) {
            this.historyItems.splice(0, exceedItemsCount);
            this.currentIndex -= exceedItemsCount;
        }
    };
    History.prototype.clear = function () {
        this.currentIndex = -1;
        this.historyItems = [];
    };
    History.MAX_HISTORY_ITEM_COUNT = 100;
    return History;
}());
exports.History = History;


/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var MouseHandler_1 = __webpack_require__(63);
var DomUtils_1 = __webpack_require__(2);
var KeyCode_1 = __webpack_require__(71);
var Browser_1 = __webpack_require__(14);
var EventManager = (function () {
    function EventManager(control) {
        this.control = control;
        this.mouseHandler = new MouseHandler_1.MouseHandler(control);
    }
    EventManager.prototype.onMouseDown = function (evt) {
        this.mouseHandler.onMouseDown(evt);
    };
    EventManager.prototype.onMouseMove = function (evt) {
        this.mouseHandler.onMouseMove(evt);
    };
    EventManager.prototype.onMouseUp = function (evt) {
        this.mouseHandler.onMouseUp(evt);
    };
    EventManager.prototype.onMouseDblClick = function (evt) {
        this.mouseHandler.onMouseDoubleClick(evt);
    };
    EventManager.prototype.onMouseWheel = function (evt) {
        this.mouseHandler.onMouseWheel(evt);
    };
    EventManager.prototype.onKeyDown = function (evt) {
        if (this.control.isFocus) {
            var code = this.getShortcutCode(evt);
            if (code == (KeyCode_1.ModifierKey.Ctrl | KeyCode_1.KeyCode.Key_z))
                this.control.history.undo();
            if (code == (KeyCode_1.ModifierKey.Ctrl | KeyCode_1.KeyCode.Key_y))
                this.control.history.redo();
            if (code == KeyCode_1.KeyCode.Delete)
                this.control.taskEditController.deleteSelectedDependency();
        }
    };
    EventManager.prototype.getShortcutCode = function (evt) {
        var keyCode = DomUtils_1.DomUtils.GetKeyCode(evt);
        var modifiers = 0;
        if (evt.altKey)
            modifiers |= KeyCode_1.ModifierKey.Alt;
        if (evt.ctrlKey)
            modifiers |= KeyCode_1.ModifierKey.Ctrl;
        if (evt.shiftKey)
            modifiers |= KeyCode_1.ModifierKey.Shift;
        if (evt.metaKey && Browser_1.Browser.MacOSPlatform)
            modifiers |= KeyCode_1.ModifierKey.Meta;
        return modifiers | keyCode;
    };
    return EventManager;
}());
exports.EventManager = EventManager;


/***/ }),
/* 63 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HandlerBase_1 = __webpack_require__(64);
var TaskAreaManager_1 = __webpack_require__(8);
var TaskEditController_1 = __webpack_require__(29);
var MouseHandlerDefaultState_1 = __webpack_require__(65);
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerMoveTaskState_1 = __webpack_require__(67);
var MouseHandlerProgressTaskState_1 = __webpack_require__(68);
var MouseHandlerTimestampTaskState_1 = __webpack_require__(69);
var MouseHandlerDependencyState_1 = __webpack_require__(70);
var MouseHandler = (function (_super) {
    __extends(MouseHandler, _super);
    function MouseHandler() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandler.prototype.onMouseDoubleClick = function (evt) {
        this.state.onMouseDoubleClick(evt);
    };
    MouseHandler.prototype.onMouseDown = function (evt) {
        var source = this.getMouseEventSource(DomUtils_1.DomUtils.getEventSource(evt));
        switch (source) {
            case TaskAreaManager_1.MouseEventSource.TaskEdit_Frame:
                this.switchState(new MouseHandlerMoveTaskState_1.MouseHandlerMoveTaskState(this));
                break;
            case TaskAreaManager_1.MouseEventSource.TaskEdit_Progress:
                this.switchState(new MouseHandlerProgressTaskState_1.MouseHandlerProgressTaskState(this));
                break;
            case TaskAreaManager_1.MouseEventSource.TaskEdit_Start:
            case TaskAreaManager_1.MouseEventSource.TaskEdit_End:
                this.switchState(new MouseHandlerTimestampTaskState_1.MouseHandlerTimestampTaskState(this));
                break;
            case TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyStart:
            case TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyFinish:
                this.switchState(new MouseHandlerDependencyState_1.MouseHandlerDependencyState(this));
                break;
        }
        this.state.onMouseDown(evt);
    };
    MouseHandler.prototype.onMouseUp = function (evt) {
        this.state.onMouseUp(evt);
    };
    MouseHandler.prototype.onMouseMove = function (evt) {
        this.state.onMouseMove(evt);
        evt.preventDefault();
    };
    MouseHandler.prototype.onMouseWheel = function (evt) {
        this.state.onMouseWheel(evt);
    };
    MouseHandler.prototype.switchToDefaultState = function () {
        this.state = new MouseHandlerDefaultState_1.MouseHandlerDefaultState(this);
    };
    MouseHandler.prototype.getMouseEventSource = function (initSource) {
        var source = initSource.nodeType === Node.ELEMENT_NODE ? initSource : initSource.parentNode;
        var className = source.className;
        return TaskEditController_1.TaskEditController.classToSource.get(className) || TaskAreaManager_1.MouseEventSource.TaskArea;
    };
    return MouseHandler;
}(HandlerBase_1.HandlerBase));
exports.MouseHandler = MouseHandler;


/***/ }),
/* 64 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HandlerBase = (function () {
    function HandlerBase(control) {
        this.control = control;
        this.switchToDefaultState();
    }
    HandlerBase.prototype.switchState = function (state) {
        if (this.state)
            this.state.finish();
        this.state = state;
        this.state.start();
    };
    HandlerBase.prototype.switchToDefaultState = function () {
        throw new Error("Not implemented");
    };
    return HandlerBase;
}());
exports.HandlerBase = HandlerBase;


/***/ }),
/* 65 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerStateBase_1 = __webpack_require__(17);
var Point_1 = __webpack_require__(3);
var MouseHandlerDefaultState = (function (_super) {
    __extends(MouseHandlerDefaultState, _super);
    function MouseHandlerDefaultState(handler) {
        var _this = _super.call(this, handler) || this;
        _this.ganttMovingHelper = new GanttMovingHelper(_this.handler.control);
        return _this;
    }
    MouseHandlerDefaultState.prototype.onMouseDown = function (evt) {
        evt.preventDefault();
        var source = DomUtils_1.DomUtils.getEventSource(evt);
        if (source.className == this.handler.control.gridLayoutCalculator.getConnectorClassName(true) ||
            source.className == this.handler.control.gridLayoutCalculator.getConnectorClassName(false)) {
            var id = source.getAttribute("dependency-id");
            this.handler.control.taskEditController.selectDependency(id);
            this.handler.control.renderedConnectorLines = [];
            this.handler.control.recreateConnectorLineElements();
        }
        else {
            if (DomUtils_1.DomUtils.IsLeftButtonPressed(evt))
                this.ganttMovingHelper.startMoving(evt);
            this.handler.control.taskEditController.selectDependency(null);
        }
    };
    MouseHandlerDefaultState.prototype.onMouseUp = function (evt) {
        this.ganttMovingHelper.onMouseUp(evt);
    };
    MouseHandlerDefaultState.prototype.onMouseMove = function (evt) {
        if (this.ganttMovingHelper.movingInfo) {
            this.ganttMovingHelper.onMouseMove(evt);
        }
    };
    MouseHandlerDefaultState.prototype.onMouseWheel = function (evt) {
        if (evt.ctrlKey) {
            evt.preventDefault();
            evt.stopPropagation();
            var increase = DomUtils_1.DomUtils.getWheelDelta(evt) > 0;
            var absolutePos = new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt));
            var currentPos = this.getRelativePos(new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt)));
            var currentDate = this.handler.control.gridLayoutCalculator.getDateByPos(currentPos.x);
            if (increase)
                this.handler.control.zoomIn();
            else
                this.handler.control.zoomOut();
            var scrollLeft = this.handler.control.gridLayoutCalculator.getPosByDate(currentDate);
            this.handler.control.taskAreaContainer.scrollLeft = scrollLeft -
                (absolutePos.x - DomUtils_1.DomUtils.getAbsolutePositionX(this.handler.control.taskAreaContainer.getElement()));
        }
    };
    return MouseHandlerDefaultState;
}(MouseHandlerStateBase_1.MouseHandlerStateBase));
exports.MouseHandlerDefaultState = MouseHandlerDefaultState;
var GanttMovingHelper = (function () {
    function GanttMovingHelper(gantt) {
        this.gantt = gantt;
        this.movingInfo = null;
    }
    GanttMovingHelper.prototype.startMoving = function (e) {
        this.movingInfo = this.calcMovingInfo(e);
        this.updateGanttAreaCursor(true);
    };
    GanttMovingHelper.prototype.cancelMoving = function () {
        this.movingInfo = null;
    };
    GanttMovingHelper.prototype.onMouseMove = function (e) {
        this.move(e);
    };
    GanttMovingHelper.prototype.onMouseUp = function (e) {
        this.cancelMoving();
        this.updateGanttAreaCursor(false);
    };
    GanttMovingHelper.prototype.move = function (e) {
        this.updateScrollPosition(e);
    };
    GanttMovingHelper.prototype.updateScrollPosition = function (e) {
        var newEventX = Math.round(DomUtils_1.DomUtils.getEventX(e));
        var newEventY = Math.round(DomUtils_1.DomUtils.getEventY(e));
        var deltaX = newEventX - this.movingInfo.eventX;
        var deltaY = newEventY - this.movingInfo.eventY;
        var dirX = deltaX < 0 ? -1 : 1;
        var dirY = deltaY < 0 ? -1 : 1;
        var maxDeltaX = dirX < 0 ? this.movingInfo.maxRightDelta : this.movingInfo.maxLeftDelta;
        var maxDeltaY = dirY < 0 ? this.movingInfo.maxBottomDelta : this.movingInfo.maxTopDelta;
        if (Math.abs(deltaX) > maxDeltaX)
            deltaX = maxDeltaX * dirX;
        if (Math.abs(deltaY) > maxDeltaY)
            deltaY = maxDeltaY * dirY;
        var newScrollLeft = this.movingInfo.scrollLeft - deltaX;
        var newScrollTop = this.movingInfo.scrollTop - deltaY;
        var taskAreaContainer = this.gantt.taskAreaContainer;
        if (taskAreaContainer.scrollLeft !== newScrollLeft)
            taskAreaContainer.scrollLeft = newScrollLeft;
        if (taskAreaContainer.scrollTop !== newScrollTop)
            taskAreaContainer.scrollTop = newScrollTop;
    };
    GanttMovingHelper.prototype.calcMovingInfo = function (e) {
        var taskAreaContainer = this.gantt.taskAreaContainer;
        return {
            eventX: DomUtils_1.DomUtils.getEventX(e),
            eventY: DomUtils_1.DomUtils.getEventY(e),
            scrollLeft: taskAreaContainer.scrollLeft,
            scrollTop: taskAreaContainer.scrollTop,
            maxLeftDelta: taskAreaContainer.scrollLeft,
            maxRightDelta: taskAreaContainer.scrollWidth - taskAreaContainer.scrollLeft - taskAreaContainer.getElement().offsetWidth,
            maxTopDelta: taskAreaContainer.scrollTop,
            maxBottomDelta: taskAreaContainer.scrollHeight - taskAreaContainer.scrollTop - taskAreaContainer.getElement().offsetHeight
        };
    };
    GanttMovingHelper.prototype.updateGanttAreaCursor = function (drag) {
        this.gantt.taskAreaContainer.getElement().style.cursor = drag ? "grabbing" : "default";
    };
    return GanttMovingHelper;
}());
exports.GanttMovingHelper = GanttMovingHelper;


/***/ }),
/* 66 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HandlerStateBase = (function () {
    function HandlerStateBase(handler) {
        this.handler = handler;
    }
    HandlerStateBase.prototype.start = function () { };
    HandlerStateBase.prototype.finish = function () { };
    return HandlerStateBase;
}());
exports.HandlerStateBase = HandlerStateBase;


/***/ }),
/* 67 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var MouseHandlerDragTaskBaseState_1 = __webpack_require__(18);
var MouseHandlerMoveTaskState = (function (_super) {
    __extends(MouseHandlerMoveTaskState, _super);
    function MouseHandlerMoveTaskState() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerMoveTaskState.prototype.onMouseUpInternal = function (_evt) {
        this.handler.control.taskEditController.confirmMove();
    };
    MouseHandlerMoveTaskState.prototype.onMouseMoveInternal = function (position) {
        this.handler.control.taskEditController.processMove(position.x - this.currentPosition.x);
    };
    return MouseHandlerMoveTaskState;
}(MouseHandlerDragTaskBaseState_1.MouseHandlerDragBaseState));
exports.MouseHandlerMoveTaskState = MouseHandlerMoveTaskState;


/***/ }),
/* 68 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var MouseHandlerDragTaskBaseState_1 = __webpack_require__(18);
var MouseHandlerProgressTaskState = (function (_super) {
    __extends(MouseHandlerProgressTaskState, _super);
    function MouseHandlerProgressTaskState() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerProgressTaskState.prototype.onMouseUpInternal = function (_evt) {
        this.handler.control.taskEditController.confirmProgress();
    };
    MouseHandlerProgressTaskState.prototype.onMouseMoveInternal = function (position) {
        var relativePosition = this.getRelativePos(position);
        this.handler.control.taskEditController.processProgress(relativePosition);
    };
    return MouseHandlerProgressTaskState;
}(MouseHandlerDragTaskBaseState_1.MouseHandlerDragBaseState));
exports.MouseHandlerProgressTaskState = MouseHandlerProgressTaskState;


/***/ }),
/* 69 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var TaskAreaManager_1 = __webpack_require__(8);
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerDragTaskBaseState_1 = __webpack_require__(18);
var MouseHandlerTimestampTaskState = (function (_super) {
    __extends(MouseHandlerTimestampTaskState, _super);
    function MouseHandlerTimestampTaskState() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerTimestampTaskState.prototype.onMouseDown = function (evt) {
        _super.prototype.onMouseDown.call(this, evt);
        this.source = this.handler.getMouseEventSource(DomUtils_1.DomUtils.getEventSource(evt));
    };
    MouseHandlerTimestampTaskState.prototype.onMouseUpInternal = function (_evt) {
        if (this.source == TaskAreaManager_1.MouseEventSource.TaskEdit_Start)
            this.handler.control.taskEditController.confirmStart();
        else
            this.handler.control.taskEditController.confirmEnd();
    };
    MouseHandlerTimestampTaskState.prototype.onMouseMoveInternal = function (position) {
        var relativePosition = this.getRelativePos(position);
        if (this.source == TaskAreaManager_1.MouseEventSource.TaskEdit_Start)
            this.handler.control.taskEditController.processStart(relativePosition);
        if (this.source == TaskAreaManager_1.MouseEventSource.TaskEdit_End)
            this.handler.control.taskEditController.processEnd(relativePosition);
    };
    return MouseHandlerTimestampTaskState;
}(MouseHandlerDragTaskBaseState_1.MouseHandlerDragBaseState));
exports.MouseHandlerTimestampTaskState = MouseHandlerTimestampTaskState;


/***/ }),
/* 70 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Point_1 = __webpack_require__(3);
var DomUtils_1 = __webpack_require__(2);
var MouseHandlerStateBase_1 = __webpack_require__(17);
var TaskAreaManager_1 = __webpack_require__(8);
var Dependency_1 = __webpack_require__(13);
var dependencyMap = [];
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyStart] = [];
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyFinish] = [];
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyStart][TaskAreaManager_1.MouseEventSource.Successor_DependencyStart] = Dependency_1.DependencyType.SS;
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyStart][TaskAreaManager_1.MouseEventSource.Successor_DependencyFinish] = Dependency_1.DependencyType.SF;
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyFinish][TaskAreaManager_1.MouseEventSource.Successor_DependencyStart] = Dependency_1.DependencyType.FS;
dependencyMap[TaskAreaManager_1.MouseEventSource.TaskEdit_DependencyFinish][TaskAreaManager_1.MouseEventSource.Successor_DependencyFinish] = Dependency_1.DependencyType.FF;
var MouseHandlerDependencyState = (function (_super) {
    __extends(MouseHandlerDependencyState, _super);
    function MouseHandlerDependencyState() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MouseHandlerDependencyState.prototype.onMouseDown = function (evt) {
        var sourceElement = DomUtils_1.DomUtils.getEventSource(evt);
        this.source = this.handler.getMouseEventSource(sourceElement);
        var pos = this.getRelativePos(new Point_1.Point(DomUtils_1.DomUtils.getAbsolutePositionX(sourceElement) + sourceElement.clientWidth / 2, DomUtils_1.DomUtils.getAbsolutePositionY(sourceElement) + sourceElement.clientHeight / 2));
        this.handler.control.taskEditController.startDependency(pos);
    };
    MouseHandlerDependencyState.prototype.onMouseUp = function (evt) {
        var target = this.handler.getMouseEventSource(DomUtils_1.DomUtils.getEventSource(evt));
        var type = target === TaskAreaManager_1.MouseEventSource.Successor_DependencyStart || target == TaskAreaManager_1.MouseEventSource.Successor_DependencyFinish ?
            dependencyMap[this.source][target] : null;
        this.handler.control.taskEditController.endDependency(type);
        this.handler.switchToDefaultState();
    };
    MouseHandlerDependencyState.prototype.onMouseMove = function (evt) {
        var relativePos = this.getRelativePos(new Point_1.Point(DomUtils_1.DomUtils.getEventX(evt), DomUtils_1.DomUtils.getEventY(evt)));
        var hoverTaskIndex = Math.floor(relativePos.y / this.handler.control.tickSize.height);
        this.handler.control.taskEditController.processDependency(relativePos);
        if (this.handler.control.viewModel.tasks.items[hoverTaskIndex])
            this.handler.control.taskEditController.showDependencySuccessor(hoverTaskIndex);
    };
    return MouseHandlerDependencyState;
}(MouseHandlerStateBase_1.MouseHandlerStateBase));
exports.MouseHandlerDependencyState = MouseHandlerDependencyState;


/***/ }),
/* 71 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ModifierKey;
(function (ModifierKey) {
    ModifierKey[ModifierKey["None"] = 0] = "None";
    ModifierKey[ModifierKey["Ctrl"] = 65536] = "Ctrl";
    ModifierKey[ModifierKey["Shift"] = 262144] = "Shift";
    ModifierKey[ModifierKey["Alt"] = 1048576] = "Alt";
    ModifierKey[ModifierKey["Meta"] = 16777216] = "Meta";
})(ModifierKey = exports.ModifierKey || (exports.ModifierKey = {}));
var KeyCode;
(function (KeyCode) {
    KeyCode[KeyCode["Backspace"] = 8] = "Backspace";
    KeyCode[KeyCode["Tab"] = 9] = "Tab";
    KeyCode[KeyCode["Enter"] = 13] = "Enter";
    KeyCode[KeyCode["Pause"] = 19] = "Pause";
    KeyCode[KeyCode["CapsLock"] = 20] = "CapsLock";
    KeyCode[KeyCode["Esc"] = 27] = "Esc";
    KeyCode[KeyCode["Space"] = 32] = "Space";
    KeyCode[KeyCode["PageUp"] = 33] = "PageUp";
    KeyCode[KeyCode["PageDown"] = 34] = "PageDown";
    KeyCode[KeyCode["End"] = 35] = "End";
    KeyCode[KeyCode["Home"] = 36] = "Home";
    KeyCode[KeyCode["Left"] = 37] = "Left";
    KeyCode[KeyCode["Up"] = 38] = "Up";
    KeyCode[KeyCode["Right"] = 39] = "Right";
    KeyCode[KeyCode["Down"] = 40] = "Down";
    KeyCode[KeyCode["Insert"] = 45] = "Insert";
    KeyCode[KeyCode["Delete"] = 46] = "Delete";
    KeyCode[KeyCode["Key_0"] = 48] = "Key_0";
    KeyCode[KeyCode["Key_1"] = 49] = "Key_1";
    KeyCode[KeyCode["Key_2"] = 50] = "Key_2";
    KeyCode[KeyCode["Key_3"] = 51] = "Key_3";
    KeyCode[KeyCode["Key_4"] = 52] = "Key_4";
    KeyCode[KeyCode["Key_5"] = 53] = "Key_5";
    KeyCode[KeyCode["Key_6"] = 54] = "Key_6";
    KeyCode[KeyCode["Key_7"] = 55] = "Key_7";
    KeyCode[KeyCode["Key_8"] = 56] = "Key_8";
    KeyCode[KeyCode["Key_9"] = 57] = "Key_9";
    KeyCode[KeyCode["Key_a"] = 65] = "Key_a";
    KeyCode[KeyCode["Key_b"] = 66] = "Key_b";
    KeyCode[KeyCode["Key_c"] = 67] = "Key_c";
    KeyCode[KeyCode["Key_d"] = 68] = "Key_d";
    KeyCode[KeyCode["Key_e"] = 69] = "Key_e";
    KeyCode[KeyCode["Key_f"] = 70] = "Key_f";
    KeyCode[KeyCode["Key_g"] = 71] = "Key_g";
    KeyCode[KeyCode["Key_h"] = 72] = "Key_h";
    KeyCode[KeyCode["Key_i"] = 73] = "Key_i";
    KeyCode[KeyCode["Key_j"] = 74] = "Key_j";
    KeyCode[KeyCode["Key_k"] = 75] = "Key_k";
    KeyCode[KeyCode["Key_l"] = 76] = "Key_l";
    KeyCode[KeyCode["Key_m"] = 77] = "Key_m";
    KeyCode[KeyCode["Key_n"] = 78] = "Key_n";
    KeyCode[KeyCode["Key_o"] = 79] = "Key_o";
    KeyCode[KeyCode["Key_p"] = 80] = "Key_p";
    KeyCode[KeyCode["Key_q"] = 81] = "Key_q";
    KeyCode[KeyCode["Key_r"] = 82] = "Key_r";
    KeyCode[KeyCode["Key_s"] = 83] = "Key_s";
    KeyCode[KeyCode["Key_t"] = 84] = "Key_t";
    KeyCode[KeyCode["Key_u"] = 85] = "Key_u";
    KeyCode[KeyCode["Key_v"] = 86] = "Key_v";
    KeyCode[KeyCode["Key_w"] = 87] = "Key_w";
    KeyCode[KeyCode["Key_x"] = 88] = "Key_x";
    KeyCode[KeyCode["Key_y"] = 89] = "Key_y";
    KeyCode[KeyCode["Key_z"] = 90] = "Key_z";
    KeyCode[KeyCode["Windows"] = 91] = "Windows";
    KeyCode[KeyCode["ContextMenu"] = 93] = "ContextMenu";
    KeyCode[KeyCode["Numpad_0"] = 96] = "Numpad_0";
    KeyCode[KeyCode["Numpad_1"] = 97] = "Numpad_1";
    KeyCode[KeyCode["Numpad_2"] = 98] = "Numpad_2";
    KeyCode[KeyCode["Numpad_3"] = 99] = "Numpad_3";
    KeyCode[KeyCode["Numpad_4"] = 100] = "Numpad_4";
    KeyCode[KeyCode["Numpad_5"] = 101] = "Numpad_5";
    KeyCode[KeyCode["Numpad_6"] = 102] = "Numpad_6";
    KeyCode[KeyCode["Numpad_7"] = 103] = "Numpad_7";
    KeyCode[KeyCode["Numpad_8"] = 104] = "Numpad_8";
    KeyCode[KeyCode["Numpad_9"] = 105] = "Numpad_9";
    KeyCode[KeyCode["Multiply"] = 106] = "Multiply";
    KeyCode[KeyCode["Add"] = 107] = "Add";
    KeyCode[KeyCode["Subtract"] = 109] = "Subtract";
    KeyCode[KeyCode["Decimal"] = 110] = "Decimal";
    KeyCode[KeyCode["Divide"] = 111] = "Divide";
    KeyCode[KeyCode["F1"] = 112] = "F1";
    KeyCode[KeyCode["F2"] = 113] = "F2";
    KeyCode[KeyCode["F3"] = 114] = "F3";
    KeyCode[KeyCode["F4"] = 115] = "F4";
    KeyCode[KeyCode["F5"] = 116] = "F5";
    KeyCode[KeyCode["F6"] = 117] = "F6";
    KeyCode[KeyCode["F7"] = 118] = "F7";
    KeyCode[KeyCode["F8"] = 119] = "F8";
    KeyCode[KeyCode["F9"] = 120] = "F9";
    KeyCode[KeyCode["F10"] = 121] = "F10";
    KeyCode[KeyCode["F11"] = 122] = "F11";
    KeyCode[KeyCode["F12"] = 123] = "F12";
    KeyCode[KeyCode["NumLock"] = 144] = "NumLock";
    KeyCode[KeyCode["ScrollLock"] = 145] = "ScrollLock";
    KeyCode[KeyCode["Semicolon"] = 186] = "Semicolon";
    KeyCode[KeyCode["Equals"] = 187] = "Equals";
    KeyCode[KeyCode["Comma"] = 188] = "Comma";
    KeyCode[KeyCode["Dash"] = 189] = "Dash";
    KeyCode[KeyCode["Period"] = 190] = "Period";
    KeyCode[KeyCode["ForwardSlash"] = 191] = "ForwardSlash";
    KeyCode[KeyCode["GraveAccent"] = 192] = "GraveAccent";
    KeyCode[KeyCode["OpenBracket"] = 219] = "OpenBracket";
    KeyCode[KeyCode["BackSlash"] = 220] = "BackSlash";
    KeyCode[KeyCode["CloseBracket"] = 221] = "CloseBracket";
    KeyCode[KeyCode["SingleQuote"] = 222] = "SingleQuote";
})(KeyCode = exports.KeyCode || (exports.KeyCode = {}));


/***/ }),
/* 72 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var TaskAreaContainer = (function () {
    function TaskAreaContainer(element, ganttView) {
        this.element = element;
        this.element.addEventListener("scroll", function () { ganttView.updateView(); });
    }
    Object.defineProperty(TaskAreaContainer.prototype, "scrollTop", {
        get: function () {
            return this.element.scrollTop;
        },
        set: function (value) {
            this.element.scrollTop = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskAreaContainer.prototype, "scrollLeft", {
        get: function () {
            return this.element.scrollLeft;
        },
        set: function (value) {
            this.element.scrollLeft = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskAreaContainer.prototype, "scrollWidth", {
        get: function () {
            return this.element.scrollWidth;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskAreaContainer.prototype, "scrollHeight", {
        get: function () {
            return this.element.scrollHeight;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskAreaContainer.prototype, "isExternal", {
        get: function () {
            return false;
        },
        enumerable: true,
        configurable: true
    });
    TaskAreaContainer.prototype.getWidth = function () {
        return this.element.offsetWidth;
    };
    TaskAreaContainer.prototype.getHeight = function () {
        return this.element.offsetHeight;
    };
    TaskAreaContainer.prototype.getElement = function () {
        return this.element;
    };
    return TaskAreaContainer;
}());
exports.TaskAreaContainer = TaskAreaContainer;


/***/ }),
/* 73 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Enums_1 = __webpack_require__(7);
var Utils_1 = __webpack_require__(0);
var Settings = (function () {
    function Settings() {
        this.viewType = undefined;
        this.taskTitlePosition = Enums_1.TaskTitlePosition.Inside;
        this.showResources = true;
        this.areHorizontalBordersEnabled = true;
        this.areVerticalBordersEnabled = true;
        this.areAlternateRowsEnabled = true;
        this.allowSelectTask = true;
        this.editing = new EditingSettings();
    }
    Settings.parse = function (settings) {
        var result = new Settings();
        if (settings) {
            if (Utils_1.JsonUtils.isExists(settings.viewType))
                result.viewType = settings.viewType;
            if (Utils_1.JsonUtils.isExists(settings.taskTitlePosition))
                result.taskTitlePosition = settings.taskTitlePosition;
            if (Utils_1.JsonUtils.isExists(settings.showResources))
                result.showResources = settings.showResources;
            if (Utils_1.JsonUtils.isExists(settings.areHorizontalBordersEnabled))
                result.areHorizontalBordersEnabled = settings.areHorizontalBordersEnabled;
            if (Utils_1.JsonUtils.isExists(settings.areVerticalBordersEnabled))
                result.areHorizontalBordersEnabled = settings.areHorizontalBordersEnabled;
            if (Utils_1.JsonUtils.isExists(settings.areAlternateRowsEnabled))
                result.areAlternateRowsEnabled = settings.areAlternateRowsEnabled;
            if (Utils_1.JsonUtils.isExists(settings.allowSelectTask))
                result.allowSelectTask = settings.allowSelectTask;
            if (Utils_1.JsonUtils.isExists(settings.editing))
                result.editing = EditingSettings.parse(settings.editing);
        }
        return result;
    };
    return Settings;
}());
exports.Settings = Settings;
var EditingSettings = (function () {
    function EditingSettings() {
        this.enabled = false;
        this.allowDependencyDeleting = true;
        this.allowDependencyAdding = true;
        this.allowDependencyUpdating = true;
        this.allowTaskDeleting = true;
        this.allowTaskAdding = true;
        this.allowTaskUpdating = true;
        this.allowResourceDeleting = true;
        this.allowResourceAdding = true;
        this.allowResourceUpdating = true;
    }
    EditingSettings.parse = function (settings) {
        var result = new EditingSettings();
        if (settings) {
            if (Utils_1.JsonUtils.isExists(settings.enabled))
                result.enabled = settings.enabled;
            if (Utils_1.JsonUtils.isExists(settings.allowDependencyDeleting))
                result.allowDependencyDeleting = settings.allowDependencyDeleting;
            if (Utils_1.JsonUtils.isExists(settings.allowDependencyAdding))
                result.allowDependencyAdding = settings.allowDependencyAdding;
            if (Utils_1.JsonUtils.isExists(settings.allowDependencyUpdating))
                result.allowDependencyUpdating = settings.allowDependencyUpdating;
            if (Utils_1.JsonUtils.isExists(settings.allowTaskDeleting))
                result.allowTaskDeleting = settings.allowTaskDeleting;
            if (Utils_1.JsonUtils.isExists(settings.allowTaskAdding))
                result.allowTaskAdding = settings.allowTaskAdding;
            if (Utils_1.JsonUtils.isExists(settings.allowTaskUpdating))
                result.allowTaskUpdating = settings.allowTaskUpdating;
            if (Utils_1.JsonUtils.isExists(settings.allowResourceDeleting))
                result.allowResourceDeleting = settings.allowResourceDeleting;
            if (Utils_1.JsonUtils.isExists(settings.allowResourceAdding))
                result.allowResourceAdding = settings.allowResourceAdding;
            if (Utils_1.JsonUtils.isExists(settings.allowResourceUpdating))
                result.allowResourceUpdating = settings.allowResourceUpdating;
        }
        return result;
    };
    return EditingSettings;
}());
exports.EditingSettings = EditingSettings;


/***/ }),
/* 74 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var EventDispatcher_1 = __webpack_require__(75);
var ModelChangesDispatcher = (function () {
    function ModelChangesDispatcher() {
        this.onModelChanged = new EventDispatcher_1.EventDispatcher();
    }
    ModelChangesDispatcher.prototype.notifyTaskCreated = function (task, callback) {
        this.onModelChanged.raise("NotifyTaskCreated", task, callback);
    };
    ModelChangesDispatcher.prototype.notifyTaskRemoved = function (taskID) {
        this.onModelChanged.raise("NotifyTaskRemoved", taskID);
    };
    ModelChangesDispatcher.prototype.notifyTaskTitleChanged = function (taskID, newValue) {
        this.onModelChanged.raise("NotifyTaskTitleChanged", taskID, newValue);
    };
    ModelChangesDispatcher.prototype.notifyTaskDescriptionChanged = function (taskID, newValue) {
        this.onModelChanged.raise("NotifyTaskDescriptionChanged", taskID, newValue);
    };
    ModelChangesDispatcher.prototype.notifyTaskStartChanged = function (taskID, newValue) {
        this.onModelChanged.raise("NotifyTaskStartChanged", taskID, newValue);
    };
    ModelChangesDispatcher.prototype.notifyTaskEndChanged = function (taskID, newValue) {
        this.onModelChanged.raise("NotifyTaskEndChanged", taskID, newValue);
    };
    ModelChangesDispatcher.prototype.notifyTaskProgressChanged = function (taskID, newValue) {
        this.onModelChanged.raise("NotifyTaskProgressChanged", taskID, newValue);
    };
    ModelChangesDispatcher.prototype.notifyDependencyInserted = function (dependency, callback) {
        this.onModelChanged.raise("NotifyDependencyInserted", dependency, callback);
    };
    ModelChangesDispatcher.prototype.notifyDependencyRemoved = function (dependencyID) {
        this.onModelChanged.raise("NotifyDependencyRemoved", dependencyID);
    };
    ModelChangesDispatcher.prototype.notifyResourceCreated = function (resource, callback) {
        this.onModelChanged.raise("NotifyResourceCreated", resource, callback);
    };
    ModelChangesDispatcher.prototype.notifyResourceRemoved = function (resourceID) {
        this.onModelChanged.raise("NotifyResourceRemoved", resourceID);
    };
    ModelChangesDispatcher.prototype.notifyResourceAssigned = function (assignment, callback) {
        this.onModelChanged.raise("NotifyResourceAssigned", assignment, callback);
    };
    ModelChangesDispatcher.prototype.notifyResourceUnassigned = function (assignmentID) {
        this.onModelChanged.raise("NotifyResourceUnassigned", assignmentID);
    };
    return ModelChangesDispatcher;
}());
exports.ModelChangesDispatcher = ModelChangesDispatcher;


/***/ }),
/* 75 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var EventDispatcher = (function () {
    function EventDispatcher() {
        this.listeners = [];
    }
    EventDispatcher.prototype.add = function (listener) {
        if (!listener)
            throw new Error("Error");
        if (!this.hasEventListener(listener))
            this.listeners.push(listener);
    };
    EventDispatcher.prototype.remove = function (listener) {
        for (var i = 0, currentListener; currentListener = this.listeners[i]; i++) {
            if (currentListener === listener) {
                this.listeners.splice(i, 1);
                break;
            }
        }
    };
    EventDispatcher.prototype.raise = function (funcName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        for (var i = 0, listener; listener = this.listeners[i]; i++) {
            listener[funcName].apply(listener, args);
        }
    };
    EventDispatcher.prototype.hasEventListener = function (listener) {
        for (var i = 0, l = this.listeners.length; i < l; i++)
            if (this.listeners[i] === listener)
                return true;
        return false;
    };
    return EventDispatcher;
}());
exports.EventDispatcher = EventDispatcher;


/***/ }),
/* 76 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var TaskCommands_1 = __webpack_require__(77);
var TaskPropertiesCommands_1 = __webpack_require__(79);
var DependencyCommands_1 = __webpack_require__(80);
var ResourceCommands_1 = __webpack_require__(81);
var TaskEditDialog_1 = __webpack_require__(82);
var ConstraintViolationDialog_1 = __webpack_require__(83);
var ResourcesDialog_1 = __webpack_require__(84);
var ClientCommand_1 = __webpack_require__(85);
var CommandManager = (function () {
    function CommandManager(control) {
        this.control = control;
        this.commands = {};
        this.createCommand(ClientCommand_1.GanttClientCommand.CreateTask, this.createTaskCommand);
        this.createCommand(ClientCommand_1.GanttClientCommand.CreateSubTask, this.createSubTaskCommand);
        this.createCommand(ClientCommand_1.GanttClientCommand.RemoveTask, this.removeTaskCommand);
        this.createCommand(ClientCommand_1.GanttClientCommand.RemoveDependency, this.removeDependencyCommand);
        this.createCommand(ClientCommand_1.GanttClientCommand.TaskInformation, this.showTaskEditDialog);
        this.createCommand(ClientCommand_1.GanttClientCommand.TaskAddContextItem, new TaskCommands_1.TaskAddContextItemCommand(this.control));
    }
    Object.defineProperty(CommandManager.prototype, "createTaskCommand", {
        get: function () { return new TaskCommands_1.CreateTaskCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "createSubTaskCommand", {
        get: function () { return new TaskCommands_1.CreateSubTaskCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "removeTaskCommand", {
        get: function () { return new TaskCommands_1.RemoveTaskCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "changeTaskTitleCommand", {
        get: function () { return new TaskPropertiesCommands_1.TaskTitleCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "changeTaskDescriptionCommand", {
        get: function () { return new TaskPropertiesCommands_1.TaskDescriptionCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "changeTaskProgressCommand", {
        get: function () { return new TaskPropertiesCommands_1.TaskProgressCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "changeTaskStartCommand", {
        get: function () { return new TaskPropertiesCommands_1.TaskStartCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "changeTaskEndCommand", {
        get: function () { return new TaskPropertiesCommands_1.TaskEndCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "createDependencyCommand", {
        get: function () { return new DependencyCommands_1.CreateDependencyCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "removeDependencyCommand", {
        get: function () { return new DependencyCommands_1.RemoveDependencyCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "createResourceCommand", {
        get: function () { return new ResourceCommands_1.CreateResourceCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "removeResourceCommand", {
        get: function () { return new ResourceCommands_1.RemoveResourceCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "assignResourceCommand", {
        get: function () { return new ResourceCommands_1.AssignResourceCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "deassignResourceCommand", {
        get: function () { return new ResourceCommands_1.DeassignResourceCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "showTaskEditDialog", {
        get: function () { return new TaskEditDialog_1.TaskEditDialogCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "showConstraintViolationDialog", {
        get: function () { return new ConstraintViolationDialog_1.ConstraintViolationDialogCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommandManager.prototype, "showResourcesDialog", {
        get: function () { return new ResourcesDialog_1.ResourcesDialogCommand(this.control); },
        enumerable: true,
        configurable: true
    });
    CommandManager.prototype.getCommand = function (key) {
        return this.commands[key];
    };
    CommandManager.prototype.createCommand = function (commandId, command) {
        this.commands[commandId] = command;
    };
    return CommandManager;
}());
exports.CommandManager = CommandManager;


/***/ }),
/* 77 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase_1 = __webpack_require__(5);
var TaskHistoryItem_1 = __webpack_require__(78);
var TaskDependencyHistoryItem_1 = __webpack_require__(19);
var ResourceHistoryItem_1 = __webpack_require__(20);
var TaskCommandBase = (function (_super) {
    __extends(TaskCommandBase, _super);
    function TaskCommandBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskCommandBase.prototype.getState = function () {
        var state = new CommandBase_1.SimpleCommandState(this.isEnabled());
        state.visible = state.enabled && !this.control.taskEditController.dependencyId;
        return state;
    };
    return TaskCommandBase;
}(CommandBase_1.CommandBase));
exports.TaskCommandBase = TaskCommandBase;
var CreateTaskCommand = (function (_super) {
    __extends(CreateTaskCommand, _super);
    function CreateTaskCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CreateTaskCommand.prototype.execute = function (start, end, title, parentId) {
        return _super.prototype.execute.call(this, start, end, title, parentId);
    };
    CreateTaskCommand.prototype.executeInternal = function (start, end, title, parentId) {
        if (!parentId) {
            var selectedTask = this.control.viewModel.findItem(this.control.currentSelectedTaskID).task;
            if (selectedTask)
                parentId = selectedTask.parentId;
        }
        var referenceItem = this.control.viewModel.findItem(parentId) || this.control.viewModel.items[0];
        var referenceTask = referenceItem.task;
        start = start || new Date(referenceTask.start.getTime());
        end = end || new Date(referenceTask.end.getTime());
        title = title || "New task";
        this.history.addAndRedo(new TaskHistoryItem_1.CreateTaskHistoryItem(this.modelManipulator, start, end, title, parentId));
        return true;
    };
    CreateTaskCommand.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowTaskAdding;
    };
    return CreateTaskCommand;
}(TaskCommandBase));
exports.CreateTaskCommand = CreateTaskCommand;
var CreateSubTaskCommand = (function (_super) {
    __extends(CreateSubTaskCommand, _super);
    function CreateSubTaskCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CreateSubTaskCommand.prototype.execute = function (parentId) {
        return _super.prototype.execute.call(this, parentId);
    };
    CreateSubTaskCommand.prototype.executeInternal = function (parentId) {
        parentId = parentId || this.control.currentSelectedTaskID;
        var selectedItem = this.control.viewModel.findItem(parentId);
        if (selectedItem.selected) {
            this.history.addAndRedo(new TaskHistoryItem_1.CreateTaskHistoryItem(this.modelManipulator, new Date(selectedItem.task.start.getTime()), new Date(selectedItem.task.end.getTime()), "New task", parentId));
            return true;
        }
        return false;
    };
    CreateSubTaskCommand.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowTaskAdding;
    };
    CreateSubTaskCommand.prototype.getState = function () {
        var state = _super.prototype.getState.call(this);
        var selectedItem = this.control.viewModel.findItem(this.control.currentSelectedTaskID);
        state.visible = state.visible && selectedItem && selectedItem.selected;
        return state;
    };
    return CreateSubTaskCommand;
}(TaskCommandBase));
exports.CreateSubTaskCommand = CreateSubTaskCommand;
var RemoveTaskCommand = (function (_super) {
    __extends(RemoveTaskCommand, _super);
    function RemoveTaskCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RemoveTaskCommand.prototype.execute = function (id) {
        return _super.prototype.execute.call(this, id);
    };
    RemoveTaskCommand.prototype.executeInternal = function (id) {
        var _this = this;
        id = id || this.control.currentSelectedTaskID;
        this.history.beginTransaction();
        var childTasks = this.control.viewModel.tasks.items.filter(function (t) { return t.parentId == id; });
        childTasks.forEach(function (t) { return new RemoveTaskCommand(_this.control).execute(t.internalId); });
        var dependencies = this.control.viewModel.dependencies.items.filter(function (d) { return d.predecessorId == id || d.successorId == id; });
        if (dependencies.length)
            if (this.control.settings.editing.allowDependencyDeleting)
                dependencies.forEach(function (d) { return _this.history.addAndRedo(new TaskDependencyHistoryItem_1.RemoveDependencyHistoryItem(_this.modelManipulator, d.internalId)); });
            else
                return false;
        var assignments = this.control.viewModel.assignments.items.filter(function (a) { return a.taskId == id; });
        assignments.forEach(function (a) { return _this.history.addAndRedo(new ResourceHistoryItem_1.DeassignResourceHistoryItem(_this.modelManipulator, a.internalId)); });
        this.history.addAndRedo(new TaskHistoryItem_1.RemoveTaskHistoryItem(this.modelManipulator, id));
        this.history.endTransaction();
        return true;
    };
    RemoveTaskCommand.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowTaskDeleting;
    };
    return RemoveTaskCommand;
}(TaskCommandBase));
exports.RemoveTaskCommand = RemoveTaskCommand;
var TaskAddContextItemCommand = (function (_super) {
    __extends(TaskAddContextItemCommand, _super);
    function TaskAddContextItemCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskAddContextItemCommand.prototype.getState = function () {
        var state = _super.prototype.getState.call(this);
        state.visible = state.visible && this.control.settings.editing.allowTaskAdding;
        return state;
    };
    TaskAddContextItemCommand.prototype.execute = function () {
        return false;
    };
    return TaskAddContextItemCommand;
}(TaskCommandBase));
exports.TaskAddContextItemCommand = TaskAddContextItemCommand;


/***/ }),
/* 78 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HistoryItem_1 = __webpack_require__(9);
var CreateTaskHistoryItem = (function (_super) {
    __extends(CreateTaskHistoryItem, _super);
    function CreateTaskHistoryItem(modelManipulator, start, end, title, parentId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.start = start;
        _this.end = end;
        _this.title = title;
        _this.parentId = parentId;
        return _this;
    }
    CreateTaskHistoryItem.prototype.redo = function () {
        this.taskId = this.modelManipulator.task.create(this.start, this.end, this.title, this.parentId, this.taskId ? this.taskId : null).internalId;
    };
    CreateTaskHistoryItem.prototype.undo = function () {
        this.modelManipulator.task.remove(this.taskId);
    };
    return CreateTaskHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.CreateTaskHistoryItem = CreateTaskHistoryItem;
var RemoveTaskHistoryItem = (function (_super) {
    __extends(RemoveTaskHistoryItem, _super);
    function RemoveTaskHistoryItem(modelManipulator, taskId) {
        var _this = _super.call(this, modelManipulator) || this;
        _this.taskId = taskId;
        return _this;
    }
    RemoveTaskHistoryItem.prototype.redo = function () {
        this.task = this.modelManipulator.task.remove(this.taskId);
    };
    RemoveTaskHistoryItem.prototype.undo = function () {
        this.modelManipulator.task.create(this.task.start, this.task.end, this.task.title, this.task.parentId, this.task.internalId);
    };
    return RemoveTaskHistoryItem;
}(HistoryItem_1.HistoryItem));
exports.RemoveTaskHistoryItem = RemoveTaskHistoryItem;


/***/ }),
/* 79 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase_1 = __webpack_require__(5);
var TaskPropertiesHistoryItem_1 = __webpack_require__(30);
var TaskPropertyCommandBase = (function (_super) {
    __extends(TaskPropertyCommandBase, _super);
    function TaskPropertyCommandBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskPropertyCommandBase.prototype.getState = function () {
        return new CommandBase_1.SimpleCommandState(this.isEnabled());
    };
    TaskPropertyCommandBase.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowTaskUpdating;
    };
    return TaskPropertyCommandBase;
}(CommandBase_1.CommandBase));
exports.TaskPropertyCommandBase = TaskPropertyCommandBase;
var TaskTitleCommand = (function (_super) {
    __extends(TaskTitleCommand, _super);
    function TaskTitleCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskTitleCommand.prototype.execute = function (id, value) {
        return _super.prototype.execute.call(this, id, value);
    };
    TaskTitleCommand.prototype.executeInternal = function (id, value) {
        var oldTitle = this.control.viewModel.tasks.getItemById(id).title;
        if (oldTitle == value)
            return false;
        this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskTitleHistoryItem(this.modelManipulator, id, value));
        return true;
    };
    return TaskTitleCommand;
}(TaskPropertyCommandBase));
exports.TaskTitleCommand = TaskTitleCommand;
var TaskDescriptionCommand = (function (_super) {
    __extends(TaskDescriptionCommand, _super);
    function TaskDescriptionCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskDescriptionCommand.prototype.execute = function (id, value) {
        return _super.prototype.execute.call(this, id, value);
    };
    TaskDescriptionCommand.prototype.executeInternal = function (id, value) {
        var oldDescription = this.control.viewModel.tasks.getItemById(id).description;
        if (oldDescription == value)
            return false;
        this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskDesriptionHistoryItem(this.modelManipulator, id, value));
        return true;
    };
    return TaskDescriptionCommand;
}(TaskPropertyCommandBase));
exports.TaskDescriptionCommand = TaskDescriptionCommand;
var TaskProgressCommand = (function (_super) {
    __extends(TaskProgressCommand, _super);
    function TaskProgressCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskProgressCommand.prototype.execute = function (id, value) {
        return _super.prototype.execute.call(this, id, value);
    };
    TaskProgressCommand.prototype.executeInternal = function (id, value) {
        var oldProgress = this.control.viewModel.tasks.getItemById(id).progress;
        if (oldProgress == value)
            return false;
        this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskProgressHistoryItem(this.modelManipulator, id, value));
        return true;
    };
    return TaskProgressCommand;
}(TaskPropertyCommandBase));
exports.TaskProgressCommand = TaskProgressCommand;
var TaskStartCommand = (function (_super) {
    __extends(TaskStartCommand, _super);
    function TaskStartCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskStartCommand.prototype.execute = function (id, value) {
        return _super.prototype.execute.call(this, id, value);
    };
    TaskStartCommand.prototype.executeInternal = function (id, value) {
        var oldStart = this.control.viewModel.tasks.getItemById(id).start;
        if (oldStart.getTime() == value.getTime())
            return false;
        this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskStartHistoryItem(this.modelManipulator, id, value));
        if (value < this.control.dataRange.start)
            this.control.dataRange.start = value;
        return true;
    };
    return TaskStartCommand;
}(TaskPropertyCommandBase));
exports.TaskStartCommand = TaskStartCommand;
var TaskEndCommand = (function (_super) {
    __extends(TaskEndCommand, _super);
    function TaskEndCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskEndCommand.prototype.execute = function (id, value) {
        return _super.prototype.execute.call(this, id, value);
    };
    TaskEndCommand.prototype.executeInternal = function (id, value) {
        var oldEnd = this.control.viewModel.tasks.getItemById(id).end;
        if (oldEnd.getTime() == value.getTime())
            return false;
        this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskEndHistoryItem(this.modelManipulator, id, value));
        if (value > this.control.dataRange.end)
            this.control.dataRange.end = value;
        return true;
    };
    return TaskEndCommand;
}(TaskPropertyCommandBase));
exports.TaskEndCommand = TaskEndCommand;


/***/ }),
/* 80 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase_1 = __webpack_require__(5);
var TaskDependencyHistoryItem_1 = __webpack_require__(19);
var DependencyCommandBase = (function (_super) {
    __extends(DependencyCommandBase, _super);
    function DependencyCommandBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DependencyCommandBase.prototype.getState = function () {
        return new CommandBase_1.SimpleCommandState(this.isEnabled());
    };
    return DependencyCommandBase;
}(CommandBase_1.CommandBase));
exports.DependencyCommandBase = DependencyCommandBase;
var CreateDependencyCommand = (function (_super) {
    __extends(CreateDependencyCommand, _super);
    function CreateDependencyCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CreateDependencyCommand.prototype.execute = function (predecessorId, successorId, type) {
        return _super.prototype.execute.call(this, predecessorId, successorId, type);
    };
    CreateDependencyCommand.prototype.executeInternal = function (predecessorId, successorId, type) {
        this.history.addAndRedo(new TaskDependencyHistoryItem_1.InsertDependencyHistoryItem(this.modelManipulator, predecessorId, successorId, type));
        return true;
    };
    CreateDependencyCommand.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowDependencyAdding;
    };
    return CreateDependencyCommand;
}(DependencyCommandBase));
exports.CreateDependencyCommand = CreateDependencyCommand;
var RemoveDependencyCommand = (function (_super) {
    __extends(RemoveDependencyCommand, _super);
    function RemoveDependencyCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RemoveDependencyCommand.prototype.execute = function (id) {
        return _super.prototype.execute.call(this, id);
    };
    RemoveDependencyCommand.prototype.executeInternal = function (id) {
        id = id || this.control.taskEditController.dependencyId;
        if (id != null) {
            this.history.addAndRedo(new TaskDependencyHistoryItem_1.RemoveDependencyHistoryItem(this.modelManipulator, id));
            return true;
        }
        return false;
    };
    RemoveDependencyCommand.prototype.isEnabled = function () {
        return _super.prototype.isEnabled.call(this) && this.control.settings.editing.allowDependencyDeleting;
    };
    RemoveDependencyCommand.prototype.getState = function () {
        var state = _super.prototype.getState.call(this);
        state.visible = state.enabled && this.control.taskEditController.dependencyId != null;
        return state;
    };
    return RemoveDependencyCommand;
}(DependencyCommandBase));
exports.RemoveDependencyCommand = RemoveDependencyCommand;


/***/ }),
/* 81 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var CommandBase_1 = __webpack_require__(5);
var ResourceHistoryItem_1 = __webpack_require__(20);
var ResourceCommandBase = (function (_super) {
    __extends(ResourceCommandBase, _super);
    function ResourceCommandBase() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourceCommandBase.prototype.getState = function () {
        return new CommandBase_1.SimpleCommandState(this.isEnabled());
    };
    return ResourceCommandBase;
}(CommandBase_1.CommandBase));
exports.ResourceCommandBase = ResourceCommandBase;
var CreateResourceCommand = (function (_super) {
    __extends(CreateResourceCommand, _super);
    function CreateResourceCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CreateResourceCommand.prototype.execute = function (text) {
        return _super.prototype.execute.call(this, text);
    };
    CreateResourceCommand.prototype.executeInternal = function (text) {
        this.history.addAndRedo(new ResourceHistoryItem_1.CreateResourceHistoryItem(this.modelManipulator, text));
        return true;
    };
    return CreateResourceCommand;
}(ResourceCommandBase));
exports.CreateResourceCommand = CreateResourceCommand;
var RemoveResourceCommand = (function (_super) {
    __extends(RemoveResourceCommand, _super);
    function RemoveResourceCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RemoveResourceCommand.prototype.execute = function (id) {
        return _super.prototype.execute.call(this, id);
    };
    RemoveResourceCommand.prototype.executeInternal = function (id) {
        var _this = this;
        this.history.beginTransaction();
        var assignments = this.control.viewModel.assignments.items.filter(function (a) { return a.resourceId == id; });
        assignments.forEach(function (a) { return _this.history.addAndRedo(new ResourceHistoryItem_1.DeassignResourceHistoryItem(_this.modelManipulator, a.internalId)); });
        this.history.addAndRedo(new ResourceHistoryItem_1.RemoveResourceHistoryItem(this.modelManipulator, id));
        this.history.endTransaction();
        return true;
    };
    return RemoveResourceCommand;
}(ResourceCommandBase));
exports.RemoveResourceCommand = RemoveResourceCommand;
var AssignResourceCommand = (function (_super) {
    __extends(AssignResourceCommand, _super);
    function AssignResourceCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AssignResourceCommand.prototype.execute = function (resourceId, taskId) {
        return _super.prototype.execute.call(this, resourceId, taskId);
    };
    AssignResourceCommand.prototype.executeInternal = function (resourceId, taskId) {
        this.history.addAndRedo(new ResourceHistoryItem_1.AssignResourceHistoryItem(this.modelManipulator, resourceId, taskId));
        return true;
    };
    return AssignResourceCommand;
}(ResourceCommandBase));
exports.AssignResourceCommand = AssignResourceCommand;
var DeassignResourceCommand = (function (_super) {
    __extends(DeassignResourceCommand, _super);
    function DeassignResourceCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DeassignResourceCommand.prototype.execute = function (assignmentId) {
        return _super.prototype.execute.call(this, assignmentId);
    };
    DeassignResourceCommand.prototype.executeInternal = function (assignmentId) {
        this.history.addAndRedo(new ResourceHistoryItem_1.DeassignResourceHistoryItem(this.modelManipulator, assignmentId));
        return true;
    };
    return DeassignResourceCommand;
}(ResourceCommandBase));
exports.DeassignResourceCommand = DeassignResourceCommand;


/***/ }),
/* 82 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DialogBase_1 = __webpack_require__(21);
var TaskPropertiesHistoryItem_1 = __webpack_require__(30);
var ResourceCollection_1 = __webpack_require__(12);
var ResourceHistoryItem_1 = __webpack_require__(20);
var CommandBase_1 = __webpack_require__(5);
var TaskEditDialogCommand = (function (_super) {
    __extends(TaskEditDialogCommand, _super);
    function TaskEditDialogCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskEditDialogCommand.prototype.applyParameters = function (newParameters, oldParameters) {
        this.history.beginTransaction();
        if (newParameters.title != oldParameters.title)
            this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskTitleHistoryItem(this.modelManipulator, oldParameters.id, newParameters.title));
        if (newParameters.progress != oldParameters.progress)
            this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskProgressHistoryItem(this.modelManipulator, oldParameters.id, newParameters.progress));
        if (newParameters.start != oldParameters.start)
            this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskStartHistoryItem(this.modelManipulator, oldParameters.id, newParameters.start));
        if (newParameters.end != oldParameters.end)
            this.history.addAndRedo(new TaskPropertiesHistoryItem_1.TaskEndHistoryItem(this.modelManipulator, oldParameters.id, newParameters.end));
        for (var i = 0; i < newParameters.assigned.length; i++) {
            var resource = oldParameters.assigned.getItemById(newParameters.assigned.getItem(i).internalId);
            if (!resource)
                this.history.addAndRedo(new ResourceHistoryItem_1.AssignResourceHistoryItem(this.modelManipulator, newParameters.assigned.getItem(i).internalId, oldParameters.id));
        }
        var _loop_1 = function (i) {
            var assigned = oldParameters.assigned.getItem(i);
            var resource = newParameters.assigned.getItemById(assigned.internalId);
            if (!resource) {
                var assignment = this_1.control.viewModel.assignments.items.find(function (assignment) { return assignment.resourceId == assigned.internalId && assignment.taskId == oldParameters.id; });
                this_1.history.addAndRedo(new ResourceHistoryItem_1.DeassignResourceHistoryItem(this_1.modelManipulator, assignment.internalId));
            }
        };
        var this_1 = this;
        for (var i = 0; i < oldParameters.assigned.length; i++) {
            _loop_1(i);
        }
        this.history.endTransaction();
        return false;
    };
    TaskEditDialogCommand.prototype.createParameters = function (options) {
        options = options || this.control.viewModel.tasks.getItemById(this.control.currentSelectedTaskID);
        var param = new TaskEditParameters();
        param.id = options.internalId;
        param.title = options.title;
        param.progress = options.progress;
        param.start = options.start;
        param.end = options.end;
        param.assigned = this.control.viewModel.getAssignedResources(options);
        param.resources = new ResourceCollection_1.ResourceCollection();
        param.resources.addRange(this.control.viewModel.resources.items);
        param.showResourcesDialogCommand = this.control.commandManager.showResourcesDialog;
        return param;
    };
    TaskEditDialogCommand.prototype.isEnabled = function () {
        return true;
    };
    TaskEditDialogCommand.prototype.getState = function () {
        var state = new CommandBase_1.SimpleCommandState(this.isEnabled());
        state.visible = !this.control.taskEditController.dependencyId;
        return state;
    };
    TaskEditDialogCommand.prototype.getDialogName = function () {
        return "TaskEdit";
    };
    return TaskEditDialogCommand;
}(DialogBase_1.DialogBase));
exports.TaskEditDialogCommand = TaskEditDialogCommand;
var TaskEditParameters = (function (_super) {
    __extends(TaskEditParameters, _super);
    function TaskEditParameters() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TaskEditParameters.prototype.clone = function () {
        var clone = new TaskEditParameters();
        clone.id = this.id;
        clone.title = this.title;
        clone.progress = this.progress;
        clone.start = this.start;
        clone.end = this.end;
        clone.assigned = new ResourceCollection_1.ResourceCollection();
        clone.assigned.addRange(this.assigned.items);
        clone.resources = new ResourceCollection_1.ResourceCollection();
        clone.resources.addRange(this.resources.items);
        clone.showResourcesDialogCommand = this.showResourcesDialogCommand;
        return clone;
    };
    return TaskEditParameters;
}(DialogBase_1.DialogParametersBase));
exports.TaskEditParameters = TaskEditParameters;


/***/ }),
/* 83 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DialogBase_1 = __webpack_require__(21);
var TaskDependencyHistoryItem_1 = __webpack_require__(19);
var ConstraintViolationOption;
(function (ConstraintViolationOption) {
    ConstraintViolationOption[ConstraintViolationOption["DoNothing"] = 0] = "DoNothing";
    ConstraintViolationOption[ConstraintViolationOption["RemoveDependency"] = 1] = "RemoveDependency";
    ConstraintViolationOption[ConstraintViolationOption["KeepDependency"] = 2] = "KeepDependency";
})(ConstraintViolationOption = exports.ConstraintViolationOption || (exports.ConstraintViolationOption = {}));
var ConstraintViolationDialogCommand = (function (_super) {
    __extends(ConstraintViolationDialogCommand, _super);
    function ConstraintViolationDialogCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ConstraintViolationDialogCommand.prototype.applyParameters = function (newParameters, oldParameters) {
        if (newParameters.option === ConstraintViolationOption.DoNothing)
            return false;
        if (newParameters.option === ConstraintViolationOption.RemoveDependency)
            this.history.addAndRedo(new TaskDependencyHistoryItem_1.RemoveDependencyHistoryItem(this.modelManipulator, oldParameters.dependencyId));
        return true;
    };
    ConstraintViolationDialogCommand.prototype.createParameters = function (options) {
        var result = new ConstraintViolationDialogParameters();
        result.dependencyId = options;
        return result;
    };
    ConstraintViolationDialogCommand.prototype.getDialogName = function () {
        return "ConstraintViolation";
    };
    return ConstraintViolationDialogCommand;
}(DialogBase_1.DialogBase));
exports.ConstraintViolationDialogCommand = ConstraintViolationDialogCommand;
var ConstraintViolationDialogParameters = (function (_super) {
    __extends(ConstraintViolationDialogParameters, _super);
    function ConstraintViolationDialogParameters() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ConstraintViolationDialogParameters.prototype.clone = function () {
        var result = new ConstraintViolationDialogParameters();
        result.dependencyId = this.dependencyId;
        result.option = this.option;
        return result;
    };
    return ConstraintViolationDialogParameters;
}(DialogBase_1.DialogParametersBase));
exports.ConstraintViolationDialogParameters = ConstraintViolationDialogParameters;


/***/ }),
/* 84 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var DialogBase_1 = __webpack_require__(21);
var ResourceCollection_1 = __webpack_require__(12);
var ResourcesDialogCommand = (function (_super) {
    __extends(ResourcesDialogCommand, _super);
    function ResourcesDialogCommand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourcesDialogCommand.prototype.applyParameters = function (newParameters, oldParameters) {
        this.history.beginTransaction();
        for (var i = 0; i < newParameters.resources.length; i++) {
            var resource = oldParameters.resources.getItemById(newParameters.resources.getItem(i).internalId);
            if (!resource)
                this.control.commandManager.createResourceCommand.execute(newParameters.resources.getItem(i).text);
        }
        for (var i = 0; i < oldParameters.resources.length; i++) {
            var resource = newParameters.resources.getItemById(oldParameters.resources.getItem(i).internalId);
            if (!resource)
                this.control.commandManager.removeResourceCommand.execute(oldParameters.resources.getItem(i).internalId);
        }
        this.history.endTransaction();
        return false;
    };
    ResourcesDialogCommand.prototype.createParameters = function () {
        var param = new ResourcesDialogParameters();
        param.resources = new ResourceCollection_1.ResourceCollection();
        param.resources.addRange(this.control.viewModel.resources.items);
        return param;
    };
    ResourcesDialogCommand.prototype.getDialogName = function () {
        return "Resources";
    };
    return ResourcesDialogCommand;
}(DialogBase_1.DialogBase));
exports.ResourcesDialogCommand = ResourcesDialogCommand;
var ResourcesDialogParameters = (function (_super) {
    __extends(ResourcesDialogParameters, _super);
    function ResourcesDialogParameters() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResourcesDialogParameters.prototype.clone = function () {
        var clone = new ResourcesDialogParameters();
        clone.resources = new ResourceCollection_1.ResourceCollection();
        clone.resources.addRange(this.resources.items);
        return clone;
    };
    return ResourcesDialogParameters;
}(DialogBase_1.DialogParametersBase));
exports.ResourcesDialogParameters = ResourcesDialogParameters;


/***/ }),
/* 85 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var GanttClientCommand;
(function (GanttClientCommand) {
    GanttClientCommand[GanttClientCommand["CreateTask"] = 0] = "CreateTask";
    GanttClientCommand[GanttClientCommand["CreateSubTask"] = 1] = "CreateSubTask";
    GanttClientCommand[GanttClientCommand["RemoveTask"] = 2] = "RemoveTask";
    GanttClientCommand[GanttClientCommand["RemoveDependency"] = 3] = "RemoveDependency";
    GanttClientCommand[GanttClientCommand["TaskInformation"] = 4] = "TaskInformation";
    GanttClientCommand[GanttClientCommand["TaskAddContextItem"] = 5] = "TaskAddContextItem";
})(GanttClientCommand = exports.GanttClientCommand || (exports.GanttClientCommand = {}));


/***/ }),
/* 86 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var BarManager = (function () {
    function BarManager(control, bars) {
        this.control = control;
        this.bars = bars;
    }
    BarManager.prototype.updateContextMenu = function () {
        for (var i = 0, bar; bar = this.bars[i]; i++) {
            if (bar.isContextMenu()) {
                bar.updateItemsList();
                var commandKeys = bar.getCommandKeys();
                for (var j = 0; j < commandKeys.length; j++) {
                    this.updateBarItem(bar, commandKeys[j]);
                }
            }
        }
    };
    BarManager.prototype.updateItemsState = function (queryCommands) {
        var anyQuerySended = !!queryCommands.length;
        for (var i = 0, bar; bar = this.bars[i]; i++) {
            if (bar.isVisible()) {
                var commandKeys = bar.getCommandKeys();
                var _loop_1 = function (j, commandKey) {
                    if (anyQuerySended && queryCommands.find(function (q) { return q == commandKey; }))
                        return "continue";
                    this_1.updateBarItem(bar, commandKey);
                };
                var this_1 = this;
                for (var j = 0, commandKey = void 0; commandKey = commandKeys[j]; j++) {
                    _loop_1(j, commandKey);
                }
            }
        }
    };
    BarManager.prototype.updateBarItem = function (bar, commandKey) {
        var command = this.control.commandManager.getCommand(commandKey);
        if (command) {
            var commandState = command.getState();
            bar.setItemVisible(commandKey, commandState.visible);
            if (commandState.visible) {
                bar.setItemEnabled(commandKey, commandState.enabled);
                bar.setItemValue(commandKey, commandState.value);
            }
        }
    };
    return BarManager;
}());
exports.BarManager = BarManager;


/***/ })
/******/ ]);
});