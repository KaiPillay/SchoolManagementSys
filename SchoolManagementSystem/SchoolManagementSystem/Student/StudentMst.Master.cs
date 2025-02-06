using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Student
{
    public partial class StudentMst : MasterPage
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
