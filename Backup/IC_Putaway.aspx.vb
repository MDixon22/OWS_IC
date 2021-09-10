Partial Public Class IC_Putaway
    Inherits System.Web.UI.Page

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
    Public _status As String
    Public strURL As String = Nothing

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Reload values entered back to screen before reposting the page to scanner
        Me.txCaseLabel.Text = Common.GetVariable("CaseLabel", Page)
        Me.txQuantity.Text = Common.GetVariable("Quantity", Page)
        Me.txStart.Text = Common.GetVariable("Start", Page)
        Me.txStop.Text = Common.GetVariable("Stop", Page)
        Me.txPrinter.Text = Common.GetVariable("Printer", Page)

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
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Common.GetVariable("ScreenParam", Page).ToString = "REPACK" Then
            Me.lbPageTitle.Text = "OW Inventory - Repack Product Putaway"
            Me.lbToBinName.Text = "OWRP"
        ElseIf Me.lbUser.Text = "WCFG" Then
            Me.lbPageTitle.Text = "OW Inventory - WC FG Product Putaway"
            Me.lbToBinName.Text = "WCFG"
        ElseIf _whs = "17" Then
            'Added to handle Montgomery FG Putaway for HF and Lot Checking
            Me.lbPageTitle.Text = "OW Inventory - Mont 9798 FG Putaway"
            Me.lbToBinName.Text = "OWFGM"
        Else
            Me.lbPageTitle.Text = "OW Inventory - FG Product Putaway"
            Me.lbToBinName.Text = "OWFG"
        End If


        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox = MTS
            'Show the Case Label Entry Function
            If Me.txCaseLabel.Visible = True And Me.txCaseLabel.Text.Length < 1 Then
                Me.lbPrompt.Text = "Scan or Enter Case Label"
                Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        'Clear Session variables for this page
        'Common.SaveVariable("Pallet", "", Page)
        Common.SaveVariable("Component", "", Page)
        Common.SaveVariable("CaseLabel", "", Page)
        Common.SaveVariable("Quantity", "", Page)
        Common.SaveVariable("Start", "", Page)
        Common.SaveVariable("Stop", "", Page)
        Common.SaveVariable("Printer", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        Me.txCaseLabel.Visible = True
        Me.lbCaseLabel.Visible = True
        Me.txQuantity.Visible = False
        Me.lbQuantity.Visible = False
        Me.txStart.Visible = False
        Me.lbStart.Visible = False
        Me.txStop.Visible = False
        Me.lbStop.Visible = False
        Me.lbToBinName.Visible = False
        Me.lbToBin.Visible = False
        Me.txPrinter.Visible = False
        Me.lbPrinter.Visible = False

        'Clear Member variables for this page
        _product = Nothing
        _productversion = Nothing
        _component = Nothing
        _tobin = Nothing
        _printer = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        _status = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbTimeDesc.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Case Label"
        Common.JavaScriptSetFocus(Page, Me.txCaseLabel)
    End Sub

    Public Function WriteTransactions()
        Dim rtnVal As Boolean
        Dim sqlCmdInsert As New System.Data.SqlClient.SqlCommand
        'Write Transaction Record to IC_Trans
        Try
            _datemodified = Now()
            _dateentered = Now()
            _trantype = "INSERT"
            _status = "A"

            Dim iQty As Integer = CInt(Me.txQuantity.Text)

            If iQty > 0 Then
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdInsert = DB.NewSQLCommand("SQL.IC_TransInsert")
                    If Not sqlCmdInsert Is Nothing Then
                        sqlCmdInsert.Parameters.AddWithValue("@TransactionType", _trantype)
                        sqlCmdInsert.Parameters.AddWithValue("@Pallet", 0)
                        sqlCmdInsert.Parameters.AddWithValue("@CaseLabel", Me.txCaseLabel.Text)
                        sqlCmdInsert.Parameters.AddWithValue("@Company", _company)
                        sqlCmdInsert.Parameters.AddWithValue("@PalletQty", iQty)
                        sqlCmdInsert.Parameters.AddWithValue("@StartTime", Me.txStart.Text)
                        sqlCmdInsert.Parameters.AddWithValue("@StopTime", Me.txStop.Text)
                        sqlCmdInsert.Parameters.AddWithValue("@ToBin", Me.lbToBinName.Text)
                        sqlCmdInsert.Parameters.AddWithValue("@UserID", UCase(Common.GetVariable("UserID", Page).ToString))
                        sqlCmdInsert.Parameters.AddWithValue("@Status", _status)
                        sqlCmdInsert.Parameters.AddWithValue("@DateEntered", _dateentered)
                        sqlCmdInsert.Parameters.AddWithValue("@DateModified", _datemodified)
                        sqlCmdInsert.Parameters.AddWithValue("@Printer", Common.GetVariable("Printer", Page).ToString)
                        sqlCmdInsert.Parameters.AddWithValue("@Warehouse", _whs)
                        If Me.txCaseLabel.Text.Length = 44 Then
                            sqlCmdInsert.Parameters.AddWithValue("@Version", Mid$(Me.txCaseLabel.Text, 42, 3))
                        Else
                            sqlCmdInsert.Parameters.AddWithValue("@Version", "")
                        End If

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

    Protected Sub txCaseLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCaseLabel.TextChanged
        Dim sqlCmdGTIN As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdVersion As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdComp As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            'Verify the Case Label - make sure GTIN, Code Date, MFGORD# are valid
            Dim iCL As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString)
            Dim iCL2 As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.AltCaseLength").ToString)

            If Me.txCaseLabel.Text.Length = iCL Or Me.txCaseLabel.Text.Length = iCL2 Then
                'Reset Component Value 
                Common.SaveVariable("Component", "", Page)

                'Verify product exists in the system with GTIN and Version
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdGTIN = DB.NewSQLCommand("SQL.Query.GTINLookupProd")
                    If Not sqlCmdGTIN Is Nothing Then
                        sqlCmdGTIN.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
                        _product = sqlCmdGTIN.ExecuteScalar()
                        sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
                        DB.KillSQLConnection()

                        If RTrim(_product).Length < 1 Then
                            Me.lbError.Text = "Invalid Product in barcode or Wireless Connection to database returned corrupted data! Check Battery and Wireless Connection - then try again or see your supervisor."
                            Me.lbError.Visible = True
                            'Save the bad case label to display on screen for correction
                            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                            Exit Try
                        ElseIf RTrim(_product).Length > 0 Then
                            Dim iProduct As Integer = CLng(_product)

                            'GTIN has been verified ----- need to verify Product Version is valid in barcode
                            If Me.txCaseLabel.Text.Length = iCL Then
                                Dim iVersion As Integer = CInt(Mid$(Me.txCaseLabel.Text, 42, 3))
                                If Not DB.MakeSQLConnection("Warehouse") Then
                                    sqlCmdVersion = DB.NewSQLCommand("SQL.Query.VersionLookup")
                                    If Not sqlCmdVersion Is Nothing Then
                                        sqlCmdVersion.Parameters.AddWithValue("@ProductID", iProduct)
                                        sqlCmdVersion.Parameters.AddWithValue("@Version", iVersion)
                                        _productversion = sqlCmdVersion.ExecuteScalar()
                                        sqlCmdVersion.Dispose() : sqlCmdVersion = Nothing
                                        DB.KillSQLConnection()

                                        If RTrim(_productversion).Length < 1 Then 'Product does not exist with version in case label 
                                            Me.lbError.Text = "Invalid Product/Version in barcode. See Supervisor!"
                                            Me.lbError.Visible = True
                                            'Save the bad case label to display on screen for correction
                                            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                                            Exit Try
                                        End If
                                    Else
                                        Me.lbError.Text = "Command Error occurred during version validation! Check Battery and Wireless Connection - then try again or see your supervisor."
                                        Me.lbError.Visible = True
                                        Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                                        sqlCmdVersion.Dispose() : sqlCmdVersion = Nothing
                                        DB.KillSQLConnection()
                                        Exit Try
                                    End If
                                Else
                                    Me.lbError.Text = "Communication Error occurred during version validation! Check Battery and Wireless Connection - then try again or see your supervisor."
                                    Me.lbError.Visible = True
                                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                                    Exit Try
                                End If
                            Else
                                _productversion = "000"
                            End If

                            Dim LotFull As String = ""
                            LotFull = CInt(Common.FindLotFromBarcode(Mid$(Me.txCaseLabel.Text, 27, 12), _whs, Mid$(Me.txCaseLabel.Text, 3, 14), CInt(Mid$(Me.txCaseLabel.Text, 42, 3)), Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2)))

                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while connecting to database to validate product! Check Battery and Wireless Connection - then try again or see your supervisor."
                        Me.lbError.Visible = True
                        Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                        sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
                        DB.KillSQLConnection()
                        Exit Try
                    End If
                Else
                    Me.lbError.Text = "Communication Error while connecting to database to validate product!  Check Battery and Wireless Connection - then try again or see your supervisor."
                    Me.lbError.Visible = True
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Exit Try
                End If
                'Product Version is valid --- validate the code date
                If IsDate(Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2)) = False Then
                    Me.lbError.Text = "CodeDate in barcode is not a valid date. If you are manually entering the case label try again, otherwise see a supervisor!"
                    Me.lbError.Visible = True
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Exit Try
                Else
                    'Product, Version, CodeDate, Component YN have been verified - Pass process onto Quantity Entry
                    Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
                    Me.txQuantity.Visible = True
                    Me.lbQuantity.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)
                End If
            Else
                'Case label scanned is not valid length
                Me.lbError.Text = "Invalid Barcode Label - see supervisor!"
                Me.lbError.Visible = True
                Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Case Label entered! Check Battery and Wireless Connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
        Finally
            If Not sqlCmdComp Is Nothing Then
                sqlCmdComp.Dispose() : sqlCmdComp = Nothing
            End If
            If Not sqlCmdVersion Is Nothing Then
                sqlCmdVersion.Dispose() : sqlCmdVersion = Nothing
            End If
            If Not sqlCmdGTIN Is Nothing Then
                sqlCmdGTIN.Dispose() : sqlCmdGTIN = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If Me.txQuantity.Text.Length > 0 Then
                Dim iQty As Integer = CInt(Me.txQuantity.Text)
                'Verify the Qty does not exceed 500 unless it is a component product then Qty allowed up to 2000
                If iQty > 0 Then
                    Common.SaveVariable("Quantity", Me.txQuantity.Text, Page)
                    Me.txStart.Visible = True
                    Me.lbStart.Visible = True
                    Me.lbTimeDesc.Visible = True

                    Me.lbPrompt.Text = "Enter pallet START time - HHMM"
                    Common.JavaScriptSetFocus(Page, Me.txStart)
                Else
                    Me.lbError.Text = "Quantity entered must be numeric. Try again."
                    Me.lbError.Visible = True
                    Common.SaveVariable("Quantity", "", Page)
                    Common.JavaScriptSetFocus(Page, Me.txQuantity)
                End If
            Else
                'Quantity is blank
                Me.lbError.Text = "Quantity can not be blank. Try again."
                Me.lbError.Visible = True
                Common.SaveVariable("Quantity", "", Page)
                Common.JavaScriptSetFocus(Page, Me.txQuantity)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Quantity entered! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
            Common.SaveVariable("Quantity", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txQuantity)
        End Try
    End Sub

    Protected Sub txStart_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txStart.TextChanged
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify the start time entered length is 4 and can be cast to Time
            Dim iStartLen As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.StartTime.Length").ToString)

            If Me.txStart.Text.Length = iStartLen Then
                Dim dStart As DateTime = Nothing

                If DateTime.TryParse(Mid(Me.txStart.Text, 1, 2) & ":" & Mid(Me.txStart.Text, 3, 2), dStart) = True Then
                    'Start Time is valid continue to Process Stop Time
                    Common.SaveVariable("Start", Me.txStart.Text, Page)
                    Me.txStop.Visible = True
                    Me.lbStop.Visible = True
                    Me.lbPrompt.Text = "Enter pallet STOP time - HHMM"
                    Common.JavaScriptSetFocus(Page, Me.txStop)
                Else 'Start time entered is not a valid time
                    Common.SaveVariable("Start", Me.txStart.Text, Page)
                    Me.lbError.Text = "Start Time Entered is not valid"
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txStart)
                End If
            Else
                Common.SaveVariable("Start", Me.txStart.Text, Page)
                Me.lbError.Text = "Start Time Entered must be 4 digit Military Time - HHMM ie; 0935 for 9:35 AM"
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txStart)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Start Time entered! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
            Common.SaveVariable("Start", Me.txStart.Text, Page)
            Common.JavaScriptSetFocus(Page, Me.txStart)
        End Try
    End Sub

    Protected Sub txStop_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txStop.TextChanged
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify the start time entered length is 4 and can be cast to Time
            Dim iStopLen As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.StopTime.Length").ToString)

            If Me.txStop.Text.Length = iStopLen Then
                Dim dStop As DateTime = Nothing

                If DateTime.TryParse(Mid(Me.txStop.Text, 1, 2) & ":" & Mid(Me.txStop.Text, 3, 2), dStop) = True Then
                    'Stop Time is valid continue to Process Stop Time
                    Common.SaveVariable("Stop", Me.txStop.Text, Page)
                    Me.lbToBinName.Visible = True
                    Me.lbToBin.Visible = True
                    Me.txPrinter.Visible = True
                    Me.lbPrinter.Visible = True
                    Me.lbPrompt.Text = "Enter Printer #"
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Else 'Stop Time entered is not a valid time
                    Common.SaveVariable("Stop", Me.txStop.Text, Page)
                    Me.lbError.Text = "Stop Time Entered is not valid"
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txStop)
                End If
            Else
                Common.SaveVariable("Stop", Me.txStop.Text, Page)
                Me.lbError.Text = "Stop Time Entered must be 4 digit Military Time - HHMM ie; 0935 for 9:35 AM"
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txStop)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Start Time entered! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
            Common.SaveVariable("Stop", Me.txStop.Text, Page)
            Common.JavaScriptSetFocus(Page, Me.txStop)
        End Try
    End Sub

    'Protected Sub txToBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txToBin.TextChanged
    '    'Verify the Bin Location entered is valid in database
    '    Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
    '    Try
    '        Me.lbError.Text = ""
    '        Me.lbError.Visible = False

    '        If Not DB.MakeSQLConnection("Warehouse") Then
    '            sqlCmdBin = DB.NewSQLCommand("SQL.Query.BinLookup")
    '            If Not sqlCmdBin Is Nothing Then
    '                sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txToBin.Text)
    '                _tobin = sqlCmdBin.ExecuteScalar()
    '                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
    '                DB.KillSQLConnection()

    '                If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
    '                    Me.lbError.Text = "Invalid Bin Location entered. Try again."
    '                    Me.lbError.Visible = True
    '                    'Save the bad Bin Location to display on screen for correction
    '                    Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '                    Common.JavaScriptSetFocus(Page, Me.txToBin)
    '                Else 'Bin Location is valid
    '                    If Common.GetVariable("Pallet", Page).ToString = "" Then 'MTS Product on Pallet, move to Printer Entry
    '                        Common.SaveVariable("ToBin", UCase(Me.txToBin.Text), Page)
    '                        Me.txPrinter.Visible = True
    '                        Me.lbPrinter.Visible = True
    '                        Me.lbPrompt.Text = "Enter Printer #"
    '                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
    '                    Else 'MTO Pallet can be written to database now
    '                        Common.SaveVariable("ToBin", UCase(Me.txToBin.Text), Page)
    '                        Common.SaveVariable("Printer", "0", Page)

    '                        If WriteTransactions() = True Then
    '                            Call InitProcess()
    '                        Else
    '                            Me.lbError.Text = "Putaway transaction did not get written to the system.  Input the Bin Location to try again, or see supervisor."
    '                            Me.lbError.Visible = True
    '                            Common.SaveVariable("ToBin", "", Page)
    '                            Common.JavaScriptSetFocus(Page, Me.txToBin)
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                Me.lbError.Text = "Command Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
    '                Me.lbError.Visible = True
    '                Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '                Common.JavaScriptSetFocus(Page, Me.txToBin)
    '            End If
    '        Else
    '            Me.lbError.Text = "Communication Error occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
    '            Me.lbError.Visible = True
    '            Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '            Common.JavaScriptSetFocus(Page, Me.txToBin)
    '        End If
    '    Catch ex As Exception
    '        Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Bin Location! Check battery and Wireless connection - then try again or see your supervisor."
    '        Me.lbError.Visible = True
    '        Common.JavaScriptSetFocus(Page, Me.txToBin)
    '    Finally
    '        If Not sqlCmdBin Is Nothing Then
    '            sqlCmdBin.Dispose() : sqlCmdBin = Nothing
    '        End If
    '        DB.KillSQLConnection()
    '    End Try
    'End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify printer # entered is for correct location and is numeric
            If IsNumeric(Me.txPrinter.Text) = True Then
                Dim iPrt As Integer = CInt(Me.txPrinter.Text)

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
                            If WriteTransactions() = True Then
                                Call InitProcess()
                            Else
                                Me.lbError.Text = "Putaway transaction did not get written to the system! Check battery and Wireless connection - then try again or see your supervisor."
                                Me.lbError.Visible = True
                                Common.SaveVariable("Printer", "", Page)
                                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            End If
                        End If
                    Else
                        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                        sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                        DB.KillSQLConnection()
                        Me.lbError.Text = "Error occurred while validating Printer - try again or see your supervisor!"
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
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

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

End Class