var FileUpploader = (function () {
    function FileUpploader() {
    }
    FileUpploader.uploadFile = function (data) {
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
            var fileSize = filePicker[0].files[0].size;
            if (fileSize / 1024 > data.maxFileSize) {
                if (data.onFileSizeExceed)
                    data.onFileSizeExceed();
                return;
            }
        }
        else {
            return;
        }
        $('#fileExt' + data.id).val(fileExt);
        var formData = new FormData(($("#form" + data.id).get(0)));
        if (data.onBeginUpload)
            data.onBeginUpload();
        $.ajax({
            url: data.url,
            type: 'POST',
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', function (e) {
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
        }).then(function (result) {
            if (data.onSuccess)
                data.onSuccess(result);
        }).fail(function (jqXhr) {
            if (data.onFail)
                data.onFail(jqXhr);
        });
    };
    return FileUpploader;
})();
;
//# sourceMappingURL=FileUploader.js.map