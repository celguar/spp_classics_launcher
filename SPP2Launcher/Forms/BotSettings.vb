
Public Class BotSettings

    Sub New()
        InitializeComponent()

        ' Расположение окна при открытии
        StartPosition = FormStartPosition.Manual
        Location = New Point(My.Settings.AppLocation.X + 40, My.Settings.AppLocation.Y + 40)

    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BotSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

#Region " === PLAYERBOTS === "

            CheckBox_PlayerbotsEnabled.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.Enabled")
            If Not CheckBox_PlayerbotsEnabled.Checked Then CheckBox_PlayerbotsEnabled_CheckedChanged(Me, e)
            TextBox_RandomBotAccountCount.Text = GV.SPP2Launcher.IniPlayerBots.ReadInt32("AiPlayerbotConf", "AiPlayerbot.RandomBotAccountCount").ToString
            CheckBox_RandomBotAutologin.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.RandomBotAutologin")
            TextBox_MinRandomBots.Text = GV.SPP2Launcher.IniPlayerBots.ReadInt32("AiPlayerbotConf", "AiPlayerbot.MinRandomBots").ToString
            TextBox_MaxRandomBots.Text = GV.SPP2Launcher.IniPlayerBots.ReadInt32("AiPlayerbotConf", "AiPlayerbot.MaxRandomBots").ToString
            TextBox_RandomBotMinLevel.Text = GV.SPP2Launcher.IniPlayerBots.ReadInt32("AiPlayerbotConf", "AiPlayerbot.RandomBotMinLevel").ToString
            TextBox_RandomBotMaxLevel.Text = GV.SPP2Launcher.IniPlayerBots.ReadInt32("AiPlayerbotConf", "AiPlayerbot.RandomBotMaxLevel").ToString
            CheckBox_RandomBotJoinLfg.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.RandomBotJoinLfg")
            CheckBox_RandomBotJoinBG.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.RandomBotJoinBG")
            CheckBox_EnableGuildTasks.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.EnableGuildTasks")

            CheckBox_GearScoreCheck.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.GearScoreCheck")
            CheckBox_EnableGreet.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.EnableGreet")
            CheckBox_RandomBotGroupNearby.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.RandomBotGroupNearby")
            CheckBox_AutoEquipUpgradeLoot.Checked = GV.SPP2Launcher.IniPlayerBots.ReadBool("AiPlayerbotConf", "AiPlayerbot.AutoEquipUpgradeLoot")

            Dim maps = GV.SPP2Launcher.IniPlayerBots.ReadString("AiPlayerbotConf", "AiPlayerbot.RandomBotMaps")
            Dim arr = maps.Split(","c)

            If arr.Contains("0") Then
                CheckBox_Eastern.Checked = True
            End If

            If arr.Contains("1") Then
                CheckBox_Kalimdor.Checked = True
            End If

            If arr.Contains("530") Then
                CheckBox_Outland.Checked = True
            End If

            If arr.Contains("571") Then
                CheckBox_Northrend.Checked = True
            End If

#End Region

#Region " === AHBOT === "

            CheckBox_AhBotEnabled.Checked = GV.SPP2Launcher.IniAhBot.ReadBool("AhbotConf", "AhBot.Enabled")
            If Not CheckBox_AhBotEnabled.Checked Then CheckBox_AhBotEnabled_CheckedChanged(Me, e)
            TextBox_TimeMin.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Time.Min").ToString
            TextBox_TimeMax.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Time.Max").ToString
            TextBox_ChanceSell.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Chance.Sell").ToString
            TextBox_ChanceBuy.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Chance.Buy").ToString
            TextBox_BidMin.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Bid.Min").ToString
            TextBox_BidMax.Text = GV.SPP2Launcher.IniAhBot.ReadInt32("AhbotConf", "AuctionHouseBot.Bid.Max").ToString

#End Region

        Catch ex As Exception
            GV.Log.WriteException(ex)
            MessageBox.Show(ex.Message,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ РАЗРЕШЕНИЯ PLAYERBOTS
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_PlayerbotsEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_PlayerbotsEnabled.CheckedChanged
        TextBox_RandomBotAccountCount.Enabled = CheckBox_PlayerbotsEnabled.Checked
        GroupBox_MainPB.Enabled = CheckBox_PlayerbotsEnabled.Checked
        GroupBox_OtherPB.Enabled = CheckBox_PlayerbotsEnabled.Checked
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ РАЗРЕШЕНИЯ AHBOT
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_AhBotEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_AhBotEnabled.CheckedChanged
        GroupBox_MainAhBot.Enabled = CheckBox_AhBotEnabled.Checked
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ - СОХРАНИТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Save_Click(sender As Object, e As EventArgs) Handles Button_Save.Click
        Try

#Region " === PLAYERBOTS === "

            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.Enabled", BoolToStr(CheckBox_PlayerbotsEnabled.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotAccountCount", IsInteger(TextBox_RandomBotAccountCount, 0, 1000))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotAutologin", BoolToStr(CheckBox_RandomBotAutologin.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.MinRandomBots", IsInteger(TextBox_MinRandomBots, 0, 1000))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.MaxRandomBots", IsInteger(TextBox_MaxRandomBots, 0, 1000))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotMinLevel", IsInteger(TextBox_RandomBotMinLevel, 1, 80))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotMaxLevel", IsInteger(TextBox_RandomBotMaxLevel, 1, 80))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotJoinLfg", BoolToStr(CheckBox_RandomBotJoinLfg.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotJoinBG", BoolToStr(CheckBox_RandomBotJoinBG.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.EnableGuildTasks", BoolToStr(CheckBox_EnableGuildTasks.Checked))

            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.GearScoreCheck", BoolToStr(CheckBox_GearScoreCheck.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.EnableGreet", BoolToStr(CheckBox_EnableGreet.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotGroupNearby", BoolToStr(CheckBox_RandomBotGroupNearby.Checked))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.AutoEquipUpgradeLoot", BoolToStr(CheckBox_AutoEquipUpgradeLoot.Checked))

            Dim arr(3) As String

            If CheckBox_Eastern.Checked Then
                arr(0) = "0"
            End If

            If CheckBox_Kalimdor.Checked Then
                arr(1) = "1"
            End If

            If CheckBox_Northrend.Checked Then
                arr(2) = "571"
            End If

            If CheckBox_Outland.Checked Then
                arr(3) = "530"
            End If

            Dim res = String.Join(",", arr.Where(Function(s) Not String.IsNullOrEmpty(s)))
            GV.SPP2Launcher.IniPlayerBots.Write("AiPlayerbotConf", "AiPlayerbot.RandomBotMaps", res)

#End Region

#Region "=== AHBOT === "

            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AhBot.Enabled", BoolToStr(CheckBox_AhBotEnabled.Checked))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Time.Min", IsInteger(TextBox_TimeMin, 1, 72))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Time.Max", IsInteger(TextBox_TimeMax, 1, 72))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Chance.Sell", IsInteger(TextBox_ChanceSell, 0, 100))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Chance.Buy", IsInteger(TextBox_ChanceBuy, 0, 100))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Bid.Min", IsInteger(TextBox_BidMin, 0, 100))
            GV.SPP2Launcher.IniAhBot.Write("AhbotConf", "AuctionHouseBot.Bid.Max", IsInteger(TextBox_BidMax, 0, 100))

#End Region

            MessageBox.Show(My.Resources.P071_Saved,
                            My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Close()

        Catch ex As Exception
            GV.Log.WriteException(ex)
            MessageBox.Show(ex.Message,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Переводит Boolean в String.
    ''' </summary>
    ''' <param name="value">Значение.</param>
    ''' <returns></returns>
    Private Function BoolToStr(value As Boolean) As String
        If value Then Return "1"
        Return "0"
    End Function

    ''' <summary>
    ''' Проверяет значение на числовое.
    ''' </summary>
    ''' <param name="caller">TextBox вызвавший проверку.</param>
    ''' <returns></returns>
    Private Function IsInteger(caller As TextBox, min As Integer, max As Integer) As String
        Try
            Dim res = CInt(caller.Text)
            If res < min Or res > max Then
                caller.BackColor = Color.Coral
                Throw New Exception(String.Format(My.Resources.P072_IllegalNumber, min, max))
            End If
            Return res.ToString
        Catch ex As Exception
            caller.BackColor = Color.Coral
            Throw New Exception(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' КНОПКА - УДАЛИТЬ БОТОВ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_RemoveBots_Click(sender As Object, e As EventArgs) Handles Button_RemoveBots.Click

    End Sub

    ''' <summary>
    ''' КНОПКА - УДАЛИТЬ ВСЕХ БОТОВ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_RemoveAllBots_Click(sender As Object, e As EventArgs) Handles Button_RemoveAllBots.Click

    End Sub

    ''' <summary>
    ''' КНОПКА - ОТМЕНИТЬ УДАЛЕНИЕ БОТОВ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_CancelRemove_Click(sender As Object, e As EventArgs) Handles Button_CancelRemove.Click

    End Sub

End Class