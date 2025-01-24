using System;
using System.Data;
using System.Web.UI;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherHome : Page
    {
        private readonly CommonFn.Commonfnx commonFn = new CommonFn.Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAttendanceData();
            }
        }

        private void LoadAttendanceData()
        {
            try
            {
                string query = "SELECT * FROM Attendance";
                DataTable dt = commonFn.Fetch(query);  // Fetch data using CommonFn

                // Ensure GridView is correctly found and bind data
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridViewAttendance2.DataSource = dt; // Correct control name
                    GridViewAttendance2.DataBind();
                }
                else
                {
                    // Handle no records found
                    lblMessage.Text = "No attendance records found.";  // Correct label name
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error loading attendance data: {ex.Message}";
            }
        }
    }
}
