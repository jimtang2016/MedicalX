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
using ICSharpCode.SharpZipLib.Zip;
namespace Com.ETMFS.Service.Common
{
    public class FileHelper
    {
        public bool ZipFile(string srcdir, string target)
        {
            if (string.IsNullOrEmpty(srcdir))
            {
                throw new Exception("folder is null ");
            }
            if (!Directory.Exists(srcdir))
            {
                throw new Exception("the path is not existed");
            }
            try
            {
                var filenames = GetFileNames(srcdir);
                var dirs = srcdir.Split('\\');
                var baseindex = srcdir.IndexOf(dirs[dirs.Length - 1]);
                using (FileStream basestream = new FileStream(target, FileMode.OpenOrCreate))
                {
                    using (ZipOutputStream stream = new ZipOutputStream(basestream))
                    {
                        stream.SetLevel(9);
                        byte[] buffer = new byte[4096];
                        foreach (var file in filenames)
                        {
                            ZipEntry zipfile = new ZipEntry(file.Substring(baseindex, file.Length - baseindex));
                            zipfile.DateTime = DateTime.Now;
                            stream.PutNextEntry(zipfile);
                            using (FileStream filestr = System.IO.File.OpenRead(file))
                            {
                                int sourceIndex=0;
                                do
                                {
                                   sourceIndex= filestr.Read(buffer, 0, buffer.Length);
                                    stream.Write(buffer, 0, buffer.Length);
                                } while (sourceIndex > 0);
                            }
                        }
                        stream.Finish();
                        stream.Close();
                    }
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool ZipFile(Dictionary<string, byte[]> list, string rootfolder,  string target,bool isWebServer=false)
        {
            
            try
            {
                char sourceFlag ,targetFlag;
                sourceFlag = targetFlag = '\\';
                if (isWebServer)
                {
                    sourceFlag = '/';
                }
                
                    using (FileStream basestream = new FileStream(target, FileMode.OpenOrCreate))
                    {
                        using (ZipOutputStream stream = new ZipOutputStream(basestream))
                        {
                            stream.SetLevel(9);


                            foreach (var file in list)
                            {
                                var srcdir = file.Key;
                                var dirs = srcdir.Split(sourceFlag);
                                var baseindex = srcdir.IndexOf(rootfolder);
                                ZipEntry zipfile = new ZipEntry(srcdir.Substring(baseindex, srcdir.Length - baseindex).Replace(sourceFlag, targetFlag));
                                zipfile.DateTime = DateTime.Now;
                                stream.PutNextEntry(zipfile);

                                stream.Write(file.Value, 0, file.Value.Length);
                            }
                           
                            stream.Finish();
                            stream.Close();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public List<string> GetFileNames(string srcdir )
        {
            List<string> templist = new List<string>();
            var folders = System.IO.Directory.GetDirectories(srcdir);
            templist.AddRange(System.IO.Directory.GetFiles(srcdir));
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    templist.AddRange(GetFileNames(folder));
                }
            }
            return templist;
            
        }
        public void UnZipFiles(string  src, string target)
        {
            var filesplit = "\\";
            if (string.IsNullOrEmpty(src))
            {
                throw new Exception("folder is null ");
            }
            if (Directory.Exists(src))
            {
                throw new Exception("the path is not existed");
            }
            if (string.IsNullOrEmpty(target))
            {
                target = src.Replace(Path.GetFileName(src), Path.GetFileNameWithoutExtension(src));
            }
            if (!target.EndsWith(filesplit))
            {
                target = target + filesplit;
            }
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            try
            {
                using (var file = new ZipInputStream(System.IO.File.OpenRead(src)))
                {
                    ZipEntry inputenty = null;
                    while ((inputenty = file.GetNextEntry())!=null)
                    {
                        string dirctname = Path.GetDirectoryName(inputenty.Name);
                        string filename = Path.GetFileName(inputenty.Name);
                        if (dirctname.Length > 0)
                        {
                            Directory.CreateDirectory(target + dirctname);
                            if (!dirctname.EndsWith(filesplit))
                            {
                                dirctname = dirctname + filesplit;
                            }
                            if (!string.IsNullOrEmpty(filename))
                            {
                                using (FileStream writer = System.IO.File.Create(target + inputenty.Name))
                                {
                                    int size = 4096;
                                    byte[] buffer = new byte[size];
                                    while(true){
                                        size = file.Read(buffer, 0, buffer.Length);
                                        if(size>0)
                                        writer.Write(buffer, 0, size);
                                        else 
                                            break;
                                    }
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
        public byte[] DownloadFile(string path, ConfigSetting config)
        {
            byte[] temp;
            if (!Directory.Exists(path)) return new byte[0];
            if (config.HostType == (int)HostType.SharePoint)
            {
                temp = DownloadWebServerFile(path, config);
            }
            else
            {
                temp = DownloadfromFileSystem(path);
            }
            return temp;
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
        public static ConfigSetting GetConfigSetting(HttpServerUtilityBase Server, ConfigSetting config)
        {
            if (config == null)
            {
                IdentityScope.Context.ConnectShareFolder(config);
                Constant.InitContentLib();
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
 
