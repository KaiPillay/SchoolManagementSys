using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.UI.WebControls;


namespace SchoolManagementSystem.Admin
{
    public partial class EditStudent : System.Web.UI.Page
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

        protected void gvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvStudents.SelectedRow;
            txtName.Text = row.Cells[1].Text; // Name is in the 2nd column (index 1)
            txtDOB.Text = row.Cells[2].Text;  // DOB is in the 3rd column (index 2)
            ddlGender.SelectedValue = row.Cells[3].Text;  // Gender is in the 4th column (index 3)
            txtMobile.Text = row.Cells[4].Text;  // Mobile is in the 5th column (index 4)
            txtAddress.Text = row.Cells[5].Text;  // Address is in the 6th column (index 5)
            ddlClass.SelectedItem.Text = row.Cells[6].Text;  // Class is in the 7th column (index 6)
            ViewState["StudentID"] = row.Cells[0].Text;  // StudentID is in the 1st column (index 0)
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string studentID = ViewState["StudentID"].ToString();
            string name = txtName.Text.Trim();
            string gender = ddlGender.SelectedValue;
            string mobile = txtMobile.Text.Trim();
            string address = txtAddress.Text.Trim();
            string classID = ddlClass.SelectedValue;

            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "UPDATE student SET Name=@Name, Gender=@Gender, Mobile=@Mobile, Address=@Address, ClassID=@ClassID WHERE StudentID=@StudentID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ClassID", classID);
                conn.Open();
                cmd.ExecuteNonQuery();
                lblStatus.Text = "Student updated successfully.";
                BindStudentGrid();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string studentID = ViewState["StudentID"].ToString();
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "DELETE FROM student WHERE StudentID=@StudentID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                conn.Open();
                cmd.ExecuteNonQuery();
                lblStatus.Text = "Student deleted successfully.";
                BindStudentGrid();
            }
        }
    }
}
