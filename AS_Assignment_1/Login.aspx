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
        background-color: #D1E8E2;
    }


    .auto-style1 {
        width: 1311px;
    }
    .auto-style5 {
        width: 437px;
    }
    .auto-style6 {
        width: 270px;
    }


</style>

    <div id="page">
    <p>
        &nbsp;</p>
        <p>
            &nbsp;</p>
    <p style="text-align:center; font-size:x-large;">
        <b>Login</b></p>
    <p>
        <table class="auto-style1">
            <tr>
                <td style="text-align:right; " class="auto-style5">
                    <asp:Label ID="lbl_email_2" runat="server" Text="Email"></asp:Label>
                </td>
                <td class="auto-style6" >
                    <asp:TextBox ID="tb_login_email" runat="server" Width="170px" TextMode="Email"></asp:TextBox>
                </td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right; " class="auto-style5">
                    <asp:Label ID="lbl_pwd_2" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="auto-style6" >
                    <asp:TextBox ID="tb_login_pwd" runat="server" Width="170px" TextMode="Password" ></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                <td class="auto-style5">
                    <asp:Label ID="lblerrorMsg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style6" >
                    <asp:CheckBox ID="cb_show_pwd_2" runat="server" Text="   Show Password" onclick="ShowHidePassword();"/>
                </td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style6">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">
                    &nbsp;</td>
                <td class="auto-style6" >
                    <asp:Button ID="btn_login" runat="server" Text="Login" Width="122px" OnClick="btn_login_Click" />
                </td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <tr>
                <td class="auto-style5">
                    &nbsp;
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    &nbsp;</td>
                <td class="auto-style6">
                    <asp:Label ID="lbl_gScore" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style5">&nbsp;</td>
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
