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
    public partial class Perfil : System.Web.UI.Page
    {
        UsuarioServices service = new UsuarioServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Usuario usuario = getUser();

                    imgFotoPerfil.ImageUrl = usuario.FotoPerfil;
                    txtNombreUsuario.Text = usuario.NombreUsuario;
                    txtContrasena.Attributes["value"] = usuario.Contrasena;
                    lblRol.Text = usuario.Rol;


                    if (usuario.FotoPerfil != "/images/user.png")
                    {
                        btnEliminarFoto.Visible = true;
                    }

                    if (Request.QueryString["edit"] != null)
                    {
                        bool editMode = Convert.ToBoolean(Request.QueryString["edit"]);
                        btnControlAcceso.Visible = true;
                        if (!editMode)
                        {
                            txtNombreUsuario.Enabled = false;
                            txtContrasena.Enabled = false;
                            fuFotoPerfil.Visible = false;
                            btnEliminarFoto.Visible = false;
                            btnGuardarCambios.Visible = false;
                            btnControlAcceso.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                    Response.Redirect("Error.aspx", false);
                }
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario user = new Usuario();

                user.IdUsuario = Convert.ToInt32(getUser().IdUsuario);
                user.NombreUsuario = txtNombreUsuario.Text;
                user.Contrasena = txtContrasena.Text;
                user.Rol = getUser().Rol;

                Response.Write(txtContrasena.Attributes["value"].ToString());

                if (fuFotoPerfil.HasFile)
                {
                    string fileExtension = System.IO.Path.GetExtension(fuFotoPerfil.FileName);
                    string fileName = $"{user.IdUsuario}_{Guid.NewGuid()}{fileExtension}";
                    string filePath = Server.MapPath("~/images/") + fileName;

                    fuFotoPerfil.SaveAs(filePath);
                    user.FotoPerfil = $"~/images/{fileName}";
                }
                else
                {
                    user.FotoPerfil = imgFotoPerfil.ImageUrl;
                }

                service.modify(user);
                if (Request.QueryString["id"] != null)
                {
                    Response.Redirect("ControlAcceso.aspx", false);
                    return;
                }
                Session.Add("FotoPerfil", user.FotoPerfil);
                Response.Redirect("Perfil.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnEliminarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario user = service.getUser(Convert.ToInt32(Session["id"]));


                user.FotoPerfil = "/images/user.png";

                service.modify(user);
                if (Request.QueryString["id"] != null)
                {
                    Session.Add("FotoPerfil", user.FotoPerfil);
                }
                Response.Redirect("Perfil.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnIrAControlAcceso_Click(object sender, EventArgs e)
        {
            Response.Redirect("ControlAcceso.aspx", false);
        }

        protected Usuario getUser()
        {
            int Id = Request.QueryString["id"] != null? Convert.ToInt32(Request.QueryString["id"]) : Convert.ToInt32(Session["id"]);

            return service.getUser(Id);
        }
    }
}