/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />


var configController = function () {
    this.HostType = [{ text: "File System", value: 1 }, { text: "Share Folder", value:2 }, { text: "Share Point", value:3 }];

}
configController.MappingFolders = function () {
    $.post("../Config/MappingFolder",function(data){
        if (data.Result) {
            alert("Folder Mapping Successfull");
        } else {
            alert(data.Message);
        }
    });
}

configController.Save = function () {
    $("#cofigform").form("submit", {
        url: '../Config/SaveConfig',
        success: function (data) {
            var redata = JSON.parse(data);
            if (redata.Result) {
                alert("Config Saved Successfull");
            }
        }
    });
}
configController.prototype.Load = function (data) {
    com.common.showFunCategory("account");
    com.common.activeFunction("fun_Config");
        $.post("../Config/GetConfigList", function (data) {
            if (data.Result) {
                data = data.data;
                $("#cofigform").form("load", data);
            } else {
                alert(data.Message);
            }
        });
    $("#configpanel").panel({
        title: "App Config",
        width:'300',
        height: '300',

        style: {
            'margin-left': '400px',
            'margin-top':'100px'
        }
    });
   
    var hostsetting = {
        data: this.HostType,
        valueField: 'value',
        textField: 'text',
        onSelect: function (e) {
            $("#UserId").textbox("setValue",null);
            $("#Password").textbox("setValue", null);
            $("#Domain").textbox("setValue", null);
            if (e.value == 2||e.value==3) {
                $("#UserId").textbox("enable");
                $("#Password").textbox("enable");
               $("#Domain").textbox("enable");
            } else {
                
                $("#UserId").textbox("disable");
                $("#Password").textbox("disable");
                $("#Domain").textbox("disable");
            }
        }
    }
    com.common.Combox("HostType", hostsetting);
 
   
    }
 