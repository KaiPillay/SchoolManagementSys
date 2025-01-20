<%@ Page Title="Edit Teacher" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="EditTeacher.aspx.cs" Inherits="SchoolManagementSystem.Admin.EditTeacher" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">Edit Teacher</h3>
    <asp:Label ID="lblStatus" runat="server" ForeColor="Black"></asp:Label>

    <asp:GridView ID="gvTeachers" runat="server" AutoGenerateColumns="False" OnRowCommand="gvTeachers_RowCommand" CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="TeacherId" HeaderText="Teacher ID" SortExpression="TeacherId" />
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="DOB" HeaderText="Date of Birth" SortExpression="DOB" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
            <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
            <asp:ButtonField ButtonType="Button" CommandName="EditTeacher" Text="Edit" HeaderText="Actions" />
            <asp:ButtonField ButtonType="Button" CommandName="DeleteTeacher" Text="Delete" HeaderText="Actions" />
        </Columns>
    </asp:GridView>

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
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" required></asp:TextBox>
    </div>

    <asp:Button ID="btnUpdate" runat="server" Text="Update Teacher" OnClick="btnUpdate_Click" CssClass="btn btn-primary" />
    <asp:HiddenField ID="hfTeacherId" runat="server" />
</asp:Content>
