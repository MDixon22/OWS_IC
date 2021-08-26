<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboToLine.aspx.vb" Inherits="OWS_IC.IC_ComboToLine" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cut-Down Combos Assign To Line</title>
        <%--<script language="javascript" type="text/javascript">
		function keepMeAlive(){
			if (document.getElementById('keepAliveIMG')) {
				document.getElementById('keepAliveIMG').src = 'someimg.gif?x=' + escape(new Date());
			}
		}
	window.setInterval("keepMeAlive()", 90000);
    </script>--%>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="Cut-Down Combos - Assign To Line" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; width: 238px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
					    Style="vertical-align: middle; text-align: center" Text="OW Inventory - Combo - Assign To Line" EnableViewState="False" >
					</asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 238px;">
                <asp:Label ID="lbFunction" runat="server" Font-Size="XX-Small" Text="0" Visible="False"></asp:Label><asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 238px;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Combo Id#" ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 240px; vertical-align: bottom; text-align: center;">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="15"></asp:TextBox></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 80px; height: 30px; text-align: right" valign="middle">
				<asp:Label ID="lbComboId" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Combo Id#-"></asp:Label>
			</td>
			<td style="width: 160px; height: 30px;" align="left" valign="middle">
                <asp:Label ID="lbComboIDVal" runat="server" Font-Bold="False" Font-Size="X-Small"
                    ForeColor="Black" Style="vertical-align: middle; text-align: right"></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 80px; height: 30px; text-align: right" valign="middle">
				<asp:Label ID="lbLine" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Line #-"></asp:Label></td>
			<td style="width: 160px; height: 30px;" align="left" valign="middle">
                <asp:Label ID="lbLineVal" runat="server" Font-Bold="False" Font-Size="X-Small"
                    ForeColor="Black" Style="vertical-align: middle; text-align: right"></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
	    <tr>
			<td style="width: 238px; text-align: center" align="center" valign="bottom">
			    <asp:Button ID="btAssignToLine" runat="server" Font-Size="Medium" Height="35px" 
			        Text="Assign To Line" Width="115px" Font-Bold="False" Visible="False" /></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; width: 238px; height: 30px;">
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
	<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
   </form>
</body>
</html>
