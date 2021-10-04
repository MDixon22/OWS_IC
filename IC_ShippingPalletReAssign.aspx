<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ShippingPalletReAssign.aspx.vb" Inherits="OWS_IC.IC_ShippingPalletsReAssign" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
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
            for (i = 0; i < strChar.length && IsNumber == true; i++) {
                strAt = strChar.charAt(i);
                if (strValidChars.indexOf(strAt) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }

        function moveNext(textBoxId, setFocusTextBoxId, labelErrorId) {
            var strlbPrompt = "lbPrompt";
            document.getElementById(labelErrorId).innerHTML = "";
            if (chkNumbers(textBoxId)) {
                if (setFocusTextBoxId == "txPallet") {
                    document.getElementById(strlbPrompt).innerHTML = "Scan or Enter Existing Pallet.";
                }
                setfocus(setFocusTextBoxId);
            }
            else {
                document.getElementById(labelErrorId).innerHTML = "Entry made in '" + textBoxId + "' must be numeric!";
                setfocus(textBoxId);
            }
        }
    </script>
</head> 
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="form1" runat="server">
		<div title="IC Shipping Pallets" class="container-fluid">
			<table class="table table-striped">
				<tr class="row flex-fill justify-content-center">
					<td style="text-align: center;">
						<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" Style="vertical-align: bottom; 
                            text-align: center" Text="OW Inventory - Shipping Pallet ReAssign" EnableViewState="False" EnableTheming="False"></asp:Label>
					</td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td style="text-align: center;">
						<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
							Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label>
					</td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td style="text-align: center;">
						<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="Scan New Pick Order"></asp:Label>
					</td>
				</tr>
			</table>
			<table class="table">
				<tr class="row flex-fill justify-content-center">
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbOrder" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right" Text="Pick Order-"></asp:Label>
					</td>
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<input id="htxOrder" type="text" onchange="moveNext('htxOrder','txPallet','lbError')" runat="server" 
                            enableviewstate="true" style="font-size: 3em; font-family: Verdana" visible="true" />
					</td>
                    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right;" Text="Existing Pallet-" Visible="False"></asp:Label>
					</td>
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txPallet" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True"></asp:TextBox>
					</td>
                    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False">
						</asp:Label>
					</td>
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txBin" runat="server" AutoPostBack="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Visible="False" Wrap="False"></asp:TextBox>
                    </td>
                    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbPrinter" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False">
						</asp:Label>
                    </td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:TextBox ID="txPrinter" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Visible="False" Wrap="False" AutoPostBack="True">
						</asp:TextBox>
                    </td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbError" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Red"
							Style="vertical-align: middle; text-align: center"></asp:Label>
					</td>
				</tr>
				<tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btReturn" runat="server" Font-Size="3em" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btRestart" runat="server" Font-Size="3em" Text="Restart" Font-Bold="True" EnableViewState="False" />
			        </td>
		        </tr>
			</table>
		</div>
	</form>
</body>
</html>
