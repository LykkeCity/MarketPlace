interface IPageManager {

    ressize();

}

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

interface IUiMobile {
    hideDetails(): boolean;
    hideDialog(callback?: () => void): boolean;
    showHeaderShadow();
}

interface IShowDialog {
    url: string;
    data?: string;
    onClose: (result?: string) => void;
}

interface IModalDialogData {
    url: string;
    data?: string;
    callBack?: (result) => void;
}