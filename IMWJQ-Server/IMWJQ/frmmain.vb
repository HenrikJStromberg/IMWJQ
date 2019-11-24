Public Class frmmain
    Private Clients As List(Of Client)
    Private Jobs As List(Of Job)

    Private Structure Job
        Dim Software As String
        Dim Ags As String
        Dim Owner As String
        Dim Timeout As Integer
        Dim Client As List(Of String)
        Dim Status As Byte
    End Structure

    Private Structure Client
        Dim Name As String
        Dim Software As List(Of String)
    End Structure

    Private Sub frmmain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Clients = LoadClients()
        Jobs = New List(Of Job)

        Jobs = LoadJobs(Jobs)
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

    Private Function LoadJobs(Path As String, res As List(Of Job)) As List(Of Job)
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
                Dim splitClients() As String
                splitClients = Split(currentRow(4), ",")
                Dim i As Integer
                For i = 0 To UBound(splitClients)
                    njob.Client.Add(splitClients(i))
                Next i
                njob.Status = 0
                res.Add(njob)
            End While
        End Using
        Return res
    End Function

End Class