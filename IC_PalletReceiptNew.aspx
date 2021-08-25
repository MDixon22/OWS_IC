<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PalletReceiptNew.aspx.vb" Inherits="OWS_IC.IC_PalletReceiptNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Inventory Control Site</title>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div style="text-align:left">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OWS Inventory Management Shipping Receipt" ></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #" ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 50px; vertical-align: middle; text-align: right; height: 30px;">
				<asp:Label ID="lbPallet" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet # :"></asp:Label>
			</td>
			<td style="width: 190px; vertical-align: middle; height: 30px">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small" 
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox></td>
		</tr>
		<tr>
			<td style="width: 50px; vertical-align: middle; text-align: right; height: 30px;">
				<asp:Label ID="lbCaseLabel" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="CaseLabel :"></asp:Label>
			</td>
			<td style="width: 190px; vertical-align: middle; height: 30px">
				<asp:TextBox ID="txCaseLabel" runat="server" Font-Bold="False" Font-Size="XX-Small" 
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="40"></asp:TextBox></td>
		</tr>
	</table>
		<asp:Label ID="lbProdDesc" runat="server"
			Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle;
			text-align: center" Width="238px" Visible="False" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 70px; text-align: right">
				<asp:Label ID="lbToBin" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="Recv To Bin :" Visible="False"></asp:Label>
			</td>
			<td style="width: 170px; text-align: left">
				<asp:Label ID="lbToBinVal" runat="server" BackColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px"
					Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
					text-align: right" Text="WHSE" Visible="False"></asp:Label></td>
		</tr>
	</table>	
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 70px; text-align: right">
				<asp:Label ID="lbFullLayers" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" 
					Style="vertical-align: middle; text-align: right" Text="Full Layers-" Visible="False"></asp:Label>
			</td>
			<td style="width: 50px; text-align: left">
				<asp:TextBox ID="txFullLayers" runat="server" Font-Size="XX-Small" ForeColor="Black" 
					BorderColor="Black" AutoPostBack="True" Visible="False" Wrap="False" BorderWidth="1px" Font-Bold="False" Columns="4"></asp:TextBox></td>
			<td style="width: 70px; text-align: right">
				<asp:Label ID="lbLooseBoxes" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Loose Cases-" Visible="False"></asp:Label>
			</td>
			<td style="width: 50px; text-align: left">
				<asp:TextBox ID="txLooseBoxes" runat="server" Font-Size="XX-Small" ForeColor="Black" Wrap="False" MaxLength="4" 
					BorderColor="Black" AutoPostBack="True" Visible="False" BorderWidth="1px" Font-Bold="False" Columns="4"></asp:TextBox></td>
		</tr>
	</table>
		<asp:Label ID="lbResult" runat="server" BorderColor="Black" BorderStyle="Solid"
			BorderWidth="1px" Font-Bold="False" Font-Size="Small" ForeColor="Black" 
			Style="vertical-align: middle; text-align: center" Visible="False" Width="238px"></asp:Label><br />
		<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
			BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Style="vertical-align: middle;
			text-align: center" Visible="False" Width="238px">
		</asp:Label><table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; height: 30px;">
				<asp:Button ID="btReturn" runat="server" Font-Size="Small" Height="35px" 
						Text="To Menu" Width="115px" Font-Bold="True" />
			</td>
			<td style="text-align: center; height: 30px;">
				<asp:Button ID="btRestart" runat="server"
						Font-Size="Small" Height="35px" Text="Restart Entry" Width="115px" Font-Bold="True" />
			</td>
		</tr>
	</table>
	<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
   </form>
</body>
</html>
