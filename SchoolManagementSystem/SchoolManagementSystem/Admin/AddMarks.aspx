<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="AddMarks.aspx.cs" Inherits="SchoolManagementSystem.Admin.AddMarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div class="container">
        <h2>Add Marks for Student</h2>

        <div class="form-group">
            <label for="ddlStudents">Select Student:</label>
            <asp:DropDownList ID="ddlStudents" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="ddlSubject">Select Subject:</label>
            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtTotalMarks">Total Marks:</label>
            <asp:TextBox ID="txtTotalMarks" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtOutOfMarks">Out of Marks:</label>
            <asp:TextBox ID="txtOutOfMarks" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <asp:Button ID="btnAddMarks" runat="server" Text="Add Marks" CssClass="btn btn-primary" OnClick="btnAddMarks_Click" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
        </div>
    </div>
</asp:Content>
