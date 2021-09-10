<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ProductLocator.aspx.vb" Inherits="OWS_IC.IC_ProductLocator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
    <%--<script language="javascript" type="text/javascript">
		function keepMeAlive(){
			if (document.getElementById('keepAliveIMG')) {
				document.getElementById('keepAliveIMG').src = 'someimg.gif?x=' + escape(new Date());
			}
		}
	window.setInterval("keepMeAlive()", 90000);
    </script>--%>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Product Locator" style="text-align: left">
		<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center; width: 238px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="CBC Inventory Management -   " EnableViewState="False" EnableTheming="False" ></asp:Label><asp:Label ID="lbMode" runat="server" 
						Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" 
						Text="Product Locator" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 238px;">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label>
                <asp:Label ID="lbReturnURL"
							runat="server" Font-Size="XX-Small" Text="'" Visible="False" Width="1px"></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center; width: 238px;">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Pick Product" ></asp:Label></td>
		</tr>
	</table>
		<asp:TextBox ID="txPickProduct" runat="server" BorderColor="Black" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: top;
			text-align: left" AutoPostBack="True" BorderStyle="Solid" BorderWidth="1px" Columns="45" Width="236px"></asp:TextBox>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 35px; text-align: right; height: 21px;">
				</td>
				<td style="width: 205px; text-align: left; height: 21px;" >
					<asp:Label ID="lbLastUser" runat="server" BackColor="White" BorderColor="Black"
						Font-Bold="False" Font-Size="XX-Small" ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False"></asp:Label></td>
				<td style="text-align: center; height: 21px;">
				</td>
			</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 35px; text-align: right">
					<asp:Label ID="lblBinLocation" runat="server" BackColor="White" BorderColor="Black" Font-Bold="False" Font-Size="XX-Small" 
						ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Bin-" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 80px; text-align: left">
					<asp:Label ID="lbBinLocation" runat="server" BackColor="White" BorderColor="Black" Font-Bold="False" Font-Size="XX-Small" 
						ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False">
					</asp:Label>
				</td>
				<td style="text-align: center; width: 125px;">
					<asp:Button ID="btSkip" runat="server" Font-Bold="True" Font-Size="Medium" Text="Skip" Width="120px" Visible="False" />
				</td>
			</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 35px; text-align: right">
				</td>
				<td style="width: 85px; text-align: right">
					<asp:Label ID="lbSupOvr" runat="server" BackColor="White" BorderColor="Black" Font-Bold="False" Font-Size="XX-Small" 
						ForeColor="Black" Style="vertical-align: middle; text-align: left" Visible="False" Text="Supervisor Override">
					</asp:Label>
				</td>
				<td style="width: 65px; text-align: center">
					<asp:TextBox ID="txSupOvrSkip" runat="server" Columns="8" TextMode="Password" Visible="False" AutoPostBack="True" Font-Size="XX-Small">
					</asp:TextBox>
				</td>
				<td style="width: 30px; text-align: center">
					<asp:Label ID="lbAllow" runat="server" Font-Size="XX-Small" Text="Allow" Visible="False">
					</asp:Label>
				</td>
				<td style="width: 20px; text-align: center">
					<asp:TextBox ID="txSkipYN" runat="server" Columns="1" Font-Size="XX-Small" Visible="False" AutoPostBack="True">
					</asp:TextBox>
				</td>
			</tr>
	</table>
		<asp:DataGrid ID="dgProductLocator" runat="server" Height="132px" Width="240px" Visible="False" EnableViewState="False" Font-Size="Small">
			<PagerStyle Font-Bold="True" Font-Size="Small" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Maroon" HorizontalAlign="Center" NextPageText="&amp;gtNext" PrevPageText="Back&amp;lt;" VerticalAlign="Middle" Wrap="False" />
		</asp:DataGrid>
		<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
			BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red"
			Style="vertical-align: middle; text-align: center" Visible="False" Width="238px"></asp:Label>
		<table style="width: 240px; height: 35px;" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="width: 120px; text-align: left; height: 30px;">
					<asp:Button ID="btReturn" runat="server" Font-Bold="True" Font-Size="Medium" Text="Return" Width="115px" EnableViewState="False" /></td>
				<td style="width: 120px; text-align: right; height: 30px;">
					<asp:Button ID="btRestart" runat="server" Font-Bold="True" Font-Size="Medium" Text="Restart Entry" Width="115px" EnableViewState="False" /></td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>