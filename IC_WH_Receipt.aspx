<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_WH_Receipt.aspx.vb" Inherits="OWS_IC.IC_WH_Receipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
        <div title="IC Warehouse Receipt" class="container-fluid">
		    <table class="table table-striped">
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em"
						    ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center"
						    Text="OW Inventory - Transfer to Offsite Warehouse" EnableViewState="False" ></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
					    <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" 
						    ForeColor="Black" Style="vertical-align: middle; text-align: center" 
						    Text="User ID : " BackColor="Transparent"></asp:Label>
                        <asp:Label ID="lbFunction"
							    runat="server" Font-Size="XX-Small" Text="0" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center;">
					    <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" 
						    ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						    Text="Scan or Enter Warehouse" BackColor="Transparent"></asp:Label>
				    </td>
			    </tr>
            </table>
            <table class="table">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbWhse" runat="server" Font-Bold="True" Font-Size="3em" 
                            ForeColor="Black" Style="vertical-align: middle; text-align: right" 
							    Text="Warehouse : " EnableViewState="False"></asp:Label>
                    </td>
                    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbWhseVal" runat="server" Font-Bold="True" Font-Size="3em" 
                            Style="text-align: left" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txData" runat="server" Font-Size="2em" ForeColor="Black" 
                            BorderWidth="1px" BorderColor="Black" AutoPostBack="True">
					    </asp:TextBox>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Style="vertical-align: middle; text-align: right" Text="Pallet -" EnableViewState="False" Visible="False"></asp:Label>
				    </td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbPalletVal" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbQty" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
						    Style="vertical-align: middle; text-align: right" Text="Pallet Qty -" Visible="False">
						</asp:Label>
				    </td>
				    <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbQtyVal" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Style="vertical-align: middle; text-align: left" Visible="False">
					    </asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-5 offset-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btNewWhse" runat="server" Font-Size="3em" Text="New Whse" Font-Bold="True" EnableViewState="False" />
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-12 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				            BorderWidth="1px" Font-Bold="False" Font-Size="2em" ForeColor="Red"
				            Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
				    </td>
			    </tr>					
			    <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btReturn" runat="server" Font-Size="3em" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btRestart" runat="server" Font-Size="3em" Text="Restart Pallet" Font-Bold="True" EnableViewState="False" />
			        </td>
		        </tr>
		    </table>
		</div>
    </form>
</body>
</html>
