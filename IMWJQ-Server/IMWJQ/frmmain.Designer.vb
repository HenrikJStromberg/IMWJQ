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
        Me.cmdRestart = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MaxIDInput = New System.Windows.Forms.NumericUpDown()
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MaxIDInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TaskWatcher
        '
        Me.TaskWatcher.EnableRaisingEvents = True
        Me.TaskWatcher.Filter = "*.csv*"
        Me.TaskWatcher.IncludeSubdirectories = True
        Me.TaskWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite
        Me.TaskWatcher.SynchronizingObject = Me
        '
        'ClientWatcher
        '
        Me.ClientWatcher.EnableRaisingEvents = True
        Me.ClientWatcher.IncludeSubdirectories = True
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
        Me.chkRun.Location = New System.Drawing.Point(11, 153)
        Me.chkRun.Name = "chkRun"
        Me.chkRun.Size = New System.Drawing.Size(80, 17)
        Me.chkRun.TabIndex = 2
        Me.chkRun.Text = "Run Server"
        Me.chkRun.UseVisualStyleBackColor = True
        '
        'cmdRestart
        '
        Me.cmdRestart.Location = New System.Drawing.Point(11, 65)
        Me.cmdRestart.Name = "cmdRestart"
        Me.cmdRestart.Size = New System.Drawing.Size(134, 40)
        Me.cmdRestart.TabIndex = 3
        Me.cmdRestart.Text = "Restart Server"
        Me.cmdRestart.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 111)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Auto restart after n jobs:"
        '
        'MaxIDInput
        '
        Me.MaxIDInput.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.MaxIDInput.Location = New System.Drawing.Point(15, 127)
        Me.MaxIDInput.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.MaxIDInput.Name = "MaxIDInput"
        Me.MaxIDInput.Size = New System.Drawing.Size(116, 20)
        Me.MaxIDInput.TabIndex = 6
        Me.MaxIDInput.ThousandsSeparator = True
        Me.MaxIDInput.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'frmmain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MaxIDInput)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdRestart)
        Me.Controls.Add(Me.chkRun)
        Me.Controls.Add(Me.lblJobDir)
        Me.Controls.Add(Me.btnSelJobDir)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmmain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IMWJQ Server"
        CType(Me.TaskWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClientWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MaxIDInput, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents cmdRestart As Windows.Forms.Button
    Friend WithEvents MaxIDInput As Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As Windows.Forms.Label
End Class
