using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class AdminMst : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            // Clear any session or authentication data (if needed)
            Session.Clear();  // Clears session data
            Session.Abandon();  // Optionally abandon the session

            // Redirect to Login.aspx
            Response.Redirect("~/Login.aspx");
        }



    }
}