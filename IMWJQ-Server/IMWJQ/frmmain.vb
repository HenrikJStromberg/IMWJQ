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
        Dim AssigendTo As String
        Dim Path As String
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
        Dim JobID As ULong
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
        For Each Job In Jobs 'Look for machine specific jobs
            If Job.Status = JobStatus.Waiting And Job.Client.Count > 0 Then
                For Each Client In Clients
                    If Client.Status = ClientStatus.Free And Job.Client.Contains(Client.Name) And Client.Software.Contains(Job.Software) Then
                        Client.Status = ClientStatus.Busy
                        Client.JobID = Job.ID
                        Job.Client.Clear()
                        Job.Client.Add(Client.Name)
                        Job.StartTime = Now
                        Job.Status = JobStatus.Working
                        Job.AssigendTo = Client.Name
                        WriteTask(Job)
                    End If
                Next
            End If
        Next

        For Each Job In Jobs 'Look for general jobs
            If Job.Status = JobStatus.Waiting And Job.Client.Count = 0 Then
                For Each Client In Clients
                    If Client.Status = ClientStatus.Free And Client.Software.Contains(Job.Software) And Job.Client.Count = 0 Then
                        Client.Status = ClientStatus.Busy
                        Client.JobID = Job.ID
                        Job.Client.Clear()
                        Job.AssigendTo = Client.Name
                        Job.StartTime = Now
                        WriteTask(Job)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub WriteTask(Job As Job)
        'Create empty client directories
        If Not System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & Job.AssigendTo) Then
            System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Clients\" & Job.AssigendTo)
        End If
        If System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "\Output") Then
            If Not DelFolder(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "\Output") Then
                Exit Sub
            End If
        End If
        System.IO.Directory.CreateDirectory(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "\Output")
        If System.IO.File.Exists(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "Job.txt") Then
            Try
                System.IO.File.Delete(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "Job.txt")
            Catch
                Exit Sub
            End Try
        End If
        System.IO.File.Copy(Job.Path & "\" & "Job.txt", My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "\Job.txt")
        Dim Writer As New System.IO.StreamWriter(My.Settings.JobDir & "\Clients\" & Job.AssigendTo & "\Job.txt", True)
        Writer.WriteLine()
        Writer.Write("TaskPath=" & Job.Path) 'ToDo: testen!
        Writer.Close()
    End Sub

    Private Sub CheckClients() 'ToDo: Overhaul
        For Each Client In Clients
            Dim eJob As Job
            Dim findex As Integer
            If System.IO.File.Exists(My.Settings.JobDir & "\Clients\" & Client.Name & ".txt") = False And Client.Status = ClientStatus.Busy Then
                'Check for finished jobs
                Client.Status = ClientStatus.Free
                findex = Jobs.FindIndex(Function(p) p.ID = Client.JobID)
                eJob = Jobs.Item(findex)
                eJob.Status = JobStatus.Finished
                Jobs.Item(findex) = eJob
                Client.JobID = Nothing
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
            nJob.Path = JobDirs(i)
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
                Jobs.Add(nJob)
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
                    Jobs.Add(nJob)
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
                    c.JobID = oc.JobID
                    c.Status = oc.Status
                    notFound = False
                    Exit For
                End If
            Next
            If notFound = True Then
                'Reset job as client has vanished
                Dim ClientsJobdex As Integer
                Dim modjob As Job
                ClientsJobdex = Jobs.FindIndex(Function(p) p.ID = oc.JobID)
                modjob = Jobs(ClientsJobdex)
                modjob.Client = Nothing
                modjob.Status = JobStatus.Waiting
                Jobs(ClientsJobdex) = modjob
            End If
        Next
        'generate communication folders for clients if needed
        For Each c In res
            If c.Status <> ClientStatus.Busy Then
                If System.IO.Directory.Exists(My.Settings.JobDir & "\Clients\" & c.Name) Then
                    DelFolder(My.Settings.JobDir & "\Clients\" & c.Name)
                End If
            End If
        Next
        Return Res
    End Function

    Private Function DelFolder(Path As String) As Boolean
        'Deletes a folder with its content
        Try
            If Not System.IO.Directory.Exists(Path) Then
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
                DelFolder(subDir)
            Next
            System.IO.Directory.Delete(Path)
            Return True
        Catch
            Return False
        End Try
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
        btnSelJobDir.Enabled = Not chkRun.Checked
        Timer.Enabled = chkRun.Checked
        TaskWatcher.EnableRaisingEvents = chkRun.Checked
        ClientWatcher.EnableRaisingEvents = chkRun.Checked
    End Sub
End Class