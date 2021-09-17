<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_WorkWithSamplePalletNew.aspx.vb" Inherits="OWS_IC.IC_WorkWithSamplePalletNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
        <div title="IC Whse Transfer" class="container-fluid" style="padding:0px">
            <table class="table table-striped">
		        <tr class="row flex-fill align-content-center">
			        <td style="text-align: center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: center" Text="OW Inventory - Work With Sample Pallet" EnableViewState="False" >
					    </asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill align-content-center">
			        <td style="text-align: center">
				        <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
						    Style="vertical-align: middle; text-align: center" Text="User ID : " >
				        </asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill align-content-center">
			        <td style="text-align: center">
				        <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: center" Text="Scan or Enter Sample Pallet #" >
				        </asp:Label>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="2em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Pallet #-" EnableViewState="False">
				        </asp:Label>
			        </td>
			        <td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="2em"
					        ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10">
				        </asp:TextBox></td>
		        </tr>
		        <tr class="row flex-fill">
			        <td class="col-6 offset-1 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Label ID="lbAddProduct" runat="server" Font-Size="2em" ForeColor="DarkRed"
					        Text="1. Add Product to Pallet" Visible="False">
				        </asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill">
			        <td class="col-6 offset-1 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Label ID="lbChgProductQty" runat="server" Font-Size="2em" ForeColor="DarkRed" Text="2. Change Product Qty on Pallet"
					        Visible="False">
				        </asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill">
			        <td class="col-6 offset-1 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Label ID="lbRmvProduct" runat="server" Font-Size="2em" ForeColor="DarkRed" Text="3. Remove Product from Pallet"
					        Visible="False">
				        </asp:Label>
			        </td>
			        <td class="col-2 d-flex flex-column text-end" style="border-style: hidden">
				        <asp:Label ID="lbOption" runat="server" Font-Bold="True" Font-Size="2em" Text="Option -"
					        Visible="False">
				        </asp:Label>
			        </td>
			        <td class="col-2 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:TextBox ID="txOption" runat="server" AutoPostBack="True" BorderColor="Black"
					        BorderWidth="1px" Font-Bold="False" Font-Size="2em" ForeColor="Black"
					        Visible="False" Wrap="False">
				        </asp:TextBox>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill">
			        <td class="col-12" style="border-style: hidden;">
				        <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
					        BorderWidth="1px" Font-Bold="False" Font-Size="2em" ForeColor="Red" 
					        Style="vertical-align: middle; text-align: center" Visible="False">
				        </asp:Label>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btReturn" runat="server" Font-Size="3em" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-start" style="border-style: hidden">
				        <asp:Button ID="btRestart" runat="server" Font-Size="3em" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
			        </td>
		        </tr>
	        </table>
		    <table id="TABLE6" class="table" border="0" cellpadding="0" cellspacing="0" language="javascript" onclick="return TABLE6_onclick()">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbExistingGridTitle" runat="server" Font-Bold="True" Font-Size="2em"
						    ForeColor="DarkRed" Text="Existing Pallet - Products" Visible="False">
					    </asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:DataGrid ID="dgExistingProducts" runat="server" PageSize="30" Visible="False" AutoGenerateColumns="false" Width="100%">
						    <AlternatingItemStyle BackColor="Gainsboro" />
                            <ItemStyle Font-Size="3em" />
						    <HeaderStyle Font-Size="2em" BackColor="Navy" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundColumn DataField="ProductID" HeaderText="Prod ID" HeaderStyle-Width="20%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CaseQty" HeaderText="Case Qty" HeaderStyle-Width="20%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ProdDesc" HeaderText="Product Description" HeaderStyle-Width="60%"></asp:BoundColumn>
                            </Columns>
					    </asp:DataGrid>
				    </td>
			    </tr>
		    </table>
	    </div>
    </form>
</body>
</html>
