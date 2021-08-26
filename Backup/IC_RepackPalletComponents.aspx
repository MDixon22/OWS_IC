<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_RepackPalletComponents.aspx.vb" Inherits="OWS_IC.IC_RepackPalletComponents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
 </head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Repack Pallet Components" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Inventory - Repack Pallet Components" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter  RePack FG Pallet" ></asp:Label><asp:Label ID="lbFunction" runat="server" Text="0" Visible="False" Width="1px" Font-Size="XX-Small" Height="1px">
                </asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 240px; height: 25px; text-align: center;">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="52"></asp:TextBox>
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 120px; height: 25px; text-align: right;">
                <asp:Label ID="lbFG_RPKPallet" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed"
                    Style="vertical-align: middle; text-align: center" Text="RePack FG Pallet# : " Visible="False"></asp:Label></td>
			<td style="width: 120px; height: 25px;">
                <asp:Label ID="lbFG_RPKPalletVal" runat="server" Font-Bold="True" Font-Size="X-Small"
                    ForeColor="Black" Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label></td>			
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 240px; text-align: center">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
					BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
					Style="vertical-align: middle; text-align: center" Visible="False" Width="238px">
				</asp:Label>
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 240px; text-align: center">
				<asp:Label ID="lbComponentsGridTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
					ForeColor="DarkRed" Text="Components in FG" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 240px; text-align: center;">
				<asp:DataGrid ID="dgComponentsUsed" runat="server" Font-Size="XX-Small" PageSize="20"
					Visible="False" Width="236px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ShowHeader="False">
					<AlternatingItemStyle BackColor="Gainsboro" Font-Size="XX-Small" />
					<HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
				</asp:DataGrid>
                <asp:Label ID="lbComponentsScanned" runat="server" Font-Bold="True" Font-Size="X-Small"
                    ForeColor="DarkRed" Text="Components Scanned" Visible="False">
                </asp:Label>
                <asp:Button ID="btFinish" runat="server" Text="Finished" Font-Bold="True" Font-Size="Medium" />
                <asp:DataGrid ID="dgComponentsScanned" runat="server" Font-Size="XX-Small" PageSize="20"
			        Visible="False" Width="236px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ShowHeader="False">
                    <AlternatingItemStyle BackColor="Gainsboro" />
                    <HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
                </asp:DataGrid></td>
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
