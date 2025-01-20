<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUsers.aspx.cs" Inherits="SchoolManagementSystem.Admin.UpdateUsers" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Update User</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f4f4f9;
        }
        .container {
            max-width: 500px;
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
            background-color: #007bff;
            color: white;
        }
        .btn-back:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card">
                <h2 class="text-center text-purple">Update User</h2>
                <hr />
                <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username" required></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="ddlRole">Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select Role" Value="" />
                        <asp:ListItem Text="Admin" Value="Admin" />
                        <asp:ListItem Text="Teacher" Value="Teacher" />
                        <asp:ListItem Text="Student" Value="Student" />
                    </asp:DropDownList>
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-purple btn-block" Text="Update" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger btn-block" Text="Delete" OnClick="btnDelete_Click" CausesValidation="false" />
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-back btn-block" Text="Back to Admin Home" OnClick="btnBack_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
