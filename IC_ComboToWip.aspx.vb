Partial Public Class IC_ComboToWip
    Inherits System.Web.UI.Page

    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _Combo As String
    Public _status As String
    Public _processareaid As String
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

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Combo
            Common.JavaScriptSetFocus(Page, Me.txCombo)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Combo TextBox 
            'Prompt Error and Set Focus to Combo TextBox
            If RTrim(Me.txCombo.Text).Length < 1 Then
                Me.lbError.Text = "Combo # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCombo)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Me.txCombo.Text = ""

        _Combo = Nothing
        _status = Nothing
        _processareaid = Nothing
        _dateentered = Nothing
        strURL = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.btNextCombo.Visible = False
        Me.lbPrompt.Text = "Scan or Enter Combo #"
        Common.JavaScriptSetFocus(Page, Me.txCombo)
    End Sub

    Protected Sub txCombo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCombo.TextChanged
        Dim dsCombo As New Data.DataSet
        Dim sqlCmdCombo As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdComboToWip As New System.Data.SqlClient.SqlCommand

        Try
            Common.SaveVariable("Combo", Me.txCombo.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iCombo As Integer

            If IsNumeric(Me.txCombo.Text) = True Then
                'txCombo # entered is valid length check numeric
                iCombo = CLng(RTrim(Me.txCombo.Text))

                'Validate that the Combo scanned does exist in the system and get Status field
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdCombo = DB.NewSQLCommand3("Select ProcessAreaID,Status From dbo.CD_ComboHeader Where ComboID = " & iCombo)
                    If Not sqlCmdCombo Is Nothing Then
                        dsCombo = DB.GetDataSet(sqlCmdCombo)
                        sqlCmdCombo.Dispose() : sqlCmdCombo = Nothing
                        DB.KillSQLConnection()
                        If dsCombo Is Nothing Then
                            Me.lbError.Text = "Data Error occurred while validating Combo #! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txCombo)
                        Else
                            If dsCombo.Tables(0).Rows.Count > 0 Then
                                _Combo = Me.txCombo.Text
                                _status = dsCombo.Tables(0).Rows(0).Item("Status")
                                _processareaid = dsCombo.Tables(0).Rows(0).Item("ProcessAreaID")

                                If CInt(RTrim(_status)) = 888 Then 'Combo already recieved at WC
                                    Me.lbError.Text = "Combo is on QC Hold and can't be processed with this function. Report this Combo to your supervisor."
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txCombo)
                                ElseIf CInt(RTrim(_status)) < 103 Then
                                    Me.lbError.Text = "Combo needs to be weighed at the kiosk before it can be added to the WIP Cooler"
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txCombo)
                                Else 'Combo is in correct status to process into the WIP Cooler
                                    _dateentered = Now()
                                    Dim sqlUpdateComboToWip As String = "spCD_UpdateComboToWip "
                                    If Not DB.MakeSQLConnection("Warehouse") Then
                                        sqlCmdComboToWip = DB.NewSQLCommand2(sqlUpdateComboToWip)
                                        If Not sqlCmdComboToWip Is Nothing Then
                                            sqlCmdComboToWip.Parameters.AddWithValue("@nComboID", iCombo)
                                            sqlCmdComboToWip.Parameters.AddWithValue("@nAssociateID", Common.GetVariable("UserID", Page).ToString)
                                            sqlCmdComboToWip.ExecuteNonQuery()
                                            sqlCmdComboToWip.Dispose() : sqlCmdComboToWip = Nothing
                                            Me.btNextCombo.Visible = True
                                            Me.lbPrompt.Text = "Combo # " & RTrim(Me.txCombo.Text) & " assigned to the WIP Cooler. Press Next Combo button to continue"
                                        Else
                                            Me.lbError.Text = "Command Error occurred while processing Combo To WIP! Check Wireless Connection and Battery then try again or see your supervisor."
                                            Me.lbError.Visible = True
                                            Common.JavaScriptSetFocus(Page, Me.txCombo)
                                        End If
                                    End If
                                End If
                            Else 'Record does not exist for Combo entered
                                Me.lbError.Text = "Combo# not in system - see supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txCombo)
                            End If
                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while validating Combo #! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txCombo)
                    End If
                Else
                    Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txCombo)
                End If

            Else
                Common.SaveVariable("Combo", "", Page)
                Me.lbError.Text = "Combo # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txCombo)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Combo # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txCombo)
        Finally
            If Not dsCombo Is Nothing Then
                dsCombo.Dispose() : dsCombo = Nothing
            End If
            If Not sqlCmdCombo Is Nothing Then
                sqlCmdCombo.Dispose() : sqlCmdCombo = Nothing
            End If
            If Not sqlCmdComboToWip Is Nothing Then
                sqlCmdComboToWip.Dispose() : sqlCmdComboToWip = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_ComboMenu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub btNextCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNextCombo.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class