Partial Public Class IC_PhysicalCountValidate
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _printer As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _component As String
    Public _status As String
    Public strURL As String = Nothing
    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString.ToString
    Private sqlConn As New SqlClient.SqlConnection(sConnString)

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
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page)
        lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = ConfigurationManager.AppSettings.Get("Warehouse").ToString
        If Not Page.IsPostBack Then
            'Common.SaveVariable("returnURL", "~/IC_Menu.aspx", Page)
            Call InitProcess() 'Setting up screen for start of Pallet
        End If
    End Sub

    Public Sub InitProcess()

        'Clear Session variables for this page
        Common.SaveVariable("newURL", Nothing, Page)

        btRestart.Visible = True
        Me.txQuantity.Visible = False
        Me.lbQuantity.Visible = False
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Click the Warehouse you are validating"
    End Sub

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify the Qty is entered and is numeric
            If Me.txQuantity.Text.Length > 0 And IsNumeric(Me.txQuantity.Text) = True Then
                'Quantity passed all test - proceeed to Bin Location Entry
                Me.btEditQty.Text = "Save Qty"
                Me.lbBin.Visible = True
                Me.txBin.Text = ""
                Me.lbPrompt.Text = "Enter Bin Location"
                Common.JavaScriptSetFocus(Page, Me.txBin)
            Else
                Me.lbError.Text = "Quantity entered must be numeric. Try again."
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

    Protected Sub txBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txBin.TextChanged
        Try
            'Verify the Bin Location entered is valid in database
            If Not DB.MakeSQLConnection("Warehouse") Then
                Dim sqlCmdBin As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.BinLookup")
                If Not sqlCmdBin Is Nothing Then
                    sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txBin.Text)
                    _tobin = sqlCmdBin.ExecuteScalar()
                    sqlCmdBin.Dispose() : sqlCmdBin = Nothing
                    DB.KillSQLConnection()
                Else
                    DB.KillSQLConnection()
                    Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                    Me.lbError.Text = "Error occurred while validating Bin Location - try again or see your supervisor!"
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txBin)
                    Exit Sub
                End If
            Else
                DB.KillSQLConnection()
                Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                Me.lbError.Text = "Communication Error occurred while validating Bin Location - try again or see your supervisor!"
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txBin)
                Exit Sub
            End If

            If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
                Me.lbError.Text = "Invalid Bin Location entered. Try again."
                Me.lbError.Visible = True
                'Save the bad Bin Location to display on screen for correction
                Common.SaveVariable("ToBin", Me.txBin.Text, Page)
                Common.JavaScriptSetFocus(Page, Me.txBin)
                Exit Sub
            Else 'Bin Location is valid
                Common.SaveVariable("ToBin", UCase(Me.txBin.Text), Page)
                Me.txPrinter.Visible = True
                Me.lbPrinter.Visible = True
                Me.lbPrompt.Text = "Enter Printer #"
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                Me.lbError.Text = ""
                Me.lbError.Visible = False
            End If
        Catch ex As Exception
            Me.lbError.Text = "An error occurred during Bin validation - Try again."
            Me.lbError.Visible = True
            Common.SaveVariable("ToBin", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txBin)
        End Try
    End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify printer # 
            If Me.txPrinter.Text.Length > 0 And IsNumeric(Me.txPrinter.Text) = True Then
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
                            'Save the bad Bin Location to display on screen for correction
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                        Else 'Printer is valid
                            Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                            If WriteTransactions() = True Then
                                Call InitProcess()
                            Else
                                Me.lbError.Text = "Transaction failed to complete. Try again."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            End If
                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    End If
                Else
                    Me.lbError.Text = "Communication Error occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                End If
            Else
                Me.lbError.Text = "Printer # should be a number. Try again."
                Me.lbError.Visible = True
                'Save the bad Printer# to display on screen for correction
                Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPrinter)
        Finally
            If Not sqlCmdPrinter Is Nothing Then
                sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function WriteTransactions() As Boolean
        WriteTransactions = False
        'Me.lbCompGTIN.Text = sGTIN
        'Me.lbCompVer.Text = Right("000" + sVer, 3).ToString
        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String
        Dim _trantype As String = "RETURN"
        Dim _caselabel As String
        Dim _prodvariant As String = Me.lbCompVer.Text
        Dim _strCompGTIN As String = Me.lbCompGTIN.Text
        Dim _strCompCodeDate As String = Common.GetVariable("CompCodeDate", Page).ToString


        'Build CaseLabelString
        _caselabel = "01" & _strCompGTIN.ToString & "15" & _strCompCodeDate.ToString & "10999999999999240" & _prodvariant.ToString
        sqlConn.Open()
        sSqlString = "Insert into IC_Trans (TransactionType,pk_nPallet,CaseLabel,Company,PalletQty,StartTime,StopTime,Bin,UserID,Status,txDateTimeEntered,txDateTimeModified,printer,whs,ProdVariant) VALUES ('" & _trantype & "',0,'" & _caselabel & "' ,'1' ," & Me.txQuantity.Text & " ,'0000' ,'0000' ,'" & Me.txBin.Text & "','" & Common.GetVariable("UserID", Page).ToString & "', 'A','" & Now.Date & "','" & Now.Date & "','" & Me.txPrinter.Text & "','10','" & _prodvariant & "')"
        cmdSqlCommand.CommandText = sSqlString
        cmdSqlCommand.Connection = sqlConn
        cmdSqlCommand.ExecuteNonQuery()
        sqlConn.Close()
        WriteTransactions = True

    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub gvScanCounts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gvScanCounts.SelectedIndexChanged
        Me.lbCompDesc.Text = Me.gvScanCounts.SelectedRow.Cells(1).Text
        Dim sGTIN As String = Nothing
        Dim sVer As String = Nothing

        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "SELECT [GTIN],[Version] FROM [Warehouse].[dbo].[vwIC_Components] Where [Component] = '" & RTrim(Me.lbCompDesc.Text) & "'"
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            sGTIN = ds.Tables("Data").Rows(0).Item(0).ToString
            sVer = ds.Tables("Data").Rows(0).Item(1).ToString

        Catch ex As Exception
            DB.KillSQLConnection()
        Finally
            DB.KillSQLConnection()
        End Try

        Me.lbCompGTIN.Text = sGTIN
        Me.lbCompVer.Text = Right("000" + sVer, 3).ToString
        Me.gvComponents.Visible = False
        Me.gvComponents.EditIndex = -1
        Me.lbCompInfo.Visible = True
        Me.lbCompDesc.Visible = True

        Me.lbCodeDate.Visible = True
        Me.txCodeDate.Visible = True
        Me.txCodeDate.Text = ""
        Me.lbPrompt.Text = "Enter Code Date in YYMMDD format. "
        Common.JavaScriptSetFocus(Page, Me.txCodeDate)
    End Sub

    Public Sub BindGrid()
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            sqlString = "SELECT BinLocation, AXItem, ProdDesc, Quantity From [dbo].[IC_FGCounts] Where AX_Whse = " & CInt(Me.lbWhse.Text)
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            Me.gvScanCounts.DataSource = dv
            Me.gvScanCounts.DataBind()

            If dv.Count > 0 Then
                Me.gvScanCounts.Visible = True
            Else
                Me.gvScanCounts.Visible = False
                Me.lbError.Text = "Scan Counts List is empty! Call your IT Department."
                Me.lbError.Visible = True
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btPackland_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPackland.Click
        Me.btGlacier.Visible = False
        Me.btPlant2.Visible = False
        Me.lbWhse.Text = "30"

        'Bind Grid with scans for Packland

    End Sub

    Protected Sub btPlant2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPlant2.Click
        Me.btGlacier.Visible = False
        Me.btPackland.Visible = False
        Me.lbWhse.Text = "20"

        'Bind Grid with scans for Plant2

    End Sub

    Protected Sub btGlacier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGlacier.Click
        Me.btPackland.Visible = False
        Me.btPlant2.Visible = False
        Me.lbWhse.Text = "60"

        'Bind Grid with scans for Glacier

    End Sub

    Protected Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class