<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ShippingPalletsNew.aspx.vb" Inherits="OWS_IC.IC_ShippingPalletsNew" %>
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

        function TABLE1_onclick() {

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
                            text-align: center" Text="OW Inventory - AddTo Shipping Pallet" EnableViewState="False" EnableTheming="False"></asp:Label>
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
						    Style="vertical-align: middle; text-align: center" Text="Select Button"></asp:Label>
				    </td>
			    </tr>
		    </table>
		    <table class="table">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Button ID="btNew" runat="server" Font-Bold="True" Font-Size="3em" Text="New Pallet" />
				    </td>
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Button ID="btComplete" runat="server" Font-Bold="True" Font-Size="3em" Text="Complete" Visible="False" />
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Button ID="btNo" runat="server" Font-Bold="True" Font-Size="4em" Text="- No -" Visible="False" />
				    </td>
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Button ID="btYes" runat="server" Font-Bold="True" Font-Size="4em" Text="- Yes -" Visible="False" />
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbShipPallet" runat="server" Font-Bold="True" Font-Size="3em" 
						    Style="vertical-align: middle; text-align: right" Text="Pallet #-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbShipPalletValue" runat="server" Font-Bold="True" Font-Size="3em" 
						    Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
				    </td>
                    <td class="col-1 d-flex flex-column" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbOrder" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right" Text="Pick Order-"></asp:Label>
				    </td>
				    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
					    <input id="htxOrder" type="text" onchange="moveNext('htxOrder','htxProduct','lbError')" 
						    runat="server" enableviewstate="true" style="font-size: 3em; font-family: Verdana" size="15" />
				    </td>
                    <td class="col-1 d-flex flex-column" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbProduct" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right;" Text="Pick Product-"></asp:Label>
				    </td>
				    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
					    <input id="htxProduct" type="text" onchange="moveNext('htxProduct','htxPallet','lbError')" 
						    runat="server" enableviewstate="true" style="font-size: 3em; font-family: Verdana" size="23" />
				    </td>
                    <td class="col-1 d-flex flex-column" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" Font-Size="3em"
						    ForeColor="DarkRed" Style="vertical-align: middle; text-align: right;" Text="Pallet-"></asp:Label>
				    </td>
				    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
					    <input id="htxPallet" type="text" onchange="moveNext('htxPallet','txQuantity','lbError')" 
						    runat="server" enableviewstate="true" style="font-size: 3em; font-family: Verdana" />
				    </td>
                    <td class="col-1 d-flex flex-column" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right;" Text="Case Qty-" Visible="False">
					    </asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txQuantity" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Columns="1" Font-Bold="False" Font-Size="3em" ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True">
					    </asp:TextBox>
				    </td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbQtyRemain" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="Red"
						    Style="vertical-align: middle; text-align: right" Text="- Remaining Qty - 00" Visible="False" 
						    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbGrossWt" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right;" Text="Gross Wt-" Visible="False">
					    </asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txGrossWt" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Columns="5" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True"></asp:TextBox>
				    </td>
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbDatePackaged" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right;" Text="Date Pkgd-" Visible="False">
					    </asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txDatePackaged" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Columns="5" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Wrap="False" Visible="False" AutoPostBack="True"></asp:TextBox>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False">
					    </asp:Label>
				    </td>
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txBin" runat="server" AutoPostBack="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Columns="10" Font-Bold="False" Font-Size="3em" ForeColor="Black" Visible="False" Wrap="False">
					    </asp:TextBox>
				    </td>
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbComplete" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right" Text="x" Visible="False"></asp:Label>
                        <asp:Label ID="lbPrinter" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txPrinter" runat="server" AutoPostBack="True" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Columns="2" Font-Bold="False" Font-Size="3em" ForeColor="Black" Visible="False" Wrap="False"></asp:TextBox>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbError" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Red"
						    Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btReturn" runat="server" Font-Size="3em" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btRestart" runat="server" Font-Size="3em" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
			        </td>
		        </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbGridTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed"
						    Text="Current Pallet - Scanned Products" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:DataGrid ID="dgProductsScanned" runat="server" Font-Size="2em" Visible="False" PageSize="30">
						    <AlternatingItemStyle BackColor="Gainsboro" />
						    <HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
					    </asp:DataGrid>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbExistingGridTitle" runat="server" Font-Bold="True" Font-Size="3em"
						    ForeColor="DarkRed" Text="Existing Pallet - Products" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:DataGrid ID="dgExistingProducts" runat="server" Font-Size="2em" Visible="False" PageSize="30">
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
