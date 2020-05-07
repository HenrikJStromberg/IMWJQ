<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.FolderDlg = New System.Windows.Forms.FolderBrowserDialog()
        Me.OpnDlg = New System.Windows.Forms.OpenFileDialog()
        Me.SavDlg = New System.Windows.Forms.SaveFileDialog()
        Me.btnLoadIPScript = New System.Windows.Forms.Button()
        Me.btnLoadCSV = New System.Windows.Forms.Button()
        Me.chkDelAnsysFiles = New System.Windows.Forms.CheckBox()
        Me.txtWorkDir = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnOpnPrj = New System.Windows.Forms.Button()
        Me.txtPyCode = New System.Windows.Forms.TextBox()
        Me.btnGenScripts = New System.Windows.Forms.Button()
        Me.opncsv = New System.Windows.Forms.OpenFileDialog()
        Me.opnPrj = New System.Windows.Forms.OpenFileDialog()
        Me.CVSSepComma = New System.Windows.Forms.RadioButton()
        Me.CSVSepDot = New System.Windows.Forms.RadioButton()
        Me.CSVSepAuto = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpnDlg
        '
        Me.OpnDlg.Filter = "WB-Iron Python Script|*.wbjn|Alle Dateien|*.*"
        '
        'SavDlg
        '
        Me.SavDlg.Filter = "WB-Iron Python Script|*.wbjn|Alle Dateien|*.*"
        '
        'btnLoadIPScript
        '
        Me.btnLoadIPScript.Location = New System.Drawing.Point(3, 4)
        Me.btnLoadIPScript.Name = "btnLoadIPScript"
        Me.btnLoadIPScript.Size = New System.Drawing.Size(165, 55)
        Me.btnLoadIPScript.TabIndex = 0
        Me.btnLoadIPScript.Text = "Load Iron Python file"
        Me.btnLoadIPScript.UseVisualStyleBackColor = True
        '
        'btnLoadCSV
        '
        Me.btnLoadCSV.Location = New System.Drawing.Point(3, 65)
        Me.btnLoadCSV.Name = "btnLoadCSV"
        Me.btnLoadCSV.Size = New System.Drawing.Size(165, 55)
        Me.btnLoadCSV.TabIndex = 1
        Me.btnLoadCSV.Text = "Load CSV table"
        Me.btnLoadCSV.UseVisualStyleBackColor = True
        '
        'chkDelAnsysFiles
        '
        Me.chkDelAnsysFiles.AutoSize = True
        Me.chkDelAnsysFiles.Location = New System.Drawing.Point(6, 126)
        Me.chkDelAnsysFiles.Name = "chkDelAnsysFiles"
        Me.chkDelAnsysFiles.Size = New System.Drawing.Size(164, 17)
        Me.chkDelAnsysFiles.TabIndex = 2
        Me.chkDelAnsysFiles.Text = "Ansys Dateien immer löschen"
        Me.chkDelAnsysFiles.UseVisualStyleBackColor = True
        '
        'txtWorkDir
        '
        Me.txtWorkDir.Location = New System.Drawing.Point(1, 162)
        Me.txtWorkDir.Name = "txtWorkDir"
        Me.txtWorkDir.Size = New System.Drawing.Size(167, 20)
        Me.txtWorkDir.TabIndex = 3
        Me.txtWorkDir.Text = "C:\AnsysWorker"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Arbeitsverzeichnis:"
        '
        'btnOpnPrj
        '
        Me.btnOpnPrj.Location = New System.Drawing.Point(3, 188)
        Me.btnOpnPrj.Name = "btnOpnPrj"
        Me.btnOpnPrj.Size = New System.Drawing.Size(165, 55)
        Me.btnOpnPrj.TabIndex = 5
        Me.btnOpnPrj.Text = "Select Ansys projekt file"
        Me.btnOpnPrj.UseVisualStyleBackColor = True
        '
        'txtPyCode
        '
        Me.txtPyCode.Dock = System.Windows.Forms.DockStyle.Right
        Me.txtPyCode.Location = New System.Drawing.Point(173, 0)
        Me.txtPyCode.Multiline = True
        Me.txtPyCode.Name = "txtPyCode"
        Me.txtPyCode.Size = New System.Drawing.Size(614, 506)
        Me.txtPyCode.TabIndex = 6
        '
        'btnGenScripts
        '
        Me.btnGenScripts.Location = New System.Drawing.Point(3, 249)
        Me.btnGenScripts.Name = "btnGenScripts"
        Me.btnGenScripts.Size = New System.Drawing.Size(165, 55)
        Me.btnGenScripts.TabIndex = 7
        Me.btnGenScripts.Text = "Generate output files"
        Me.btnGenScripts.UseVisualStyleBackColor = True
        '
        'opncsv
        '
        Me.opncsv.Filter = "CSV-Datei|*.csv|Alle Dateien|*.*"
        '
        'opnPrj
        '
        Me.opnPrj.Filter = "Ansys Projekt Datei|*.wbpj|Alle Dateien|*.*"
        '
        'CVSSepComma
        '
        Me.CVSSepComma.AutoSize = True
        Me.CVSSepComma.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CVSSepComma.Location = New System.Drawing.Point(6, 20)
        Me.CVSSepComma.Name = "CVSSepComma"
        Me.CVSSepComma.Size = New System.Drawing.Size(33, 28)
        Me.CVSSepComma.TabIndex = 8
        Me.CVSSepComma.Text = ","
        Me.CVSSepComma.UseVisualStyleBackColor = True
        '
        'CSVSepDot
        '
        Me.CSVSepDot.AutoSize = True
        Me.CSVSepDot.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CSVSepDot.Location = New System.Drawing.Point(45, 20)
        Me.CSVSepDot.Name = "CSVSepDot"
        Me.CSVSepDot.Size = New System.Drawing.Size(33, 28)
        Me.CSVSepDot.TabIndex = 9
        Me.CSVSepDot.Text = "."
        Me.CSVSepDot.UseVisualStyleBackColor = True
        '
        'CSVSepAuto
        '
        Me.CSVSepAuto.AutoSize = True
        Me.CSVSepAuto.Checked = True
        Me.CSVSepAuto.Location = New System.Drawing.Point(6, 54)
        Me.CSVSepAuto.Name = "CSVSepAuto"
        Me.CSVSepAuto.Size = New System.Drawing.Size(95, 17)
        Me.CSVSepAuto.TabIndex = 10
        Me.CSVSepAuto.TabStop = True
        Me.CSVSepAuto.Text = "System Culture"
        Me.CSVSepAuto.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CSVSepAuto)
        Me.GroupBox1.Controls.Add(Me.CVSSepComma)
        Me.GroupBox1.Controls.Add(Me.CSVSepDot)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 310)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(114, 85)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CSV Seperator"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 506)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnGenScripts)
        Me.Controls.Add(Me.txtPyCode)
        Me.Controls.Add(Me.btnOpnPrj)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtWorkDir)
        Me.Controls.Add(Me.chkDelAnsysFiles)
        Me.Controls.Add(Me.btnLoadCSV)
        Me.Controls.Add(Me.btnLoadIPScript)
        Me.Name = "frmMain"
        Me.Text = "AnsysWB Job Maker"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FolderDlg As FolderBrowserDialog
    Friend WithEvents OpnDlg As OpenFileDialog
    Friend WithEvents SavDlg As SaveFileDialog
    Friend WithEvents btnLoadIPScript As Button
    Friend WithEvents btnLoadCSV As Button
    Friend WithEvents chkDelAnsysFiles As CheckBox
    Friend WithEvents txtWorkDir As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnOpnPrj As Button
    Friend WithEvents txtPyCode As TextBox
    Friend WithEvents btnGenScripts As Button
    Friend WithEvents opncsv As OpenFileDialog
    Friend WithEvents opnPrj As OpenFileDialog
    Friend WithEvents CVSSepComma As RadioButton
    Friend WithEvents CSVSepDot As RadioButton
    Friend WithEvents CSVSepAuto As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
End Class
