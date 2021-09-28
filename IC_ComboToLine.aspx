<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboToLine.aspx.vb" Inherits="OWS_IC.IC_ComboToLine" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cut-Down Combos Assign To Line</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="Cut-Down Combos - Assign To Line" style="text-align: left">
    <table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - Combo - Assign To Line" EnableViewState="False" >
					</asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="row flex-fill" style="text-align: center;">
                <asp:Label ID="lbFunction" runat="server" Text="0" Visible="False"></asp:Label>
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : ">
				</asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Scan or Enter Combo Id#" >
				</asp:Label></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="vertical-align: bottom; text-align: center;">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="15"></asp:TextBox></td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: right" valign="middle">
				<asp:Label ID="lbComboId" runat="server"
					Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Combo Id#-"></asp:Label>
			</td>
			<td style="align="left" valign="middle">
                <asp:Label ID="lbComboIDVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: right"></asp:Label></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right" valign="middle">
				<asp:Label ID="lbLine" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Line #-"></asp:Label></td>
			<td class="col-2" style="" align="left" valign="middle">
                <asp:Label ID="lbLineVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: right"></asp:Label></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
	    <tr class="row flex-fill">
			<td class="col-1" style="text-align: center" align="center" valign="bottom">
			    <asp:Button ID="btAssignToLine" runat="server" Text="Assign To Line" Font-Bold="False" Visible="False" /></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center;">
			    <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False">
			    </asp:Label>
		    </td>
		</tr>
	</table>
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: left;">
				<asp:Button ID="btReturn" runat="server" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			</td>
			<td class="col-2" style="text-align: right;">
				<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
   </form>
</body>
</html>
