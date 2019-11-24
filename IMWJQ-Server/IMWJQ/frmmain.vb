Public Class frmmain
    Private Clients As List(Of Client)
    Private Jobs As List(Of Job)
    Private MaxID As ULong

    Private Structure Job
        Dim Software As String
        Dim Ags As String
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
    End Structure

    Private Enum ClientStatus
        Free
        Busy
        Dead
    End Enum

    Private Sub frmmain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Clients = LoadClients()
        Jobs = New List(Of Job)
        CrawlJobFolder()
        TaskWatcher.Path = My.Application.Info.DirectoryPath & "\input"
        ClientWatcher.Path = My.Application.Info.DirectoryPath & "\Clients"
        AssigneJobs()
    End Sub

    Private Sub AssigneJobs()
        For Each Job In Jobs 'Look For machine specific jobs
            If Job.Status = JobStatus.Waiting And Job.Client.Count > 0 Then
                For Each Client In Clients
                    If Client.Status = ClientStatus.Free And Job.Client.Contains(Client.Name) And Client.Software.Contains(Job.Software) Then
                        Client.Status = ClientStatus.Busy
                        Client.Job = Job
                        Job.Client.Clear()
                        Job.Client.Add(Client.Name)
                        Job.StartTime = Now
                        WriteTask(My.Application.Info.DirectoryPath & "\Clients\" & Client.Name & ".txt", Job)
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
                        WriteTask(My.Application.Info.DirectoryPath & "\Clients\" & Client.Name & ".txt", Job)
                    End If
                Next
            End If
        Next
        WrtieOutputs()
    End Sub

    Private Sub WriteTask(Path As String, Job As Job)
        Dim writer As New System.IO.StreamWriter(Path)
        writer.WriteLine(Job.Software & ";" & Job.Ags)
        writer.Close()
    End Sub

    Private Sub WrtieOutputs()
        Dim writer As System.IO.StreamWriter
        Dim clinetsstr As String
        writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Warteliste.csv")
        writer.WriteLine("Software;Argrumente;Verantwortlicher;Timeout;Client")
        For Each job In Jobs 'Create waiting list
            If job.Status = JobStatus.Waiting Then
                clinetsstr = ""
                For Each client In job.Client
                    clinetsstr = clinetsstr & "," & client
                Next
                clinetsstr = clinetsstr.Substring(1)
                writer.WriteLine(job.Software & ";" & job.Ags & ";" & job.Owner & ";" & job.Timeout & ";" & clinetsstr)
            End If
        Next job
        writer.Close()
        writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "In Arbeit.csv")
        writer.WriteLine("Software;Argrumente;Verantwortlicher;Timeout;Client;Startzeit")
        For Each job In Jobs 'Create working list
            If job.Status = JobStatus.Working Then
                writer.WriteLine(job.Software & ";" & job.Ags & ";" & job.Owner & ";" & job.Timeout & ";" & job.Client.First & ";" & job.StartTime.ToShortTimeString)
            End If
        Next
        writer.Close()
        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "Abgeschlossen.csv") = False Then
            writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Abgeschlossen.csv")
            writer.WriteLine("Software;Argrumente;Verantwortlicher;Timeout;Client;Startzeit")
            writer.Close()
        End If
        writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Abgeschlossen.csv", True)
        For Each job In Jobs 'Create working list
            If job.Status = JobStatus.Finished Then
                writer.WriteLine(job.Software & ";" & job.Ags & ";" & job.Owner & ";" & job.Timeout & ";" & job.Client.First & ";" & job.StartTime.ToShortTimeString)
            End If
        Next
        writer.Close()
    End Sub

    Private Sub CheckClients()
        For Each Client In Clients
            Dim eJob As Job
            Dim findex As Integer
            If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Clients\" & Client.Name & ".txt") = False And Client.Status = ClientStatus.Busy Then
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
        Dim JobFiles() As String
        JobFiles = System.IO.Directory.GetFiles(My.Application.Info.DirectoryPath & "\input")
        Dim i As Integer
        For i = 0 To UBound(JobFiles)
            If JobFiles(i).Substring(JobFiles(i).LastIndexOf(".")) = ".csv" Then
                Jobs.AddRange(LoadJobs(JobFiles(i)))
                System.IO.File.Delete(JobFiles(i))
            End If
        Next i
    End Sub

    Private Function LoadClients() As List(Of Client)
        Dim res As New List(Of Client)
        Using MyReader As New Microsoft.VisualBasic.FileIO.
        TextFieldParser(My.Application.Info.DirectoryPath & "\Clients.csv")
            MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
            MyReader.Delimiters = New String() {";"}
            Dim currentRow As String()
            MyReader.ReadFields()
            While Not MyReader.EndOfData
                Dim nclient As New Client
                currentRow = MyReader.ReadFields()
                nclient.Name = currentRow(0)
                nclient.Software = New List(Of String)
                Dim sw() As String
                sw = Split(currentRow(1), ",")
                Dim i As Integer
                For i = 0 To UBound(sw)
                    nclient.Software.Add(sw(i))
                Next i
                res.Add(nclient)
            End While
        End Using
        Return res
    End Function

    Private Function LoadJobs(Path As String) As List(Of Job)
        Dim res As New List(Of Job)
        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(Path)
            MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
            MyReader.Delimiters = New String() {";"}
            Dim currentRow As String()
            MyReader.ReadFields()
            While Not MyReader.EndOfData
                Dim njob As New Job
                currentRow = MyReader.ReadFields()
                njob.Software = currentRow(0)
                njob.Ags = currentRow(1)
                njob.Owner = currentRow(2)
                njob.Timeout = currentRow(3)
                njob.Client = New List(Of String)
                njob.ID = MaxID + 1
                MaxID = MaxID + 1
                Dim splitClients() As String
                splitClients = Split(currentRow(4), ",")
                Dim i As Integer
                For i = 0 To UBound(splitClients)
                    njob.Client.Add(splitClients(i))
                Next i
                njob.Status = JobStatus.Waiting
                res.Add(njob)
            End While
        End Using
        Return res
    End Function

    Private Sub TaskWatcher_Changed(sender As Object, e As IO.FileSystemEventArgs) Handles TaskWatcher.Changed
        CrawlJobFolder()
        AssigneJobs()
    End Sub

    Private Sub ClientWatcher_Changed(sender As Object, e As IO.FileSystemEventArgs) Handles ClientWatcher.Changed
        CheckClients()
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
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
                If System.IO.File.Exists(My.Application.Info.DirectoryPath & "Fehlgeschlagen.csv") = False Then
                    writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Abgeschlossen.csv")
                    writer.WriteLine("Software;Argrumente;Verantwortlicher;Timeout;Client;Startzeit")
                    writer.Close()
                End If
                writer = New IO.StreamWriter(My.Application.Info.DirectoryPath & "Abgeschlossen.csv", True)
                writer.WriteLine(client.Job.Software & ";" & client.Job.Ags & ";" & client.Job.Owner & ";" & client.Job.Timeout & ";" & client.Name & ";" & client.Job.StartTime)
                writer.Close()
                client.Job = Nothing
            End If
        Next
    End Sub
End Class