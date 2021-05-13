using Add_On_SFS_4.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Add_On_SFS_4
{
    static class Program
    {
        public static void Main()
        {
            Add_On oAdd_On = null;
            oAdd_On = new Add_On();
            Application.Run();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
