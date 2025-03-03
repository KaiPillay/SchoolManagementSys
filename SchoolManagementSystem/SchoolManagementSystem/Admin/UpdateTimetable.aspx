<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateTimetable.aspx.cs" Inherits="SchoolManagementSystem.Admin.UpdateTimetable" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Update Timetable</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f4f4f9;
        }
        .container {
            max-width: 700px;
        }
        .card {
            margin-top: 50px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
        }
        .btn-purple {
            background-color: #6a1b9a;
            color: white;
        }
        .btn-purple:hover {
            background-color: #9c4dcc;
        }
        .alert {
            margin-top: 20px;
        }
        .btn-back {
            margin-top: 20px;
            background-color: #6c757d;
            color: white;
        }
        .btn-back:hover {
            background-color: #5a6268;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card">
                <h2 class="text-center text-purple">Manage Timetable</h2>
                <hr />
                <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
                <div class="form-group">
                    <label for="ddlStudents">Select Student</label>
                    <asp:DropDownList ID="ddlStudents" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlTimetableID">Timetable ID</label>
                    <asp:DropDownList ID="ddlTimetableID" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTimetableID_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlDay">Day</label>
                    <asp:DropDownList ID="ddlDay" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Monday" Value="Monday" />
                        <asp:ListItem Text="Tuesday" Value="Tuesday" />
                        <asp:ListItem Text="Wednesday" Value="Wednesday" />
                        <asp:ListItem Text="Thursday" Value="Thursday" />
                        <asp:ListItem Text="Friday" Value="Friday" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlSession">Session</label>
                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control">
                        <asp:ListItem Text="9:00" Value="9:00" />
                        <asp:ListItem Text="10:45" Value="10:45" />
                        <asp:ListItem Text="13:15" Value="13:15" />
                        <asp:ListItem Text="15:00" Value="15:00" />
                        <asp:ListItem Text="16:30" Value="16:30" />
                    </asp:DropDownList>
                </div> 
                <div class="form-group">
                    <label for="ddlClass">Class</label>
                    <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlSubject">Subject</label>
                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlTeacher">Teacher</label>
                    <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtRoom">Room</label>
                    <asp:TextBox ID="txtRoom" runat="server" CssClass="form-control" placeholder="Enter room"></asp:TextBox>
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="btnAddTimetable" runat="server" CssClass="btn btn-purple btn-block" Text="Add Timetable" OnClick="btnAddTimetable_Click" />
                    <asp:Button ID="btnUpdateTimetable" runat="server" CssClass="btn btn-purple btn-block" Text="Update Timetable" OnClick="btnUpdateTimetable_Click" />
                    <asp:Button ID="btnDeleteTimetable" runat="server" CssClass="btn btn-danger btn-block" Text="Delete Timetable" OnClick="btnDeleteTimetable_Click" />
                </div>
            </div>
            <div class="text-center">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-back" Text="Back to Admin Home" OnClick="btnBack_Click" />
            </div>
        </div>
    </form>
</body>
</html>