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
    public partial class FormMarca : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack && Request.Form[btnEliminar.UniqueID] != null) 
                {
                    return;
                }

                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    MarcaServices services = new MarcaServices();
                    List<Marca> lista = services.listar(id);
                    Marca seleccionado = lista[0];

                    //Precargar los datos
                    txtIdMarca.Text = id;
                    txtNombreMarca.Text = seleccionado.Nombre;

                    //Pregunta de confirmacion al eliminar
                    btnEliminar.OnClientClick = "return confirmarEliminacion('" + id + "', '" + seleccionado.Nombre + "');";

                }
                else
                {
                    btnGuardar.Text = "Crear marca";
                    btnVolver.Text = "Cancelar";

                    btnEliminar.Visible = false;
                    btnModificar.Visible = false;
                    btnGuardar.Visible = true;

                    lblIdMarca.Visible = false;
                    txtIdMarca.Visible = false;

                    txtNombreMarca.ReadOnly = false;
                    lblTitulo.Text = "Registrando marca";
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

                lblIdMarca.Visible = false;
                txtIdMarca.Visible = false;

                txtNombreMarca.ReadOnly = false;

                lblTitulo.Text = "Modificando Marca";
            }
            catch (Exception ex)
            {

                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                MarcaServices services = new MarcaServices();
                Marca nuevo = new Marca();
                nuevo.Nombre = txtNombreMarca.Text;
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && Page.IsValid)
                {
                    nuevo.IdMarca = int.Parse(txtIdMarca.Text);

                    string script = $"if(confirm('¿Estás seguro que deseas modificar la marca con ID: {txtIdMarca.Text} y Nombre: {txtNombreMarca.Text}?')) {{ {ClientScript.GetPostBackEventReference(btnGuardar, null)}; }}";
                    ClientScript.RegisterStartupScript(this.GetType(), "ConfirmacionGuardar", script, true);

                    services.modify(nuevo);
                }
                else
                {
                    // Nuevo producto
                    services.add(nuevo);
                }
                Response.Redirect("Marcas.aspx", false);

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                MarcaServices services = new MarcaServices();
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

                // Intentar eliminar la categoría
                services.delete(id);

                // Redirigir solo si no hubo errores
                Response.Redirect("Marcas.aspx", false);
            }
            catch (SqlException ex)
            {
                // Si hay una excepción relacionada con la clave referenciada
                if (ex.Message.Contains("No se puede eliminar la marca porque está referenciada"))
                {
                    lblError.Text = "No se puede eliminar la marca porque está referenciada por productos.";
                    lblError.Visible = true;
                }
                else
                {
                    // Otros posibles errores
                    lblError.Text = "Ocurrió un error al intentar eliminar la marca porque esta referenciada a productos.";
                    lblError.Visible = true;
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Marcas.aspx", false);
        }
    }
}