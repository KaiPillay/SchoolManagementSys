<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkParentStudent.aspx.cs" Inherits="SchoolManagementSystem.Admin.LinkParentStudent" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Link Parent to Student</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f4f4f9;
        }
        .container {
            max-width: 800px;
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
                <h2 class="text-center text-purple">Link Parent to Student</h2>
                <hr />
                <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
                <div class="form-group">
                    <label for="ddlParent">Select Parent</label>
                    <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlStudent">Select Student</label>
                    <asp:DropDownList ID="ddlStudent" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="btnLink" runat="server" CssClass="btn btn-purple btn-block" Text="Link Parent to Student" OnClick="btnLink_Click" />
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-back btn-block" Text="Back to Admin Home" OnClick="btnBack_Click" CausesValidation="false" />
                </div>
                <hr />
                <h4 class="text-center">Linked Parent-Student Data</h4>
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ParentName" HeaderText="Parent Name" />
                        <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
