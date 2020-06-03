


$(document).ready(function () {
    CKEDITOR.replace("txtDetail", {
        customConfig: '/Assets/Admin/plugins/ckeditor/config.js',
    });
    $("#btnselectImg").click(function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (url) {
            $("#imgtxt").val(url);
        };
        finder.popup();
    });
});