
Imports System.Threading

Public Class Splash
    Public WithEvents LGV As New GV

    ''' <summary>
    ''' ПРИ ИНИЦИАЛИЗАЦИИ ФОРМЫ
    ''' </summary>
    Sub New()
        LGV.BaseInit()
        ' Если установлен новый тип сервера после перезагрузки
        If My.Settings.NextLoadServerType <> "" Then
            My.Settings.LastLoadedServerType = My.Settings.NextLoadServerType
            My.Settings.NextLoadServerType = ""
            My.Settings.Save()
        End If
        InitializeComponent()
        ' Первоначальные настройки
        Label_Error.Text = ""
        PictureBox1.Image = My.Resources.splash
        Opacity = 0
        ' Запускаем фэйдер входа
        FadeIn.Start()
    End Sub

    ''' <summary>
    ''' Событие из класса инициализации.
    ''' </summary>
    ''' <param name="code">Код сообщения.</param>
    ''' <param name="message">Сообщение.</param>
    Private Sub OutMessage(code As GV.ECodeSend, message As String) Handles LGV.OutMessage
        Select Case code

            Case GV.ECodeSend.Error
                PictureBox_Error.Image = My.Resources.warning
                Label_Error.Text = message
                GV.Log.WriteError(message)

            Case GV.ECodeSend.AddModule
                ' Добавлен новый модуль
                Select Case message
                    Case GV.EModule.Classic.ToString
                        Dim pb = GetPictureBox()
                        pb.Image = My.Resources.vanilla
                    Case GV.EModule.Tbc.ToString
                        Dim pb = GetPictureBox()
                        pb.Image = My.Resources.tbc
                    Case GV.EModule.Wotlk.ToString
                        Dim pb = GetPictureBox()
                        pb.Image = My.Resources.wotlk
                    Case Else
                        ' Неизвестный модуль
                        Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                        GV.Log.WriteError(str)
                        MessageBox.Show(str,
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                End Select
                GV.Log.WriteInfo(String.Format(My.Resources.P002_ModuleAdded, message))

            Case Else

        End Select
    End Sub

    ''' <summary>
    ''' Возвращает PictureBox согласно количеству найденных модулей
    ''' </summary>
    ''' <returns></returns>
    Private Function GetPictureBox() As PictureBox
        Select Case GV.Modules.Count
            Case 1
                Return PictureBox_M1
            Case 2
                Return PictureBox_M2
            Case 3
                Return PictureBox_M3
            Case Else
                Return PictureBox_Error
        End Select
    End Function


    ''' <summary>
    ''' Таймер FadeIn
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FadeIn_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles FadeIn.Tick
        Opacity += 0.04
        If Opacity >= 1 Then
            ' Останавливаем фэйдер и проверяем наличие уже запущенного лаунчера
            FadeIn.Stop()

            ' Запускаем полную инициализацию приложения
            If GV.ErrorCode = GV.ECode.ErrorLauncherAlready Then
                PictureBox_Error.Image = My.Resources.warning
                Label_Error.Text = My.Resources.E002_AlreadyRunning
            ElseIf GV.ErrorCode = GV.ECode.ErrorLauncherConfig Then
                PictureBox_Error.Image = My.Resources.warning
                Label_Error.Text = My.Resources.E011_ErrorMainConfig
            Else
                LGV.FullInit()
                If GV.ErrorCode = GV.ECode.OK Then FadeOut.Start()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Таймер FadeOut
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FadeOut_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles FadeOut.Tick
        Do While Opacity <> 0
            Me.Opacity -= 0.04
            Thread.Sleep(10)
        Loop
        ' Останавливаем фэйдер и проверяем код ошибки
        FadeOut.Stop()
        Threading.Thread.Sleep(1000)
        ' Выход если приложение уже запущено или не удалось загрузить конфигурацию лаунчера
        If GV.ErrorCode = GV.ECode.ErrorLauncherAlready Or
            GV.ErrorCode = GV.ECode.ErrorLauncherConfig Then
            GV.Log.WriteInfo(My.Resources.P005_Exiting)
            Close()
        ElseIf GV.ErrorCode = GV.ECode.ErrorBaseCat Or
                GV.ErrorCode = GV.ECode.ErrorMangoCat Or
                GV.ErrorCode = GV.ECode.ErrorModulesCat Then
            ' Запускаем настройки лаунчера
            Hide()
            Dim fLauncherSettings = New LauncherSettings
            fLauncherSettings.ShowDialog()
            Close()
        Else
            Me.Hide()
            ' Проверяем количество найденных модулей
            If GV.Modules.Count = 0 Then
                ' Нет ни одного модуля сервера WoW
                GV.Log.WriteInfo(My.Resources.P005_Exiting)
                Me.Close()
            ElseIf GV.Modules.Count = 1 Then
                ' У нас всего один сервер, его и запускаем
                My.Settings.LastLoadedServerType = GV.Modules.Item(0).ModuleType.ToString
                Dim fLauncher As New Launcher
                fLauncher.ShowDialog()
                Me.Close()
            ElseIf My.Settings.LastLoadedServerType = "" Or My.Settings.LastLoadedServerType = GV.EModule.Restart.ToString Then
                ' Было предложение о смене типа сервера
                Dim fServerSelector As New ServerSelector()
                fServerSelector.ShowDialog()
                Me.Close()
            Else
                ' Есть настройки автозапуска лаунчера
                Try
                    GV.SPP2Launcher = New Launcher
                    GV.SPP2Launcher.ShowDialog()
                    ' Если включен флаг - можно закрывать
                    If GV.SPP2Launcher.EnableClosing Then Me.Close()
                Catch ex As Exception
                    GV.Log.WriteException(ex)
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Нажатие кнопки мыши.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If Me.Opacity >= 1 AndAlso GV.ErrorCode > GV.ECode.OK Then
            ' Запускаем фэйдер выхода
            FadeOut.Start()
        End If
    End Sub

End Class