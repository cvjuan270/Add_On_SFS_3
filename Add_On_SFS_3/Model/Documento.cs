using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_On_SFS_3.Model
{
    public class Documento
    {
        public int DocEntry { get; set; }
        public string TipDoc { get; set; }
        public string Serie { get; set; }
        public int NumCor { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public DateTime TaxDate { get; set; }
        public float DocTotal { get; set; }
        

    }
}
