Imports System.IO
Imports System.IO.File

Partial Public Class IC_ComboBuild
    Inherits System.Web.UI.Page

    Public strURL As String = Nothing
    Public _whs As String
    Public _company As String
    Public _function As Integer
    Public _product As String
    Public _productversion As String
    Public _BuildID As Long
    Public _ComboTypeID As Integer
    Public _ComboType As String
    Public _Formula As String
    Public _StuffingGroupID As String
    Public _Lot As Integer
    Public _buildIdAttempts As Integer = 0
    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString
    Private sqlConn As New SqlClient.SqlConnection(sConnString)

#Region "Form Events - No Buttons"
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
        _company = ConfigurationManager.AppSettings.Get("CompanyNumber").ToString

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txData)
        Else 'Page is posted back 
            _function = CInt(lbFunction.Text)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If
    End Sub

    Public Sub InitProcess() 'Initialize screen on initial open.
        Me.lbError.Text = ""
        Me.lbError.Visible = False
        Me._buildIdAttempts = 0

        Me.txData.Visible = True
        Me.txData.Text = ""

        Me.lbStation.Visible = False
        Me.lbStationValue.Visible = False
        Me.lbStationValue.Text = ""
        Me.lbComboType.Visible = False
        Me.lbComboTypeVal.Visible = False
        Me.lbComboTypeVal.Text = ""
        Me.lbComboTypeValDesc.Visible = False
        Me.lbComboTypeValDesc.Text = ""
        Me.lbBuild.Visible = False
        Me.lbBuildID.Visible = False
        Me.lbBuildID.Text = ""
        Me.lbFormula.Visible = False
        Me.lbFormulaValue.Visible = False
        Me.lbFormulaValue.Text = ""
        Me.lbStuffingGroup.Visible = False
        Me.lbStuffingGroupValue.Visible = False
        Me.lbStuffingGroupValue.Text = ""

        Me.gvStuffingGroup.Visible = False
        Me.lbBuildRacks.Visible = False
        Me.gvBuildRacks.Visible = False

        Me.btRestartCombo.Visible = False
        Me.btFinishCombo.Visible = False
        Me.btVerifyFinished.Visible = False

        Common.SaveVariable("RackSeq", "", Page)

        Me.lbPrompt.Text = "Scan or Enter Station #"
        Me.lbFunction.Text = "1"

    End Sub

    Public Sub InitNewCombo()
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.txData.Visible = True
        Me.txData.Text = ""

        Me.lbStation.Visible = True
        Me.lbStationValue.Visible = True
        Me.lbComboType.Visible = False
        Me.lbComboTypeVal.Visible = False
        Me.lbComboTypeVal.Text = ""
        Me.lbComboTypeValDesc.Visible = False
        Me.lbComboTypeValDesc.Text = ""

        Me.lbFormula.Visible = False
        Me.lbFormulaValue.Visible = False
        Me.lbFormulaValue.Text = ""
        Me.lbStuffingGroup.Visible = False
        Me.lbStuffingGroupValue.Visible = False
        Me.lbStuffingGroupValue.Text = ""
        Me.lbLot.Visible = False
        Me.lbLot.Text = ""

        Me.gvStuffingGroup.Visible = False
        Me.lbBuildRacks.Visible = False
        Me.gvBuildRacks.Visible = False

        Me.btRestartCombo.Visible = False
        Me.btFinishCombo.Visible = False
        Me.btVerifyFinished.Visible = False

        Common.SaveVariable("RackSeq", "", Page)

        Me.lbPrompt.Text = "Scan or Enter Combo Type"
        Me.lbFunction.Text = "2"
    End Sub

    Public Sub DisplayActiveBuild(ByVal _buildval As Long, ByVal _combotypeval As Integer _
                                    , ByVal _combotype As String, ByVal _formulaval As String _
                                    , ByVal _stuffgroupval As String, ByVal _lotval As Integer)
        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.txData.Visible = True
        Me.txData.Text = ""

        Me.lbBuild.Visible = True
        Me.lbBuildID.Visible = True
        Me.lbBuildID.Text = _buildval.ToString
        Me.lbComboType.Visible = True
        Me.lbComboTypeVal.Visible = True
        Me.lbComboTypeVal.Text = _combotypeval.ToString
        Me.lbComboTypeValDesc.Visible = True
        Me.lbComboTypeValDesc.Text = _combotype.ToString
        Me.lbFormula.Visible = True
        Me.lbFormulaValue.Visible = True
        Me.lbFormulaValue.Text = _formulaval.ToString
        Me.lbLot.Visible = True
        Me.lbLot.Text = _lotval
        Me.lbStuffingGroup.Visible = True
        Me.lbStuffingGroupValue.Visible = True
        Me.lbStuffingGroupValue.Text = _stuffgroupval.ToString

        Me.gvStuffingGroup.Visible = False
        Me.lbBuildRacks.Visible = True
        LoadComboBuildRacks(_buildval)

        Me.btVerifyFinished.Visible = False
        Me.btRestartCombo.Visible = False
        Me.btContinueCombo.Visible = True
        Me.btFinishCombo.Visible = True


    End Sub

    Public Sub txData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txData.TextChanged
        Try
            If Len(RTrim(Me.txData.Text)) < 1 Then
                Me.lbError.Text = "Value must be entered - try again."
                Me.lbError.Visible = True
                Exit Sub
            End If

            If IsNumeric(Me.txData.Text) = False Then
                Me.lbError.Text = "Value entered must be a number - try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txData)
                Exit Sub
            End If

            Select Case _function

                Case 1 'Scan or enter Station #
                    If ValidateStation(Me.txData.Text) = False Then
                        Me.lbError.Text = "Error validating Station # scanned in database - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "1"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)

                    Else 'Found record for Station check for Builds with status = 10 (Active)
                        Me.lbStation.Visible = True
                        Me.lbStationValue.Text = Me.txData.Text
                        Me.lbStationValue.Visible = True

                        If GetActiveBuilds(Me.txData.Text) = False Then
                            'No current Active builds exist - Ready screen to continue new Build
                            Call NewRestart()

                        Else 'Build exists in database with status 10 (Active) for this station - display it on screen
                            _BuildID = CLng(Common.GetVariable("BuildID", Page))
                            _ComboTypeID = CInt(Common.GetVariable("ComboTypeID", Page))
                            _ComboType = Common.GetVariable("ComboType", Page)
                            _Formula = Common.GetVariable("Formula", Page)
                            _StuffingGroupID = Common.GetVariable("StuffingGroupID", Page)
                            _Lot = Common.GetVariable("LotNo", Page)
                            Call DisplayActiveBuild(_BuildID, _ComboTypeID, _ComboType, _Formula, _StuffingGroupID, _Lot)
                            Me.lbPrompt.Text = "Press Continue or Finish Combo button"

                        End If
                    End If

                Case 2 'User has scanned the Combo Type being used
                    'Validate the ComboType
                    If ValidateComboType(Me.txData.Text) = False Then
                        Me.lbError.Text = "Error validating Combo Type scanned in database - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "2"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else 'ComboType Valid
                        'Prompt user to Scan the 1st Rack# for the Combo Build - display ComboType to screen
                        Me.lbComboType.Visible = True
                        Me.lbComboTypeVal.Text = _ComboTypeID
                        Me.lbComboTypeVal.Visible = True
                        Me.lbComboTypeValDesc.Text = _ComboType
                        Me.lbComboTypeValDesc.Visible = True
                        Me.lbFunction.Text = "3"
                        Me.txData.Text = ""
                        Me.lbPrompt.Text = "Scan 1st Rack# for the Combo Build"
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    End If

                Case 3 'User has Scanned a Rack to the Combo Build 
                    If ValidateRack(Me.txData.Text) = False Then
                        Me.lbError.Text = "Error validating Rack# scanned in database - try again or see your supervisor."
                        Me.lbError.Visible = True
                        Me.lbFunction.Text = "3"
                        Me.txData.Text = ""
                        Common.JavaScriptSetFocus(Page, Me.txData)
                    Else 'Rack exists
                        If Me.lbFormulaValue.Text = "" Then 'Rack scanned is the 1st added to Combo Build
                            'Set RackSeq = FIRST and prompt user for Stuffing Group. Process 1st rack after that.
                            Common.SaveVariable("RackSeq", "FIRST", Page)
                            Me.lbFormula.Visible = True
                            Me.lbFormulaValue.Visible = True
                            Me.lbFormulaValue.Text = Common.GetVariable("RackFormula", Page).ToString
                            Me.lbLot.Text = Common.GetVariable("RackLotNumber", Page).ToString
                            Me.lbLot.Visible = True

                            'Prompt user for the Stuffing Size for the Cut Down - Hide the txData
                            If LoadStuffingGroups(Common.GetVariable("RackFormula", Page).ToString) = True Then
                                Me.txData.Visible = False
                                Me.lbPrompt.Text = "Select Stuffing Group from list so we can process Rack scanned"
                            Else
                                Me.lbError.Text = "Error occurred while loading Stuffing Groups for Formula - Press Restart Combo to try again or see your supervisor."
                                Me.lbError.Visible = True
                                Exit Sub
                            End If
                        Else
                            'Check that Formula and Lot match the 1st Rack added to the Combo
                            If Me.lbLot.Text = Common.GetVariable("RackLotNumber", Page).ToString And Me.lbFormulaValue.Text = Common.GetVariable("RackFormula", Page).ToString Then
                                Common.SaveVariable("RackSeq", "", Page)
                                'This Rack# is not the first one added go ahead and write Build Rack record
                                If ProcessRack(Me.txData.Text, CLng(Me.lbBuildID.Text)) = False Then
                                    Me.lbError.Text = "Error occurred while adding Rack# to Combo Build - Scan Rack# again."
                                    Me.lbError.Visible = True
                                    Me.lbFunction.Text = "3"
                                    Me.txData.Text = ""
                                    Me.lbPrompt.Text = "Scan last Rack again."
                                    Exit Sub
                                Else 'success writing Build Rack record
                                    'Prompt user for the Next Rack or to Press Finish Combo
                                    Me.lbFunction.Text = "3"
                                    Me.txData.Text = ""
                                    Me.lbPrompt.Text = "Scan Next Rack or Press Finish Combo button"
                                    Me.btFinishCombo.Visible = True
                                    Common.SaveVariable("RackNumber", "", Page)
                                    Common.SaveVariable("RackFormula", "", Page)
                                    Common.SaveVariable("RackLotNumber", "", Page)
                                    Common.JavaScriptSetFocus(Page, Me.txData)
                                End If
                            Else
                                Me.lbError.Text = "Rack # scanned not from same Formula and Lot required."
                                Me.lbError.Visible = True
                                Me.lbFunction.Text = "3"
                                Me.txData.Text = ""
                                Me.lbPrompt.Text = "Scan a Rack that matches Formula and Lot displayed on screen."
                                Exit Sub
                            End If

                        End If
                    End If
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Function ProcessRack(ByVal _rack As Long, ByVal _build As Long) As Boolean
        ProcessRack = False
        If Common.GetVariable("RackSeq", Page).ToString = "FIRST" Then
            'Create the Build record and write the Rack to Build Racks table
            If CreateBuildCombo(_build) = True Then
                'Write the 1st Rack to the CD_BuildRacks table
                If CreateBuildComboRack(_build, _rack) = True Then
                    Common.SaveVariable("RackSeq", "", Page)
                    LoadComboBuildRacks(_build)
                    ProcessRack = True
                End If
            Else
                Exit Function
            End If
        Else
            'Write the Rack to the CD_BuildRacks table
            If CreateBuildComboRack(_build, _rack) = True Then
                LoadComboBuildRacks(_build)
                ProcessRack = True
            Else
                Exit Function
            End If
        End If

    End Function

    Public Function ValidateRack(ByVal _rack As Long) As Boolean
        ValidateRack = False

        Dim dsValidateRack As New Data.DataSet
        Dim sqlCmdValidateRack As New System.Data.SqlClient.SqlCommand

        'open database connection
        If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""ValidateRack Connection not established""")

        sqlCmdValidateRack = DB.NewSQLCommand("SQL.Query.ValidateRack")
        If sqlCmdValidateRack Is Nothing Then Throw New Exception("""Loading ValidateRack Command failed. Restart screen and try again.""")
        sqlCmdValidateRack.Parameters.AddWithValue("@RackNumber", _rack)
        dsValidateRack = DB.GetDataSet(sqlCmdValidateRack)
        sqlCmdValidateRack.Dispose() : sqlCmdValidateRack = Nothing
        DB.KillSQLConnection()

        If dsValidateRack Is Nothing Then Throw New Exception("""Running ValidateRack Command failed. Restart screen and try again.""")
        If dsValidateRack.Tables(0).Rows.Count > 0 Then
            Common.SaveVariable("RackNumber", _rack, Page)
            Common.SaveVariable("RackFormula", dsValidateRack.Tables(0).Rows(0).Item("Formula"), Page)
            Common.SaveVariable("RackLotNumber", dsValidateRack.Tables(0).Rows(0).Item("LotNumber"), Page)

            ValidateRack = True
        End If
    End Function

    Public Sub gvStuffingGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gvStuffingGroup.SelectedIndexChanged
        Me.lbStuffingGroupValue.Text = Me.gvStuffingGroup.SelectedRow.Cells(1).Text
        Me.lbStuffingGroup.Visible = True
        Me.lbStuffingGroupValue.Visible = True
        Me.gvStuffingGroup.SelectedIndex = -1
        Me.gvStuffingGroup.Visible = False

        'Create Build Record and Write 1st Rack record in BuildRacks table
        If ProcessRack(Me.txData.Text, CLng(Me.lbBuildID.Text)) = False Then
            Me.lbError.Text = "Error occurred while adding Rack# to Combo Build - Scan Rack# again."
            Me.lbError.Visible = True
            Me.lbFunction.Text = "3"
            Me.txData.Text = ""
            Me.txData.Visible = True
            Me.lbPrompt.Text = "Scan last Rack again."
            Exit Sub
        Else 'success writing Build Rack record
            'Prompt user for the Next Rack or to Press Finish Combo
            Me.lbFunction.Text = "3"
            Me.txData.Text = ""
            Me.txData.Visible = True
            Me.lbPrompt.Text = "Scan Next Rack or Press Finish Combo button"
            Me.btFinishCombo.Visible = True
            Common.SaveVariable("RackNumber", "", Page)
            Common.SaveVariable("RackFormula", "", Page)
            Common.SaveVariable("RackLotNumber", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txData)
        End If

    End Sub

#End Region

#Region "Form Events - Buttons"

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Combo Menu
        Common.SaveVariable("newURL", "~/IC_ComboMenu.aspx", Page)
    End Sub

    Protected Sub btRestartCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestartCombo.Click
        If Trim(Me.lbBuildID.Text.Length) > 0 Then
            ''Stored Procedure to update record in CD_Builds to deleted so that user can start over
            If UpdateBuildCombo(CLng(Me.lbBuildID.Text), 99) = False Then
                'Record update failed 
                lbError.Text = "Restart Combo Build failed to Finish, please press the Restart Combo button again or contact supervisor."
                lbError.Visible = True
                Me.btFinishCombo.Visible = True
                Me.lbPrompt.Text = "Press the Restart Combo button again"
                Exit Sub
            End If
        End If

        Call NewRestart()
    End Sub

    Protected Sub btFinishCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFinishCombo.Click
        Me.btVerifyFinished.Visible = True
        Me.btFinishCombo.Visible = False
        Me.btContinueCombo.Text = "Cancel"
        Me.btContinueCombo.Visible = True
        Me.lbPrompt.Text = "If Done - Press Verified-Yes button or Press Cancel button to continue filling combo."
    End Sub

    Protected Sub btVerifyFinished_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btVerifyFinished.Click
        ''Stored Procedure to update record in CD_Builds
        If UpdateBuildCombo(CLng(Me.lbBuildID.Text), 20) = True Then
            'Record successfully updated and Status changed to 20 (Filled)
            Me.lbPrompt.Text = "Press New Combo button to start next Combo"
            Me.btContinueCombo.Visible = False
            Me.btVerifyFinished.Visible = False
            Me.btContinueCombo.Visible = False
            Me.btNextCombo.Visible = True


        Else
            'Record update failed 
            lbError.Text = "Combo Build failed to Finish, please press the Finish Combo button again or contact supervisor."
            lbError.Visible = True
            Me.btFinishCombo.Visible = True
            Me.lbPrompt.Text = "Press the Finish Combo button again"
        End If
        Me.btVerifyFinished.Visible = False

    End Sub

    Protected Sub btContinueCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btContinueCombo.Click
        If Me.btContinueCombo.Text = "Cancel" Then
            Me.btVerifyFinished.Visible = False
            Me.btContinueCombo.Text = "Continue"
        End If
        Me.btContinueCombo.Visible = False

        'Prompt user to scan the next Rack or click Finish Combo
        Me.lbFunction.Text = "3"
        Me.txData.Text = ""
        Me.lbPrompt.Text = "Scan next Rack# or Press Finish Combo button."
        Common.JavaScriptSetFocus(Page, Me.txData)
    End Sub

    Protected Sub btNextCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNextCombo.Click
        Call NewRestart()
    End Sub

#End Region

#Region "Custom Events"

    Public Sub NewRestart()
        Call GetNextBuildID() 'ExecuteNonQuery with spGetNextBuildID

        'Initialize screen for Next Combo and prompt user to Scan ComboType barcode he is using for the Combo
        InitNewCombo()

        'Prompt user next action
        _function = 2
        Me.lbPrompt.Text = "Scan Combo Type for this New Combo Build"
    End Sub

    Public Sub GetNextBuildID()
        Dim sqlGetNextBuildID As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Dim _newbuild As Long = 0
        _buildIdAttempts = _buildIdAttempts + 1
        Me.lbError.Visible = False

        Try
            sqlString = "Exec spCD_NextBuildID "

            'Get Record to acquire Formula and Prod Description
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Get Next Build ID Connection not established.""")
            sqlGetNextBuildID = DB.NewSQLCommand3(sqlString)

            If sqlGetNextBuildID Is Nothing Then Throw New Exception("""Get Next Build ID Command - Failed to create query.""")
            _newbuild = sqlGetNextBuildID.ExecuteScalar()

            If _newbuild <> 0 Then
                Me.lbBuildID.Text = _newbuild
                Me.lbBuildID.Visible = True
                Me.lbBuild.Visible = True
            Else
                If _buildIdAttempts = 1 Then
                    'Try again to get a new Build Id # from the database
                    Call NewRestart()
                Else
                    Throw New Exception("""Get Next Build ID Command - Failed to acquire BuildID from database""")
                End If
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & " occurred while Getting Next Build #. Restart screen and try again."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function ValidateStation(ByVal _station As Integer) As Boolean
        ValidateStation = False
        Dim dsStation As New Data.DataSet
        Dim sqlCmdStation As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Me.lbError.Visible = False

        Try
            sqlString = "Select * from CD_Stations Where StationID = " & _station & " And Active = 'Y'"

            'Get Record to acquire Formula and Prod Description
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Validate Station Connection not established.""")
            sqlCmdStation = DB.NewSQLCommand3(sqlString)
            If sqlCmdStation Is Nothing Then Throw New Exception("""Validate Station Command - Failed to create query.""")
            dsStation = DB.GetDataSet(sqlCmdStation)
            sqlCmdStation.Dispose() : sqlCmdStation = Nothing
            DB.KillSQLConnection()

            If dsStation Is Nothing Then Throw New Exception("""Running Validate Station Command failed.""")
            If dsStation.Tables(0).Rows.Count > 0 Then
                ValidateStation = True
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & " occurred while validating Station #. Restart screen and try again."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function ValidateComboType(ByVal _combotyp As Integer) As Boolean
        ValidateComboType = False
        Dim dsValidateComboType As New Data.DataSet
        Dim sqlCmdValidateComboType As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Validate Combo Type Connection not established""")

            sqlCmdValidateComboType = DB.NewSQLCommand("SQL.Query.ValidateComboType")
            If sqlCmdValidateComboType Is Nothing Then Throw New Exception("""Validate Combo Type Command - Failed to create query.""")
            sqlCmdValidateComboType.Parameters.AddWithValue("@ComboTypeID", _combotyp)
            dsValidateComboType = DB.GetDataSet(sqlCmdValidateComboType)
            sqlCmdValidateComboType.Dispose() : sqlCmdValidateComboType = Nothing
            DB.KillSQLConnection()

            If dsValidateComboType Is Nothing Then Throw New Exception("""Running Validate Combo Type Command failed.""")
            If dsValidateComboType.Tables(0).Rows.Count > 0 Then
                ValidateComboType = True
                _ComboTypeID = CInt(dsValidateComboType.Tables(0).Rows(0).Item("ComboTypeID").ToString)
                _ComboType = dsValidateComboType.Tables(0).Rows(0).Item("ComboType").ToString
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & " occurred while validating Combo Type. Restart screen and try again."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function GetActiveBuilds(ByVal _station As Integer) As Boolean
        GetActiveBuilds = False
        Dim dsActiveBuilds As New Data.DataSet
        Dim sqlCmdActiveBuilds As New System.Data.SqlClient.SqlCommand
        Dim sqlString As String = Nothing
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Get Active Builds Connection not established""")

            sqlCmdActiveBuilds = DB.NewSQLCommand("SQL.Query.GetActiveBuilds")
            If sqlCmdActiveBuilds Is Nothing Then Throw New Exception("""Get Active Builds Command failed.""")
            sqlCmdActiveBuilds.Parameters.AddWithValue("@StationID", _station)
            dsActiveBuilds = DB.GetDataSet(sqlCmdActiveBuilds)
            sqlCmdActiveBuilds.Dispose() : sqlCmdActiveBuilds = Nothing
            DB.KillSQLConnection()

            If dsActiveBuilds Is Nothing Then Throw New Exception("""Running Get Active Builds Command failed.""")
            If dsActiveBuilds.Tables(0).Rows.Count > 0 Then
                GetActiveBuilds = True
                Common.SaveVariable("BuildID", dsActiveBuilds.Tables(0).Rows(0).Item("BuildID").ToString, Page)
                Common.SaveVariable("ComboTypeID", dsActiveBuilds.Tables(0).Rows(0).Item("ComboTypeID").ToString, Page)
                Common.SaveVariable("ComboType", dsActiveBuilds.Tables(0).Rows(0).Item("ComboType").ToString, Page)
                Common.SaveVariable("Formula", dsActiveBuilds.Tables(0).Rows(0).Item("Formula").ToString, Page)
                Common.SaveVariable("StuffingGroupID", dsActiveBuilds.Tables(0).Rows(0).Item("StuffingGroupID").ToString, Page)
                Common.SaveVariable("LotNo", dsActiveBuilds.Tables(0).Rows(0).Item("LotNo").ToString, Page)
            End If
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & " occurred while Getting Active Builds. Restart screen and try again."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function LoadComboBuildRacks(ByVal _buildid As Long) As Boolean
        LoadComboBuildRacks = False

        'Load Me.gvBuildRacks for Build ID passed in to Sub
        Try
            Dim sqlString As String
            Dim sqlDABuildRacks As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            sqlString = "Select * from vwCD_BuildsRacks Where BuildID = " & _buildid
            sqlDABuildRacks = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDABuildRacks)
            sqlDABuildRacks.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            Me.gvBuildRacks.DataSource = dv

            Me.gvBuildRacks.DataBind()

            If dv.Count > 0 Then
                Me.gvBuildRacks.Visible = True
                LoadComboBuildRacks = True
            Else
                Me.gvBuildRacks.Visible = False
                LoadComboBuildRacks = False
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function LoadStuffingGroups(ByVal _formulaID As String) As Boolean
        LoadStuffingGroups = False

        'Load Me.gvBuildRacks for Build ID passed in to Sub
        Try
            Dim sqlString As String
            Dim sqlDAStuffingGroups As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            sqlString = "Select StuffingGroupID, StuffingGroupDesc from vwCD_StuffingGroups Where Formula = '" & _formulaID & "'"
            sqlDAStuffingGroups = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAStuffingGroups)
            sqlDAStuffingGroups.Fill(ds, "Data")
            dv = New DataView(ds.Tables("Data"), Nothing, Nothing, DataViewRowState.CurrentRows)

            Me.gvStuffingGroup.DataSource = dv

            Me.gvStuffingGroup.DataBind()

            If dv.Count > 0 Then
                Me.gvStuffingGroup.Visible = True
                LoadStuffingGroups = True
            Else
                Me.gvStuffingGroup.Visible = False
                LoadStuffingGroups = False
            End If
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function CreateBuildCombo(ByVal _buildid As Long) As Boolean
        CreateBuildCombo = False
        Dim sqlCmdUpdateBuildStatus As New System.Data.SqlClient.SqlCommand
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Create Build Combo Connection not established""")
            sqlCmdUpdateBuildStatus = DB.NewSQLCommand("SQL.Query.CreateBuildCombo")
            If sqlCmdUpdateBuildStatus Is Nothing Then Throw New Exception("""Create Build Combo - Failed to create query""")
            With sqlCmdUpdateBuildStatus.Parameters
                .AddWithValue("@iBuildID", _buildid)
                .AddWithValue("@iStation", CInt(Me.lbStationValue.Text))
                .AddWithValue("@iComboTypeID", CInt(Me.lbComboTypeVal.Text))
                .AddWithValue("@iUserID", CInt(Common.GetVariable("UserID", Page).ToString()))
                .AddWithValue("@sFormula", Me.lbFormulaValue.Text)
                .AddWithValue("@sStuffingGroupID", Me.lbStuffingGroupValue.Text)
                .AddWithValue("@iStatus", 10) '10(Active)
                .AddWithValue("@LotNo", CInt(Me.lbLot.Text))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdUpdateBuildStatus.ExecuteNonQuery()

            If sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("""Create Build Combo - SQL store procedure returned value '" & sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value & "'""")
            Else
                CreateBuildCombo = True
            End If
            DB.KillSQLConnection()
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & "Please show supervisor this error for Combo Build # " & _buildid & " so they can notify the IT Department."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function UpdateBuildCombo(ByVal _buildid As Long, ByVal _status As Integer) As Boolean
        UpdateBuildCombo = False
        Dim sqlCmdUpdateBuildStatus As New System.Data.SqlClient.SqlCommand
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Update Build Combo Status Connection not established""")
            sqlCmdUpdateBuildStatus = DB.NewSQLCommand("SQL.Query.UpdateBuildStatus")
            If sqlCmdUpdateBuildStatus Is Nothing Then Throw New Exception("""Update Build Combo Status - Failed to create query""")
            With sqlCmdUpdateBuildStatus.Parameters
                .AddWithValue("@iBuildID", _buildid)
                .AddWithValue("@iStatus", _status) '10(Active),20(Filled), 90(Finished), 99(Deleted)
                .AddWithValue("@iUserID", CInt(Common.GetVariable("UserID", Page).ToString()))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdUpdateBuildStatus.ExecuteNonQuery()

            If sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("""Update Build Combo Status - SQL store procedure returned value '" & sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value & "'""")
            Else
                UpdateBuildCombo = True
            End If
            DB.KillSQLConnection()
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & "Please show supervisor this error for Combo Build # " & _buildid & ": Status =" & _status & " so they can notify the IT Department."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function CreateBuildComboRack(ByVal _buildid As Long, ByVal _rack As Long) As Boolean
        CreateBuildComboRack = False
        Dim sqlCmdUpdateBuildStatus As New System.Data.SqlClient.SqlCommand
        Me.lbError.Visible = False

        Try
            If DB.MakeSQLConnection("Warehouse") Then Throw New Exception("""Create Build Combo Rack Connection not established""")
            sqlCmdUpdateBuildStatus = DB.NewSQLCommand("SQL.Query.CreateBuildComboRack")
            If sqlCmdUpdateBuildStatus Is Nothing Then Throw New Exception("""Create Build Combo Rack - Failed to create query""")
            With sqlCmdUpdateBuildStatus.Parameters
                .AddWithValue("@iBuildID", _buildid)
                .AddWithValue("@iRack", _rack)
                .AddWithValue("@iUserID", CInt(Common.GetVariable("UserID", Page).ToString()))
                With .Add("@iErrorCode", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
            End With

            sqlCmdUpdateBuildStatus.ExecuteNonQuery()

            If sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value <> 0 Then
                Throw New Exception("""Create Build Combo Rack - SQL store procedure returned value '" & sqlCmdUpdateBuildStatus.Parameters.Item("@iErrorCode").Value & "'""")
            Else
                CreateBuildComboRack = True
            End If
            DB.KillSQLConnection()
        Catch ex As Exception
            Me.lbError.Text = "Error : " & ex.Message.ToString & "Please show supervisor this error for Combo Build # " & _buildid & " Rack # " & _rack & " so they can notify the IT Department."
            Me.lbError.Visible = True
            DB.KillSQLConnection()
        End Try

    End Function

#End Region

End Class