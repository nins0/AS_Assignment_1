<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AS_Assignment_1.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    &nbsp;<br />
    HOME</p>
    <p>
    &nbsp;<asp:Label ID="lblMessage" runat="server"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" Width="95px" />
        <asp:Button ID="btn_change" runat="server" Text="Change Password" Width="95px" PostBackUrl="~/Update.aspx"/>
    </p>
<p>
    &nbsp;</p>
    <p>
        Email :
        <asp:Label ID="lbl_email" runat="server"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lbl_card" runat="server" Visible="False"></asp:Label>
    </p>
</asp:Content>
