<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="OWS_IC._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body>
	<form id="form1" runat="server" defaultbutton="btRestart">
		<div title="Default Page" class="container-fluid" style="text-align:left" >
		<table class="table table-striped" style="font-size:3em">
			<tr>
				<td style="text-align: center; width: 240px;">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" 
				Style="vertical-align: middle; text-align: center" Text="OW Inventory - Logon" BackColor="White" EnableViewState="False"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="text-align: center;">
				</td>
			</tr>
			<tr>
				<td style="text-align: center;">
				<asp:Label ID="lbDirections" runat="server" Font-Bold="True" ForeColor="DarkRed" Text="Scan or Enter UserID" 
				Style="vertical-align: middle; text-align: center"></asp:Label>
				</td>
			</tr>
		</table>
		<table class="table table-striped" style="">
			<tr>
				<td style="width: 75px; text-align: right; height: 30px;">
					<asp:Label ID="lbUserID" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="UserID : " 
								Style="vertical-align: middle; text-align: right" EnableViewState="False"></asp:Label></td>
				<td style="width: 100px; height: 30px">
					<asp:TextBox ID="txUserID" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid" 
								Font-Bold="False" AutoPostBack="True" TextMode="Password" BorderWidth="1px" Columns="10" >
					</asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 75px; text-align: right; height: 30px;">
					<asp:Label ID="lbPassword" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="Password : " 
								Visible="False" Style="vertical-align: middle; text-align: right" EnableViewState="False" ></asp:Label></td>
				<td style="width: 100px; height: 30px">
					<asp:TextBox ID="txPassword" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid"
								Font-Bold="False" AutoPostBack="True" TextMode="Password" BorderWidth="1px" Columns="10" >
					</asp:TextBox></td>
			</tr>
		</table>
		<div class="row flex-fill justify-content-center" style="margin-top: 2em">
			<div class="col-2 1 d-grid">
				<asp:Button ID="btRestart" runat="server" Font-Bold="True" Text="Restart" UseSubmitBehavior="False" />
			</div>
			<div class ="col-2 offset-1 d-grid">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" 
					Font-Bold="False" ForeColor="Red" Visible="false" 
					Style="vertical-align: middle; text-align: center" BorderWidth="1px" Width="238px">
				</asp:Label>
				</div>

		</div>
		</div>
	</form>
</body>
</html>

