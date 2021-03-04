using Add_On_SFS_3.Controller;
using Add_On_SFS_3.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.Reporting.Processing;

namespace Add_On_SFS_3.View
{
    public partial class FormReporte : Form
    {

        public static string PATH = Settings.Default.RutaLOG;
        Log oLog = new Log(PATH);
        public string[] respuestaReporte = new string[2];
        private static DataTable oDdataTable = null;


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
        public FormReporte(SAPbouiCOM.Form oOrderForm)
        {
            Datos(oOrderForm);
            //Rutas();
            InitializeComponent();
           
        }

        private void FormReporte_Load(object sender, EventArgs e)
        {
            
            string Impresoras;
            for (int i = 0; i < System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count; i++)
            {
                Impresoras = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                comboBox1.Items.Add(Impresoras);
            }

            string cmd = string.Format("EXEC [dbo].[Consulta_Datos_Reporte_GR] @DocEntry = {0}", oDocEntry);
            DataTable dt = Conexion.Ejecutar_dt(cmd);

            var ReportGR_A4 = new Reportes.Report_GR_A4(dt,Settings.Default.DirectorioIMG+"Logo.png");
            ReportGR_A4.Name = oRucEmi + "-09-" + oSerie + "-" + oFolioNum;
            ReportGR_A4.DocumentName= oRucEmi + "-09-" + oSerie + "-" + oFolioNum;
            reportViewer1.Report = ReportGR_A4;
            reportViewer1.RefreshReport();
        }
        
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
            //RutaData = Settings.Default.DirectorioDATA + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".json";
            RutaXml = Settings.Default.DirectorioFIRMA_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
            RutaPdf = Settings.Default.DirectorioREPO_GR + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".pdf";
            RutaCdr = Settings.Default.DirectorioRPTA_GR + "R" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".zip";
            RutaXmlCdr = Settings.Default.DirectorioRPTA_GR + "R" + "-" + oRucEmi + "-" + oDocType + "-" + oSerie + "-" + oFolioNum + ".xml";
        }

        private void buttonImprimir_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Seleccionar Impresora");
            }
            else
            {
                var documento = new PrintDocument();
                documento.PrinterSettings.PrinterName = comboBox1.Text;
                if (documento.PrinterSettings.IsValid)
                {
                    PrinterSettings printerSettings = new PrinterSettings();
                    printerSettings.PrinterName = comboBox1.Text;
                    printerSettings.PrintFileName = oRucEmi + "-09-" + oSerie + "-" + oFolioNum;
                    printerSettings.Copies = short.Parse(numericUpDown1.Value.ToString());
                    var reportProcesor = new ReportProcessor();
                    reportProcesor.PrintReport(reportViewer1.ReportSource, printerSettings);

                }
                Dispose();
                this.Close();
            }
            
        }
    }
}
