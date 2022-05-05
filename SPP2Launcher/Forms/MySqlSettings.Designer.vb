<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MySqlSettings
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MySqlSettings))
        Me.GroupBox_Parameters = New System.Windows.Forms.GroupBox()
        Me.PictureBox_InfoMySqlAutostart = New System.Windows.Forms.PictureBox()
        Me.CheckBox_MySqlAutostart = New System.Windows.Forms.CheckBox()
        Me.CheckBox_UseIntMySQL = New System.Windows.Forms.CheckBox()
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox_Databases = New System.Windows.Forms.GroupBox()
        Me.TextBox_Armory = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox_Realmd = New System.Windows.Forms.TextBox()
        Me.Label_Realmd = New System.Windows.Forms.Label()
        Me.TextBox_Playerbot = New System.Windows.Forms.TextBox()
        Me.Label_Playerbot = New System.Windows.Forms.Label()
        Me.TextBox_Mangos = New System.Windows.Forms.TextBox()
        Me.Label_Mangos = New System.Windows.Forms.Label()
        Me.TextBox_Logs = New System.Windows.Forms.TextBox()
        Me.Label_Logs = New System.Windows.Forms.Label()
        Me.TextBox_Characters = New System.Windows.Forms.TextBox()
        Me.Label_Chars = New System.Windows.Forms.Label()
        Me.GroupBox_Connection = New System.Windows.Forms.GroupBox()
        Me.TextBox_Password = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_UserName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_Port = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_Host = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Parameters.SuspendLayout()
        CType(Me.PictureBox_InfoMySqlAutostart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Databases.SuspendLayout()
        Me.GroupBox_Connection.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Parameters
        '
        resources.ApplyResources(Me.GroupBox_Parameters, "GroupBox_Parameters")
        Me.GroupBox_Parameters.Controls.Add(Me.PictureBox_InfoMySqlAutostart)
        Me.GroupBox_Parameters.Controls.Add(Me.CheckBox_MySqlAutostart)
        Me.GroupBox_Parameters.Name = "GroupBox_Parameters"
        Me.GroupBox_Parameters.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox_Parameters, resources.GetString("GroupBox_Parameters.ToolTip"))
        '
        'PictureBox_InfoMySqlAutostart
        '
        resources.ApplyResources(Me.PictureBox_InfoMySqlAutostart, "PictureBox_InfoMySqlAutostart")
        Me.PictureBox_InfoMySqlAutostart.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.info
        Me.PictureBox_InfoMySqlAutostart.Name = "PictureBox_InfoMySqlAutostart"
        Me.PictureBox_InfoMySqlAutostart.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox_InfoMySqlAutostart, resources.GetString("PictureBox_InfoMySqlAutostart.ToolTip"))
        '
        'CheckBox_MySqlAutostart
        '
        resources.ApplyResources(Me.CheckBox_MySqlAutostart, "CheckBox_MySqlAutostart")
        Me.CheckBox_MySqlAutostart.Name = "CheckBox_MySqlAutostart"
        Me.ToolTip1.SetToolTip(Me.CheckBox_MySqlAutostart, resources.GetString("CheckBox_MySqlAutostart.ToolTip"))
        Me.CheckBox_MySqlAutostart.UseVisualStyleBackColor = True
        '
        'CheckBox_UseIntMySQL
        '
        resources.ApplyResources(Me.CheckBox_UseIntMySQL, "CheckBox_UseIntMySQL")
        Me.CheckBox_UseIntMySQL.Name = "CheckBox_UseIntMySQL"
        Me.ToolTip1.SetToolTip(Me.CheckBox_UseIntMySQL, resources.GetString("CheckBox_UseIntMySQL.ToolTip"))
        Me.CheckBox_UseIntMySQL.UseVisualStyleBackColor = True
        '
        'Button_OK
        '
        resources.ApplyResources(Me.Button_OK, "Button_OK")
        Me.Button_OK.Name = "Button_OK"
        Me.ToolTip1.SetToolTip(Me.Button_OK, resources.GetString("Button_OK.ToolTip"))
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox_Databases
        '
        resources.ApplyResources(Me.GroupBox_Databases, "GroupBox_Databases")
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Armory)
        Me.GroupBox_Databases.Controls.Add(Me.Label5)
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Realmd)
        Me.GroupBox_Databases.Controls.Add(Me.Label_Realmd)
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Playerbot)
        Me.GroupBox_Databases.Controls.Add(Me.Label_Playerbot)
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Mangos)
        Me.GroupBox_Databases.Controls.Add(Me.Label_Mangos)
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Logs)
        Me.GroupBox_Databases.Controls.Add(Me.Label_Logs)
        Me.GroupBox_Databases.Controls.Add(Me.TextBox_Characters)
        Me.GroupBox_Databases.Controls.Add(Me.Label_Chars)
        Me.GroupBox_Databases.Name = "GroupBox_Databases"
        Me.GroupBox_Databases.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox_Databases, resources.GetString("GroupBox_Databases.ToolTip"))
        '
        'TextBox_Armory
        '
        resources.ApplyResources(Me.TextBox_Armory, "TextBox_Armory")
        Me.TextBox_Armory.Name = "TextBox_Armory"
        Me.ToolTip1.SetToolTip(Me.TextBox_Armory, resources.GetString("TextBox_Armory.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'TextBox_Realmd
        '
        resources.ApplyResources(Me.TextBox_Realmd, "TextBox_Realmd")
        Me.TextBox_Realmd.Name = "TextBox_Realmd"
        Me.ToolTip1.SetToolTip(Me.TextBox_Realmd, resources.GetString("TextBox_Realmd.ToolTip"))
        '
        'Label_Realmd
        '
        resources.ApplyResources(Me.Label_Realmd, "Label_Realmd")
        Me.Label_Realmd.Name = "Label_Realmd"
        Me.ToolTip1.SetToolTip(Me.Label_Realmd, resources.GetString("Label_Realmd.ToolTip"))
        '
        'TextBox_Playerbot
        '
        resources.ApplyResources(Me.TextBox_Playerbot, "TextBox_Playerbot")
        Me.TextBox_Playerbot.Name = "TextBox_Playerbot"
        Me.ToolTip1.SetToolTip(Me.TextBox_Playerbot, resources.GetString("TextBox_Playerbot.ToolTip"))
        '
        'Label_Playerbot
        '
        resources.ApplyResources(Me.Label_Playerbot, "Label_Playerbot")
        Me.Label_Playerbot.Name = "Label_Playerbot"
        Me.ToolTip1.SetToolTip(Me.Label_Playerbot, resources.GetString("Label_Playerbot.ToolTip"))
        '
        'TextBox_Mangos
        '
        resources.ApplyResources(Me.TextBox_Mangos, "TextBox_Mangos")
        Me.TextBox_Mangos.Name = "TextBox_Mangos"
        Me.ToolTip1.SetToolTip(Me.TextBox_Mangos, resources.GetString("TextBox_Mangos.ToolTip"))
        '
        'Label_Mangos
        '
        resources.ApplyResources(Me.Label_Mangos, "Label_Mangos")
        Me.Label_Mangos.Name = "Label_Mangos"
        Me.ToolTip1.SetToolTip(Me.Label_Mangos, resources.GetString("Label_Mangos.ToolTip"))
        '
        'TextBox_Logs
        '
        resources.ApplyResources(Me.TextBox_Logs, "TextBox_Logs")
        Me.TextBox_Logs.Name = "TextBox_Logs"
        Me.ToolTip1.SetToolTip(Me.TextBox_Logs, resources.GetString("TextBox_Logs.ToolTip"))
        '
        'Label_Logs
        '
        resources.ApplyResources(Me.Label_Logs, "Label_Logs")
        Me.Label_Logs.Name = "Label_Logs"
        Me.ToolTip1.SetToolTip(Me.Label_Logs, resources.GetString("Label_Logs.ToolTip"))
        '
        'TextBox_Characters
        '
        resources.ApplyResources(Me.TextBox_Characters, "TextBox_Characters")
        Me.TextBox_Characters.Name = "TextBox_Characters"
        Me.ToolTip1.SetToolTip(Me.TextBox_Characters, resources.GetString("TextBox_Characters.ToolTip"))
        '
        'Label_Chars
        '
        resources.ApplyResources(Me.Label_Chars, "Label_Chars")
        Me.Label_Chars.Name = "Label_Chars"
        Me.ToolTip1.SetToolTip(Me.Label_Chars, resources.GetString("Label_Chars.ToolTip"))
        '
        'GroupBox_Connection
        '
        resources.ApplyResources(Me.GroupBox_Connection, "GroupBox_Connection")
        Me.GroupBox_Connection.Controls.Add(Me.TextBox_Password)
        Me.GroupBox_Connection.Controls.Add(Me.Label1)
        Me.GroupBox_Connection.Controls.Add(Me.TextBox_UserName)
        Me.GroupBox_Connection.Controls.Add(Me.Label2)
        Me.GroupBox_Connection.Controls.Add(Me.TextBox_Port)
        Me.GroupBox_Connection.Controls.Add(Me.Label3)
        Me.GroupBox_Connection.Controls.Add(Me.TextBox_Host)
        Me.GroupBox_Connection.Controls.Add(Me.Label4)
        Me.GroupBox_Connection.Name = "GroupBox_Connection"
        Me.GroupBox_Connection.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox_Connection, resources.GetString("GroupBox_Connection.ToolTip"))
        '
        'TextBox_Password
        '
        resources.ApplyResources(Me.TextBox_Password, "TextBox_Password")
        Me.TextBox_Password.Name = "TextBox_Password"
        Me.ToolTip1.SetToolTip(Me.TextBox_Password, resources.GetString("TextBox_Password.ToolTip"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'TextBox_UserName
        '
        resources.ApplyResources(Me.TextBox_UserName, "TextBox_UserName")
        Me.TextBox_UserName.Name = "TextBox_UserName"
        Me.ToolTip1.SetToolTip(Me.TextBox_UserName, resources.GetString("TextBox_UserName.ToolTip"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'TextBox_Port
        '
        resources.ApplyResources(Me.TextBox_Port, "TextBox_Port")
        Me.TextBox_Port.Name = "TextBox_Port"
        Me.ToolTip1.SetToolTip(Me.TextBox_Port, resources.GetString("TextBox_Port.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'TextBox_Host
        '
        resources.ApplyResources(Me.TextBox_Host, "TextBox_Host")
        Me.TextBox_Host.Name = "TextBox_Host"
        Me.ToolTip1.SetToolTip(Me.TextBox_Host, resources.GetString("TextBox_Host.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'MySqlSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox_Databases)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox_Connection)
        Me.Controls.Add(Me.CheckBox_UseIntMySQL)
        Me.Controls.Add(Me.GroupBox_Parameters)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MySqlSettings"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox_Parameters.ResumeLayout(False)
        Me.GroupBox_Parameters.PerformLayout()
        CType(Me.PictureBox_InfoMySqlAutostart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Databases.ResumeLayout(False)
        Me.GroupBox_Databases.PerformLayout()
        Me.GroupBox_Connection.ResumeLayout(False)
        Me.GroupBox_Connection.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox_Parameters As GroupBox
    Friend WithEvents CheckBox_MySqlAutostart As CheckBox
    Friend WithEvents CheckBox_UseIntMySQL As CheckBox
    Friend WithEvents Button_OK As Button
    Friend WithEvents GroupBox_Databases As GroupBox
    Friend WithEvents TextBox_Realmd As TextBox
    Friend WithEvents Label_Realmd As Label
    Friend WithEvents TextBox_Playerbot As TextBox
    Friend WithEvents Label_Playerbot As Label
    Friend WithEvents TextBox_Mangos As TextBox
    Friend WithEvents Label_Mangos As Label
    Friend WithEvents TextBox_Logs As TextBox
    Friend WithEvents Label_Logs As Label
    Friend WithEvents TextBox_Characters As TextBox
    Friend WithEvents Label_Chars As Label
    Friend WithEvents GroupBox_Connection As GroupBox
    Friend WithEvents TextBox_Password As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox_UserName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox_Port As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox_Host As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox_Armory As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents PictureBox_InfoMySqlAutostart As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
