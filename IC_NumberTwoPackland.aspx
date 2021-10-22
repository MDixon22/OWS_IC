<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_NumberTwoPackland.aspx.vb" Inherits="OWS_IC.IC_NumberTwoPackland" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Number Two" style="text-align: left">
    <table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - New # 2 Scanning" EnableViewState="False" ></asp:Label>&nbsp;
                <asp:Label ID="lbLocation" runat="server" BorderStyle="None" Font-Bold="True" ForeColor="Black" Style="text-align: left">Packland</asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="Scan Case Label or Press MIXED button" ></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6">
				</asp:TextBox></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center;">
				<asp:Button ID="btVerifyFinished" runat="server" Text="Verified - Yes" Font-Bold="False" Visible="False" />
                <asp:Button ID="btMixed" runat="server" Text="MIXED" Font-Bold="False" /></td>
		</tr>
	</table>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
        <tr class="row flex-fill">
            <td class="col-2" style="text-align: left;">
                <asp:Label ID="lbDistressed" runat="server" Font-Bold="False" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="1. Distressed Product" Visible="False"></asp:Label></td>
            <td class="col-2" style="text-align: left;">\
				<asp:Label ID="lbTest" runat="server" Font-Bold="False" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="2. Test Product" Visible="False"></asp:Label></td>
        </tr>
        <tr class="row flex-fill">
            <td class="col-2" style="text-align: left;">
                <asp:Label ID="lbDefective" runat="server" Font-Bold="False" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="3. Defective Product"
                    Visible="False"></asp:Label></td>
            <td class="col-2" style="text-align: left;">
                <asp:Label ID="lbEndOfRunPartial" runat="server" Font-Bold="False" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="4. End Of Run/Partial Case"
                    Visible="False"></asp:Label></td>
        </tr>
    </table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-3" style="text-align: right;">
			        <asp:Label ID="lbFormula" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Visible="False" BorderStyle="None">
					</asp:Label></td>
			<td class="col-3" style="text-align: right;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text=" : " Visible="False"></asp:Label></td>
			<td class="col-3" style="text-align: left;">
				    <asp:Label ID="lbProdDesc" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Visible="False" BorderStyle="None">
					</asp:Label></td>
		</tr>
	</table>
		<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
        <tr class="row flex-fill">
            <td class="col-3" style="text-align: right;">
            </td>
            <td class="col-3" style="text-align: right;">
                <asp:Label ID="lbSplit" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="--" Visible="False"></asp:Label></td>
            <td class="col-3" style="width: 130px; text-align: left;">
            </td>
        </tr>
    </table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right;">
				<asp:Label ID="lbLot" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Lot -" Visible="False"></asp:Label>
			</td>
			<td class="col-2" style="text-align: left;">
				<asp:Label ID="lbLotData" runat="server" Font-Bold="False" ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right;">
				<asp:Label ID="lbPounds" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Pounds -" Visible="False"></asp:Label>
			</td>
			<td class="col-2" style="text-align: left;">
				<asp:Label ID="lbPoundsData" runat="server" Font-Bold="False" ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label><asp:Label ID="lbFunction" runat="server" Font-Bold="False" Style="text-align: left" Visible="False" Text="0" Width="1px" Font-Size="X-Small"></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
            <td class="col-2" style="text-align: right;">
                <asp:Label ID="lbReason" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Visible="False">ReasonCode</asp:Label></td>
            <td class="col-2" style="text-align: left;">
                <asp:Label ID="lbReasonCode" runat="server" Font-Bold="False" ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label></td>
        </tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: left;">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False">
				</asp:Label></td>
		</tr>
	</table>
	<table class="table table-striped" style="border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: left;">
				<asp:Button ID="btReturn" runat="server" Text="To Menu" Font-Bold="True" EnableViewState="False" />
			</td>
			<td class="col-2" style="text-align: right;">
				<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
	</div>
   </form>
</body>
</html>
