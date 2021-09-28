<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_Menu.aspx.vb" Inherits="OWS_IC.IC_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server" >
        <div class="container-fluid">
		    <table class="table table-striped">
			    <tr class="row flex-fill align-content-center">
				    <td class="col-12 text-center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: center" Text="OW Inventory - Menu" EnableViewState="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-12 text-center">
					    <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
						    Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label>
				    </td>
			    </tr>
		    </table>
		    <table class="table justify-content-center">
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbFGPutaway" runat="server" 
						    Font-Bold="False" Font-Size="2em" ForeColor="DarkRed" Text="1. FG Putaway">
					    </asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbNewNumTwoProcess" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="11. New Number 2 Process"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbBinTransfer" runat="server" 
						    Font-Bold="False" Font-Size="2em" ForeColor="DarkRed" Text="2. Bin Transfer">
					    </asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbPacklandNumber2" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="DarkRed"
                            Text="12.PackLand Number 2"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbShippingPallet" runat="server" 
						    Font-Bold="False" Font-Size="2em" ForeColor="DarkRed" Text="3. Crt Shipping Pallet"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbCreateSamplePallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="13. Create Sample Pallet"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbWorkWithShippingPallet" runat="server" Font-Bold="False" Font-Size="2em"
						    ForeColor="DarkRed" Text="4. Wrk W/Shipping Pallet"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbWorkWithSamplePallet" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="DarkRed"
                            Text="14. Wrk W/Sample Pallet"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbWhseTransfer" runat="server" 
						    Font-Bold="False" Font-Size="2em" ForeColor="DarkRed" Text="5. Stock Pallet Transfer"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbRecvAbbylandPallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="15. Rcv CoManufactrd Pallet"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbAdjPalletQty" runat="server" Font-Bold="False" Font-Size="2em"
						    ForeColor="DarkRed" Text="6. Adj. Pallet Qty"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbComboMenu" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="16.CutDown Combo Menu"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbVoidPallet" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="DarkRed"
						    Text="7. Void Pallet"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbWrkShippingPallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="17. New-Wrk Shipping Pallet"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbNumberTwo" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="DarkRed"
						    Text="8. Number 2 Process"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbNewCrtSamplePallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="18. New-Crt Sample Pallet"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbTestNewFunction" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text=" 9. Test New FG Putaway"></asp:Label></td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbNewWrkSamplePallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="19. New-Wrk Sample Pallet"></asp:Label></td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbReAssignPallet" runat="server" Font-Bold="False" Font-Size="2em"
                            ForeColor="DarkRed" Text="10. ReAssign Pallet"></asp:Label></td>
			    </tr>
		    </table>		
		    <table class="table">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Button ID="btExit" CssClass="btn btn-secondary btn-block" runat="server" Font-Size="3em" Text="Log Off" Font-Bold="True" EnableViewState="False" />
				    </td>
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label id="Label1" runat="server" style="vertical-align: middle; text-align: right"
						    Text="Selection-" ForeColor="Black" Font-Size="3em" Font-Bold="True" ></asp:Label></td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:TextBox ID="txOption" runat="server" Columns="2" MaxLength="2" AutoPostBack="True" BorderColor="Black" BorderWidth="1px" Font-Size="3em"></asp:TextBox>
				    </td>
			    </tr>
                <tr class="row flex-fill align-content-center">
                    <td class="col-12 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" 
				            Font-Bold="False" Font-Size="2em" ForeColor="Red" Visible="False" Width="238px" 
				            Style="vertical-align: middle; text-align: center">
			            </asp:Label>
                    </td>
                </tr>
		    </table>
        </div>
    </form>
</body>
</html>
