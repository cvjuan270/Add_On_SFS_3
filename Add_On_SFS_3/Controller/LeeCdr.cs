using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Add_On_SFS_3.Controller
{
    public class LeeCdr
    {
        public static string[] GetCdr(string oRutCdrZip, string oRutCdrxml)
        {
            string[] ReturnCdr = new string[2];

            if (!File.Exists(oRutCdrxml))
            {
                ZipFile.ExtractToDirectory(oRutCdrZip, Settings.Default.DirectorioRPTA);
            }

            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(oRutCdrxml);

            //Display all the book titles.
            XmlNodeList elemList = doc.GetElementsByTagName("cbc:ResponseCode");
            XmlNodeList elemList1 = doc.GetElementsByTagName("cbc:Description");
            ReturnCdr[0] = elemList[0].InnerXml;
            ReturnCdr[1] = elemList1[0].InnerXml;
            return ReturnCdr;
        }

    }
}
