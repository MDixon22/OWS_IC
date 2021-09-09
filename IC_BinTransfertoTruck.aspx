<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_BinTransfertoTruck.aspx.vb" Inherits="OWS_IC.IC_BinTransfertoTruck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid">
    <form id="form1" runat="server">
    <div title="IC Bin Transfer To Truck" style="font-size:2em;">
    <table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - Load Pallets on Packland Truck" EnableViewState="False" ></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #" ></asp:Label>
			</td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-2" style=" vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbPallet" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Pallet #-" EnableViewState="False"></asp:Label>
			</td>
			<td class="col-2" style="width: 145px; vertical-align: bottom; height: 30px">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True"></asp:TextBox>
			</td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr>
			<td style="text-align: center">
			    <asp:Label ID="lbResult" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: right" BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:Label>
			</td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
		</tr>
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btReturn" runat="server" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
   </form>
</body>
</html>
