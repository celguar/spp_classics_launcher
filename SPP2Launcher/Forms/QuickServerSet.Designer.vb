<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QuickServerSet
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QuickServerSet))
        Me.Button_Change = New System.Windows.Forms.Button()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.ComboBox_GameType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_ServerName = New System.Windows.Forms.TextBox()
        Me.ComboBox_RealmId = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_BindIP = New System.Windows.Forms.ComboBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Change
        '
        resources.ApplyResources(Me.Button_Change, "Button_Change")
        Me.Button_Change.Name = "Button_Change"
        Me.Button_Change.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        resources.ApplyResources(Me.groupBox1, "groupBox1")
        Me.groupBox1.Controls.Add(Me.ComboBox_GameType)
        Me.groupBox1.Controls.Add(Me.Label4)
        Me.groupBox1.Controls.Add(Me.Label3)
        Me.groupBox1.Controls.Add(Me.TextBox_ServerName)
        Me.groupBox1.Controls.Add(Me.ComboBox_RealmId)
        Me.groupBox1.Controls.Add(Me.Label2)
        Me.groupBox1.Controls.Add(Me.ComboBox_BindIP)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.Button_Change)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.TabStop = False
        '
        'ComboBox_GameType
        '
        Me.ComboBox_GameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_GameType.FormattingEnabled = True
        Me.ComboBox_GameType.Items.AddRange(New Object() {resources.GetString("ComboBox_GameType.Items"), resources.GetString("ComboBox_GameType.Items1"), resources.GetString("ComboBox_GameType.Items2"), resources.GetString("ComboBox_GameType.Items3"), resources.GetString("ComboBox_GameType.Items4")})
        resources.ApplyResources(Me.ComboBox_GameType, "ComboBox_GameType")
        Me.ComboBox_GameType.Name = "ComboBox_GameType"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'TextBox_ServerName
        '
        resources.ApplyResources(Me.TextBox_ServerName, "TextBox_ServerName")
        Me.TextBox_ServerName.Name = "TextBox_ServerName"
        '
        'ComboBox_RealmId
        '
        Me.ComboBox_RealmId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_RealmId.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_RealmId, "ComboBox_RealmId")
        Me.ComboBox_RealmId.Name = "ComboBox_RealmId"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'ComboBox_BindIP
        '
        Me.ComboBox_BindIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_BindIP.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_BindIP, "ComboBox_BindIP")
        Me.ComboBox_BindIP.Name = "ComboBox_BindIP"
        '
        'label1
        '
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'QuickServerSet
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "QuickServerSet"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents Button_Change As Button
    Private WithEvents groupBox1 As GroupBox
    Private WithEvents label1 As Label
    Private WithEvents ComboBox_BindIP As ComboBox
    Private WithEvents ComboBox_RealmId As ComboBox
    Private WithEvents Label2 As Label
    Private WithEvents ComboBox_GameType As ComboBox
    Private WithEvents Label4 As Label
    Private WithEvents Label3 As Label
    Friend WithEvents TextBox_ServerName As TextBox
End Class
