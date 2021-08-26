Partial Public Class IC_ComboMenu
    Inherits System.Web.UI.Page

    Public userspecial As String = "N"
    Public department As String = Nothing
    Public whse As String = Nothing
    Public strURL As String = Nothing

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.txOption.Text = Common.GetVariable("Option", Page)

        strURL = Common.GetVariable("newURL", Page)
        Common.SaveVariable("newURL", Nothing, Page)

        If Not strURL Is Nothing Then
            Common.SaveVariable("Option", "", Page)
            Page.Response.Clear()
            Page.Response.Redirect(strURL)
            Page.Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Common.CheckLogin(Page)
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page)
        whse = Common.GetVariable("Whse", Page).ToString
        If whse = "21" Then
            Me.lbPageTitle.Text = "OW Plant 2 Combo - Menu"
        ElseIf whse = "35" Then
            Me.lbPageTitle.Text = "OW Weeden Creek Combo - Menu"
        End If

        If Not Page.IsPostBack Then
            Common.SaveVariable("Option", "", Page)
            Call InitMenuLoad()
            'btEnter.Visible = True
            Me.btExit.Visible = True
            Me.txOption.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txOption)
        Else
            If Me.txOption.Text.Length < 1 Then
                Call InitMenuLoad()
                Me.btExit.Visible = True
                Me.txOption.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txOption)
            End If
        End If
    End Sub

    Public Sub InitMenuLoad()
        Me.lbError.Visible = False
        Me.lbError.Text = ""
        Me.lbUser.Visible = True


        userspecial = Common.GetVariable("Special", Page)
        whse = Common.GetVariable("Whse", Page).ToString

        Select Case whse
            Case "21"
                'Set Menu for Slicing Department
                Me.lbOption1.Text = "1. Xfer Combo to Weeden Creek"
                Me.lbOption2.Text = ""
                Me.lbOption3.Text = ""              '"3. Receive Combo from Weeden Creek"
                Me.lbOption4.Text = ""              '"4. Void Combo Errors"
                Me.lbOption5.Text = ""
                Me.lbOption6.Text = ""


            Case "35"
                'Set Menu for Slicing Department
                Me.lbOption1.Text = "1. Scan Combo in WIP Cooler"
                Me.lbOption2.Text = "2. Assign Combo to Line"
                Me.lbOption3.Text = ""                                      '"3. Lookup Combo for Item/Ver"
                Me.lbOption4.Text = ""                                      '"4. Return Combo to Cooler"
                Me.lbOption5.Text = ""                                      '"5. Void Depleted Combo"
                Me.lbOption6.Text = ""                                      '"6. Return Combo to Plant 2"

        End Select
        Common.JavaScriptSetFocus(Page, Me.txOption)

    End Sub

    Protected Sub txOption_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txOption.TextChanged
        If IsNumeric(Me.txOption.Text) Then
            Call ProcessSelection()
        Else
            Me.lbError.Text = "Option # entered must be a number"
            Me.lbError.Visible = True
            Me.txOption.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txOption)
        End If
    End Sub

    Public Sub ProcessSelection()
        Dim xBadSelect As Boolean
        xBadSelect = False
        whse = Common.GetVariable("Whse", Page).ToString

        Select Case Me.txOption.Text
            Case "1"
                If Me.lbOption1.Text.Length > 0 Then
                    If whse = "21" Then
                        strURL = "~/IC_ComboXfer_WC.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf whse = "35" Then
                        strURL = "~/IC_ComboToWip.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "2"
                If Me.lbOption2.Text.Length > 0 Then
                    If whse =  "35" Then
                        strURL = "~/IC_ComboToLine.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "3"
                If Me.lbOption3.Text.Length > 0 Then
                    If whse = "21" Then
                        strURL = "" 'IC_ComboReceive
                        Common.SaveVariable("ScreenParam", "21", Page)
                    ElseIf whse = "35" Then
                        strURL = "" 'IC_MoveComboWC
                        Common.SaveVariable("ScreenParam", "LINE", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "4"
                If Me.lbOption4.Text.Length > 0 Then
                    If whse = "21" Then
                        strURL = "" 'IC_ComboVoid
                        Common.SaveVariable("ScreenParam", "21", Page)
                    ElseIf whse = "35" Then
                        strURL = "" 'IC_MoveComboWC
                        Common.SaveVariable("ScreenParam", "COOLER", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "5"
                If Me.lbOption5.Text.Length > 0 Then
                    If whse = "21" Then
                        strURL = ""
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf whse = "35" Then
                        strURL = "" 'IC_ComboVoid
                        Common.SaveVariable("ScreenParam", "35", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "6"
                If Me.lbOption6.Text.Length > 0 Then
                    If whse = "21" Then
                        strURL = ""
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf whse = "35" Then
                        strURL = "" 'Clone IC_BinTransfertoTruck.aspx
                        Common.SaveVariable("ScreenParam", "35", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case Else
                    xBadSelect = True

        End Select

        If xBadSelect = True Then
            Me.lbError.Text = "Please make an entry from menu, or see your supervisor."
            Me.lbError.Visible = True
            Me.txOption.Text = ""
            Common.JavaScriptSetFocus(Page, Me.txOption)
        Else
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Common.SaveVariable("Option", Me.txOption.Text, Page)
            Common.SaveVariable("newURL", strURL, Page)
        End If
    End Sub

    Protected Sub btExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btExit.Click
        Page.Session.Clear()
        Page.Response.Redirect("~/IC_Menu.aspx")
    End Sub
End Class