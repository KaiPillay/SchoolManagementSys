<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="TeacherViewAnnouncements.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherViewAnnouncements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /* General Styles */
        .announcements-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            font-family: Arial, sans-serif;
        }

        .announcement {
            background-color: #ffffff;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            transition: box-shadow 0.3s ease;
        }

        .announcement:hover {
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
        }

        .announcement h3 {
            margin-top: 0;
            color: #2c3e50;
            font-size: 24px;
            font-weight: 600;
        }

        .announcement p {
            color: #34495e;
            font-size: 16px;
            line-height: 1.6;
            margin: 10px 0;
        }

        .announcement small {
            display: block;
            color: #7f8c8d;
            font-size: 14px;
            margin-top: 10px;
        }

        .announcement hr {
            border: 0;
            height: 1px;
            background: #e0e0e0;
            margin: 20px 0;
        }

        /* Error Message Style */
        #lblError {
            display: block;
            color: #e74c3c;
            font-size: 16px;
            margin-bottom: 20px;
            text-align: center;
        }

        /* Header Style */
        h2 {
            text-align: center;
            color: #2c3e50;
            font-size: 32px;
            margin-bottom: 30px;
            font-weight: 700;
        }
    </style>

    <div class="announcements-container">
        <h2>Announcements</h2>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        <asp:Repeater ID="rptAnnouncements" runat="server">
            <ItemTemplate>
                <div class="announcement">
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Content") %></p>
                    <small>Posted by: <%# Eval("AuthorID") %> on <%# Eval("CreatedDate", "{0:MM/dd/yyyy hh:mm tt}") %></small>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>