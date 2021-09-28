<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboXfer_WC.aspx.vb" Inherits="OWS_IC.IC_ComboXfer_WC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC WC Combo Receipt" style="text-align: left;">
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0" bordercolor="#000000">
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True"	ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center" Text="OW Inventory - Combo Xfer to Weeden Creek" EnableViewState="False" ></asp:Label></td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : " BackColor="Transparent"></asp:Label>
					<asp:Label ID="lbFunction" runat="server" Text="0" Visible="False"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center; vertical-align: top;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Scan Xfer Truck barcode"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center;">
					<asp:TextBox ID="txData" runat="server" ForeColor="Black" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
				</td>
			</tr>
		</table>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
            <tr class="row flex-fill">
                <td class="col-2" style="text-align: left;">
                    <asp:Button ID="btPrintManifest" runat="server" Text="Print Manifest" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
                <td class="col-2" style="text-align: right;">
                    <asp:Button ID="btFinishTransferProcess" runat="server" Text="Finish Transfer" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
            </tr>
        </table>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center;">
					<asp:Label ID="lbXferTruckNum" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="Xfer Truck # : " EnableViewState="False">
					</asp:Label><asp:Label ID="lbXferTruckVal" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                    <asp:Label ID="lbXferTruckID" runat="server" Font-Bold="False" Visible="False">0</asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center;">
                    <asp:Label ID="lbCombosInTruck" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="Combos in Truck " BorderStyle="None" Visible="False"></asp:Label></td>
			</tr>
		</table>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center;">
                    <asp:GridView ID="gvCombos" runat="server" AutoGenerateColumns="False" CellPadding="1"
                        CellSpacing="1" PageSize="40" Visible="False">
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
				BorderWidth="1px" Font-Bold="False" ForeColor="Red"	Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
			<tr class="row flex-fill">
				<td class="col-2" style="text-align: left;">
					<asp:Button ID="btReturn" runat="server" Text="Combo Menu" Font-Bold="True" EnableViewState="False" />
			</td>
				<td class="col-2" style="text-align: right;">
					<asp:Button ID="btFinishedLoading" runat="server" Text="Loading Finished" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>
