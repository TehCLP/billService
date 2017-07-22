function formatDate(evt) {
    var e = event || evt;
    var charCode = e.which || e.keyCode;
    var re = "^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d+$";
    
}
function onlyDecimal(evt) {
    var e = event || evt; // for trans-browser compatibility
    var charCode = e.which || e.keyCode;
    var b = !(charCode < 48 || charCode > 57);
    return b;
}
function printPage() {
    var contentL =  jQuery('#contentL');
    var wrapper = jQuery('#wrapper');
    var bottomButton = jQuery(".bottombutton");
    var gridview = jQuery("#ctl00_cph_content_gvListRepair");
    var pnlGVEdit = jQuery("#pnlGridRepair_Edit");
    var pnlGVPrint = jQuery("#pnlGridRepair_Print");
    contentL.css({ "display": "none" });
    wrapper.css({ "border": "none" });
    bottomButton.css({ "display": "none" });
    gridview.removeClass("mGrid").addClass("mGrid_print");
    pnlGVEdit.css({ "display": "none" });
    pnlGVPrint.css({ "display": "block" });
    
    window.print();

    contentL.removeAttr("style")
    wrapper.css({ "border": "1px solid #eaeaea" });
    bottomButton.css({ "display": "block" });
    gridview.removeClass("mGrid_print").addClass("mGrid");
    pnlGVEdit.css({ "display": "block" });
    pnlGVPrint.css({ "display": "none" });
    
    return false;
}