using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Common
{
    public class IdentityScope : IDisposable
    {  
        private bool disposed;
        private static IdentityScope _context;
        private IdentityScope()
        {

        }

        public static IdentityScope Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new IdentityScope();
                }
                return _context;
            }
        }
        public void ConnectShareFolder(string path)
        {
            var config = XMLHelper.GetXMLEntity<ConfigSetting>(path);
        
            if (config!=null&&config.HostType == (int)HostType.ShareFolder)
            {
                Process proc = new Process();

                try
                {

                    proc.StartInfo.FileName = "cmd.exe";

                    proc.StartInfo.UseShellExecute = false;

                    proc.StartInfo.RedirectStandardInput = true;

                    proc.StartInfo.RedirectStandardOutput = true;

                    proc.StartInfo.RedirectStandardError = true;

                    proc.StartInfo.CreateNoWindow = true;

                    proc.Start();

                    string dosLine = @"net use " + config.PathURI + " /User:" + config.Domain + "\\" + config.UserId + " " + config.Password;
                    proc.StandardInput.WriteLine("net use * /d /y");
                    proc.StandardInput.WriteLine(dosLine);

                    proc.StandardInput.WriteLine("exit");

                    while (!proc.HasExited)
                    {

                        proc.WaitForExit(1000);

                    }

                    string errormsg = proc.StandardError.ReadToEnd();

                    proc.StandardError.Close();

                    if (!string.IsNullOrEmpty(errormsg))
                    {
                        throw new Exception(errormsg);
                    }

                }

                catch (Exception ex)
                {
                    throw ex;

                }

                finally
                {

                    proc.Close();

                    proc.Dispose();

                }

            }
           
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
              
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }   

}
