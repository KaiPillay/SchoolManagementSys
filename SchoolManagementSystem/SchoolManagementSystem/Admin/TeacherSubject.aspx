<%@ Page Title="Teacher Subject Assignment" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="TeacherSubject.aspx.cs" Inherits="SchoolManagementSystem.Admin.TeacherSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container p-md-4 p-sm-4">
        <h3 class="text-center">Assign Teacher to Class and Subject</h3>
        
        <div class="row mb-3">
            <div class="col-md-4">
                <label for="ddlTeacher">Select Teacher</label>
                <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="form-control" AutoPostBack="true">
                    <asp:ListItem Text="Select Teacher" Value="0" />
                </asp:DropDownList>
            </div>

            <div class="col-md-4">
                <label for="ddlClass">Select Class</label>
                <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select Class" Value="0" />
                </asp:DropDownList>
            </div>

            <div class="col-md-4">
                <label for="ddlSubject">Select Subject</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select Subject" Value="0" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnAssignTeacher" runat="server" CssClass="btn btn-primary" Text="Assign Teacher" OnClick="btnAssignTeacher_Click" />
            </div>
        </div>

        <br />

        <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="False" EmptyDataText="No Record To Display" DataKeyNames="Id" OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Teacher" SortExpression="Name" />
                <asp:BoundField DataField="ClassName" HeaderText="Class" SortExpression="ClassName" />
                <asp:BoundField DataField="SubjectName" HeaderText="Subject" SortExpression="SubjectName" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
