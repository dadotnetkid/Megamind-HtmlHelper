/*
 -----------------------------------------------------------------------------------------------------
   Helper Methods 


   getValue
   enableField
   disableField
   enableOption
   disableOption
   showField
   hideField
 -----------------------------------------------------------------------------------------------------
 */


$.prototype.getValue = function () {
    var self = $(this);
    return self.val();
}
$.prototype.setValue = function (value) {
    var self = $(this);
    return self.val(value);
}
$.prototype.setSelectValue = function (value) {
    var self = $(this);
    return self.val(value).trigger('change');
}

$.prototype.enableField = function () {
    var self = $(this);
    self.removeAttr("disabled");
    return self;
}


$.prototype.disableField = function () {
    var self = $(this);
    self.attr("disabled", "");
    return self;
}


$.prototype.enableOption = function () {
    var self = $(this);
    // TODO update this method to handle the enable/disable option menu
    self.removeAttr("disabled");
    return self;
}


$.prototype.disableOption = function () {
    var self = $(this);
    // TODO update this method to handle the enable/disable option menu
    self.attr("disabled", "");
    return self;
}


$.prototype.hideField = function () {
    var self = $(this);
    // TODO update this method to handle the hide/show field
    self.hide();
    return self;
}


$.prototype.showField = function () {
    var self = $(this);
    // TODO update this method to handle the hide/show field
    self.show();
    return self;
}