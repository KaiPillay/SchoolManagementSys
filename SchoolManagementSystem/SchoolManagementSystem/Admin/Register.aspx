<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SchoolManagementSystem.Admin.Register" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>User Registration</title>
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
                <h2 class="text-center text-purple">Register</h2>
                <hr />
                <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username" required></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password" required></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm your password" required></asp:TextBox>
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
                    <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-purple btn-block" Text="Register" OnClick="btnRegister_Click" />
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
