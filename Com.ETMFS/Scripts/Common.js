/// <reference path="jquery.min.js" />

var com = {}
Array.prototype.remove = function (val) {

    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

$.extend($.fn.form.methods, {
    getData: function (jq, params) {
        var formArray = jq.serializeArray();
        var oRet = {};
        for (var i in formArray) {
            if (typeof (oRet[formArray[i].name]) == 'undefined') {
                if (params) {
                    oRet[formArray[i].name] = (formArray[i].value == "true" || formArray[i].value == "false") ? formArray[i].value == "true" : formArray[i].value;
                }
                else {
                    oRet[formArray[i].name] = formArray[i].value;
                }
            }
            else {
                if (params) {
                    oRet[formArray[i].name] = (formArray[i].value == "true" || formArray[i].value == "false") ? formArray[i].value == "true" : formArray[i].value;
                }
                else {
                    oRet[formArray[i].name] += "," + formArray[i].value;
                }
            }
        }
        return oRet;
    }
});

String.prototype.FormatDate = function () {
    var temp = "";
    if (this != null && this != undefined && this != "") {
        var datestring = this.split("(")[1].split(")")[0];
        var date = new Date(parseFloat( datestring));
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var year = date.getFullYear();
        temp = year + "-";
        if (month <10) {
            temp = temp + "0" + month+"-";
        } else {
            temp = temp  + month+"-";
        }

        if (day < 10) {
            temp = temp + "0" + day;
        } else {
            temp = temp + day;
        }
    }
    return temp;
}

String.prototype.FormatMinutsDate = function () {
    var temp = "";
    if (this != null && this != undefined && this != "") {
        var datestring = this.split("(")[1].split(")")[0];
        var date = new Date(parseFloat(datestring));
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var year = date.getFullYear();
        temp = year + "-";
        if (month < 10) {
            temp = temp + "0" + month + "-";
        } else {
            temp = temp + month + "-";
        }

        if (day < 10) {
            temp = temp + "0" + day;
        } else {
            temp = temp + day;
        }


        var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var second = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        temp = temp + " " + hour + ":" + minute + ":" + second;
    }
    return temp;
}

com.common = {
    AlertText:function(){
        alert("you have no permission to edit this item");
    },
    ShowChart: function (el, chartobj) {
        Highcharts.chart(el, chartobj);
    },
    formatStatus: function (value, row, index) {
        var div = "<div style='width:30px;height:30px;background-color:";
        if (value == "Uploaded") {
            div = div + "blue";
        } else if (value == "Reviewed") {
            div = div + "Green";
        } else if (value == "Issued") {
            div = div + "red";
        }
        return div + "'></div>";
    },
    Init3dPie:function(optoins,serises){
       var piechart= {
            chart: {
                    type: 'pie',
                    options3d: {
                    enabled: false,
                    alpha: 45,
                    beta: 0
                    }
            },
            title: optoins.title,
            tooltip: optoins.tooltip,
            plotOptions: {
                pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                        enabled: false,
                        format: '{point.name}'
                        }
                }
            },
            series: serises,
            credits: {
                enabled: false
            }
       };
       return piechart;
    },
    LogOff:function(){
        $.ajax({
            url: '../../Permission/Account/LogOff',
            type: 'post',
            
            dataType: 'json',
            success: function (data) {
                alert(data.Message);
                location.href = "../../Permission/Account/index";
            },
            error: function (data) {

            }
        });
    },
    Combox:function(el,options){
        $('#' + el).combobox(options); 
    }
  ,
    GetEditItem: function (el) {
        var users = $('#' + el).datagrid('getChecked');
        if (users.length > 1) {
            alert("please choose only one item for editing");
            return null;
        } else if (users.length == 0) {
            alert("please choose one item for editing");
            return null;
        }
        return users[0];
    }
    ,
    formatdate: function (value, row, index) {
        if(value!=undefined&&value!=null)
            return value.FormatDate();
        else {
            return value;
        }
    },
    formatminutdate: function (value, row, index) {
        if (value != undefined && value != null)
            return value.FormatMinutsDate();
        else {
            return value;
        }
    },
    
    openDialog: function (el, title, funsave, funCancel, width,showbuttons,height) {
        if (width == undefined || width == null) {
            width = 400;
        }
        if (showbuttons== undefined || showbuttons== null) {
            showbuttons = true;
        }
        if (height == undefined || height == null) {
            height = 'auto';
        } 
    var options=    {
            title: title,
            modal: true,
            width: width,
            height: height
    }
    if (showbuttons) {
        options.buttons = [
                {
                    text: 'OK',
                    iconCls: 'icon-ok',
                    handler: funsave
                },
            {
                text: 'Cancel',
                iconCls: 'icon-cancel',
                handler: funCancel
            }
        ];
    }
    
    $("#" + el).dialog(options);
    }
    ,
    closeDialog: function (el) {
        $("#" + el).dialog('close');
    }
,
    showFunCategory: function (categroy) {
        var list = $(".panel-body .accordion-body");
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if ($(item).attr('id') == categroy) {
                $(".easyui-accordion").accordion('select', i);
                break;
            }
        }
    }
 ,

    activeFunction: function (funid) {
        $("#" + funid).addClass('menu-active');
    }
 ,

    initGrid: function (el, title, ico, url, addcallback, editcallback, deletecallback) {
        $('#' + el).datagrid({
            title: title,
            iconCls: ico,//图标 
            width: 'auto',
            height: 'auto',
            nowrap: false,
            striped: true,
            border: true,
            collapsible: false,//是否可折叠的 
            url: url,
            method: 'post',
            //sortName: 'code', 
            //sortOrder: 'desc', 
            remoteSort: false,
            idField: 'Id',
            singleSelect: false,//是否单选 
            pagination: true,//分页控件 
            rownumbers: true,//行号 
            frozenColumns: [[
                { field: 'ck', checkbox: true }
            ]],
            toolbar: [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: addcallback
            }, '-', {
                text: 'Edit',
                iconCls: 'icon-edit',
                handler: editcallback
            }, '-', {
                text: 'Remove',
                iconCls: 'icon-remove',
                handler: deletecallback
            }],
        });
        //设置分页控件 
        var p = $('#list_data').datagrid('getPager');
        $(p).pagination({
            pageSize: 10,//每页显示的记录条数，默认为10 
            pageList: [5, 10, 15]//可以设置每页记录条数的列表 
        });

    },


    initCommonGrid: function (el, title, ico, url,  tooltip) {
        $('#' + el).datagrid({
            title: title,
            iconCls: ico,//图标 
            width: 'auto',
            height: 'auto',
            nowrap: false,
            striped: true,
            border: true,
            collapsible: false,//是否可折叠的 
            url: url,
            method: 'post',
             
            remoteSort: false,
            idField: 'Id',
            singleSelect: false,//是否单选 
            pagination: true,//分页控件 
            rownumbers: true,//行号 
            frozenColumns: [[
                { field: 'ck', checkbox: true }
            ]],
            toolbar: tooltip,
        });
        //设置分页控件 
        var p = $('#list_data').datagrid('getPager');
        $(p).pagination({
            pageSize: 10,//每页显示的记录条数，默认为10 
            pageList: [5, 10, 15]//可以设置每页记录条数的列表 


        });

    },

    initqueryGrid: function (el, title, ico, url, query,tooltip) {
        $('#' + el).datagrid({
            title: title,
            iconCls: ico,//图标 
            width: 'auto',
            height: 'auto',
            nowrap: false,
            striped: true,
            border: true,
            collapsible: false,//是否可折叠的 
            url: url,
            method: 'post',
            queryParams:query,
            //sortName: 'code', 
            //sortOrder: 'desc', 
            remoteSort: false,
            idField: 'Id',
            singleSelect: false,//是否单选 
            pagination: true,//分页控件 
            rownumbers: true,//行号 
            frozenColumns: [[
                { field: 'ck', checkbox: true }
            ]],
            toolbar: tooltip,
        });
        //设置分页控件 
        var p = $('#list_data').datagrid('getPager');
        $(p).pagination({
            pageSize: 10,//每页显示的记录条数，默认为10 
            pageList: [5, 10, 15]//可以设置每页记录条数的列表 


        });

    }
}
