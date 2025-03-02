<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" 
    CodeBehind="TeacherAddAnnouncement.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherAddAnnouncement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Create New Announcement</h2>
    
    <div class="form-group">
        <asp:Label runat="server" Text="Title"></asp:Label>
        <asp:TextBox runat="server" ID="txtTitle" CssClass="form-control" MaxLength="255"></asp:TextBox>
    </div>
    
    <div class="form-group">
        <asp:Label runat="server" Text="Content"></asp:Label>
        <asp:TextBox runat="server" ID="txtContent" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
    </div>
    
    <div class="form-group">
        <h4>Target Audience</h4>
        <asp:CheckBoxList ID="chkAudience" runat="server">
            <asp:ListItem Text="All Students" Value="AllStudents"></asp:ListItem>
            <asp:ListItem Text="All Staff" Value="AllStaff"></asp:ListItem>
        </asp:CheckBoxList>
        
        <asp:Label runat="server" Text="Specific Classes (optional)"></asp:Label>
        <asp:ListBox ID="lstClasses" runat="server" SelectionMode="Multiple" DataTextField="ClassName" 
            DataValueField="ClassID" CssClass="form-control"></asp:ListBox>
    </div>
    
    <asp:Button ID="btnSubmit" runat="server" Text="Publish Announcement" 
        CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>