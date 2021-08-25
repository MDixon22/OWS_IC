<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_PhysicalCountValidate.aspx.vb" Inherits="OWS_IC.IC_PhysicalCountValidate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control Site</title>
</head><body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
<form id="form1" runat="server">
<div title="Component Pallets" style="text-align: left;">
		<table style="width: 1024px" border="0" cellpadding="0" cellspacing="0">
			<tr> <td style="text-align: center; width: 1022px; height: 19px;">
					<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="DarkRed" Style="vertical-align: bottom; text-align: center" Text="OWS FG Inventory Count Validation for AX Warehouse #" EnableViewState="False" ></asp:Label>
                <asp:Label ID="lbWhse" runat="server" BackColor="Transparent" Font-Bold="True" Font-Size="Medium"
                    ForeColor="Black" Style="vertical-align: middle; text-align: center"></asp:Label></td> </tr>
			<tr> <td style="text-align: center; width: 1024px;">
					<asp:Label ID="lbUser" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Black" Style="vertical-align: middle; text-align: center" Text="User ID : " BackColor="Transparent"></asp:Label>
				</td> </tr>
			<tr> <td style="text-align: center; vertical-align: top; width: 1022px;">
					<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="Click the Warehouse you are validating" BackColor="Transparent"></asp:Label>
				</td> </tr>
		</table>
		<table style="width: 1024px" border="0" cellpadding="3" cellspacing="0">
		    <tr> 
		        <td style="text-align: center; width: 204px;"></td>
		        <td style="text-align: center; width: 205px;"><asp:Button ID="btPackland" runat="server" Font-Size="Medium" Height="50px" Text="PACKLAND" Width="150px" Font-Bold="True" EnableViewState="False" /></td>
		        <td style="text-align: center; width: 205px;"><asp:Button ID="btPlant2" runat="server" Font-Size="Medium" Height="50px" Text="PLANT 2" Width="150px" Font-Bold="True" EnableViewState="False" /></td>
		        <td style="text-align: center; width: 205px;"><asp:Button ID="btGlacier" runat="server" Font-Size="Medium" Height="50px" Text="GLACIER" Width="150px" Font-Bold="True" EnableViewState="False" /></td>
		        <td style="text-align: center; width: 205px;">
                    <asp:Label ID="lbSKU_RPK" runat="server" Font-Size="Medium" ForeColor="DarkRed" Text="SKU / RPK"
                        Visible="False"></asp:Label><asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" BorderColor="Black"
                        BorderWidth="1px" Columns="13" Font-Size="Medium" ForeColor="Black" Visible="False"></asp:TextBox></td>
		    </tr>
		</table>
		<table style="width: 1024px" border="0" cellpadding="0" cellspacing="0">
			<tr> <td style="text-align: center; width: 1024px;" valign="middle">
                <asp:GridView ID="gvScanCounts" runat="server" AutoGenerateColumns="False" CellPadding="1"
                    CellSpacing="1" OnSelectedIndexChanged="gvScanCounts_SelectedIndexChanged" Visible="False" AllowPaging="True">
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" >
                            <HeaderStyle Width="84px" />
                        </asp:CommandField>
                        <asp:BoundField DataField="recordid" HeaderText="recordid" Visible="False" />
                        <asp:BoundField DataField="BinLocation" HeaderText="Bin Location">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AXItem" HeaderText="SKU/RPK">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProdDesc" HeaderText="Product Description">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="dtmCounted" HeaderText="DateTime Counted">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <PagerSettings PageButtonCount="20" />
                    <SelectedRowStyle BackColor="Yellow" Font-Bold="True" ForeColor="Navy" />
                </asp:GridView>
            </td> </tr>
		</table>
		<table style="width: 1024px" border="0" cellpadding="0" cellspacing="0">
			<tr> 
			    <td style="text-align: center; width: 256px;"><asp:Button ID="btSaveQty" runat="server" Font-Size="Medium" Height="50px" Text="SAVE QTY" Width="150px" Font-Bold="True" EnableViewState="False" Visible="False" /></td>
		        <td style="text-align: center; width: 256px;"><asp:Button ID="btEditQty" runat="server" Font-Size="Medium" Height="50px" Text="EDIT QTY" Width="150px" Font-Bold="True" EnableViewState="False" Visible="False" /></td>
		        <td style="text-align: center; width: 256px;"><asp:Button ID="btDeleteScan" runat="server" Font-Size="Medium" Height="50px" Text="DELETE SCAN" Width="150px" Font-Bold="True" EnableViewState="False" Visible="False" /></td> 
		        <td style="text-align: center; width: 256px;">
					<asp:Label ID="lbQuantity" runat="server" Font-Bold="False" Font-Size="Medium" ForeColor="DarkRed" Style="vertical-align: middle; text-align: right" Text="Qty-" Visible="False"></asp:Label>
					<asp:TextBox ID="txQuantity" runat="server" Font-Size="Medium" ForeColor="Black" BorderColor="Black" Visible="False" AutoPostBack="True" BorderWidth="1px" Columns="7"></asp:TextBox></td>
		    </tr>
		</table>
		<table style="width: 1024px" border="0" cellpadding="0" cellspacing="0">
		    <tr> <td style="text-align: center">
			    <asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" Font-Size="Medium" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False" Width="1022px"></asp:Label>
			    </td> </tr>
		</table>
		<table style="width: 1024px" border="0" cellpadding="0" cellspacing="0">
		
			<tr> 
			    <td style="text-align: center; width: 512px;"></td>
			    <td style="text-align: right; vertical-align: bottom;"><asp:Button ID="btReturn" runat="server" Font-Size="Small" Height="50px" Text="Return" Width="150px" Font-Bold="True" EnableViewState="False" /></td>
			    <td style="text-align: center; vertical-align: bottom;"><asp:Button ID="btRestart" runat="server" Font-Size="Small" Height="50px" Text="Restart Entry" Width="150px" Font-Bold="True" EnableViewState="False" /></td>
		    </tr>
		</table>
    </div>
    </form>
</body>
</html>
