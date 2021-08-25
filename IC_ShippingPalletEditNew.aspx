<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ShippingPalletEditNew.aspx.vb" Inherits="OWS_IC.IC_ShippingPalletEditNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
    <script language="javascript" type="text/javascript">
        function setfocus(a_field_id) {
				document.getElementById(a_field_id).focus();
        }

        function chkNumbers(textBoxId) {
				var strValidChars = "0123456789";
				var IsNumber = true;
				var strChar = document.getElementById(textBoxId).value
				var strAt;

				if (strChar.length == 0) return false;
				for (i = 0; i < strChar.length && IsNumber == true; i++)
					{
					strAt = strChar.charAt(i);
					if (strValidChars.indexOf(strAt) == -1)
						{
						IsNumber = false;
						}
					}
				return IsNumber;
		}

        function moveNext(textBoxId, setFocusTextBoxId, labelErrorId) { 
				var strlbPrompt = "lbPrompt";
				document.getElementById(labelErrorId).innerHTML = "";
					if (chkNumbers(textBoxId)) {
						if (setFocusTextBoxId == "htxProduct") {
							document.getElementById(strlbPrompt).innerHTML = "Scan or Enter Pick Product.";
						}
						if (setFocusTextBoxId == "htxPallet") {
							document.getElementById(strlbPrompt).innerHTML = "Scan or Enter Pallet Tag.";
						}
						if (setFocusTextBoxId == "txQuantity") {
							document.getElementById(strlbPrompt).innerHTML = "Enter Case Quantity.";
						}
						setfocus(setFocusTextBoxId);
					}
					else {
						document.getElementById(labelErrorId).innerHTML = "Entry made in '" + textBoxId + "' must be numeric!";
						setfocus(textBoxId);
					}
				}
		}

         </script>
</head> 
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
	<div style="text-align: left" title="IC Shipping Pallets" align="left">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="text-align: center; height: 19px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						Text="OW Inventory - Shipping Pallet Remove Product" EnableViewState="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center; height: 19px;">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label></td>
			</tr>
			<tr>
				<td style="text-align: center;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" 
						Text="Select Button">
					</asp:Label></td>
			</tr>
		</table>
		<table border="0" cellpadding="0" cellspacing="0" style="width: 240px">
			<tr>
				<td style="width: 119px; text-align: left">
					<asp:Button ID="btNew" runat="server" Font-Bold="True" Font-Size="Small" Height="30px"
						Text="Next Product" Width="110px" /></td>
				<td style="text-align: left">
					<asp:Button ID="btComplete" runat="server" Font-Bold="True" Font-Size="Medium" Height="30px"
						Text="Complete" Visible="False" Width="110px" /></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 119px; text-align: center">
					<asp:Button ID="btNo" runat="server" Font-Bold="True" Font-Size="Large" Text="- No -" Visible="False" Height="30px" />
				</td>
				<td style="text-align: center">
					<asp:Button ID="btYes" runat="server" Font-Bold="True" Font-Size="Large" Text="- Yes -" Visible="False" Height="30px" />
				</td>
			</tr>
		</table>		
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 70px; text-align: right" align="right">
					<asp:Label ID="lbShipPallet" runat="server" Font-Bold="True" Font-Size="X-Small" 
						Style="vertical-align: middle; text-align: right" 
						Text="Pallet #-" Visible="False"></asp:Label>
				</td>
				<td style="width: 170px; text-align: left; height: 14px;" align="left">
					<asp:Label ID="lbShipPalletValue" runat="server" Font-Bold="True" Font-Size="X-Small" 
						Style="vertical-align: middle; text-align: right" Visible="False">
					</asp:Label></td>
			</tr>
		</table>		
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 70px; text-align: right" align="right">
					<asp:Label ID="lbOrder" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right" 
						Text="Pick Order-">
					</asp:Label>
				</td>
				<td style="width: 170px; text-align: left; height: 14px;">
					<input id="htxOrder" type="text" onchange="moveNext('htxOrder','htxProduct','lbError')" 
						runat="server" enableviewstate="true" style="font-size: 7px; font-family: Verdana" size="15" /></td>
			</tr>
			<tr>
				<td style="width: 70px; text-align: right" align="right">
					<asp:Label ID="lbProduct" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right;" Text="Pick Product-">
					</asp:Label>
				</td>
				<td style="width: 170px; text-align: left; height: 14px;">
					<input id="htxProduct" type="text" onchange="moveNext('htxProduct','htxPallet','lbError')" 
						runat="server" enableviewstate="true" style="font-size: 7px; font-family: Verdana" size="23" /></td>
			</tr>
			<tr>
				<td style="width: 70px; text-align: right" align="right">
					<asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Inv Pallet#-"></asp:Label></td>
				<td style="width: 170px; text-align: left; height: 14px;">
					<input id="htxPallet" runat="server" enableviewstate="true" onchange="moveNext('htxPallet','txQuantity','lbError')" style="font-size: 7px; font-family: Verdana" type="text" /></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 70px; text-align: right">
					<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right;" Text="New Total Qty-" Visible="False"></asp:Label>
				</td>
				<td style="width: 70px; text-align: left">
					<asp:TextBox ID="txQuantity" runat="server" BorderColor="Black"
						BorderStyle="Solid" BorderWidth="1px" Columns="1" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True">
					</asp:TextBox></td>
				<td style="width: 100px; text-align: center">
                    <asp:Label ID="lbQtyRemain" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Style="vertical-align: middle;
                        text-align: right" Text="- Remaining Qty - 00" Visible="False"></asp:Label></td>
			</tr>
		</table>
				<table style="width: 240px" border="0" cellpadding="0" cellspacing="0" id="TABLE1" onclick="return TABLE1_onclick()">
			<tr>
			    <td style="width: 70px; text-align: right">
					<asp:Label ID="lbGrossWt" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right;" Text="Gross Wt-" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 70px; text-align: left">
					<asp:TextBox ID="txGrossWt" runat="server" BorderColor="Black"
						BorderStyle="Solid" BorderWidth="1px" Columns="5" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True"></asp:TextBox></td>
				<td style="width: 60px; text-align: right">
					<asp:Label ID="lbDatePackaged" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right;" Text="Date Pkgd-" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 40px; text-align: left">
					<asp:TextBox ID="txDatePackaged" runat="server" BorderColor="Black"
						BorderStyle="Solid" BorderWidth="1px" Columns="5" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True"></asp:TextBox></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 70px; text-align: right">
					<asp:Label ID="lbBin" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 70px; text-align: left">
					<asp:TextBox ID="txBin" runat="server" AutoPostBack="True" BorderColor="Black"
						BorderStyle="Solid" BorderWidth="1px" Columns="9" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Visible="False" Wrap="False"></asp:TextBox></td>
				<td style="width: 50px; text-align: right">
					<asp:Label ID="lbPrinter" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
						Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 50px; text-align: left">
					<asp:TextBox ID="txPrinter" runat="server" AutoPostBack="True" BorderColor="Black"
						BorderStyle="Solid" BorderWidth="1px" Columns="2" Font-Bold="False" Font-Size="XX-Small"
						ForeColor="Black" Visible="False" Wrap="False">
					</asp:TextBox></td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 240px">
					<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
						BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
						Style="vertical-align: middle; text-align: center" Width="238px">
					</asp:Label>
				</td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 120px; text-align: left">
					<asp:Button ID="btReturn" runat="server" Font-Bold="True" Font-Size="Medium" 
						Height="30px" Text="Return" Width="110px" />
				</td>
				<td style="width: 120px; text-align: left">
					<asp:Button ID="btRestart" runat="server" Font-Bold="True" Font-Size="Medium" 
						Height="30px" Text="Restart" Width="110px" Visible="False"/>
				</td>
			</tr>
		</table>
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 240px; text-align: center; height: 12px;" >
					<asp:Label ID="lbGridTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed"
						Text="Current Pallet - Scanned Products" Visible="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 240px; text-align: center;" >
					<asp:DataGrid ID="dgProductsScanned" runat="server" Width="234px" Font-Size="X-Small" Visible="False" PageSize="30">
						<AlternatingItemStyle BackColor="Gainsboro" />
						<HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
					</asp:DataGrid>
				</td>
			</tr>
		</table><table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 240px; text-align: center;" >
					<asp:Label ID="lbExistingGridTitle" runat="server" Font-Bold="True" Font-Size="X-Small"
						ForeColor="DarkRed" Text="Existing Pallet - Products" Visible="False"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 240px; text-align: center;" >
					<asp:DataGrid ID="dgExistingProducts" runat="server" Width="234px" Font-Size="X-Small" Visible="False" PageSize="30">
						<AlternatingItemStyle BackColor="Gainsboro" />
						<HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
					</asp:DataGrid>
				</td>
			</tr>
		</table>
	</div>
   </form>
</body>
</html>
