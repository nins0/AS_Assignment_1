<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment_1.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<script src="https://www.google.com/recaptcha/api.js?render=6LcUtUEaAAAAALcVkdTROCNmOq20k4pWWTROixHU">
</script>


<script type="text/javascript">
    function ShowHidePassword() {
        var txt = $('#<%=tb_login_pwd.ClientID%>');
        if (txt.prop("type") == "password") {
            txt.prop("type", "text");
        }
        else {
            txt.prop("type", "password");
        }
    }
</script>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Noto+Serif+SC&display=swap');
    td{
        padding:5px;
    }
    #page{
        font-family: 'Noto Serif SC', serif;
    }

</style>

    <div id="page">
    <p>
        &nbsp;</p>
    <p style="padding-left:200px; font-size:large;">
        <b>Login</b></p>
    <p>
        <table style="width:100%;">
            <tr>
                <td style="text-align:center; ">
                    <asp:Label ID="lbl_email_2" runat="server" Text="Email"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_login_email" runat="server" Width="170px" TextMode="Email"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:center; ">
                    <asp:Label ID="lbl_pwd_2" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_login_pwd" runat="server" Width="170px" TextMode="Password" ></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblerrorMsg" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:CheckBox ID="cb_show_pwd_2" runat="server" Text="   Show Password" onclick="ShowHidePassword();"/>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btn_login" runat="server" Text="Login" Width="122px" OnClick="btn_login_Click" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lbl_gScore" runat="server" Visible="False"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            </table>
    </p>
    </div>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LcUtUEaAAAAALcVkdTROCNmOq20k4pWWTROixHU', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });

    </script>
</asp:Content>
