interface IUi {
    showDialog(url: string, params?: string);
    hideDialog(callback?: () => void);
    initFocus();
    showError(component: string, text: string, caption?: string, placement?: string);
    noConnection();
    hasConnection();

    dialogIsShown: boolean;

    ressize();
}


interface IPageManager {
    navigate(aurl: string, aparams?: string, anoEffect?: boolean);
    refresh();
    ressize();
    hideHeader(callback?: () => void);
}




interface IUiMobile {
    hideDetails(): boolean;
    hideDialog(callback?: () => void): boolean;
    showHeaderShadow();
}