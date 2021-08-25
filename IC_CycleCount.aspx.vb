Partial Public Class IC_CycleCount
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
    Public _SupOver As String
    Public _SupOverName As String
    Public _active As String

    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString.ToString

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify the user is logged in 
        Common.CheckLogin(Page)
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        Me.lbPageTitle.Text = ConfigurationManager.AppSettings.Get("Title").ToString & "Pallet Counting"
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        'SAM 4/10/2018 - Changed to use login user warehouse
        _whs = Common.GetVariable("Whse", Page).ToString

        Try
            If Not Page.IsPostBack Then
                Call InitBatch() 'Setting up screen for start of Pallet
                Common.JavaScriptSetFocus(Page, Me.txBatch)
            Else 'Page is posted back 
                'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
                'Prompt Error and Set Focus to Pallet TextBox
                If RTrim(Me.txBatch.Text).Length < 1 Then
                    Me.lbError.Text = "Batch # needs to be entered."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txBatch)
                    Exit Sub
                End If
                Me.lbProductDescription.Text = Common.GetVariable("ProdDesc", Page)
                Me.lbBatchVal.Text = Common.GetVariable("BatchVal", Page)
                Me.lbBatchVal.Visible = True

            End If
        Catch ex As Exception
            Me.lbError.Text = "Error - " & ex.Message.ToString & " - occurred while loading page. Try process again or show your supervisor!"
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        End Try
    End Sub

    Public Sub InitBatch()
        Try
            Common.SaveVariable("BatchID", Nothing, Page)
            Common.SaveVariable("BatchVal", Nothing, Page)
            Common.SaveVariable("Pallet", Nothing, Page)
            Common.SaveVariable("ProdDesc", Nothing, Page)
            Common.SaveVariable("Qty", Nothing, Page)
            Common.SaveVariable("Bin", Nothing, Page)
            Common.SaveVariable("NewQty", Nothing, Page)
            Common.SaveVariable("Status", Nothing, Page)

            Me.txBatch.Text = ""
            Me.lbBatch.Visible = True
            Me.lbBatchVal.Visible = False
            Me.lbBatchVal.Text = ""
            Me.lbProductDescription.Visible = False
            Me.txPallet.Visible = False
            Me.lbPallet.Visible = False
            Me.lbCaseQty.Visible = False
            Me.txCaseQty.Visible = False
            Me.lbNewCaseQty.Visible = False
            Me.txNewCaseQty.Visible = False
            Me.txToBin.Visible = False
            Me.lbToBin.Visible = False
            Me.txPallet.Text = ""
            Me.txCaseQty.Text = ""
            Me.txNewCaseQty.Text = ""
            Me.txToBin.Text = ""

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
            Me.btRestart.Visible = False
            Me.btReturn.Visible = True

            Me.lbPrompt.Text = "Enter Batch #"
            Common.JavaScriptSetFocus(Page, Me.txBatch)

        Catch ex As Exception
            Me.lbError.Text = "Try again - Init Batch Error = " & ex.Message.ToString
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txBatch)
        End Try
    End Sub

    Public Sub InitNewPallet()
        Try
            Common.SaveVariable("Pallet", Nothing, Page)
            Common.SaveVariable("ProdDesc", Nothing, Page)
            Common.SaveVariable("Qty", Nothing, Page)
            Common.SaveVariable("Bin", Nothing, Page)
            Common.SaveVariable("NewQty", Nothing, Page)
            Common.SaveVariable("Status", Nothing, Page)


            Me.txPallet.Visible = True
            Me.lbPallet.Visible = True
            Me.lbProductDescription.Visible = False
            Me.lbCaseQty.Visible = False
            Me.txCaseQty.Visible = False
            Me.lbNewCaseQty.Visible = False
            Me.txNewCaseQty.Visible = False
            Me.txToBin.Visible = False
            Me.lbToBin.Visible = False
            Me.txYN.Visible = False
            Me.lbYN.Visible = False
            Me.txToBin.Text = ""
            Me.txPallet.Text = ""
            Me.txCaseQty.Text = ""
            Me.txNewCaseQty.Text = ""

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
            Me.btRestart.Visible = True
            Me.btReturn.Visible = True


            Me.lbPrompt.Text = "Scan or Enter Pallet#"
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Catch ex As Exception
            Me.lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        End Try
    End Sub

    Protected Sub txBatch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txBatch.TextChanged
        Dim dsBatch As New Data.DataSet
        Dim sqlCmdBatch As New System.Data.SqlClient.SqlCommand
        Dim regexBatch As New Regex("^\d+$")
        Try
            Common.SaveVariable("BatchID", Me.txBatch.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iBatch As Int64 = 0

            If RTrim(Me.txBatch.Text).Length < 1 Then
                Common.SaveVariable("BatchID", "", Page)
                Me.lbError.Text = "Batch # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txBatch)
                Exit Try
            End If

            'Use Regular expression \d to verify that string contains only numbers between 0-9
            If regexBatch.IsMatch(RTrim(Me.txBatch.Text)) = False Then
                Common.SaveVariable("BatchID", "", Page)
                Me.txPallet.Text = ""
                Me.lbError.Text = "Batch # contains an invalid character, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txBatch)
                Exit Try
            End If

            iBatch = CLng(Me.txBatch.Text)
            Dim strBatchLookup As String = "SELECT * FROM [dbo].[IC_BatchCounts] WHERE ([BatchNo] = " & iBatch & ") AND ([Active] = 1)"
            'open database connection
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection not established""")
            'get query object
            sqlCmdBatch = DB.NewSQLCommand3(strBatchLookup)
            If sqlCmdBatch Is Nothing Then Throw New Exception("""Query name invalid""")
            dsBatch = DB.CreateDataSet(sqlCmdBatch)

            sqlCmdBatch.Dispose() : sqlCmdBatch = Nothing
            DB.KillSQLConnection()

            If dsBatch Is Nothing Then Throw New Exception("""Batch info is invalid""")
            If dsBatch.Tables.Count < 1 Then Throw New Exception("""Batch data is empty""")
            If dsBatch.Tables(0).Rows.Count < 1 Then Throw New Exception("""Batch does not exist""")

            _active = dsBatch.Tables(0).Rows(0).Item("Active")

            If _active = 0 Then
                Me.lbError.Text = "Batch is not active. Please check KnowledgeMine."
                Me.lbError.Visible = True
            Else 'Batch is active  -  ok to proceed 
                Me.lbBatchVal.Text = dsBatch.Tables(0).Rows(0).Item("BatchDescription")
                Common.SaveVariable("BatchVal", dsBatch.Tables(0).Rows(0).Item("BatchDescription"), Page)
                Common.SaveVariable("BatchID", Me.txBatch.Text, Page)
                Me.lbBatchVal.Visible = True

                InitNewPallet()
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Finally
            If Not dsBatch Is Nothing Then
                dsBatch.Dispose() : dsBatch = Nothing
            End If
            If Not sqlCmdBatch Is Nothing Then
                sqlCmdBatch.Dispose() : sqlCmdBatch = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        Dim regexPallet As New Regex("^\d+$")
        Try
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Int64 = 0

            If RTrim(Me.txPallet.Text).Length < 1 Then
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Try
            End If

            'Use Regular expression \d to verify that string contains only numbers between 0-9
            If regexPallet.IsMatch(RTrim(Me.txPallet.Text)) = False Then
                Common.SaveVariable("Pallet", "", Page)
                Me.txPallet.Text = ""
                Me.lbError.Text = "Pallet # contains an invalid character, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Try
            End If

            iPallet = CLng(Me.txPallet.Text)

            'open database connection
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection not established""")
            'get query object
            Dim strPalletLookup As String = "SELECT * FROM [vwICPalletsSumLookup_AllInv] WHERE ([Pallet] = " & iPallet & ")"
            sqlCmdPallet = DB.NewSQLCommand3(strPalletLookup)
            If sqlCmdPallet Is Nothing Then Throw New Exception("""Query name invalid""")
            dsPallet = DB.CreateDataSet(sqlCmdPallet)

            'Dispose of objects used during command execution
            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            DB.KillSQLConnection()

            If dsPallet Is Nothing Then Throw New Exception("""Pallet info is invalid""")
            If dsPallet.Tables.Count < 1 Then Throw New Exception("""Pallet data is empty""")
            If dsPallet.Tables(0).Rows.Count < 1 Then Throw New Exception("""Pallet does not exist""")

            _pallet = Me.txPallet.Text
            _status = dsPallet.Tables(0).Rows(0).Item("Status")

            If RTrim(_status) = "H" Then
                Me.lbError.Text = "Pallet Status is QAHold. Make sure to enter Bin Location for QA Hold Pallets"
                Me.lbError.Visible = True
            End If

            If RTrim(_status) = "V" Then 'Pallet is void -  Throw an error
                Me.lbError.Text = "Pallet Status is Void. Do you want to Activate the Pallet"
                Me.lbError.Visible = True
                Me.lbYN.Visible = True
                Me.txYN.Visible = True
                Me.txYN.Text = ""
                Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                Common.SaveVariable("Status", _status, Page)
                Common.SaveVariable("Qty", dsPallet.Tables(0).Rows(0).Item("Qty"), Page)
                Common.SaveVariable("Bin", dsPallet.Tables(0).Rows(0).Item("Bin"), Page)

                If dsPallet.Tables(0).Rows(0).Item("PalletType").ToString = "I" Then
                    Me.lbProductDescription.Text = dsPallet.Tables(0).Rows(0).Item("ProdDesc")
                    Common.SaveVariable("ProdDesc", dsPallet.Tables(0).Rows(0).Item("ProdDesc"), Page)
                    Me.lbProductDescription.Visible = True
                End If
                Common.JavaScriptSetFocus(Page, Me.txYN)
                'Common.JavaScriptSetFocus(Page, Me.txPallet)
            Else 'Pallet is in correct status to proceed
                Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                Common.SaveVariable("Status", _status, Page)
                Common.SaveVariable("Qty", dsPallet.Tables(0).Rows(0).Item("Qty"), Page)
                Common.SaveVariable("Bin", dsPallet.Tables(0).Rows(0).Item("Bin"), Page)
                Common.SaveVariable("PalletType", dsPallet.Tables(0).Rows(0).Item("PalletType").ToString, Page)

                If dsPallet.Tables(0).Rows(0).Item("PalletType").ToString = "I" Then
                    Me.lbProductDescription.Text = dsPallet.Tables(0).Rows(0).Item("ProdDesc")
                    Common.SaveVariable("ProdDesc", dsPallet.Tables(0).Rows(0).Item("ProdDesc"), Page)
                    Me.lbProductDescription.Visible = True
                    Me.txCaseQty.Visible = True
                    Me.lbCaseQty.Visible = True
                    Me.txToBin.Visible = False
                    Me.lbToBin.Visible = False
                    Me.lbPrompt.Text = "Enter Case Quantity"
                    Common.JavaScriptSetFocus(Page, Me.txCaseQty)
                ElseIf dsPallet.Tables(0).Rows(0).Item("PalletType").ToString = "S" Then
                    Me.txToBin.Visible = True
                    Me.lbToBin.Visible = True
                    Me.lbPrompt.Text = "Enter Bin Location"
                    Common.JavaScriptSetFocus(Page, Me.txToBin)
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Finally
            'Dispose of objects used during command execution
            If Not dsPallet Is Nothing Then
                dsPallet.Dispose() : dsPallet = Nothing
            End If
            If Not sqlCmdPallet Is Nothing Then
                sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txYN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txYN.TextChanged
        Select Case UCase(Me.txYN.Text)
            Case Is = "Y"
                _status = "A"

                Dim sqlCmd As New SqlClient.SqlCommand
                Dim ErrorResult As Integer = 0

                Try
                    Dim strPalletActivated As String = "EXEC spActivatePallet " & CLng(Me.txPallet.Text) & ",'" & _status & "','" & Common.GetVariable("UserID", Page) & "',0"
                    If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection not established")
                    sqlCmd = DB.NewSQLCommand3(strPalletActivated)
                    If sqlCmd Is Nothing Then Throw New Exception("Failed to create query")
                    
                    ErrorResult = sqlCmd.ExecuteScalar

                    'Dispose of objects used during command execution
                    sqlCmd.Dispose() : sqlCmd = Nothing
                    DB.KillSQLConnection()

                    If ErrorResult <> 0 Then Throw New Exception("SQL stored procedure returned value '" & ErrorResult & "'")

                    Me.lbYN.Visible = False
                    Me.txYN.Visible = False
                    Me.txYN.Text = ""
                    Me.txCaseQty.Visible = True
                    Me.lbCaseQty.Visible = True
                    Me.lbProductDescription.Visible = True
                    Me.txToBin.Visible = False
                    Me.lbToBin.Visible = False
                    Me.lbPrompt.Text = "Enter Case Quantity"
                    Me.lbError.Text = ""
                    Me.lbError.Visible = False
                    Common.JavaScriptSetFocus(Page, Me.txCaseQty)
                Catch ex As Exception
                    Me.lbError.Text = "Error occured during Pallet Activation - " & ex.Message & ". Try entry again."
                    Me.lbError.Visible = True
                    Me.txYN.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txYN)
                Finally
                    'Dispose of objects used during command execution
                    If Not sqlCmd Is Nothing Then
                        sqlCmd.Dispose() : sqlCmd = Nothing
                    End If
                    DB.KillSQLConnection()
                End Try

            Case Is = "N"
                'Reset screen values to default and start process over
                Call InitNewPallet()
            Case Else
                Me.lbError.Text = "Entry must be Y or N. Please re-enter"
                Me.lbError.Visible = True
                Me.txYN.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txYN)
        End Select
    End Sub

    Protected Sub txCaseQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCaseQty.TextChanged
        'Test Entry compared to Saved Quantity
        Dim regexQty As New Regex("^\d+$")
        Try
            If Me.txCaseQty.Text.Length < 1 Then
                Common.SaveVariable("NewQty", "", Page)
                Me.txCaseQty.Text = ""
                Me.lbError.Text = "Quantity must be entered, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCaseQty)
                Throw New Exception("""Quantity not entered""")
                Exit Try
            End If

            'Use Regular expression \d to verify that string contains only numbers between 0-9
            If regexQty.IsMatch(RTrim(Me.txCaseQty.Text)) = False Then
                Common.SaveVariable("NewQty", "", Page)
                Me.txCaseQty.Text = ""
                Me.lbError.Text = "Quantity entered contains an invalid character, try again."
                Me.lbError.Visible = True
                Throw New Exception("""Quantity entered contains an invalid character""")
                Common.JavaScriptSetFocus(Page, Me.txCaseQty)
                Exit Try
            End If

            If CInt(Common.GetVariable("Qty", Page)) <> CInt(Me.txCaseQty.Text) Then
                Me.lbPrompt.Text = "Please enter case quantity again for validation"
                Me.lbNewCaseQty.Visible = True
                Me.txNewCaseQty.Visible = True
                Me.lbProductDescription.Visible = True
                Me.lbBatchVal.Visible = True
                Me.lbCaseQty.Visible = False
                Me.txCaseQty.Visible = False
                Common.JavaScriptSetFocus(Page, Me.txNewCaseQty)

            Else 'Quantity matches what is in the system proceed to Bin Location entry
                Me.lbPrompt.Text = "Please enter Bin Location"
                Me.lbToBin.Visible = True
                Me.txToBin.Visible = True
                Me.lbProductDescription.Visible = True
                Me.lbBatchVal.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txToBin)
            End If
            Common.SaveVariable("NewQty", Me.txCaseQty.Text, Page)
        Catch ex As Exception
            Me.lbError.Text = "Error occured during Case Qty entry - " & ex.Message & ". Try entry again."
            Me.lbError.Visible = True
            Me.txCaseQty.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txCaseQty)
        End Try
    End Sub

    Protected Sub txNewCaseQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txNewCaseQty.TextChanged
        Dim regexQty As New Regex("^\d+$")
        Me.lbProductDescription.Visible = True
        Me.lbBatchVal.Visible = True
        Try
            If Me.txNewCaseQty.Text.Length < 1 Then
                Me.txNewCaseQty.Text = ""
                Me.lbError.Text = "Quantity must be entered, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txNewCaseQty)
                Throw New Exception("""Quantity not entered""")
                Exit Try
            End If

            'Use Regular expression  "^\d+$"  to verify that string contains only numbers between 0-9
            If regexQty.IsMatch(RTrim(Me.txNewCaseQty.Text)) = False Then
                Me.txNewCaseQty.Text = ""
                Me.lbError.Text = "Quantity entered contains an invalid character, try again."
                Me.lbError.Visible = True
                Throw New Exception("""Quantity entered contains an invalid character""")
                Common.JavaScriptSetFocus(Page, Me.txNewCaseQty)
                Exit Try
            End If

            If CInt(Common.GetVariable("NewQty", Page)) <> CInt(Me.txNewCaseQty.Text) Then
                Me.lbPrompt.Text = "Quantities entered are not the same. Press Restart Entry to start pallet again."
                Me.lbNewCaseQty.Visible = True
                Me.txNewCaseQty.Visible = True
                Me.lbProductDescription.Visible = True
                Me.lbCaseQty.Visible = False
                Me.txCaseQty.Visible = False
                Common.JavaScriptSetFocus(Page, Me.txNewCaseQty)

            Else 'Quantity matches what is in the system proceed to Bin Location entry
                Me.lbPrompt.Text = "Please enter Bin Location"
                Me.lbToBin.Visible = True
                Me.txToBin.Visible = True
                Me.lbProductDescription.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txToBin)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error occured during New Case Qty entry - " & ex.Message & ". Try entry again."
            Me.lbError.Visible = True
            Me.txNewCaseQty.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txNewCaseQty)
        End Try
    End Sub

    Protected Sub txToBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txToBin.TextChanged
        'Verify the Bin Location entered is valid in database
        Me.lbProductDescription.Visible = True
        Me.lbBatchVal.Visible = True
        Try
            If Me.ValidLocation(Trim(Me.txToBin.Text)) = True Then
                If PalletCounted(Me.txPallet.Text, UCase(Me.txToBin.Text)) = True Then
                    Call InitNewPallet()
                Else
                    Me.lbError.Text = "Transaction failed to complete. Check battery and Wireless connection - then press Restart Entry button to process pallet again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txToBin)
                    Exit Sub
                End If
            Else
                Common.SaveVariable("Bin", "", Page)
                Me.lbError.Text = "Bin does not exist in the system - Scan again or see your supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txToBin)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & "! Try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txToBin)
        End Try
    End Sub

    Public Function PalletCounted(ByVal _pallet As Long, ByVal _bin As String) As Boolean
        PalletCounted = False
        Dim sqlCmd As New SqlClient.SqlCommand
        Dim ErrorResult As Integer = 0

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection not established")
            Dim strPalletCounted As String = "EXEC spPalletCounted " & _pallet & ",'" & _bin & "','" & Common.GetVariable("Bin", Page) & "'," & CInt(Common.GetVariable("NewQty", Page)) & _
                                             "," & CInt(Common.GetVariable("Qty", Page)) & ",'" & Common.GetVariable("UserID", Page) & "'," & CLng(Common.GetVariable("BatchID", Page)) & ",0"
            sqlCmd = DB.NewSQLCommand3(strPalletCounted)
            If sqlCmd Is Nothing Then Throw New Exception("Failed to create query")

            ErrorResult = sqlCmd.ExecuteScalar

            'Dispose of objects used during command execution
            sqlCmd.Dispose() : sqlCmd = Nothing
            DB.KillSQLConnection()

            'Throw Exception on Error returned from stored procedure
            If ErrorResult <> 0 Then Throw New Exception("SQL store procedure returned value '" & ErrorResult & "'")

            PalletCounted = True
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & "! Try again or see your supervisor."
            Me.lbError.Visible = True
        Finally
            'Dispose of objects used during command execution
            If Not sqlCmd Is Nothing Then
                sqlCmd.Dispose() : sqlCmd = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function ValidLocation(ByVal bin As String) As Boolean
        ValidLocation = False
        Dim strAXWhse As String = ""
        Dim sqlString As String = "Select AX_Whse from vwIC_Bin_AXWhse Where BIN_LOCATION = '" & bin & "'"

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
            sqlCmdBin = DB.NewSQLCommand3(sqlString)
            If sqlCmdBin Is Nothing Then Throw New Exception("""Bin Location Lookup Command failed. Press Restart Entry button to try again.""")
            strAXWhse = sqlCmdBin.ExecuteScalar()
            sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            DB.KillSQLConnection()
            If Not strAXWhse Is Nothing Then
                ValidLocation = True
                Common.SaveVariable("AX_Whse", strAXWhse, Page)
            End If
        Catch ex As Exception
            lbError.Text = "Bin Location Validation failed with Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        strURL = "~/IC_Menu.aspx"
        If Not strURL Is Nothing Then
            Page.Response.Redirect(strURL)
        End If
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitNewPallet()
    End Sub

    Protected Sub btNewBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNewBatch.Click
        InitBatch()
    End Sub

End Class