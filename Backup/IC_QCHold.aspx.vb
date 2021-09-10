Partial Public Class IC_QCHold
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _status As String
    Public _shippingreceipt As String = Nothing
    Public strURL As String = Nothing

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
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Else 'Page is posted back 
            'Check that Me.txPallet.text is not empty
            If RTrim(Me.txPallet.Text).Length < 1 Then
                Me.lbError.Text = "Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)

        Me.txConfirm.Visible = False
        Me.lbConfirm.Visible = False
        Me.lbYN.Visible = False

        Me.txConfirm.Text = ""
        Me.txPallet.Text = ""

        _pallet = Nothing
        _status = Nothing
        _shippingreceipt = Nothing
        _tobin = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Pallet# to put on QC Hold"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                'Pallet # entered is valid length check numeric
                iPallet = CInt(Me.txPallet.Text)
            Else
                Me.lbError.Text = "Pallet # must be a numeric entry, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Sub
            End If

            'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetPalletInfo")
                If Not sqlCmdPallet Is Nothing Then
                    sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
                    dsPallet = DB.GetDataSet(sqlCmdPallet)
                    sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                    DB.KillSQLConnection()
                    If dsPallet Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while validating Pallet #! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                        Exit Sub
                    End If

                    If dsPallet.Tables(0).Rows.Count < 1 Then
                        Me.lbError.Text = "Pallet# not in system - see supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                        Exit Sub
                    End If

                    'Data was retrieved - continue process
                    _pallet = Me.txPallet.Text
                    _status = dsPallet.Tables(0).Rows(0).Item("Status")
                    _tobin = dsPallet.Tables(0).Rows(0).Item("Bin")

                    Select Case UCase(RTrim(_status))
                        Case Is = "V"
                            Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                        Case Is = "Q"
                            Me.lbError.Text = "Pallet already on QC Hold. See supervisor if needed"
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                        Case Else
                            'Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                            Me.txConfirm.Visible = True
                            Me.lbConfirm.Visible = True
                            Me.lbYN.Visible = True

                            Me.lbPrompt.Text = "Enter Y or N to confirm request"
                            Common.JavaScriptSetFocus(Page, Me.txConfirm)
                    End Select
                    '******** Commented out and replace with above code 7-19-2011 SAM
                    'If RTrim(_status) = "V" Then 'Pallet is void -  Throw an error
                    'Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                    'Me.lbError.Visible = True
                    'Common.JavaScriptSetFocus(Page, Me.txPallet)
                    'Else 'Pallet is in correct status to proceed
                    '    Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                    '    Common.SaveVariable("Status", _status, Page)
                    '    Me.txToBin.Visible = True
                    '    Me.lbToBin.Visible = True
                    '    Me.lbPrompt.Text = "Enter To Bin Location"
                    '    Common.JavaScriptSetFocus(Page, Me.txToBin)
                    'End If
                Else
                    Me.lbError.Text = "Command Error occurred while validating Pallet #! Check battery and wireless connection - then try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                End If
            Else
                Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
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

    Protected Sub txConfirm_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txConfirm.TextChanged
        'Process the screen depending on Confirm answer Y or N
        Dim NumRowsAffected As Integer = 0
        Dim sqlCmdUpdate As New System.Data.SqlClient.SqlCommand
        Dim dtmNow As DateTime = Now

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Select Case UCase(Trim(txConfirm.Text))
            Case Is = "Y"
                Try
                    If Not DB.MakeSQLConnection("Warehouse") Then
                        sqlCmdUpdate = DB.NewSQLCommand2("spUpdtPalletQCHold")
                        sqlCmdUpdate.Parameters.AddWithValue("@pnPallet", CLng(Me.txPallet.Text))
                        sqlCmdUpdate.Parameters.AddWithValue("@pdtmScanned", dtmNow)
                        sqlCmdUpdate.Parameters.AddWithValue("@pstrUnit", Common.GetVariable("UserID", Page).ToString)
                        If sqlCmdUpdate Is Nothing Then
                            sqlCmdUpdate.Dispose()
                            DB.KillSQLConnection()
                            Me.lbError.Text = "Command Error occurred while updating pallet status! Check battery and Wireless connection - then try again or see your supervisor."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txConfirm)
                            Exit Sub
                        End If

                        'Run Command
                        sqlCmdUpdate.ExecuteNonQuery()

                        'Clean up resources from SQL Command
                        sqlCmdUpdate.Dispose() : sqlCmdUpdate = Nothing
                        DB.KillSQLConnection()

                        Call InitProcess()

                        'If NumRowsAffected = 0 Then
                        '    Me.lbError.Text = "Pallet " & Me.txPallet.Text & " status was NOT updated. Please Confirm and Process the pallet again."
                        '    Me.lbError.Visible = True
                        '    Me.txConfirm.Text = ""
                        '    Common.JavaScriptSetFocus(Page, Me.txConfirm)

                        'ElseIf NumRowsAffected > 0 Then
                        '    'Pallet was updated and can init the screen for the next pallet
                        '    Call InitProcess()
                        'End If
                    End If

                Catch ex As Exception
                    Me.lbError.Text = "Please show supervisor this Error - " & ex.Message.ToString & " - that occurred while updating Pallet " & Me.txPallet.Text & "."
                    Me.lbError.Visible = True
                    Me.txConfirm.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txConfirm)
                Finally
                    If Not sqlCmdUpdate Is Nothing Then
                        'Clean up resources from SQL Command
                        sqlCmdUpdate.Dispose() : sqlCmdUpdate = Nothing
                    End If
                    DB.KillSQLConnection()
                End Try

            Case Is = "N"
                'Setting up screen for start of Next Pallet
                Call InitProcess()
            Case Else
                Me.lbError.Text = "Entry must be Y or N to confirm for QC Hold status change."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txConfirm)
        End Select
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