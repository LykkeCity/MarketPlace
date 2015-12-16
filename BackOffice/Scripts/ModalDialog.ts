/// <reference path="ui.ts" />




class ModalDialogEngine {

    public ui : IUi; 

    private callBack: (result) => void;


    public hideDialog(result) {
        this.ui.hideDialog();
    }

    public show(data: IModalDialogData) {
        this.ui.showDialog(data.url, data.data);
    }

}

var ModalDialog = new ModalDialogEngine();