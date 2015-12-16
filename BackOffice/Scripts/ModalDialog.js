/// <reference path="ui.ts" />
var ModalDialogEngine = (function () {
    function ModalDialogEngine() {
    }
    ModalDialogEngine.prototype.hideDialog = function (result) {
        this.ui.hideDialog();
    };
    ModalDialogEngine.prototype.show = function (data) {
        this.ui.showDialog(data.url, data.data);
    };
    return ModalDialogEngine;
})();
var ModalDialog = new ModalDialogEngine();
//# sourceMappingURL=ModalDialog.js.map