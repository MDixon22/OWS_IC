Partial Public Class IC_CreateInvPallet
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
        'Me.txCaseLabel.Text = Common.GetVariable("CaseLabel", Page)
        'Me.txQuantity.Text = Common.GetVariable("Quantity", Page)
        'Me.txToBin.Text = Common.GetVariable("ToBin", Page)
        'Me.txPrinter.Text = Common.GetVariable("Printer", Page)

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

            Me.lbCaseLabel.Visible = True
            Me.txCaseLabel.Visible = True
            _caselabel = ""
            Me.txCaseLabel.Text = ""

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


            _status = "A"
            _status = UCase(Common.GetVariable("Status", Page).ToString)
            _trantype = "RETURN"


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

                    Me.lbQuantity.Text = "Case Qty :"
                End If

                'Product, Version, CodeDate, Component YN , Total Packaging YN, MTO ProdQtyException YN - have been verified - Pass process onto Quantity Entry
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Me.lbError.Text = ""
                    Me.lbError.Visible = False
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)

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