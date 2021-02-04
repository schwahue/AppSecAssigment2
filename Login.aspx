<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppSecAssigment2.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
        .auto-style3 {
            height: 23px;
            width: 174px;
        }
        .auto-style4 {
            margin-bottom: 0px;
        }
        .auto-style5 {
            width: 174px;
            height: 26px;
        }
        .auto-style6 {
            height: 26px;
        }
        .auto-style7 {
            height: 22px;
            width: 174px;
        }
        .auto-style8 {
            height: 22px;
        }
    </style>
    <script src="https://www.google.com/recaptcha/api.js?render=6LdHgOUZAAAAALB4CShfEHq9hQGfyg2mMmzXzpRP"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lb_Msg" runat="server" Visible="False"></asp:Label>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="lb_Login" runat="server" Text="Email: "></asp:Label>
                    </td>
                    <td class="auto-style6">
                        <asp:TextBox ID="tb_Email" runat="server" TextMode="Email"></asp:TextBox>
                    </td>
                    <td class="auto-style6"></td>
                </tr>
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="lb_Pwd" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td class="auto-style6">
                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                    <td class="auto-style6"></td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Button ID="btn_resetPwd" runat="server" Text="Change Password" Visible="False" />
                    </td>
                    <td class="auto-style1">
                        <asp:Button ID="btn_Login" runat="server" CssClass="auto-style4" OnClick="btn_Login_Click" Text="Login" />
                    </td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td class="auto-style7">
                        &nbsp;</td>
                    <td class="auto-style8">
                        <asp:Button ID="btn_Register" runat="server" OnClick="btn_Register_Click" Text="Register" />
                    </td>
                    <td class="auto-style8"></td>
                </tr>
                <tr>
                    <td class="auto-style7">
                       <input type ="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />

                    </td>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style8">&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdHgOUZAAAAALB4CShfEHq9hQGfyg2mMmzXzpRP', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
