<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboMenu.aspx.vb" Inherits="OWS_IC.IC_ComboMenu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cut Down Combo Menu</title>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server" >
    <div>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="OW Inventory - Cut-Down Combo Menu" EnableViewState="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Select Option From List"></asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 30px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption1" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="1. Build Combo">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 30px; height: 19px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption2" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="2. Transfer Combo to Weeden Creek">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 30px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption3" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="3. Receive Combo Back From Weeden Creek">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 30px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption4" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="4. Void Cut Down Combo Errors">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 30px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption5" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="5. Void Combo Consumed by Line">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 30px;"></td>
				<td style="width: 210px;">
					<asp:Label ID="lbOption6" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="6. Return Problem Combo to Plant2">
					</asp:Label></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		    <tr>
		        <td style="width: 150px; text-align: right;">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed"
                        Style="vertical-align: middle; text-align: center" Text="Option -"></asp:Label></td>
				<td style="width: 90px;">
                    <asp:TextBox ID="txOption" runat="server" Columns="1" AutoPostBack="True" BorderWidth="1px"></asp:TextBox></td>
		    </tr>
		
		</table>		
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" 
				Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Visible="False" Width="238px" 
				Style="vertical-align: middle; text-align: center">
			</asp:Label>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 80px; text-align: center;"></td>
				<td style="width: 80px; text-align: center;">
					<asp:Button ID="btExit" runat="server" Font-Size="Medium" Height="35px" Text="Log Off" Width="115px" Font-Bold="True" EnableViewState="False" />
				</td>
				<td style="width: 80px; text-align: center;"></td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>
