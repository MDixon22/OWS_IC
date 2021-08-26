Partial Public Class IC_NumberTwoNew
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _whs As String
    Public _company As String
    Public _function As Integer
    Public _product As String
    Public _productversion As String
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

        Me.txData.Visible = True
        Me.lbFormula.Visible = False
        Me.lbProdDesc.Visible = False
        Me.lbLotData.Visible = False
        Me.lbLot.Visible = False
        Me.lbPounds.Visible = False
        Me.lbPoundsData.Visible = False
        Me.lbReason.Visible = False
        Me.lbReasonCode.Visible = False
        Me.lbTest.Visible = False
        Me.lbDefective.Visible = False
        Me.lbDistressed.Visible = False
        Me.lbEndOfRunPartial.Visible = False
        Me.lbCheese.Visible = False
        Me.lbEndsCurls.Visible = False

        Select Case _whs
            Case Is = "17"
                Me.lbLocation.Text = "MONTGOMERY"
            Case Is = "35"
                Me.lbLocation.Text = "WEEDEN"
            Case Is = "20"
                Me.lbLocation.Text = "PLANT2"
        End Select

        Me.lbLocation.Visible = True
        Me.btMixed.Visible = True

        Session.Item("Formula") = ""
        Session.Item("ProductID") = ""
        Session.Item("ProdDesc") = ""
        Session.Item("Version") = ""
        Session.Item("GTIN") = ""
        Session.Item("StuffGroup") = ""
        Session.Item("SOC") = "X"

        lbPrompt.Text = "Scan Case Label or Press MIXED button"
        lbFunction.Text = "1"
    End Sub

    Protected Sub btMixed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btMixed.Click
        Session.Item("GTIN") = "99999999999999"
        Session.Item("Formula") = "MIXED"
        Session.Item("Version") = "999"
        Session.Item("StuffGroup") = "MIXED"
        Me.lbFormula.Text = Session.Item("Formula").ToString
        Me.lbFormula.Visible = True
        Me.lbProdDesc.Text = "Mixed Product"
        Me.lbProdDesc.Visible = True
        Me.lbLotData.Text = "999"
        Me.lbLotData.Visible = True
        Me.lbLot.Visible = True
        Me.lbFunction.Text = "2"
        Me.txData.Text = ""
        Me.lbPrompt.Text = "Enter Pounds"
        Me.btMixed.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(Me.txData.Text)) < 1 Then
                Exit Sub
            End If

            If IsNumeric(Me.txData.Text) = False And _function <> 4 Then
                Me.lbError.Text = "Value entered must be a number - try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
                Exit Sub
            End If

            Select Case _function

                Case 1 'Scanned Case Label
                    If ProcessCaseLabel(Me.txData.Text) = False Then
                        Me.lbError.Text = "Error validating Case Label scanned in database - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "1"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)

                    Else
                        'found record for Scanned Case Label
                        Session.Item("GTIN") = Mid(Me.txData.Text, 3, 14)
                        Me.lbFormula.Text = Session.Item("Formula").ToString
                        Me.lbFormula.Visible = True
                        Me.lbProdDesc.Text = Session.Item("ProdDesc").ToString
                        Me.lbProdDesc.Visible = True

                        Dim LotFull As String = ""
                        LotFull = CInt(Common.FindLotFromBarcode(Mid$(Me.txData.Text, 27, 12), _whs, Mid$(Me.txData.Text, 3, 14), CInt(Mid$(Me.txData.Text, 42, 3)), Mid$(Mid$(Me.txData.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txData.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txData.Text, 19, 6), 1, 2)))

                        Me.lbLotData.Text = Mid(LotFull, 3, 3)
                        Me.lbLotData.Visible = True
                        Me.lbLot.Visible = True

                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                        Me.lbFunction.Text = "2"
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Pounds"

                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case 2 'Enter Pounds

                    If CInt(Me.txData.Text) > 300 Then
                        'Prompt screen for a Supervisor Override
                        Me.lbPoundsData.Text = Me.txData.Text
                        Me.lbPoundsData.Visible = True
                        Me.lbPounds.Visible = True
                        Me.txData.Text = ""

                        Me.lbError.Text = "Pounds entered exceeds the maximum of 300. If Pounds incorrect then press Restart Entry button to start transaction over. If Pounds correct get supervisor to enter override code."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "4"
                        Me.lbPrompt.Text = "Enter Supervisor Override Code"
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        Me.lbPoundsData.Text = Me.txData.Text
                        Me.lbPoundsData.Visible = True
                        Me.lbPounds.Visible = True
                        Me.txData.Text = ""

                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                        Me.lbFunction.Text = "3"
                        Me.lbPrompt.Text = "Enter Reason Code"
                        Me.lbDefective.Visible = True
                        Me.lbDistressed.Visible = True
                        Me.lbEndOfRunPartial.Visible = True
                        Me.lbTest.Visible = True
                        Me.lbCheese.Visible = True
                        Me.lbEndsCurls.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If


                Case 3 'Enter Reason Code
                    If CInt(Me.txData.Text) < 1 Or CInt(Me.txData.Text) > 6 Then
                        Me.lbError.Text = "Reason Code must be 1, 2, 3, 4, 5, or 6 - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbFunction.Text = "3"
                        Me.lbPrompt.Text = "Enter Reason Code"
                        Me.lbDefective.Visible = True
                        Me.lbDistressed.Visible = True
                        Me.lbEndOfRunPartial.Visible = True
                        Me.lbTest.Visible = True
                        Me.lbCheese.Visible = True
                        Me.lbEndsCurls.Visible = True
                        Me.lbGreaseOut.Visible = False

                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        Me.lbReasonCode.Text = Me.txData.Text
                        Me.lbReasonCode.Visible = True
                        Me.lbReason.Visible = True
                        Me.txData.Text = ""
                        Me.txData.Visible = True
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False
                        Me.lbPrompt.Text = "Verify Data then press Verified - Yes button."
                        Me.btVerifyFinished.Text = "Verified - Yes"
                        Me.btVerifyFinished.Visible = True
                        Me.lbDefective.Visible = False
                        Me.lbDistressed.Visible = False
                        Me.lbEndOfRunPartial.Visible = False
                        Me.lbTest.Visible = False
                        Me.lbCheese.Visible = False
                        Me.lbEndsCurls.Visible = False
                        Me.lbGreaseOut.Visible = False
                    End If
                Case 4 'Enter Supervisor Code
                    If ValidateSupCode(Me.txData.Text) = True Then
                        Me.lbError.Text = ""
                        Me.lbError.Visible = False

                        Me.lbFunction.Text = "3"
                        Me.lbPrompt.Text = "Enter Reason Code"
                        Me.lbDefective.Visible = True
                        Me.lbDistressed.Visible = True
                        Me.lbEndOfRunPartial.Visible = True
                        Me.lbTest.Visible = True
                        Me.lbCheese.Visible = True
                        Me.lbEndsCurls.Visible = True
                        Me.lbGreaseOut.Visible = False

                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else
                        Me.lbError.Text = "Supervisor Code entered not valid in the system. Re-Enter Supervisor Override Code or Press Restart Entry button to cancel transaction"
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "4"
                        Me.lbPrompt.Text = "Re-Enter Supervisor Override Code"
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If
                    
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Function ValidateSupCode(ByVal supcode As String) As Boolean
        Dim soc As String = Nothing
        ValidateSupCode = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Connection to Warehouse Data failed. Press Restart Entry button to try again.""")
            Dim sqlCmdSupOC As New System.Data.SqlClient.SqlCommand
            sqlCmdSupOC = DB.NewSQLCommand("SQL.Query.ValidateSOC")
            If sqlCmdSupOC Is Nothing Then Throw New Exception("""SOC Lookup Command failed. Press Restart Entry button to try again.""")
            sqlCmdSupOC.Parameters.AddWithValue("@soc", UCase(Trim(Me.txData.Text)))
            soc = sqlCmdSupOC.ExecuteScalar()
            sqlCmdSupOC.Dispose() : sqlCmdSupOC = Nothing
            DB.KillSQLConnection()
            If Not soc Is Nothing Then
                ValidateSupCode = True
                Common.SaveVariable("SOC", UCase(Me.txData.Text), Page)
            End If
        Catch ex As Exception
            lbError.Text = "Supervisor Override Validation failed with Error = " & ex.Message.ToString
            lbError.Visible = True
        End Try

    End Function

    Public Function ProcessCaseLabel(ByVal caselbl As String) As Boolean
        ProcessCaseLabel = False
        Dim dsCaseLabel As New Data.DataSet
        Dim sqlCmdCaseLabel As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Dim strGTIN As String = Mid(caselbl, 3, 14)
        Dim strVersion As String = Mid(caselbl, 42, 3)

        Try
            sqlString = "Select * from vwGtinProductsAll Where GTIN = " & CLng(strGTIN) & " And Version = " & CInt(strVersion)

            'Get Record to acquire Formula and Prod Description
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdCaseLabel = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdCaseLabel Is Nothing Then
                    dsCaseLabel = DB.GetDataSet(sqlCmdCaseLabel)
                    sqlCmdCaseLabel.Dispose() : sqlCmdCaseLabel = Nothing
                    DB.KillSQLConnection()
                End If
            End If

            If dsCaseLabel.Tables(0).Rows.Count < 1 Then
                ProcessCaseLabel = False
            Else
                'Record found for GTIN and Version
                Session.Item("Formula") = dsCaseLabel.Tables(0).Rows(0).Item("Formula")
                Session.Item("ProductID") = dsCaseLabel.Tables(0).Rows(0).Item("pk_nProductID")
                Session.Item("ProdDesc") = dsCaseLabel.Tables(0).Rows(0).Item("strProductDescription")
                Session.Item("Version") = dsCaseLabel.Tables(0).Rows(0).Item("pk_nVersion")
                Session.Item("StuffGroup") = dsCaseLabel.Tables(0).Rows(0).Item("AS400StuffingGroup")
                ProcessCaseLabel = True
            End If
        Catch ex As Exception
            ProcessCaseLabel = False
        End Try
        
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'ReOpen IC_NumberTwoNew.aspx
        Common.SaveVariable("newURL", "~/IC_NumberTwoNew.aspx", Page)
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
                Me.lbError.Text = "Number 2 product: " & Me.lbFormula.Text & "-" & Me.lbProdDesc.Text & " - Lot# " & Me.lbLotData.Text & " - for " & Me.lbPoundsData.Text & " Pounds was created."
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

            Select Case _whs
                Case Is = "17"
                    sqlCmdWriteNumberTwo.CommandText = "insertNumberTwoNewVer17 "
                Case Is = "35"
                    sqlCmdWriteNumberTwo.CommandText = "insertNumberTwoNewVer35 "
                Case Is = "20"
                    sqlCmdWriteNumberTwo.CommandText = "insertNumberTwoNewVer2 "
            End Select

            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrPartNo", Session.Item("GTIN").ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrVersion", Session.Item("Version").ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrStuffingGroup", Session.Item("StuffGroup").ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrFormula", Me.lbFormula.Text)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrLotNumber", Me.lbLotData.Text)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnPounds", CInt(Me.lbPoundsData.Text))
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnReasonCode", CInt(Me.lbReasonCode.Text))
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pdtmEntered", dtmEntered)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@soc", Session.Item("SOC").ToString)
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