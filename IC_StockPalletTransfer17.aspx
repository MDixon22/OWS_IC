<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_StockPalletTransfer17.aspx.vb" Inherits="OWS_IC.IC_StockPalletTransfer17"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
        <div title="IC Bin Transfer Stock Pallet To Truck" class="container-fluid">
            <table class="table table-striped">
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center;">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" EnableViewState="False" 
							Style="vertical-align: middle; text-align: center" Text="OW Inventory <br/>Load Stock Pallet on Transfer Truck" ></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center;">
				        <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
						        Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center;">
				        <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: center" 
                            Text="Use < > buttons to adjust Ship Date then Scan or Enter Truck Bin Location." ></asp:Label>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill justify-content-center">
		            <td class="col-1 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btMinus" runat="server" Font-Size="2.5em" Text=" < " Font-Bold="True" />
		            </td>
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbSelectShipDate" runat="server" EnableViewState="False" Font-Bold="False"
                            Font-Size="3em" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Ship Date-"></asp:Label>
			        </td>
			        <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbSelectedShipDate" runat="server" Font-Bold="True" Font-Size="3em"
                            ForeColor="Black" Style="vertical-align: middle; text-align: right" Text="7/16/2014"></asp:Label>
			        </td>
			        <td class="col-1 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btPlus" runat="server" Text=" > " Font-Bold="True" Font-Size="2.5em" />
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbTruckBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Truck Bin-" EnableViewState="False">
				        </asp:Label>
			        </td>
			        <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:TextBox ID="txTruckBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Pallet #-" EnableViewState="False" Visible="False"></asp:Label>
			        </td>
			        <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Visible="False"></asp:TextBox>
			        </td>
		        </tr>
	            <tr class="row flex-fill justify-content-center" style="padding-top: 1em">
	                <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
	                    <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" ForeColor="Red"
                            Font-Bold="False" Font-Size="2em" Style="vertical-align: middle; text-align: center" Visible="False">
			            </asp:Label>
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
	        </table>
	    </div>
    </form>
</body>
</html>
