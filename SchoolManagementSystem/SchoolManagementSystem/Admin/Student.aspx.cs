using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Admin
{
    public partial class Student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Call method to bind classes to dropdown
                BindClasses();
            }
        }

        private void BindClasses()
        {
            // Set up the connection string (replace this with your actual connection string)
            string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // SQL query to get all classes
                    string query = "SELECT ClassID, ClassName FROM class";

                    // Open connection
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Bind data to the dropdown list
                    ddlClass.DataSource = reader;
                    ddlClass.DataTextField = "ClassName"; // Display the class name
                    ddlClass.DataValueField = "ClassID"; // Use ClassID as the value
                    ddlClass.DataBind();

                    // Add a default "Select Class" option
                    ddlClass.Items.Insert(0, new ListItem("Select Class", ""));
                }
                catch (Exception ex)
                {
                    // Handle any errors (optional logging)
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        // Handle other actions like adding, updating, or deleting students
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch data from the form
                string name = txtName.Text.Trim();
                string dob = txtDOB.Text.Trim();
                string gender = ddlGender.SelectedValue;
                string mobile = txtMobile.Text.Trim();
                string address = txtAddress.Text.Trim();
                string classID = ddlClass.SelectedValue; // Selected class

                // Ensure that a class is selected
                if (string.IsNullOrEmpty(classID))
                {
                    lblStatus.Text = "Please select a class.";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    return;
                }

                // Database insertion logic here
                string connString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    string query = "INSERT INTO student (Name, DOB, Gender, Mobile, Address, ClassID) VALUES (@Name, @DOB, @Gender, @Mobile, @Address, @ClassID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@ClassID", classID);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblStatus.Text = "Student added successfully.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to add student.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        // Other methods (e.g., for handling GridView actions like deleting, editing, etc.) can be added below
    }
}
