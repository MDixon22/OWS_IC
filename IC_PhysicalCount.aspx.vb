Partial Public Class IC_PhysicalCount
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _user As String
    Public _function As Integer
    Public _product As Integer
    Public _productversion As String = ""
    Public _dateentered As DateTime
    Public strURL As String = Nothing

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        If Not strURL Is Nothing Then
            Common.SaveVariable("ScreenParam", "", Page)
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify the user is logged in 
        Common.CheckLogin(Page)
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        Me.lbError.Visible = False
        Me.lbPageTitle.Text = "OW Inventory - FG Physical Count"

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet Counting
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(Me.lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If
    End Sub

    Public Sub InitProcess()
        'Clear Session variables for this page
        Common.SaveVariable("Bin", "", Page)
        Common.SaveVariable("AX_Whse", "", Page)
        Common.SaveVariable("CaseLabel", "", Page)
        Common.SaveVariable("SKU", "", Page)
        Common.SaveVariable("SKU_DESC", "", Page)
        Common.SaveVariable("LotNum", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("Version", "", Page)
        Common.SaveVariable("Quantity", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        Me.btnVerified.Visible = False
        Me.btNextLocation.Visible = False

        Me.lbBin.Visible = False
        Me.lbBinValue.Visible = False
        Me.lbBinValue.Text = ""
        Me.lbCaseLabel.Visible = False
        Me.lbCaseLabelValue.Visible = False
        Me.lbCaseLabelValue.Text = ""
        Me.lbQuantity.Visible = False
        Me.lbQuantityValue.Visible = False
        Me.lbQuantityValue.Text = ""
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbFunction.Text = "1"
        Me.txData.Visible = True
        Me.txData.Text = ""

        Me.lbPrompt.Text = "Scan or Enter Row #"
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub
    Public Sub InitItem()
        'Clear Session variables for this page
        Common.SaveVariable("CaseLabel", "", Page)
        Common.SaveVariable("SKU", "", Page)
        Common.SaveVariable("SKU_DESC", "", Page)
        Common.SaveVariable("LotNum", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("Version", "", Page)
        Common.SaveVariable("Quantity", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        Me.btnVerified.Visible = False
        Me.btNextLocation.Visible = True

        Me.lbBin.Visible = True
        Me.lbBinValue.Visible = True
        Me.lbBinValue.Text = Trim(Common.GetVariable("Bin", Page).ToString)
        Me.lbCaseLabel.Visible = False
        Me.lbCaseLabelValue.Visible = False
        Me.lbCaseLabelValue.Text = ""
        Me.lbQuantity.Visible = False
        Me.lbQuantityValue.Visible = False
        Me.lbQuantityValue.Text = ""
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Case Label"
        Me.lbFunction.Text = "2"
        Me.txData.Visible = True
        Me.txData.Text = ""
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub
    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        If Len(RTrim(Me.txData.Text)) < 1 Then
            Exit Sub
        End If

        Select Case _function
            Case Is = 1 'Scanning Bin Location
                'Validate Bin Location exists using SQL.Query.BinLookup
                Common.SaveVariable("Bin", Trim(Me.txData.Text), Page)

                If ValidateBin(Trim(Me.txData.Text)) = True Then
                    Me.btNextLocation.Visible = True
                    Me.lbBin.Visible = True
                    Me.lbBinValue.Visible = True
                    Me.lbBinValue.Text = Trim(Me.txData.Text)
                    Me.lbPrompt.Text = "Scan or Enter Case Label"
                    Me.lbFunction.Text = "2"
                    Me.txData.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txData)
                Else
                    Common.SaveVariable("Bin", "", Page)
                    Me.lbError.Text = "Bin does not exist in the system - Scan again or see your supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txData)
                End If
                

            Case Is = 2 'Scanning Case Label
                'Validate GTIN Version exist in system
                Dim iCL As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString)
                If Trim(Me.txData.Text).Length = iCL Then
                    Common.SaveVariable("CaseLabel", Trim(Me.txData.Text), Page)
                    If ValidateCaseLabel(Me.txData.Text) = True Then
                        Me.lbCaseLabel.Visible = True
                        Me.lbCaseLabelValue.Visible = True
                        Me.lbCaseLabelValue.Text = Trim(Me.txData.Text)
                        Me.lbSKU.Visible = True
                        Me.lbSKU.Text = Common.GetVariable("SKU", Page).ToString
                        Me.lbProdDesc.Visible = True
                        Me.lbProdDesc.Text = Common.GetVariable("SKU_DESC", Page).ToString
                        Me.lbPrompt.Text = "Enter Item Quantity"
                        Me.lbFunction.Text = "3"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        Common.SaveVariable("CaseLabel", "", Page)
                        Me.lbError.Text = "Item does not exist in the system - Scan again or see your supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If
                Else
                    Me.lbError.Text = "Barcode Scanned is not the correct length for a Case Label - Scan the correct barcode."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txData)
                End If
            Case Is = 3 'Entering Quantity Counted
                If IsNumeric(Trim(Me.txData.Text)) = True Then
                    Common.SaveVariable("Quantity", Trim(Me.txData.Text), Page)
                    Me.lbQuantity.Visible = True
                    Me.lbQuantityValue.Visible = True
                    Me.lbQuantityValue.Text = Trim(Me.txData.Text)
                    Me.lbPrompt.Text = "Verify entries on screen then press Verified Count or Restart Entry button."
                    Me.btnVerified.Visible = True
                    Me.txData.Visible = False
                    Me.lbFunction.Text = "0"
                Else
                    Me.lbError.Text = "Quantity must be a number - please enter the quantity again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txData)
                End If
                
        End Select
    End Sub

    Public Function ValidateBin(ByVal bin As String) As Boolean
        'Validate that bin entered is valid and capture AX_Whse value
        ValidateBin = False
        Dim strAXWhse As String = ""
        Dim sqlString As String = "Select AX_Whse from vwIC_Bin_AXWhse Where BIN_LOCATION = '" & bin & "'"

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
            sqlCmdBin = DB.NewSQLCommand3(sqlString)
            If sqlCmdBin Is Nothing Then Throw New Exception("""Bin Location Lookup Command failed. Press Restart Entry button to try again.""")
            strAXWhse = sqlCmdBin.ExecuteScalar()
            sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            DB.KillSQLConnection()
            If Not strAXWhse Is Nothing Then
                ValidateBin = True
                Common.SaveVariable("AX_Whse", strAXWhse, Page)
            End If
        Catch ex As Exception
            lbError.Text = "Bin Location Validation failed with Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try

    End Function

    Public Function ValidateCaseLabel(ByVal caselbl As String) As Boolean
        'Verify the Case Label - make sure GTIN and Version are valid using SQL.Query.GTINVerLookup
        'Also store values needed for database in Session Variables
        ValidateCaseLabel = False

        Dim dsCaseLabel As New Data.DataSet
        Dim sqlCmdCaseLabel As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Dim strGTIN As String = Mid(caselbl, 3, 14)
        Dim strVersion As String = Mid(caselbl, 42, 3)
        Dim strCodeDate As String = Mid(caselbl, 19, 6)
        Dim strLot As String = Mid(caselbl, 34, 5)
        'LookupLot in PrintJobs table in OWLabels database where PrintJob = Clng(Mid$(Me.txCaseLabel.Text, 27, 12))
        '  If not in that table use Mid$(Me.txCaseLabel.Text, 34, 5) above to get from owlabel system label.

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            
            sqlString = "Select * from vwGtinProductsAll Where GTIN = " & CLng(strGTIN) & " And Version = " & CInt(strVersion)

            'Verify product exists in the system with GTIN and Version
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdGTIN = DB.NewSQLCommand3(sqlString)
            If sqlCmdGTIN Is Nothing Then Throw New Exception("""GTIN Version Lookup Command failed. Press Restart Entry button to try again.""")
            dsCaseLabel = DB.GetDataSet(sqlCmdGTIN)
            sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
            DB.KillSQLConnection()

            If dsCaseLabel.Tables(0).Rows.Count > 0 Then
                'Record found for GTIN and Version
                Common.SaveVariable("CaseLabel", caselbl, Page)
                Common.SaveVariable("SKU", dsCaseLabel.Tables(0).Rows(0).Item("AXItem"), Page)
                Common.SaveVariable("SKU_DESC", dsCaseLabel.Tables(0).Rows(0).Item("strProductDescription"), Page)
                Common.SaveVariable("LotNum", strLot, Page)
                Common.SaveVariable("CodeDate", strCodeDate, Page)
                Common.SaveVariable("GTIN", strGTIN, Page)
                Common.SaveVariable("Version", strVersion, Page)
                
                ValidateCaseLabel = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error occurred while validating Case Label --- " & ex.Message.ToString
            Me.lbError.Visible = True
            ValidateCaseLabel = False
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub btnVerified_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerified.Click
        'Write record to the table IC_FGCounts using StoredProcedure = spInsertCount
        '@iWhse, @sBin, @sAXItem, @sProdDesc, @lGTIN, @iVersion, @sLot, @sCodeDate, @iQuantity, @sUserID, @iErrorCode
        Dim iWhse As Integer = CInt(Common.GetVariable("AX_Whse", Page).ToString)
        Dim sBin As String = Me.lbBinValue.Text
        Dim sSku As String = Me.lbSKU.Text
        Dim sProdDesc As String = Me.lbProdDesc.Text
        Dim lGTIN As Long = CLng(Common.GetVariable("GTIN", Page).ToString)
        Dim iVersion As Integer = CInt(Common.GetVariable("Version", Page).ToString)
        Dim sLotNumber As String = Common.GetVariable("LotNum", Page).ToString
        Dim sCodeDate As String = Common.GetVariable("CodeDate", Page).ToString
        Dim iQuantity As Integer = CInt(Me.lbQuantityValue.Text)

        Dim sqlCmdInsertCount As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Process Into Oven Connection not established")
            sqlCmdInsertCount = DB.NewSQLCommand("SQL.Query.InsertFGCount")
            If sqlCmdInsertCount Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmdInsertCount.Parameters
                .AddWithValue("@iWhse", iWhse)
                .AddWithValue("@sBin", sBin)
                .AddWithValue("@sAXItem", sSku)
                .AddWithValue("@sProdDesc", sProdDesc)
                .AddWithValue("@lGTIN", lGTIN)
                .AddWithValue("@iVersion", iVersion)
                .AddWithValue("@sLot", sLotNumber)
                .AddWithValue("@sCodeDate", sCodeDate)
                .AddWithValue("@iQuantity", iQuantity)
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdInsertCount.ExecuteNonQuery()

            If sqlCmdInsertCount.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("SQL store procedure returned value '" & sqlCmdInsertCount.Parameters.Item("@iErrorCode").Value & "'")
            Else
                Call InitItem()
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error occurred while saving count for " & sSku & "-" & sProdDesc & "---" & ex.Message.ToString
            Me.lbError.Visible = True
        End Try
    End Sub

    Protected Sub btNextLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNextLocation.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class