using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Parents
{
    public partial class ParentMst : System.Web.UI.MasterPage
    {
        // Page_Load event handler
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Any logic required on initial page load
            }
        }

        // btnLogOut_Click event handler
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            // Clear session and logout user
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}