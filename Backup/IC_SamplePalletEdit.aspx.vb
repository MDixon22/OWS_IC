Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_SamplePalletEdit
    Inherits System.Web.UI.Page

    Private Enum ScanModes
        StartMode
        OrderMode
        ProductMode
        CaseLabelMode
        QuantityMode
        BinMode
        PrinterMode
    End Enum

#Region "Variable Declaration"
    Private scanmode As String
    Private Mode As ScanModes
    Public _company As String
    Public _whs As String
    Public _dateentered As Date
    Public strURL As String
    Public validation_count As Integer = 0
    Public _tobin As String
    Public _printer As String
    Public _remqtyflag As String
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

        If _screenmode = "ChgQty" Then
            Me.lbPageTitle.Text = "OW Inventory - Sample Pallet Prod Qty Change"

        ElseIf _screenmode = "Remove" Then
            Me.lbPageTitle.Text = "OW Inventory - Sample Pallet Remove Product"
            Me.lbQuantity.Text = "Confirm Delete"
        ElseIf _screenmode = "AddTo" Then
            Me.lbPageTitle.Text = "OW Inventory - Sample Pallet Add Product"
        End If

        Me.lbExistingGridTitle.Text = "Existing Pallet Products"

        If Not Page.IsPostBack Then
            Common.SaveVariable("UpdatedSamplePallet", "", Page)
            Me.lbShipPalletValue.Text = Common.GetVariable("EditPalletNumber", Page)
            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            Call ResetControls()
        End If
    End Sub

    Public Sub ResetControls() 'fix this process - add back in the case label field
        Select Case Mode

            Case ScanModes.OrderMode 'Prepare screen to accept Pick Order Scan
                Me.lbShipPallet.Visible = True
                Me.lbShipPalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxCaseLabel.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True

                Me.htxOrder.Value = ""
                Me.htxCaseLabel.Value = ""
                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Scan Pick Order"
                Common.JavaScriptSetFocus(Page, Me.htxOrder)

            Case ScanModes.CaseLabelMode 'Prepare screen to accept CaseLabel Scan
                Me.lbShipPallet.Visible = True
                Me.lbShipPalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxCaseLabel.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True

                Me.htxCaseLabel.Value = ""
                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Scan or Enter Case Label"
                Common.JavaScriptSetFocus(Page, Me.htxCaseLabel)

            Case ScanModes.QuantityMode 'Prepare screen to accept quantity entry
                Me.lbShipPallet.Visible = True
                Me.lbShipPalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxCaseLabel.Visible = True
                Me.lbCaseLabel.Visible = True
                Me.txQuantity.Visible = True
                Me.lbQuantity.Visible = True

                Me.txBin.Visible = False
                Me.lbBin.Visible = False
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True

                Me.txQuantity.Text = ""
                Me.txBin.Text = ""
                Me.txPrinter.Text = ""
                If _screenmode = "ChgQty" Then
                    Me.lbPrompt.Text = "Enter # of Cases for Product"
                    Me.lbQuantity.Text = "New Total Qty-"
                ElseIf _screenmode = "Remove" Then
                    Me.lbPrompt.Text = "Enter Y or N to Confirm Product Delete"
                    Me.lbQuantity.Text = "Confirm Delete-"
                ElseIf _screenmode = "AddTo" Then
                    Me.lbPrompt.Text = "Enter # of Cases for Product"
                    Me.lbQuantity.Text = "Qty-"
                End If

                Common.JavaScriptSetFocus(Page, Me.txQuantity)

            Case ScanModes.BinMode  'Prepare screen to accept bin location entry
                Me.lbShipPallet.Visible = True
                Me.lbShipPalletValue.Visible = True

                Me.htxOrder.Visible = False
                Me.lbOrder.Visible = False
                Me.htxCaseLabel.Visible = False
                Me.lbCaseLabel.Visible = False
                Me.txQuantity.Visible = False
                Me.lbQuantity.Visible = False

                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = False
                Me.lbPrinter.Visible = False

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True

                Me.txBin.Text = ""
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Enter Bin Location for Pallet"
                Common.JavaScriptSetFocus(Page, Me.txBin)

            Case ScanModes.PrinterMode 'Prepare screen to accept Printer # entry
                Me.lbShipPallet.Visible = True
                Me.lbShipPalletValue.Visible = True

                Me.htxOrder.Visible = True
                Me.lbOrder.Visible = True
                Me.htxCaseLabel.Visible = False
                Me.lbCaseLabel.Visible = False
                Me.txQuantity.Visible = False
                Me.lbQuantity.Visible = False

                Me.txBin.Visible = True
                Me.lbBin.Visible = True
                Me.txPrinter.Visible = True
                Me.lbPrinter.Visible = True

                Me.btRestart.Visible = True
                Me.btReturn.Visible = True
                Me.txPrinter.Text = ""

                Me.lbPrompt.Text = "Enter Printer #"
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
        End Select
        Try
            Me.LoadExistingPalletGrid()
            If Common.GetVariable("UpdatedSamplePallet", Page) = "True" Then
                Me.btComplete.Visible = True
                Me.btComplete.Text = "Finish"
            Else
                Me.btComplete.Visible = False
            End If

        Catch ex As Exception
            Me.lbError.Text = ex.ToString
            Me.lbError.Visible = True
        End Try
    End Sub
#End Region

#Region "Table Functions"
    'Delete IC_Pallets_Record
    Public Function DeleteProdFromSamplePallet(ByVal pallet As Long, ByVal gtin As String, ByVal lotno As String) As Boolean

        DeleteProdFromSamplePallet = False
        Dim dtmScanned As Date = Now()
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString
        Me.lbError.Text = ""

        Dim strSQLDeleteSampleProd As String = "Delete From IC_Pallets Where pk_nPallet = " & pallet & " And GTIN = '" & gtin & "' And Lot = '" & lotno & "'"
        Dim strHistoryDeleteProductInfo As String = "Product " & gtin & "with Lot " & lotno & "was deleted from Sample Pallet " & pallet

        Dim strSQLHistoryDeleteSampleProd As String = ""
        strSQLHistoryDeleteSampleProd = "Insert Into IC_PalletsHistory (pk_nPallet,txDateTimeEntered,Process,UserID,Info)" & _
            "VALUES (" & pallet & ",'" & dtmScanned & "','SamplePallet','" & strUnit & "','" & strHistoryDeleteProductInfo & "')"

        Try
            'Delete IC_Pallets record
            Dim sqlCmdDeleteSampleProd As New System.Data.SqlClient.SqlCommand
            Dim sqlCmdHistoryDeleteSampleProd As New System.Data.SqlClient.SqlCommand

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdDeleteSampleProd = DB.NewSQLCommand3(strSQLDeleteSampleProd)
                sqlCmdHistoryDeleteSampleProd = DB.NewSQLCommand3(strSQLHistoryDeleteSampleProd)

                If Not sqlCmdDeleteSampleProd Is Nothing Then
                    sqlCmdDeleteSampleProd.ExecuteNonQuery()
                    sqlCmdDeleteSampleProd.Dispose() : sqlCmdDeleteSampleProd = Nothing
                    If Not sqlCmdHistoryDeleteSampleProd Is Nothing Then
                        sqlCmdHistoryDeleteSampleProd.ExecuteNonQuery()
                        sqlCmdHistoryDeleteSampleProd.Dispose() : sqlCmdHistoryDeleteSampleProd = Nothing
                    Else
                        Me.lbError.Text = "Command Error occurred while writing history for deleting product from pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                        Me.lbError.Visible = True
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while deleting product from pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                    Me.lbError.Visible = True
                End If
                If Not sqlCmdDeleteSampleProd Is Nothing Then
                    sqlCmdDeleteSampleProd.Dispose() : sqlCmdDeleteSampleProd = Nothing
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while deleting product from pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                Me.lbError.Visible = True
            End If
            If Me.lbError.Text = "" Then
                DeleteProdFromSamplePallet = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while deleting product from pallet! Check Wireless Connection and Battery then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function UpdateSampleProdQty(ByVal pallet As Long, ByVal gtin As String, ByVal lotno As String, ByVal caseqty As Integer) As Boolean
        'Update IC_Pallets_Record
        UpdateSampleProdQty = False
        Dim dtmScanned As Date = Now()
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString
        Me.lbError.Text = ""
        Dim strSQLUpdateSampleProdQty As String = "Update IC_Pallets Set PalletQty = " & caseqty & ", txDatetimeModified = '" & dtmScanned & "' Where pk_nPallet = " & pallet & " And GTIN = '" & gtin & "' And Lot = '" & lotno & "'"
        Dim strHistoryUpdateProductInfo As String = "Product " & gtin & "with Lot " & lotno & " on Pallet " & pallet & " - Quantity changed to " & caseqty

        Dim strSQLHistoryUpdateSampleProdQty As String = ""
        strSQLHistoryUpdateSampleProdQty = "Insert Into IC_PalletsHistory (pk_nPallet,txDateTimeEntered,Process,UserID,Info)" & _
            "VALUES (" & pallet & ",'" & dtmScanned & "','SamplePallet','" & strUnit & "','" & strHistoryUpdateProductInfo & "')"
        Try
            'Update IC_Pallets record and write IC_History record
            Dim sqlCmdUpdateSampleProdQty As New System.Data.SqlClient.SqlCommand
            Dim sqlCmdHistoryUpdateSampleProdQty As New System.Data.SqlClient.SqlCommand
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdUpdateSampleProdQty = DB.NewSQLCommand3(strSQLUpdateSampleProdQty)
                sqlCmdHistoryUpdateSampleProdQty = DB.NewSQLCommand3(strSQLHistoryUpdateSampleProdQty)
                If Not sqlCmdUpdateSampleProdQty Is Nothing Then
                    sqlCmdUpdateSampleProdQty.ExecuteNonQuery()
                    sqlCmdUpdateSampleProdQty.Dispose() : sqlCmdUpdateSampleProdQty = Nothing
                    If Not sqlCmdHistoryUpdateSampleProdQty Is Nothing Then
                        sqlCmdHistoryUpdateSampleProdQty.ExecuteNonQuery()
                        sqlCmdHistoryUpdateSampleProdQty.Dispose() : sqlCmdHistoryUpdateSampleProdQty = Nothing
                        UpdateSampleProdQty = True
                    Else
                        Me.lbError.Text = "Command Error occurred while writing history for updating product qty on sample pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                        Me.lbError.Visible = True
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while updating product qty on sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
                    Me.lbError.Visible = True
                End If
                If Not sqlCmdUpdateSampleProdQty Is Nothing Then
                    sqlCmdUpdateSampleProdQty.Dispose() : sqlCmdUpdateSampleProdQty = Nothing
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while updating product qty on sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
                Me.lbError.Visible = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while updating product qty on sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Function
    Public Function AddProdToSamplePallet(ByVal pallet As Long, ByVal gtin As String, ByVal codedate As String, ByVal palletqty As Integer, _
                                                ByVal mfgord As Integer, ByVal orderid As Long, ByVal prodvariant As String, ByVal lotno As String) As Boolean

        AddProdToSamplePallet = False
        Dim dtmScanned As Date = Now()
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString
        Me.lbError.Text = ""
        Dim strSQLInsertIC_Pallets As String = "INSERT Into IC_Pallets (pk_nPallet,GTIN,CodeDate,Company,CompanyID,PalletQty,txDateTimeEntered,txDateTimeModified,MfgOrd,OrderID,OrderLine,ProdVariant,Lot) " & _
        "VALUES (" & pallet & ",'" & gtin & "','" & codedate & "','2',2," & palletqty & ",'" & dtmScanned & "','" & dtmScanned & "','" & mfgord & "'," & orderid & ",0,'" & prodvariant & "','" & lotno & "')"

        Dim strHistoryAddedProductInfo As String = "Product " & gtin & "with Lot " & lotno & " was added to Pallet " & pallet & " - with quantity - " & palletqty

        Dim strSQLHistoryInsertIC_Pallets As String = ""
        strSQLHistoryInsertIC_Pallets = "Insert Into IC_PalletsHistory (pk_nPallet,txDateTimeEntered,Process,UserID,Info)" & _
            "VALUES (" & pallet & ",'" & dtmScanned & "','SamplePallet','" & strUnit & "','" & strHistoryAddedProductInfo & "')"
        Try
            'Insert IC_Pallets record
            Dim sqlCmdInsertIC_Pallets As New System.Data.SqlClient.SqlCommand
            Dim sqlCmdHistoryIC_Pallets As New System.Data.SqlClient.SqlCommand
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdInsertIC_Pallets = DB.NewSQLCommand3(strSQLInsertIC_Pallets)
                sqlCmdHistoryIC_Pallets = DB.NewSQLCommand3(strSQLHistoryInsertIC_Pallets)
                If Not sqlCmdInsertIC_Pallets Is Nothing Then
                    sqlCmdInsertIC_Pallets.ExecuteNonQuery()
                    sqlCmdInsertIC_Pallets.Dispose() : sqlCmdInsertIC_Pallets = Nothing
                    If Not sqlCmdHistoryIC_Pallets Is Nothing Then
                        sqlCmdHistoryIC_Pallets.ExecuteNonQuery()
                        sqlCmdHistoryIC_Pallets.Dispose() : sqlCmdHistoryIC_Pallets = Nothing
                    Else
                        Me.lbError.Text = "Command Error occurred while writing history for added product on sample pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                        Me.lbError.Visible = True
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while saving sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
                    Me.lbError.Visible = True
                End If
                If Not sqlCmdInsertIC_Pallets Is Nothing Then
                    sqlCmdInsertIC_Pallets.Dispose() : sqlCmdInsertIC_Pallets = Nothing
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while saving sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
                Me.lbError.Visible = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while saving sample pallet in database! Check Wireless Connection and Battery then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Function
#End Region

#Region "User Entry"

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        Dim strPallet As String = Me.lbShipPalletValue.Text

        Try
            Me.lbError.Text = ""
            'Validate previous field entries
            Call ValidateEntries()
            If Me.lbError.Text.Length = 0 Then
                Dim nPallet As Long = CLng(strPallet)
                Dim nOrder As Long = CLng(Me.htxOrder.Value)
                Dim strPartCode As String = Mid(Me.htxCaseLabel.Value, 3, 14)
                Dim strCDate As String = Mid(Me.htxCaseLabel.Value, 19, 6)
                Dim strLotNum As String = Mid(Me.htxCaseLabel.Value, 27, 12)
                Dim strVersion As String = Mid(Me.htxCaseLabel.Value, 42, 3)
                Dim prodqty As Integer = 0

                If _screenmode = "ChgQty" Then 'process screen for update to prod qty on Sample pallet
                    If IsNumeric(Me.txQuantity.Text) = True Then
                        prodqty = CInt(Me.txQuantity.Text)

                        'Update sample product record
                        If UpdateSampleProdQty(nPallet, strPartCode, Mid(strLotNum, 8, 5), prodqty) = False Then
                            'Error occurred while deleting current product from sample pallet - set Mode to quantity to try again
                            Mode = ScanModes.QuantityMode
                            Me.lbError.Text = "Error occurred while updating product quantity on Sample Pallet - " & nPallet & ". Check Wireless Connection and then try entering case quantity again."
                            Me.lbError.Visible = True
                        Else
                            Me.LoadExistingPalletGrid()
                            Mode = ScanModes.CaseLabelMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Common.SaveVariable("UpdatedSamplePallet", "True", Page)
                        End If
                        ResetControls()
                    Else
                        Me.lbError.Text = "Case Quantity entered needs to be numeric!"
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txQuantity)
                    End If
                ElseIf _screenmode = "AddTo" Then 'process screen for added prod on Sample pallet
                    If IsNumeric(Me.txQuantity.Text) = True Then
                        prodqty = CInt(Me.txQuantity.Text)

                        'Update sample product record
                        If AddProdToSamplePallet(nPallet, strPartCode, strCDate, prodqty, Mid(strLotNum, 3, 5), nOrder, strVersion, Mid(strLotNum, 8, 5)) = False Then
                            'Error occurred while adding product to sample pallet - set Mode to quantity to try again
                            Mode = ScanModes.QuantityMode
                            Me.lbError.Text = "Error occurred while adding product to sample pallet - " & nPallet & ". Check Wireless Connection and then try entering case quantity again."
                            Me.lbError.Visible = True
                        Else
                            Me.LoadExistingPalletGrid()
                            Mode = ScanModes.CaseLabelMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Common.SaveVariable("UpdatedSamplePallet", "True", Page)
                        End If
                        ResetControls()
                    Else
                        Me.lbError.Text = "Case Quantity entered needs to be numeric!"
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txQuantity)
                    End If
                ElseIf _screenmode = "Remove" Then 'If me.txQuantity.text = y then delete record from IC_Pallets if one exists for product scanned Else return to screen and reset for CaseLabel Entry
                    If Me.lbError.Text.Length = 0 Then
                        If UCase(RTrim(Me.txQuantity.Text)) = "Y" Then
                            'Delete Product from Pallet
                            If DeleteProdFromSamplePallet(nPallet, strPartCode, Mid(strLotNum, 8, 5)) = False Then
                                'Error occurred while deleting current product from sample pallet - set Mode to quantity to try again
                                Mode = ScanModes.QuantityMode
                                Me.lbError.Text = "Error occurred while Deleting Product from Sample Pallet - " & nPallet & ". Check Wireless Connection and then try entering case quantity again."
                                Me.lbError.Visible = True
                            Else
                                Me.LoadExistingPalletGrid()
                                Mode = ScanModes.CaseLabelMode
                                Common.SaveVariable("8SavedMode", Mode, Page)
                                Common.SaveVariable("UpdatedSamplePallet", "True", Page)
                            End If
                            ResetControls()

                        ElseIf UCase(RTrim(Me.txQuantity.Text)) = "N" Then
                            Mode = ScanModes.OrderMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Common.SaveVariable("UpdatedSamplePallet", "False", Page)
                            ResetControls()
                        Else
                            Me.lbError.Text = "Entry must be Y or N!"
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txQuantity)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during validation of Quantity entry! Press Restart button and reprocess the Pallet."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txQuantity)
        Finally
            DB.KillSQLConnection()
        End Try

    End Sub

    Protected Sub txBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txBin.TextChanged
        'Verify the Bin Location entered is valid in database
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Me.txBin.Text = UCase(Me.txBin.Text)
        Dim sqlCmdTruck As New System.Data.SqlClient.SqlCommand
        Dim sTruck As Integer = 0

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If Me.txBin.Text.Length > 0 Then
                If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to database failed. Restart screen and try again.""")
                sqlCmdBin = DB.NewSQLCommand("SQL.Query.BinLookup")
                If sqlCmdBin Is Nothing Then Throw New Exception("""Bin Lookup Command failed. Restart screen and try again.""")
                sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txBin.Text)
                _tobin = sqlCmdBin.ExecuteScalar()
                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                DB.KillSQLConnection()

                If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
                    Me.lbError.Text = "Invalid Bin Location entered. Try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txBin)
                Else 'Bin Location is valid
                    'Get Printer #
                    Mode = ScanModes.PrinterMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                End If
            Else
                Me.lbError.Text = "Bin Location entry is required."
                Me.lbError.Visible = True
                Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txBin)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txBin)
        Finally
            If Not sqlCmdBin Is Nothing Then
                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        'Init the saved values used when printing pallet tag and then Shipto Tag
        Common.SaveVariable("HR_PalletID", "", Page)
        Common.SaveVariable("HR_ShipTo", "", Page)
        Common.SaveVariable("HR_ShipToName", "", Page)
        Common.SaveVariable("HR_Location", "", Page)
        Common.SaveVariable("HR_ShipDate", "", Page)
        Common.SaveVariable("HR_OrderID", "", Page)
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify printer # entered is for correct location and is numeric
            If IsNumeric(Me.txPrinter.Text) = True Then
                Dim iPrt As Integer = CInt(Me.txPrinter.Text)
                Dim strPallet As String = ""
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
                            Me.lbError.Visible = True
                            'Save the bad Printer to display on screen for correction
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                        Else 'Printer is valid
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            
                            'Write file to Loftware server to print out the Pallet Tag
                            If PrintSamplePalletTag() = True Then
                                Common.SaveVariable("PrintSecondTime", "N", Page)
                                Me.dgExistingProducts.SelectedIndex = -1
                                Common.SaveVariable("UpdatedSamplePallet", "True", Page)
                                btReturn_Click(sender, e)
                            Else
                                Common.SaveVariable("PrintSecondTime", "Y", Page)
                                Me.lbError.Text = "Shipping Pallet Tag failed to print. Try printing again."
                                Mode = ScanModes.PrinterMode
                                Common.SaveVariable("8SavedMode", Mode, Page)
                                ResetControls()
                            End If
                        End If
                    End If
                Else
                    Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                    Me.lbError.Text = "Communication Error occurred while validating Printer - try again or see your supervisor!"
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                End If
            Else
                Me.lbError.Text = "Printer # needs to be single digit numeric entry!"
                Me.lbError.Visible = True
                Common.SaveVariable("Printer", "", Page)
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
            End If
        Catch ex As Exception
            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Printer! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
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
        Me.LoadExistingPalletGrid()
        Mode = ScanModes.OrderMode
        Common.SaveVariable("8SavedMode", Mode, Page)
        ResetControls()
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to Work with Shipping Pallet Menu
        Common.SaveVariable("ScreenParam", Nothing, Page)
        Common.SaveVariable("newURL", "~/IC_WorkWithSamplePallet.aspx", Page)
    End Sub

    Protected Sub btComplete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btComplete.Click
        'Prompt for Bin Location
        Mode = ScanModes.BinMode
        Common.SaveVariable("8SavedMode", Mode, Page)
        ResetControls()
    End Sub
#End Region

#Region "Custom Processes"

    Public Sub ValidateEntries()
        'Validate Order Length
        Me.lbError.Text = ""
        Try
            If Me.htxOrder.Value.Length <> CInt(ConfigurationManager.AppSettings.Get("UI.Order.Length").ToString) Then
                Me.lbError.Text = "Value entered for Pick Order is wrong length - start process again!"
                Me.lbError.Visible = True
            Else
                'Length is correct check to see if it matches existing pallet
                If CLng(Me.htxOrder.Value) <> CLng(Common.GetVariable("EditPalletOrderID", Page).ToString) Then
                    Me.lbError.Text = "Value entered for Pick Order does not match OrderID on the Existing Sample Pallet - start process again!"
                    Me.lbError.Visible = True
                End If
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

        If _screenmode = "ChgQty" Or _screenmode = "Remove" Then
            Try
                Dim strPartCode As String = Mid(Me.htxCaseLabel.Value, 3, 14)
                Dim iQty As Integer = 0
                'Validate that the CaseLabel scanned matches an existing product on the Sample Pallet.
                If Not DB.MakeSQLConnection("Warehouse") Then
                    Dim sqlCmdSamplePallet As New System.Data.SqlClient.SqlCommand
                    sqlCmdSamplePallet = DB.NewSQLCommand("SQL.Query.GetSamplePalletGTIN")
                    If Not sqlCmdSamplePallet Is Nothing Then
                        sqlCmdSamplePallet.Parameters.AddWithValue("@PalletID", Me.lbShipPalletValue.Text)
                        sqlCmdSamplePallet.Parameters.AddWithValue("@GTIN", strPartCode)
                        iQty = sqlCmdSamplePallet.ExecuteScalar()
                        sqlCmdSamplePallet.Dispose() : sqlCmdSamplePallet = Nothing
                        DB.KillSQLConnection()
                        If IsDBNull(iQty) = True Then
                            Me.lbError.Text = "Product not found on Sample Pallet - Check Connection, then Try again!"
                            Me.lbError.Visible = True
                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while validating Sample Pallet Products - Check Connection, then Try again!"
                        Me.lbError.Visible = True
                    End If
                Else

                    Me.lbError.Text = "Connection Error occurred while validating Sample Pallet Products - Check Connection, then Try again!"
                    Me.lbError.Visible = True
                End If
            Catch ex As Exception
                Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Sample Pallet Products! Check Battery and Wireless Connection then Restart Pallet"
            End Try

            If Me.lbError.Text.Length > 0 Then
                Mode = ScanModes.CaseLabelMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
                Exit Sub
            End If
        End If

    End Sub

    Public Sub LoadExistingPalletGrid()
        Dim sqlString As String
        Dim dvExistingPalletGrd As New DataView
        Dim dsExistingPalletGrd As New Data.DataSet
        Dim sqlCmdExistingPalletGrd As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text
            sqlString = "Select ProductID,ProdVariant as Version,CaseQty,ProdDesc,Lot from vwIC_ExistingPalletGridView Where PalletID = " & CLng(strPallet)
            'Get Products on SamplePallet
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdExistingPalletGrd = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdExistingPalletGrd Is Nothing Then
                    dsExistingPalletGrd = DB.GetDataSet(sqlCmdExistingPalletGrd)
                    sqlCmdExistingPalletGrd.Dispose() : sqlCmdExistingPalletGrd = Nothing
                    DB.KillSQLConnection()
                    If dsExistingPalletGrd Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while displaying existing products on current pallet! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Else
                        dvExistingPalletGrd = New DataView(dsExistingPalletGrd.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
                        dgExistingProducts.DataSource = dvExistingPalletGrd
                        dgExistingProducts.DataBind()

                        If dvExistingPalletGrd.Count > 0 Then
                            Me.lbExistingGridTitle.Visible = True
                            dgExistingProducts.Visible = True
                        Else
                            Me.lbExistingGridTitle.Visible = False
                            dgExistingProducts.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while displaying existing products on current pallet! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
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
            'Used for getting ShipTo Info and Shipping Pallet Details From vwIC_OW_ShippingPallets
            Dim dsSamplePallet As New Data.DataSet
            Dim sqlCmdSamplePallet As New System.Data.SqlClient.SqlCommand

            'Get Shipto Info for SamplePallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdSamplePallet = DB.NewSQLCommand("SQL.Query.GetSamplePallet")
                If Not sqlCmdSamplePallet Is Nothing Then
                    _pallet = CLng(Me.lbShipPalletValue.Text)
                    sqlCmdSamplePallet.Parameters.AddWithValue("@PalletID", _pallet)
                    dsSamplePallet = DB.GetDataSet(sqlCmdSamplePallet)
                    sqlCmdSamplePallet.Dispose() : sqlCmdSamplePallet = Nothing
                    DB.KillSQLConnection()
                    If dsSamplePallet Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while getting Shipping Pallet Information from database! Check battery and wireless connection - then try again."
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
                            'Added CBCShipDate to the Pallet Tag - SAM - 3/7/2012
                            Dim strLine7_1 As String = "HR_CB_ShipDate," & dsSamplePallet.Tables(0).Rows(0).Item("CBCShipDate").ToString

                            Dim strLine8 As String = "HR_OrderID," & Me.htxOrder.Value.ToString
                            'Initialize detail placeholders to blank
                            Dim strLine9 As String = "HR_ProductID_01," & ""
                            Dim strLine10 As String = "HR_CaseQty_01," & ""
                            Dim strLine11 As String = "HR_ProdDesc_01," & ""
                            Dim strLine11_1 As String = "HR_ProdCDate_01," & ""
                            Dim strLine12 As String = "HR_ProductID_02," & ""
                            Dim strLine13 As String = "HR_CaseQty_02," & ""
                            Dim strLine14 As String = "HR_ProdDesc_02," & ""
                            Dim strLine14_1 As String = "HR_ProdCDate_02," & ""
                            Dim strLine15 As String = "HR_ProductID_03," & ""
                            Dim strLine16 As String = "HR_CaseQty_03," & ""
                            Dim strLine17 As String = "HR_ProdDesc_03," & ""
                            Dim strLine17_1 As String = "HR_ProdCDate_03," & ""
                            Dim strLine18 As String = "HR_ProductID_04," & ""
                            Dim strLine19 As String = "HR_CaseQty_04," & ""
                            Dim strLine20 As String = "HR_ProdDesc_04," & ""
                            Dim strLine20_1 As String = "HR_ProdCDate_04," & ""
                            Dim strLine21 As String = "HR_ProductID_05," & ""
                            Dim strLine22 As String = "HR_CaseQty_05," & ""
                            Dim strLine23 As String = "HR_ProdDesc_05," & ""
                            Dim strLine23_1 As String = "HR_ProdCDate_05," & ""
                            Dim strLine24 As String = "HR_ProductID_06," & ""
                            Dim strLine25 As String = "HR_CaseQty_06," & ""
                            Dim strLine26 As String = "HR_ProdDesc_06," & ""
                            Dim strLine26_1 As String = "HR_ProdCDate_06," & ""
                            Dim strLine27 As String = "HR_ProductID_07," & ""
                            Dim strLine28 As String = "HR_CaseQty_07," & ""
                            Dim strLine29 As String = "HR_ProdDesc_07," & ""
                            Dim strLine29_1 As String = "HR_ProdCDate_07," & ""
                            Dim strLine30 As String = "HR_ProductID_08," & ""
                            Dim strLine31 As String = "HR_CaseQty_08," & ""
                            Dim strLine32 As String = "HR_ProdDesc_08," & ""
                            Dim strLine32_1 As String = "HR_ProdCDate_08," & ""
                            Dim strLine33 As String = "HR_ProductID_09," & ""
                            Dim strLine34 As String = "HR_CaseQty_09," & ""
                            Dim strLine35 As String = "HR_ProdDesc_09," & ""
                            Dim strLine35_1 As String = "HR_ProdCDate_09," & ""
                            Dim strLine36 As String = "HR_ProductID_10," & ""
                            Dim strLine37 As String = "HR_CaseQty_10," & ""
                            Dim strLine38 As String = "HR_ProdDesc_10," & ""
                            Dim strLine38_1 As String = "HR_ProdCDate_10," & ""
                            Dim strLine39 As String = "HR_ProductID_11," & ""
                            Dim strLine40 As String = "HR_CaseQty_11," & ""
                            Dim strLine41 As String = "HR_ProdDesc_11," & ""
                            Dim strLine41_1 As String = "HR_ProdCDate_11," & ""
                            Dim strLine42 As String = "HR_ProductID_12," & ""
                            Dim strLine43 As String = "HR_CaseQty_12," & ""
                            Dim strLine44 As String = "HR_ProdDesc_12," & ""
                            Dim strLine44_1 As String = "HR_ProdCDate_12," & ""
                            Dim strLine45 As String = "HR_ProductID_13," & ""
                            Dim strLine46 As String = "HR_CaseQty_13," & ""
                            Dim strLine47 As String = "HR_ProdDesc_13," & ""
                            Dim strLine47_1 As String = "HR_ProdCDate_13," & ""
                            Dim strLine48 As String = "HR_ProductID_14," & ""
                            Dim strLine49 As String = "HR_CaseQty_14," & ""
                            Dim strLine50 As String = "HR_ProdDesc_14," & ""
                            Dim strLine50_1 As String = "HR_ProdCDate_14," & ""
                            Dim strLine51 As String = "HR_ProductID_15," & ""
                            Dim strLine52 As String = "HR_CaseQty_15," & ""
                            Dim strLine53 As String = "HR_ProdDesc_15," & ""
                            Dim strLine53_1 As String = "HR_ProdCDate_15," & ""
                            Dim strLine54 As String = "HR_ProductID_16," & ""
                            Dim strLine55 As String = "HR_CaseQty_16," & ""
                            Dim strLine56 As String = "HR_ProdDesc_16," & ""
                            Dim strLine56_1 As String = "HR_ProdCDate_16," & ""
                            Dim strLine57 As String = "HR_ProductID_17," & ""
                            Dim strLine58 As String = "HR_CaseQty_17," & ""
                            Dim strLine59 As String = "HR_ProdDesc_17," & ""
                            Dim strLine59_1 As String = "HR_ProdCDate_17," & ""
                            Dim strLine60 As String = "HR_ProductID_18," & ""
                            Dim strLine61 As String = "HR_CaseQty_18," & ""
                            Dim strLine62 As String = "HR_ProdDesc_18," & ""
                            Dim strLine62_1 As String = "HR_ProdCDate_18," & ""
                            Dim strLine63 As String = "HR_ProductID_19," & ""
                            Dim strLine64 As String = "HR_CaseQty_19," & ""
                            Dim strLine65 As String = "HR_ProdDesc_19," & ""
                            Dim strLine65_1 As String = "HR_ProdCDate_19," & ""
                            Dim strLine66 As String = "HR_ProductID_20," & ""
                            Dim strLine67 As String = "HR_CaseQty_20," & ""
                            Dim strLine68 As String = "HR_ProdDesc_20," & ""
                            Dim strLine68_1 As String = "HR_ProdCDate_20," & ""
                            Dim strLine69 As String = "HR_ProductID_21," & ""
                            Dim strLine70 As String = "HR_CaseQty_21," & ""
                            Dim strLine71 As String = "HR_ProdDesc_21," & ""
                            Dim strLine71_1 As String = "HR_ProdCDate_21," & ""
                            Dim strLine72 As String = "HR_ProductID_22," & ""
                            Dim strLine73 As String = "HR_CaseQty_22," & ""
                            Dim strLine74 As String = "HR_ProdDesc_22," & ""
                            Dim strLine74_1 As String = "HR_ProdCDate_22," & ""
                            Dim strLine75 As String = "HR_ProductID_23," & ""
                            Dim strLine76 As String = "HR_CaseQty_23," & ""
                            Dim strLine77 As String = "HR_ProdDesc_23," & ""
                            Dim strLine77_1 As String = "HR_ProdCDate_23," & ""
                            Dim strLine78 As String = "HR_ProductID_24," & ""
                            Dim strLine79 As String = "HR_CaseQty_24," & ""
                            Dim strLine80 As String = "HR_ProdDesc_24," & ""
                            Dim strLine80_1 As String = "HR_ProdCDate_24," & ""
                            Dim strLine81 As String = "HR_ProductID_25," & ""
                            Dim strLine82 As String = "HR_CaseQty_25," & ""
                            Dim strLine83 As String = "HR_ProdDesc_25," & ""
                            Dim strLine83_1 As String = "HR_ProdCDate_25," & ""
                            Dim strLine84 As String = "HR_ProductID_26," & ""
                            Dim strLine85 As String = "HR_CaseQty_26," & ""
                            Dim strLine86 As String = "HR_ProdDesc_26," & ""
                            Dim strLine86_1 As String = "HR_ProdCDate_26," & ""
                            Dim strLine87 As String = "HR_ProductID_27," & ""
                            Dim strLine88 As String = "HR_CaseQty_27," & ""
                            Dim strLine89 As String = "HR_ProdDesc_27," & ""
                            Dim strLine89_1 As String = "HR_ProdCDate_27," & ""
                            Dim strLine90 As String = "HR_ProductID_28," & ""
                            Dim strLine91 As String = "HR_CaseQty_28," & ""
                            Dim strLine92 As String = "HR_ProdDesc_28," & ""
                            Dim strLine92_1 As String = "HR_ProdCDate_28," & ""
                            Dim strLine93 As String = "HR_ProductID_29," & ""
                            Dim strLine94 As String = "HR_CaseQty_29," & ""
                            Dim strLine95 As String = "HR_ProdDesc_29," & ""
                            Dim strLine95_1 As String = "HR_ProdCDate_29," & ""
                            Dim strLine96 As String = "HR_ProductID_30," & ""
                            Dim strLine97 As String = "HR_CaseQty_30," & ""
                            Dim strLine98 As String = "HR_ProdDesc_30," & ""
                            Dim strLine98_0 As String = "HR_ProdCDate_30," & ""
                            Dim strLine98_1 As String = "HR_TotalCases," & ""
                            Dim strLine98_2 As String = "HR_GrossWeight," & ""

                            For x = 0 To (dsSamplePallet.Tables(0).Rows.Count - 1)
                                Select Case x
                                    Case 0 '
                                        strLine9 = "HR_ProductID_01," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine10 = "HR_CaseQty_01," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine11 = "HR_ProdDesc_01," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine11_1 = "HR_ProdCDate_01," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 1
                                        strLine12 = "HR_ProductID_02," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine13 = "HR_CaseQty_02," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine14 = "HR_ProdDesc_02," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine14_1 = "HR_ProdCDate_02," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 2
                                        strLine15 = "HR_ProductID_03," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine16 = "HR_CaseQty_03," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine17 = "HR_ProdDesc_03," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine17_1 = "HR_ProdCDate_03," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 3
                                        strLine18 = "HR_ProductID_04," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine19 = "HR_CaseQty_04," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine20 = "HR_ProdDesc_04," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine20_1 = "HR_ProdCDate_04," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 4
                                        strLine21 = "HR_ProductID_05," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine22 = "HR_CaseQty_05," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine23 = "HR_ProdDesc_05," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine23_1 = "HR_ProdCDate_05," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 5
                                        strLine24 = "HR_ProductID_06," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine25 = "HR_CaseQty_06," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine26 = "HR_ProdDesc_06," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine26_1 = "HR_ProdCDate_06," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 6
                                        strLine27 = "HR_ProductID_07," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine28 = "HR_CaseQty_07," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine29 = "HR_ProdDesc_07," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine29_1 = "HR_ProdCDate_07," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 7
                                        strLine30 = "HR_ProductID_08," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine31 = "HR_CaseQty_08," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine32 = "HR_ProdDesc_08," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine32_1 = "HR_ProdCDate_08," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 8
                                        strLine33 = "HR_ProductID_09," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine34 = "HR_CaseQty_09," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine35 = "HR_ProdDesc_09," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine35_1 = "HR_ProdCDate_09," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 9
                                        strLine36 = "HR_ProductID_10," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine37 = "HR_CaseQty_10," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine38 = "HR_ProdDesc_10," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine38_1 = "HR_ProdCDate_10," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 10
                                        strLine39 = "HR_ProductID_11," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine40 = "HR_CaseQty_11," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine41 = "HR_ProdDesc_11," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine41_1 = "HR_ProdCDate_11," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 11
                                        strLine42 = "HR_ProductID_12," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine43 = "HR_CaseQty_12," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine44 = "HR_ProdDesc_12," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine44_1 = "HR_ProdCDate_12," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 12
                                        strLine45 = "HR_ProductID_13," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine46 = "HR_CaseQty_13," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine47 = "HR_ProdDesc_13," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine47_1 = "HR_ProdCDate_13," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 13
                                        strLine48 = "HR_ProductID_14," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine49 = "HR_CaseQty_14," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine50 = "HR_ProdDesc_14," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine50_1 = "HR_ProdCDate_14," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 14
                                        strLine51 = "HR_ProductID_15," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine52 = "HR_CaseQty_15," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine53 = "HR_ProdDesc_15," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine53_1 = "HR_ProdCDate_15," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 15
                                        strLine54 = "HR_ProductID_16," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine55 = "HR_CaseQty_16," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine56 = "HR_ProdDesc_16," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine56_1 = "HR_ProdCDate_16," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 16
                                        strLine57 = "HR_ProductID_17," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine58 = "HR_CaseQty_17," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine59 = "HR_ProdDesc_17," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine59_1 = "HR_ProdCDate_17," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 17
                                        strLine60 = "HR_ProductID_18," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine61 = "HR_CaseQty_18," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine62 = "HR_ProdDesc_18," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine62_1 = "HR_ProdCDate_18," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 18
                                        strLine63 = "HR_ProductID_19," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine64 = "HR_CaseQty_19," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine65 = "HR_ProdDesc_19," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine65_1 = "HR_ProdCDate_19," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 19
                                        strLine66 = "HR_ProductID_20," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine67 = "HR_CaseQty_20," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine68 = "HR_ProdDesc_20," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine68_1 = "HR_ProdCDate_20," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 20
                                        strLine69 = "HR_ProductID_21," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine70 = "HR_CaseQty_21," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine71 = "HR_ProdDesc_21," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine71_1 = "HR_ProdCDate_21," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 21
                                        strLine72 = "HR_ProductID_22," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine73 = "HR_CaseQty_22," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine74 = "HR_ProdDesc_22," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine74_1 = "HR_ProdCDate_22," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 22
                                        strLine75 = "HR_ProductID_23," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine76 = "HR_CaseQty_23," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine77 = "HR_ProdDesc_23," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine77_1 = "HR_ProdCDate_23," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 23
                                        strLine78 = "HR_ProductID_24," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine79 = "HR_CaseQty_24," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine80 = "HR_ProdDesc_24," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine80_1 = "HR_ProdCDate_24," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 24
                                        strLine81 = "HR_ProductID_25," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine82 = "HR_CaseQty_25," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine83 = "HR_ProdDesc_25," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine83_1 = "HR_ProdCDate_25," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 25
                                        strLine84 = "HR_ProductID_26," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine85 = "HR_CaseQty_26," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine86 = "HR_ProdDesc_26," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine86_1 = "HR_ProdCDate_26," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 26
                                        strLine87 = "HR_ProductID_27," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine88 = "HR_CaseQty_27," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine89 = "HR_ProdDesc_27," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine89_1 = "HR_ProdCDate_27," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 27
                                        strLine90 = "HR_ProductID_28," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine91 = "HR_CaseQty_28," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine92 = "HR_ProdDesc_28," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine92_1 = "HR_ProdCDate_28," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 28
                                        strLine93 = "HR_ProductID_29," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine94 = "HR_CaseQty_29," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine95 = "HR_ProdDesc_29," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine95_1 = "HR_ProdCDate_29," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsSamplePallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 29
                                        strLine96 = "HR_ProductID_30," & dsSamplePallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsSamplePallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine97 = "HR_CaseQty_30," & dsSamplePallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine98 = "HR_ProdDesc_30," & dsSamplePallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine98_0 = "HR_ProdCDate_30," & dsSamplePallet.Tables(0).Rows(x).Item("CodeDate").ToString
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
                            'Added CBCShipDate to the Pallet Tag - SAM - 3/7/2012
                            s.WriteLine(strLine7_1)

                            s.WriteLine(strLine8)
                            s.WriteLine(strLine9)
                            s.WriteLine(strLine10)
                            s.WriteLine(strLine11)
                            s.WriteLine(strLine11_1)
                            s.WriteLine(strLine12)
                            s.WriteLine(strLine13)
                            s.WriteLine(strLine14)
                            s.WriteLine(strLine14_1)
                            s.WriteLine(strLine15)
                            s.WriteLine(strLine16)
                            s.WriteLine(strLine17)
                            s.WriteLine(strLine17_1)
                            s.WriteLine(strLine18)
                            s.WriteLine(strLine19)
                            s.WriteLine(strLine20)
                            s.WriteLine(strLine20_1)
                            s.WriteLine(strLine21)
                            s.WriteLine(strLine22)
                            s.WriteLine(strLine23)
                            s.WriteLine(strLine23_1)
                            s.WriteLine(strLine24)
                            s.WriteLine(strLine25)
                            s.WriteLine(strLine26)
                            s.WriteLine(strLine26_1)
                            s.WriteLine(strLine27)
                            s.WriteLine(strLine28)
                            s.WriteLine(strLine29)
                            s.WriteLine(strLine29_1)
                            s.WriteLine(strLine30)
                            s.WriteLine(strLine31)
                            s.WriteLine(strLine32)
                            s.WriteLine(strLine32_1)
                            s.WriteLine(strLine33)
                            s.WriteLine(strLine34)
                            s.WriteLine(strLine35)
                            s.WriteLine(strLine35_1)
                            s.WriteLine(strLine36)
                            s.WriteLine(strLine37)
                            s.WriteLine(strLine38)
                            s.WriteLine(strLine38_1)
                            s.WriteLine(strLine39)
                            s.WriteLine(strLine40)
                            s.WriteLine(strLine41)
                            s.WriteLine(strLine41_1)
                            s.WriteLine(strLine42)
                            s.WriteLine(strLine43)
                            s.WriteLine(strLine44)
                            s.WriteLine(strLine44_1)
                            s.WriteLine(strLine45)
                            s.WriteLine(strLine46)
                            s.WriteLine(strLine47)
                            s.WriteLine(strLine47_1)
                            s.WriteLine(strLine48)
                            s.WriteLine(strLine49)
                            s.WriteLine(strLine50)
                            s.WriteLine(strLine50_1)
                            s.WriteLine(strLine51)
                            s.WriteLine(strLine52)
                            s.WriteLine(strLine53)
                            s.WriteLine(strLine53_1)
                            s.WriteLine(strLine54)
                            s.WriteLine(strLine55)
                            s.WriteLine(strLine56)
                            s.WriteLine(strLine56_1)
                            s.WriteLine(strLine57)
                            s.WriteLine(strLine58)
                            s.WriteLine(strLine59)
                            s.WriteLine(strLine59_1)
                            s.WriteLine(strLine60)
                            s.WriteLine(strLine61)
                            s.WriteLine(strLine62)
                            s.WriteLine(strLine62_1)
                            s.WriteLine(strLine63)
                            s.WriteLine(strLine64)
                            s.WriteLine(strLine65)
                            s.WriteLine(strLine65_1)
                            s.WriteLine(strLine66)
                            s.WriteLine(strLine67)
                            s.WriteLine(strLine68)
                            s.WriteLine(strLine68_1)
                            s.WriteLine(strLine69)
                            s.WriteLine(strLine70)
                            s.WriteLine(strLine71)
                            s.WriteLine(strLine71_1)
                            s.WriteLine(strLine72)
                            s.WriteLine(strLine73)
                            s.WriteLine(strLine74)
                            s.WriteLine(strLine74_1)
                            s.WriteLine(strLine75)
                            s.WriteLine(strLine76)
                            s.WriteLine(strLine77)
                            s.WriteLine(strLine77_1)
                            s.WriteLine(strLine78)
                            s.WriteLine(strLine79)
                            s.WriteLine(strLine80)
                            s.WriteLine(strLine80_1)
                            s.WriteLine(strLine81)
                            s.WriteLine(strLine82)
                            s.WriteLine(strLine83)
                            s.WriteLine(strLine83_1)
                            s.WriteLine(strLine84)
                            s.WriteLine(strLine85)
                            s.WriteLine(strLine86)
                            s.WriteLine(strLine86_1)
                            s.WriteLine(strLine87)
                            s.WriteLine(strLine88)
                            s.WriteLine(strLine89)
                            s.WriteLine(strLine89_1)
                            s.WriteLine(strLine90)
                            s.WriteLine(strLine91)
                            s.WriteLine(strLine92)
                            s.WriteLine(strLine92_1)
                            s.WriteLine(strLine93)
                            s.WriteLine(strLine94)
                            s.WriteLine(strLine95)
                            s.WriteLine(strLine95_1)
                            s.WriteLine(strLine96)
                            s.WriteLine(strLine97)
                            s.WriteLine(strLine98)
                            s.WriteLine(strLine98_0)
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
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            rtnval = False
                        End If
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while printing Shipping Pallet Tag! Check battery and wireless connection - then try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    rtnval = False
                End If
            Else
                Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                Me.lbError.Visible = True
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