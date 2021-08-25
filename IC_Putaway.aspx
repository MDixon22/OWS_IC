<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_Putaway.aspx.vb" Inherits="OWS_IC.IC_Putaway" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
<body bottommargin="0" leftmargin="2" rightmargin="2" topmargin="0">
    <form id="form1" runat="server">
    <div style="text-align:left">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Inventory - Product Putaway" BackColor="White" EnableViewState="False"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
								Style="vertical-align: middle; text-align: center" Text="User ID : ">
					</asp:Label>
				</td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #" BackColor="Transparent">
					</asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 250px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 55px; text-align: right">
					&nbsp;</td>
				<td style="width: 195px; height: 19px;" valign="middle">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right" Text="CaseLabel-" Visible="False"></asp:Label>
				</td>
				<td style="width: 195px;" valign="middle">
					<asp:TextBox ID="txCaseLabel" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False" Columns="42" MaxLength="44"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: right">
					<asp:Label ID="lbQuantity" runat="server"
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: right" Text="Qty-" Visible="False"></asp:Label>
				</td>
				<td style="width: 195px" valign="middle">
					<asp:TextBox ID="txQuantity" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False" Columns="4">
					</asp:TextBox>
				</td>
			</tr>
			<tr>
				<td style="width: 55px; text-align: center; height: 19px;">
					</td>
				<td align="center" style="width: 195px; text-align: left; height: 19px;">
					<asp:Label ID="lbTimeDesc" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkBlue"
						Style="vertical-align: middle; text-align: center" Text="Start / Stop is in Military Time - HHMM"
						Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 250px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 55px; text-align: right;">
					<asp:Label ID="lbStart" runat="server"
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: right" Text="Start-" Visible="False"></asp:Label>
				</td>
				<td style="width: 65px">
					<asp:TextBox ID="txStart" runat="server" Font-Size="XX-Small" ForeColor="Black" 
						MaxLength="4" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False" Columns="4">
					</asp:TextBox>
				</td>
				<td style="width: 55px; text-align: right;">
					<asp:Label ID="lbStop" runat="server" Font-Bold="False" Font-Size="XX-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Stop-" Visible="False"></asp:Label>
				</td>
				<td style="width: 65px; text-align: left;">
					<asp:TextBox ID="txStop" runat="server" Font-Size="XX-Small" ForeColor="Black" 
						MaxLength="4" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False" Columns="4">
					</asp:TextBox></td>
			</tr>
		</table>
		<table style="width: 250px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 55px; text-align: right;">
					<asp:Label ID="lbToBin" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: right" Text="To Bin-" Visible="False"></asp:Label>
				</td>
				<td style="width: 65px">
					&nbsp;<asp:Label ID="lbToBinName" runat="server" BorderColor="Black" BorderStyle="Solid"
						BorderWidth="1px" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
						text-align: center" Text="OWFG" Width="44px"></asp:Label></td>
				<td style="width: 55px; text-align: right;" valign="top">
					<asp:Label ID="lbPrinter" runat="server" Font-Bold="False" Font-Size="XX-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False"></asp:Label>
				</td>
				<td style="width: 65px;" valign="top">
					<asp:TextBox ID="txPrinter" runat="server" Font-Size="XX-Small"
						ForeColor="Black" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False" Columns="2">
					</asp:TextBox>
				</td>
			</tr>
		</table>
					<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
						BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
						Style="vertical-align: middle; text-align: center" Visible="False" Width="241px">
					</asp:Label>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
			<td style="text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Size="Medium" Height="35px" 
						Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
				</td>
				<td style="text-align: right; height: 30px;">
					<asp:Button ID="btRestart" runat="server"
						Font-Size="Medium" Height="35px" Text="Restart Entry" Width="115px" 
						Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
    <%--<img id="keepAliveIMG" height="0" src="someimg.GIF" style="width: 0px; height: 0px" />--%></div>
    </form>
</body>
</html>
