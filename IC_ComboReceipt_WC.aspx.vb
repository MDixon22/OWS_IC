Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_ComboReceipt_WC
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _company As String
    Public _whs As String
    Public _function As Integer
    Public nQtyOWFG As Integer

    'Private sConnString As String = "Server=192.168.5.4;Initial Catalog=OWS;Trusted_Connection=Yes;connect timeout=10;Persist Security Info=True"
    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString
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
        _whs = Common.GetVariable("Whse", Page).ToString

        If Not Page.IsPostBack Then
            'Setting up screen for start of new Combo Xfer
            Common.SaveVariable("newURL", Nothing, Page)
            Call InitProcess()
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, txData)
        End If
    End Sub

    Public Sub InitProcess()
        Me.txData.Text = ""
        Me.txData.Visible = True

        Me.lbXferID.Visible = False
        Me.lbXferIDVal.Text = ""
        Me.lbXferIDVal.Visible = False

        Me.lbFunction.Visible = False
        Me.lbFunction.Text = 1
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan Xfer Truck barcode at bottom of manifest"
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(txData.Text)) < 1 Then
                Exit Sub
            End If

            Me.lbError.Text = ""
            Me.lbError.Visible = False

            Select Case _function
                Case Is = 1
                    'Validate Xfer Truck Scanned or Entered to the Screen
                    If ValidateComboXferID(Trim(txData.Text)) = False Then
                        Me.lbError.Text = "Combo Xfer ID scanned is not valid"
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-scan Combo Xfer ID or see supervisor"
                    Else 'Combo Transfer # is valid in the system - update screen and show combos associated with that Combo Transfer # 
                        Me.lbXferID.Visible = True
                        Me.lbXferIDVal.Text = UCase(Trim(txData.Text))
                        Me.lbXferIDVal.Visible = True
                        Me.lbFunction.Text = 2
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Scan Combo Tag unloading from truck then move it to the Combo Receiving Area"
                        RefreshCombosInTruckGrid()
                    End If
                    Common.JavaScriptSetFocus(Page, Me.txData)

                Case Is = 2 'Scan Combo unloading from truck.
                    Me.lbError.Visible = False
                    'Validate Combo Scanned to the Screen to make sure it is in the correct status
                    If ValidateCombo(Trim(txData.Text)) = False Then
                        'Me.lbError.text is set in the Combo Validation function
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-Enter Combo Tag # or see supervisor"
                    Else
                        'Combo is valid - update Combo Status = 111 and ProcessAreaID = 35 in CD_Combos 
                        If ProcessCombo(Trim(txData.Text)) = False Then
                            Me.lbError.Text = "Error occurred while processing the Combo off the Xfer Truck.  Please scan the combo again."
                            Me.lbError.Visible = True
                            Me.lbFunction.Text = 2
                            Me.txData.Text = ""
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        Else
                            'Combo successfully updated
                            RefreshCombosInTruckGrid()
                            Me.lbFunction.Text = 2
                            Me.txData.Text = ""
                            Me.lbPrompt.Text = "Scan Combo Tag unloading from truck then move it to the Combo Receiving Area"
                        End If
                    End If
            End Select
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while connecting to database to Xfer Combos to Weeden Creek! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Sub

    Public Function ValidateComboXferID(ByVal _comboxferid As Integer) As Boolean
        ValidateComboXferID = False
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "Select * from CDT_TransferHeader Where TransferNumber = " & _comboxferid & "AND Status < 2 "
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            If ds.Tables("Data").Rows.Count > 0 Then
                ValidateComboXferID = True
            End If
        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function

    Public Function ValidateCombo(ByVal _combo As Long) As Boolean
        ValidateCombo = False
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "Select ComboID, TransferNumber,ComboStatus from vwCDT_TransferDetail Where ComboID = " & _combo
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            If ds.Tables("Data").Rows.Count > 0 Then
                If ds.Tables("Data").Rows(0).Item("ComboStatus").ToString = "110" Then
                    ValidateCombo = True
                Else
                    Select Case ds.Tables("Data").Rows(0).Item("ComboStatus").ToString
                        Case Is = "100"
                            Me.lbError.Text = "Combo Tag scanned was not transfered correctly in system. Notify supervisor."

                        Case Is = "105"
                            Me.lbError.Text = "Combo Tag scanned was not transfered correctly in system. Notify supervisor."

                        Case Is = "111"
                            Me.lbError.Text = "Combo Tag scanned was already received into Weeden Creek. Put combo in Combo Receiving Area."

                        Case Else
                            Me.lbError.Text = "Combo Tag scanned is in the wrong status. Verify with supervisor."

                    End Select
                End If
            Else
                Me.lbError.Text = "Combo Tag scanned was not found in system. Please scan the Combo Tag again or show to supervisor."
            End If
        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function

    Public Function ProcessCombo(ByVal _combo As Long) As Boolean
        ProcessCombo = False

        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String

        Try
            sqlConn.Open()
            sSqlString = "spCD_ComboReceipt "
            cmdSqlCommand.CommandType = CommandType.StoredProcedure
            cmdSqlCommand.CommandText = sSqlString
            cmdSqlCommand.Connection = sqlConn
            cmdSqlCommand.Parameters.AddWithValue("@pnComboID", _combo)
            cmdSqlCommand.Parameters.AddWithValue("@pnTransferNumber", Me.lbXferIDVal.Text)
            cmdSqlCommand.Parameters.AddWithValue("@pnAssociateID", RTrim(Common.GetVariable("UserID", Page).ToString))
            cmdSqlCommand.ExecuteNonQuery()
            sqlConn.Close()
            ProcessCombo = True
        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function

    Public Sub RefreshCombosInTruckGrid()
        'Load Me.gvCombos
        Try
            Dim sqlString As String
            Dim sqlDABuildRacks As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            If Trim(Me.lbXferIDVal.Text) = "" Then
                Me.lbXferIDVal.Text = "0"
            End If

            sqlString = "Select ComboID,Formula,StuffingGroupID,LotNo from vwCDT_TransferDetail Where Status = 0 AND TransferNumber = " & CInt(Me.lbXferIDVal.Text)
            sqlDABuildRacks = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDABuildRacks)
            sqlDABuildRacks.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            Me.gvCombos.DataSource = dv

            Me.gvCombos.DataBind()

            If dv.Count > 0 Then
                Me.gvCombos.Visible = True
            Else
                Me.gvCombos.Visible = False
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_ComboMenu.aspx", Page) 'new
    End Sub

    Protected Sub btFinished_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinished.Click
        If Me.gvCombos.Visible = False Then
            Dim cmdSqlCommand As New SqlClient.SqlCommand
            Dim sSqlString As String

            Try
                sqlConn.Open()
                sSqlString = "spCDT_ComboXferComplete "
                cmdSqlCommand.CommandType = CommandType.StoredProcedure
                cmdSqlCommand.CommandText = sSqlString
                cmdSqlCommand.Connection = sqlConn
                cmdSqlCommand.Parameters.AddWithValue("@pnTransferNumber", Me.lbXferIDVal.Text)
                cmdSqlCommand.Parameters.AddWithValue("@pnAssociateID", RTrim(Common.GetVariable("UserID", Page).ToString))
                cmdSqlCommand.ExecuteNonQuery()
                sqlConn.Close()

                InitProcess()

            Catch ex As Exception
                'End and dispose an active connection if it exists to avoid error
                If sqlConn.State <> ConnectionState.Closed Then
                    sqlConn.Close()
                    sqlConn.Dispose()
                End If
            Finally
                'End and dispose an active connection if it exists to avoid error
                If sqlConn.State <> ConnectionState.Closed Then
                    sqlConn.Close()
                    sqlConn.Dispose()
                End If
            End Try

        Else
            Me.lbError.Text = "There are still combos in the list to receive. If they are not there contact your supervisor."
            Me.lbError.Visible = True
        End If

    End Sub

End Class