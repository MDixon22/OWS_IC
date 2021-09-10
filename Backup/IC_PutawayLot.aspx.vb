Partial Public Class IC_PutawayLot
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
    Public _printjoblot As Integer
    Public _productversion As String
    Public _component As String
    Public _status As String
    Public _lot As String
    Public _caselotYY As String
    Public _caselotDDD As String
    Public _codedate As DateTime
    Public _gtin As Int64
    Public _version As Integer
    Public _printerline As Integer

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
            'Common.SaveVariable("ScreenParam", "", Page)
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
        ElseIf Common.GetVariable("ScreenParam", Page).ToString = "WCFG" Then
            Me.lbPageTitle.Text = "OW Inventory - WC FG Product Putaway"
            Me.lbToBinName.Text = "WCFG"
        ElseIf _whs = "17" Then
            'Added to handle Montgomery FG Putaway for HF and Lot Checking
            Me.lbPageTitle.Text = "OW Inventory - Montgomery FG Putaway"
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
        _lot = Nothing

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
        Dim sqlCmdGTINVer As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdProdVerFormulaLot As New System.Data.SqlClient.SqlCommand

        Dim sqlCmdAddPrintJobLot As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            'Verify the Case Label - make sure GTIN, Code Date, MFGORD# are valid
            Dim iCL As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString)
            'Dim iCL2 As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.AltCaseLength").ToString)

            If Me.txCaseLabel.Text.Length <> iCL Then Throw New Exception("Invalid Barcode Label - see supervisor!")
            'Reset Component Value 
            Common.SaveVariable("Component", "", Page)

            'Verify product exists in the system with GTIN and Version
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection to Database failed.  Please try again")

            sqlCmdGTINVer = DB.NewSQLCommand("SQL.Query.GtinVerLookupProd")
            If sqlCmdGTINVer Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
            sqlCmdGTINVer.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
            sqlCmdGTINVer.Parameters.AddWithValue("@Version", CInt(Mid$(Me.txCaseLabel.Text, 42, 3)))
            _product = sqlCmdGTINVer.ExecuteScalar()
            sqlCmdGTINVer.Dispose() : sqlCmdGTINVer = Nothing
            DB.KillSQLConnection()

            If RTrim(_product).Length < 1 Then Throw New Exception("Invalid Product in barcode - see your supervisor")
            'CLng(Mid$(Me.txCaseLabel.Text, 27, 12))
            Dim LotFull As String = ""
            LotFull = CInt(Common.FindLotFromBarcode(Mid$(Me.txCaseLabel.Text, 27, 12), _whs, Mid$(Me.txCaseLabel.Text, 3, 14), CInt(Mid$(Me.txCaseLabel.Text, 42, 3)), Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2)))

            ''Added 3/11/2021 - check to see if Lot exists in PrintJobs table in OWLabels database where PrintJob = CLng(Mid$(Me.txCaseLabel.Text, 27, 12))
            'If DB.MakeSQLConnection("OWLabels") Then Throw New Exception("Connection to OWLabels Database failed.  Please try again")
            'Dim sqlPrintJob As String = "Select LotYYDDD from vwGetPrintJobLot Where PrintJob = " & CLng(Mid$(Me.txCaseLabel.Text, 27, 12))
            'Dim sqlCmdPrintJobLot As New System.Data.SqlClient.SqlCommand
            'sqlCmdPrintJobLot = DB.NewSQLCommand3(sqlPrintJob)
            'If sqlCmdPrintJobLot Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
            '_printjoblot = sqlCmdPrintJobLot.ExecuteScalar()
            'sqlCmdPrintJobLot.Dispose() : sqlCmdPrintJobLot = Nothing
            'DB.KillSQLConnection()


            'If _printjoblot < 1 Then
            '    'Added 10/19/2020 - Write record to PrintJobs table in OWLabels database. 
            '    _caselotDDD = Mid$(Me.txCaseLabel.Text, 36, 3)
            '    _caselotYY = "20" + Mid$(Me.txCaseLabel.Text, 34, 2).ToString
            '    LotFull = Right(_caselotYY.ToString, 2) & _caselotDDD.ToString

            '    If DB.MakeSQLConnection("OWLabels") Then Throw New Exception("Connection to OWLabels Database failed.  Please try again")
            '    sqlCmdAddPrintJobLot = DB.NewSQLCommand("SQL.IC_InsPrintJob")
            '    If sqlCmdAddPrintJobLot Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@PrintJob", Mid$(Me.txCaseLabel.Text, 27, 12))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@PrintAt", Now)
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Plant", _whs)
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Line", Mid$(Me.txCaseLabel.Text, 27, 2))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Printer", Mid$(Me.txCaseLabel.Text, 29, 2))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@GTIN", Mid$(Me.txCaseLabel.Text, 3, 14))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Version", CInt(Mid$(Me.txCaseLabel.Text, 42, 3)))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@CodeDate", Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2))
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Lot", _caselotDDD)
            '    sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Year", _caselotYY)
            '    sqlCmdAddPrintJobLot.ExecuteNonQuery()
            '    sqlCmdAddPrintJobLot.Dispose() : sqlCmdAddPrintJobLot = Nothing
            '    DB.KillSQLConnection()

            'Else
            '    _caselotDDD = Mid$(_printjoblot.ToString, 3, 3)
            '    _caselotYY = "20" + Mid$(_printjoblot.ToString, 1, 2)
            '    LotFull = Right(_caselotYY.ToString, 2) & _caselotDDD.ToString

            'End If

            'Check to see if product is HF. If not then check for valid lot# formula existance.
            If ChkHFException(Mid$(Me.txCaseLabel.Text, 3, 14)) = False Then
                If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection to Database failed.  Please try again")
                sqlCmdProdVerFormulaLot = DB.NewSQLCommand("SQL.Query.ProdVerFormulaLotLookup")
                If sqlCmdProdVerFormulaLot Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
                sqlCmdProdVerFormulaLot.Parameters.AddWithValue("@Prod", CInt(_product))
                sqlCmdProdVerFormulaLot.Parameters.AddWithValue("@Version", CInt(Mid$(Me.txCaseLabel.Text, 42, 3)))
                'sqlCmdProdVerFormulaLot.Parameters.AddWithValue("@Lot", Mid$(Me.txCaseLabel.Text, 34, 5))
                'Changed 10/19/2020 to accomodate Dan's new label program.
                sqlCmdProdVerFormulaLot.Parameters.AddWithValue("@Lot", LotFull)
                
                _lot = sqlCmdProdVerFormulaLot.ExecuteScalar()
                sqlCmdProdVerFormulaLot.Dispose() : sqlCmdProdVerFormulaLot = Nothing
                DB.KillSQLConnection()

                If RTrim(_lot).Length < 1 Then Throw New Exception("Lot on Case Label not valid for this Product - Return to Packaging")
            End If

            'Product Version and Lot is valid --- validate the code date
            If IsDate(Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txCaseLabel.Text, 19, 6), 1, 2)) = False Then Throw New Exception("CodeDate in barcode is not a valid date - see supervisor!")

            'Product, Version, CodeDate, Component YN have been verified - Pass process onto Quantity Entry
            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
            Me.txQuantity.Visible = True
            Me.lbQuantity.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txQuantity)

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Case Label entered - try again or see your supervisor."
            Me.lbError.Visible = True
            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
        Finally

            If Not sqlCmdGTINVer Is Nothing Then
                sqlCmdGTINVer.Dispose() : sqlCmdGTINVer = Nothing
            End If
            If Not sqlCmdProdVerFormulaLot Is Nothing Then
                sqlCmdProdVerFormulaLot.Dispose() : sqlCmdProdVerFormulaLot = Nothing
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

    Public Function ChkHFException(ByVal lngGTin As Long) As Boolean
        ChkHFException = False

        Dim strSqlGTinChkException As String = "SELECT Prod FROM IC_ExceptionsMaster Where GTIN = " & lngGTin & " AND Exception = 'HickoryFarms' AND [Active] = 1"
        Dim iProd As Integer

        'Get Product Exception for HickoryFarms
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
                Me.lbError.Text = "Command Error occurred while getting Product Exception for HickoryFarms - Check Connection then press Complete again."
            End If
        Else
            Me.lbError.Text = "Connection Error occurred while getting Product Exception for HickoryFarms - Check Connection then press Complete again."
        End If

    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class