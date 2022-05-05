
Public Class MySqlSettings

    Private _IsLoading As Boolean = True

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
    Private Sub MySqlSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox_UseIntMySQL.Checked = My.Settings.UseIntMySQL
        ChangeUseIntServer()
        CheckBox_MySqlAutostart.Checked = My.Settings.MySqlAutostart
        _IsLoading = False
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ОК
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        My.Settings.UseIntMySQL = CheckBox_UseIntMySQL.Checked
        My.Settings.MySqlAutostart = CheckBox_MySqlAutostart.Checked
        If CheckBox_UseIntMySQL.Checked Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    My.Settings.MySqlClassicIntHost = TextBox_Host.Text
                    My.Settings.MySqlClassicIntPort = TextBox_Port.Text
                    My.Settings.MySqlClassicIntUserName = TextBox_UserName.Text
                    My.Settings.MySqlClassicIntPassword = TextBox_Password.Text
                    My.Settings.MySqlClassicIntArmory = TextBox_Armory.Text
                    My.Settings.MySqlClassicIntCharacters = TextBox_Characters.Text
                    My.Settings.MySqlClassicIntLogs = TextBox_Logs.Text
                    My.Settings.MySqlClassicIntMangos = TextBox_Mangos.Text
                    My.Settings.MySqlClassicIntPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlClassicIntRealmd = TextBox_Realmd.Text
                Case GV.EModule.Tbc.ToString
                    My.Settings.MySqlTbcIntHost = TextBox_Host.Text
                    My.Settings.MySqlTbcIntPort = TextBox_Port.Text
                    My.Settings.MySqlTbcIntUserName = TextBox_UserName.Text
                    My.Settings.MySqlTbcIntPassword = TextBox_Password.Text
                    My.Settings.MySqlTbcIntArmory = TextBox_Armory.Text
                    My.Settings.MySqlTbcIntCharacters = TextBox_Characters.Text
                    My.Settings.MySqlTbcIntLogs = TextBox_Logs.Text
                    My.Settings.MySqlTbcIntMangos = TextBox_Mangos.Text
                    My.Settings.MySqlTbcIntPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlTbcIntRealmd = TextBox_Realmd.Text
                Case GV.EModule.Wotlk.ToString
                    My.Settings.MySqlWotlkIntHost = TextBox_Host.Text
                    My.Settings.MySqlWotlkIntPort = TextBox_Port.Text
                    My.Settings.MySqlWotlkIntUserName = TextBox_UserName.Text
                    My.Settings.MySqlWotlkIntPassword = TextBox_Password.Text
                    My.Settings.MySqlWotlkIntArmory = TextBox_Armory.Text
                    My.Settings.MySqlWotlkIntCharacters = TextBox_Characters.Text
                    My.Settings.MySqlWotlkIntLogs = TextBox_Logs.Text
                    My.Settings.MySqlWotlkIntMangos = TextBox_Mangos.Text
                    My.Settings.MySqlWotlkIntPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlWotlkIntRealmd = TextBox_Realmd.Text
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    My.Settings.MySqlClassicExtHost = TextBox_Host.Text
                    My.Settings.MySqlClassicExtPort = TextBox_Port.Text
                    My.Settings.MySqlClassicExtUserName = TextBox_UserName.Text
                    My.Settings.MySqlClassicExtPassword = TextBox_Password.Text
                    My.Settings.MySqlClassicExtArmory = TextBox_Armory.Text
                    My.Settings.MySqlClassicExtCharacters = TextBox_Characters.Text
                    My.Settings.MySqlClassicExtLogs = TextBox_Logs.Text
                    My.Settings.MySqlClassicExtMangos = TextBox_Mangos.Text
                    My.Settings.MySqlClassicExtPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlClassicExtRealmd = TextBox_Realmd.Text
                Case GV.EModule.Tbc.ToString
                    My.Settings.MySqlTbcExtHost = TextBox_Host.Text
                    My.Settings.MySqlTbcExtPort = TextBox_Port.Text
                    My.Settings.MySqlTbcExtUserName = TextBox_UserName.Text
                    My.Settings.MySqlTbcExtPassword = TextBox_Password.Text
                    My.Settings.MySqlTbcExtArmory = TextBox_Armory.Text
                    My.Settings.MySqlTbcExtCharacters = TextBox_Characters.Text
                    My.Settings.MySqlTbcExtLogs = TextBox_Logs.Text
                    My.Settings.MySqlTbcExtMangos = TextBox_Mangos.Text
                    My.Settings.MySqlTbcExtPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlTbcExtRealmd = TextBox_Realmd.Text
                Case GV.EModule.Wotlk.ToString
                    My.Settings.MySqlWotlkExtHost = TextBox_Host.Text
                    My.Settings.MySqlWotlkExtPort = TextBox_Port.Text
                    My.Settings.MySqlWotlkExtUserName = TextBox_UserName.Text
                    My.Settings.MySqlWotlkExtPassword = TextBox_Password.Text
                    My.Settings.MySqlWotlkExtArmory = TextBox_Armory.Text
                    My.Settings.MySqlWotlkExtCharacters = TextBox_Characters.Text
                    My.Settings.MySqlWotlkExtLogs = TextBox_Logs.Text
                    My.Settings.MySqlWotlkExtMangos = TextBox_Mangos.Text
                    My.Settings.MySqlWotlkExtPlayerbots = TextBox_Playerbot.Text
                    My.Settings.MySqlWotlkExtRealmd = TextBox_Realmd.Text
            End Select
        End If
        My.Settings.Save()
        Close()
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ ФЛАГА ВСТРОЕННОГО MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_UseIntMySQL_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_UseIntMySQL.CheckedChanged
        ChangeUseIntServer()
    End Sub

    ''' <summary>
    ''' Изменение разрешения использования встроенного сервера MySQL
    ''' </summary>
    Private Sub ChangeUseIntServer()
        If CheckBox_UseIntMySQL.Checked Then
            If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & SPP2MYSQL) Then
                CheckBox_MySqlAutostart.Enabled = False
                TextBox_Armory.Enabled = False
                TextBox_Characters.Enabled = False
                TextBox_Logs.Enabled = False
                TextBox_Mangos.Enabled = False
                TextBox_Playerbot.Enabled = False
                TextBox_Realmd.Enabled = False
                CheckBox_MySqlAutostart.Enabled = True
                Select Case My.Settings.LastLoadedServerType
                    Case GV.EModule.Classic.ToString
                        TextBox_Host.Text = My.Settings.MySqlClassicIntHost
                        TextBox_Port.Text = My.Settings.MySqlClassicIntPort
                        TextBox_UserName.Text = My.Settings.MySqlClassicIntUserName
                        TextBox_Password.Text = My.Settings.MySqlClassicIntPassword
                        TextBox_Armory.Text = My.Settings.MySqlClassicIntArmory
                        TextBox_Characters.Text = My.Settings.MySqlClassicIntCharacters
                        TextBox_Logs.Text = My.Settings.MySqlClassicIntLogs
                        TextBox_Mangos.Text = My.Settings.MySqlClassicIntMangos
                        TextBox_Playerbot.Text = My.Settings.MySqlClassicIntPlayerbots
                        TextBox_Realmd.Text = My.Settings.MySqlClassicIntRealmd
                    Case GV.EModule.Tbc.ToString
                        TextBox_Host.Text = My.Settings.MySqlTbcIntHost
                        TextBox_Port.Text = My.Settings.MySqlTbcIntPort
                        TextBox_UserName.Text = My.Settings.MySqlTbcIntUserName
                        TextBox_Password.Text = My.Settings.MySqlTbcIntPassword
                        TextBox_Armory.Text = My.Settings.MySqlTbcIntArmory
                        TextBox_Characters.Text = My.Settings.MySqlTbcIntCharacters
                        TextBox_Logs.Text = My.Settings.MySqlTbcIntLogs
                        TextBox_Mangos.Text = My.Settings.MySqlTbcIntMangos
                        TextBox_Playerbot.Text = My.Settings.MySqlTbcIntPlayerbots
                        TextBox_Realmd.Text = My.Settings.MySqlTbcIntRealmd
                    Case GV.EModule.Wotlk.ToString
                        TextBox_Host.Text = My.Settings.MySqlWotlkIntHost
                        TextBox_Port.Text = My.Settings.MySqlWotlkIntPort
                        TextBox_UserName.Text = My.Settings.MySqlWotlkIntUserName
                        TextBox_Password.Text = My.Settings.MySqlWotlkIntPassword
                        TextBox_Armory.Text = My.Settings.MySqlWotlkIntArmory
                        TextBox_Characters.Text = My.Settings.MySqlWotlkIntCharacters
                        TextBox_Logs.Text = My.Settings.MySqlWotlkIntLogs
                        TextBox_Mangos.Text = My.Settings.MySqlWotlkIntMangos
                        TextBox_Playerbot.Text = My.Settings.MySqlWotlkIntPlayerbots
                        TextBox_Realmd.Text = My.Settings.MySqlWotlkIntRealmd
                End Select
            Else
                ' Каталог MySQL не найден
                MessageBox.Show(My.Resources.E005_MySqlNotFound,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox_UseIntMySQL.Checked = False
            End If
        Else
            CheckBox_MySqlAutostart.Enabled = False
            TextBox_Armory.Enabled = True
            TextBox_Characters.Enabled = True
            TextBox_Logs.Enabled = True
            TextBox_Mangos.Enabled = True
            TextBox_Playerbot.Enabled = True
            TextBox_Realmd.Enabled = True
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    TextBox_Host.Text = My.Settings.MySqlClassicExtHost
                    TextBox_Port.Text = My.Settings.MySqlClassicExtPort
                    TextBox_UserName.Text = My.Settings.MySqlClassicExtUserName
                    TextBox_Password.Text = My.Settings.MySqlClassicExtPassword
                    TextBox_Armory.Text = My.Settings.MySqlClassicExtArmory
                    TextBox_Characters.Text = My.Settings.MySqlClassicExtCharacters
                    TextBox_Logs.Text = My.Settings.MySqlClassicExtLogs
                    TextBox_Mangos.Text = My.Settings.MySqlClassicExtMangos
                    TextBox_Playerbot.Text = My.Settings.MySqlClassicExtPlayerbots
                    TextBox_Realmd.Text = My.Settings.MySqlClassicExtRealmd
                Case GV.EModule.Tbc.ToString
                    TextBox_Host.Text = My.Settings.MySqlTbcExtHost
                    TextBox_Port.Text = My.Settings.MySqlTbcExtPort
                    TextBox_UserName.Text = My.Settings.MySqlTbcExtUserName
                    TextBox_Password.Text = My.Settings.MySqlTbcExtPassword
                    TextBox_Armory.Text = My.Settings.MySqlTbcExtArmory
                    TextBox_Characters.Text = My.Settings.MySqlTbcExtCharacters
                    TextBox_Logs.Text = My.Settings.MySqlTbcExtLogs
                    TextBox_Mangos.Text = My.Settings.MySqlTbcExtMangos
                    TextBox_Playerbot.Text = My.Settings.MySqlTbcExtPlayerbots
                    TextBox_Realmd.Text = My.Settings.MySqlTbcExtRealmd
                Case GV.EModule.Wotlk.ToString
                    TextBox_Host.Text = My.Settings.MySqlWotlkExtHost
                    TextBox_Port.Text = My.Settings.MySqlWotlkExtPort
                    TextBox_UserName.Text = My.Settings.MySqlWotlkExtUserName
                    TextBox_Password.Text = My.Settings.MySqlWotlkExtPassword
                    TextBox_Armory.Text = My.Settings.MySqlWotlkExtArmory
                    TextBox_Characters.Text = My.Settings.MySqlWotlkExtCharacters
                    TextBox_Logs.Text = My.Settings.MySqlWotlkExtLogs
                    TextBox_Mangos.Text = My.Settings.MySqlWotlkExtMangos
                    TextBox_Playerbot.Text = My.Settings.MySqlWotlkExtPlayerbots
                    TextBox_Realmd.Text = My.Settings.MySqlWotlkExtRealmd
            End Select
        End If
    End Sub

End Class