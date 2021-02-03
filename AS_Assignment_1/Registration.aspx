<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment_1.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script src="http://code.jquery.com/jquery-1.11.3.js" type="text/javascript"></script>
<script type="text/javascript">
    function ShowHidePassword() {
        var txt = $('#<%=tb_pwd.ClientID%>');
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
    #register{
       font-family: 'Noto Serif SC', serif;
        background-color: #D1E8E2;

</style>
    <div id="register">
    <p>
        &nbsp;</p>
    <p style="text-align:center; font-size:x-large " >
        <b>Registration</b><br />
    </p>
    <p>
        <table style="width: 1224px;">
            <tr>
                <td style="text-align:right; padding-right: 100px;">
        <asp:Label ID="lbl_f_name" runat="server" Text="First Name"></asp:Label>
                </td>
                <td style="height: 22px; width: 408px">
                    <asp:TextBox ID="tb_f_name" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right: 100px;">
                    <asp:Label ID="lbl_l_name" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td style="width: 408px">
                    <asp:TextBox ID="tb_l_name" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right: 100px;">
                    <asp:Label ID="lbl_email" runat="server" Text="Email"></asp:Label>
                </td>
                <td style="width: 408px">
                    <asp:TextBox ID="tb_email" runat="server" TextMode="Email" Width="170px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_emailchecker" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right: 100px;">
                    <asp:Label ID="lbl_dob" runat="server" Text="Date of Birth"></asp:Label>
                </td>
                <td style="width: 408px">
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date" Width="170px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right: 100px;">
                    <asp:Label ID="lbl_card" runat="server" Text="Credit Card"></asp:Label>
                </td>
                <td style="width: 408px">
                    <asp:TextBox ID="tb_card" runat="server" TextMode="Number" Width="170px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_cardchecker" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right: 100px;">
                    <asp:Label ID="lbl_pwd" runat="server" Text="Password" ></asp:Label>
                </td>
                <td style="height: 22px; width: 408px">
                    <asp:TextBox ID="tb_pwd" runat="server" Width="170px" TextMode="Password"  onkeyup="javascript:validation()" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_pwdchecker" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 408px">
                    &nbsp;</td>
                <td style="width: 408px">
                    <asp:CheckBox ID="cb_pwd" runat="server" Text="  Show Password" onclick="ShowHidePassword();"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_pwdchecker2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 408px">&nbsp;</td>
                <td style="width: 408px">
                    <asp:Label ID="lbl_inputchecker" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btn_reg" runat="server" Text="Register" Width="174px" OnClick="btn_reg_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 408px; height: 20px"></td>
                <td style="height: 20px; width: 408px"></td>
            </tr>
            <tr>
                <td style="width: 408px">&nbsp;</td>
                <td style="width: 408px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 408px">&nbsp;</td>
                <td style="width: 408px">&nbsp;</td>
            </tr>
        </table>
    </p>
    </div>
    <script>
        function validation() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;            var checker = document.getElementById('<%=lbl_pwdchecker.ClientID %>');
            if (str.length < 8) {
                checker.innerHTML = "Password Length must be at least 8 characters";
                checker.style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                checker.innerHTML = "Password require at least 1 Number";
                checker.style.color = "Red";
                return ("no_number");
            }

            else if (str.search(/[A-Z]/) == -1) {
                checker.innerHTML = "Password require at least 1 Uppercase character";
                checker.style.color = "Red";
                return ("no_uppercase");
            }

            else if (str.search(/[a-z]/) == -1) {
                checker.innerHTML = "Password require at least 1 Lowercase character";
                checker.style.color = "Red";
                return ("no_lowercase");
            }

            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                checker.innerHTML = "Password require at least a special character";
                checker.style.color = "Red";
                return ("no_special_character");
            }

            checker.innerHTML = "Excellent!";
            checker.style.color = 'Green';

        }

    </script>
</asp:Content>
