using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherMst : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Any logic required on initial page load
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            // Clear session and logout user
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
