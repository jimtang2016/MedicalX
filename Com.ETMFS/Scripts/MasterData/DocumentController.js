/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="StudyController.js" />


com.DocumentController = function () {

    var self = {}
    self.CurrentTMF = {};
    self.StudyService = com.StudyController();
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

        if (Site   == defaultvalue) {
            self.CurrentTMF.Site = null;
        }   
    }
    self.Permission = { IsOwner: false, IsUploader: false, IsReviewer: false };
    self.LoadDatas = function () {
        $("#tmflist").tree("reload");
        self.loadtreedocs();
        self.validatePermission();
        self.LoadCharts();
    }
    self.LoadCharts = function () {
        $.ajax(

            {
                url:"../../MasterData/StudyDocument/GetSumView", 
                data: { condition: self.CurrentTMF },
                type: 'POST',
                success:function (data) {
                    var serise1=[];
                    var se1={      type: 'pie',
                        data: []
                    }
                    var tooltip = {
                        pointFormat: '<b>{point.percentage:.1f}%</b>'
                    }
                    var datalabel={ enabled: true, format: '{point.name}:{y}' }
                    se1.data.push({ name: "Uploaded", color: "#eff5ff", y: data.UploadSum.Done, dataLabels: datalabel });
                    se1.data.push({ name: "Need Uploading", color: "#6b9cde", y: data.UploadSum.Total - data.UploadSum.Done, dataLabels: datalabel });
                    serise1.push(se1);
                    var chartobj1 = com.common.Init3dPie({
                        title: { text: "" }, tooltip: tooltip, enabled: true
                    }, serise1);
                    
                    com.common.ShowChart("chart_upload", chartobj1);

                    var serise2 = [];
                    var se2= {
                        type: 'pie',
                        name: 'Reviewed',
                        data: []
                    }
                    se2.data.push({ name: "Reviewed", color: "#eff5ff", y: data.ReviewSum.Done, dataLabels: datalabel });
                    se2.data.push({ name: "Need Review", color: "#6b9cde", y: data.ReviewSum.Total - data.ReviewSum.Done, dataLabels: datalabel });
                    serise2.push(se2);
                   
                    var chartobj = com.common.Init3dPie({
                        title: { text: "" }, tooltip: tooltip, enabled: true
                    }, serise2);
                 
                    com.common.ShowChart("chart_review", chartobj);

                },error: function (e) {

                    alert(e)
                }
            }
                );
  
    }
    self.loadtreedocs = function () {
        self.ClearCurrentTMF();
        if ($("#list_data").datagrid("getChecked").length>0) {
            $("#list_data").datagrid("clearChecked");
        }
        $("#list_data").datagrid({ queryParams: { condition: self.CurrentTMF } });
    }
    self.initCombobox = function () {

        var region = {
            url: "../Study/GetTrialReginals/",
            valueField: 'CountryId',
            textField: 'CountryName',
            onBeforeLoad: function (param) {
                param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
                param.countryId = self.CurrentTMF.Country == null ? 0 : self.CurrentTMF.Country;
            }
        };
        region.onSelect = function (data) {
            self.CurrentTMF.Country = data.CountryId;
            self.CurrentTMF.CountryName = data.CountryCode;
            self.CurrentTMF.Site = null;
            $("#SiteList").combobox("setValue", null);
            $("#SiteList").combobox("reload");
            self.LoadDatas();
        };
    
        $("#CountryList").combobox(region);

        var siteoption = {
            url: "../Study/GetTrialSites/"  ,
            valueField: 'Id',
            textField: 'SiteName',
            onBeforeLoad: function (param) {
                param.id = self.CurrentTMF.Study == null ? 0 : self.CurrentTMF.Study;
                param.countryId = self.CurrentTMF.Country == null ? 0 : self.CurrentTMF.Country;
            },
            onSelect: function (data) {
                self.CurrentTMF.Site = data.Id;
                self.CurrentTMF.SiteName = data.SiteName;
                self.LoadDatas();
            }
        }
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
                    self.CurrentTMF.Country = null;
                    self.CurrentTMF.Site = null;
                 
                    $("#CountryList").combobox("setValue", null);
                    $("#SiteList").combobox("setValue", null);
                    $("#CountryList").combobox("reload");
                    $("#SiteList").combobox("reload");
                    self.LoadDatas();
                }

            }

        };
        $("#StudyList").combobox(UserOptoin);
        
    }
    self.iniTreeList = function () {
        $("#tmflist").tree({
            url: "../../MasterData/TMFModel/GetUserTMFS",
            method: "post",
            onBeforeLoad: function (node, param) {
            
                param.condition = self.CurrentTMF;
            },
            onClick: function (node) {
                self.filter();
                self.FillCurrentTMF(node.category);
                self.loadtreedocs();
            } 
        });
    }
    self.validatePermission = function () {
        $.ajax({
            url: "../../MasterData/Study/GetUserPermission",
            type: "post",
            data: { filter: self.CurrentTMF },
            success: function (data) {
                self.Permission = data;
            }

        });
        self.StudyService.BackItem.Study = self.CurrentTMF.Study;
        self.StudyService.BackItem.Country = self.CurrentTMF.Country;
        self.StudyService.BackItem.Site = self.CurrentTMF.Site;
    }
    self.initDocumentList = function () {

        var tooltip = [
            {
                text: "Upload",
                iconCls: 'icon-add',
                handler: function () {
                    if (!self.Permission.IsOwner && !self.Permission.IsUploader) {
                        alert("you have no permission to do the opration");
                        return;
                    }

                    if (self.CurrentTMF.Study != null) {
                        var edititem;
                        var items = $("#list_data").datagrid("getChecked");
                        if (items.length > 0) {
                            edititem = items[items.length - 1]
                        }
                        if (edititem != null) {
                            self.CurrentTMF.DocumentId = edititem.Id;
                            self.CurrentTMF.DocumentName = edititem.DocumentName;
                            self.CurrentTMF.DocumentType = edititem.DocumentType;
                            self.CurrentTMF.VersionId = edititem.VersionId;
                            self.CurrentTMF.Comments = edititem.Comments;

                        }
                        if (self.CurrentTMF.DocumentId > 0) {
                            self.CurrentTMF.Operation = "Update";
                        } else {
                            self.CurrentTMF.Operation = "Create";

                        }
                        self.CurrentTMF.Active = true;
                        $("#groupform").form("load", self.CurrentTMF);
                        com.common.openDialog("add_dialog", "Upload Document", function () {
                            $("#groupform").form("submit", {
                                url: "../../MasterData/StudyDocument/UpateLoadDocument",
                                method: "post",
                                success: function (event) {
                                    var data = JSON.parse(event);
                                    if (data.result) {
                                        alert("document is Saved to the server");
                                        
                                        self.Closedialog();
                                    }
                                }
                            });
                        }, self.Closedialog, 400);
                    }

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
                        SiteId: item.SiteId,
                        ZoneNo: item.ZoneNo,
                        SectionNo: item.SectionNo,
                        ArticleNo: item.ArtifactNo,
                        VersionId: item.VersionId,
                        DocumentName: item.DocumentName,
                        DocumentType: item.DocumentType
                    }

                    window.open("../../MasterData/StudyDocument/DownloadFile?tmfilter=" + JSON.stringify(temffilter), "_blank");

                });
            }

        }

         ,
        {
            text: "Remove",
            iconCls: 'icon-remove',
            handler: function () {
                if (!self.Permission.IsOwner && !self.Permission.IsUploader) {
                    alert("you have no permission to do the opration");
                    return;
                }

                var edit = $("#list_data").datagrid("getChecked");
                if (edit != null) {
                    $(edit).each(function (i, item) {
                        item.Operation = "Delete";
                        item.Active = false;
                        item.Study = item.StudyId;
                        if (item.CountryId > 0)
                            item.Country = item.CountryId;
                        item.DocumentId = item.Id;
                        if (item.SiteId > 0)
                            item.Site = item.SiteId;
                        item.StudyTemplateId = item.StudyTemplateId;
                    });
                    $.ajax({
                        url: "../../MasterData/StudyDocument/SaveDocument",
                        method: "post",
                        data: { tmf: JSON.stringify(edit) },
                        success: function (data) {
                            if (data.result) {
                                alert("document is removed");
                            }
                        }
                    })
                }

            }

        }

          ,
        {
            text: "Review",
            iconCls: 'icon-ok',
            handler: function () {
                if (!self.Permission.IsOwner && !self.Permission.IsReviewer) {
                    alert("you have no permission to do the opration");
                    return;
                }

                var edit = $("#list_data").datagrid("getChecked");
                if (edit != null) {
                    $(edit).each(function(i,item){
                        item.Operation = "Review"
                        item.Active = true;
                        item.Study = item.StudyId;
                        if (item.CountryId>0)
                        item.Country = item.CountryId;
                        item.DocumentId = item.Id;
                        if(item.SiteId>0)
                        item.Site = item.SiteId;
                        item.StudyTemplateId = item.StudyTemplateId;
                    });
                    $.ajax({
                        url: "../../MasterData/StudyDocument/SaveDocument",
                        method: "post",
                        data: { tmf:JSON.stringify( edit) },
                        success: function (data) {
                            if (data.result) {
                                alert("document is reviewed");
                                $("#list_data").datagrid("reload");
                            }
                        }
                    })
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


        ];

        com.common.initCommonGrid("list_data", "Document List", 'icon-edit', "../../MasterData/StudyDocument/GetDocumentList", tooltip);

        $("#list_data").datagrid({
            onCheck: function (i, data) {

                self.selectNode(data);
            },
            onLoadSuccess: function () {
                //var datas = $("#list_data").datagrid("getData");
                //if (datas.total > 0) {
                //    self.selectNode(datas.rows[0]);
                //}
            },
            onUncheck: function (i, event) {
                var items = $("#list_data").datagrid("getChecked");

                if (items.length > 0) {
                    var data = items[items.length - 1];
                }
                if (data != undefined)
                    self.selectNode(data);
            }
        });
    }

    self.initDocumentHistoryList = function () {

        com.common.initqueryGrid("history_list_data", "History", "ico-edit", "../../MasterData/StudyDocument/GetDocumentHistory", { queryParams: { documentId: null } }, []);
      
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
      }
      self.Closedialog=   function () {
          $("#groupform").form("clear");
         
          self.loadtreedocs();
         com.common.closeDialog("add_dialog");
          
      }

      self.LoadDocuments=function(data){
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
          self.CurrentTMF.StudyTemplateId = data.StudyTemplateId;
      }

      self.ClearCurrentTMF = function ( ) {
          self.CurrentTMF.DocumentId  = null;
          self.CurrentTMF.DocumentName  = null;
          self.CurrentTMF.DocumentType  = null;
          self.CurrentTMF.VersionId = null;
          self.CurrentTMF.Comments  = null;
         
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