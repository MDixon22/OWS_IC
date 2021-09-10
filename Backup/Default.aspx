<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="OWS_IC._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
<body  bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" >
<form id="form1" runat="server" defaultbutton="btRestart">
<Div style="text-align:left" >
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; width: 240px;">
			<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
			Style="vertical-align: middle; text-align: center" Text="OW Inventory - Logon" BackColor="White" EnableViewState="False"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="text-align: center; width: 240px;">
			</td>
		</tr>
		<tr>
			<td style="text-align: center; width: 240px;">
			<asp:Label ID="lbDirections" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" Text="Scan or Enter UserID" 
			Style="vertical-align: middle; text-align: center"></asp:Label>
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 20px; text-align: right; height: 30px;"></td>
			<td style="width: 75px; text-align: right; height: 30px;">
				<asp:Label ID="lbUserID" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="UserID : " 
							Style="vertical-align: middle; text-align: right" EnableViewState="False"></asp:Label></td>
			<td style="width: 100px; height: 30px">
				<asp:TextBox ID="txUserID" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid" 
							Font-Bold="False" Font-Size="XX-Small" AutoPostBack="True" TextMode="Password" BorderWidth="1px" Columns="10" >
				</asp:TextBox></td>
		</tr>
		<tr>
			<td style="width: 20px; text-align: right; height: 30px;"></td>
			<td style="width: 75px; text-align: right; height: 30px;">
				<asp:Label ID="lbPassword" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="Password : " 
							Visible="False" Style="vertical-align: middle; text-align: right" EnableViewState="False" ></asp:Label></td>
			<td style="width: 100px; height: 30px">
				<asp:TextBox ID="txPassword" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid"
							Font-Bold="False" Font-Size="XX-Small" AutoPostBack="True" TextMode="Password" BorderWidth="1px" Columns="10" >
				</asp:TextBox></td>
		</tr>
	</table>
		<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" 
			Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Visible="False" 
			Style="vertical-align: middle; text-align: center" BorderWidth="1px" Width="238px">
		</asp:Label><br />
		<asp:Button ID="btRestart" runat="server" Font-Bold="True" Font-Size="Small" Text="Restart" Height="35px" Width="115px" UseSubmitBehavior="False" />
	<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
</form>
</body>
</html>

