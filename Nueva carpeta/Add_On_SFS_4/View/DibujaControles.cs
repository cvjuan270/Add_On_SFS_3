using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.View
{
    public class DibujaControles
    {
        private static SAPbouiCOM.Item oItem;
        private static SAPbouiCOM.Button oButton;
        private static SAPbouiCOM.StaticText oStaticText;
        private static SAPbouiCOM.ComboBox oComboBox;


        public static void cbTipNotCre(SAPbouiCOM.Form oForm)
        {
            // oForm.DataSources.UserDataSources.Add("TipoNC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 20);

            oItem = oForm.Items.Add("TipoNC", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
            oItem.Top = 98;
            oItem.Height = 20;
            oItem.Width = 70;
            oItem.Left = 490;
            oComboBox = ((SAPbouiCOM.ComboBox)(oItem.Specific));
            //oComboBox.DataBind.SetBound(true,"", "TipoNC");
            oComboBox.ValidValues.Add("01", "Anulación de la operación");
            oComboBox.ValidValues.Add("02", "Anulación por error en el RUC");
            oComboBox.ValidValues.Add("03", "Corrección por error en la descripción");
            oComboBox.ValidValues.Add("04", "Descuento global");
            oComboBox.ValidValues.Add("05", "Descuento po item");
            oComboBox.ValidValues.Add("06", "Devolución total");
            oComboBox.ValidValues.Add("07", "Devolución por ítem");
            oComboBox.ValidValues.Add("08", "Bonificación");
            oComboBox.ValidValues.Add("09", "Disminución en el valor");
            oComboBox.ValidValues.Add("10", "Otros Conceptos ");
            oComboBox.ValidValues.Add("11", "Ajustes de operaciones de exportación");
            oComboBox.ValidValues.Add("12", "Ajustes afectos al IVAP");

            // oComboBox.Select("01");
        }
        public static void BtnGenDE(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnDocEle", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 98;
            oItem.Height = 20;
            oItem.Width = 80;
            oItem.Left = 310;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "SUNAT";
        }

        public static void BtnImp(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnImp", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 98;
            oItem.Height = 20;
            oItem.Width = 80;
            oItem.Left = 400;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Inprimir";
        }

        public static void BtnEnvCo(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnEnC", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 98;
            oItem.Height = 20;
            oItem.Width = 70;
            oItem.Left = 490;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "E-mail";
        }

        public static void BtnAcDir(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnAcDir", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 370;
            oItem.Height = 20;
            oItem.Width = 80;
            oItem.Left = 290;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Actualza direccion";

            oItem.FromPane = 7;
            oItem.ToPane = 7;
        }

        public static void stxtDire(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("stxtDire", SAPbouiCOM.BoFormItemTypes.it_STATIC);
            oItem.Top = 300;
            oItem.Height = 20;
            oItem.Width = 200;
            oItem.Left = 290;
            oItem.FontSize = 14;
            oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
            oStaticText.Caption = "";
            oItem.FromPane = 7;
            oItem.ToPane = 7;
        }

        public static void BtnAddSN(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnAddSN", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 78;
            oItem.Height = 20;
            oItem.Width = 100;
            oItem.Left = 300;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Crear Modo Rapido";
        }

        public static void BtnCoRuc(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnCRu", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 55;
            oItem.Height = 20;
            oItem.Width = 80;
            oItem.Left = 300;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Consulta RUC";

        }
    }
}
