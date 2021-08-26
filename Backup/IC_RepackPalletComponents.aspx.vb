Partial Public Class IC_RepackPalletComponents
    Inherits System.Web.UI.Page

    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _codedate As String
    Public _status As String
    Public _gtin As String
    Public _fglot As String
    Public _lot As String
    Public LotFull As String
    Public _ver As String
    Public _itemupc As String
    Public _compprod As String
    Public strURL As String = Nothing

#Region "Page Level"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
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
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
            'Prompt Error and Set Focus to Pallet TextBox
            If RTrim(Me.txData.Text).Length < 1 Then
                Me.lbError.Text = "RePack FG Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("RePackFGPallet", Nothing, Page)
        Common.SaveVariable("CompStockPallet", Nothing, Page)

        Me.txData.Visible = True
        Me.txData.Text = ""

        _pallet = Nothing
        strURL = Nothing
        _status = Nothing
        _gtin = Nothing

        Me.lbFG_RPKPallet.Visible = False
        Me.lbFG_RPKPalletVal.Text = ""
        Me.lbFG_RPKPalletVal.Visible = False

        Me.lbComponentsGridTitle.Visible = False
        Me.dgComponentsUsed.Visible = False

        Me.lbComponentsScanned.Visible = False
        Me.dgComponentsScanned.Visible = False

        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan or Enter RePack FG Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txData)
        Me.lbFunction.text = "1"
    End Sub

#End Region

#Region "User Entry"

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim dsComponent As New Data.DataSet
        Dim sqlCmdComponent As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txData.Text).Length < 1 Then
                Me.lbError.Text = "Entry must be made.  Please try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
                Exit Sub
            End If

            Select Case Me.lbFunction.Text
                Case Is = "1" 'Entering RePack FG Pallet # - and displaying Components Grid
                    'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
                    If IsNumeric(Me.txData.Text) = True Then
                        iPallet = CLng(Me.txData.Text)
                    Else
                        Me.lbError.Text = "RePack FG Pallet# entry must be numeric.  Please try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                        Exit Sub
                    End If

                    If Not DB.MakeSQLConnection("Warehouse") Then
                        sqlCmdPallet = DB.NewSQLCommand("SQL.Query.ValidateRepackPallet")
                        If Not sqlCmdPallet Is Nothing Then
                            sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
                            dsPallet = DB.GetDataSet(sqlCmdPallet)
                            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                            DB.KillSQLConnection()
                        Else
                            Me.lbError.Text = "Command Error occurred while validating RePack FG Pallet#! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        End If

                        If dsPallet Is Nothing Then
                            Me.lbError.Text = "Data Error occurred while validating RePack FG Pallet#! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        Else
                            If dsPallet.Tables(0).Rows.Count > 0 Then
                                _pallet = Me.txData.Text
                                _gtin = dsPallet.Tables(0).Rows(0).Item("GTIN")
                                _ver = dsPallet.Tables(0).Rows(0).Item("pk_nVersion")

                                Common.SaveVariable("RePackFGPallet", _pallet, Page)
                                Me.lbFG_RPKPalletVal.Text = _pallet
                                Me.LoadComponentsUsed(_gtin, _ver)

                                Me.lbFG_RPKPalletVal.Visible = True
                                Me.lbFG_RPKPallet.Visible = True

                                Me.lbFunction.Text = "2"
                                Me.lbPrompt.Text = "Scan Component Case Labels Used"
                                Me.txData.Text = ""
                                Common.JavaScriptSetFocus(Page, Me.txData)

                            Else 'Record not in active status for Pallet entered
                                Me.lbError.Text = "RePack FG Pallet# not active in system - see supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txData)
                            End If
                        End If
                    Else
                        Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case Is = "2" 'Entering Component Case Labels used - and writing to IC_RePackComponents
                    'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
                    If Me.txData.Text.Length = 44 Then
                        _gtin = Mid(Me.txData.Text, 3, 14).ToString
                        _ver = Mid(Me.txData.Text, 42, 3).ToString
                        _codedate = Mid(Me.txData.Text, 19, 6).ToString
                        _lot = Mid(Me.txData.Text, 34, 5).ToString
                        'LookupLot in PrintJobs table in OWLabels database where PrintJob = Clng(Mid$(Me.txCaseLabel.Text, 27, 12))
                        '  If not in that table use Mid$(Me.txCaseLabel.Text, 34, 5) above to get from owlabel system label.

                        LotFull = CInt(Common.FindLotFromBarcode(Mid$(Me.txData.Text, 27, 12), _whs, _gtin, CInt(_ver), Mid$(_codedate, 3, 2) & "/" & Mid$(_codedate, 5, 2) & "/20" & Mid$(_codedate, 1, 2)))

                    Else
                        Me.lbError.Text = "Barcode scanned is incorrect - scan the case label barcode.  Please try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                        Exit Sub
                    End If

                    If Not DB.MakeSQLConnection("Warehouse") Then
                        sqlCmdComponent = DB.NewSQLCommand("SQL.Query.ValidateComponent")
                        If Not sqlCmdComponent Is Nothing Then
                            sqlCmdComponent.Parameters.AddWithValue("@GTIN", _gtin)
                            sqlCmdComponent.Parameters.AddWithValue("@ProdVer", _ver)
                            dsComponent = DB.GetDataSet(sqlCmdComponent)
                            sqlCmdComponent.Dispose() : sqlCmdComponent = Nothing
                            DB.KillSQLConnection()
                        Else
                            Me.lbError.Text = "Command Error occurred while validating Component Barcode! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        End If

                        If dsComponent Is Nothing Then
                            Me.lbError.Text = "Data Error occurred while validating Component Barcode! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        Else
                            If dsComponent.Tables(0).Rows.Count > 0 Then
                                _compprod = dsComponent.Tables(0).Rows(0).Item("ProdID").ToString
                                _itemupc = dsComponent.Tables(0).Rows(0).Item("ItmUPC").ToString

                                Call Me.WriteComponentScanned(Me.lbFG_RPKPalletVal.Text, _gtin, _compprod, _ver, _codedate, LotFull, _itemupc)

                                Me.LoadComponentsScanned(Me.lbFG_RPKPalletVal.Text)
                                Me.lbFunction.Text = "2"
                                Me.lbPrompt.Text = "Scan Next Component Pallet # Used"
                                Me.txData.Text = ""
                                Common.JavaScriptSetFocus(Page, Me.txData)

                            Else 'Record not found as Component Pallet in system Prompt if using Case Stock as Component
                                Common.SaveVariable("CompStockPallet", Me.txData.Text, Page)
                                Me.lbPrompt.Text = "Is Component from Case Stock - Enter Y or N"
                                Me.lbFunction.Text = "3"
                                Me.txData.Text = ""
                                Common.JavaScriptSetFocus(Page, Me.txData)

                            End If
                        End If

                    Else
                        Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case Else

            End Select

        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            If Not dsPallet Is Nothing Then
                dsPallet.Dispose() : dsPallet = Nothing
            End If
            If Not sqlCmdPallet Is Nothing Then
                sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

#Region "Screen Buttons"

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub btFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinish.Click
        'Check for missing component scans for FGPallet #
        Dim dsComponentsCheck As New Data.DataSet
        Dim sqlCmdComponentsCheck As New System.Data.SqlClient.SqlCommand
        Dim rwComponentsCheck As DataRow


        Try
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdComponentsCheck = DB.NewSQLCommand("SQL.Query.GetRepackCompNotScanned")
                If Not sqlCmdComponentsCheck Is Nothing Then
                    sqlCmdComponentsCheck.Parameters.AddWithValue("@FGPallet", Me.lbFG_RPKPalletVal.Text)
                    dsComponentsCheck = DB.GetDataSet(sqlCmdComponentsCheck)
                    sqlCmdComponentsCheck.Dispose() : sqlCmdComponentsCheck = Nothing
                    DB.KillSQLConnection()
                    If dsComponentsCheck Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while listing Component Products Scanned! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        'Got a returned Dataset 
                        If dsComponentsCheck.Tables(0).Rows.Count > 0 Then
                            Me.lbError.Text = "You still need to scan Component - "
                            For Each rwComponentsCheck In dsComponentsCheck.Tables(0).Rows
                                Me.lbError.Text = Me.lbError.Text & " " & rwComponentsCheck.Item("iProductUPC")
                            Next
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        Else
                            'All Compoonents scanned for Repack Item
                            Me.InitProcess()
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while listing Component Products Scanned! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

#Region "Custom Processes"

    Public Sub LoadComponentsUsed(ByVal gtin As String, ByVal version As Integer)
        'Dim sqlString As String
        Dim dvComponentsUsed As New DataView
        Dim dsComponentsUsed As New Data.DataSet
        Dim sqlCmdComponentsUsed As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            'Get Repack FG Components
            If Not DB.MakeSQLConnection("Warehouse") Then
                'sqlCmdComponentsUsed = DB.NewSQLCommand("SQL.Query.GetRepackFGComponentsNew")
                sqlCmdComponentsUsed = DB.NewSQLCommand("SQL.Query.GetRepackFGComponents")
                If Not sqlCmdComponentsUsed Is Nothing Then
                    sqlCmdComponentsUsed.Parameters.AddWithValue("@GTIN", gtin)
                    sqlCmdComponentsUsed.Parameters.AddWithValue("@FG_Ver", version)
                    dsComponentsUsed = DB.GetDataSet(sqlCmdComponentsUsed)
                    sqlCmdComponentsUsed.Dispose() : sqlCmdComponentsUsed = Nothing
                    DB.KillSQLConnection()
                    If dsComponentsUsed Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while listing Component Products in FG Item! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        dvComponentsUsed = New DataView(dsComponentsUsed.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
                        dgComponentsUsed.DataSource = dvComponentsUsed
                        dgComponentsUsed.DataBind()

                        If dvComponentsUsed.Count > 0 Then
                            Me.lbComponentsGridTitle.Visible = True
                            dgComponentsUsed.Visible = True
                        Else
                            Me.lbComponentsGridTitle.Visible = False
                            dgComponentsUsed.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while listing Component Products in FG Item! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Sub LoadComponentsScanned(ByVal fgpallet As String)
        'Dim sqlString As String
        Dim dvComponentsScanned As New DataView
        Dim dsComponentsScanned As New Data.DataSet
        Dim sqlCmdComponentsScanned As New System.Data.SqlClient.SqlCommand

        Try
            'Get Repack FG Components
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdComponentsScanned = DB.NewSQLCommand("SQL.Query.GetRepackCompScans")
                If Not sqlCmdComponentsScanned Is Nothing Then
                    sqlCmdComponentsScanned.Parameters.AddWithValue("@FGPallet", fgpallet)
                    dsComponentsScanned = DB.GetDataSet(sqlCmdComponentsScanned)
                    sqlCmdComponentsScanned.Dispose() : sqlCmdComponentsScanned = Nothing
                    DB.KillSQLConnection()
                    If dsComponentsScanned Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while listing Component Products Scanned! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        dvComponentsScanned = New DataView(dsComponentsScanned.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
                        Me.dgComponentsScanned.DataSource = dvComponentsScanned
                        Me.dgComponentsScanned.DataBind()

                        If dvComponentsScanned.Count > 0 Then
                            Me.lbComponentsScanned.Visible = True
                            Me.dgComponentsScanned.Visible = True
                        Else
                            Me.lbComponentsScanned.Visible = False
                            Me.dgComponentsScanned.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while listing Component Products Scanned! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Sub WriteComponentScanned(ByVal fgpallet As String, ByVal gtin As String _
                                        , ByVal compprod As String, ByVal ver As String _
                                        , ByVal codedate As String, ByVal lot As String _
                                        , ByVal itemupc As String)

        Dim sqlCmdInsCompPallet As New System.Data.SqlClient.SqlCommand
        Dim strInsCompPallet As String = "Insert into IC_RepackPalletComponents(FGPallet,Comp_GTIN,Comp_Prod,Comp_Ver,CodeDate,Lot,ItmUPC,dtmEntered) Values(" & CLng(fgpallet) & "," & CLng(gtin) & "," & CLng(compprod) & "," & CInt(ver) & ",'" & codedate & "','" & lot & "'," & CLng(itemupc) & ",'" & Now.Date & "')"

        If Not DB.MakeSQLConnection("Warehouse") Then
            sqlCmdInsCompPallet = DB.NewSQLCommand3(strInsCompPallet)
            If Not sqlCmdInsCompPallet Is Nothing Then
                sqlCmdInsCompPallet.ExecuteNonQuery()
                sqlCmdInsCompPallet.Dispose() : sqlCmdInsCompPallet = Nothing
                DB.KillSQLConnection()
            End If
        End If
    End Sub

#End Region

End Class