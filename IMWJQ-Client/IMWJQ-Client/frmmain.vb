Imports System
Imports System.IO

Public Class frmmain
    Private cnf As config
    Private Structure config
        Dim Prio As Diagnostics.ProcessPriorityClass
        Dim Software As List(Of executable)
        Dim TempDir As String
        Dim ServerPath As String
        Dim Tokens As List(Of Token)
    End Structure

    Private Structure Job
        Dim Software As String
        Dim Arg As String
        Dim TaskPath As String
        Dim ID As ULong
    End Structure

    Private Structure Token
        Dim Name As String
        Dim Value As String
    End Structure

    Private Structure executable
        Dim Name As String
        Dim Path As String
    End Structure

    Private Sub frmmain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnf = ReadConfig()
        If System.IO.Directory.Exists(cnf.TempDir) Then
            DelDir(cnf.TempDir)
        End If
        System.IO.Directory.CreateDirectory(cnf.TempDir)

        Diagnostics.Process.GetCurrentProcess().PriorityClass = cnf.Prio

        If File.Exists(cnf.ServerPath & "\Job.txt") Then
            ExecuteTask()
        End If

        Watcher.Path = cnf.ServerPath.Substring(0, cnf.ServerPath.LastIndexOf("\"))
    End Sub

    Private Function DelDir(Path As String) As Boolean
        'Deletes a folder with its content
        Try
            If Not System.IO.Directory.Exists(Path) Then
                If System.IO.File.Exists(Path) Then
                    System.IO.File.Delete(Path)
                End If
                Return True
                Exit Function
            End If
            Dim SubFiles() As String
            Dim SubDirs() As String
            SubFiles = System.IO.Directory.GetFiles(Path)
            For Each file In SubFiles
                System.IO.File.Delete(file)
            Next
            SubDirs = System.IO.Directory.GetDirectories(Path)
            For Each subDir In SubDirs
                DelDir(subDir)
            Next
            System.IO.Directory.Delete(Path)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function ApplyTokens(str As String, Tokens As List(Of Token))
        For Each Token In Tokens
            str = str.Replace(Token.Name, Token.Value)
        Next
        Return str
    End Function

    Private Sub ExecuteTask()
        If Not System.IO.File.Exists(cnf.ServerPath & "\Job.txt") Then Exit Sub

        Dim readbuffer As String
        Dim reader As New StreamReader(cnf.ServerPath & "\Job.txt")
        Dim writer As StreamWriter
        Dim Parameter As String
        Dim Value As String
        Dim Job As Job

        lblStatus.Text = "Copying input files"

        While Not reader.EndOfStream
            readbuffer = reader.ReadLine()
            If Not readbuffer.Contains("=") Then Continue While
            Parameter = readbuffer.Substring(0, readbuffer.IndexOf("="))
            Value = readbuffer.Substring(readbuffer.IndexOf("=") + 1)
            Select Case Parameter
                Case "Software"
                    Job.Software = Value
                Case "Args"
                    Job.Arg = ApplyTokens(Value, cnf.Tokens)
                Case "TaskPath"
                    Job.TaskPath = Value
                Case "Clear"
                    DelDir(ApplyTokens(Value, cnf.Tokens))
                Case "ID"
                    Job.ID = CULng(Value)
            End Select
        End While
        If System.IO.Directory.Exists(Job.TaskPath & "\Util files") Then
            My.Computer.FileSystem.CopyDirectory(Job.TaskPath & "\Util files", cnf.TempDir & "\Util files")
        End If

        Dim SubFiles() As String
        SubFiles = System.IO.Directory.GetFiles(Job.TaskPath)
        readbuffer = ""
        For Each file In SubFiles
            If Path.GetFileNameWithoutExtension(file) = "task" Then
                If System.IO.File.Exists(cnf.TempDir & "\" & Path.GetFileName(file)) Then
                    System.IO.File.Delete(cnf.TempDir & "\" & Path.GetFileName(file))
                End If
                reader = New StreamReader(file)
                readbuffer = readbuffer & reader.ReadToEnd
                reader.Close()
                writer = New StreamWriter(cnf.TempDir & "\" & Path.GetFileName(file))
                writer.Write(ApplyTokens(readbuffer, cnf.Tokens))
                writer.Close()
                Exit For
            End If
        Next

        Dim apppath As String
        For Each sw In cnf.Software
            If sw.Name = Job.Software Then
                apppath = sw.Path
                Exit For
            End If
        Next
        If apppath = "" Then
            Exit Sub
        End If
        Dim runer As New Process()

        lblStatus.Text = "Running worker"

        runer = runer.Start(apppath, Job.Arg)
        Try
            runer.PriorityClass = cnf.Prio
        Catch
        End Try
        runer.WaitForExit()
        If Directory.Exists(cnf.TempDir & "\Output") Then
            lblStatus.Text = "Copying Output files"
            My.Computer.FileSystem.CopyDirectory(cnf.TempDir & "\Output", cnf.ServerPath & "\Output", True)
        End If
        lblStatus.Text = "Free"
    End Sub

    Private Function ReadConfig() As config
        Dim readbuffer As String
        Dim Parameter As String
        Dim cnf As config
        Dim Value As String
        Dim NewSW As executable
        Dim NewToken As Token
        cnf.Software = New List(Of executable)
        cnf.Tokens = New List(Of Token)
        Dim reader As New StreamReader(My.Application.Info.DirectoryPath & "\LocalConfig.conf")
        While Not reader.EndOfStream
            readbuffer = reader.ReadLine()
            If Not readbuffer.Contains("=") Then Continue While
            Parameter = readbuffer.Substring(0, readbuffer.IndexOf("="))
            Value = readbuffer.Substring(readbuffer.IndexOf("=") + 1)
            Select Case Parameter
                Case "ServerPath"
                    cnf.ServerPath = Value
                Case "TempDir"
                    cnf.TempDir = Value
                Case "Priority"
                    Select Case Value
                        Case "1"
                            cnf.Prio = ProcessPriorityClass.Idle
                        Case "2"
                            cnf.Prio = ProcessPriorityClass.BelowNormal
                        Case "4"
                            cnf.Prio = ProcessPriorityClass.AboveNormal
                        Case "5"
                            cnf.Prio = ProcessPriorityClass.High
                        Case Else
                            cnf.Prio = ProcessPriorityClass.Normal
                    End Select
                Case "Software"
                    If readbuffer.Contains(";") Then
                        readbuffer = readbuffer.Substring(readbuffer.IndexOf("=") + 1)
                        NewSW.Name = readbuffer.Substring(0, readbuffer.IndexOf(";"))
                        NewSW.Path = readbuffer.Substring(readbuffer.IndexOf(";") + 1)
                        cnf.Software.Add(NewSW)
                    End If
                Case "Token"
                    If readbuffer.Contains(";") Then
                        readbuffer = readbuffer.Substring(readbuffer.IndexOf("=") + 1)
                        NewToken.Name = readbuffer.Substring(0, readbuffer.IndexOf(";"))
                        NewToken.Value = readbuffer.Substring(readbuffer.IndexOf(";") + 1)
                        cnf.Tokens.Add(NewToken)
                    End If
            End Select
        End While
        reader.Close()
        Return cnf
    End Function

    Private Sub Watcher_Changed(sender As Object, e As FileSystemEventArgs) Handles Watcher.Changed
        ExecuteTask()
    End Sub

    Private Sub ChkStop_CheckedChanged(sender As Object, e As EventArgs) Handles ChkStop.CheckedChanged
        Watcher.EnableRaisingEvents = Not ChkStop.Checked
    End Sub
End Class