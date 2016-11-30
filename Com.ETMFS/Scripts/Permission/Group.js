/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />
/// <reference path="../Common.js" />

com.GroupController = function(){
    var self = this;
    self.ChangeUsers = [];
    self.Load=function () {
        $('document').ready(function () {
            com.common.activeFunction("fun_groups");
            com.common.initGrid('list_data', 'Group List', 'icon-edit', '../Group/GroupList', self.Add, self.Edit, self.Remove);

         var tooltip=   [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: self.AddUser
            } , '-', {
                text: 'Remove',
                iconCls: 'icon-remove',
                handler: self.RemoveUser
            }]
         com.common.initqueryGrid('user_datas', 'Group User', 'icon-edit', '../Group/GroupUserList', { id: 0 }, tooltip);
         com.common.initqueryGrid('searchuser_datas', 'User List', 'icon-search', '../Account/UserList', { id: 0 }, []);
        });
    }
   self. Add= function () {
       com.common.openDialog('add_dialog', 'Add', self.Save, self.Cancel);
       $('#user_datas').datagrid({ queryParams: { id: 0 } });
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
       $("#groupform").form('load', users[0]);
       com.common.openDialog('add_dialog', 'Edit', self.Save, self.Cancel);
       $('#user_datas').datagrid({ queryParams: { id: users[0].Id } });
     
    }
   self. Cancel= function () {
        com.common.closeDialog('add_dialog');
    }
   self. Remove= function () {
       var users = $('#list_data').datagrid('getChecked');
       $.ajax({
           url: "../Group/RemoveGroups",
           type: 'post',
           datatype: 'json',
           data: { group: JSON.stringify(users) },
           success: function (data) {
               if (data.result) {
                   alert("Group Removed");
                   $("#list_data").datagrid("clearChecked");
                   $("#list_data").datagrid("reload");
                  
                   
               } else {
                   alert(data.message);
               }

           },
           error: function (data) {

           }
       })
    }
   self.Save = function () {
       var id = 0;
       if ($("#Id").val() != null&& $("#Id").val() != "") {
           id = parseInt($("#Id").val());
       }
       var group = {
           GroupName: $("#GroupName").val(),
           Description: $("#Description").val(),
           GUsers: self.ChangeUsers,
           Id: id,
       }
       $.ajax({
           url: "../Group/SaveGroup",
           type: 'post',
           datatype:'json',
           data: { group: JSON.stringify(group) },
           success: function (data) {
               if (data.result) {
                   alert("Group Saved");
                   $("#groupform").form('clear');
                   $("#list_data").datagrid("clearChecked");
                   $("#list_data").datagrid("reload");
                 com.common.closeDialog('add_dialog');
               } else {
                   alert(data.message);
               }
         
           },
           error: function (data) {

           }
       })
   }
   self.EditUser = function () {

   }

   self.RemoveUser = function () {
       var users = $('#user_datas').datagrid('getChecked');
       $(users).each(function (i, item) {
         var index = $("#user_datas").datagrid('getRowIndex', item);
         $("#user_datas").datagrid('deleteRow', index);
         if (self.ChangeUsers.indexOf(item) > -1) {
             self.ChangeUsers[self.ChangeUsers.indexOf(item)].OPStatus = 3;
         } else {
             item.OPStatus = 3;
             self.ChangeUsers.push(item);
         }

       });
       $("#user_datas").datagrid("clearChecked");
   }
   self.SaveUser = function () {
       var users = $('#searchuser_datas').datagrid('getChecked');
       if (users.length <= 0) {
           
           return;
       }
       $(users).each(function (i, item) {
           item.OPStatus = 1;
           $("#user_datas").datagrid('insertRow', {
               index: 0,
               row: item
           });
           self.ChangeUsers.push(item);
       });
       $("#searchuser_datas").datagrid("clearChecked");
       com.common.closeDialog('search_dialog');

      
   }
   self.CancelUser = function () {
       com.common.closeDialog('search_dialog');
   }
   self.AddUser = function () {
       com.common.openDialog('search_dialog', 'User Search', self.SaveUser, self.CancelUser);
       var users = $('#searchuser_datas').datagrid('reload');
   }
   return self;
};