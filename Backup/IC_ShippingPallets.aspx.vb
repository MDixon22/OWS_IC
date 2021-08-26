Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_ShippingPallets
    Inherits System.Web.UI.Page

    Private Enum ScanModes
        StartMode
        OrderMode
        ProductMode
        CaseLabelMode
        QuantityMode
        GrossWtMode
        HFDatePkgMode
        BinMode
        PrinterMode
    End Enum

#Region "Variable Declaration"
    'Private scanmode As String
    'Public validation_count As Integer = 0
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

#End Region

#Region "Page Level"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        _remqtyflag = Common.GetVariable("RemQtyFlag", Page)

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
        _whs = Common.GetVariable("Whse", Page)
        _screenmode = Common.GetVariable("ScreenParam", Page).ToString
        If _screenmode = "AddTo" Then
            bAddTo = True
            Me.lbPageTitle.Text = "OW Inventory - AddTo Shipping Pallet"
            Me.lbGridTitle.Text = "Existing Pallet - Products Just Added"
            Me.btReturn.Text = "Return"

        Else
            Me.lbPageTitle.Text = "OW Inventory - Create Shipping Pallet"
            bAddTo = False
        End If
        'AddToPalletNumber
        If Not Page.IsPostBack Then
            Common.SaveVariable("RemQtyFlag", "", Page)
            Common.SaveVariable("PrintSecondTime", "N", Page)
            If bAddTo = True Then
                Common.SaveVariable("UpdatedShippingPallet", Nothing, Page)
                Me.lbShipPalletValue.Text = Common.GetVariable("EditPalletNumber", Page)
                Mode = ScanModes.OrderMode
            Else
                Mode = ScanModes.StartMode
            End If
            Common.SaveVariable("8SavedMode", Mode, Page)
            Call ResetControls()
        End If
    End Sub

    Public Sub ResetControls()
        Try
            Select Case Mode
                Case ScanModes.StartMode 'Prepare screen for start up
                    Me.lbShipPallet.Visible = False
                    Me.lbShipPalletValue.Visible = False

                    Me.htxOrder.Visible = False
                    Me.lbOrder.Visible = False
                    Me.htxProduct.Visible = False
                    Me.lbProduct.Visible = False
                    Me.htxCaseLabel.Visible = False
                    Me.lbCaseLabel.Visible = False
                    Me.btReturn.Visible = False
                    Me.txQuantity.Visible = False
                    Me.lbQuantity.Visible = False
                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False
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
                    Me.htxProduct.Value = ""
                    Me.htxCaseLabel.Value = ""
                    Me.txQuantity.Text = ""
                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""
                    Me.lbShipPalletValue.Text = ""

                    Me.lbPrompt.Text = "Press New Pallet or To Menu button"

                Case ScanModes.OrderMode 'Prepare screen to accept Pick Order Scan
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = True
                    Me.lbProduct.Visible = True
                    Me.htxCaseLabel.Visible = True
                    Me.lbCaseLabel.Visible = True
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True

                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False
                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btRestart.Visible = True
                    Me.btNew.Visible = False
                    Me.btReturn.Visible = True

                    Me.htxOrder.Value = ""
                    Me.htxProduct.Value = ""
                    Me.htxCaseLabel.Value = ""
                    Me.txQuantity.Text = ""
                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Scan or Enter Pick Order."
                    Common.JavaScriptSetFocus(Page, Me.htxOrder)

                Case ScanModes.ProductMode 'Prepare screen to accept Pick Product Scan
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = True
                    Me.lbProduct.Visible = True
                    Me.htxCaseLabel.Visible = True
                    Me.lbCaseLabel.Visible = True
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True

                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False
                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btRestart.Visible = True
                    Me.btReturn.Visible = True
                    Me.btNew.Visible = False

                    Me.htxProduct.Value = ""
                    Me.htxCaseLabel.Value = ""
                    Me.txQuantity.Text = ""
                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Scan or Enter Pick Product."
                    Common.JavaScriptSetFocus(Page, Me.htxProduct)

                Case ScanModes.CaseLabelMode 'Prepare screen to accept CaseLabel Scan
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = True
                    Me.lbProduct.Visible = True
                    Me.htxCaseLabel.Visible = True
                    Me.lbCaseLabel.Visible = True
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True

                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False
                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btRestart.Visible = True
                    Me.btReturn.Visible = True

                    If Common.GetVariable("RemQtyFlag", Page).ToString = "T" Then
                        Me.btNew.Visible = True
                        Me.btNew.Text = "Next Product"
                    Else
                        Me.btNew.Visible = False
                    End If

                    Me.htxCaseLabel.Value = ""
                    Me.txQuantity.Text = ""
                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Scan or Enter Case Label."
                    Common.JavaScriptSetFocus(Page, Me.htxCaseLabel)

                Case ScanModes.QuantityMode 'Prepare screen to accept quantity entry
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = True
                    Me.lbProduct.Visible = True
                    Me.htxCaseLabel.Visible = True
                    Me.lbCaseLabel.Visible = True
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True
                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False

                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btRestart.Visible = True
                    Me.btReturn.Visible = True
                    Me.btNew.Visible = False

                    Me.txQuantity.Text = ""
                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Enter Case Quantity."
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)

                Case ScanModes.GrossWtMode  'Prepare screen to accept gross weight entry
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = False
                    Me.lbOrder.Visible = False
                    Me.htxProduct.Visible = False
                    Me.lbProduct.Visible = False
                    Me.htxCaseLabel.Visible = False
                    Me.lbCaseLabel.Visible = False
                    Me.txQuantity.Visible = False
                    Me.lbQuantity.Visible = False
                    Me.lbGrossWt.Visible = True
                    Me.txGrossWt.Visible = True
                    Me.lbDatePackaged.Visible = False
                    Me.txDatePackaged.Visible = False

                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btComplete.Visible = False
                    Me.btRestart.Visible = True
                    Me.btReturn.Visible = True
                    Me.btNew.Visible = False

                    Me.txGrossWt.Text = ""
                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Enter Gross Weight for Pallet."
                    Common.JavaScriptSetFocus(Page, Me.txGrossWt)

                Case ScanModes.HFDatePkgMode  'Prepare screen to accept HF Date Packaged entry
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = False
                    Me.lbOrder.Visible = False
                    Me.htxProduct.Visible = False
                    Me.lbProduct.Visible = False
                    Me.htxCaseLabel.Visible = False
                    Me.lbCaseLabel.Visible = False
                    Me.txQuantity.Visible = False
                    Me.lbQuantity.Visible = False
                    Me.lbGrossWt.Visible = False
                    Me.txGrossWt.Visible = False
                    Me.lbDatePackaged.Visible = True
                    Me.txDatePackaged.Visible = True

                    Me.txBin.Visible = False
                    Me.lbBin.Visible = False
                    Me.txPrinter.Visible = False
                    Me.lbPrinter.Visible = False

                    Me.btComplete.Visible = False
                    Me.btRestart.Visible = True
                    Me.btReturn.Visible = True
                    Me.btNew.Visible = False

                    Me.txDatePackaged.Text = ""
                    Me.txBin.Text = ""
                    Me.txPrinter.Text = ""

                    Me.lbPrompt.Text = "Enter Date Packaged from the HF Case Label."
                    Common.JavaScriptSetFocus(Page, Me.txGrossWt)

                Case ScanModes.BinMode  'Prepare screen to accept bin location entry
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = False
                    Me.lbProduct.Visible = False
                    Me.htxCaseLabel.Visible = False
                    Me.lbCaseLabel.Visible = False
                    Me.txQuantity.Visible = False
                    Me.lbQuantity.Visible = False

                    If Me.txGrossWt.Text.Length > 0 Then
                        Me.lbGrossWt.Visible = True
                        Me.txGrossWt.Visible = True
                    Else
                        Me.lbGrossWt.Visible = False
                        Me.txGrossWt.Visible = False
                    End If

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
                    Me.lbShipPallet.Visible = True
                    Me.lbShipPalletValue.Visible = True

                    Me.htxOrder.Visible = True
                    Me.lbOrder.Visible = True
                    Me.htxProduct.Visible = False
                    Me.lbProduct.Visible = False
                    Me.htxCaseLabel.Visible = False
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
        Catch ex As Exception
            Me.lbError.Text = ex.ToString
            Me.lbError.Visible = True
        End Try
        
        Try 'Must change to look at IC_TmpShipPallets
            Dim strPallet As String = ""
            strPallet = Me.lbShipPalletValue.Text
            If bAddTo = True Then
                If strPallet.Length > 0 Then
                    Me.LoadExistingPalletGrid()
                Else
                    Me.dgExistingProducts.Visible = False
                End If
            End If
            If strPallet.Length > 0 Then
                Dim sqlString As String
                Dim dsShippingPallet As New Data.DataSet
                Dim sqlCmdShippingPallet As New System.Data.SqlClient.SqlCommand
                Dim strCanSave As Boolean = False

                sqlString = "Select * from IC_TmpShipPallets Where PalletID = " & CLng(strPallet)
                'Get Temp ShippingPallets to validate whether to allow Complete function
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdShippingPallet = DB.NewSQLCommand3(sqlString)
                    If Not sqlCmdShippingPallet Is Nothing Then
                        dsShippingPallet = DB.GetDataSet(sqlCmdShippingPallet)
                        sqlCmdShippingPallet.Dispose() : sqlCmdShippingPallet = Nothing
                        DB.KillSQLConnection()
                        If dsShippingPallet.Tables(0).Rows.Count > 0 Then
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
                Me.dgExistingProducts.Visible = False
                Me.lbGridTitle.Visible = False
                Me.lbExistingGridTitle.Visible = False

            End If
        Catch ex As Exception
            Me.lbError.Text = ex.ToString
            Me.lbError.Visible = True
        End Try
    End Sub

#End Region

#Region "Temp Table"

    Public Function WriteTempTableRecord(ByVal pallet As Long, ByVal order As Long, ByVal lineno As Integer, _
                                            ByVal ppgtin As String, ByVal ordqty As Integer, _
                                            ByVal clgtin As String, ByVal expire As String, _
                                            ByVal lotno As String, ByVal caseqty As Integer, ByVal prodvariant As String) As Boolean

        WriteTempTableRecord = False
        Dim dtmScanned As Date = Now()
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString
        Me.lbError.Text = ""
        Dim strSQLInsertTemp As String = "INSERT Into IC_TmpShipPallets (PalletID,OrderID,OrderLine,PP_GTIN,OrderQty,CL_GTIN,CodeDate,Lot,CaseQty,txDateTimeEntered,UserID,ProdVariant) " & _
        "VALUES (" & pallet & "," & order & "," & lineno & ",'" & ppgtin & "'," & ordqty & ",'" & clgtin & "','" & expire & "','" & lotno & "'," & caseqty & ",'" & dtmScanned & "','" & strUnit & "','" & prodvariant & "')"

        Dim sqlCmdTempPallet As New System.Data.SqlClient.SqlCommand

        Try
            'Insert Temp Shipping Pallet record

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdTempPallet = DB.NewSQLCommand3(strSQLInsertTemp)
                If Not sqlCmdTempPallet Is Nothing Then
                    sqlCmdTempPallet.ExecuteNonQuery()
                    sqlCmdTempPallet.Dispose() : sqlCmdTempPallet = Nothing
                    WriteTempTableRecord = True
                Else
                    Me.lbError.Text = "Command Error occurred while saving temp shipping pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
                End If
                If Not sqlCmdTempPallet Is Nothing Then
                    sqlCmdTempPallet.Dispose() : sqlCmdTempPallet = Nothing
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while saving temp shipping pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while saving temp shipping pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            If Not sqlCmdTempPallet Is Nothing Then
                sqlCmdTempPallet.Dispose() : sqlCmdTempPallet = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Function

    Public Sub CleanupTempTableRecords()
        Dim sqlCmdCleanupShippingPallet As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""
        Dim strSqlTempCleanup As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text
            strSqlTempCleanup = "Delete from IC_TmpShipPallets Where PalletID = " & CLng(strPallet)

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdCleanupShippingPallet = DB.NewSQLCommand3(strSqlTempCleanup)
                If Not sqlCmdCleanupShippingPallet Is Nothing Then
                    sqlCmdCleanupShippingPallet.ExecuteNonQuery()
                    sqlCmdCleanupShippingPallet.Dispose() : sqlCmdCleanupShippingPallet = Nothing
                    DB.KillSQLConnection()
                Else
                    Me.lbError.Text = "Command Error occurred while cleaning up temp records for shipping pallet! Check battery and wireless connection - then try again."
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while cleaning up temp records for shipping pallet! Check battery and wireless connection - then try again."
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while cleaning up temp records for shipping pallet! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            If Not sqlCmdCleanupShippingPallet Is Nothing Then
                sqlCmdCleanupShippingPallet.Dispose() : sqlCmdCleanupShippingPallet = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

#Region "User Entry"

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        'Validate previous field entries
        Me.lbError.Text = ""
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text

            Call ValidateEntries(Me.htxOrder.Value, Me.htxProduct.Value, Me.htxCaseLabel.Value)

            If Me.lbError.Text.Length = 0 Then
                If IsNumeric(Me.txQuantity.Text) = True Then
                    Dim prodqty As Integer = CInt(Me.txQuantity.Text)
                    Dim SumCaseQty As Integer = 0
                    Dim qtyRemain As Integer = 0

                    'Get sum of case quantities already written to IC_TmpShipPallets using view (vwIC_TmpShipPalletsSum_byGTIN) (if any exist)
                    'Select SumCaseQty from vwIC_TmpShipPalletsSum_byGTIN where PalletID = strPallet and CL_GTIN = Mid(Me.htxCaseLabel.Value, 3, 14)
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        Dim sqlCmdShipPalletSum As New System.Data.SqlClient.SqlCommand
                        sqlCmdShipPalletSum = DB.NewSQLCommand("SQL.Query.GetShippingPalletSum")
                        If Not sqlCmdShipPalletSum Is Nothing Then
                            sqlCmdShipPalletSum.Parameters.AddWithValue("@PalletID", strPallet)
                            sqlCmdShipPalletSum.Parameters.AddWithValue("@CL_GTIN", Mid(Me.htxProduct.Value, 4, 14))
                            sqlCmdShipPalletSum.Parameters.AddWithValue("@OrdLine", CInt(Mid(Me.htxProduct.Value, 1, 3)))
                            SumCaseQty = sqlCmdShipPalletSum.ExecuteScalar()
                            sqlCmdShipPalletSum.Dispose() : sqlCmdShipPalletSum = Nothing
                            DB.KillSQLConnection()
                            If IsDBNull(SumCaseQty) = True Then
                                SumCaseQty = 0
                            End If
                            'subtract from original Pick Qty to get qty remaining
                            qtyRemain = CInt(Mid(Me.htxProduct.Value, 18, 7)) - SumCaseQty
                        Else
                            'Error getting SumCaseQty  prompt user to try again
                            Mode = ScanModes.QuantityMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Me.lbError.Text = "Command Error occurred while validating Shipping Pallet Qty - Check Connection, then Try again!"
                            ResetControls()
                        End If
                    Else
                        'Error getting New Pallet # prompt user to try again
                        Mode = ScanModes.QuantityMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Me.lbError.Text = "Connection Error occurred while validating Shipping Pallet Qty - Check Connection, then Try again!"
                        ResetControls()
                    End If

                    'Compare qty already scanned to qty entered to determine which mode the screen should be in next.
                    '** SAM ** added qtyRemain < 1 code below to stop over scanning for product quantity
                    If qtyRemain < 1 Then
                        Mode = ScanModes.ProductMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Common.SaveVariable("QtyRemain", 0, Page)
                        Common.SaveVariable("RemQtyFlag", "", Page)
                        Me.lbError.Text = "Product Quantity on order already filled! See supervisor if you feel this is not true."
                        Exit Sub
                    End If
                    If prodqty = qtyRemain Then
                        Mode = ScanModes.ProductMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Common.SaveVariable("QtyRemain", 0, Page)
                        Common.SaveVariable("RemQtyFlag", "", Page)
                    Else
                        'Qty remaining > 0 and program will expect another case label scan
                        Mode = ScanModes.CaseLabelMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Common.SaveVariable("QtyRemain", qtyRemain - prodqty, Page)
                        Common.SaveVariable("RemQtyFlag", "T", Page)
                    End If

                    'Set variables needed to write IC_TmpShipPallets record
                    Dim nPallet As Long = CLng(strPallet)
                    Dim nOrder As Long = CLng(Me.htxOrder.Value)
                    Dim iLineNo As Integer = CInt(Mid(Me.htxProduct.Value, 1, 3))
                    Dim strProdId As String = Mid(Me.htxProduct.Value, 4, 14)
                    Dim iPickQty As Integer = CInt(Mid(Me.htxProduct.Value, 18, 7))
                    Dim strProdVariant As String = Mid(Me.htxProduct.Value, 25, 3)
                    Dim strPartCode As String = Mid(Me.htxCaseLabel.Value, 3, 14)
                    Dim strCDate As String = Mid(Me.htxCaseLabel.Value, 19, 6)
                    Dim strLotNum As String = Mid(Me.htxCaseLabel.Value, 27, 12)

                    If WriteTempTableRecord(nPallet, nOrder, iLineNo, strProdId, iPickQty, strPartCode, strCDate, strLotNum, prodqty, strProdVariant) = False Then
                        'Error occurred while adding current product to shipping pallet temp table - set Mode to quantity to try again
                        Mode = ScanModes.QuantityMode
                        Me.lbError.Text = "Error occurred while adding Product - " & strProdId & " to Shipping Pallet - " & nPallet & ". Check Wireless Connection and then try entering case quantity again."
                    End If
                    ResetControls()
                Else
                    Me.lbError.Text = "Case Quantity entered needs to be numeric!"
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)
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

    Protected Sub txGrossWt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txGrossWt.TextChanged
        'Gross Weight has been entered
        If Me.txGrossWt.Text.Length > 0 And IsNumeric(Me.txGrossWt.Text) = True Then
            'Prompt for Bin Location
            Mode = ScanModes.BinMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        Else
            Me.lbError.Text = "Gross Weight needs to be a numeric entry!"
            Common.JavaScriptSetFocus(Page, Me.txGrossWt)
        End If
    End Sub

    Protected Sub txDatePackaged_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txDatePackaged.TextChanged
        'SAM 6/13/2018 - Added for the HF Pallet Tag changes.
        'Gross Weight has been entered
        If Me.txDatePackaged.Text.Length = 6 And IsNumeric(Me.txDatePackaged.Text) = True Then
            'Prompt for Bin Location
            Mode = ScanModes.BinMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            Common.SaveVariable("HFDatePkg", Me.txDatePackaged.Text, Page)
            ResetControls()
        Else
            Me.lbError.Text = "Date Pkgd needs to be a 6 digit MMDDYY date!"
            Common.JavaScriptSetFocus(Page, Me.txDatePackaged)
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
                            Dim strPallet As String = Me.lbShipPalletValue.Text

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
                                'delete records from IC_TmpShipPallets where PalletID match current screen
                                CleanupTempTableRecords()
                                Common.SaveVariable("PrintSecondTime", "N", Page)
                                Me.dgProductsScanned.SelectedIndex = -1
                                If bAddTo = True Then
                                    Common.SaveVariable("UpdatedShippingPallet", "True", Page)
                                    btReturn_Click(sender, e)
                                Else
                                    Me.lbShipPalletValue.Text = ""
                                    Mode = ScanModes.StartMode
                                    Common.SaveVariable("8SavedMode", Mode, Page)
                                    ResetControls()
                                End If
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
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""

            'Verify printer # entered is for correct location and is numeric
            If Me.txPrinter.Text.Length > 0 And IsNumeric(Me.txPrinter.Text) = True Then
                Dim iPrt As Integer = CInt(Me.txPrinter.Text)
                Dim strPallet As String = ""

                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdPrinter = DB.NewSQLCommand("SQL.Query.PrinterLookup")
                    If Not sqlCmdPrinter Is Nothing Then
                        sqlCmdPrinter.Parameters.AddWithValue("@Printer", iPrt)
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

                            If Common.GetVariable("PrintSecondTime", Page).ToString = "N" Then
                                'Write records to IC_PalletsHeader, IC_Pallets, IC_PalletsHistory
                                strPallet = Me.lbShipPalletValue.Text
                                SavePallet(strPallet & Me.htxOrder.Value)
                                Common.SaveVariable("PrintSecondTime", "Y", Page)
                            Else
                                'Skip writing records to database process because "PrintSecondTime" session variable = "Y"
                                Me.lbError.Text = ""
                            End If

                            If Me.lbError.Text = "" Then
                                'Write file to Loftware server to print out the Pallet Tag
                                If PrintShippingPalletTag() = True Then
                                    'delete records from IC_TmpShipPallets where PalletID match current screen
                                    CleanupTempTableRecords()
                                    Common.SaveVariable("PrintSecondTime", "N", Page)
                                    Me.dgProductsScanned.SelectedIndex = -1
                                    If bAddTo = True Then
                                        Common.SaveVariable("UpdatedShippingPallet", "True", Page)
                                        btReturn_Click(sender, e)
                                    Else
                                        Me.lbShipPalletValue.Text = ""
                                        Mode = ScanModes.StartMode
                                        Common.SaveVariable("8SavedMode", Mode, Page)
                                        ResetControls()
                                    End If
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

        'delete records from IC_TmpShipPallets where PalletID, OrderID, UserID match current screen
        Try
            CleanupTempTableRecords()
            Me.dgProductsScanned.Dispose()
            Me.btNew.Visible = False

            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
        Catch ex As Exception
            Me.lbError.Text = "Error occurred during restart"
            Me.lbError.Visible = True
        End Try
        
    End Sub

    Protected Sub btYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btYes.Click
        Me.lbError.Text = ""
        Me.btYes.Visible = False
        Me.btNo.Visible = False

        If Common.GetVariable("RemQtyFlag", Page).ToString = "T" Then
            'Prompt for PickProduct Entry
            Me.lbError.Text = ""
            Mode = ScanModes.ProductMode
        Else
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'If Product GTIN exists in the IC_Exceptions table then we need to prompt user for the Gross Weight
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If ChkGrossWeightException(CLng(Common.GetVariable("GTIN", Page).ToString)) = True Then
                'Prompt for Gross Weight because the product has the GrossWeight exception record
                Mode = ScanModes.GrossWtMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
            ElseIf ChkHFException(CLng(Common.GetVariable("GTIN", Page).ToString)) = True Then
                'Prompt for Date Packaged in MMDDYY format because the product is for HF
                Mode = ScanModes.HFDatePkgMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
            Else
                'All Products on Pallet picked completely
                'Prompt for Bin Location
                Mode = ScanModes.BinMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''If Product GTIN exists in the IC_Exceptions table then we need to prompt user for the Gross Weight
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'If ChkGrossWeightException(Mid(Me.htxProduct.Value, 4, 14)) = True Then
            '    'Prompt for Gross Weight because the product has the GrossWeight exception record
            '    Mode = ScanModes.GrossWtMode
            '    Common.SaveVariable("8SavedMode", Mode, Page)
            '    ResetControls()
            'Else
            '    'Done adding products to the pallet and are going to save it regardless of error reported.
            '    'Prompt for Bin Location
            '    Mode = ScanModes.BinMode
            '    Common.SaveVariable("8SavedMode", Mode, Page)
            '    ResetControls()
            'End If
            ''Prompt for Bin Location ------Code below was removed and replaced with code above
            ''Me.lbError.Text = ""
            ''Mode = ScanModes.BinMode
        End If
        Me.btNew.Visible = False

        Me.lbGridTitle.Visible = False
        Common.SaveVariable("RemQtyFlag", "", Page)
        Common.SaveVariable("8SavedMode", Mode, Page)
        ResetControls()
    End Sub

    Protected Sub btNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNo.Click
        Me.lbError.Text = ""
        Me.btYes.Visible = False
        Me.btNo.Visible = False

        If Common.GetVariable("RemQtyFlag", Page).ToString = "T" Then
            'Prompt for CaseLabel Entry
            Me.lbError.Text = ""
            Mode = ScanModes.CaseLabelMode
        Else
            'Prompt for PickProduct Entry
            Me.lbError.Text = ""
            Mode = ScanModes.ProductMode
        End If
        Me.btNew.Visible = False

        Common.SaveVariable("RemQtyFlag", "", Page)
        Common.SaveVariable("8SavedMode", Mode, Page)
        ResetControls()
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        Common.SaveVariable("ScreenParam", Nothing, Page)
        If bAddTo = True Then
            'Setup Main Menu URL so program will redirect to Main Menu
            Common.SaveVariable("newURL", "~/IC_WorkWithShippingPallet.aspx", Page)
        Else
            'Setup Main Menu URL so program will redirect to Main Menu
            Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
        End If

    End Sub

    Protected Sub btComplete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btComplete.Click
        'Want to go through each product on pallet and make sure quantity matches what was picked
        'Read through products on pallet getting quantity expected to pick
        'read through all caselabels by product summing quantity entered on screen
        'compare the 2 quantities - if any of them have a quantity difference show prompt

        Me.lbError.Text = ""        ' Must change to use the IC_TmpShipPallets table on SRV04 
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text
            Dim SumCaseQty As Integer = 0
            Dim validateMessage As String = ""
            'Dim prodqty As Integer = CInt(Me.txQuantity.Text)
            Dim qtyRemain As Integer = 0
            Dim strSqlQtyChk As String = "SELECT PalletID, OrderLine, CL_GTIN, MAX(OrderQty) AS OrderQty, SUM(CaseQty) AS SumCaseQty FROM IC_TmpShipPallets Where PalletID = " & CLng(strPallet) & " GROUP BY PalletID, CL_GTIN, OrderLine"
            Dim dsShippingPalletQtyChk As New Data.DataSet
            Dim drShippingPalletQtyChk As Data.DataRow

            'Get sum of case quantities already written to IC_TmpShipPallets to compare with PickProduct OrderQty
            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdShipPalletQtyChk As New System.Data.SqlClient.SqlCommand
                sqlCmdShipPalletQtyChk = DB.NewSQLCommand3(strSqlQtyChk)
                If Not sqlCmdShipPalletQtyChk Is Nothing Then
                    dsShippingPalletQtyChk = DB.GetDataSet(sqlCmdShipPalletQtyChk)
                    For Each drShippingPalletQtyChk In dsShippingPalletQtyChk.Tables(0).Rows
                        If drShippingPalletQtyChk.Item("OrderQty") <> drShippingPalletQtyChk.Item("SumCaseQty") Then
                            validateMessage += "Quantity for product " & drShippingPalletQtyChk.Item("CL_GTIN") & " does not match, expected: " & drShippingPalletQtyChk.Item("OrderQty") & "; scanned = " & drShippingPalletQtyChk.Item("SumCaseQty") & ". "
                        End If
                    Next
                Else
                    'Error getting SumCaseQty  prompt user to try again
                    Me.lbError.Text = "Command Error occurred while validating Shipping Pallet Quantities - Check Connection, then Try again!"
                End If
            Else
                'Error getting SumCaseQty  prompt user to try again
                Me.lbError.Text = "Connection Error occurred while validating Shipping Pallet Quantities - Check Connection, then Try again!"
            End If

            If Not validateMessage = "" Then
                Me.lbError.Text = validateMessage + " Are you sure you wish to save? Press Yes to complete or No to continue current pallet."
                Me.btYes.Visible = True
                Me.btNo.Visible = True
            Else
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'If Product GTIN exists in the IC_Exceptions table then we need to prompt user for the Gross Weight
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                If ChkGrossWeightException(CLng(Common.GetVariable("GTIN", Page).ToString)) = True Then
                    'Prompt for Gross Weight because the product has the GrossWeight exception record
                    Mode = ScanModes.GrossWtMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                ElseIf ChkHFException(CLng(Common.GetVariable("GTIN", Page).ToString)) = True Then
                    'Prompt for Date Packaged in MMDDYY format because the product is for HF
                    Mode = ScanModes.HFDatePkgMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                Else
                    'All Products on Pallet picked completely
                    'Prompt for Bin Location
                    Mode = ScanModes.BinMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = ex.ToString + " See your supervisor, Last Pallet failed to update system!"
            Me.lbError.Visible = True
        End Try
    End Sub

    Protected Sub btNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNew.Click
        If Me.btNew.Text = "New Pallet" Then
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
                        If IsDBNull(ShippingPallet) = False And ShippingPallet <> 0 Then 'Successfully got New Pallet #
                            Me.lbShipPalletValue.Text = ShippingPallet
                            Mode = ScanModes.OrderMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Me.lbError.Text = ""
                            ResetControls()
                        Else
                            'Error getting New Pallet # prompt user to try again
                            Mode = ScanModes.StartMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            Me.lbError.Text = "Data Error occurred while getting New Pallet # - Check Connection, then Try again!"
                            ResetControls()
                        End If
                    Else
                        'Error getting New Pallet # prompt user to try again
                        Mode = ScanModes.StartMode
                        Common.SaveVariable("8SavedMode", Mode, Page)
                        Me.lbError.Text = "Command Error occurred while getting New Pallet # - Check Connection, then Try again!"
                        ResetControls()
                    End If
                Else
                    'Error getting New Pallet # prompt user to try again
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
                Me.lbError.Visible = True
                ResetControls()
            End Try
        ElseIf Me.btNew.Text = "Next Product" Then
            'Prompt message stating qty difference and ask if continue Yes or No
            Me.lbError.Text = "The last case quantity entered for the product added to the Shipping Pallet did not match what was ordered. Press Yes button to accept the quantity difference or the No button to scan another Case Label and add more cases."
            Me.btNew.Visible = False
            Me.btYes.Visible = True
            Me.btNo.Visible = True
            Common.SaveVariable("RemQtyFlag", "T", Page)
        End If
    End Sub

#End Region

#Region "Custom Processes"

    Public Sub SavePallet(ByVal oPallet As String) ' Must change to use the IC_TmpShipPallets table on SRV04 
        Dim sqlPalletInsert As String = ""
        Dim strPallet As String = Mid(oPallet.ToString, 1, 9)
        Dim strOrder As String = Mid(oPallet.ToString, 10, 9)
        Dim strUnit As String = Common.GetVariable("UserID", Page).ToString

        Me.lbError.Text = ""

        Try
            'Insert Pallet records
            _dateentered = Now()
            sqlPalletInsert = "insertShipPallet "

            Dim SumCaseQty As Integer = 0
            Dim sqlString As String
            Dim dv As New DataView
            Dim dsShippingPalletSave As New Data.DataSet
            Dim sqlCmdShippingPalletSave As New System.Data.SqlClient.SqlCommand

            sqlString = "Select * from IC_TmpShipPallets Where PalletID = " & CLng(strPallet)

            'Get Temp ShippingPallets and write to IC_PalletsHeader and IC_Pallets tables
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
                        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
                        sqlCmdPallet = DB.NewSQLCommand2(sqlPalletInsert)
                        If Not sqlCmdPallet Is Nothing Then
                            sqlCmdPallet.Parameters.AddWithValue("@pnPallet", CLng(strPallet))
                            sqlCmdPallet.Parameters.AddWithValue("@pnOrder", CLng(strOrder))
                            sqlCmdPallet.Parameters.AddWithValue("@pdtmScanned", _dateentered)
                            sqlCmdPallet.Parameters.AddWithValue("@pstrUnit", strUnit)
                            sqlCmdPallet.Parameters.AddWithValue("@pstrProduct", drShippingPalletSave.Item("PP_GTIN"))
                            sqlCmdPallet.Parameters.AddWithValue("@pnLineNumber", drShippingPalletSave.Item("OrderLine"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrExpiration", drShippingPalletSave.Item("CodeDate"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrMfgOrd", Mid(drShippingPalletSave.Item("Lot"), 3, 5))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrLot", Mid(drShippingPalletSave.Item("Lot"), 8, 5))
                            sqlCmdPallet.Parameters.AddWithValue("@pnQuantity", drShippingPalletSave.Item("CaseQty"))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrBin", RTrim(Me.txBin.Text))
                            sqlCmdPallet.Parameters.AddWithValue("@pstrProcess", "AP")
                            sqlCmdPallet.Parameters.AddWithValue("@pstrProdVariant", drShippingPalletSave.Item("ProdVariant"))
                            'sqlCmdPallet.Parameters("@iErrorCode").Direction = ParameterDirection.Output
                            sqlCmdPallet.ExecuteNonQuery()
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                        Else
                            Me.lbError.Text = "Command Error occurred while saving pallet to database! Check Wireless Connection and Battery then try again or see your supervisor."
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

    Public Sub ValidateEntries(ByVal orderid As String, ByVal product As String, ByVal caselabel As String)
        'Validate Order Length
        Me.lbError.Text = ""
        Try
            If Me.htxOrder.Value.Length <> CInt(ConfigurationManager.AppSettings.Get("UI.Order.Length").ToString) Then
                Me.lbError.Text = "Value entered for Pick Order is wrong length - start process again!"
            Else
                'Length is correct check to see if it matches existing pallet if doing a addto
                If bAddTo = True Then
                    If CLng(Me.htxOrder.Value) <> CLng(Common.GetVariable("EditPalletOrderID", Page).ToString) Then
                        Me.lbError.Text = "Value entered for Pick Order does not match OrderID on the Existing Shipping Pallet - start process again!"
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Pick Order! Check Battery and Wireless Connection then Restart Pallet"
            Me.lbError.Visible = True
        End Try

        If Me.lbError.Text.Length > 0 Then
            Mode = ScanModes.OrderMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
            Exit Sub
        End If
        'Validate GTIN in PickProduct is valid
        If Me.htxProduct.Value.Length = CInt(ConfigurationManager.AppSettings.Get("UI.PickProduct.Length").ToString) Then
            Try
                'Validate GTIN in PickProduct is valid
                Dim strPrdNo As String = Nothing
                Dim sqlCmdValGTIN As New System.Data.SqlClient.SqlCommand
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdValGTIN = DB.NewSQLCommand("SQL.Query.GTINLookupProd")
                    If Not sqlCmdValGTIN Is Nothing Then
                        sqlCmdValGTIN.Parameters.AddWithValue("@GTIN", Mid(Me.htxProduct.Value, 4, 14))
                        strPrdNo = sqlCmdValGTIN.ExecuteScalar
                        sqlCmdValGTIN.Dispose() : sqlCmdValGTIN = Nothing
                        DB.KillSQLConnection()
                        If strPrdNo.Length < 1 Then
                            Me.lbError.Text = "Pick Product scanned not valid in database! Try again or see your supervisor."
                            Common.SaveVariable("OrderQty", "0", Page)
                        Else
                            Common.SaveVariable("OrderQty", CInt(Mid(Me.htxProduct.Value, 18, 7)), Page)
                            Common.SaveVariable("PickVer", CInt(Mid(Me.htxProduct.Value, 25, 3)), Page)
                            Common.SaveVariable("GTIN", CLng(Mid(Me.htxProduct.Value, 4, 14)), Page)
                        End If
                    Else
                        sqlCmdValGTIN.Dispose() : sqlCmdValGTIN = Nothing
                        DB.KillSQLConnection()

                        Me.lbError.Text = "Command Error occurred while validating Pick Product! Check Battery and Wireless Connection -then try again or see your supervisor."
                    End If
                Else
                    Me.lbError.Text = "Error while connecting to database! Try again or see your supervisor."
                End If

                'Handle errors
                If Me.lbError.Text.Length > 0 Then
                    Mode = ScanModes.ProductMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    DB.KillSQLConnection()
                    ResetControls()
                    Exit Sub
                End If
            Catch ex As Exception
                Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Pick Product! Check Battery and Wireless Connection -then try again or see your supervisor."
                Me.lbError.Visible = True
                Mode = ScanModes.ProductMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                DB.KillSQLConnection()
                ResetControls()
                Exit Sub
            Finally
                DB.KillSQLConnection()
            End Try
        Else
            Me.lbError.Text = "Entry made for Pick Product is not valid!  Try again or see your supervisor."
            Me.lbError.Visible = True
            Mode = ScanModes.ProductMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
            Exit Sub
        End If

        'Validate Case Label and compare values to PickProduct
        If Me.htxCaseLabel.Value.Length = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString) Or Me.htxCaseLabel.Value.Length = CInt(ConfigurationManager.AppSettings.Get("UI.AltCaseLength").ToString) Then
            Try
                Dim strCDate As String = Mid(Me.htxCaseLabel.Value, 19, 6)
                If Mid(Me.htxCaseLabel.Value, 3, 14) <> Mid(Me.htxProduct.Value, 4, 14) Then
                    Me.lbError.Text = "You have scanned an incorrect case label.  Try again. Case PartCode = " & Mid(Me.htxCaseLabel.Value, 3, 14) & " and Pick ProductId = " & Mid(Me.htxProduct.Value, 4, 14)
                    Me.lbError.Visible = True
                    Mode = ScanModes.CaseLabelMode
                    Common.SaveVariable("8SavedMode", Mode, Page)
                    ResetControls()
                    Exit Sub
                Else
                    If RTrim(Me.htxCaseLabel.Value).Length = 44 And RTrim(Me.htxProduct.Value).Length = 27 Then
                        If Mid(Me.htxCaseLabel.Value, 42, 3) <> Mid(Me.htxProduct.Value, 25, 3) Then
                            Me.lbError.Text = "You have scanned an incorrect case label.  Try again. Case Version = " & Mid(Me.htxCaseLabel.Value, 42, 3) & " and Pick Version = " & Mid(Me.htxProduct.Value, 25, 3)
                            Me.lbError.Visible = True
                            Mode = ScanModes.CaseLabelMode
                            Common.SaveVariable("8SavedMode", Mode, Page)
                            ResetControls()
                            Exit Sub
                        End If
                    Else

                    End If
                End If
            Catch ex As Exception
                Me.lbError.Text = ex.ToString
                Me.lbError.Visible = True
                Mode = ScanModes.CaseLabelMode
                Common.SaveVariable("8SavedMode", Mode, Page)
                ResetControls()
                Exit Sub
            End Try
        Else
            Me.lbError.Text = "CaseLabel Barcode scanned is incorrect length.  Try again."
            Me.lbError.Visible = True
            Mode = ScanModes.CaseLabelMode
            Common.SaveVariable("8SavedMode", Mode, Page)
            ResetControls()
            Exit Sub
        End If
    End Sub

    Public Sub LoadGrid()
        Dim sqlString As String = ""
        Dim dvGrd As New DataView
        Dim dsShippingPalletGrd As New Data.DataSet
        Dim sqlCmdShippingPalletGrd As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text
            sqlString = "Select ProductID, ProdVariant as Version,CaseQty,ProdDesc from vwIC_TmpShipPalletsGridView Where PalletID = " & CLng(strPallet)
            'Get Temp ShippingPallet Products for Grid
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdShippingPalletGrd = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdShippingPalletGrd Is Nothing Then
                    dsShippingPalletGrd = DB.GetDataSet(sqlCmdShippingPalletGrd)
                    sqlCmdShippingPalletGrd.Dispose() : sqlCmdShippingPalletGrd = Nothing
                    DB.KillSQLConnection()
                    If dsShippingPalletGrd Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while displaying products added to current pallet! Check battery and wireless connection - then try again."
                    Else
                        dvGrd = New DataView(dsShippingPalletGrd.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
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
                Else
                    sqlCmdShippingPalletGrd.Dispose() : sqlCmdShippingPalletGrd = Nothing
                    DB.KillSQLConnection()
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while displaying products added to current pallet! Check battery and wireless connection - then try again."
        End Try
    End Sub

    Public Sub LoadExistingPalletGrid()
        Dim sqlString As String
        Dim dvExistingPalletGrd As New DataView
        Dim dsExistingPalletGrd As New Data.DataSet
        Dim sqlCmdExistingPalletGrd As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            strPallet = Me.lbShipPalletValue.Text
            sqlString = "Select ProductID, ProdVariant as Version, CaseQty,ProdDesc from vwIC_ExistingPalletGridView Where PalletID = " & CLng(strPallet)
            'Get Shipto Info for ShippingPallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdExistingPalletGrd = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdExistingPalletGrd Is Nothing Then
                    dsExistingPalletGrd = DB.GetDataSet(sqlCmdExistingPalletGrd)
                    sqlCmdExistingPalletGrd.Dispose() : sqlCmdExistingPalletGrd = Nothing
                    DB.KillSQLConnection()
                    If dsExistingPalletGrd Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while displaying existing products on current pallet! Check battery and wireless connection - then try again."
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
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function ChkGrossWeightException(ByVal lngGTin As Long) As Boolean
        ChkGrossWeightException = False

        Dim strSqlGTinChkException As String = "SELECT Prod FROM IC_ExceptionsMaster Where GTIN = " & lngGTin & " AND Exception = 'GrossWeight'"
        Dim iProd As Integer

        'Get Product Exception for GrossWeight
        If Not DB.MakeSQLConnection("Warehouse") Then
            Dim sqlCmdGTinChkException As New System.Data.SqlClient.SqlCommand
            sqlCmdGTinChkException = DB.NewSQLCommand3(strSqlGTinChkException)
            If Not sqlCmdGTinChkException Is Nothing Then
                iProd = sqlCmdGTinChkException.ExecuteScalar()
                sqlCmdGTinChkException.Dispose() : sqlCmdGTinChkException = Nothing
                DB.KillSQLConnection()
                If iProd > 0 Then
                    ChkGrossWeightException = True
                End If
            Else
                'Error getting Product Exception for GrossWeight
                Me.lbError.Text = "Command Error occurred while getting Product Exception for GrossWeight - Check Connection then press Complete again."
            End If
        Else
            'Error getting Product Exception for GrossWeight
            Me.lbError.Text = "Connection Error occurred while getting Product Exception for GrossWeight - Check Connection then press Complete again."
        End If

    End Function

    Public Function ChkHFException(ByVal lngGTin As Long) As Boolean
        ChkHFException = False

        Dim strSqlGTinChkException As String = "SELECT Prod FROM IC_ExceptionsMaster Where GTIN = " & lngGTin & " AND Exception = 'HickoryFarms' AND [Active] = 1"
        Dim iProd As Integer

        'Get Product Exception for GrossWeight
        If Not DB.MakeSQLConnection("Warehouse") Then
            Dim sqlCmdGTinChkException As New System.Data.SqlClient.SqlCommand
            sqlCmdGTinChkException = DB.NewSQLCommand3(strSqlGTinChkException)
            If Not sqlCmdGTinChkException Is Nothing Then
                iProd = sqlCmdGTinChkException.ExecuteScalar()
                sqlCmdGTinChkException.Dispose() : sqlCmdGTinChkException = Nothing
                DB.KillSQLConnection()
                If iProd > 0 Then
                    ChkHFException = True
                End If
            Else
                'Error getting Product Exception for GrossWeight
                Me.lbError.Text = "Command Error occurred while getting Product Exception for HickoryFarms - Check Connection then press Complete again."
            End If
        Else
            'Error getting Product Exception for GrossWeight
            Me.lbError.Text = "Connection Error occurred while getting Product Exception for HickoryFarms - Check Connection then press Complete again."
        End If

    End Function

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
        Dim _HFItem As String = ""
        Dim _ShipToAddress As String = ""
        Dim _HFPalletQty As String = ""
        Dim _BCPalletQty As String = ""
        Dim _LotCode As String = ""
        Dim _HFLocation As String = ""
        Dim _HFOrderID As String = ""
        Dim _HFPONUM As String = ""
        Dim _HFDescription As String = ""
        Dim _HF_Batch As String = ""
        Dim _HF_EachQty As String = ""
        Dim _HF_ItemUOM As String = ""
        Dim _HF_CaseCt As String = ""
        Dim _HF_NetWt As String = ""
        Dim _HF_MfgDate As String = ""
        Dim _HF_MMDDYY As String = ""

        Try

            'Used for getting ShipTo Info and Shipping Pallet Details From vwIC_OW_ShippingPallets
            Dim dsShippingPallet As New Data.DataSet
            Dim sqlCmdShippingPallet As New System.Data.SqlClient.SqlCommand

            'Get Shipto Info for ShippingPallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                'SAM - 6/13/2018 - Modified to handle 2018 Pallet specs for Hickory Farms
                'sqlCmdShippingPallet = DB.NewSQLCommand("SQL.Query.GetShippingPalletNew2")
                sqlCmdShippingPallet = DB.NewSQLCommand("SQL.Query.GetShippingPalletNew2")
                If Not sqlCmdShippingPallet Is Nothing Then
                    _pallet = CLng(Me.lbShipPalletValue.Text)
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
                            Dim strLine1 As String = "*FORMAT,RDC_OW_SHIPPINGPALLETNEW"
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

                            For x = 0 To (dsShippingPallet.Tables(0).Rows.Count - 1)
                                Select Case x
                                    Case 0 '
                                        strLine9 = "HR_ProductID_01," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine10 = "HR_CaseQty_01," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine11 = "HR_ProdDesc_01," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine11_1 = "HR_ProdCDate_01," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                        'Modified 3/30/2016 - for Hickory Farms ShipTo Tag
                                        _HFItem = Trim(dsShippingPallet.Tables(0).Rows(x).Item("HFItemNum").ToString)
                                        _ShipToAddress = dsShippingPallet.Tables(0).Rows(x).Item("ShiptoAddress").ToString
                                        _HFPalletQty = Trim(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString) & " CS"
                                        _BCPalletQty = Trim(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        _LotCode = dsShippingPallet.Tables(0).Rows(x).Item("LotCode").ToString
                                        _HFLocation = dsShippingPallet.Tables(0).Rows(x).Item("HF_Location").ToString
                                        _HF_MMDDYY = dsShippingPallet.Tables(0).Rows(x).Item("HF_BestBy_MMDDYY").ToString
                                        _HFPONUM = dsShippingPallet.Tables(0).Rows(x).Item("CustomerPO").ToString
                                        _HFOrderID = dsShippingPallet.Tables(0).Rows(x).Item("OrderID").ToString
                                        _HFDescription = dsShippingPallet.Tables(0).Rows(x).Item("HFDescription").ToString
                                        _HF_Batch = dsShippingPallet.Tables(0).Rows(x).Item("HF_Batch").ToString
                                        _HF_EachQty = dsShippingPallet.Tables(0).Rows(x).Item("HF_EachQty").ToString
                                        _HF_ItemUOM = dsShippingPallet.Tables(0).Rows(x).Item("HF_ItemUOM").ToString
                                        _HF_CaseCt = dsShippingPallet.Tables(0).Rows(x).Item("HF_CaseCt").ToString
                                        _HF_NetWt = dsShippingPallet.Tables(0).Rows(x).Item("HF_NetWt").ToString

                                    Case 1
                                        strLine12 = "HR_ProductID_02," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine13 = "HR_CaseQty_02," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine14 = "HR_ProdDesc_02," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine14_1 = "HR_ProdCDate_02," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 2
                                        strLine15 = "HR_ProductID_03," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine16 = "HR_CaseQty_03," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine17 = "HR_ProdDesc_03," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine17_1 = "HR_ProdCDate_03," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 3
                                        strLine18 = "HR_ProductID_04," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine19 = "HR_CaseQty_04," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine20 = "HR_ProdDesc_04," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine20_1 = "HR_ProdCDate_04," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 4
                                        strLine21 = "HR_ProductID_05," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine22 = "HR_CaseQty_05," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine23 = "HR_ProdDesc_05," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine23_1 = "HR_ProdCDate_05," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 5
                                        strLine24 = "HR_ProductID_06," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine25 = "HR_CaseQty_06," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine26 = "HR_ProdDesc_06," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine26_1 = "HR_ProdCDate_06," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 6
                                        strLine27 = "HR_ProductID_07," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine28 = "HR_CaseQty_07," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine29 = "HR_ProdDesc_07," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine29_1 = "HR_ProdCDate_07," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 7
                                        strLine30 = "HR_ProductID_08," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine31 = "HR_CaseQty_08," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine32 = "HR_ProdDesc_08," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine32_1 = "HR_ProdCDate_08," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 8
                                        strLine33 = "HR_ProductID_09," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine34 = "HR_CaseQty_09," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine35 = "HR_ProdDesc_09," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine35_1 = "HR_ProdCDate_09," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 9
                                        strLine36 = "HR_ProductID_10," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine37 = "HR_CaseQty_10," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine38 = "HR_ProdDesc_10," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine38_1 = "HR_ProdCDate_10," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 10
                                        strLine39 = "HR_ProductID_11," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine40 = "HR_CaseQty_11," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine41 = "HR_ProdDesc_11," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine41_1 = "HR_ProdCDate_11," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 11
                                        strLine42 = "HR_ProductID_12," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine43 = "HR_CaseQty_12," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine44 = "HR_ProdDesc_12," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine44_1 = "HR_ProdCDate_12," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 12
                                        strLine45 = "HR_ProductID_13," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine46 = "HR_CaseQty_13," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine47 = "HR_ProdDesc_13," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine47_1 = "HR_ProdCDate_13," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 13
                                        strLine48 = "HR_ProductID_14," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine49 = "HR_CaseQty_14," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine50 = "HR_ProdDesc_14," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine50_1 = "HR_ProdCDate_14," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 14
                                        strLine51 = "HR_ProductID_15," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine52 = "HR_CaseQty_15," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine53 = "HR_ProdDesc_15," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine53_1 = "HR_ProdCDate_15," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 15
                                        strLine54 = "HR_ProductID_16," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine55 = "HR_CaseQty_16," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine56 = "HR_ProdDesc_16," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine56_1 = "HR_ProdCDate_16," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 16
                                        strLine57 = "HR_ProductID_17," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine58 = "HR_CaseQty_17," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine59 = "HR_ProdDesc_17," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine59_1 = "HR_ProdCDate_17," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 17
                                        strLine60 = "HR_ProductID_18," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine61 = "HR_CaseQty_18," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine62 = "HR_ProdDesc_18," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine62_1 = "HR_ProdCDate_18," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 18
                                        strLine63 = "HR_ProductID_19," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine64 = "HR_CaseQty_19," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine65 = "HR_ProdDesc_19," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine65_1 = "HR_ProdCDate_19," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 19
                                        strLine66 = "HR_ProductID_20," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine67 = "HR_CaseQty_20," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine68 = "HR_ProdDesc_20," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine68_1 = "HR_ProdCDate_20," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 20
                                        strLine69 = "HR_ProductID_21," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine70 = "HR_CaseQty_21," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine71 = "HR_ProdDesc_21," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine71_1 = "HR_ProdCDate_21," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 21
                                        strLine72 = "HR_ProductID_22," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine73 = "HR_CaseQty_22," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine74 = "HR_ProdDesc_22," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine74_1 = "HR_ProdCDate_22," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 22
                                        strLine75 = "HR_ProductID_23," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine76 = "HR_CaseQty_23," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine77 = "HR_ProdDesc_23," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine77_1 = "HR_ProdCDate_23," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 23
                                        strLine78 = "HR_ProductID_24," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine79 = "HR_CaseQty_24," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine80 = "HR_ProdDesc_24," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine80_1 = "HR_ProdCDate_24," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 24
                                        strLine81 = "HR_ProductID_25," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine82 = "HR_CaseQty_25," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine83 = "HR_ProdDesc_25," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine83_1 = "HR_ProdCDate_25," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 25
                                        strLine84 = "HR_ProductID_26," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine85 = "HR_CaseQty_26," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine86 = "HR_ProdDesc_26," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine86_1 = "HR_ProdCDate_26," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 26
                                        strLine87 = "HR_ProductID_27," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine88 = "HR_CaseQty_27," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine89 = "HR_ProdDesc_27," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine89_1 = "HR_ProdCDate_27," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 27
                                        strLine90 = "HR_ProductID_28," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine91 = "HR_CaseQty_28," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine92 = "HR_ProdDesc_28," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine92_1 = "HR_ProdCDate_28," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 28
                                        strLine93 = "HR_ProductID_29," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine94 = "HR_CaseQty_29," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine95 = "HR_ProdDesc_29," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine95_1 = "HR_ProdCDate_29," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                    Case 29
                                        strLine96 = "HR_ProductID_30," & dsShippingPallet.Tables(0).Rows(x).Item("ProductID").ToString & " - " & dsShippingPallet.Tables(0).Rows(x).Item("ProdVariant").ToString
                                        strLine97 = "HR_CaseQty_30," & dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString
                                        strLine98 = "HR_ProdDesc_30," & dsShippingPallet.Tables(0).Rows(x).Item("ProdDesc").ToString
                                        strLine98_0 = "HR_ProdCDate_30," & dsShippingPallet.Tables(0).Rows(x).Item("CodeDate").ToString
                                        totalcases = totalcases + CInt(dsShippingPallet.Tables(0).Rows(x).Item("Qty").ToString)
                                        grossweight = grossweight + CInt(dsShippingPallet.Tables(0).Rows(x).Item("GrossWt").ToString)
                                End Select
                            Next
                            strLine98_1 = "HR_TotalCases," & totalcases
                            Common.SaveVariable("HR_TotalCases", totalcases, Page)

                            If Me.txGrossWt.Text.Length > 0 Then
                                'Use the gross weight entered by the associate
                                grossweight = CInt(Me.txGrossWt.Text)
                            End If
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

                            'Modified 3/30/2016 - SAM - For printing Hickory Farms Shipto Tag instead of Standard Tag
                            Dim destPathST As String = ""
                            Dim tempPathST As String = ""
                            Dim filenmST As String = ""
                            Dim PathFileST As String = ""
                            Dim sourceFileST As String = ""
                            Dim successfileST As String = ""
                            Dim destinationFileST As String = ""
                            Dim _shiptotag As String = ""

                            If _HFItem = "N" Then
                                'Print the Standard ShipTo Tag 
                                _shiptotag = _pallet & "STTag"

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

                                destPathST = "\\192.168.5.4\wddrop\"
                                'destPath = ConfigurationManager.AppSettings.Get("LW.DestPath").ToString
                                tempPathST = "\\192.168.5.4\wddroptemp\"
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

                            Else ' The pallet is being shipped to Hickory Farms
                                'Print the Hickory Farms Special ShipTo Tag 
                                _shiptotag = _pallet & "HFTag"
                                _HF_MfgDate = Common.GetVariable("HFDatePkg", Page).ToString()

                                'Drop file into Loftware program for printing of pallet labels _HFPalletQty   _LotCode dsShippingPallet.Tables(0).Rows(0).Item("ShipTo").ToString
                                Dim strLineHF1 As String = "*FORMAT,RDC_OW_SHIPTO_HF_NEW3"
                                Dim strLineHF2 As String = strLine5.ToString 'ShiptoName from above
                                Dim strLineHF3 As String = "HR_Location," & RTrim(_HFLocation)
                                Dim strLineHF4 As String = "HR_Address," & RTrim(_ShipToAddress)
                                Dim strLineHF5 As String = "HR_OrderNo," & RTrim(_HFOrderID)
                                Dim strLineHF6 As String = "HR_HFPONUM," & RTrim(_HFPONUM)
                                Dim strLineHF7 As String = "BC_HFPONUM," & RTrim(_HFPONUM)
                                Dim strLineHF8 As String = "BC_HFItem," & RTrim(_HFItem)
                                Dim strLineHF9 As String = "HR_HFItem," & RTrim(_HFItem)
                                Dim strLineHF10 As String = "HR_HF_DESC," & RTrim(_HFDescription)
                                Dim strLineHF11 As String = "BC_HFBatch," & RTrim(_HF_Batch)
                                Dim strLineHF12 As String = "HR_HFBatch," & RTrim(_HF_Batch)
                                Dim strLineHF13 As String = "HR_PalletQty," & RTrim(_HF_EachQty)
                                Dim strLineHF14 As String = "BC_PalletQty," & RTrim(_HF_EachQty)
                                Dim strLineHF15 As String = "HR_ItemUOM," & RTrim(_HF_ItemUOM)
                                Dim strLineHF16 As String = "BC_ItemUOM," & RTrim(_HF_ItemUOM)
                                Dim strLineHF17 As String = "HR_HF_CaseCt," & RTrim(_HF_CaseCt)
                                Dim strLineHF18 As String = "HR_HF_NetWt," & RTrim(_HF_NetWt)
                                Dim strLineHF19 As String = "HR_HF_MMDDYY," & RTrim(_HF_MMDDYY)
                                Dim strLineHF20 As String = "BC_HF_MMDDYY," & RTrim(_HF_MMDDYY)
                                Dim strLineHF21 As String = "HR_HFMfgDate," & RTrim(_HF_MfgDate)
                                Dim strLineHF22 As String = "BC_HFMfgDate," & RTrim(_HF_MfgDate)
                                Dim strLineHF23 As String = "*QUANTITY,2"
                                Dim strLineHF24 As String = "*PRINTERNUMBER," & _printer
                                Dim strLineHF25 As String = "*PRINTLABEL"

                                Dim destPathHF As String = "\\192.168.5.4\wddrop\"
                                'destPath = ConfigurationManager.AppSettings.Get("LW.DestPath").ToString
                                Dim tempPathHF As String = "\\192.168.5.4\wddroptemp\"
                                'tempPath = ConfigurationManager.AppSettings.Get("LW.TempDestPath").ToString

                                Dim filenmHF As String = Trim(_shiptotag) & DatePart("yyyy", Now) & DatePart("m", Now) & DatePart("d", Now) & DatePart("h", Now) & DatePart("n", Now) & DatePart("s", Now) & ".pas"
                                Dim PathFileHF = tempPathHF & filenmHF

                                'declaring a FileStream and creating a text document file named file with access mode of writing
                                Dim fsHF As New FileStream(PathFileHF, FileMode.Create, FileAccess.Write)
                                'creating a new StreamWriter and passing the filestream object fs as argument
                                Dim sHF As New StreamWriter(fsHF)
                                'the seek method is used to move the cursor to next position to avoid text to be overwritten
                                sHF.BaseStream.Seek(0, SeekOrigin.End)

                                'Write each line of the pas file required by Loftware
                                sHF.WriteLine(strLineHF1)
                                sHF.WriteLine(strLineHF2)
                                sHF.WriteLine(strLineHF3)
                                sHF.WriteLine(strLineHF4)
                                sHF.WriteLine(strLineHF5)
                                sHF.WriteLine(strLineHF6)
                                sHF.WriteLine(strLineHF7)
                                sHF.WriteLine(strLineHF8)
                                sHF.WriteLine(strLineHF9)
                                sHF.WriteLine(strLineHF10)
                                sHF.WriteLine(strLineHF11)
                                sHF.WriteLine(strLineHF12)
                                sHF.WriteLine(strLineHF13)
                                sHF.WriteLine(strLineHF14)
                                sHF.WriteLine(strLineHF15)
                                sHF.WriteLine(strLineHF16)
                                sHF.WriteLine(strLineHF17)
                                sHF.WriteLine(strLineHF18)
                                sHF.WriteLine(strLineHF19)
                                sHF.WriteLine(strLineHF20)
                                sHF.WriteLine(strLineHF21)
                                sHF.WriteLine(strLineHF22)
                                sHF.WriteLine(strLineHF23)
                                sHF.WriteLine(strLineHF24)
                                sHF.WriteLine(strLineHF25)
                                sHF.Close()

                                Dim sourceFileHF As String = tempPathHF & filenmHF
                                Dim successfileHF As String = tempPathHF & filenmHF & ".success"
                                Dim destinationFileHF As String = destPathHF & filenmHF

                                'Delete destinationFile for overwrite,
                                'causes exception if already exists.
                                File.Delete(destinationFileHF)
                                File.Copy(sourceFileHF, destinationFileHF)
                            End If
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