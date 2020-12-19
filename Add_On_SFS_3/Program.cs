using Add_On_SFS_3.Controller;
using System;
using System.Windows.Forms;

namespace Add_On_SFS_3
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Add_On add_On = null;
            add_On = new Add_On();
            Application.Run();
        }
    }
}
