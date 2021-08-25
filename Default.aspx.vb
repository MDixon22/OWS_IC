Partial Public Class _Default
    Inherits System.Web.UI.Page

    Public UserName As String = Nothing
    Public Special As String = "N"
    Public strURL As String = ""

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.txUserID.Text = Common.GetVariable("UserID", Page)
        Me.txPassword.Text = Common.GetVariable("Password", Page)

        strURL = Common.GetVariable("newURL", Page)
        'strURL = Me.hdnNewURL.Value
        Common.SaveVariable("newURL", Nothing, Page)
        'Me.hdnNewURL.Value = Nothing

        If Not strURL Is Nothing Then
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.txUserID.Attributes("value") = Me.txUserID.Text
        Me.txPassword.Attributes("value") = Me.txPassword.Text
        If Not Page.IsPostBack Then '1st time into screen
            Me.txUserID.Text = ""
            Me.txPassword.Text = ""
            Me.txPassword.Visible = False
            Me.lbPassword.Visible = False
            Common.JavaScriptSetFocus(Page, Me.txUserID)
        Else 'Is a postback
            If Me.txPassword.Text.Length < 1 And Me.txUserID.Text.Length < 1 Then
                Common.JavaScriptSetFocus(Page, Me.txUserID)
            ElseIf Me.txPassword.Text.Length < 1 And Me.txUserID.Text.Length > 1 Then
                Common.JavaScriptSetFocus(Page, Me.txPassword)
            End If
        End If
    End Sub

    Private Sub txUserID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txUserID.TextChanged
        Dim dsUser As New Data.DataSet
        Dim sqlCmdUser As New System.Data.SqlClient.SqlCommand
        Try
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdUser = DB.NewSQLCommand("SQL.Query.UserLookup2")
                If Not sqlCmdUser Is Nothing Then
                    sqlCmdUser.Parameters.AddWithValue("@UserID", UCase(Me.txUserID.Text))
                    dsUser = DB.GetDataSet(sqlCmdUser)
                    sqlCmdUser.Dispose() : sqlCmdUser = Nothing
                    DB.KillSQLConnection()
                    If Not dsUser Is Nothing Then
                        If dsUser.Tables(0).Rows.Count > 0 Then
                            'Valid USERID was entered save needed values
                            Common.SaveVariable("UserID", UCase(Me.txUserID.Text), Page)
                            Common.SaveVariable("User", dsUser.Tables(0).Rows(0).Item("EmpName").ToString, Page)
                            Common.SaveVariable("Special", UCase(dsUser.Tables(0).Rows(0).Item("Special").ToString), Page)
                            Common.SaveVariable("Department", dsUser.Tables(0).Rows(0).Item("Department").ToString, Page)
                            Common.SaveVariable("Password", UCase(dsUser.Tables(0).Rows(0).Item("OPERATOR_PASSWORD").ToString), Page)
                            Common.SaveVariable("Whse", UCase(dsUser.Tables(0).Rows(0).Item("Whse").ToString), Page)
                            Me.txPassword.Visible = True
                            Me.lbPassword.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPassword)
                        Else
                            lbError.Text = "UserID entered is not valid try again"
                            lbError.Visible = True
                            UserName = ""
                            Special = ""
                            Me.txUserID.Text = ""
                            Common.JavaScriptSetFocus(Page, Me.txUserID)
                        End If
                    Else
                        'Failed to retrieve dataset
                        Me.lbError.Text = "A data error occurred during UserId Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txUserID)
                    End If
                Else
                    lbError.Text = "A command error occurred during UserId Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                    lbError.Visible = True
                    Me.txUserID.Text = ""
                    Common.JavaScriptSetFocus(Page, Me.txUserID)
                End If
            Else
                lbError.Text = "A communication error occurred during UserId Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                lbError.Visible = True
                Me.txUserID.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txUserID)
            End If
        Catch ex As Exception
            lbError.Text = "Error " & ex.Message.ToString & " occurred during UserId Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
            lbError.Visible = True
            UserName = ""
            Special = ""
            Me.txUserID.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txUserID)
        Finally
            If Not sqlCmdUser Is Nothing Then
                sqlCmdUser.Dispose() : sqlCmdUser = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Private Sub txPassword_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txPassword.TextChanged
        If Me.txPassword.Text.Length > 0 Then
            If UCase(RTrim(Me.txPassword.Text)) = UCase(RTrim(Common.GetVariable("Password", Page).ToString)) Then
                Common.SaveVariable("LoggedIn", "True", Page)
                Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
            Else
                lbError.Text = "Password entered is not valid try again"
                lbError.Visible = True
                Common.SaveVariable("LoggedIn", "False", Page)
                Me.txPassword.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txPassword)
            End If
        Else
            lbError.Text = "Password entered is not valid try again"
            lbError.Visible = True
            Common.SaveVariable("LoggedIn", "False", Page)
            Me.txPassword.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txPassword)
        End If
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        Me.txUserID.Text = ""
        Me.txPassword.Text = ""
        Me.txUserID.Attributes("value") = ""
        Me.txPassword.Attributes("value") = ""
        Common.SaveVariable("UserID", "", Page)
        Common.SaveVariable("User", "", Page)
        Common.SaveVariable("Special", "", Page)
        Common.SaveVariable("Department", "", Page)
        Common.SaveVariable("Password", "", Page)
        Me.txPassword.Visible = False
        Me.lbPassword.Visible = False
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Common.JavaScriptSetFocus(Page, Me.txUserID)
    End Sub

End Class