Partial Public Class IC_AbbylandPallet
    Inherits System.Web.UI.Page

    Public _caselabel As String
    Public _tobin As String
    Public _user As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _product As String
    Public _productversion As String
    Public _status As String
    Public _prodid As Integer
    Public strURL As String = Nothing
    Public _function As Integer
    Public _qtymax As Integer

    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString.ToString
    'Private sConnString As String = "server=SQL1;max pool size=300;user id=sa;password=buddig;database=Warehouse"
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
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page).ToString
        lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString
        _qtymax = ConfigurationManager.AppSettings.Get("UI.Abbyland.MaxQty").ToString
        Me.lbPageTitle.Text = "OWS Inv Mgmt Rcv Co-Manufactured Pallet Tag"

        If Not Page.IsPostBack Then
            'Setting up screen for start of 1st Pallet
            Common.SaveVariable("newURL", Nothing, Page)
            Call InitProcess()
            Common.JavaScriptSetFocus(Page, Me.txText)
        Else 'Page is posted back 
            _function = CInt(Me.lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txText)
        End If
    End Sub

    Public Sub InitProcess()
        Try
            Common.SaveVariable("Pallet", "", Page)
            Common.SaveVariable("CaseLabel", "", Page)
            Common.SaveVariable("Quantity", "", Page)
            Common.SaveVariable("ToBin", "", Page)

            Me.txText.Text = ""
            Me.txText.Visible = True
            Me.lbPallet.Visible = False
            Me.lbPalletVal.Visible = False
            Me.lbPalletVal.Text = ""
            Me.lbCaseLabel.Visible = False
            Me.lbCaseLabelVal.Visible = False
            Me.lbCaseLabelVal.Text = ""
            Me.lbQuantity.Visible = False
            Me.lbQuantityVal.Visible = False
            Me.lbQuantityVal.Text = ""
            Me.lbToBin.Visible = False
            Me.lbToBinVal.Visible = False
            Me.lbToBinVal.Text = ""
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Me.lbFunction.Text = "1"
            Me.btFinished.Visible = False
            Me.btNextPallet.Visible = False

            Me.lbPrompt.Text = "Scan or Enter Co-Manufactured Pallet#"
            Common.JavaScriptSetFocus(Page, Me.txText)
        Catch ex As Exception
            lbError.Text = "Try again - Init Screen Error = " & ex.Message.ToString & " - Press Restart Entry button"
            lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txText)
        End Try
    End Sub
    Protected Sub txText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txText.TextChanged
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Try
            If RTrim(Me.txText.Text).Length < 1 Then Throw New Exception("""Entry cannot be blank""")

            Select Case _function
                Case Is = 1

                    If IsNumeric(Me.txText.Text) = False Then Throw New Exception("""Pallet entry must be numeric. Please enter Pallet # again.""")
                    'If CInt(Mid(Me.txText.Text, 1, 1)) <> 5 And CInt(Mid(Me.txText.Text, 1, 1)) <> 6 And CInt(Mid(Me.txText.Text, 1, 1)) <> 9 Then Throw New Exception("""Pallet scanned is not Co-Manufactured!""")

                    'Validate that Pallet entered is not already in the system
                    If ExistingPallet(Me.txText.Text) = False Then
                        'Pallet is not in the system - continue with entry
                        Me.lbPalletVal.Text = Me.txText.Text
                        Me.lbPalletVal.Visible = True
                        Me.lbPallet.Visible = True
                        Common.SaveVariable("Pallet", Me.txText.Text, Page)
                        Me.txText.Text = ""
                        Me.lbFunction.Text = "2"
                        Me.lbPrompt.Text = "Scan a Case Label from the Pallet"
                        Common.JavaScriptSetFocus(Page, Me.txText)
                    Else
                        Me.txText.Text = ""
                        Me.lbFunction.Text = "1"
                        Common.JavaScriptSetFocus(Page, Me.txText)

                    End If
                Case Is = 2
                    Dim iCL As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.CaseLength").ToString)
                    Dim iCL2 As Integer = CInt(ConfigurationManager.AppSettings.Get("UI.AltCaseLength").ToString)
                    If RTrim(Me.txText.Text).Length <> iCL And RTrim(Me.txText.Text).Length <> iCL2 Then Throw New Exception("""Case Label entered is incorrect length. Please scan the Case Label again.""")

                    'Validate that the Case Label has valid GTIN,CodeDate,LotCode,Version
                    If ValidCaseLabel() = True Then
                        Me.lbCaseLabelVal.Text = RTrim(Me.txText.Text)
                        Me.lbCaseLabel.Visible = True
                        Me.lbCaseLabelVal.Visible = True
                        Common.SaveVariable("CaseLabel", Me.txText.Text, Page)
                        Me.txText.Text = ""
                        Me.lbFunction.Text = "3"
                        Me.lbPrompt.Text = "Enter the Case Quantity on the Pallet"
                        Common.JavaScriptSetFocus(Page, Me.txText)

                    End If

                Case Is = 3
                    'Validate Quantity entered is numeric and not greater than Pallet Max for that product.
                    If CInt(Me.txText.Text) > _qtymax Then Throw New Exception("""Quantity entered for Pallet is greater than what is allowed for Co-Manufactured Pallets. Please re-enter the Case quantity or see supervisor.""")

                    Me.lbQuantity.Visible = True
                    Me.lbQuantityVal.Text = Me.txText.Text
                    Me.lbQuantityVal.Visible = True
                    Common.SaveVariable("Quantity", Me.txText.Text, Page)
                    Me.lbFunction.Text = "4"
                    Me.txText.Text = ""
                    Me.lbPrompt.Text = "Scan the Whse Location ie-PACKLAND,GLACA"
                    Common.JavaScriptSetFocus(Page, Me.txText)

                Case Is = 4
                    'Validate Location entry has a BinType of WH
                    If ValidateWHSE() = True Then
                        Common.SaveVariable("ToBin", UCase(Me.txText.Text), Page)
                        Me.btFinished.Visible = True
                        Me.btFinished.Text = "Finish Pallet"
                        Me.txText.Visible = False
                        Me.lbToBin.Visible = True
                        Me.lbToBinVal.Text = UCase(Me.txText.Text)
                        Me.lbToBinVal.Visible = True
                        Me.lbPrompt.Text = "Verify values on screen, then press Finish Pallet button"
                    End If

            End Select

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txText)
        End Try
        
    End Sub

    Public Function ExistingPallet(ByVal _pallet As Long) As Boolean
        ExistingPallet = True
        Dim dsPallet As New Data.DataSet
        Try

            Dim iPallet As Int64 = _pallet
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ExistingPallet function""")
            Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand
            sqlCmdPallet = DB.NewSQLCommand("SQL.Query.GetPalletInfo")
            If sqlCmdPallet Is Nothing Then Throw New Exception("""Error creating sqlCmdPallet Command during ExistingPallet function""")

            sqlCmdPallet.Parameters.AddWithValue("@Pallet", iPallet)
            dsPallet = DB.GetDataSet(sqlCmdPallet)
            sqlCmdPallet.Dispose() : sqlCmdPallet = Nothing
            DB.KillSQLConnection()

            If dsPallet Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ExistingPallet function""")

            If dsPallet.Tables(0).Rows.Count > 0 Then Throw New Exception("""Pallet scanned is already in the system. Continue to the next Pallet.""")

            ExistingPallet = False

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "! Press Restart Entry button to try again or see supervisor"
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txText)
        Finally
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function ValidCaseLabel() As Boolean
        ValidCaseLabel = False
        Dim dsGtinVer As New Data.DataSet
        Try
            'Validate GTIN and Version
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ValidCaseLabel function""")
            Dim sqlCmdGTINVer As New System.Data.SqlClient.SqlCommand
            sqlCmdGTINVer = DB.NewSQLCommand("SQL.Query.GTINVerLookup")
            If sqlCmdGTINVer Is Nothing Then Throw New Exception("""Error creating sqlCmdPallet Command during ValidCaseLabel function""")

            sqlCmdGTINVer.Parameters.AddWithValue("@GTIN", Mid(Me.txText.Text, 3, 14))
            sqlCmdGTINVer.Parameters.AddWithValue("@Version", Mid(Me.txText.Text, 42, 3))

            dsGtinVer = DB.GetDataSet(sqlCmdGTINVer)
            sqlCmdGTINVer.Dispose() : sqlCmdGTINVer = Nothing
            DB.KillSQLConnection()

            If dsGtinVer Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ValidCaseLabel function""")

            If dsGtinVer.Tables(0).Rows.Count < 1 Then Throw New Exception("""GTIN and Version not valid in Case Label.""")

            If IsDate(Mid$(Me.txText.Text, 21, 2) & "/" & Mid$(Me.txText.Text, 23, 2) & "/20" & Mid$(Me.txText.Text, 19, 2)) = False Then Throw New Exception("""Code Date in barcode is not a valid date. Try again or see supervisor.""")

            Dim LotFull As String = ""
            LotFull = CInt(Common.FindLotFromBarcode(Mid$(Me.txText.Text, 27, 12), _whs, Mid$(Me.txText.Text, 3, 14), CInt(Mid$(Me.txText.Text, 42, 3)), Mid$(Mid$(Me.txText.Text, 19, 6), 3, 2) & "/" & Mid$(Mid$(Me.txText.Text, 19, 6), 5, 2) & "/20" & Mid$(Mid$(Me.txText.Text, 19, 6), 1, 2)))

            ValidCaseLabel = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "! Try Case Label entry again or see supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txText)
        Finally
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function ValidateWHSE() As Boolean
        ValidateWHSE = False
        Dim dsWHSE As New Data.DataSet
        Dim strQuery As String = "Select * from IC_Bins Where BIN_LOCATION = '" & Me.txText.Text & "' And BIN_TYPE = 'WH'"
        Try

            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Error connecting to Database during ValidateWHSE function""")
            Dim sqlCmdWHSE As New System.Data.SqlClient.SqlCommand
            sqlCmdWHSE = DB.NewSQLCommand3(strQuery)
            If sqlCmdWHSE Is Nothing Then Throw New Exception("""Error creating sqlCmdPallet Command during ValidateWHSE function""")

            dsWHSE = DB.GetDataSet(sqlCmdWHSE)
            sqlCmdWHSE.Dispose() : sqlCmdWHSE = Nothing
            DB.KillSQLConnection()

            If dsWHSE Is Nothing Then Throw New Exception("""Error getting Dataset from Database during ValidateWHSE function""")

            If dsWHSE.Tables(0).Rows.Count < 1 Then Throw New Exception("""Entry made for Whse Location is not valid. Try again or see supervisor.""")

            ValidateWHSE = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString & "! Try again or see supervisor"
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txText)
        Finally
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

    Protected Sub btFinished_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinished.Click
        Try

            If WriteAbbylandPallet() = False Then Throw New Exception("""Co-Manufactured Pallet was not saved completely! Press the Restart button and process the pallet again.""")
            Me.lbPrompt.Text = "Co-Manufactured Pallet # " & Me.lbPalletVal.Text & " was successfully added to the system. Press Next Pallet button."
            Me.btFinished.Visible = False
            Me.btNextPallet.Visible = True

        Catch ex As Exception
            Me.lbError.Text = "Exception Error: " & ex.Message.ToString
            Me.lbError.Visible = True
        End Try
    End Sub

    Protected Sub btNextPallet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNextPallet.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub

    Public Function WriteAbbylandPallet() As Boolean
        WriteAbbylandPallet = False
        Dim sqlCmdWriteAbbylandPallet As New System.Data.SqlClient.SqlCommand
        Dim iReturnValue As Int64 = 0
        'LookupLot 
        'Write IC Records using Stored Procedure with TransAction
        Try
            sqlCmdWriteAbbylandPallet.Connection = sqlConn
            sqlConn.Open()
            sqlCmdWriteAbbylandPallet.CommandType = CommandType.StoredProcedure
            sqlCmdWriteAbbylandPallet.CommandText = "spRecvAbbylandPallet "
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@Pallet", CLng(Me.lbPalletVal.Text))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@GTIN", Mid$(Me.lbCaseLabelVal.Text, 3, 14))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@CodeDate", Mid$(Me.lbCaseLabelVal.Text, 19, 6))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@Prod_Order_OW", Mid$(Me.lbCaseLabelVal.Text, 27, 12))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@Lot", Mid$(Me.lbCaseLabelVal.Text, 34, 5))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@Version", Mid$(Me.lbCaseLabelVal.Text, 42, 3))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@PalletQty", CInt(Me.lbQuantityVal.Text))
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@ToBin", Me.lbToBinVal.Text)
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@UserID", Common.GetVariable("UserID", Page).ToString)
            sqlCmdWriteAbbylandPallet.Parameters.AddWithValue("@iErrorCode", 0)
            sqlCmdWriteAbbylandPallet.Parameters("@iErrorCode").Direction = ParameterDirection.Output
            sqlCmdWriteAbbylandPallet.ExecuteNonQuery()
            iReturnValue = sqlCmdWriteAbbylandPallet.Parameters("@iErrorCode").Value
            sqlCmdWriteAbbylandPallet.Dispose() : sqlCmdWriteAbbylandPallet = Nothing
            sqlConn.Close()
            sqlConn.Dispose()

            'No errors occured during stored procedure
            If iReturnValue = 0 Then
                WriteAbbylandPallet = True
            End If
        Catch ex As Exception
            If Not sqlCmdWriteAbbylandPallet Is Nothing Then
                sqlCmdWriteAbbylandPallet.Dispose()
                sqlCmdWriteAbbylandPallet = Nothing
            End If
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function


End Class