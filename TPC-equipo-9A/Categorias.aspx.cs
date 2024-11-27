using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using System.Globalization;
using System.Text;

namespace TPC_equipo_9A
{
    public partial class Categorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoriaServices services = new CategoriaServices();
                Session.Add("listaCategorias", services.listar());
                dgvCategoria.DataSource = Session["listaCategorias"];
                dgvCategoria.DataBind();
            }
        }

        protected void dgvCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = dgvCategoria.SelectedDataKey.Value.ToString();
            Response.Redirect("FormCategoria.aspx?id=" + id);
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormCategoria.aspx", false);
        }

        protected void dgvCategoria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCategoria.PageIndex = e.NewPageIndex;
            CategoriaServices services = new CategoriaServices();
            dgvCategoria.DataSource = services.listar();
            dgvCategoria.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Categoria> lista = (List<Categoria>)Session["listaCategorias"];
            List<Categoria> listaFiltrada = lista.FindAll(x => RemoveAccents(x.Nombre.ToLower()).Contains(txtBuscar.Text.ToLower()) || x.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));
            dgvCategoria.DataSource = listaFiltrada;
            dgvCategoria.DataBind();
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