using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Services;

namespace SchoolManagementSystem.Teacher
{
    public partial class TeacherChat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure only teachers are allowed to access this page
            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "Teacher")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        // Fetch teacher's name based on logged-in UserID
        [WebMethod]
        public static string GetTeacherName()
        {
            string teacherName = string.Empty;

            try
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return "Session expired. Please log in again.";
                }

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                string query = @"
                    SELECT t.Name 
                    FROM teacher t
                    INNER JOIN users u ON u.Id = t.UserId
                    WHERE u.Id = @UserID AND u.Role = 'Teacher'";

                DataTable dt = FetchData(query, new MySqlParameter("@UserID", userId));

                if (dt.Rows.Count > 0)
                {
                    teacherName = dt.Rows[0]["Name"].ToString();
                }
                else
                {
                    teacherName = "Teacher not found";
                }
            }
            catch (Exception ex)
            {
                teacherName = "Error: " + ex.Message;
                LogError("GetTeacherName", ex);
            }

            return teacherName;
        }

        // Fetch all users with their roles for populating the dropdown
        [WebMethod]
        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return users; // Empty list if no session
                }

                int currentUserId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                // Get all users except the current one
                string query = @"
                    SELECT Id, Username AS Name, Role 
                    FROM users 
                    WHERE Role IN ('Teacher', 'Student', 'Parent') 
                    AND Id != @CurrentUserId";

                DataTable dt = FetchData(query, new MySqlParameter("@CurrentUserId", currentUserId));

                foreach (DataRow row in dt.Rows)
                {
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        Role = row["Role"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetUsers", ex);
            }

            return users;
        }

        // Fetch previous messages between the teacher and the selected recipient
        [WebMethod]
        public static List<ChatMessage> GetPreviousMessages(int receiverId)
        {
            List<ChatMessage> messages = new List<ChatMessage>();

            try
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return messages; // Empty list if no session
                }

                int senderId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                string query = @"
                    SELECT 
                        m.Id, 
                        m.SenderId, 
                        CASE 
                            WHEN m.SenderId = @CurrentUserId THEN (SELECT t.Name FROM teacher t WHERE t.UserId = @CurrentUserId)
                            ELSE (SELECT Username FROM users WHERE Id = m.SenderId) 
                        END AS SenderName,
                        m.ReceiverId, 
                        m.MessageText, 
                        m.Timestamp,
                        CASE WHEN m.SenderId = @CurrentUserId THEN 1 ELSE 0 END AS IsSentByMe
                    FROM 
                        messages m
                    WHERE 
                        (m.SenderId = @CurrentUserId AND m.ReceiverId = @ReceiverId)
                        OR (m.SenderId = @ReceiverId AND m.ReceiverId = @CurrentUserId)
                    ORDER BY 
                        m.Timestamp ASC";

                DataTable dt = FetchData(query,
                    new MySqlParameter("@CurrentUserId", senderId),
                    new MySqlParameter("@ReceiverId", receiverId));

                foreach (DataRow row in dt.Rows)
                {
                    messages.Add(new ChatMessage
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        SenderId = Convert.ToInt32(row["SenderId"]),
                        SenderName = row["SenderName"].ToString(),
                        ReceiverId = Convert.ToInt32(row["ReceiverId"]),
                        MessageText = row["MessageText"].ToString(),
                        Timestamp = Convert.ToDateTime(row["Timestamp"]),
                        IsSentByMe = Convert.ToBoolean(row["IsSentByMe"])
                    });
                }

                // Mark messages as read if they were sent to current user
                if (messages.Count > 0)
                {
                    MarkMessagesAsRead(senderId, receiverId);
                }
            }
            catch (Exception ex)
            {
                LogError("GetPreviousMessages", ex);
            }

            return messages;
        }

        // Save message to the database
        [WebMethod]
        public static MessageResult SaveMessage(int receiverId, string message)
        {
            try
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return new MessageResult { Success = false, ErrorMessage = "Session expired. Please log in again." };
                }

                int senderId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                int chatRoomId = 1; // Default chat room

                string query = @"
                    INSERT INTO messages (SenderId, ReceiverId, ChatRoomId, MessageText, Timestamp, IsRead)
                    VALUES (@SenderId, @ReceiverId, @ChatRoomId, @Message, NOW(), 0);
                    SELECT LAST_INSERT_ID() AS InsertedId;";

                int insertedId = 0;

                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString))
                {
                    connection.Open(); // Explicitly open the connection

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SenderId", senderId);
                        cmd.Parameters.AddWithValue("@ReceiverId", receiverId);
                        cmd.Parameters.AddWithValue("@ChatRoomId", chatRoomId);
                        cmd.Parameters.AddWithValue("@Message", message);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            insertedId = Convert.ToInt32(result);
                        }
                    }
                }

                if (insertedId > 0)
                {
                    return new MessageResult { Success = true, MessageId = insertedId, SenderId = senderId };
                }
                else
                {
                    return new MessageResult { Success = false, ErrorMessage = "Failed to insert message into the database." };
                }
            }
            catch (Exception ex)
            {
                LogError("SaveMessage", ex);
                return new MessageResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        // Mark messages as read
        private static void MarkMessagesAsRead(int currentUserId, int otherUserId)
        {
            try
            {
                string query = @"
                    UPDATE messages 
                    SET IsRead = 1 
                    WHERE SenderId = @OtherUserId 
                    AND ReceiverId = @CurrentUserId 
                    AND IsRead = 0";

                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString))
                {
                    connection.Open(); // Explicitly open the connection

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId);
                        cmd.Parameters.AddWithValue("@OtherUserId", otherUserId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("MarkMessagesAsRead", ex);
            }
        }

        // Helper method to fetch data from the database
        private static DataTable FetchData(string query, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolSys"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open(); // Explicitly open the connection

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Log errors to a file or database
        private static void LogError(string method, Exception ex)
        {
            string errorMessage = $"Error in {method}: {ex.Message}\nStack Trace: {ex.StackTrace}";

            // Log to file (example)
            string logFilePath = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            System.IO.File.AppendAllText(logFilePath, $"{DateTime.Now}: {errorMessage}\n\n");

            // Log to console for development
            System.Diagnostics.Debug.WriteLine(errorMessage);
        }
    }

    // User class to represent users in the dropdown
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }

    // Class to represent chat messages
    public class ChatMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSentByMe { get; set; }
    }

    // Class to represent result of saving a message
    public class MessageResult
    {
        public bool Success { get; set; }
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string ErrorMessage { get; set; }
    }
}