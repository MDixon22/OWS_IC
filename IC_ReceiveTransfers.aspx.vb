Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_ReceiveTransfers
    Inherits System.Web.UI.Page

    Public _tobin As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
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
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        'SAM 4/10/2018 - Changed to use login user warehouse
        _whs = Common.GetVariable("Whse", Page).ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Transfer
            Common.JavaScriptSetFocus(Page, Me.txTransferNo)
        End If

    End Sub

    Public Sub InitProcess()
        Try
            Common.SaveVariable("newURL", Nothing, Page)
            Common.SaveVariable("Pallet", Nothing, Page)
            Common.SaveVariable("Transfer", Nothing, Page)

            Me.txTransferNo.Visible = True
            Me.txTransferNo.Text = ""
            Me.lbTransferNo.Visible = True
            Me.lbTranferNumValue.Visible = False
            Me.lbTranferNumValue.Text = ""
            Me.txPallet.Text = ""
            Me.txPallet.Visible = False
            Me.lbPallet.Visible = False
            Me.dgPalletsOnTransfer.Visible = False

            _pallet = Nothing
            _status = Nothing
            _trantype = Nothing
            _dateentered = Nothing
            _datemodified = Nothing
            strURL = Nothing

            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Me.btRestart.Visible = True
            Me.btReturn.Visible = True

            Me.lbPrompt.Text = "Scan or Enter Transfer#"
            Common.JavaScriptSetFocus(Page, Me.txTransferNo)
        Catch ex As Exception
            Me.lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txTransferNo)
        End Try
    End Sub

    Public Sub NextPallet()
        Common.SaveVariable("Pallet", Nothing, Page)
        'Added for Bin Entry process - SAM - 7/19/2013
        Common.SaveVariable("Bin", Nothing, Page)

        Me.txTransferNo.Visible = False
        Me.lbTransferNo.Visible = True
        Me.lbTranferNumValue.Visible = True
        'Me.lbTranferNumValue.Text = ""
        Me.txPallet.Text = ""
        Me.txPallet.Visible = True
        Me.lbPallet.Visible = True

        Me.dgPalletsOnTransfer.Visible = True

        _pallet = Nothing
        _status = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.btRestart.Visible = True
        Me.btReturn.Visible = True

        Me.lbPrompt.Text = "Scan or Enter Next Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txTransferNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txTransferNo.TextChanged
        'Verify the Bin Location entered is valid in database
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If Me.txTransferNo.Text.Length > 0 Then
                'Validate the Transfer #
                Dim ValidTransfer As Boolean = ValidTran(UCase(Me.txTransferNo.Text))
                If ValidTransfer <> True Then
                    Throw New Exception("Error occurred during validation of Transfer # - Try again.")
                    Exit Sub
                End If
                Me.lbTranferNumValue.Text = Me.txTransferNo.Text
                Me.lbTranferNumValue.Visible = True
                Me.txTransferNo.Visible = False
                Me.txPallet.Visible = True
                Me.lbPallet.Visible = True
                DisplayPalletsNotReceived()
                Me.lbPrompt.Text = "Scan or Enter Pallet#"
                Common.JavaScriptSetFocus(Page, Me.txPallet)

            Else 'Nothing entered on screen for Bin Location
                Me.lbError.Text = "Transfer # must be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txTransferNo)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error during Transfer # entry - " & ex.ToString & "! Try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txTransferNo)
        End Try
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
        '7/26/2011 - SAM - Changed the regular expression from \d so it would test all characters are numeric not just 1
        Dim regexPallet As New Regex("^\d+$")
        Dim _xferstatus As Integer

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

            If RTrim(Me.txPallet.Text).Length < 9 Then
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # scanned is not from OWS, try again."
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
            sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetPalletInfoWithXferID")
            If sqlCmdPallet Is Nothing Then Throw New Exception("""Query name invalid""")
            'query for pallet
            sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
            dsPallet = DB.CreateDataSet(sqlCmdPallet)

            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            DB.KillSQLConnection()

            If dsPallet Is Nothing Then Throw New Exception("""Pallet info is invalid""")
            If dsPallet.Tables.Count < 1 Then Throw New Exception("""Pallet data is empty""")
            If dsPallet.Tables(0).Rows.Count < 1 Then Throw New Exception("""Pallet does not exist""")

            _pallet = Me.txPallet.Text
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)

            'Added code to make the XferDoneWException process work when pallet not associated with a load. Null value was causing error that ended
            If IsDBNull(dsPallet.Tables(0).Rows(0).Item("XferStatus")) = False Then
                _xferstatus = dsPallet.Tables(0).Rows(0).Item("XferStatus")
            Else
                _xferstatus = 0
            End If

            If _xferstatus = 4 Then
                Me.lbError.Text = "Pallet # " & Me.txPallet.Text & " was already processed. Please scan the next Pallet!"
                Me.lbError.Visible = True

                Call NextPallet()
                Exit Try
            End If

            'Verify Pallet is associated with Current Load
            If IsDBNull(dsPallet.Tables(0).Rows(0).Item("LoadID")) = True Then
                'Update Pallet Header Record Bin = RA and write History Record with Process = XferDoneWException
                If RcvTransferPalletException(_pallet, "NoLoad") = False Then
                    Throw New Exception("""Pallet not updated to RA location. Process Pallet again.""")
                Else
                    Me.lbError.Text = "Pallet # " & Me.txPallet.Text & " was not associated with any Load.  Pallet now added to current Load!"
                    Me.lbError.Visible = True
                End If
            ElseIf Trim(dsPallet.Tables(0).Rows(0).Item("LoadID").ToString) <> Trim(Me.lbTranferNumValue.Text) Then
                'Update Pallet Header Record Bin = RA and write History Record with Process = XferDoneWException
                If RcvTransferPalletException(_pallet, "DiffLoad") = False Then
                    Throw New Exception("""Pallet not updated to RA location. Process Pallet again.""")
                Else
                    Me.lbError.Text = "Pallet # " & Me.txPallet.Text & " was associated with a different Load. Pallet now added to current Load!"
                    Me.lbError.Visible = True
                End If
            Else
                'Update Pallet Header Record Bin = RA and write History Record with Process = XferDone
                If RcvTransferPallet(_pallet, "WCRA") = False Then Throw New Exception("""Pallet not updated to WCRA location. Process Pallet again.""")
            End If

            
            'Reload Grid
            DisplayPalletsNotReceived()
            'Setup Screen for next entry
            Call NextPallet()


            'Commented out below to make Bin Entry not needed
            ''Prompt for the Bin Location for this OW Pallet
            'Me.txToBin.Visible = True
            'Me.lbToBin.Visible = True
            'Me.lbPrompt.Text = "Scan or Enter To Bin Location"
            'Common.JavaScriptSetFocus(Page, Me.txToBin)

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

    Public Function RcvTransferPallet(ByVal _pallet As Long, ByVal _bin As String) As Boolean
        RcvTransferPallet = False
        Dim sqlCmd As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection not established")
            sqlCmd = DB.NewSQLCommand("SQL.Query.RcvTransferPallet2")
            If sqlCmd Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmd.Parameters
                .AddWithValue("@iPallet", _pallet)
                .AddWithValue("@iTranID", CInt(Me.lbTranferNumValue.Text))
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                .AddWithValue("@Bin", _bin)
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmd.ExecuteNonQuery()

            If sqlCmd.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("SQL stored procedure returned value '" & sqlCmd.Parameters.Item("@iErrorCode").Value & "'")
            Else
                'Update SRV04 TransferDetail.Status = 9 (finished transfering)
                If SetOWSTransferDetailStatus(_pallet) = False Then
                    Throw New Exception("Error occurred while updating OWS Database record.  Contact IT Department")
                Else
                    RcvTransferPallet = True
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while performing RcvTransferPallet(" & _pallet & ", " & Me.txTransferNo.Text & ")! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
        End Try
    End Function

    Public Function RcvTransferPalletException(ByVal _pallet As Long, ByVal _exception As String) As Boolean
        RcvTransferPalletException = False
        Dim sqlCmd As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Connection not established")
            sqlCmd = DB.NewSQLCommand("SQL.Query.RcvTransferPalletException")
            If sqlCmd Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmd.Parameters
                .AddWithValue("@iPallet", _pallet)
                .AddWithValue("@iTranID", CInt(Me.lbTranferNumValue.Text))
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                .AddWithValue("@sException", _exception)
                .AddWithValue("@iPalletQty", Common.GetVariable("XferPalletQty", Page))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmd.ExecuteNonQuery()

            If sqlCmd.Parameters.Item("@iErrorCode").Value = 0 Then
                '    Throw New Exception("SQL stored procedure returned value '" & sqlCmd.Parameters.Item("@iErrorCode").Value & "'")
                'Else
                RcvTransferPalletException = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while performing RcvTransferPalletException(" & _pallet & ", " & Me.txTransferNo.Text & ")! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
        End Try
    End Function

    Public Function ValidTran(ByVal _TranNum As Integer) As Boolean
        ValidTran = False
        Dim sqlCmd As New SqlClient.SqlCommand
        Dim goodtran As String = ""

        Try
            If DB.MakeSQLConnection("Warehouse") Then 'Failed to make connection - pause and try again
                DB.KillSQLConnection()

                System.Threading.Thread.Sleep(2000)

                If DB.MakeSQLConnection("Warehouse") Then
                    Throw New Exception("Connection to Database Error")
                End If
            End If

            sqlCmd = DB.NewSQLCommand("SQL.Query.TransferLookup")

            If sqlCmd Is Nothing Then
                Throw New Exception("Command Creation Error")
            End If

            sqlCmd.Parameters.AddWithValue("@Transfer", _TranNum)
            goodtran = sqlCmd.ExecuteScalar

            If Not sqlCmd Is Nothing Then sqlCmd.Dispose()
            DB.KillSQLConnection()
            If goodtran.Length > 0 Then
                ValidTran = True
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while performing ValidTran(" & _TranNum & ")! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
        Finally
            If Not sqlCmd Is Nothing Then sqlCmd.Dispose() : sqlCmd = Nothing
            DB.KillSQLConnection()
        End Try
    End Function

    Public Sub DisplayPalletsNotReceived()
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            sqlString = "SELECT Pallet FROM [dbo].[TransferDetail] WHERE ([TransferNumber] = " & CInt(Me.lbTranferNumValue.Text) & " And [Status] = 1)"
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            dgPalletsOnTransfer.DataSource = dv
            dgPalletsOnTransfer.DataBind()

            If dv.Count > 0 Then
                dgPalletsOnTransfer.Visible = True
            Else
                dgPalletsOnTransfer.Visible = False

                'SAM 12-04-2013 
                'Set the Header Records for SRV04 and SQL1 to 9 for completed
                If SetOWSTransferHeaderStatus(CInt(Me.lbTranferNumValue.Text)) = False Then
                    Me.lbError.Text = "Error occurred while finishing transfer.  Please see supervisor."
                    Me.lbError.Visible = True
                Else
                    Call InitProcess()
                End If
                'Removed the code below - Made the process automatic for Header = 9 (completed) when no more pallets exist for Transfer Id 
                'Me.btnFinish.Visible = True
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function SetOWSTransferDetailStatus(ByVal _pallet As Long) As Boolean
        SetOWSTransferDetailStatus = False

        Try
            Dim cmdOWSTransferDetailStatus As New SqlClient.SqlCommand
            Dim sSqlOWSTransferDetailStatus As String

            sqlConn.Open()
            sSqlOWSTransferDetailStatus = "UPDate TransferDetail SET Status = 9 Where Pallet = " & _pallet
            cmdOWSTransferDetailStatus.CommandText = sSqlOWSTransferDetailStatus
            cmdOWSTransferDetailStatus.Connection = sqlConn
            cmdOWSTransferDetailStatus.ExecuteNonQuery()
            sqlConn.Close()
            SetOWSTransferDetailStatus = True
        Catch ex As Exception
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
            End If
        End Try
    End Function

    Public Function SetOWSTransferHeaderStatus(ByVal _transfer As Long) As Boolean
        SetOWSTransferHeaderStatus = False

        Try
            'Set Status on SRV06
            Dim cmdOWSTransferHeaderStatus As New SqlClient.SqlCommand
            Dim sSqlOWSTransferHeaderStatus As String = ""

            sqlConn.Open()
            sSqlOWSTransferHeaderStatus = "UPDate TransferHeader SET Status = 9 , RecieveDate = '" & Now() & "' Where TransferNumber = " & _transfer
            cmdOWSTransferHeaderStatus.CommandText = sSqlOWSTransferHeaderStatus
            cmdOWSTransferHeaderStatus.Connection = sqlConn
            cmdOWSTransferHeaderStatus.ExecuteNonQuery()
            sqlConn.Close()

            SetOWSTransferHeaderStatus = True
        Catch ex As Exception
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
            End If
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        Try
            strURL = "~/IC_Menu.aspx"
            Common.SaveVariable("newURL", strURL, Page)

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while performing ReturnToPageError(" & strURL & ")! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
        End Try
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''' Obsolete Subs and Functions '''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Protected Sub txToBin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txToBin.TextChanged
    '    Try
    '        'Verify the Bin Location entered is valid in database
    '        If Not DB.MakeSQLConnection("Warehouse") Then
    '            Dim sqlCmdBin As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.BinLookup")
    '            If Not sqlCmdBin Is Nothing Then
    '                sqlCmdBin.Parameters.AddWithValue("@Bin", Me.txToBin.Text)
    '                _tobin = sqlCmdBin.ExecuteScalar()
    '                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
    '                DB.KillSQLConnection()
    '            Else
    '                DB.KillSQLConnection()
    '                Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '                Me.lbError.Text = "Error occurred while validating Bin Location - try again or see your supervisor!"
    '                Me.lbError.Visible = True
    '                Common.JavaScriptSetFocus(Page, Me.txToBin)
    '                Exit Sub
    '            End If
    '        Else
    '            DB.KillSQLConnection()
    '            Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '            Me.lbError.Text = "Communication Error occurred while validating Bin Location - try again or see your supervisor!"
    '            Me.lbError.Visible = True
    '            Common.JavaScriptSetFocus(Page, Me.txToBin)
    '            Exit Sub
    '        End If

    '        If RTrim(_tobin).Length < 1 Then 'Bin Location does not exist in database
    '            Me.lbError.Text = "Invalid Bin Location entered. Try again."
    '            Me.lbError.Visible = True
    '            'Save the bad Bin Location to display on screen for correction
    '            Common.SaveVariable("ToBin", Me.txToBin.Text, Page)
    '            Common.JavaScriptSetFocus(Page, Me.txToBin)
    '            Exit Sub
    '        Else 'Bin Location is valid
    '            _pallet = Common.GetVariable("Pallet", Page).ToString
    '            If RcvTransferPallet(_pallet, UCase(Me.txToBin.Text)) = True Then
    '                'Reload Grid
    '                DisplayPalletsNotReceived()

    '                'Setup Screen for next entry
    '                Call NextPallet()
    '            Else
    '                Me.lbError.Text = "Pallet not processed correctly. Reprocess last pallet scanned."
    '                Me.lbError.Visible = True
    '                Common.JavaScriptSetFocus(Page, Me.txPallet)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Me.lbError.Text = "An error occurred during Bin validation - Try again."
    '        Me.lbError.Visible = True
    '        Common.SaveVariable("ToBin", "", Page)
    '        Common.JavaScriptSetFocus(Page, Me.txToBin)
    '    End Try

    'End Sub
End Class