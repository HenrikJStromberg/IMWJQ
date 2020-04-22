Module Module1

    Sub Main()
        Dim reader As System.IO.StreamReader
        Dim Task() As String
        Dim Readbuffer As String
        reader = New IO.StreamReader(My.Application.CommandLineArgs().First)
        While Not reader.EndOfStream
            Readbuffer = reader.ReadLine
            If Not Readbuffer.Contains("=") Then Continue While
            Task = Readbuffer.Split("=")
            If Task(0) = "" Or Task(1) = "" Then Continue While
            If System.IO.File.Exists(Task(0)) Then
                System.IO.File.Delete(Task(0))
            End If
            If Not System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Task(0))) Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Task(0)))
            End If
            Dim writer As New System.IO.StreamWriter(Task(0))
            writer.Write(Task(1))
            writer.Close()
        End While
        reader.Close()
    End Sub

End Module
