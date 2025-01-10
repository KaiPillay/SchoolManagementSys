<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="ClassFees.aspx.cs" Inherits="SchoolManagementSystem.Admin.ClassFees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h3 class="text-center">New Fees</h3>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                     <div class="col-md-6">
                    <label for="ddlClass">Class Fees</label>
                         <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control"></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Class is required" 
                             ControlToValidate="ddlClass" Display="Dynamic" ForeColor="Red" InitialValue="Select Class" 
                             SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6">
                    <label for="txtFeeAmount">Fees (Annual)</label>
                    <asp:TextBox ID="txtFeeAmount" runat="server" CssClass="form-control" placeholder="Enter Fees Amount" required></asp:TextBox>
                </div>
            </div>
            <asp:Label ID="lblStatus" runat="server" ForeColor="Black" />
            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColour="#5558C9" Text="Add Class" />
                </div>
            </div>
                <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                    <div class="col-md-6">
                        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                    </div>
                </div>
        </div>
    </div>

</asp:Content>
