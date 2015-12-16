
class UiMobile implements IUi {



    constructor() {
        $(window).resize(() => this.ressize());
    }


    showDialog(url: string, params?: string) {
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
        }).then((result) => {
            $('#dialog').html(result);
            this.initFocus();
        }).fail(jqXhr => {
            if (jqXhr && jqXhr.status === 403)
                window.location.reload();
        });
    }

    hideDialog(callback?: () => void) {


        if (!this.dialogIsShown)
            return;
        $('#dialogPad').fadeOut(200, () => {
            $('#dialog').hide();
            $('#dialog').html('');
        });
        this.dialogIsShown = !this.dialogIsShown;
    }

    initFocus() {
        $('.setCursor').focus();
        $('.setCursor').select();
    }

    showError(component: string, text: string, caption?: string, placement?: string) {
        if (text != "") {
            if (!placement)
                placement = "top";
            var options = { title: caption, html: true, content: '<div style="color:black">' + text + '</div>', placement: placement };
            $(component).popover(options);
            $(component).popover('show');

            var serrId = setTimeout(() => {
                clearTimeout(serrId);
                $(component).popover('hide');
                $(component).popover('destroy');
            }, 3000);
        }

        $(component).focus();
        $(component).select();
    }

    noConnection() {}

    hasConnection() {}

    ressize() {
        var header = $('.header');
        var body = $(window);

        var cnvH = body.innerHeight() - header.innerHeight();


        $('#rootContent').css({ width: body.outerWidth() + "px", top: header.outerHeight() + "px", height: cnvH + "px"  });

        var mstrDetail = $('#msrtDtl');
        mstrDetail.width(body.outerWidth() * 2 + "px");

        mstrDetail.height(cnvH);
        $("#pamain").css({ width: body.outerWidth() + "px", height: cnvH+"px"});
        $("#detail").css({ width: body.outerWidth() + "px", height: cnvH + "px" });
        $("#mainMenu").height(cnvH);
        $('#dialogPad').css({ top: header.innerHeight() + "px", height: cnvH + "px", width: body.outerWidth() + "px" });



    }

    dialogIsShown: boolean;

    private menuIsShown = false;

    menuClick() {

        if (this.dialogIsShown)
            return;
        if (this.menuIsShown) {
            $('#dialogPad').fadeOut(200);
            $('#mainMenu').animate({ left: "-220px" }, 200, () => {
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
    }


    padClick() {
        if (this.menuIsShown)
            this.menuClick();

        if (this.dialogIsShown)
            this.hideDialog();
    }
}