<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_CycleCount.aspx.vb" Inherits="OWS_IC.IC_CycleCount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Counting" style="text-align: left">
    <table class="table table-striped" style="" border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="CBC Inventory Management Counting" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Enter Batch #" ></asp:Label></td>
		</tr>
	</table>
	<table class="table table-striped" style="" border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-2" style="vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbBatch" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Batch #-" EnableViewState="False"></asp:Label>
			</td>
			<td class="col-2" style="vertical-align: bottom;">
				<asp:TextBox ID="txBatch" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6"></asp:TextBox>&nbsp;
				<asp:Label
						ID="lbBatchVal" runat="server" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
						EnableViewState="False" Font-Bold="True" Font-Size="X-Small" ForeColor="Black"
						Style="vertical-align: middle; text-align: right" Visible="False">
				</asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbPallet" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet #-" EnableViewState="False"></asp:Label>
			</td>
			<td class="col-2" style="vertical-align: bottom;">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox>
				<asp:Button ID="btNewBatch" runat="server" Text="New Batch" Font-Bold="True" EnableViewState="False" /></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="vertical-align: bottom; text-align: right;">
				<asp:Label ID="lbYN" runat="server" EnableViewState="False" Font-Bold="False" ForeColor="MidnightBlue" Style="vertical-align: middle; text-align: right" Text="Activate Y/N -"
					Visible="False"></asp:Label></td>
			<td class="col-2" style="vertical-align: bottom;">
				<asp:TextBox ID="txYN" runat="server" AutoPostBack="True" BorderColor="MidnightBlue"
					BorderWidth="1px" Columns="1" Font-Bold="False" ForeColor="MidnightBlue"
					Visible="False" Wrap="False"></asp:TextBox><asp:Label ID="lbProductDescription" runat="server"
					Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: right" EnableViewState="False" Visible="False"></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right;">
				<asp:Label ID="lbCaseQty" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Case Qty-" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:TextBox ID="txCaseQty" runat="server" AutoPostBack="True" BorderColor="Black"
					BorderWidth="1px" Columns="15" ForeColor="Black" Visible="False"></asp:TextBox></td>
		</tr>
	</table>
	<table class="table table-striped" style="" border="0" cellpadding="0" cellspacing="0">
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right;">
				<asp:Label ID="lbNewCaseQty" runat="server" Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Validate Qty-" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:TextBox ID="txNewCaseQty" runat="server" AutoPostBack="True" BorderColor="Black" BorderWidth="1px" Columns="15" ForeColor="Black" Visible="False"></asp:TextBox></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="text-align: right">
				<asp:Label ID="lbToBin" runat="server"
					Font-Bold="False" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="To Bin-" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:TextBox ID="txToBin" runat="server" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="15"></asp:TextBox></td>
		</tr>
	</table>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False">
			</asp:Label><table class="table table-striped" style="" border="0" cellpadding="0" cellspacing="0">
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