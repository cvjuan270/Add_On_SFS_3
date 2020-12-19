using Add_On_SFS_3.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_On_SFS_3.Controller
{
    public class DocElecController
    {
        private Documento DatosDocumeto(SAPbouiCOM.Form form) 
        {
            Documento oDocumento = null;

            DataSet ds = Conexion.Ejecutar_ds("");

            return oDocumento;
        }
    }
}
