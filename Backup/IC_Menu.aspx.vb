Partial Public Class IC_Menu
    Inherits System.Web.UI.Page

    Public userspecial As String = "N"
    Public department As String = Nothing
    Public strURL As String = Nothing
    Public _whs As String = ""

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
        _whs = Common.GetVariable("Whse", Page).ToString
        If Common.GetVariable("Department", Page) = "Smoking" Then
            Me.lbPageTitle.Text = "OW Smoking Process - Menu"
        ElseIf Common.GetVariable("Department", Page) = "Combos" Then
            Me.lbPageTitle.Text = "OW CutDown Combos Process - Menu"
        Else
            Me.lbPageTitle.Text = "OW Inventory - Menu"
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
        department = Common.GetVariable("Department", Page)

        Select Case department
            Case "Combos"
                Me.lbFGPutaway.Text = "1. Cut Down Combos Menu"                                ' 1. Process Racks 
                Me.lbBinTransfer.Text = ""
                Me.lbShippingPallet.Text = ""
                Me.lbWorkWithShippingPallet.Text = ""
                Me.lbWhseTransfer.Text = ""
                Me.lbAdjPalletQty.Text = ""
                Me.lbVoidPallet.Text = ""
                Me.lbNumberTwo.Text = ""
                Me.lbTestNewFunction.Text = ""
                Me.lbReAssignPallet.Text = ""
                Me.lbNewNumTwoProcess.Text = ""
                Me.lbPacklandNumber2.Text = ""
                Me.lbCreateSamplePallet.Text = ""
                Me.lbWorkWithSamplePallet.Text = ""
                Me.lbRecvAbbylandPallet.Text = ""
                Me.lbComboMenu.Text = ""
                Me.lbWrkShippingPallet.Text = ""
                Me.lbNewCrtSamplePallet.Text = ""
                Me.lbNewWrkSamplePallet.Text = ""
            Case "Smoking"
                'Set Menu for Slicing Department
                Me.lbFGPutaway.Text = "1. Process Racks"                                ' 1. Process Racks 
                Me.lbBinTransfer.Text = ""
                Me.lbShippingPallet.Text = ""
                Me.lbWorkWithShippingPallet.Text = ""
                Me.lbWhseTransfer.Text = ""
                Me.lbAdjPalletQty.Text = ""
                Me.lbVoidPallet.Text = ""
                Me.lbNumberTwo.Text = ""
                Me.lbTestNewFunction.Text = ""
                Me.lbReAssignPallet.Text = ""
                Me.lbNewNumTwoProcess.Text = ""
                Me.lbPacklandNumber2.Text = ""
                Me.lbCreateSamplePallet.Text = ""
                Me.lbWorkWithSamplePallet.Text = ""
                Me.lbRecvAbbylandPallet.Text = ""
                Me.lbComboMenu.Text = ""
                Me.lbWrkShippingPallet.Text = ""
                Me.lbNewCrtSamplePallet.Text = ""
                Me.lbNewWrkSamplePallet.Text = ""

            Case "Slicing"
                If _whs = "17" Then
                    'Set Menu for Slicing Department
                    Me.lbFGPutaway.Text = "1. FG Putaway OWS Mont"                          ' 1. FG Putaway HF
                    Me.lbBinTransfer.Text = "2. Bin Transfer"                               ' 2. Bin Transfer
                    Me.lbShippingPallet.Text = ""                                           ' 3. Changing Pallet status to Q = QC Hold
                    Me.lbWorkWithShippingPallet.Text = ""                                   ' 4. WHSE Transfer
                    Me.lbWhseTransfer.Text = "5. Stock Pallet Transfer"                     ' 5. Stock Pallet Transfer
                    Me.lbAdjPalletQty.Text = "6. Adj. Production Pallet Qty"                ' 6. Adjust qty at Production wrapper
                    Me.lbVoidPallet.Text = ""                                               ' 7.
                    Me.lbNumberTwo.Text = ""                                                ' 8. Number 2 Process - Old
                    Me.lbTestNewFunction.Text = ""                                          ' 9.
                    Me.lbReAssignPallet.Text = ""                                           '10. Load Pallet - Packland Trk
                    Me.lbNewNumTwoProcess.Text = "11. New Number 2 Process"                 '11. New Number 2 Process
                    Me.lbPacklandNumber2.Text = ""                                          '12. Packland Number 2
                    Me.lbCreateSamplePallet.Text = ""                                       '13. Create Sample Pallet
                    Me.lbWorkWithSamplePallet.Text = ""                                     '14. Work With Sample Pallets
                    Me.lbRecvAbbylandPallet.Text = "15. FG Putaway 9798"                      '15. Recv Abbyland Pallet
                    Me.lbComboMenu.Text = ""                                                '16. Cut-Down Combo Menu
                    Me.lbWrkShippingPallet.Text = ""                                        '17.
                    Me.lbNewCrtSamplePallet.Text = ""                                       '18.
                    Me.lbNewWrkSamplePallet.Text = ""                                       '19.
                Else
                    If Trim(Common.GetVariable("User", Page).ToString) = "WCFG" Then
                        'Set Menu for Slicing Department
                        Me.lbFGPutaway.Text = "1. FG Putaway"                                   ' 1. FG Putaway
                        Me.lbBinTransfer.Text = "2. Bin Transfer"                               ' 2. Bin Transfer
                        Me.lbShippingPallet.Text = "3. QC Hold Pallet"                          ' 3. Changing Pallet status to Q = QC Hold
                        Me.lbWorkWithShippingPallet.Text = ""                                   ' 4. WHSE Transfer
                        Me.lbWhseTransfer.Text = ""                                             ' 5. 
                        Me.lbAdjPalletQty.Text = "6. Adj. Production Pallet Qty"                ' 6. Adjust qty at Production wrapper
                        Me.lbVoidPallet.Text = ""                                               ' 7.
                        Me.lbNumberTwo.Text = "8. Simms Repack Compnt"                            ' 8. Repack Components
                        Me.lbTestNewFunction.Text = "9. Simms FG Putaway"                         ' 9. Repack Putaway
                        Me.lbReAssignPallet.Text = ""                                           '10. Load Pallet - Packland Trk
                        Me.lbNewNumTwoProcess.Text = "11. New Number 2 Process"                 '11. New Number 2 Process
                        Me.lbPacklandNumber2.Text = ""                                          '12. 
                        Me.lbCreateSamplePallet.Text = ""                                       '13. Create Sample Pallet
                        Me.lbWorkWithSamplePallet.Text = ""                                     '14. Work With Sample Pallets
                        Me.lbRecvAbbylandPallet.Text = "15. FG Putaway HF"                      '15. Recv Abbyland Pallet
                        Me.lbComboMenu.Text = "16. Cut-Down Combo Menu"                         '16. Cut-Down Combo Menu
                        Me.lbWrkShippingPallet.Text = ""                                        '17.
                        Me.lbNewCrtSamplePallet.Text = ""                                       '18.
                        Me.lbNewWrkSamplePallet.Text = ""                                       '19.
                    Else
                        'Set Menu for Slicing Department
                        Me.lbFGPutaway.Text = "1. FG Putaway"                                   ' 1. FG Putaway
                        Me.lbBinTransfer.Text = "2. Bin Transfer"                               ' 2. Bin Transfer
                        Me.lbShippingPallet.Text = "3. QC Hold Pallet"                          ' 3. Changing Pallet status to Q = QC Hold
                        Me.lbWorkWithShippingPallet.Text = "4. WHSE Transfer"                   ' 4. WHSE Transfer
                        Me.lbWhseTransfer.Text = ""                                             ' 5. 
                        Me.lbAdjPalletQty.Text = "6. Adj. Production Pallet Qty"                ' 6. Adjust qty at Production wrapper
                        Me.lbVoidPallet.Text = ""                                               ' 7.
                        Me.lbNumberTwo.Text = "8. Simms Repack Compnt"                          ' 8. Repack Components
                        Me.lbTestNewFunction.Text = "9. Simms FG Putaway"                       ' 9. Repack Putaway
                        Me.lbReAssignPallet.Text = "10. Load Pallet - Packland Trk"             '10.
                        Me.lbNewNumTwoProcess.Text = "11. New Number 2 Process"                 '11. New Number 2 Process
                        Me.lbPacklandNumber2.Text = ""                                          '12.  
                        Me.lbCreateSamplePallet.Text = ""                                       '13. Create Sample Pallet
                        Me.lbWorkWithSamplePallet.Text = ""                                     '14. Work With Sample Pallets
                        Me.lbRecvAbbylandPallet.Text = "15. FG Putaway HF"                      '15. Recv Abbyland Pallet
                        Me.lbComboMenu.Text = "16. Cut-Down Combo Menu"                         '16. Cut-Down Combo Menu
                        Me.lbWrkShippingPallet.Text = ""                                        '17.
                        Me.lbNewCrtSamplePallet.Text = ""                                       '18.
                        Me.lbNewWrkSamplePallet.Text = ""                                       '19.
                    End If
                End If


            Case "Shipping"
                If _whs = "17" Then
                    'Set Menu for Shipping Department
                    Me.lbFGPutaway.Text = "1. FG Recv From Plant 2"                             ' 1. FG Recv From Plant
                    Me.lbBinTransfer.Text = "2. Bin Transfer"                                   ' 2. Bin Transfer
                    Me.lbShippingPallet.Text = "3. Cycle Count Mont"                            ' 3. Cycle Count
                    Me.lbWorkWithShippingPallet.Text = ""                                       '
                    Me.lbWhseTransfer.Text = "5. Stock Pallet Transfer"                         ' 5. Stock Pallet Transfer
                    Me.lbAdjPalletQty.Text = "6. Shipping Rcpt @ Wrapper"                       ' 6. Shipping Rcpt @ Wrapper 
                    Me.lbVoidPallet.Text = "7. Void Shipped Pallet"                             ' 7. Voiding Shipped pallets
                    Me.lbNumberTwo.Text = "8. Recv Xfer Mont"                                   ' 8. Recv Xfer Mont
                    Me.lbTestNewFunction.Text = "9. Void Pallet"                                ' 9. Void Pallet
                    Me.lbReAssignPallet.Text = "10. ReAssign Pallet"                            '10. ReAssign Pallet
                    Me.lbNewNumTwoProcess.Text = "11. Crt New Inv Pallet Tag"                   '11. Crt New Inv Pallet Tag 
                    Me.lbPacklandNumber2.Text = "12. PackLand Number 2"                         '12. Packland Number 2 IC_NumberTwoPackland.aspx
                    Me.lbCreateSamplePallet.Text = "13. Create Sample Pallet"                   '13. Create Sample Pallet
                    Me.lbWorkWithSamplePallet.Text = "14. Work With Sample Pallets"             '14. Work With Sample Pallets
                    Me.lbRecvAbbylandPallet.Text = "15. Recv Abbyland Pallet"                   '15. Recv Abbyland Pallet lbWrkShippingPallet
                    Me.lbComboMenu.Text = "16. New-Crt Shipping Pallet"                         '16. New-Crt Shipping Pallet
                    Me.lbWrkShippingPallet.Text = "17. New-Wrk Shipping Pallet"                 '17. New-Wrk Shipping Pallet
                    Me.lbNewCrtSamplePallet.Text = "18. New-Crt Sample Pallet"                  '18. New-Crt Sample Pallet
                    Me.lbNewWrkSamplePallet.Text = "19. New-Wrk Sample Pallet"                  '19. New-Wrk Sample Pallet

                Else
                    'Set Menu for Shipping Department
                    Me.lbFGPutaway.Text = "1. FG Recv From Plant 2"                             ' 1. FG Recv From Plant
                    Me.lbBinTransfer.Text = "2. Bin Transfer"                                   ' 2. Bin Transfer
                    Me.lbShippingPallet.Text = ""                                               ' 3. Cycle Count
                    Me.lbWorkWithShippingPallet.Text = ""                                       ' 4. Work With Shipping Pallet
                    Me.lbWhseTransfer.Text = "5. Stock Pallet Transfer"                         ' 5. Stock Pallet Transfer
                    Me.lbAdjPalletQty.Text = "6. Shipping Rcpt @ Wrapper"                       ' 6. Shipping Rcpt @ Wrapper 
                    Me.lbVoidPallet.Text = "7. Void Shipped Pallet"                             ' 7. Voiding Shipped pallets
                    Me.lbNumberTwo.Text = "8. Recv Xfer Mont"                                   ' 8. Recv Xfer Mont
                    Me.lbTestNewFunction.Text = "9. Void Pallet"                                ' 9. Void Pallet
                    Me.lbReAssignPallet.Text = "10. ReAssign Pallet"                            '10. ReAssign Pallet
                    Me.lbNewNumTwoProcess.Text = "11. Crt New Inv Pallet Tag"                   '11. Crt New Inv Pallet Tag 
                    Me.lbPacklandNumber2.Text = "12. PackLand Number 2"                         '12. Packland Number 2 IC_NumberTwoPackland.aspx
                    Me.lbCreateSamplePallet.Text = ""                                           '13. Create Sample Pallet
                    Me.lbWorkWithSamplePallet.Text = ""                                         '14. Work With Sample Pallets
                    Me.lbRecvAbbylandPallet.Text = "15. Recv Abbyland Pallet"                   '15. Recv Abbyland Pallet lbWrkShippingPallet
                    Me.lbComboMenu.Text = "16. New-Crt Shipping Pallet"                         '16. New-Crt Shipping Pallet
                    Me.lbWrkShippingPallet.Text = "17. New-Wrk Shipping Pallet"                 '17. New-Wrk Shipping Pallet
                    Me.lbNewCrtSamplePallet.Text = "18. New-Crt Sample Pallet"                  '18. New-Crt Sample Pallet
                    Me.lbNewWrkSamplePallet.Text = "19. New-Wrk Sample Pallet"                  '19. New-Wrk Sample Pallet
                End If
                
            Case "CutDown"
                'Set Menu for Shipping Department
                Me.lbFGPutaway.Text = ""                                                ' 1. FG Recv From Plant
                Me.lbBinTransfer.Text = ""                                              ' 2. Bin Transfer
                Me.lbShippingPallet.Text = ""                                           ' 3. Create Shipping Pallet
                Me.lbWorkWithShippingPallet.Text = ""                                   ' 4. Work With Shipping Pallet
                Me.lbWhseTransfer.Text = ""                                             ' 5. Stock Pallet Transfer
                Me.lbAdjPalletQty.Text = ""                                             ' 6. 
                Me.lbVoidPallet.Text = ""                                               ' 7. Voiding Shipped pallets
                Me.lbNumberTwo.Text = ""                                                ' 8. Number 2 Process - Old
                Me.lbTestNewFunction.Text = ""                                          ' 9. Void Pallet
                Me.lbReAssignPallet.Text = ""                                           '10. ReAssign Pallet
                Me.lbNewNumTwoProcess.Text = ""                                         '11. New Number 2 Process 
                Me.lbPacklandNumber2.Text = ""                                          '12. Packland Number 2 IC_NumberTwoPackland.aspx
                Me.lbCreateSamplePallet.Text = ""                                       '13. Create Sample Pallet
                Me.lbWorkWithSamplePallet.Text = ""                                     '14. Work With Sample Pallets
                Me.lbRecvAbbylandPallet.Text = ""                                       '15. Recv Abbyland Pallet
                Me.lbComboMenu.Text = "16. Cut-Down Combo Menu"                         '16. Cut-Down Combo Menu
                Me.lbWrkShippingPallet.Text = ""                                        '17.
                Me.lbNewCrtSamplePallet.Text = ""                                       '18.
                Me.lbNewWrkSamplePallet.Text = ""                                       '19.

            Case "Repack"
                'Set Menu for Repack Department
                Me.lbFGPutaway.Text = "1. Repack Putaway"                               ' 1. Repack Putaway
                Me.lbBinTransfer.Text = "2. Bin Transfer"                               ' 2. Bin Transfer
                Me.lbShippingPallet.Text = ""                                           ' 3.
                Me.lbWorkWithShippingPallet.Text = ""                                   ' 4.
                Me.lbWhseTransfer.Text = ""                                             ' 5.
                Me.lbAdjPalletQty.Text = "6. Adj. Repack Pallet Qty"                    ' 6. Adjust qty at Repack wrapper
                Me.lbVoidPallet.Text = ""                                               ' 7.
                Me.lbNumberTwo.Text = "8. Repack Components"                            ' 8. Repack Components
                Me.lbTestNewFunction.Text = ""                                          ' 9. 
                Me.lbReAssignPallet.Text = ""                                           '10.
                Me.lbNewNumTwoProcess.Text = ""                                         '11. 
                Me.lbPacklandNumber2.Text = "12. PackLand Number 2"                     '12. Packland Number 2 IC_NumberTwoPackland.aspx
                Me.lbCreateSamplePallet.Text = ""                                       '13. Create Sample Pallet
                Me.lbWorkWithSamplePallet.Text = ""                                     '14. Work With Sample Pallets
                Me.lbRecvAbbylandPallet.Text = ""                                       '15. Recv Abbyland Pallet
                Me.lbComboMenu.Text = ""                                                '16.
                Me.lbWrkShippingPallet.Text = ""                                        '17.
                Me.lbNewCrtSamplePallet.Text = ""                                       '18.
                Me.lbNewWrkSamplePallet.Text = ""                                       '19.

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

        Select Case Me.txOption.Text
            Case "1"
                If Me.lbFGPutaway.Text.Length > 0 Then
                    If Trim(Common.GetVariable("User", Page).ToString) = "WCFG" Then
                        strURL = "~/IC_PutawayLot.aspx"
                        Common.SaveVariable("ScreenParam", "WCFG", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Shipping" Then
                        strURL = "~/IC_WH_Receipt.aspx"
                        Common.SaveVariable("ScreenParam", "FGRECV", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Repack" Then
                        strURL = "~/IC_Putaway.aspx"
                        Common.SaveVariable("ScreenParam", "REPACK", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Smoking" Then
                        strURL = "~/IC_SmokingRacks.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Combos" Then
                        strURL = "~/IC_ComboMenu.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Slicing" And _whs = "17" Then
                        strURL = "~/IC_PutawayLot.aspx"
                        'strURL = "~/IC_PutawayMontHF.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Slicing" And _whs = "35" Then
                    Else
                        strURL = "~/IC_PutawayLot.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "2"
                If Me.lbBinTransfer.Text.Length > 0 Then
                    'If Common.GetVariable("Department", Page) = "Smoking" Then
                    '    strURL = "~/IC_SmokingOut.aspx"
                    '    Common.SaveVariable("ScreenParam", "", Page)
                    'Else
                    strURL = "~/IC_BinTransfer.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                    'End If

                Else
                    xBadSelect = True
                End If

            Case "3"
                If Me.lbShippingPallet.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Slicing" Then
                        strURL = "~/IC_QCHold.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                        'ElseIf Common.GetVariable("Department", Page) = "Smoking" Then
                        '    strURL = "~/IC_SmokingToPackaging.aspx"
                        '    Common.SaveVariable("ScreenParam", "", Page)
                    Else
                        strURL = "~/IC_CycleCount.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "4"
                If Me.lbWorkWithShippingPallet.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Shipping" Then
                        strURL = "~/IC_WorkWithShippingPallet.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Slicing" Then
                        strURL = "~/IC_WH_Receipt.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If

                Else
                    xBadSelect = True
                End If

            Case "5"
                If Me.lbWhseTransfer.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Shipping" Then
                        strURL = "~/IC_StockPalletTransfer.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    ElseIf Common.GetVariable("Department", Page) = "Slicing" Then
                        strURL = "~/IC_StockPalletTransfer17.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "6"
                If Me.lbAdjPalletQty.Text.Length > 0 And Common.GetVariable("Department", Page) = "Shipping" Then
                    strURL = "~/IC_ShippingReceipt.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                ElseIf Me.lbAdjPalletQty.Text.Length > 0 Then
                    strURL = "~/IC_AdjPalletQty.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "7"
                If Me.lbVoidPallet.Text.Length > 0 Then
                    strURL = "~/IC_VoidShippedPallet.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "8"
                If Me.lbNumberTwo.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Shipping" Then
                        strURL = "~/IC_ReceiveTransfers.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    Else
                        strURL = "~/IC_RepackPalletComponents.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                    
                Else
                    xBadSelect = True
                End If

            Case "9"
                If Me.lbTestNewFunction.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Slicing" Then
                        strURL = "~/IC_Putaway.aspx"
                        Common.SaveVariable("ScreenParam", "REPACK", Page)
                    Else
                        strURL = "~/IC_VoidInventoryPallet.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    End If
                Else
                    xBadSelect = True
                End If

            Case "10"
                    If Me.lbReAssignPallet.Text.Length > 0 Then
                        If Common.GetVariable("Department", Page) = "Slicing" Then
                            strURL = "~/IC_LoadPalletOnPacklandTruck.aspx"
                            Common.SaveVariable("ScreenParam", "", Page)
                        ElseIf Common.GetVariable("Department", Page) = "Shipping" Then
                            strURL = "~/IC_ShippingPalletReAssign.aspx"
                            Common.SaveVariable("ScreenParam", "", Page)
                        End If

                    Else
                        xBadSelect = True
                    End If

            Case "11"
                If Me.lbNewNumTwoProcess.Text.Length > 0 Then
                    If Common.GetVariable("Department", Page) = "Shipping" Then
                        strURL = "~/IC_CreateInvPallet.aspx"
                        Common.SaveVariable("ScreenParam", "CREATE", Page)
                    Else
                        strURL = "~/IC_NumberTwoNew.aspx"
                        Common.SaveVariable("ScreenParam", "CREATE", Page)
                    End If
                    
                Else
                    xBadSelect = True
                End If

            Case "12"
                    If Me.lbPacklandNumber2.Text.Length > 0 Then
                        strURL = "~/IC_NumberTwoPackland.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    Else
                        xBadSelect = True
                    End If

            Case "13"
                    If Me.lbCreateSamplePallet.Text.Length > 0 Then
                        strURL = "~/IC_SamplePallet.aspx"
                        Common.SaveVariable("ScreenParam", "", Page)
                    Else
                        xBadSelect = True
                    End If
            Case "14"
                    If Me.lbWorkWithSamplePallet.Text.Length > 0 Then
                        If Common.GetVariable("Department", Page) = "Shipping" Then
                            strURL = "~/IC_WorkWithSamplePallet.aspx"
                            Common.SaveVariable("ScreenParam", "", Page)
                        Else
                            xBadSelect = True
                        End If
                    Else
                        xBadSelect = True
                    End If
            Case "15"
                    If Me.lbRecvAbbylandPallet.Text.Length > 0 Then
                        If Common.GetVariable("Department", Page) = "Shipping" Then
                            strURL = "~/IC_AbbylandPallet.aspx"
                            Common.SaveVariable("ScreenParam", "", Page)
                        ElseIf Common.GetVariable("Department", Page) = "Slicing" Then
                            strURL = "~/IC_Putaway.aspx"
                            Common.SaveVariable("ScreenParam", "", Page)
                        Else
                            xBadSelect = True
                        End If
                    Else
                        xBadSelect = True
                    End If

            Case "16"
                If Me.lbWrkShippingPallet.Text.Length > 0 Then
                    strURL = "~/IC_ShippingPalletsNew.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "17"
                If Me.lbComboMenu.Text.Length > 0 Then
                    strURL = "~/IC_WorkWithShippingPalletNew.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "18"
                If Me.lbWrkShippingPallet.Text.Length > 0 Then
                    strURL = "~/IC_SamplePalletNew.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "19"
                If Me.lbWrkShippingPallet.Text.Length > 0 Then
                    strURL = "~/IC_WorkWithSamplePalletNew.aspx"
                    Common.SaveVariable("ScreenParam", "", Page)
                Else
                    xBadSelect = True
                End If

            Case "98" 'FG Inventory Counting Montgomery
                strURL = "~/IC_PhysicalCount17.aspx"
                Common.SaveVariable("ScreenParam", "", Page)

            Case "99" 'Monthend FG Inventory Counting
                strURL = "~/IC_PhysicalCountOWS.aspx"
                Common.SaveVariable("ScreenParam", "", Page)

            Case "92"
                If _whs = "17" Then
                    strURL = "http://kma2.carlbuddig.com/inventorycontrol/default.aspx"
                Else
                    xBadSelect = True
                End If

            Case Else
                    xBadSelect = True
        End Select 'IC_NumberTwoPackland.aspx

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
        Page.Response.Redirect("~/")
    End Sub
End Class