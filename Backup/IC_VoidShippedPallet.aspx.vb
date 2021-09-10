Partial Public Class IC_VoidShippedPallet
    Inherits System.Web.UI.Page

    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _status As String
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
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
            'Prompt Error and Set Focus to Pallet TextBox
            If RTrim(Me.txPallet.Text).Length < 1 Then
                Me.lbError.Text = "Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Me.txPallet.Text = ""

        _pallet = Nothing
        _status = Nothing
        _dateentered = Nothing
        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdVoidShippedPallet As New System.Data.SqlClient.SqlCommand

        Try
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                'Pallet # entered is valid length check numeric
                iPallet = CLng(RTrim(Me.txPallet.Text))

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
                        Else
                            If dsPallet.Tables(0).Rows.Count > 0 Then
                                _pallet = Me.txPallet.Text
                                _status = dsPallet.Tables(0).Rows(0).Item("Status")

                                If RTrim(_status) = "V" Then 'Pallet is void -  Throw message
                                    Me.lbError.Text = "Pallet already processed as shipped. Press Restart Entry button to continue."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                                ElseIf RTrim(_status) = "Q" Then
                                    Me.lbError.Text = "Pallet on QC Hold and can't be processed with this function. Report this pallet to your supervisor."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                                Else 'Pallet is in correct status to proceed
                                    _dateentered = Now()
                                    Dim sqlVoidShippedPallet As String = "spIC_VoidShippedPallets "
                                    If Not DB.MakeSQLConnection("Warehouse") Then
                                        sqlCmdVoidShippedPallet = DB.NewSQLCommand2(sqlVoidShippedPallet)
                                        If Not sqlCmdVoidShippedPallet Is Nothing Then
                                            sqlCmdVoidShippedPallet.Parameters.AddWithValue("@pnPallet", CLng(Me.txPallet.Text))
                                            sqlCmdVoidShippedPallet.Parameters.AddWithValue("@pdtmScanned", _dateentered)
                                            sqlCmdVoidShippedPallet.Parameters.AddWithValue("@pstrUnit", Common.GetVariable("UserID", Page).ToString)
                                            sqlCmdVoidShippedPallet.ExecuteNonQuery()
                                            sqlCmdVoidShippedPallet.Dispose() : sqlCmdVoidShippedPallet = Nothing
                                            'Reset screen values to default and start process over
                                            Call InitProcess()
                                        Else
                                            Me.lbError.Text = "Command Error occurred while processing shipped pallet! Check Wireless Connection and Battery then try again or see your supervisor."
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                                        End If
                                    End If
                                End If
                            Else 'Record does not exist for Pallet entered
                                    Me.lbError.Text = "Pallet# not in system - see supervisor."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                            End If
                        End If
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

            Else
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."
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

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class