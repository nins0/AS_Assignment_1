<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="AS_Assignment_1.Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
function ShowHidePassword() {
    var txt = $('#<%=tb_cfm_pwd.ClientID%>');
    var txt2 = $('#<%=tb_new_pwd.ClientID%>');
    var txt3 = $('#<%=tb_current_pwd.ClientID%>');
    if (txt.prop("type") == "password" && txt2.prop("type") == "password" && txt3.prop("type") == "password") {
        txt.prop("type", "text");
        txt2.prop("type", "text");
        txt3.prop("type", "text");
    }
    else {
        txt.prop("type", "password");
        txt2.prop("type", "password");
        txt3.prop("type", "password");
    }
}
</script>
<style>
    @import url('https://fonts.googleapis.com/css2?family=Noto+Serif+SC&display=swap');
    td{
        padding:5px;
    }
    #table{
        font-family: 'Noto Serif SC', serif;
        
    }
    #page{
        background-color: #D1E8E2;
    }

    .auto-style2 {
        width: 1578px;
    }
    .auto-style7 {
        width: 526px;
    }
    .auto-style8 {
        width: 526px;
        height: 29px;
    }

</style>
<div id="page">
    <p style="font-family: 'Noto Serif SC', serif; font-size:x-large; text-align:center;">
        <br />
        Change Password</p>
 
        <table id="table" class="auto-style2">
            <tr>
                <td class="auto-style7" style="text-align:right;">&nbsp;</td>
                <td style="text-align:center;" class="auto-style7">Email : <asp:Label ID="lbl_email" runat="server"></asp:Label></td>
                <td style="text-align:left;" class="auto-style7">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7" style="text-align:right;"><asp:Label ID="lbl_current_pwd" runat="server" Text="Current Password"></asp:Label></td>
                <td style="text-align:center;" class="auto-style7"><asp:TextBox ID="tb_current_pwd" runat="server" Width="170px" TextMode="Password" ></asp:TextBox></td>
                <td style="text-align:left;" class="auto-style7">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7" style="text-align:right;"><asp:Label ID="lbl_new_pwd" runat="server" Text="New Password"></asp:Label></td>
                <td style="text-align:center;" class="auto-style7"><asp:TextBox ID="tb_new_pwd" runat="server" Width="170px" TextMode="Password" onkeyup="javascript:validation()" ></asp:TextBox></td>
                <td style="text-align:left;" class="auto-style7"><asp:Label ID="lblerrorMsg0" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style7" style="text-align:right;"><asp:Label ID="lbl_cfm_pwd" runat="server" Text="Comfirm Password"></asp:Label></td>
                <td style="text-align:center;" class="auto-style7"><asp:TextBox ID="tb_cfm_pwd" runat="server" Width="170px" TextMode="Password" ></asp:TextBox></td>
                <td class="auto-style7"><asp:Label ID="lblerrorMsg" runat="server" ></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style8"></td>
                <td style="text-align:center;" class="auto-style8"><asp:CheckBox ID="cb_show_pwd_2" runat="server" Text="   Show Password" onclick="ShowHidePassword();"/></td>
                <td class="auto-style8"><asp:Label ID="lblerrorMsg1" runat="server" ></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style7">&nbsp;</td>
                <td style="text-align:center;" class="auto-style7"><asp:Button ID="btn_change" runat="server" Text="Change Password" Width="140px" OnClick="btn_change_Click"/></td>
                <td class="auto-style7">&nbsp;</td>
            </tr>
        </table>
</div>
    <script>
        function validation() {
            var str = document.getElementById('<%=tb_new_pwd.ClientID %>').value;
            var checker = document.getElementById('<%=lblerrorMsg.ClientID %>');
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
