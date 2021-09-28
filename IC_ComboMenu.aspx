<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboMenu.aspx.vb" Inherits="OWS_IC.IC_ComboMenu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cut Down Combo Menu</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid">
    <form id="form1" runat="server" >
    <div>
		<table class="table table-striped" style="">
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - Cut-Down Combo Menu" EnableViewState="False"></asp:Label></td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True"  ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : "></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-1" style="text-align: center">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Select Option From List"></asp:Label>
				</td>
			</tr>
		</table>
		<table class="table table-striped" style="">
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption1" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="1. Build Combo"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption2" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="2. Transfer Combo to Weeden Creek"></asp:Label>
				</td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption3" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="3. Receive Combo Back From Weeden Creek">
					</asp:Label></td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption4" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="4. Void Cut Down Combo Errors"></asp:Label></td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption5" runat="server" 
						Font-Bold="False" ForeColor="DarkRed" Text="5. Void Combo Consumed by Line">
					</asp:Label></td>
			</tr>
			<tr class="row flex-fill">
				<td class="col-2" style=""></td>
				<td class="col-2" style="">
					<asp:Label ID="lbOption6" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="6. Return Problem Combo to Plant2">
					</asp:Label></td>
			</tr>
		</table>
		<table class="table table-striped" style="">
		    <tr class="row flex-fill">
		        <td class="col-2" style="text-align: right;">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Option -"></asp:Label></td>
				<td class="col-2" style="">
                    <asp:TextBox ID="txOption" runat="server" Columns="1" AutoPostBack="True" BorderWidth="1px"></asp:TextBox></td>
		    </tr>
		</table>		
			<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" 
				Font-Bold="False" ForeColor="Red" Visible="False" Style="vertical-align: middle; text-align: center">
			</asp:Label>
		<table class="table table-striped" style="">
			<tr class="row flex-fill">
				<td class="col-2" style="text-align: center;"></td>
				<td class="col-2" style="text-align: center;">
					<asp:Button ID="btExit" runat="server" Text="Log Off" Font-Bold="True" EnableViewState="False" />
				</td>
			</tr>
		</table>
		</div>
    </form>
</body>
</html>
