Partial Public Class IC_BinTransfertoTruck
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
        Common.SaveVariable("Pallet", Nothing, Page)
        Common.SaveVariable("Status", Nothing, Page)
        Common.SaveVariable("ToBin", Nothing, Page)

        Me.txPallet.Text = ""

        _pallet = Nothing
        _status = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

        Try
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                'Pallet # entered is valid length check numeric
                iPallet = CInt(Me.txPallet.Text)

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

                                Select Case UCase(RTrim(_status))
                                    Case Is = "V"
                                        Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                                        Me.lbError.Visible = True
                                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                                    Case Is = "Q"
                                        Me.lbError.Text = "Pallet Status is QC Hold. Pallet movement not allowed - see supervisor"
                                        Me.lbError.Visible = True
                                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                                    Case Else
                                        Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                                        Common.SaveVariable("Status", _status, Page)

                                        _dateentered = Now()
                                        Dim sqlLoadPalletsOnPacklandTruck As String = "spLoadPalletsOnPacklandTruck "
                                        If Not DB.MakeSQLConnection("Warehouse") Then
                                            Dim sqlCmdLoadPalletsOnPacklandTruck As New System.Data.SqlClient.SqlCommand
                                            sqlCmdLoadPalletsOnPacklandTruck = DB.NewSQLCommand2(sqlLoadPalletsOnPacklandTruck)
                                            If Not sqlCmdLoadPalletsOnPacklandTruck Is Nothing Then
                                                sqlCmdLoadPalletsOnPacklandTruck.Parameters.AddWithValue("@pnPallet", CLng(Me.txPallet.Text))
                                                sqlCmdLoadPalletsOnPacklandTruck.Parameters.AddWithValue("@pdtmScanned", _dateentered)
                                                sqlCmdLoadPalletsOnPacklandTruck.Parameters.AddWithValue("@pstrUnit", Common.GetVariable("UserID", Page).ToString)
                                                sqlCmdLoadPalletsOnPacklandTruck.ExecuteNonQuery()
                                                sqlCmdLoadPalletsOnPacklandTruck.Dispose() : sqlCmdLoadPalletsOnPacklandTruck = Nothing
                                                'Reset screen values to default and start process over
                                                Me.lbResult.Text = "Pallet # : " & Me.txPallet.Text & " was successfully loaded on the Packland Truck."

                                                Call InitProcess()
                                            Else
                                                Me.lbError.Text = "Command Error occurred while Loading Pallet on Packland Truck! Check Wireless Connection and Battery then try again or see your supervisor."
                                                Me.lbError.Visible = True
                                                Common.JavaScriptSetFocus(Page, Me.txPallet)
                                            End If
                                            If Not sqlCmdLoadPalletsOnPacklandTruck Is Nothing Then
                                                sqlCmdLoadPalletsOnPacklandTruck.Dispose() : sqlCmdLoadPalletsOnPacklandTruck = Nothing
                                            End If
                                        Else
                                            Me.lbError.Text = "Communication Error occurred while Loading Pallet on Packland Truck! Check Wireless Connection and Battery then try again or see your supervisor."
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                                        End If
                                End Select

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