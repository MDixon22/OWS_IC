<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_RepackPalletComponents.aspx.vb" Inherits="OWS_IC.IC_RepackPalletComponents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
        <div title="IC Repack Pallet Components" class="container-fluid">
            <table class="table table-striped">
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" EnableViewState="False" 
							Style="vertical-align: middle; text-align: center" Text="OW Inventory - Repack Pallet Components" ></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center">
				        <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
						    Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center">
				        <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: center" Text="Scan or Enter RePack FG Pallet" ></asp:Label>
                        <asp:Label ID="lbFunction" runat="server" Text="0" Visible="False" Width="1px" Font-Size="3em"></asp:Label>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbFG_RPKPallet" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed"
                            Style="vertical-align: middle; text-align: center" Text="RePack FG Pallet# : " Visible="False"></asp:Label>
			        </td>
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbFG_RPKPalletVal" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
                            Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
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
			        <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbComponentsGridTitle" runat="server" Font-Bold="True" Font-Size="3em"
					        ForeColor="DarkRed" Text="Components in FG" Visible="False"></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:DataGrid ID="dgComponentsUsed" runat="server" Font-Size="2em" PageSize="20" Visible="False" 
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ShowHeader="False">
					        <AlternatingItemStyle BackColor="Gainsboro" />
					        <HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
				        </asp:DataGrid>
                    </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbComponentsScanned" runat="server" Font-Bold="True" Font-Size="3em"
                            ForeColor="DarkRed" Text="Components Scanned" Visible="False"></asp:Label>
                    </td>
                    <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Button ID="btFinish" runat="server" Text="Finished" Font-Bold="True" Font-Size="3em" />
                    </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:DataGrid ID="dgComponentsScanned" runat="server" Font-Size="2em" PageSize="20"
			                Visible="False" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ShowHeader="False">
                            <AlternatingItemStyle BackColor="Gainsboro" />
                            <HeaderStyle BackColor="Navy" Font-Bold="True" ForeColor="White" />
                        </asp:DataGrid>
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
