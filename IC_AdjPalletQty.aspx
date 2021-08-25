<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_AdjPalletQty.aspx.vb" Inherits="OWS_IC.IC_AdjPalletQty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
   <%-- <script language="javascript" type="text/javascript">
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
    <div title="IC Adjust Pallet Quantity" style="text-align: left;">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0" bordercolor="#000000">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						Text="OW Inventory - Adjust Pallet Qty" EnableViewState="False" ></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="Black" Style="vertical-align: middle; text-align: center" 
						Text="User ID : " BackColor="Transparent"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center; vertical-align: top; height: 25px;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						Text="Scan or Enter Pallet # to adjust" BackColor="Transparent"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 75px; text-align: right">
					<asp:Label ID="lbPallet" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="Pallet #-" EnableViewState="False"></asp:Label>
				</td>
				<td style="width: 165px; text-align: left">
					<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 75px; text-align: right">
					<asp:Label ID="lbCurrent" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: left" 
						Text="Current Qty-" EnableViewState="False"></asp:Label>
				</td>
				<td style="width: 165px; text-align: left">
					<asp:Label ID="lbProdID_ProdDesc" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: left"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="width: 75px; text-align: right">
					<asp:Label ID="lbQuantity" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="New Total Qty-" Visible="False"></asp:Label>
				</td>
				<td style="width: 165px; text-align: left">
					<asp:TextBox ID="txQuantity" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="4"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 75px; text-align: right">
                    &nbsp;</td>
				<td style="width: 165px; text-align: left">
					</td>
			</tr>
			<tr>
				<td style="width: 75px; text-align: right">
					<asp:Label ID="lbPrinter" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
						Text="Printer-" Visible="False"></asp:Label>
				</td>
				<td style="width: 165px; text-align: left">
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
					<asp:Button ID="btReturn" runat="server" Font-Size="Small" Height="35px" 
								Text="Return" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btRestart" runat="server"
							Font-Size="Small" Height="35px" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			</tr>
		</table>
		<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
    </form>
</body>
</html>
