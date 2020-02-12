using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration; //para accear al WebConfig
using System.Data.SqlClient;
using System.Data;

namespace CONTRATOS.Aplicacion
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnAIFDIA"].ConnectionString);

            try
            {
                con.Open();
                this.LabelInfo.Text = "Ok conexion a BD";



            }
            catch (SqlException ex)
            {
                this.LabelInfo.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con = null;
            }
        }
    }
}