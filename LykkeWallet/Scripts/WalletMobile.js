var WalletMobile = (function () {
    function WalletMobile() {
    }
    WalletMobile.prototype.switchPage = function (url) {
        this.requests.doRequest({ url: url, showLoading: true, divResult: '#pamain' });
    };
    WalletMobile.prototype.highLightMenu = function (div) {
        if (this.currentPage)
            $(this.currentPage).removeClass("mnu-active");
        this.currentPage = div;
        $(this.currentPage).addClass("mnu-active");
    };
    WalletMobile.prototype.selectPage = function (url, mnu) {
        this.switchPage(url);
        this.highLightMenu(mnu);
    };
    return WalletMobile;
})();
//# sourceMappingURL=WalletMobile.js.map