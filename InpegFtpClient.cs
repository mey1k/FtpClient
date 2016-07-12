using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using System.Net.FtpClient;

namespace InpegFtpClientLib
{
    public class InpegFtpClient
    {
        private string serverIP;
        private int serverPort;
        private string username;
        private string password;
        private int timeout = 5000;

        public InpegFtpClient(string ip, int port, string username, string password)
        {
            this.serverIP = ip;
            this.serverPort = port;
            this.username = username;
            this.password = password;
        }

        public InpegFtpClient(string ip, int port, string username, string password, int timeout)
        {
            this.serverIP = ip;
            this.serverPort = port;
            this.username = username;
            this.password = password;
            this.timeout = timeout;
        }

        private void SetConnection(FtpClient conn)
        {
            conn.Host = serverIP;
            conn.Port = serverPort;
            conn.Credentials = new NetworkCredential(username, password);
            conn.ConnectTimeout = timeout;
        }

        public FtpListItem[] GetListing(string path)
        {
            FtpListItem[] itemList = null;

            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    itemList = conn.GetListing(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return itemList;
        }

        public bool DownloadFile(string remoteFilepath, string localFilepath)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    using (Stream ostream = new FileStream(localFilepath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1400];
                        int len;
                        using (Stream istream = conn.OpenRead(remoteFilepath))
                        {
                            while ((len = istream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ostream.Write(buffer, 0, len);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool UploadFile(string remoteFilepath, string localFilepath)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);

                    using (Stream istream = new FileStream(localFilepath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[1400];
                        int len;
                        using (Stream ostream = conn.OpenWrite(remoteFilepath))
                        {
                            while ((len = istream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ostream.Write(buffer, 0, len);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool UploadFile(string remoteFilepath, byte[] sendData)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    using (Stream istream = new MemoryStream(sendData))
                    {
                        byte[] buffer = new byte[1400];
                        int len;
                        using (Stream ostream = conn.OpenWrite(remoteFilepath))
                        {
                            while ((len = istream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ostream.Write(buffer, 0, len);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool RenameFile(string srcFilepath, string dstFilepath)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    conn.Rename(srcFilepath, dstFilepath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool DeleteFile(string path)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    conn.DeleteFile(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool IsExistDirectory(string path)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    return conn.DirectoryExists(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }

        public void CreateDirectory(string path)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    conn.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void DeleteDirectory(string path)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    conn.DeleteDirectory(path, true, FtpListOption.AllFiles | FtpListOption.ForceList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public long GetFileSize(string path)
        {
            using (FtpClient conn = new FtpClient())
            {
                try
                {
                    SetConnection(conn);
                    return conn.GetFileSize(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return -1;
                }
            }
        }

        public FtpReply ExecuteCommand(string command, params object[] args)
        {
            using (FtpClient conn = new FtpClient())
            {
                SetConnection(conn);
                return conn.Execute(command, args);
            }
        }
    }
}
