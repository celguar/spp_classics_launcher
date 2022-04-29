<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ServerSettings
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServerSettings))
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox_ServerSettings = New System.Windows.Forms.GroupBox()
        Me.CheckBox_ServerAutostart = New System.Windows.Forms.CheckBox()
        Me.GroupBox_ServerSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        resources.ApplyResources(Me.Button_OK, "Button_OK")
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox_ServerSettings
        '
        resources.ApplyResources(Me.GroupBox_ServerSettings, "GroupBox_ServerSettings")
        Me.GroupBox_ServerSettings.Controls.Add(Me.CheckBox_ServerAutostart)
        Me.GroupBox_ServerSettings.Name = "GroupBox_ServerSettings"
        Me.GroupBox_ServerSettings.TabStop = False
        '
        'CheckBox_ServerAutostart
        '
        resources.ApplyResources(Me.CheckBox_ServerAutostart, "CheckBox_ServerAutostart")
        Me.CheckBox_ServerAutostart.Name = "CheckBox_ServerAutostart"
        Me.CheckBox_ServerAutostart.UseVisualStyleBackColor = True
        '
        'ServerSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox_ServerSettings)
        Me.Controls.Add(Me.Button_OK)
        Me.Name = "ServerSettings"
        Me.GroupBox_ServerSettings.ResumeLayout(False)
        Me.GroupBox_ServerSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_OK As Button
    Friend WithEvents GroupBox_ServerSettings As GroupBox
    Friend WithEvents CheckBox_ServerAutostart As CheckBox
End Class
