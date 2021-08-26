<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_WH_Receipt.aspx.vb" Inherits="OWS_IC.IC_WH_Receipt" %>

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
    <div title="IC Warehouse Receipt" style="text-align: left;">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0" bordercolor="#000000">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						Text="OW Inventory - Transfer to Offsite Warehouse" EnableViewState="False" ></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="Black" Style="vertical-align: middle; text-align: center" 
						Text="User ID : " BackColor="Transparent"></asp:Label><asp:Label ID="lbFunction"
							runat="server" Font-Size="XX-Small" Text="0" Visible="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center; vertical-align: top; height: 25px;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						Text="Scan or Enter Warehouse" BackColor="Transparent"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center;">
					<asp:Label ID="lbWhse" runat="server"
							Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: right" 
							Text="Warehouse : " EnableViewState="False">
					</asp:Label><asp:Label ID="lbWhseVal" runat="server" Font-Bold="True" Font-Size="X-Small"
							Visible="False">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center;">
					<asp:TextBox ID="txData" runat="server" Font-Size="X-Small"
						ForeColor="Black" BorderColor="Black" AutoPostBack="True" 
						BorderWidth="1px" Columns="15">
					</asp:TextBox></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 120px; text-align: right; height: 19px;">
					<asp:Label ID="lbPallet" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: left" 
						Text="Pallet -" EnableViewState="False" Visible="False"></asp:Label></td>
				<td style="width: 120px; text-align: left; height: 19px;">
					<asp:Label ID="lbPalletVal" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False">
						</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 120px; text-align: right">
					<asp:Label ID="lbQty" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: right" Text="Pallet Qty -" Visible="False">
						</asp:Label>
				</td>
				<td style="width: 120px; text-align: left">
					<asp:Label ID="lbQtyVal" runat="server"
						Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False">
					</asp:Label></td>
			</tr>
			<tr>
				<td style="width: 120px; text-align: right; height: 15px;">
					</td>
				<td style="width: 120px; text-align: right; height: 15px;"><asp:Button ID="btNewWhse" runat="server"
							Font-Size="Small" Text="New Whse" Font-Bold="True" EnableViewState="False" /></td>
			</tr>
			<tr>
				<td style="width: 120px; text-align: right; height: 15px;"></td>
				<td style="width: 120px; text-align: right; height: 15px;"></td>
			</tr>					
		</table>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
				Style="vertical-align: middle; text-align: center" Visible="False" Width="238px"></asp:Label>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Size="Small" Height="35px" 
								Text="Ret To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
				<td style="text-align: right; height: 30px;">
					<asp:Button ID="btRestart" runat="server"
							Font-Size="Small" Height="35px" Text="Restart Pallet" Width="115px" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>
