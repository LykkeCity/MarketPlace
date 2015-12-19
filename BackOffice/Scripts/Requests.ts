/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/bootstrap/bootstrap.d.ts" />
/// <reference path="typings/lib.d.ts" />
/// <reference path="ui.ts" />

interface IRequestData {
    url: string;
    formId?: string;
    params?: any;
    onStart?: () => void;
    onFinish?: () => void;
    divResult?: string;
    divError?: string;
    placement? :string;
    callBack?: (result?) => void;
    replaceDiv? : boolean;

    hideSocial?: boolean;

    putToHistory?: boolean;

    onShowLoading?: () => void;
    onHideLoading?: () => void;
    showLoading?: boolean;
    notToHistory? : boolean;
}




class Requests {
    public static pageManager: IPageManager;
    public static ui: IUi;

    private static reloadAction ={
        reloadPage: true,
        url: "",
        data: undefined,
        div: ""
    }

    public static doRequest(o: IRequestData) {

        o.params = o.formId ? $(o.formId).serialize() : o.params;

        if (o.onStart)
            o.onStart();

        this.blockOnRequest();

        if (o.divResult) {
            $(o.divResult).fadeOut(200, () => this.peformRequest(o));
        } else
            this.peformRequest(o);

    }

    static setRefreshAsReloadPage() {
        this.reloadAction.reloadPage = true;
    }

    static setRefreshUrl(div:string, url:string, data?) {
        this.reloadAction.reloadPage = false;
        this.reloadAction.div = div;
        this.reloadAction.url = url;
        this.reloadAction.data = data;
    }


    private static peformRequest(o: IRequestData) {

        if (o.onShowLoading)
            o.onShowLoading();
        else {
            if (o.showLoading !== false && o.divResult) {
                $(o.divResult).html('<div style="text-align:center; margin-top:20px;"><img src="/Images/Loading-pa.gif"/></div>');
                $(o.divResult).show();
            }
        }

        var options = { url: o.url, type: 'POST', data: o.params };
        $.ajax(options)
            .then(result => {
                if (o.onHideLoading)
                    o.onHideLoading();
                if (o.onFinish)
                    o.onFinish();

                if (o.divResult)
                    $(o.divResult).hide();

                this.unBlockOnRequest();
                this.requestOkHandler(o, result);
            })
            .fail(jqXhr => {
                if (o.onHideLoading)
                    o.onHideLoading();
                if (o.onFinish)
                    o.onFinish();
                this.unBlockOnRequest();
                this.requestFailHandle(o, jqXhr.responseText, jqXhr);
            });

    }

    public static requestOkHandler(o: IRequestData, result) {


        if (result.status === 'Fail') {

            if (result.divError)
                o.divError = result.divError;

            o.placement = result.Placement;

            this.requestFailHandle(o, result.msg);
            return;
        }

        if (result.status === "Reload") {
            this.ui.hideDialog();
            if (this.reloadAction.reloadPage)
                location.reload();
            else
                this.doRequest({ url: this.reloadAction.url, params: this.reloadAction.data, divResult: this.reloadAction.div }); 

            return;
        }


        if (result.refreshUrl) {
            this.ui.hideDialog();
            this.doRequest({ url: result.refreshUrl, params: result.prms, divResult: result.div, putToHistory: result.putToHistory });
            return;
        }

        if (result.inputId) {
            $(result.inputId).val(result.text);
            return;
        }

        if (result.status === "Request") {
            this.doRequest({ url: result.url, params: result.prms, putToHistory: result.putToHistory });
            return;
        }


        if (result.status === "OkAndNothing") {
            if (o.callBack)
                o.callBack(result);
            return;
        }

        if (result.status === "ShowDialog") {
            this.ui.hideDialog(() => {
                this.ui.showDialog(result.url, result.prms);
            });
        }

        if (result.status === "Redirect") {
            if (result.Params) {
                window.location = <any>(result.Url + "?" + result.Params);
            } else
                window.location = result.Url;
            return;
        }





        if (o.divResult) {

            if (o.replaceDiv) {
                $(o.divResult).replaceWith(result);
            } else {
                $(o.divResult).html(result);

                $(o.divResult).fadeIn(200, () => {
                    this.pageManager.ressize();
                });
            }
            this.pageManager.ressize();

            this.ui.initFocus();

        }

    }


    public static handleErrorReq(o, result) {

        if (result.status === "Fail")
            this.ui.showError(result.divError, result.Result, undefined, result.Placement);

        else if (result.responseText && o.divError)
            this.ui.showError(o.divError, result.responseText);
    }

    public static requestFailHandle(o: IRequestData, text, jqXhr?) {
        if (jqXhr && jqXhr.status === 403) {
            window.location.reload();
        } else {
            this.ui.showError(o.divError, text, undefined, o.placement);
        }

    }

    public static blockOnRequest() {

        $('.disableOnRequest').each(function() {
            $(this).attr('disabled', 'true');
        });

        $('.hideOnRequest').each(function() {
            $(this).css('display', 'none');
        });

        $('.showOnRequest').each(function() {
            $(this).css('display', 'inline');
        });
    }

    public static unBlockOnRequest() {
        $('.disableOnRequest').each(function() {
            $(this).removeAttr('disabled');
        });

        $('.hideOnRequest').each(function() {
            $(this).css('display', 'inline');
        });

        $('.showOnRequest').each(function() {
            $(this).css('display', 'none');
        });
    }

}

var requests = Requests;


