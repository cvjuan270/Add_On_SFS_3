using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Model
{
    public class RootGR
    {
        public CabeceraGR cabecera { get; set; }
        public List<DetalleGR> detalle { get; set; }
    }
}
