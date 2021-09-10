<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_BuildCombo.aspx.vb" Inherits="OWS_IC.IC_BuildCombo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Build Combo</title>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <div title="IC Build Combo" style="text-align: left">
    <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="DarkRed" 
						Style="vertical-align: middle; text-align: center" Text="OW Inventory - Build Combo" EnableViewState="False" ></asp:Label>&nbsp;
                <asp:Label ID="lbLocation" runat="server" BorderStyle="None" Font-Bold="True" Font-Size="X-Small"
                    ForeColor="Black" Style="text-align: left">Plant2</asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Firebrick" 
						Style="vertical-align: middle; text-align: center" Text="Scan or Enter Station #" ></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: center">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" Font-Size="X-Small"
					ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6">
				</asp:TextBox></td>
		</tr>
	</table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 240px">
            <tr>
                <td style="width: 240px; text-align: center">
                    &nbsp;</td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 240px">
            <tr>
                <td style="width: 240px; text-align: center">
                    <asp:GridView ID="gvStuffingGroup" runat="server" AutoGenerateColumns="False" CellPadding="1"
                        CellSpacing="1" OnSelectedIndexChanged="gvStuffingGroup_SelectedIndexChanged" PageSize="100" Visible="False" Font-Size="X-Small">
                        <Columns>
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" SelectText="Sel" >
                                <ControlStyle Width="35px" />
                            </asp:CommandField>
                            <asp:BoundField DataField="StuffingGroupID" HeaderText="Stuffing Group" >
                                <HeaderStyle BackColor="#E0E0E0" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StuffingGroupDesc" HeaderText="Description">
                                <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="180px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="text-align: left; height: 30px;">
				    <asp:Button ID="btVerifyFinished" runat="server" Font-Size="Medium" 
						Text="Verified - Yes" Font-Bold="True" Visible="False" Width="115px" /></td>
                <td style="text-align: right; height: 30px;"><asp:Button ID="btNextCombo" runat="server" Font-Size="Medium" 
						Text="Next Combo" Font-Bold="True" Visible="False" Width="115px" /></td>
            </tr>
            <tr>
                <td style="text-align: left; height: 30px;"><asp:Button ID="btContinueCombo" runat="server" Font-Size="Medium" 
						Text="Continue" Font-Bold="True" Visible="False" Width="115px" /></td>
                <td style="text-align: right; height: 30px;">
                    <asp:Button ID="btFinishCombo" runat="server"
						Font-Size="Medium" Text="Finish Combo" Width="115px" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
            </tr>
        </table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left;">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid"
					BorderWidth="1px" Font-Bold="False" Font-Size="X-Small" ForeColor="Red" 
					Style="vertical-align: middle; text-align: center" Visible="False" Width="236px">
				</asp:Label></td>
		</tr>
	</table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
	    <tr>
			<td style="width: 72px; text-align: right; height: 20px;">
				<asp:Label ID="lbStation" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Text="Station # -" Visible="False"></asp:Label></td>
			<td style="width: 170px; text-align: left; height: 20px;">
				<asp:Label ID="lbStationValue" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label>
				<asp:Label ID="lbFunction" runat="server" Font-Bold="False" Style="text-align: left" 
					Visible="False" Text="0" Width="1px" Font-Size="X-Small"></asp:Label></td>
		</tr>
	    <tr>
			<td style="width: 72px; text-align: right; height: 20px;">
                <asp:Label ID="lbBuild" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Black"
                    Style="text-align: left" Text="Build # -" Visible="False"></asp:Label></td>
			<td style="width: 170px; text-align: left; height: 20px;">
                <asp:Label ID="lbBuildID" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label></td>
		</tr>
	    <tr>
			<td style="width: 72px; text-align: right; height: 20px;">
				<asp:Label ID="lbComboType" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Text="Combo Type -" Visible="False"></asp:Label></td>
			<td style="width: 170px; text-align: left; height: 20px;">
				<asp:Label ID="lbComboTypeVal" runat="server" Font-Bold="False" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False">10</asp:Label>
				<asp:Label ID="lbComboTypeValDesc" runat="server" Font-Bold="False" Style="text-align: left" 
					Visible="False" Font-Size="X-Small" BorderWidth="1px">Combo</asp:Label></td>
		</tr>
		<tr>
			<td style="width: 72px; text-align: right; height: 20px;">
				<asp:Label ID="lbFormula" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Text="Formula -" Visible="False"></asp:Label></td>
			<td style="width: 170px; text-align: left; height: 20px;">
				<asp:Label ID="lbFormulaValue" runat="server" Font-Bold="True" Font-Size="X-Small" 
					    ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label>
                <asp:Label ID="lbLot" runat="server" BorderWidth="1px" Font-Bold="False" Font-Size="X-Small"
                    Style="text-align: left" Visible="False" Width="15px"></asp:Label></td>
		</tr>
		<tr>
			<td style="width: 72px; text-align: right; height: 20px;">
				<asp:Label ID="lbStuffingGroup" runat="server" Font-Bold="True" Font-Size="X-Small" 
					ForeColor="Black" Style="text-align: left" Text="Stuffing Grp -" Visible="False"></asp:Label></td>
			<td style="width: 170px; text-align: left; height: 20px;">
				<asp:Label ID="lbStuffingGroupValue" runat="server" Font-Bold="True" Font-Size="X-Small" 
					    ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label></td>
		</tr>
	</table>
    <table border="1" cellpadding="0" cellspacing="0" style="width: 240px">
    	<tr>
			<td style="width: 240px; text-align: center; height: 20px;">
				<asp:Label ID="lbBuildRacks" runat="server" Font-Bold="True" Font-Size="Small" 
					ForeColor="Black" Style="text-align: left" Text="Racks In Combo" Visible="False"></asp:Label></td>			
		</tr>
	</table>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 240px">
        <tr>
            <td style="width: 240px; text-align: center">
                <asp:GridView ID="gvBuildRacks" runat="server" AutoGenerateColumns="False" CellPadding="1"
                        CellSpacing="1" PageSize="5" Visible="False" Font-Size="Small">
                    <Columns>
                        <asp:BoundField DataField="RackNumber" HeaderText="Rack #" >
                            <HeaderStyle BackColor="#E0E0E0" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Formula" HeaderText="Formula">
                            <HeaderStyle BackColor="#E0E0E0" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LotNo" HeaderText="Lot #">
                            <HeaderStyle BackColor="#E0E0E0" BorderWidth="1px" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="180px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
	<table style="width: 240px" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="text-align: left; height: 30px;">
				<asp:Button ID="btReturn" runat="server" Font-Size="Medium" 
						Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			<td style="text-align: right; height: 30px;">
				<asp:Button ID="btRestartCombo" runat="server"
						Font-Size="Medium" Text="Restart" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
	</div>
   </form>
</body>
</html>
