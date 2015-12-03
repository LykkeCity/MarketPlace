class UiMobile implements IUi {

    init(demoOrRealUrl: string) {
        this.demoOrRealUrl = demoOrRealUrl;
    }

    showError(component: string, text: string, caption?: string, placement?: string) {

        if (text !== "") {
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
        $('html, body').animate({
            scrollTop: $(component).offset()+ 'px'
        });
    }



    private menuIsHidden = true;

    public hideShowMenu() {
        this.menuIsHidden = !this.menuIsHidden;

        if (this.menuIsHidden) {
            $('#mainMenu').hide();
        }
        else {
            $('#mainMenu').show();
        }

    }

    public demoOrRealUrl: string;
    public isLive = true;



    public initFocus() {
        $('.setCursor').focus();
        $('.setCursor').select();
    }

    private dialogHeader :JQuery;
    private getDialogHeader():JQuery {

        if (!this.dialogHeader)
            this.dialogHeader = $('#dialogHeader');
        return this.dialogHeader;
    }

    private dialogItem: JQuery;
    private getDialogItem(): JQuery {
        if (!this.dialogItem)
            this.dialogItem = $('#mobileDialog');

        return this.dialogItem;
    }


    private dialogData:JQuery;
    private getDialogData(): JQuery {
        if (!this.dialogData)
            this.dialogData = $('#dialogData');

        return this.dialogData;
    }
    private showContent(callBack:()=>void) {
        var itm = this.getDialogItem();
        this.getDialogHeader().html('');

        var w = window.innerWidth;
        itm.css({ width: w + 'px', left: -w + 'px', top:0 });
        itm.show();
        itm.animate({ left: 0 },200, callBack);
    }

    private loadModalContent(j:JQuery, url: string, params?: any) {
        j.html('<div style="text-align:center;"><img src="/Images/Loading.gif" style="margin-top:20px;"/></div>');
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then((result) => {
            j.html(result);
                this.initFocus();
            }).fail(jqXhr => {
                if (jqXhr && jqXhr.status === 403)
                    window.location.reload();
            });
    }

    private onClose: (result?: string) => void;
    dialogIsShown = false;

    showDialog(url: string, params?: string) {
        if (this.dialogIsShown) {
            this.hideDialog(() => {
                this.showDialog(url, params);
            });
            return;
        }
        this.onClose = undefined;

        this.showContent(() => {
            //PersonalArea.pushMobileBackToHistory();
            this.loadModalContent(this.getDialogData(), url, params);
        });

        this.dialogIsShown = true;
    }


    private hideDialogMobile(callback?: () => void, result?: string) {

        var w = window.innerWidth;
        var itm = this.getDialogItem();

        itm.animate({
                left: -w + 'px'
            },
            200, () => {
                itm.hide();
                this.getDialogData().html('');
                this.dialogIsShown = false;

                if (this.onClose)
                    this.onClose(result);

                if (callback) callback();

            });
    }

    public hideDialog(callback?: () => void):boolean {
        if (!this.dialogIsShown) {
            if (callback)
                callback();
            if (this.onClose)
                this.onClose(undefined);
            return false;
        }

        this.hideDialogMobile(callback);
        return true;
    }


    private screenWrapperItm : JQuery;
    private getScreenWrapper():JQuery {
        if (!this.screenWrapperItm)
            this.screenWrapperItm = $('#msrtDtl');

        return this.screenWrapperItm;
    }

    setDetailsCaption(caption: string) {
        $('#detailsHeader').html(caption);
        
    }

    private isDetailsShown = false;
    showDetails(data: { url: string; params?: any; title: string }) {

        if (this.isDetailsShown)
            return;

        this.setDetailsCaption(data.title);

        var w = this.getScreenWidth();
        var itm = this.getScreenWrapper();

        itm.animate({
                left: -w + 'px'
            },
            200, () => {
            //PersonalArea.pushMobileBackToHistory();
            this.isDetailsShown = true;
            this.loadModalContent($('#detailsContent'), data.url, data.params);
        });

    }


    hideDetails():boolean {
        if (!this.isDetailsShown)
            return false;

        var itm = this.getScreenWrapper();

        itm.animate({
            left: 0
        },
            200, () => {
                this.isDetailsShown = false;
                $('#detailsContent').html('');
                $('#detailsHeader').html('');
            });

        return true;
    }


    showHeaderShadow() {
        $('#appHeader').css('box-shadow', '0 2px 2px rgba(0, 0, 0, 0.17)');
    }


    hideHeaderShadow() {
        $('#appHeader').css('box-shadow','none');
    }

    private isConnected = true;
    noConnection() {
        if (!this.isConnected)
            return;

        this.isConnected = false;
        $('#noConnection').show();
    }

    hasConnection() {
        if (this.isConnected)
            return;

        this.isConnected = true;
        $('#noConnection').hide();
    }


    private getScreenHeight() {
        return window.innerHeight;
    }

    private getScreenWidth() {
        return window.innerWidth;
    }

    ressize() {
        var w = this.getScreenWidth();
        var h = this.getScreenHeight();

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

        $('#rootContent').css({ height: h + "px", width: w + "px" });
        $('#msrtDtl').css({ height: contH + "px", width: w*2 + "px"});

        var padCss = { height: contH, top: headerH, left: 0, width: w };
        $('#dialogPad').css(padCss);
        $('#mainMenu').css(padCss);
        $('#pamain').css(padCss);

    }


    menuClick() {
        this.hideShowMenu();
    }




} 

