<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_WorkWithShippingPalletNew.aspx.vb" Inherits="OWS_IC.IC_WorkWithShippingPalletNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
 </head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Whse Transfer" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Inventory - Work With Shipping Pallet" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Shipping Pallet #" ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 55px; text-align: right; height: 31px;">
				<asp:Label ID="lbPallet" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet #-" EnableViewState="False">
				</asp:Label>
			</td>
			<td style="width: 185px; height: 31px;">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10">
				</asp:TextBox></td>
		</tr>
		<tr>
			<td style="width: 55px;">
			</td>
			<td style="width: 185px; text-align: left;">
				<asp:Label ID="lbAddProduct" runat="server" Font-Size="X-Small" ForeColor="DarkRed"
					Text="1. Add Product to Pallet" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 55px;">
			</td>
			<td style="width: 185px; text-align: left;">
				<asp:Label ID="lbChgProductQty" runat="server" Font-Size="X-Small" ForeColor="DarkRed" Text="2. Change Product Qty on Pallet"
					Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 55px;">
			</td>
			<td style="width: 185px; text-align: left;">
				<asp:Label ID="lbRmvProduct" runat="server" Font-Size="X-Small" ForeColor="DarkRed" Text="3. Remove Product from Pallet"
					Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 55px; text-align: right;">
				<asp:Label ID="lbOption" runat="server" Font-Bold="True" Font-Size="X-Small" Text="Option -"
					Visible="False"></asp:Label></td>
			<td style="width: 185px; text-align: left;">
				<asp:TextBox ID="txOption" runat="server" AutoPostBack="True" BorderColor="Black"
					BorderWidth="1px" Columns="1" Font-Bold="False" Font-Size="X-Small" ForeColor="Black"
					Visible="False" Wrap="False"></asp:TextBox></td>
		</tr>
		
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
					BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
					Style="vertical-align: middle; text-align: center" Visible="False" Width="238px">
				</asp:Label>
			</td>
		</tr>
	</table>
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
		<table id="TABLE6" border="0" cellpadding="0" cellspacing="0" language="javascript"
			onclick="return TABLE6_onclick()" style="width: 240px">
			<tr>
				<td style="width: 240px; text-align: center">
					<asp:Label ID="lbExistingGridTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Text="Existing Pallet - Products" Visible="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 240px; text-align: center">
					<asp:DataGrid ID="dgExistingProducts" runat="server" Font-Size="X-Small" PageSize="30"
						Visible="False" Width="234px">
						<AlternatingItemStyle BackColor="Gainsboro" />
						<HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
					</asp:DataGrid>
				</td>
			</tr>
		</table>
	</div>
   </form>
</body>
</html>
