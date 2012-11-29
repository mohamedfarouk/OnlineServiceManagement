/// <reference path="jquery-1.7.1.js" />
/// <reference path="knockout-2.1.0.js" />

var koHelper = {
    ViewModel: {},
    LoadViewModel: function (resultData) {
        //var strViewModel = document.getElementById(viewModelFieldId).value;
        //var clientViewModel = ko.utils.parseJson(resultData);
        var clientViewModel = resultData;
        this.ViewModel = clientViewModel;
        ko.applyBindings(this.ViewModel);
    },
    getWindowsServices: function () {
        $.ajax({
            url: "/Home/GetWindowsServices/",
            type: 'post',
            //data: "{'customerID':'1' }",
            contentType: 'application/json',
            success: function (result) {
                koHelper.LoadViewModel(result);
                var dt = new Date();
                koHelper.UpdateStatus("Autoupdate service status by Signal-R");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errorMessage = '';
                $('#message').html(jqXHR.responseText);
            }
        });
    },
    StartService: function (data, event) {        
        $.ajax({
            url: "/Home/StartService/",
            type: 'post',
            data: JSON.stringify( { 'ServiceName': data.ServiceName } ),
            contentType: 'application/json',
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    koHelper.LoadViewModel(result);
                    var dt = new Date();
                    koHelper.UpdateStatus("StartService");
                    //$.windowsServiceManager.performedServiceStatusChange(data.ServiceName);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errorMessage = '';
                $('#message').html(jqXHR.responseText);
            }
        });
    },
    StopService: function (data, event) {
        $.ajax({
            url: "/Home/StopService/",
            type: 'post',
            data: JSON.stringify({ 'ServiceName': data.ServiceName }),
            contentType: 'application/json',
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    koHelper.LoadViewModel(result);
                    var dt = new Date();
                    koHelper.UpdateStatus("StopService");
                    //$.windowsServiceManager.performedServiceStatusChange(data.ServiceName);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errorMessage = '';
                $('#message').html(jqXHR.responseText);
            }
        });
    },
    RestartService: function (data, event) {
        $.ajax({
            url: "/Home/RestartService/",
            type: 'post',
            data: JSON.stringify({ 'ServiceName': data.ServiceName }),
            contentType: 'application/json',
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    koHelper.LoadViewModel(result);
                    var dt = new Date();
                    koHelper.UpdateStatus("RestartService");
                    //$.windowsServiceManager.performedServiceStatusChange(data.ServiceName);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errorMessage = '';
                $('#message').html(jqXHR.responseText);
            }
        });
    },
    UpdateStatus: function (statusData) {
        var content = "</br>" + new Date() + "</br>" + statusData + "</br>";
        content += $('#statusInfo').html() ;
        $('#statusInfo').html(content);
    }
}


