interface IUi {
    ressize();
    hideDialog();
    initFocus();
    showError(component: string, text: string, caption?: string, placement?: string);
}