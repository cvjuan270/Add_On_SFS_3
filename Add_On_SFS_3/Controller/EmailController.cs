using Add_On_SFS_3.Model;
using Add_On_SFS_3.View;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Add_On_SFS_3.Controller
{
    public class EmailController
    {
        #region VAriables de documento
        private static string oRucEmi;
        private static string oRazSocEmi;
        private static string oDireccEmi;

        private static string oCardCode;
        private static string oLicTradNum;
        private static string oCardName;
        private static string oDocType;
        private static string oNomDocu;
        private static int oDocEntry;
        private static string oSerie;
        private static string oFolioNum;
        private static string oTaxdate;
        private static string oDocTotal;
        #endregion

        #region rutas
        //private static string RutaData;
        private static string RutaXml;
        private static string RutaPdf;
        private static string RutaCdr;
        private static string RutaXmlCdr;
        #endregion

        public string[] respuestaSendcorreo = new string[2];

        private DataTable dt;
        public static List<string> Archivo = new List<string>();

        public static void Datos(SAPbouiCOM.Form oOrderForm)
        {
            #region Emisor
            DataTable dtEmi = Conexion.Ejecutar_dt("EXEC [dbo].[Consulta_Datos_Generales]");
            oRucEmi = dtEmi.Rows[0].ItemArray[0].ToString();
            oRazSocEmi = dtEmi.Rows[0].ItemArray[1].ToString();
            oDireccEmi = dtEmi.Rows[0].ItemArray[2].ToString();
            #endregion

            #region Documento

            SAPbouiCOM.EditText txtCardCode = (SAPbouiCOM.EditText)oOrderForm.Items.Item("4").Specific;
            SAPbouiCOM.EditText txtLicTRadNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("123").Specific;
            SAPbouiCOM.EditText txtCadName = (SAPbouiCOM.EditText)oOrderForm.Items.Item("54").Specific;
            // SAPbouiCOM.EditText txtTaxDate = (SAPbouiCOM.EditText)oOrderForm.Items.Item("46").Specific;
            SAPbouiCOM.ComboBox txtSerie = (SAPbouiCOM.ComboBox)oOrderForm.Items.Item("88").Specific;
            SAPbouiCOM.EditText txtFolioNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("211").Specific;
            SAPbouiCOM.EditText txtDocTotal = (SAPbouiCOM.EditText)oOrderForm.Items.Item("29").Specific;
            SAPbouiCOM.EditText txtTaxDate = (SAPbouiCOM.EditText)oOrderForm.Items.Item("46").Specific;

            oCardCode = txtCardCode.Value.ToString();
            oLicTradNum = txtLicTRadNum.Value.ToString();
            oCardName = txtCadName.Value.ToString();
            oSerie = txtSerie.Selected.Description.ToString();
            oFolioNum = txtFolioNum.Value.ToString();
            oDocTotal = txtDocTotal.Value.ToString();
            oTaxdate = txtTaxDate.Value.ToString();

            switch (oOrderForm.Type.ToString())
            {
                /*FATURA*/
                case "133":
                    oNomDocu = "FACTURA ELECTRONICA";
                    oDocType = "01";

                    break;

                /*GUIA*/
                case "140":
                    oNomDocu = "GUIA DE REMISION";
                    oDocType = "09";
                    break;

                /*NOTA CREDITO*/
                case "179":
                    oNomDocu = "NOTA DE CREDITO";
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
            //RutaData = Settings.Default.DirectorioDATA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
            RutaXml = Settings.Default.DirectorioFIRMA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            RutaPdf = Settings.Default.DirectorioREPO + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
            RutaCdr = Settings.Default.DirectorioRPTA + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
            RutaXmlCdr = Settings.Default.DirectorioRPTA + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
        }

        private void SendEmail(FormCorreo formCorreo)
        {

            try
            {
                string Mensaje = Settings.Default.CuerpoCorreo;
                Mensaje = Mensaje.Replace("@SerNumCor", oSerie + "-" + oFolioNum);
                Mensaje = Mensaje.Replace("@NomSocNeg", oCardName);
                Mensaje = Mensaje.Replace("@FecEmi", oTaxdate);
                Mensaje = Mensaje.Replace("@TotDoc", oDocTotal);
                Mensaje = Mensaje.Replace("@RazSoc", oRazSocEmi);
                Mensaje = Mensaje.Replace("@Ruc", oRucEmi);
                Mensaje = Mensaje.Replace("@DocType", oNomDocu);

                Archivo.Clear();
                Archivo.Add(RutaPdf);
                Archivo.Add(RutaXml);

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(formCorreo.oFromMail));
                mail.From = new MailAddress(Settings.Default.UsuarioSMTP);
                mail.Subject = formCorreo.oAsunto;
                mail.Body = Mensaje;
                mail.IsBodyHtml = true;

                /**/
                foreach (string Adjunto in Archivo)
                {
                    mail.Attachments.Add(new Attachment(Adjunto));
                }

                /**/

                SmtpClient client = new SmtpClient(Settings.Default.ServidorSMTP.ToString(), Settings.Default.PuertoSMTP);

                client.Credentials = new System.Net.NetworkCredential(Settings.Default.UsuarioSMTP, Settings.Default.PassUsuaSMTP);
                client.EnableSsl = true;
                client.Send(mail);
                mail.Dispose();
                Archivo.Clear();
                client.Dispose();
                    respuestaSendcorreo[0] = "0";
                    respuestaSendcorreo[1] = "Mensaje enviado con exito";
             }
                    catch (Exception ex)
                    {

                        respuestaSendcorreo[0] = "1";
                        respuestaSendcorreo[1] = "Excepción: " + ex.Message;
                    }
}

        private static string CreaJsonNomAr()
        {
            //{"nomArch":"20558326256-01-F003-326"}
            JsonPdf jsonPdf = new JsonPdf();
            jsonPdf.nomArch = oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum;
            return JsonConvert.SerializeObject(jsonPdf);
        }
        public static void GetPdf()
        {

            string oJson = CreaJsonNomAr();
            var client = new RestClient(Settings.Default.httpPdf);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response =  client.Execute(solicitud);
         
        }
        public EmailController(SAPbouiCOM.Form oOrderForm)
        {
            
            Datos(oOrderForm);
            Rutas();
            dt = Conexion.Ejecutar_dt(string.Format("select E_Mail from OCRD where CardCode = '{0}'", oCardCode));

            FormCorreo formCorreo = new FormCorreo(dt.Rows[0].ItemArray[0].ToString(), "Comprobante " + "|" + oSerie + "-" + oFolioNum + " | " + oRazSocEmi);
            formCorreo.oFromMail = dt.Rows[0].ItemArray[0].ToString();
           formCorreo.oAsunto = "Comprobante " + "|" + oSerie + "-" + oFolioNum + " | " + oRazSocEmi;

            if (!File.Exists(RutaPdf))
            {
                 GetPdf();
            }


            if (formCorreo.ShowDialog() == DialogResult.OK)
            {
                SendEmail(formCorreo);
                //formCorreo.Dispose();
            }
            else
            {
                respuestaSendcorreo[0] = "1";
                respuestaSendcorreo[1] = "Operacion envio de correo [Cancelada]";
            }

            //if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[0].ToString()))
            //{
            //    if (formCorreo.ShowDialog() == DialogResult.OK)
            //    {
            //        SendEmail(formCorreo);
            //    }
            //    else
            //    {
            //        respuestaSendcorreo[0] = "1";
            //        respuestaSendcorreo[1] = "Operacion envio de correo [Cancelada]";
            //    }
            //}
            //else
            //{
            //    SendEmail(formCorreo);
            //    formCorreo.Dispose();
            //}

        }

    }
}
