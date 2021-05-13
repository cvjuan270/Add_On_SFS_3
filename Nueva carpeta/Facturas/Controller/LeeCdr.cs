using Facturas;
using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Facturas.Controller
{
    public class LeeCdr
    {
        public static string[] GetCdr(string oRutCdrZip, string oRutCdrxml, string DocType = null)
        {
            string CdrXml = null;
            string[] ReturnCdr = new string[3];

            if (DocType =="09")
            {
                CdrXml= Settings.Default.RutaDirectorioGR+Settings.Default.DirectorioRPTA;
            }
            else
            {
                CdrXml = CdrXml = Settings.Default.RutaDirectorio+Settings.Default.DirectorioRPTA;
            }
            if (!File.Exists(oRutCdrxml))
            {
                ZipFile.ExtractToDirectory(oRutCdrZip, CdrXml );
            }

            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(oRutCdrxml);

            //Display all the book titles.
            XmlNodeList elemList = doc.GetElementsByTagName("cbc:ResponseCode");
            XmlNodeList elemList1 = doc.GetElementsByTagName("cbc:Description");
            XmlNodeList elemList2 = doc.GetElementsByTagName("DigestValue");
            ReturnCdr[0] = elemList[0].InnerXml;
            ReturnCdr[1] = elemList1[0].InnerXml;
            ReturnCdr[2] = elemList2[0].InnerXml;
            return ReturnCdr;
        }

    }
}
