<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_CycleCount.aspx.vb" Inherits="OWS_IC.IC_CycleCount" %>

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
    <div title="IC Counting" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
								Style="vertical-align: middle; text-align: center" Text="CBC Inventory Management Counting" EnableViewState="False" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black" 
						Style="vertical-align: middle; text-align: center" Text="User ID : " ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="Enter Batch #" ></asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 65px; vertical-align: bottom; text-align: right; height: 30px;">
				<asp:Label ID="lbBatch" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Batch #-" EnableViewState="False"></asp:Label>
			</td>
			<td style="width: 175px; vertical-align: bottom; height: 30px">
				<asp:TextBox ID="txBatch" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6"></asp:TextBox>&nbsp;
				<asp:Label
						ID="lbBatchVal" runat="server" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
						EnableViewState="False" Font-Bold="True" Font-Size="X-Small" ForeColor="Black"
						Style="vertical-align: middle; text-align: right" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 65px; vertical-align: bottom; text-align: right; height: 30px;">
				<asp:Label ID="lbPallet" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" 
					Text="Pallet #-" EnableViewState="False"></asp:Label>
			</td>
			<td style="width: 175px; vertical-align: bottom; height: 30px">
				<asp:TextBox ID="txPallet" runat="server" Font-Bold="False" Font-Size="XX-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="10"></asp:TextBox>
				<asp:Button ID="btNewBatch" runat="server"
						Font-Size="Medium" Text="New Batch" Width="97px" Font-Bold="True" EnableViewState="False" /></td>
		</tr>
		<tr>
			<td style="width: 65px; vertical-align: bottom; text-align: right; height: 30px;">
				<asp:Label ID="lbYN" runat="server" EnableViewState="False" Font-Bold="False" Font-Size="X-Small"
					ForeColor="MidnightBlue" Style="vertical-align: middle; text-align: right" Text="Activate Y/N -"
					Visible="False"></asp:Label></td>
			<td style="width: 175px; vertical-align: bottom; height: 30px">
				<asp:TextBox ID="txYN" runat="server" AutoPostBack="True" BorderColor="MidnightBlue"
					BorderWidth="1px" Columns="1" Font-Bold="False" Font-Size="XX-Small" ForeColor="MidnightBlue"
					Visible="False" Wrap="False"></asp:TextBox><asp:Label ID="lbProductDescription" runat="server"
					Font-Bold="True" Font-Size="X-Small" ForeColor="Black" Style="vertical-align: middle; text-align: right" EnableViewState="False" Visible="False"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 65px; text-align: right; height: 19px;">
				<asp:Label ID="lbCaseQty" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
					Style="vertical-align: middle; text-align: right" Text="Case Qty-" Visible="False"></asp:Label>&nbsp;</td>
			<td style="width: 175px; height: 25px;">
				<asp:TextBox ID="txCaseQty" runat="server" AutoPostBack="True" BorderColor="Black"
					BorderWidth="1px" Columns="15" Font-Size="XX-Small" ForeColor="Black" Visible="False"></asp:TextBox></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width: 65px; text-align: right; height: 25px;">
				<asp:Label ID="lbNewCaseQty" runat="server" Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed"
					Style="vertical-align: middle; text-align: right" Text="Validate Qty-" Visible="False"></asp:Label></td>
			<td style="width: 175px; height: 25px;">
				<asp:TextBox ID="txNewCaseQty" runat="server" AutoPostBack="True" BorderColor="Black"
					BorderWidth="1px" Columns="15" Font-Size="XX-Small" ForeColor="Black" Visible="False"></asp:TextBox></td>
		</tr>
		<tr>
			<td style="width: 65px; text-align: right">
				<asp:Label ID="lbToBin" runat="server"
					Font-Bold="False" Font-Size="X-Small" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="To Bin-" Visible="False"></asp:Label>&nbsp;</td>
			<td style="width: 175px; height: 25px;">
				<asp:TextBox ID="txToBin" runat="server" 
					Font-Size="XX-Small" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="15"></asp:TextBox></td>
		</tr>
	</table>
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
				BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
				Style="vertical-align: middle; text-align: center" Visible="False" Width="238px">
			</asp:Label><table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btReturn" runat="server" Font-Size="Medium" Height="35px" 
						Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="text-align: right; height: 30px; width: 120px;">
				<asp:Button ID="btRestart" runat="server"
						Font-Size="Medium" Height="35px" Text="Restart Entry" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
</div>
   </form>
</body>
</html>