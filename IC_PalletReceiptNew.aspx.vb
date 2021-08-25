Partial Public Class IC_PalletReceiptNew
    Inherits System.Web.UI.Page

    Public _productdescription As String
    Public _maxnumlayers As String
    Public _layer As String
    Public _stdqty As String
    Public _prodtype As String
    Public _tobin As String
    Public _trantype As String
    Public _pallet As String
    Public _quantity As String
    Public _gtin As String
    Public _version As String
    Public _status As String
    Public _gemdbkey As String
    Public _whs As String
    Public _company As String
    Public _code As String
    Public strURL As String = Nothing
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _prodid As String
    Public _rossprodqty As String

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Reload values entered back to screen before reposting the page to scanner
        Me.txPallet.Text = Common.GetVariable("Pallet", Page)
        Me.txFullLayers.Text = Common.GetVariable("LayersEntered", Page)
        Me.txLooseBoxes.Text = Common.GetVariable("LooseCasesEntered", Page)
        'Me.txToBin.Text = Common.GetVariable("ToBin", Page)
        Me.lbProdDesc.Text = Common.GetVariable("ProductDescription", Page)

        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        If Not strURL Is Nothing Then
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify the user is logged in - if the Logged in Session Variable is Nothing then redirect to Log in Page "~/"
        'Common.CheckLogin2(Page, "Pallet Receipt New")
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page)
        Me.lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox - Display error
            If Me.lbProdDesc.Visible = False And RTrim(Me.txPallet.Text).Length < 1 Then
                Me.lbError.Text = "Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Try
            'Clear Session variables for this page
            Common.SaveVariable("Pallet", "", Page)
            Common.SaveVariable("ProductDescription", "", Page)
            Common.SaveVariable("Quantity", "", Page)
            Common.SaveVariable("Status", "", Page)
            Common.SaveVariable("GTIN", "", Page)
            Common.SaveVariable("Version", "", Page)
            Common.SaveVariable("StdQty", "", Page)
            Common.SaveVariable("Layer", "", Page)
            Common.SaveVariable("MaxNumLayers", "", Page)
            Common.SaveVariable("Component", "", Page)
            Common.SaveVariable("LayersEntered", "", Page)
            Common.SaveVariable("LooseCasesEntered", "", Page)
            Common.SaveVariable("LayersCaseTotal", "", Page)
            Common.SaveVariable("LooseCaseTotal", "", Page)
            Common.SaveVariable("ShippingCaseTotal", "", Page)
            'Common.SaveVariable("ToBin", "", Page)
            Common.SaveVariable("TotalPackaging", "", Page)
            Common.SaveVariable("QtyView", "", Page)

            Me.txFullLayers.Visible = False
            Me.txFullLayers.Text = ""
            Me.lbFullLayers.Visible = False
            Me.txLooseBoxes.Visible = False
            Me.txLooseBoxes.Text = ""
            Me.lbLooseBoxes.Visible = False
            'Me.txToBin.Visible = False
            'Me.txToBin.Text = ""
            Me.lbToBin.Visible = False
            Me.lbProdDesc.Visible = False
            Me.txPallet.Text = ""
            Me.lbCaseLabel.Visible = False
            Me.txCaseLabel.Text = ""
            Me.txCaseLabel.Visible = False


            'Clear Member variables for this page
            _pallet = Nothing
            _tobin = Nothing
            _productdescription = Nothing
            _trantype = Nothing
            _dateentered = Nothing
            _datemodified = Nothing
            _quantity = Nothing
            _status = Nothing
            _prodtype = Nothing

            Me.lbResult.Text = ""
            Me.lbResult.Visible = False

            Me.lbError.Text = ""
            Me.lbError.Visible = False

            Me.btRestart.Text = "Restart Entry"

            Me.lbPrompt.Text = "Scan or Enter Pallet#"
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Catch ex As Exception
            'If ex.InnerException Is Nothing Then
            '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:InitProcess - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'Else
            '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:InitProcess - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'End If

            lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        End Try
    End Sub

    Public Function WriteTransactions()
        Dim rtnVal As Boolean = False
        Dim sqlCmdInsert As New System.Data.SqlClient.SqlCommand
        'Write Transaction Record to IC_Trans
        Try
            Dim iSCT As Integer = CInt(Common.GetVariable("ShippingCaseTotal", Page).ToString)
            Dim _ver As String = Nothing
            _datemodified = Now()
            _dateentered = Now()
            _trantype = "PLTRCV"
            If Common.GetVariable("Version", Page).Length < 1 Then
                _ver = " "
            Else
                _ver = Common.GetVariable("Version", Page)
            End If

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdInsert = DB.NewSQLCommand("SQL.IC_TransInsert")
                If Not sqlCmdInsert Is Nothing Then
                    sqlCmdInsert.Parameters.AddWithValue("@TransactionType", _trantype)
                    sqlCmdInsert.Parameters.AddWithValue("@Pallet", Me.txPallet.Text)
                    sqlCmdInsert.Parameters.AddWithValue("@CaseLabel", Common.GetVariable("GTIN", Page).ToString)
                    sqlCmdInsert.Parameters.AddWithValue("@Company", _company)
                    sqlCmdInsert.Parameters.AddWithValue("@PalletQty", iSCT)
                    sqlCmdInsert.Parameters.AddWithValue("@StartTime", "")
                    sqlCmdInsert.Parameters.AddWithValue("@StopTime", "")
                    sqlCmdInsert.Parameters.AddWithValue("@ToBin", "WHSE")
                    sqlCmdInsert.Parameters.AddWithValue("@UserID", Common.GetVariable("UserID", Page).ToString)
                    sqlCmdInsert.Parameters.AddWithValue("@Status", Common.GetVariable("Status", Page).ToString)
                    sqlCmdInsert.Parameters.AddWithValue("@DateEntered", _dateentered)
                    sqlCmdInsert.Parameters.AddWithValue("@DateModified", _datemodified)
                    sqlCmdInsert.Parameters.AddWithValue("@Printer", "")
                    sqlCmdInsert.Parameters.AddWithValue("@Warehouse", _whs)
                    sqlCmdInsert.Parameters.AddWithValue("@Version", _ver)
                    sqlCmdInsert.Parameters.AddWithValue("@Processed", "N")
                    sqlCmdInsert.Parameters.AddWithValue("@ReprintLabel", "")
                    sqlCmdInsert.Parameters.AddWithValue("@Override", "")
                    sqlCmdInsert.ExecuteNonQuery()
                    sqlCmdInsert.Dispose() : sqlCmdInsert = Nothing
                    DB.KillSQLConnection()
                    rtnVal = True
                Else
                    rtnVal = False
                End If
            Else
                rtnVal = False
            End If
        Catch ex As Exception
            rtnVal = False
        Finally
            If Not sqlCmdInsert Is Nothing Then
                sqlCmdInsert.Dispose() : sqlCmdInsert = Nothing
            End If
            DB.KillSQLConnection()
        End Try
        Return rtnVal
    End Function

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPalRecInfo As New Data.DataSet
        Dim sqlCmdPalRecInfo As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Int64

            If RTrim(Me.txPallet.Text).Length > 0 Then 'Pallet # was entered - check numeric
                If IsNumeric(Me.txPallet.Text) = True Then
                    Try
                        iPallet = CLng(Me.txPallet.Text)
                    Catch ex As Exception
                        'If ex.InnerException Is Nothing Then
                        '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:PalletChg - " & Me.txPallet.Text & " - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
                        'Else
                        '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:PalletChg - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
                        'End If

                        Me.lbError.Text = "Invalid entry made for Pallet #."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                    End Try

                Else
                    Me.lbError.Text = "Pallet # needs to be numbers."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                    Exit Sub
                End If
            Else
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."

                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Sub
            End If

            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            'Validate pallet in vwPalletsReceiptLookup - get Status,ProductDescription,GTIN,Qty,Ver,StdQty,Layer,MaxNumLayers,ProdType,Gem_dbkey
            If Not DB.MakeSQLConnection("Warehouse") Then

                sqlCmdPalRecInfo = DB.NewSQLCommand("SQL.Query.GetPalletReceiptInfo")
                If Not sqlCmdPalRecInfo Is Nothing Then
                    sqlCmdPalRecInfo.Parameters.AddWithValue("@Pallet", iPallet)
                    dsPalRecInfo = DB.GetDataSet(sqlCmdPalRecInfo)
                    sqlCmdPalRecInfo.Dispose() : sqlCmdPalRecInfo = Nothing
                    DB.KillSQLConnection()
                Else
                    Me.lbError.Text = "An error occurred during Pallet Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                    DB.KillSQLConnection()
                    Exit Sub
                End If
            Else
                'Throw New Exception("""Connection not established""")
                Me.lbError.Text = "Connection to the database failed - Press Restart Entry button to try again, or bring scanner to supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                DB.KillSQLConnection()
                Exit Sub
            End If

            If dsPalRecInfo Is Nothing Then 'No dataset retreived
                Me.lbError.Text = "A communications error occurred during Pallet Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Try
            ElseIf dsPalRecInfo.Tables.Count > 0 Then 'Dataset was returned - check 
                If dsPalRecInfo.Tables(0).Rows.Count > 0 Then 'Record exists for Pallet get Pallet Info Needed
                    _pallet = iPallet
                    _quantity = dsPalRecInfo.Tables(0).Rows(0).Item("Qty")
                    _rossprodqty = dsPalRecInfo.Tables(0).Rows(0).Item("RP_Qty")
                    _gtin = dsPalRecInfo.Tables(0).Rows(0).Item("GTIN")
                    _version = dsPalRecInfo.Tables(0).Rows(0).Item("Ver")
                    _status = dsPalRecInfo.Tables(0).Rows(0).Item("Status")
                    _productdescription = dsPalRecInfo.Tables(0).Rows(0).Item("strProductDescription")
                    _prodtype = dsPalRecInfo.Tables(0).Rows(0).Item("ProdType")
                    If dsPalRecInfo.Tables(0).Rows(0).Item("ProdID") = 0 Then
                        _prodid = ""
                    Else
                        _prodid = dsPalRecInfo.Tables(0).Rows(0).Item("ProdID")
                    End If

                    If RTrim(UCase(_prodtype)) = "COMP" Then
                        _stdqty = "0"
                        _layer = "0"
                        _maxnumlayers = "0"
                    Else 'Not a component - get actual stdqty,layer, and maxnumlayers
                        If IsDBNull(dsPalRecInfo.Tables(0).Rows(0).Item("StdQty").ToString) Or RTrim(dsPalRecInfo.Tables(0).Rows(0).Item("StdQty").ToString).Length < 1 Then
                            _stdqty = "0"
                        Else
                            _stdqty = dsPalRecInfo.Tables(0).Rows(0).Item("StdQty").ToString
                        End If

                        If IsDBNull(dsPalRecInfo.Tables(0).Rows(0).Item("Layer").ToString) Or RTrim(dsPalRecInfo.Tables(0).Rows(0).Item("Layer").ToString).Length < 1 Then
                            _layer = "0"
                        Else
                            _layer = dsPalRecInfo.Tables(0).Rows(0).Item("Layer").ToString
                        End If

                        If IsDBNull(dsPalRecInfo.Tables(0).Rows(0).Item("MaxNumLayers").ToString) Or RTrim(dsPalRecInfo.Tables(0).Rows(0).Item("MaxNumLayers").ToString).Length < 1 Then
                            _maxnumlayers = "0"
                        Else
                            _maxnumlayers = dsPalRecInfo.Tables(0).Rows(0).Item("MaxNumLayers") + 2.ToString
                        End If
                    End If

                    If IsDBNull(dsPalRecInfo.Tables(0).Rows(0).Item("GEM_DBKEY")) = False Then
                        _gemdbkey = dsPalRecInfo.Tables(0).Rows(0).Item("GEM_DBKEY")
                    Else
                        _gemdbkey = ""
                    End If

                    If _gemdbkey <> "" Then  'Pallet was processed in shipping receipt - show error 
                        Me.lbError.Text = "Pallet#" & _pallet & "was already processed, have a supervisor lookup pallet location or perform a Pallet Lookup."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                    Else 'Record does not exist for Pallet entered and it is ok for process to continue
                        Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                        Common.SaveVariable("Quantity", _quantity, Page)
                        Common.SaveVariable("RossProdQty", _rossprodqty, Page)
                        Common.SaveVariable("GTIN", _gtin, Page)
                        Common.SaveVariable("Version", _version, Page)
                        Common.SaveVariable("ProductDescription", _productdescription, Page)
                        Common.SaveVariable("StdQty", _stdqty, Page)
                        Common.SaveVariable("Layer", _layer, Page)
                        Common.SaveVariable("MaxNumLayers", _maxnumlayers, Page)
                        Common.SaveVariable("Component", UCase(_prodtype), Page)
                        Common.SaveVariable("Status", _status, Page)

                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                        If RTrim(UCase(_prodid)).Length > 0 Then
                            'Product is a Total Packaging Part - Enter Lbs in Tub
                            Common.SaveVariable("TotalPackaging", "Y", Page)
                            Common.SaveVariable("QtyView", "LBS", Page)

                        ElseIf RTrim(UCase(_prodtype)) = "COMP" Or _layer.ToString = "0" Or _layer.Length < 1 Then
                            'Product is either a Component Part or does not have a layer configuration in table
                            Common.SaveVariable("QtyView", "CASE", Page)
                        Else
                            'Product not a component - not a tot pack prod and has Layers Configured
                            Common.SaveVariable("QtyView", "LAYER", Page)
                        End If

                        Dim iSlicingQty As Integer = CInt(_quantity)
                        Dim iRossProductionQty As Integer = CInt(_rossprodqty)

                        If iSlicingQty <> iRossProductionQty Then
                            Me.lbError.Text = "KnowledgeMine Qty does not match Pallet Qty . Please return pallet to slicing."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                            Exit Sub
                        End If

                        Me.lbCaseLabel.Visible = True
                        Me.txCaseLabel.Text = ""
                        Me.txCaseLabel.Visible = True
                        Me.lbPrompt.Text = "Scan a Case Label barcode on the Pallet."

                        'Changed code to prompt user for CaseLabel scan
                        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)

                    End If
                Else 'Record does not exist for Pallet entered
                    Me.lbError.Text = "Pallet# not created in Slicing Department - see supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                    Exit Sub
                End If
            Else
                Me.lbError.Text = "Pallet# did not return dataset. Try process again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If

        Catch sqlex As SqlClient.SqlException
            'If sqlex.InnerException Is Nothing Then
            '    Common.LogError(sqlex.Message, "", sqlex.Source, "PalletReceipt:PalletChg - ", Page, Me.lbPageTitle.Text, sqlex.StackTrace)
            'Else
            '    Common.LogError(sqlex.Message, sqlex.InnerException.Message, sqlex.Source, "PalletReceipt:PalletChg - ", Page, Me.lbPageTitle.Text, sqlex.StackTrace)
            'End If

            Me.lbError.Text = "Exception " & sqlex.Message.ToString & " occurred during Pallet validation - Press Restart Entry button to try again, or bring scanner to supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)

        Catch ex As Exception
            'If ex.InnerException Is Nothing Then
            '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:PalletChg - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'Else
            '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:PalletChg - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'End If

            Me.lbError.Text = "Exception " & ex.Message.ToString & " occurred during Pallet validation - Press Restart Entry button to try again, or bring scanner to supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
            'add code to write exception to error log text file
        Finally
            If Not dsPalRecInfo Is Nothing Then
                dsPalRecInfo.Dispose() : dsPalRecInfo = Nothing
            End If
            If Not sqlCmdPalRecInfo Is Nothing Then
                sqlCmdPalRecInfo.Dispose() : sqlCmdPalRecInfo = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txCaseLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCaseLabel.TextChanged
        Dim strGTIN As String = Common.GetVariable("GTIN", Page)
        Dim strVersion As String = Common.GetVariable("Version", Page)
        Dim strProductDesc As String = Common.GetVariable("ProductDescription", Page)
        Dim strQtyView As String = Common.GetVariable("QtyView", Page)


        If Trim(Me.txCaseLabel.Text).Length <> 44 Then
            Me.lbError.Text = "Barcode scanned is not from the Case Label.  Please scan again."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
            Exit Sub
        End If
        If Mid$(Me.txCaseLabel.Text, 3, 14) <> strGTIN Then
            Me.lbError.Text = "Case Label product does not match product on the Pallet - Return to Slicing."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
            Exit Sub
        Else
            If Mid$(Me.txCaseLabel.Text, 42, 3) <> strVersion Then
                Me.lbError.Text = "Case Label product version does not match product version on the Pallet - Return to Slicing."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                Exit Sub
            End If
        End If

        ' Pallet info matches Case Label info - ok to proceed
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Select Case strQtyView
            Case Is = "LBS"
                Me.lbFullLayers.Text = "Lbs in Tub-"
                Me.lbPrompt.Text = "Enter Lbs in Tub"
            Case Is = "CASE"
                Me.lbFullLayers.Text = "# of Cases-"
                Me.lbPrompt.Text = "Enter Case Qty - No Layers Configured"
            Case Is = "LAYER"
                Me.lbFullLayers.Text = "Full Layers-"
                Me.lbPrompt.Text = "Enter # of Full Layers on Pallet"
        End Select

        Me.lbProdDesc.Text = strProductDesc
        Me.lbProdDesc.Visible = True
        Me.txFullLayers.Visible = True
        Me.lbFullLayers.Visible = True
        Common.JavaScriptSetFocus(Page, Me.txFullLayers)

    End Sub
    Protected Sub txFullLayers_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txFullLayers.TextChanged
        'Verify the Qty does not exceed 500 unless it is a component product then Qty allowed up to 2000
        Try
            If Trim(Me.txFullLayers.Text).Length > 0 And IsNumeric(Me.txFullLayers.Text) = True Then
                Dim iFull As Integer = CInt(Me.txFullLayers.Text)
                Dim iSlicQty As Integer = CInt(Common.GetVariable("Quantity", Page).ToString)
                'Dim iRossProdQty As Integer = CInt(Common.GetVariable("RossProdQty", Page).ToString)
                Dim iMaxLayr As Integer = CInt(Common.GetVariable("MaxNumLayers", Page).ToString)
                If RTrim(UCase(Common.GetVariable("Component", Page).ToString)) = "COMP" Or Common.GetVariable("Layer", Page).Length < 1 Then
                    'Pallet contains component parts - compare entered value to Quantity variable
                    If iFull <> iSlicQty Then
                        'Qty entered in shipping does not match value entered in slicing
                        Me.lbError.Text = "Quantity entered does not match value entered in Slicing. See Supervisor."
                        Me.lbError.Visible = True
                        Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                        Common.JavaScriptSetFocus(Page, Me.txFullLayers)
                    Else 'Qty matches value entered in Slicing - ok to continue - bypass loose cases entry( value set to 0)
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False
                        Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                        Common.SaveVariable("LooseCaseEntered", "0", Page)
                        Common.SaveVariable("ShippingCaseTotal", Me.txFullLayers.Text, Page)
                        'Me.txToBin.Visible = True
                        'Me.lbToBin.Visible = True
                        'Me.lbPrompt.Text = "Scan or Enter Bin Location"
                        'Common.JavaScriptSetFocus(Page, Me.txToBin)
                        ProcessPallet()
                    End If
                Else
                    If Common.GetVariable("TotalPackaging", Page).ToString = "Y" Then
                        'Pallet contains Total Packaging part - compare entered value to Quantity variable
                        If iFull <> iSlicQty Then
                            'Qty entered in shipping does not match value entered in slicing
                            Me.lbError.Text = "Pounds entered does not match value entered in Slicing. See Supervisor."
                            Me.lbError.Visible = True
                            Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txFullLayers)
                        Else 'Pounds matches value entered in Slicing - ok to continue - bypass loose cases entry( value set to 0)
                            Me.lbError.Text = ""
                            Me.lbError.Visible = False
                            Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                            Common.SaveVariable("LooseCaseEntered", "0", Page)
                            Common.SaveVariable("ShippingCaseTotal", Me.txFullLayers.Text, Page)
                            'Me.txToBin.Visible = True
                            'Me.lbToBin.Visible = True
                            'Me.lbPrompt.Text = "Scan or Enter Bin Location"
                            'Common.JavaScriptSetFocus(Page, Me.txToBin)
                            ProcessPallet()
                        End If
                    Else
                        'Not a component part and not a Total Packaging part - check the layer calculations
                        If iFull > iMaxLayr Then
                            'Layers entered exceeds maximum - throw an error
                            Me.lbError.Text = "# of Layers entered exceeds the maximum. Try again or see supervisor."
                            Me.lbError.Visible = True
                            Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txFullLayers)
                        Else 'Layers entered are at or below maximum - ok to continue to Loose Case Entry
                            Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                            Me.lbError.Text = ""
                            Me.lbError.Visible = False
                            Me.txLooseBoxes.Visible = True
                            Me.lbLooseBoxes.Visible = True
                            Me.lbPrompt.Text = "Enter # of Loose Cases"
                            Common.JavaScriptSetFocus(Page, Me.txLooseBoxes)
                        End If
                    End If
                End If
            Else
                Me.lbError.Text = Me.lbFullLayers.Text & " entered must be numeric. Try again."
                Me.lbError.Visible = True
                Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txFullLayers)
            End If
        Catch ex As Exception
            'If ex.InnerException Is Nothing Then
            '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:Layers - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'Else
            '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:Layers - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'End If

            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during " & Me.lbFullLayers.Text & " validation. Try again."
            Me.lbError.Visible = True
            Common.SaveVariable("LayersEntered", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txFullLayers)
        End Try
    End Sub

    Protected Sub txLooseBoxes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txLooseBoxes.TextChanged
        'verify Loose Cases entry is numeric and does not exceed SaveVariable("Layer")
        Try
            If IsNumeric(Me.txLooseBoxes.Text) Then
                Dim iLoose As Integer = CInt(Me.txLooseBoxes.Text)
                Dim iLayerEnt As Integer = CInt(Common.GetVariable("LayersEntered", Page).ToString)
                Dim iLayerVal As Integer = CInt(Common.GetVariable("Layer", Page).ToString)
                Dim iSlicQty As Integer = CInt(Common.GetVariable("Quantity", Page).ToString)

                If iLoose >= iLayerVal Then
                    'Throw error informing associate to retry entry of loose boxes or Press button to Re-Enter Layers
                    Me.lbError.Text = "# of Loose Cases entered equal to or greater than Full Layer. Try again or Press button to Restart Entry."
                    Me.lbError.Visible = True
                    Common.SaveVariable("LooseCasesEntered", Me.txLooseBoxes.Text, Page)
                    Common.JavaScriptSetFocus(Page, Me.txLooseBoxes)
                Else
                    '# of Loose Cases is LT Layer and ok to continue process - Calc Case Total from LayersEntered * Layer + LooseCasesEntered
                    If ((iLayerVal * iLayerEnt) + iLoose) <> iSlicQty Then
                        'Qty Calc of Layers and Loose Cases entered in shipping does not match value entered in slicing
                        Me.lbError.Text = "Full Layers and Loose Cases entered do not match value entered in Slicing. See Supervisor or Press button to Restart Entry."
                        Me.lbError.Visible = True
                        Common.SaveVariable("LooseCasesEntered", Me.txLooseBoxes.Text, Page)
                        Common.JavaScriptSetFocus(Page, Me.txLooseBoxes)
                    Else 'Qty Calc of Layers and Loose Cases entered in shipping matches value entered in Slicing - ok to continue
                        Common.SaveVariable("LooseCasesEntered", Me.txLooseBoxes.Text, Page)
                        Common.SaveVariable("LayersEntered", Me.txFullLayers.Text, Page)
                        Common.SaveVariable("ShippingCaseTotal", iSlicQty, Page)
                        'Me.txToBin.Visible = True
                        'Me.lbToBin.Visible = True
                        'Me.lbPrompt.Text = "Scan or Enter Bin Location"
                        'Common.JavaScriptSetFocus(Page, Me.txToBin)
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False
                        ProcessPallet()
                    End If
                End If
            Else
                Me.lbError.Text = "# of Loose Cases entered must be numeric. Try again."
                Me.lbError.Visible = True
                Common.SaveVariable("LooseCasesEntered", Me.txLooseBoxes.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txLooseBoxes)
            End If
        Catch ex As Exception
            'If ex.InnerException Is Nothing Then
            '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:Loose Cases - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'Else
            '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:Loose Cases - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'End If

            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during Loose Cases validation. Try again."
            Me.lbError.Visible = True
            Common.SaveVariable("LooseCasesEntered", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txLooseBoxes)
        End Try
    End Sub

    Public Sub ProcessPallet()
        'Verify the Bin Location entered is valid in database
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If WriteTransactions() = True Then
                'Change button for Restart to read Next Pallet
                Me.lbResult.Text = "Pallet - " & Me.txPallet.Text & " with Quantity - " & Common.GetVariable("Quantity", Page).ToString() & " was valid and received into Bin Location - WHSE. Press Next Pallet button."
                Me.lbResult.Visible = True
                Me.btRestart.Text = "Next Pallet"
            Else
                Me.lbError.Text = "Transaction failed to complete. Check battery and Wireless connection - then press Restart Entry button to process pallet again."
                Me.lbError.Visible = True
                Exit Sub
            End If
        Catch ex As Exception
            'If ex.InnerException Is Nothing Then
            '    Common.LogError(ex.Message, "", ex.Source, "PalletReceipt:ProcessPallet - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'Else
            '    Common.LogError(ex.Message, ex.InnerException.Message, ex.Source, "PalletReceipt:ProcessPallet - ", Page, Me.lbPageTitle.Text, ex.StackTrace)
            'End If

            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            If Not sqlCmdBin Is Nothing Then
                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub


End Class