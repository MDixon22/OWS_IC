<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_StockPalletTransfer17.aspx.vb" Inherits="OWS_IC.IC_StockPalletTransfer17"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Bin Transfer Stock Pallet To Truck" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; width: 240px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Inventory -Load Stock Pallet on Transfer Truck" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 240px;">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 240px;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Use < > buttons to adjust Ship Date then Scan or Enter Truck Bin Location." ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
		    <td style="width: 30px; vertical-align: bottom; text-align: right;">
                <asp:Button ID="btMinus" runat="server" Font-Size="Medium" Text=" < " Font-Bold="True" /></td>
			<td style="width: 65px; vertical-align: bottom; text-align: right;">
                <asp:Label ID="lbSelectShipDate" runat="server" EnableViewState="False" Font-Bold="False"
                    Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right"
                    Text="Ship Date-"></asp:Label></td>
			<td style="width: 115px; vertical-align: bottom;">
                <asp:Label ID="lbSelectedShipDate" runat="server" Font-Bold="True" Font-Size="Small"
                    ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="7/16/2014" Width="60px"></asp:Label></td>
			<td style="width: 30px; vertical-align: bottom;">
                <asp:Button ID="btPlus" runat="server" Text=" > " Font-Bold="True" Font-Size="Medium" /></td>
		</tr>
		<tr>
		    <td style="width: 30px; vertical-align: bottom; text-align: right;"></td>
			<td style="width: 65px; vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbTruckBin" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Truck Bin-" EnableViewState="False">
				</asp:Label>
			</td>
			<td style="width: 115px; vertical-align: bottom;">
				<asp:TextBox ID="txTruckBin" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox></td>
            <td style="width: 30px; vertical-align: bottom; text-align: right;"></td>
		</tr>
		<tr>
		    <td style="width: 30px; vertical-align: bottom; text-align: right;"></td>
			<td style="width: 65px; vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbPallet" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet #-" EnableViewState="False" Visible="False"></asp:Label>
			</td>
			<td style="width: 115px; vertical-align: bottom;">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10" Visible="False"></asp:TextBox></td>
			<td style="width: 30px; vertical-align: bottom; text-align: right;"></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
	    <tr>
	        <td style="width: 240px; vertical-align: bottom; text-align: center;">
	        
	        </td>
	    </tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
	    <tr>
	        <td style="width: 240px; vertical-align: bottom; text-align: center;">
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
	</div>
   </form>
</body>
</html>
