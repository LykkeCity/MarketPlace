var UiDesktop = (function () {
    function UiDesktop() {
        this.dialogIsShown = false;
        this.isConnected = true;
        this.isBlured = false;
    }
    UiDesktop.prototype.initFocus = function () {
        $('.setCursor').focus();
        $('.setCursor').select();
    };
    UiDesktop.prototype.loadModalContent = function (url, params) {
        var _this = this;
        $('#modalData').html('<div style="text-align:center;"><img src="/Images/Loading.gif"/></div>');
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then(function (result) {
            $('#modalData').html(result);
            _this.initFocus();
            //  this.pageManager.ressize();
        }).fail(function (jqXhr) {
            if (jqXhr && jqXhr.status === 403)
                window.location.reload();
        });
    };
    UiDesktop.prototype.showDialog = function (url, params) {
        var _this = this;
        if (params && params[0] === '#')
            params = $(params).serialize();
        this.onClose = undefined;
        $('#modalBackground').fadeIn(200, function () {
            //      $('#pamain').css('-webkit-filter', 'blur(4px)');
            _this.loadModalContent(url, params);
        });
        this.dialogIsShown = true;
    };
    UiDesktop.prototype.showDialogWithCallBack = function (data) {
        this.showDialog(data.url, data.data);
        this.onClose = data.onClose;
        this.dialogResult = "close";
    };
    UiDesktop.prototype.hideDialogLaptop = function (callback, result) {
        //     $('#pamain').css('-webkit-filter', '');
        var _this = this;
        $('#modalBackground').fadeOut(200, function () {
            $('#modalData').html("");
            _this.dialogIsShown = false;
            if (callback)
                callback();
            if (_this.onClose)
                _this.onClose(result);
        });
    };
    UiDesktop.prototype.hideDialog = function (callback) {
        if (!this.dialogIsShown) {
            if (callback)
                callback();
            if (this.onClose)
                this.onClose(undefined);
            return;
        }
        this.hideDialogLaptop(callback);
    };
    UiDesktop.prototype.hideDialogWithResult = function (result) {
        this.hideDialogLaptop(undefined, result);
    };
    UiDesktop.prototype.showError = function (component, text, caption, placement) {
        if (text != "") {
            if (!placement)
                placement = "top";
            var options = { title: caption, html: true, content: '<div style="color:black">' + text + '</div>', placement: placement };
            $(component).popover(options);
            $(component).popover('show');
            var serrId = setTimeout(function () {
                clearTimeout(serrId);
                $(component).popover('hide');
                $(component).popover('destroy');
            }, 3000);
        }
        $(component).focus();
        $(component).select();
    };
    UiDesktop.prototype.noConnection = function () {
        if (!this.isConnected)
            return;
        this.isConnected = false;
        this.blur();
        $('#noConnection').show();
    };
    UiDesktop.prototype.hasConnection = function () {
        if (this.isConnected)
            return;
        this.isConnected = true;
        $('#noConnection').hide();
        this.unBlur();
    };
    UiDesktop.prototype.blur = function () {
        if (this.isBlured)
            return;
        $('#pamain').addClass('blur');
        this.isBlured = true;
    };
    UiDesktop.prototype.unBlur = function () {
        if (!this.isBlured)
            return;
        $('#pamain').removeClass('blur');
        this.isBlured = false;
    };
    UiDesktop.prototype.calcHeight = function (data) {
        var datas = data.split(";");
        var result = 0;
        for (var i in datas) {
            var itm = datas[i];
            result += itm[0] === "#" ? $(itm).height() : new Number(itm);
        }
        return result;
    };
    UiDesktop.prototype.ressizeDependency = function (c) {
        var _this = this;
        $("[data-autoheight]").each(function (indx, element) {
            var ahData = $(element).attr("data-autoheight");
            var oh = (ahData.length > 0 && ahData[0] == '#')
                ? -_this.calcHeight(ahData)
                : new Number(ahData);
            if (ahData.length > 0 && ahData[0] === '#') {
                console.log("ahData:" + oh);
            }
            $(element).css('height', ((c.height) + oh) + 'px');
        });
        var sbw = $('.sideBar').width();
        var a = { left: sbw + "px", width: (c.width - sbw) + 'px', height: c.height + "px" };
        $('#content').css(a);
        $('#modalBackground').css(a);
    };
    UiDesktop.prototype.ressize = function () {
        var w = window.innerWidth;
        var h = window.innerHeight;
        this.ressizeDependency({ width: w, height: h });
    };
    return UiDesktop;
})();
var ui = new UiDesktop();
$(window).resize(function () { ui.ressize(); });
//# sourceMappingURL=UI.js.map