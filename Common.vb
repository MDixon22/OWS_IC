Public Class Common
    Private Const _strSessionPrefix As String = "App."

    ''' <summary>
    ''' Writes a session state variable to reside between pages.
    ''' </summary>
    ''' <param name="strVariable">The name of the variable to save.</param>
    ''' <param name="strValue">The value to save to the variable.</param>
    ''' <param name="uiPage">Web page object holding the session state variables.</param>
    ''' <returns>The name of the variable on success.</returns>
    ''' <remarks></remarks>
    Public Shared Function SaveVariable(ByVal strVariable As String, _
                                        ByVal strValue As String, _
                                        ByRef uiPage As System.Web.UI.Page) As String
        SaveVariable = ""
        If strVariable Is Nothing Or uiPage Is Nothing Then Exit Function
        If Len(strVariable) < 2 Then Exit Function
        strVariable = Common._strSessionPrefix + strVariable
        uiPage.Session.Remove(strVariable)
        uiPage.Session.Add(strVariable, strValue)
        SaveVariable = strVariable
    End Function

    ''' <summary>
    ''' Retrieves the session state variable that we previously saved.
    ''' </summary>
    ''' <param name="strVariable">The name of the variable to retrieve.</param>
    ''' <param name="uiPage">Web page object holding the session state variables.</param>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVariable(ByVal strVariable As String, _
                                       ByRef uiPage As System.Web.UI.Page) As String
        GetVariable = Nothing
        If strVariable Is Nothing Then Exit Function
        If Len(strVariable) < 2 Then Exit Function
        GetVariable = uiPage.Session.Item(Common._strSessionPrefix + strVariable)
    End Function

    ''' <summary>
    ''' Inserts client-side JavaScript to popup a confirm message box 
    ''' </summary>
    ''' <param name="uiPage">Web page object holding the session state variables.</param>
    ''' <param name="ConfirmSave">String of your Javascript.</param>
    ''' <remarks></remarks>
    Public Shared Sub JavaScriptConfirmSave(ByRef uiPage As System.Web.UI.Page, _
                                         ByRef ConfirmSave As String)
        If Not uiPage Is Nothing Then
            uiPage.ClientScript.RegisterStartupScript(uiPage.GetType(), "ConfirmSave", ConfirmSave, False)
        End If
    End Sub
    ''' <summary>
    ''' Inserts client-side JavaScript to set the focus to a particular control.
    ''' </summary>
    ''' <param name="uiPage">Web page object holding the session state variables.</param>
    ''' <param name="webControl">The control to set focus to.</param>
    ''' <remarks></remarks>
    Public Shared Sub JavaScriptSetFocus(ByRef uiPage As System.Web.UI.Page, _
                                         ByRef webControl As System.Web.UI.Control)
        If Not uiPage Is Nothing And Not webControl Is Nothing Then
            webControl.Focus()
            uiPage.ClientScript.RegisterStartupScript(webControl.GetType(), "setFocus", "<script FOR=window EVENT=onload>document.form1." + webControl.ID + ".focus();</script>", False)
        End If
    End Sub

    ''' <summary>
    ''' Check for login session variable and redirect to the login page if not found.
    ''' </summary>
    ''' <param name="uiPage">Web page object holding the session state variables.</param>
    ''' <remarks></remarks>
    Public Shared Sub CheckLogin(ByRef uiPage As System.Web.UI.Page)
        If Common.GetVariable("LoggedIn", uiPage) Is Nothing Then
            uiPage.Response.Clear()
            uiPage.Response.Redirect("~/", True)
        End If

    End Sub

    Public Shared Function FindLotFromBarcode(ByVal barcodedata As String, ByVal whse As Integer, ByVal gtin As Long, ByVal ver As Integer, ByVal codedate As String) As Integer
        Dim sqlCmdAddPrintJobLot As New System.Data.SqlClient.SqlCommand
        Dim _caselotYY As String
        Dim _caselotDDD As String

        'barcodedata = full 12 digit AI10
        'LookupLot
        Dim LotNum As Integer = 0
        'Check for Entry in OWLABELS db PrintJobs table.
        'Added 3/11/2021 - check to see if Lot exists in PrintJobs table in OWLabels database where PrintJob = CLng(Mid$(Me.txCaseLabel.Text, 27, 12))
        If DB.MakeSQLConnection("OWLabels") Then Throw New Exception("Connection to OWLabels Database failed.  Please try again")
        Dim sqlPrintJob As String = "Select LotYYDDD from vwGetPrintJobLot Where PrintJob = " & CLng(barcodedata)
        Dim sqlCmdPrintJobLot As New System.Data.SqlClient.SqlCommand
        sqlCmdPrintJobLot = DB.NewSQLCommand3(sqlPrintJob)
        If sqlCmdPrintJobLot Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
        LotNum = sqlCmdPrintJobLot.ExecuteScalar()
        sqlCmdPrintJobLot.Dispose() : sqlCmdPrintJobLot = Nothing
        DB.KillSQLConnection()

        If LotNum < 1 Then
            'Added 10/19/2020 - Write record to PrintJobs table in OWLabels database. 
            _caselotDDD = Mid$(barcodedata, 10, 3)
            _caselotYY = "20" + Mid$(barcodedata, 8, 2).ToString
            LotNum = Right(_caselotYY.ToString, 2) & _caselotDDD.ToString

            If DB.MakeSQLConnection("OWLabels") Then Throw New Exception("Connection to OWLabels Database failed.  Please try again")
            sqlCmdAddPrintJobLot = DB.NewSQLCommand("SQL.IC_InsPrintJob")
            If sqlCmdAddPrintJobLot Is Nothing Then Throw New Exception("Command creation failed.  Please try again")
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@PrintJob", barcodedata)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@PrintAt", Now)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Plant", whse)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Line", Mid$(barcodedata, 1, 2))
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Printer", Mid$(barcodedata, 3, 2))
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@GTIN", gtin)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Version", ver)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@CodeDate", codedate)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Lot", _caselotDDD)
            sqlCmdAddPrintJobLot.Parameters.AddWithValue("@Year", _caselotYY)
            sqlCmdAddPrintJobLot.ExecuteNonQuery()
            sqlCmdAddPrintJobLot.Dispose() : sqlCmdAddPrintJobLot = Nothing
            DB.KillSQLConnection()

        Else
            _caselotDDD = Mid$(LotNum.ToString, 3, 3)
            _caselotYY = "20" + Mid$(LotNum.ToString, 1, 2)
            LotNum = Right(_caselotYY.ToString, 2) & _caselotDDD.ToString

        End If

        Return LotNum

    End Function
End Class
