/// <reference path="../Common.js" />

com.CountryController = function ()
{
    var self = this;
    self. Load=function () {
        com.common.activeFunction("fun_country");
        com.common.showFunCategory("Masterdata");
        com.common.initGrid('list_data', 'Country List', 'icon-edit', '../Study/GetCountryList', self.Add, self.Edit, self.Remove);
    }

    self.Add = function () {
        $("#Id").val(0);
        com.common.openDialog("add_dialog", "Add", self.Save, self.Cancel)
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
        com.common.openDialog("add_dialog", "Edit", self.Save, self.Cancel)
    }
    self.Remove = function () {
        var users = $('#list_data').datagrid('getChecked');

        if (users.length > 0 && confirm("Users will be removed from country list do you confirm them?")) {
            $.ajax({
                url: '../Country/DeleteCountrys',
                type: 'post',
                data: { countries: JSON.stringify(users) },
                dataType: 'json',
                success: function (data) {
                    if (data.result) {
                        alert("Countries are removed ");
                        $("#list_data").datagrid("clearChecked");
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
    self.Save = function () {
        var countrycode = $("#CountryCode").val();
        var countryName = $("#CountryName").val();
        var id= $("#Id").val();
        $.ajax({
            url: "../Country/SaveCountry",
            data: { CountryName: countryName, Id: id, CountryCode: countrycode },
            type: 'post',
            dataType: 'json',
            success: function (data) {
            
                alert("Country Saved");
                $("#list_data").datagrid("clearChecked");
                $('#list_data').datagrid('reload');
                $('#userform').form('clear');
                com.common.closeDialog("add_dialog");
                 
            }
            ,
            error: function (data) {
                alert(data);
            }

        })

    }
    self.Cancel = function () {
        com.common.closeDialog("add_dialog");
    }
    return self;
}