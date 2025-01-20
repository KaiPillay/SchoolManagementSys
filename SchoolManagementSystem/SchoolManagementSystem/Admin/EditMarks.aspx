<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="EditMarks.aspx.cs" Inherits="SchoolManagementSystem.Admin.EditMarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div class="container">
        <h2>Edit Marks for Student</h2>

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
            <asp:Button ID="btnEditMarks" runat="server" Text="Edit Marks" CssClass="btn btn-primary" OnClick="btnEditMarks_Click" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
        </div>
    </div>
</asp:Content>
