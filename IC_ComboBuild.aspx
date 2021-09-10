<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IC_ComboBuild.aspx.vb" Inherits="OWS_IC.IC_ComboBuild" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Build Combo</title>
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous"/>
</head>
<body class="container-fluid">
    <form id="form1" runat="server">
    <div title="IC Build Combo" style="text-align: left">
    <table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPageTitle" runat="server" Font-Bold="True" ForeColor="DarkRed" Style="vertical-align: middle; text-align: center" Text="OW Inventory - Combo Build" EnableViewState="False" ></asp:Label>
                <asp:Label ID="lbLocation" runat="server" BorderStyle="None" Font-Bold="True" ForeColor="Black" Style="text-align: left">Plant2</asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:Label ID="lbPrompt" runat="server" Font-Bold="True" ForeColor="Firebrick" Style="vertical-align: middle; text-align: center" Text="Scan or Enter Station #" ></asp:Label>
			</td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center">
				<asp:TextBox ID="txData" runat="server" Font-Bold="False" ForeColor="Black" Wrap="False" BorderColor="Black" AutoPostBack="True" BorderWidth="1px" Columns="6"></asp:TextBox>
			</td>
		</tr>
	</table>
        <table class="table table-striped">
            <tr class="row flex-fill">
                <td class="col-1" style="text-align: center"></td>
            </tr>
        </table>
        <table class="table table-striped">
            <tr class="row flex-fill" style="align-content:center;">
                <td class="col-1" style="text-align: center">
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
        <table class="table table-striped">
            <tr class="row flex-fill">
                <td class="col-2" style="margin-left: auto; margin-right: auto;">
				    <asp:Button ID="btVerifyFinished" runat="server" Text="Verified - Yes" Font-Bold="True" Visible="False"/>
                </td>
                <td class="col-2" style="margin-left: auto; margin-right: auto;">
					<asp:Button ID="btNextCombo" runat="server" Text="Next Combo" Font-Bold="True" Visible="False"/>
                </td>
            </tr>
            <tr class="row flex-fill">
                <td class="col-2" style="text-align: left;"><asp:Button ID="btContinueCombo" runat="server" Text="Continue" Font-Bold="True" Visible="False"/>
                </td>
                <td class="col-2" style="text-align: right;">
                    <asp:Button ID="btFinishCombo" runat="server" Text="Finish Combo" Font-Bold="True" EnableViewState="False" Visible="False" />
                </td>
            </tr>
        </table>
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-1" style="text-align: center;">
				<asp:Label ID="lbError" runat="server" BorderColor="DarkRed" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" ForeColor="Red" Style="vertical-align: middle; text-align: center" Visible="False"></asp:Label>
			</td>
		</tr>
	</table>
	<table class="table table-striped" style="">
	    <tr class="row flex-fill">
			<td class="col-2" style="text-align:right;">
				<asp:Label ID="lbStation" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Station # -" Visible="False"></asp:Label>
			</td>
			<td class="col-2" style="">
				<asp:Label ID="lbStationValue" runat="server" Font-Bold="False"	ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label>
				<asp:Label ID="lbFunction" runat="server" Font-Bold="False" Style="text-align: left" Visible="False" Text="0" Width="1px"></asp:Label>
			</td>
		</tr>
	    <tr class="row flex-fill">
			<td class="col-2" style="text-align: right;">
                <asp:Label ID="lbBuild" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Build # -" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
                <asp:Label ID="lbBuildID" runat="server" Font-Bold="False" ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False"></asp:Label></td>
		</tr>
	    <tr class="row flex-fill">
			<td class="col-2" style="text-align:right;">
				<asp:Label ID="lbComboType" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Combo Type -" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:Label ID="lbComboTypeVal" runat="server" Font-Bold="False" ForeColor="Black" Style="text-align: left" BorderWidth="1px" Visible="False">10</asp:Label>
				<asp:Label ID="lbComboTypeValDesc" runat="server" Font-Bold="False" Style="text-align: left" Visible="False" BorderWidth="1px">Combo</asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="text-align:right;">
				<asp:Label ID="lbFormula" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Formula -" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:Label ID="lbFormulaValue" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label>
                <asp:Label ID="lbLot" runat="server" BorderWidth="1px" Font-Bold="False" Style="text-align: left" Visible="False" ></asp:Label></td>
		</tr>
		<tr class="row flex-fill">
			<td class="col-2" style="text-align:right;">
				<asp:Label ID="lbStuffingGroup" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Stuffing Grp -" Visible="False"></asp:Label></td>
			<td class="col-2" style="">
				<asp:Label ID="lbStuffingGroupValue" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Visible="False" BorderWidth="1px"></asp:Label></td>
		</tr>
	</table>
    <table class="table table-striped">
    	<tr class="row flex-fill">
			<td class="col-2" style="text-align: center;">
				<asp:Label ID="lbBuildRacks" runat="server" Font-Bold="True" ForeColor="Black" Style="text-align: left" Text="Racks In Combo" Visible="False"></asp:Label></td>			
		</tr>
	</table>
    <table class="table table-striped">
        <tr class="row flex-fill" style="align-content:center;" >
            <td class="col-2" style="">
                <asp:GridView ID="gvBuildRacks" runat="server" AutoGenerateColumns="False" CellPadding="1"
                        CellSpacing="1" PageSize="5" Visible="False">
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
	<table class="table table-striped" style="">
		<tr class="row flex-fill">
			<td class="col-2" style="">
				<asp:Button ID="btReturn" runat="server" Text="To Menu" Width="115px" Font-Bold="True" EnableViewState="False" />
			</td>
			<td class="col-2" style="text-align: right;">
				<asp:Button ID="btRestartCombo" runat="server" Text="Restart" Font-Bold="True" EnableViewState="False" />
			</td>
		</tr>
	</table>
	</div>
   </form>
</body>
</html>
