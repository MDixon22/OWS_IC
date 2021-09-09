<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_AdjPalletQty.aspx.vb" Inherits="OWS_IC.IC_AdjPalletQty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Adjust Pallet Quantity" class="container-fluid" style="font-size:2em;">
		<table class="table table-striped">
			<tr class="row flex-fill align-content-center">
				<td style="text-align: center; font-size:2em;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True"
						ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						Text="OW Inventory - Adjust Pallet Qty" EnableViewState="False" ></asp:Label></td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td style="text-align: center; font-size:1.5em">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" 
						ForeColor="Black" Style="vertical-align: middle; text-align: center" 
						Text="User ID : " BackColor="Transparent"></asp:Label></td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td style="text-align: center; vertical-align: top;>
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						Text="Scan or Enter Pallet # to adjust" BackColor="Transparent"></asp:Label></td>
			</tr>
		</table>
		<table class="table table-striped">
			<tr class="row flex-fill align-content-center">
				<td class="col-2" style="font-size:1em;" align="right">
					<asp:Label ID="lbPallet" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Pallet #-" EnableViewState="False"></asp:Label>
				</td>
				<td class="col-2" style="font-size:1em;">
					<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox></td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td class="col-2" style="font-size:1em;" align="right">
					<asp:Label ID="lbCurrent" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: left" Text="Current Qty-" EnableViewState="False"></asp:Label>
				</td>
				<td class="col-2" style="font-size:1em;">
					<asp:Label ID="lbProdID_ProdDesc" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: left"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td class="col-2" style="font-size:1em;" align="right">
					<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="New Total Qty-" Visible="False"></asp:Label>
				</td>
				<td class="col-2" style="font-size:1em;">
					<asp:TextBox ID="txQuantity" runat="server" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="4"></asp:TextBox>
				</td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td class="col-2" style="font-size:1em;"></td>
				<td class="col-2" style="font-size:1em;"></td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td class="col-2" style="font-size:1em;" align="right">
					<asp:Label ID="lbPrinter" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False"></asp:Label>
				</td>
				<td class="col-2" style="font-size:1em;">
					<asp:TextBox ID="txPrinter" runat="server" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
				</td>
			</tr>			
		</table>
		<table class="table table-striped">
			<tr class="row flex-fill">
				<td class="col-1" align="center">
					<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle;" Visible="False" Align="center"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill align-content-center">
				<td class="col-2" align="right">
						<asp:Button ID="btReturn" runat="server" Text="Return" Font-Bold="True" EnableViewState="False" />
				</td>
				<td class="col-2" align="left">
					<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
    </form>
</body>
</html>
