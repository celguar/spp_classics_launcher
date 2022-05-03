<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LauncherSettings
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LauncherSettings))
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ComboBox_Theme = New System.Windows.Forms.ComboBox()
        Me.ComboBox_FontSize = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox_FontStyle = New System.Windows.Forms.ComboBox()
        Me.Button_DirSPP2 = New System.Windows.Forms.Button()
        Me.TextBox_DirSPP2 = New System.Windows.Forms.TextBox()
        Me.Label_ProjectDir = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        resources.ApplyResources(Me.Button_OK, "Button_OK")
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.ComboBox_Theme)
        Me.GroupBox1.Controls.Add(Me.ComboBox_FontSize)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.ComboBox_FontStyle)
        Me.GroupBox1.Controls.Add(Me.Button_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.TextBox_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.Label_ProjectDir)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'ComboBox_Theme
        '
        resources.ApplyResources(Me.ComboBox_Theme, "ComboBox_Theme")
        Me.ComboBox_Theme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Theme.FormattingEnabled = True
        Me.ComboBox_Theme.Name = "ComboBox_Theme"
        '
        'ComboBox_FontSize
        '
        resources.ApplyResources(Me.ComboBox_FontSize, "ComboBox_FontSize")
        Me.ComboBox_FontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontSize.FormattingEnabled = True
        Me.ComboBox_FontSize.Name = "ComboBox_FontSize"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'ComboBox_FontStyle
        '
        resources.ApplyResources(Me.ComboBox_FontStyle, "ComboBox_FontStyle")
        Me.ComboBox_FontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontStyle.FormattingEnabled = True
        Me.ComboBox_FontStyle.Name = "ComboBox_FontStyle"
        '
        'Button_DirSPP2
        '
        resources.ApplyResources(Me.Button_DirSPP2, "Button_DirSPP2")
        Me.Button_DirSPP2.Name = "Button_DirSPP2"
        Me.Button_DirSPP2.UseVisualStyleBackColor = True
        '
        'TextBox_DirSPP2
        '
        resources.ApplyResources(Me.TextBox_DirSPP2, "TextBox_DirSPP2")
        Me.TextBox_DirSPP2.Name = "TextBox_DirSPP2"
        '
        'Label_ProjectDir
        '
        resources.ApplyResources(Me.Label_ProjectDir, "Label_ProjectDir")
        Me.Label_ProjectDir.Name = "Label_ProjectDir"
        '
        'LauncherSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LauncherSettings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_OK As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button_DirSPP2 As Button
    Friend WithEvents TextBox_DirSPP2 As TextBox
    Friend WithEvents Label_ProjectDir As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox_FontStyle As ComboBox
    Friend WithEvents ComboBox_FontSize As ComboBox
    Friend WithEvents ComboBox_Theme As ComboBox
End Class
