var UiMobile = (function () {
    function UiMobile() {
        var _this = this;
        this.menuIsShown = false;
        $(window).resize(function () { return _this.ressize(); });
    }
    UiMobile.prototype.showDialog = function (url, params) {
        var _this = this;
        if (this.menuIsShown)
            return;
        if (this.dialogIsShown)
            return;
        $('#dialog').show();
        $('#dialogPad').fadeIn(200);
        this.dialogIsShown = !this.dialogIsShown;
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then(function (result) {
            $('#dialog').html(result);
            _this.initFocus();
        }).fail(function (jqXhr) {
            if (jqXhr && jqXhr.status === 403)
                window.location.reload();
        });
    };
    UiMobile.prototype.hideDialog = function (callback) {
        if (!this.dialogIsShown)
            return;
        $('#dialogPad').fadeOut(200, function () {
            $('#dialog').hide();
            $('#dialog').html('');
        });
        this.dialogIsShown = !this.dialogIsShown;
    };
    UiMobile.prototype.initFocus = function () {
        $('.setCursor').focus();
        $('.setCursor').select();
    };
    UiMobile.prototype.showError = function (component, text, caption, placement) {
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
    UiMobile.prototype.noConnection = function () { };
    UiMobile.prototype.hasConnection = function () { };
    UiMobile.prototype.ressize = function () {
        var header = $('.header');
        var body = $(window);
        var cnvH = body.innerHeight() - header.innerHeight();
        $('#rootContent').css({ width: body.outerWidth() + "px", top: header.outerHeight() + "px", height: cnvH + "px" });
        var mstrDetail = $('#msrtDtl');
        mstrDetail.width(body.outerWidth() * 2 + "px");
        mstrDetail.height(cnvH);
        $("#pamain").css({ width: body.outerWidth() + "px", height: cnvH + "px" });
        $("#detail").css({ width: body.outerWidth() + "px", height: cnvH + "px" });
        $("#mainMenu").height(cnvH);
        $('#dialogPad').css({ top: header.innerHeight() + "px", height: cnvH + "px", width: body.outerWidth() + "px" });
    };
    UiMobile.prototype.menuClick = function () {
        if (this.dialogIsShown)
            return;
        if (this.menuIsShown) {
            $('#dialogPad').fadeOut(200);
            $('#mainMenu').animate({ left: "-220px" }, 200, function () {
                $('#mainMenu').hide();
            });
        }
        else {
            $('#mainMenu').css({ left: "-220px" });
            $('#mainMenu').show();
            $('#dialogPad').fadeIn(200);
            $('#mainMenu').animate({ left: 0 }, 200);
        }
        this.menuIsShown = !this.menuIsShown;
    };
    UiMobile.prototype.padClick = function () {
        if (this.menuIsShown)
            this.menuClick();
        if (this.dialogIsShown)
            this.hideDialog();
    };
    return UiMobile;
})();
//# sourceMappingURL=UiMobile.js.map