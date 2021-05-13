using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Model
{
    public class RootFT
    {
        public Cabecera cabecera { get; set; }
        public  List<AdicionalCabecera> adicionalCabecera { get; set; }
        public List<Detalle> detalle { get; set; }
        public List<Leyenda> leyendas { get; set; }
        //public List<AdicionalDetalle> adicionalDetalle { get; set; }
        public List<Tributo> tributos { get; set; }
    }
}
