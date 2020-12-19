using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Add_On_SFS_3.View
{
    public partial class FormImprimir : Form
    {
        public string ImpresoraName;
        public int NumCopias;
        public FormImprimir()
        {
            InitializeComponent();

            string Impresoras;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                Impresoras = PrinterSettings.InstalledPrinters[i];
                comboBox1.Items.Add(Impresoras);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex!=-1)
            {
                ImpresoraName = comboBox1.Text;
               // NumCopias = (int)numericUpDown1.Value;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
