<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PhysicalCount17.aspx.vb" Inherits="OWS_IC.IC_PhysicalCount17" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="2" rightmargin="2" topmargin="0">
    <form id="form1" runat="server">
        <div class="container-fluid">
		    <table class="table table-striped">
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="OW Inventory - FG Physical Count" EnableViewState="False">
					    </asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
					    <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
							Style="vertical-align: middle; text-align: center" Text="User ID : ">
					    </asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
					    <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Bin Location">
					    </asp:Label>
				    </td>
			    </tr>
		    </table>
		    <table class="table">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:TextBox ID="txData" runat="server" AutoPostBack="True" BorderColor="Black" BorderWidth="1px"
                            Columns="43" Font-Bold="False" Font-Size="3em" ForeColor="Black"></asp:TextBox>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-9 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbBinValue" runat="server" Font-Bold="False" Font-Size="3em"
                            ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
                        <asp:Label ID="lbFunction" runat="server" Font-Bold="False" Font-Size="3em"
                            ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False">0</asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
						    Style="vertical-align: middle; text-align: right" Text="Pallet-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-9 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbPalletValue" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black"
                            Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbSKU" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black"
                            Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
                        <asp:Label ID="lbDash" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black"
                            Style="vertical-align: middle; text-align: center" Visible="False"> - </asp:Label>
				    </td>
				    <td class="col-8 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbProdDesc" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black"
                            Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Qty-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-9 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbQuantityValue" runat="server" Font-Bold="False" Font-Size="3em"
                            ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btNextLocation" runat="server" Font-Size="3em" Font-Bold="True"
						    Text="Next Location" EnableViewState="False" Visible="False" />
				    </td>
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btnVerified" runat="server" Font-Size="3em" Font-Bold="True" 
						    Text="Verified Count" EnableViewState="False" Visible="False" />
				    </td>
			    </tr>
		        <tr class="row flex-fill justify-content-center">
		            <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" ForeColor="Red" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: center" Visible="False">
					    </asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
					<td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
						<asp:Button ID="btReturn" runat="server" Font-Size="3em" Text="Return" Font-Bold="True" EnableViewState="False" />
					</td>
					<td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
						<asp:Button ID="btRestart" runat="server" Font-Size="3em" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
					</td>
				</tr>
		    </table>
        </div>
    </form>
</body>
</html>
