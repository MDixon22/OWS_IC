<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="OWS_IC._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body>
	<form id="form1" runat="server" defaultbutton="btRestart">
		<div title="Default Page" class="container-fluid flex-fill" style="text-align:left" >
		    <table class="table table-striped" style="font-size:3em">
			    <tr class="row flex-fill align-content-center">
				    <td class="col-12 text-center">
					    <asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center;" Text="OW Inventory - Log On" EnableViewState="False"></asp:Label>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-12 text-center">
				        <asp:Label ID="lbDirections" runat="server" Font-Bold="True" ForeColor="DarkRed" Text="Scan or Enter UserID" 
				        Style="vertical-align: middle; text-align: center;"></asp:Label>
				    </td>
			    </tr>
		    </table>
		    <table class="table">
			    <tr class="row flex-fill align-content-center">
				    <td class="col-3 text-end" style="border-style: hidden;">
					    <asp:Label ID="lbUserID" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="UserID" Style="vertical-align: middle; text-align: right; font-size: 3em;" EnableViewState="False"></asp:Label>
				    </td>
				    <td class="col-6 text-center" style="border-style: hidden;">
					    <asp:TextBox ID="txUserID" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid" Font-Bold="False" AutoPostBack="True" TextMode="Password" style="font-size:3em;" ></asp:TextBox>
				    </td>
			    </tr>
			    <tr class="row flex-fill align-content-center">
				    <td class="col-3 text-end" style="border-style: hidden;">
					    <asp:Label ID="lbPassword" runat="server" Font-Bold="False" ForeColor="DarkRed" Text="Password :" Visible="False" Style="vertical-align: middle; text-align: right; font-size:3em;" EnableViewState="False"></asp:Label>
				    </td>
				    <td class="col-6 text-center" style="border-style: hidden;">
					    <asp:TextBox ID="txPassword" runat="server" MaxLength="10" pie:Maxlength="10" BorderColor="Black" BorderStyle="Solid" Font-Bold="False" AutoPostBack="True" TextMode="Password" Style="font-size:3em;" ></asp:TextBox>
				    </td>
			    </tr>
		    </table>
		    <div class="row flex-fill justify-content-center" style="margin-top: 2em">
			    <div class="col-10 d-grid">
				    <asp:Button ID="btRestart" runat="server" Font-Bold="True" Text="Restart" UseSubmitBehavior="False" Style="font-size:3em;" />
			    </div>
			    
		    </div>
            <div class="row flex-fill justify-content-center" style="margin-top: 2em">
                <div class ="col-10 d-grid">
				    <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" Font-Bold="False" Font-Size="2em" ForeColor="Red" Visible="false" Style="vertical-align: middle; text-align: center">
				    </asp:Label>
			    </div>
            </div>
		</div>
	</form>
</body>
</html>

