Public Class PopulateWord

    Dim wrdApp As Word.Application
    Dim wrdDoc As Word.Document

    ' per farlo funzionare: prima mettere un riferimento alla type library di Office
    ' (click destro sulla soluzione | aggiungi riferimento | COM | Libreria oggetti di Microsoft Word .. 
    ' (o Microsoft Office Object Library o Model, o qualcosa del genere..) )

    Public Sub New(ByVal TemplateFile As String)
        'Apre word e riempie il modello

        'apertura di word
        'Try
        '    wrdApp = CreateObject("Word.Application")
        wrdApp = New Word.Application()

        'wrdApp = GetObject(, "Word.Application")
        'Catch
        '    Exit Sub
        'End Try
        'wrdApp = CreateObject("Word.Application")
        Try
            wrdApp.Documents.Add(TemplateFile)
            'compilazione del bookmark
            wrdDoc = wrdApp.ActiveDocument
        Catch ex As Exception
            LogErrore("PopulateWord|New(): " + ex.Message)
        End Try

        'If GLOB.NumeriVerbaleInWord Then
        '    InsertWord(wrd, "Numero", CStr(lVacc))
        'End If

        'InsertWord(wrd, "BK1", CSafeDate(rsVerb.Fields("data")) & "")

        'Dim TempStr As String
        'TempStr = CSafeStr(rsCli.Fields("Nomeazienda")) & " "
        'If CSafeStr(rsVerb.Fields("CodiceFiscale")) <> "" Then
        '    TempStr = TempStr & " - C.F. " & CSafeStr(rsVerb.Fields("CodiceFiscale"))
        'End If
        'If CSafeStr(rsVerb.Fields("PartitaIVA")) <> "" Then
        '    TempStr = TempStr & " - P.IVA " & CSafeStr(rsVerb.Fields("PartitaIVA"))
        'End If
        'InsertWord(wrd, "BK2", TempStr)
        'InsertWord(wrd, "BK3", CSafeStr(rsCli.Fields("Indirizzo")) & "")
        'InsertWord(wrd, "BK4", CSafeStr(rsCli.Fields("Cap")) & "")
        'InsertWord(wrd, "BK5", CSafeStr(rsCli.Fields("Citta")) & "")
        'InsertWord(wrd, "BK6", CSafeStr(rsCli.Fields("Provincia")) & "")

        'InsertWord(wrd, "BK7", CSafeStr(rsVerb.Fields("cantiere1")) & " " & CSafeStr(rsVerb.Fields("cantiere2")) & " " & CSafeStr(rsVerb.Fields("cantiere3")))
        'InsertWord(wrd, "BK8", CSafeStr(rsVerb.Fields("diretlav")) & "")
        'InsertWord(wrd, "BK9", CSafeStr(rsVerb.Fields("formapag")) & "")
        'InsertWord(wrd, "BK10", CSafeStr(rsVerb.Fields("spedizione")) & "")
        'InsertWord(wrd, "BK11", CSafeStr(rsVerb.Fields("intestafatt")) & "")
        'InsertWord(wrd, "BK12", CSafeStr(rsVerb.Fields("note1")) & " " & CSafeStr(rsVerb.Fields("note2")) & " " & CSafeStr(rsVerb.Fields("note3")))

    End Sub

    ''' <summary>
    ''' Inserisce all'interno del file impostato durante la costruzione il testo dopo il segnalibro
    ''' </summary>
    ''' <param name="bookmarkName"> nome del segnalibro dopo il quale si vuole inserire il testo</param>
    ''' <param name="text">testo da inserire nel documento</param>
    ''' <remarks></remarks>
    Public Sub InsertWord(ByVal bookmarkName As String, ByVal text As String)
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto wrdDoc. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        wrdDoc.Bookmarks.Item(bookmarkName).Range.InsertAfter(text)
        'wrdDoc.Save()

    End Sub

    Public Sub chiudiDocumento()
        Try
            wrdDoc.Save()
        Catch ex As Exception
            MessageBox.Show("Impossibile salvare il documento" + vbCrLf + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End Try

        'wrdDoc.Close()
        wrdApp.Visible = True
        If wrdApp.Application.Visible = False Then
            wrdApp.Application.Visible = True
        End If
        wrdApp.Application.Activate()

    End Sub

    Public Function InsertTableRowText(ByRef oRow As Object, ByVal s1 As String, ByVal s2 As String, ByVal s3 As String, ByVal s4 As String, ByVal bCreateNewRow As Boolean) As Object
        With oRow
            'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto oRow.Cells. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            .Cells(1).Range.InsertAfter(s1)
            'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto oRow.Cells. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            .Cells(2).Range.InsertAfter(s2)
            'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto oRow.Cells. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            .Cells(3).Range.InsertAfter(s3)
            'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto oRow.Cells. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If s4 <> "" Then .Cells(4).Range.InsertAfter(s4)

            If bCreateNewRow Then
                'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto oRow.Range. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                InsertTableRowText = oRow.Range.Rows.Add
            End If
        End With
    End Function

    Public Function InsertTableRowText(ByRef oRow As Object, ByVal s1 As String, ByVal s2 As String, ByVal s3 As String, ByVal s4 As String, ByVal s5 As String, ByVal bCreateNewRow As Boolean) As Object
        With oRow
            .Cells(1).Range.InsertAfter(s1)
            .Cells(2).Range.InsertAfter(s2)
            .Cells(3).Range.InsertAfter(s3)
            .Cells(4).Range.InsertAfter(s4)
            .Cells(5).Range.InsertAfter(s5)
            If bCreateNewRow Then
                InsertTableRowText = oRow.Range.Rows.Add
            End If
        End With
    End Function

    Public Function GetTableRowFromBookmark(ByVal sBookmark As String) As Object
        Dim oRow As Object
        Dim bk As Object
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto wrdDoc. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        bk = wrdDoc.Bookmarks.Item(sBookmark)
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto bk.Range. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oRow = bk.Range.Cells(1).Row
        GetTableRowFromBookmark = oRow
    End Function

    Public Sub ShowWordWindow()
        On Error Resume Next
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto wrdApp.Application. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        wrdApp.Application.Visible = True
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto wrdApp.Application. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'wrdApp.Application.WindowState = System.Windows.Forms.FormWindowState.Maximized
        'FormPrincipale.ZOrder 1
        AppActivate("Microsoft Word - Documento1")
        Err.Clear()
    End Sub
End Class
