using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Net.FtpClient;
using InpegFtpClientLib;

namespace FtpClientTest
{
    class Program
    {
        static InpegFtpClient client = new InpegFtpClient("192.168.1.145", 21, "tes", "tes", 3000);

        static void GetListing()
        {
            FtpListItem[] itemList = client.GetListing("./");

            foreach (FtpListItem item in itemList)
            {
                Console.WriteLine("[{0}] {1}", item.Type.ToString(), item.Name);
            }
        }

        static void Download()
        {
            client.DownloadFile("./BL/test.jpg", "aaa.jpg");
        }

        static void Upload()
        {
            client.UploadFile("./BL/aaa.jpg", "aaa.jpg");
        }

        static void Upload2()
        {
            string str = "this is upload test";
            byte[] buffer = Encoding.Default.GetBytes(str);
            client.UploadFile("aaa.txt", buffer);
        }

        static void Rename()
        {
            client.RenameFile("test.jpg", "aaa.jpg");
        }

        static void DeleteFile()
        {
            client.DeleteFile("aaa.txt");
        }

        static void Directory()
        {
            if (!client.IsExistDirectory("/AAA"))
            {
                client.CreateDirectory("/AAA");
            }
            client.DeleteDirectory("/AAA");
        }

        static void GetFileSize()
        {
            Console.WriteLine("file size : {0}", client.GetFileSize("/STPL/STPM201606010000"));
        }

        static void Main(string[] args)
        {
            //GetListing();
            //Download();
            //Upload();
            //Upload2();
            //Rename();
            //DeleteFile();
            //Directory();
            //GetFileSize();
        }
    }
}
