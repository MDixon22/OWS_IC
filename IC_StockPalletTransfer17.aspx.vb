Partial Public Class IC_StockPalletTransfer17
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String

    Public _shippingreceipt As String = Nothing
    Public _pallettype As String = Nothing
    Public _bin As String = Nothing
    Public iPallet As Integer
    Public _status As String
    Public _userid As String
    Public _prod As Integer
    Public _version As Integer
    Public _palletqty As Integer
    Public _shipdate As DateTime

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
            Call InitProcess() 'Setting up screen for selection of Ship Date
            Common.JavaScriptSetFocus(Page, Me.txTruckBin)
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("Pallet", Nothing, Page)
        Common.SaveVariable("Status", Nothing, Page)
        Common.SaveVariable("ToBin", Nothing, Page)
        Common.SaveVariable("SelectedDate", Nothing, Page)

        Me.lbSelectShipDate.Visible = True
        Me.lbSelectedShipDate.Visible = True
        Me.lbSelectedShipDate.Text = Now.ToShortDateString

        Me.txPallet.Text = ""
        Me.txTruckBin.Text = ""

        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Use < > buttons to adjust Ship Date then Scan or Enter Truck Bin Location."
        Common.JavaScriptSetFocus(Page, Me.txTruckBin)
    End Sub

    Public Sub NextStockPallet()
        Common.SaveVariable("Pallet", Nothing, Page)
        Common.SaveVariable("Status", Nothing, Page)

        Me.txPallet.Text = ""

        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter next Stock Pallet or press the To Menu button if finished."
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txTruckBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txTruckBin.TextChanged
        'Validate Bin is type CTruck
        Dim sWhse As String = Nothing
        Try
            'Validate Bin
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to database failed. Restart screen and try again.""")
            Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
            sqlCmdBin = DB.NewSQLCommand("SQL.Query.OTruckLookup")
            If sqlCmdBin Is Nothing Then Throw New Exception("""Transfer Truck Lookup Command failed. Restart screen and try again.""")
            sqlCmdBin.Parameters.AddWithValue("@Bin", UCase(Trim(Me.txTruckBin.Text)))
            sWhse = sqlCmdBin.ExecuteScalar()
            sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            DB.KillSQLConnection()
            If sWhse Is Nothing Then Throw New Exception("""Transfer Truck Lookup failed validation. Check entry and try again.""")

            'If Transfer Truck lookup is true then save variable for future use
            Common.SaveVariable("ToBin", UCase(Me.txTruckBin.Text), Page)
            Me.txPallet.Text = ""
            Me.txPallet.Visible = True
            Me.lbPallet.Visible = True
            Me.lbPrompt.Text = "Scan or Enter Stock Pallet."
            Common.JavaScriptSetFocus(Page, Me.txPallet)

        Catch ex As Exception
            lbError.Text = "Try again - Transfer Truck Lookup - Entry Error = " & ex.Message.ToString
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txTruckBin)
        End Try


    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim iReturnValue As Integer = 0

        Try
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False


            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                'Pallet # entered is valid length check numeric
                iPallet = CInt(Me.txPallet.Text)

                'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
                If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to database failed when validating Stock Pallet. Restart screen and try again.""")

                sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetStockPalletInfo")
                If sqlCmdPallet Is Nothing Then Throw New Exception("""Stock Pallet Lookup Command failed. Restart screen and try again.""")

                sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
                dsPallet = DB.GetDataSet(sqlCmdPallet)
                sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                DB.KillSQLConnection()
                If dsPallet Is Nothing Then Throw New Exception("""Data Error occurred while validating Stock Pallet #! Check battery and wireless connection - then try again.""")

                Common.JavaScriptSetFocus(Page, Me.txPallet)

                If dsPallet.Tables(0).Rows.Count < 1 Then Throw New Exception("""Stock Pallet# not found in system- see supervisor.""")

                _pallettype = dsPallet.Tables(0).Rows(0).Item("PalletType")
                _bin = UCase(Me.txTruckBin.Text)
                _status = dsPallet.Tables(0).Rows(0).Item("Status")
                _userid = Common.GetVariable("UserID", Page).ToString
                _prod = CInt(dsPallet.Tables(0).Rows(0).Item("Prod"))
                _version = CInt(dsPallet.Tables(0).Rows(0).Item("Version"))
                _palletqty = CInt(dsPallet.Tables(0).Rows(0).Item("PalletQty"))
                _shipdate = CDate(Me.lbSelectedShipDate.Text)

                If _bin = Me.lbTruckBin.Text Then Throw New Exception("""Pallet# already scanned to the truck.""")

                If _pallettype <> "I" Then Throw New Exception("""Pallet# entered is not a Stock Pallet - try again or see supervisor.""")

                Select Case UCase(RTrim(_status))
                    Case Is = "V"
                        Throw New Exception("""Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate.""")
                    Case Is = "H"
                        Throw New Exception("""Pallet Status is QC Hold. Pallet movement not allowed - see supervisor.""")
                    Case Is = "W"
                        Throw New Exception("""Pallet Status is Warehouse Hold. Pallet movement not allowed - see supervisor.""")
                    Case Is = "C"
                        Throw New Exception("""Pallet is not a Stock Pallet. Pallet movement not allowed - see supervisor.""")
                    Case Else
                        Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                        Common.SaveVariable("Status", _status, Page)

                        _dateentered = Now()
                        Dim sqlInsertStockPalletTransfer As String = "spInsertStockPalletTransfer "
                        If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Communication Error occurred while Loading Stock Pallet on Transfer Truck! Check Wireless Connection and Battery then try again or see your supervisor.""")

                        Dim sqlCmdInsertStockPalletTransfer As New System.Data.SqlClient.SqlCommand
                        sqlCmdInsertStockPalletTransfer = DB.NewSQLCommand2(sqlInsertStockPalletTransfer)
                        If sqlCmdInsertStockPalletTransfer Is Nothing Then Throw New Exception("""Pallet is not a Stock Pallet. Pallet movement not allowed - see supervisor.""")

                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@sBin", _bin)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@iPallet", iPallet)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@sStatus", _status)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@sUserID", _userid)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@iProd", _prod)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@iVersion", _version)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@iPalletQty", _palletqty)
                        sqlCmdInsertStockPalletTransfer.Parameters.AddWithValue("@dShipDate", _shipdate)
                        sqlCmdInsertStockPalletTransfer.ExecuteNonQuery()
                        sqlCmdInsertStockPalletTransfer.Dispose() : sqlCmdInsertStockPalletTransfer = Nothing

                        'Reset screen for next Stock Pallet to Transfer
                        Call NextStockPallet()

                End Select
            Else
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Stock Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
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

    Protected Sub btMinus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btMinus.Click
        Me.lbSelectedShipDate.Text = CDate(Me.lbSelectedShipDate.Text).AddDays(-1)
    End Sub

    Protected Sub btPlus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPlus.Click
        Me.lbSelectedShipDate.Text = CDate(Me.lbSelectedShipDate.Text).AddDays(1)
    End Sub
End Class