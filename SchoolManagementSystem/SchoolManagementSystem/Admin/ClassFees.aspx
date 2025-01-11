<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="ClassFees.aspx.cs" Inherits="SchoolManagementSystem.Admin.ClassFees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image:url('../Images/bg3.jpg'); width:100%; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
            <h3 class="text-center">New Fees</h3>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="ddlClass">Class Fees</label>
                    <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-md-6">
                    <label for="txtFeeAmount">Fees (Annual)</label>
                    <asp:TextBox ID="txtFeeAmount" runat="server" CssClass="form-control" placeholder="Enter Fees Amount" TextMode="Number"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" Text="Add Class" OnClick="btnAdd_Click" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-12">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No records found"
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="4" DataKeyNames="FeesId" 
                    OnPageIndexChanging="GridView1_PageIndexChanging"
                    OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" 
                    OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="FeesId" HeaderText="FeesID" ReadOnly="True" />
                        <asp:BoundField DataField="ClassName" HeaderText="Class" ReadOnly="True" />
                        <asp:BoundField DataField="FeesAmount" HeaderText="Fees (Annual)" ReadOnly="True" />
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
