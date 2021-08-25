<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_CreateInvPallet.aspx.vb" Inherits="OWS_IC.IC_CreateInvPallet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
    <%--<script language="javascript" type="text/javascript">
		function keepMeAlive(){
			if (document.getElementById('keepAliveIMG')) {
				document.getElementById('keepAliveIMG').src = 'someimg.gif?x=' + escape(new Date());
			}
		}
	window.setInterval("keepMeAlive()", 90000);
    </script>--%>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Returned Pallet" style="text-align: left">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OWS Inventory Mngmnt Create Missing Pallet Tag" BackColor="White" EnableViewState="False"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
								Style="vertical-align: middle; text-align: center" Text="User ID : ">
					</asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Case Label" BackColor="Transparent"></asp:Label></td>
		</tr>
    </table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbCaseLabel" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="CaseLabel-" Visible="False"></asp:Label></td>
				<td style="width: 195px;text-align: left">
					<asp:TextBox ID="txCaseLabel" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="40"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbQuantity" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="Quantity-" Visible="False"></asp:Label>
				</td>
				<td style="width: 195px;text-align: left">
					<asp:TextBox ID="txQuantity" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="4"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbToBin" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="To Bin-" Visible="False"></asp:Label>
				</td>
				<td style="width: 195px;text-align: left">
					<asp:TextBox ID="txToBin" runat="server" 
						Font-Size="XX-Small" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="15"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbPrinter" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="Printer-" Visible="False"></asp:Label>
				</td>
				<td style="width: 195px;text-align: left">
					<asp:TextBox ID="txPrinter" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="2"></asp:TextBox></td>
			</tr>
		</table>
		<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
			BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
			Style="vertical-align: middle; text-align: center" Visible="False" Width="238px"></asp:Label>
<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
			<td style="text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Size="Medium" Height="35px" 
								Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
				</td>
				<td style="text-align: right; height: 30px;">
					<asp:Button ID="btRestart" runat="server"
								Font-Size="Medium" Height="35px" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
	<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
    </form>
</body>
</html>