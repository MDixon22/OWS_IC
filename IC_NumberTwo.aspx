<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_NumberTwo.aspx.vb" Inherits="OWS_IC.IC_NumberTwo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Number Two" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="OW Inventory - # 2 Scanning" EnableViewState="False" >
				</asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Firebrick" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Wisconsin Special Code" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="X-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6">
				</asp:TextBox></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; height: 36px; width: 240px;">
				<asp:Button ID="btVerifyFinished" runat="server" Font-Size="Smaller" 
						Text="Verified - Yes" Font-Bold="False" Visible="False" /></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 40px; text-align: right;"></td>
			<td style="width: 200px; text-align: left;">
				<asp:Label ID="lbWSCData" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Visible="False" BorderStyle="None"></asp:Label>
				<asp:Label ID="lbWSC_Desc" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Visible="False" BorderStyle="None"></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 65px; text-align: right;">
				<asp:Label ID="lbLot" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="DarkRed" Style="text-align: left" Text="Lot -" Visible="False"></asp:Label>
			</td>
			<td style="width: 175px; text-align: left;">
				<asp:Label ID="lbLotData" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 65px; text-align: right;">
				<asp:Label ID="lbPounds" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="DarkRed" Style="text-align: left" Text="Pounds -" Visible="False"></asp:Label>
			</td>
			<td style="width: 175px; text-align: left;">
				<asp:Label ID="lbPoundsData" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label>
				<asp:Label ID="lbFunction" runat="server" Font-Bold="False" Style="text-align: left" Visible="False" Text="0" Width="1px"></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left;">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
					BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
					Style="vertical-align: middle; text-align: center" Visible="False" Width="236px">
				</asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btReturn" runat="server" Font-Size="Medium" 
						Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btRestart" runat="server"
						Font-Size="Medium" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
	</div>
   </form>
</body>
</html>
