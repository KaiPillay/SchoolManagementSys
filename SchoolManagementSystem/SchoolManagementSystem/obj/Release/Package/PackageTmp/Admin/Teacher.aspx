<%@ Page Title="Add Teacher" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="Teacher.aspx.cs" Inherits="SchoolManagementSystem.Admin.Teacher" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">Add New Teacher</h3>
    <asp:Label ID="lblStatus" runat="server" ForeColor="Black"></asp:Label>

    <div class="form-group">
        <label>Teacher Name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Date of Birth</label>
        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Gender</label>
        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
            <asp:ListItem Value="Male">Male</asp:ListItem>
            <asp:ListItem Value="Female">Female</asp:ListItem>
            <asp:ListItem Value="Other">Other</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <label>Mobile</label>
        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Address</label>
        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Photo URL</label>
        <asp:TextBox ID="txtPhotoUrl" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add Teacher" OnClick="btnAdd_Click" />
</asp:Content>
