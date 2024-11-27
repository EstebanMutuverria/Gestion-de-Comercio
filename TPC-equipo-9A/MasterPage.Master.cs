using Models;
using Services;
using Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        UsuarioServices service = new UsuarioServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Session["rol"] != null)
                    {
                        string userRole = Session["rol"].ToString();
                        int userId = Convert.ToInt32(Session["id"]);
                        bool isEditing = Request.QueryString["edit"] != null;
                        bool isInProfile = Request.QueryString["id"] != null;

                        if (Page is AgregarUsuario || (Page is Perfil && (isEditing || isInProfile)))
                        {
                            if (!Utils.Security.HaveAccess(userRole))
                            {
                                Response.Redirect("Default.aspx", false);
                            }
                        }
                        

                        if (!string.IsNullOrEmpty(userRole))
                        {
                            liPerfil.Visible = true;

                            if (userRole == "Administrador")
                            {
                                liControlAcceso.Visible = true;
                                liInicio.Visible = true;
                                liRelacionesComerciales.Visible = true;
                                liInventario.Visible = true;
                                liOperaciones.Visible = true;
                            }
                            else if (userRole == "Vendedor")
                            {
                                liInicio.Visible = true;
                                liRelacionesComerciales.Visible = true;
                                liInventario.Visible = true;
                                liOperaciones.Visible = true;
                            }

                            imgUser.ImageUrl = "/images/user.png";
                            Usuario user = service.getUser(userId);
                            if (user != null)
                            {
                                imgUser.ImageUrl = user.FotoPerfil.ToString();
                            }

                        }
                        else
                        {
                            Session.Add("error", "Rol no especificado");
                            Response.Redirect("Error.aspx", false);
                        }
                    }
                    else
                    {
                        if(!(Page is Login))
                        {
                            Response.Redirect("Login.aspx", false);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

           protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }




    }
}