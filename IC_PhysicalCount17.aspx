<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PhysicalCount17.aspx.vb" Inherits="OWS_IC.IC_PhysicalCount17" %>

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
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Inventory - FG Physical Count" BackColor="White" EnableViewState="False"></asp:Label>
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
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Bin Location" BackColor="Transparent"></asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 240px; text-align: center;" valign="middle">
                    <asp:TextBox ID="txData" runat="server" AutoPostBack="True" BorderColor="Black" BorderWidth="1px"
                        Columns="43" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"></asp:TextBox></td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 50px; text-align: right">
					<asp:Label ID="lbBin" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False"></asp:Label></td>
				<td style="width: 190px;" valign="middle">
                    <asp:Label ID="lbBinValue" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
                    <asp:Label ID="lbFunction" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False"
                        Width="1px">0</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 50px; text-align: right">
					<asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right" Text="Pallet-" Visible="False"></asp:Label></td>
				<td style="width: 190px" valign="middle">
                    <asp:Label ID="lbPalletValue" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"
                        Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 80px; text-align: right;">
                    <asp:Label ID="lbSKU" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"
                        Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label><asp:Label
                            ID="lbDash" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"
                            Style="vertical-align: middle; text-align: center" Visible="False" Width="3px"> - </asp:Label></td>
				<td align="center" style="width: 160px; text-align: left;">
                    <asp:Label ID="lbProdDesc" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"
                        Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 50px; text-align: right;">
					<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Qty-" Visible="False"></asp:Label></td>
				<td align="center" style="width: 190px; text-align: left;">
                    <asp:Label ID="lbQuantityValue" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 120px; text-align: left;"><asp:Button ID="btNextLocation" runat="server"
						Font-Size="Medium" Height="35px" Text="Next Location" Width="115px" 
						Font-Bold="True" EnableViewState="False" Visible="False" /></td>
				<td style="width: 120px; text-align: right;"><asp:Button ID="btnVerified" runat="server"
						Font-Size="Medium" Height="35px" Text="Verified Count" Width="115px" 
						Font-Bold="True" EnableViewState="False" Visible="False" /></td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
		    <tr>
		        <td style="width: 240px; height: 25px;" valign="middle">
					<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
						BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
						Style="vertical-align: middle; text-align: center" Visible="False" Width="239px">
					</asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 242px" border="0" cellpadding="0" cellspacing="0">
			<tr>
			    <td style="text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Size="Medium" Height="35px" Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" /></td>
				<td style="text-align: right; height: 30px;">
					<asp:Button ID="btRestart" runat="server" Font-Size="Medium" Height="35px" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" /></td>
			</tr>
		</table>
    </div>
    </form>
</body>
</html>
