Imports System.ComponentModel

Public Class frmmain
    Private Clients As List(Of Client)
    Private Jobs As List(Of Job)
    Private MaxID As ULong

    Private Structure Job
        Dim Software As String
        Dim Args As String
        Dim Owner As String
        Dim Timeout As Integer
        Dim Client As List(Of String)
        Dim Status As JobStatus
        Dim StartTime As Date
        Dim ID As ULong
    End Structure

    Private Enum JobStatus
        Waiting
        Working
        Finished
        Failed
    End Enum

    Private Structure Client
        Dim Name As String
        Dim Software As List(Of String)
        Dim Status As ClientStatus
        Dim Job As Job
        Dim Background As Boolean
    End Structure

    Private Enum ClientStatus
        Free
        Busy
        Dead
    End Enum

    Private Sub frmmain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblJobDir.Text = My.Settings.JobDir
        Jobs = New List(Of Job)
        Clients = New List(Of Client)
    End Sub

    Private Sub AssigneJobs() 'ToDo: Overhaul
        For Each Job In Jobs 'Look For machine specific jobs
            If Job.Status = JobStatus.Waiting And Job.Client.Count > 0 Then
                For Each Client In Clients
                    If Client.Status = ClientStatus.Free And Job.Client.Contains(Client.Name) And Client.Software.Contains(Job.Software) Then
                        Client.Status = ClientStatus.Busy
                        Client.Job = Job
                        Job.Client.Clear()
                        Job.Client.Add(Client.Name)
                        Job.StartTime = Now
                        WriteTask(My.Settings.JobDir & "\Clients\" & Client.Name & ".txt", Job)
                    End If
                Next
            End If
        Next

        For Each Job In Jobs 'Look For machine specific jobs
            If Job.Status = JobStatus.Waiting And Job.Client.Count = 0 Then
                For Each Client In Clients
                    If Client.Status = ClientStatus.Free And Client.Software.Contains(Job.Software) Then
                        Client.Status = ClientStatus.Busy
                        Client.Job = Job
                        Job.Client.Clear()
                        Job.Client.Add(Client.Name)
                        Job.StartTime = Now
                        WriteTask(My.Settings.JobDir & "\Clients\" & Client.Name & ".txt", Job)
                    End If
                Next
            End If
        Next
        WrtieOutputs()
    End Sub

    Private Sub WriteTask(Path As String, Job As Job) 'ToDo: Overhaul
        Dim writer As New System.IO.StreamWriter(Path)
        writer.WriteLine(Job.Software & ";" & Job.Args)
        writer.Close()
    End Sub

    Private Sub WrtieOutputs() 'ToDo: Overhaul
        Dim writer As System.IO.StreamWriter
        Dim clinetsstr As String
        writer = New IO.StreamWriter(My.Settings.JobDir & "\Waiting.csv")
        writer.WriteLine("Software;Argrument;User;Timeout;Client")
        For Each job In Jobs 'Create waiting list
            If job.Status = JobStatus.Waiting Then
                clinetsstr = ""
                For Each client In job.Client
                    clinetsstr = clinetsstr & "," & client
                Next
                clinetsstr = clinetsstr.Substring(1)
                writer.WriteLine(job.Software & ";" & job.Args & ";" & job.Owner & ";" & job.Timeout & ";" & clinetsstr)
            End If
        Next job
        writer.Close()
        writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "In work.csv")
        writer.WriteLine("Software;Argrument;User;Timeout;Client;Start time")
        For Each job In Jobs 'Create working list
            If job.Status = JobStatus.Working Then
                writer.WriteLine(job.Software & ";" & job.Args & ";" & job.Owner & ";" & job.Timeout & ";" & job.Client.First & ";" & job.StartTime.ToShortTimeString)
            End If
        Next
        writer.Close()
        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "Done.csv") = False Then
            writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Done.csv")
            writer.WriteLine("Software;Argrument;User;Timeout;Client;Start time")
            writer.Close()
        End If
        writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Done.csv", True)
        For Each job In Jobs 'Create working list
            If job.Status = JobStatus.Finished Then
                writer.WriteLine(job.Software & ";" & job.Args & ";" & job.Owner & ";" & job.Timeout & ";" & job.Client.First & ";" & job.StartTime.ToShortTimeString)
            End If
        Next
        writer.Close()
    End Sub

    Private Sub CheckClients() 'ToDo: Overhaul
        For Each Client In Clients
            Dim eJob As Job
            Dim findex As Integer
            If System.IO.File.Exists(My.Settings.JobDir & "\Clients\" & Client.Name & ".txt") = False And Client.Status = ClientStatus.Busy Then
                'Check for finished jobs
                Client.Status = ClientStatus.Free
                findex = Jobs.FindIndex(Function(p) p.ID = Client.Job.ID)
                eJob = Jobs.Item(findex)
                eJob.Status = JobStatus.Finished
                Jobs.Item(findex) = eJob
                Client.Job = Nothing
            End If
        Next
    End Sub

    Private Sub CrawlJobFolder()
        Dim JobDirs() As String
        JobDirs = System.IO.Directory.GetDirectories(My.Settings.JobDir & "\Jobs")
        Dim i As Integer

        For i = 0 To UBound(JobDirs)
            If Not System.IO.File.Exists(JobDirs(i) & "\Job.txt") Then Continue For
            Dim Reader As New System.IO.StreamReader(JobDirs(i) & "\Job.txt")
            Dim line As String
            Dim nJob As New Job
            nJob.Software = ""
            nJob.Args = ""
            nJob.Software = ""
            nJob.Client = New List(Of String)
            nJob.ID = Nothing
            nJob.Status = JobStatus.Waiting
            nJob.Timeout = 0
            Do While Not Reader.EndOfStream

                line = Reader.ReadLine
                If line.EndsWith("=") Or Not line.Contains("=") Then Continue Do
                Select Case LCase(line.Substring(0, line.IndexOf("=")))
                    Case "software"
                        nJob.Software = line.Substring(line.IndexOf("=") + 1)
                    Case "user"
                        nJob.Owner = line.Substring(line.IndexOf("=") + 1)
                    Case "args"
                        nJob.Args = line.Substring(line.IndexOf("=") + 1)
                    Case "client"
                        Dim csplit() As String
                        csplit = line.Substring(line.IndexOf("=") + 1).Split(",")
                        For Each item In csplit
                            nJob.Client.Add(item)
                        Next
                    Case "id"
                        nJob.ID = CULng(line.Substring(line.IndexOf("=") + 1))
                    Case "timeout"
                        nJob.Timeout = CInt(line.Substring(line.IndexOf("=") + 1))
                End Select
            Loop
            Reader.Close()

            If nJob.ID = 0 Then
                'Job is new
                MaxID = MaxID + 1
                nJob.ID = MaxID

                Try
                    SetJobID(JobDirs(i) & "\Job.txt", nJob.ID)
                Catch
                    Continue For
                End Try
            Else
                'Job already exists
                Dim oJobIndex As Integer = Jobs.FindIndex(Function(x) x.ID = nJob.ID)
                If oJobIndex = -1 Then
                    MaxID = MaxID + 1
                    nJob.ID = MaxID
                    'Job has ID but is not found in list
                    Try
                        SetJobID(JobDirs(i) & "\Job.txt", nJob.ID)
                    Catch
                        Continue For
                    End Try
                Else
                    If Jobs(oJobIndex).Status = JobStatus.Waiting Then
                        Jobs(oJobIndex) = nJob
                    End If
                    If Jobs(oJobIndex).Status = JobStatus.Failed Then
                        nJob.Status = JobStatus.Waiting
                        Jobs(oJobIndex) = nJob
                    End If
                End If
            End If

        Next i
    End Sub

    Private Sub SetJobID(Path As String, ID As ULong)
        Dim Reader As New System.IO.StreamReader(Path)
        Dim File As New List(Of String)
        Dim Line As String
        Do While Not Reader.EndOfStream
            Line = Reader.ReadLine
            If Not Line.Contains("ID") Then
                File.Add(Line)
            End If
        Loop
        Reader.Close()

        Dim Writer As New System.IO.StreamWriter(Path)
        For Each L In File
            Writer.Write(L)
            Writer.WriteLine()
        Next
        Writer.Write("ID=" & CStr(ID))
        Writer.Close()
    End Sub

    Private Function LoadClients(oldList As List(Of Client)) As List(Of Client)
        'Loads the client list
        Dim res As New List(Of Client)
        Dim fi As New System.IO.FileInfo(My.Settings.JobDir & "\Clients.csv")
        Dim l As Integer
        l = fi.Length
        Using MyReader As New Microsoft.VisualBasic.FileIO.
        TextFieldParser(My.Settings.JobDir & "\Clients.csv")
            MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
            If MyReader.PeekChars(l).Contains(";") Then
                MyReader.Delimiters = New String() {";"}
            Else
                MyReader.Delimiters = New String() {","}
            End If
            Dim currentRow As String()
            MyReader.ReadFields()
            While Not MyReader.EndOfData
                Dim nclient As New Client
                currentRow = MyReader.ReadFields()
                nclient.Name = currentRow(0)
                nclient.Software = New List(Of String)
                Dim sw() As String
                sw = Split(currentRow(1), "&")
                Dim i As Integer
                For i = 0 To UBound(sw)
                    nclient.Software.Add(Trim(sw(i)))
                Next i
                nclient.Background = CBool(currentRow(2))
                res.Add(nclient)
            End While
        End Using
        'Put jobs back into preexisting clients
        Dim notFound As Boolean
        For Each oc In oldList
            notFound = True
            For Each c In res
                If oc.Name = c.Name Then
                    c.Job = oc.Job
                    notFound = False
                    Exit For
                End If
            Next
            If notFound = True Then
                'Reset job as client has vanished
                oc.Job.Client = Nothing
                oc.Job.Status = JobStatus.Waiting
            End If
        Next
        'generate communication folders for clients if needed
        For Each c In res
            If c.Status <> ClientStatus.Busy Then
                If System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & c.Name & "\Input") Then
                    System.IO.Directory.Delete(My.Settings.JobDir & "\Clients\" & c.Name & "\Input")
                End If
                If System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & c.Name & "\Output") Then
                    System.IO.Directory.Delete(My.Settings.JobDir & "\Clients\" & c.Name & "\Output")
                End If
            End If
            If Not System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & c.Name & "\Input") Then
                System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Clients\" & c.Name & "\Input")
            End If
            If Not System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & c.Name & "\Output") Then
                System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Clients\" & c.Name & "\Output")
            End If
        Next
        Return res
    End Function

    Private Sub TaskWatcher_Changed(sender As Object, e As IO.FileSystemEventArgs) Handles TaskWatcher.Changed
        If chkRun.Checked Then
            CrawlJobFolder()
            AssigneJobs()
        End If
    End Sub

    Private Sub ClientWatcher_Changed(sender As Object, e As IO.FileSystemEventArgs) Handles ClientWatcher.Changed
        If chkRun.Checked Then
            CheckClients()
        End If
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick 'ToDo: Overhaul
        Dim findex As Integer
        Dim fjob As Job
        Dim writer As System.IO.StreamWriter
        For Each client In Clients
            If (Now - client.Job.StartTime).TotalMinutes > client.Job.Timeout Then
                client.Status = ClientStatus.Dead
                findex = Jobs.FindIndex(Function(p) p.ID = client.Job.ID)
                fjob = Jobs.Item(findex)
                fjob.Status = JobStatus.Failed
                Jobs.Item(findex) = fjob
                If System.IO.File.Exists(My.Settings.JobDir & "\Failed.csv") = False Then
                    writer = New IO.StreamWriter(My.Settings.JobDir & "\Done.csv")
                    writer.WriteLine("Software;Argument;User;Timeout;Client;Start time")
                    writer.Close()
                End If
                writer = New IO.StreamWriter(My.Settings.JobDir & "\Done.csv", True)
                writer.WriteLine(client.Job.Software & ";" & client.Job.Args & ";" & client.Job.Owner & ";" & client.Job.Timeout & ";" & client.Name & ";" & client.Job.StartTime)
                writer.Close()
                client.Job = Nothing
            End If
        Next
    End Sub

    Private Sub btnSelJobDir_Click(sender As Object, e As EventArgs) Handles btnSelJobDir.Click
        FolderBrowserDialog.SelectedPath = My.Settings.JobDir
        If FolderBrowserDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            My.Settings.Item("JobDir") = FolderBrowserDialog.SelectedPath
            lblJobDir.Text = My.Settings.JobDir
        End If
    End Sub

    Private Sub frmmain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.Save()
    End Sub

    Private Sub chkRun_CheckedChanged(sender As Object, e As EventArgs) Handles chkRun.CheckedChanged
        If chkRun.Checked = True Then
            If System.IO.Directory.Exists(My.Settings.JobDir) = False Then
                chkRun.Checked = False
                MsgBox("The set working directory does not exist", vbCritical)
                Exit Sub
            End If
            If System.IO.Directory.Exists(My.Settings.JobDir & "\Jobs") = False Then
                System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Jobs")
            End If
            If System.IO.Directory.Exists(My.Settings.JobDir & "\Clients") = False Then
                System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Clients")
            End If
            Clients = LoadClients(Clients)
            TaskWatcher.Path = My.Settings.JobDir & "\Jobs"
            ClientWatcher.Path = My.Settings.JobDir & "\Clients"
            CrawlJobFolder()
            AssigneJobs()
        End If
        Timer.Enabled = chkRun.Checked
        TaskWatcher.EnableRaisingEvents = chkRun.Checked
        ClientWatcher.EnableRaisingEvents = chkRun.Checked
    End Sub
End Class