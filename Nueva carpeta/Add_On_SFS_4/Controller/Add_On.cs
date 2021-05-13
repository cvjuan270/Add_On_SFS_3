using Facturas.View;
using Facturas.Controller;
using Microsoft.VisualBasic;

namespace Add_On_SFS_4.Controller
{
    class Add_On
    {
        private SAPbouiCOM.Application SBO_Application;

        private SAPbouiCOM.Form oOrderForm;

        private void setApplication()
        {
            SAPbouiCOM.ISboGuiApi sboGuiApi = null;
            string sConnectionString = null;
            sboGuiApi = new SAPbouiCOM.SboGuiApi();
            sConnectionString = Interaction.Command();
            sboGuiApi.Connect(sConnectionString);
            SBO_Application = sboGuiApi.GetApplication(-1);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (((pVal.FormType == 133 | pVal.FormType == 140 | pVal.FormType == 179 & pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD) & (pVal.Before_Action == true)))
            {
                oOrderForm = SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                /*dibuja botones*/
                if (((pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) & (pVal.BeforeAction == true)))
                {
                    DibujaControles.BtnGenDE(oOrderForm);
                    DibujaControles.BtnImp(oOrderForm);
                    DibujaControles.BtnEnvCo(oOrderForm);

                }


                if (pVal.ItemUID == "btnDocEle" & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED))
                {
                    if (oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE & oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    {
                        SAPbouiCOM.EditText txtFolioNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("211").Specific;

                        if (!string.IsNullOrEmpty(txtFolioNum.Value))
                        {
                            switch (oOrderForm.Type.ToString())
                            {

                                /*FATURA*/
                                case "133":
                                     FacturaController factura = new FacturaController(oOrderForm);
                                    string[] rpta = factura.respuestacdr;

                                    if (rpta[0] == "0")
                                    {
                                        SBO_Application.StatusBar.SetText(rpta[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                        //EnviaEmail();
                                    }
                                    else
                                    {
                                        SBO_Application.StatusBar.SetText(rpta[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }

                                    
                                    break;
                                /*NOTA CREDITO*/
                                case "179":
                                    //oDocType = "07";

                                    NotaCreditoController NotaCredito = new NotaCreditoController(oOrderForm);
                                    string[] rptanc = NotaCredito.respuestacdr;
                                    if (rptanc[0] == "0")
                                    {
                                        SBO_Application.StatusBar.SetText(rptanc[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                        EnviaEmail();
                                    }
                                    else
                                    {
                                        SBO_Application.StatusBar.SetText(rptanc[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                    break;

                                /*guia de remision*/
                                case "140":
                                    GuiaController guia = new GuiaController(oOrderForm);
                                    string[] rptagr = guia.respuestacdr;
                                    if (rptagr[0] == "0")
                                    {
                                        SBO_Application.StatusBar.SetText(rptagr[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                        // EnviaEmail();
                                    }
                                    else
                                    {
                                        SBO_Application.StatusBar.SetText(rptagr[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            SBO_Application.MessageBox("Generar Numero Corelativo");
                        }

                    }
                    else
                    {
                        SBO_Application.MessageBox("[Crear] ó [Actualizar] documento antes de enviar a sunat");
                    }


                }

                if (pVal.ItemUID == "btnImp" & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED))
                {
                    if (oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE & oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    {
                        SAPbouiCOM.EditText txtFolioNum = (SAPbouiCOM.EditText)oOrderForm.Items.Item("211").Specific;

                        if (!string.IsNullOrEmpty(txtFolioNum.Value))
                        {


                            switch (oOrderForm.Type.ToString())
                            {
                                /*FATURA*/
                                case "133":
                                    try
                                    {
                                        FormReporte formReporte = new FormReporte(oOrderForm);
                                        formReporte.ShowDialog();
                                    }
                                    catch (System.Exception ex)
                                    {

                                        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                   
                                    break;
                                //nota de crediro
                                case "179":
                                    try
                                    {
                                        FormReporte formReporte = new FormReporte(oOrderForm);
                                        formReporte.ShowDialog();
                                    }
                                    catch (System.Exception ex)
                                    {

                                        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                    break;
                                //guia de remision
                                case "140":

                                    try
                                    {
                                        FormReporte formReporte = new FormReporte(oOrderForm);
                                        formReporte.ShowDialog();
                                    }
                                    catch (System.Exception ex)
                                    {

                                        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                    break;

                            }
                        }
                        else
                        {
                            SBO_Application.MessageBox("Generar Numero Corelativo");
                        }

                    }
                    else
                    {
                        SBO_Application.MessageBox("[Crear] ó [Actualizar] documento antes de enviar a sunat");
                    }

                }

                if (pVal.ItemUID == "btnEnC" & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED))
                {
                    if (oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE & oOrderForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    {
                        EnviaEmail();
                    }
                    else
                    {
                        SBO_Application.MessageBox("[Crear] ó [Actualizar] documento antes de enviar a sunat");
                    }
                }
            }
        }

        private void ImprimeReportes()
        {
            //ReportController report = new ReportController(oOrderForm);
            //string[] rptaReport = report.respuestaReporte;

            //if (rptaReport[0] == "0")
            //{
            //    SBO_Application.StatusBar.SetText(rptaReport[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            //}
            //else
            //{
            //    SBO_Application.StatusBar.SetText(rptaReport[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            //}
        }
        private void EnviaEmail()
        {
            EmailController emailController = new EmailController(oOrderForm);
            string[] rptaEma = emailController.respuestaSendcorreo;
            if (rptaEma[0] == "0")
            {
                SBO_Application.StatusBar.SetText(rptaEma[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            else
            {
                SBO_Application.StatusBar.SetText(rptaEma[1], SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
        public Add_On()
        {
            setApplication();
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
    }
}
