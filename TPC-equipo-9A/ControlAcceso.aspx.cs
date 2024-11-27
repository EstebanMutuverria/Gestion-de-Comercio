using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class ControlAcceso : System.Web.UI.Page
    {
        UsuarioServices service = new UsuarioServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid(string filters = null)
        {
            try
            {
                List<Usuario> list = service.listar(filters);
                if (list != null)
                {
                    gvUsuarios.DataSource = list;
                    gvUsuarios.DataBind();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;

            string filters = ViewState["filters"] as string;

            BindGrid(filters);
        }


        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IdUsuario = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "Editar":
                        Response.Redirect($"Perfil.aspx?id={IdUsuario}&edit={true}", false);
                        break;

                    case "Eliminar":
                        if (service.getUser(IdUsuario).Rol == "Administrador")
                        {
                            int admins = service.countActiveAdmins();
                            if (admins == 1)
                            {
                                string script = "alert('No es posible Eliminar al único administrador de la plataforma.');";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                                return;
                            }
                        }
                        service.delete(IdUsuario);
                        Response.Redirect("ControlAcceso.aspx", false);
                        break;

                    case "VerPerfil":
                        Response.Redirect($"Perfil.aspx?id={IdUsuario}&edit={false}", false);
                        break;

                    case "Activar":
                        service.updateEstado(IdUsuario, true);
                        Response.Redirect("ControlAcceso.aspx", false);
                        break;

                    case "Desactivar":
                        if (service.getUser(IdUsuario).Rol == "Administrador")
                        {
                            int admins = service.countActiveAdmins();
                            if (admins == 1)
                            {
                                string script = "alert('No es posible Desactivar al único administrador de la plataforma.');";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                                return;
                            }
                        }
                        service.updateEstado(IdUsuario, false);

                        if (IdUsuario == Convert.ToInt32(Session["id"]))
                        {
                            Session.Abandon();
                            Response.Redirect("Login.aspx", false);
                            return;
                        }
                        Response.Redirect("ControlAcceso.aspx", false);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreUsuario = txtNombreUsuario.Text;
                string rolSeleccionado = ddlRol.SelectedValue.ToString();
                string estadoSeleccionado = ddlEstado.SelectedValue.ToString();

                string filters = "";

                if (!string.IsNullOrEmpty(nombreUsuario))
                {
                    filters += $"NombreUsuario LIKE '%{nombreUsuario}%'";
                }

                if (!string.IsNullOrEmpty(rolSeleccionado))
                {
                    if (!string.IsNullOrEmpty(filters))
                    {
                        filters += " and ";
                    }
                    filters += $"Rol = '{rolSeleccionado}'";
                }

                if (!string.IsNullOrEmpty(estadoSeleccionado))
                {
                    if (!string.IsNullOrEmpty(filters))
                    {
                        filters += " and ";
                    }
                    filters += $"Estado = '{estadoSeleccionado}'";
                }

                ViewState["filters"] = filters;

                BindGrid(filters);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AgregarUsuario.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
