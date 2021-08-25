Imports System
Imports System.IO
Imports System.IO.File
Partial Public Class IC_AdjPalletQty
    Inherits System.Web.UI.Page

    Public _printer As String
    Public _trantype As String
    Public _datemodified As DateTime
    Public _dateentered As DateTime
    Public _whs As String
    Public _company As String
    Public _pallet As String
    Public _component As String
    Public _status As String
    Public strURL As String = Nothing

    'Private sConnString As String = "Server=192.168.5.4;Initial Catalog=OWS;Trusted_Connection=Yes;connect timeout=10;Persist Security Info=True"
    'Private sConnString As String = "Server=SRV06;Initial Catalog=OWS;Trusted_Connection=Yes;connect timeout=10;Persist Security Info=True"
    Private sConnString As String = ConfigurationManager.ConnectionStrings("Warehouse").ConnectionString
    Private sqlConn As New SqlClient.SqlConnection(sConnString)

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Reload values entered back to screen before reposting the page to scanner
        Me.txPallet.Text = Common.GetVariable("Pallet", Page)
        Me.txQuantity.Text = Common.GetVariable("NewQty", Page)
        Me.txPrinter.Text = Common.GetVariable("Printer", Page)
        Me.lbCurrent.Text = "Curr Qty - " & Common.GetVariable("Qty", Page) & " - "

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

        If Not Page.IsPostBack Then
            Call InitProcess() 'Setting up screen for start of Pallet
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Else 'Page is posted back 
            'Enter Key was pressed, without an entry, while inside the Pallet TextBox 
            'Prompt Error and Set Focus to Pallet TextBox
            If Me.lbProdID_ProdDesc.Visible = False And RTrim(Me.txPallet.Text).Length < 1 Then
                Me.lbError.Text = "Pallet # needs to be entered."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
            End If
        End If
    End Sub

    Public Sub InitSessVar()
        'Clear Session variables for this page
        Common.SaveVariable("Pallet", "", Page)
        Common.SaveVariable("GTIN", "", Page)
        Common.SaveVariable("ProdID", "", Page)
        Common.SaveVariable("ProdVariant", "", Page)
        Common.SaveVariable("ProdDesc", "", Page)
        Common.SaveVariable("Qty", "", Page)
        Common.SaveVariable("NewQty", "", Page)
        Common.SaveVariable("CodeDate", "", Page)
        Common.SaveVariable("Lot", "", Page)
        Common.SaveVariable("Code", "", Page)
        Common.SaveVariable("Printer", "", Page)
        Common.SaveVariable("newURL", Nothing, Page)
    End Sub

    Public Sub InitProcess()
        Call InitSessVar()

        Me.lbCurrent.Visible = False
        Me.lbProdID_ProdDesc.Visible = False
        Me.txQuantity.Visible = False
        Me.lbQuantity.Visible = False
        Me.txPrinter.Visible = False
        Me.lbPrinter.Visible = False

        'Clear Member variables for this page
        _pallet = Nothing
        _status = Nothing
        _printer = Nothing
        _trantype = Nothing
        _dateentered = Nothing
        _datemodified = Nothing
        _component = Nothing


        Me.lbError.Text = ""
        Me.lbError.Visible = False

        Me.lbPrompt.Text = "Scan or Enter Pallet#"
        Common.JavaScriptSetFocus(Page, Me.txPallet)

    End Sub

    Protected Sub txPallet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPallet.TextChanged
        'Dim dsPallet As New Data.DataSet
        'Dim sqlCmdPallet As New System.Data.SqlClient.SqlCommand

        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Dim iPallet As Long

            'get valid pallet lengths from web config

            If RTrim(Me.txPallet.Text).Length > 0 Then
                'Pallet # entered is valid length check numeric
                If IsNumeric(Me.txPallet.Text) = True Then
                    iPallet = CLng(Me.txPallet.Text)
                Else
                    Me.lbError.Text = "Pallet # needs to be numbers."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPallet)
                    Exit Sub
                End If
            Else
                Common.SaveVariable("Pallet", "", Page)
                Me.lbError.Text = "Pallet # can not be blank, try again."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPallet)
                Exit Sub
            End If

            If PalletExist(iPallet) = False Then
                lbError.Text = "PalletID is invalid"
                lbError.Visible = True
                Exit Sub
            End If

            'Good Pallet, get the product
            If GetPalletProduct(iPallet) = False Then
                lbError.Text = "Error occurred during Pallet Product Lookup - Press Restart Entry button to try again, or bring scanner to supervisor."
                lbError.Visible = True
                Exit Sub
            End If

            'Good Pallet and Product
            Common.SaveVariable("Pallet", Me.txPallet.Text, Page)
            'Me.lbCurrent.Text = "Current Qty - " & Common.GetVariable("Qty", Page)
            Me.lbCurrent.Visible = True
            Me.lbProdID_ProdDesc.Text = Common.GetVariable("ProdID", Page) & "-" & Common.GetVariable("ProdDesc", Page)
            Me.lbProdID_ProdDesc.Visible = True
            Me.lbQuantity.Visible = True
            Me.txQuantity.Visible = True
            Me.lbPrompt.Text = "Enter New Total Quantity for Pallet"
            Common.JavaScriptSetFocus(Page, Me.txQuantity)

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while connecting to database to validate Pallet entered! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPallet)
        Finally
            DB.KillSQLConnection()
        End Try
    End Sub

    Protected Sub txQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txQuantity.TextChanged
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False
            Common.SaveVariable("NewQty", "", Page)

            If Me.txQuantity.Text.Length < 1 Then
                Me.lbError.Text = "Quantity Entry is required"
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txQuantity)
                Exit Sub
            End If

            If IsNumeric(Me.txQuantity.Text) = False Then
                Me.lbError.Text = ""
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txQuantity)
                Exit Sub
            End If

            'Quantity passed all test - proceeed to Printer Entry
            Common.SaveVariable("NewQty", Me.txQuantity.Text, Page)
            Me.lbPrinter.Visible = True
            Me.txPrinter.Visible = True

            Me.lbPrompt.Text = "Enter Printer #"
            Common.JavaScriptSetFocus(Page, Me.txPrinter)

        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Quantity entered! Check battery and wireless connection - then return to menu and try again."
            Me.lbError.Visible = True
            Common.SaveVariable("NewQty", "", Page)
            Common.JavaScriptSetFocus(Page, Me.txQuantity)
        End Try
    End Sub

    Protected Sub txPrinter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txPrinter.TextChanged
        Dim sqlCmdPrinter As New System.Data.SqlClient.SqlCommand
        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
        Try
            Me.lbError.Text = ""
            Me.lbError.Visible = False

            'Verify printer # 
            If Me.txPrinter.Text.Length < 1 Then
                Me.lbError.Text = "Printer Entry is required"
                Me.lbError.Visible = True
                Exit Sub
            End If

            If IsNumeric(Me.txPrinter.Text) = False Then
                Me.lbError.Text = "Printer Entry must be a number"
                Me.lbError.Visible = True
                Exit Sub
            End If

            Dim iPrt As Integer = CInt(Me.txPrinter.Text)

            If Not DB.MakeSQLConnection("Warehouse") Then
                sqlCmdPrinter = DB.NewSQLCommand("SQL.Query.PrinterLookup")
                If Not sqlCmdPrinter Is Nothing Then
                    sqlCmdPrinter.Parameters.AddWithValue("@Printer", iPrt)
                    _printer = sqlCmdPrinter.ExecuteScalar()
                    sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
                    DB.KillSQLConnection()

                    If RTrim(_printer).Length < 1 Then 'Printer does not exist in database
                        Me.lbError.Text = "Invalid Printer # entered. Try again."
                        Me.lbError.Visible = True
                        'Save the bad Bin Location to display on screen for correction
                        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                        Common.JavaScriptSetFocus(Page, Me.txPrinter)
                    Else 'Printer is valid
                        Common.SaveVariable("Printer", Me.txPrinter.Text, Page)
                        If AdjustPallet(Common.GetVariable("Pallet", Page).ToString, Common.GetVariable("NewQty", Page).ToString) = True Then
                            If PrintPalletTag() = False Then
                                Me.lbError.Text = "Transaction Complete but failed to print new Pallet Tag. Do again."
                                Me.lbError.Visible = True
                                Common.JavaScriptSetFocus(Page, Me.txPrinter)
                            Else
                                Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page) 'new
                            End If
                        Else
                            Me.lbError.Text = "Transaction failed to complete. Do again."
                            Me.lbError.Visible = True
                            Common.JavaScriptSetFocus(Page, Me.txPrinter)
                        End If
                    End If
                Else
                    Me.lbError.Text = "Command Error occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
                    Me.lbError.Visible = True
                    Common.JavaScriptSetFocus(Page, Me.txPrinter)
                End If
            Else
                Me.lbError.Text = "Communication Error occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
                Me.lbError.Visible = True
                Common.JavaScriptSetFocus(Page, Me.txPrinter)
            End If
            
        Catch ex As Exception
            Me.lbError.Text = "Exception Error - " & ex.Message.ToString & " - occurred while validating Printer entry! Check battery and Wireless connection - then try again or see your supervisor."
            Me.lbError.Visible = True
            Common.JavaScriptSetFocus(Page, Me.txPrinter)
        Finally
            If Not sqlCmdPrinter Is Nothing Then
                sqlCmdPrinter.Dispose() : sqlCmdPrinter = Nothing
            End If
            DB.KillSQLConnection()
        End Try
    End Sub

    Public Function PalletExist(ByVal _pallet As Long) As Boolean
        PalletExist = False

        Try
            Dim sqlString As String
            Dim sqlDAProdData As New SqlClient.SqlDataAdapter
            Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
            Dim ds As New DataSet
            Dim dv As New DataView

            sqlString = "Select * from vwIC_InventoryPallets Where pk_nPallet = " & _pallet
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Dupes")
            dv = New DataView(ds.Tables("Dupes"), Nothing, Nothing, DataViewRowState.CurrentRows)

            If dv.Count <> 0 Then
                PalletExist = True
            Else
                PalletExist = False
            End If

        Catch ex As Exception
            'End and dispose an active connection if it exists to avoid error
            DB.KillSQLConnection()
        End Try

    End Function

    Public Function GetPalletProduct(ByVal _pallet As Long) As Boolean
        Dim sqlString As String
        Dim sqlDAProdData As New SqlClient.SqlDataAdapter
        Dim sqlCommandBuilder As SqlClient.SqlCommandBuilder
        Dim ds As New DataSet
        Dim dv As New DataView

        GetPalletProduct = False

        Try
            sqlString = "Select * from vwICPallets_ForQtyAdjust Where Pallet = " & _pallet
            sqlDAProdData = New SqlClient.SqlDataAdapter(sqlString, sqlConn)
            sqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqlDAProdData)
            sqlDAProdData.Fill(ds, "Data")

            Common.SaveVariable("GTIN", ds.Tables("Data").Rows(0).Item("GTIN"), Page)
            Common.SaveVariable("ProdID", ds.Tables("Data").Rows(0).Item("ProdID"), Page)
            Common.SaveVariable("ProdVariant", ds.Tables("Data").Rows(0).Item("ProdVariant"), Page)
            Common.SaveVariable("ProdDesc", ds.Tables("Data").Rows(0).Item("ProdDesc"), Page)
            Common.SaveVariable("Qty", ds.Tables("Data").Rows(0).Item("Qty"), Page)
            Common.SaveVariable("CodeDate", ds.Tables("Data").Rows(0).Item("CodeDate"), Page)
            Common.SaveVariable("Lot", Right(ds.Tables("Data").Rows(0).Item("Lot"), 3), Page)
            If IsDBNull(ds.Tables("Data").Rows(0).Item("Code")) = True Or UCase(RTrim(ds.Tables("Data").Rows(0).Item("Code"))) = "BRAND" Then
                Common.SaveVariable("Code", " ", Page)
            Else
                Common.SaveVariable("Code", " - " & RTrim(ds.Tables("Data").Rows(0).Item("Code")), Page)
            End If

            GetPalletProduct = True
        Catch ex As Exception
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function AdjustPallet(ByVal _pallet As Long, ByVal _qty As Integer) As Boolean
        AdjustPallet = False

        Dim cmdSqlCommand As New SqlClient.SqlCommand
        Dim sSqlString As String

        Try
            sqlConn.Open()
            'sSqlString = "spPalletAdjust "
            'Mod - SAM 3/6/2019
            sSqlString = "spPalletAdjust "
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
            DB.KillSQLConnection()
        End Try
    End Function

    Public Function PrintPalletTag() As Boolean
        Dim destPath As String = ""
        Dim tempPath As String = ""
        Dim filenm As String = ""
        Dim PathFile As String = ""
        Dim sourceFile As String = ""
        Dim successfile As String = ""
        Dim destinationFile As String = ""

        PrintPalletTag = False

        Try
            If RTrim(Me.txPrinter.Text) <> "0" Then         'Printer "0" is a skip printing function used by OWS 
                'Drop file into Loftware program for printing of pallet labels
                Dim strLine1 As String = "*FORMAT,RDC_INTELLILABELOWS2"
                Dim strLine2 As String = "HR_Pallet," & Trim(Me.txPallet.Text)
                Dim strLine3 As String = "BC_Pallet," & Trim(Me.txPallet.Text)
                Dim strLine4 As String = "HR_GTIN," & Trim(Common.GetVariable("GTIN", Page).ToString)
                Dim strLine5 As String = "HR_ProdID_Ver," & Trim(Common.GetVariable("ProdID", Page).ToString) & " - " & Trim(Common.GetVariable("ProdVariant", Page).ToString)
                Dim strLine6 As String = "HR_ProdDesc," & Trim(Common.GetVariable("ProdDesc", Page).ToString) & Trim(Common.GetVariable("Code", Page).ToString)
                Dim strLine7 As String = "HR_Quantity," & Trim(Me.txQuantity.Text)
                Dim strLine8 As String = "HR_CodeDate," & Trim(Common.GetVariable("CodeDate", Page).ToString)
                Dim strLine9 As String = "HR_Lot," & Trim(Common.GetVariable("Lot", Page).ToString)
                Dim strLine10 As String = "*QUANTITY,1"
                Dim strLine11 As String = "*PRINTERNUMBER," & Trim(Me.txPrinter.Text)
                Dim strLine12 As String = "*PRINTLABEL"

                destPath = "\\192.168.5.4\wddrop\"
                'destPath = ConfigurationManager.AppSettings.Get("LW.DestPath").ToString
                tempPath = "\\192.168.5.4\wddroptemp\"
                'tempPath = ConfigurationManager.AppSettings.Get("LW.TempDestPath").ToString

                filenm = Trim(_pallet) & DatePart("yyyy", Now) & DatePart("m", Now) & DatePart("d", Now) & DatePart("h", Now) & DatePart("n", Now) & DatePart("s", Now) & ".pas"
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
                s.WriteLine(strLine6)
                s.WriteLine(strLine7)
                s.WriteLine(strLine8)
                s.WriteLine(strLine9)
                s.WriteLine(strLine10)
                s.WriteLine(strLine11)
                s.WriteLine(strLine12)
                s.Close()

                sourceFile = tempPath & filenm
                successfile = tempPath & filenm & ".success"
                destinationFile = destPath & filenm

                'Delete destinationFile for overwrite,
                'causes exception if already exists.
                File.Delete(destinationFile)
                File.Copy(sourceFile, destinationFile)

                File.Copy(sourceFile, successfile)
                File.Delete(sourceFile)

                PrintPalletTag = True
            End If
        Catch ex As Exception
            Dim _exMessageText As String = ex.Message.ToString & " while dropping file to print. "
            Me.lbError.Text = _exMessageText
        End Try
    End Function

    Protected Sub btReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btReturn.Click
        'Return to the Main Menu
        Common.SaveVariable("newURL", "~/IC_Menu.aspx", Page) 'new
    End Sub

    Protected Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Reset screen values to default and start process over
        Call InitProcess()
    End Sub
End Class