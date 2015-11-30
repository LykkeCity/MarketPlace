
class WalletMobile {
    ui: IUi;
    requests: Requests;


    private switchPage(url:string) {
        this.requests.doRequest({ url: url, showLoading: true, divResult:'#pamain'});
    }

    private currentPage: string;

    private highLightMenu(div: string) {

        if (this.currentPage)
            $(this.currentPage).removeClass("mnu-active");

        this.currentPage = div;

        $(this.currentPage).addClass("mnu-active");
    }


    selectPage(url:string, mnu:string) {
        this.switchPage(url);
        this.highLightMenu(mnu);
    }


}