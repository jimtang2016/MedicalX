/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />
/// <reference path="../Common.js" />
var AlertType = [{ Value: 0, Text: "MileStone" }, { Value: 1, Text: "Customer" }];
var RuleFields = [{ Value: "PlanStartDate", Text: "PlanStartDate" }, { Value: "ActualStartDate", Text: "ActualStartDate" }, { Value: "ActualEndDate", Text: "ActualEndDate" }, { Value: "PlanEndDate", Text: "PlanEndDate" }];


var AlertRule = [{ Value: 0, Text: "Before" }, { Value: 1, Text: "After" }];
formateAlertType = function (value, row, index) {
    if (value == 0)
        return "MileStone";
    else if (value == 1) {
        return "Customer";
    } else {
        return value;
    }
}
formateAlertRule = function (value, row, index) {
    if (value == 0)
        return "Before";
    else if (value == 1) {
        return "After";
    } else {
        return value;
    }
}
formateMileStone = function (value, row, index) {
    var datas = com.NotifyController.datas;
    var tvalue = value;
    if (datas != null)
        for (var i = 0; i < datas.length; i++) {
            if (datas[i].Id == parseInt(value)) {
                tvalue = datas[i].Title;
                break;
            }
        }

    return tvalue;
}

function loadmilestone(ele) {
    var param = {};
    param.studyId = $("#StudyId").val();
    var countryId = $("#StudyCountryId").val();
    var siteId = $("#StudySiteId").val();
    var tmfid = $("#StudyTMFId").val();
    param.stonetype = 2;
    if (siteId > 0) {
        param.stonetype = 1;
        param.id = siteId;
    } else if (countryId > 0) {
        param.stonetype = 0;
        param.id = countryId;
    }

    $.ajax({
        url: "../Study/GetMileStoneView",
        method: 'post',
        data: {
            studyId: param.studyId, id: param.id, stonetype: param.stonetype
        }
        ,
        success: function (data) {
            while (true) {
                if (com.NotifyController.datas.length > 0) {
                    com.NotifyController.datas.pop();
                } else {
                    break;
                }
            }
           
            $(data).each(function (i, item) {
                com.NotifyController.datas.push(item);
            });
            $(ele).datagrid({ queryParams: { studyId: param.studyId, countryId: countryId, siteId: siteId, tmfId: tmfid } });
        }

    });
}
com.NotifyController = {
    datas: [],
    init: function (el) {
        tooltip = [{
            text: 'Add',
            iconCls: 'icon-add',
            handler: function () {
                com.NotifyController.Add();
            }
        }, '-',

    {
        text: 'Remove',
        iconCls: 'icon-remove',
        handler: function () {
            com.NotifyController.Remove();
        }
    }];

        $(".notifymenuitems").click(function () {

            if ($(this).attr("id").indexOf("remove") > 0) {
                com.NotifyController.Remove();
            } else   if ($(this).attr("id").indexOf("add") > 0) {
                    com.NotifyController.Add();
                
            } else if ($(this).attr("id").indexOf("edit") > 0) {
                com.NotifyController.Edit();

            }
        });

        com.common.initEditGrid(el, "Notify Rules", "", "../StudyDocument/GetNotifyRules", tooltip);

        $("#" + el).datagrid({
            singleSelect: true,
            onRowContextMenu: function (e, index, row) {
                $("#notifymenu").css("margin-left", e.screenX);
                $("#notifymenu").css("margin-top", e.screenY-60);
                $("#notifymenu").show();
               
                e.preventDefault();
            },
            columns: [
                [{
                    field: 'AlertType', title: "Alert Type", formatter: formateAlertType, editor: {
                        type: 'combobox', options: {
                            data: AlertType, valueField: 'Value', textField: 'Text', id: 'divAlertType', onChange: function (newvalue, oldvalue) {
                                var selected = $("#" + el).datagrid("getSelections");
                                var index = $("#" + el).datagrid("getRowIndex", selected[selected.length-1]);
                                var target = $("#" + el).datagrid("getEditor", { index: index, field: "MileStoneId" }).target;
                                var targetf = $("#" + el).datagrid("getEditor", { index: index, field: "RuleField" }).target;
                                var targett = $("#" + el).datagrid("getEditor", { index: index, field: "TriggerOnDateText" }).target;
                                var targetar = $("#" + el).datagrid("getEditor", { index: index, field: "AlertRule" }).target;

                                var targettd = $("#" + el).datagrid("getEditor", { index: index, field: "TriggerDay" }).target;
                                if (newvalue == 1) {
                                    $(target).combobox("setValue", null);
                                    $(targetf).combobox("setValue", null);
                                    $(targetar).combobox("setValue", null);
                                    $(targettd).val(null);

                                    $(target).combobox("disable");
                                    $(targetf).combobox("disable");
                                    $(targett).datebox("enable");
                                    $(targetar).combobox("disable");

                                    $(targettd).attr("disabled", "disabled");
                                } else {
                                    $(targett).datebox("setValue", null);
                                    $(target).combobox("enable");
                                    $(targetf).combobox("enable");
                                    $(targett).datebox("disable");
                                    $(targetar).combobox("enable");
                                    $(targettd).removeAttr("disabled");
                                }
                            }
                        }
                    }, width: "200"
                },
                 {
                     field: 'MileStoneId', formatter: formateMileStone, title: "Mile Stone", editor: {
                         type: 'combobox', options: {
                             data: com.NotifyController.datas,
                             valueField: 'Id',
                             textField: 'Title'
                         }
                     }, width: "200"
                 },
                  { field: 'RuleField', title: "Rule Field", editor: { type: 'combobox', options: { data: RuleFields, valueField: 'Value', textField: 'Text' } }, width: "200" },

                   { field: 'TriggerOnDateText', title: "Trigger On Date", editor: { type: 'datebox' }, width: "200" },
                { field: 'AlertRule', title: "Alert Rule", formatter: formateAlertRule, editor: { type: 'text' }, editor: { type: 'combobox', options: { data: AlertRule, valueField: 'Value', textField: 'Text' } }, width: "200" },
                { field: 'TriggerDay', title: "Trigger Day", editor: { type: 'text' }, width: "100" },


                ]]
        });
    },
    show: function (data, el, dialel) {
        var ele = "#" + el;
        this.FillHiden(data);
        loadmilestone(ele);
        com.common.openDialog(dialel, "Notify Rules", function () {
            com.NotifyController.Save();
        }, function () {
            com.common.closeDialog(dialel);
        }, 1200, true, 500);
        $("#" + dialel).dialog({
            onClose: function () {
                com.NotifyController.ClearHiden();
            }
        });

    },
    FillHiden: function (data) {
        $("#StudyId").val(data.Study);
        $("#StudyCountryId").val(data.Country);
        $("#StudySiteId").val(data.Site);
        $("#StudyTMFId").val(data.StudyTemplateId);
    }
    ,
    ClearHiden: function () {
        $("#StudyId").val(null);
        $("#StudyCountryId").val(null);
        $("#StudySiteId").val(null);
        $("#StudyTMFId").val(null);
    }
    ,
    Add: function () {
        var data = {
            StudyId: $("#StudyId").val(),
            StudyCountryId: $("#StudyCountryId").val(),
            StudySiteId: $("#StudySiteId").val(),
            StudyTMFId: $("#StudyTMFId").val()
        }
        $("#notifyrule_data").datagrid("appendRow", data);
    }
    ,
    Edit:function(){
        var element = $("#notifyrule_data");
        var datas = element.datagrid("getSelected");
        var index = element.datagrid("getRowIndex", datas);
        element.datagrid("beginEdit", index);
    },
    Remove: function () {
        var element = $("#notifyrule_data");
        var datas = element.datagrid("getSelections");
        com.common.FinishEdit("notifyrule_data");
      var  existeditem = [];
      var   newitems = [];
        $(datas).each(function (index, item) {
            item.TriggerOnDate = null;
            if (item.Id>0) {
                existeditem.push(item);
            } else {
                var index = element.datagrid("getRowIndex", item);
                element.datagrid("deleteRow", index);
            }
        });
        if (existeditem.length > 0) {
            $.ajax({
                url: "../StudyDocument/DeleteNotifyRules",
                method: "post",
                data: { notifyRules: JSON.stringify(datas) },
                success: function (data) {
                    if (data.result) {
                        element.datagrid("reload");
                    }
                    else {
                        alert(data.message);
                    }
                },
                error: function (error) {

                }

            });
        }
        
    },
    Save: function () {
        com.common.FinishEdit(el);
        var datas = $(ele).datagrid("getChanges");
        $(datas).each(function (index, item) {
            item.TriggerOnDate = null;
        });
        $.ajax({
            url: "../StudyDocument/SaveNotifyRules",
            method: "post",
            data: { notifyRules: JSON.stringify(datas) },
            success: function (data) {
                if (data.result) {
                    alert("Notify Rules Saved");
                    com.common.closeDialog(dialel);
                }
                else {
                    alert(data.message);
                }
            },
            error: function (error) {

            }

        });
    }
}