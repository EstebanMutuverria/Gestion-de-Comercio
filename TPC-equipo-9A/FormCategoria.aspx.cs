using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class FromCategoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar si el botón de eliminar fue el disparador de la carga de página
                if (IsPostBack && Request.Form[btnEliminar.UniqueID] != null)
                {
                    // No habilitar los campos si se trata de una eliminación
                    return;
                }

                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    CategoriaServices services = new CategoriaServices();
                    List<Categoria> lista = services.listar(id);
                    Categoria seleccionado = lista[0];

                    //Precargar los datos
                    txtIdCategoria.Text = id;
                    txtNombreCategoria.Text = seleccionado.Nombre;

                    btnEliminar.OnClientClick = "return confirmarEliminacion('" + id + "', '" + seleccionado.Nombre + "');";
                }
                else
                {
                    btnGuardar.Text = "Crear categoría";
                    btnVolver.Text = "Cancelar";
                    btnEliminar.Visible = false;
                    btnModificar.Visible = false;
                    btnGuardar.Visible = true;

                    lblIdCategoria.Visible = false;
                    txtIdCategoria.Visible = false;

                    txtNombreCategoria.ReadOnly = false;
                    lblTitulo.Text = "Registrando categoría";
                }
            }
            catch (Exception ex)
            {

                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
       
                btnVolver.Text = "Cancelar";

                btnEliminar.Visible = false;
                btnModificar.Visible = false;
                btnGuardar.Visible = true;

                lblIdCategoria.Visible = false;
                txtIdCategoria.Visible = false;

                txtNombreCategoria.ReadOnly = false;

                lblTitulo.Text = "Modificando Categoría";

            }
            catch (Exception ex)
            {

                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            CategoriaServices services = new CategoriaServices();
            Categoria nuevo = new Categoria();
            nuevo.Nombre = txtNombreCategoria.Text;

            string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
            if (id != "" && Page.IsValid)
            {
                nuevo.IdCategoria = int.Parse(txtIdCategoria.Text);

                string script = $"if(confirm('¿Estás seguro que deseas modificar la categoría con ID: {txtIdCategoria.Text} y Nombre: {txtNombreCategoria.Text}?')) {{ {ClientScript.GetPostBackEventReference(btnGuardar, null)}; }}";
                ClientScript.RegisterStartupScript(this.GetType(), "ConfirmacionGuardar", script, true);

                services.modify(nuevo);
            }
            else
            {
                // Nuevo producto
                services.add(nuevo);
            }
            Response.Redirect("Categorias.aspx", false);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                
                CategoriaServices services = new CategoriaServices();
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

                // Intentar eliminar la categoría
                services.delete(id);


                // Redirigir solo si no hubo errores
                Response.Redirect("Categorias.aspx", false);
            }
            catch (SqlException ex)
            {
                // Si hay una excepción relacionada con la clave referenciada
                if (ex.Message.Contains("No se puede eliminar la categoría porque está referenciada"))
                {
                    lblError.Text = "No se puede eliminar la categoría porque está referenciada por productos.";
                    lblError.Visible = true;
                }
                else
                {
                    // Otros posibles errores
                    lblError.Text = "Ocurrió un error al intentar eliminar la categoría porque esta referenciada a productos.";
                    lblError.Visible = true;
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Categorias.aspx", false);
        }
    }
}