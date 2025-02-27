<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterParent.aspx.cs" Inherits="SchoolManagementSystem.Admin.RegisterParent" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Parent</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2 class="text-center">Assign Parent to Students</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none" Visible="false"></asp:Label>
            
            <div class="mb-3">
                <label class="form-label">Select Parent:</label>
                <asp:DropDownList ID="ddlParents" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="mb-3">
                <label class="form-label">Select Students:</label>
                <asp:CheckBoxList ID="chkStudents" runat="server" CssClass="form-check"></asp:CheckBoxList>
            </div>

            <div class="d-flex justify-content-between">
                <asp:Button ID="btnAssign" runat="server" CssClass="btn btn-success" Text="Assign Parent" OnClick="btnAssign_Click" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-secondary" Text="Back" OnClick="btnBack_Click" />
            </div>
        </div>
    </form>
</body>
</html>
