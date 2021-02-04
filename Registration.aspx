<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AppSecAssigment2.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
        .auto-style4 {
            height: 23px;
            width: 199px;
        }
        .auto-style8 {
            height: 23px;
            width: 450px;
        }
        .auto-style10 {
            height: 24px;
            width: 199px;
        }
        .auto-style12 {
            height: 24px;
            width: 450px;
        }
        .auto-style13 {
            height: 24px;
        }
        .auto-style14 {
            width: 199px;
            height: 26px;
        }
        .auto-style16 {
            width: 450px;
            height: 26px;
        }
        .auto-style17 {
            height: 26px;
        }
        .auto-style18 {
            height: 23px;
            width: 230px;
        }
        .auto-style20 {
            width: 230px;
            height: 26px;
        }
        .auto-style21 {
            height: 24px;
            width: 230px;
        }
    </style>
</head>
    <body>
    <form id="form1" runat="server">
        <asp:Label ID="lb_genError" runat="server" Visible="False"></asp:Label>
        <table style="width:100%;">
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="lb_fName" runat="server" Text="First Name:"></asp:Label>
                </td>
                <td class="auto-style18">
                    <asp:TextBox ID="tb_fName" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="lb_fNameError" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style1">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="lb_lName" runat="server" Text="Last Name:"></asp:Label>
                </td>
                <td class="auto-style18">
                    <asp:TextBox ID="tb_lName" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="lb_lNameError" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="lb_DOB" runat="server" Text="Date of birth:"></asp:Label>
                </td>
                <td class="auto-style18">
                    <asp:TextBox ID="tb_DOB" runat="server" TextMode="Date"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="lb_DOBError" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="auto-style10">
                    <asp:Label ID="lb_Pwd" runat="server" Text="Password:"></asp:Label>
                </td>
                <td class="auto-style21">
                    <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                </td>
                <td class="auto-style12">
                    <asp:Label ID="lb_pwdError" runat="server"></asp:Label>
                </td>
                <td class="auto-style13"></td>
            </tr>
            <tr>
                <td class="auto-style14">
                    <asp:Label ID="lb_Email" runat="server" Text="Email address:"></asp:Label>
                </td>
                <td class="auto-style20">
                    <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style16">
                    <asp:Label ID="lb_emailError" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style17"></td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="lb_CardNum" runat="server" Text="Credit Card Info:"></asp:Label>
                </td>
                <td class="auto-style18">
                    <asp:TextBox ID="tb_creditCard" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="lb_creditCardInfoError" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="auto-style10"></td>
                <td class="auto-style21">
                    <asp:Button ID="btn_Register" runat="server" OnClick="btn_Register_Click" Text="Registration" />
                </td>
                <td class="auto-style12"></td>
                <td class="auto-style13"></td>
            </tr>
        </table>
    </form>    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;
            console.log("Reached")

            if (str.length < 8) {
                document.getElementById("lb_pwdError").innerHTML = "Password length must be at least 8 characters";
                document.getElementById("lb_pwdError").style.color = "Red";
                return ("too_short");
            }
            if (str.search(/[0-9]/) == -1) {
                document.getElementById("lb_pwdError").innerHTML = "Password require at least 1 number";
                document.getElementById("lb_pwdError").style.color = "Red";
                return ("no_number");
            }
            if (str.search(/[a-z]/) == -1) {
                document.getElementById("lb_pwdError").innerHTML = "Password require at least 1 lower case";
                document.getElementById("lb_pwdError").style.color = "Red";
                return ("no_lower_case");
            }
            if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lb_pwdError").innerHTML = "Password require at least 1 upper case";
                document.getElementById("lb_pwdError").style.color = "Red";
                return ("no_upper_case");
            }
            if (str.search(/[!@#$%^&*(),.?]/) == -1) {
                document.getElementById("lb_pwdError").innerHTML = "Password require at least 1 special character";
                document.getElementById("lb_pwdError").style.color = "Red";
                return ("no_special_character");
            }

            document.getElementById("lb_pwdError").innerHTML = "Excellent!";
            document.getElementById("lb_pwdError").style.color = "Blue";
        }
    </script>

</body>
</html>
