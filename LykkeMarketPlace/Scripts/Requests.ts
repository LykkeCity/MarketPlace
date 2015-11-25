/// <reference path="typings/jquery/jquery.d.ts" />

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

    public doRequest(o: IRequestData) {

        o.params = o.formId ? $(o.formId).serialize() : o.params;

        if (o.onStart)
            o.onStart();

        this.blockOnRequest();


        if (o.divResult) {
            $(o.divResult).fadeOut(200, () => this.peformRequest(o));
        } else
            this.peformRequest(o);


    }



    private peformRequest(o: IRequestData) {

        if (o.onShowLoading)
            o.onShowLoading();
        else {
            if (o.showLoading && o.divResult) {
                $(o.divResult).html('<div style="margin:50px auto; text-align:center"><img src="/Images/Loading.gif"/></div>');
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

    public requestOkHandler(o: IRequestData, result) {

        if (result.Status === 'Fail') {

            if (result.divError)
                o.divError = result.divError;

            o.placement = result.Placement;

            this.requestFailHandle(o, result.Result);
            return;
        }

        if (result.Status === 'HideDialog') {
            ui.hideDialog();
            return;
        }

        if (result.inputId) {
            $(result.inputId).val(result.text);
            return;
        }

        if (result.Status === "Request") {
            this.doRequest({ url: result.url, params: result.prms, divResult: result.divResult, putToHistory: result.putToHistory,  showLoading:result.showLoading });
            return;
        }



        if (result.Status === "OkAndNothing") {
            if (o.callBack)
                o.callBack(result);
            return;
        }


        if (result.Status === "Redirect") {
            if (result.params) {
                window.location = <any>(result.url + "?" + result.params);
            } else
                window.location = result.url;
            return;
        }




        if (o.divResult) {

            if (o.replaceDiv) {
                $(o.divResult).replaceWith(result);
            }
            else {
                $(o.divResult).html(result);
                ui.ressize();
                $(o.divResult).fadeIn(200, () => {

      
                    ui.initFocus();
        
                });
            }


        }

    }


    public handleErrorReq(o, result) {

        if (result.Status === "Fail")
            ui.showError(result.divError, result.Result, undefined, result.Placement);

        else if (result.responseText && o.divError)
            ui.showError(o.divError, result.responseText);
    }

    public requestFailHandle(o: IRequestData, text, jqXhr?) {
        if (jqXhr && jqXhr.status === 403) {
            window.location.reload();
        } else {
            ui.showError(o.divError, text, undefined, o.placement);
        }

    }

    public blockOnRequest() {

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

    public unBlockOnRequest() {
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


var requests = new Requests();

