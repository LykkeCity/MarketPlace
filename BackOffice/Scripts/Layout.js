var Layout = (function () {
    function Layout() {
        this.nowRessize = false;
        this.topOffset = 0;
    }
    Layout.prototype.calcHeight = function (data) {
        var datas = data.split(";");
        var result = 0;
        for (var i in datas) {
            var itm = datas[i];
            result += itm[0] === "#" ? $(itm).innerHeight() : parseInt(itm);
        }
        return result;
    };
    Layout.prototype.ressizeDependency = function (c) {
        var _this = this;
        $('.headerIcon').each(function (indx, element) {
            $(element).css('left', (c.width - 66) + 'px');
        });
        //      var headerH = $('#appHeader').outerHeight();
        $("[data-fullscreen]").each(function (indx, element) {
            $(element).css('height', (c.height) + 'px');
        });
        $("[data-autoheight]").each(function (indx, element) {
            var ahData = $(element).attr("data-autoheight");
            var oh = (ahData.length > 0 && ahData[0] === '#')
                ? -_this.calcHeight(ahData)
                : parseInt(ahData);
            $(element).css('height', ((c.height) + oh) + 'px');
        });
        $("[data-top]").each(function (indx, element) {
            var ow = $(element).attr("data-top");
            var h = $(ow).innerHeight();
            $(element).css("top", h + "px");
        });
        $("[data-autowidth]").each(function (indx, element) {
            var aw = $(element).attr("data-autowidth");
            if (aw.length > 0 && aw[0] == '*') {
                var mul = parseInt(aw.substr(1, aw.length - 1));
                $(element).css('width', ((c.width) * mul) + 'px');
            }
            else
                $(element).css('width', (c.width + parseInt(aw)) + 'px');
        });
        $("[data-doublewidth]").each(function (indx, element) {
            var ow = new Number($(element).attr("data-doublewidth"));
            $(element).css('width', (c.width * ow) + 'px');
        });
        $('[data-maxheight]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-maxheight"));
            $(element).css('max-height', (c.height + ow) + 'px');
        });
        $('[data-xcenter]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-xcenter"));
            var w = $(element).width();
            var left = c.width / 2 - w / 2 + ow;
            $(element).css('left', left + 'px');
        });
        $('[data-ycenter]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-xcenter"));
            var top = c.height / 2 - $(element).height() / 2 + ow;
            $(element).css('top', top + 'px');
        });
        $('[data-autotop]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-autotop"));
            var top = c.height + ow;
            $(element).css('top', top + 'px');
        });
        $('[data-autoright]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-autoright"));
            var left = c.width - $(element).width() + ow;
            $(element).css('left', left + 'px');
        });
        $('[data-autobottom]').each(function (indx, element) {
            var ow = new Number($(element).attr("data-autobottom"));
            var top = c.height - $(element).height() + ow;
            $(element).css('top', top + 'px');
        });
    };
    Layout.prototype.ressize = function () {
        if (this.nowRessize)
            return;
        this.nowRessize = true;
        var w = window.innerWidth;
        var h = window.innerHeight - this.topOffset;
        var t = w - 1920;
        var l = h - 1200;
        $('#backgrnd').css("background-position", t + "px " + l + "px");
        var o = { left: 0, top: this.topOffset, width: w, height: h };
        $('#htmlcontent').css(o);
        this.ressizeDependency({ width: w, height: h });
        this.nowRessize = false;
        var nowContent = $('#nowContent');
        if (nowContent.length > 0) {
            var nh = nowContent.height();
            console.log("nh=" + nh + "; h=" + h);
        }
        this.ui.ressize();
    };
    return Layout;
})();
var layout = new Layout();
