Partial Public Class IC_NumberTwoPackland
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
        Me.lbLocation.Text = "PACKLAND"
        Me.lbLocation.Visible = True
        Me.btMixed.Visible = True

        Session.Item("Formula") = ""
        Session.Item("ProductID") = ""
        Session.Item("ProdDesc") = ""
        Session.Item("Version") = ""
        Session.Item("GTIN") = ""

        lbPrompt.Text = "Scan Case Label or Press MIXED button"
        lbFunction.Text = "1"
    End Sub

    Protected Sub btMixed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btMixed.Click
        Session.Item("GTIN") = "99999999999999"
        Session.Item("Formula") = "MIXED"
        Session.Item("Version") = "999"
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

            If IsNumeric(Me.txData.Text) = False Then
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
                        Me.lbLotData.Text = Mid(Me.txData.Text, 36, 3)
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

                    Common.JavaScriptSetFocus(Page, Me.txData)

                Case 3 'Enter Reason Code
                    If CInt(Me.txData.Text) < 1 Or CInt(Me.txData.Text) > 4 Then
                        Me.lbError.Text = "Reason Code must be 1, 2, 3, or 4 - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbFunction.Text = "3"
                        Me.lbPrompt.Text = "Enter Reason Code"
                        Me.lbDefective.Visible = True
                        Me.lbDistressed.Visible = True
                        Me.lbEndOfRunPartial.Visible = True
                        Me.lbTest.Visible = True

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

                    End If
            End Select
        Catch ex As Exception

        End Try
    End Sub

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
                'Session.Item("StuffGroup") = dsCaseLabel.Tables(0).Rows(0).Item("AS400StuffingGroup")
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
        Common.SaveVariable("newURL", "~/IC_NumberTwoPackland.aspx", Page)
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
            sqlCmdWriteNumberTwo.CommandText = "insertNumberTwoPackland "
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrPartNo", Session.Item("GTIN").ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrVersion", Session.Item("Version").ToString)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrFormula", Me.lbFormula.Text)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pstrLotNumber", Me.lbLotData.Text)
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnPounds", CInt(Me.lbPoundsData.Text))
            sqlCmdWriteNumberTwo.Parameters.AddWithValue("@pnReasonCode", CInt(Me.lbReasonCode.Text))
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