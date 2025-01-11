using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class ClassFees : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                GetFees();
            }
        }

        private void GetClass()
        {
            // Fetching classes for the dropdown
            DataTable dt = fn.Fetch("SELECT * FROM Class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("Select Class", "0"));
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Handling GridView page change
            GridView1.PageIndex = e.NewPageIndex;
            GetFees();
        }

        private void GetFees()
        {
            try
            {
                // Fetching fees data
                DataTable dt = fn.Fetch("SELECT f.FeesId, IFNULL(c.ClassName, 'No Class') AS ClassName, f.FeesAmount FROM Fees f LEFT JOIN Class c ON c.ClassId = f.ClassId");

                if (dt.Rows.Count == 0)
                {
                    lblMsg.Text = "No records found.";
                    lblMsg.CssClass = "alert alert-warning";
                }
                else
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    lblMsg.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validating inputs
                if (ddlClass.SelectedValue == "0" || string.IsNullOrWhiteSpace(txtFeeAmount.Text))
                {
                    lblMsg.Text = "Please select a class and enter a fee amount.";
                    lblMsg.CssClass = "alert alert-warning";
                    return;
                }

                // Inserting fee information into the database
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Fees WHERE ClassId = @ClassId";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@ClassId", ddlClass.SelectedValue);

                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (exists == 0)
                    {
                        string insertQuery = "INSERT INTO Fees (ClassId, FeesAmount) VALUES (@ClassId, @FeesAmount)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@ClassId", ddlClass.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@FeesAmount", txtFeeAmount.Text.Trim());
                        insertCmd.ExecuteNonQuery();

                        lblMsg.Text = "Fee added successfully.";
                        lblMsg.CssClass = "alert alert-success";
                        GetFees();
                    }
                    else
                    {
                        lblMsg.Text = "Fee already exists for this class.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }

                ddlClass.SelectedIndex = 0;
                txtFeeAmount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the GridView to editing mode
            GridView1.EditIndex = e.NewEditIndex;
            GetFees();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the FeeId and FeeAmount of the edited row
            int feeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string updatedFeeAmount = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0]).Text;

            try
            {
                // Update the fee in the database
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Fees SET FeesAmount = @FeesAmount WHERE FeesId = @FeesId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@FeesId", feeId);
                    updateCmd.Parameters.AddWithValue("@FeesAmount", updatedFeeAmount);
                    updateCmd.ExecuteNonQuery();

                    lblMsg.Text = "Fee updated successfully.";
                    lblMsg.CssClass = "alert alert-success";
                    GridView1.EditIndex = -1;
                    GetFees();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel the edit operation
            GridView1.EditIndex = -1;
            GetFees();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Get the FeesId of the record to be deleted
                int feeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

                // Deleting the fee record from the database
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Fees WHERE FeesId = @FeesId";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@FeesId", feeId);
                    deleteCmd.ExecuteNonQuery();

                    lblMsg.Text = "Fee deleted successfully.";
                    lblMsg.CssClass = "alert alert-success";
                    GetFees();  // Refresh the GridView after deletion
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

    }

}
