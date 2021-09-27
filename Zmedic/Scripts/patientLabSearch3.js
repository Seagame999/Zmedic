function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 32)
        return true;
    if (48 <= charCode && charCode <= 57)
        return true;
    if (65 <= charCode && charCode <= 90)
        return true;
    if (97 <= charCode && charCode <= 122)
        return true;
    return false;
}