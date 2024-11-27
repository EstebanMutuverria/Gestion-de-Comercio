using Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;
using Models;
using System.Globalization;

namespace TPC_equipo_9A
{
    public partial class FormProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CategoriaServices categoriaServices = new CategoriaServices();
                    List<Categoria> categorias = categoriaServices.listar().OrderBy(c => c.Nombre).ToList();

                    ddlCategoria.DataSource = categorias;
                    ddlCategoria.DataTextField = "Nombre"; // El nombre de la columna que quieres mostrar
                    ddlCategoria.DataValueField = "IdCategoria"; // El valor de la columna (ID)
                    ddlCategoria.DataBind();
                    // ítem de "placeholder" al DropDownList de Categoría
                    ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría", ""));

                    MarcaServices marcaServices = new MarcaServices();
                    List<Marca> marcas = marcaServices.listar().OrderBy(m => m.Nombre).ToList();

                    ddlMarca.DataSource = marcas;
                    ddlMarca.DataTextField = "Nombre"; // El nombre de la columna que quieres mostrar
                    ddlMarca.DataValueField = "IdMarca"; // El valor de la columna (ID)
                    ddlMarca.DataBind();
                    // ítem de "placeholder" al DropDownList de Marca
                    ddlMarca.Items.Insert(0, new ListItem("Seleccione una marca", ""));
                }

                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    ProductoServices services = new ProductoServices();
                    List<Producto> lista = services.listar(id);
                    Producto seleccionado = lista[0];

                    //Precargar los datos
                    txtIdProducto.Text = id;
                    txtNombre.Text = seleccionado.Nombre;
                    txtPorcentajeGanancia.Text = seleccionado.PorcentajeGanancia.ToString();
                    txtStockActual.Text = seleccionado.StockActual.ToString();
                    txtStockMinimo.Text = seleccionado.StockMinimo.ToString();
                    ddlCategoria.SelectedValue = seleccionado.Categoria.IdCategoria.ToString();
                    ddlMarca.SelectedValue = seleccionado.Marca.IdMarca.ToString();
                    txtFechaVencimiento.Text = seleccionado.FechaVencimiento?.ToString("yyyy-MM-dd") ?? "";
                    txtPrecio.Text = seleccionado.Precio.ToString();


                    lblOpcional.Visible = false;

                    //btnGuardar.OnClientClick = "return confirmarModificacion('" + id + "', '" + seleccionado.Nombre + "');";
                    btnEliminar.OnClientClick = "return confirmarEliminacion('" + id + "', '" + seleccionado.Nombre + "');";

                }
                else
                {
                    lblTitulo.Text = "Registrando Producto";

                    btnGuardar.Text = "Crear producto";

                    btnEliminar.Visible = false;
                    btnModificar.Visible = false;
                    btnGuardar.Visible = true;


                    //txtIdProducto.Text = "";
                    //txtNombre.Text = "";
                    //txtPorcentajeGanancia.Text = "";
                    //txtStockActual.Text = "";
                    //txtStockMinimo.Text = "";

                    btnVolver.Text = "Cancelar";

                    txtNombre.ReadOnly = false;
                    txtPorcentajeGanancia.ReadOnly = false;

                    txtIdProducto.Visible = false;
                    lblIdProducto.Visible = false;
                    txtStockActual.Visible = false;
                    lblStockActual.Visible = false;
                    txtBuscarCat.Enabled = true;
                    txtBuscarMar.Enabled = true;
                    txtBuscarCat.Visible = true;
                    txtBuscarMar.Visible = true;
                    txtPrecio.Visible = false;
                    lblPrecio.Visible = false;



                    txtStockMinimo.ReadOnly = false;
                    ddlCategoria.Enabled = true;
                    ddlMarca.Enabled = true;
                    txtFechaVencimiento.ReadOnly = false;


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
                btnGuardar.Text = "Guarda cambios";
                txtNombre.ReadOnly = false;
                txtPorcentajeGanancia.ReadOnly = false;
                txtStockActual.ReadOnly = false;
                txtStockMinimo.ReadOnly = false;
                ddlCategoria.Enabled = true;
                ddlMarca.Enabled = true;
                txtFechaVencimiento.ReadOnly = false;
                txtBuscarCat.Enabled = true;
                txtBuscarMar.Enabled = true;
                txtBuscarCat.Visible = true;
                txtBuscarMar.Visible = true;
                txtPrecio.Visible = false;
                lblPrecio.Visible = false;

                btnEliminar.Visible = false;
                btnModificar.Visible = false;
                btnGuardar.Visible = true;

                lblTitulo.Text = "Modificando Producto";
                lblOpcional.Visible = true;

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
                ProductoServices services = new ProductoServices();
                Producto nuevo = new Producto();
                nuevo.Nombre = txtNombre.Text;
                nuevo.Marca = new Marca();
                nuevo.Marca.IdMarca = Convert.ToInt32(ddlMarca.SelectedValue);
                nuevo.Categoria = new Categoria();
                nuevo.Categoria.IdCategoria = Convert.ToInt32(ddlCategoria.SelectedValue);
                nuevo.StockMinimo = Convert.ToInt32(txtStockMinimo.Text);
                nuevo.StockActual = Convert.ToInt32(0);
                nuevo.PorcentajeGanancia = Convert.ToDecimal(txtPorcentajeGanancia.Text);
                // Solo asigna FechaVencimiento si el campo no está vacío
                if (!string.IsNullOrWhiteSpace(txtFechaVencimiento.Text))
                {
                    nuevo.FechaVencimiento = Convert.ToDateTime(txtFechaVencimiento.Text);
                }
                else
                {
                    nuevo.FechaVencimiento = null; // Asigna null si el campo está vacío
                }
                nuevo.Precio = 0;


                // Si el ID del producto está presente, estamos modificando
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && Page.IsValid)
                {
                    nuevo.IdProducto = int.Parse(id);

                    string script = $"if(confirm('¿Estás seguro que deseas modificar el producto con ID: {txtIdProducto.Text} y Nombre: {txtNombre.Text}?')) {{ {ClientScript.GetPostBackEventReference(btnGuardar, null)}; }}";
                    ClientScript.RegisterStartupScript(this.GetType(), "ConfirmacionGuardar", script, true);

                    services.modify(nuevo);
                }
                else
                {
                    // Nuevo producto
                    services.add(nuevo);
                }

                Response.Redirect("Productos.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ProductoServices services = new ProductoServices();

            string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            bool withoutSells = services.verifySells(int.Parse(id));
            bool withoutBuys = services.verifyBuys(int.Parse(id));

            if(withoutBuys && withoutSells)
            {
            services.delete(id);
            Response.Redirect("Productos.aspx", false);
            }
            else
            {
                string script = "alert('No fue posible eliminar el producto. Este tiene operaciones asociadas.'); window.location.href = 'Productos.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
                return;
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Productos.aspx", false);
        }

        protected void txtBuscarMar_TextChanged(object sender, EventArgs e)
        {
            FiltroDropDownList(ddlMarca, txtBuscarMar.Text, "Marca");
        }

        protected void txtBuscarCat_TextChanged(object sender, EventArgs e)
        {
            FiltroDropDownList(ddlCategoria, txtBuscarCat.Text, "Categoria");
        }

        private void FiltroDropDownList(DropDownList ddl, string filterText, string filtro)
        {
            lblNoEncontradoCat.Visible = false;
            lblNoEncontradoMar.Visible = false;
            bool encontrado = false;
            var textoBuscadoNormalizado = RemoveAccents(filterText.ToLower().Replace(" ", ""));
            foreach (ListItem item in ddl.Items)
            {
                var textoItemNormalizado = RemoveAccents(item.Text.ToLower().Replace(" ", ""));
                item.Enabled = textoItemNormalizado.Contains(textoBuscadoNormalizado);
                if (item.Enabled)
                {
                    encontrado = true;
                }
            }


            if (!encontrado)
            {
                if (filtro == "Marca")
                {

                    lblNoEncontradoMar.Visible = true;
                }
                else
                {

                    lblNoEncontradoCat.Visible = true;
                }
            }


        }

        public static string RemoveAccents(string text)
        {
            var withAccents = "áéíóúüñ";
            var withoutAccents = "aeiounn";

            for (int i = 0; i < withAccents.Length; i++)
            {
                text = text.Replace(withAccents[i], withoutAccents[i]);
            }

            return text;
        }
                
    }
}
