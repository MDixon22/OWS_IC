<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PalletReceiptNew.aspx.vb" Inherits="OWS_IC.IC_PalletReceiptNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="form1" runat="server">
		<div class="container-fluid">
			<table class="table table-striped">
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center">
						<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="OWS Inventory Management Shipping Receipt" ></asp:Label>
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
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #" ></asp:Label>
				    </td>
			    </tr>
		    </table>
		    <table class="table">
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Pallet # :"></asp:Label>
				    </td>
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
                            Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
				    </td>
                    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="CaseLabel :"></asp:Label>
				    </td>
				    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txCaseLabel" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="Black" 
						    Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px"></asp:TextBox>
				    </td>
			    </tr>
                <tr class="row flex-fill justify-content-center">
                    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbProdDesc" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" Visible="False" 
				            Style="vertical-align: middle; text-align: center" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </td>
                </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbToBin" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
						    Style="vertical-align: middle; text-align: right" Text="Recv To Bin :" Visible="False"></asp:Label>
				    </td>
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbToBinVal" runat="server" BackColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
						    Font-Bold="True" Font-Size="3em" Style="vertical-align: middle; text-align: center" Text="WHSE" Visible="False"></asp:Label>
				    </td>
                    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden"></td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbFullLayers" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
						    Style="vertical-align: middle; text-align: right" Text="Full Layers-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txFullLayers" runat="server" Font-Size="3em" Font-Bold="False" ForeColor="Black" 
						    BorderColor="Black" AutoPostBack="True" Visible="False" Wrap="False" BorderWidth="1px"></asp:TextBox>
				    </td>
				    <td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:Label ID="lbLooseBoxes" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Loose Cases-" Visible="False"></asp:Label>
				    </td>
				    <td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
					    <asp:TextBox ID="txLooseBoxes" runat="server" Font-Size="3em" Font-Bold="False" ForeColor="Black" Wrap="False"
						    BorderColor="Black" BorderWidth="1px" AutoPostBack="True" Visible="False" MaxLength="4" ></asp:TextBox>
				    </td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
			    </tr>
                <tr class="row flex-fill justify-content-center">
                    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbResult" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Visible="False" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Black" Style="vertical-align: middle; text-align: center"></asp:Label>
				    </td>
                </tr>
				<tr class="row flex-fill justify-content-center">
                    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Visible="False" 
                            Font-Bold="False" Font-Size="3em" ForeColor="Red" Style="vertical-align: middle; text-align: center"></asp:Label>
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
