
Public Class ServerSettings

    Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ServerSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox_ServerAutostart.Checked = GetServerAutostart()
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ОК
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        Select Case My.Settings.LastLoadedServerType
            Case GV.EModule.Classic.ToString
                My.Settings.ServerClassicAutostart = CheckBox_ServerAutostart.Checked
            Case GV.EModule.Tbc.ToString
                My.Settings.ServerTbcAutostart = CheckBox_ServerAutostart.Checked
            Case GV.EModule.Wotlk.ToString
                My.Settings.ServerWotlkAutostart = CheckBox_ServerAutostart.Checked
        End Select
        My.Settings.Save()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Устанавливаем режим работы Apache.
    ''' </summary>
    Private Function GetServerAutostart() As Boolean
        If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & SPP2SETTINGS) Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    Return My.Settings.ServerClassicAutostart
                Case GV.EModule.Tbc.ToString
                    Return My.Settings.ServerTbcAutostart
                Case GV.EModule.Wotlk.ToString
                    Return My.Settings.ServerWotlkAutostart
                Case Else
                    Return False
            End Select
        Else
            ' Каталог Settings не найден
            MessageBox.Show(My.Resources.E013_SettingsNotFound,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
    End Function

End Class