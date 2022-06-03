Public Class ApacheSettings

    ''' <summary>
    ''' Флаг изменения настроек
    ''' </summary>
    Private _changed As Boolean

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
    Private Sub ApacheSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox_UseIntApache.Checked = My.Settings.UseIntApache
        ' Заполняем список локальных IpV4 адресов
        ComboBox_Host.Items.Clear()
        ComboBox_Host.Items.AddRange(GetLocalIpAddresses().ToArray)
        ' Устанавливаем режим Apache - встроенный?
        ChangeUseIntServer()
        CheckBox_ApacheAutostart.Checked = My.Settings.ApacheAutostart
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОКИ ОК
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        MessageBox.Show(My.Resources.P031_ChangeToRestart, My.Resources.P016_WarningCaption)
        My.Settings.UseIntApache = CheckBox_UseIntApache.Checked
        My.Settings.ApacheAutostart = CheckBox_ApacheAutostart.Checked
        If CheckBox_UseIntApache.Checked Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    If ComboBox_Host.SelectedItem.ToString <> My.Settings.ApacheClassicIntHost Or
                        TextBox_Port.Text <> My.Settings.ApacheClassicIntPort Then _changed = True
                    My.Settings.ApacheClassicIntHost = ComboBox_Host.SelectedItem.ToString
                    My.Settings.ApacheClassicIntPort = TextBox_Port.Text
                Case GV.EModule.Tbc.ToString
                    If ComboBox_Host.SelectedItem.ToString <> My.Settings.ApacheTbcIntHost Or
                        TextBox_Port.Text <> My.Settings.ApacheTbcIntPort Then _changed = True
                    My.Settings.ApacheTbcIntHost = ComboBox_Host.SelectedItem.ToString
                    My.Settings.ApacheTbcIntPort = TextBox_Port.Text
                Case GV.EModule.Wotlk.ToString
                    If ComboBox_Host.SelectedItem.ToString <> My.Settings.ApacheWotlkIntHost Or
                        TextBox_Port.Text <> My.Settings.ApacheWotlkIntPort Then _changed = True
                    My.Settings.ApacheWotlkIntHost = ComboBox_Host.SelectedItem.ToString
                    My.Settings.ApacheWotlkIntPort = TextBox_Port.Text
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    My.Settings.ApacheClassicExtHost = TextBox_Host.Text
                    My.Settings.ApacheClassicExtPort = TextBox_Port.Text
                Case GV.EModule.Tbc.ToString
                    My.Settings.ApacheTbcExtHost = TextBox_Host.Text
                    My.Settings.ApacheTbcExtPort = TextBox_Port.Text
                Case GV.EModule.Wotlk.ToString
                    My.Settings.ApacheWotlkExtHost = TextBox_Host.Text
                    My.Settings.ApacheWotlkExtPort = TextBox_Port.Text
            End Select
        End If
        My.Settings.Save()
        Close()
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ ФЛАГА ВСТРОЕННОГО Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_UseIntApache_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_UseIntApache.CheckedChanged
        ChangeUseIntServer()
    End Sub

    ''' <summary>
    ''' Устанавливаем режим работы Apache.
    ''' </summary>
    Private Sub ChangeUseIntServer()
        If CheckBox_UseIntApache.Checked Then
            ' Отображаем ComboBox
            TextBox_Host.Visible = False
            ComboBox_Host.Visible = True
            ComboBox_Host.Location = New Point(118, 23)
            ComboBox_Host.Size = New Size(167, 20)
            If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & SPP2APACHE) Then
                CheckBox_ApacheAutostart.Enabled = True
                Select Case My.Settings.LastLoadedServerType
                    Case GV.EModule.Classic.ToString
                        ComboBox_Host.SelectedItem = My.Settings.ApacheClassicIntHost
                        TextBox_Port.Text = My.Settings.ApacheClassicIntPort
                    Case GV.EModule.Tbc.ToString
                        ComboBox_Host.SelectedItem = My.Settings.ApacheTbcIntHost
                        TextBox_Port.Text = My.Settings.ApacheTbcIntPort
                    Case GV.EModule.Wotlk.ToString
                        ComboBox_Host.SelectedItem = My.Settings.ApacheWotlkIntHost
                        TextBox_Port.Text = My.Settings.ApacheWotlkIntPort
                End Select
            Else
                ' Каталог Apache24 не найден
                MessageBox.Show(My.Resources.E006_ApacheNotFound,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox_ApacheAutostart.Checked = False
            End If
        Else
            ' Отображаем TextBox
            TextBox_Host.Visible = True
            ComboBox_Host.Visible = False
            TextBox_Host.Location = New Point(118, 24)
            TextBox_Host.Size = New Size(167, 20)
            CheckBox_ApacheAutostart.Enabled = False
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    TextBox_Host.Text = My.Settings.ApacheClassicExtHost
                    TextBox_Port.Text = My.Settings.ApacheClassicExtPort
                Case GV.EModule.Tbc.ToString
                    TextBox_Host.Text = My.Settings.ApacheTbcExtHost
                    TextBox_Port.Text = My.Settings.ApacheTbcExtPort
                Case GV.EModule.Wotlk.ToString
                    TextBox_Host.Text = My.Settings.ApacheWotlkExtHost
                    TextBox_Port.Text = My.Settings.ApacheWotlkExtPort
            End Select
        End If
    End Sub

End Class