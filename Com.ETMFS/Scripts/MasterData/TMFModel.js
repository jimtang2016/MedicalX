/// <reference path="../Common.js" />
com.TMFModelController = function () {
    var self = this;
    self.Load = function () {
        com.common.activeFunction("fun_tmfm");
        com.common.showFunCategory("Masterdata");
        var tooltip=[
            {
                text: 'Add',
                iconCls: 'icon-add',
                handler: self.Add
            } , '-',
            {
                text: 'Edit',
                iconCls: 'icon-edit',
                handler: self.Edit
            }, '-',
             {
                 text: 'Remove',
                 iconCls: 'icon-remove',
                 handler: self.Remove
             }, '-',
             {
                 text: 'Import',
                 iconCls: 'icon-import',
                 handler: self.Import
             }, '-',
              {
                  text: 'Export',
                  iconCls: 'icon-import',
                  handler: self.Export
              }
        ]
        com.common.initCommonGrid("list_data", "TMF Model Reference List", 'icon-edit', "../TMFModel/GetTMFList", tooltip);

    }
    self.Add = function () {
        $("#Id").val(0);
        com.common.openDialog("add_dialog", "Add", self.Save, self.Cancel);
    }
    self.Edit = function () {
        var users = $('#list_data').datagrid('getChecked');
        if (users.length > 1) {
            alert("please choose only one item for editing");
            return;
        } else if (users.length == 0) {
            alert("please choose one item for editing");
            return;
        }
        $("#userform").form('load', users[0]);
        com.common.openDialog("add_dialog", "Edit", self.Save, self.Cancel);
    }
    self.Remove = function () {
        var users = $('#list_data').datagrid('getChecked');


        if (users.length > 0 && confirm("TMF Models will be removed from TMF Model Reference list do you confirm them?")) {
            $(users).each(function (i, item) {
                item.Created = null;
                item.Modified = null;
            });
            $.ajax({
                url: '../TMFModel/DeleteTMFs',
                type: 'post',
                data: { templates: JSON.stringify(users) },
                dataType: 'json',
                success: function (data) {
                    if (data.result) {
                        alert("TMF Models are removed ");
                        $('#list_data').datagrid('clearChecked');
                        $('#list_data').datagrid('reload');
                    } else {
                        alert("There are some errors in this operation ");
                    }

                },
                error: function (data) {
                    alert(data);
                }
            });
        }
    }
    self.Import = function () {
        com.common.openDialog("file_upload", "Import TMF Model Reference", self.UploadFile, self.UploadCancel);
    }
    self.Export = function () {
        window.open("../TMFModel/ExportExcel", "_blank");
    }
    self.UploadCancel=function(){
        $('#tmffile').form('clear');
        com.common.closeDialog('file_upload');
    }
    self.UploadFile=function(){
        $('#tmffile').form('submit', {
            url: "../TMFModel/Import",
            success: function (data) {
                var data1 = JSON.parse(data);
                if (data1.result) {
                    alert("TMF Model information imported");
                    $('#list_data').datagrid('clearChecked');
                    $('#list_data').datagrid('reload');
                    $('#tmffile').form('clear');
                    com.common.closeDialog('file_upload');
                } else {
                    alert("TMF Model information saved fail ");
                }

            }
        });
    }
    self.Save = function () {
        $('#userform').form('submit', {
            url: "../TMFModel/SaveTMF",
            success: function (data) {
                var data1 = JSON.parse(data);
                if (data1.result) {
                    alert("TMF Model information saved");
                    $('#list_data').datagrid('clearChecked');
                    $('#list_data').datagrid('reload');
                   
                    $('#userform').form('clear');
                    com.common.closeDialog('add_dialog');
                } else {
                    alert("TMF Model information saved fail ");
                }

            }
        });
    }
    self.Cancel = function () {
        com.common.closeDialog("add_dialog");
    }
    return self;
}