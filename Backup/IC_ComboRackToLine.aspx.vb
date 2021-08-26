Partial Public Class IC_ComboRackToLine
    Inherits System.Web.UI.Page

    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _function As Integer
    Public _CheckOldestLot As String
    Public _comboExists As Boolean
    Public _formula As String
    Public _cutsize As String
    Public _combolot As String
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
        _CheckOldestLot = ConfigurationManager.AppSettings.Get("CheckOldestLot").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            'Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(Me.lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If
    End Sub

    Public Sub InitProcess()
        _formula = ""
        _cutsize = ""
        _combolot = ""
        Common.SaveVariable("ComboFormula", "", Page)
        Common.SaveVariable("ComboCutSize", "", Page)
        Common.SaveVariable("ComboLot", "", Page)
        Common.SaveVariable("ComboNet", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbRackId.Visible = True
        Me.lbRackIDVal.Visible = True
        Me.lbRackIDVal.Text = ""
        Me.lbLine.Visible = True
        Me.lbLineVal.Visible = True
        Me.lbLineVal.Text = ""
        Me.btAssignToLine.Visible = False
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan or Enter ComboID#"
        Me.lbFunction.Text = "1"
        _function = CInt(Me.lbFunction.Text)
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Try
            If RTrim(Me.txData.Text).Length < 1 Then Throw New Exception("""Entry cannot be blank""")

            Select Case _function
                Case Is = 1 'Rack ID # was entered
                    If IsNumeric(Me.txData.Text) = False Then Throw New Exception("""RackID# entry must be numeric. Please enter ComboID# again.""")
                    'Validate RackID# has a status of 200 before asking for the Line #
                    If ValidateRackID(Me.txData.Text) = True Then
                        If _CheckOldestLot = "Y" Then
                            If ValidateOldestLot() = False Then Throw New Exception("""RackID# entry is not from the Oldest Lot Available. Please enter another ComboID or see your supervisor.""")
                        End If
                    End If
                    Me.lbRackIDVal.Text = Me.txData.Text
                    Me.txData.Text = ""
                    Me.lbPrompt.Text = "Enter Line #"
                    Me.lbFunction.Text = "2"
                    Common.JavaScriptSetFocus(Page, Me.txData)

                Case Is = 2 'Line # was entered
                    If IsNumeric(Me.txData.Text) = False Then Throw New Exception("""Line# entry must be numeric. Please enter ComboID# again.""")
                    'Validate Line #
                    If ValidateLine(Me.txData.Text) = True Then
                        'Show Assign button 
                        Me.btAssignToLine.Visible = True
                        Me.lbLineVal.Text = Me.txData.Text
                        Me.txData.Visible = False
                        Me.lbPrompt.Text = "Verify entries and Press Assign To Line button, or press Restart button to start over."
                        Me.lbFunction.Text = "0"
                    End If
            End Select

        Catch ex As Exception

        End Try
    End Sub

    Public Function ValidateRackID(ByVal _comboid As Long) As Boolean
        ValidateRackID = False
        Dim dsComboID As New Data.DataSet
        Dim strSelect As String = ""
        Try

            Dim iComboID As Int64 = _comboid

            strSelect = "Select * from vwCD_CombosAvailForPackaging Where ComboId = " & iComboID

            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ValidateComboID function""")
            Dim sqlCmdComboID As New System.Data.SqlClient.SqlCommand
            sqlCmdComboID = DB.NewSQLCommand3(strSelect)
            If sqlCmdComboID Is Nothing Then Throw New Exception("""Error creating sqlCmdComboID Command during ValidateComboID function""")

            sqlCmdComboID.Parameters.AddWithValue("@ComboID", iComboID)
            dsComboID = DB.GetDataSet(sqlCmdComboID)
            sqlCmdComboID.Dispose() : sqlCmdComboID = Nothing
            DB.KillSQLConnection()

            If dsComboID Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ValidateComboID function""")

            If dsComboID.Tables(0).Rows.Count < 1 Then Throw New Exception("""ComboID scanned is not available for Packaging.""")

            Common.SaveVariable("ComboFormula", dsComboID.Tables(0).Rows(0).Item("Formula"), Page)
            Common.SaveVariable("ComboCutSize", dsComboID.Tables(0).Rows(0).Item("CutSize"), Page)
            Common.SaveVariable("ComboLot", dsComboID.Tables(0).Rows(0).Item("Lot"), Page)
            Common.SaveVariable("ComboNet", dsComboID.Tables(0).Rows(0).Item("NetWeight"), Page)


            ValidateRackID = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "! Press Restart Entry button to try again or see supervisor"
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function ValidateOldestLot() As Boolean
        ValidateOldestLot = False
        Dim dsOldestLot As New Data.DataSet
        Dim strSelect As String = ""
        Dim strOldestLotAvail As String = ""

        _formula = Common.GetVariable("ComboFormula", Page).ToString
        _cutsize = Common.GetVariable("ComboCutSize", Page).ToString
        _combolot = Common.GetVariable("ComboLot", Page).ToString()

        Try

            strSelect = "Select Top 1 Lot from vwCD_ValidateOldestLot Where Formula = '" & _formula & "' And CutSize = '" & _cutsize & "'"

            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ValidateComboID function""")
            Dim sqlCmdOldestLot As New System.Data.SqlClient.SqlCommand
            sqlCmdOldestLot = DB.NewSQLCommand3(strSelect)
            If sqlCmdOldestLot Is Nothing Then Throw New Exception("""Error creating sqlCmdComboID Command during ValidateComboID function""")

            dsOldestLot = DB.GetDataSet(sqlCmdOldestLot)
            sqlCmdOldestLot.Dispose() : sqlCmdOldestLot = Nothing
            DB.KillSQLConnection()

            If dsOldestLot Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ValidateComboID function""")

            If dsOldestLot.Tables(0).Rows.Count < 1 Then Throw New Exception("""ComboID scanned has a Formula/Cut Size problem.""")

            strOldestLotAvail = dsOldestLot.Tables(0).Rows(0).Item("Lot")

            If strOldestLotAvail <> _combolot Then Throw New Exception("""ComboID scanned is not from Oldest Lot Available in Wip Cooler.""")

            ValidateOldestLot = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "! Press Restart Entry button to try again or see supervisor"
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function ValidateLine(ByVal _line As Integer) As Boolean
        ValidateLine = False
        Dim dsValidateLine As New Data.DataSet
        Dim strSelect As String = ""

        Try

            strSelect = "Select Line from LineMaster Where Line = " & _line & " And Warehouse = " & _whs

            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ValidateLine function""")
            Dim sqlCmdValidateLine As New System.Data.SqlClient.SqlCommand
            sqlCmdValidateLine = DB.NewSQLCommand3(strSelect)
            If sqlCmdValidateLine Is Nothing Then Throw New Exception("""Error creating sqlCmdComboID Command during ValidateLine function""")

            dsValidateLine = DB.GetDataSet(sqlCmdValidateLine)
            sqlCmdValidateLine.Dispose() : sqlCmdValidateLine = Nothing
            DB.KillSQLConnection()

            If dsValidateLine Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ValidateLine function""")

            If dsValidateLine.Tables(0).Rows.Count < 1 Then Throw New Exception("""Line # Entered is not valid for your Warehouse. Please enter Line # again.""")

            ValidateLine = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "!"
            Me.lbError.Visible = True
            Me.lbFunction.Text = "2"
            Me.txData.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            DB.KillSQLConnection()
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_ComboMenu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub btAssignToLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAssignToLine.Click
        'Use stored procedure to update status to 201 and write record to CD_CombosConsumed table
        Dim sqlCmdAssignToLine As New System.Data.SqlClient.SqlCommand
        Dim sqlAssignToLine As String = "spCD_AssignToLine "
        Dim iReturnValue As Integer

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during AssignToLine function""")
            sqlCmdAssignToLine = DB.NewSQLCommand2(sqlAssignToLine)
            If Not sqlCmdAssignToLine Is Nothing Then
                sqlCmdAssignToLine.Parameters.AddWithValue("@nComboID", CLng(Me.lbRackIDVal.Text))
                sqlCmdAssignToLine.Parameters.AddWithValue("@nLine", 100 + CInt(Me.lbLineVal.Text))
                sqlCmdAssignToLine.Parameters.AddWithValue("@nNetWeight", CInt(Common.GetVariable("ComboNet", Page)))
                sqlCmdAssignToLine.Parameters.AddWithValue("@nAssociateID", CInt(Common.GetVariable("UserID", Page).ToString))
                sqlCmdAssignToLine.Parameters.AddWithValue("@iErrorCode", 0)
                sqlCmdAssignToLine.Parameters("@iErrorCode").Direction = ParameterDirection.Output
                sqlCmdAssignToLine.ExecuteNonQuery()
                iReturnValue = sqlCmdAssignToLine.Parameters("@iErrorCode").Value
                sqlCmdAssignToLine.Dispose() : sqlCmdAssignToLine = Nothing

            Else
                Me.lbError.Text = "Command Error occurred while processing Combo To Line! Try again or see your supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
                Me.btAssignToLine.Visible = True
                Me.lbFunction.Text = "0"
            End If

            'No errors occured during stored procedure
            If iReturnValue = 0 Then
                Me.lbFunction.Text = "1"
                InitProcess()
            Else
                Throw New Exception("""Assign To Line Failed for current ComboID. Press Retry button and attempt to assign ComboID to Line again.""")
                Me.lbFunction.Text = "0"
            End If

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "!"
            Me.lbError.Visible = True
            Me.txData.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txData)
        End Try
    End Sub
End Class