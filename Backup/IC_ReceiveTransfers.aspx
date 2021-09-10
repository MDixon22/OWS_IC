<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ReceiveTransfers.aspx.vb" Inherits="OWS_IC.IC_ReceiveTransfers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
   <%-- <script language="javascript" type="text/javascript">
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
    <div title="IC Receive Transfer" style="text-align: left;">
		   <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="OWS Inventory Management Recv Transfer Load" EnableViewState="False" ></asp:Label><asp:Label
									ID="lbReturnURL" runat="server" Visible="False" Width="1px">i</asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; height: 19px;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Enter Transfer #" ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 65px; vertical-align: bottom; text-align: right; height: 30px;">
                <asp:Label ID="lbTransferNo" runat="server" EnableViewState="False" Font-Bold="False"
                    Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right"
                    Text="Transfer #-"></asp:Label></td>
			<td style="width: 175px; vertical-align: bottom; height: 30px">
                <asp:TextBox ID="txTransferNo" runat="server" AutoPostBack="True" BorderColor="Black"
                    BorderWidth="1px" Columns="10" Font-Bold="False" Font-Size="XX-Small" ForeColor="Black"
                    Wrap="False"></asp:TextBox><asp:Label ID="lbTranferNumValue" runat="server"
                        Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle;
                        text-align: right"></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 65px; text-align: right; height: 25px;">
				<asp:Label ID="lbPallet" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet #-" EnableViewState="False"></asp:Label></td>
			<td style="width: 65px; height: 25px;">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="9"></asp:TextBox></td>
			<td style="width: 40px; text-align: right; height: 25px;">
				</td>
			<td style="width: 70px; height: 25px;">
				</td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
		    <td style="width: 40px; vertical-align: bottom; text-align: right;"></td>
			<td style="width: 200px; vertical-align: bottom; text-align: left;">
                <asp:DataGrid ID="dgPalletsOnTransfer" runat="server" AutoGenerateColumns="False"
                    EnableViewState="False" Font-Size="X-Small" Height="132px" Width="125px">
                    <PagerStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                        Font-Strikeout="False" Font-Underline="False" ForeColor="Maroon" HorizontalAlign="Center"
                        NextPageText="&amp;gtNext" PrevPageText="Back&amp;lt;" VerticalAlign="Middle"
                        Wrap="False" />
                    <AlternatingItemStyle BackColor="Gainsboro" />
                    <Columns>
                        <asp:BoundColumn DataField="Pallet" HeaderText="Pallet#" ReadOnly="True" SortExpression="Pallet">
                            <HeaderStyle Width="190px" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                    </Columns>
                    <HeaderStyle BackColor="ActiveCaption" BorderColor="White" BorderStyle="None" Font-Names="Arial"
                        Font-Size="Smaller" ForeColor="WhiteSmoke" HorizontalAlign="Center" VerticalAlign="Middle"
                        Wrap="False" />
                </asp:DataGrid>
                </td>
		
		</tr>
	</table>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
				Style="vertical-align: middle; text-align: center" Visible="False" Width="238px">
			</asp:Label><table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
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