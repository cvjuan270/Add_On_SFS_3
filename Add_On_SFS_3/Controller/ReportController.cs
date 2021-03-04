using Add_On_SFS_3.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Add_On_SFS_3.View;
using RestSharp;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Add_On_SFS_3.Controller
{
    public class ReportController
    {
        public static string PATH = Settings.Default.RutaLOG;
        Log oLog = new Log(PATH);
        public string[] respuestaReporte = new string[2];


        #region VAriables de documento
        private static string oRucEmi;
        private static string oRazSocEmi;
        private static string oDireccEmi;

        private static string oLicTradNum;
        private static string oCardName;
        private static string oDocType;
        private static int oDocEntry;
        private static string oSerie;
        private static string oFolioNum;
        #endregion

        #region rutas
        //private static string RutaData;
        private static string RutaXml;
        private static string RutaPdf;
        private static string RutaCdr;
        private static string RutaXmlCdr;
        #endregion

        

        public static void Datos(SAPbouiCOM.Form oOrderForm)
        {
            #region Emisor
            DataTable dtEmi = Conexion.Ejecutar_dt("EXEC [dbo].[Consulta_Datos_Generales]");
            oRucEmi = dtEmi.Rows[0].ItemArray[0].ToString();
            oRazSocEmi = dtEmi.Rows[0].ItemArray[1].ToString();
            oDireccEmi = dtEmi.Rows[0].ItemArray[2].ToString();
            #endregion

            #region Documento
            SAPbouiCOM.EditText txtLicTRadNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("123").Specific;
            SAPbouiCOM.EditText txtCadName = (SAPbouiCOM.EditText)oOrderForm.Items.Item("54").Specific;
            // SAPbouiCOM.EditText txtTaxDate = (SAPbouiCOM.EditText)oOrderForm.Items.Item("46").Specific;
            SAPbouiCOM.ComboBox txtSerie = (SAPbouiCOM.ComboBox)oOrderForm.Items.Item("88").Specific;
            SAPbouiCOM.EditText txtFolioNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("211").Specific;
            SAPbouiCOM.EditText txtDocTotal = (SAPbouiCOM.EditText)oOrderForm.Items.Item("29").Specific;

            oLicTradNum = txtLicTRadNum.Value.ToString();
            oCardName = txtCadName.Value.ToString();
            oSerie = txtSerie.Selected.Description.ToString();
            oFolioNum = txtFolioNum.Value.ToString();
            switch (oOrderForm.Type.ToString())
            {
                /*FATURA*/
                case "133":
                    oDocType = "01";

                    break;

                /*GUIA*/
                case "140":
                    oDocType = "09";
                    break;

                /*NOTA CREDITO*/
                case "179":
                    oDocType = "07";
                    break;
            }

            /*DocEntry*/
            string doctxt = oOrderForm.BusinessObject.Key;
            XDocument docxml = XDocument.Parse(doctxt);
            XElement DocumentParams = docxml.Element("DocumentParams");
            oDocEntry = int.Parse(DocumentParams.Element("DocEntry").Value);
            #endregion

        }
        public static void Rutas()
        {
            if (oDocType == "09")
            {
                //RutaData = Settings.Default.DirectorioDATA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
                RutaXml = Settings.Default.DirectorioFIRMA_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
                RutaPdf = Settings.Default.DirectorioREPO_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
                RutaCdr = Settings.Default.DirectorioRPTA_GR + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
                RutaXmlCdr = Settings.Default.DirectorioRPTA_GR + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            }
            else
            {
                //RutaData = Settings.Default.DirectorioDATA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
                RutaXml = Settings.Default.DirectorioFIRMA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
                RutaPdf = Settings.Default.DirectorioREPO + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
                RutaCdr = Settings.Default.DirectorioRPTA + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
                RutaXmlCdr = Settings.Default.DirectorioRPTA + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            }
        }

        private static string CreaJsonNomAr()
        {
            //{"nomArch":"20558326256-01-F003-326"}
            JsonPdf jsonPdf = new JsonPdf();
            jsonPdf.nomArch = oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum;
            return JsonConvert.SerializeObject(jsonPdf);
        }
        public static async Task<bool> GetPdf()
        {

            string oJson = CreaJsonNomAr();
            var client = new RestClient(Settings.Default.httpPdf);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        private  void Imprimir()
        {
            if (!File.Exists(RutaPdf))
            {
                var pdf = GetPdf();
                pdf.Wait();
                var rpta= pdf.Result;
            }

            FormImprimir formImprimir = new FormImprimir();
            DialogResult dialogResult = formImprimir.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                   

                    var impresora = new PrinterSettings();
                    impresora.PrinterName = formImprimir.ImpresoraName;
                    for (int i = 0; i <= formImprimir.NumCopias; i++)
                    {
                        Process p = new Process();
                        p.StartInfo = new ProcessStartInfo()
                        {
                            CreateNoWindow = true,
                            Verb = "Print",
                            FileName = RutaPdf,//put the correct path here,
                            Arguments = impresora.PrinterName
                            // WindowStyle = ProcessWindowStyle.Hidden
                        };
                        p.Start();
                        p.Close();
                    }



                    respuestaReporte[0] = "0";
                    respuestaReporte[1] = "Documento impreso en: " + formImprimir.ImpresoraName;
                }
                catch (Exception ex)
                {

                    respuestaReporte[0] = "1";
                    respuestaReporte[1] = "Excepcion" + ex.Message;
                }
            }
            else
            {
                respuestaReporte[0] = "1";
                respuestaReporte[1] = "Operación Cancelada" ;
            }
        }
        public static bool soloGeneraPDF(SAPbouiCOM.Form oOrderForm)
        {
            //solo genera el PDF para enviar por correo
            bool orpta;
            Datos(oOrderForm);
            Rutas();
            if (!File.Exists(RutaPdf))
            { 
                var pdf = GetPdf();
                pdf.Wait();

                orpta = pdf.Result;
            }
            return true;
        }
        public ReportController(SAPbouiCOM.Form oOrderForm )
        {
            Datos(oOrderForm);
            Rutas();

            if (File.Exists(RutaXml)== true)
            {
                if (File.Exists(RutaCdr))
                {
                    //leer cdr
                    string[] rptaCdr = new string[2];
                    rptaCdr = LeeCdr.GetCdr(RutaCdr, RutaXmlCdr);

                    if (rptaCdr[0] == "0")
                    {
                        Imprimir();
                        

                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("ResponsCode[" + rptaCdr[0] + "] | Descripción[" + rptaCdr[1] + "]\n\n Desea imprimir de todas formas?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.OK)
                        {
                            //Imprimir
                            Imprimir();
                        }
                        else
                        {
                            respuestaReporte[0] = "1";
                            respuestaReporte[1] = "No se imprimio ningun reporte: CDR="+rptaCdr[0]+"|"+rptaCdr[1];
                        }
                    }
                }
                else
                {
                  DialogResult result =  MessageBox.Show("Desea inprimirlo de todas formas?", "Comprobante aun no se envió a sunat", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        Imprimir();
                    }
                    else
                    {
                        respuestaReporte[0] = "1";
                        respuestaReporte[1] = "No se imprimio ningun reporte";
                    }
                }
            }
            else
            {
                respuestaReporte[0] = "1";
                respuestaReporte[1] = "D.E. sin Firmar; Probar de forma manual en: "+Settings.Default.httpBandeja;
            }
        }
    }



}
