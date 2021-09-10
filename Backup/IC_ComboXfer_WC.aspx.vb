Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.IO
Imports System.IO.File
Partial Public Class IC_ComboXfer_WC
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _company As String
    Public _whs As String
    Public _userid As String
    Public _function As Integer
    Public nQtyOWFG As Integer
    Public lwDest As String = "\\SRV04\wddrop\"
    Public lwTemp As String = "\\SRV04\wddroptemp\"

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
        _userid = RTrim(Common.GetVariable("UserID", Page).ToString)

        If Not Page.IsPostBack Then
            'Setting up screen for start of new Combo Xfer
            Common.SaveVariable("newURL", Nothing, Page)
            Call InitProcess()
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, txData)
        End If
    End Sub

    Public Sub InitProcess()
        Me.txData.Text = ""
        Me.txData.Visible = True
        Me.lbXferTruckNum.Visible = False
        Me.lbXferTruckVal.Visible = False
        Me.lbXferTruckVal.Text = ""
        Me.lbXferTruckID.Visible = False
        Me.lbXferTruckID.Text = ""

        Me.lbFunction.Visible = False
        Me.lbFunction.Text = 1
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me.lbPrompt.Text = "Scan Xfer Truck barcode"
        RefreshCombosInTruckGrid()

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
                    'Validate Xfer Truck Scanned or Entered to the Screen
                    If ValidateXferTruck(Trim(txData.Text)) = False Then
                        Me.lbError.Text = "Xfer Truck scanned is not valid"
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-scan Xfer Truck or see supervisor"
                    Else
                        Me.lbXferTruckNum.Visible = True
                        Me.lbXferTruckVal.Text = Trim(txData.Text)
                        Me.lbXferTruckVal.Visible = True
                        Me.lbFunction.Text = 2
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Scan Combo Tag loading on truck"
                        RefreshCombosInTruckGrid()
                    End If
                    Common.JavaScriptSetFocus(Page, Me.txData)

                Case Is = 2
                    Me.lbError.Visible = False
                    'Validate Combo Scanned to the Screen to make sure it is in the correct status
                    If ValidateCombo(Trim(txData.Text)) = False Then
                        'Me.lbError.text is set in the Combo Validation function
                        Me.lbError.Visible = True
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Re-Enter Pallet # or see supervisor"
                    Else
                        'Combo is valid - update Combo Status = 105 and ProcessAreaID = XferTruckID in CD_Combos 
                        If ProcessCombo(Trim(txData.Text), Trim(Me.lbXferTruckID.Text)) = False Then
                            Me.lbError.Text = "Error occurred while processing the Combo to the Xfer Truck.  Please scan the combo again."
                            Me.lbError.Visible = True
                            Me.lbFunction.Text = 2
                            Me.txData.Text = ""
                            Common.JavaScriptSetFocus(Page, Me.txData)
                        Else
                            'Combo successfully updated
                            RefreshCombosInTruckGrid()
                            Me.lbFunction.Text = 2
                            Me.txData.Text = ""
                            Me.lbPrompt.Text = "Scan Combo Tag loading on truck"
                        End If
                    End If
            End Select
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while connecting to database to Xfer Combos to Weeden Creek! Check battery and Wireless connection - then try again or see your supervisor."
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

    Public Function ValidateXferTruck(ByVal _xfertruck As String) As Boolean
        ValidateXferTruck = False
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "Select * from CD_ProcessAreas Where ProcessAreaDesc = '" & Trim(_xfertruck) & "'"
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            If ds.Tables("Data").Rows.Count > 0 Then
                Me.lbXferTruckID.Text = ds.Tables("Data").Rows(0).Item("ProcessAreaID").ToString
                ValidateXferTruck = True
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

    Public Function ValidateCombo(ByVal _combo As Long) As Boolean
        ValidateCombo = False
        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet

            sqlString = "Select * from CD_Combos Where ComboID = " & _combo
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            If ds.Tables("Data").Rows.Count > 0 Then
                If ds.Tables("Data").Rows(0).Item("Status").ToString = "100" Then
                    ValidateCombo = True
                Else
                    Me.lbError.Text = "Combo Tag scanned is in the wrong status. Verify with supervisor."
                End If
            Else
                Me.lbError.Text = "Combo Tag scanned was not found in system. Please scan the Combo Tag again or show to supervisor."
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

    Public Function ProcessCombo(ByVal _combo As Long, ByVal _xfertruckid As Integer) As Boolean
        ProcessCombo = False

        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String

        Try
            sqlConn.Open()
            sSqlString = "spCD_ComboInTruck "
            cmdSqlCommand.CommandType = CommandType.StoredProcedure
            cmdSqlCommand.CommandText = sSqlString
            cmdSqlCommand.Connection = sqlConn
            cmdSqlCommand.Parameters.AddWithValue("@pnComboID", _combo)
            cmdSqlCommand.Parameters.AddWithValue("@pnProcessAreaID", _xfertruckid)
            cmdSqlCommand.Parameters.AddWithValue("@pnAssociateID", _userid)
            cmdSqlCommand.ExecuteNonQuery()
            sqlConn.Close()
            ProcessCombo = True
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

    Public Sub RefreshCombosInTruckGrid()
        'Load Me.gvCombos
        Try
            Dim sqlString As String
            Dim sqlDABuildRacks As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            If Trim(Me.lbXferTruckID.Text) = "" Then
                Me.lbXferTruckID.Text = 0
            End If

            sqlString = "Select ComboID, Formula from vwCD_Combos Where Status = 105 AND ProcessAreaID = " & CInt(Me.lbXferTruckID.Text)
            sqlDABuildRacks = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDABuildRacks)
            sqlDABuildRacks.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            Me.gvCombos.DataSource = dv

            Me.gvCombos.DataBind()

            If dv.Count > 0 Then
                Me.btFinishedLoading.Visible = True
                Me.gvCombos.Visible = True
            Else
                Me.btFinishedLoading.Visible = False
                Me.gvCombos.Visible = False
            End If
        Catch ex As Exception

            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Combo Menu
        Common.SaveVariable("newURL", "~/IC_ComboMenu.aspx", Page)
    End Sub

    Protected Sub btFinishedLoading_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinishedLoading.Click
        Me.btPrintManifest.Visible = True
        Me.btFinishedLoading.Visible = False
        Me.lbPrompt.Text = "Verify the list of Combos on the Transfer truck then press the Print Maifest button."

    End Sub

    Protected Sub btPrintManifest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btPrintManifest.Click
        Dim _printer As Integer = 1
        Dim destPath As String = ""
        Dim tempPath As String = ""
        Dim filenm As String = ""
        Dim PathFile As String = ""
        Dim sourceFile As String = ""
        Dim successfile As String = ""
        Dim destinationFile As String = ""
        Dim x As Integer = 0
        Dim sqlString As String
        Dim sqlDAProdData As New SqlClient.SqlDataAdapter
        Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
        Dim dtResults As New DataTable
        Dim dsManifest As New DataSet
        Dim Row As DataRow
        Dim TtlComboQty As Integer = 0
        Dim TtlGrossWt As Integer = 0
        Dim _ComboXferID As Integer = 0
        Dim strLine3 As String = ""
        Dim strLine4 As String = ""
        Dim strLine5 As String = ""
        Dim strLine6 As String = ""
        Dim strLine7 As String = ""
        Dim strLine8 As String = ""
        Dim strLine9 As String = ""
        Dim strLine10 As String = ""
        Dim strLine11 As String = ""
        Dim strLine12 As String = ""
        Dim strLine13 As String = ""
        Dim strLine14 As String = ""
        Dim strLine15 As String = ""
        Dim strLine16 As String = ""
        Dim strLine17 As String = ""
        Dim strLine18 As String = ""
        Dim strLine19 As String = ""
        Dim strLine20 As String = ""
        Dim strLine21 As String = ""
        Dim strLine22 As String = ""
        Dim strLine23 As String = ""
        Dim strLine24 As String = ""
        Dim strLine25 As String = ""
        Dim strLine26 As String = ""
        Dim strLine27 As String = ""
        Dim strLine28 As String = ""
        Dim strLine29 As String = ""
        Dim strLine30 As String = ""
        Dim strLine31 As String = ""
        Dim strLine32 As String = ""
        Dim strLine33 As String = ""
        Dim strLine34 As String = ""
        Dim strLine35 As String = ""
        Dim strLine36 As String = ""
        Dim strLine37 As String = ""
        Dim strLine38 As String = ""
        Dim strLine39 As String = ""
        Dim strLine40 As String = ""
        Dim strLine41 As String = ""
        Dim strLine42 As String = ""
        Dim strLine43 As String = ""
        Dim strLine44 As String = ""
        Dim strLine45 As String = ""
        Dim strLine46 As String = ""
        Dim strLine47 As String = ""
        Dim strLine48 As String = ""
        Dim strLine49 As String = ""
        Dim strLine50 As String = ""
        Dim strLine51 As String = ""
        Dim strLine52 As String = ""
        Dim strLine53 As String = ""
        Dim strLine54 As String = ""
        Dim strLine55 As String = ""
        Dim strLine56 As String = ""
        Dim strLine57 As String = ""
        Dim strLine58 As String = ""
        Dim strLine59 As String = ""
        Dim strLine60 As String = ""
        Dim strLine61 As String = ""
        Dim strLine62 As String = ""
        Dim strLine63 As String = ""
        Dim strLine64 As String = ""
        Dim strLine65 As String = ""
        Dim strLine66 As String = ""
        Dim strLine67 As String = ""
        Dim strLine68 As String = ""
        Dim strLine69 As String = ""
        Dim strLine70 As String = ""
        Dim strLine71 As String = ""
        Dim strLine72 As String = ""
        Dim strLine73 As String = ""
        Dim strLine74 As String = ""
        Dim strLine75 As String = ""
        Dim strLine76 As String = ""
        Dim strLine77 As String = ""
        Dim strLine78 As String = ""
        Dim strLine79 As String = ""
        Dim strLine80 As String = ""
        Dim strLine81 As String = ""
        Dim strLine82 As String = ""
        Dim strLine83 As String = ""
        Dim strLine84 As String = ""
        Dim strLine85 As String = ""
        Dim strLine86 As String = ""
        Dim strLine87 As String = ""
        Dim strLine88 As String = ""
        Dim strLine89 As String = ""
        Dim strLine90 As String = ""
        Dim strLine91 As String = ""
        Dim strLine92 As String = ""
        Dim strLine93 As String = ""
        Dim strLine94 As String = ""
        Dim strLine95 As String = ""
        Dim strLine96 As String = ""
        Dim strLine97 As String = ""
        Dim strLine98 As String = ""
        Dim strLine99 As String = ""
        Dim strLine100 As String = ""
        Dim strLine101 As String = ""
        Dim strLine102 As String = ""
        Dim strLine103 As String = ""
        Dim strLine104 As String = ""
        Dim strLine105 As String = ""
        Dim strLine106 As String = ""
        Dim strLine107 As String = ""
        Dim strLine108 As String = ""
        Dim strLine109 As String = ""
        Dim strLine110 As String = ""
        Dim strLine111 As String = ""
        Dim strLine112 As String = ""
        Dim strLine113 As String = ""
        Dim strLine114 As String = ""
        Dim strLine115 As String = ""
        Dim strLine116 As String = ""
        Dim strLine117 As String = ""
        Dim strLine118 As String = ""
        Dim strLine119 As String = ""
        Dim strLine120 As String = ""
        Dim strLine121 As String = ""
        Dim strLine122 As String = ""
        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String
        Dim _newLoad As Integer
        Try
            ' Execute stored procedure to write records to CDT_Header and Detail tables, and update status and location for Combo to INTransit
            sqlConn.Open()
            sSqlString = "EXECUTE @i = spCD_TransferCombos '" & Me.lbXferTruckVal.Text & "', 110,'" & _userid & "',0,0"
            cmdSqlCommand.CommandText = sSqlString
            cmdSqlCommand.Connection = sqlConn
            cmdSqlCommand.Parameters.Add("@i", SqlDbType.Int).Direction = ParameterDirection.Output
            cmdSqlCommand.ExecuteNonQuery()
            _newLoad = cmdSqlCommand.Parameters("@i").Value
            sqlConn.Close()

            'Print the Manifest for the records
            sqlString = "Select * from vwCD_CombosManifest Where ProcessAreaDesc = '" & Me.lbXferTruckVal.Text & "' AND TransferNumber = " & _newLoad
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(dsManifest, "Data")

            If dsManifest Is Nothing Then
                Me.lbError.Text = "Error getting data for Combo Manifest try again or contact MIS"
                Exit Sub
            Else
                If dsManifest.Tables("Data").Rows.Count > 0 Then
                    Dim strLine1 As String = "*FORMAT,COMBO_MANIFEST"
                    Dim strLine2 As String = "HR_Truck," & Me.lbXferTruckVal.Text

                    For Each Row In dsManifest.Tables("Data").Rows
                        x = x + 1
                        Select Case x
                            Case Is = 1
                                strLine3 = "HR_ComboQty01," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine4 = "HR_Formula_Size01," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine5 = "HR_Lot01," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine6 = "HR_ForGrsWt01," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 2
                                strLine7 = "HR_ComboQty02," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine8 = "HR_Formula_Size02," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine9 = "HR_Lot02," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine10 = "HR_ForGrsWt02," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 3
                                strLine11 = "HR_ComboQty03," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine12 = "HR_Formula_Size03," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine13 = "HR_Lot03," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine14 = "HR_ForGrsWt03," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 4
                                strLine15 = "HR_ComboQty04," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine16 = "HR_Formula_Size04," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine17 = "HR_Lot04," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine18 = "HR_ForGrsWt04," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 5
                                strLine19 = "HR_ComboQty05," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine20 = "HR_Formula_Size05," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine21 = "HR_Lot05," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine22 = "HR_ForGrsWt05," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 6
                                strLine23 = "HR_ComboQty06," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine24 = "HR_Formula_Size06," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine25 = "HR_Lot06," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine26 = "HR_ForGrsWt06," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 7
                                strLine27 = "HR_ComboQty07," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine28 = "HR_Formula_Size07," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine29 = "HR_Lot07," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine30 = "HR_ForGrsWt07," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 8
                                strLine31 = "HR_ComboQty08," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine32 = "HR_Formula_Size08," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine33 = "HR_Lot08," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine34 = "HR_ForGrsWt08," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 9
                                strLine35 = "HR_ComboQty09," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine36 = "HR_Formula_Size09," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine37 = "HR_Lot09," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine38 = "HR_ForGrsWt09," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 10
                                strLine39 = "HR_ComboQty10," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine40 = "HR_Formula_Size10," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine41 = "HR_Lot10," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine42 = "HR_ForGrsWt10," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 11
                                strLine43 = "HR_ComboQty11," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine44 = "HR_Formula_Size11," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine45 = "HR_Lot11," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine46 = "HR_ForGrsWt11," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 12
                                strLine47 = "HR_ComboQty12," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine48 = "HR_Formula_Size12," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine49 = "HR_Lot12," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine50 = "HR_ForGrsWt12," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 13
                                strLine51 = "HR_ComboQty13," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine52 = "HR_Formula_Size13," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine53 = "HR_Lot13," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine54 = "HR_ForGrsWt13," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 14
                                strLine55 = "HR_ComboQty14," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine56 = "HR_Formula_Size14," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine57 = "HR_Lot14," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine58 = "HR_ForGrsWt14," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 15
                                strLine59 = "HR_ComboQty15," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine60 = "HR_Formula_Size15," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine61 = "HR_Lot15," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine62 = "HR_ForGrsWt15," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 16
                                strLine63 = "HR_ComboQty16," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine64 = "HR_Formula_Size16," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine65 = "HR_Lot16," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine66 = "HR_ForGrsWt16," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 17
                                strLine67 = "HR_ComboQty17," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine68 = "HR_Formula_Size17," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine69 = "HR_Lot17," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine70 = "HR_ForGrsWt17," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 18
                                strLine71 = "HR_ComboQty18," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine72 = "HR_Formula_Size18," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine73 = "HR_Lot18," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine74 = "HR_ForGrsWt18," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 19
                                strLine75 = "HR_ComboQty19," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine76 = "HR_Formula_Size19," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine77 = "HR_Lot19," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine78 = "HR_ForGrsWt19," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 20
                                strLine79 = "HR_ComboQty20," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine80 = "HR_Formula_Size20," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine81 = "HR_Lot20," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine82 = "HR_ForGrsWt20," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 21
                                strLine83 = "HR_ComboQty21," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine84 = "HR_Formula_Size21," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine85 = "HR_Lot21," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine86 = "HR_ForGrsWt21," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 22
                                strLine87 = "HR_ComboQty22," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine88 = "HR_Formula_Size22," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine89 = "HR_Lot22," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine90 = "HR_ForGrsWt22," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 23
                                strLine91 = "HR_ComboQty23," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine92 = "HR_Formula_Size23," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine93 = "HR_Lot23," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine94 = "HR_ForGrsWt23," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 24
                                strLine95 = "HR_ComboQty24," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine96 = "HR_Formula_Size24," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine97 = "HR_Lot24," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine98 = "HR_ForGrsWt24," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 25
                                strLine99 = "HR_ComboQty25," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine100 = "HR_Formula_Size25," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine101 = "HR_Lot25," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine102 = "HR_ForGrsWt25," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 26
                                strLine103 = "HR_ComboQty26," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine104 = "HR_Formula_Size26," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine105 = "HR_Lot26," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine106 = "HR_ForGrsWt26," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 27
                                strLine107 = "HR_ComboQty27," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine108 = "HR_Formula_Size27," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine109 = "HR_Lot27," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine110 = "HR_ForGrsWt27," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 28
                                strLine111 = "HR_ComboQty28," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine112 = "HR_Formula_Size28," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine113 = "HR_Lot28," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine114 = "HR_ForGrsWt28," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 29
                                strLine115 = "HR_ComboQty29," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine116 = "HR_Formula_Size29," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine117 = "HR_Lot29," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine118 = "HR_ForGrsWt29," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                            Case Is = 30
                                strLine119 = "HR_ComboQty30," & dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty")
                                TtlComboQty = TtlComboQty + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("ComboQty"))
                                strLine120 = "HR_Formula_Size30," & dsManifest.Tables("Data").Rows(x - 1).Item("FormulaStuffingGroup")
                                strLine121 = "HR_Lot30," & dsManifest.Tables("Data").Rows(x - 1).Item("LotNo")
                                'strLine122 = "HR_ForGrsWt30," & dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt")
                                'TtlGrossWt = TtlGrossWt + CInt(dsManifest.Tables("Data").Rows(x - 1).Item("SumGrossWt"))
                        End Select

                    Next
                    Dim strLine123 As String = "HR_TotalCombos," & TtlComboQty
                    'Dim strLine124 As String = "HR_TotalGrsWt," & TtlGrossWt
                    Dim strLine125 As String = "BC_ComboXferID," & _newLoad
                    Dim strLine126 As String = "*QUANTITY,1"
                    Dim strLine127 As String = "*PRINTERNUMBER," & _printer
                    Dim strLine128 As String = "*PRINTLABEL"

                    destPath = "\\SRV04\wddrop\"
                    tempPath = "\\SRV04\wddroptemp\"

                    filenm = Trim(_ComboXferID) & DatePart("yyyy", Now) & DatePart("m", Now) & DatePart("d", Now) & DatePart("h", Now) & DatePart("n", Now) & DatePart("s", Now) & ".pas"
                    PathFile = tempPath & filenm

                    'declaring a FileStream and creating a text document file named file with access mode of writing
                    Dim fs As New FileStream(PathFile, FileMode.Create, FileAccess.Write)
                    'creating a new StreamWriter and passing the filestream object fs as argument
                    Dim s As New StreamWriter(fs)
                    'the seek method is used to move the cursor to next position to avoid text to be overwritten
                    s.BaseStream.Seek(0, SeekOrigin.End)

                    'Write each line of the pas file required by Loftware
                    s.WriteLine(strLine1)
                    s.WriteLine(strLine2)
                    s.WriteLine(strLine3)
                    s.WriteLine(strLine4)
                    s.WriteLine(strLine5)
                    's.WriteLine(strLine6)
                    s.WriteLine(strLine7)
                    s.WriteLine(strLine8)
                    s.WriteLine(strLine9)
                    's.WriteLine(strLine10)
                    s.WriteLine(strLine11)
                    s.WriteLine(strLine12)
                    s.WriteLine(strLine13)
                    's.WriteLine(strLine14)
                    s.WriteLine(strLine15)
                    s.WriteLine(strLine16)
                    s.WriteLine(strLine17)
                    's.WriteLine(strLine18)
                    s.WriteLine(strLine19)
                    s.WriteLine(strLine20)
                    s.WriteLine(strLine21)
                    's.WriteLine(strLine22)
                    s.WriteLine(strLine23)
                    s.WriteLine(strLine24)
                    s.WriteLine(strLine25)
                    's.WriteLine(strLine26)
                    s.WriteLine(strLine27)
                    s.WriteLine(strLine28)
                    s.WriteLine(strLine29)
                    's.WriteLine(strLine30)
                    s.WriteLine(strLine31)
                    s.WriteLine(strLine32)
                    s.WriteLine(strLine33)
                    's.WriteLine(strLine34)
                    s.WriteLine(strLine35)
                    s.WriteLine(strLine36)
                    s.WriteLine(strLine37)
                    's.WriteLine(strLine38)
                    s.WriteLine(strLine39)
                    s.WriteLine(strLine40)
                    s.WriteLine(strLine41)
                    's.WriteLine(strLine42)
                    s.WriteLine(strLine43)
                    s.WriteLine(strLine44)
                    s.WriteLine(strLine45)
                    's.WriteLine(strLine46)
                    s.WriteLine(strLine47)
                    s.WriteLine(strLine48)
                    s.WriteLine(strLine49)
                    's.WriteLine(strLine50)
                    s.WriteLine(strLine51)
                    s.WriteLine(strLine52)
                    s.WriteLine(strLine53)
                    's.WriteLine(strLine54)
                    s.WriteLine(strLine55)
                    s.WriteLine(strLine56)
                    s.WriteLine(strLine57)
                    's.WriteLine(strLine58)
                    s.WriteLine(strLine59)
                    s.WriteLine(strLine60)
                    s.WriteLine(strLine61)
                    's.WriteLine(strLine62)
                    s.WriteLine(strLine63)
                    s.WriteLine(strLine64)
                    s.WriteLine(strLine65)
                    's.WriteLine(strLine66)
                    s.WriteLine(strLine67)
                    s.WriteLine(strLine68)
                    s.WriteLine(strLine69)
                    's.WriteLine(strLine70)
                    s.WriteLine(strLine71)
                    s.WriteLine(strLine72)
                    s.WriteLine(strLine73)
                    's.WriteLine(strLine74)
                    s.WriteLine(strLine75)
                    s.WriteLine(strLine76)
                    s.WriteLine(strLine77)
                    's.WriteLine(strLine78)
                    s.WriteLine(strLine79)
                    s.WriteLine(strLine80)
                    s.WriteLine(strLine81)
                    's.WriteLine(strLine82)
                    s.WriteLine(strLine83)
                    s.WriteLine(strLine84)
                    s.WriteLine(strLine85)
                    's.WriteLine(strLine86)
                    s.WriteLine(strLine87)
                    s.WriteLine(strLine88)
                    s.WriteLine(strLine89)
                    's.WriteLine(strLine90)
                    s.WriteLine(strLine91)
                    s.WriteLine(strLine92)
                    s.WriteLine(strLine93)
                    's.WriteLine(strLine94)
                    s.WriteLine(strLine95)
                    s.WriteLine(strLine96)
                    s.WriteLine(strLine97)
                    's.WriteLine(strLine98)
                    s.WriteLine(strLine99)
                    s.WriteLine(strLine100)
                    s.WriteLine(strLine101)
                    's.WriteLine(strLine102)
                    s.WriteLine(strLine103)
                    s.WriteLine(strLine104)
                    s.WriteLine(strLine105)
                    's.WriteLine(strLine106)
                    s.WriteLine(strLine107)
                    s.WriteLine(strLine108)
                    s.WriteLine(strLine109)
                    's.WriteLine(strLine110)
                    s.WriteLine(strLine111)
                    s.WriteLine(strLine112)
                    s.WriteLine(strLine113)
                    's.WriteLine(strLine114)
                    s.WriteLine(strLine115)
                    s.WriteLine(strLine116)
                    s.WriteLine(strLine117)
                    's.WriteLine(strLine118)
                    s.WriteLine(strLine119)
                    s.WriteLine(strLine120)
                    s.WriteLine(strLine121)
                    's.WriteLine(strLine122)
                    s.WriteLine(strLine123)
                    's.WriteLine(strLine124)
                    s.WriteLine(strLine125)
                    s.WriteLine(strLine126)
                    s.WriteLine(strLine127)
                    s.WriteLine(strLine128)
                    s.Close()

                    sourceFile = tempPath & filenm
                    successfile = tempPath & filenm & ".success"
                    destinationFile = destPath & filenm

                    'Delete destinationFile for overwrite,
                    'causes exception if already exists.
                    File.Delete(destinationFile)
                    File.Copy(sourceFile, destinationFile)

                    Me.btFinishTransferProcess.Visible = True
                    Me.btFinishedLoading.Visible = False
                    Me.lbPrompt.Text = "After you get the manifest from the printer, press the Finish Transfer button."
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error - " & ex.Message.ToString & " - occurred while printing manifest for Truck - " & Me.lbXferTruckVal.Text
        End Try
    End Sub

    Protected Sub btFinishTransferProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinishTransferProcess.Click
        Me.InitProcess()
    End Sub

End Class