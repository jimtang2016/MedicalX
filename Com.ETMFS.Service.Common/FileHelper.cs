using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Net;


namespace Com.ETMFS.Service.Common
{
  public  class FileHelper
    {
      public void UploadtoFileSystem(string path, HttpPostedFileBase file)
      {
          file.SaveAs(path);
      }
      public byte [] DownloadfromFileSystem(string path)
      {
          byte [] bytes = null;
          using (var filstream = new FileStream(path, FileMode.Open))
          {
                bytes = new byte[(int)filstream.Length];
                 filstream.Read(bytes, 0, bytes.Length);
          }
          return bytes;
      } 

      public void UploadtoSharePoint(string path, ConfigSetting config, HttpPostedFileBase file )
      {
          using (ClientContext client = new ClientContext(config.PathURI))
          {
              client.Credentials = new NetworkCredential(config.UserId, config.Password, config.Domain);
              var folderpath =path.Substring(0, path.LastIndexOf('/')+1);
              Folder folder = client.Web.GetFolderByServerRelativeUrl(folderpath);
              byte [] bytes=new Byte[file.ContentLength];
              file.InputStream.Read(bytes,0,file.ContentLength-1);
              client.Load(folder);
              client.ExecuteQuery();
              FileCreationInformation fileinfo = new FileCreationInformation()
              {
                  Content = bytes,
                  Overwrite = true,
                  Url = folder.ServerRelativeUrl + path.Substring(path.LastIndexOf('/') , path.Length - path.LastIndexOf('/') )
              };
              var sfile = folder.Files.Add(fileinfo);
              client.ExecuteQuery();
          
          }
      }
      public byte[] DownloadWebServerFile(string path, ConfigSetting config)
      {
           byte [] folder=null;
          using (WebClient client = new WebClient())
          {
              try
              {
                  client.Credentials = new NetworkCredential(config.UserId, config.Password, config.Domain);
                  folder = client.DownloadData(path);
              }
              catch (Exception ex)
              {
                  throw ex;
              }
            
          }
          return folder;
      }

      public void MappingFileFolder(List<string> paths)
      {
          paths.ForEach(path =>
          {
              
              DirectoryInfo folderpath = new DirectoryInfo(path);
              if (!folderpath.Exists)
              {
                  folderpath.Create();
              }
          });
      }

      
      public void MappingSharePointFolder(List<string> paths, ConfigSetting appconfig)
      {
          using (ClientContext client = new ClientContext(appconfig.PathURI))
          {
              
              paths.ForEach(path =>
              {
                
                  client.Credentials = new NetworkCredential(appconfig.UserId, appconfig.Password, appconfig.Domain);
               
                
                  Folder folder = client.Web.GetFolderByServerRelativeUrl( path);
                  client.Load(folder);
                  try
                  {
                      client.ExecuteQuery();
                  }
                  catch (Exception ex)
                  {
                     var leng= path.LastIndexOf('/')+1;
                   
                     var parenfolderurl = path.Substring(0, leng);
                     var currentfolderurl = path.Substring(leng-1, path.Length - leng+1);
                      Folder folders = client.Web.GetFolderByServerRelativeUrl(parenfolderurl);
                      client.Load(folders);
                      client.ExecuteQuery();
                     var nfolder= folders.Folders.Add(folders.ServerRelativeUrl + currentfolderurl);
                     client.ExecuteQuery();
                      
                  }
                
              });
             
          }

      }
    }
}
