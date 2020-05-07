Public Class frmMain
    Private Parameters As List(Of String)
    Private Jobs As List(Of List(Of Double))
    Private ProjectPath As String

    Private Sub btnLoadIPScript_Click(sender As Object, e As EventArgs) Handles btnLoadIPScript.Click
        If OpnDlg.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If
        Dim fs As System.IO.FileStream
        fs = OpnDlg.OpenFile()
        Dim reader As New System.IO.StreamReader(fs)
        txtPyCode.Text = reader.ReadToEnd()
        reader.Close()
    End Sub

    Private Sub btnLoadCSV_Click(sender As Object, e As EventArgs) Handles btnLoadCSV.Click
        Dim i As Integer
        If opncsv.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If
        Parameters = New List(Of String)
        Jobs = New List(Of List(Of Double))
        Dim fs As New System.IO.FileStream(opncsv.FileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(opncsv.FileName)
            MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
            'My.Computer.Info.InstalledUICulture().NumberFormat.
            If CSVSepAuto.Checked Then
                If My.Computer.Info.InstalledUICulture().NumberFormat.CurrencyDecimalSeparator = "," Then
                    MyReader.Delimiters = New String() {";"}
                Else
                    MyReader.Delimiters = New String() {","}
                End If
            Else
                If CSVSepDot.Checked Then
                    MyReader.Delimiters = New String() {","}
                Else
                    MyReader.Delimiters = New String() {";"}
                End If
            End If
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    If Parameters.Count = 0 Then
                        For i = 0 To UBound(currentRow)
                            Parameters.Add(currentRow(i))
                        Next i
                    Else
                        Jobs.Add(New List(Of Double))
                        For i = 0 To UBound(currentRow)
                            Jobs.Last.Add(CDbl(currentRow(i)))
                        Next i
                    End If
                Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                    MsgBox("Zeile " & ex.Message & " ist ungültig.  Wird übersprungen")
                End Try
            End While
        End Using
    End Sub

    Private Sub btnOpnPrj_Click(sender As Object, e As EventArgs) Handles btnOpnPrj.Click
        If opnPrj.ShowDialog() = DialogResult.OK Then
            ProjectPath = opnPrj.FileName
        End If
    End Sub

    Private Sub btnGenScripts_Click(sender As Object, e As EventArgs) Handles btnGenScripts.Click
        If IsNothing(Jobs) Then
            Exit Sub
        End If
        If Jobs.Count = 0 Or ProjectPath = "" Then
            Exit Sub
        End If
        If FolderDlg.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        Dim i As Integer
        Dim j As Integer
        Dim outfile As String
        Dim inscript As String
        Dim modscript As String
        Dim vstring As String
        inscript = txtPyCode.Text
        If inscript.Contains("SetScriptVersion") Then
            vstring = inscript.Substring(inscript.IndexOf("SetScriptVersion"))
            vstring = vstring.Substring(0, vstring.IndexOf(vbNewLine))
            inscript = inscript.Replace(vstring, "")
        Else
            vstring = ""
        End If
        Try
            For i = 0 To Jobs.Count - 1
                modscript = inscript
                'Replace Symbols
                For j = 0 To Parameters.Count - 1
                    modscript = modscript.Replace(Parameters.Item(j), Jobs.Item(i).Item(j))
                Next j

                outfile = outfile & "import shutil" & vbNewLine & vstring & vbNewLine & "#Parameters" & vbNewLine & "workpath=" & Chr(34) & txtWorkDir.Text & Chr(34) & vbNewLine & "dl = " & -CInt(chkDelAnsysFiles.Checked)
                outfile = outfile & vbNewLine & "src=" & Chr(34) & ProjectPath.Substring(0, ProjectPath.LastIndexOf("\")) & Chr(34) & vbNewLine
                outfile = outfile & "#Start header" & vbNewLine & "shutil.rmtree(workpath,ignore_errors=1) #Delete working directory" & vbNewLine
                outfile = outfile & "shutil.copytree(src,workpath) #Copy working directory" & vbNewLine
                outfile = outfile & "Open(FilePath= workpath + " & ProjectPath & Chr(34) & ")" & vbNewLine
                outfile = outfile & "#End header" & vbNewLine & vbNewLine & "#Start your stuff" & vbNewLine
                outfile = outfile & modscript & vbNewLine
                outfile = outfile & "#End your stuff" & vbNewLine
                outfile = outfile & "if dl==1:" & vbNewLine & "	shutil.rmtree(workpath,ignore_errors=1) #Delete working directory"
                Dim writer As New System.IO.StreamWriter(FolderDlg.SelectedPath & "\" & i & ".wbjn")
                writer.Write(outfile)
                writer.Close()
                writer.Dispose()
            Next i
            MsgBox(Jobs.Count & " Eingabescripte wurden erzeugt.")
        Catch
            MsgBox("Unbekannter Fehler beim Erzeugen der Eingabescripte.")
        End Try
    End Sub
End Class
