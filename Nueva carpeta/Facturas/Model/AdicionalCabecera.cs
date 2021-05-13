using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Model
{
    public class AdicionalCabecera
    {
        public string ctaBancoNacionDetraccion { get; set; }
        public string codBienDetraccion { get; set; }
        public string porDetraccion { get; set; }
        public string mtoDetraccion { get; set; }
        public string codMedioPago { get; set; }
        public string codPaisCliente { get; set; }
        public string codUbigeoCliente { get; set; }
        public string desDireccionCliente { get; set; }
        public string codPaisEntrega { get; set; }
        public string codUbigeoEntrega { get; set; }
        public string desDireccionEntrega { get; set; }
    }
}
