using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SnakeDeathmatch.Game;

namespace NewGameUI.Services.FTP
{
    public class FTPFileRepository
    {
        private string _ftpServerIP = "ftp.hostuju.cz/";
        private string _ftpUserName = "snake.hostuju.cz";
        private string _ftpPassword = "123snake123";

        public IEnumerable<string> ListFiles()
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpServerIP);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        return (reader.ReadToEnd()).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                }
            }
        }

        public SavedGame LoadSavedGame(string fileName)
        {

            var fileString = DownloadFileFromFTP(fileName);

            if (string.IsNullOrEmpty(fileString))
                return null;

            List<string> records = (fileString).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (records.Any())
            {
                int firstRow = Int32.Parse(records[0]);

                int playgroundSizeInDots = firstRow;

                var colorconverter = new ColorConverter();


                var recordLines = records.Skip(1)
                       .Select(x => x.Split(';'))
                       .Select(n => new RecordLine(Int32.Parse(n[0]), Int32.Parse(n[3]), Int32.Parse(n[4]), (Color)colorconverter.ConvertFromString(n[2]), n[1])).ToList();

                return new SavedGame() { PlayGroundSizeInDots = playgroundSizeInDots, RecordLines = recordLines };
            }

            return null;
        }

        private string DownloadFileFromFTP(string fileName)
        {

            using (WebClient request = new WebClient())
            {
                string url = "ftp://" + _ftpServerIP + fileName;
                request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
                request.Proxy = null;

                try
                {
                    byte[] newFileData = request.DownloadData(url);
                    return System.Text.Encoding.UTF8.GetString(newFileData);
                }
                catch (WebException e)
                {
                    // Do something such as log error, but this is based on OP's original code
                    // so for now we do nothing.
                    return "";
                }
            }

        }


        public void UploadToFTP(string filename)
        {

            FileInfo objFile = new FileInfo(filename);
            FtpWebRequest objFTPRequest;

            // Create FtpWebRequest object 
            objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + _ftpServerIP + "/" + objFile.Name));

            // Set Credintials
            objFTPRequest.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

            // By default KeepAlive is true, where the control connection is 
            // not closed after a command is executed.
            objFTPRequest.KeepAlive = false;

            // Set the data transfer type.
            objFTPRequest.UseBinary = true;

            // Set content length
            objFTPRequest.ContentLength = objFile.Length;

            // Set request method
            objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Set buffer size
            int intBufferLength = 16 * 1024;
            byte[] objBuffer = new byte[intBufferLength];

            // Opens a file to read
            using (FileStream objFileStream = objFile.OpenRead())
            {
                try
                {
                    // Get Stream of the file
                    using (Stream objStream = objFTPRequest.GetRequestStream())
                    {
                        int len = 0;

                        while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                        {
                            // Write file Content 
                            objStream.Write(objBuffer, 0, len);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
