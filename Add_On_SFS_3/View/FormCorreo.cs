using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Add_On_SFS_3.View
{
    public partial class FormCorreo : Form
    {
        public string oFromMail;
        public string oAsunto;
        public FormCorreo(string otoEmail, string oasunto)
        {
            InitializeComponent();
            textBoxDestCorr.Text = otoEmail;
            textBoxAsunCorr.Text = oasunto;
        }

        private void ButtonEnviarCorr_Click(object sender, EventArgs e)
        {
            oFromMail = textBoxDestCorr.Text;
            oAsunto = textBoxAsunCorr.Text;
        }
    }
}
