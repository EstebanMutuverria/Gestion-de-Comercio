using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class AgregarUsuario : System.Web.UI.Page
    {
        UsuarioServices service = new UsuarioServices();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreUsuario = txtNombreUsuario.Text.Trim();
                string contrasena = txtContrasena.Text.Trim();
                string rol = ddlRol.SelectedValue;

                Usuario newUsuario = new Usuario();
                newUsuario.NombreUsuario = txtNombreUsuario.Text;
                newUsuario.Contrasena = txtContrasena.Text;
                newUsuario.Rol = rol.ToString();

                if(Page.IsValid)
                {
                    service.add(newUsuario);
                    Response.Redirect("ControlAcceso.aspx", false);
                }

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }


        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ControlAcceso.aspx", false);
                return;
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void ValidateUsername(object source, ServerValidateEventArgs args)
        {
            string username = args.Value;
            args.IsValid = service.UserNameAvailable(username.ToString());
        }

        protected void ValidatePasswords(object source, ServerValidateEventArgs args)
        {
            args.IsValid = txtContrasena.Text == txtConfirmarContrasena.Text;
        }

    }
}