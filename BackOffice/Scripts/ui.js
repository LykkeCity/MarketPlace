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
        $('#modalData').html('<div style="text-align:center;"><img src="/Images/Loading-pa.gif"/></div>');
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then(function (result) {
            $('#modalData').html(result);
            _this.initFocus();
            _this.pageManager.ressize();
        }).fail(function (jqXhr) {
            if (jqXhr && jqXhr.status === 403)
                window.location.reload();
        });
    };
    UiDesktop.prototype.ressize = function () {
    };
    UiDesktop.prototype.showDialog = function (url, params) {
        var _this = this;
        if (params && typeof params === "string" && params[0] === '#')
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
    return UiDesktop;
})();
//# sourceMappingURL=ui.js.map