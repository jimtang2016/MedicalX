using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Net;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
namespace Com.ETMFS.Service.Common
{
    public class FileHelper
    {
        public void UploadtoFileSystem(string path, HttpPostedFileBase file)
        {
            file.SaveAs(path);
        }
        public byte[] DownloadfromFileSystem(string path)
        {
            byte[] bytes = null;
            using (var filstream = new FileStream(path, FileMode.Open))
            {
                bytes = new byte[(int)filstream.Length];
                filstream.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public void UploadtoSharePoint(string path, ConfigSetting config, HttpPostedFileBase file)
        {
            using (ClientContext client = new ClientContext(config.PathURI))
            {
                client.Credentials = new NetworkCredential(config.UserId, config.Password, config.Domain);
                var folderpath = path.Substring(0, path.LastIndexOf('/') + 1);
                Folder folder = client.Web.GetFolderByServerRelativeUrl(folderpath);
                byte[] bytes = new Byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength - 1);
                client.Load(folder);
                client.ExecuteQuery();
                FileCreationInformation fileinfo = new FileCreationInformation()
                {
                    Content = bytes,
                    Overwrite = true,
                    Url = folder.ServerRelativeUrl + path.Substring(path.LastIndexOf('/'), path.Length - path.LastIndexOf('/'))
                };
                var sfile = folder.Files.Add(fileinfo);
                client.ExecuteQuery();

            }
        }
        public byte[] DownloadWebServerFile(string path, ConfigSetting config)
        {
            byte[] folder = null;
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


                    Folder folder = client.Web.GetFolderByServerRelativeUrl(path);
                    client.Load(folder);
                    try
                    {
                        client.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        var leng = path.LastIndexOf('/') + 1;

                        var parenfolderurl = path.Substring(0, leng);
                        var currentfolderurl = path.Substring(leng - 1, path.Length - leng + 1);
                        Folder folders = client.Web.GetFolderByServerRelativeUrl(parenfolderurl);
                        client.Load(folders);
                        client.ExecuteQuery();
                        var nfolder = folders.Folders.Add(folders.ServerRelativeUrl + currentfolderurl);
                        client.ExecuteQuery();

                    }

                });

            }

        }

        public bool ConvertFromWord(string path, string savepath)
        {
          Word.ApplicationClass word = new Word.ApplicationClass();
             Type wordType = word.GetType();
            Word.Documents docs = word.Documents;
          Type docsType = docs.GetType();
          Word.Document doc = (Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
          Type docType = doc.GetType();
            try
            {
               
                string strSaveFileName = savepath;

                object saveFileName = (object)strSaveFileName;

                docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Word.WdSaveFormat.wdFormatPDF });
                    
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                
                doc.Close(false, Type.Missing, Type.Missing);
                wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }


        public bool ConvertFromExcel(string path, string savepath)
        {

            object missing = Type.Missing;
            Excel.ApplicationClass application = null;
            Excel.Workbook workBook = null;
            try
            {
                application = new Excel.ApplicationClass();
                object target = savepath;

                workBook = application.Workbooks.Open(path, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(false, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

        }
        public static ConfigSetting GetConfigSetting(HttpServerUtilityBase Server, HttpApplicationStateBase  Application)
        {
            var config = Application["ConfigSetting"] as ConfigSetting;
            if (config == null)
            {
                IdentityScope.Context.ConnectShareFolder(Server.MapPath(ConfigList.ConfigXMLPath));
                Constant.InitContentLib();
                config = XMLHelper.GetXMLEntity<ConfigSetting>(Server.MapPath(ConfigList.ConfigXMLPath));
                Application.Add("ConfigSetting", config);
            }
            return config;
        }
        public bool ConvertFromPPT(string path, string savepath)
        {
            Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
            string strSourceFile = path;
            string strDestinationFile = savepath;
            Microsoft.Office.Interop.PowerPoint.Presentation prsPres =
                       ppApp.Presentations.Open(strSourceFile, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            try
            {
                prsPres.SaveAs(strDestinationFile, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
               
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                prsPres.Close();
                ppApp.Quit();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }
    }

}
 
