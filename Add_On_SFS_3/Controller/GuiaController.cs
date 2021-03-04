using Add_On_SFS_3.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Add_On_SFS_3.Controller
{
    public class GuiaController
    {
        #region VAriables de documento
        private static DataTable DTCabecera;

        public static string PATH = Settings.Default.RutaLOG;
        Log oLog = new Log(PATH);

        public string[] respuestacdr = new string[2];
       
        private static string oRucEmi;
        private static string oRazSocEmi;
        private static string oDireccEmi;

        private static string oLicTradNum;
        private static string oCardName;
        private static string oDocName;

        private static string oDocType;
        private static int oDocEntry;
        private static string oSerie;
        private static string oFolioNum;

        #region rutas
        private static string RutaData;
        private static string RutaXml;
        private static string RutaPdf;
        private static string RutaCdr;
        private static string RutaXmlCdr;
        #endregion
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
            RutaData = Settings.Default.DirectorioDATA_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
            RutaXml = Settings.Default.DirectorioFIRMA_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            RutaPdf = Settings.Default.DirectorioREPO_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
            RutaCdr = Settings.Default.DirectorioRPTA_GR + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
            RutaXmlCdr = Settings.Default.DirectorioRPTA_GR + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
        }
        private static DataTable GetDTCabecera(int DocEntry)
        {
            DataTable dataTableReturn = null;
            string cmd = string.Format("EXEC [dbo].[Consulta_SFS_CAB_GR] @DocEntry = {0}", DocEntry);
            dataTableReturn = Conexion.Ejecutar_dt(cmd);
            return dataTableReturn;
        }
        private static DataTable GetDTDetalle(int DocEntry)
        {
            DataTable dataTableReturn = null;
            string cmd = string.Format("EXEC [dbo].[Consulta_SFS_DET_GR] @DocEntry = {0}", DocEntry);
            dataTableReturn = Conexion.Ejecutar_dt(cmd);
            return dataTableReturn;
        }
        private static List<DetalleGR> GetListDetalleGR(int DocEntry)
        {
            List<DetalleGR> detalleGRsReturn = new List<DetalleGR>();
            DataTable dt = GetDTDetalle(DocEntry);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DetalleGR det = new DetalleGR();
                det.uniMedidaItem = dt.Rows[i].ItemArray[0].ToString();
                det.canItem = dt.Rows[i].ItemArray[1].ToString();
                det.desItem = dt.Rows[i].ItemArray[2].ToString();
                det.codItem = dt.Rows[i].ItemArray[3].ToString();

                detalleGRsReturn.Add(det);
            }
            return detalleGRsReturn;
        }
        private static CabeceraGR GetCabeceraGR(int DocEntry)
        {
            DTCabecera = null;
            DTCabecera = GetDTCabecera(DocEntry);
            CabeceraGR cabeceraGR = new CabeceraGR();

            cabeceraGR.fecEmision = DTCabecera.Rows[0].ItemArray[0].ToString();
            cabeceraGR.horEmision = DTCabecera.Rows[0].ItemArray[1].ToString();
            cabeceraGR.tipDocGuia = DTCabecera.Rows[0].ItemArray[2].ToString();
            cabeceraGR.serNumDocGuia = DTCabecera.Rows[0].ItemArray[3].ToString();
            cabeceraGR.numDocDestinatario = DTCabecera.Rows[0].ItemArray[4].ToString();
            cabeceraGR.tipDocDestinatario = DTCabecera.Rows[0].ItemArray[5].ToString();
            cabeceraGR.rznSocialDestinatario = DTCabecera.Rows[0].ItemArray[6].ToString();
            cabeceraGR.motTrasladoDatosEnvio = DTCabecera.Rows[0].ItemArray[7].ToString();
            cabeceraGR.desMotivoTrasladoDatosEnvio = DTCabecera.Rows[0].ItemArray[8].ToString();
            cabeceraGR.indTransbordoProgDatosEnvio = DTCabecera.Rows[0].ItemArray[9].ToString();
            cabeceraGR.psoBrutoTotalBienesDatosEnvio = DTCabecera.Rows[0].ItemArray[10].ToString();
            cabeceraGR.uniMedidaPesoBrutoDatosEnvio = DTCabecera.Rows[0].ItemArray[11].ToString();
            cabeceraGR.numBultosDatosEnvio = DTCabecera.Rows[0].ItemArray[12].ToString();
            cabeceraGR.modTrasladoDatosEnvio = DTCabecera.Rows[0].ItemArray[13].ToString();
            cabeceraGR.fecInicioTrasladoDatosEnvio = DTCabecera.Rows[0].ItemArray[14].ToString();
            cabeceraGR.numDocTransportista = DTCabecera.Rows[0].ItemArray[15].ToString();
            cabeceraGR.tipDocTransportista = DTCabecera.Rows[0].ItemArray[16].ToString();
            cabeceraGR.nomTransportista = DTCabecera.Rows[0].ItemArray[17].ToString();
            cabeceraGR.numPlacaTransPrivado = DTCabecera.Rows[0].ItemArray[18].ToString();
            cabeceraGR.numDocIdeConductorTransPrivado = DTCabecera.Rows[0].ItemArray[19].ToString();
            cabeceraGR.tipDocIdeConductorTransPrivado = DTCabecera.Rows[0].ItemArray[20].ToString();
            cabeceraGR.nomConductorTransPrivado = DTCabecera.Rows[0].ItemArray[21].ToString();
            cabeceraGR.ubiLlegada = DTCabecera.Rows[0].ItemArray[22].ToString();
            cabeceraGR.dirLlegada = DTCabecera.Rows[0].ItemArray[23].ToString();
            cabeceraGR.ubiPartida = DTCabecera.Rows[0].ItemArray[24].ToString();
            cabeceraGR.dirPartida = DTCabecera.Rows[0].ItemArray[25].ToString();
            cabeceraGR.ublVersionId = DTCabecera.Rows[0].ItemArray[26].ToString();
            cabeceraGR.customizationId = DTCabecera.Rows[0].ItemArray[27].ToString();

            return cabeceraGR;
        }
        public  static RootGR GetDocumentElectronicoGR(int DocEntry)
        {
            RootGR root = new RootGR
            {
                cabecera = GetCabeceraGR(DocEntry),
                detalle = GetListDetalleGR(DocEntry)
            };
            return root;
        }
        public void GuardaJson(RootGR root)
        {
            string json = JsonConvert.SerializeObject(root, Formatting.Indented);
            File.WriteAllText(RutaData, json);
        }
        public static string CreaJsonDoc(string num_ruc, string tip_docu, string num_docu)
        {
            JsonDoc body = new JsonDoc();

            body.num_ruc = num_ruc;
            body.tip_docu = tip_docu;
            body.num_docu = num_docu;

            return JsonConvert.SerializeObject(body);
        }
        public async Task<bool> GetXmlAsync()
        {
            string oJson = CreaJsonDoc(oRucEmi, oDocType, oSerie + "-" + oFolioNum);
            var client = new RestClient(Settings.Default.httpFirma_GR);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        public async Task<bool> GetCdrAsync()
        {
            string oJson = CreaJsonDoc(oRucEmi, oDocType, oSerie + "-" + oFolioNum);
            var client = new RestClient(Settings.Default.httpEnvio_GR);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        private static string CreaJsonNomAr()
        {
            //{"nomArch":"20558326256-01-F003-326"}
            JsonPdf jsonPdf = new JsonPdf();
            jsonPdf.nomArch = oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum;
            return JsonConvert.SerializeObject(jsonPdf);
        }
        public static void GetPdf_GR()
        {
            string oJson = CreaJsonNomAr();
            var client = new RestClient(Settings.Default.httpPdf_GR);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(solicitud);
        }
        public async Task<bool> GetActualizaPantallaAsync()
        {
            var client = new RestClient(Settings.Default.httpActualizar_GR);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", "{\"txtSecuencia\":\"000\"}", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        private void ActualiRptaCdt()
        {
            Conexion.EjecutarQuery(string.Format("EXEC [dbo].[Actualizar_Rpta_Cdr_GR]@Docentry = {0}, @U_ResponseCode = '{1}', @U_Description = '{2}',@U_DigestValue = '{3}'", oDocEntry, respuestacdr[0], respuestacdr[1],respuestacdr[2]));
        }

        public GuiaController(SAPbouiCOM.Form oOrderForm)
        {
            try
            {
                Datos(oOrderForm);
                Rutas();

                /*crea el archivo Json*/
                var docjson = GetDocumentElectronicoGR(oDocEntry);
                GuardaJson(docjson);

                var actualiza = GetActualizaPantallaAsync();
                actualiza.Wait();

                /*FIRMA XML*/
                var rptaXml = GetXmlAsync();
                rptaXml.Wait();
                var resultXml = rptaXml.Result;
                actualiza.Dispose();
                rptaXml.Dispose();

                if (File.Exists(RutaXml))
                {
                    actualiza.Wait();
                    /*ENVIA XML Y RECUPERA CDR*/
                    var getcdr = GetCdrAsync();
                    getcdr.Wait();
                    var resultCdr = getcdr.Result;

                    actualiza.Dispose();
                    getcdr.Dispose();

                    if (File.Exists(RutaCdr))
                    {
                        respuestacdr = LeeCdr.GetCdr(RutaCdr, RutaXmlCdr,"09");
                        ActualiRptaCdt();
                        if (!File.Exists(RutaPdf))
                        {
                            GetPdf_GR();
                        }
                    }
                    else
                    {
                        respuestacdr[0] = "1";
                        respuestacdr[1] = "Rechazado por SUNAT y / ó Error al invocar el servicio de SUNAT.";
                        oLog.Add("Error: " + "DocType: " + oDocType + " - DocEntry: " + oDocEntry + " Exception" + respuestacdr);
                    }
                }
                else
                {
                    respuestacdr[0] = "1";
                    respuestacdr[1] = "Error al firmar archivo XML";
                    oLog.Add("Error: " + "DocType: " + oDocType + " - DocEntry: " + oDocEntry + " Exception" + respuestacdr);
                }

            }
            catch (Exception ex)
            {

                respuestacdr[0] = "1";
                respuestacdr[1] = ex.Message;
                oLog.Add("Error: " + "DocType: " + oDocType + " - DocEntry: " + oDocEntry + " Exception" + ex.Message);

            }
            //guardarRptaCdt(respuestacdr[0],respuestacdr[1]);

        }
    }
}
