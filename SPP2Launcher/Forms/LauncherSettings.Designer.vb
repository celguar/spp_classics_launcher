﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.ComboBox_MessageFilter = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox_Theme = New System.Windows.Forms.ComboBox()
        Me.ComboBox_FontSize = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_FontStyle = New System.Windows.Forms.ComboBox()
        Me.CheckBox_UseConsoleBuffering = New System.Windows.Forms.CheckBox()
        Me.CheckBox_UseAutoHints = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
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
        Me.GroupBox1.Controls.Add(Me.Button_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.TextBox_DirSPP2)
        Me.GroupBox1.Controls.Add(Me.Label_ProjectDir)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
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
        'Panel1
        '
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Name = "Panel1"
        '
        'TabControl1
        '
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        resources.ApplyResources(Me.TabPage1, "TabPage1")
        Me.TabPage1.Name = "TabPage1"
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
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'ComboBox_SqlLogLevel
        '
        Me.ComboBox_SqlLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SqlLogLevel.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_SqlLogLevel, "ComboBox_SqlLogLevel")
        Me.ComboBox_SqlLogLevel.Name = "ComboBox_SqlLogLevel"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'ComboBox_LogLevel
        '
        Me.ComboBox_LogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_LogLevel.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_LogLevel, "ComboBox_LogLevel")
        Me.ComboBox_LogLevel.Name = "ComboBox_LogLevel"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        resources.ApplyResources(Me.TabPage2, "TabPage2")
        Me.TabPage2.Name = "TabPage2"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CheckBox_UseAutoHints)
        Me.GroupBox4.Controls.Add(Me.CheckBox_UseConsoleBuffering)
        Me.GroupBox4.Controls.Add(Me.ComboBox_MessageFilter)
        Me.GroupBox4.Controls.Add(Me.Label5)
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        '
        'ComboBox_MessageFilter
        '
        Me.ComboBox_MessageFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_MessageFilter.FormattingEnabled = True
        Me.ComboBox_MessageFilter.Items.AddRange(New Object() {resources.GetString("ComboBox_MessageFilter.Items"), resources.GetString("ComboBox_MessageFilter.Items1")})
        resources.ApplyResources(Me.ComboBox_MessageFilter, "ComboBox_MessageFilter")
        Me.ComboBox_MessageFilter.Name = "ComboBox_MessageFilter"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.ComboBox_Theme)
        Me.GroupBox2.Controls.Add(Me.ComboBox_FontSize)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.ComboBox_FontStyle)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'ComboBox_Theme
        '
        Me.ComboBox_Theme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Theme.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_Theme, "ComboBox_Theme")
        Me.ComboBox_Theme.Name = "ComboBox_Theme"
        '
        'ComboBox_FontSize
        '
        Me.ComboBox_FontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontSize.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_FontSize, "ComboBox_FontSize")
        Me.ComboBox_FontSize.Name = "ComboBox_FontSize"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'ComboBox_FontStyle
        '
        Me.ComboBox_FontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FontStyle.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_FontStyle, "ComboBox_FontStyle")
        Me.ComboBox_FontStyle.Name = "ComboBox_FontStyle"
        '
        'CheckBox_UseConsoleBuffering
        '
        resources.ApplyResources(Me.CheckBox_UseConsoleBuffering, "CheckBox_UseConsoleBuffering")
        Me.CheckBox_UseConsoleBuffering.Name = "CheckBox_UseConsoleBuffering"
        Me.CheckBox_UseConsoleBuffering.UseVisualStyleBackColor = True
        '
        'CheckBox_UseAutoHints
        '
        resources.ApplyResources(Me.CheckBox_UseAutoHints, "CheckBox_UseAutoHints")
        Me.CheckBox_UseAutoHints.Name = "CheckBox_UseAutoHints"
        Me.CheckBox_UseAutoHints.UseVisualStyleBackColor = True
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
End Class
