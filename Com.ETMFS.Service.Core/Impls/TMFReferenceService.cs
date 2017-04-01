using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Impls.Core;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.Service.Core.Impls
{
   public class TMFReferenceService : ITMFReferenceService
    {
       ITMFTemplateRepository _tmtcontext;
       IUnitOfWork _unitwork;
       IStudyRepository _studycontext;
       public TMFReferenceService(ITMFTemplateRepository tmtcontext, IUnitOfWork unitwork,IStudyRepository studycontext)
       {
           _tmtcontext = tmtcontext;
           _unitwork = unitwork;
           _studycontext = studycontext;
       }
        
       public List<TmfNote> GetTMFModelList(int p, TMFFilter condition)
       {
           List<StudyTemplateViewModel> tmfs = null;
        
           if (condition.Study != null)
           {
               if (condition.Study.HasValue)
               {

                   var studylist = _studycontext.GetUserStudyList(p).FirstOrDefault(from => from.Id == condition.Study);
                   var tmfsquery = from tmf in _studycontext.GetSutdyTemplates( studylist,condition)
                       select ConvertStudyTemplates( tmf);
                   tmfs = tmfsquery.ToList();
               }
           }
           else
           {
              var tmfsquery = from tmf in _tmtcontext.GetAll()
                      where tmf.Active.HasValue && tmf.Active.Value
                        select ConvertStudyTemplates( tmf);;
               tmfs = tmfsquery.ToList();
           }
           return GetNodeList(tmfs);
       }

       List<TmfNote> GetNodeList(List<StudyTemplateViewModel> tmfs)
       {
            var templist = new List<TmfNote>();
            tmfs.GroupBy(f => f.ZoneNo).ToList().ForEach(t =>
           {

               var temp = t.FirstOrDefault();
               var filter = new TMFFilter()
               {
                   ZoneNo = temp.ZoneNo,
                   ZoneName = temp.ZoneName
               };
               var item = new TmfNote() { id = temp.ZoneNo, text = temp.ZoneName, category = filter };


               t.GroupBy(ts => ts.SectionNo).ToList().ForEach(ts =>
               {
                   var temps = ts.FirstOrDefault();
                   var filters = new TMFFilter()
                   {
                       ZoneNo = temp.ZoneNo,
                       SectionNo = temps.SectionNo,
                       SectionName = temps.SectionName
                   };

                   var items = new TmfNote() { id = filters.ZoneNo + filters.SectionNo, text = temps.SectionName, category = filters };
                   ts.ToList().ForEach(tmf =>
                   {
                       var tmfilter = new TMFFilter()
                       {
                           ZoneNo = filters.ZoneNo,
                           SectionNo = filters.SectionNo,
                           SectionName = filters.SectionName,
                           ArticleNo = tmf.ArtifactNo,
                           ArticleName = tmf.ArtifactName,
                           TMFId = tmf.RTMId,
                           StudyTemplateId=tmf.StudyTemplateId
                       };

                       var itemts = new TmfNote() { id = tmfilter.ZoneNo + tmfilter.SectionNo + tmfilter.ArticleNo, text = tmf.ArtifactName, category = tmfilter };
                       items.children.Add(itemts);
                   });
                 
                   item.children.Add(items);
               });

               templist.Add(item);
           });
           

      
       return templist;
       }

       StudyTemplateViewModel ConvertStudyTemplates(TMFTemplate temp)
       {
           var studyveiw = new StudyTemplateViewModel()
           {
                RTMId=temp.Id,
               ZoneNo = temp.ZoneNo,

               ZoneName = temp.ZoneName,

               SectionNo = temp.SectionNo,

               SectionName = temp.SectionName,

               ArtifactNo = temp.ArtifactNo,

               ArtifactName = temp.ArtifactName
                
           };
           return studyveiw;
       }

       StudyTemplateViewModel ConvertStudyTemplates(StudyTemplate temp)
       {
           var studyveiw = new StudyTemplateViewModel()
           {
               StudyTemplateId = temp.Id,
               ZoneNo = temp.TMFTemplate.ZoneNo,

               ZoneName = temp.TMFTemplate.ZoneName,

               SectionNo = temp.TMFTemplate.SectionNo,

               SectionName = temp.TMFTemplate.SectionName,

               ArtifactNo = temp.TMFTemplate.ArtifactNo,

               ArtifactName = temp.TMFTemplate.ArtifactName,
               RTMId = temp.TMFTemplate.Id
               
           };
           return studyveiw;
       }

       public PageResult<TMFRefernceViewModel> GetTMFModelList(int id, int page, int rows)
       {
           var pagein = new PageResult<TMFTemplate>()
           {
               PageSize = rows,
               CurrentPage = page
           };
           pagein = _tmtcontext.GetTMTList(id, pagein);
           var pageout = new PageResult<TMFRefernceViewModel>()
           {
               PageSize = rows,
               CurrentPage = page,
               Total = pagein.Total
           };
           pageout.ResultRows = Common<TMFTemplate, TMFRefernceViewModel>.ConvertToViewModel(pagein.ResultRows);
           return pageout;
       }
       #region ITMFReferenceService Members
       public void SaveTMFReferences(DataTable datatable,string operatorId){
           try { 
           for (var i = 1; i < datatable.Rows.Count;i++ )
           {
               var row = datatable.Rows[i] as DataRow;
               if (row != null)
               {
                   var temmodel = GetTmfModel(row, operatorId);

                   if (temmodel.Id == 0)
                   {
                       temmodel.CreateBy = operatorId;
                       temmodel.Created = DateTime.Now;
                       _tmtcontext.Add(temmodel);
                   }

               }
               _unitwork.Commit();
           }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       TMFTemplate GetTmfModel(DataRow rows,string op)
       {
           var tem = _tmtcontext.GetByUniqueID(rows[11].ToString());
           if(tem==null)
             tem = new TMFTemplate();
           if (rows[0] != null)
           tem.ZoneNo = rows[0].ToString();
           if (rows[1] != null)
           tem.ZoneName = rows[1].ToString();
           if (rows[2] != null)
           tem.SectionNo = rows[2].ToString();
           if (rows[3] != null)
           tem.SectionName = rows[3].ToString();
           if (rows[4] != null)
           tem.ArtifactNo = rows[4].ToString();
           if (rows[5] != null)
           tem.ArtifactName = rows[5].ToString();
           if (rows[6] != null)
          tem.AlternateNames = rows[6].ToString();
           if (rows[7] != null)
           tem.Purpose = rows[7].ToString();
           if (rows[8] != null)
           tem.Inclusion = rows[8].ToString();
           if (rows[9] != null)
           tem.ICHCode = rows[9].ToString();
          if (rows[10] != null)
           tem.SubArtifacts = rows[10].ToString();
           
           if (rows[11] != null)
               tem.EDMName = rows[11].ToString();
           if (rows[12] != null)
               tem.UniqueID = rows[12].ToString();
           if (rows[13] != null)
               tem.DeviceSponsorRequired = rows[13].ToString();
           if (rows[14] != null)
               tem.DeviceInvestRequired = rows[14].ToString();
           if (rows[15] != null)
           tem.NonDeviceSponsorRequired = rows[15].ToString();
           if (rows[16] != null)
               tem.NonDeviceInvestRequired = rows[16].ToString();
           if (rows[17] != null)
               tem.InvestigatorInitated = rows[17].ToString();
           if (rows[18] != null)
               tem.IsBaseMetaData = rows[18].ToString();
           if (rows[19] != null)
               tem.ProcessNumber = rows[19].ToString();
           if (rows[20] != null)
               tem.ProcessName = rows[20].ToString();
           if (rows[21] != null)
               tem.IsTrialLevel = rows[21].ToString();
           if (rows[22]!=null)
               tem.IsCountryLevel = rows[22].ToString();
           if (rows[22] != null)
               tem.IsSiteLevel = rows[22].ToString();
           tem.Modified = DateTime.Now;
           tem.ModifiBy = op;
           tem.Active = true;
           return tem;
       }
       public  PageResult< TMFRefernceViewModel> GetTMFModelList(int page, int rows)
       {
           var pagein = new PageResult<TMFTemplate>()
           {
               PageSize=rows,
               CurrentPage=page
           };
           pagein = _tmtcontext.GetTMTList(pagein);
           var pageout = new PageResult<TMFRefernceViewModel>()
           {
               PageSize = rows,
               CurrentPage = page,
               Total=pagein.Total
           };
           pageout.ResultRows = Common<TMFTemplate,TMFRefernceViewModel >.ConvertToViewModel(pagein.ResultRows);
           return pageout;
       }
      public void SaveTMFReference(TMFRefernceViewModel template, string operatorId)
       {
           var tem = _tmtcontext.GetById(template.Id);
           if (tem == null)
           {
               tem = new TMFTemplate()
               {
                   CreateBy = operatorId,
                   Created = DateTime.Now,
                   Active = true
               };
           }
           tem.ZoneNo = template.ZoneNo;

           tem.ZoneName = template.ZoneName;

           tem.SectionNo = template.SectionNo;
           tem.SubArtifacts = template.SubArtifacts;
           tem.SectionName = template.SectionName;

           tem.ArtifactNo = template.ArtifactNo;

           tem.ArtifactName = template.ArtifactName;

           tem.AlternateNames = template.AlternateNames;
           tem.Purpose = template.Purpose;

           tem.Inclusion = template.Inclusion;

           tem.ICHCode = template.ICHCode;

           tem.EDMName = template.EDMName;

           tem.UniqueID = template.UniqueID;

           tem.DeviceInvestRequired = template.DeviceInvestRequired;

           tem.DeviceSponsorRequired = template.DeviceSponsorRequired;

           tem.NonDeviceInvestRequired = template.NonDeviceInvestRequired;

           tem.NonDeviceSponsorRequired = template.NonDeviceSponsorRequired;

           tem.IsBaseMetaData = template.IsBaseMetaData;

           tem.ProcessNumber = template.ProcessNumber;

           tem.ProcessName = template.ProcessName;

           tem.IsTrialLevel = template.IsTrialLevel;

           tem.IsCountryLevel = template.IsCountryLevel;

           tem.IsSiteLevel = template.IsSiteLevel;


           tem.Modified = DateTime.Now;

           tem.ModifiBy = operatorId;
           if (tem.Id == 0)
           {
               _tmtcontext.Add(tem);
           }
           _unitwork.Commit();
       }
      public byte[] GetAllTemplateStream()
      {
          StringBuilder sbhtml = new StringBuilder();
          MemoryStream stream = new MemoryStream();
          var alltmfs = _tmtcontext.GetAll().Where(fg=>fg.Active.HasValue&&fg.Active.Value).ToList();
          var linend = "\r\n";
          var formats = "\"{0}\",";
          var allproperty = typeof(TMFRefernceViewModel).GetProperties().Where(f=>f.Name!="Id");
          foreach(var item in allproperty ){
              sbhtml.Append(string.Format(formats, item.Name));
          }
        var header=   "\uFEFF" + sbhtml.ToString().Substring(0, sbhtml.Length - 1) +linend;
        var buffer = Encoding.UTF8.GetBytes(header);
        stream.Write(buffer, 0, buffer.Length);
        alltmfs.ToList().ForEach(fg =>
        {
            var sproperties = fg.GetType().GetProperties();
            var colline =new  StringBuilder();
            allproperty.ToList().ForEach(fp =>
            {
              var sf=  sproperties.FirstOrDefault(f => f.Name == fp.Name);
              if (sf != null)
              {
                  var values = sf.GetValue(fg, null);
                  if (values != null)
                  {
                      if (values.GetType().Name.ToLower() == "datetime")
                      {
                          colline.Append(string.Format(formats, ((DateTime)values).ToString("yyyy-MM-dd")));
                      }
                      else
                      {
                          colline.Append(string.Format(formats, values.ToString()));
                      }

                  }
                  else
                  {
                      colline.Append(string.Format(formats, string.Empty));
                  }

              }
             
            });
            var buffers = Encoding.UTF8.GetBytes(colline.ToString().Substring(0,colline.Length-1)+linend);
            stream.Write(buffers, 0, buffers.Length);
        });
        return stream.ToArray();
      }
      public void DeleteTMFReference(List<TMFRefernceViewModel> templates, string operatorId)
       {
           templates.ForEach(f =>
           {
               var tem = _tmtcontext.GetById(f.Id);
               if (tem != null)
               {
                   tem.Active = false;
                   tem.ModifiBy = operatorId;
                   tem.Modified = DateTime.Now;
               }
           });
           _unitwork.Commit();
       }

       #endregion
    }
}
