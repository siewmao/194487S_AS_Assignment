<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Week6.Registration"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Form</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("JS_lbPwd").innerHTML = "Password Length Must be at Least 8 Characters";
                document.getElementById("JS_lbPwd").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("JS_lbPwd").innerHTML = "Password require at least 1 number";
                document.getElementById("JS_lbPwd").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("JS_lbPwd").innerHTML = "Password require at least 1 special characters";
                document.getElementById("JS_lbPwd").style.color = "Red";
                return ("no_specialcharacter");

            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("JS_lbPwd").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("JS_lbPwd").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("JS_lbPwd").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("JS_lbPwd").style.color = "Red";
                return ("no_uppercase");
            }
            document.getElementById("JS_lbPwd").innerHTML = "Excellent!"
            document.getElementById("JS_lbPwd").style.color = "Blue";

        }
    </script>

    <script src="https://www.google.com/recaptcha/api.js?render=6LeMGz0aAAAAANY-RPxffnz5oyZ2__SerhAS7Q14"></script>

    
</head>
<body style="height: 832px">
    <form id="form2" runat="server">
        <section>
            
            <div class="container">
            
                   
                         
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">
                            <h1>
                            <asp:Label ID="Label3" runat="server" Text="Registration"></asp:Label>
                            </h1>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="JS_tbFName" runat="server" Height="32px" Width="269px" placeholder="First Name" title="First Name is Required"></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbFName" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <br />
                            <asp:TextBox ID="JS_tbDOB" runat="server" Height="32px" Width="269px" placeholder="Date of Birth (dd/mm/yyyy)" Enabled="False" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbDOB" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>
                            <asp:Button ID="JS_ChooseDOB" runat="server" Text="Select Date" OnClick="JS_ChooseDOB_Click" />
                            <br />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:TextBox ID="JS_tbLName" runat="server" Height="32px" Width="269px" placeholder="Last Name" ></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbLName" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td rowspan="8">
                            <asp:Label ID="JS_lbYear" runat="server" Text="Year:"></asp:Label>
                            <asp:DropDownList ID="JS_ddlYear" runat="server" OnSelectedIndexChanged="JS_ddlYear_SelectedIndexChanged" Height="24px" Width="93px" AutoPostBack="True">
                            </asp:DropDownList>
                            &nbsp;<asp:Label ID="JS_lbMonth" runat="server" Text="Month:"></asp:Label>
                            <asp:DropDownList ID="JS_ddlMonth" runat="server" OnSelectedIndexChanged="JS_ddlMonth_SelectedIndexChanged" Height="24px" Width="93px" AutoPostBack="True">
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="line" runat="server" Text="__________________________________"></asp:Label>
                            <asp:Calendar ID="JS_Calendar" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="234px" NextPrevFormat="ShortMonth" Width="279px" OnSelectionChanged="JS_Calendar_SelectionChanged">
                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                                <DayStyle BackColor="#CCCCCC" />
                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="White" Height="12pt" />
                                <TodayDayStyle BackColor="#999999" ForeColor="White" />
                            </asp:Calendar>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="JS_tbCCInfo" runat="server" Height="32px" Width="269px" placeholder="Credit Card Information" ></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbCCInfo" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="JS_tbEmailAddr" ErrorMessage="Invalid Email" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="JS_tbEmailAddr" runat="server" Height="32px" Width="269px" placeholder="Email Address (abc@hotmail.com)" ></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbEmailAddr" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lbEmailExisted" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="JS_btnCheck" runat="server" OnClick="JS_btnCheck_Click" Text="Check Password Strength" Height="36px" />
                            &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="269px" placeholder="Password" onkeyup="javascript:validate()" TextMode="Password" ></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbPwd" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="JS_lbPwd" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="269px" placeholder="Confirm Password" TextMode="Password" ></asp:TextBox>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lbCfmPwd" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lbMismatchPwd" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tb_cfpwd" ErrorMessage="Password doesn't meet the criterial" ForeColor="Red" ValidationExpression="^.*(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&amp;])[A-Za-z\d$@$!%*?&amp;]{8,10}.*$"></asp:RegularExpressionValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>
                            <asp:Button ID="JS_btn_Submit" runat="server" Text="Submit" Width="207px" OnClick="JS_btn_Submit_Click" Height="29px" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            
                   
                         
        </div>
               
        </section>
    </form>

    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LeMGz0aAAAAANY-RPxffnz5oyZ2__SerhAS7Q14', { action: 'login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    
    
</body>
</html>
