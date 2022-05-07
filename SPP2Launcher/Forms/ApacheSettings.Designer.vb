<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApacheSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ApacheSettings))
        Me.CheckBox_UseIntApache = New System.Windows.Forms.CheckBox()
        Me.GroupBox_Parameters = New System.Windows.Forms.GroupBox()
        Me.CheckBox_ApacheAutostart = New System.Windows.Forms.CheckBox()
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ComboBox_Host = New System.Windows.Forms.ComboBox()
        Me.TextBox_Port = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_Host = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox_Parameters.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CheckBox_UseIntApache
        '
        resources.ApplyResources(Me.CheckBox_UseIntApache, "CheckBox_UseIntApache")
        Me.CheckBox_UseIntApache.Name = "CheckBox_UseIntApache"
        Me.CheckBox_UseIntApache.UseVisualStyleBackColor = True
        '
        'GroupBox_Parameters
        '
        Me.GroupBox_Parameters.Controls.Add(Me.CheckBox_ApacheAutostart)
        resources.ApplyResources(Me.GroupBox_Parameters, "GroupBox_Parameters")
        Me.GroupBox_Parameters.Name = "GroupBox_Parameters"
        Me.GroupBox_Parameters.TabStop = False
        '
        'CheckBox_ApacheAutostart
        '
        resources.ApplyResources(Me.CheckBox_ApacheAutostart, "CheckBox_ApacheAutostart")
        Me.CheckBox_ApacheAutostart.Name = "CheckBox_ApacheAutostart"
        Me.CheckBox_ApacheAutostart.UseVisualStyleBackColor = True
        '
        'Button_OK
        '
        resources.ApplyResources(Me.Button_OK, "Button_OK")
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ComboBox_Host)
        Me.GroupBox1.Controls.Add(Me.TextBox_Port)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.TextBox_Host)
        Me.GroupBox1.Controls.Add(Me.Label4)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'ComboBox_Host
        '
        Me.ComboBox_Host.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Host.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_Host, "ComboBox_Host")
        Me.ComboBox_Host.Name = "ComboBox_Host"
        '
        'TextBox_Port
        '
        resources.ApplyResources(Me.TextBox_Port, "TextBox_Port")
        Me.TextBox_Port.Name = "TextBox_Port"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'TextBox_Host
        '
        resources.ApplyResources(Me.TextBox_Host, "TextBox_Host")
        Me.TextBox_Host.Name = "TextBox_Host"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'ApacheSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox_Parameters)
        Me.Controls.Add(Me.CheckBox_UseIntApache)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ApacheSettings"
        Me.GroupBox_Parameters.ResumeLayout(False)
        Me.GroupBox_Parameters.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CheckBox_UseIntApache As CheckBox
    Friend WithEvents GroupBox_Parameters As GroupBox
    Friend WithEvents CheckBox_ApacheAutostart As CheckBox
    Friend WithEvents Button_OK As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TextBox_Port As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox_Host As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBox_Host As ComboBox
End Class
