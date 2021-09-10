Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_ShippingPalletsReAssign
    Inherits System.Web.UI.Page

    Private Enum ScanModes
        OrderMode
        PalletMode
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
    Public _screenmode As String

#End Region

#Region "Page Level"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
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
        
        If Not Page.IsPostBack Then
            Mode = ScanModes.OrderMode
            Call ResetControls()
        End If


    End Sub

    Public Sub ResetControls()
        Select Case Mode
            Case ScanModes.OrderMode 'Prepare screen to accept Pick Order Scan
                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.txPallet.Visible = True
                Me.lbPallet.Visible = True
                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False
                Me.btReturn.Visible = True
                Me.btRestart.Visible = False
                Me.htxOrder.Value = ""
                Me.txPallet.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""
                Me.lbPrompt.Text = "Scan or Enter Pick Order."
                Common.JavaScriptSetFocus(Page, Me.htxOrder)

            Case ScanModes.PalletMode 'Prepare screen to accept Pick Product Scan
                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.txPallet.Visible = True
                Me.lbPallet.Visible = True
                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False
                Me.btReturn.Visible = True
                Me.btRestart.Visible = True
                Me.txPallet.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""
                Me.lbPrompt.Text = "Scan or Enter Existing Shipping Pallet."
                Common.JavaScriptSetFocus(Page, Me.txPallet)

            Case ScanModes.BinMode  'Prepare screen to accept bin location entry
                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.txPallet.Visible = True
                Me.lbPallet.Visible = True
                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False
                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""
                Me.lbPrompt.Text = "Scan or Enter Bin Location for Pallet."
                Common.JavaScriptSetFocus(Page, Me.txBin)

            Case ScanModes.PrinterMode 'Prepare screen to accept Printer # entry
                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.txPallet.Visible = True
                Me.lbPallet.Visible = True
                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = True
                Me.lbPrinter.Visible = True
                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.txPrinter.Text = ""
                Me.lbPrompt.Text = "Enter Printer #."
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
        End Select
    End Sub

#End Region

#Region "User Entry"

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        'Validate Order and Pallet
        ValidateEntries(Me.htxOrder.Value, Me.txPallet.Text)
        If Me.lbError.Text.Length > 0 Then
            'Error occurred during validation

        Else
            'Both passed validation - continue process
            Mode = ScanModes.BinMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        End If
    End Sub

    Protected Sub txBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txBin.TextChanged
        'Verify the Bin Location entered is valid in database
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdTruck As New System.Data.SqlClient.SqlCommand
        Dim sTruck As Integer = 0

        Me.txBin.Text = UCase(Me.txBin.Text)
        Try
            Me.lbError.Text = ""

            If Me.txBin.Text.Length > 0 Then
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdBin = DB.NewSQLCommand("SQL.Query.BinLookup")
                    If Not sqlCmdBin Is Nothing Then
                        sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txBin.Text)
                        _tobin = sqlCmdBin.ExecuteScalar()
                        sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                        DB.KillSQLConnection()

                        If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
                            Me.lbError.Text = "Invalid Bin Location entered. Try again."
                            Common.JavaScriptSetFocus(Page, Me.txBin)
                        Else 'Bin Location is valid
                            Dim strPallet As String = Me.txPallet.Text

                            'Test to see if order is for UPS or Fedex -- If Truck exists in exception table with NoPalletIDPrint 
                            If Not DB.MakeSQLConnection("Warehouse") Then
                                sqlCmdTruck = DB.NewSQLCommand("SQL.Query.SkipPalletPrintForTruck")
                                If Not sqlCmdBin Is Nothing Then
                                    sqlCmdTruck.Parameters.AddWithValue("@OrderID", CLng(Me.htxOrder.Value))
                                    sTruck = sqlCmdTruck.ExecuteScalar()
                                    sqlCmdTruck.Dispose() : sqlCmdTruck = Nothing
                                    DB.KillSQLConnection()
                                End If
                            End If

                            If sTruck > 0 Then 'Truck is in the exception table
                                'Write records to IC_PalletsHeader, IC_Pallets, IC_PalletsHistory
                                SavePallet(strPallet & Me.htxOrder.Value)
                                
                                Me.txPallet.Text = ""
                                Mode = ScanModes.OrderMode
                                Common.SaveVariable("8SavedMode", Mode, Page)
                                ResetControls()
                            Else
                                'Get Printer #
                                Mode = ScanModes.PrinterMode
                                Common.SaveVariable("8SavedMode", Mode, Page)
                                ResetControls()
                            End If
                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
                        Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                        Common.JavaScriptSetFocus(Page, Me.txBin)
                    End If
                Else
                    Me.lbError.Text = "Communication Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
                    Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                    Common.JavaScriptSetFocus(Page, Me.txBin)
                End If
            Else 'Nothing entered on screen for Bin Location
                Me.lbError.Text = "Bin Location must be entered."
                Common.JavaScriptSetFocus(Page, Me.txBin)
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
        Try
            Me.lbError.Text = ""

            'Verify printer # entered is for correct location and is numeric
            If Me.txPrinter.Text.Length = 1 And IsNumeric(Me.txPrinter.Text) = True Then
                Dim iPrt As Integer = CInt(Me.txPrinter.Text)
                Dim strPallet As String = Me.txPallet.Text

                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdPrinter = DB.NewSQLCommand("SQL.Query.PrinterLookup")
                    If Not sqlCmdPrinter Is Nothing Then
                        sqlCmdPrinter.Parameters.AddWithValue("@Printer", iPrt)
                        sqlCmdPrinter.Parameters.AddWithValue("@Warehouse", 20)
                        _printer = sqlCmdPrinter.ExecuteScalar()
                        sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                        DB.KillSQLConnection()

                        If RTrim(_printer).Length < 1 Then 'Printer does not exist in database
                            Me.lbError.Text = "Invalid Printer # entered. Try again."
                            'Save the bad Printer to display on screen for correction
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                        Else 'Printer is valid
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            Me.lbError.Text = ""
                            SavePallet(strPallet & Me.htxOrder.Value)

                            If Me.lbError.Text = "" Then
                                'Write file to Loftware server to print out the Pallet Tag
                                If PrintShippingPalletTag() = True Then
                                    Me.txPallet.Text = ""
                                    Mode = ScanModes.OrderMode
                                    Common.SaveVariable("8SavedMode", Mode, Page)
                                    ResetControls()
                                Else
                                    Common.SaveVariable("PrintSecondTime", "Y", Page)
                                    Me.lbError.Text = "Shipping Pallet Tag failed to print. Check Wireless Connection and then try entering Printer again."
                                    Mode = ScanModes.PrinterMode
                                    Common.SaveVariable("8SavedMode", Mode, Page)
                                    ResetControls()
                                End If
                            Else
                                Me.lbError.Text = Me.lbError.Text.ToString + " See your supervisor, Last Shipping Pallet failed to update in system!"
                            End If
                        End If
                    Else
                        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                        Me.lbError.Text = "Error occurred while validating Printer - try again or see your supervisor!"
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    End If
                Else
                    Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                    Me.lbError.Text = "Communication Error occurred while validating Printer - try again or see your supervisor!"
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                End If
            Else
                Me.lbError.Text = "Printer # needs to be single digit numeric entry!"
                Common.SaveVariable("Printer", "", Page)
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
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
        'delete records from IC_TmpShipPallets where PalletID, OrderID, UserID match current screen
        Try
            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        Catch ex As Exception
            Me.lbError.Text = "Error occurred during restart"
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        Common.SaveVariable("ScreenParam", Nothing, Page)
        'Setup Main Menu URL so program will redirect to Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

#End Region

#Region "Custom Processes"

    Public Sub SavePallet(ByVal oPallet As String)
        Dim sqlPalletInsert As String = ""
        Dim strPallet As String = Mid(oPallet.ToString, 1, 9)
        Dim strOrder As String = Mid(oPallet.ToString, 10, 9)
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString

        Me.lbError.Text = ""

        'GetNextShippingPallet
        GetNextShippingPallet()
        Dim NewShippingPallet As Long = Common.GetVariable("nNewShippingPallet", Page)

        Try
            'Insert Pallet records
            _dateentered = Now()
            sqlPalletInsert = "spReAssignShippingPallet "

            Dim SumCaseQty As Integer = 0
            Dim sqlString As String
            Dim sqlString2 As String
            Dim dv As New DataView
            Dim dsShippingPalletSave As New Data.DataSet
            Dim sqlCmdShippingPalletSave As New System.Data.SqlClient.SqlCommand
            Dim strLineNumber As String = Nothing
            Dim sqlCmdLineNumber As New System.Data.SqlClient.SqlCommand

            sqlString = "Select * from IC_Pallets Where pk_nPallet = " & CLng(strPallet)

            'Get IC_Pallets records and write new Shipping Pallet to IC_PalletsHeader and IC_Pallets tables
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdShippingPalletSave = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdShippingPalletSave Is Nothing Then
                    dsShippingPalletSave = DB.GetDataSet(sqlCmdShippingPalletSave)
                    sqlCmdShippingPalletSave.Dispose() : sqlCmdShippingPalletSave = Nothing
                    DB.KillSQLConnection()
                End If
            End If

            If dsShippingPalletSave.Tables(0).Rows.Count < 1 Then
                Me.lbError.Text = "Data Error occurred while saving Shipping Pallet Information to database! Check battery and wireless connection - then try again."
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
            Else
                'records retrieved - process through each row and Insert Pallet records
                Dim drShippingPalletSave As Data.DataRow
                If Not DB.MakeSQLConnection("Warehouse") Then
                    For Each drShippingPalletSave In dsShippingPalletSave.Tables(0).Rows
                        'records retrieved - process through each row to get current order - LineNumber
                        Try
                            sqlString2 = "Select Top 1 OrderLine from vwOrderDetailGTIN Where GTIN = '" & drShippingPalletSave.Item("GTIN") & "' and OrderID = " & CLng(Me.htxOrder.Value)

                            If Not DB.MakeSQLConnection("Warehouse") Then
                                sqlCmdLineNumber = DB.NewSQLCommand3(sqlString2)
                                If Not sqlCmdLineNumber Is Nothing Then
                                    strLineNumber = sqlCmdLineNumber.ExecuteScalar
                                    sqlCmdLineNumber.Dispose() : sqlCmdLineNumber = Nothing
                                    DB.KillSQLConnection()

                                    If strLineNumber.Length < 1 Then
                                        Me.lbError.Text = "Failed to get Order Line Number from database! Try again or see your supervisor."
                                    End If
                                Else
                                    sqlCmdLineNumber.Dispose() : sqlCmdLineNumber = Nothing
                                    DB.KillSQLConnection()
                                    Me.lbError.Text = "Command Error occurred while getting Order Line Number from database! Check Battery and Wireless Connection -then try again or see your supervisor."
                                End If
                            Else
                                Me.lbError.Text = "Error while connecting to database! Try again or see your supervisor."
                            End If

                            'Handle errors
                            If Me.lbError.Text.Length > 0 Then
                                Mode = ScanModes.PalletMode
                                Common.SaveVariable("8SavedMode", Mode, Page)
                                DB.KillSQLConnection()
                                ResetControls()
                                Exit Sub
                            End If
                        Catch ex As Exception
                            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while getting Order Line Number from database! Check Battery and Wireless Connection -then try again or see your supervisor."
                            Mode = ScanModes.PalletMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            DB.KillSQLConnection()
                            ResetControls()
                            Exit Sub
                        Finally
                            DB.KillSQLConnection()
                        End Try

                        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
                        If Not DB.MakeSQLConnection("Warehouse") Then
                            sqlCmdPallet = DB.NewSQLCommand2(sqlPalletInsert)
                            If Not sqlCmdPallet Is Nothing Then
                                sqlCmdPallet.Parameters.AddWithValue("@pnPallet", NewShippingPallet) '
                                sqlCmdPallet.Parameters.AddWithValue("@pnOrder", CLng(strOrder)) '
                                sqlCmdPallet.Parameters.AddWithValue("@pdtmScanned", _dateentered) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrUnit", strUnit) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrProduct", drShippingPalletSave.Item("GTIN")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pnLineNumber", CLng(strLineNumber)) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrExpiration", drShippingPalletSave.Item("CodeDate")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrMfgOrd", drShippingPalletSave.Item("MfgOrd")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrLot", drShippingPalletSave.Item("Lot")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pnQuantity", drShippingPalletSave.Item("PalletQty")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrBin", RTrim(Me.txBin.Text)) '
                                sqlCmdPallet.Parameters.AddWithValue("@pstrProdVariant", drShippingPalletSave.Item("ProdVariant")) '
                                sqlCmdPallet.Parameters.AddWithValue("@pnOldPallet", CLng(Me.txPallet.Text))
                                sqlCmdPallet.ExecuteNonQuery()
                                sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                            Else
                                Me.lbError.Text = "Command Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                            End If
                        Else
                            Me.lbError.Text = "Connection Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                        End If

                        If Not sqlCmdPallet Is Nothing Then
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                        End If
                    Next
                Else
                    Me.lbError.Text = "Communication Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                End If

            End If
        Catch e As Exception
            Me.lbError.Text = "Error - " & e.ToString & " - occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Sub ValidateEntries(ByVal orderid As String, ByVal pallet As String)
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
        'Validate Pallet entered exists in database
        Try
            Dim strPallet As String = ""
            Dim sqlCmdShippingPalletSave As New System.Data.SqlClient.SqlCommand
            Dim sqlString As String = "Select pk_nPallet from IC_PalletsHeader Where pk_nPallet = " & CLng(pallet) & " And [Status] <> 'V'"

            'Get IC_Pallets records and write new Shipping Pallet to IC_PalletsHeader and IC_Pallets tables
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdShippingPalletSave = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdShippingPalletSave Is Nothing Then
                    strPallet = sqlCmdShippingPalletSave.ExecuteScalar
                    sqlCmdShippingPalletSave.Dispose() : sqlCmdShippingPalletSave = Nothing
                    DB.KillSQLConnection()
                    If strPallet.Length < 1 Then
                        Me.lbError.Text = "Error occurred while validating Pallet # entered. Have supervisor lookup pallet on KnowledgeMine."
                    End If
                End If
            End If

            'Handle errors
            If Me.lbError.Text.Length > 0 Then
                Mode = ScanModes.PalletMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                DB.KillSQLConnection()
                ResetControls()
                Exit Sub
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Pick Product! Check Battery and Wireless Connection -then try again or see your supervisor."
            Mode = ScanModes.PalletMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            DB.KillSQLConnection()
            ResetControls()
            Exit Sub
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function PrintShippingPalletTag()
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

            'Used for getting ShipTo Info and Shipping Pallet Details From vwIC_OW_ShippingPallets
            Dim dsShippingPallet As New Data.DataSet
            Dim sqlCmdShippingPallet As New System.Data.SqlClient.SqlCommand

            'Get Shipto Info for ShippingPallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdShippingPallet = DB.NewSQLCommand("SQL.Query.GetShippingPallet")
                If Not sqlCmdShippingPallet Is Nothing Then
                    _pallet = Common.GetVariable("nNewShippingPallet", Page)
                    sqlCmdShippingPallet.Parameters.AddWithValue("@PalletID", _pallet)
                    dsShippingPallet = DB.GetDataSet(sqlCmdShippingPallet)
                    sqlCmdShippingPallet.Dispose() : sqlCmdShippingPallet = Nothing
                    DB.KillSQLConnection()
                    If dsShippingPallet Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while getting Shipping Pallet Information from database! Check battery and wireless connection - then try again."
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Else
                        If dsShippingPallet.Tables(0).Rows.Count > 0 Then
                            'Drop file into Loftware program for printing of pallet labels
                            Dim strLine1 As String = "*FORMAT,RDC_OW_SHIPPINGPALLET"
                            Dim strLine2 As String = "HR_SERIAL," & _pallet
                            Dim strLine3 As String = "BC_SERIAL," & _pallet
                            'Get Header Info from 1st record in dataset
                            Dim strLine4 As String = "HR_ShipTo," & dsShippingPallet.Tables(0).Rows(0).Item("ShipTo").ToString
                            Dim strLine5 As String = "HR_ShipToName," & dsShippingPallet.Tables(0).Rows(0).Item("ShipToName").ToString
                            Dim strLine6 As String = "HR_Location," & dsShippingPallet.Tables(0).Rows(0).Item("Location").ToString
                            Dim strLine7 As String = "HR_ShipDate," & dsShippingPallet.Tables(0).Rows(0).Item("ShipDate").ToString
                            'Added CBCShipDate to the Pallet Tag - SAM - 3/7/2012
                            Dim strLine7_1 As String = "HR_CB_ShipDate," & dsShippingPallet.Tables(0).Rows(0).Item("CBCShipDate").ToString

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

                            For x = 0 To (dsShippingPallet.Tables(0).Rows.Count - 1)
                                Select Case x
                                    Case 0 '
                                        strLine9 = "HR_ProductID_01," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine10 = "HR_CaseQty_01," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine11 = "HR_ProdDesc_01," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 1
                                        strLine12 = "HR_ProductID_02," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine13 = "HR_CaseQty_02," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine14 = "HR_ProdDesc_02," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 2
                                        strLine15 = "HR_ProductID_03," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine16 = "HR_CaseQty_03," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine17 = "HR_ProdDesc_03," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 3
                                        strLine18 = "HR_ProductID_04," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine19 = "HR_CaseQty_04," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine20 = "HR_ProdDesc_04," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 4
                                        strLine21 = "HR_ProductID_05," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine22 = "HR_CaseQty_05," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine23 = "HR_ProdDesc_05," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 5
                                        strLine24 = "HR_ProductID_06," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine25 = "HR_CaseQty_06," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine26 = "HR_ProdDesc_06," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 6
                                        strLine27 = "HR_ProductID_07," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine28 = "HR_CaseQty_07," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine29 = "HR_ProdDesc_07," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 7
                                        strLine30 = "HR_ProductID_08," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine31 = "HR_CaseQty_08," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine32 = "HR_ProdDesc_08," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 8
                                        strLine33 = "HR_ProductID_09," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine34 = "HR_CaseQty_09," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine35 = "HR_ProdDesc_09," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 9
                                        strLine36 = "HR_ProductID_10," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine37 = "HR_CaseQty_10," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine38 = "HR_ProdDesc_10," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 10
                                        strLine39 = "HR_ProductID_11," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine40 = "HR_CaseQty_11," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine41 = "HR_ProdDesc_11," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 11
                                        strLine42 = "HR_ProductID_12," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine43 = "HR_CaseQty_12," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine44 = "HR_ProdDesc_12," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 12
                                        strLine45 = "HR_ProductID_13," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine46 = "HR_CaseQty_13," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine47 = "HR_ProdDesc_13," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 13
                                        strLine48 = "HR_ProductID_14," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine49 = "HR_CaseQty_14," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine50 = "HR_ProdDesc_14," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 14
                                        strLine51 = "HR_ProductID_15," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine52 = "HR_CaseQty_15," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine53 = "HR_ProdDesc_15," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 15
                                        strLine54 = "HR_ProductID_16," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine55 = "HR_CaseQty_16," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine56 = "HR_ProdDesc_16," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 16
                                        strLine57 = "HR_ProductID_17," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine58 = "HR_CaseQty_17," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine59 = "HR_ProdDesc_17," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 17
                                        strLine60 = "HR_ProductID_18," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine61 = "HR_CaseQty_18," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine62 = "HR_ProdDesc_18," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 18
                                        strLine63 = "HR_ProductID_19," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine64 = "HR_CaseQty_19," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine65 = "HR_ProdDesc_19," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 19
                                        strLine66 = "HR_ProductID_20," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine67 = "HR_CaseQty_20," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine68 = "HR_ProdDesc_20," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 20
                                        strLine69 = "HR_ProductID_21," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine70 = "HR_CaseQty_21," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine71 = "HR_ProdDesc_21," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 21
                                        strLine72 = "HR_ProductID_22," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine73 = "HR_CaseQty_22," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine74 = "HR_ProdDesc_22," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 22
                                        strLine75 = "HR_ProductID_23," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine76 = "HR_CaseQty_23," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine77 = "HR_ProdDesc_23," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 23
                                        strLine78 = "HR_ProductID_24," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine79 = "HR_CaseQty_24," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine80 = "HR_ProdDesc_24," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 24
                                        strLine81 = "HR_ProductID_25," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine82 = "HR_CaseQty_25," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine83 = "HR_ProdDesc_25," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 25
                                        strLine84 = "HR_ProductID_26," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine85 = "HR_CaseQty_26," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine86 = "HR_ProdDesc_26," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 26
                                        strLine87 = "HR_ProductID_27," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine88 = "HR_CaseQty_27," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine89 = "HR_ProdDesc_27," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 27
                                        strLine90 = "HR_ProductID_28," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine91 = "HR_CaseQty_28," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine92 = "HR_ProdDesc_28," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 28
                                        strLine93 = "HR_ProductID_29," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine94 = "HR_CaseQty_29," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine95 = "HR_ProdDesc_29," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 29
                                        strLine96 = "HR_ProductID_30," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine97 = "HR_CaseQty_30," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine98 = "HR_ProdDesc_30," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
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
                            'Added CBCShipDate to the Pallet Tag - SAM - 3/7/2012
                            s.WriteLine(strLine7_1)

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

                            'Print ShipTo Tag 
                            Dim destPathST As String = ""
                            Dim tempPathST As String = ""
                            Dim filenmST As String = ""
                            Dim PathFileST As String = ""
                            Dim sourceFileST As String = ""
                            Dim successfileST As String = ""
                            Dim destinationFileST As String = ""
                            Dim _shiptotag As String = _pallet & "STTag"


                            'Drop file into Loftware program for printing of pallet labels
                            Dim strLineST1 As String = "*FORMAT,RDC_OW_SHIPTO_TAG"
                            Dim strLineST2 As String = strLine5.ToString 'ShiptoName from above
                            Dim strLineST3 As String = strLine6.ToString 'Location from above
                            Dim strLineST4 As String = strLine4.ToString 'Shipto from above
                            Dim strLineST5 As String = "HR_PalletID," & _pallet
                            Dim strLineST6 As String = "HR_TotalCases," & totalcases
                            Dim strLineST7 As String = "HR_GrossWeight," & grossweight
                            Dim strLineST8 As String = "*QUANTITY,1"
                            Dim strLineST9 As String = "*PRINTERNUMBER," & _printer
                            Dim strLineST10 As String = "*PRINTLABEL"

                            destPath = "\\192.168.5.4\wddrop\"
                            'destPath = ConfigurationManager.AppSettings.Get("LW.DestPath").ToString
                            tempPath = "\\192.168.5.4\wddroptemp\"
                            'tempPath = ConfigurationManager.AppSettings.Get("LW.TempDestPath").ToString

                            filenmST = Trim(_shiptotag) & DatePart("yyyy", Now) & DatePart("m", Now) & DatePart("d", Now) & DatePart("h", Now) & DatePart("n", Now) & DatePart("s", Now) & ".pas"
                            PathFileST = tempPathST & filenmST

                            'declaring a FileStream and creating a text document file named file with access mode of writing
                            Dim fsST As New FileStream(PathFileST, FileMode.Create, FileAccess.Write)
                            'creating a new StreamWriter and passing the filestream object fs as argument
                            Dim sST As New StreamWriter(fsST)
                            'the seek method is used to move the cursor to next position to avoid text to be overwritten
                            sST.BaseStream.Seek(0, SeekOrigin.End)

                            'Write each line of the pas file required by Loftware
                            sST.WriteLine(strLineST1)
                            sST.WriteLine(strLineST2)
                            sST.WriteLine(strLineST3)
                            sST.WriteLine(strLineST4)
                            sST.WriteLine(strLineST5)
                            sST.WriteLine(strLineST6)
                            sST.WriteLine(strLineST7)
                            sST.WriteLine(strLineST8)
                            sST.WriteLine(strLineST9)
                            sST.WriteLine(strLineST10)
                            sST.Close()

                            sourceFileST = tempPathST & filenmST
                            successfileST = tempPathST & filenmST & ".success"
                            destinationFileST = destPathST & filenmST

                            'Delete destinationFile for overwrite,
                            'causes exception if already exists.
                            File.Delete(destinationFileST)
                            File.Copy(sourceFileST, destinationFileST)

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

    Public Sub GetNextShippingPallet()
        Try
            Dim ShippingPallet As Integer
            ShippingPallet = 0
            'Get next pallet id for shipping pallets using stored procedure
            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdShipPallet As New System.Data.SqlClient.SqlCommand
                sqlCmdShipPallet = DB.NewSQLCommand2("spNextShipPallet ")
                If Not sqlCmdShipPallet Is Nothing Then
                    sqlCmdShipPallet.Parameters.AddWithValue("@nShipPallet", 0)
                    sqlCmdShipPallet.Parameters("@nShipPallet").Direction = ParameterDirection.Output

                    ShippingPallet = sqlCmdShipPallet.ExecuteScalar()
                    If IsDBNull(ShippingPallet) = False And ShippingPallet <> 0 Then 'Successfully got New Shipping Pallet #
                        Common.SaveVariable("nNewShippingPallet", ShippingPallet, Page)
                        Me.lbError.Text = ""
                    Else
                        'Error getting New Pallet # prompt user to try again
                        Me.lbError.Text = "Data Error occurred while getting New Shipping Pallet # - Check Connection, then Press Restart button!"
                    End If
                Else
                    'Error getting New Pallet # prompt user to try again
                    Me.lbError.Text = "Command Error occurred while getting New Shipping Pallet # - Check Connection, then Press Restart button!"
                End If
            Else
                'Error getting New Pallet # prompt user to try again
                Me.lbError.Text = "Connection Error occurred while getting New Pallet # - Check Connection, then Press Restart button!"
            End If
        Catch ex As Exception
            'Error getting New Pallet # prompt user to try again
            Me.lbError.Text = "General Error occurred while getting New Pallet # - Check Connection, then Press Restart button!"
        End Try
    End Sub

#End Region

End Class