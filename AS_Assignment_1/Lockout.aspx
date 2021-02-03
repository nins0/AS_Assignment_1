<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Lockout.aspx.cs" Inherits="AS_Assignment_1.Lockout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p style="color: #D70000">
        <br />
&nbsp; Error : You cannot access your account because you have exceeded the limit of login attempts. Try again after 5 mins.</p>
    <p style="color: #D70000">
        <asp:Timer ID="Timer" OnTick="Timer_Tick" runat="server" Interval="10000" /></p>
</asp:Content>
