using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem.Teacher
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStudentGrid();
                BindClasses();
            }
        }

        private void BindStudentGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT s.StudentID, s.Name, s.DOB, s.Gender, s.Mobile, s.Address, c.ClassName FROM student s INNER JOIN class c ON s.ClassID = c.ClassID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                gvStudents.DataSource = cmd.ExecuteReader();
                gvStudents.DataBind();
            }
        }

        private void BindClasses()
        {
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT ClassID, ClassName FROM class";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                ddlClass.DataSource = cmd.ExecuteReader();
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassID";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0, new ListItem("Select Class", ""));
            }
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gvStudents.Rows[index];

                // Populate the details into the form below
                txtName.Text = selectedRow.Cells[1].Text;  // Name column
                txtDOB.Text = selectedRow.Cells[2].Text;   // DOB column
                ddlGender.SelectedValue = selectedRow.Cells[3].Text;  // Gender column
                txtMobile.Text = selectedRow.Cells[4].Text; // Mobile column
                txtAddress.Text = selectedRow.Cells[5].Text; // Address column
                ddlClass.SelectedValue = selectedRow.Cells[6].Text;  // Class column
            }
        }
    }
}
