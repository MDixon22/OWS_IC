<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_BinTransfer.aspx.vb" Inherits="OWS_IC.IC_BinTransfer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid">
    <form id="form1" runat="server">
    <div title="IC Bin Transfer" style="text-align: left; font-size: 2em;">
    <table class="table table-striped">
		<tr class="row flex-fill">
			<td class="col-1" style="">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - Bin Transfer" EnableViewState="False" ></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="font-size:1em;">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="font-size:1em;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #" ></asp:Label>
			</td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-2" style="vertical-align: bottom; text-align: right; font-size:1em;">
				<asp:Label ID="lbPallet" runat="server" Font-Bold="False"  ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Pallet #-" EnableViewState="False"></asp:Label>
			</td>
			<td class="col-2" style="">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="font-size:1em;" align="right">
				<asp:Label ID="lbToBin" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle;" Text="To Bin-" Visible="False"></asp:Label>
			</td>
			<td class="col-2" style="font-size: 1em;">
				<asp:TextBox ID="txToBin" runat="server" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
			</td>
		</tr>
	</table>
	<table style="font-size:1em" border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" align="center">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle;" Visible="False"></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td style="font-size:1em;" align="right">
				<asp:Button ID="btReturn" runat="server" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="font-size:1em;" align="left">
				<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
   </form>
</body>
</html>
