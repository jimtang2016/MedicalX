/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="StudyController.js" />


com.DocumentController = function () {

    var self = {}
    self.CurrentTMF = {};
    self.initmembers = function () {
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
                    $("#mems_SiteId").combobox('enable');
                } else if (level == "Country") {
                    $("#mem_CountryId").combobox('enable');
                    $("#mems_SiteId").combobox('disable');
                    $("#mems_SiteId").combobox('setValue', null);
                } else {
                    $("#mem_SiteId").combobox('disable');
                    $("#mem_CountryId").combobox('disable');
                    $("#mems_SiteId").combobox('setValue', null);
                    $("#mem_CountryId").combobox('setValue', null);
                }
            }

        }
         
        var teginalOptoin = {
            url: "../Study/GetTrialReginals/" + self.CurrentTMF.Study,
            valueField: 'CountryId',
            textField: 'CountryName',
            onSelect:
                function (data) {
                    self.CurrentTMF.Country = data.CountryId;
                    $("#mems_SiteId").combobox("reload");

                }
        }

        var siteoptoinstr = {
            url: "../Study/GetTrialSites/",
            valueField: 'Id',
            textField: 'SiteName',
            onBeforeLoad: function (param) {
                var studyId = self.CurrentTMF.Study;
                param.id = studyId == "" || studyId ==null? 0 : studyId;
                param.countryId = self.CurrentTMF.Country == "" || self.CurrentTMF.Country == null ? 0 : self.CurrentTMF.Country;

            },
            onSelect:
               function (data) {
                   var s = this;
               }
        }
        var   userOptoin= {
            url: "../Study/GetUserList",
            valueField: 'Id',
            textField: 'UserName'
        }
        com.common.Combox("mem_OwnerId", userOptoin);
        com.common.Combox("mem_CountryId", teginalOptoin);

        com.common.Combox("mems_SiteId", siteoptoinstr);
        com.common.Combox("mem_RoleLevel", roleleveloptoin);
        com.common.Combox("mem_Role", roleoptoin);
    }
    self.EditUserInfos = function (level) {
        var qaram = {};
        if (level == "Study" && self.Permission.IsOwner) {
            qaram.id = self.CurrentTMF.Study;
        } else if (level == "Country" && self.Permission.IsOwner) {
            qaram.id = self.CurrentTMF.Study;
            qaram.countryId = self.CurrentTMF.Country;
        } else if (level == "Site" && self.Permission.IsOwner) {
            qaram.id = self.CurrentTMF.Study;
            qaram.countryId = self.CurrentTMF.Country;
            if (self.CurrentTMF.Site==0)
                qaram.siteId = null;
            else
                qaram.siteId = self.CurrentTMF.Site;
        }
       
        if ( qaram.id >0) {
            com.common.openDialog("memberdialog", level + " Member List", function () {
                com.common.closeDialog("memberdialog");
            }, function () {
                com.common.closeDialog("memberdialog");
            }, 1200, false, 400);
            $("#member_list").datagrid({ queryParams: qaram });
            self.initmembers();
        } else {
            com.common.AlertText();
        }
        
    }
    self.loadMemebers = function (data) {
        $("#mem_RoleLevel").combobox({ readonly: true });
        $("#memberform").form("load", data);
        com.common.openDialog("member_dialog", "Member Edit", function () {
            if ($("#memberform").form('validate')) {
                $("#memberform").form("submit", {
                    url: '../Study/SaveMember',
                    method: 'post',
                    success: function (data) {
                        var d = JSON.parse(data);
                        if (d.result) {
                            $("#memberform").form("clear");
                            $("#member_list").datagrid("clearChecked");
                            $("#member_list").datagrid("reload");
                            com.common.closeDialog("member_dialog");
                        }
                    },
                    error: function (data) {
                        var d = JSON.parse(data);
                        alert(d.Messsage)
                    }
                });
            }
        },function(){
            $("#memberform").form("clear");
            $("#member_list").datagrid("clearChecked");
            $("#member_list").datagrid("reload");
            com.common.closeDialog("member_dialog");
            });
        }
    self.initUserDialog = function () {
        com.common.initqueryGrid("member_list", "", '', '../Study/GetMembers', { id: 0 }, [{
            text: "Add", iconCls: "icon-add", handler: function () {
                var data = { StudyId: self.CurrentTMF.Study, CountryCode: self.CurrentTMF.Country, SiteId: self.CurrentTMF.Site, Active: true,RoleLevel:"Study" };
                if (self.CurrentTMF.Site != null
                    && self.CurrentTMF.Site != undefined
                    && self.CurrentTMF.Site > 0) {
                    data.RoleLevel = "Site";
                } else if (self.CurrentTMF.Country != ""
                    && self.CurrentTMF.Country != null
                    && self.CurrentTMF.Country != undefined) {
                    data.RoleLevel = "Country";
                }
                
                self.loadMemebers(data);
            }
        }, {
            text: "Edit", iconCls: "icon-edit", handler: function () {
                var data = com.common.GetEditItem("member_list");
                if (data!=null) {
                    data.UserId = data.MemberId;
                    self.loadMemebers(data);
                }
                
            }
        }, {
            text: "Remove User", iconCls: "icon-remove", handler: function () {
                var users = $("#member_list").datagrid("getChecked");
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
                                $("#member_list").datagrid("clearChecked");
                                $('#member_list').datagrid('reload');
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
        }]);
    }
    self.RemoveReginals = function () {
        if (self.Permission.IsStudyOwner) {
            if (confirm("Will you remove this country?")) {
                $.post("../../MasterData/Study/GetTrialRegional", { id: self.CurrentTMF.Study, countryId: self.CurrentTMF.Country }, function (data) {
                    data.reginal.Active = false;
                    self.SaveReginals(data.reginal);
                });
            }
        } else {
            com.common.AlertText();
        }
    }
    self.RemoveSite = function () {
        if (self.Permission.IsCountryOwner || self.Permission.IsStudyOwner) {
            if (confirm("Will you remove this site?")) {
                $.post("../../MasterData/Study/GetTrialSite", { id: self.CurrentTMF.Study, countryId: self.CurrentTMF.Country, siteId: self.CurrentTMF.Site }, function (data) {
                    data.site.Active = false;
                    data.site.StudyId = self.CurrentTMF.Study;
                    self.SaveStudySite(data.site);
                });
            }
        } else {
            com.common.AlertText();
        }

    }
    self.SaveReginals = function (item) {
        $.ajax({
            url: "../../MasterData/Study/SaveTrialRegionals",
            method: "post",
            data: { trialReg: item },
            success: function (data) {
                if (data.result) {
                    alert("Country info is saved");
                    $("#reginalform").form("clear");
                    $("#studyTreeList").tree("reload");
                    com.common.closeDialog("reginal_dialog");
                } else {
                    alert(data.message);
                }
            },
            error: function () {

            }
        });
    }
    self.StudyService = com.StudyController();
    self.EditStudyInfo = function () {
        if (self.Permission.IsStudyOwner) {
            var datas = $("#StudyList").combobox("getData");
            var study = null;
            for (var i = 0; i < datas.length; i++) {
                var item = datas[i];
                if (item.Id == self.CurrentTMF.Study) {
                    study = item;
                    break;
                }
            }
            if (study != null) {
                $("#studyInfo").form("load", study);
                com.common.openDialog("stbasicinfo", "Basic Trial Infoamtion", function () {
                    var item = $("#studyInfo").form("getData");
                    $.ajax({
                        url: "../../MasterData/Study/SaveStudy",
                        method: "post",
                        data: { study: JSON.stringify(item) },
                        success: function (data) {
                            if (data.result) {
                                alert("study info is saved");
                                $("#studyInfo").form("clear");
                                $("#studyTreeList").tree("reload");
                                com.common.closeDialog("stbasicinfo");
                            } else {
                                alert(data.message);
                            }
                        },
                        error: function () {

                        }
                    });
                }, function () {
                    $("#studyInfo").form("clear");
                    com.common.closeDialog("stbasicinfo");
                }, 800);

            }
        } else {
            com.common.AlertText();
        }




    }
    self.EditCountryInfo = function () {
        if (self.Permission.IsCountryOwner || self.Permission.IsStudyOwner) {
            var datas = $("#CountryList").combobox("getData");
            var countyoptoin = {
                url: "../Study/GetCountryList",
                valueField: 'Id',
                textField: 'CountryName'
            }
            var ddoptoin = {
                url: "../Study/GetOptionListByParentId/" + 27,
                valueField: 'OptionValue',
                textField: 'ENText'
            }

            $.post("../../MasterData/Study/GetTrialRegional", { id: self.CurrentTMF.Study, countryId: self.CurrentTMF.Country }, function (data) {
                if (data.result) {
                    var country = data.reginal;
                    if (country == null || country == undefined) {
                        country = { Active: true, Id: 0 };
                    } else {
                        country.Active = true;
                    }
                    country.StudyId = self.CurrentTMF.Study;
                    $("#reginalform").form("load", country);
                    com.common.openDialog("reginal_dialog", "Trial Country Infoamtion", function () {
                        var item = $("#reginalform").form("getData");
                        self.SaveReginals(item);
                    }, function () {
                        $("#reginalform").form("clear");
                        com.common.closeDialog("reginal_dialog");
                    }, 800);

                } else {
                    alert(data.message);
                }
            });
            com.common.Combox("CountryId", countyoptoin);
            com.common.Combox("OwnerId", ddoptoin);



        } else {
            com.common.AlertText();
        }
    }
    self.SaveTMFS = function (templists, isdel, country) {
        $(templists).each(function (i, item) {
            item.Created = null;
            item.Modified = null;
        });
        if (templists.length > 0) {
            $.ajax({
                url: '../Study/SaveStudyTemplates',
                type: 'post',
                data: { id: self.CurrentTMF.Study, studytemps: JSON.stringify(templists), isdel: isdel, countrys: JSON.stringify(country) },
                dataType: 'json',
                success: function (data) {
                    if (data.result) {
                        if (isdel) {
                            alert("Templates are Deleted ");
                        } else {
                            alert("Templates are Saved ");
                        }

                        $('#tmflist').tree("reload");

                    } else {
                        alert(data.message);
                    }
                }

            });
        }
    }
    self.EditTMFModels = function (isCountryLevel) {
        if ((!isCountryLevel&&self.Permission.IsStudyOwner) || (isCountryLevel&&self.Permission.IsCountryOwner)) {
            com.common.openDialog("tmf_dialog", "Trial Model Reference", function () {
                var datas = $('#alltemlate_data').datagrid("getChecked");
                var country = [];
                if (isCountryLevel) {
                    country.push(self.CurrentTMF.Country);
                }
                self.SaveTMFS(datas, false, country);
                $("#alltemlate_data").datagrid("clearChecked");
                com.common.closeDialog("tmf_dialog");
            }, function () {
                $('#alltemlate_data').datagrid("clearChecked");
                com.common.closeDialog("tmf_dialog");
            }, 800);
            $('#alltemlate_data').datagrid({ queryParams: { id: self.CurrentTMF.Study, isCountryLevel: isCountryLevel, countryId: self.CurrentTMF.Country } });
        } else {
            com.common.AlertText();
        }

    }
    self.SaveStudySite = function (item) {
        $.ajax({
            url: "../../MasterData/Study/SaveStudySite",
            method: "post",
            data: { site: item },
            success: function (data) {
                if (data.result) {
                    alert("Site info is saved");
                    $("#siteform").form("clear");
                    $("#studyTreeList").tree("reload");
                    com.common.closeDialog("site_dialog");
                } else {
                    alert(data.message);
                }
            },
            error: function () {

            }
        });
    }
    self.RemoveTMFModels = function () {
        if (self.Permission.IsStudyOwner || self.Permission.IsCountryOwner) {
            if (self.CurrentTMF.Study > 0) {
                var item = {
                    Id: self.CurrentTMF.TMFId
                };
                var country = [];

                if (self.Permission.IsCountryOwner && !self.Permission.IsStudyOwner) {
                    country.push(self.CurrentTMF.Country);
                    self.SaveTMFS([item], true, country);
                } else {
                    $("#tmfcountryList").datagrid({ queryParams: { id: self.CurrentTMF.Study } });
                    com.common.openDialog("tmfcountryList_dialog", "", function () {
                        com.common.closeDialog("tmfcountryList_dialog");
                        var list = $("#tmfcountryList").datagrid("getChecked");
                        $(list).each(function (i, item) {
                            country.push(item.CountryId);
                        });
                        self.SaveTMFS([item], true, country);
                        $("#tmfcountryList").datagrid("clearChecked");
                    }, function () { com.common.closeDialog("tmfcountryList_dialog"); $("#tmfcountryList").datagrid("clearChecked"); })



                }


            } else {
                alert("please choose a trial");
            }
        } else {
            com.common.AlertText();
        }
    }
    self.EditSiteInfo = function () {
        if (self.Permission.IsOwner) {
            var statusoptoin = {
                url: "../Study/GetOptionListByParentId/" + 9,
                valueField: 'OptionValue',
                textField: 'ENText'
            }

            var region = {
                url: "../Study/GetTrialReginals/",
                valueField: 'CountryId',
                textField: 'CountryName',
                onBeforeLoad: function (param) {
                    param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
                }
            };
            $("#site_CountryId").combobox(region);
            $("#site_Status").combobox(statusoptoin);

            $.post("../../MasterData/Study/GetTrialSite", { id: self.CurrentTMF.Study, countryId: self.CurrentTMF.Country, siteId: self.CurrentTMF.Site }, function (data) {
                var site = data.site;
                if (site == null || site == undefined) {
                    site = { Active: true, Id: 0 };
                }
                site.StudyId = self.CurrentTMF.Study;
                site.CountryId = self.CurrentTMF.Country;
                $("#siteform").form("load", site);
                com.common.openDialog("site_dialog", "Trial Site Infoamtion", function () {
                    var item = $("#siteform").form("getData");
                    self.SaveStudySite(item);
                }, function () {
                    $("#siteform").form("clear");
                    com.common.closeDialog("site_dialog");
                }, 800);
            });
        } else {
            com.common.AlertText();
        }
       
    }
    self.filter = function () {
        var study = $("#StudyList").combobox("getValue");
        var defaultvalue = "";

        if (study == defaultvalue) {
            study = { Id: 0 }
            self.CurrentTMF.Study = null;
            self.StudyService.LoadStudyDatas(study);
        }
        var country = $("#CountryList").combobox("getValue");

        if (country == defaultvalue) {
            self.CurrentTMF.Country = null;
        }

        var Site = $("#SiteList").combobox("getValue");

        if (Site == defaultvalue) {
            self.CurrentTMF.Site = null;
        }
    }
    self.Permission = { IsOwner: false, IsUploader: false, IsReviewer: false, IsCountryOwner: false, IsSiteOwner: false, IsStudyOwner: false, IsAdministrator: false, IsSiteReviewer: false, IsStudyReviewer: false, IsCountryReviewer: false, IsStudyUploader: false, IsSiteUploader: false, IsCountryUploader: false }
    self.LoadDatas = function () {
        $("#tmflist").tree("reload");
        self.loadtreedocs();
        self.LoadCharts();
    }
    self.LoadCharts = function () {
        $.ajax(

            {
                url: "../../MasterData/StudyDocument/GetSumView",
                data: { condition: self.CurrentTMF },
                type: 'POST',
                success: function (data) {
                    var uperctent = data.UploadSum.Done / data.UploadSum.Total*100;
                    var rpercent = data.ReviewSum.Done / data.ReviewSum.Total * 100;
                    var ipercent = data.IssueSum.Done / data.IssueSum.Total * 100;
                    $("#chart_review").circliful({ title: "Reviewed", percent: rpercent, foregroundColor: "green" });
                    $("#chart_Issued").circliful({ title: "Issueded", percent: ipercent, foregroundColor: "red" });
                    $("#chart_upload").circliful({ title: "Uploaded", percent: uperctent, foregroundColor: "blue" });
                }, error: function (e) {

                    alert(e)
                }
            }
                );

    }
    self.loadtreedocs = function () {

        if ($("#list_data").datagrid("getChecked").length > 0) {
            $("#list_data").datagrid("clearChecked");
        }
        if (self.CurrentTMF.Site == 0) {
            self.CurrentTMF.Site = null;
        }
        if (self.CurrentTMF.Country == 0) {
            self.CurrentTMF.Country = null;
        }
        $("#list_data").datagrid({ queryParams: { condition: self.CurrentTMF } });
    }
    self.initCombobox = function () {

        var region = {
           
            valueField: 'CountryId',
            textField: 'CountryName',
          
        };

        var docrgion = {
            url: "../Study/GetTrialReginals/",
            valueField: 'CountryId',
            textField: 'CountryName',
            onBeforeLoad: function (param) {
                param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
            },
            onLoadSuccess: function () {
                if (self.CurrentTMF.Country!=null)
                $("#doc_country").combobox("setValue", "" + self.CurrentTMF.Country)
            }
        };

        docrgion.onSelect = function (data) {
            $("#doc_countryname").val(data.CountryCode);
            self.CurrentTMF.Country = data.CountryId;
            $("#doc_site").combobox("setValue", null);
            $("#doc_site").combobox("reload");

        };
        $("#doc_country").combobox(docrgion);
        region.onSelect = function (data) {
            self.CurrentTMF.Country = data.CountryId;
            self.CurrentTMF.CountryName = data.CountryCode;
            $.ajax({
                url: "../Study/GetTrialSites/",
                method: "post",
                data: { id: self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study,countryId :self.CurrentTMF.Country == 0 ? null : self.CurrentTMF.Country },
                success: function (data) {
                    var datas = [{ Id: 0, SiteName: "All" }]
                    $(data).each(function (i, item) {
                        datas.push(item);
                    });
                    $("#SiteList").combobox({ data: datas });
                }
            });
           
        };
        $("#CountryList").combobox(region);
        var siteoption = {
            valueField: 'Id',
            textField: 'SiteName',
            onSelect: function (data) {
                self.CurrentTMF.Site = data.Id;
                self.CurrentTMF.SiteName = data.SiteName;
                self.LoadDatas();
            }
        }


        var tmfeoption = {
            url: "../Study/GetAllTrialTempaltes/",
            valueField: 'Id',
            textField: 'ArtifactName',
            onBeforeLoad: function (param) {
                param.codition = self.CurrentTMF;
            },
            onSelect: function (data) {
                $("#doc_Zone").val(data.ZoneName);
                $("#doc_Section").val(data.SectionName);
                $("#doc_DocumentType").val(data.ArtifactNo + "-" + data.ArtifactName);
                $("#doc_ZoneNo").val(data.ZoneNo);
                $("#doc_ArticleNo").val(data.ArtifactNo);
                $("#doc_SectionNo").val(data.SectionNo);
                var leveldata = [];
                var trueflag = "X";
                var sitelevel = { id: "Site", text: "Site" };
                var countrylevel = { id: "Country", text: "Country" };
                var triallevel = { id: "Trial", text: "Trial" };
                if (data.IsCountryLevel == trueflag&&(self.Permission.IsCountryOwner||self.Permission.IsCountryUploader)) {
                    leveldata.push(countrylevel);
                    leveldata.push(sitelevel);
                }
                if (data.IsSiteLevel == trueflag && (self.Permission.IsSiteOwner || self.Permission.IsSiteUploader)) {
                    leveldata.push(sitelevel);
                }
                if (data.IsTrialLevel == trueflag && (self.Permission.IsStudyOwner || self.Permission.IsStudyUploader)) {
                    leveldata.push(triallevel);
                    leveldata.push(countrylevel);
                    leveldata.push(sitelevel);
                }

                var leveloption = {
                    data: leveldata,
                    valueField: 'id',
                    textField: 'text',
                    onSelect: function (data) {
                        if (data.id == "Trial") {
                            $("#doc_country").combobox({ disabled: true });
                            $("#doc_site").combobox({ disabled: true });
                        } else if (data.id == "Country") {
                            $("#doc_country").combobox({ disabled: false, required:true });
                          
                            $("#doc_site").combobox({ disabled: true });
                        } else if (data.id == "Site") {
                            $("#doc_country").combobox({ disabled: false, required: true });
                            $("#doc_site").combobox({ disabled: false, required: true });
                        }
                    }
                   
                }
                $("#doc_DocumentLevel").combobox(leveloption);
                $("#doc_DocumentLevel").combobox("setValue", self.CurrentTMF.DocumentLevel);
            }
        }
        $("#doc_TMF").combobox(tmfeoption);
        var doc_siteoption = {
            url: "../Study/GetTrialSites/",
            valueField: 'Id',
            textField: 'SiteName',
            onBeforeLoad: function (param) {
                param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
                param.countryId = self.CurrentTMF.Country == null ? 0 : self.CurrentTMF.Country;
            },
            onSelect: function (data) {
                $("#doc_SiteName").val(data.SiteName);
            },
            onLoadSuccess: function () {
                var datas = $("#doc_site").combobox("getData");
                for (var i = 0; i < datas.length; i++) {
                    var item = datas[i];
                    if (item.Id == self.CurrentTMF.Site) {
                        $("#doc_site").combobox("setValue", "" + self.CurrentTMF.Site);
                        break;
                    }
                }
        }
        }

        $("#doc_site").combobox(doc_siteoption);
        $("#SiteList").combobox(siteoption);

        var UserOptoin = {
            url: "../../MasterData/Study/GetUserStudyList",
            valueField: 'Id',
            textField: 'StudyNum',
            onSelect: function (data) {
                if (data != null) {
                    self.CurrentTMF = {};

                    self.CurrentTMF.Study = data.Id;
                    self.CurrentTMF.StudyNum = data.StudyNum;
                    self.StudyService.LoadStudyDatas(data);
                    $.ajax({
                        url: "../Study/GetTrialReginals/",
                        method: "post",
                        data: { id: self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study },
                        success: function (data) {
                            var datas = [{ CountryId: 0, CountryName: "All" }]
                            $(data).each(function (i, item) {
                                datas.push(item);
                            });
                            $("#CountryList").combobox({ data: datas });
                        }
                    });
                }

            }

        };
        $("#StudyList").combobox(UserOptoin);

    }
    self.SaveReveiwData = function (isaccept) {
        var item = com.common.GetEditItem("list_data");
        if (item != null) {
            item.Operation = "Review"
            item.Active = true;
            item.Study = item.StudyId;
            if (item.CountryId > 0)
                item.Country = item.CountryId;
            item.DocumentId = item.Id;
            if (item.SiteId > 0)
                item.Site = item.SiteId;
            if (!isaccept) {
                item.IssueLogViewModel = $("#IssueLogForm").form("getData", false);
                item.Operation = "Issued"
            }
            var items = [item]
            $.ajax({
                url: "../../MasterData/StudyDocument/SaveDocument",
                method: "post",
                data: { tmf: JSON.stringify(items) },
                success: function (data) {
                    if (data.result) {
                        if (isaccept) {
                            alert("document is reviewed");

                        } else {
                            alert("Issue is Saved");
                        }

                        $("#list_data").datagrid("reload");
                        com.common.closeDialog("reviewe_dialog");
                    } else {
                        alert(data.ex);
                    }
                }
            })
        }

    }

    self.FillDocData = function (isIssue) {
        var edititems = $("#list_data").datagrid("getChecked");
        var edititem = null;
        if (edititems.length > 0) {
            edititem = edititems[edititems.length - 1];
        }
        if (edititem != null) {
            if (!isIssue) {
                self.CurrentTMF.DocumentId = edititem.Id;
                self.CurrentTMF.DocumentName = edititem.DocumentName;
                self.CurrentTMF.DocumentType = edititem.DocumentType;
                self.CurrentTMF.VersionId = edititem.VersionId;
                self.CurrentTMF.Comments = edititem.Comments;
                self.CurrentTMF.DocumentLevel = edititem.DocumentLevel;
                self.CurrentTMF.ProtocolNumber = edititem.ProtocolNumber;
                self.CurrentTMF.Language = edititem.Language;
                self.CurrentTMF.IsCountryShared = edititem.IsCountryShared;
                self.CurrentTMF.IsSiteShared = edititem.IsSiteShared;
                self.CurrentTMF.SharedCountryIds = edititem.SharedCountryIds;
                self.CurrentTMF.DocumentDate = edititem.DocumentDate;
                self.CurrentTMF.SharedSiteIds = edititem.SharedSiteIds;
                self.CurrentTMF.StudyTemplateId = edititem.StudyTemplateId;
                self.CurrentTMF.ProtocolNumber = edititem.ProtocolNumber;
                self.CurrentTMF.TMFId = edititem.TMFId;
                self.CurrentTMF.Study = edititem.StudyId;
                self.CurrentTMF.Country = edititem.CountryId;
                self.CurrentTMF.Site = edititem.SiteId;
                if (self.CurrentTMF.DocumentId > 0) {
                    self.CurrentTMF.Operation = "Update";
                } else {
                    self.CurrentTMF.Operation = "Create";
                }
                self.CurrentTMF.Active = true;
            } else {
                self.CurrentTMF = {
                    StudyNum: edititem.StudyNum,
                    Study: edititem.StudyId,
                    CountryName: edititem.CountryName,
                    Country: edititem.CountryId,
                    SiteName: edititem.SiteName,
                    Site: edititem.SiteId,
                    ZoneNo: edititem.ZoneNo,
                    SectionNo: edititem.SectionNo,
                    ArticleNo: edititem.ArtifactNo,
                    VersionId: edititem.VersionId,
                    DocumentName: edititem.DocumentName,
                    DocumentType: edititem.DocumentType,
                    DocumentId : edititem.Id
                };
            }
        }
        if (self.CurrentTMF.DocumentId == null || self.CurrentTMF.DocumentId == undefined) {
            if (self.CurrentTMF.Site != null &&self.CurrentTMF.Site > 0) {
                self.CurrentTMF.DocumentLevel = "Site";
            } else if (self.CurrentTMF.Country != null || self.CurrentTMF.Country > 0) {
                self.CurrentTMF.DocumentLevel = "Country";
            } else {
                self.CurrentTMF.DocumentLevel = "Trial";
            }
        }
      
    }
    self.iniTreeList = function () {
        $("#tmflist").tree({
            url: "../../MasterData/TMFModel/GetUserTMFS",
            method: "post",
            onBeforeLoad: function (node, param) {

                param.condition = self.CurrentTMF;
            },
            onContextMenu: function (er, t) {
                er.preventDefault();
                var locate = {
                    left: er.pageX,
                    top: er.pageY
                }
                $("#tmfmenu").menu('show', locate);
            },
            onClick: function (node) {
            
                self.FillCurrentTMF(node.category);
                self.loadtreedocs();
            }
        });

        $("#studyTreeList").tree({
            url: "../../MasterData/Study/GetUserStudyListView",
            method: "post",
            onContextMenu: function (er, title) {
                er.preventDefault();
                var selected = $("#studyTreeList").tree("getSelected");

                var locate = {
                    left: er.pageX,
                    top: er.pageY
                }
                if (selected != null) {
                    var data = $("#studyTreeList").tree("getData", selected.target);
                    if (data.RoleLevel == "Study") {
                        $("#stmenu").menu('show', locate);
                    } else if (data.RoleLevel == "Country") {
                        $("#countrymenu").menu('show', locate);
                    } else if (data.RoleLevel == "Site") {
                        $("#sitemenu").menu('show', locate);
                    }
                }


            },
            onBeforeLoad: function (node, param) {
                param.condition = self.CurrentTMF;
            },
            onClick: function (node) {
                self.CurrentTMF = {
                    Study: node.category.Study,
                    Country: node.category.Country,
                    Site: node.category.Site,
                    StudyNum: node.category.StudyNum,
                }
                self.LoadDatas();
                self.validatePermission();
            }
        });
    }
    self.validatePermission = function (callback) {
        if (self.CurrentTMF.Study != null && self.CurrentTMF.Study != undefined) {
            $.ajax({
                url: "../../MasterData/Study/GetUserPermission",
                type: "post",
                data: { filter: self.CurrentTMF },
                success: function (data) {
                    self.Permission = data;
                    if (callback != null && callback != undefined) {
                        callback();
                    }
                }

            });
        } else {
            alert("please choose a trial or document item");
        }
      
    
    }
    self.initUploadDatas = function () {
        $("#doc_SharedCountryIds").combobox(
               {
                   url: "../Study/GetAllTrialReginals/",
                   valueField: 'CountryId',
                   textField: 'CountryName',
                   multiple: true,
                   onBeforeLoad: function (param) {
                       param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
                   },

                   onLoadSuccess: function () {
                       if (self.CurrentTMF.SharedCountryIds != null && self.CurrentTMF.SharedCountryIds != undefined)
                           $("#doc_SharedCountryIds").combobox("setValues", self.CurrentTMF.SharedCountryIds.split(','));
                   }
               }

           );

        $("#doc_SharedSiteIds").combobox(
          {
              url: "../Study/GetAllTrialSites/",
              valueField: 'Id',
              textField: 'SiteName',
              multiple: true,
              onBeforeLoad: function (param) {
                  param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
              },

              onLoadSuccess: function () {
                  if (self.CurrentTMF.SharedSiteIds != null && self.CurrentTMF.SharedSiteIds != undefined)
                      $("#doc_SharedSiteIds").combobox("setValues", self.CurrentTMF.SharedSiteIds.split(','));
              }
          }

      );
        $("#doc_IsShareSites").change(function () {
            if (this.checked) {
                $("#doc_SharedSiteIds").combobox({ disabled: false });
            } else {
                $("#doc_SharedSiteIds").combobox({ disabled: true });
            }
            $("#doc_hideIsShareSites").val(this.checked);
        });
        $("#doc_Search").click(function () {
            if (!doc_IsIssueLog.checked) return;
            var edit = com.common.GetEditItem("list_data");
            if (edit != null) {
                $("#issuelog_list_data").datagrid({ queryParams: { documentId: edit.Id, isAllIssues: false, status: 'Issued' } });
                com.common.openDialog("Issuelog_dialog", edit.ArtifactName, function () {
                    var items = $("#issuelog_list_data").datagrid("getChecked");
                    var lognums = "";
                    var logIds = "";
                    $(items).each(function (i, r) {
                        if (i == items.length - 1) {
                            logIds = logIds + r.Id;
                            lognums = lognums + r.LogNum;
                        } else {
                            logIds = logIds + r.Id + ",";
                            lognums = lognums + r.LogNum + ",";
                        }
                    });
                    $("#doc_IssueLogs").val(lognums);
                    $("#doc_IssueLogIds").val(logIds);
                    $("#issuelog_list_data").datagrid("clearChecked");
                    com.common.closeDialog("Issuelog_dialog");
                }, function () {
                    $("#issuelog_list_data").datagrid("clearChecked");
                    com.common.closeDialog("Issuelog_dialog");
                }, 1200);
            }
        });
        $("#doc_IsIssueLog").change(function () {
            if (this.checked) {
                $("#doc_Search").linkbutton({ disabled: false });
            } else {
                $("#doc_IssueLogs").val(null);
                $("#doc_IssueLogIds").val(null);
                $("#doc_Search").linkbutton({ disabled: true });
            }
            $("#doc_IshiddenIssueLog").val(this.checked);
        });

        $("#doc_IsShareCountry").change(function () {
            if (this.checked) {
                $("#doc_SharedCountryIds").combobox({ disabled: false });

            } else {
                $("#doc_SharedCountryIds").combobox({ disabled: true });
            }
            $("#doc_hideIsShareCountry").val(this.checked);
        });

        $("#doc_files").change(function (event) {
            var filepath = $(this).val();
            if (filepath == null || filepath == undefined) {
                return;
            }
            var filePaths = filepath.split('\\');
            var filenames = filePaths[filePaths.length - 1].split('.');
            var filename = "";
            for (var i = 0; i < filenames.length - 1; i++) {
                if (i < filenames.length - 2) {
                    filename = filename + filenames[i] + ".";
                } else {
                    filename = filename + filenames[i];
                }

            }
            $("#doc_Name").val(filename);

            $("#doc_Format").val(filenames[filenames.length - 1]);
        });
    }
    self.docUploadCallback=function(){
        if (!self.Permission.IsOwner && !self.Permission.IsUploader) {
            com.common.AlertText();
            return;
        }
        self.initUploadDatas();
        $("#doc_country").combobox("reload");
        $("#doc_TMF").combobox("reload");
        $("#groupform").form({
            onSubmit: function () {
                var items = $("#doc_SharedSiteIds").combobox("getValues");
                var strsites = "";
                for (var i = 0; i < items.length; i++) {
                    if (i < items.length - 1) {
                        strsites = strsites + items[i] + ",";
                    } else if (i == items.length - 1) {
                        strsites = strsites + items[i];
                    }
                }
                var citems = $("#doc_SharedCountryIds").combobox("getValues");
                var strcountry = "";
                for (var i = 0; i < citems.length; i++) {
                    if (i < citems.length - 1) {
                        strcountry = strcountry + citems[i] + ",";
                    } else if (i == citems.length - 1) {
                        strcountry = strcountry + citems[i];
                    }
                }

                $("#doc_SharedCountryNames").val($("#doc_SharedCountryIds").combobox("getText"));
                $("#doc_SharedSiteNames").val($("#doc_SharedSiteIds").combobox("getText"));
                $("#doc_hideSharedCountryIds").val(strcountry);
                $("#doc_hideSharedSiteIds").val(strsites);
                return true;
            }
        });
        $("#groupform").form("load", self.CurrentTMF);
        doc_IsShareSites.checked = Boolean(self.CurrentTMF.IsSiteShared);
        doc_IsShareCountry.checked = Boolean(self.CurrentTMF.IsCountryShared);
        var hascheck = self.CurrentTMF.HasIssue == null || self.CurrentTMF.HasIssue == undefined ? false: Boolean(self.CurrentTMF.HasIssue);
        doc_IsIssueLog.checked = hascheck;
        if (hascheck) {
            $("#doc_Search").linkbutton({ disabled: false });
        } else {
            $("#doc_Search").linkbutton({ disabled: true });
        }

        com.common.openDialog("add_dialog", "Upload Document", function () {
            $("#groupform").form("submit", {
                url: "../../MasterData/StudyDocument/UpateLoadDocument",
                method: "post",
                success: function (event) {
                    var data = JSON.parse(event);
                    if (data.result) {
                        alert("document is Saved to the server");
                        self.Closedialog();
                    } else {
                        alert(data.ex);
                    }
                }
            });
        }, self.Closedialog, 800);
        $("#add_dialog").dialog({ closable: false });
    }
 
    self.reviewCallback = function () {

        if (!self.Permission.IsOwner && !self.Permission.IsReviewer) {
            alert("you have no permission to do the opration");
            return;
        }
        $.ajax({
            url: '../../MasterData/StudyDocument/GetDocumentUser/',
            method: "post",
            data: { tmfilter: JSON.stringify(self.CurrentTMF) },
            success: function (data) {
                if (data) {
                    $("#issue_AssignUsers").combobox({
                        data: data,
                        valueField: 'Id',
                        textField: 'UserName',
                        multiple: true,
                        panelHeight: 'auto'
                    });
                }
            }
        });

        $("#issue_Reason").combobox({
            url: '../../MasterData/Study/GetOptionListByParentId/' + 18,
            valueField: 'OptionValue',
            textField: 'ENText',
            multiple: true,
            panelHeight: 'auto',
            onSelect: function (data) {
                if (data.OptionValue == "Others") {
                    $("#issue_Comment").removeAttr("readonly");
                } else {
                    $("#issue_Comment").attr("readonly", "readonly");
                }
            }
        });

        $("#issue_Status").combobox({
            url: '../../MasterData/Study/GetOptionListByParentId/' + 14,
            valueField: 'OptionValue',
            textField: 'ENText',

            panelHeight: 'auto'
        });

        com.common.openDialog("reviewe_dialog", "Quality Review", null
       , null, 1300, false, 600);
        $.ajax({
            url: "../../MasterData/StudyDocument/GetPDFFile",
            method: "post",
            data: { tmfilter: JSON.stringify(self.CurrentTMF) },
            success: function (data) {
                if (data.result) {
                    var doc = document.getElementById("documentfile");
                    doc.src = data.url;
                } else {
                    alert(data.message);
                }
            }
        });
        var cur = new Date();
        var data = { Status: "Issued", ReviewerId: Currentuser.UserId, ReviewName: Currentuser.UserName, ReviewDate: "" + cur.getFullYear() + "-" + (cur.getMonth() + 1) + "-" + cur.getDate() };
        $("#IssueLogForm").form("load", data);
    }
    self.initDocumentList = function () {

        var tooltip = [
            {
                text: "Upload",
                iconCls: 'icon-add',
                handler: function () {
                  
                        self.FillDocData(false);
                        self.validatePermission(
                             self.docUploadCallback);
                        
                }
            }
            ,
        {
            text: "Download",
            iconCls: 'icon-arow',
            handler: function () {
                var filelist = $("#list_data").datagrid("getChecked");
                $(filelist).each(function (i, item) {
                    var temffilter = {
                        StudyNum: item.StudyNum,
                        CountryName: item.CountryName,
                        Country: item.CountryId,
                        SiteName: item.SiteName,
                        Site: item.SiteId,
                        ZoneNo: item.ZoneNo,
                        SectionNo: item.SectionNo,
                        ArticleNo: item.ArtifactNo,
                        VersionId: item.VersionId,
                        DocumentName: item.DocumentName,
                        DocumentType: item.DocumentType
                    }
                    var title = "tmf_"+item.Id;
                    $("#download_form").attr("target", title);
                    $("#doc_download_file").val(JSON.stringify(temffilter));
                    window.open("about:blank", title, "")
                    $("#download_form").submit();
                   
                });
            }

        }

         ,
        {
            text: "Remove",
            iconCls: 'icon-remove',
            handler: function () {
                var edit = $("#list_data").datagrid("getChecked");
                if (edit != null) {
                    var study = 0;
                    var country = 0;
                    var site = 0;
                    var ispermission = true;
                    for (var i = 0; i < edit.length; i++) {
                        var item = edit[i];
                        item.Operation = "Delete";
                        item.Active = false;
                        item.Study = item.StudyId;
                        if (item.CountryId > 0)
                            item.Country = item.CountryId;
                        item.DocumentId = item.Id;
                        item.StudyTemplateId = item.StudyTemplateId;
                        if (item.SiteId > 0)
                            item.Site = item.SiteId;
                        if (i == 0) {
                            study = item.Study;
                            country = item.CountryId;
                            site = item.SiteId;
                        } else {
                            ispermission = item.Study == study && country == item.CountryId && site == item.SiteId;
                        }
                        if(!ispermission){
                            break;
                        }
                    }
                    self.CurrentTMF.Study = study;
                    self.CurrentTMF.Country = country;
                    self.CurrentTMF.Site = site;
                    if (self.CurrentTMF.Site == 0) {
                        self.CurrentTMF.Site = null;
                    }
                    if (ispermission) {
                        var callback = function () {
                            if (self.Permission.IsOwner || self.Permission.IsUploader) {
                                $.ajax({
                                    url: "../../MasterData/StudyDocument/SaveDocument",
                                    method: "post",
                                    data: { tmf: JSON.stringify(edit) },
                                    success: function (data) {
                                        if (data.result) {
                                            alert("document is removed");
                                            $("#list_data").datagrid("clearChecked");
                                            $("#list_data").datagrid("reload");
                                        }
                                    }
                                });
                            } else {
                                com.common.AlertText();
                                $("#list_data").datagrid("clearChecked");
                            }

                        }
                        self.validatePermission(callback);
                        
                    } else {
                        alert("make sure your selected items in the same trial,country,site");
                    }
                    
                }

            }

        }

          ,
        {
            text: "Review",
            iconCls: 'icon-ok',
            handler: function () {
                self.FillDocData(true);
                if (self.CurrentTMF.DocumentId != null && self.CurrentTMF.DocumentId > 0) {
                    self.validatePermission(self.reviewCallback);
                }
                
            }
        }
        ,
        {
            text: "History",
            iconCls: 'icon-search',
            handler: function () {
                var edit = com.common.GetEditItem("list_data");
                if (edit != null) {
                    $("#history_list_data").datagrid({ queryParams: { documentId: edit.Id } });
                    com.common.openDialog("history_dialog", edit.ArtifactName, function () {
                        com.common.closeDialog("history_dialog");
                    }, function () {
                        com.common.closeDialog("history_dialog");
                    }, 1200);
                }
            }

        }

         ,
        {
            text: "Issue Log",
            iconCls: 'icon-search',
            handler: function () {
                var edit = com.common.GetEditItem("list_data");
                if (edit != null) {
                    $("#issuelog_list_data").datagrid({ queryParams: { documentId: edit.Id, isAllIssues: true, status: '' } });
                    com.common.openDialog("Issuelog_dialog", edit.ArtifactName, function () {
                        $("#issuelog_list_data").datagrid("clearChecked");
                        com.common.closeDialog("Issuelog_dialog");
                    }, function () {
                        $("#issuelog_list_data").datagrid("clearChecked");
                        com.common.closeDialog("Issuelog_dialog");
                    }, 1200);
                }
            }

        }
        ];

        com.common.initCommonGrid("list_data", "Document List", 'icon-edit', "../../MasterData/StudyDocument/GetDocumentList", tooltip);

        $("#list_data").datagrid({
            onCheck: function (i, data) {

            
            },
            onLoadSuccess: function () {
               
            },
            onUncheck: function (i, event) {
            
            }
        });
    }

    self.initDocumentHistoryList = function () {

        com.common.initqueryGrid("history_list_data", "History", "ico-edit", "../../MasterData/StudyDocument/GetDocumentHistory", { queryParams: { documentId: null } }, []);

        com.common.initqueryGrid("issuelog_list_data", "Issue Log", "ico-edit", "../../MasterData/StudyDocument/GetIssuelogs", { queryParams: { documentId: null } }, []);

    }
    self.Load = function () {
        self.iniTreeList();

        self.initCombobox();

        self.initDocumentList();
        self.initDocumentHistoryList();
        self.StudyService.InitReginalGrid();
        self.StudyService.InitSitesGrid();
        self.StudyService.InitTemplatesGrid();
        self.StudyService.InitMemberGrid();
        self.StudyService.InitComboBox();
        self.initReviewForm();
        com.common.initqueryGrid("tmfcountryList", "Trial Country", 'icon-edit', '../Study/GetTrialRegionals', { id: 0 });
        com.common.initqueryGrid("alltemlate_data", "Trial Template Reference", 'icon-edit', '../Study/GetAllTemplates', { id: 0 });
        self.initUserDialog();
    }
    self.initReviewForm = function () {
        $("#btn_Review_doc").click(function () { self.SaveReveiwData(true); });
        $("#btn_reject_doc").click(function () { self.SaveReveiwData(false); });


    }
    self.Closedialog = function () {
        self.ClearCurrentTMF();
        self.loadtreedocs();

        $("#groupform").form("clear");
        com.common.closeDialog("add_dialog");
    }

    self.LoadDocuments = function (data) {
        var datas = $('#StudyList').combobox('getData');
        if (data.StudyId != self.CurrentTMF.Study ||
          data.CountryId > 0 && data.CountryId != self.CurrentTMF.Country
            || data.SiteId > 0 && data.SiteId != self.CurrentTMF.Site) {
            if (data.StudyId != self.CurrentTMF.Study) {
                self.CurrentTMF.Study = data.StudyId;
                for (var i = 0; i < datas.length; i++) {
                    var item = datas[i];
                    if (item.Id == data.StudyId) {
                        self.CurrentTMF.StudyNum = item.StudyNum;
                        self.StudyService.LoadStudyDatas(item);

                        break;
                    }
                }
            }
            if (data.CountryId > 0) {
                self.CurrentTMF.Country = data.CountryId;
                self.CurrentTMF.CountryName = data.CountryName;
            }

            if (data.SiteId > 0) {
                self.CurrentTMF.Site = data.SiteId;
                self.CurrentTMF.SiteName = data.SiteName;
            }

        }
        self.FillCurrentTMF(data);
        self.validatePermission();
    }
    self.FillCurrentTMF = function (data) {
        self.CurrentTMF.ZoneNo = data.ZoneNo;
        self.CurrentTMF.ArticleNo = data.ArtifactNo;
        self.CurrentTMF.ArticleName = data.ArtifactName;
        if (self.CurrentTMF.ArticleNo == undefined) {
            self.CurrentTMF.ArticleName = data.ArticleName;
            self.CurrentTMF.ArticleNo = data.ArticleNo;
        }
        self.CurrentTMF.SectionNo = data.SectionNo;
        self.CurrentTMF.TMFId = data.TMFId;
        self.CurrentTMF.StudyTemplateId = data.StudyTemplateId;
    }

    self.ClearCurrentTMF = function () {
        var newtmf = { Study: self.CurrentTMF.Study, Country: self.CurrentTMF.Country, Site: self.CurrentTMF.Site, StudyNum: self.CurrentTMF.StudyNum };
        self.CurrentTMF = newtmf;
       
    }
    self.selectNode = function (data) {
        self.LoadDocuments(data);
        var tempid = "";
        var i = 0;
        while (i < 3) {

            if (i == 0) {
                tempid = data.ZoneNo;
            } else if (i == 1) {
                tempid = tempid + data.SectionNo;
            }
            else if (i == 2) {
                tempid = tempid + data.ArtifactNo;
            }
            var node = $("#tmflist").tree("find", tempid);
            if (node != null) {
                $("#tmflist").tree("expand", node.target);
                if (i == 2) {
                    $("#tmflist").tree("select", node.target);
                }
            }

            i++;
        }



    }

    return self;
}