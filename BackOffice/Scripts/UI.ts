/// <reference path="typings/framework.d.ts" />



class UiDesktop implements IUi {

    pageManager:IPageManager;

    initFocus() {
        $('.setCursor').focus();
        $('.setCursor').select();
    }

    public dialogIsShown = false;

    private loadModalContent(url: string, params?: string) {
        $('#modalData').html('<div style="text-align:center;"><img src="/Images/Loading-pa.gif"/></div>');
        $.ajax({
            url: url,
            data: params,
            type: 'POST'
        }).then((result) => {
                $('#modalData').html(result);
                this.initFocus();
                this.pageManager.ressize();
            }).fail(jqXhr => {
                if (jqXhr && jqXhr.status === 403)
                    window.location.reload();
            });
    }

    public ressize() {
        
    }


    dialogResult:string;

    private onClose: (result: string) => void;

    showDialog(url: string, params?: string) {

        if (params && typeof params === "string" && params[0] === '#') 
            params = $(params).serialize();

        this.onClose = undefined;

        $('#modalBackground').fadeIn(200, () => {
            //      $('#pamain').css('-webkit-filter', 'blur(4px)');
            this.loadModalContent(url, params);
        });


        this.dialogIsShown = true;
    }

    public showDialogWithCallBack(data: IShowDialog) {
        this.showDialog(data.url, data.data);
        this.onClose = data.onClose;
        this.dialogResult = "close";
    }


    private hideDialogLaptop(callback?: () => void, result?: string) {
   //     $('#pamain').css('-webkit-filter', '');


        $('#modalBackground').fadeOut(200, () => {
            $('#modalData').html("");
            this.dialogIsShown = false;
            if (callback)
                callback();
            if (this.onClose)
                this.onClose(result);
        });
    }


    hideDialog(callback?: () => void) {
        if (!this.dialogIsShown) {
            if (callback)
                callback();

            if (this.onClose)
                this.onClose(undefined);
            return;
        }

            this.hideDialogLaptop(callback);
    }

    hideDialogWithResult(result:string) {
            this.hideDialogLaptop(undefined, result);
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
    }



    private isConnected = true;
    noConnection() {
        if (!this.isConnected)
            return;

        this.isConnected = false;
        this.blur();
        $('#noConnection').show();
    }

    public hasConnection() {
        if (this.isConnected)
            return;

        this.isConnected = true;
        $('#noConnection').hide();
        this.unBlur();
    }


    private isBlured = false;
    public blur() {
        if (this.isBlured)
            return;

        $('#pamain').addClass('blur');

        this.isBlured = true;

    }

    public unBlur() {
        if (!this.isBlured)
            return;

        $('#pamain').removeClass('blur');

        this.isBlured = false;

    }

}
