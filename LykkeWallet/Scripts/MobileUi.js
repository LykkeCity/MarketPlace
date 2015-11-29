var UiMobile = (function () {
    function UiMobile() {
        this.menuIsHidden = true;
        this.isLive = true;
        this.dialogIsShown = false;
        this.isDetailsShown = false;
        this.isConnected = true;
    }
    UiMobile.prototype.init = function (demoOrRealUrl) {
        this.demoOrRealUrl = demoOrRealUrl;
    };
    UiMobile.prototype.showError = function (component, text, caption, placement) {
        if (text !== "") {
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
        $('html, body').animate({
            scrollTop: $(component).offset() + 'px'
        });
    };
    UiMobile.prototype.hideShowMenu = function () {
        this.menuIsHidden = !this.menuIsHidden;
        if (this.menuIsHidden) {
            $('#mainMenu').hide();
        }
        else {
            $('#mainMenu').show();
        }
    };
    UiMobile.prototype.initFocus = function () {
        $('.setCursor').focus();
        $('.setCursor').select();
    };
    UiMobile.prototype.getDialogHeader = function () {
        if (!this.dialogHeader)
            this.dialogHeader = $('#dialogHeader');
        return this.dialogHeader;
    };
    UiMobile.prototype.getDialogItem = function () {
        if (!this.dialogItem)
            this.dialogItem = $('#mobileDialog');
        return this.dialogItem;
    };
    UiMobile.prototype.getDialogData = function () {
        if (!this.dialogData)
            this.dialogData = $('#dialogData');
        return this.dialogData;
    };
    UiMobile.prototype.showContent = function (callBack) {
        var itm = this.getDialogItem();
        this.getDialogHeader().html('');
        var w = window.innerWidth;
        itm.css({ width: w + 'px', left: -w + 'px', top: 0 });
        itm.show();
        itm.animate({ left: 0 }, 200, callBack);
    };
    UiMobile.prototype.loadModalContent = function (j, url, params) {
        var _this = this;
        j.html('<div style="text-align:center;"><img src="/Images/Loading-pa.gif" style="margin-top:20px;"/></div>');
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then(function (result) {
            j.html(result);
            _this.initFocus();
        }).fail(function (jqXhr) {
            if (jqXhr && jqXhr.status === 403)
                window.location.reload();
        });
    };
    UiMobile.prototype.showDialog = function (url, params) {
        var _this = this;
        if (this.dialogIsShown) {
            this.hideDialog(function () {
                _this.showDialog(url, params);
            });
            return;
        }
        this.onClose = undefined;
        this.showContent(function () {
            //PersonalArea.pushMobileBackToHistory();
            _this.loadModalContent(_this.getDialogData(), url, params);
        });
        this.dialogIsShown = true;
    };
    UiMobile.prototype.hideDialogMobile = function (callback, result) {
        var _this = this;
        var w = window.innerWidth;
        var itm = this.getDialogItem();
        itm.animate({
            left: -w + 'px'
        }, 200, function () {
            itm.hide();
            _this.getDialogData().html('');
            _this.dialogIsShown = false;
            if (_this.onClose)
                _this.onClose(result);
            if (callback)
                callback();
        });
    };
    UiMobile.prototype.hideDialog = function (callback) {
        if (!this.dialogIsShown) {
            if (callback)
                callback();
            if (this.onClose)
                this.onClose(undefined);
            return false;
        }
        this.hideDialogMobile(callback);
        return true;
    };
    UiMobile.prototype.getScreenWrapper = function () {
        if (!this.screenWrapperItm)
            this.screenWrapperItm = $('#screenWrapper');
        return this.screenWrapperItm;
    };
    UiMobile.prototype.requestScreenWraperData = function (url, params) {
    };
    UiMobile.prototype.showDetails = function (data) {
        var _this = this;
        if (this.isDetailsShown)
            return;
        $('#detailsHeader').html(data.title);
        var w = window.innerWidth;
        var itm = this.getScreenWrapper();
        itm.animate({
            left: -w + 'px'
        }, 200, function () {
            //PersonalArea.pushMobileBackToHistory();
            _this.isDetailsShown = true;
            _this.loadModalContent($('#detailsContent'), data.url, data.params);
        });
    };
    UiMobile.prototype.hideDetails = function () {
        var _this = this;
        if (!this.isDetailsShown)
            return false;
        var itm = this.getScreenWrapper();
        itm.animate({
            left: 0
        }, 200, function () {
            _this.isDetailsShown = false;
            $('#detailsContent').html('');
            $('#detailsHeader').html('');
        });
        return true;
    };
    UiMobile.prototype.showHeaderShadow = function () {
        $('#appHeader').css('box-shadow', '0 2px 2px rgba(0, 0, 0, 0.17)');
    };
    UiMobile.prototype.hideHeaderShadow = function () {
        $('#appHeader').css('box-shadow', 'none');
    };
    UiMobile.prototype.noConnection = function () {
        if (!this.isConnected)
            return;
        this.isConnected = false;
        $('#noConnection').show();
    };
    UiMobile.prototype.hasConnection = function () {
        if (this.isConnected)
            return;
        this.isConnected = true;
        $('#noConnection').hide();
    };
    UiMobile.prototype.ressize = function () {
        var w = window.innerWidth;
        var h = window.innerHeight;
        if (this.isDetailsShown) {
            var itm = this.getScreenWrapper();
            itm.css({ left: -w + "px" });
        }
        var hi = $('.headerIcon');
        if (hi.length > 0) {
            hi.css({ left: (w - 70) + 'px' });
        }
        var headerH = $('.header').innerHeight();
        var contH = h - headerH;
        $('#rootContent').css({ height: contH + "px" });
        var padCss = { height: contH, top: headerH, left: 0, width: w };
        $('#dialogPad').css(padCss);
        $('#mainMenu').css(padCss);
    };
    UiMobile.prototype.menuClick = function () {
        this.hideShowMenu();
    };
    return UiMobile;
})();
//# sourceMappingURL=MobileUi.js.map