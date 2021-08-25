Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_WH_Receipt
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _company As String
    Public _whs As String
    Public _function As Integer
    Public nQtyOWFG As Integer

    'Private sConnString As String = "Server=192.168.5.4;Initial Catalog=OWS;Trusted_Connection=Yes;connect timeout=10;Persist Security Info=True"
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
        Me.lbUser.Text = "User : " & Common.GetVariable("User", Page)
        lbError.Visible = False
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString
        _whs = Common.GetVariable("Whse", Page).ToString

        If Common.GetVariable("ScreenParam", Page).ToString = "FGRECV" Then
            Me.lbPageTitle.Text = "OW Inventory - FG Recv From Plant 2"
        Else
            Me.lbPageTitle.Text = "OW Inventory - WHSE Transfer"
        End If

        If Not Page.IsPostBack Then
            'Setting up screen for start of 1st Pallet
            'Common.SaveVariable("Whse", "", Page)
            Common.SaveVariable("newURL", Nothing, Page)
            Call InitProcess()
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, txData)
        End If
    End Sub

    Public Sub InitSessVar()
        'Clear Session variables for this page
        Common.SaveVariable("Pallet", "", Page)
        Common.SaveVariable("Qty", "", Page)
        Common.SaveVariable("QtyOWFG", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("Version", "", Page)
        Common.SaveVariable("WhseFrom", "", Page)
        Me.lbPallet.Visible = False
        Me.lbPalletVal.Visible = False
        Me.lbQty.Visible = False
        Me.lbQtyVal.Visible = False
        Me.lbFunction.Text = 2
        Me.txData.Text = ""
        Me.lbPrompt.Text = "Scan or Enter Pallet #"
    End Sub

    Public Sub InitProcess()
        Call InitSessVar()
        Common.SaveVariable("WhseTo", "", Page)
        Me.lbWhse.Visible = False
        Me.lbWhseVal.Visible = False
        Me.lbPallet.Visible = False
        Me.lbPalletVal.Visible = False
        Me.lbQty.Visible = False
        Me.lbQtyVal.Visible = False
        Me.lbFunction.Visible = False
        Me.lbFunction.Text = 1
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan or Enter Warehouse"
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Protected Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(txData.Text)) < 1 Then
                Exit Sub
            End If

            Me.lbError.Text = ""
            Me.lbError.Visible = False

            Select Case _function
                Case Is = 1
                    'Validate Warehouse Scanned or Entered to the Screen
                    If WarehouseValid(Trim(txData.Text)) = False Then
                        Me.lbError.Text = "Warehouse entered is not valid"
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-Enter Warehouse or see supervisor"
                    Else
                        Me.lbWhse.Visible = True
                        Me.lbWhseVal.Text = UCase(Trim(txData.Text))
                        Me.lbWhseVal.Visible = True
                        Me.lbFunction.Text = 2
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Scan or Enter Pallet #"

                    End If
                    Common.JavaScriptSetFocus(Page, Me.txData)

                Case Is = 2 'Validate Pallet Scanned to the Screen
                    Me.lbError.Visible = False

                    If PalletExist(Trim(txData.Text)) = False Then
                        Me.lbError.Text = "Pallet entered is not valid"
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-Enter Pallet # or see supervisor"
                    Else
                        '' '' '' '' ''Dim sWHFrm As String = Trim(Common.GetVariable("WhseFrom", Page).ToString)
                        '' '' '' '' ''Dim sWHTo As String = Trim(Common.GetVariable("WhseTo", Page).ToString)
                        '' '' '' '' ''Dim sToBin As String = UCase(Trim(Me.lbWhseVal.Text))
                        '' '' '' '' ''Dim sFromBin As String = UCase(Trim(Common.GetVariable("FromBin", Page).ToString))

                        ' '' '' '' '' ''*Added 
                        '' '' '' '' ''If sWHFrm = sWHTo And sFromBin = sToBin Then
                        '' '' '' '' ''    Me.lbError.Text = "Pallet entered was already received into the Warehouse"
                        '' '' '' '' ''    Me.lbError.Visible = True
                        '' '' '' '' ''    Call InitSessVar()
                        '' '' '' '' ''Else
                        'Pallet is valid continue process
                        Me.lbPallet.Visible = True
                        Me.lbPalletVal.Text = Trim(txData.Text)
                        Me.lbPalletVal.Visible = True
                        Me.lbError.Visible = False
                        Me.lbFunction.Text = 3
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Case Qty on Pallet"
                        'End If
                    End If

                Case Is = 3 'Validate Case Qty on Pallet that was entered
                    If IsNumeric(Trim(Me.txData.Text)) = False Then
                        Me.lbError.Text = "Quantity entered is not numeric. Please re-enter the Case Qty."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = 3
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Case Qty on Pallet"
                        Exit Sub
                    End If

                    If CLng(Trim(Me.txData.Text)) > 99999 Then
                        Me.lbError.Text = "Quantity entered is greater than maximum allowed. Please re-enter the Case Qty."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = 3
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Case Qty on Pallet"
                        Exit Sub
                    End If

                    'Compare Qty to what was entered in packaging
                    nQtyOWFG = CInt(Common.GetVariable("QtyOWFG", Page).ToString)
                    Me.lbQtyVal.Text = Trim(Me.txData.Text)

                    If CInt(Me.txData.Text) <> nQtyOWFG Then
                        'Prompt user to enter the quantity again for validation
                        Me.lbError.Text = "Quantity entered does not match original pallet quantity. Please re-enter the Case Qty for validation."
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbFunction.Text = 4
                        Me.lbPrompt.Text = "Enter Case Qty again."
                    Else
                        Dim whseto As String = Common.GetVariable("WhseTo", Page)
                        Dim whsefrom As String = Common.GetVariable("WhseFrom", Page)
                        Dim gtin As String = Common.GetVariable("GTIN", Page)
                        Dim ver As String = Common.GetVariable("Version", Page)
                        Dim codedt As String = Common.GetVariable("CodeDate", Page)

                        If ProcessPallet(Me.lbPalletVal.Text, Me.lbQtyVal.Text, Me.lbWhseVal.Text, whseto, whsefrom, gtin, ver, codedt) = True Then
                            Call InitSessVar()
                            Me.lbError.Text = "Pallet " & Me.lbPalletVal.Text & " process completed."
                            Me.lbError.Visible = True
                        Else
                            Me.lbError.Text = "Pallet " & Me.lbPalletVal.Text & "did not process completely. Please process the pallet again."
                            Me.lbError.Visible = True
                        End If
                    End If

                Case Is = 4
                    If IsNumeric(Trim(Me.txData.Text)) = False Then
                        Me.lbError.Text = "Quantity entered is not numeric. Please re-enter the Case Qty."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = 4
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Case Qty again."
                        Exit Sub
                    End If

                    If CLng(Trim(Me.txData.Text)) > 99999 Then
                        Me.lbError.Text = "Quantity entered is greater than maximum allowed. Please re-enter the Case Qty."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = 3
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Enter Case Qty again."
                        Exit Sub
                    End If

                    Me.lbQtyVal.Text = Trim(Me.txData.Text)

                    'Qty entered for 2nd time - update pallet qty
                    If AdjustPallet(Me.lbPalletVal.Text, Me.lbQtyVal.Text) = True Then

                        Dim whseto As String = Common.GetVariable("WhseTo", Page)
                        Dim whsefrom As String = Common.GetVariable("WhseFrom", Page)
                        Dim gtin As String = Common.GetVariable("GTIN", Page)
                        Dim ver As String = Common.GetVariable("Version", Page)
                        Dim codedt As String = Common.GetVariable("CodeDate", Page)

                        If ProcessPallet(Me.lbPalletVal.Text, Me.lbQtyVal.Text, Me.lbWhseVal.Text, whseto, whsefrom, gtin, ver, codedt) = True Then

                            Call InitSessVar()
                            Me.lbError.Text = "Pallet " & Me.lbPalletVal.Text & " with updated quantity = " & Me.lbQtyVal.Text & " - process completed."
                            Me.lbError.Visible = True
                        Else
                            Me.lbError.Text = "Pallet " & Me.lbPalletVal.Text & "did not process completely. Please process the pallet again."
                            Me.lbError.Visible = True
                        End If
                    Else
                        Me.lbError.Text = "Pallet " & Me.lbPalletVal.Text & " quantity adjustment failed. Please process the pallet again."
                        Me.lbError.Visible = True
                    End If
            End Select
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while connecting to database to validate Pallet entered! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txData)
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Sub

    Public Function WarehouseValid(ByVal _warehouse As String) As Boolean
        WarehouseValid = False

        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            'Dim dv As New DataView

            sqlString = "Select * from IC_Bins Where BIN_LOCATION = '" & Trim(_warehouse) & "'"
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            If ds.Tables("Data").Rows.Count > 0 Then
                If Trim(ds.Tables("Data").Rows(0).Item("BIN_TYPE").ToString) = "WH" Then
                    Common.SaveVariable("WhseTo", Trim(ds.Tables("Data").Rows(0).Item("WAREHOUSE").ToString), Page)
                    'Warehouse is valid
                    WarehouseValid = True
                End If
            End If

        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        Finally
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function

    Public Function PalletExist(ByVal _pallet As Long) As Boolean
        PalletExist = False

        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            'Changed from vwIC_InventoryPallets to vwIC_PalletsForTransfer to handle Both I and S type Pallets
            sqlString = "Select * from vwIC_PalletsForTransfer Where pk_nPallet = " & _pallet
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Pallet")
            dv = New DataView(ds.Tables("Pallet"), Nothing, Nothing, DataViewRowState.CurrentRows)

            If dv.Count <> 0 Then
                Common.SaveVariable("QtyOWFG", ds.Tables("Pallet").Rows(0).Item("PalletQty"), Page)
                Common.SaveVariable("GTIN", ds.Tables("Pallet").Rows(0).Item("GTIN"), Page)
                Common.SaveVariable("CodeDate", ds.Tables("Pallet").Rows(0).Item("CodeDate"), Page)
                Common.SaveVariable("Version", ds.Tables("Pallet").Rows(0).Item("ProdVariant"), Page)
                Common.SaveVariable("WhseFrom", ds.Tables("Pallet").Rows(0).Item("Warehouse"), Page)
                Common.SaveVariable("FromBin", ds.Tables("Pallet").Rows(0).Item("Bin"), Page)

                PalletExist = True
            Else
                PalletExist = False
            End If

        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try

    End Function

    Public Function AdjustPallet(ByVal _pallet As Long, ByVal _qty As Integer) As Boolean
        AdjustPallet = False

        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String

        Try
            sqlConn.Open()
            sSqlString = "spPalletAdjust " '& _pallet & ", " & _qty & ",'" & Common.GetVariable("UserID", Page).ToString & "'"
            cmdSqlCommand.CommandType = CommandType.StoredProcedure
            cmdSqlCommand.CommandText = sSqlString
            cmdSqlCommand.Connection = sqlConn
            cmdSqlCommand.Parameters.AddWithValue("@nPallet", _pallet)
            cmdSqlCommand.Parameters.AddWithValue("@nToQty", _qty)
            cmdSqlCommand.Parameters.AddWithValue("@sUserID", RTrim(Common.GetVariable("UserID", Page).ToString))
            cmdSqlCommand.ExecuteNonQuery()
            sqlConn.Close()
            AdjustPallet = True
        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try
    End Function

    Public Function ProcessPallet(ByVal _pallet As Long, ByVal _qty As Integer, ByVal _warehouseto As String, _
                                    ByVal _whseto As String, ByVal _whsefrom As String, ByVal _gtin As String, _
                                    ByVal _version As String, ByVal _codedate As String) As Boolean

        ProcessPallet = False

        Dim cmdProcess As New SqlClient.SqlCommand
        Dim sSqlProcess As String

        Try
            sqlConn.Open()
            sSqlProcess = "spWH_Receipt2 " '& _pallet & ", " & _qty & ",'" & Common.GetVariable("UserID", Page).ToString & "'"
            cmdProcess.CommandType = CommandType.StoredProcedure
            cmdProcess.CommandText = sSqlProcess
            cmdProcess.Connection = sqlConn
            cmdProcess.Parameters.AddWithValue("@nPallet", _pallet)
            cmdProcess.Parameters.AddWithValue("@nToQty", _qty)
            cmdProcess.Parameters.AddWithValue("@sWarehouseTo", _warehouseto)
            cmdProcess.Parameters.AddWithValue("@sWhseFrom", _whsefrom)
            cmdProcess.Parameters.AddWithValue("@sWhseTo", _whseto)
            cmdProcess.Parameters.AddWithValue("@sGTIN", _gtin)
            cmdProcess.Parameters.AddWithValue("@sVersion", _version)
            cmdProcess.Parameters.AddWithValue("@sCodeDate", _codedate)
            cmdProcess.Parameters.AddWithValue("@sUserID", RTrim(Common.GetVariable("UserID", Page).ToString))
            cmdProcess.ExecuteNonQuery()
            sqlConn.Close()
            sqlConn.Dispose()
            ProcessPallet = True
        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page) 'new
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset session values to default and start Pallet over
        Call InitSessVar()
    End Sub

    Protected Sub btNewWhse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNewWhse.Click
        Call InitProcess()
    End Sub
End Class