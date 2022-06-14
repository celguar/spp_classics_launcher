<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Accounts
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Accounts))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Button_Next1 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_Change = New System.Windows.Forms.RadioButton()
        Me.RadioButton_Create = New System.Windows.Forms.RadioButton()
        Me.Button_Next2 = New System.Windows.Forms.Button()
        Me.Button_Back = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ComboBox_AccountSearch = New System.Windows.Forms.ComboBox()
        Me.CheckBox_ChangePassword = New System.Windows.Forms.CheckBox()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ComboBox_Expansion = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox_AccountType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button_Create = New System.Windows.Forms.Button()
        Me.TextBox_UserName = New System.Windows.Forms.TextBox()
        Me.TextBox_Password = New System.Windows.Forms.TextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        resources.ApplyResources(Me.SplitContainer1, "SplitContainer1")
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button_Next1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Button_Next2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Button_Back)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox2)
        '
        'Button_Next1
        '
        resources.ApplyResources(Me.Button_Next1, "Button_Next1")
        Me.Button_Next1.Name = "Button_Next1"
        Me.Button_Next1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.RadioButton_Change)
        Me.GroupBox1.Controls.Add(Me.RadioButton_Create)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'RadioButton_Change
        '
        resources.ApplyResources(Me.RadioButton_Change, "RadioButton_Change")
        Me.RadioButton_Change.Name = "RadioButton_Change"
        Me.RadioButton_Change.UseVisualStyleBackColor = True
        '
        'RadioButton_Create
        '
        resources.ApplyResources(Me.RadioButton_Create, "RadioButton_Create")
        Me.RadioButton_Create.Checked = True
        Me.RadioButton_Create.Name = "RadioButton_Create"
        Me.RadioButton_Create.TabStop = True
        Me.RadioButton_Create.UseVisualStyleBackColor = True
        '
        'Button_Next2
        '
        resources.ApplyResources(Me.Button_Next2, "Button_Next2")
        Me.Button_Next2.Name = "Button_Next2"
        Me.Button_Next2.UseVisualStyleBackColor = True
        '
        'Button_Back
        '
        resources.ApplyResources(Me.Button_Back, "Button_Back")
        Me.Button_Back.Name = "Button_Back"
        Me.Button_Back.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.ComboBox_AccountSearch)
        Me.GroupBox2.Controls.Add(Me.CheckBox_ChangePassword)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'ComboBox_AccountSearch
        '
        Me.ComboBox_AccountSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_AccountSearch.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_AccountSearch, "ComboBox_AccountSearch")
        Me.ComboBox_AccountSearch.Name = "ComboBox_AccountSearch"
        '
        'CheckBox_ChangePassword
        '
        resources.ApplyResources(Me.CheckBox_ChangePassword, "CheckBox_ChangePassword")
        Me.CheckBox_ChangePassword.Name = "CheckBox_ChangePassword"
        Me.CheckBox_ChangePassword.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        resources.ApplyResources(Me.SplitContainer2, "SplitContainer2")
        Me.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox3)
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.ComboBox_Expansion)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.ComboBox_AccountType)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Button_Create)
        Me.GroupBox3.Controls.Add(Me.TextBox_UserName)
        Me.GroupBox3.Controls.Add(Me.TextBox_Password)
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'ComboBox_Expansion
        '
        Me.ComboBox_Expansion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Expansion.FormattingEnabled = True
        Me.ComboBox_Expansion.Items.AddRange(New Object() {resources.GetString("ComboBox_Expansion.Items"), resources.GetString("ComboBox_Expansion.Items1"), resources.GetString("ComboBox_Expansion.Items2")})
        resources.ApplyResources(Me.ComboBox_Expansion, "ComboBox_Expansion")
        Me.ComboBox_Expansion.Name = "ComboBox_Expansion"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'ComboBox_AccountType
        '
        Me.ComboBox_AccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_AccountType.FormattingEnabled = True
        Me.ComboBox_AccountType.Items.AddRange(New Object() {resources.GetString("ComboBox_AccountType.Items"), resources.GetString("ComboBox_AccountType.Items1"), resources.GetString("ComboBox_AccountType.Items2"), resources.GetString("ComboBox_AccountType.Items3")})
        resources.ApplyResources(Me.ComboBox_AccountType, "ComboBox_AccountType")
        Me.ComboBox_AccountType.Name = "ComboBox_AccountType"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Button_Create
        '
        resources.ApplyResources(Me.Button_Create, "Button_Create")
        Me.Button_Create.Name = "Button_Create"
        Me.Button_Create.UseVisualStyleBackColor = True
        '
        'TextBox_UserName
        '
        resources.ApplyResources(Me.TextBox_UserName, "TextBox_UserName")
        Me.TextBox_UserName.Name = "TextBox_UserName"
        '
        'TextBox_Password
        '
        resources.ApplyResources(Me.TextBox_Password, "TextBox_Password")
        Me.TextBox_Password.Name = "TextBox_Password"
        '
        'Accounts
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.Controls.Add(Me.SplitContainer2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Accounts"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button_Next1 As Button
    Friend WithEvents RadioButton_Change As RadioButton
    Friend WithEvents RadioButton_Create As RadioButton
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Button_Back As Button
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Button_Next2 As Button
    Friend WithEvents SplitContainer2 As SplitContainer
    Private WithEvents GroupBox3 As GroupBox
    Private WithEvents Label4 As Label
    Private WithEvents ComboBox_Expansion As ComboBox
    Private WithEvents Label3 As Label
    Private WithEvents ComboBox_AccountType As ComboBox
    Private WithEvents Label2 As Label
    Private WithEvents Label1 As Label
    Private WithEvents Button_Create As Button
    Public WithEvents TextBox_UserName As TextBox
    Public WithEvents TextBox_Password As TextBox
    Friend WithEvents CheckBox_ChangePassword As CheckBox
    Friend WithEvents ComboBox_AccountSearch As ComboBox
End Class
