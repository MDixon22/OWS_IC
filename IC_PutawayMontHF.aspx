<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PutawayMontHF.aspx.vb" Inherits="OWS_IC.IC_PutawayMontHF" %>

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
						<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" EnableViewState="False"
							Style="vertical-align: middle; text-align: center" Text="OW Inventory - Product Putaway"></asp:Label>
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
								Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pallet #">
						</asp:Label>
					</td>
				</tr>
			</table>
			<table class="table">
				<tr class="row flex-fill justify-content-center" style="margin-top:1em">
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed"
							Style="vertical-align: middle; text-align: right" Text="CaseLabel-" Visible="False"></asp:Label>
					</td>
					<td class="col-8 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txCaseLabel" runat="server" Font-Size="3em" ForeColor="Black" Font-Bold="False" 
                            AutoPostBack="True" BorderWidth="1px" BorderColor="Black"></asp:TextBox>
					</td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: right" Text="Qty-" Visible="False"></asp:Label>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txQuantity" runat="server" Font-Size="3em" Font-Bold="False"
							ForeColor="Black" BorderColor="Black" AutoPostBack="True" BorderWidth="1px">
						</asp:TextBox>
					</td>
                    <td class="col-7 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-9 offset-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbTimeDesc" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkBlue" Visible="False"
							Style="vertical-align: middle; text-align: center" Text="Start / Stop is in Military Time - HHMM"></asp:Label>
					</td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbStart" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: right" Text="Start-" Visible="False"></asp:Label>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txStart" runat="server" Font-Size="3em" ForeColor="Black" Font-Bold="False" 
							MaxLength="4" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="4">
						</asp:TextBox>
					</td>
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbStop" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: right" Text="Stop-" Visible="False"></asp:Label>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txStop" runat="server" Font-Size="3em" Font-Bold="False" ForeColor="Black" 
							MaxLength="4" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="4">
						</asp:TextBox>
					</td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbToBin" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: right" Text="To Bin-" Visible="False"></asp:Label>
					</td>
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbToBinName" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
                            Font-Bold="True" Font-Size="3em" Style="vertical-align: middle; text-align: center" Text="OWFG"></asp:Label>
					</td>
					<td class="col-3 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbPrinter" runat="server" Font-Bold="False" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: right" Text="Printer-" Visible="False"></asp:Label>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txPrinter" runat="server" Font-Size="3em" ForeColor="Black" 
                            BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Font-Bold="False">
						</asp:TextBox>
					</td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
			    <tr class="row flex-fill justify-content-center">
                    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px"
                            Font-Size="3em" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False">
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
