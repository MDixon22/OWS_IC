
Public Module SE
    Public Function NZ(ByVal s As String) As String
        Return IIf(String.IsNullOrEmpty(s), String.Empty, s)
    End Function
End Module



Public Class CaseLabel
    Public Const Fn1 As Char = "^"c

    Private _mode As Byte = 1
    Private _gtin As ULong
    Private _date As DateTime
    Private _dateAI As UShort
    Private _batch As String
    Private _serial As ULong
    Private _version As UShort
    Private _line As UShort
    Private _lot As UShort
    Private _year As UShort
    Private _pack As String
    Private _good As Boolean = False

#Region "Properties"

    Public Property GTIN() As ULong
        Get
            Return Me._gtin
        End Get
        Set(ByVal value As ULong)
            If value > 99999999999999 Then Return
            Me._gtin = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtGTIN() As String
        Get
            Return String.Format("{0:D14}", Me._gtin)
        End Get
    End Property

    Public Property CodeDate() As DateTime
        Get
            Return Me._date
        End Get
        Set(ByVal value As DateTime)
            Me._date = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtCodeDate() As String
        Get
            Return String.Format("{0:yyMMdd}", Me._date)
        End Get
    End Property

    Public Property Version() As UShort
        Get
            Return Me._version
        End Get
        Set(ByVal value As UShort)
            If value > 999 Then Return
            Me._version = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtVersion() As String
        Get
            Return String.Format("{0:D3}", Me._version)
        End Get
    End Property

    Public Property Batch() As String
        Get
            Return Me._batch
        End Get
        Set(ByVal value As String)
            Me._batch = String.Format("{0,12}", value).Replace(" "c, "0"c)
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtBatch() As String
        Get
            Return Me._batch
        End Get
    End Property

    Public Property Serial() As ULong
        Get
            Return Me._serial
        End Get
        Set(ByVal value As ULong)
            Dim comp As ULong = 99999
            If Me._mode = 2 Then comp = 9999999999
            If value > comp Then Return
            Me._serial = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtSerial() As String
        Get
            Dim fmt As String = "{0,5}"
            If Me._mode = 2 Then fmt = "{0,10}"
            Return String.Format(fmt, Me._serial).Replace(" "c, "0"c)
        End Get
    End Property

    Public Property Line() As UShort
        Get
            Return Me._line
        End Get
        Set(ByVal value As UShort)
            If value > 99 Then Return
            Me._line = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property txtLine() As String
        Get
            Return Right("0" & Me._line, 2)
        End Get
    End Property

    Public Property Year() As UShort
        Get
            Return Me._year
        End Get
        Set(ByVal value As UShort)
            If value > 9999 Or value < 10 Then Return
            Me._year = value
        End Set
    End Property

    '<System.Web.Script.Serialization.ScriptIgnore()> _
    Public ReadOnly Property strYear() As String
        Get
            Return Right(Me._year.ToString(), 2)
        End Get
    End Property

    Public Property PackCode() As String
        Get
            Return Me._pack
        End Get
        Set(ByVal value As String)
            Me._pack = value
        End Set
    End Property

    Public Property IsGood() As Boolean
        Get
            Return Me._good
        End Get
        Private Set(ByVal value As Boolean)
            Me._good = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Use for saving label data between page loads
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Serialize() As String
        'Dim js As New System.Web.Script.Serialization.JavaScriptSerializer()
        'Return js.Serialize(Me)
    End Function

    ''' <summary>
    ''' Use for restoring label data between page loads
    ''' </summary>
    ''' <param name="data">Saved data from an instance of the Serialize() method</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Deserialize(ByVal data As String) As CaseLabel
        If SE.NZ(data) = "" Then Return Nothing
        Try
            'Dim js As New System.Web.Script.Serialization.JavaScriptSerializer()
            'Return js.Deserialize(Of CaseLabel)(data)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Read in a check data from a case barcode
    ''' </summary>
    ''' <param name="barcode">Data read in from case barcode</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal barcode As String)
        Dim cmd As SqlClient.SqlCommand = Nothing
        Dim ds As DataSet = Nothing
        If SE.NZ(barcode) = "" Then Return
        ' check if barcode starts with symbol identifier
        If Not barcode.StartsWith("]C1") Then
            ' probably dealing with a case label from an incorrectly configured scanner
            barcode = "]C1" & Left(barcode, 38) & Fn1 & Right(barcode, 6)
        End If
        Try
            ' parse out the GS1 details
            cmd = DB.NewSQLCommand()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM [OWLabels].[dbo].[fnParseGS1](@barcode, N'" & CaseLabel.Fn1 & "')"
            cmd.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = barcode
            ds = DB.CreateDataSet(cmd)
            ' check for the AI's we care about
            For Each r As DataRow In ds.Tables(0).Rows
                Select Case r("AI")
                    Case 1
                        Me._gtin = ULong.Parse(r("Data"))
                        Exit Select
                    Case 13, 15
                        Me._date = DateTime.Parse(r("Data"))
                        Me._dateAI = r("AI")
                        Exit Select
                    Case 240
                        Me._version = UShort.Parse(r("Data"))
                        Exit Select
                    Case 10
                        Me._batch = r("Data")
                        Exit Select
                End Select
            Next
            ' batch details
            ds.Dispose()
            cmd.Parameters.Clear()
            cmd.CommandText = "SELECT * FROM [OWLabels].[dbo].[fnGetJobInfo](@job)"
            cmd.Parameters.Add("@job", SqlDbType.BigInt).Value = ULong.Parse(Right(Me._batch, 10))
            ds = DB.CreateDataSet(cmd)
            If IsNothing(ds) OrElse ds.Tables.Count = 0 Then
                ' some sort of query error
                Throw New Exception("Error reading job info")
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                ' not from new label program
                Me._line = UShort.Parse(Left(Me._batch, 2))
                Me._lot = UShort.Parse(Right(Me._batch, 3))
                Me._year = UShort.Parse(Left(Right(Me._batch, 5), 2))
            Else
                ' data stored from new label app
                With ds.Tables(0).Rows(0)
                    Me._line = .Item("Line")
                    Me._lot = .Item("Lot")
                    Me._pack = SE.NZ(.Item("PackCode"))
                    Dim dt As DateTime = CType(.Item("PrintAt"), DateTime)
                    Me._year = dt.Year
                    If dt.DayOfYear() < Me._lot Then Me._year -= 1
                End With
                Me._mode = 2
            End If
            Me._good = True
        Catch ex As Exception
            Me._good = False
        Finally
            If Not IsNothing(ds) Then ds.Dispose()
            If Not IsNothing(cmd) Then cmd.Dispose()
        End Try
    End Sub
End Class