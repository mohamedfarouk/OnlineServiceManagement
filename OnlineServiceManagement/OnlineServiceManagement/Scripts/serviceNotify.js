/// <reference path="common.js" />


$(function () {

    $.windowsServiceManager = $.connection.windowsServiceManager;
    $.windowsServiceManager.notifyChanges = function (serviceName) {
        //alert("status changed for service " + serviceName);
        koHelper.getWindowsServices();
    }
  

    //$.connection.hub.start(function () {
    //    //photoTracker.hasAnyOneSharedNewPhotos()
    //    //.done(function (data) {
    //    //        if (data != "No") {
    //    //            //photoTracker.updateSlideView(data);
    //    //        }
    //    //});        
    //}).done(function () {
    //    $.windowsServiceManager.registerMe();
    //});

    $.connection.hub.start().done(function () {
        $.windowsServiceManager.registerMe();
    });
    
});