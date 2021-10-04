<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_SmokingRacks.aspx.vb" Inherits="OWS_IC.IC_SmokingRacks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
        <div title="IC Bin Transfer" class="container-fluid">
            <table class="table table-striped">
		        <tr class="row flex-fill justify-content-center">
			        <td style="text-align: center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="OW Smoking - Rack Processing" EnableViewState="False" ></asp:Label>
                        <asp:Label ID="lbProcessType" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Style="vertical-align: middle; text-align: right"></asp:Label>
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
						    Style="vertical-align: middle; text-align: center" Text="Scan or Enter Rack #" ></asp:Label>
			        </td>
		        </tr>
	        </table>
	        <table class="table">
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
                        <asp:Label ID="lbFunction" runat="server" Visible="False" Width="8px">0</asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Button ID="btIntoOven" runat="server" Font-Size="3em" Font-Bold="True"
                            Text="Into Oven" EnableViewState="False" Visible="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Button ID="btOutOfOven" runat="server" Font-Size="3em" Font-Bold="True" 
                            Text="Out Of Oven" EnableViewState="False" Visible="False" />
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center" style="padding-top: 3em">
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Button ID="btIntoBlast" runat="server" Font-Size="3em" Font-Bold="True" 
						        Text="Into Blast" EnableViewState="False" Visible="False" />
			        </td>
			        <td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Button ID="btIntoRTE" runat="server" Font-Size="3em" Font-Bold="True" 
                            Text="Into RTE" EnableViewState="False" Visible="False" />
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbRack" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Rack #-" Visible="False"></asp:Label>
			        </td>
			        <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbRackValue" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
                        <asp:Label ID="lbRackProduct" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbOven" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Oven #-" Visible="False"></asp:Label>
			        </td>
			        <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbOvenValue" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
                        <asp:Label ID="lbOvenDesc" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
			        <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
				        <asp:Label ID="lbOvenLocation" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Oven Location-" Visible="False"></asp:Label>
			        </td>
			        <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbOvenLocValue" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
                        <asp:Label ID="lbOvenLocDesc" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                            Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label>
			        </td>
		        </tr>
		        <tr class="row flex-fill justify-content-center">
		            <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
			            <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" ForeColor="Red" 
                            Font-Bold="False" Font-Size="2em" Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
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
	    <%--<img id="keepAliveIMG" height="0" src="someimg.GIF" />--%>
        </div>
    </form>
</body>
</html>
