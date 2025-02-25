<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="AddClassAttendance.aspx.cs" Inherits="SchoolManagementSystem.Teacher.AddClassAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <h3 class="text-center">Record Student Attendance</h3>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="ddlClass">Select Class</label>
                    <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtDate">Attendance Date</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                </div>
            </div>

            <asp:Repeater ID="rptStudents" runat="server">
                <HeaderTemplate>
                    <div class="table-responsive">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Student Name</th>
                                    <th>Attendance Status</th>
                                </tr>
                            </thead>
                            <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                            <%# Eval("Name") %>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select Status" Value="" />
                                <asp:ListItem Text="Present" Value="1" />
                                <asp:ListItem Text="Absent" Value="0" />
                                <asp:ListItem Text="Late" Value="2" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                            </tbody>
                        </table>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Label ID="lblStatus" runat="server" ForeColor="Black" />

            <div class="row mb-3">
                <div class="col-md-3">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary btn-block" Text="Submit Attendance" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

