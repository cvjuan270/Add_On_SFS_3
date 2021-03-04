using Add_On_SFS_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using RestSharp;
//using Spire.Pdf;
using System.Xml.Linq;
using System.Threading;

namespace Add_On_SFS_3.Controller
{
    public class FacturaController
    {
        #region VAriables de documento
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

        public static  RootFT GetDocumentoElectronico(int DocEntry)
        {
            RootFT root = new RootFT
            {
                cabecera = GetCabecera(DocEntry),
                detalle = GetListDetalle(DocEntry),
                leyendas = GetLeyenda(DocEntry),
                tributos = GetTributo(DocEntry)
            };
            return  root;
        }

        public static Cabecera GetCabecera(int DocEntry)
        {
            DataTable dt = Conexion.Ejecutar_dt(string.Format("EXEC[dbo].[Consulta_SFS_CAB] @DocEntry = '{0}'", DocEntry));

            Cabecera cabecera = new Cabecera();

            cabecera.tipOperacion = dt.Rows[0].ItemArray[0].ToString();
            cabecera.fecEmision = dt.Rows[0].ItemArray[1].ToString();
            cabecera.horEmision = dt.Rows[0].ItemArray[2].ToString();
            cabecera.fecVencimiento = dt.Rows[0].ItemArray[3].ToString();
            cabecera.codLocalEmisor = dt.Rows[0].ItemArray[4].ToString();
            cabecera.tipDocUsuario = dt.Rows[0].ItemArray[5].ToString();
            cabecera.numDocUsuario = dt.Rows[0].ItemArray[6].ToString();
            cabecera.rznSocialUsuario = dt.Rows[0].ItemArray[7].ToString();
            cabecera.tipMoneda = dt.Rows[0].ItemArray[8].ToString();
            cabecera.sumTotTributos = dt.Rows[0].ItemArray[9].ToString();
            cabecera.sumTotValVenta = dt.Rows[0].ItemArray[10].ToString();
            cabecera.sumPrecioVenta = dt.Rows[0].ItemArray[11].ToString();
            cabecera.sumDescTotal = dt.Rows[0].ItemArray[12].ToString();
            cabecera.sumOtrosCargos = dt.Rows[0].ItemArray[13].ToString();
            cabecera.sumTotalAnticipos = dt.Rows[0].ItemArray[14].ToString();
            cabecera.sumImpVenta = dt.Rows[0].ItemArray[15].ToString();
            cabecera.ublVersionId = dt.Rows[0].ItemArray[16].ToString();
            cabecera.customizationId = dt.Rows[0].ItemArray[17].ToString();
            cabecera.adicionalCabecera = GetAditionalCabecera(DocEntry);

            return cabecera;
        }
        public static AdicionalCabecera GetAditionalCabecera(int DocEntry)
        {
            DataTable dt = Conexion.Ejecutar_dt(string.Format("EXEC  [dbo].[Consulta_SFS_ACA] @DocEntry = '{0}'", DocEntry));
            
            AdicionalCabecera adicionalCabecera = new AdicionalCabecera();

            adicionalCabecera.ctaBancoNacionDetraccion = dt.Rows[0].ItemArray[0].ToString();
            adicionalCabecera.codBienDetraccion = dt.Rows[0].ItemArray[1].ToString();
            adicionalCabecera.porDetraccion = dt.Rows[0].ItemArray[2].ToString();
            adicionalCabecera.mtoDetraccion = dt.Rows[0].ItemArray[3].ToString();
            adicionalCabecera.codMedioPago = dt.Rows[0].ItemArray[4].ToString();
            adicionalCabecera.codPaisCliente = dt.Rows[0].ItemArray[5].ToString();
            adicionalCabecera.codUbigeoCliente = dt.Rows[0].ItemArray[6].ToString();
            adicionalCabecera.desDireccionCliente = dt.Rows[0].ItemArray[7].ToString();
            adicionalCabecera.codPaisEntrega = dt.Rows[0].ItemArray[8].ToString();
            adicionalCabecera.codUbigeoEntrega = dt.Rows[0].ItemArray[9].ToString();
            adicionalCabecera.desDireccionEntrega = dt.Rows[0].ItemArray[10].ToString();

            return adicionalCabecera;
        }
        public static List<Detalle> GetListDetalle(int DocEntry) 
        {
            DataTable dt = Conexion.Ejecutar_dt(string.Format("EXEC [dbo].[Consulta_SFS_DET] @DocEntry = '{0}'", DocEntry));
            List<Detalle> odetalle = new List<Detalle>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Detalle detalle = new Detalle();
                detalle.codUnidadMedida = dt.Rows[i].ItemArray[0].ToString();
                detalle.codUnidadMedida = dt.Rows[i].ItemArray[0].ToString();
                detalle.ctdUnidadItem = dt.Rows[i].ItemArray[1].ToString();
                detalle.codProducto = dt.Rows[i].ItemArray[2].ToString();
                detalle.codProductoSUNAT = dt.Rows[i].ItemArray[3].ToString();
                detalle.desItem = dt.Rows[i].ItemArray[4].ToString();
                detalle.mtoValorUnitario = dt.Rows[i].ItemArray[5].ToString();
                detalle.sumTotTributosItem = dt.Rows[i].ItemArray[6].ToString();
                detalle.codTriIGV = dt.Rows[i].ItemArray[7].ToString();
                detalle.mtoIgvItem = dt.Rows[i].ItemArray[8].ToString();
                detalle.mtoBaseIgvItem = dt.Rows[i].ItemArray[9].ToString();
                detalle.nomTributoIgvItem = dt.Rows[i].ItemArray[10].ToString();
                detalle.codTipTributoIgvItem = dt.Rows[i].ItemArray[11].ToString();
                detalle.tipAfeIGV = dt.Rows[i].ItemArray[12].ToString();
                detalle.porIgvItem = dt.Rows[i].ItemArray[13].ToString();
                detalle.codTriISC = dt.Rows[i].ItemArray[14].ToString();
                detalle.mtoIscItem = dt.Rows[i].ItemArray[15].ToString();
                detalle.mtoBaseIscItem = dt.Rows[i].ItemArray[16].ToString();
                detalle.nomTributoIscItem = dt.Rows[i].ItemArray[17].ToString();
                detalle.codTipTributoIscItem = dt.Rows[i].ItemArray[18].ToString();
                detalle.tipSisISC = dt.Rows[i].ItemArray[19].ToString();
                detalle.porIscItem = dt.Rows[i].ItemArray[20].ToString();
                detalle.codTriOtro = dt.Rows[i].ItemArray[21].ToString();
                detalle.mtoTriOtroItem = dt.Rows[i].ItemArray[22].ToString();
                detalle.mtoBaseTriOtroItem = dt.Rows[i].ItemArray[23].ToString();
                detalle.nomTributoOtroItem = dt.Rows[i].ItemArray[24].ToString();
                detalle.codTipTributoOtroItem = dt.Rows[i].ItemArray[25].ToString();
                detalle.porTriOtroItem = dt.Rows[i].ItemArray[26].ToString();
                detalle.codTriIcbper = dt.Rows[i].ItemArray[27].ToString();
                detalle.mtoTriIcbperItem = dt.Rows[i].ItemArray[28].ToString();
                detalle.ctdBolsasTriIcbperItem = dt.Rows[i].ItemArray[29].ToString();
                detalle.nomTributoIcbperItem = dt.Rows[i].ItemArray[30].ToString();
                detalle.codTipTributoIcbperItem = dt.Rows[i].ItemArray[31].ToString();
                detalle.mtoTriIcbperUnidad = dt.Rows[i].ItemArray[32].ToString();
                detalle.mtoPrecioVentaUnitario = dt.Rows[i].ItemArray[33].ToString();
                detalle.mtoValorVentaItem = dt.Rows[i].ItemArray[34].ToString();
                detalle.mtoValorReferencialUnitario = dt.Rows[i].ItemArray[35].ToString();

                odetalle.Add(detalle);
            }

            return odetalle;
        }       
        public static List<Leyenda> GetLeyenda(int DocEntry)
        {
            DataTable dt = Conexion.Ejecutar_dt(string.Format("EXEC [dbo].[Consulta_SFS_LEY] @DocEntry = '{0}'", DocEntry));

            List<Leyenda> leyendas = new List<Leyenda>();
            Leyenda leyenda = new Leyenda();

            leyenda.codLeyenda = dt.Rows[0].ItemArray[0].ToString();
            leyenda.desLeyenda = dt.Rows[0].ItemArray[1].ToString();

            leyendas.Add(leyenda);
            return leyendas;
        }
        public static List<Tributo> GetTributo(int DocEntry)
        {
            DataTable dt = Conexion.Ejecutar_dt(string.Format("EXEC  [dbo].[Consulta_SFS_TRY] @DocEntry = '{0}'", DocEntry));
            List<Tributo> tributos = new List<Tributo>();
            Tributo tributo = new Tributo();

            tributo.ideTributo = dt.Rows[0].ItemArray[0].ToString();
            tributo.nomTributo = dt.Rows[0].ItemArray[1].ToString();
            tributo.codTipTributo = dt.Rows[0].ItemArray[2].ToString();
            tributo.mtoBaseImponible = dt.Rows[0].ItemArray[3].ToString();
            tributo.mtoTributo = dt.Rows[0].ItemArray[4].ToString();

            tributos.Add(tributo);
            return tributos;
        }

        public static string CreaJsonDoc(string num_ruc, string tip_docu, string num_docu)
        {
            JsonDoc body = new JsonDoc();

            body.num_ruc = num_ruc;
            body.tip_docu = tip_docu;
            body.num_docu = num_docu;

            return JsonConvert.SerializeObject(body);
        }

        //public static void ImprimirPDFFactura(string path, string Impresora)
        //{
        //    //PdfDocument pdf = new PdfDocument();
        //    //pdf.LoadFromFile(path);
        //    //pdf.PrinterName=Impresora;
        //    //pdf.Print();
        //}
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
        public  static void Rutas()
        {
            RutaData = Settings.Default.DirectorioDATA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
            RutaXml = Settings.Default.DirectorioFIRMA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            RutaPdf = Settings.Default.DirectorioREPO + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
            RutaCdr = Settings.Default.DirectorioRPTA + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
            RutaXmlCdr = Settings.Default.DirectorioRPTA + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
        }
        public void GuardaJson(RootFT root)
        {
 
                string json = JsonConvert.SerializeObject(root, Formatting.Indented);
                File.WriteAllText(RutaData, json);

        }

        public async Task<bool> GetXmlAsync()
        {
            string oJson = FacturaController.CreaJsonDoc(oRucEmi, oDocType, oSerie + "-" + oFolioNum);
            var client = new RestClient(Settings.Default.httpFirma);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
             IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        public async Task<bool> GetCdrAsync()
        {
            string oJson = FacturaController.CreaJsonDoc(oRucEmi, oDocType, oSerie + "-" + oFolioNum);
            var client = new RestClient(Settings.Default.httpEnvio);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", oJson, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        public async Task<bool> GetActualizaPantallaAsync()
        {
            var client = new RestClient(Settings.Default.httpActualizar);
            var solicitud = new RestRequest(Method.POST);
            solicitud.AddHeader("Content-Type", "application/json");
            solicitud.AddParameter("application/json", "{\"txtSecuencia\":\"000\"}", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(solicitud);
            return response.IsSuccessful;
        }

        private void ActualiRptaCdt()
        {
            Conexion.EjecutarQuery(string.Format("EXEC [dbo].[Actualizar_Rpta_Cdr]@Docentry = {0}, @U_ResponseCode = '{1}', @U_Description = '{2}'", oDocEntry, respuestacdr[0], respuestacdr[1]));
        }
        public  FacturaController(SAPbouiCOM.Form oOrderForm)
        {
            try
            {
                Datos(oOrderForm);
                Rutas();

                /*crea el archivo Json*/
                var docjson = GetDocumentoElectronico(oDocEntry);
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
                        respuestacdr = LeeCdr.GetCdr(RutaCdr, RutaXmlCdr);
                        ActualiRptaCdt();
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
                    respuestacdr[1]= "Error al firmar archivo XML";
                    oLog.Add("Error: " + "DocType: " + oDocType + " - DocEntry: " + oDocEntry + " Exception" + respuestacdr);
                }
   
            }
            catch (Exception ex)
            {
                
                respuestacdr[0] = "1";
                respuestacdr[1] = ex.Message;
                oLog.Add("Error: "+"DocType: "+oDocType+" - DocEntry: "+oDocEntry+" Exception"+ex.Message);

            }
            //guardarRptaCdt(respuestacdr[0],respuestacdr[1]);
 
        }
    }
}

