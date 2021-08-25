<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_Menu.aspx.vb" Inherits="OWS_IC.IC_Menu" %>

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
    <form id="form1" runat="server" >
    <div>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="OW Inventory - Menu" EnableViewState="False"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbFGPutaway" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="1. FG Putaway">
					</asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbNewNumTwoProcess" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="11. New Number 2 Process"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px; height: 19px;"></td>
				<td style="width: 110px; height: 19px;">
					<asp:Label ID="lbBinTransfer" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="2. Bin Transfer">
					</asp:Label></td>
				<td style="width: 117px; height: 19px;">
                    <asp:Label ID="lbPacklandNumber2" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
                        Text="12.PackLand Number 2"></asp:Label></td>
				<td style="width: 3px; height: 19px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbShippingPallet" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="3. Crt Shipping Pallet"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbCreateSamplePallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="13. Create Sample Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbWorkWithShippingPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="DarkRed" Text="4. Wrk W/Shipping Pallet"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbWorkWithSamplePallet" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
                        Text="14. Wrk W/Sample Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbWhseTransfer" runat="server" 
						Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed" Text="5. Stock Pallet Transfer"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbRecvAbbylandPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="15. Rcv CoManufactrd Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbAdjPalletQty" runat="server" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="DarkRed" Text="6. Adj. Pallet Qty"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbComboMenu" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="16.CutDown Combo Menu"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbVoidPallet" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
						Text="7. Void Pallet"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbWrkShippingPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="17. New-Wrk Shipping Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
					<asp:Label ID="lbNumberTwo" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="DarkRed"
						Text="8. Number 2 Process"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbNewCrtSamplePallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="18. New-Crt Sample Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px;">
                    <asp:Label ID="lbTestNewFunction" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text=" 9. Test New FG Putaway"></asp:Label></td>
				<td style="width: 117px;">
                    <asp:Label ID="lbNewWrkSamplePallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="19. New-Wrk Sample Pallet"></asp:Label></td>
				<td style="width: 3px;"></td>
			</tr>
			<tr>
				<td style="width: 10px;"></td>
				<td style="width: 110px; text-align: left;">
                    <asp:Label ID="lbReAssignPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
                        ForeColor="DarkRed" Text="10. ReAssign Pallet"></asp:Label></td>
				<td style="width: 117px;"></td>
				<td style="width: 3px;"></td>
			</tr>
		</table>		
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 80px; text-align: center;">
				<asp:Button ID="btExit" runat="server" Font-Size="Medium" Height="35px" Text="Log Off" Width="115px" Font-Bold="True" EnableViewState="False" /></td>
				<td style="width: 80px; text-align: right;">
					<asp:Label id="Label1" runat="server" style="vertical-align: middle; text-align: right"
						Text="Selection-" ForeColor="Black" Font-Size="XX-Small" Font-Bold="True" ></asp:Label></td>
				<td style="width: 80px; text-align: left;"><asp:TextBox ID="txOption" runat="server" 
						Columns="2" MaxLength="2" AutoPostBack="True" BorderColor="Black" BorderWidth="1px" Font-Size="XX-Small"></asp:TextBox></td>
			</tr>
		</table>
		<%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%></div>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" 
				Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Visible="False" Width="238px" 
				Style="vertical-align: middle; text-align: center">
			</asp:Label>
    </form>
</body>
</html>
