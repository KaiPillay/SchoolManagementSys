<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMst.Master" AutoEventWireup="true" CodeBehind="ViewTeacherPhoto.aspx.cs" Inherits="SchoolManagementSystem.Student.ViewTeacherPhoto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">Teacher Photo</h3>

    <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
    
    <div class="text-center">
        <asp:Image ID="imgTeacher" runat="server" CssClass="img-fluid rounded" Width="200px" Height="200px" />
        <br /><br />
        <asp:Button ID="btnBack" runat="server" Text="Back to Teachers" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
    </div>
</asp:Content>
