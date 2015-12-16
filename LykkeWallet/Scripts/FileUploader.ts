interface IUploadFile {
    id: string;
    url: string;
    maxFileSize: number;
    onBeginUpload?: () => void;
    onSuccess?: (result) => void;
    onFail?: (result) => void;
    exts: string[];
    onWrongExtention?: () => void;
    onFileSizeExceed?: () => void;

}


class FileUpploader {

    public static uploadFile(data: IUploadFile) {

        $('#panelMaxSize' + data.id).hide();
        $('#panelExtention' + data.id).hide();



        var filePicker = $('#filePicker' + data.id);
        var fileExt = filePicker.val().split('.').pop().toLowerCase();



        if (fileExt && fileExt != "") {


            if (data.exts) {
                var found = false;
                for (var i in data.exts) {
                    if (fileExt == data.exts[i]) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    if (data.onWrongExtention)
                        data.onWrongExtention();
                    return;
                }
            }

            var fileSize = (<any>filePicker[0]).files[0].size;

            if (fileSize / 1024 > data.maxFileSize) {
                if (data.onFileSizeExceed)
                    data.onFileSizeExceed();
                return;
            }
        } else {
            return;
        }

        $('#fileExt' + data.id).val(fileExt);

        var formData = new FormData(<any>($("#form" + data.id).get(0)));
        if (data.onBeginUpload)
            data.onBeginUpload();

        $.ajax({
            url: data.url,
            type: 'POST',
            xhr: () => {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', e=> {
                        if (e.lengthComputable) {
                            $("#progress" + data.id).attr({ value: e.loaded, max: e.total });
                        }
                    }, false);
                }
                return myXhr;
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        }).then(result => {

            if (data.onSuccess)
                data.onSuccess(result);

        }).fail((jqXhr) => {
            if (data.onFail)
                data.onFail(jqXhr);
        });
    }

};
