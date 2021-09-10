Partial Public Class IC_ShippingReceipt
    Inherits System.Web.UI.Page

    Public Shared _oQty As String
    Public Shared _bin As String
    Public Shared _trantype As String
    Public Shared _datemodified As DateTime
    Public Shared _dateentered As DateTime
    Public Shared _whs As String
    Public Shared _company As String
    Public Shared _pallet As String
    Public Shared _status As String
    Public Shared _gtin As String
    Public Shared _codedate As String
    Public Shared _shippingreceipt As String = Nothing
    Public Shared strURL As String = Nothing

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
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
            'Prompt Error and Set Focus to Pallet TextBox
            If RTrim(Me.txPallet.Text).Length < 1 Then
                Me.lbError.Text = "Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("Pallet", Nothing, Page)
        Common.SaveVariable("Status", Nothing, Page)
        Common.SaveVariable("oQty", Nothing, Page)

        Me.txCaseLabel.Visible = False
        Me.lbCaseLabel.Visible = False
        Me.txQty.Visible = False
        Me.lbQty.Visible = False
        Me.lbYN.Visible = False
        Me.txYN.Visible = False
        Me.txQty.Text = ""
        Me.txPallet.Text = ""
        Me.txCaseLabel.Text = ""
        Me.txYN.Text = ""

        _pallet = Nothing
        _status = Nothing
        _shippingreceipt = Nothing
        _oQty = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        strURL = Nothing
        _gtin = Nothing
        _codedate = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.btRestart.Visible = True
        Me.btNextPallet.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

        Try
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                'Pallet # entered is valid length check numeric
                iPallet = CInt(Me.txPallet.Text)

                'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetShipPalletInfo")
                    If Not sqlCmdPallet Is Nothing Then
                        sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
                        dsPallet = DB.GetDataSet(sqlCmdPallet)
                        sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
                        DB.KillSQLConnection()
                        If dsPallet Is Nothing Then
                            Me.lbError.Text = "Data Error occurred while validating Pallet #! Check battery and wireless connection - then try again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                        Else
                            If dsPallet.Tables(0).Rows.Count > 0 Then
                                _pallet = Me.txPallet.Text
                                _status = dsPallet.Tables(0).Rows(0).Item("Status")
                                _bin = dsPallet.Tables(0).Rows(0).Item("Bin")
                                _oQty = dsPallet.Tables(0).Rows(0).Item("Qty")
                                Common.SaveVariable("oQty", _oQty, Page)
                                _codedate = dsPallet.Tables(0).Rows(0).Item("CodeDate")
                                _gtin = dsPallet.Tables(0).Rows(0).Item("GTIN")

                                Select Case UCase(RTrim(_status))
                                    Case Is = "V"
                                        Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                                        Me.lbError.Visible = True
                                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                                    Case Else
                                        If _bin = "WCFG" Or _bin = "OWRP" Or _bin = "OWFG" Then
                                            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                                            Common.SaveVariable("Status", _status, Page)
                                            Me.txQty.Visible = True
                                            Me.lbQty.Visible = True
                                            Me.lbPrompt.Text = "Enter Pallet Qty"
                                            Common.JavaScriptSetFocus(Page, Me.txQty)
                                        ElseIf _bin = "OWFGM" Then
                                            'SAM 12/20/2019 - Modify to prompt for case label scan if _bin = OWFGM for Montgomery
                                            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
                                            Common.SaveVariable("Status", _status, Page)
                                            Me.lbCaseLabel.Visible = True
                                            Me.txCaseLabel.Visible = True
                                            Me.lbPrompt.Text = "Scan Case Label from Pallet"
                                            Common.JavaScriptSetFocus(Page, Me.txCaseLabel)

                                        Else
                                            Me.lbPrompt.Text = "Pallet scanned was already processed. Press Next Pallet button to continue"
                                            Me.btNextPallet.Visible = True
                                            Me.btRestart.Visible = False
                                            Common.JavaScriptSetFocus(Page, Me.txPallet)
                                        End If


                                End Select

                            Else 'Record does not exist for Pallet entered
                                Me.lbError.Text = "Pallet# not in system - see supervisor."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txPallet)
                            End If
                        End If
                    Else
                        Me.lbError.Text = "Command Error occurred while validating Pallet #! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                    End If
                Else
                    Me.lbError.Text = "Connection Error occurred while connecting to database! Check battery and wireless connection - then try again."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                End If

            Else
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while validating Pallet # entered! Check battery and wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Finally
            If Not dsPallet Is Nothing Then
                dsPallet.Dispose() : dsPallet = Nothing
            End If
            If Not sqlCmdPallet Is Nothing Then
                sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txCaseLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txCaseLabel.TextChanged
        Dim sqlCmdGTINVer As New System.Data.SqlClient.SqlCommand
        Dim sqlCmdProdVerFormulaLot As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            'Verify the Case Label - make sure GTIN, Code Date match for Pallet Scanned
            If _gtin <> Mid$(Me.txCaseLabel.Text, 3, 14) Then Throw New Exception("GTIN does not match for Pallet scanned - see supervisor!")
            If _codedate <> Mid$(Me.txCaseLabel.Text, 19, 6) Then Throw New Exception("Code Date does not match for Pallet scanned - see supervisor!")

            'GTIN and CodeDate have been verified - Pass process onto Quantity Entry
            Me.txQty.Visible = True
            Me.lbQty.Visible = True
            Me.lbPrompt.Text = "Enter Case Qty on Pallet"
            Common.JavaScriptSetFocus(Page, Me.txQty)

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Case Label entered - try again or see your supervisor."
            Me.lbError.Visible = True
            Common.SaveVariable("CaseLabel", Me.txCaseLabel.Text, Page)
        Finally

            If Not sqlCmdGTINVer Is Nothing Then
                sqlCmdGTINVer.Dispose() : sqlCmdGTINVer = Nothing
            End If
            If Not sqlCmdProdVerFormulaLot Is Nothing Then
                sqlCmdProdVerFormulaLot.Dispose() : sqlCmdProdVerFormulaLot = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQty.TextChanged
        'Verify quantity entered with qty assigned when pallet was created
        Dim sqlCmdBin As New System.Data.SqlClient.SqlCommand
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            If Me.txQty.Text.Length > 0 And IsNumeric(Me.txQty.Text) = True Then
                If CInt(Me.txQty.Text) <> CInt(Common.GetVariable("oQty", Page).ToString) Then
                    Me.txYN.Visible = True
                    Me.lbYN.Visible = True
                    Me.lbYorN.Visible = True
                    Me.lbPrompt.Text = "Enter Y to Accept Qty or N to re-enter Qty"
                    Common.JavaScriptSetFocus(Page, Me.txYN)
                Else
                    'Update records for pallet and OWRossProduction leaving qty the same
                    ShippingReceipt(CLng(Me.txPallet.Text), CInt(Me.txQty.Text), CInt(Common.GetVariable("oQty", Page).ToString), "N", Common.GetVariable("UserID", Page).ToString)

                    If Me.lbError.Visible = False Then
                        Me.btNextPallet.Visible = True
                        Me.btRestart.Visible = False
                        Me.lbPrompt.Text = "Pallet # " & Me.txPallet.Text & " was processed. Press Next Pallet button to continue."
                    End If
                End If
            Else 'Nothing entered on screen for Quantity
                Me.lbError.Text = "Qty must be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txQty)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.ToString & " - occurred while validating Qty entered! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txQty)
        Finally
            If Not sqlCmdBin Is Nothing Then
                sqlCmdBin.Dispose() : sqlCmdBin = Nothing
            End If
            DB.KillSQLConnection()
        End Try

    End Sub

    Protected Sub txYN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txYN.TextChanged
        Select Case UCase(Me.txYN.Text)
            Case Is = "Y"
                'Update records for pallet and OWRossProduction changing qty on pallet
                ShippingReceipt(CLng(Me.txPallet.Text), CInt(Me.txQty.Text), CInt(Common.GetVariable("oQty", Page).ToString), "Y", Common.GetVariable("UserID", Page).ToString)

                If Me.lbError.Visible = False Then
                    Me.btNextPallet.Visible = True
                    Me.btRestart.Visible = False
                    Me.lbPrompt.Text = "Pallet # " & Me.txPallet.Text & " was processed and quantity updated. Press Next Pallet button to continue."
                End If
            Case Is = "N"
                Me.lbPrompt.Text = "Enter Pallet Qty"
                Me.txQty.Text = ""
                Common.JavaScriptSetFocus(Page, Me.txQty)
            Case Else
                Me.lbError.Text = "Y or N must be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txYN)
        End Select
    End Sub

    Public Function ShippingReceipt(ByVal _pallet As Long, ByVal _newqty As Integer, ByVal _oqty As Integer, ByVal _yn As String, ByVal _userid As String) As Boolean
        ShippingReceipt = False
        Dim sqlCmdShippingReceipt As New System.Data.SqlClient.SqlCommand
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Create Build Combo Connection not established""")
            If _whs = 17 Then
                sqlCmdShippingReceipt = DB.NewSQLCommand("SQL.Query.WCShippingReceipt17")
            Else
                sqlCmdShippingReceipt = DB.NewSQLCommand("SQL.Query.WCShippingReceipt")
            End If

            If sqlCmdShippingReceipt Is Nothing Then Throw New Exception("""Shipping Receipt - Failed to create query""")
            With sqlCmdShippingReceipt.Parameters
                .AddWithValue("@iPallet", _pallet)
                .AddWithValue("@iNewQty", _newqty)
                .AddWithValue("@iOriginalQty", _oqty)
                .AddWithValue("@sYN", _yn)
                .AddWithValue("@sUserID", _userid)
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdShippingReceipt.ExecuteNonQuery()

            If sqlCmdShippingReceipt.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("""ShippingReceipt - SQL store procedure returned value '" & sqlCmdShippingReceipt.Parameters.Item("@iErrorCode").Value & "'""")
            Else
                ShippingReceipt = True
            End If
            DB.KillSQLConnection()
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & "Please show supervisor this error for Shipping Receipt Pallet so they can notify the IT Department."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Protected Sub btNextPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNextPallet.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class