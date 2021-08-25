Partial Public Class IC_SmokingRacks
    Inherits System.Web.UI.Page

    Public _datescanned As DateTime
    Public _dateovenstarted As DateTime
    Public _whs As String
    Public _company As String
    Public _function As Integer
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
        'SAM - 6/5/2018 - Switched to User Warehouse to handle Montgomery
        '_whs = ConfigurationManager.AppSettings.Get("Warehouse").ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Rack
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Data TextBox()
            'Prompt Error and Set Focus to Data TextBox
            If RTrim(Me.txData.Text).Length < 1 Then
                Me.lbError.Text = "Entry cannot be blank."
                Me.lbError.Visible = True
            End If
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If
    End Sub

    Protected Sub InitProcess()
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan or Enter Rack #."
        Me.lbFunction.Text = "1"
        Common.SaveVariable("RackNum", "", Page)
        Common.SaveVariable("RackProduct", "", Page)
        Common.SaveVariable("OvenNum", "", Page)
        Common.SaveVariable("OvenDesc", "", Page)
        Common.SaveVariable("OvenLocNum", "", Page)
        Common.SaveVariable("OvenLocDesc", "", Page)
        Common.SaveVariable("OvenType", "", Page)
        Common.SaveVariable("ProcessType", "", Page)
        Me.lbProcessType.Text = ""


        Me.lbRack.Visible = False
        Me.lbRackValue.Text = ""
        Me.lbRackValue.Visible = False
        Me.lbRackProduct.Text = ""
        Me.lbRackProduct.Visible = False

        Me.lbOven.Visible = False
        Me.lbOvenValue.Text = ""
        Me.lbOvenValue.Visible = False
        Me.lbOvenDesc.Text = ""
        Me.lbOvenDesc.Visible = False

        Me.lbOvenLocation.Visible = False
        Me.lbOvenLocValue.Text = ""
        Me.lbOvenLocValue.Visible = False
        Me.lbOvenLocDesc.Text = ""
        Me.lbOvenLocDesc.Visible = False

        Me.btIntoOven.Visible = False
        Me.btOutOfOven.Visible = False
        Me.btIntoBlast.Visible = False
        Me.btIntoRTE.Visible = False

        Me.txData.Text = ""


    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(txData.Text)) < 1 Then
                Exit Sub
            End If
            Select Case _function
                Case 1 'Rack # entered
                    'IsNumeric
                    If IsNumeric(txData.Text) = False Then
                        lbError.Text = "Rack must be numeric."
                        lbError.Visible = True
                        Exit Sub
                    End If

                    'Check if it is a valid Rack# in the system
                    If ValidRack(CInt(Me.txData.Text)) = True Then
                        Me.lbRack.Visible = True
                        Me.lbRackValue.Text = Common.GetVariable("RackNum", Page).ToString
                        Me.lbRackValue.Visible = True
                        Me.lbRackProduct.Text = Common.GetVariable("RackProduct", Page).ToString
                        Me.lbRackProduct.Visible = True
                        Me.btIntoOven.Visible = True
                        Me.btOutOfOven.Visible = True
                        Me.btIntoBlast.Visible = True
                        Me.btIntoRTE.Visible = True
                        Me.lbPrompt.Text = "Press the button that describes your process."
                        Me.txData.Visible = False
                    Else
                        Me.lbError.Text = "Rack entered is not valid in the system. Try again or contact supervisor."
                        Me.lbError.Visible = True
                    End If

                Case 2 'Oven # entered
                    'IsNumeric
                    If IsNumeric(txData.Text) = False Then
                        lbError.Text = "Oven must be numeric."
                        lbError.Visible = True
                        Exit Sub
                    End If

                    'Check if it is a valid Oven # in the system
                    If ValidOven(CInt(Me.txData.Text)) = True Then
                        ''Check if Rack has already been processed in this area before
                        'If CheckDupScans() = True Then Throw New Exception("""Rack was already scanned into this oven""")

                        Me.lbOven.Visible = True
                        Me.lbOvenValue.Text = Common.GetVariable("OvenNum", Page).ToString
                        Me.lbOvenValue.Visible = True
                        Me.lbOvenDesc.Text = Common.GetVariable("OvenDesc", Page).ToString
                        Me.lbOvenDesc.Visible = True

                        If Common.GetVariable("ProcessType", Page).ToString = "INTO_OVEN" Then
                            'If _whs = "17" Then
                            '    Me.lbPrompt.Text = "Verify entry - Press Y to Complete rack , N to Cancel rack."
                            '    Me.txData.Text = ""
                            '    Me.txData.Visible = True
                            '    Me.lbFunction.Text = "4"
                            '    Me.lbOvenLocation.Visible = True
                            '    Me.lbOvenLocValue.Text = "1"
                            '    Me.lbOvenLocValue.Visible = True
                            '    Me.lbOvenLocDesc.Text = "1"
                            '    Me.lbOvenLocDesc.Visible = True
                            'Else
                            Me.lbPrompt.Text = "Scan or Enter the Oven Location for this rack."
                            Me.txData.Text = ""
                            Me.txData.Visible = True
                            Me.lbFunction.Text = "3"
                            'End If

                            Me.lbError.Text = ""
                            Me.lbError.Visible = False
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        End If
                        If Common.GetVariable("ProcessType", Page).ToString = "OUT_OF_OVEN" Then
                            Me.lbPrompt.Text = "Verify entry - Press Y to Complete rack , N to Cancel rack."
                            Me.txData.Text = ""
                            Me.txData.Visible = True
                            Me.lbFunction.Text = "4"
                            Me.lbError.Text = ""
                            Me.lbError.Visible = False
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        End If
                    End If

                Case 3 'Oven Location entered
                    'IsNumeric
                    If IsNumeric(txData.Text) = False Then
                        lbError.Text = "Oven Location must be numeric."
                        lbError.Visible = True
                        Exit Sub
                    End If

                    'Check if it is a valid Oven Location in the system
                    If ValidOvenLocation(CInt(Me.txData.Text)) = True Then
                        Me.lbOvenLocation.Visible = True
                        Me.lbOvenLocValue.Text = Common.GetVariable("OvenLocNum", Page).ToString
                        Me.lbOvenLocValue.Visible = True
                        Me.lbOvenLocDesc.Text = Common.GetVariable("OvenLocDesc", Page).ToString
                        Me.lbOvenLocDesc.Visible = True

                        Me.lbPrompt.Text = "Verify entry - Press Y to Complete rack , N to Cancel rack."
                        Me.txData.Text = ""
                        Me.txData.Visible = True
                        Me.lbFunction.Text = "4"
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case 4
                    If UCase(Me.txData.Text) = "Y" Then
                        If Common.GetVariable("ProcessType", Page).ToString = "INTO_OVEN" Then
                            'Process Into Oven transaction
                            If ProcessIntoOven(Me.lbRackValue.Text, Me.lbOvenValue.Text, Me.lbOvenLocValue.Text) = False Then
                                'Rack was not written or updated in SQL Server
                                Me.lbError.Text = "Into Oven process for Rack # " & Me.lbRackValue.Text & " in Oven # " & Me.lbOvenValue.Text & " did not get recorded. Please press Restart Entry button."
                                Me.lbError.Visible = True
                                Exit Sub
                            End If
                        End If
                        If Common.GetVariable("ProcessType", Page).ToString = "OUT_OF_OVEN" Then
                            'Process Out Of Oven transaction
                            If ProcessOutOfOven(Me.lbRackValue.Text, Me.lbOvenValue.Text) = False Then
                                'Rack was not updated in SQL Server
                                Me.lbError.Text = "Out Of Oven process for Rack # " & Me.lbRackValue.Text & " for Oven # " & Me.lbOvenValue.Text & " did not get recorded. Please press Restart Entry button."
                                Me.lbError.Visible = True
                                Exit Sub
                            End If
                        End If
                        If Common.GetVariable("ProcessType", Page).ToString = "INTO_BLAST" Then
                            'Process Non Oven transaction into RTE
                            If ProcessNonOvenMovement(Me.lbRackValue.Text, CInt(Common.GetVariable("OvenType", Page)), CInt(Common.GetVariable("OvenNumber", Page))) = False Then
                                'Rack was not updated in SQL Server
                                Me.lbError.Text = "Into Blast process for Rack # " & Me.lbRackValue.Text & " for Oven # " & Me.lbOvenValue.Text & " did not get recorded. Please press Restart Entry button."
                                Me.lbError.Visible = True
                                Exit Sub
                            End If
                        End If
                        If Common.GetVariable("ProcessType", Page).ToString = "INTO_RTE" Then
                            'Process Non Oven transaction into RTE
                            If ProcessNonOvenMovement(Me.lbRackValue.Text, CInt(Common.GetVariable("OvenType", Page)), CInt(Common.GetVariable("OvenNumber", Page))) = False Then
                                'Rack was not updated in SQL Server
                                Me.lbError.Text = "Into RTE process for Rack # " & Me.lbRackValue.Text & " for Oven # " & Me.lbOvenValue.Text & " did not get recorded. Please press Restart Entry button."
                                Me.lbError.Visible = True
                                Exit Sub
                            End If
                        End If
                        'Initialize screen for next rack entry
                        Call InitProcess()

                    ElseIf UCase(Me.txData.Text) = "N" Then
                        'Initialize screen for next rack entry
                        Call InitProcess()

                    Else
                        'Error for bad entry
                        Me.lbError.Text = "Enter Y to Complete rack, N to Cancel rack."
                        Me.lbError.Visible = True
                        Exit Sub
                    End If
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Function ValidRack(ByVal _rack As Integer) As Boolean
        ValidRack = False
        Dim _rackproduct As String = Nothing

        'open database connection
        If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Valid Rack Connection not established""")
        Dim sqlCmdValidRack As New System.Data.SqlClient.SqlCommand
        'SAM - 6/13/2018 - Modified for Montgomery
        'sqlCmdValidRack = DB.NewSQLCommand("SQL.Query.SmokingRackValid")
        sqlCmdValidRack = DB.NewSQLCommand("SQL.Query.SmokingRackValidWH")
        If sqlCmdValidRack Is Nothing Then Throw New Exception("""Loading Valid Rack Command failed. Restart screen and try again.""")
        sqlCmdValidRack.Parameters.AddWithValue("@Rack", _rack)
        sqlCmdValidRack.Parameters.AddWithValue("@Whs", _whs)
        _rackproduct = sqlCmdValidRack.ExecuteScalar()
        sqlCmdValidRack.Dispose() : sqlCmdValidRack = Nothing
        DB.KillSQLConnection()

        If Not _rackproduct Is Nothing Then
            Common.SaveVariable("RackNum", _rack, Page)
            Common.SaveVariable("RackProduct", _rackproduct, Page)
            ValidRack = True
        End If

    End Function

    Public Function ValidOven(ByVal _oven As Integer) As Boolean
        ValidOven = False

        Dim dsValidOven As New Data.DataSet
        Dim sqlCmdValidOven As New System.Data.SqlClient.SqlCommand

        'open database connection
        If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Valid Oven Connection not established""")

        sqlCmdValidOven = DB.NewSQLCommand("SQL.Query.SmokingOvenValidWH")
        If sqlCmdValidOven Is Nothing Then Throw New Exception("""Loading Valid Oven Command failed. Restart screen and try again.""")
        sqlCmdValidOven.Parameters.AddWithValue("@OvenID", _oven)
        sqlCmdValidOven.Parameters.AddWithValue("@Whs", _whs)
        dsValidOven = DB.GetDataSet(sqlCmdValidOven)
        sqlCmdValidOven.Dispose() : sqlCmdValidOven = Nothing
        DB.KillSQLConnection()

        If dsValidOven Is Nothing Then Throw New Exception("""Running Valid Oven Command failed. Restart screen and try again.""")
        If dsValidOven.Tables(0).Rows.Count > 0 Then
            Common.SaveVariable("OvenNum", _oven, Page)
            Common.SaveVariable("OvenDesc", dsValidOven.Tables(0).Rows(0).Item("OvenDescription"), Page)
            Common.SaveVariable("OvenType", dsValidOven.Tables(0).Rows(0).Item("OvenTypeID"), Page)
            ValidOven = True
        End If
    End Function

    Public Function ValidOvenLocation(ByVal _ovenlocation As Integer) As Boolean
        ValidOvenLocation = False
        Dim _ovenlocationdesc As String = Nothing

        'open database connection
        If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Valid Oven Location Connection not established""")
        Dim sqlCmdValidOvenLocation As New System.Data.SqlClient.SqlCommand
        sqlCmdValidOvenLocation = DB.NewSQLCommand("SQL.Query.SmokingOvenLocationValid")
        If sqlCmdValidOvenLocation Is Nothing Then Throw New Exception("""Loading Valid Oven Location Command failed. Restart screen and try again.""")
        sqlCmdValidOvenLocation.Parameters.AddWithValue("@OvenID", CInt(Common.GetVariable("OvenNum", Page)))
        sqlCmdValidOvenLocation.Parameters.AddWithValue("@OvenLocationID", _ovenlocation)
        _ovenlocationdesc = sqlCmdValidOvenLocation.ExecuteScalar()
        sqlCmdValidOvenLocation.Dispose() : sqlCmdValidOvenLocation = Nothing
        DB.KillSQLConnection()

        If Not _ovenlocationdesc Is Nothing Then
            Common.SaveVariable("OvenLocNum", _ovenlocation, Page)
            Common.SaveVariable("OvenLocDesc", _ovenlocationdesc, Page)
            ValidOvenLocation = True
        End If
    End Function

    Public Function ProcessIntoOven(ByVal _rack As Integer, ByVal _oven As Integer, ByVal _ovenloc As Integer) As Boolean
        ProcessIntoOven = False
        Dim sqlCmdProcessIntoOven As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Process Into Oven Connection not established")
            sqlCmdProcessIntoOven = DB.NewSQLCommand("SQL.Query.SmokingRackIntoOven")
            If sqlCmdProcessIntoOven Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmdProcessIntoOven.Parameters
                .AddWithValue("@iRack", _rack)
                .AddWithValue("@iOvenType", CInt(Common.GetVariable("OvenType", Page)))
                .AddWithValue("@iOven", _oven)
                .AddWithValue("@iOvenLoc", _ovenloc)
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                .AddWithValue("@Whs", _whs)
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdProcessIntoOven.ExecuteNonQuery()

            If sqlCmdProcessIntoOven.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("SQL store procedure returned value '" & sqlCmdProcessIntoOven.Parameters.Item("@iErrorCode").Value & "'")
            Else
                ProcessIntoOven = True
                Call InitProcess()
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Function ProcessOutOfOven(ByVal _rack As Integer, ByVal _oven As Integer) As Boolean
        ProcessOutOfOven = False
        Dim sqlCmdProcessOutOfOven As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Process Out Of Oven Connection not established")
            sqlCmdProcessOutOfOven = DB.NewSQLCommand("SQL.Query.SmokingRackOutOfOven")
            If sqlCmdProcessOutOfOven Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmdProcessOutOfOven.Parameters
                .AddWithValue("@iRack", _rack)
                .AddWithValue("@iOven", _oven)
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                .AddWithValue("@Whs", _whs)
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdProcessOutOfOven.ExecuteNonQuery()

            If sqlCmdProcessOutOfOven.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("SQL store procedure returned value '" & sqlCmdProcessOutOfOven.Parameters.Item("@iErrorCode").Value & "'")
            Else
                ProcessOutOfOven = True
                Call InitProcess()
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Function ProcessNonOvenMovement(ByVal _rack As Integer, ByVal _oventype As Integer, ByVal _ovennumber As Integer) As Boolean
        ProcessNonOvenMovement = False
        Dim sqlCmdProcessNonOvenMovement As New SqlClient.SqlCommand

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("Process Non Oven Movement Connection not established")
            'SAM - 6/5/2018 - Switched to handle Montgomery Warehouse
            'sqlCmdProcessNonOvenMovement = DB.NewSQLCommand("SQL.Query.SmokingRackNonOvenMovement")
            sqlCmdProcessNonOvenMovement = DB.NewSQLCommand("SQL.Query.SmokingRackNonOvenMovementWH")
            If sqlCmdProcessNonOvenMovement Is Nothing Then Throw New Exception("Failed to create query")
            With sqlCmdProcessNonOvenMovement.Parameters
                .AddWithValue("@iRack", _rack)
                .AddWithValue("@iOvenType", _oventype)
                .AddWithValue("@iOvenNum", _ovennumber)
                .AddWithValue("@sUserID", Common.GetVariable("UserID", Page))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdProcessNonOvenMovement.ExecuteNonQuery()

            If sqlCmdProcessNonOvenMovement.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("SQL store procedure returned value '" & sqlCmdProcessNonOvenMovement.Parameters.Item("@iErrorCode").Value & "'")
            Else
                ProcessNonOvenMovement = True
                Call InitProcess()
            End If
        Catch ex As Exception

        End Try
    End Function

    Protected Sub btIntoOven_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btIntoOven.Click
        Me.btIntoOven.Visible = False
        Me.btOutOfOven.Visible = False
        Me.btIntoBlast.Visible = False
        Me.btIntoRTE.Visible = False
        Me.lbProcessType.Text = "INTO OVEN"
        Common.SaveVariable("ProcessType", "INTO_OVEN", Page)
        'Prompt user to scan or enter the OvenID
        Me.lbPrompt.Text = "Scan or Enter the Oven Number you are loading."
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbFunction.Text = "2"
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txData)

    End Sub

    Protected Sub btOutOfOven_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btOutOfOven.Click
        Me.btIntoOven.Visible = False
        Me.btOutOfOven.Visible = False
        Me.btIntoBlast.Visible = False
        Me.btIntoRTE.Visible = False
        Me.lbProcessType.Text = "OUT OF OVEN"
        Common.SaveVariable("ProcessType", "OUT_OF_OVEN", Page)
        'Prompt user to scan or enter the OvenID
        Me.lbPrompt.Text = "Scan or Enter the Oven Number you are unloading."
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbFunction.Text = "2"
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txData)

    End Sub

    Protected Sub btIntoBlast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btIntoBlast.Click
        Me.btIntoOven.Visible = False
        Me.btOutOfOven.Visible = False
        Me.btIntoBlast.Visible = False
        Me.btIntoRTE.Visible = False
        Me.lbProcessType.Text = "INTO BLAST"
        Common.SaveVariable("ProcessType", "INTO_BLAST", Page)
        Common.SaveVariable("OvenType", "8", Page)
        If _whs = 17 Then
            Common.SaveVariable("OvenNumber", "89", Page)
        Else
            Common.SaveVariable("OvenNumber", "88", Page)
        End If
        Me.lbPrompt.Text = "Verify entry - Press Y to Complete rack , N to Cancel rack."
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbFunction.Text = "4"
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txData)

        'Update the Rack record to the time the Rack was scanned into the BlastCooler.

    End Sub

    Protected Sub btIntoRTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btIntoRTE.Click
        Me.btIntoOven.Visible = False
        Me.btOutOfOven.Visible = False
        Me.btIntoBlast.Visible = False
        Me.btIntoRTE.Visible = False
        Me.lbProcessType.Text = "INTO RTE"
        Common.SaveVariable("ProcessType", "INTO_RTE", Page)
        Common.SaveVariable("OvenType", "9", Page)
        If _whs = 17 Then
            Common.SaveVariable("OvenNumber", "98", Page)
        Else
            Common.SaveVariable("OvenNumber", "99", Page)
        End If
        Me.lbPrompt.Text = "Verify entry - Press Y to Complete rack , N to Cancel rack."
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbFunction.Text = "4"
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txData)

        'Update the Rack record to the time the Rack was scanned into the RTE Holding Area.

    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)

    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen for Rack Entry
        Call InitProcess()

    End Sub
End Class