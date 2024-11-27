using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Security
    {

        public static bool HaveAccess(string rol)
        {
            if (rol != "Administrador")
            {
                return false;
            }
            return true;
        }
    }
}
