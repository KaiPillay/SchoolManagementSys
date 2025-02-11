using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Teacher
{
    public partial class Subject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetSubjects();
            }
        }

        private void GetSubjects()
        {
            try
            {
                string query = "SELECT SubjectID, SubjectName FROM Subject";
                DataTable dt = fn.Fetch(query);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                lblStatus.Text = "Subject list loaded successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the subject list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
