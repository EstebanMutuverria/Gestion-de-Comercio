using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Services;
using Models;

namespace TPC_equipo_9A
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductoServices services = new ProductoServices();
                List<Producto> listaProductos = services.listar();
                Session["listaProductos"] = listaProductos;
                dgvProductos.DataSource = listaProductos;
                dgvProductos.DataBind();
            }
        }


        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = dgvProductos.SelectedDataKey.Value.ToString();
            Response.Redirect("FormProducto.aspx?id=" + id);
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormProducto.aspx");
        }

        protected void dgvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvProductos.PageIndex = e.NewPageIndex;

            List<Producto> listaProductos = (List<Producto>)Session["listaProductos"];
            dgvProductos.DataSource = listaProductos;
            dgvProductos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Producto> lista = (List<Producto>)Session["listaProductos"];
            List<Producto> listaFiltrada = lista.FindAll(x => RemoveAccents(x.Nombre.ToLower().Replace(" ", "")).Contains(txtBuscar.Text.ToLower().Replace(" ","")) 
            || x.Nombre.ToLower().Replace(" ", "").Contains(txtBuscar.Text.ToLower().Replace(" ","")) 
            || RemoveAccents(x.Categoria.Nombre.ToLower().Replace(" ","")).Contains(txtBuscar.Text.ToLower().Replace(" ", "")) 
            || x.Categoria.Nombre.ToLower().Replace(" ","").Contains(txtBuscar.Text.ToLower().Replace(" ", "")) 
            || RemoveAccents(x.Marca.Nombre.ToLower().Replace(" ","")).Contains(txtBuscar.Text.ToLower().Replace(" ", "")) 
            || x.Marca.Nombre.ToLower().Replace(" ","").Contains(txtBuscar.Text.ToLower().Replace(" ", "")));
            dgvProductos.DataSource = listaFiltrada;
            dgvProductos.DataBind();
        }

        public static string RemoveAccents(string text)
        {
            var withAccents = "áéíóúüñ";
            var withoutAccents = "aeiouun"; // Debe incluir 'u' para la ü.

            for (int i = 0; i < withAccents.Length; i++)
            {
                text = text.Replace(withAccents[i], withoutAccents[i]);
            }

            return text;
        }
    }
}