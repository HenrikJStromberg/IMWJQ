<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmmain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TaskWatcher = New System.IO.FileSystemWatcher()
        Me.ClientWatcher = New System.IO.FileSystemWatcher()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSelJobDir = New System.Windows.Forms.Button()
        Me.lblJobDir = New System.Windows.Forms.Label()
        Me.chkRun = New System.Windows.Forms.CheckBox()
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TaskWatcher
        '
        Me.TaskWatcher.Filter = "*.csv*"
        Me.TaskWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite
        Me.TaskWatcher.SynchronizingObject = Me
        '
        'ClientWatcher
        '
        Me.ClientWatcher.SynchronizingObject = Me
        '
        'Timer
        '
        Me.Timer.Interval = 60000
        '
        'btnSelJobDir
        '
        Me.btnSelJobDir.Location = New System.Drawing.Point(12, 12)
        Me.btnSelJobDir.Name = "btnSelJobDir"
        Me.btnSelJobDir.Size = New System.Drawing.Size(133, 41)
        Me.btnSelJobDir.TabIndex = 0
        Me.btnSelJobDir.Text = "Select Job Directory"
        Me.btnSelJobDir.UseVisualStyleBackColor = True
        '
        'lblJobDir
        '
        Me.lblJobDir.AutoSize = True
        Me.lblJobDir.Location = New System.Drawing.Point(153, 28)
        Me.lblJobDir.Name = "lblJobDir"
        Me.lblJobDir.Size = New System.Drawing.Size(0, 13)
        Me.lblJobDir.TabIndex = 1
        '
        'chkRun
        '
        Me.chkRun.AutoSize = True
        Me.chkRun.Location = New System.Drawing.Point(17, 65)
        Me.chkRun.Name = "chkRun"
        Me.chkRun.Size = New System.Drawing.Size(80, 17)
        Me.chkRun.TabIndex = 2
        Me.chkRun.Text = "Run Server"
        Me.chkRun.UseVisualStyleBackColor = True
        '
        'frmmain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.chkRun)
        Me.Controls.Add(Me.lblJobDir)
        Me.Controls.Add(Me.btnSelJobDir)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmmain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IMWJQ Server"
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TaskWatcher As IO.FileSystemWatcher
    Friend WithEvents ClientWatcher As IO.FileSystemWatcher
    Friend WithEvents Timer As Windows.Forms.Timer
    Friend WithEvents lblJobDir As Windows.Forms.Label
    Friend WithEvents btnSelJobDir As Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog As Windows.Forms.FolderBrowserDialog
    Friend WithEvents chkRun As Windows.Forms.CheckBox
End Class
