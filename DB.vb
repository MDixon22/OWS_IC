Public Class DB
    Private Shared _strConn As String = Nothing
    Private Shared _sqlConn As System.Data.SqlClient.SqlConnection = Nothing
    Private Shared _sqlTran As System.Data.SqlClient.SqlTransaction = Nothing

    ''' <summary>
    ''' Creates a new SQL Server connection using a connection string specified in the web.config file.
    ''' </summary>
    ''' <param name="strConnection">Name of the connection string to use from the web.config file.</param>
    ''' <returns>0/False if connection established</returns>
    ''' <remarks></remarks>
    Public Shared Function MakeSQLConnection(ByVal strConnection As String) As Boolean
        MakeSQLConnection = True
        If strConnection Is Nothing Then Exit Function
        Dim strConn As String
        strConn = ConfigurationManager.ConnectionStrings(strConnection).ConnectionString
        If strConn Is Nothing Then Exit Function
        If DB._sqlConn Is Nothing And DB._strConn <> strConnection Then
            Try
                If strConn.IndexOf("Application Name") < 0 AndAlso Not IsNothing(System.Web.HttpContext.Current) Then
                    If Not strConn.EndsWith(";") Then strConn &= ";"
                    Dim seg As String() = System.Web.HttpContext.Current.Request.Url.Segments
                    strConn &= "Application Name=" & seg(seg.Length - 1) & ";"
                End If
                DB._sqlConn = New Data.SqlClient.SqlConnection(strConn)
                If Not IsNothing(DB._sqlConn) Then DB._sqlConn.Open()
                DB._strConn = strConnection
            Catch ex As Exception
                Debug.Print(ex.Message)
                If Not DB._sqlConn Is Nothing Then
                    DB._sqlConn.Dispose() : DB._sqlConn = Nothing
                End If
            End Try
        End If
        If Not DB._sqlConn Is Nothing Then
            MakeSQLConnection = False
        End If
    End Function

    ''' <summary>
    ''' Closes and destroys an open SQL connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub KillSQLConnection()
        If Not DB._sqlConn Is Nothing Then
            DB._sqlConn.Dispose()
            DB._sqlConn = Nothing
            DB._strConn = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Initializes and marks the beginning of a SQL transaction.
    ''' </summary>
    ''' <returns>0/False if transaction is marked successfully</returns>
    ''' <remarks></remarks>
    Public Shared Function BeginSQLTransaction() As Boolean
        BeginSQLTransaction = True
        If DB._sqlConn Is Nothing Then Exit Function
        If Not DB._sqlTran Is Nothing Then Exit Function
        Try
            DB._sqlTran = DB._sqlConn.BeginTransaction()
            If Not DB._sqlTran Is Nothing Then BeginSQLTransaction = False
        Catch ex As System.Data.SqlClient.SqlException
            Debug.Print(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Applies and closes an open SQL transaction.
    ''' </summary>
    ''' <returns>0/False if transaction is applied.</returns>
    ''' <remarks></remarks>
    Public Shared Function CommitSQLTransaction() As Boolean
        CommitSQLTransaction = True
        If DB._sqlTran Is Nothing Then Exit Function
        Try
            DB._sqlTran.Commit()
            DB._sqlTran.Dispose()
            DB._sqlTran = Nothing
            CommitSQLTransaction = False
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Cancels and destroys an open SQL transaction.
    ''' </summary>
    ''' <returns>0/False if transaction is aborted.</returns>
    ''' <remarks></remarks>
    Public Shared Function RollbackSQLTransaction() As Boolean
        RollbackSQLTransaction = True
        If DB._sqlTran Is Nothing Then Exit Function
        Try
            DB._sqlTran.Rollback()
            DB._sqlTran.Dispose()
            DB._sqlTran = Nothing
            RollbackSQLTransaction = False
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Creates a new, empty SQL command object
    ''' </summary>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function NewSQLCommand() As System.Data.SqlClient.SqlCommand
        NewSQLCommand = Nothing
        If Not DB._sqlConn Is Nothing Then
            NewSQLCommand = DB._sqlConn.CreateCommand()
            If Not DB._sqlTran Is Nothing Then NewSQLCommand.Transaction = DB._sqlTran
        End If
    End Function

    ''' <summary>
    ''' Creates a new SQL command using a query specified in the web.config file.
    ''' </summary>
    ''' <param name="strQuery">The key to use as the query for the SQL command object.</param>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function NewSQLCommand(ByVal strQuery As String) As System.Data.SqlClient.SqlCommand
        'Used when sql string is in web config
        NewSQLCommand = DB.NewSQLCommand()
        If Not NewSQLCommand Is Nothing Then
            NewSQLCommand.CommandType = CommandType.Text
            NewSQLCommand.CommandText = ConfigurationManager.AppSettings.Get(strQuery)
        End If
    End Function
    ''' <summary>
    ''' Creates a new SQL command using a query specified in the web.config file.
    ''' </summary>
    ''' <param name="strStoredProcedureName">The key to use as the query for the SQL command object.</param>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function NewSQLCommand2(ByVal strStoredProcedureName As String) As System.Data.SqlClient.SqlCommand
        'Used for command that access stored procedures on SQL
        NewSQLCommand2 = DB.NewSQLCommand()
        If Not NewSQLCommand2 Is Nothing Then
            NewSQLCommand2.CommandType = CommandType.StoredProcedure
            NewSQLCommand2.CommandText = strStoredProcedureName
        End If
    End Function
    Public Shared Function NewSQLCommand3(ByVal strQuery As String) As System.Data.SqlClient.SqlCommand
        'Used for command when building sql strings on the fly
        NewSQLCommand3 = DB.NewSQLCommand()
        If Not NewSQLCommand() Is Nothing Then
            NewSQLCommand3.CommandType = CommandType.Text
            NewSQLCommand3.CommandText = strQuery
        End If
    End Function

    ''' <summary>
    ''' Creates a dataset object to use for binding data to controls
    ''' </summary>
    ''' <param name="cmd">SQL command object to use to populate the table(s)</param>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateDataSet(ByVal cmd As Data.SqlClient.SqlCommand) As Data.DataSet
        Dim ds As Data.DataSet = Nothing
        Dim x As Integer = 0
        If cmd Is Nothing Then Return Nothing
        Try
            Using adp As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter(cmd)
                ds = New Data.DataSet()
                If adp.Fill(ds) < 0 Then Return Nothing
            End Using
            For Each dt As DataTable In ds.Tables
                If SE.NZ(dt.TableName) = "" Then dt.TableName = "Table" & x
                x += 1
            Next
        Catch ex As Exception
            If Not IsNothing(ds) Then ds.Dispose()
            Return Nothing
        End Try
        Return ds
    End Function

    ''' <summary>
    ''' Creates a dataview object to use for binding data to controls
    ''' </summary>
    ''' <param name="sqlCommand">SQL command object to use to populate the table</param>
    ''' <returns>Nothing on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateDataView(ByRef sqlCommand As Data.SqlClient.SqlCommand) As Data.DataView
        Dim dsSet As Data.DataSet
        Dim dvView As Data.DataView
        CreateDataView = Nothing
        dsSet = CreateDataSet(sqlCommand)
        If dsSet Is Nothing Then Exit Function
        dvView = New Data.DataView(dsSet.Tables(0), _
                              Nothing, Nothing, _
                              Data.DataViewRowState.CurrentRows)
        If dvView Is Nothing Then
            dsSet.Dispose() : dsSet = Nothing
            CreateDataView = Nothing : Exit Function
        End If
        dsSet.Dispose() : dsSet = Nothing
        CreateDataView = dvView
    End Function

    Public Shared Function GetDataSet(ByRef sqlCommand As Data.SqlClient.SqlCommand) As Data.DataSet
        Dim sqlAdapter As Data.SqlClient.SqlDataAdapter
        Dim dsSet As Data.DataSet
        GetDataSet = Nothing
        If (sqlCommand Is Nothing) Then Exit Function
        sqlAdapter = New Data.SqlClient.SqlDataAdapter(sqlCommand)
        If sqlAdapter Is Nothing Then
            sqlCommand.Dispose() : sqlCommand = Nothing
            Exit Function
        End If
        dsSet = New Data.DataSet
        If dsSet Is Nothing Then
            sqlAdapter.Dispose() : sqlAdapter = Nothing
            sqlCommand.Dispose() : sqlCommand = Nothing
            Exit Function
        End If
        If sqlAdapter.Fill(dsSet) > -1 Then
            GetDataSet = dsSet
        Else
            dsSet.Dispose() : dsSet = Nothing
        End If
        sqlAdapter.Dispose() : sqlAdapter = Nothing
    End Function

    ''' <summary>
    ''' Binds a table to use as the entries in a combobox control.
    ''' </summary>
    ''' <param name="sqlCommand">SQL command object to query for the data.</param>
    ''' <param name="ctlList">ComboBox control to populate with the data.</param>
    ''' <param name="strValueColumn">The column used for the value of each entry.</param>
    ''' <param name="strTextColumn">The column used for the text/description of each entry.</param>
    ''' <returns>True on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function BindDropDownData(ByRef sqlCommand As Data.SqlClient.SqlCommand, _
                                      ByRef ctlList As System.Web.UI.WebControls.ListControl, _
                                      ByRef strValueColumn As String, _
                                      Optional ByRef strTextColumn As String = "") As Boolean
        Dim dvView As Data.DataView
        BindDropDownData = True
        If sqlCommand Is Nothing Then Exit Function
        If ctlList Is Nothing Or strValueColumn Is Nothing Then Exit Function
        dvView = CreateDataView(sqlCommand)
        If dvView Is Nothing Then
            Exit Function
        End If
        If dvView.Count() < 1 Then
            Exit Function
        End If
        If strValueColumn.Length < 1 Then strTextColumn = strValueColumn
        ctlList.DataSource = dvView
        ctlList.DataValueField() = strValueColumn
        ctlList.DataTextField() = strTextColumn
        ctlList.DataBind()
        dvView.Dispose() : dvView = Nothing
        BindDropDownData = False
    End Function

    ''' <summary>
    ''' Fill a generic list control with data from a database query.
    ''' </summary>
    ''' <param name="sqlCommand">SQL command object used to query for data.</param>
    ''' <param name="ctlList">List control to populate with query data.</param>
    ''' <param name="strValueColumn">The column used for the value of each entry.</param>
    ''' <param name="strTextColumn">The column used for the text/description of each entry.</param>
    ''' <param name="boolAll">Should a default/all entry be added.</param>
    ''' <param name="allValue">Value used for the default/all entry.</param>
    ''' <param name="allText">Text used for the default/all entry.</param>
    ''' <returns>True on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function FillList(ByRef sqlCommand As Data.SqlClient.SqlCommand, _
                                   ByRef ctlList As System.Web.UI.WebControls.ListControl, _
                                   ByRef strValueColumn As String, _
                                   Optional ByRef strTextColumn As String = "", _
                                   Optional ByVal boolAll As Boolean = False, _
                                   Optional ByRef allValue As Object = Nothing, _
                                   Optional ByRef allText As String = "All") As Boolean
        FillList = True
        If BindDropDownData(sqlCommand, ctlList, strValueColumn, strTextColumn) Then Exit Function
        If boolAll <> False Then
            Dim itmAll As System.Web.UI.WebControls.ListItem
            itmAll = New System.Web.UI.WebControls.ListItem(allText, _
                                                            allValue.ToString())
            If itmAll Is Nothing Then Exit Function
            ctlList.Items.Insert(0, itmAll)
            itmAll = Nothing
        End If
        FillList = False
    End Function

    ''' <summary>
    ''' Populates a generic data view object with a database query's data.
    ''' </summary>
    ''' <param name="sqlCommand">SQL command object used to query for data.</param>
    ''' <param name="ctlDataList">Data list control to populate with query data.</param>
    ''' <returns>True on an error.</returns>
    ''' <remarks></remarks>
    Private Shared Function BindDataListData(ByRef sqlCommand As Data.SqlClient.SqlCommand, _
                                             ByRef ctlDataList As System.Web.UI.WebControls.BaseDataList) As Boolean
        Dim dvView As Data.DataView
        BindDataListData = True
        If ctlDataList Is Nothing Then Exit Function
        dvView = CreateDataView(sqlCommand)
        If dvView Is Nothing Then Exit Function
        If dvView.Count() < 1 Then Exit Function
        ctlDataList.DataSource = dvView
        ctlDataList.DataBind()
        dvView.Dispose() : dvView = Nothing
        BindDataListData = False
    End Function

    ''' <summary>
    ''' Clears out and fills in a datagrid object with a query.
    ''' </summary>
    ''' <param name="sqlCommand">SQL command object used to query for data.</param>
    ''' <param name="ctlDataGrid">Datagrid object to fill in.</param>
    ''' <returns>True on an error.</returns>
    ''' <remarks></remarks>
    Public Shared Function FillDataGrid(ByRef sqlCommand As Data.SqlClient.SqlCommand, _
                                        ByRef ctlDataGrid As System.Web.UI.WebControls.DataGrid) As Boolean
        FillDataGrid = True
        ctlDataGrid.DataSource = Nothing
        ctlDataGrid.DataBind()
        If BindDataListData(sqlCommand, ctlDataGrid) Then Exit Function
        FillDataGrid = False
    End Function

    Private Shared Function BooleanStringToBit(ByVal strBoolean As String) As Integer
        BooleanStringToBit = 0
        If strBoolean = Boolean.TrueString Then BooleanStringToBit = 1
    End Function

    ''' <summary>
    ''' Write out the transaction records to be processed on the SQL server.
    ''' </summary>
    ''' <param name="uiPage">Web page object holding session state variables.</param>
    ''' <param name="strParams">Parameters needed to process the transaction.</param>
    ''' <param name="strValues">Values to the parameters needed to process the transaction.</param>
    ''' <param name="intTransType">The transaction application code.</param>
    ''' <param name="intTransSubType">The transaction operation type.</param>
    ''' <param name="intTransRevType">The transaction processing level.</param>
    ''' <returns>True on an error.</returns>
    ''' <remarks></remarks>
    Private Shared Function WriteTransaction(ByRef uiPage As System.Web.UI.Page, _
                                             ByRef strParams() As String, _
                                             ByRef strValues() As String, _
                                             ByVal intTransType As Integer, _
                                             ByVal intTransSubType As Integer, _
                                             ByVal intTransRevType As Integer) As Boolean
        WriteTransaction = True
        Try
            If Not DB.MakeSQLConnection("Transactions") Then
                If Not DB.BeginSQLTransaction() Then
                    Dim guidXID As Guid
                    Dim sqlCmd As System.Data.SqlClient.SqlCommand = DB.NewSQLCommand("SQL.Query.Transaction.Parameter")
                    If Not sqlCmd Is Nothing Then
                        Dim intParam As Integer
                        guidXID = Guid.NewGuid()
                        sqlCmd.Parameters.AddWithValue("@XID", guidXID)
                        sqlCmd.Parameters.AddWithValue("@PRM", "UserID")
                        sqlCmd.Parameters.AddWithValue("@VAL", Common.GetVariable("Employee", uiPage))
                        sqlCmd.Parameters.AddWithValue("@TYP", intTransType)
                        If sqlCmd.ExecuteNonQuery() <> 1 Then Throw New Exception("Failed to create user parameter")
                        sqlCmd.Parameters("@PRM").Value = "Location"
                        sqlCmd.Parameters("@VAL").Value = Common.GetVariable("Location", uiPage)
                        If sqlCmd.ExecuteNonQuery() <> 1 Then Throw New Exception("Failed to create location parameter")
                        For intParam = 0 To strParams.Length - 1
                            sqlCmd.Parameters("@PRM").Value = strParams(intParam)
                            sqlCmd.Parameters("@VAL").Value = strValues(intParam)
                            If sqlCmd.ExecuteNonQuery <> 1 Then Throw New Exception("Failed to create " + strParams(intParam) + " parameter")
                        Next intParam
                        sqlCmd.Dispose() : sqlCmd = Nothing
                        sqlCmd = DB.NewSQLCommand("SQL.Query.Transaction.Header")
                        If Not sqlCmd Is Nothing Then
                            sqlCmd.Parameters.AddWithValue("@XID", guidXID)
                            sqlCmd.Parameters.AddWithValue("@TYP", intTransType)
                            sqlCmd.Parameters.AddWithValue("@SUB", intTransSubType)
                            sqlCmd.Parameters.AddWithValue("@REV", intTransRevType)
                            sqlCmd.Parameters.AddWithValue("@NUM", strParams.Length + 2)
                            If sqlCmd.ExecuteNonQuery() <> 1 Then Throw New Exception("Failed to create transaction header")
                            sqlCmd.Dispose() : sqlCmd = Nothing
                            DB.CommitSQLTransaction()
                            WriteTransaction = False
                        End If
                    End If
                End If
                DB.KillSQLConnection()
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            DB.RollbackSQLTransaction()
            DB.KillSQLConnection()
        End Try
    End Function

    '''' <summary>
    '''' Writes the transaction to move a basket from the cooler to a line.
    '''' </summary>
    '''' <param name="uiPage">Web page object holding the session state variables.</param>
    '''' <param name="strBasketID">The basket number to move out.</param>
    '''' <param name="strLineNum">The line number to move the basket to.</param>
    '''' <returns>True on an error.</returns>
    '''' <remarks></remarks>
    'Public Shared Function MoveBasket2Line(ByRef uiPage As System.Web.UI.Page, _
    '                                       ByVal strBasketID As String, _
    '                                       ByVal strLineNum As String, _
    '                                       ByVal strOverride As String) As Boolean
    '    Dim strParams() As String = {"BasketID", "LineNumber", "Override"}
    '    Dim strValues() As String = {strBasketID, strLineNum, DB.BooleanStringToBit(strOverride)}
    '    MoveBasket2Line = DB.WriteTransaction(uiPage, strParams, strValues, ConfigurationManager.AppSettings("SQL.Transaction.Type"), _
    '        ConfigurationManager.AppSettings("SQL.Transaction.SubType.Update"), ConfigurationManager.AppSettings("SQL.Transaction.RevType.Update.Basket"))
    'End Function

    '''' <summary>
    '''' Writes the transaction to move a basket from 1 line to another.
    '''' </summary>
    '''' <param name="uiPage">Web page object holding the session state variables.</param>
    '''' <param name="strBasketID">The basket number to move over.</param>
    '''' <param name="strLineNum">The line number to move the basket to.</param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Shared Function MoveLine2Line(ByRef uiPage As System.Web.UI.Page, _
    '                                     ByVal strBasketID As String, _
    '                                     ByVal strLineNum As String, _
    '                                     ByVal strOverride As String) As Boolean
    '    Dim strParams() As String = {"BasketID", "LineNumber", "Override"}
    '    Dim strValues() As String = {strBasketID, strLineNum, DB.BooleanStringToBit(strOverride)}
    '    MoveLine2Line = DB.WriteTransaction(uiPage, strParams, strValues, ConfigurationManager.AppSettings("SQL.Transaction.Type"), _
    '        ConfigurationManager.AppSettings("SQL.Transaction.SubType.Update"), ConfigurationManager.AppSettings("SQL.Transaction.RevType.Update.Line"))
    'End Function

    'Public Shared Function MoveBasket2Truck(ByRef uiPage As System.Web.UI.Page, _
    '                                        ByVal strBasketID As String, _
    '                                        ByVal strWarehouse As String, _
    '                                        ByVal strOverride As String) As Boolean
    '    Dim strParams() As String = {"BasketID", "Warehouse", "Override"}
    '    Dim strValues() As String = {strBasketID, strWarehouse, DB.BooleanStringToBit(strOverride)}
    '    MoveBasket2Truck = DB.WriteTransaction(uiPage, strParams, strValues, ConfigurationManager.AppSettings("SQL.Transaction.Type"), _
    '        ConfigurationManager.AppSettings("SQL.Transaction.SubType.Update"), ConfigurationManager.AppSettings("SQL.Transaction.RevType.Update.Transfer"))
    'End Function

    'Public Shared Function MoveTruck2Basket(ByRef uiPage As System.Web.UI.Page, _
    '                                        ByVal strBasketID As String, _
    '                                        ByVal strWarehouse As String)
    '    Dim strParams() As String = {"BasketID", "Warehouse"}
    '    Dim strValues() As String = {strBasketID, strWarehouse}
    '    MoveTruck2Basket = DB.WriteTransaction(uiPage, strParams, strValues, ConfigurationManager.AppSettings("SQL.Transaction.Type"), _
    '        ConfigurationManager.AppSettings("SQL.Transaction.SubType.Update"), ConfigurationManager.AppSettings("SQL.Transaction.RevType.Update.Receive"))
    'End Function
End Class
