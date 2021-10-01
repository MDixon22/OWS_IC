<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ProductLocator.aspx.vb" Inherits="OWS_IC.IC_ProductLocator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="form1" runat="server">
		<div title="IC Product Locator" class="container-fluid">
			<table class="table table-striped">
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center;">
						<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" Style="vertical-align: middle; 
							text-align: center" Text="CBC Inventory Management - " EnableViewState="False" EnableTheming="False" ></asp:Label>
                        <asp:Label ID="lbMode" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
                            Style="vertical-align: middle; text-align: center" Text="Product Locator" EnableViewState="False" ></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center;">
					    <asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" 
							Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
					    <asp:Label ID="lbReturnURL" runat="server" Font-Size="3em" Text="'" Visible="False" Width="1px"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill justify-content-center">
				    <td style="text-align: center;">
					    <asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="DarkRed" 
							Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pick Product" ></asp:Label>
				    </td>
			    </tr>
		    </table>
			
		    <table class="table">
                <tr class="row flex-fill justify-content-center">
                    <td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
                        <asp:TextBox ID="txPickProduct" runat="server" Font-Bold="True" Font-Size="3em" ForeColor="Black" AutoPostBack="True" 
				            Style="vertical-align: top; text-align: left" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black"></asp:TextBox>
                    </td>
                </tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-8 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbLastUser" runat="server" BackColor="White" BorderColor="Black" ForeColor="Black"
							Font-Bold="False" Font-Size="3em" Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label>
					</td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lblBinLocation" runat="server" BorderColor="Black" Font-Bold="False" Font-Size="3em" 
							ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False">
						</asp:Label>
					</td>
					<td class="col-4 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbBinLocation" runat="server" BorderColor="Black" Font-Bold="False" Font-Size="3em" 
							ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False">
						</asp:Label>
					</td>
					<td class="col-5 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Button ID="btSkip" runat="server" Font-Bold="True" Font-Size="3em" Text="Skip" Visible="False" />
					</td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
				<tr class="row flex-fill justify-content-center">
					<td class="col-6 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbSupOvr" runat="server" BackColor="White" BorderColor="Black" Font-Bold="False" Font-Size="3em" 
							ForeColor="Black" Style="vertical-align: middle; text-align: right" Visible="False" Text="Supervisor Override">
						</asp:Label>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txSupOvrSkip" runat="server" TextMode="Password" Visible="False" AutoPostBack="True" Font-Size="3em">
						</asp:TextBox>
					</td>
					<td class="col-2 d-flex flex-column text-center" style="border-style: hidden">
						<asp:Label ID="lbAllow" runat="server" Font-Size="3em" Text="Allow" Visible="False">
						</asp:Label>
					</td>
					<td class="col-1 d-flex flex-column text-center" style="border-style: hidden">
						<asp:TextBox ID="txSkipYN" runat="server" Columns="1" Font-Size="3em" Visible="False" AutoPostBack="True">
						</asp:TextBox>
					</td>
                    <td class="col-1 d-flex flex-column text-center" style="border-style: hidden"></td>
				</tr>
                <tr class="row flex-fill justify-content-center">
					<td class="col-11 d-flex flex-column text-center" style="border-style: hidden">
			            <asp:DataGrid ID="dgProductLocator" runat="server" Visible="False" EnableViewState="False" Font-Size="2em">
				            <PagerStyle Font-Bold="True" Font-Size="2em" ForeColor="Maroon" HorizontalAlign="Center" 
                                NextPageText="&amp;gtNext" PrevPageText="Back&amp;lt;" VerticalAlign="Middle" Wrap="False" />
			            </asp:DataGrid>
                    </td>
				</tr>
                <tr class="row flex-fill justify-content-center">
					<td class="col-10 d-flex flex-column text-center" style="border-style: hidden">
			            <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Visible="False" 
                            Font-Bold="False" Font-Size="X-Small" ForeColor="Red" Style="vertical-align: middle; text-align: center"></asp:Label>
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