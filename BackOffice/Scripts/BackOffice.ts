/// <reference path="modaldialog.ts" />

class BackOffice {

    private static clientViewDialogUrl: string;

    public static init(clientViewDialogUrl: string) {
        this.clientViewDialogUrl = clientViewDialogUrl;
    }

    public static showClientInfoDialog(id: string) {
        ModalDialog.show({ url: this.clientViewDialogUrl, data: 'id=' + id });
    }
}


interface IRangeMode {
    Id: string;
    Name:string
}

class RangeModes {
    
    private static selectedRangeElement: string;

    public static SelectRangeElement(rangeprofilesUrl:string, id: string) {
        if (this.selectedRangeElement)
            $(this.selectedRangeElement).removeClass('listElementSelected');

        this.selectedRangeElement = '#' + id;

        $(this.selectedRangeElement).addClass('listElementSelected');

        this.GetRangeProfiles(rangeprofilesUrl, id);
    }

    public static ShowCloneRangeMode(id: string) {
        $('#SrcId').val(id);
        $('#newElement').show();
        $('#NewModeName').focus();
        $('#NewModeName').select();
    }

    public static GetRangeProfiles(url: string, id: string) {
        $('#rangeProfiles').html('');

        $.ajax({ url: url, type: 'POST', data: 'Id=' + id }).then(result=> {
            $('#rangeProfiles').html(result);
        });
    }

    public static RefreshModelList(url: string, div: string) {
        $.ajax({ url: url, type: "POST" }).then( result => {
            var html = "";
            for (var i = 0; i < result.length; i++) {
                html += '<option value="' + result[i].Id + '">' + result[i].Name + '</option>';
                $(div).html(html);
            }
        });
    }


     
    private static htmlBeforeDelete: string;
    private static divBeforeDelete: string;

    public static DeleteConfirmation(div: string, url:string, params:string) {
        this.CancelDelete();

        this.htmlBeforeDelete = $(div).html();
        this.divBeforeDelete = div;
        var html = '<div>Confirm delete action' +
            '<a class="btn btn-success btn-xs" onclick="Requests.doRequest({ Url: \''+url+'\', Params: \''+params+'\' })"><span class="glyphicon glyphicon-pencil" ></span>' +
            '<a class="btn btn-warning btn-xs" onclick="RangeModes.CancelDelete()"><span class="glyphicon glyphicon-remove"></span>' +
            '</a></div>';
        $(div).html(html);
    }

    public static CancelDelete() {
        if (this.divBeforeDelete) {
            $(this.divBeforeDelete).html(this.htmlBeforeDelete);
            this.divBeforeDelete = undefined;
            this.htmlBeforeDelete = undefined;
        }  
    }


}