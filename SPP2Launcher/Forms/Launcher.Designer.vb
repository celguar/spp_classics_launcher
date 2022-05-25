<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Launcher
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Launcher))
        Me.StatusStrip_SPP2 = New System.Windows.Forms.StatusStrip()
        Me.TSSL_MySQL = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_Apache = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_Realm = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_World = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_ALL = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_Online = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSL_Count = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip_SPP2 = New System.Windows.Forms.MenuStrip()
        Me.TSMI_RunWow = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Server = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_WowAutoStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_ServerStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_ServerStop = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_CreateAutosave = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Server2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_MySQL = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_MySqlStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_MySqlRestart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_MySqlStop = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_MySQL1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_MySqlSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Apache = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_ApacheStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_ApacheRestart = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_ApacheStop = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Apache1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_ApacheSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Sever1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_ServerSwitcher = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Tools = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Bots = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Characters = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Saves = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_World = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_Updates = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Settings = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_LanguageTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_English = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Russian = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_Launcher = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMI_S1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_Reset = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotifyIcon_SPP2 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenu_SPP2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TSMI_OpenLauncher = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMI_CloseLauncher = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Console = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TSMI_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextBox_Command = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage_MySQL = New System.Windows.Forms.TabPage()
        Me.RichTextBox_ConsoleMySQL = New System.Windows.Forms.RichTextBox()
        Me.TabPage_Realmd = New System.Windows.Forms.TabPage()
        Me.RichTextBox_ConsoleRealmd = New System.Windows.Forms.RichTextBox()
        Me.TabPage_World = New System.Windows.Forms.TabPage()
        Me.RichTextBox_ConsoleWorld = New System.Windows.Forms.RichTextBox()
        Me.Button_UnlockAll = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StatusStrip_SPP2.SuspendLayout()
        Me.MenuStrip_SPP2.SuspendLayout()
        Me.ContextMenu_SPP2.SuspendLayout()
        Me.ContextMenuStrip_Console.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_MySQL.SuspendLayout()
        Me.TabPage_Realmd.SuspendLayout()
        Me.TabPage_World.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip_SPP2
        '
        Me.StatusStrip_SPP2.BackColor = System.Drawing.Color.LightGray
        Me.StatusStrip_SPP2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSSL_MySQL, Me.TSSL_Apache, Me.TSSL_Realm, Me.TSSL_World, Me.TSSL_ALL, Me.TSSL_Online, Me.TSSL_Count})
        resources.ApplyResources(Me.StatusStrip_SPP2, "StatusStrip_SPP2")
        Me.StatusStrip_SPP2.Name = "StatusStrip_SPP2"
        Me.StatusStrip_SPP2.ShowItemToolTips = True
        Me.StatusStrip_SPP2.SizingGrip = False
        '
        'TSSL_MySQL
        '
        resources.ApplyResources(Me.TSSL_MySQL, "TSSL_MySQL")
        Me.TSSL_MySQL.Name = "TSSL_MySQL"
        '
        'TSSL_Apache
        '
        resources.ApplyResources(Me.TSSL_Apache, "TSSL_Apache")
        Me.TSSL_Apache.Name = "TSSL_Apache"
        '
        'TSSL_Realm
        '
        resources.ApplyResources(Me.TSSL_Realm, "TSSL_Realm")
        Me.TSSL_Realm.Name = "TSSL_Realm"
        '
        'TSSL_World
        '
        resources.ApplyResources(Me.TSSL_World, "TSSL_World")
        Me.TSSL_World.Name = "TSSL_World"
        '
        'TSSL_ALL
        '
        Me.TSSL_ALL.Name = "TSSL_ALL"
        resources.ApplyResources(Me.TSSL_ALL, "TSSL_ALL")
        Me.TSSL_ALL.Spring = True
        '
        'TSSL_Online
        '
        Me.TSSL_Online.Name = "TSSL_Online"
        resources.ApplyResources(Me.TSSL_Online, "TSSL_Online")
        '
        'TSSL_Count
        '
        Me.TSSL_Count.Name = "TSSL_Count"
        resources.ApplyResources(Me.TSSL_Count, "TSSL_Count")
        '
        'MenuStrip_SPP2
        '
        Me.MenuStrip_SPP2.BackColor = System.Drawing.Color.LightGray
        Me.MenuStrip_SPP2.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip_SPP2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_RunWow, Me.TSMI_Server, Me.TSMI_Tools, Me.TSMI_Settings})
        resources.ApplyResources(Me.MenuStrip_SPP2, "MenuStrip_SPP2")
        Me.MenuStrip_SPP2.Name = "MenuStrip_SPP2"
        '
        'TSMI_RunWow
        '
        resources.ApplyResources(Me.TSMI_RunWow, "TSMI_RunWow")
        Me.TSMI_RunWow.Name = "TSMI_RunWow"
        '
        'TSMI_Server
        '
        Me.TSMI_Server.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_WowAutoStart, Me.TSMI_ServerStart, Me.TSMI_ServerStop, Me.TSMI_CreateAutosave, Me.TSMI_Server2, Me.TSMI_MySQL, Me.TSMI_Apache, Me.TSMI_Sever1, Me.TSMI_ServerSwitcher})
        resources.ApplyResources(Me.TSMI_Server, "TSMI_Server")
        Me.TSMI_Server.Name = "TSMI_Server"
        '
        'TSMI_WowAutoStart
        '
        Me.TSMI_WowAutoStart.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.protect
        Me.TSMI_WowAutoStart.Name = "TSMI_WowAutoStart"
        resources.ApplyResources(Me.TSMI_WowAutoStart, "TSMI_WowAutoStart")
        '
        'TSMI_ServerStart
        '
        resources.ApplyResources(Me.TSMI_ServerStart, "TSMI_ServerStart")
        Me.TSMI_ServerStart.Name = "TSMI_ServerStart"
        '
        'TSMI_ServerStop
        '
        resources.ApplyResources(Me.TSMI_ServerStop, "TSMI_ServerStop")
        Me.TSMI_ServerStop.Name = "TSMI_ServerStop"
        '
        'TSMI_CreateAutosave
        '
        Me.TSMI_CreateAutosave.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.save
        Me.TSMI_CreateAutosave.Name = "TSMI_CreateAutosave"
        resources.ApplyResources(Me.TSMI_CreateAutosave, "TSMI_CreateAutosave")
        '
        'TSMI_Server2
        '
        Me.TSMI_Server2.Name = "TSMI_Server2"
        resources.ApplyResources(Me.TSMI_Server2, "TSMI_Server2")
        '
        'TSMI_MySQL
        '
        Me.TSMI_MySQL.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_MySqlStart, Me.TSMI_MySqlRestart, Me.TSMI_MySqlStop, Me.TSMI_MySQL1, Me.TSMI_MySqlSettings})
        resources.ApplyResources(Me.TSMI_MySQL, "TSMI_MySQL")
        Me.TSMI_MySQL.Name = "TSMI_MySQL"
        '
        'TSMI_MySqlStart
        '
        resources.ApplyResources(Me.TSMI_MySqlStart, "TSMI_MySqlStart")
        Me.TSMI_MySqlStart.Name = "TSMI_MySqlStart"
        '
        'TSMI_MySqlRestart
        '
        resources.ApplyResources(Me.TSMI_MySqlRestart, "TSMI_MySqlRestart")
        Me.TSMI_MySqlRestart.Name = "TSMI_MySqlRestart"
        '
        'TSMI_MySqlStop
        '
        resources.ApplyResources(Me.TSMI_MySqlStop, "TSMI_MySqlStop")
        Me.TSMI_MySqlStop.Name = "TSMI_MySqlStop"
        '
        'TSMI_MySQL1
        '
        Me.TSMI_MySQL1.Name = "TSMI_MySQL1"
        resources.ApplyResources(Me.TSMI_MySQL1, "TSMI_MySQL1")
        '
        'TSMI_MySqlSettings
        '
        resources.ApplyResources(Me.TSMI_MySqlSettings, "TSMI_MySqlSettings")
        Me.TSMI_MySqlSettings.Name = "TSMI_MySqlSettings"
        '
        'TSMI_Apache
        '
        Me.TSMI_Apache.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_ApacheStart, Me.TSMI_ApacheRestart, Me.TSMI_ApacheStop, Me.TSMI_Apache1, Me.TSMI_ApacheSettings})
        resources.ApplyResources(Me.TSMI_Apache, "TSMI_Apache")
        Me.TSMI_Apache.Name = "TSMI_Apache"
        '
        'TSMI_ApacheStart
        '
        resources.ApplyResources(Me.TSMI_ApacheStart, "TSMI_ApacheStart")
        Me.TSMI_ApacheStart.Name = "TSMI_ApacheStart"
        '
        'TSMI_ApacheRestart
        '
        resources.ApplyResources(Me.TSMI_ApacheRestart, "TSMI_ApacheRestart")
        Me.TSMI_ApacheRestart.Name = "TSMI_ApacheRestart"
        '
        'TSMI_ApacheStop
        '
        resources.ApplyResources(Me.TSMI_ApacheStop, "TSMI_ApacheStop")
        Me.TSMI_ApacheStop.Name = "TSMI_ApacheStop"
        '
        'TSMI_Apache1
        '
        Me.TSMI_Apache1.Name = "TSMI_Apache1"
        resources.ApplyResources(Me.TSMI_Apache1, "TSMI_Apache1")
        '
        'TSMI_ApacheSettings
        '
        resources.ApplyResources(Me.TSMI_ApacheSettings, "TSMI_ApacheSettings")
        Me.TSMI_ApacheSettings.Name = "TSMI_ApacheSettings"
        '
        'TSMI_Sever1
        '
        Me.TSMI_Sever1.Name = "TSMI_Sever1"
        resources.ApplyResources(Me.TSMI_Sever1, "TSMI_Sever1")
        '
        'TSMI_ServerSwitcher
        '
        Me.TSMI_ServerSwitcher.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.warning
        Me.TSMI_ServerSwitcher.Name = "TSMI_ServerSwitcher"
        resources.ApplyResources(Me.TSMI_ServerSwitcher, "TSMI_ServerSwitcher")
        '
        'TSMI_Tools
        '
        Me.TSMI_Tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_Bots, Me.TSMI_Characters, Me.TSMI_Saves, Me.TSMI_World, Me.ToolStripMenuItem1, Me.TSMI_Updates})
        resources.ApplyResources(Me.TSMI_Tools, "TSMI_Tools")
        Me.TSMI_Tools.Name = "TSMI_Tools"
        '
        'TSMI_Bots
        '
        Me.TSMI_Bots.Name = "TSMI_Bots"
        resources.ApplyResources(Me.TSMI_Bots, "TSMI_Bots")
        '
        'TSMI_Characters
        '
        Me.TSMI_Characters.Name = "TSMI_Characters"
        resources.ApplyResources(Me.TSMI_Characters, "TSMI_Characters")
        '
        'TSMI_Saves
        '
        Me.TSMI_Saves.Name = "TSMI_Saves"
        resources.ApplyResources(Me.TSMI_Saves, "TSMI_Saves")
        '
        'TSMI_World
        '
        Me.TSMI_World.Name = "TSMI_World"
        resources.ApplyResources(Me.TSMI_World, "TSMI_World")
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'TSMI_Updates
        '
        Me.TSMI_Updates.Name = "TSMI_Updates"
        resources.ApplyResources(Me.TSMI_Updates, "TSMI_Updates")
        '
        'TSMI_Settings
        '
        Me.TSMI_Settings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_LanguageTool, Me.TSMI_Launcher, Me.TSMI_S1, Me.TSMI_Reset})
        resources.ApplyResources(Me.TSMI_Settings, "TSMI_Settings")
        Me.TSMI_Settings.Name = "TSMI_Settings"
        '
        'TSMI_LanguageTool
        '
        Me.TSMI_LanguageTool.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_English, Me.TSMI_Russian})
        Me.TSMI_LanguageTool.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.locale
        resources.ApplyResources(Me.TSMI_LanguageTool, "TSMI_LanguageTool")
        Me.TSMI_LanguageTool.Name = "TSMI_LanguageTool"
        '
        'TSMI_English
        '
        resources.ApplyResources(Me.TSMI_English, "TSMI_English")
        Me.TSMI_English.Name = "TSMI_English"
        '
        'TSMI_Russian
        '
        resources.ApplyResources(Me.TSMI_Russian, "TSMI_Russian")
        Me.TSMI_Russian.Name = "TSMI_Russian"
        '
        'TSMI_Launcher
        '
        resources.ApplyResources(Me.TSMI_Launcher, "TSMI_Launcher")
        Me.TSMI_Launcher.Name = "TSMI_Launcher"
        '
        'TSMI_S1
        '
        Me.TSMI_S1.Name = "TSMI_S1"
        resources.ApplyResources(Me.TSMI_S1, "TSMI_S1")
        '
        'TSMI_Reset
        '
        Me.TSMI_Reset.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.warning
        Me.TSMI_Reset.Name = "TSMI_Reset"
        resources.ApplyResources(Me.TSMI_Reset, "TSMI_Reset")
        '
        'NotifyIcon_SPP2
        '
        Me.NotifyIcon_SPP2.ContextMenuStrip = Me.ContextMenu_SPP2
        resources.ApplyResources(Me.NotifyIcon_SPP2, "NotifyIcon_SPP2")
        '
        'ContextMenu_SPP2
        '
        Me.ContextMenu_SPP2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_OpenLauncher, Me.ToolStripMenuItem7, Me.TSMI_CloseLauncher})
        Me.ContextMenu_SPP2.Name = "ContextMenu_SPP2"
        resources.ApplyResources(Me.ContextMenu_SPP2, "ContextMenu_SPP2")
        '
        'TSMI_OpenLauncher
        '
        Me.TSMI_OpenLauncher.Name = "TSMI_OpenLauncher"
        resources.ApplyResources(Me.TSMI_OpenLauncher, "TSMI_OpenLauncher")
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        resources.ApplyResources(Me.ToolStripMenuItem7, "ToolStripMenuItem7")
        '
        'TSMI_CloseLauncher
        '
        resources.ApplyResources(Me.TSMI_CloseLauncher, "TSMI_CloseLauncher")
        Me.TSMI_CloseLauncher.Name = "TSMI_CloseLauncher"
        '
        'ContextMenuStrip_Console
        '
        Me.ContextMenuStrip_Console.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMI_Copy})
        Me.ContextMenuStrip_Console.Name = "ContextMenuStrip_Console"
        resources.ApplyResources(Me.ContextMenuStrip_Console, "ContextMenuStrip_Console")
        '
        'TSMI_Copy
        '
        Me.TSMI_Copy.Name = "TSMI_Copy"
        resources.ApplyResources(Me.TSMI_Copy, "TSMI_Copy")
        '
        'TextBox_Command
        '
        Me.TextBox_Command.BackColor = System.Drawing.Color.Silver
        Me.TextBox_Command.BorderStyle = System.Windows.Forms.BorderStyle.None
        resources.ApplyResources(Me.TextBox_Command, "TextBox_Command")
        Me.TextBox_Command.ForeColor = System.Drawing.Color.Black
        Me.TextBox_Command.Name = "TextBox_Command"
        '
        'TabControl1
        '
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Controls.Add(Me.TabPage_MySQL)
        Me.TabControl1.Controls.Add(Me.TabPage_Realmd)
        Me.TabControl1.Controls.Add(Me.TabPage_World)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        '
        'TabPage_MySQL
        '
        Me.TabPage_MySQL.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_MySQL.Controls.Add(Me.RichTextBox_ConsoleMySQL)
        resources.ApplyResources(Me.TabPage_MySQL, "TabPage_MySQL")
        Me.TabPage_MySQL.Name = "TabPage_MySQL"
        '
        'RichTextBox_ConsoleMySQL
        '
        Me.RichTextBox_ConsoleMySQL.BackColor = System.Drawing.Color.Black
        Me.RichTextBox_ConsoleMySQL.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_ConsoleMySQL.ContextMenuStrip = Me.ContextMenuStrip_Console
        resources.ApplyResources(Me.RichTextBox_ConsoleMySQL, "RichTextBox_ConsoleMySQL")
        Me.RichTextBox_ConsoleMySQL.ForeColor = System.Drawing.Color.OrangeRed
        Me.RichTextBox_ConsoleMySQL.Name = "RichTextBox_ConsoleMySQL"
        Me.RichTextBox_ConsoleMySQL.ReadOnly = True
        '
        'TabPage_Realmd
        '
        Me.TabPage_Realmd.Controls.Add(Me.RichTextBox_ConsoleRealmd)
        resources.ApplyResources(Me.TabPage_Realmd, "TabPage_Realmd")
        Me.TabPage_Realmd.Name = "TabPage_Realmd"
        Me.TabPage_Realmd.UseVisualStyleBackColor = True
        '
        'RichTextBox_ConsoleRealmd
        '
        Me.RichTextBox_ConsoleRealmd.BackColor = System.Drawing.Color.Black
        Me.RichTextBox_ConsoleRealmd.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_ConsoleRealmd.ContextMenuStrip = Me.ContextMenuStrip_Console
        resources.ApplyResources(Me.RichTextBox_ConsoleRealmd, "RichTextBox_ConsoleRealmd")
        Me.RichTextBox_ConsoleRealmd.ForeColor = System.Drawing.Color.Green
        Me.RichTextBox_ConsoleRealmd.Name = "RichTextBox_ConsoleRealmd"
        Me.RichTextBox_ConsoleRealmd.ReadOnly = True
        '
        'TabPage_World
        '
        Me.TabPage_World.Controls.Add(Me.RichTextBox_ConsoleWorld)
        resources.ApplyResources(Me.TabPage_World, "TabPage_World")
        Me.TabPage_World.Name = "TabPage_World"
        Me.TabPage_World.UseVisualStyleBackColor = True
        '
        'RichTextBox_ConsoleWorld
        '
        resources.ApplyResources(Me.RichTextBox_ConsoleWorld, "RichTextBox_ConsoleWorld")
        Me.RichTextBox_ConsoleWorld.BackColor = System.Drawing.Color.Black
        Me.RichTextBox_ConsoleWorld.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_ConsoleWorld.ContextMenuStrip = Me.ContextMenuStrip_Console
        Me.RichTextBox_ConsoleWorld.ForeColor = System.Drawing.Color.DarkGreen
        Me.RichTextBox_ConsoleWorld.Name = "RichTextBox_ConsoleWorld"
        Me.RichTextBox_ConsoleWorld.ReadOnly = True
        '
        'Button_UnlockAll
        '
        resources.ApplyResources(Me.Button_UnlockAll, "Button_UnlockAll")
        Me.Button_UnlockAll.BackColor = System.Drawing.Color.Brown
        Me.Button_UnlockAll.ForeColor = System.Drawing.Color.Silver
        Me.Button_UnlockAll.Name = "Button_UnlockAll"
        Me.ToolTip1.SetToolTip(Me.Button_UnlockAll, resources.GetString("Button_UnlockAll.ToolTip"))
        Me.Button_UnlockAll.UseVisualStyleBackColor = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 15000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 100
        '
        'Launcher
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.Controls.Add(Me.Button_UnlockAll)
        Me.Controls.Add(Me.TextBox_Command)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip_SPP2)
        Me.Controls.Add(Me.MenuStrip_SPP2)
        Me.HelpButton = True
        Me.MainMenuStrip = Me.MenuStrip_SPP2
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Launcher"
        Me.StatusStrip_SPP2.ResumeLayout(False)
        Me.StatusStrip_SPP2.PerformLayout()
        Me.MenuStrip_SPP2.ResumeLayout(False)
        Me.MenuStrip_SPP2.PerformLayout()
        Me.ContextMenu_SPP2.ResumeLayout(False)
        Me.ContextMenuStrip_Console.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_MySQL.ResumeLayout(False)
        Me.TabPage_Realmd.ResumeLayout(False)
        Me.TabPage_World.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip_SPP2 As StatusStrip
    Friend WithEvents MenuStrip_SPP2 As MenuStrip
    Friend WithEvents TSMI_RunWow As ToolStripMenuItem
    Friend WithEvents TSMI_Settings As ToolStripMenuItem
    Friend WithEvents TSMI_Tools As ToolStripMenuItem
    Friend WithEvents TSMI_LanguageTool As ToolStripMenuItem
    Friend WithEvents NotifyIcon_SPP2 As NotifyIcon
    Friend WithEvents TSMI_English As ToolStripMenuItem
    Friend WithEvents TSMI_Russian As ToolStripMenuItem
    Friend WithEvents TSMI_Launcher As ToolStripMenuItem
    Friend WithEvents TSSL_Online As ToolStripStatusLabel
    Friend WithEvents TSSL_MySQL As ToolStripStatusLabel
    Friend WithEvents TSMI_Characters As ToolStripMenuItem
    Friend WithEvents TSMI_Saves As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents TSMI_Updates As ToolStripMenuItem
    Friend WithEvents TSMI_Server As ToolStripMenuItem
    Friend WithEvents TSMI_MySQL As ToolStripMenuItem
    Friend WithEvents TSMI_Apache As ToolStripMenuItem
    Friend WithEvents TSSL_Apache As ToolStripStatusLabel
    Friend WithEvents TSSL_Realm As ToolStripStatusLabel
    Friend WithEvents TSSL_World As ToolStripStatusLabel
    Friend WithEvents TSSL_ALL As ToolStripStatusLabel
    Friend WithEvents TSMI_MySqlStart As ToolStripMenuItem
    Friend WithEvents TSMI_MySqlRestart As ToolStripMenuItem
    Friend WithEvents TSMI_MySqlStop As ToolStripMenuItem
    Friend WithEvents TSMI_ApacheStart As ToolStripMenuItem
    Friend WithEvents TSMI_ApacheRestart As ToolStripMenuItem
    Friend WithEvents TSMI_ApacheStop As ToolStripMenuItem
    Friend WithEvents TSMI_Sever1 As ToolStripSeparator
    Friend WithEvents TSMI_ServerStart As ToolStripMenuItem
    Friend WithEvents TSMI_ServerStop As ToolStripMenuItem
    Friend WithEvents TSMI_Server2 As ToolStripSeparator
    Friend WithEvents TSMI_ServerSwitcher As ToolStripMenuItem
    Friend WithEvents TSMI_MySQL1 As ToolStripSeparator
    Friend WithEvents TSMI_MySqlSettings As ToolStripMenuItem
    Friend WithEvents TSMI_Apache1 As ToolStripSeparator
    Friend WithEvents TSMI_ApacheSettings As ToolStripMenuItem
    Friend WithEvents ContextMenu_SPP2 As ContextMenuStrip
    Friend WithEvents TSMI_OpenLauncher As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As ToolStripSeparator
    Friend WithEvents TSMI_CloseLauncher As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Console As ContextMenuStrip
    Friend WithEvents TSMI_Copy As ToolStripMenuItem
    Private WithEvents TextBox_Command As TextBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage_MySQL As TabPage
    Private WithEvents RichTextBox_ConsoleMySQL As RichTextBox
    Friend WithEvents TabPage_Realmd As TabPage
    Private WithEvents RichTextBox_ConsoleRealmd As RichTextBox
    Friend WithEvents TabPage_World As TabPage
    Private WithEvents RichTextBox_ConsoleWorld As RichTextBox
    Friend WithEvents TSSL_Count As ToolStripStatusLabel
    Friend WithEvents TSMI_WowAutoStart As ToolStripMenuItem
    Friend WithEvents TSMI_Bots As ToolStripMenuItem
    Friend WithEvents TSMI_World As ToolStripMenuItem
    Friend WithEvents TSMI_S1 As ToolStripSeparator
    Friend WithEvents TSMI_Reset As ToolStripMenuItem
    Friend WithEvents Button_UnlockAll As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TSMI_CreateAutosave As ToolStripMenuItem
End Class
