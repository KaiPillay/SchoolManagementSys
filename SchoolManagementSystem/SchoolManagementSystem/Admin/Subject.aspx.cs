using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySqlConnector;
using static SchoolManagementSystem.Models.CommonFn;
using System.Configuration;

using System.Data.SqlClient;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySqlConnector;
using System.Configuration;
using SchoolManagementSystem.Models; // Ensure you are using the correct namespace

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySqlConnector;
using System.Configuration;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Admin
{
    public partial class Subject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();  // Fetch and bind class data to dropdown
                GetSubject(); // Fetch and display existing subjects
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
            string query = "SELECT * FROM subjects";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Log the full exception
                lblStatus.Text = $"An error occurred while binding the grid: {ex.Message}<br>{ex.StackTrace}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }


        private void GetClass()
        {
            try
            {
                DataTable dt = fn.Fetch("SELECT * FROM Class");
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassID";
                ddlClass.DataBind();

                ddlClass.Items.Insert(0, new ListItem("Select Class", "0"));
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the class list: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlClass.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please select a class.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                string classID = ddlClass.SelectedItem.Value;
                string subjectName = txtSubject.Text.Trim();

                if (string.IsNullOrEmpty(subjectName))
                {
                    lblStatus.Text = "Subject name is required.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT COUNT(*) FROM Subject WHERE ClassID = @ClassID AND SubjectName = @SubjectName";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@ClassID", classID);
                    selectCmd.Parameters.AddWithValue("@SubjectName", subjectName);

                    int subjectCount = Convert.ToInt32(selectCmd.ExecuteScalar());

                    if (subjectCount == 0)
                    {
                        string insertQuery = "INSERT INTO Subject (ClassID, SubjectName) VALUES (@ClassID, @SubjectName)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@ClassID", classID);
                        insertCmd.Parameters.AddWithValue("@SubjectName", subjectName);

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Subject added successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            GetSubject(); // Refresh the subject list
                        }
                        else
                        {
                            lblStatus.Text = "Failed to add the subject.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Subject already exists for this class.";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while adding the subject: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void GetSubject()
        {
            try
            {
                string query = "SELECT s.SubjectID, s.ClassID, c.ClassName, s.SubjectName " +
                               "FROM Subject s INNER JOIN Class c ON s.ClassID = c.ClassID";
                DataTable dt = fn.Fetch(query);

                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    lblStatus.Text = "No subjects available to display.";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An error occurred while loading the subjects: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetSubject();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int index = e.NewEditIndex;
            GridView1.EditIndex = index;
            BindGrid();
        }
    }
}




