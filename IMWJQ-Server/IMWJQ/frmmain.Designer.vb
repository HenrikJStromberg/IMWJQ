<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmmain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TaskWatcher = New System.IO.FileSystemWatcher()
        Me.ClientWatcher = New System.IO.FileSystemWatcher()
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TaskWatcher
        '
        Me.TaskWatcher.EnableRaisingEvents = True
        Me.TaskWatcher.Filter = "*.csv*"
        Me.TaskWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite
        Me.TaskWatcher.SynchronizingObject = Me
        '
        'ClientWatcher
        '
        Me.ClientWatcher.EnableRaisingEvents = True
        Me.ClientWatcher.SynchronizingObject = Me
        '
        'frmmain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Name = "frmmain"
        Me.Text = "frmmain"
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TaskWatcher As IO.FileSystemWatcher
    Friend WithEvents ClientWatcher As IO.FileSystemWatcher
End Class
