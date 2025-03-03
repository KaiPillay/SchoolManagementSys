<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentTimetable.aspx.cs" Inherits="SchoolManagementSystem.Student.StudentTimetable" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Student Timetable</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f4f4f9;
        }
        .container {
            max-width: 800px;
            margin-top: 50px;
        }
        .card {
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
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
                <h2 class="text-center text-purple">My Timetable</h2>
                <hr />
                <asp:Label ID="lblError" runat="server" CssClass="alert" Visible="false"></asp:Label>
                <asp:GridView ID="gvTimetable" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Day" HeaderText="Day" />
                        <asp:BoundField DataField="Session" HeaderText="Session" />
                        <asp:BoundField DataField="ClassName" HeaderText="Class" />
                        <asp:BoundField DataField="SubjectName" HeaderText="Subject" />
                        <asp:BoundField DataField="TeacherName" HeaderText="Teacher" />
                        <asp:BoundField DataField="Room" HeaderText="Room" />
                    </Columns>
                </asp:GridView>
                <!-- Back Button -->
                <div class="text-center">
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-back" Text="Back to Dashboard" OnClick="btnBack_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>