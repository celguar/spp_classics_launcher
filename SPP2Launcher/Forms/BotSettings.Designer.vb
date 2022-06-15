<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BotSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BotSettings))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage_PB1 = New System.Windows.Forms.TabPage()
        Me.CheckBox_PlayerbotsEnabled = New System.Windows.Forms.CheckBox()
        Me.TextBox_RandomBotAccountCount = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox_MainPB = New System.Windows.Forms.GroupBox()
        Me.CheckBox_EnableGuildTasks = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RandomBotJoinBG = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RandomBotAutologin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RandomBotJoinLfg = New System.Windows.Forms.CheckBox()
        Me.TextBox_MaxRandomBots = New System.Windows.Forms.TextBox()
        Me.TextBox_RandomBotMaxLevel = New System.Windows.Forms.TextBox()
        Me.TextBox_RandomBotMinLevel = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_MinRandomBots = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage_PB2 = New System.Windows.Forms.TabPage()
        Me.GroupBox_OtherPB = New System.Windows.Forms.GroupBox()
        Me.GroupBox_MapsPB = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Eastern = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Northrend = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Kalimdor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Outland = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoEquipUpgradeLoot = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RandomBotGroupNearby = New System.Windows.Forms.CheckBox()
        Me.CheckBox_EnableGreet = New System.Windows.Forms.CheckBox()
        Me.CheckBox_GearScoreCheck = New System.Windows.Forms.CheckBox()
        Me.TabPage_AHB = New System.Windows.Forms.TabPage()
        Me.CheckBox_AhBotEnabled = New System.Windows.Forms.CheckBox()
        Me.GroupBox_MainAhBot = New System.Windows.Forms.GroupBox()
        Me.TextBox_TimeMax = New System.Windows.Forms.TextBox()
        Me.TextBox_TimeMin = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_BidMax = New System.Windows.Forms.TextBox()
        Me.TextBox_BidMin = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_ChanceBuy = New System.Windows.Forms.TextBox()
        Me.TextBox_ChanceSell = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CheckBox_AllowLegendary = New System.Windows.Forms.CheckBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_PB1.SuspendLayout()
        Me.GroupBox_MainPB.SuspendLayout()
        Me.TabPage_PB2.SuspendLayout()
        Me.GroupBox_OtherPB.SuspendLayout()
        Me.GroupBox_MapsPB.SuspendLayout()
        Me.TabPage_AHB.SuspendLayout()
        Me.GroupBox_MainAhBot.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_PB1)
        Me.TabControl1.Controls.Add(Me.TabPage_PB2)
        Me.TabControl1.Controls.Add(Me.TabPage_AHB)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        '
        'TabPage_PB1
        '
        Me.TabPage_PB1.Controls.Add(Me.CheckBox_PlayerbotsEnabled)
        Me.TabPage_PB1.Controls.Add(Me.TextBox_RandomBotAccountCount)
        Me.TabPage_PB1.Controls.Add(Me.Label5)
        Me.TabPage_PB1.Controls.Add(Me.GroupBox_MainPB)
        resources.ApplyResources(Me.TabPage_PB1, "TabPage_PB1")
        Me.TabPage_PB1.Name = "TabPage_PB1"
        Me.TabPage_PB1.UseVisualStyleBackColor = True
        '
        'CheckBox_PlayerbotsEnabled
        '
        resources.ApplyResources(Me.CheckBox_PlayerbotsEnabled, "CheckBox_PlayerbotsEnabled")
        Me.CheckBox_PlayerbotsEnabled.Name = "CheckBox_PlayerbotsEnabled"
        Me.CheckBox_PlayerbotsEnabled.UseVisualStyleBackColor = True
        '
        'TextBox_RandomBotAccountCount
        '
        resources.ApplyResources(Me.TextBox_RandomBotAccountCount, "TextBox_RandomBotAccountCount")
        Me.TextBox_RandomBotAccountCount.Name = "TextBox_RandomBotAccountCount"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'GroupBox_MainPB
        '
        resources.ApplyResources(Me.GroupBox_MainPB, "GroupBox_MainPB")
        Me.GroupBox_MainPB.Controls.Add(Me.CheckBox_EnableGuildTasks)
        Me.GroupBox_MainPB.Controls.Add(Me.CheckBox_RandomBotJoinBG)
        Me.GroupBox_MainPB.Controls.Add(Me.CheckBox_RandomBotAutologin)
        Me.GroupBox_MainPB.Controls.Add(Me.CheckBox_RandomBotJoinLfg)
        Me.GroupBox_MainPB.Controls.Add(Me.TextBox_MaxRandomBots)
        Me.GroupBox_MainPB.Controls.Add(Me.TextBox_RandomBotMaxLevel)
        Me.GroupBox_MainPB.Controls.Add(Me.TextBox_RandomBotMinLevel)
        Me.GroupBox_MainPB.Controls.Add(Me.Label4)
        Me.GroupBox_MainPB.Controls.Add(Me.TextBox_MinRandomBots)
        Me.GroupBox_MainPB.Controls.Add(Me.Label1)
        Me.GroupBox_MainPB.Name = "GroupBox_MainPB"
        Me.GroupBox_MainPB.TabStop = False
        '
        'CheckBox_EnableGuildTasks
        '
        resources.ApplyResources(Me.CheckBox_EnableGuildTasks, "CheckBox_EnableGuildTasks")
        Me.CheckBox_EnableGuildTasks.Name = "CheckBox_EnableGuildTasks"
        Me.CheckBox_EnableGuildTasks.UseVisualStyleBackColor = True
        '
        'CheckBox_RandomBotJoinBG
        '
        resources.ApplyResources(Me.CheckBox_RandomBotJoinBG, "CheckBox_RandomBotJoinBG")
        Me.CheckBox_RandomBotJoinBG.Name = "CheckBox_RandomBotJoinBG"
        Me.CheckBox_RandomBotJoinBG.UseVisualStyleBackColor = True
        '
        'CheckBox_RandomBotAutologin
        '
        resources.ApplyResources(Me.CheckBox_RandomBotAutologin, "CheckBox_RandomBotAutologin")
        Me.CheckBox_RandomBotAutologin.Name = "CheckBox_RandomBotAutologin"
        Me.CheckBox_RandomBotAutologin.UseVisualStyleBackColor = True
        '
        'CheckBox_RandomBotJoinLfg
        '
        resources.ApplyResources(Me.CheckBox_RandomBotJoinLfg, "CheckBox_RandomBotJoinLfg")
        Me.CheckBox_RandomBotJoinLfg.Name = "CheckBox_RandomBotJoinLfg"
        Me.CheckBox_RandomBotJoinLfg.UseVisualStyleBackColor = True
        '
        'TextBox_MaxRandomBots
        '
        resources.ApplyResources(Me.TextBox_MaxRandomBots, "TextBox_MaxRandomBots")
        Me.TextBox_MaxRandomBots.Name = "TextBox_MaxRandomBots"
        '
        'TextBox_RandomBotMaxLevel
        '
        resources.ApplyResources(Me.TextBox_RandomBotMaxLevel, "TextBox_RandomBotMaxLevel")
        Me.TextBox_RandomBotMaxLevel.Name = "TextBox_RandomBotMaxLevel"
        '
        'TextBox_RandomBotMinLevel
        '
        resources.ApplyResources(Me.TextBox_RandomBotMinLevel, "TextBox_RandomBotMinLevel")
        Me.TextBox_RandomBotMinLevel.Name = "TextBox_RandomBotMinLevel"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'TextBox_MinRandomBots
        '
        resources.ApplyResources(Me.TextBox_MinRandomBots, "TextBox_MinRandomBots")
        Me.TextBox_MinRandomBots.Name = "TextBox_MinRandomBots"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'TabPage_PB2
        '
        Me.TabPage_PB2.Controls.Add(Me.GroupBox_OtherPB)
        resources.ApplyResources(Me.TabPage_PB2, "TabPage_PB2")
        Me.TabPage_PB2.Name = "TabPage_PB2"
        Me.TabPage_PB2.UseVisualStyleBackColor = True
        '
        'GroupBox_OtherPB
        '
        Me.GroupBox_OtherPB.Controls.Add(Me.GroupBox_MapsPB)
        Me.GroupBox_OtherPB.Controls.Add(Me.CheckBox_AutoEquipUpgradeLoot)
        Me.GroupBox_OtherPB.Controls.Add(Me.CheckBox_RandomBotGroupNearby)
        Me.GroupBox_OtherPB.Controls.Add(Me.CheckBox_EnableGreet)
        Me.GroupBox_OtherPB.Controls.Add(Me.CheckBox_GearScoreCheck)
        resources.ApplyResources(Me.GroupBox_OtherPB, "GroupBox_OtherPB")
        Me.GroupBox_OtherPB.Name = "GroupBox_OtherPB"
        Me.GroupBox_OtherPB.TabStop = False
        '
        'GroupBox_MapsPB
        '
        Me.GroupBox_MapsPB.Controls.Add(Me.CheckBox_Eastern)
        Me.GroupBox_MapsPB.Controls.Add(Me.CheckBox_Northrend)
        Me.GroupBox_MapsPB.Controls.Add(Me.CheckBox_Kalimdor)
        Me.GroupBox_MapsPB.Controls.Add(Me.CheckBox_Outland)
        resources.ApplyResources(Me.GroupBox_MapsPB, "GroupBox_MapsPB")
        Me.GroupBox_MapsPB.Name = "GroupBox_MapsPB"
        Me.GroupBox_MapsPB.TabStop = False
        '
        'CheckBox_Eastern
        '
        resources.ApplyResources(Me.CheckBox_Eastern, "CheckBox_Eastern")
        Me.CheckBox_Eastern.Name = "CheckBox_Eastern"
        Me.CheckBox_Eastern.UseVisualStyleBackColor = True
        '
        'CheckBox_Northrend
        '
        resources.ApplyResources(Me.CheckBox_Northrend, "CheckBox_Northrend")
        Me.CheckBox_Northrend.Name = "CheckBox_Northrend"
        Me.CheckBox_Northrend.UseVisualStyleBackColor = True
        '
        'CheckBox_Kalimdor
        '
        resources.ApplyResources(Me.CheckBox_Kalimdor, "CheckBox_Kalimdor")
        Me.CheckBox_Kalimdor.Name = "CheckBox_Kalimdor"
        Me.CheckBox_Kalimdor.UseVisualStyleBackColor = True
        '
        'CheckBox_Outland
        '
        resources.ApplyResources(Me.CheckBox_Outland, "CheckBox_Outland")
        Me.CheckBox_Outland.Name = "CheckBox_Outland"
        Me.CheckBox_Outland.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoEquipUpgradeLoot
        '
        resources.ApplyResources(Me.CheckBox_AutoEquipUpgradeLoot, "CheckBox_AutoEquipUpgradeLoot")
        Me.CheckBox_AutoEquipUpgradeLoot.Name = "CheckBox_AutoEquipUpgradeLoot"
        Me.CheckBox_AutoEquipUpgradeLoot.UseVisualStyleBackColor = True
        '
        'CheckBox_RandomBotGroupNearby
        '
        resources.ApplyResources(Me.CheckBox_RandomBotGroupNearby, "CheckBox_RandomBotGroupNearby")
        Me.CheckBox_RandomBotGroupNearby.Name = "CheckBox_RandomBotGroupNearby"
        Me.CheckBox_RandomBotGroupNearby.UseVisualStyleBackColor = True
        '
        'CheckBox_EnableGreet
        '
        resources.ApplyResources(Me.CheckBox_EnableGreet, "CheckBox_EnableGreet")
        Me.CheckBox_EnableGreet.Name = "CheckBox_EnableGreet"
        Me.CheckBox_EnableGreet.UseVisualStyleBackColor = True
        '
        'CheckBox_GearScoreCheck
        '
        resources.ApplyResources(Me.CheckBox_GearScoreCheck, "CheckBox_GearScoreCheck")
        Me.CheckBox_GearScoreCheck.Name = "CheckBox_GearScoreCheck"
        Me.CheckBox_GearScoreCheck.UseVisualStyleBackColor = True
        '
        'TabPage_AHB
        '
        Me.TabPage_AHB.Controls.Add(Me.CheckBox_AhBotEnabled)
        Me.TabPage_AHB.Controls.Add(Me.GroupBox_MainAhBot)
        resources.ApplyResources(Me.TabPage_AHB, "TabPage_AHB")
        Me.TabPage_AHB.Name = "TabPage_AHB"
        Me.TabPage_AHB.UseVisualStyleBackColor = True
        '
        'CheckBox_AhBotEnabled
        '
        resources.ApplyResources(Me.CheckBox_AhBotEnabled, "CheckBox_AhBotEnabled")
        Me.CheckBox_AhBotEnabled.Name = "CheckBox_AhBotEnabled"
        Me.CheckBox_AhBotEnabled.UseVisualStyleBackColor = True
        '
        'GroupBox_MainAhBot
        '
        Me.GroupBox_MainAhBot.Controls.Add(Me.CheckBox_AllowLegendary)
        Me.GroupBox_MainAhBot.Controls.Add(Me.Label7)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TrackBar1)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_TimeMax)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_TimeMin)
        Me.GroupBox_MainAhBot.Controls.Add(Me.Label3)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_BidMax)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_BidMin)
        Me.GroupBox_MainAhBot.Controls.Add(Me.Label2)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_ChanceBuy)
        Me.GroupBox_MainAhBot.Controls.Add(Me.TextBox_ChanceSell)
        Me.GroupBox_MainAhBot.Controls.Add(Me.Label6)
        resources.ApplyResources(Me.GroupBox_MainAhBot, "GroupBox_MainAhBot")
        Me.GroupBox_MainAhBot.Name = "GroupBox_MainAhBot"
        Me.GroupBox_MainAhBot.TabStop = False
        '
        'TextBox_TimeMax
        '
        resources.ApplyResources(Me.TextBox_TimeMax, "TextBox_TimeMax")
        Me.TextBox_TimeMax.Name = "TextBox_TimeMax"
        '
        'TextBox_TimeMin
        '
        resources.ApplyResources(Me.TextBox_TimeMin, "TextBox_TimeMin")
        Me.TextBox_TimeMin.Name = "TextBox_TimeMin"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'TextBox_BidMax
        '
        resources.ApplyResources(Me.TextBox_BidMax, "TextBox_BidMax")
        Me.TextBox_BidMax.Name = "TextBox_BidMax"
        '
        'TextBox_BidMin
        '
        resources.ApplyResources(Me.TextBox_BidMin, "TextBox_BidMin")
        Me.TextBox_BidMin.Name = "TextBox_BidMin"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'TextBox_ChanceBuy
        '
        resources.ApplyResources(Me.TextBox_ChanceBuy, "TextBox_ChanceBuy")
        Me.TextBox_ChanceBuy.Name = "TextBox_ChanceBuy"
        '
        'TextBox_ChanceSell
        '
        resources.ApplyResources(Me.TextBox_ChanceSell, "TextBox_ChanceSell")
        Me.TextBox_ChanceSell.Name = "TextBox_ChanceSell"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'TabPage1
        '
        resources.ApplyResources(Me.TabPage1, "TabPage1")
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button_Save
        '
        resources.ApplyResources(Me.Button_Save, "Button_Save")
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'TrackBar1
        '
        resources.ApplyResources(Me.TrackBar1, "TrackBar1")
        Me.TrackBar1.Name = "TrackBar1"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'CheckBox_AllowLegendary
        '
        resources.ApplyResources(Me.CheckBox_AllowLegendary, "CheckBox_AllowLegendary")
        Me.CheckBox_AllowLegendary.Name = "CheckBox_AllowLegendary"
        Me.CheckBox_AllowLegendary.UseVisualStyleBackColor = True
        '
        'BotSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BotSettings"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_PB1.ResumeLayout(False)
        Me.TabPage_PB1.PerformLayout()
        Me.GroupBox_MainPB.ResumeLayout(False)
        Me.GroupBox_MainPB.PerformLayout()
        Me.TabPage_PB2.ResumeLayout(False)
        Me.GroupBox_OtherPB.ResumeLayout(False)
        Me.GroupBox_OtherPB.PerformLayout()
        Me.GroupBox_MapsPB.ResumeLayout(False)
        Me.GroupBox_MapsPB.PerformLayout()
        Me.TabPage_AHB.ResumeLayout(False)
        Me.TabPage_AHB.PerformLayout()
        Me.GroupBox_MainAhBot.ResumeLayout(False)
        Me.GroupBox_MainAhBot.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage_PB1 As TabPage
    Friend WithEvents TabPage_AHB As TabPage
    Friend WithEvents GroupBox_MainPB As GroupBox
    Friend WithEvents CheckBox_PlayerbotsEnabled As CheckBox
    Friend WithEvents TextBox_RandomBotMinLevel As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox_MinRandomBots As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox_RandomBotAccountCount As TextBox
    Friend WithEvents Label5 As Label
    Private WithEvents CheckBox_RandomBotJoinBG As CheckBox
    Private WithEvents CheckBox_RandomBotJoinLfg As CheckBox
    Friend WithEvents TextBox_RandomBotMaxLevel As TextBox
    Private WithEvents CheckBox_RandomBotAutologin As CheckBox
    Friend WithEvents TextBox_MaxRandomBots As TextBox
    Private WithEvents CheckBox_EnableGuildTasks As CheckBox
    Friend WithEvents TabPage_PB2 As TabPage
    Friend WithEvents GroupBox_OtherPB As GroupBox
    Private WithEvents GroupBox_MapsPB As GroupBox
    Private WithEvents CheckBox_Eastern As CheckBox
    Private WithEvents CheckBox_Northrend As CheckBox
    Private WithEvents CheckBox_Kalimdor As CheckBox
    Private WithEvents CheckBox_Outland As CheckBox
    Private WithEvents CheckBox_AutoEquipUpgradeLoot As CheckBox
    Private WithEvents CheckBox_RandomBotGroupNearby As CheckBox
    Private WithEvents CheckBox_EnableGreet As CheckBox
    Private WithEvents CheckBox_GearScoreCheck As CheckBox
    Friend WithEvents Button_Save As Button
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents CheckBox_AhBotEnabled As CheckBox
    Friend WithEvents GroupBox_MainAhBot As GroupBox
    Friend WithEvents TextBox_ChanceBuy As TextBox
    Friend WithEvents TextBox_ChanceSell As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox_BidMax As TextBox
    Friend WithEvents TextBox_BidMin As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox_TimeMax As TextBox
    Friend WithEvents TextBox_TimeMin As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents CheckBox_AllowLegendary As CheckBox
End Class
