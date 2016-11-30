/// <reference path="../jquery.min.js" />
/// <reference path="../knockout-3.4.0.js" />
/// <reference path="../Common.js" />

com.AccountController = function () {
    var self = {}
    self.login = function () {
        $.ajax({
            url: '../Account/Login',
            type: 'post',
            data: { logindata: JSON.stringify(this.User) },
            dataType: 'json',
            success: function (data) {
                alert(data.Message);
                location.href = "../../home/";
            },
            error: function (data) {

            }
        });
    }
    self.LoadUsers = function () {
        $('document').ready(function () {
            var controller = com.AccountController();
            com.common.showFunCategory("account");
            com.common.activeFunction("fun_users");
            controller.initGrid();
        });
    }
    self.initGrid = function () {
        $('#list_data').datagrid({
            title: 'User List',
            iconCls: 'icon-edit',//图标 
            width: 'auto',
            height: 'auto',
            nowrap: false,
            striped: true,
            border: true,
            collapsible: false,//是否可折叠的 
            
            url: '../Account/UserList',
            method:'post',
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
                handler: function () {
                    var model = com.UserViewModel();
                  
                    $("#userform").form('load', model);
                    com.common.openDialog('add_dialog', 'Add', com.AccountController().Save, function () { com.common.closeDialog('add_dialog') });
                }
            }, '-', {
                text: 'Edit',
                iconCls: 'icon-edit',
                handler: function () {
                    var users = $('#list_data').datagrid('getChecked');
                    if (users.length > 1) {
                        alert("please choose only one item for editing");
                        return;
                    } else if (users.length == 0) {
                        alert("please choose one item for editing");
                        return;
                    }
                    $("#userform").form('load', users[0]);
                    com.common.openDialog('add_dialog', 'Edit', com.AccountController().Save, function () { com.common.closeDialog('add_dialog') });

                    $('#grouplist_data').datagrid({ queryParams: { id: users[0].Id } });

                }
            }, '-', {
                text: 'Remove',
                iconCls: 'icon-remove',
                handler: function () {
                    com.AccountController().deleteUsers();
                }
            }],
        });
        //设置分页控件 
        var p = $('#list_data').datagrid('getPager');
        $(p).pagination({
            pageSize: 10,//每页显示的记录条数，默认为10 
            pageList: [5, 10, 15]//可以设置每页记录条数的列表 
             
            
        });
        com.common.initqueryGrid('grouplist_data', 'User Group', 'icon-search', '../Account/GetGroupsByUserId', { id: 0 }, []);
     
    }
    self.User = { UserName: null, Password: null }
    self.init = function () {
        $('document').ready(function () {
            $("#login").click(function () {
                var account = com.AccountController();
                account.User.UserName = $("#Username").val();
                account.User.Password = $("#Password").val();
                if (account.User.UserName == null || account.User.Password == null) {
                    alert("Username or password is null");
                    return;
                }
                account.login();
            });
        });
    }

    self.Save = function () {
            $('#userform').form('submit', {
                url: "../account/SaveUser",
                success: function (data) {
                  var data1= JSON.parse(data);
                  if (data1.result) {
                        alert("User information saved");
                        $('#list_data').datagrid('reload');
                        $('#userform').form('clear');
                        com.common.closeDialog('add_dialog');
                    } else {
                      alert("User information saved fail ");
                    }
                 
                }
            });
        }

    self.deleteUsers = function () {
        var users = $('#list_data').datagrid('getChecked');
     
        if (users.length > 0 && confirm("Users will be removed from user list do you confirm them?")) {
            $.ajax({
                url: '../Account/RemoveUsers',
                type: 'post',
                data: { users: JSON.stringify(users) },
                dataType: 'json',
                success: function (data) {
                    if (data.result) {
                        alert("Users are removed ");
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
    return self;
}

com.UserViewModel = function () {
    var self = {};
    self.UserName = '';
    self.Id = 0;
    self.PassWord = '';
    self.Country = '';
    self.CompanyId = 0;
    self.IsMainContact = true;
       
      return self;
}