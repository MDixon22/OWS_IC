<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_AbbylandPallet.aspx.vb" Inherits="OWS_IC.IC_AbbylandPallet" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
	<script type="text/javascript">
        $('body').height(document.documentElement.clientHeight);
	</script>
</head>
<body class="d-flex flex-column min-vh-100" style="transform:rotate(90deg);">
	<div class="container-fluid flex-fill" title="Recv Co-Manufactured  Pallet">
    <form id="form1" runat="server">
		<table class="table table-striped" style="">
		<tr class="row flex-fill align-content-center">
			<td class="col-12 text-center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="" Text="OWS Inv Mgmt Rcv Co-Manufactured Pallet Tag" BackColor="White" EnableViewState="False"></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill text-center">
			<td class="col-12">
                <asp:Label ID="lbFunction" runat="server" Text="0" Visible="True" Width="8px"></asp:Label>
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" Style="" Text="User ID : ">
					</asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-12 d-flex flex-column text-center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="" Text="Scan or Enter Co-Manufactured Pallet #" BackColor="Transparent"></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-12 d-flex flex-column" style="padding-left:20em; padding-right:20em;">
				<asp:TextBox ID="txText" runat="server" AutoPostBack="True" BorderColor="Black" BorderWidth="1px" ForeColor="Black" style="width:inherit; padding-left:1em; padding-right:1em;"></asp:TextBox>
			</td>
		</tr>
    </table>
		<table class="table table-striped justify-content-center" style="">
			<tr class="row flex-fill">
				<td class="col-6 d-flex flex-column" style="text-align:right">
					<asp:Label ID="lbPallet" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="" Text="Pallet#-"></asp:Label>
				</td>
				<td class="col-6 d-flex flex-column" style="text-align:left;">
					<asp:Label ID="lbPalletVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle;">TEST</asp:Label>
				</td>				
			</tr>
			<tr class="row flex-fill">
				<td class="col-6 d-flex flex-column" style="text-align:right">
                    <asp:Label ID="lbCaseLabel" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="" Text="CaseLabel-" Visible="True"></asp:Label>
                </td>
                <td class="col-6 d-flex flex-column" style="text-align:left">
					<asp:Label ID="lbCaseLabelVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle;" Visible="True">test2</asp:Label>
                </td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-6 d-flex flex-column" style="text-align:right">
					<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="" Text="Quantity-" Visible="True"></asp:Label></td>
				<td class="col-6 d-flex flex-column" style="text-align:left">
					<asp:Label ID="lbQuantityVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle;" Visible="True">trueee</asp:Label>
                </td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-6 d-flex flex-column" style="text-align:right">
					<asp:Label ID="lbToBin" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="" Text="Warehouse-" Visible="True"></asp:Label>
				</td>
				<td class="col-6 d-flex flex-column" style="text-align:left">
                    <asp:Label ID="lbToBinVal" runat="server" Font-Bold="False" ForeColor="Black" Style="vertical-align: middle;" Visible="True">yyy</asp:Label></td>
			</tr>
		</table>
		<table style="" class="table table-striped">
			<tr class="row flex-fill">
			    <td class="col-6 d-flex flex-column text-center">
					<asp:Button ID="btFinished" runat="server" Text="Finish Pallet"  Font-Bold="True" Visible="True" />
			    </td>
				<td class="col-6 d-flex flex-column text-center">
					<asp:Button ID="btNextPallet" runat="server" Text="Next Pallet"  Font-Bold="True" Visible="True" />
				</td>
				<tr class="row flex-fill">
					<td class="col-12"></td>
				</tr>
			</tr>
		</table>
		<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="True"></asp:Label>
        <table style="" class="table table-striped">
			<tr class="row flex-fill">
				<td class="col-6 d-flex flex-column text-center">
					<asp:Button ID="btReturn" runat="server" Text="To Menu" Font-Bold="True" EnableViewState="False" />
				</td>
				<td class="col-6 d-flex flex-column text-center">
					<asp:Button ID="btRestart" runat="server" Text="Restart Entry" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
	</form>
</div>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>