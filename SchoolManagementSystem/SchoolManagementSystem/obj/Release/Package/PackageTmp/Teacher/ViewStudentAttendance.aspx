<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="ViewStudentAttendance.aspx.cs" Inherits="SchoolManagementSystem.Teacher.ViewStudentAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <h3 class="text-center">Record Student Attendance</h3>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="ddlStudent">Select Student</label>
                    <asp:DropDownList ID="ddlStudent" runat="server" CssClass="form-control" required>
                        <asp:ListItem Text="Select Student" Value="" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="txtDate">Attendance Date</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="ddlStatus">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" required>
                        <asp:ListItem Text="Select Status" Value="" />
                        <asp:ListItem Text="Present" Value="1" />
                        <asp:ListItem Text="Absent" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <asp:Label ID="lblStatus" runat="server" ForeColor="Black" />

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary btn-block" Text="Submit Attendance" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
