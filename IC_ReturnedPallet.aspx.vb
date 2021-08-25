Partial Public Class IC_ReturnedPallet
    Inherits System.Web.UI.Page

    Public _caselabel As String
    Public _tobin As String
    Public _printer As String
    Public _user As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _product As String
    Public _productversion As String
    Public _component As String
    Public _mto As String
    Public _mto2 As String
    Public _mtoqty As Integer
    Public _mtoqty2 As Integer
    Public _mfgordqty As Integer
    Public _mfgordqty2 As Integer
    Public _status As String
    Public _qtyremain As Integer
    Public _prodid As Integer
    Public strURL As String = Nothing

    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString.ToString
    'Private sConnString As String = "server=SQL1;max pool size=300;user id=sa;password=buddig;database=Warehouse"
    Private sqlConn As New SqlClient.SqlConnection(sConnString)

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Reload values entered back to screen before reposting the page to scanner
        Me.txCaseLabel.Text = Common.GetVariable("CaseLabel", Page)
        Me.txQuantity.Text = Common.GetVariable("Quantity", Page)
        Me.txToBin.Text = Common.GetVariable("ToBin", Page)
        Me.txPrinter.Text = Common.GetVariable("Printer", Page)

        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        If Not strURL Is Nothing Then
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify the user is logged in 
        Common.CheckLogin(Page)
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Common.GetVariable("ScreenParam", Page).ToString = "CREATE" Then
            Me.lbPageTitle.Text = "OWS Inventory Mngmnt Create Missing Pallet Tag"

        ElseIf Common.GetVariable("ScreenParam", Page).ToString = "REPACK" Then
            Me.lbPageTitle.Text = "OWS Inventory Management Repack Product"
        Else
            Me.lbPageTitle.Text = "OWS Inventory Management Customer Return"
        End If

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
            'Prompt Error and Set Focus to Pallet TextBox
            If Me.txCaseLabel.Text.Length < 1 Then
                Common.SaveVariable("CaseLabel", "", Page)
                Me.lbError.Text = "Case Label needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Try
            Common.SaveVariable("Component", "", Page)
            Common.SaveVariable("CaseLabel", "", Page)
            Common.SaveVariable("Quantity", "", Page)
            Common.SaveVariable("ToBin", "", Page)
            Common.SaveVariable("Printer", "", Page)
            Common.SaveVariable("Status", "", Page)
            Common.SaveVariable("QtyRemain", "", Page)
            Common.SaveVariable("MTOException", "", Page)
            Common.SaveVariable("TotalPackaging", "", Page)

            Me.lbCaseLabel.Visible = True
            Me.txCaseLabel.Visible = True
            _caselabel = ""

            Me.txQuantity.Visible = False
            Me.lbQuantity.Visible = False
            Me.lbQuantity.Text = "Case Qty :"
            Me.txToBin.Visible = False
            Me.lbToBin.Visible = False
            Me.txPrinter.Visible = False
            Me.lbPrinter.Visible = False
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            Me.lbPrompt.Text = "Scan or Enter Case Label"
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
        Catch ex As Exception
            lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
        End Try
    End Sub

    Public Function WriteTransactions()
        'Write Transaction Record to IC_Trans
        Try
            _datemodified = Now()
            _dateentered = Now()


            If Common.GetVariable("ScreenParam", Page).ToString = "CREATE" Then
                '_status = "A"
                _status = UCase(Common.GetVariable("Status", Page).ToString)
                _trantype = "RETURN"
            ElseIf Common.GetVariable("ScreenParam", Page).ToString = "REPACK" Then
                _status = UCase(Common.GetVariable("Status", Page).ToString)
                _trantype = "REPACK"
            Else
                _status = "Q"
                _trantype = "RETURN"
            End If

            'End and dispose an active connection if it exists to avoid error
            DB.KillSQLConnection()

            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdInsert As New System.Data.SqlClient.SqlCommand
                sqlCmdInsert = DB.NewSQLCommand("SQL.IC_TransInsert")
                If Not sqlCmdInsert Is Nothing Then
                    sqlCmdInsert.Parameters.AddWithValue("@TransactionType", _trantype)
                    sqlCmdInsert.Parameters.AddWithValue("@Pallet", "0")
                    sqlCmdInsert.Parameters.AddWithValue("@CaseLabel", Me.txCaseLabel.Text)
                    sqlCmdInsert.Parameters.AddWithValue("@Company", _company)
                    sqlCmdInsert.Parameters.AddWithValue("@PalletQty", CInt(Me.txQuantity.Text))
                    sqlCmdInsert.Parameters.AddWithValue("@StartTime", "0000")
                    sqlCmdInsert.Parameters.AddWithValue("@StopTime", "0000")
                    sqlCmdInsert.Parameters.AddWithValue("@ToBin", Me.txToBin.Text)
                    sqlCmdInsert.Parameters.AddWithValue("@UserID", Common.GetVariable("UserID", Page).ToString)
                    sqlCmdInsert.Parameters.AddWithValue("@Status", _status)
                    sqlCmdInsert.Parameters.AddWithValue("@DateEntered", _dateentered)
                    sqlCmdInsert.Parameters.AddWithValue("@DateModified", _datemodified)
                    sqlCmdInsert.Parameters.AddWithValue("@Printer", Common.GetVariable("Printer", Page).ToString)
                    sqlCmdInsert.Parameters.AddWithValue("@Warehouse", _whs)
                    sqlCmdInsert.Parameters.AddWithValue("@Version", Mid$(Me.txCaseLabel.Text, 42, 3))
                    sqlCmdInsert.Parameters.AddWithValue("@Processed", "N")
                    sqlCmdInsert.Parameters.AddWithValue("@ReprintLabel", "")
                    sqlCmdInsert.Parameters.AddWithValue("@Override", "")
                    sqlCmdInsert.ExecuteNonQuery()
                    sqlCmdInsert.Dispose() : sqlCmdInsert = Nothing
                    DB.KillSQLConnection()
                    Return True
                Else
                    DB.KillSQLConnection()
                    Return False
                End If
            Else
                DB.KillSQLConnection()
                Return False
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
            Return False
        Finally
            DB.KillSQLConnection()
        End Try
    End Function

    Protected Sub txCaseLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCaseLabel.TextChanged
        'Verify the Case Label - make sure GTIN, Version, Code Date, MFGORD# are valid 
        If Trim(Me.txCaseLabel.Text).Length = 44 Or Trim(Me.txCaseLabel.Text).Length = 38 Then
            'Reset Component Value 
            Common.SaveVariable("Component", "", Page)
            Common.SaveVariable("MTOException", "", Page)
            Common.SaveVariable("TotalPackaging", "", Page)

            Try
                'Verify product exists in the system with GTIN and Version
                If Not DB.MakeSQLConnection("Warehouse") Then
                    Dim sqlCmdGTIN As New System.Data.SqlClient.SqlCommand
                    sqlCmdGTIN = DB.NewSQLCommand("SQL.Query.GTINLookupProd")
                    If Not sqlCmdGTIN Is Nothing Then
                        sqlCmdGTIN.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
                        _product = sqlCmdGTIN.ExecuteScalar()
                        sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
                        DB.KillSQLConnection()
                    Else
                        Me.lbError.Text = "An error occurred during Product Lookup - try again, or bring scanner to supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                        DB.KillSQLConnection()
                        Exit Sub
                    End If
                Else
                    Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                    DB.KillSQLConnection()
                    Exit Sub
                End If

                If RTrim(_product).Length < 1 Then 'Gtin inside label is not in the database
                    Me.lbError.Text = "Invalid Product in barcode. See Supervisor!"
                    Me.lbError.Visible = True
                    'Save the bad case label to display on screen for correction
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Exit Sub

                Else
                    'GTIN has been verified ----- need to verify Product Version is valid in barcode
                    If Me.txCaseLabel.Text.Length = 44 Then 'Version exists in barcode

                        'End and dispose an active connection if it exists to avoid error
                        DB.KillSQLConnection()

                        If Not DB.MakeSQLConnection("Warehouse") Then
                            Dim sqlCmdVersion As New System.Data.SqlClient.SqlCommand
                            sqlCmdVersion = DB.NewSQLCommand("SQL.Query.VersionLookup")
                            If Not sqlCmdVersion Is Nothing Then
                                sqlCmdVersion.Parameters.AddWithValue("@ProductID", CInt(_product))
                                sqlCmdVersion.Parameters.AddWithValue("@Version", CInt(Mid$(Me.txCaseLabel.Text, 42, 3)))
                                _productversion = sqlCmdVersion.ExecuteScalar()
                                sqlCmdVersion.Dispose() : sqlCmdVersion = Nothing
                                DB.KillSQLConnection()
                            Else
                                Me.lbError.Text = "An error occurred during Version Lookup - try again, or bring scanner to supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                                DB.KillSQLConnection()
                                Exit Sub
                            End If
                        Else
                            Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If

                        If RTrim(_productversion).Length < 1 Then 'Product does not exist with version in case label 
                            Me.lbError.Text = "Invalid Product Version in barcode. See Supervisor!"
                            Me.lbError.Visible = True
                            'Save the bad case label to display on screen for correction
                            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                            Exit Sub
                        End If
                    Else
                        _productversion = "000"
                    End If

                    'Product Version is valid --- validate the code date
                    If IsDate(Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2)) = False Then
                        Me.lbError.Text = "CodeDate in barcode is not a valid date. If you are manually entering the case label try again, otherwise see a supervisor!"
                        Me.lbError.Visible = True
                        Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                        Exit Sub
                    End If

                    'If Product scanned is a component part --- qty up to 2000 will be allowed in Quantity Entered
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        Dim sqlCmdComp As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.ComponentLookup")
                        If Not sqlCmdComp Is Nothing Then
                            sqlCmdComp.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
                            _component = sqlCmdComp.ExecuteScalar()
                            sqlCmdComp.Dispose() : sqlCmdComp = Nothing
                            DB.KillSQLConnection()
                        Else
                            Me.lbError.Text = "An error occurred during Component Lookup - try again, or bring scanner to supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If
                    Else
                        Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                        DB.KillSQLConnection()
                        Exit Sub
                    End If

                    If RTrim(_component).Length < 1 Then 'Product Scanned is not a Component
                        Common.SaveVariable("Component", "", Page)
                    Else
                        Common.SaveVariable("Component", UCase(_component), Page)
                    End If

                    'If Product scanned is a Total Packaging part --- Quantity Entered will be allowed to exceed UI.Pallet.Qty.StdMax in web config
                    Dim sqlTotPack As String = "Select ProdID from IC_TotalPackagingProducts Where GTIN = '" & Mid$(Me.txCaseLabel.Text, 3, 14) & "'"
                    'If Not DB.MakeSQLConnection("Warehouse") Then
                    Dim sqlCmdTotPack As New System.Data.SqlClient.SqlCommand
                    sqlCmdTotPack.Connection = sqlConn
                    sqlCmdTotPack.CommandText = sqlTotPack
                    sqlConn.Open()
                    If Not sqlCmdTotPack Is Nothing Then
                        _prodid = sqlCmdTotPack.ExecuteScalar()
                        sqlCmdTotPack.Dispose() : sqlCmdTotPack = Nothing
                        DB.KillSQLConnection()
                    Else
                        Me.lbError.Text = "An error occurred during Component Lookup - try again, or bring scanner to supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                        DB.KillSQLConnection()
                        Exit Sub
                    End If
                    sqlConn.Close()
                    sqlConn.Dispose()

                    '    Else
                    '    Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                    '    Me.lbError.Visible = True
                    '    Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                    '    DB.KillSQLConnection()
                    '    Exit Sub
                    'End If

                    If _prodid = 0 Then 'Product Scanned is not a Total Packaging part
                        Common.SaveVariable("TotalPackaging", "", Page)
                        Me.lbQuantity.Text = "Case Qty :"
                    Else     'Product Scanned is a Total Packaging part
                        Common.SaveVariable("TotalPackaging", "Y", Page)
                        Me.lbQuantity.Text = "Pounds :"
                    End If


                    _mto = Nothing
                    _mtoqty = Nothing

                    'Determine MTO or MTS Pallet
                    Dim dsMTOMTS As New Data.DataSet
                    Dim sqlCmdMTOMTS As New System.Data.SqlClient.SqlCommand
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        sqlCmdMTOMTS = DB.NewSQLCommand("SQL.Query.MTOPalletLookup2")
                        If Not sqlCmdMTOMTS Is Nothing Then
                            sqlCmdMTOMTS.Parameters.AddWithValue("@MfgOrd", CInt(Mid$(Me.txCaseLabel.Text, 27, 6)))
                            dsMTOMTS = DB.GetDataSet(sqlCmdMTOMTS)
                            sqlCmdMTOMTS.Dispose() : sqlCmdMTOMTS = Nothing
                            DB.KillSQLConnection()
                            If dsMTOMTS.Tables(0).Rows.Count > 0 Then
                                _mto = dsMTOMTS.Tables(0).Rows(0).Item("MFGORD")
                                _mtoqty = dsMTOMTS.Tables(0).Rows(0).Item("Qty")
                            End If
                        Else
                            Me.lbError.Text = "Error occurred while connecting to database! Try again or see your supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If
                    Else
                        Me.lbError.Text = "Communication Error occurred while connecting to database! Try again or see your supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                        DB.KillSQLConnection()
                        Exit Sub
                    End If

                    If RTrim(_mto).Length < 1 Then 'Pallet is MTS and Status = MTS_Status from web config 
                        'Common.SaveVariable("Status", ConfigurationManager.AppSettings.Get("MTS_Status").ToString, Page)
                        Common.SaveVariable("Status", "A", Page)
                    Else
                        'Check to see if product scanned is in IC_MTO_ProdQtyException table 
                        Dim _prodexception As String = ""

                        If Not DB.MakeSQLConnection("Warehouse") Then
                            Dim sqlCmdMTOException As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.MTOProdQtyExceptionLookup")
                            If Not sqlCmdMTOException Is Nothing Then
                                sqlCmdMTOException.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
                                sqlCmdMTOException.Parameters.AddWithValue("@Version", Mid$(Me.txCaseLabel.Text, 42, 3))
                                _prodexception = sqlCmdMTOException.ExecuteScalar()
                                sqlCmdMTOException.Dispose() : sqlCmdMTOException = Nothing
                                DB.KillSQLConnection()
                            Else
                                Me.lbError.Text = "An error occurred during MTO Exception Lookup - try again, or bring scanner to supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                                DB.KillSQLConnection()
                                Exit Sub
                            End If
                        Else
                            Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If

                        If RTrim(_prodexception).Length < 1 Then 'Product Scanned is not a MTO ProdQtyException and should be checked for quantities
                            Common.SaveVariable("MTOException", "NO", Page)
                            'Check to see if MTO has already been scanned into the system.
                            _mfgordqty = Nothing
                            Dim dsMTOMfgOrdQty As New Data.DataSet
                            Dim sqlCmdMTOMfgOrdQty As New System.Data.SqlClient.SqlCommand
                            If Not DB.MakeSQLConnection("Warehouse") Then
                                sqlCmdMTOMfgOrdQty = DB.NewSQLCommand("SQL.Query.MTOMfgOrdQty")
                                If Not sqlCmdMTOMfgOrdQty Is Nothing Then
                                    sqlCmdMTOMfgOrdQty.Parameters.AddWithValue("@MfgOrd", _mto)
                                    'dsMTOMfgOrdQty = DB.GetDataSet(sqlCmdMTOMfgOrdQty)
                                    _mfgordqty = sqlCmdMTOMfgOrdQty.ExecuteScalar()
                                    sqlCmdMTOMfgOrdQty.Dispose() : sqlCmdMTOMfgOrdQty = Nothing
                                    DB.KillSQLConnection()
                                    If Not _mfgordqty = Nothing Then
                                        'Compare qty scheduled to qty already in file
                                        If _mfgordqty >= _mtoqty Then
                                            Me.lbError.Text = "MFGORD # already completed at shrink wrapper. Please see supervisor so case labels can be corrected."
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                                            Exit Sub
                                        Else
                                            Common.SaveVariable("QtyRemain", _mtoqty - _mfgordqty, Page)
                                        End If
                                    Else
                                        Common.SaveVariable("QtyRemain", _mtoqty, Page)
                                    End If
                                Else
                                    Me.lbError.Text = "Error occurred while connecting to database! Try again or see your supervisor."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                                    DB.KillSQLConnection()
                                    Exit Sub
                                End If
                            Else
                                Me.lbError.Text = "Communication Error occurred while connecting to database! Try again or see your supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                                DB.KillSQLConnection()
                                Exit Sub
                            End If
                            If Not sqlCmdMTOMfgOrdQty Is Nothing Then
                                sqlCmdMTOMfgOrdQty.Dispose() : sqlCmdMTOMfgOrdQty = Nothing
                            End If
                        End If
                        Common.SaveVariable("Status", "C", Page)
                    End If

                    'Product, Version, CodeDate, Component YN , Total Packaging YN, MTO ProdQtyException YN - have been verified - Pass process onto Quantity Entry
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Me.lbError.Text = ""
                    Me.lbError.Visible = False
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)
                End If
            Catch ex As Exception
                Me.lbError.Text = "An error occurred during CaseLabel validation - Press the Restart Entry button."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
                DB.KillSQLConnection()
                Exit Sub
            End Try

        Else
            'Case label scanned is not valid length
            Me.lbError.Text = "Invalid Barcode Label - see supervisor!"
            Me.lbError.Visible = True
            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
            Exit Sub
        End If

    End Sub

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        'Verify the Qty does not exceed 500 unless it is a component product then Qty allowed up to 2000
        Try
            'Verify the Qty does not exceed 500 unless it is a component product then Qty allowed up to 2000
            If IsNumeric(Me.txQuantity.Text) = True Then
                If RTrim(UCase(Common.GetVariable("Component", Page).ToString)) = "COMP" Then
                    If CInt(Me.txQuantity.Text) <= CInt(ConfigurationManager.AppSettings.Get("UI.Pallet.Qty.ComponentMax").ToString) Then
                        'Qty entered is valid for component part
                        Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                    Else 'Qty exceeds maximum for component part
                        Me.lbError.Text = "Quantity entered exceeds maximum for Component Product. Try again."
                        Me.lbError.Visible = True
                        Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                        Common.JavaScriptSetFocus(Page, Me.txQuantity)
                        Exit Sub
                    End If
                Else 'Not a component part
                    If Common.GetVariable("Status", Page).ToString = "C" And Common.GetVariable("MTOException", Page).ToString = "NO" Then
                        _qtyremain = Nothing
                        If Not IsDBNull(Common.GetVariable("QtyRemain", Page).ToString) Then
                            _qtyremain = CInt(Common.GetVariable("QtyRemain", Page))
                            If CInt(Me.txQuantity.Text) > _qtyremain Then
                                Me.lbError.Text = "Quantity entered exceeds quantity scheduled for this MTO. Please see supervisor regarding this pallet."
                                Me.lbError.Visible = True
                                Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                                Common.JavaScriptSetFocus(Page, Me.txQuantity)
                                Exit Sub
                            End If
                        End If
                    End If

                    If CInt(Me.txQuantity.Text) <= CInt(ConfigurationManager.AppSettings.Get("UI.Pallet.Qty.StdMax").ToString) Then
                        'Qty is OK for non component part continue process to Start Time Entry
                        Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                    Else 'Qty exceeds maximum for Product
                        If Common.GetVariable("TotalPackaging", Page) = "Y" Then 'Product is Total Packaging - allow quantity entered
                            Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                            Me.lbError.Text = ""
                            Me.lbError.Visible = False
                        Else
                            Me.lbError.Text = "Quantity entered exceeds maximum for Product. Try again."
                            Me.lbError.Visible = True
                            Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txQuantity)
                            Exit Sub
                        End If
                    End If
                End If
                Me.txToBin.Visible = True
                Me.lbToBin.Visible = True
                Me.lbPrompt.Text = "Enter Bin Location"
                Common.JavaScriptSetFocus(Page, Me.txToBin)

            Else
                Me.lbError.Text = "Quantity entered must be numeric. Try again."
                Me.lbError.Visible = True
                Common.SaveVariable("Quantity", "", Page)
                Common.JavaScriptSetFocus(Page, Me.txQuantity)
            End If
        Catch ex As Exception
            Me.lbError.Text = "An error occurred during Quantity validation - Try again."
            Me.lbError.Visible = True
            Common.SaveVariable("Quantity", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txQuantity)
        End Try

    End Sub

    Protected Sub txToBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txToBin.TextChanged
        If Me.txToBin.Text.Length < 1 Then
            Exit Sub
        End If

        Try
            'Verify the Bin Location entered is valid in database
            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
                sqlCmdBin = DB.NewSQLCommand("SQL.Query.BinLookup")
                If Not sqlCmdBin Is Nothing Then
                    sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txToBin.Text)
                    _tobin = sqlCmdBin.ExecuteScalar()
                    sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                    DB.KillSQLConnection()
                Else
                    Me.lbError.Text = "A command error occurred during Bin Lookup - try again, or bring scanner to supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txToBin)
                    DB.KillSQLConnection()
                    Exit Sub
                End If
            Else
                Me.lbError.Text = "Connection to the database failed - try again, or bring scanner to supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txToBin)
                DB.KillSQLConnection()
                Exit Sub
            End If

            If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
                Me.lbError.Text = "Invalid Bin Location entered. Try again."
                Me.lbError.Visible = True
                'Save the bad Bin Location to display on screen for correction
                Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txToBin)
            Else 'Bin Location is valid
                Common.SaveVariable("ToBin", UCase(Me.txToBin.Text), Page)
                Me.txPrinter.Visible = True
                Me.lbPrinter.Visible = True
                Me.lbPrompt.Text = "Enter Printer #"
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Me.lbError.Text = ""
                Me.lbError.Visible = False
            End If
        Catch ex As Exception
            Me.lbError.Text = "An error occurred during Bin Lookup - try again, or bring scanner to supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txToBin)

        Finally
            DB.KillSQLConnection()
        End Try

    End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        If Me.txPrinter.Text.Length < 1 Then
            Exit Sub
        End If

        Try
            'Verify printer # entered is for correct location and is numeric
            If IsNumeric(Me.txPrinter.Text) Then
                If Not DB.MakeSQLConnection("Warehouse") Then
                    Dim sqlCmdPrinter As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.PrinterLookup")
                    If Not sqlCmdPrinter Is Nothing Then
                        sqlCmdPrinter.Parameters.AddWithValue("@Printer", CInt(Me.txPrinter.Text))
                        _printer = sqlCmdPrinter.ExecuteScalar()
                        sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                        DB.KillSQLConnection()
                    Else
                        DB.KillSQLConnection()
                        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                        Me.lbError.Text = "Error occurred while validating Printer - try again or see your supervisor!"
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                        Exit Sub
                    End If
                Else
                    DB.KillSQLConnection()
                    Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                    Me.lbError.Text = "Communication Error occurred while validating Printer - try again or see your supervisor!"
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Exit Sub
                End If

                If RTrim(_printer).Length < 1 Then 'Printer does not exist in database
                    Me.lbError.Text = "Invalid Printer # entered. Try again."
                    Me.lbError.Visible = True
                    'Save the bad Bin Location to display on screen for correction
                    Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Exit Sub
                Else 'Printer is valid
                    Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                    Dim txStatus As String = Common.GetVariable("Status", Page).ToString
                    If RTrim(txStatus) = "C" And Common.GetVariable("MTOException", Page).ToString = "NO" Then
                        'Check one last time for completed entries on MTO before write record to database.
                        _mto2 = Nothing
                        _mtoqty2 = Nothing

                        'Get MTO scheduled quantity
                        Dim dsMTOMTS2 As New Data.DataSet
                        Dim sqlCmdMTOMTS2 As New System.Data.SqlClient.SqlCommand
                        If Not DB.MakeSQLConnection("Warehouse") Then
                            sqlCmdMTOMTS2 = DB.NewSQLCommand("SQL.Query.MTOPalletLookup2")
                            If Not sqlCmdMTOMTS2 Is Nothing Then
                                sqlCmdMTOMTS2.Parameters.AddWithValue("@MfgOrd", CInt(Mid$(Me.txCaseLabel.Text, 27, 6)))
                                dsMTOMTS2 = DB.GetDataSet(sqlCmdMTOMTS2)
                                sqlCmdMTOMTS2.Dispose() : sqlCmdMTOMTS2 = Nothing
                                DB.KillSQLConnection()
                                If dsMTOMTS2.Tables(0).Rows.Count > 0 Then
                                    _mto2 = dsMTOMTS2.Tables(0).Rows(0).Item("MFGORD")
                                    _mtoqty2 = dsMTOMTS2.Tables(0).Rows(0).Item("Qty")
                                End If
                            Else
                                Me.lbError.Text = "Error occurred while connecting to database! Try again or see your supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                                DB.KillSQLConnection()
                                Exit Sub
                            End If
                        Else
                            Me.lbError.Text = "Communication Error occurred while connecting to database! Try again or see your supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If
                        'Check again to see if MTO has already been scanned into the system.
                        _mfgordqty2 = Nothing

                        Dim dsMTOMfgOrdQty2 As New Data.DataSet
                        Dim sqlCmdMTOMfgOrdQty2 As New System.Data.SqlClient.SqlCommand
                        If Not DB.MakeSQLConnection("Warehouse") Then
                            sqlCmdMTOMfgOrdQty2 = DB.NewSQLCommand("SQL.Query.MTOMfgOrdQty")
                            If Not sqlCmdMTOMfgOrdQty2 Is Nothing Then
                                sqlCmdMTOMfgOrdQty2.Parameters.AddWithValue("@MfgOrd", _mto2)
                                'dsMTOMfgOrdQty = DB.GetDataSet(sqlCmdMTOMfgOrdQty)
                                _mfgordqty2 = sqlCmdMTOMfgOrdQty2.ExecuteScalar()
                                sqlCmdMTOMfgOrdQty2.Dispose() : sqlCmdMTOMfgOrdQty2 = Nothing
                                DB.KillSQLConnection()
                                If Not _mfgordqty2 = Nothing Then
                                    'Compare qty scheduled to qty already in file
                                    If _mfgordqty2 >= _mtoqty2 Then
                                        Me.lbError.Text = "MFGORD # already completed at shrink wrapper. Please see supervisor so case labels can be corrected."
                                        Me.lbError.Visible = True
                                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                                        Exit Sub
                                    Else
                                        Common.SaveVariable("QtyRemain", _mtoqty2 - _mfgordqty2, Page)
                                    End If
                                Else
                                    Common.SaveVariable("QtyRemain", _mtoqty2, Page)
                                End If
                            Else
                                Me.lbError.Text = "Error occurred while connecting to database! Try again or see your supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                                DB.KillSQLConnection()
                                Exit Sub
                            End If
                        Else
                            Me.lbError.Text = "Communication Error occurred while connecting to database! Try again or see your supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            DB.KillSQLConnection()
                            Exit Sub
                        End If
                        If Not sqlCmdMTOMfgOrdQty2 Is Nothing Then
                            sqlCmdMTOMfgOrdQty2.Dispose() : sqlCmdMTOMfgOrdQty2 = Nothing
                        End If
                    End If

                    If WriteTransactions() = True Then
                        Call InitProcess()
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "An error occurred during Printer validation - Try again."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPrinter)
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