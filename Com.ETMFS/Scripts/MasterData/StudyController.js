/// <reference path="../Common.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />

com.StudyController = function () {
    var self = {
        BackItem: {Study:null,Country:null,Site:null},
        CurrentItem:{Study:null,Country:null,Site:null},
        StudyLevel: "Study",
        CountryLevel: "Country",
        SiteLevel: "Site",
       
        InitAction: function (IsStudyLevel, EditGrid, IsDelete, callback) {
           
         
            if (IsStudyLevel==self.StudyLevel) {
                self.CurrentItem.Country = null;
                self.CurrentItem.Site = null;
                self.CurrentItem.Study = self.BackItem.Study;
            }  else 
            if (IsStudyLevel == self.CountryLevel) {
                self.CurrentItem.Site = null;
                self.CurrentItem.Country = self.BackItem.Country;
                 self.CurrentItem.Study = self.BackItem.Study;
            } else {
                self.CurrentItem.Country = self.BackItem.Country;
                self.CurrentItem.Site = self.BackItem.Site;
                self.CurrentItem.Study = self.BackItem.Study;
            }

            if (self.CurrentItem.Study == null) {
                alert("Please choose a trial");
                return;
            }
            if (EditGrid != null && EditGrid!=undefined) {
                var ets = $("#" + EditGrid).datagrid("getChecked");
                if (!self.validateSameOwner(ets) && IsDelete) {
                    alert("Selected list contain two or more owner please check");
                    return;
                }
                if (ets.length > 0) {
                    var et = ets[0];
                    if (EditGrid == "reginal_data") {
                       
                        self.CurrentItem.Country = et.CountryId;
                    } else if (EditGrid == "site_data") {
                       
                        self.CurrentItem.Site = et.Id;
                        self.CurrentItem.Country = et.CountryId;
                    }
                    else if (EditGrid == "member_data") {
                        self.CurrentItem.Country = et.CountryCode;
                        self.CurrentItem.Site = et.SiteId;
                    }
                }
                }
               
            

            $.ajax({
                url: "../../MasterData/Study/GetUserPermission",
                type: "post",
                data: { filter: self.CurrentItem },
                success: function (data) {
                    if (data.IsOwner) {
                        callback();
                    } else {
                        alert("User have no permission to do this operation");
                    }
             
                }

            });
        },
        validateSameOwner:function(ets){
            var temcountryId = 0;
            var hassamecountries = true;
            for (var i = 0; i < ets.length; i++) {
                temp = ets[i];
                if (i == 0) {
                    temcountryId = temp.OwnerId;
                } else if (temp.OwnerId != temcountryId) {
                    hassamecountries = false;
                }
            }
            return hassamecountries;
        },

        

        Load: function () {
            this.InitComboBox();
            this.InitStudyGrid();
            this.InitMemberGrid();
            this.InitReginalGrid();
            this.InitTemplatesGrid();
            this.InitSitesGrid();
        },
        InitComboBox: function () {
            var data = [
             {
                 id: "Active",
                 text: "Active"
             },
         {
             id: "Not Start",
             text: "Not Start"
         }
            ]
            var optoin = {
                data: data,
                valueField: 'id',
                textField: 'text'
            }
            com.common.Combox("Status", optoin);
            com.common.activeFunction("fun_study");
            com.common.showFunCategory("Studymanage");
        },
        InitStudyGrid: function () {
            var tooltip = [

                   {
                       text: 'Add',
                       iconCls: 'icon-add',
                       handler: this.Add
                   }, '-',

               {
                   text: 'Edit',
                   iconCls: 'icon-edit',
                   handler: this.Edit
               }, '-',
                 {
                     text: 'Remove',
                     iconCls: 'icon-remove',
                     handler: this.Remove
                 }, '-',
                  {
                      text: 'Trial Reginal',
                      iconCls: 'icon-edit',
                      handler: this.ManageReginals
                  }, '-',
                   {
                       text: 'Trial Member',
                       iconCls: 'icon-edit',
                       handler: this.ManageMembers
                   }
                   , '-',
                   {
                       text: 'TMF Model Reference',
                       iconCls: 'icon-edit',
                       handler: this.ManageReferences
                   },
                   '-',
                   {
                       text: 'TMF Site',
                       iconCls: 'icon-edit',
                       handler: this.ManageSite
                   }

            ];
            com.common.initCommonGrid("list_data", "Study List", 'icon-edit', '../Study/GetStudyList', tooltip);


        },
        InitReginalGrid: function () {
            com.common.initqueryGrid("reginal_data", "", '', '../Study/GetTrialRegionals', { id: 0 }, [

                   {
                       text: 'Add',
                       iconCls: 'icon-add',
                       handler: self.Reginal_Add
                   }
                ,
           {
               text: 'Edit',
               iconCls: 'icon-edit',
               handler: self.Reginal_Edit
           },
           {
               text: 'Remove',
               iconCls: 'icon-remove',
               handler: self.Reginal_Remove
           }

            ]);
        },
        InitTemplatesGrid: function () {
            com.common.initqueryGrid("temlate_data", "", '', '../Study/GetTrialTempaltes', { id: 0 },
                [
     {
         text: 'Add',
         iconCls: 'icon-add',
         handler: self.Template_Add
     }
                ,
                {
                    text: 'Remove',
                    iconCls: 'icon-remove',
                    handler: self.Template_Remove
                }

                ]
                );

            com.common.initqueryGrid("alltemlate_data", "Trial Template Reference", 'icon-edit', '../Study/GetAllTemplates', { id: 0 });
        },
        InitMemberGrid: function () {

            com.common.initqueryGrid("member_data", "", '', '../Study/GetMembers', { id: 0 }, [

                  {
                      text: 'Add',
                      iconCls: 'icon-add',
                      handler: self.Member_Add
                  }
               ,
          {
              text: 'Edit',
              iconCls: 'icon-edit',
              handler: self.Member_Edit
          },
          {
              text: 'Remove',
              iconCls: 'icon-remove',
              handler: self.Member_Remove
          }

            ]);

        }
        ,
        InitSitesGrid:function(){
            com.common.initqueryGrid("site_data", "", '', '../Study/GetStudySites', { id: 0 }, [

                 {
                     text: 'Add',
                     iconCls: 'icon-add',
                     handler: self.Site_Add
                 }
              ,
         {
             text: 'Edit',
             iconCls: 'icon-edit',
             handler: self.Site_Edit
         },
         {
             text: 'Remove',
             iconCls: 'icon-remove',
             handler: self.Site_Remove
         }

            ]);
        },
        Site_Remove: function () {
            self.InitAction(self.CountryLevel,"site_data",true,
            function () {
                var users = $('#site_data').datagrid('getChecked');
                if (users.length > 0 && confirm("Sites will be removed from Site list do you confirm them?")) {
                    $(users).each(function (i, item) {
                        item.Active = false;
                        item.StudyId = $("#Id").val();
                    });
                    $.ajax({
                        url: '../Study/DelStudySites',
                        type: 'post',
                        data: { sites: JSON.stringify(users) },
                        dataType: 'json',
                        success: function (data) {
                            if (data.result) {
                                alert("Sites are removed ");
                                $("#site_data").datagrid("clearChecked");
                                $('#site_data').datagrid('reload');
                                $('#member_data').datagrid('reload');
                            } else {
                                alert("There are some errors in this operation ");
                            }

                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            });
        },
        Site_Add: function () {
            var site = { Id: 0, Active: true, Status: "NotSelected", MemberId: 0 };
            self.LoadSite(site);
        },
        LoadSite:function(site){
            var statusoptoin = {
                url: "../Study/GetOptionListByParentId/" + 9,
                valueField: 'OptionValue',
                textField: 'ENText'
            }
            site.StudyId = $("#Id").val();
            self.ReginalOptoin.url=self.ReginalOptoin.url.substring(0, self.ReginalOptoin.url.lastIndexOf('/')+1) +site.StudyId;
            $("#site_CountryId").combobox(self.ReginalOptoin);
            $("#site_Status").combobox(statusoptoin);
            $("#site_OwnerId").combobox(self.UserOptoin);
            $("#siteform").form("load", site);
            com.common.openDialog("site_dialog", "Trial Site", self.Site_Save, self.Site_Cancel);
        },
        Site_Edit: function () {
            self.InitAction(self.CountryLevel, "site_data",false, function () {
                var site = com.common.GetEditItem("site_data");
                self.LoadSite(site);
            });
        },
        Site_Save: function () {
            $("#siteform").form("submit", {
                url: '../Study/SaveStudySite',
                method: 'post',
                success: function (data) {
                    var d = JSON.parse(data);
                    if (d.result) {
                        $('#site_data').datagrid("clearChecked");
                        $('#site_data').datagrid("reload");
                        $("#siteform").form("clear");
                        com.common.closeDialog("site_dialog");
                    }
                },
                error: function (data) {
                    var d = JSON.parse(data);
                    alert(d.Messsage)
                }
            });
        },
        Site_Cancel: function () {
            $("#siteform").form("clear");
            com.common.closeDialog("site_dialog", "Trial Site", self.Site_Save, self.Site_Cancel);
        },
        Template_Add: function () {
            self.InitAction(self.StudyLevel, null, false, function () {
                var studyId = $("#Id").val();
                com.common.openDialog("tmf_dialog", "Trial Model Reference", self.Template_Save, self.Template_Cancel, 800);
                $('#alltemlate_data').datagrid({ queryParams: { id: studyId } });
            });
          
        },
        Template_Remove: function () {
            self.InitAction(self.StudyLevel, "#temlate_data", true, function () {
                var templists = $("#temlate_data").datagrid("getChecked");
                self.Temlate_onSave(templists, true, []);
            });
        },
        Temlate_onSave: function (templists, isdel, country) {
            var id = $("#Id").val();
            $(templists).each(function (i, item) {
                item.Created = null;
                item.Modified = null;
            });
            if (templists.length > 0) {
                $.ajax({
                    url: '../Study/SaveStudyTemplates',
                    type: 'post',
                    data: { id: id, studytemps: JSON.stringify(templists), isdel: isdel, countrys: JSON.stringify(country) },
                    dataType: 'json',
                    success: function (data) {
                        if (data.result) {
                            if (isdel) {
                                alert("Templates are Deleted ");
                            } else {
                                alert("Templates are Saved ");
                            }

                            $("#alltemlate_data").datagrid("clearChecked");
                            $("#temlate_data").datagrid("clearChecked");
                            $('#temlate_data').datagrid('reload');
                            com.common.closeDialog("tmf_dialog");
                        } else {
                            alert("There are some errors in this operation ");
                        }

                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            }
        },
        Template_Save: function () {
            var templists = $('#alltemlate_data').datagrid('getChecked');
            self.Temlate_onSave(templists, false, []);

        },
        Template_Cancel: function () {
            com.common.closeDialog("tmf_dialog");
        },
          UserOptoin:{
            url: "../Study/GetUserList",
            valueField: 'Id',
            textField: 'UserName'
          },
            ReginalOptoin : {
              url: "../Study/GetTrialReginals/" ,
              valueField: 'CountryId',
              textField: 'CountryName'
          },
        Member_Load: function (reginal) {

            var roleoptoin = {
                url: "../Study/GetOptionListByParentId/" + 1,
                valueField: 'OptionValue',
                textField: 'ENText',

            }
            var roleleveloptoin = {
                url: "../Study/GetOptionListByParentId/" + 5,
                valueField: 'OptionValue',
                textField: 'ENText',
                onSelect: function (e) {
                    var level = e.OptionValue;
                    if (level == "Site") {
                        $("#mem_CountryId").combobox('enable');
                        $("#mem_SiteId").combobox('enable');
                    } else if(level=="Country"){
                        $("#mem_CountryId").combobox('enable');
                        $("#mem_SiteId").combobox('disable');
                        $("#mem_SiteId").combobox('setValue',null);
                    } else {
                        $("#mem_SiteId").combobox('disable');
                        $("#mem_CountryId").combobox('disable');
                        $("#mem_SiteId").combobox('setValue', null);
                        $("#mem_CountryId").combobox('setValue', null);
                    }
                }

            }
            reginal.StudyId = $("#Id").val();
            var teginalOptoin = {
                url: "../Study/GetTrialReginals/" + reginal.StudyId,
                valueField: 'CountryId',
                textField: 'CountryName',
                onSelect:
                    function (data) {
                        var studyId = $("#Id").val();
                        var siteoptoin = {
                            url: "../Study/GetTrialSites/" + studyId + "?countryId" + data.CountryId,
                            valueField: 'SiteId',
                            textField: 'SiteName'
                        }
                        com.common.Combox("mem_SiteId", siteoptoin);
                    }
            }
            com.common.Combox("mem_OwnerId", self.UserOptoin);
            com.common.Combox("mem_CountryId", teginalOptoin);
            
          


            com.common.Combox("mem_RoleLevel", roleleveloptoin);
            com.common.Combox("mem_Role", roleoptoin);
            
            com.common.openDialog("member_dialog", "Trial Member", self.Member_Save, self.Member_Cancel);
            $("#memberform").form("load", reginal);
        },
        Member_Add: function () {
                var reginal = { Id: 0, Active: true, StudyId: $("#Id").val() };
                self.Member_Load(reginal);
        },
        Member_Edit: function () {
            self.InitAction(self.SiteLevel, "member_data", false, function () {
                var users = com.common.GetEditItem("member_data");
                self.Member_Load(users);
            });
       
        },
        Member_Remove: function () {

            self.InitAction(self.SiteLevel, "member_data", true, function () {
                var users = $('#member_data').datagrid('getChecked');
                if (users.length > 0 && confirm("Studies will be removed from Member list do you confirm them?")) {
                    $(users).each(function (i, item) {
                        item.Active = false;
                    });
                    $.ajax({
                        url: '../Study/DeleteMembers',
                        type: 'post',
                        data: { mems: JSON.stringify(users) },
                        dataType: 'json',
                        success: function (data) {
                            if (data.result) {
                                alert("Members are removed ");
                                $("#member_data").datagrid("clearChecked");
                                $('#member_data').datagrid('reload');
                                $('#reginal_data').datagrid('reload');
                                $('#site_data').datagrid('reload');
                            } else {
                                alert("There are some errors in this operation ");
                            }

                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            });
            
        },
        Member_Save: function () {
            if ($("#memberform").form('validate')) {
                $("#memberform").form("submit", {
                    url: '../Study/SaveMember',
                    method: 'post',
                    success: function (data) {
                        var d = JSON.parse(data);
                        if (d.result) {
                            $('#member_data').datagrid("reload");
                            $("#memberform").form("clear");
                            $('#reginal_data').datagrid('reload');
                            $('#site_data').datagrid('reload');
                            com.common.closeDialog("member_dialog");
                        }
                    },
                    error: function (data) {
                        var d = JSON.parse(data);
                        alert(d.Messsage)
                    }
                });
            }

            
        },
        Member_Cancel: function () {
            $("#memberform").form("clear");
            com.common.closeDialog("member_dialog");
        },
        Reginal_Add: function () {
            self.InitAction(self.StudyLevel, null, false, function () {
                var reginal = { Id: 0, Active: true, MemberId: 0 };
                self.Reginal_Load(reginal);
            });
          
        },
        Reginal_Edit: function () {
            self.InitAction(self.CountryLevel, "reginal_data",false, function () {
                var reginal = com.common.GetEditItem("reginal_data");
                self.Reginal_Load(reginal);
            });
          
        },
        Reginal_Load: function (reginal) {
            var countyoptoin = {
                url: "../Study/GetCountryList",
                valueField: 'Id',
                textField: 'CountryName'
            }
            com.common.Combox("CountryId", countyoptoin);
            com.common.Combox("OwnerId", self.UserOptoin);
            reginal.StudyId = $("#Id").val();
            $("#reginalform").form("load", reginal);
            com.common.openDialog("reginal_dialog", "Trial Reginal", self.Reginal_Save, self.Reginal_Cancel);
        }
        ,

        Reginal_Save: function () {
            $("#reginalform").form("submit", {
                url: '../Study/SaveTrialRegionals',
                method: 'post',
                success: function (data) {
                    var d = JSON.parse(data);
                    if (d.result) {
                        $("#reginal_data").datagrid("clearChecked");
                        $('#reginal_data').datagrid("reload");
                        $("#reginalform").form("clear");
                        $('#member_data').datagrid("reload");
                        com.common.closeDialog("reginal_dialog");
                    }
                },
                error: function (data) {
                    var d = JSON.parse(data);
                    alert(d.Messsage);
                }
            });
        }
        ,
        Reginal_Cancel: function () {
            $("#reginalform").form("clear");
            com.common.closeDialog("reginal_dialog");
        }
        ,
        Reginal_Remove: function () {
            self.InitAction(self.StudyLevel, "reginal_data", false, function () {
                var users = $('#reginal_data').datagrid('getChecked');
                if (users.length > 0 && confirm("Reginal will be removed from Reginal list do you confirm them?")) {
                    $(users).each(function (i, item) {
                        item.Active = false;
                    });
                    $.ajax({
                        url: '../Study/DeleteTrialRegionals',
                        type: 'post',
                        data: { trialRegs: JSON.stringify(users) },
                        dataType: 'json',
                        success: function (data) {
                            if (data.result) {
                                alert("Reginal are removed ");
                                $("#reginal_data").datagrid("clearChecked");
                                $('#reginal_data').datagrid('reload');
                                $('#member_data').datagrid('reload');
                            } else {
                                alert("There are some errors in this operation ");
                            }

                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            });
        }
        ,
        Edit: function () {
            self.LoadStudy(false);
            self.SetActiveTab("studyTabs", "basicinfo");
        },
        LoadStudy: function (isnew) {

            var title = "Add";
            var user = { Id: 0, Active: true };
            if (!isnew) {
                user = com.common.GetEditItem("list_data");
                title = "Edit";
            }
         
            if (user != null) {
                com.common.openDialog("add_dialog", title, self.Save, self.Cancel, 1024);
                id = user.Id;
            }
 
            self.LoadStudyDatas(user);
            return user.Id;
        },
        LoadStudyDatas: function (user) {
          
            if(user.Id>0){
               
                $("#userform").form('load', user);
            }
            else {
                $("#userform").form('clear');
               
            }
          
            $('#reginal_data').datagrid({ queryParams: { id: user.Id } });
            $('#member_data').datagrid({ queryParams: { id: user.Id } });
            $('#temlate_data').datagrid({ queryParams: { id: user.Id } });
            $('#site_data').datagrid({ queryParams: { id: user.Id } });

        },
        Add: function () {
            self.LoadStudy(true);
        },
        Remove: function () {
            var users = $('#list_data').datagrid('getChecked');

            if (users.length > 0 && confirm("Studies will be removed from Study list do you confirm them?")) {
                $(users).each(function (i, item) {
                    item.Active = false;
                    item.Created = null;
                    item.Modified = null;
                });
                $.ajax({
                    url: '../Study/DeleteStudyList',
                    type: 'post',
                    data: { studyList: JSON.stringify(users) },
                    dataType: 'json',
                    success: function (data) {
                        if (data.result) {
                            alert("Studies are removed ");
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
        ,
        SetActiveTab: function (el, tabId) {
            var alltabs = $("#" + el).tabs("tabs");
            for (var i = 0; i < alltabs.length; i++) {
                var item = alltabs[i];
                if ($(item).attr("id") == tabId) {
                    var sindex = $("#" + el).tabs("getTabIndex", $("#" + el).tabs("getSelected"));
                    var index = $("#" + el).tabs("getTabIndex", item);
                    $("#" + el).tabs("unselect", sindex);
                    $("#" + el).tabs("select", index);
                    break;
                }
            }
            var edititem = com.common.GetEditItem("list_data");
            if(edititem!=null)
            self.BackItem.Study = com.common.GetEditItem("list_data").Id;
        }
        ,
        ManageSite: function () {
            var studyId = self.LoadStudy();
            self.SetActiveTab("studyTabs", "trialSite");
        }


            ,
        ManageReginals: function () {
            var studyId = self.LoadStudy();
            self.SetActiveTab("studyTabs", "trialregional");
        },
        ManageMembers: function () {
            var studyId = self.LoadStudy();
            self.SetActiveTab("studyTabs", "trialmember");

        },
        ManageReferences: function () {
            var studyId = self.LoadStudy();
            self.SetActiveTab("studyTabs", "trialTemplates");
        }
        ,
        Save: function () {
            var StudyNum = $("#StudyNum").val();
            var ShortTitle = $("#ShortTitle").val();
            var Status = $("#Status").combobox("getValue");
            var Active = $("#Active").val();
            var id = $("#Id").val();
            $.ajax({
                url: "../Study/SaveStudy",
                data: { study: JSON.stringify({ StudyNum: StudyNum, Id: id, ShortTitle: ShortTitle, Status: Status, Active: Active }) },
                type: 'post',
                dataType: 'json',
                success: function (data) {

                    alert("Study Saved");
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
        },
        Cancel: function () {
            com.common.closeDialog("add_dialog");
        }
    }
    return self;
}