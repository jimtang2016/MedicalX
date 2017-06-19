/// <reference path="../jquery.min.js" />
/// <reference path="../jquery.easyui.min.js" />
/// <reference path="../Common.js" />
com.MileStoneController = {

    init: function (el) {
        com.common.initEditGrid(el, "Mile Stone", "", "../Study/GetMileStones", [],[]);
    }
    ,
    show: function (el, studyId, id, milestoneType) {
       
        $("#" + el).datagrid({ queryParams: { studyId: studyId, id: id, stonetype: milestoneType } });
        milestoneType>0?  $("#StudySiteId").val(id): $("#StudyCountryId").val(id);
        $("#StudyId").val(studyId);
        $("#StoneType").val(milestoneType);
        com.common.openDialog("mileStone_dialog", milestoneType > 0 ? "Site Mile Stone" : "Country Mile Stone", function () {
            com.common.FinishEdit("mileStone_data");
            var datas = $("#" + el).datagrid("getData").rows;
            $(datas).each(function (i, item) {
                item.StudySiteId = $("#StudySiteId").val();
                item.StudyCountryId = $("#StudyCountryId").val();
            });
            var studyId = $("#StudyId").val();
            var stoneType = $("#StoneType").val();
            $.ajax({
                url: "../Study/SaveMileStones",
                method: "post",
                data: { studyId: studyId, stonetype: stoneType, datas: JSON.stringify(datas) },
                success: function (data) {
                    if (data.result) {
                        alert("Mile Stone Saved");
                        com.MileStoneController.clearDatas();
                    }
                    else {
                        alert(data.message);
                    }
                },
                error: function (error) {

                }
                
            })

        }, function () {
            com.MileStoneController.clearDatas();
        }, 800, true, 400);
       
    },
    clearDatas: function () {
        com.common.closeDialog("mileStone_dialog");
        $("#StudySiteId").val(null);
        $("#StudyCountryId").val(null);
        $("#StudyId").val(null);
        com.common.FinishEdit("mileStone_data");
        
    }
     

}