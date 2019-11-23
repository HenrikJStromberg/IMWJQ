Imports System
Imports System.IO
Imports System.Security.Permissions

Module Module1
    Private cnf As config
    Private Structure config
        Dim Prio As Diagnostics.ProcessPriorityClass
        Dim Software As List(Of executable)
        Dim WorkerPath As String
        Dim TaskPath As String
    End Structure

    Private Structure executable
        Dim Name As String
        Dim Path As String
    End Structure

    Sub Main()
        cnf = ReadConfig()
        Diagnostics.Process.GetCurrentProcess().PriorityClass = cnf.Prio
        If File.Exists(cnf.TaskPath) Then
            ExecuteTask()
        End If

        Dim watchfolder As New System.IO.FileSystemWatcher()
        watchfolder.Path = cnf.TaskPath.Substring(0, cnf.TaskPath.LastIndexOf("\"))
        watchfolder.NotifyFilter = NotifyFilters.FileName = NotifyFilters.CreationTime
        watchfolder.Filter = cnf.TaskPath.Substring(cnf.TaskPath.LastIndexOf("\") + 1)
        AddHandler watchfolder.Created, AddressOf ExecuteTask
        watchfolder.EnableRaisingEvents = True
    End Sub

    Private Sub ExecuteTask()
        Dim readbuffer As String
        Dim appstring As String
        Dim apppath As String
        Dim arg As String
        Dim reader As New StreamReader(cnf.TaskPath)
        apppath = ""
        If reader.EndOfStream = False Then
            readbuffer = reader.ReadLine()
        Else
            Exit Sub
        End If
        reader.Close()
        appstring = readbuffer.Substring(0, readbuffer.IndexOf(";"))
        arg = readbuffer.Substring(readbuffer.IndexOf(";") + 1)
        For Each sw In cnf.Software
            If sw.Name = appstring Then
                apppath = sw.Path
            End If
        Next
        If apppath = "" Then
            Exit Sub
        End If
        Dim runer As New Process()
        runer = runer.Start(apppath, arg)
        runer.PriorityClass = cnf.Prio
        runer.WaitForExit()
        File.Delete(cnf.TaskPath)
    End Sub

    Private Function ReadConfig() As config
        Dim cnf As config
        Dim readbuffer As String
        cnf.Software = New List(Of executable)
        Dim reader As New StreamReader(My.Application.Info.DirectoryPath & "\LocalConfig.conf")
        cnf.TaskPath = reader.ReadLine()
        cnf.WorkerPath = reader.ReadLine()
        readbuffer = reader.ReadLine()
        Select Case readbuffer
            Case 1
                cnf.Prio = ProcessPriorityClass.Idle
            Case 2
                cnf.Prio = ProcessPriorityClass.BelowNormal
            Case 3
                cnf.Prio = ProcessPriorityClass.Normal
            Case 4
                cnf.Prio = ProcessPriorityClass.AboveNormal
            Case 5
                cnf.Prio = ProcessPriorityClass.High
        End Select
        While reader.EndOfStream = False
            Dim sw As executable
            readbuffer = reader.ReadLine
            sw.Name = readbuffer.Substring(0, readbuffer.IndexOf(";"))
            sw.Path = readbuffer.Substring(readbuffer.IndexOf(";") + 1)
            cnf.Software.Add(sw)
        End While
        reader.Close()
        Return cnf
    End Function


End Module
