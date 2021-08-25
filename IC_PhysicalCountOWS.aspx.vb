Partial Public Class IC_PhysicalCountOWS
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _user As String
    Public _function As Integer
    Public _product As Integer
    Public _productversion As String = ""
    Public _dateentered As DateTime
    Public strURL As String = Nothing
    Public _whs As Integer

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
        _whs = Common.GetVariable("Whse", Page).ToString

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
        Common.SaveVariable("Pallet", "", Page)
        Common.SaveVariable("SKU", "", Page)
        Common.SaveVariable("SKU_DESC", "", Page)
        Common.SaveVariable("LotNum", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("Version", "", Page)
        Common.SaveVariable("Quantity", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("UpdateMode", "N", Page)

        Me.btnVerified.Visible = False
        Me.btNextLocation.Visible = False

        Me.lbBin.Visible = False
        Me.lbBinValue.Visible = False
        Me.lbBinValue.Text = ""
        Me.lbPallet.Visible = False
        Me.lbPalletValue.Visible = False
        Me.lbPalletValue.Text = ""
        Me.lbSKU.Text = ""
        Me.lbProdDesc.Text = ""
        Me.lbQuantity.Visible = False
        Me.lbQuantityValue.Visible = False
        Me.lbQuantityValue.Text = ""
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbFunction.Text = "1"
        Me.txData.Visible = True
        Me.txData.Text = ""

        Me.lbPrompt.Text = "Scan or Enter Bin Location"
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Public Sub InitItem()
        'Clear Session variables for this page
        Common.SaveVariable("Pallet", "", Page)
        Common.SaveVariable("SKU", "", Page)
        Common.SaveVariable("SKU_DESC", "", Page)
        Common.SaveVariable("LotNum", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("Version", "", Page)
        Common.SaveVariable("Quantity", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("UpdateMode", "N", Page)

        Me.btnVerified.Visible = False
        Me.btNextLocation.Visible = True

        Me.lbBin.Visible = True
        Me.lbBinValue.Visible = True
        Me.lbBinValue.Text = Trim(Common.GetVariable("Bin", Page).ToString)
        Me.lbPallet.Visible = False
        Me.lbPalletValue.Visible = False
        Me.lbPalletValue.Text = ""
        Me.lbSKU.Text = ""
        Me.lbProdDesc.Text = ""
        Me.lbQuantity.Visible = False
        Me.lbQuantityValue.Visible = False
        Me.lbQuantityValue.Text = ""
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Inventory Pallet"
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
                    Me.lbPrompt.Text = "Scan or Enter Inventory Pallet"
                    Me.lbFunction.Text = "2"
                    Me.txData.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txData)
                Else
                    Common.SaveVariable("Bin", "", Page)
                    Me.lbError.Text = "Bin does not exist in the system - Scan again or see your supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txData)
                End If


            Case Is = 2 'Scanning Pallet
                'Validate GTIN Version exist in system
                Common.SaveVariable("Pallet", Trim(Me.txData.Text), Page)
                If ValidatePallet(Me.txData.Text) = True Then
                    Me.lbPallet.Visible = True
                    Me.lbPalletValue.Visible = True
                    Me.lbPalletValue.Text = Trim(Me.txData.Text)
                    Me.lbSKU.Visible = True
                    Me.lbSKU.Text = Common.GetVariable("SKU", Page).ToString
                    Me.lbProdDesc.Visible = True
                    Me.lbProdDesc.Text = Common.GetVariable("SKU_DESC", Page).ToString

                    'Check to see if the Pallet has already been counted.
                    If ValidatePalletScanned(Me.txData.Text) = True Then
                        Me.lbQuantity.Visible = True
                        Me.lbQuantityValue.Visible = True
                        Me.lbQuantityValue.Text = Common.GetVariable("Quantity", Page).ToString
                        Me.lbPrompt.Text = "Pallet was already counted. Enter Y to enter new Qty or N to scan next pallet."
                        Me.lbFunction.Text = "5"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)
                        Exit Sub
                    End If

                    Me.lbPrompt.Text = "Enter Item Quantity"
                    Me.lbFunction.Text = "3"
                    Me.txData.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txData)
                Else
                    Common.SaveVariable("Pallet", "", Page)
                    Me.lbError.Text = "Pallet does not exist in the system for your warehouse - Scan again or see your supervisor."
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

            Case Is = 5 'Y or N to enter new pallet quantity in the IC_FGCountsOWS table.
                If Trim(UCase(Me.txData.Text)) = "Y" Then
                    Me.lbPrompt.Text = "Enter Item Quantity"
                    Me.lbFunction.Text = "3"
                    Me.txData.Text = ""
                    Common.SaveVariable("UpdateMode", "Y", Page)
                    Common.JavaScriptSetFocus(Page, Me.txData)
                Else
                    Me.lbPrompt.Text = "Scan or Enter Next Inventory Pallet"
                    Me.lbFunction.Text = "2"
                    Me.txData.Text = ""
                    Common.SaveVariable("UpdateMode", "N", Page)
                    Common.JavaScriptSetFocus(Page, Me.txData)
                End If

        End Select
    End Sub

    Public Function ValidateBin(ByVal bin As String) As Boolean
        'Validate that bin entered is valid and capture AX_Whse value
        ValidateBin = False
        Dim strAXWhse As String = ""
        Dim sqlString As String = "Select AX_Whse from vwIC_Bin_AXWhseOWS Where BIN_LOCATION = '" & bin & "'"

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

    Public Function ValidatePallet(ByVal pallet As String) As Boolean
        'Verify the Pallet and get values needed to write Count record
        ValidatePallet = False

        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If _whs = 17 Then
                sqlString = "Select * from vwIC_MontgomeryAllPallets Where pk_nPallet = " & CLng(pallet)
            Else
                sqlString = "Select * from vwIC_WiscAllPallets Where pk_nPallet = " & CLng(pallet)
            End If


            'Verify product exists in the system with GTIN and Version
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdGTIN = DB.NewSQLCommand3(sqlString)
            If sqlCmdGTIN Is Nothing Then Throw New Exception("""GTIN Version Lookup Command failed. Press Restart Entry button to try again.""")
            dsPallet = DB.GetDataSet(sqlCmdGTIN)
            sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
            DB.KillSQLConnection()

            If dsPallet.Tables(0).Rows.Count > 0 Then
                'Record found for GTIN and Version
                Common.SaveVariable("Pallet", pallet, Page)
                Common.SaveVariable("SKU", dsPallet.Tables(0).Rows(0).Item("AXItem"), Page)
                Common.SaveVariable("SKU_DESC", dsPallet.Tables(0).Rows(0).Item("strProductDescription"), Page)
                Common.SaveVariable("LotNum", dsPallet.Tables(0).Rows(0).Item("Lot"), Page)
                Common.SaveVariable("CodeDate", dsPallet.Tables(0).Rows(0).Item("CodeDate"), Page)
                Common.SaveVariable("GTIN", dsPallet.Tables(0).Rows(0).Item("GTIN"), Page)
                Common.SaveVariable("Version", dsPallet.Tables(0).Rows(0).Item("Version"), Page)

                ValidatePallet = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error occurred while validating Pallet --- " & ex.Message.ToString
            Me.lbError.Visible = True
            ValidatePallet = False
        End Try
    End Function

    Public Function ValidatePalletScanned(ByVal pallet As String) As Boolean
        'Verify the Pallet and get values needed to write Count record
        ValidatePalletScanned = False

        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            sqlString = "Select * from IC_FGCountsOWS Where Pallet = " & CLng(pallet)

            'Verify if counted pallet exists in the system
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdGTIN = DB.NewSQLCommand3(sqlString)
            If sqlCmdGTIN Is Nothing Then Throw New Exception("""Counted Pallet Lookup Command failed. Press Restart Entry button to try again.""")
            dsPallet = DB.GetDataSet(sqlCmdGTIN)
            sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
            DB.KillSQLConnection()

            If dsPallet.Tables(0).Rows.Count > 0 Then
                'Record found for GTIN and Version
                Common.SaveVariable("Quantity", dsPallet.Tables(0).Rows(0).Item("Quantity"), Page)
                Common.SaveVariable("SKU", dsPallet.Tables(0).Rows(0).Item("AXItem"), Page)
                Common.SaveVariable("SKU_DESC", dsPallet.Tables(0).Rows(0).Item("ProdDesc"), Page)


                ValidatePalletScanned = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error occurred while validating if pallet scanned already --- " & ex.Message.ToString
            Me.lbError.Visible = True
            ValidatePalletScanned = False
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
        'Write record to the table IC_FGCountsOWS using StoredProcedure = spInsertCount
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
        Dim iPallet As Long = CInt(Common.GetVariable("Pallet", Page).ToString)

        Dim sqlCmdInsertCount As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Process Verified Entry Connection not established")
            If Common.GetVariable("UpdateMode", Page).ToString = "Y" Then
                sqlCmdInsertCount = DB.NewSQLCommand("SQL.Query.UpdateFGCountOWS")
            Else
                sqlCmdInsertCount = DB.NewSQLCommand("SQL.Query.InsertFGCountOWS")
            End If

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
                .AddWithValue("@iPallet", iPallet)
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