Partial Public Class IC_NumberTwo
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _whs As String
    Public _company As String
    Public _function As Integer
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
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString
        'Me.lbError.Visible = False

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If
    End Sub
    Public Sub InitProcess()
        lbError.Text = ""
        lbError.Visible = False

        lbPrompt.Text = "Scan Wisconsin Special Code"
        lbFunction.Text = "1"
    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(txData.Text)) < 1 Then
                Exit Sub
            End If

            If IsNumeric(Me.txData.Text) = False Then
                lbError.Text = "Value entered must be a number - try again."
                lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
                Exit Sub
            End If

            Select Case _function
                Case 1 'Scanned Wisconsin Special Code
                    If ValidWSC(Me.txData.Text) = False Then
                        lbError.Text = "Wisconsin Special Code not active in database - try again or see your supervisor."
                        lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                        Exit Sub
                    Else
                        'found record for Wisconsin Special Code - Load values to screen
                        Me.lbWSCData.Text = Me.txData.Text
                        Me.lbWSCData.Visible = True
                        Me.lbWSC_Desc.Text = Common.GetVariable("WSC_Description", Page).ToString
                        Me.lbWSC_Desc.Visible = True
                        lbError.Text = ""
                        lbError.Visible = False
                        If UCase(RTrim(Common.GetVariable("Formula", Page).ToString)) = "NONE" Then
                            Me.lbLotData.Text = "999"
                            Me.lbLotData.Visible = True
                            Me.lbLot.Visible = True
                            
                            Me.lbFunction.Text = "3"
                            Me.txData.Text = ""
                            Me.lbPrompt.Text = "Enter Pounds on scale"
                        Else
                            Me.lbFunction.Text = "2"
                            Me.txData.Text = ""
                            Me.lbPrompt.Text = "Enter Lot #"
                        End If
                        
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case 2 'Enter Lot #
                    If ValidLot(CInt(Me.txData.Text), Common.GetVariable("Formula", Page).ToString) = False Then
                        lbError.Text = "Lot # entered not valid for " & Me.lbWSC_Desc.Text & " - try again or see your supervisor."
                        lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                        Exit Sub
                    Else
                        'Lot # has been verified continue process to entering pounds
                        Me.lbLotData.Text = Me.txData.Text
                        Me.lbLotData.Visible = True
                        Me.lbLot.Visible = True
                        lbError.Text = ""
                        lbError.Visible = False
                        Me.lbFunction.Text = "3"
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Pounds on scale"
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case 3 'Enter Pounds
                    Me.lbPoundsData.Text = Me.txData.Text
                    Me.lbPoundsData.Visible = True
                    Me.lbPounds.Visible = True
                    Me.txData.Text = ""
                    Me.txData.Visible = False
                    Me.lbPrompt.Text = "Verify Data then press Verified - Yes button."
                    Me.btVerifyFinished.Text = "Verified - Yes"
                    Me.btVerifyFinished.Visible = True

            End Select
        Catch ex As Exception

        End Try
    End Sub
    Public Function ValidWSC(ByVal _wsc As Long) As Boolean
        ValidWSC = False

        Try
            'Initialize Session Variables used below
            Common.SaveVariable("Formula", "", Page)
            Common.SaveVariable("WSC_Description", "", Page)

            'Close an active connection if it exists to avoid error
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If

            'Get info for WSC passed in only if the code is Active = 1 (Yes)
            Dim sqlString As String
            Dim sqlDA_WSC As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "Select [Formula],[Description] from [OWS].[dbo].[IC_WiscSpecCodes] Where [Active] = 1 And [WiscSpecCode] = " & _wsc
            sqlDA_WSC = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDA_WSC)
            sqlDA_WSC.Fill(ds, "WSC")
            sqlDA_WSC.Dispose()

            'Close SqlClient Connection if it is still Open
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If

            'Save Wisconsin Spec Code Formula and Description for future use in app
            If ds.Tables("WSC").Rows.Count > 0 Then
                Common.SaveVariable("Formula", ds.Tables("WSC").Rows(0).Item("Formula"), Page)
                Common.SaveVariable("WSC_Description", ds.Tables("WSC").Rows(0).Item("Description"), Page)

                ValidWSC = True
            End If

        Catch ex As Exception
            'Close an active connection if it exists to avoid error
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

    End Function

    Public Function ValidLot(ByVal _lot As Integer, ByVal _formula As String) As Boolean
        ValidLot = False

        Try
            'Close an active connection if it exists to avoid error
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If

            'Make sure Lot# entered is valid for Formula entered
            Dim sqlString As String
            Dim sqlDA_Lot As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim dsLot As New DataSet

            sqlString = "SELECT [LotNumber] FROM [OWS].[dbo].[vwFormulaByLot] WHERE CONVERT(INTEGER,[LotNumber]) = " & _lot & " AND RTrim([Formula]) = '" & RTrim(_formula) & "'"
            sqlDA_Lot = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDA_Lot)
            sqlDA_Lot.Fill(dsLot, "LOT")
            sqlDA_Lot.Dispose()

            'Close SqlClient Connection if it is still Open
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If

            'Save Wisconsin Spec Code Formula and Description for future use in app
            If dsLot.Tables("Lot").Rows.Count > 0 Then
                ValidLot = True
            End If

        Catch ex As Exception
            'Close an active connection if it exists to avoid error
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'ReOpen IC_NumberTwo.aspx
        Common.SaveVariable("newURL", "~/IC_NumberTwo.aspx", Page)
    End Sub

    Protected Sub btVerifyFinished_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btVerifyFinished.Click
        If Me.btVerifyFinished.Text = "Verified - Yes" Then
            Me.btVerifyFinished.Text = "Finish # 2"
            Me.lbPrompt.Text = "Press Finish # 2 button to Save."
        ElseIf Me.btVerifyFinished.Text = "Finish # 2" Then
            Me.btVerifyFinished.Visible = False

            'Stored Procedure to write record to IC_NumberTwo and IC_PalletHistory tables
            If WriteNumberTwo() = True Then
                'Records were successfully written to tables
                Me.lbError.Text = "Number 2 product: " & Me.lbWSCData.Text & "-" & Me.lbWSC_Desc.Text & " - Lot# " & Me.lbLotData.Text & " - for " & Me.lbPoundsData.Text & " Pounds was created."
                Me.lbError.Visible = True
                Me.lbPrompt.Text = "Press the Next Entry button to continue."
                Me.btRestart.Text = "Next Entry"
            Else
                'Records failed to write to tables
                lbError.Text = "Number 2 product: was NOT created - restart entry or see your supervisor."
                lbError.Visible = True
                Me.lbPrompt.Text = "Press the Restart Entry button to continue."
                Me.btRestart.Text = "Restart Entry"
            End If
        End If
    End Sub
    Public Function WriteNumberTwo() As Boolean
        WriteNumberTwo = False

        Dim iReturnValue As Int64 = 0
        Dim dtmEntered As Date = Now()
        Dim sqlCmdWriteNumberTwo As New SqlClient.SqlCommand

        Try
            sqlCmdWriteNumberTwo.Connection = sqlConn
            sqlConn.Open()
            sqlCmdWriteNumberTwo.CommandType = CommandType.StoredProcedure
            sqlCmdWriteNumberTwo.CommandText = "insertNumberTwo "
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnWiscSpecCode", CLng(Me.lbWSCData.Text))
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrFormula", Common.GetVariable("Formula", Page).ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrLotNumber", Me.lbLotData.Text)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnPounds", CInt(Me.lbPoundsData.Text))
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pdtmEntered", dtmEntered)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@iErrorCode", 0)
            sqlCmdWriteNumberTwo.Parameters("@iErrorCode").Direction = ParameterDirection.Output
            sqlCmdWriteNumberTwo.ExecuteNonQuery()
            iReturnValue = sqlCmdWriteNumberTwo.Parameters("@iErrorCode").Value
            sqlCmdWriteNumberTwo.Dispose()
            sqlCmdWriteNumberTwo = Nothing
            sqlConn.Close()
            sqlConn.Dispose()

            'No errors occured during stored procedure
            If iReturnValue = 0 Then
                WriteNumberTwo = True
            End If
        Catch ex As Exception
            If Not sqlCmdWriteNumberTwo Is Nothing Then
                sqlCmdWriteNumberTwo.Dispose()
                sqlCmdWriteNumberTwo = Nothing
            End If
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function
End Class