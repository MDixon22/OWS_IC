Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_SamplePalletNew
    Inherits System.Web.UI.Page

    Private Enum ScanModes
        StartMode
        OrderMode
        PalletMode
        QuantityMode
        BinMode
        PrinterMode
    End Enum

#Region "Variable Declaration"
    Private Mode As ScanModes
    Public _company As String
    Public _whs As String
    Public _dateentered As Date
    Public strURL As String
    Public _tobin As String
    Public _printer As String
    Public _remqtyflag As String
    Public _screenmode As String
    Public bAddTo As Boolean
    Public _gtin As String
    Public _codedate As String
    Public _ver As String
    Public _status As String
    Public _qty As Integer
    Public _lot As String

#End Region

#Region "Page Level"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '_remqtyflag = Common.GetVariable("RemQtyFlag", Page)

        Mode = Common.GetVariable("8SavedMode", Page)

        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        If Not strURL Is Nothing Then
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify the user is logged in lbPageTitle
        Common.CheckLogin(Page)

        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString
        _screenmode = Common.GetVariable("ScreenParam", Page).ToString

        If _screenmode = "AddTo" Then
            bAddTo = True
            Me.lbPageTitle.Text = "OW Inventory - AddTo Sample Pallet"
            Me.lbGridTitle.Text = "Existing Pallet - Products Just Added"
            Me.btReturn.Text = "Return"

        Else
            Me.lbPageTitle.Text = "OW Inventory - Create Sample Pallet"
            bAddTo = False
        End If

        'AddToPalletNumber
        If Not Page.IsPostBack Then
            Common.SaveVariable("PrintSecondTime", "N", Page)
            If bAddTo = True Then
                Common.SaveVariable("UpdatedSamplePallet", Nothing, Page)
                Me.lbSamplePalletValue.Text = Common.GetVariable("EditPalletNumber", Page).ToString
                Mode = ScanModes.OrderMode
            Else
                Mode = ScanModes.StartMode
            End If
            Common.SaveVariable("8SavedMode", Mode, Page)
            Call ResetControls()
        End If
    End Sub

    Public Sub ResetControls()
        Select Case Mode
            Case ScanModes.StartMode 'Prepare screen for start up
                Me.lbSamplePallet.Visible = False
                Me.lbSamplePalletValue.Visible = False

                Me.htxOrder.Visible = False
                Me.lbOrder.Visible = False
                Me.htxPallet.Visible = False
                Me.lbCaseLabel.Visible = False
                Me.btReturn.Visible = False
                Me.txQuantity.Visible = False
                Me.lbQuantity.Visible = False
                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btReturn.Visible = True
                Me.btComplete.Visible = False
                Me.btRestart.Visible = False
                Me.btNew.Visible = True
                Me.btNew.Text = "New Pallet"

                Me.htxOrder.Value = ""
                Me.htxPallet.Value = ""
                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""
                Me.lbSamplePalletValue.Text = ""

                Me.lbPrompt.Text = "Press New Pallet or To Menu button"

            Case ScanModes.OrderMode 'Prepare screen to accept Pick Order Scan
                Me.lbSamplePallet.Visible = True
                Me.lbSamplePalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxPallet.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btNew.Visible = False
                Me.btReturn.Visible = True

                Me.htxOrder.Value = ""
                Me.htxPallet.Value = ""
                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Scan or Enter Pick Order."
                Common.JavaScriptSetFocus(Page, Me.htxOrder)


            Case ScanModes.PalletMode 'Prepare screen to accept CaseLabel Scan
                Me.lbSamplePallet.Visible = True
                Me.lbSamplePalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxPallet.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True

                'If Common.GetVariable("RemQtyFlag", Page).ToString = "T" Then
                'Me.btNew.Visible = True
                'Me.btNew.Text = "Next Product"

                Me.htxPallet.Value = ""
                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Scan or Enter Case Label."
                Common.JavaScriptSetFocus(Page, Me.htxPallet)

            Case ScanModes.QuantityMode 'Prepare screen to accept quantity entry
                Me.lbSamplePallet.Visible = True
                Me.lbSamplePalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxPallet.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.btNew.Visible = False

                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Enter Case Quantity."
                Common.JavaScriptSetFocus(Page, Me.txQuantity)

            Case ScanModes.BinMode  'Prepare screen to accept bin location entry
                Me.lbSamplePallet.Visible = True
                Me.lbSamplePalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxPallet.Visible = False
                Me.lbCaseLabel.Visible = False
                Me.txQuantity.Visible = False
                Me.lbQuantity.Visible = False

                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btComplete.Visible = False
                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.btNew.Visible = False

                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Enter Bin Location for Pallet."
                Common.JavaScriptSetFocus(Page, Me.txBin)

            Case ScanModes.PrinterMode 'Prepare screen to accept Printer # entry
                Me.lbSamplePallet.Visible = True
                Me.lbSamplePalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxPallet.Visible = False
                Me.lbCaseLabel.Visible = False
                Me.txQuantity.Visible = False
                Me.lbQuantity.Visible = False

                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = True
                Me.lbPrinter.Visible = True

                Me.btComplete.Visible = False
                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.btNew.Visible = False

                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Enter Printer #."
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
        End Select
        Try 'Must change to look at IC_TmpSamplePallets
            Dim strPallet As String = ""
            strPallet = Me.lbSamplePalletValue.Text
            If strPallet.Length > 0 Then
                Dim sqlString As String
                Dim dsSamplePallet As New Data.DataSet
                Dim sqlCmdSamplePallet As New System.Data.SqlClient.SqlCommand
                Dim strCanSave As Boolean = False

                sqlString = "Select * from IC_TmpSamplePalletsNew Where PalletID = " & CLng(strPallet)
                'Get Temp SamplePallets to validate whether to allow Complete function
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdSamplePallet = DB.NewSQLCommand3(sqlString)
                    If Not sqlCmdSamplePallet Is Nothing Then
                        dsSamplePallet = DB.GetDataSet(sqlCmdSamplePallet)
                        sqlCmdSamplePallet.Dispose() : sqlCmdSamplePallet = Nothing
                        DB.KillSQLConnection()
                        If dsSamplePallet.Tables(0).Rows.Count > 0 Then
                            strCanSave = True
                            Me.LoadGrid()
                        Else
                            Me.dgProductsScanned.Visible = False
                            Me.lbGridTitle.Visible = False
                        End If
                    End If
                End If
                If Me.txBin.Visible = True Then
                    Me.btComplete.Visible = False
                Else
                    Me.btComplete.Visible = strCanSave
                End If
            Else
                Me.dgProductsScanned.Visible = False
                Me.lbGridTitle.Visible = False
            End If
        Catch ex As Exception
            Me.lbError.Text = ex.ToString
        End Try
    End Sub

#End Region

#Region "Temp Table"

    Public Function WriteTempTableRecord(ByVal pallet As Long, ByVal order As Long, _
                                            ByVal clgtin As String, ByVal expire As String, _
                                            ByVal lotno As String, ByVal caseqty As Integer, _
                                            ByVal prodvariant As String, ByVal srcPallet As Long) As Boolean

        WriteTempTableRecord = False
        Dim dtmScanned As Date = Now()
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString
        Me.lbError.Text = ""
        Dim strSQLInsertTemp As String = "INSERT Into IC_TmpSamplePalletsNew (PalletID,OrderID,CL_GTIN,CodeDate,Lot,CaseQty,txDateTimeEntered,UserID,ProdVariant,SourcePallet) " & _
        "VALUES (" & pallet & "," & order & ",'" & clgtin & "','" & expire & "','" & lotno & "'," & caseqty & ",'" & dtmScanned & "','" & strUnit & "','" & prodvariant & "'," & srcPallet & ")"

        Try
            'Insert Temp Sample Pallet record
            Dim sqlCmdTempPallet As New System.Data.SqlClient.SqlCommand
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdTempPallet = DB.NewSQLCommand3(strSQLInsertTemp)
                If Not sqlCmdTempPallet Is Nothing Then
                    sqlCmdTempPallet.ExecuteNonQuery()
                    sqlCmdTempPallet.Dispose() : sqlCmdTempPallet = Nothing
                    WriteTempTableRecord = True
                Else
                    Me.lbError.Text = "Command Error occurred while saving temp Sample pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                End If
                If Not sqlCmdTempPallet Is Nothing Then
                    sqlCmdTempPallet.Dispose() : sqlCmdTempPallet = Nothing
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while saving temp Sample pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while saving temp Sample pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
        Finally
            DB.KillSQLConnection()
        End Try
    End Function

    Public Sub CleanupTempTableRecords()
        Dim sqlCmdCleanupSamplePallet As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""
        Dim strSqlTempCleanup As String = ""

        Try
            strPallet = Me.lbSamplePalletValue.Text
            strSqlTempCleanup = "Update IC_TmpSamplePalletsNew Set Processed = 'Y' Where PalletID = " & CLng(strPallet)

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdCleanupSamplePallet = DB.NewSQLCommand3(strSqlTempCleanup)
                If Not sqlCmdCleanupSamplePallet Is Nothing Then
                    sqlCmdCleanupSamplePallet.ExecuteNonQuery()
                    sqlCmdCleanupSamplePallet.Dispose() : sqlCmdCleanupSamplePallet = Nothing
                    DB.KillSQLConnection()
                Else
                    Me.lbError.Text = "Command Error occurred while cleaning up temp records for Sample pallet! Check battery and wireless connection - then try again."
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while cleaning up temp records for Sample pallet! Check battery and wireless connection - then try again."
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while cleaning up temp records for Sample pallet! Check battery and wireless connection - then try again."
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

#Region "User Entry"

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        'Validate previous field entries
        Me.lbError.Text = ""
        Dim strPallet As String = ""
        Dim prodqty As Integer = 0
        Dim SumCaseQty As Integer = 0
        Dim qtyRemain As Integer = 0

        Try
            strPallet = Me.lbSamplePalletValue.Text

            Call ValidateEntries(Me.htxOrder.Value, Me.htxPallet.Value)

            If Me.lbError.Text.Length = 0 Then
                If IsNumeric(Me.txQuantity.Text) = True Then
                    prodqty = CInt(Me.txQuantity.Text)
                Else
                    Me.lbError.Text = "Case Quantity entered needs to be numeric!"
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)
                    Exit Sub
                End If

                'Scan the next Case Label
                Mode = ScanModes.PalletMode
                Common.SaveVariable("8SavedMode", Mode, Page)


                'Set variables needed to write IC_TmpSamplePallets record
                Dim nPallet As Long = CLng(strPallet)
                Dim nOrder As Long = CLng(Me.htxOrder.Value)
                Dim strProdVariant As String = _ver
                Dim nSrcPallet As Long = CLng(Me.htxPallet.Value) 'Inventory Source Pallet
                'Get these values from the Pallet Tag lookup. htxPallet
                Dim strPartCode As String = _gtin
                Dim strCDate As String = _codedate
                Dim strLotNum As String = _lot

                If WriteTempTableRecord(nPallet, nOrder, strPartCode, strCDate, strLotNum, prodqty, strProdVariant, nSrcPallet) = False Then
                    'Error occurred while adding current product to Sample pallet temp table - set Mode to quantity to try again
                    Mode = ScanModes.QuantityMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    Me.lbError.Text = "Error occurred while adding Product - " & strPartCode & " to Sample Pallet - " & nPallet & ". Check Wireless Connection and then try entering case quantity again."
                End If
                ResetControls()

            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during validation of Quantity entry! Press Restart button and reprocess the Pallet."
            Common.JavaScriptSetFocus(Page, Me.txQuantity)
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txBin.TextChanged
        'Verify the Bin Location entered is valid in database
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Me.txBin.Text = UCase(Me.txBin.Text)
        Me.lbError.Text = ""

        Try
            If Trim(Me.txBin.Text).Length = 0 Then
                'Nothing entered on screen for Bin Location
                Me.lbError.Text = "Bin Location must be entered."
                Common.JavaScriptSetFocus(Page, Me.txBin)
                Exit Sub
            End If

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdBin = DB.NewSQLCommand("SQL.Query.BinLookup")
            Else
                Me.lbError.Text = "Communication Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
                Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txBin)
                Exit Sub
            End If

            If Not sqlCmdBin Is Nothing Then
                sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txBin.Text)
                _tobin = sqlCmdBin.ExecuteScalar()

                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                DB.KillSQLConnection()
            Else
                Me.lbError.Text = "Command Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
                Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txBin)

                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                DB.KillSQLConnection()
                Exit Sub
            End If

            If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
                Me.lbError.Text = "Invalid Bin Location entered. Try again."
                Common.JavaScriptSetFocus(Page, Me.txBin)
                Exit Sub
            Else 'Bin Location is valid
                'Get Printer #
                Mode = ScanModes.PrinterMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
            Common.JavaScriptSetFocus(Page, Me.txBin)
        Finally
            If Not sqlCmdBin Is Nothing Then
                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""
        Dim iPrt As Integer = 0
        Me.lbError.Text = ""

        Try
            'Verify printer # entered is for correct location and is numeric
            If IsNumeric(Me.txPrinter.Text) = True Then
                iPrt = CInt(Me.txPrinter.Text)
            Else
                Me.lbError.Text = "Printer # needs to be single digit numeric entry!"
                Common.SaveVariable("Printer", "", Page)
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Exit Sub
            End If

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdPrinter = DB.NewSQLCommand("SQL.Query.PrinterLookup")
            Else
                Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                Me.lbError.Text = "Communication Error occurred while validating Printer - try again or see your supervisor!"
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Exit Sub
            End If

            If Not sqlCmdPrinter Is Nothing Then
                sqlCmdPrinter.Parameters.AddWithValue("@Printer", iPrt)
                sqlCmdPrinter.Parameters.AddWithValue("@Warehouse", 20)
                _printer = sqlCmdPrinter.ExecuteScalar()

                sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                DB.KillSQLConnection()
            Else
                Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                Me.lbError.Text = "Error occurred while validating Printer - try again or see your supervisor!"
                Common.JavaScriptSetFocus(Page, Me.txPrinter)

                sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                DB.KillSQLConnection()
                Exit Sub
            End If

            If Trim(_printer).Length < 1 Then 'Printer does not exist in database
                Me.lbError.Text = "Invalid Printer # entered. Try again."
                'Save the bad Printer to display on screen for correction
                Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Exit Sub
            Else 'Printer is valid
                Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                Me.lbError.Text = ""
            End If

            'Write records to IC_PalletsHeader, IC_Pallets, IC_PalletsHistory
            strPallet = Me.lbSamplePalletValue.Text
            SavePallet(strPallet & Me.htxOrder.Value)

            If Me.lbError.Text = "" Then
                'Write file to Loftware server to print out the Pallet Tag
                If PrintSamplePalletTag() = True Then
                    'delete records from IC_TmpSamplePallets where PalletID match current screen
                    CleanupTempTableRecords()
                    Me.dgProductsScanned.SelectedIndex = -1
                    Me.lbSamplePalletValue.Text = ""
                    Mode = ScanModes.StartMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                Else
                    Mode = ScanModes.PrinterMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                End If
            Else
                Me.lbError.Text = Me.lbError.Text.ToString + " See your supervisor, Last Sample Pallet failed to update in system!"
            End If
        Catch ex As Exception
            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Printer! Check battery and Wireless connection - then try again or see your supervisor."
            Common.JavaScriptSetFocus(Page, Me.txPrinter)
        Finally
            If Not sqlCmdPrinter Is Nothing Then
                sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

#Region "Screen Buttons"

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        Me.lbError.Text = ""

        'delete records from IC_TmpSamplePallets where PalletID, OrderID, UserID match current screen
        Try
            CleanupTempTableRecords()
            Me.dgProductsScanned.Dispose()
            Me.btNew.Visible = False

            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        Catch ex As Exception
            Me.lbError.Text = "Error occurred during restart"
        End Try

    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        Common.SaveVariable("ScreenParam", Nothing, Page)
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btComplete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btComplete.Click
        Me.lbError.Text = ""
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbSamplePalletValue.Text

            'Prompt for Bin Location
            Mode = ScanModes.BinMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        Catch ex As Exception
            Me.lbError.Text = ex.ToString + " See your supervisor, Last Pallet failed to update system!"
        End Try
    End Sub

    Protected Sub btNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNew.Click
        If Me.btNew.Text = "New Pallet" Then
            Try
                Dim SamplePallet As Integer
                SamplePallet = 0
                'Get next pallet id for Sample pallets using stored procedure
                If Not DB.MakeSQLConnection("Warehouse") Then
                    Dim sqlCmdSamplePallet As New System.Data.SqlClient.SqlCommand
                    sqlCmdSamplePallet = DB.NewSQLCommand2("spNextSamplePallet ")
                    If Not sqlCmdSamplePallet Is Nothing Then
                        sqlCmdSamplePallet.Parameters.AddWithValue("@nSamplePallet", 0)
                        sqlCmdSamplePallet.Parameters("@nSamplePallet").Direction = ParameterDirection.Output

                        SamplePallet = sqlCmdSamplePallet.ExecuteScalar()
                        If IsDBNull(SamplePallet) = False And SamplePallet <> 0 Then 'Successfully got New Sample Pallet #
                            Me.lbSamplePalletValue.Text = SamplePallet
                            Mode = ScanModes.OrderMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Me.lbError.Text = ""
                            ResetControls()
                        Else
                            'Error getting New Sample Pallet # prompt user to try again
                            Mode = ScanModes.StartMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Me.lbError.Text = "Data Error occurred while getting New Pallet # - Check Connection, then Try again!"
                            ResetControls()
                        End If
                    Else
                        'Error getting New Sample Pallet # prompt user to try again
                        Mode = ScanModes.StartMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Me.lbError.Text = "Command Error occurred while getting New Pallet # - Check Connection, then Try again!"
                        ResetControls()
                    End If
                Else
                    'Error getting New Sample Pallet # prompt user to try again
                    Mode = ScanModes.StartMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    Me.lbError.Text = "Connection Error occurred while getting New Pallet # - Check Connection, then Try again!"
                    ResetControls()
                End If
            Catch ex As Exception
                'Error getting New Pallet # prompt user to try again
                Mode = ScanModes.StartMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                Me.lbError.Text = "General Error occurred while getting New Pallet # - Check Connection, then Try again!"
                ResetControls()
            End Try
        End If
    End Sub

#End Region

#Region "Custom Processes"

    Public Sub SavePallet(ByVal oPallet As String) ' Must change to use the IC_TmpSamplePallets table on SRV04 
        Dim sqlPalletInsert As String = ""
        Dim sqlPalletUpdate As String = ""
        Dim strPallet As String = Mid(oPallet.ToString, 1, 9)
        Dim strOrder As String = Mid(oPallet.ToString, 10, 9)
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString

        Me.lbError.Text = ""

        Try
            'Insert Pallet records
            _dateentered = Now()
            sqlPalletInsert = "insertSamplePallet2019 "
            sqlPalletUpdate = "insertSamplePallet2019Step2 "

            Dim SumCaseQty As Integer = 0
            Dim sqlString As String
            Dim dv As New DataView
            Dim dsSamplePalletSave As New Data.DataSet
            Dim sqlCmdSamplePalletSave As New System.Data.SqlClient.SqlCommand

            sqlString = "Select * from IC_TmpSamplePalletsNew Where PalletID = " & CLng(strPallet) & " And Processed = 'N'"

            'Get Temp SamplePallets and write to IC_PalletsHeader and IC_Pallets tables
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdSamplePalletSave = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdSamplePalletSave Is Nothing Then
                    dsSamplePalletSave = DB.GetDataSet(sqlCmdSamplePalletSave)
                    sqlCmdSamplePalletSave.Dispose() : sqlCmdSamplePalletSave = Nothing
                    DB.KillSQLConnection()
                End If
            End If

            If dsSamplePalletSave.Tables(0).Rows.Count < 1 Then
                Me.lbError.Text = "Data Error occurred while saving Sample Pallet Information to database! Check battery and wireless connection - then try again."
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
            Else
                'records retrieved - process through each row and Insert Pallet records
                Dim drSamplePalletSave As Data.DataRow
                For Each drSamplePalletSave In dsSamplePalletSave.Tables(0).Rows
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
                        sqlCmdPallet = DB.NewSQLCommand2(sqlPalletInsert)
                        If Not sqlCmdPallet Is Nothing Then
                            sqlCmdPallet.Parameters.AddWithValue("@pnPallet", CLng(strPallet))
                            sqlCmdPallet.Parameters.AddWithValue("@pnOrder", CLng(strOrder))
                            sqlCmdPallet.Parameters.AddWithValue("@pdtmScanned", _dateentered)
                            sqlCmdPallet.Parameters.AddWithValue("@pstrUnit", strUnit)
                            sqlCmdPallet.Parameters.AddWithValue("@pstrProduct", drSamplePalletSave.Item("CL_GTIN"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrExpiration", drSamplePalletSave.Item("CodeDate"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrMfgOrd", Mid(drSamplePalletSave.Item("Lot"), 3, 5))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrLot", Mid(drSamplePalletSave.Item("Lot"), 8, 5))
                            sqlCmdPallet.Parameters.AddWithValue("@pnQuantity", drSamplePalletSave.Item("CaseQty"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrBin", RTrim(Me.txBin.Text))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrProdVariant", drSamplePalletSave.Item("ProdVariant"))
                            sqlCmdPallet.Parameters.AddWithValue("@pnSourcePallet", drSamplePalletSave.Item("SourcePallet"))
                            sqlCmdPallet.ExecuteNonQuery()
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                            DB.KillSQLConnection()

                        Else
                            Me.lbError.Text = "Command Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                            Me.lbError.Visible = True
                        End If
                        If Not sqlCmdPallet Is Nothing Then
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                        End If

                    Else
                        Me.lbError.Text = "Communication Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                        Me.lbError.Visible = True
                    End If

                    If Not DB.MakeSQLConnection("Warehouse") Then

                        Dim sqlCmdPalletUpdate As New System.Data.SqlClient.SqlCommand
                        sqlCmdPalletUpdate = DB.NewSQLCommand2(sqlPalletUpdate)
                        If Not sqlCmdPalletUpdate Is Nothing Then
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pdtmScanned", _dateentered)
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pstrUnit", strUnit)
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pstrProduct", drSamplePalletSave.Item("CL_GTIN"))
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pstrExpiration", drSamplePalletSave.Item("CodeDate"))
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pstrMfgOrd", Mid(drSamplePalletSave.Item("Lot"), 3, 5))
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pstrLot", Mid(drSamplePalletSave.Item("Lot"), 8, 5))
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pnQuantity", CInt(Common.GetVariable("InvPalletQty", Page).ToString) - CInt(drSamplePalletSave.Item("CaseQty")))
                            sqlCmdPalletUpdate.Parameters.AddWithValue("@pnSourcePallet", drSamplePalletSave.Item("SourcePallet"))
                            sqlCmdPalletUpdate.ExecuteNonQuery()
                            sqlCmdPalletUpdate.Dispose() : sqlCmdPalletUpdate = Nothing
                            DB.KillSQLConnection()
                        Else
                            Me.lbError.Text = "Command Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                            Me.lbError.Visible = True
                        End If
                        If Not sqlCmdPalletUpdate Is Nothing Then
                            sqlCmdPalletUpdate.Dispose() : sqlCmdPalletUpdate = Nothing
                        End If
                    Else
                        Me.lbError.Text = "Communication Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                        Me.lbError.Visible = True
                    End If
                Next
            End If
        Catch e As Exception
            Me.lbError.Text = "Error - " & e.ToString & " - occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Sub ValidateEntries(ByVal orderid As String, ByVal caselabel As String)
        'Validate Order Length
        Me.lbError.Text = ""
        Try
            If Me.htxOrder.Value.Length <> CInt(ConfigurationManager.AppSettings.Get("UI.Order.Length").ToString) Then
                Me.lbError.Text = "Value entered for Pick Order is wrong length - start process again!"
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Pick Order! Check Battery and Wireless Connection then Restart Pallet"
        End Try

        If Me.lbError.Text.Length > 0 Then
            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
            Exit Sub
        End If

        'Validate Pallet Tag- get product fields and compare values to PickProduct
        If Me.htxPallet.Value.Length > 0 And IsNumeric(Me.htxPallet.Value) = True Then
            ' = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString) Or Me.htxCaseLabel.Value.Length = CInt(ConfigurationManager.AppSettings.Get("UI.AltCaseLength").ToString) Then
            Dim dsPallet As New Data.DataSet
            Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

            Try
                Me.lbError.Text = ""
                Me.lbError.Visible = False
                Dim iPallet As Integer

                If IsNumeric(Me.htxPallet.Value) = True Then
                    'txCombo # entered is valid length check numeric
                    iPallet = CLng(RTrim(Me.htxPallet.Value))

                    'Validate that the Pallet scanned does exist in the system and get Status field
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        sqlCmdPallet = DB.NewSQLCommand3("Select GTIN,CodeDate,Ver,Qty,[Status],Lot,MfgOrd From vwIC_PalletsIC Where Pallet = " & iPallet)
                        If Not sqlCmdPallet Is Nothing Then
                            dsPallet = DB.GetDataSet(sqlCmdPallet)
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                            DB.KillSQLConnection()
                            If dsPallet Is Nothing Then
                                Me.lbError.Text = "Data Error occurred while validating Pallet scanned or entered! Check battery and wireless connection - then try again."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.htxPallet)
                            Else
                                If dsPallet.Tables(0).Rows.Count > 0 Then
                                    _gtin = dsPallet.Tables(0).Rows(0).Item("GTIN")
                                    _codedate = dsPallet.Tables(0).Rows(0).Item("CodeDate")
                                    _ver = dsPallet.Tables(0).Rows(0).Item("Ver")
                                    _status = dsPallet.Tables(0).Rows(0).Item("Status")
                                    _qty = dsPallet.Tables(0).Rows(0).Item("Qty")
                                    Common.SaveVariable("InvPalletQty", _qty, Page)
                                    _lot = dsPallet.Tables(0).Rows(0).Item("MfgOrd") & dsPallet.Tables(0).Rows(0).Item("Lot")

                                    Select Case UCase(RTrim(_status))
                                        Case Is = "V"
                                            Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.htxPallet)
                                        Case Is = "H"
                                            Me.lbError.Text = "Pallet Status is QC Hold. Pallet cannot be used for Shipping Pallet - see supervisor"
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.htxPallet)
                                        Case Is = "W"
                                            Me.lbError.Text = "Pallet Status is Warehouse Hold. Pallet cannot be used for Shipping Pallet - see supervisor"
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.htxPallet)
                                    End Select
                                Else 'Record does not exist for Pallet entered
                                    Me.lbError.Text = "Pallet # not in system - see supervisor."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.htxPallet)
                                End If
                            End If
                        Else
                            Me.lbError.Text = "Command Error occurred while validating Combo #! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.htxPallet)
                        End If
                    Else
                        Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.htxPallet)
                    End If

                Else
                    Me.lbError.Text = "Pallet # can not be blank, try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.htxPallet)
                End If
            Catch ex As Exception
                Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Combo # entered! Check battery and wireless connection - then try again or see your supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.htxPallet)
            Finally
                If Not dsPallet Is Nothing Then
                    dsPallet.Dispose() : dsPallet = Nothing
                End If
                If Not sqlCmdPallet Is Nothing Then
                    sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                End If
                DB.KillSQLConnection()
            End Try
        Else
            Me.lbError.Text = "Pallet Barcode scanned or entered must be numeric or was not entered.  Try again."
            Me.lbError.Visible = True
            Mode = ScanModes.PalletMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
            Exit Sub
        End If
    End Sub

    Public Sub LoadGrid()
        Dim sqlString As String = ""
        Dim dvGrd As New DataView
        Dim dsSamplePalletGrd As New Data.DataSet
        Dim sqlCmdSamplePalletGrd As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbSamplePalletValue.Text
            sqlString = "Select ProductID, ProdVariant as Version,CaseQty,ProdDesc from vwIC_TmpSamplePalletsGridViewNew Where PalletID = " & CLng(strPallet)
            'Get Temp ShippingPallet Products for Grid
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdSamplePalletGrd = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdSamplePalletGrd Is Nothing Then
                    dsSamplePalletGrd = DB.GetDataSet(sqlCmdSamplePalletGrd)
                    sqlCmdSamplePalletGrd.Dispose() : sqlCmdSamplePalletGrd = Nothing
                    DB.KillSQLConnection()
                    If dsSamplePalletGrd Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while displaying products added to current pallet! Check battery and wireless connection - then try again."
                    Else
                        dvGrd = New DataView(dsSamplePalletGrd.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
                        dgProductsScanned.DataSource = dvGrd
                        dgProductsScanned.DataBind()

                        If dvGrd.Count > 0 Then
                            Me.lbGridTitle.Visible = True
                            dgProductsScanned.Visible = True
                        Else
                            Me.lbGridTitle.Visible = False
                            dgProductsScanned.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while displaying products added to current pallet! Check battery and wireless connection - then try again."
        End Try
    End Sub

    Public Function PrintSamplePalletTag()
        Dim rtnval As Boolean = False
        Dim destPath As String = ""
        Dim tempPath As String = ""
        Dim filenm As String = ""
        Dim PathFile As String = ""
        Dim sourceFile As String = ""
        Dim successfile As String = ""
        Dim destinationFile As String = ""
        Dim _pallet As Integer
        Dim _printer As Integer = CInt(Me.txPrinter.Text)
        Dim x As Integer = 0
        Dim totalcases As Integer = 0
        Dim grossweight As Integer = 0
        Try

            'Used for getting ShipTo Info and Sample Pallet Details From vwIC_OW_SamplePallets
            Dim dsSamplePallet As New Data.DataSet
            Dim sqlCmdSamplePallet As New System.Data.SqlClient.SqlCommand

            'Get Shipto Info for SamplePallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdSamplePallet = DB.NewSQLCommand("SQL.Query.GetSamplePallet")
                If Not sqlCmdSamplePallet Is Nothing Then
                    _pallet = CLng(Me.lbSamplePalletValue.Text)
                    sqlCmdSamplePallet.Parameters.AddWithValue("@PalletID", _pallet)
                    dsSamplePallet = DB.GetDataSet(sqlCmdSamplePallet)
                    sqlCmdSamplePallet.Dispose() : sqlCmdSamplePallet = Nothing
                    DB.KillSQLConnection()
                    If dsSamplePallet Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while getting Sample Pallet Information from database! Check battery and wireless connection - then try again."
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Else
                        If dsSamplePallet.Tables(0).Rows.Count > 0 Then
                            'Drop file into Loftware program for printing of pallet labels
                            Dim strLine1 As String = "*FORMAT,RDC_OW_SAMPLEPALLET"
                            Dim strLine2 As String = "HR_SERIAL," & _pallet
                            Dim strLine3 As String = "BC_SERIAL," & _pallet
                            'Get Header Info from 1st record in dataset
                            Dim strLine4 As String = "HR_ShipTo," & dsSamplePallet.Tables(0).Rows(0).Item("ShipTo").ToString
                            Dim strLine5 As String = "HR_ShipToName," & dsSamplePallet.Tables(0).Rows(0).Item("ShipToName").ToString
                            Dim strLine6 As String = "HR_Location," & dsSamplePallet.Tables(0).Rows(0).Item("Location").ToString
                            Dim strLine7 As String = "HR_ShipDate," & dsSamplePallet.Tables(0).Rows(0).Item("ShipDate").ToString
                            Dim strLine8 As String = "HR_OrderID," & Me.htxOrder.Value.ToString
                            'Initialize detail placeholders to blank
                            Dim strLine9 As String = "HR_ProductID_01," & ""
                            Dim strLine10 As String = "HR_CaseQty_01," & ""
                            Dim strLine11 As String = "HR_ProdDesc_01," & ""
                            Dim strLine12 As String = "HR_ProductID_02," & ""
                            Dim strLine13 As String = "HR_CaseQty_02," & ""
                            Dim strLine14 As String = "HR_ProdDesc_02," & ""
                            Dim strLine15 As String = "HR_ProductID_03," & ""
                            Dim strLine16 As String = "HR_CaseQty_03," & ""
                            Dim strLine17 As String = "HR_ProdDesc_03," & ""
                            Dim strLine18 As String = "HR_ProductID_04," & ""
                            Dim strLine19 As String = "HR_CaseQty_04," & ""
                            Dim strLine20 As String = "HR_ProdDesc_04," & ""
                            Dim strLine21 As String = "HR_ProductID_05," & ""
                            Dim strLine22 As String = "HR_CaseQty_05," & ""
                            Dim strLine23 As String = "HR_ProdDesc_05," & ""
                            Dim strLine24 As String = "HR_ProductID_06," & ""
                            Dim strLine25 As String = "HR_CaseQty_06," & ""
                            Dim strLine26 As String = "HR_ProdDesc_06," & ""
                            Dim strLine27 As String = "HR_ProductID_07," & ""
                            Dim strLine28 As String = "HR_CaseQty_07," & ""
                            Dim strLine29 As String = "HR_ProdDesc_07," & ""
                            Dim strLine30 As String = "HR_ProductID_08," & ""
                            Dim strLine31 As String = "HR_CaseQty_08," & ""
                            Dim strLine32 As String = "HR_ProdDesc_08," & ""
                            Dim strLine33 As String = "HR_ProductID_09," & ""
                            Dim strLine34 As String = "HR_CaseQty_09," & ""
                            Dim strLine35 As String = "HR_ProdDesc_09," & ""
                            Dim strLine36 As String = "HR_ProductID_10," & ""
                            Dim strLine37 As String = "HR_CaseQty_10," & ""
                            Dim strLine38 As String = "HR_ProdDesc_10," & ""
                            Dim strLine39 As String = "HR_ProductID_11," & ""
                            Dim strLine40 As String = "HR_CaseQty_11," & ""
                            Dim strLine41 As String = "HR_ProdDesc_11," & ""
                            Dim strLine42 As String = "HR_ProductID_12," & ""
                            Dim strLine43 As String = "HR_CaseQty_12," & ""
                            Dim strLine44 As String = "HR_ProdDesc_12," & ""
                            Dim strLine45 As String = "HR_ProductID_13," & ""
                            Dim strLine46 As String = "HR_CaseQty_13," & ""
                            Dim strLine47 As String = "HR_ProdDesc_13," & ""
                            Dim strLine48 As String = "HR_ProductID_14," & ""
                            Dim strLine49 As String = "HR_CaseQty_14," & ""
                            Dim strLine50 As String = "HR_ProdDesc_14," & ""
                            Dim strLine51 As String = "HR_ProductID_15," & ""
                            Dim strLine52 As String = "HR_CaseQty_15," & ""
                            Dim strLine53 As String = "HR_ProdDesc_15," & ""
                            Dim strLine54 As String = "HR_ProductID_16," & ""
                            Dim strLine55 As String = "HR_CaseQty_16," & ""
                            Dim strLine56 As String = "HR_ProdDesc_16," & ""
                            Dim strLine57 As String = "HR_ProductID_17," & ""
                            Dim strLine58 As String = "HR_CaseQty_17," & ""
                            Dim strLine59 As String = "HR_ProdDesc_17," & ""
                            Dim strLine60 As String = "HR_ProductID_18," & ""
                            Dim strLine61 As String = "HR_CaseQty_18," & ""
                            Dim strLine62 As String = "HR_ProdDesc_18," & ""
                            Dim strLine63 As String = "HR_ProductID_19," & ""
                            Dim strLine64 As String = "HR_CaseQty_19," & ""
                            Dim strLine65 As String = "HR_ProdDesc_19," & ""
                            Dim strLine66 As String = "HR_ProductID_20," & ""
                            Dim strLine67 As String = "HR_CaseQty_20," & ""
                            Dim strLine68 As String = "HR_ProdDesc_20," & ""
                            Dim strLine69 As String = "HR_ProductID_21," & ""
                            Dim strLine70 As String = "HR_CaseQty_21," & ""
                            Dim strLine71 As String = "HR_ProdDesc_21," & ""
                            Dim strLine72 As String = "HR_ProductID_22," & ""
                            Dim strLine73 As String = "HR_CaseQty_22," & ""
                            Dim strLine74 As String = "HR_ProdDesc_22," & ""
                            Dim strLine75 As String = "HR_ProductID_23," & ""
                            Dim strLine76 As String = "HR_CaseQty_23," & ""
                            Dim strLine77 As String = "HR_ProdDesc_23," & ""
                            Dim strLine78 As String = "HR_ProductID_24," & ""
                            Dim strLine79 As String = "HR_CaseQty_24," & ""
                            Dim strLine80 As String = "HR_ProdDesc_24," & ""
                            Dim strLine81 As String = "HR_ProductID_25," & ""
                            Dim strLine82 As String = "HR_CaseQty_25," & ""
                            Dim strLine83 As String = "HR_ProdDesc_25," & ""
                            Dim strLine84 As String = "HR_ProductID_26," & ""
                            Dim strLine85 As String = "HR_CaseQty_26," & ""
                            Dim strLine86 As String = "HR_ProdDesc_26," & ""
                            Dim strLine87 As String = "HR_ProductID_27," & ""
                            Dim strLine88 As String = "HR_CaseQty_27," & ""
                            Dim strLine89 As String = "HR_ProdDesc_27," & ""
                            Dim strLine90 As String = "HR_ProductID_28," & ""
                            Dim strLine91 As String = "HR_CaseQty_28," & ""
                            Dim strLine92 As String = "HR_ProdDesc_28," & ""
                            Dim strLine93 As String = "HR_ProductID_29," & ""
                            Dim strLine94 As String = "HR_CaseQty_29," & ""
                            Dim strLine95 As String = "HR_ProdDesc_29," & ""
                            Dim strLine96 As String = "HR_ProductID_30," & ""
                            Dim strLine97 As String = "HR_CaseQty_30," & ""
                            Dim strLine98 As String = "HR_ProdDesc_30," & ""
                            Dim strLine98_1 As String = "HR_TotalCases," & ""
                            Dim strLine98_2 As String = "HR_GrossWeight," & ""

                            For x = 0 To (dsSamplePallet.Tables(0).Rows.Count - 1)
                                Select Case x
                                    Case 0 '
                                        strLine9 = "HR_ProductID_01," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine10 = "HR_CaseQty_01," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine11 = "HR_ProdDesc_01," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 1
                                        strLine12 = "HR_ProductID_02," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine13 = "HR_CaseQty_02," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine14 = "HR_ProdDesc_02," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 2
                                        strLine15 = "HR_ProductID_03," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine16 = "HR_CaseQty_03," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine17 = "HR_ProdDesc_03," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 3
                                        strLine18 = "HR_ProductID_04," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine19 = "HR_CaseQty_04," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine20 = "HR_ProdDesc_04," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 4
                                        strLine21 = "HR_ProductID_05," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine22 = "HR_CaseQty_05," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine23 = "HR_ProdDesc_05," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 5
                                        strLine24 = "HR_ProductID_06," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine25 = "HR_CaseQty_06," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine26 = "HR_ProdDesc_06," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 6
                                        strLine27 = "HR_ProductID_07," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine28 = "HR_CaseQty_07," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine29 = "HR_ProdDesc_07," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 7
                                        strLine30 = "HR_ProductID_08," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine31 = "HR_CaseQty_08," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine32 = "HR_ProdDesc_08," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 8
                                        strLine33 = "HR_ProductID_09," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine34 = "HR_CaseQty_09," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine35 = "HR_ProdDesc_09," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 9
                                        strLine36 = "HR_ProductID_10," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine37 = "HR_CaseQty_10," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine38 = "HR_ProdDesc_10," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 10
                                        strLine39 = "HR_ProductID_11," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine40 = "HR_CaseQty_11," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine41 = "HR_ProdDesc_11," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 11
                                        strLine42 = "HR_ProductID_12," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine43 = "HR_CaseQty_12," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine44 = "HR_ProdDesc_12," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 12
                                        strLine45 = "HR_ProductID_13," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine46 = "HR_CaseQty_13," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine47 = "HR_ProdDesc_13," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 13
                                        strLine48 = "HR_ProductID_14," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine49 = "HR_CaseQty_14," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine50 = "HR_ProdDesc_14," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 14
                                        strLine51 = "HR_ProductID_15," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine52 = "HR_CaseQty_15," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine53 = "HR_ProdDesc_15," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 15
                                        strLine54 = "HR_ProductID_16," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine55 = "HR_CaseQty_16," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine56 = "HR_ProdDesc_16," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 16
                                        strLine57 = "HR_ProductID_17," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine58 = "HR_CaseQty_17," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine59 = "HR_ProdDesc_17," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 17
                                        strLine60 = "HR_ProductID_18," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine61 = "HR_CaseQty_18," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine62 = "HR_ProdDesc_18," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 18
                                        strLine63 = "HR_ProductID_19," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine64 = "HR_CaseQty_19," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine65 = "HR_ProdDesc_19," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 19
                                        strLine66 = "HR_ProductID_20," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine67 = "HR_CaseQty_20," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine68 = "HR_ProdDesc_20," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 20
                                        strLine69 = "HR_ProductID_21," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine70 = "HR_CaseQty_21," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine71 = "HR_ProdDesc_21," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 21
                                        strLine72 = "HR_ProductID_22," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine73 = "HR_CaseQty_22," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine74 = "HR_ProdDesc_22," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 22
                                        strLine75 = "HR_ProductID_23," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine76 = "HR_CaseQty_23," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine77 = "HR_ProdDesc_23," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 23
                                        strLine78 = "HR_ProductID_24," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine79 = "HR_CaseQty_24," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine80 = "HR_ProdDesc_24," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 24
                                        strLine81 = "HR_ProductID_25," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine82 = "HR_CaseQty_25," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine83 = "HR_ProdDesc_25," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 25
                                        strLine84 = "HR_ProductID_26," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine85 = "HR_CaseQty_26," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine86 = "HR_ProdDesc_26," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 26
                                        strLine87 = "HR_ProductID_27," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine88 = "HR_CaseQty_27," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine89 = "HR_ProdDesc_27," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 27
                                        strLine90 = "HR_ProductID_28," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine91 = "HR_CaseQty_28," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine92 = "HR_ProdDesc_28," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 28
                                        strLine93 = "HR_ProductID_29," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine94 = "HR_CaseQty_29," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine95 = "HR_ProdDesc_29," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 29
                                        strLine96 = "HR_ProductID_30," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine97 = "HR_CaseQty_30," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine98 = "HR_ProdDesc_30," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                End Select
                            Next
                            strLine98_1 = "HR_TotalCases," & totalcases
                            Common.SaveVariable("HR_TotalCases", totalcases, Page)

                            strLine98_2 = "HR_GrossWeight," & grossweight
                            Common.SaveVariable("HR_GrossWeight", grossweight, Page)

                            Dim strLine99 As String = "*QUANTITY,1"
                            Dim strLine100 As String = "*PRINTERNUMBER," & _printer
                            Dim strLine101 As String = "*PRINTLABEL"

                            destPath = "\\192.168.5.4\wddrop\"
                            'destPath = ConfigurationManager.AppSettings.Get("LW.DestPath").ToString
                            tempPath = "\\192.168.5.4\wddroptemp\"
                            'tempPath = ConfigurationManager.AppSettings.Get("LW.TempDestPath").ToString

                            filenm = Trim(_pallet) & DatePart("yyyy", Now) & DatePart("m", Now) & DatePart("d", Now) & DatePart("h", Now) & DatePart("n", Now) & DatePart("s", Now) & ".pas"
                            PathFile = tempPath & filenm

                            'declaring a FileStream and creating a text document file named file with access mode of writing
                            Dim fs As New FileStream(PathFile, FileMode.Create, FileAccess.Write)
                            'creating a new StreamWriter and passing the filestream object fs as argument
                            Dim s As New StreamWriter(fs)
                            'the seek method is used to move the cursor to next position to avoid text to be overwritten
                            s.BaseStream.Seek(0, SeekOrigin.End)

                            'Write each line of the pas file required by Loftware
                            s.WriteLine(strLine1)
                            s.WriteLine(strLine2)
                            s.WriteLine(strLine3)
                            s.WriteLine(strLine4)
                            s.WriteLine(strLine5)
                            s.WriteLine(strLine6)
                            s.WriteLine(strLine7)
                            s.WriteLine(strLine8)
                            s.WriteLine(strLine9)
                            s.WriteLine(strLine10)
                            s.WriteLine(strLine11)
                            s.WriteLine(strLine12)
                            s.WriteLine(strLine13)
                            s.WriteLine(strLine14)
                            s.WriteLine(strLine15)
                            s.WriteLine(strLine16)
                            s.WriteLine(strLine17)
                            s.WriteLine(strLine18)
                            s.WriteLine(strLine19)
                            s.WriteLine(strLine20)
                            s.WriteLine(strLine21)
                            s.WriteLine(strLine22)
                            s.WriteLine(strLine23)
                            s.WriteLine(strLine24)
                            s.WriteLine(strLine25)
                            s.WriteLine(strLine26)
                            s.WriteLine(strLine27)
                            s.WriteLine(strLine28)
                            s.WriteLine(strLine29)
                            s.WriteLine(strLine30)
                            s.WriteLine(strLine31)
                            s.WriteLine(strLine32)
                            s.WriteLine(strLine33)
                            s.WriteLine(strLine34)
                            s.WriteLine(strLine35)
                            s.WriteLine(strLine36)
                            s.WriteLine(strLine37)
                            s.WriteLine(strLine38)
                            s.WriteLine(strLine39)
                            s.WriteLine(strLine40)
                            s.WriteLine(strLine41)
                            s.WriteLine(strLine42)
                            s.WriteLine(strLine43)
                            s.WriteLine(strLine44)
                            s.WriteLine(strLine45)
                            s.WriteLine(strLine46)
                            s.WriteLine(strLine47)
                            s.WriteLine(strLine48)
                            s.WriteLine(strLine49)
                            s.WriteLine(strLine50)
                            s.WriteLine(strLine51)
                            s.WriteLine(strLine52)
                            s.WriteLine(strLine53)
                            s.WriteLine(strLine54)
                            s.WriteLine(strLine55)
                            s.WriteLine(strLine56)
                            s.WriteLine(strLine57)
                            s.WriteLine(strLine58)
                            s.WriteLine(strLine59)
                            s.WriteLine(strLine60)
                            s.WriteLine(strLine61)
                            s.WriteLine(strLine62)
                            s.WriteLine(strLine63)
                            s.WriteLine(strLine64)
                            s.WriteLine(strLine65)
                            s.WriteLine(strLine66)
                            s.WriteLine(strLine67)
                            s.WriteLine(strLine68)
                            s.WriteLine(strLine69)
                            s.WriteLine(strLine70)
                            s.WriteLine(strLine71)
                            s.WriteLine(strLine72)
                            s.WriteLine(strLine73)
                            s.WriteLine(strLine74)
                            s.WriteLine(strLine75)
                            s.WriteLine(strLine76)
                            s.WriteLine(strLine77)
                            s.WriteLine(strLine78)
                            s.WriteLine(strLine79)
                            s.WriteLine(strLine80)
                            s.WriteLine(strLine81)
                            s.WriteLine(strLine82)
                            s.WriteLine(strLine83)
                            s.WriteLine(strLine84)
                            s.WriteLine(strLine85)
                            s.WriteLine(strLine86)
                            s.WriteLine(strLine87)
                            s.WriteLine(strLine88)
                            s.WriteLine(strLine89)
                            s.WriteLine(strLine90)
                            s.WriteLine(strLine91)
                            s.WriteLine(strLine92)
                            s.WriteLine(strLine93)
                            s.WriteLine(strLine94)
                            s.WriteLine(strLine95)
                            s.WriteLine(strLine96)
                            s.WriteLine(strLine97)
                            s.WriteLine(strLine98)
                            s.WriteLine(strLine98_1)
                            s.WriteLine(strLine98_2)
                            s.WriteLine(strLine99)
                            s.WriteLine(strLine100)
                            s.WriteLine(strLine101)
                            s.Close()

                            sourceFile = tempPath & filenm
                            successfile = tempPath & filenm & ".success"
                            destinationFile = destPath & filenm

                            'Delete destinationFile for overwrite,
                            'causes exception if already exists.
                            File.Delete(destinationFile)
                            File.Copy(sourceFile, destinationFile)

                            rtnval = True
                        Else 'Record does not exist for Pallet entered
                            Me.lbError.Text = "Pallet# not in system - an error occurred while writing the records - see supervisor."
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            rtnval = False
                        End If
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while printing Shipping Pallet Tag! Check battery and wireless connection - then try again."
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    rtnval = False
                End If
            Else
                Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                rtnval = False
            End If
        Catch ex As Exception
            rtnval = False
        End Try

        Return rtnval
    End Function

#End Region

End Class