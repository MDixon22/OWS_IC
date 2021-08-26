<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_SmokingRacks.aspx.vb" Inherits="OWS_IC.IC_SmokingRacks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Bin Transfer" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OW Smoking - Rack Processing" EnableViewState="False" ></asp:Label>
                <asp:Label ID="lbProcessType" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right"></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Rack #" ></asp:Label>
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 238px; vertical-align: middle; height: 30px; text-align: center;">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="20"></asp:TextBox>
                <asp:Label ID="lbFunction" runat="server" Visible="False" Width="8px">0</asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btIntoOven" runat="server" Font-Size="Medium" Height="35px" 
						Text="Into Oven" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btOutOfOven" runat="server"
						Font-Size="Medium" Height="35px" Text="Out Of Oven" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 238px; vertical-align: middle; height: 5px; text-align: center;"> </td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btIntoBlast" runat="server" Font-Size="Medium" Height="35px" 
						Text="Into Blast" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btIntoRTE" runat="server"
						Font-Size="Medium" Height="35px" Text="Into RTE" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
			</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 75px; text-align: right">
				<asp:Label ID="lbRack" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Rack #-" Visible="False"></asp:Label>
			</td>
			<td style="width: 165px">
                <asp:Label ID="lbRackValue" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label>
                <asp:Label ID="lbRackProduct" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                    Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 75px; text-align: right">
				<asp:Label ID="lbOven" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Oven #-" Visible="False"></asp:Label>
			</td>
			<td style="width: 165px">
                <asp:Label ID="lbOvenValue" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label>
                <asp:Label ID="lbOvenDesc" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                    Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 75px; text-align: right">
				<asp:Label ID="lbOvenLocation" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Oven Location-" Visible="False"></asp:Label>
			</td>
			<td style="width: 165px">
                <asp:Label ID="lbOvenLocValue" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label>
                <asp:Label ID="lbOvenLocDesc" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                    Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                    text-align: right" Visible="False"></asp:Label></td>
		</tr>
	</table>
	
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
		<td style="width: 239px; vertical-align: middle; height: 30px; text-align: center;">
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
				Style="vertical-align: middle; text-align: center" Visible="False" Width="237px"></asp:Label></td></tr>
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
