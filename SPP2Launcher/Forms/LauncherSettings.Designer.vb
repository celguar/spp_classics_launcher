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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LauncherSettings))
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button_DirSPP2 = New System.Windows.Forms.Button()
        Me.TextBox_DirSPP2 = New System.Windows.Forms.TextBox()
        Me.Label_ProjectDir = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBox_SqlLogLevel = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ComboBox_LogLevel = New System.Windows.Forms.ComboBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.CheckBox_UseAutoHints = New System.Windows.Forms.CheckBox()
        Me.CheckBox_UseConsoleBuffering = New System.Windows.Forms.CheckBox()
        Me.ComboBox_MessageFilter = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.CheckBox_UpdateRightNow = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox_Theme = New System.Windows.Forms.ComboBox()
        Me.ComboBox_FontSize = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_FontStyle = New System.Windows.Forms.ComboBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        resources.ApplyResources(Me.Button_OK, "Button_OK")
        Me.Button_OK.Name = "Button_OK"
        Me.ToolTip1.SetToolTip(Me.Button_OK, resources.GetString("Button_OK.ToolTip"))
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.Button_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.TextBox_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.Label_ProjectDir)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'Button_DirSPP2
        '
        resources.ApplyResources(Me.Button_DirSPP2, "Button_DirSPP2")
        Me.Button_DirSPP2.Name = "Button_DirSPP2"
        Me.ToolTip1.SetToolTip(Me.Button_DirSPP2, resources.GetString("Button_DirSPP2.ToolTip"))
        Me.Button_DirSPP2.UseVisualStyleBackColor = True
        '
        'TextBox_DirSPP2
        '
        resources.ApplyResources(Me.TextBox_DirSPP2, "TextBox_DirSPP2")
        Me.TextBox_DirSPP2.Name = "TextBox_DirSPP2"
        Me.ToolTip1.SetToolTip(Me.TextBox_DirSPP2, resources.GetString("TextBox_DirSPP2.ToolTip"))
        '
        'Label_ProjectDir
        '
        resources.ApplyResources(Me.Label_ProjectDir, "Label_ProjectDir")
        Me.Label_ProjectDir.Name = "Label_ProjectDir"
        Me.ToolTip1.SetToolTip(Me.Label_ProjectDir, resources.GetString("Label_ProjectDir.ToolTip"))
        '
        'Panel1
        '
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Name = "Panel1"
        Me.ToolTip1.SetToolTip(Me.Panel1, resources.GetString("Panel1.ToolTip"))
        '
        'TabControl1
        '
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.ToolTip1.SetToolTip(Me.TabControl1, resources.GetString("TabControl1.ToolTip"))
        '
        'TabPage1
        '
        resources.ApplyResources(Me.TabPage1, "TabPage1")
        Me.TabPage1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Name = "TabPage1"
        Me.ToolTip1.SetToolTip(Me.TabPage1, resources.GetString("TabPage1.ToolTip"))
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.ComboBox_SqlLogLevel)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.ComboBox_LogLevel)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox3, resources.GetString("GroupBox3.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'ComboBox_SqlLogLevel
        '
        resources.ApplyResources(Me.ComboBox_SqlLogLevel, "ComboBox_SqlLogLevel")
        Me.ComboBox_SqlLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SqlLogLevel.FormattingEnabled = True
        Me.ComboBox_SqlLogLevel.Name = "ComboBox_SqlLogLevel"
        Me.ToolTip1.SetToolTip(Me.ComboBox_SqlLogLevel, resources.GetString("ComboBox_SqlLogLevel.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'ComboBox_LogLevel
        '
        resources.ApplyResources(Me.ComboBox_LogLevel, "ComboBox_LogLevel")
        Me.ComboBox_LogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_LogLevel.FormattingEnabled = True
        Me.ComboBox_LogLevel.Name = "ComboBox_LogLevel"
        Me.ToolTip1.SetToolTip(Me.ComboBox_LogLevel, resources.GetString("ComboBox_LogLevel.ToolTip"))
        '
        'TabPage2
        '
        resources.ApplyResources(Me.TabPage2, "TabPage2")
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Name = "TabPage2"
        Me.ToolTip1.SetToolTip(Me.TabPage2, resources.GetString("TabPage2.ToolTip"))
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.CheckBox_UseAutoHints)
        Me.GroupBox4.Controls.Add(Me.CheckBox_UseConsoleBuffering)
        Me.GroupBox4.Controls.Add(Me.ComboBox_MessageFilter)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        '
        'CheckBox_UseAutoHints
        '
        resources.ApplyResources(Me.CheckBox_UseAutoHints, "CheckBox_UseAutoHints")
        Me.CheckBox_UseAutoHints.Name = "CheckBox_UseAutoHints"
        Me.ToolTip1.SetToolTip(Me.CheckBox_UseAutoHints, resources.GetString("CheckBox_UseAutoHints.ToolTip"))
        Me.CheckBox_UseAutoHints.UseVisualStyleBackColor = True
        '
        'CheckBox_UseConsoleBuffering
        '
        resources.ApplyResources(Me.CheckBox_UseConsoleBuffering, "CheckBox_UseConsoleBuffering")
        Me.CheckBox_UseConsoleBuffering.Name = "CheckBox_UseConsoleBuffering"
        Me.ToolTip1.SetToolTip(Me.CheckBox_UseConsoleBuffering, resources.GetString("CheckBox_UseConsoleBuffering.ToolTip"))
        Me.CheckBox_UseConsoleBuffering.UseVisualStyleBackColor = True
        '
        'ComboBox_MessageFilter
        '
        resources.ApplyResources(Me.ComboBox_MessageFilter, "ComboBox_MessageFilter")
        Me.ComboBox_MessageFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_MessageFilter.FormattingEnabled = True
        Me.ComboBox_MessageFilter.Items.AddRange(New Object() {resources.GetString("ComboBox_MessageFilter.Items"), resources.GetString("ComboBox_MessageFilter.Items1")})
        Me.ComboBox_MessageFilter.Name = "ComboBox_MessageFilter"
        Me.ToolTip1.SetToolTip(Me.ComboBox_MessageFilter, resources.GetString("ComboBox_MessageFilter.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.PictureBox1)
        Me.GroupBox2.Controls.Add(Me.CheckBox_UpdateRightNow)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.ComboBox_Theme)
        Me.GroupBox2.Controls.Add(Me.ComboBox_FontSize)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.ComboBox_FontStyle)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.info
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, resources.GetString("PictureBox1.ToolTip"))
        '
        'CheckBox_UpdateRightNow
        '
        resources.ApplyResources(Me.CheckBox_UpdateRightNow, "CheckBox_UpdateRightNow")
        Me.CheckBox_UpdateRightNow.Name = "CheckBox_UpdateRightNow"
        Me.ToolTip1.SetToolTip(Me.CheckBox_UpdateRightNow, resources.GetString("CheckBox_UpdateRightNow.ToolTip"))
        Me.CheckBox_UpdateRightNow.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'ComboBox_Theme
        '
        resources.ApplyResources(Me.ComboBox_Theme, "ComboBox_Theme")
        Me.ComboBox_Theme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Theme.FormattingEnabled = True
        Me.ComboBox_Theme.Name = "ComboBox_Theme"
        Me.ToolTip1.SetToolTip(Me.ComboBox_Theme, resources.GetString("ComboBox_Theme.ToolTip"))
        '
        'ComboBox_FontSize
        '
        resources.ApplyResources(Me.ComboBox_FontSize, "ComboBox_FontSize")
        Me.ComboBox_FontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontSize.FormattingEnabled = True
        Me.ComboBox_FontSize.Name = "ComboBox_FontSize"
        Me.ToolTip1.SetToolTip(Me.ComboBox_FontSize, resources.GetString("ComboBox_FontSize.ToolTip"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'ComboBox_FontStyle
        '
        resources.ApplyResources(Me.ComboBox_FontStyle, "ComboBox_FontStyle")
        Me.ComboBox_FontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontStyle.FormattingEnabled = True
        Me.ComboBox_FontStyle.Name = "ComboBox_FontStyle"
        Me.ToolTip1.SetToolTip(Me.ComboBox_FontStyle, resources.GetString("ComboBox_FontStyle.ToolTip"))
        '
        'LauncherSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Button_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LauncherSettings"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_OK As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents ComboBox_Theme As ComboBox
    Friend WithEvents ComboBox_FontSize As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox_FontStyle As ComboBox
    Friend WithEvents Button_DirSPP2 As Button
    Friend WithEvents TextBox_DirSPP2 As TextBox
    Friend WithEvents Label_ProjectDir As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ComboBox_SqlLogLevel As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBox_LogLevel As ComboBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents ComboBox_MessageFilter As ComboBox
    Friend WithEvents CheckBox_UseAutoHints As CheckBox
    Friend WithEvents CheckBox_UseConsoleBuffering As CheckBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents CheckBox_UpdateRightNow As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
