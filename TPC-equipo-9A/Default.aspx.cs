using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener el nombre del usuario de la sesión
                string NombreUsuario = Session["NombreUsuario"] != null ? Session["NombreUsuario"].ToString() : "Usuario";
                litBienvenida.Text = "Hola, " + NombreUsuario + "!";
            }
        }
    }
}