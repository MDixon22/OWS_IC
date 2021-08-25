<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboXfer_WC.aspx.vb" Inherits="OWS_IC.IC_ComboXfer_WC" %>

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
    <div title="IC WC Combo Receipt" style="text-align: left;">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0" bordercolor="#000000">
			<tr>
				<td style="text-align: center; height: 19px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						Text="OW Inventory - Combo Xfer to Weeden Creek" EnableViewState="False" ></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="Black" Style="vertical-align: middle; text-align: center" 
						Text="User ID : " BackColor="Transparent"></asp:Label><asp:Label ID="lbFunction"
							runat="server" Font-Size="XX-Small" Text="0" Visible="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center; vertical-align: top; height: 25px;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						Text="Scan Xfer Truck barcode"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center;">
					<asp:TextBox ID="txData" runat="server" Font-Size="X-Small"
						ForeColor="Black" BorderColor="Black" AutoPostBack="True" 
						BorderWidth="1px" Columns="15">
					</asp:TextBox></td>
			</tr>
		</table><table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="text-align: left; height: 30px;">
                    <asp:Button ID="btPrintManifest" runat="server" Font-Size="Small" Height="35px" 
								Text="Print Manifest" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
                <td style="text-align: right; height: 30px;">
                    <asp:Button ID="btFinishTransferProcess" runat="server"
							Font-Size="Small" Height="35px" Text="Finish Transfer" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
            </tr>
        </table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center;">
					<asp:Label ID="lbXferTruckNum" runat="server"
							Font-Bold="False" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: right" 
							Text="Xfer Truck # : " EnableViewState="False">
					</asp:Label><asp:Label ID="lbXferTruckVal" runat="server" Font-Bold="False" Font-Size="X-Small"
							Visible="False"></asp:Label>
                    <asp:Label ID="lbXferTruckID" runat="server" Font-Bold="False" Font-Size="X-Small"
                        Visible="False">0</asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center;">
                    <asp:Label ID="lbCombosInTruck" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="Small"
                        ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="Combos in Truck " BorderStyle="None" Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 240px; text-align: center; height: 19px;">
                    <asp:GridView ID="gvCombos" runat="server" AutoGenerateColumns="False" CellPadding="1"
                        CellSpacing="1" Font-Size="Small" PageSize="40" Visible="False">
                        <Columns>
                            <asp:BoundField DataField="ComboID" HeaderText="Combo #">
                                <HeaderStyle BackColor="#E0E0E0" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Formula" HeaderText="Formula">
                                <HeaderStyle BackColor="#E0E0E0" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
					</td>
				
			</tr>
			
		</table>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
				Style="vertical-align: middle; text-align: center" Visible="False" Width="238px"></asp:Label>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Size="Small" Height="35px" 
								Text="Combo Menu" Width="100px" Font-Bold="True" EnableViewState="False" />
			</td>
				<td style="text-align: right; height: 30px;">
					<asp:Button ID="btFinishedLoading" runat="server"
							Font-Size="Small" Height="35px" Text="Loading Finished" Width="125px" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>
