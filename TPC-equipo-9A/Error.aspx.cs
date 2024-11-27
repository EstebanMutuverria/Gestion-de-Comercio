using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_9A
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             string errorMessage = Session["error"].ToString();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                lblErrorMessage.Text = "Detalles del error: " + errorMessage;
                lblErrorMessage.Visible = true;
            }
        }

        protected void btnBackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}