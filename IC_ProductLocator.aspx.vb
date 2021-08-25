Partial Public Class IC_ProductLocator
    Inherits System.Web.UI.Page

    Public _pickproduct As String
    Public strURL As String
    Public _company As String
    Public _whs As String
    Public _option As String
    Public _caselabel As String
    Public _gtin As String
    Public _codedate As String
    Public _trackinginfo As String
    Public _starttime As String
    Public _stoptime As String
    Public _tobin As String
    Public _printer As String
    Public _user As String
    Public _pass As String
    Public _putawaymode As String
    Public _trantype As String
    Public _palletqty As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _prodoldacceptcodedate As String
    Public _prodvariant As String
    Public _palletlookup As String
    Public _status As String
    Public _lastuser As String
    Public _filtercodedate As String
    Public _sorting As String
    Public _SupOver As String

    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString.ToString
    'Private sConnString As String = "server=SQL1;max pool size=300;user id=sa;password=buddig;database=Warehouse"
    Private sqlConn As New SqlClient.SqlConnection(sConnString)

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.txPickProduct.Text = Common.GetVariable("PassedPickedProduct", Page)
        Me.lbBinLocation.Text = Common.GetVariable("Bin", Page)
        Me.lbLastUser.Text = Common.GetVariable("LastUser", Page)
        Me.txSupOvrSkip.Text = Common.GetVariable("SupCode", Page)
        Me.txSkipYN.Text = Common.GetVariable("SupOvrYN", Page)

        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)
        If Not strURL Is Nothing Then

            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Retain Supervisor OverRide Code Field in Password Field
        If RTrim(Me.txSupOvrSkip.Text).Length > 0 Then
            Me.txSupOvrSkip.Attributes("value") = Me.txSupOvrSkip.Text
        End If

        'Verify the user is logged in
        Common.CheckLogin(Page)

        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        'Value passed from the Main Menu
        Me.lbMode.Text = Common.GetVariable("ScreenParam", Page).ToString

        If Not Page.IsPostBack Then
            Common.SaveVariable("FullPallet", "N", Page)
            Try
                Call InitProcess() 'Setting up screen for start up
                Common.JavaScriptSetFocus(Page, Me.txPickProduct)

            Catch ex As Exception
                Me.lbError.Text = "Error - " & ex.Message.ToString & " - occurred on page load. Try again!"
                Me.lbError.Visible = True
            End Try

        Else 'Page is posted back 

            If Trim(Me.txPickProduct.Text).Length < 1 Then
                'Enter Key was pressed, without an entry, while inside the PickProduct TextBox 
                'Prompt Error and Set Focus to PickProduct TextBox
                Me.lbError.Text = "Pick Product needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPickProduct)
            End If
        End If
        btRestart.Visible = True
    End Sub

    Public Sub InitProcess()
        Try
            Common.SaveVariable("ToBin", "", Page)
            Common.SaveVariable("Bin", "", Page)
            Common.SaveVariable("PassedPickedProduct", "", Page)
            Common.SaveVariable("PassedGTIN_OACD", "", Page) 'GTIN + FilterCodeDate + Version
            Common.SaveVariable("PassedOACD", "", Page) 'FilterCodeDate
            Common.SaveVariable("EnteredCD", "", Page) 'CodeDateInScan
            Common.SaveVariable("PassedVersion", "", Page)
            Common.SaveVariable("PassedPallet", "", Page)
            Common.SaveVariable("PassedGTIN", "", Page)
            Common.SaveVariable("Skip", "N", Page)
            Common.SaveVariable("SupOvrYN", "", Page)
            Common.SaveVariable("SupCode", "", Page)
            Common.SaveVariable("SupOverName", "", Page)

            Me.txPickProduct.Visible = True
            _pickproduct = Nothing

            Me.lbBinLocation.Visible = False
            Me.dgProductLocator.Visible = False
            Me.lblBinLocation.Visible = False
            Me.lbLastUser.Visible = False

            Me.lbSupOvr.Visible = False
            Me.txSupOvrSkip.Visible = False
            Me.lbAllow.Visible = False
            Me.txSkipYN.Visible = False
            Me.btSkip.Visible = False
            Me.txSupOvrSkip.Text = ""
            Me.txSupOvrSkip.Attributes("value") = Me.txSupOvrSkip.Text
            Me.txSkipYN.Text = ""

            Me.lbError.Text = ""
            Me.lbError.Visible = False

            Me.lbPrompt.Text = "Scan Pick Product for Lookup"

            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
        Catch ex As Exception
            lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
        End Try
    End Sub



    Public Function WriteSkpPalHistory()
        Dim rtnVal As Boolean = False
        Dim sqlCmdSkpInsert As New System.Data.SqlClient.SqlCommand
        'Write Transaction Record to IC_Trans
        Try
            _datemodified = Now()
            _dateentered = Now()

            _trantype = "SKPPAL"

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdSkpInsert = DB.NewSQLCommand("SQL.IC_TransInsert")
                If Not sqlCmdSkpInsert Is Nothing Then
                    sqlCmdSkpInsert.Parameters.AddWithValue("@TransactionType", _trantype)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Pallet", "0")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@CaseLabel", Common.GetVariable("PassedGTIN_OACD", Page).ToString) 'Gtin + Current FilterDate
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Company", _company)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@PalletQty", 0)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@StartTime", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@StopTime", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@ToBin", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@UserID", Common.GetVariable("UserID", Page).ToString)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Status", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@DateEntered", _dateentered)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@DateModified", _datemodified)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Printer", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Warehouse", _whs)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Version", Common.GetVariable("PassedVersion", Page).ToString)
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Processed", "N")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@ReprintLabel", "")
                    sqlCmdSkpInsert.Parameters.AddWithValue("@Override", Common.GetVariable("SupOverName", Page).ToString)
                    sqlCmdSkpInsert.ExecuteNonQuery()
                    sqlCmdSkpInsert.Dispose() : sqlCmdSkpInsert = Nothing
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
            If Not sqlCmdSkpInsert Is Nothing Then
                sqlCmdSkpInsert.Dispose() : sqlCmdSkpInsert = Nothing
            End If
            DB.KillSQLConnection()
        End Try
        Return rtnVal
    End Function

    Protected Sub txPickProduct_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPickProduct.TextChanged
        Try
            If Me.lbSupOvr.Visible = True Then
                Me.lbError.Text = "Supervisor override is required to continue."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
            Else
                Common.SaveVariable("PassedPickedProduct", Me.txPickProduct.Text, Page)
                Call ProcessInput()
            End If
        Catch ex As Exception
            lbError.Text = "Try again - Pick Product Entry Error = " & ex.Message.ToString
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
        End Try
    End Sub

    Protected Sub ProcessInput()
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Try
            'ScreenParam is PickFace Replenishment or Product Locator
            If txPickProduct.Text.Length > 0 Then
                Dim regexPickProduct As New Regex("^[0-9]+$")
                If regexPickProduct.IsMatch(Trim(Me.txPickProduct.Text)) = False Then
                    Me.txPickProduct.Text = ""
                    Me.lbError.Text = "Entry must be all numbers, try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPickProduct)
                    Exit Sub
                End If

                Select Case Trim(Me.txPickProduct.Text.Length)
                    Case 27
                        'Entry made is Pick Product from Pick Sheet


                    Case 44
                        'Entry made is case label from box with version

                    Case Else
                        'If not any of the lengths above then generate error msg
                        Me.lbError.Text = "Entry made is an incorrect length - Scan Pick Product, CaseLabel, or enter Product#."
                        Me.lbError.Visible = True
                        Me.txPickProduct.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
                End Select

                If Me.lbError.Text.Length = 0 Then
                    'Product was found in system and oldest available code date was retreived
                    'Get Dataset for datagrid top 10 pallets that meet criteria
                    Dim dsDG As New Data.DataSet
                    Dim sqlCmdDG As New System.Data.SqlClient.SqlCommand
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        'Create select statement depending on input type and get Dataset for Grid
                        _prodvariant = Common.GetVariable("PassedVersion", Page).ToString

                        'Sort Pallets Qty Asc
                        Common.SaveVariable("PassedPallet", "777", Page)
                        If _prodvariant.Length < 1 Then
                            sqlCmdDG = DB.NewSQLCommand("SQL.Query.GetPalletsForLookup")
                            If Not sqlCmdDG Is Nothing Then
                                sqlCmdDG.Parameters.AddWithValue("@GTIN", Common.GetVariable("PassedGTIN", Page).ToString)
                                sqlCmdDG.Parameters.AddWithValue("@FilterOACD", _filtercodedate)
                                If Not DB.FillDataGrid(sqlCmdDG, Me.dgProductLocator) Then
                                    Me.dgProductLocator.Visible = True
                                    Me.btSkip.Visible = True
                                Else
                                    Me.lbError.Text = "No Pallets exist that meet search criteria."
                                    Me.dgProductLocator.Visible = False
                                    Me.lbError.Visible = True
                                    Me.txPickProduct.Text = ""
                                    Common.JavaScriptSetFocus(Page, Me.txPickProduct)
                                End If
                                sqlCmdDG.Dispose() : sqlCmdDG = Nothing
                                DB.KillSQLConnection()
                            End If
                        Else
                            sqlCmdDG = DB.NewSQLCommand("SQL.Query.GetPalletsForLookupWithVer_WH")
                            If Not sqlCmdDG Is Nothing Then
                                sqlCmdDG.Parameters.AddWithValue("@GTIN", Common.GetVariable("PassedGTIN", Page).ToString)
                                sqlCmdDG.Parameters.AddWithValue("@FilterOACD", _filtercodedate)
                                sqlCmdDG.Parameters.AddWithValue("@Version", _prodvariant)
                                If Not DB.FillDataGrid(sqlCmdDG, Me.dgProductLocator) Then
                                    Me.dgProductLocator.Visible = True
                                    Me.btSkip.Visible = True
                                Else
                                    Me.lbError.Text = "No Pallets exist that meet search criteria."
                                    Me.dgProductLocator.Visible = False
                                    Me.lbError.Visible = True
                                    Me.txPickProduct.Text = ""
                                    Common.JavaScriptSetFocus(Page, Me.txPickProduct)
                                End If
                                sqlCmdDG.Dispose() : sqlCmdDG = Nothing
                                DB.KillSQLConnection()
                            End If
                        End If
                    End If

                    If Not sqlCmdDG Is Nothing Then
                        sqlCmdDG.Dispose() : sqlCmdDG = Nothing
                    End If
                    DB.KillSQLConnection()

                    'If Me.lbError.Text.Length = 0 Then
                    '    'Write a record for lookup tracking
                    '    If WriteTransactions() = False Then
                    '        Me.lbError.Text = "Lookup History transaction record did not get written to database.  Please tell supervisor."
                    '        Me.lbError.Visible = True
                    '        Me.dgProductLocator.Visible = False
                    '        Me.txPickProduct.Text = ""
                    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
                    '    End If
                    'End If
                End If

            Else
                Me.lbError.Text = "Pick Product is required."
                Me.lbError.Visible = True
                Me.txPickProduct.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txPickProduct)
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during Lookup! Check Battery and Wireless Connection - then try again or see supervisor."
            Me.lbError.Visible = True
            Me.txPickProduct.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        Try
            If Me.lbSupOvr.Visible = True Then
                Me.lbError.Text = "Supervisor override is required to continue."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
            Else
                'Clear out Passed values which is not used in pick face screen- obsolete
                Common.SaveVariable("PassedGTIN_OACD", "", Page) 'For CaseLabel value in writing record
                Common.SaveVariable("PassedPallet", "", Page) 'For MTO Lookup
                Common.SaveVariable("PassedGTIN", "", Page) 'For Non MTO Lookup
                Common.SaveVariable("PassedVersion", "", Page) 'For Non MTO Lookup
                Common.SaveVariable("PassedOACD", "", Page) 'For Non MTO Lookup
                Common.SaveVariable("FromLookup", "False", Page) 'True/False 
                Common.SaveVariable("ScreenParam", "", Page) 'Used in place of g_lookuptype
                Common.SaveVariable("SupOvrYN", "", Page)
                Common.SaveVariable("SupCode", "", Page)
                Common.SaveVariable("Skip", "", Page)
                Common.SaveVariable("SupOverName", "", Page)

                'Return to the Main Menu
                'Common.SaveVariable("newURL", Me.lbReturnURL.Text, Page)
                If Request.QueryString("bin") <> Nothing Then
                    'Common.SaveVariable("newURL", "~/IC_OrderMaking2.aspx?bin=" & Request.QueryString("bin"), Page)
                    Common.SaveVariable("newURL", "~/IC_OrderMakingNew.aspx?bin=" & Request.QueryString("bin"), Page)
                    Common.SaveVariable("FullPallet", "", Page)
                Else
                    Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
                End If
            End If
        Catch ex As Exception
            lbError.Text = "Try again - Return to Prior Screen Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        Try
            If Me.lbSupOvr.Visible = True Then
                Me.lbError.Text = "Supervisor override is required to continue."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
            Else
                'Reload screen and start process over
                If Request.QueryString("bin") <> Nothing Then
                    strURL = "~/IC_ProductLocator.aspx?bin=" & Request.QueryString("bin")
                    Common.SaveVariable("newURL", strURL, Page)
                Else
                    strURL = "~/IC_ProductLocator.aspx"
                    Common.SaveVariable("newURL", strURL, Page)
                End If

                Page.Response.Clear()
                Page.Response.Redirect(strURL)
                Page.Response.End()
            End If
        Catch ex As Exception
            lbError.Text = "Try again - Restart Process Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try
    End Sub

    Protected Sub btSkip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSkip.Click
        Try
            If btSkip.Text = "Skip" Then
                Me.lbSupOvr.Visible = True
                Me.txSupOvrSkip.Visible = True
                Me.lbPrompt.Text = "Have supervisor scan Override barcode"
                Me.btSkip.Visible = False
                Me.txSupOvrSkip.Text = ""
                Me.txSupOvrSkip.Attributes("value") = Me.txSupOvrSkip.Text
                Me.txSkipYN.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
            ElseIf btSkip.Text = "Submit Again" Then 'Configured as (Submit Again) Button
                'Write IC_Trans record for Transaction Type SKPPAL (Skipped Pallets during lookup)
                If WriteSkpPalHistory() = False Then
                    Me.lbError.Text = "Skip To Next Code Date transaction failed again - not written to database. Close program - check battery - reboot if needed."
                    Me.lbError.Visible = True
                    Me.btSkip.Text = "Submit Again"
                    Me.btSkip.Visible = True
                    Exit Sub
                Else
                    Me.txSupOvrSkip.Text = ""
                    Me.txSupOvrSkip.Attributes("value") = Me.txSupOvrSkip.Text
                    Me.txSkipYN.Text = ""
                    Me.lbAllow.Visible = False
                    Me.lbSupOvr.Visible = False
                    Common.SaveVariable("SupCode", "", Page)
                    Call ProcessInput()
                End If
            End If
        Catch ex As Exception
            lbError.Text = "Try again - Skip Pallet Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try
    End Sub

    Protected Sub txSupOvrSkip_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txSupOvrSkip.TextChanged
        Try
            If Me.txSkipYN.Visible = False Then
                If Me.txSupOvrSkip.Visible = True Then
                    If ValidateSuperOverride(Me.txSupOvrSkip.Text) = True Then
                        Common.SaveVariable("SupCode", Me.txSupOvrSkip.Text, Page)
                        Me.lbAllow.Visible = True
                        Me.txSkipYN.Visible = True
                        Me.txSkipYN.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txSkipYN)
                    Else
                        Me.lbError.Text = "A supervisor override code is required to continue."
                        Me.lbError.Visible = True
                        Me.txSupOvrSkip.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
                    End If
                End If
            End If
        Catch ex As Exception
            lbError.Text = "Try again - Supervisor Skip Entry Error = " & ex.Message.ToString
            lbError.Visible = True
            Me.txSupOvrSkip.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txSupOvrSkip)
        End Try
    End Sub

    Public Function ValidateSuperOverride(ByVal SOC As String) As Boolean
        Dim rtnVal As Boolean
        Dim _SupOverName As String = Nothing
        Try
            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdSuper As New System.Data.SqlClient.SqlCommand
                sqlCmdSuper = DB.NewSQLCommand("SQL.Query.SupervisorCodeLookup")
                If Not sqlCmdSuper Is Nothing Then
                    sqlCmdSuper.Parameters.AddWithValue("@UserID", SOC)
                    sqlCmdSuper.Parameters.AddWithValue("@Department", "OC")
                    _SupOverName = sqlCmdSuper.ExecuteScalar()
                    sqlCmdSuper.Dispose() : sqlCmdSuper = Nothing
                    DB.KillSQLConnection()
                    If Not _SupOverName Is Nothing Then
                        If RTrim(_SupOverName).Length < 1 Then 'Invalid Supervisor Code was entered
                            rtnVal = False
                        Else 'Valid Supervisor Code was entered
                            Common.SaveVariable("SupOverName", _SupOverName, Page)
                            rtnVal = True
                        End If
                    End If
                Else 'sqlCmdSuper Is Nothing 
                    rtnVal = False
                End If
            Else 'DB.MakeSQLConnection("Warehouse") not created
                rtnVal = False
            End If
        Catch ex As Exception
            rtnVal = False
        Finally
            'End and dispose an active connection if it exists to avoid error
            DB.KillSQLConnection()
        End Try
        Return rtnVal
    End Function

    Protected Sub txSkipYN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txSkipYN.TextChanged
        Try
            If RTrim(UCase(Me.txSkipYN.Text)) = "Y" Then
                Common.SaveVariable("SupOvrYN", "Y", Page)
                Common.SaveVariable("Skip", "Y", Page)
                'Write IC_Trans record for Transaction Type SKPPAL (Skipped Pallets during lookup)
                If WriteSkpPalHistory() = False Then
                    Me.lbError.Text = "Skip To Next Code Date transaction record did not get written to database.  Press Submit Again button."
                    Me.lbError.Visible = True
                    Me.btSkip.Text = "Submit Again"
                    Me.btSkip.Visible = True
                    Exit Sub
                Else
                    Me.txSupOvrSkip.Visible = False
                    Me.txSkipYN.Visible = False
                    Me.lbAllow.Visible = False
                    Me.lbSupOvr.Visible = False
                    Common.SaveVariable("SupCode", "", Page)
                    Call ProcessInput()
                End If
            ElseIf RTrim(UCase(Me.txSkipYN.Text)) = "N" Then
                Common.SaveVariable("SupOvrYN", "N", Page)
                Common.SaveVariable("Skip", "N", Page)
                Common.SaveVariable("SupCode", "", Page)

                'Reload screen and start process over
                If Request.QueryString("bin") <> Nothing Then
                    strURL = "~/IC_ProductLocator.aspx?bin=" & Request.QueryString("bin")
                    Common.SaveVariable("newURL", strURL, Page)
                Else
                    strURL = "~/IC_ProductLocator.aspx"
                    Common.SaveVariable("newURL", strURL, Page)
                End If

                Page.Response.Clear()
                Page.Response.Redirect(strURL)
                Page.Response.End()
            Else
                Me.lbError.Text = "Enter Y or N to continue."
                Me.lbError.Visible = True
                Me.txSkipYN.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txSkipYN)
            End If
            Common.SaveVariable("SupOvrYN", "", Page)
        Catch ex As Exception
            lbError.Text = "Try again - Skip Y/N Entry Error = " & ex.Message.ToString
            lbError.Visible = True
            Me.txSkipYN.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txSkipYN)
        End Try
    End Sub
    'Public Function WriteTransactions()
    '    Dim rtnVal As Boolean = False
    '    Dim sqlCmdInsert As New System.Data.SqlClient.SqlCommand
    '    'Write Transaction Record to IC_Trans
    '    Try
    '        _datemodified = Now()
    '        _dateentered = Now()

    '        _trantype = "LOOKUP"

    '        If Not DB.MakeSQLConnection("Warehouse") Then
    '            sqlCmdInsert = DB.NewSQLCommand("SQL.IC_TransInsert")
    '            If Not sqlCmdInsert Is Nothing Then
    '                sqlCmdInsert.Parameters.AddWithValue("@TransactionType", _trantype)
    '                sqlCmdInsert.Parameters.AddWithValue("@Pallet", Common.GetVariable("PassedPallet", Page).ToString)
    '                sqlCmdInsert.Parameters.AddWithValue("@CaseLabel", Common.GetVariable("PassedGTIN_OACD", Page).ToString)
    '                sqlCmdInsert.Parameters.AddWithValue("@Company", _company)
    '                sqlCmdInsert.Parameters.AddWithValue("@PalletQty", 0)
    '                sqlCmdInsert.Parameters.AddWithValue("@StartTime", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@StopTime", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@ToBin", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@UserID", Common.GetVariable("UserID", Page).ToString)
    '                sqlCmdInsert.Parameters.AddWithValue("@Status", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@DateEntered", _dateentered)
    '                sqlCmdInsert.Parameters.AddWithValue("@DateModified", _datemodified)
    '                sqlCmdInsert.Parameters.AddWithValue("@Printer", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@Warehouse", _whs)
    '                sqlCmdInsert.Parameters.AddWithValue("@Version", Common.GetVariable("PassedVersion", Page).ToString)
    '                sqlCmdInsert.Parameters.AddWithValue("@Processed", "N")
    '                sqlCmdInsert.Parameters.AddWithValue("@ReprintLabel", "")
    '                sqlCmdInsert.Parameters.AddWithValue("@Override", "")
    '                sqlCmdInsert.ExecuteNonQuery()
    '                sqlCmdInsert.Dispose() : sqlCmdInsert = Nothing
    '                DB.KillSQLConnection()
    '                rtnVal = True
    '            Else
    '                rtnVal = False
    '            End If
    '        Else
    '            rtnVal = False
    '        End If
    '    Catch ex As Exception
    '        rtnVal = False
    '    Finally
    '        If Not sqlCmdInsert Is Nothing Then
    '            sqlCmdInsert.Dispose() : sqlCmdInsert = Nothing
    '        End If
    '        DB.KillSQLConnection()
    '    End Try
    '    Return rtnVal
    'End Function

    'Public Function CheckFullPallet(ByVal shippingpallet As String) As Boolean
    '    CheckFullPallet = False
    '    Dim sqlCmd As New SqlClient.SqlCommand
    '    Dim _fp As String = ""

    '    Try
    '        If DB.MakeSQLConnection("Warehouse") Then 'Failed to make connection - pause and try again
    '            DB.KillSQLConnection()

    '            System.Threading.Thread.Sleep(2000)

    '            If DB.MakeSQLConnection("Warehouse") Then
    '                Throw New Exception("Connection to Database Error")
    '            End If
    '        End If

    '        sqlCmd = DB.NewSQLCommand("SQL.Query.FullShippingPalletCheck")

    '        If sqlCmd Is Nothing Then
    '            Throw New Exception("Command Creation Error")
    '        End If

    '        sqlCmd.Parameters.AddWithValue("@Pallet", CLng(shippingpallet))
    '        _fp = sqlCmd.ExecuteScalar

    '        If Not sqlCmd Is Nothing Then sqlCmd.Dispose()
    '        DB.KillSQLConnection()
    '        If _fp = "Y" Then
    '            CheckFullPallet = True
    '        End If

    '    Catch ex As Exception
    '        Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred during CheckFullPallet Lookup! Check Battery and Wireless Connection - then try again or see supervisor."
    '        Me.lbError.Visible = True
    '        Me.txPickProduct.Text = ""
    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        'Common.LogError(ex.Message, "", "CheckFullPallet(" & shippingpallet & ")", "", Me, Me.Title, ex.StackTrace)
    '    Finally
    '        If Not sqlCmd Is Nothing Then sqlCmd.Dispose() : sqlCmd = Nothing
    '        DB.KillSQLConnection()
    '    End Try

    'End Function

    'From Process Section
    'If Me.lbMode.Text = "MTO Locator" Then
    '    If txPickProduct.Text.Length > 0 Then
    '        If IsNumeric(txPickProduct.Text) = False Then
    '            'Common.SaveVariable("LookupComplete", "False", Page)
    '            Me.lbError.Text = "Pallet # entered needs to be numeric."
    '            Me.lbError.Visible = True
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '            Exit Sub
    '        End If

    '        If Me.txPickProduct.Text.Length > 10 And Me.txPickProduct.Text.Length < 16 Then
    '            'Pallet entered or scanned was from the Loader book
    '            Common.SaveVariable("PassedPallet", Mid(Me.txPickProduct.Text, 1, 6), Page)

    '        ElseIf Me.txPickProduct.Text.Length <= 10 Then
    '            Common.SaveVariable("PassedPallet", Me.txPickProduct.Text, Page)

    '        Else 'Pallet entered or scanned was not from the loader book and is invalid
    '            Me.lbError.Text = "Pallet # entered is not valid. Scan correct barcode."
    '            Me.lbError.Visible = True
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '            Exit Sub
    '        End If

    '        'Locate pallet in VWICPallets and display Bin Location
    '        Dim dsSlicPal As New Data.DataSet
    '        Dim sqlCmdSlicPal As New System.Data.SqlClient.SqlCommand
    '        If Not DB.MakeSQLConnection("Warehouse") Then
    '            sqlCmdSlicPal = DB.NewSQLCommand("SQL.Query.SlicingPalletLookup")
    '            If Not sqlCmdSlicPal Is Nothing Then
    '                sqlCmdSlicPal.Parameters.AddWithValue("@Pallet", CInt(Common.GetVariable("PassedPallet", Page).ToString))
    '                dsSlicPal = DB.GetDataSet(sqlCmdSlicPal)
    '                sqlCmdSlicPal.Dispose() : sqlCmdSlicPal = Nothing
    '                DB.KillSQLConnection()
    '            Else
    '                Me.lbError.Text = "Error occurred while connecting to database! Try again or see your supervisor."
    '                Me.lbError.Visible = True
    '                Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '            End If
    '        Else
    '            Me.lbError.Text = "Communication Error occurred while connecting to database! Try again or see your supervisor."
    '            Me.lbError.Visible = True
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        End If
    '        If Not sqlCmdSlicPal Is Nothing Then
    '            sqlCmdSlicPal.Dispose() : sqlCmdSlicPal = Nothing
    '        End If

    '        If Me.lbError.Text.Length = 0 Then
    '            If dsSlicPal.Tables(0).Rows.Count > 0 Then 'Record found for Pallet in VWICPallets
    '                Dim strBin As String = dsSlicPal.Tables(0).Rows(0).Item("Bin")
    '                Dim strLastUser As String = dsSlicPal.Tables(0).Rows(0).Item("LastUser")
    '                Common.SaveVariable("Bin", UCase(strBin), Page)
    '                Common.SaveVariable("LastUser", strLastUser, Page)
    '                Common.SaveVariable("PickProduct", Me.txPickProduct.Text, Page)
    '                lbBinLocation.Text = strBin
    '                lbLastUser.Text = strLastUser

    '                Me.lblBinLocation.Visible = True
    '                Me.lbBinLocation.Visible = True

    '                If lbLastUser.Text.Length < 1 Then
    '                    Me.lbLastUser.Text = "UNKNOWN"
    '                End If

    '                Me.lbLastUser.Visible = True

    '                'Write IC_Trans record for lookup tracking successful lookup
    '                Common.SaveVariable("CaseLabel", "", Page)
    '                Common.SaveVariable("Version", "", Page)

    '                If WriteTransactions() = False Then
    '                    Me.lbError.Text = "Lookup History transaction record did not get written to database.  Please tell supervisor."
    '                    Me.lbError.Visible = True
    '                End If

    '            Else 'Record does not exist for Pallet entered
    '                Me.lbError.Text = "Pallet# entered is not in system - see supervisor."
    '                Me.lbError.Visible = True
    '                Common.SaveVariable("PickProduct", Me.txPickProduct.Text, Page)
    '                Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '            End If
    '        End If
    '    Else
    '        Me.lbError.Text = "Pallet # is required for search."
    '        Me.lbError.Visible = True
    '        Me.txPickProduct.Text = ""
    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '    End If


    'ElseIf Me.lbMode.Text = "OWS CBQ Locator" Then
    '    ' SAM - 9/7/2010 - Added to handle option 5 as OWS Pallet Lookup
    '    If txPickProduct.Text.Length < 1 Or txPickProduct.Text.Length > 13 Then
    '        'Error stating that entrymade for OWS Order is not correct length
    '        Me.lbError.Text = "Entry made for OWS/CBQ Order is not the correct length."
    '        Me.lbError.Visible = True
    '        Me.txPickProduct.Text = ""
    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        Exit Sub
    '    End If

    '    Dim dsDG As New Data.DataSet
    '    Dim sqlCmdDG As New System.Data.SqlClient.SqlCommand
    '    If Not DB.MakeSQLConnection("Warehouse") Then
    '        Common.SaveVariable("GTIN", Me.txPickProduct.Text, Page)
    '        Dim intGTIN As Int64 = CLng(Me.txPickProduct.Text)
    '        Dim strSqlOWSPallet As String = "Select Distinct Pallet,Bin FROM vwICPalletsOW WHERE GTIN = " & intGTIN & " or OrderID = " & intGTIN & " or OrderRefNo = " & intGTIN
    '        sqlCmdDG.CommandType = CommandType.Text
    '        sqlCmdDG.Connection = sqlConn
    '        sqlCmdDG.CommandText = strSqlOWSPallet
    '        If Not DB.FillDataGrid(sqlCmdDG, Me.dgProductLocator) Then
    '            Me.dgProductLocator.Visible = True
    '        Else
    '            Me.lbError.Text = "No Pallets exist for OWS/CBQ Order entered."
    '            Me.dgProductLocator.Visible = False
    '            Me.lbError.Visible = True
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        End If
    '        sqlCmdDG.Dispose() : sqlCmdDG = Nothing
    '        DB.KillSQLConnection()

    '    End If

    'ElseIf Me.lbMode.Text = "Meat Locator" Then
    '    ' SAM - 9/7/2010 - Added to handle option 5 as OWS Pallet Lookup
    '    If txPickProduct.Text.Length < 3 Or txPickProduct.Text.Length > 14 Then
    '        'Error stating that entrymade for OWS Order is not correct length
    '        Me.lbError.Text = "Entry made for Meat Code Lookup is not the correct length."
    '        Me.lbError.Visible = True
    '        Me.txPickProduct.Text = ""
    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        Exit Sub
    '    End If
    '    Dim intGTIN As Int64 = 0

    '    Dim dsDG As New Data.DataSet
    '    Dim sqlCmdDG As New System.Data.SqlClient.SqlCommand
    '    If Not DB.MakeSQLConnection("Warehouse") Then
    '        Common.SaveVariable("GTIN", Me.txPickProduct.Text, Page)
    '        If txPickProduct.Text.Length = 3 Then
    '            intGTIN = CLng("90000000000" & Trim(Me.txPickProduct.Text))
    '        Else 'the len of the field is the 14 for gtin
    '            intGTIN = CLng(Me.txPickProduct.Text)
    '        End If
    '        Dim strSqlOWSPallet As String = "Select Pallet,Bin,CodeDate as SliceBy FROM vwIC_ComponentCoolerMeatPallets WHERE GTIN = " & intGTIN & " And [Status] <> 'V'  Order By CodeDate Asc"
    '        sqlCmdDG.CommandType = CommandType.Text
    '        sqlCmdDG.Connection = sqlConn
    '        sqlCmdDG.CommandText = strSqlOWSPallet
    '        If Not DB.FillDataGrid(sqlCmdDG, Me.dgProductLocator) Then
    '            Me.dgProductLocator.Visible = True
    '        Else
    '            Me.lbError.Text = "No Pallets exist for Meat Code entered."
    '            Me.dgProductLocator.Visible = False
    '            Me.lbError.Visible = True
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        End If
    '        sqlCmdDG.Dispose() : sqlCmdDG = Nothing
    '        DB.KillSQLConnection()

    '    End If

    'ElseIf Me.lbMode.Text = "OM Locator" Then 'SAM 4/13/2014 - Verify Code Below after talk with Bill about the process again

    '    If txPickProduct.Text.Length = 29 Then
    '        Dim regexPickProduct As New Regex("^[0-9]+$")
    '        If regexPickProduct.IsMatch(Trim(Me.txPickProduct.Text)) = False Then
    '            Me.txPickProduct.Text = ""
    '            Me.lbError.Text = "Entry must be all numbers, try again."
    '            Me.lbError.Visible = True
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '            Exit Sub
    '        End If
    '    Else
    '        Me.lbError.Text = "Pick Product is required.(Long barcode in Pick/Loader Book)"
    '        Me.lbError.Visible = True
    '        Me.txPickProduct.Text = ""
    '        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        Exit Sub
    '    End If

    '    Dim sqlCmdFOCD As New System.Data.SqlClient.SqlCommand
    '    If Not DB.MakeSQLConnection("Warehouse") Then
    '        If RTrim(Common.GetVariable("Skip", Page).ToString) = "N" Then
    '            'Entry made is pick product from loader book with oacd and version
    '            Common.SaveVariable("PassedGTIN", Mid(Me.txPickProduct.Text, 3, 14), Page)
    '            Common.SaveVariable("EnteredCD", Mid(Me.txPickProduct.Text, 21, 6), Page)
    '            Common.SaveVariable("PassedVersion", Mid(Me.txPickProduct.Text, 27, 3), Page)
    '            If Common.GetVariable("FullPallet", Page) = "Y" Then
    '                sqlCmdFOCD = DB.NewSQLCommand("SQL.Query.OM_ProdOACDVerLookupFP")
    '            Else
    '                sqlCmdFOCD = DB.NewSQLCommand("SQL.Query.OCD_ProdOACDVerLookup")
    '            End If

    '            If Not sqlCmdFOCD Is Nothing Then
    '                sqlCmdFOCD.Parameters.AddWithValue("@GTIN", Mid(Me.txPickProduct.Text, 3, 14))
    '                sqlCmdFOCD.Parameters.AddWithValue("@ProdOACD", Mid(Me.txPickProduct.Text, 21, 6))
    '                sqlCmdFOCD.Parameters.AddWithValue("@Version", Mid(Me.txPickProduct.Text, 27, 3))
    '                _filtercodedate = sqlCmdFOCD.ExecuteScalar()
    '                sqlCmdFOCD.Dispose() : sqlCmdFOCD = Nothing
    '                DB.KillSQLConnection()
    '            End If

    '        ElseIf Common.GetVariable("Skip", Page).ToString = "Y" Then
    '            Dim _skipCodeDate As Date
    '            Dim strDate As String = Common.GetVariable("PassedOACD", Page).ToString

    '            _skipCodeDate = Convert.ToDateTime(Mid$(strDate, 3, 2) & "/" & Mid$(strDate, 5, 2) & "/20" & Mid$(strDate, 1, 2))

    '            'Entry made is pick product from loader book with oacd and version
    '            'Values will be passed to PCKF screen
    '            Common.SaveVariable("PassedGTIN", Mid(Me.txPickProduct.Text, 3, 14), Page)
    '            Common.SaveVariable("EnteredCD", Mid(Me.txPickProduct.Text, 21, 6), Page)
    '            Common.SaveVariable("PassedVersion", Mid(Me.txPickProduct.Text, 27, 3), Page)
    '            If Common.GetVariable("FullPallet", Page) = "Y" Then
    '                sqlCmdFOCD = DB.NewSQLCommand("SQL.Query.OM_ProdOACDVerLookupSkipFP")
    '            Else
    '                sqlCmdFOCD = DB.NewSQLCommand("SQL.Query.OCD_ProdOACDVerLookupSkip")
    '            End If

    '            If Not sqlCmdFOCD Is Nothing Then
    '                sqlCmdFOCD.Parameters.AddWithValue("@GTIN", Mid(Me.txPickProduct.Text, 3, 14))
    '                sqlCmdFOCD.Parameters.AddWithValue("@CodeDate", _skipCodeDate)
    '                sqlCmdFOCD.Parameters.AddWithValue("@Version", Mid(Me.txPickProduct.Text, 27, 3))
    '                _filtercodedate = sqlCmdFOCD.ExecuteScalar()
    '                sqlCmdFOCD.Dispose() : sqlCmdFOCD = Nothing
    '                DB.KillSQLConnection()
    '            End If
    '        End If
    '    End If

    '    If Not sqlCmdFOCD Is Nothing Then
    '        sqlCmdFOCD.Dispose() : sqlCmdFOCD = Nothing
    '    End If
    '    DB.KillSQLConnection()
    '    If Me.lbError.Text.Length = 0 Then
    '        If _filtercodedate Is Nothing Then 'Product was not found in system - throw error
    '            Me.lbError.Text = "No Pallets exist that meet search criteria."
    '            Me.lbError.Visible = True
    '            Me.dgProductLocator.Visible = False
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)

    '        Else 'Product was found in system and oldest available code date was retreived
    '            Common.SaveVariable("PassedOACD", _filtercodedate, Page)
    '            Common.SaveVariable("PassedGTIN_OACD", Common.GetVariable("PassedGTIN", Page).ToString + _filtercodedate + Common.GetVariable("PassedVersion", Page).ToString, Page)

    '            'Get Dataset for datagrid top 10 pallets that meet criteria
    '            Dim dsDG As New Data.DataSet
    '            Dim sqlCmdDG As New System.Data.SqlClient.SqlCommand
    '            If Not DB.MakeSQLConnection("Warehouse") Then
    '                'Create select statement depending on input type and get Dataset for Grid
    '                _prodvariant = Common.GetVariable("PassedVersion", Page).ToString

    '                'Sort Pallets Qty Asc
    '                Common.SaveVariable("PassedPallet", "777", Page)

    '                If Common.GetVariable("FullPallet", Page) = "Y" Then
    '                    sqlCmdDG = DB.NewSQLCommand("SQL.Query.OM_GetPalletsForLookupFP")
    '                Else
    '                    sqlCmdDG = DB.NewSQLCommand("SQL.Query.GetPalletsForLookupWithVer_WH")
    '                End If

    '                If Not sqlCmdDG Is Nothing Then
    '                    sqlCmdDG.Parameters.AddWithValue("@GTIN", Common.GetVariable("PassedGTIN", Page).ToString)
    '                    sqlCmdDG.Parameters.AddWithValue("@FilterOACD", _filtercodedate)
    '                    sqlCmdDG.Parameters.AddWithValue("@Version", _prodvariant)
    '                    If Not DB.FillDataGrid(sqlCmdDG, Me.dgProductLocator) Then
    '                        Me.dgProductLocator.Visible = True
    '                        Me.btSkip.Visible = True

    '                    Else
    '                        Me.lbError.Text = "No Pallets exist that meet search criteria."
    '                        Me.dgProductLocator.Visible = False
    '                        Me.lbError.Visible = True
    '                        Me.txPickProduct.Text = ""
    '                        Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '                    End If
    '                    sqlCmdDG.Dispose() : sqlCmdDG = Nothing
    '                    DB.KillSQLConnection()
    '                End If
    '            End If
    '            'End If
    '            If Not sqlCmdDG Is Nothing Then
    '                sqlCmdDG.Dispose() : sqlCmdDG = Nothing
    '            End If
    '            DB.KillSQLConnection()
    '        End If
    '    End If
    '    If Me.lbError.Text.Length = 0 Then
    '        'Write a record for lookup tracking
    '        If WriteTransactions() = False Then
    '            Me.lbError.Text = "Lookup History transaction record did not get written to database.  Please tell supervisor."
    '            Me.lbError.Visible = True
    '            Me.dgProductLocator.Visible = False
    '            Me.txPickProduct.Text = ""
    '            Common.JavaScriptSetFocus(Page, Me.txPickProduct)
    '        End If
    '    End If
    'Else


End Class