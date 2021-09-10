Partial Public Class IC_WorkWithSamplePalletNew
    Inherits System.Web.UI.Page

    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _status As String
    Public _orderid As String
    Public strURL As String = Nothing

#Region "Page Level"

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
                Me.lbError.Text = "Sample Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitProcess()
        Common.SaveVariable("newURL", Nothing, Page)
        Common.SaveVariable("Status", Nothing, Page)
        Common.SaveVariable("EditPalletNumber", Nothing, Page)
        Common.SaveVariable("EditPalletOrderID", Nothing, Page)

        Me.lbPallet.Visible = True
        Me.txPallet.Visible = True
        Me.txPallet.Text = ""

        _pallet = Nothing
        strURL = Nothing
        _status = Nothing
        _orderid = Nothing

        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbExistingGridTitle.Visible = False
        dgExistingProducts.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Sample Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)
    End Sub

#End Region

#Region "User Entry"

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        Dim dsPallet As New Data.DataSet
        Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Integer

            If RTrim(Me.txPallet.Text).Length > 0 And IsNumeric(Me.txPallet.Text) = True Then
                iPallet = CInt(Me.txPallet.Text)

                'Validate that the pallet scanned does exist in the system and get Status field along with ShippingReceipt value
                If Not DB.MakeSQLConnection("Warehouse") Then
                    sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetSamplePallet")
                    If Not sqlCmdPallet Is Nothing Then
                        sqlCmdPallet.Parameters.AddWithValue("@PalletID", iPallet)
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
                                _orderid = dsPallet.Tables(0).Rows(0).Item("OrderID")


                                If RTrim(_status) = "V" Then 'Pallet is void -  Throw an error
                                    Me.lbError.Text = "Pallet Status is Void. See Supervisor to make sure Pallet Status is accurate"
                                    Me.lbError.Visible = True
                                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                                Else 'Pallet is in correct status to proceed
                                    Common.SaveVariable("EditPalletNumber", _pallet, Page)
                                    Common.SaveVariable("Status", _status, Page)
                                    Common.SaveVariable("EditPalletOrderID", _orderid, Page)

                                    Me.LoadExistingPalletGrid()

                                    Me.lbAddProduct.Visible = True
                                    Me.lbChgProductQty.Visible = True
                                    Me.lbRmvProduct.Visible = True
                                    Me.txOption.Visible = True
                                    Me.lbOption.Visible = True

                                    Me.lbPrompt.Text = "Enter Option from list."
                                    Common.JavaScriptSetFocus(Page, Me.txOption)
                                End If
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

    Protected Sub txOption_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txOption.TextChanged
        Select Case Me.txOption.Text
            Case 1  'Call AddToPallet screen
                strURL = "~/IC_SamplePalletEditNew.aspx"
                Common.SaveVariable("newURL", strURL, Page)
                Common.SaveVariable("ScreenParam", "AddTo", Page)
            Case 2
                strURL = "~/IC_SamplePalletEditNew.aspx"
                Common.SaveVariable("newURL", strURL, Page)
                Common.SaveVariable("ScreenParam", "ChgQty", Page)
            Case 3
                strURL = "~/IC_SamplePalletEditNew.aspx"
                Common.SaveVariable("newURL", strURL, Page)
                Common.SaveVariable("ScreenParam", "Remove", Page)
            Case Else
                Me.lbError.Text = "Enter a valid option from the list!"
                Me.lbError.Visible = True
        End Select
    End Sub

#End Region

#Region "Screen Buttons"

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page)
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

#End Region

#Region "Custom Processes"

    Public Sub LoadExistingPalletGrid()
        Dim sqlString As String
        Dim dvExistingPalletGrd As New DataView
        Dim dsExistingPalletGrd As New Data.DataSet
        Dim sqlCmdExistingPalletGrd As New System.Data.SqlClient.SqlCommand
        Dim strPallet As String = ""

        Try
            strPallet = Me.txPallet.Text
            sqlString = "Select ProductID,CaseQty,ProdDesc from vwIC_ExistingPalletGridView Where PalletID = " & CLng(strPallet)
            'Get Shipto Info for ShippingPallet Tags
            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdExistingPalletGrd = DB.NewSQLCommand3(sqlString)
                If Not sqlCmdExistingPalletGrd Is Nothing Then
                    dsExistingPalletGrd = DB.GetDataSet(sqlCmdExistingPalletGrd)
                    sqlCmdExistingPalletGrd.Dispose() : sqlCmdExistingPalletGrd = Nothing
                    DB.KillSQLConnection()
                    If dsExistingPalletGrd Is Nothing Then
                        Me.lbError.Text = "Data Error occurred while displaying existing products on current pallet! Check battery and wireless connection - then try again."
                        Me.lbError.Visible = True
                        Common.JavaScriptSetFocus(Page, Me.txPallet)
                    Else
                        dvExistingPalletGrd = New DataView(dsExistingPalletGrd.Tables(0), Nothing, Nothing, DataViewRowState.CurrentRows)
                        dgExistingProducts.DataSource = dvExistingPalletGrd
                        dgExistingProducts.DataBind()

                        If dvExistingPalletGrd.Count > 0 Then
                            Me.lbExistingGridTitle.Visible = True
                            dgExistingProducts.Visible = True
                        Else
                            Me.lbExistingGridTitle.Visible = False
                            dgExistingProducts.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Exception Error " & ex.Message.ToString & " occurred while displaying existing products on current pallet! Check battery and wireless connection - then try again."
            Me.lbError.Visible = True
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

#End Region

End Class