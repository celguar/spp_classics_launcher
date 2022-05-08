
Public Class ServerSelector

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    Sub New()
        InitializeComponent()
        PictureBox_Classic.Enabled = False
        Label_ClassicStatus.Text = My.Resources.P009_NotInstalled
        PictureBox_TBC.Enabled = False
        Label_TbcStatus.Text = My.Resources.P009_NotInstalled
        PictureBox_WotLK.Enabled = False
        Label_WotlkStatus.Text = My.Resources.P009_NotInstalled
    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ServerSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = My.Resources.P004_SelectServer
        For Each md In GV.Modules
            Select Case md.ModuleType

                Case GV.EModule.Classic

                    PictureBox_Classic.Enabled = True
                    Label_ClassicStatus.Text = My.Resources.P008_Installed

                Case GV.EModule.Tbc
                    PictureBox_TBC.Enabled = True
                    Label_TbcStatus.Text = My.Resources.P008_Installed

                Case GV.EModule.Wotlk
                    PictureBox_WotLK.Enabled = True
                    Label_WotlkStatus.Text = My.Resources.P008_Installed

            End Select
        Next
    End Sub

    ''' <summary>
    ''' ЗАПУСК Classic
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_Classic_Click(sender As Object, e As EventArgs) Handles PictureBox_Classic.Click
        My.Settings.LastLoadedServerType = GV.EModule.Classic.ToString
        GV.Log.WriteInfo(String.Format(My.Resources.P027_ServerSelected, GV.EModule.Classic.ToString))
        SelectorClose()
    End Sub

    ''' <summary>
    ''' ЗАПУСК TBC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_TBC_Click(sender As Object, e As EventArgs) Handles PictureBox_TBC.Click
        My.Settings.LastLoadedServerType = GV.EModule.Tbc.ToString
        GV.Log.WriteInfo(String.Format(My.Resources.P027_ServerSelected, GV.EModule.Tbc.ToString))
        SelectorClose()
    End Sub

    ''' <summary>
    ''' ЗАПУСК WotLK
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_WotLK_Click(sender As Object, e As EventArgs) Handles PictureBox_WotLK.Click
        My.Settings.LastLoadedServerType = GV.EModule.Wotlk.ToString
        GV.Log.WriteInfo(String.Format(My.Resources.P027_ServerSelected, GV.EModule.Wotlk.ToString))
        SelectorClose()
    End Sub

    ''' <summary>
    ''' ЗАКРЫВАЕТ ФОРМУ
    ''' </summary>
    Private Sub SelectorClose()
        My.Settings.Save()
        GV.Log.WriteInfo(My.Resources.P029_LaunchMain)
        Timer_FormClosing.Start()
    End Sub

    ''' <summary>
    ''' ТИК ТАЙМЕРА ПРИ ЗАКРЫТИИ ОКНА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Timer_FormClosing_Tick(sender As Object, e As EventArgs) Handles Timer_FormClosing.Tick
        Timer_FormClosing.Stop()
        Me.Hide()
        GV.SPP2Launcher = New Launcher
        GV.SPP2Launcher.ShowDialog()
        If My.Settings.FirstStart Then
            My.Settings.FirstStart = False
            My.Settings.Save()
            Application.Restart()
        Else
            Me.Close()
        End If
    End Sub

#Region " === КУРСОР МЫШИ === "

    ''' <summary>
    ''' ПРИ НАЕЗДЕ НА PictureBox Classic
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_Vanilla_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_Classic.MouseEnter
        If PictureBox_Classic.Enabled Then
            Me.Cursor = Cursors.Hand
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ПОКИДАНИИ PictureBox Classic
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_Vanilla_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_Classic.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' ПРИ НАЕЗДЕ НА PictureBox TBC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_TBC_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_TBC.MouseEnter
        If PictureBox_TBC.Enabled Then
            Me.Cursor = Cursors.Hand
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ПОКИДАНИИ PictureBox TBC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_TBC_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_TBC.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' ПРИ НАЕЗДЕ НА PictureBox WotLK
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_WotLK_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_WotLK.MouseEnter
        If PictureBox_WotLK.Enabled Then
            Me.Cursor = Cursors.Hand
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ПОКИДАНИИ PictureBox WotLK
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_WotLK_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_WotLK.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

#End Region

End Class