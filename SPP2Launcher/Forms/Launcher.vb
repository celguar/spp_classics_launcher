﻿
Imports System.Reflection
Imports System.Threading

Public Class Launcher

    ' Объект блокировки процедуры запуска World
    Private ReadOnly lockWorld As New Object

    ' Объект блокировки процедуры запуска Realmd
    Private ReadOnly lockRealmd As New Object

#Region " === ДРУЖЕСТВЕННЫЕ СВОЙСТВА === "

    ''' <summary>
    ''' Файл конфигурации сервера Realmd.
    ''' </summary>
    Friend ReadOnly Property IniRealmd As IniFiles

    ''' <summary>
    ''' Файл конфигурации сервера World.
    ''' </summary>
    Friend ReadOnly Property IniWorld As IniFiles

    ''' <summary>
    ''' Файл конфигурации PlayerBots
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property IniPlayerBots As IniFiles

    ''' <summary>
    ''' Файл конфигурации Auction House Bot.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property IniAhBot As IniFiles

    ''' <summary>
    ''' Флаг требования запуска сервера Realmd.
    ''' </summary>
    ''' <returns></returns>
    Friend Property NeedRealmdStart As Boolean

    ''' <summary>
    ''' Фильтр сообщений консоли World.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property CurrentWorldConsoleFilter As Integer

    ''' <summary>
    ''' Возвращает состояние процедуры создания резервных копий БД.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property IsBackupStarted As Boolean
        Get
            Return _isBackupStarted
        End Get
    End Property

    ''' <summary>
    ''' Флаг объявляющий требование выхода из программы.
    ''' </summary>
    Friend ReadOnly Property NeedExitLauncher As Boolean

    ''' <summary>
    ''' Возвращает состояние стартового потока.
    ''' </summary>
    ''' <returns></returns>
    Friend Property StartThreadCompleted As Boolean
        Get
            Return IsNothing(_isStart) OrElse _isStart.IsAlive
        End Get
        Set(value As Boolean)
            _isStart = Nothing
        End Set
    End Property

    ''' <summary>
    ''' Флаг немедленной остановки серверов WoW.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property NeedServerStop As Boolean

    Friend Property ServerIsStarting As Boolean

    ''' <summary>
    ''' Флаг автоматического запуска сервера WoW. Он же контроль от падения. 
    ''' </summary>
    Friend ReadOnly Property ServerWowAutostart As Boolean

    ''' <summary>
    ''' Флаг разрешения закрытия лаунчера.
    ''' </summary>
    Friend Property EnableClosing As Boolean

    ''' <summary>
    ''' Процесс REALMD
    ''' </summary>
    ''' <returns></returns>
    Friend Property RealmdProcess As Process

    ''' <summary>
    ''' Флаг работы Realmd.
    ''' </summary>
    ''' <returns></returns>
    Friend Property RealmdON As Boolean

    ''' <summary>
    ''' Процесс WORLD
    ''' </summary>
    ''' <returns></returns>
    Friend Property WorldProcess As Process

    ''' <summary>
    ''' Флаг работы World.
    ''' </summary>
    ''' <returns></returns>
    Friend Property WorldON As Boolean

    ''' <summary>
    ''' Флаг обнаружения включенного MySQL - БЕЗ РАЗНИЦЫ КАКОГО
    ''' </summary>
    ''' <returns></returns>
    Friend Property MysqlON As Boolean

    ''' <summary>
    ''' Флаг изоляции сервера MySQL.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property MySqlLOCKED As Boolean

    ''' <summary>
    ''' Флаг изоляции сервера Apache
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property ApacheLOCKED As Boolean

    ''' <summary>
    ''' Серверы WORLD или REALMD или ОБА заблокированы.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property ServersLOCKED As Boolean

#End Region

#Region " === ПРИВАТНЫЕ ПОЛЯ === "

    Private _firstRealmdStart As Boolean = True

    ''' <summary>
    ''' Флаг сообщающий о идущем процессе резервного сохранения.
    ''' </summary>
    Private _isBackupStarted As Boolean

    ''' <summary>
    ''' Флаг-требование запуска всех серверов WoW.
    ''' </summary>
    Private _NeedServerStart As Boolean

    ''' <summary>
    ''' Хранит команду ручного запуска сервера.
    ''' </summary>
    Private _isLastCommandStart As Boolean

    ''' <summary>
    ''' Буфер команд отправленных с консоли
    ''' </summary>
    Private ReadOnly ConsoleCommandBuffer As New ConsoleBuffer

    ''' <summary>
    ''' Поток при запуске лаунчера.
    ''' </summary>
    Private _isStart As Threading.Thread

    ''' <summary>
    ''' Процесс MySQL
    ''' </summary>
    Private _mysqlProcess As Process

    ''' <summary>
    ''' Флаг запрета запуска Apache
    ''' </summary>
    Private _apacheSTOP As Boolean

#End Region

#Region " === КОНСТРУКТОР ИНИЦИАЛИЗАЦИИ === "

    Sub New()

        ' Восстановим размеры и положение окна в буфер
        Dim sp = FormStartPosition.Manual
        Dim sz = My.Settings.AppSize
        Dim loc = My.Settings.AppLocation

        InitializeComponent()

        ' Перенесём размеры и положение из буфера в форму
        Me.StartPosition = sp
        Me.Size = sz

        ' Устанавливаем локацию
        If loc.X < 0 Or loc.Y < 0 Then
            ' Исправляем ошибку, если сервер был прихлопнут в свёрнутом состоянии
            loc = New Point(0, 0)
            My.Settings.AppLocation = loc
            My.Settings.Save()
        End If
        Me.Location = loc

        ' Если всего один модуль, то прячем смену типа сервера
        If GV.Modules.Count = 1 Then
            TSMI_ServerSwitcher.Visible = False
            TSMI_Sever1.Visible = False
        End If

        ' Настраиваем таймеры
        TimerStartMySQL = New Threading.Timer(AddressOf TimerTik_StartMySQL)
        TimerStartMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartWorld = New Threading.Timer(AddressOf TimerTik_StartWorld)
        TimerStartWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartRealmd = New Threading.Timer(AddressOf TimerTik_StartRealmd)
        TimerStartRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)

        ' Загружаем шрифт (попытка загрузить в память через GetFont пока не удалась - преследуют ошибки)
        LoadFont()

        ' Инициализируем BaseProcess
        BP = New ProcessController

        ' Первоначальная надпись в строке состояния ToolTip
        TSSL_Count.ToolTipText = String.Format(My.Resources.P011_AllChars, "N/A")

        ' Создаём каталоги сохранений
        Dim cat = SPP2SettingsProvider.SettingsFolder
        If Not IO.Directory.Exists(cat & "\Saves") Then IO.Directory.CreateDirectory(cat & "\Saves")
        If Not IO.Directory.Exists(cat & "\Saves\classic") Then IO.Directory.CreateDirectory(cat & "\Saves\classic")
        If Not IO.Directory.Exists(cat & "\Saves\classic\autosave") Then IO.Directory.CreateDirectory(cat & "\Saves\classic\autosave")
        If Not IO.Directory.Exists(cat & "\Saves\tbc") Then IO.Directory.CreateDirectory(cat & "\Saves\tbc")
        If Not IO.Directory.Exists(cat & "\Saves\tbc\autosave") Then IO.Directory.CreateDirectory(cat & "\Saves\tbc\autosave")
        If Not IO.Directory.Exists(cat & "\Saves\wotlk") Then IO.Directory.CreateDirectory(cat & "\Saves\wotlk")
        If Not IO.Directory.Exists(cat & "\Saves\wotlk\autosave") Then IO.Directory.CreateDirectory(cat & "\Saves\wotlk\autosave")

        ' Исправляем СТАРЫЕ параметры .cfg
        If My.Settings.ApacheClassicIntHost = "ANY" Then My.Settings.ApacheClassicIntHost = "0.0.0.0"
        If My.Settings.ApacheTbcIntHost = "ANY" Then My.Settings.ApacheTbcIntHost = "0.0.0.0"
        If My.Settings.ApacheWotlkIntHost = "ANY" Then My.Settings.ApacheWotlkIntHost = "0.0.0.0"
        My.Settings.Save()

    End Sub

#End Region

#Region " === СТАНДАРТЫЕ ОПЕРАЦИИ === "

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Инициализируем настройки приложения
        LoadSettings()

        ' Включем поток ежесекундного тика
        Dim tikTok = New Threading.Thread(Sub() EverySecond()) With {
            .CurrentCulture = GV.CI,
            .CurrentUICulture = GV.CI,
            .IsBackground = True
        }
        tikTok.Start()

        ' Включаем поток вывода команды разработчиков
        _isStart = New Threading.Thread(Sub() PreStart()) With {
            .CurrentCulture = GV.CI,
            .CurrentUICulture = GV.CI,
            .IsBackground = True
        }
        _isStart.Start()

    End Sub

    ''' <summary>
    ''' ПРИ ЗАКРЫТИИ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If EnableClosing = False Then
            Try
                Me.WindowState = FormWindowState.Minimized
            Catch
            End Try
            e.Cancel = True
        Else
            GV.Log.WriteInfo(My.Resources.P005_Exiting)
            Threading.Thread.Sleep(1000)
            GV.SPP2Launcher.NotifyIcon_SPP2.Visible = False
            If GV.NeedRestart Then Application.Restart()
        End If
    End Sub

#End Region

#Region " === ЗАПУСК КЛИЕНТА WOW === "

    ''' <summary>
    ''' Запуск клиента WoW
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_RunWow_Click(sender As Object, e As EventArgs) Handles TSMI_RunWow.Click
        StartWowClient()
    End Sub

    ''' <summary>
    ''' ЗАПУСКАЕТ WoW КЛИЕНТА
    ''' </summary>
    Private Sub StartWowClient()
        Try
            If My.Settings.WowClientPath = "" Then
                Using openFileDialog As New OpenFileDialog()
                    openFileDialog.Title = My.Resources.P070_PathWowClient
                    openFileDialog.InitialDirectory = My.Settings.DirSPP2
                    openFileDialog.Filter = My.Resources.P003_SetWowClientPath
                    openFileDialog.FilterIndex = 1
                    openFileDialog.RestoreDirectory = True

                    If openFileDialog.ShowDialog() = DialogResult.OK Then
                        My.Settings.WowClientPath = openFileDialog.FileName
                        My.Settings.Save()
                    End If
                End Using
            End If

            If My.Settings.WowClientPath <> "" And Not CheckProcess(EProcess.wow) Then
                Process.Start(My.Settings.WowClientPath)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region " === ЯЗЫК ИНТЕРФЕЙСА === "

    ''' <summary>
    ''' АНГЛИЙСКИЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_English_Click(sender As Object, e As EventArgs) Handles TSMI_English.Click
        Dim locale = "en-GB"
        Dim result = MessageBox.Show(String.Format(My.Resources.P015_ChangeLocale, locale),
                                     My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            GV.Log.WriteInfo(String.Format(My.Resources.P015_ChangeLocale, locale))
            My.Settings.Locale = locale
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' РУССКИЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Russian_Click(sender As Object, e As EventArgs) Handles TSMI_Russian.Click
        Dim locale = "ru-RU"
        Dim result = MessageBox.Show(String.Format(My.Resources.P015_ChangeLocale, locale),
                                     My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            GV.Log.WriteInfo(String.Format(My.Resources.P015_ChangeLocale, locale))
            My.Settings.Locale = locale
            My.Settings.Save()
        End If
    End Sub

#End Region

#Region " === ЛАУНЧЕР === "

    ''' <summary>
    ''' МЕНЮ - БОТЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Bots_Click(sender As Object, e As EventArgs) Handles TSMI_Bots.Click
        Dim fBots As New BotSettings
        fBots.ShowDialog()
    End Sub

    ''' <summary>
    ''' МЕНЮ - АККАУНТЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Accounts_Click(sender As Object, e As EventArgs) Handles TSMI_Accounts.Click
        If Not _MysqlON Then
            MessageBox.Show(My.Resources.P054_NeedMySQL,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim fAccounts As New Accounts
            fAccounts.ShowDialog()
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - СМЕНИТЬ IP
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_QuickSettings_Click(sender As Object, e As EventArgs) Handles TSMI_QuickSettings.Click
        If Not _MysqlON Then
            MessageBox.Show(My.Resources.P054_NeedMySQL,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else
            If CheckProcess(EProcess.realmd) Or CheckProcess(EProcess.world) Then
                MessageBox.Show(My.Resources.P059_FirstServerStop,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        End If
        Dim fLan = New QuickServerSet
        Dim res = fLan.ShowDialog()
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОТКРЫТЬ ЛОКАЛЬНЫЙ САЙТ РЕГИСТРАЦИИ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_OpenSite_Click(sender As Object, e As EventArgs) Handles TSMI_OpenSite.Click
        System.Diagnostics.Process.Start("http://127.0.0.1")
    End Sub

    ''' <summary>
    ''' МЕНЮ - НАСТРОЙКИ ПРИЛОЖЕНИЯ.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Launcher_Click(sender As Object, e As EventArgs) Handles TSMI_Launcher.Click
        Dim fLauncherSettings As New LauncherSettings
        fLauncherSettings.ShowDialog()
    End Sub

    ''' <summary>
    ''' МЕНЮ - СБРОС НАСТРОЕК ПРИЛОЖЕНИЯ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Reset_Click(sender As Object, e As EventArgs) Handles TSMI_Reset.Click
        If Not CheckProcess(EProcess.realmd) And Not CheckProcess(EProcess.world) Then
            Dim dr = MessageBox.Show(My.Resources.P014_Reset,
                                     My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If dr = DialogResult.Yes Then
                ' Устанавливаем флаги - нужна перезагрузка, это сброс настроек
                GV.NeedRestart = True
                GV.ResetSettings = True
                ' Останавливаем ВСЕ сервера
                ShutdownAll(True)
            End If
        End If
    End Sub

    ''' <summary>
    ''' КНОПКА - РАЗБЛОКИРОВАТЬ ВСЁ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_UnlockAll_Click(sender As Object, e As EventArgs) Handles Button_UnlockAll.Click
        Dim result = MessageBox.Show(My.Resources.P050_Exit1,
                                 My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.No Then Exit Sub
        RichTextBox_ConsoleMySQL.Clear()
        ' Блокируем меню серверов
        LockedServersMenu()
        _NeedExitLauncher = False
        _NeedServerStop = True
        _MySqlLOCKED = False
        _ServersLOCKED = False
        If _ApacheLOCKED Then _ApacheLOCKED = False : ShutdownApache()
        ShutdownWorld(True)
        'Button_UnlockAll.Visible = False
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ РАЗМЕРА ОКНА.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Me.Size.Width >= 640 And Me.Size.Height >= 420 Then
            My.Settings.AppSize = Me.Size
        End If
        My.Settings.Save()
        If Me.WindowState = FormWindowState.Minimized Then
            ' Эта штука переводит приложение в фоновый режим
            ' А ОНО НАДО?
            Me.Hide()
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИЕ ЛОКАЦИИ ПРИЛОЖЕНИЯ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_LocationChanged(sender As Object, e As EventArgs) Handles MyBase.LocationChanged
        If Location.X > 0 And Location.Y > 0 Then
            My.Settings.AppLocation = Location
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' Вывод сообщения в StatusStrip.
    ''' </summary>
    ''' <param name="text"></param>
    Friend Function UpdateMessageStatusStrip(text As String) As String
        If Me.InvokeRequired Then
            Me.Invoke(Function() As String
                          Return UpdateMessageStatusStrip(text)
                      End Function)
        Else
            TSSL_ALL.Text = text
        End If
        Return text
    End Function

    ''' <summary>
    ''' Обновление параметров для вывода на экран.
    ''' </summary>
    Friend Sub LoadSettings()

        ' Заголовок приложения
        Dim srv As String = ""
        Select Case My.Settings.LastLoadedServerType

            Case GV.EModule.Classic.ToString
                srv = "Classic"
                _IniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\realmd.conf")
                _IniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\mangosd.conf")
                _IniPlayerBots = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\aiplayerbot.conf")
                _IniAhBot = New IniFiles(My.Settings.DirSPP2 & "\" & "\" & SPP2SETTINGS & "\vanilla\ahbot.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\vanilla\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\vanilla\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\"
                _ServerWowAutostart = My.Settings.ServerClassicAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_classic_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))

            Case GV.EModule.Tbc.ToString
                srv = "The Burning Crusade"
                _IniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\realmd.conf")
                _IniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\mangosd.conf")
                _IniPlayerBots = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\aiplayerbot.conf")
                _IniAhBot = New IniFiles(My.Settings.DirSPP2 & "\" & "\" & SPP2SETTINGS & "\tbc\ahbot.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\tbc\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\tbc\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\"
                _ServerWowAutostart = My.Settings.ServerTbcAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_tbc_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))

            Case GV.EModule.Wotlk.ToString
                srv = "Wrath of the Lich King"
                _IniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\realmd.conf")
                _IniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\mangosd.conf")
                _IniPlayerBots = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\aiplayerbot.conf")
                _IniAhBot = New IniFiles(My.Settings.DirSPP2 & "\" & "\" & SPP2SETTINGS & "\wotlk\ahbot.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\wotlk\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\wotlk\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\"
                _ServerWowAutostart = My.Settings.ServerWotlkAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_wotlk_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))

            Case Else
                Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                GV.Log.WriteError(str)
                MessageBox.Show(str,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Select
        My.Settings.Save()

        ' Устанавливаем начальный уровень фильтрации сообщений консоли World
        _CurrentWorldConsoleFilter = My.Settings.ConsoleMessageFilter

        ' Выводим имя в заголовок
        Dim fv = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        Text = My.Resources.P010_LauncherCaption & " (" & fv.FileVersion & ")" & " : " & srv

        ' Инициализируем MySQL
        GV.SQL = New MySqlProvider()

        ' Настраиваем консоли
        Dim bc As Color = If(My.Settings.ConsoleTheme = "Black", Drawing.Color.Black, Drawing.Color.SeaShell)
        Dim fnt = GetCurrentFont()

        ' Realmd
        RichTextBox_ConsoleRealmd.ForeColor = My.Settings.RealmdConsoleForeColor
        RichTextBox_ConsoleRealmd.BackColor = bc
        RichTextBox_ConsoleRealmd.Font = fnt

        ' World
        RichTextBox_ConsoleWorld.ForeColor = My.Settings.WorldConsoleForeColor
        RichTextBox_ConsoleWorld.BackColor = bc
        RichTextBox_ConsoleWorld.Font = fnt

        ' MySQL
        RichTextBox_ConsoleMySQL.ForeColor = ECONSOLE
        RichTextBox_ConsoleMySQL.BackColor = bc
        RichTextBox_ConsoleMySQL.Font = fnt

        ' Настраиваем контроллер BP и запускаем процесс контроля
        BP.AddProcess(GV.EProcess.Realmd)
        BP.AddProcess(GV.EProcess.World)
        If Not IsNothing(PC) Then PC.Abort()
        PC = New Threading.Thread(Sub() Controller()) With {
                .CurrentCulture = GV.CI,
                .CurrentUICulture = GV.CI,
                .IsBackground = True
            }
        PC.CurrentCulture = GV.CI
        PC.CurrentUICulture = GV.CI
        PC.Start()

        ' Разбираемся с пунктом меню смены типа сервера
        If ServerWowAutostart Then
            ' Запрещаем доступ к меню смены сервера
            TSMI_ServerSwitcher.Enabled = False
            ' Запрещаем доступ к меню сброса настроек
            TSMI_Reset.Enabled = False
            ' Запрещаем доступ к меню запуска серверов
            TSMI_ServerStart.Enabled = False
        Else
            ' Разрешаем доступ к меню смены сервера
            TSMI_ServerSwitcher.Enabled = True
            ' Разрешаем доступ к меню сброса настроек
            TSMI_Reset.Enabled = True
            ' Разрешаем доступ к меню запуска серверов
            TSMI_ServerStart.Enabled = True
        End If

        ' Устанавливаем тему консоли.
        SetConsoleTheme()

        ' Выводим состояние пункта меню автозапуска сервера
        TSMI_WowAutoStart.Checked = ServerWowAutostart

        ' Активируем вкладку World
        TabControl1.SelectedTab = TabPage_World

        ' Разбираемся с меню сайта
        TSMI_OpenSite.Enabled = My.Settings.UseIntApache

        ' Настраиваем меню серверов.
        UpdateMainMenu(True)

    End Sub

    ''' <summary>
    ''' Блокировка меню всех серверов.
    ''' </summary>
    Friend Sub LockedServersMenu()
        If Not IsNothing(Me) Then
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              LockedServersMenu()
                          End Sub)
            Else
                TSMI_ServerStart.Enabled = False
                TSMI_ServerStop.Enabled = False
                TSMI_MySqlStart.Enabled = False
                TSMI_MySqlRestart.Enabled = False
                TSMI_MySqlStop.Enabled = False
                TSMI_ApacheStart.Enabled = False
                TSMI_ApacheRestart.Enabled = False
                TSMI_ApacheStop.Enabled = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Обновление параметров главного меню приложения.
    ''' </summary>
    ''' <param name="launchON">Флаг начального запуска приложения.</param>
    Friend Sub UpdateMainMenu(launchON As Boolean)
        If Not IsNothing(Me) Then
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              UpdateMainMenu(launchON)
                          End Sub)
            Else

                ' Обходим проверку блокировки серверов, если это выход из приложения
                If Not _NeedExitLauncher Then
                    If launchON Then

                        ' Проверяем процесс Apache которого не должно быть
                        If launchON AndAlso CheckProcess(EProcess.apache) Then _ApacheLOCKED = True Else _ApacheLOCKED = False

                        ' Проверяем процесс MySQL которого не должно быть
                        If CheckProcess(EProcess.mysqld) Then _MySqlLOCKED = True Else _MySqlLOCKED = False

                        ' Поверяем процесс World
                        If CheckProcess(EProcess.world) Then
                            _ServersLOCKED = True
                        Else
                            _ServersLOCKED = False
                            ' Однако проверим процесс Realmd и на всякий случай его кокнем
                            CheckProcess(EProcess.realmd, True)
                        End If

                    End If
                End If

                ' Обустраиваем вкладки
                If _ApacheLOCKED Or _MySqlLOCKED Or _ServersLOCKED Then
                    TabPage_MySQL.Text = "Errors"
                    TabControl1.SelectedTab = TabPage_MySQL
                Else
                    TabPage_MySQL.Text = "MySQL & Apache"
                End If

                ' Обустраиваем меню MySQL
                If My.Settings.UseIntMySQL Then
                    If _ServersLOCKED Then
                        TSMI_MySqlStart.Enabled = False
                        TSMI_MySqlRestart.Enabled = False
                        TSMI_MySqlStop.Enabled = False
                    ElseIf _MySqlLOCKED Then
                        TSMI_MySqlStart.Enabled = False
                        TSMI_MySqlRestart.Enabled = False
                        TSMI_MySqlStop.Enabled = True
                    ElseIf ServerWowAutostart Then
                        TSMI_MySqlStart.Enabled = False
                        TSMI_MySqlRestart.Enabled = False
                        TSMI_MySqlStop.Enabled = False
                    Else
                        TSMI_MySqlStart.Enabled = True
                        TSMI_MySqlRestart.Enabled = True
                        TSMI_MySqlStop.Enabled = True
                    End If
                Else
                    TSMI_MySqlStart.Enabled = False
                    TSMI_MySqlRestart.Enabled = False
                    TSMI_MySqlStop.Enabled = False
                End If

                ' Обустраиваем меню Apache
                If My.Settings.UseIntApache Then
                    If _ApacheLOCKED Then
                        TSMI_ApacheStart.Enabled = False
                        TSMI_ApacheRestart.Enabled = False
                        TSMI_ApacheStop.Enabled = True
                    Else
                        TSMI_ApacheStart.Enabled = True
                        TSMI_ApacheRestart.Enabled = True
                        TSMI_ApacheStop.Enabled = True
                    End If
                Else
                    TSMI_ApacheStart.Enabled = False
                    TSMI_ApacheRestart.Enabled = False
                    TSMI_ApacheStop.Enabled = False
                End If

                ' Обустраиваем меню Server
                If MySqlLOCKED Or _ServersLOCKED Then
                    TSMI_ServerStart.Enabled = False
                    TSMI_ServerStop.Enabled = False
                Else
                    TSMI_ServerStart.Enabled = True
                    TSMI_ServerStop.Enabled = True
                End If

                ' Если нет других mysqld.exe используется встроенный сервер MySQL и включен его автостарт и при этом это не первый запуск
                If Not MySqlLOCKED And
                    My.Settings.UseIntMySQL And
                    Not ApacheLOCKED And
                    My.Settings.MySqlAutostart And
                    launchON Then

                    ' Запускаем MySQL с ожиданием завершения другого процесса, если таковой был
                    WaitProcessEnd(EProcess.mysqld, EAction.Start, False)

                ElseIf Not MySqlLOCKED And
                    My.Settings.UseIntMySQL And
                    Not ApacheLOCKED And
                    Not _ServersLOCKED And
                    launchON And
                    ServerWowAutostart Then

                    ' Выставляем флаг необходимости запуска всего комплекса
                    _NeedServerStart = True
                    _isLastCommandStart = True

                    ' Изменяем пункты меню - ЗАПРЕЩАЕМ
                    ChangeServersMenu(False, False, False)

                    ' Запускаем MySQL
                    StartMySQL(Nothing)

                End If

                ' Отображение кнопки блокировки/разблокировки всех серверов
                If MySqlLOCKED Or ApacheLOCKED Or _ServersLOCKED Then
                    RichTextBox_ConsoleMySQL.Clear()
                    Button_UnlockAll.Visible = True
                    ' Запрещаем доступ до сброса настроек
                    TSMI_Reset.Enabled = False
                Else
                    Button_UnlockAll.Visible = False
                    If ServerWowAutostart Then
                        _NeedServerStart = True
                        ' Изменяем пункты меню - ЗАПРЕЩАЕМ
                        ChangeServersMenu(False, False, False)
                    End If
                End If

                ' ТЕПЕРЬ ПО LOCKED ПОДСКАЗКАМ
                If ApacheLOCKED Then
                    GV.Log.WriteWarning(OutMySqlConsole(String.Format(My.Resources.P046_ProcessDetected, "spp-httpd.exe"), ECONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked01, WCONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked02, WCONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked08 & vbCrLf, QCONSOLE))
                End If

                If _ServersLOCKED Then
                    GV.Log.WriteWarning(OutMySqlConsole(String.Format(My.Resources.P046_ProcessDetected, "mangosd.exe"), ECONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked05, WCONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked06, WCONSOLE))
                    GV.Log.WriteWarning(OutMySqlConsole(My.Resources.P047_Locked07 & vbCrLf, WCONSOLE))

                ElseIf MySqlLOCKED Then
                    GV.Log.WriteWarning(UpdateMySQLConsole(String.Format(My.Resources.P046_ProcessDetected, "mysqld.exe"), ECONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked03, WCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked09, WCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked04, WCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked10, QCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked11, QCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked12, QCONSOLE))
                    GV.Log.WriteWarning(UpdateMySQLConsole(My.Resources.P047_Locked13 & vbCrLf, QCONSOLE))
                End If

            End If
        End If
    End Sub

    ''' <summary>
    ''' Изменение пунктов меню запуска серверов, смены типа серверов и сброса настроек
    ''' </summary>
    ''' <param name="serversStart">Пункт меню запуска серверов WoW</param>
    ''' <param name="changeServerType">Пункт меню смены типа сервера WoW</param>
    ''' <param name="reset">Пункт меню сброса настроек приложения.</param>
    Public Sub ChangeServersMenu(serversStart As Boolean, changeServerType As Boolean, reset As Boolean)
        If Not IsNothing(Me) Then
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              ChangeServersMenu(serversStart, changeServerType, reset)
                          End Sub)
            Else
                ' Запрещаем доступ к меню запуска серверов
                TSMI_ServerStart.Enabled = serversStart
                TSMI_ServerSwitcher.Enabled = changeServerType
                TSMI_Reset.Enabled = reset
            End If
        End If
    End Sub

#End Region

#Region " === РЕЗЕРВНОЕ СОХРАНЕНИЕ === "

    ''' <summary>
    ''' Выполняет запуск потока автосохранения БД.
    ''' </summary>
    Friend Sub AutoBackups()
        _isBackupStarted = True
        Dim au As New Threading.Thread(AddressOf Backup) With {
            .CurrentCulture = GV.CI,
            .CurrentUICulture = GV.CI,
            .IsBackground = True
        }
        au.Start()
        UpdateMessageStatusStrip(My.Resources.P032_AutoBackup)
    End Sub

    ''' <summary>
    ''' Выполняет сохранение БД.
    ''' </summary>
    Private Sub Backup()
        Try
            If _MysqlON Then
                Dim suffix = Strings.Format(Date.Now, "yyyy-MM-dd HH-mm-ss")
                UpdateMySQLConsole(String.Format(My.Resources.P048_Backup, "REALMD"), QCONSOLE)
                MySqlDB.Backup.REALMD(suffix, True)
                Threading.Thread.Sleep(500)
                UpdateMySQLConsole(String.Format(My.Resources.P048_Backup, "CHARACTERS"), QCONSOLE)
                MySqlDB.Backup.CHARACTERS(suffix, True)
                Threading.Thread.Sleep(500)
                If My.Settings.BackupBotsDatabase Then
                    UpdateMySQLConsole(String.Format(My.Resources.P048_Backup, "PLAYERBOTS"), QCONSOLE)
                    MySqlDB.Backup.PLAYERBOTS(suffix, True)
                    Threading.Thread.Sleep(500)
                End If
                UpdateMessageStatusStrip("")
            End If
        Catch ex As Exception
            GV.Log.WriteException(UpdateMySQLConsole(ex.Message, ECONSOLE), ex)
        Finally
            _isBackupStarted = False
        End Try
    End Sub

#End Region

#Region " === СМЕНИТЬ ТИП СЕРВЕРА === "

    ''' <summary>
    ''' КНОПКА МЕНЮ - СМЕНА ТИПА СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerSwitcher_Click(sender As Object, e As EventArgs) Handles TSMI_ServerSwitcher.Click
        ' Выводим сообщение о перезагрузке приложения
        Dim dr = MessageBox.Show(My.Resources.P006_NeedReboot,
                                 My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If dr = DialogResult.Yes Then
            ' Устанавливаем флаг - нужна перезагрузка
            GV.NeedRestart = True
            ' Останавливаем ВСЕ сервера
            ShutdownAll(True)
        End If
    End Sub

#End Region

#Region " ===  MySQL === "

    ''' <summary>
    ''' МЕНЮ - ЗАПУСК MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlStart_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlStart.Click
        ' Активируем вкладку MySQL
        TabControl1.SelectedTab = TabPage_MySQL
        ' Если не изолирован и используется встроенный MySQL и отсутствует процесс тогда ВПЕРЁД
        If Not _MySqlLOCKED And My.Settings.UseIntMySQL And Not CheckProcess(EProcess.mysqld) Then TimerStartMySQL.Change(1000, 1000)
    End Sub

    ''' <summary>
    ''' МЕНЮ - ПЕРЕЗАПУСК MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlRestart_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlRestart.Click
        ' Если сервер не изолирован
        If Not _MySqlLOCKED Then
            ' Если не запущен Realmd и если не запущен World 
            If Not CheckProcess(EProcess.realmd) And Not CheckProcess(EProcess.world) Then
                ' Активируем вкладку MySQL
                TabControl1.SelectedTab = TabPage_MySQL
                ' Выключаем MySQl с последующим запуском
                ShutdownMySQL(EProcess.mysqld, EAction.Start)
            End If
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlStop_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlStop.Click
        ' Остановка MySQL с обновлением главного меню
        _MySqlLOCKED = False
        ' Активируем вкладку MySQL
        TabControl1.SelectedTab = TabPage_MySQL
        RichTextBox_ConsoleMySQL.Clear()
        ShutdownMySQL(EProcess.mysqld, EAction.UpdateMainMenu)
    End Sub

    ''' <summary>
    ''' МЕНЮ - НАСТРОЙКИ MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlSettings_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlSettings.Click
        Dim fMySqlSettings As New MySqlSettings
        fMySqlSettings.ShowDialog()
    End Sub

    ''' <summary>
    ''' Запускает сервер MySQL
    ''' </summary>
    Friend Sub StartMySQL(obj As Object)

        ' Проверяем изолированность
        If Not _MySqlLOCKED Then

            ' Если нет флага для выхода из программы и это встроенный MySQl, нет Realmd и нет World - запускаем
            If Not _NeedExitLauncher And My.Settings.UseIntMySQL And Not CheckProcess(EProcess.realmd) And Not CheckProcess(EProcess.world) Then

                ' Проверка на наличие процесса MySql - GOOD
                If Not CheckProcess(EProcess.mysqld) Then

                    ' ЭТО СТАРТ ПРОЦЕССА MYSQL
                    GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P042_ServerStart, "MySQL"), CONSOLE))

                    Dim exefile As String = My.Settings.DirSPP2 & "\" & SPP2MYSQL & "\bin\mysqld.exe"
                    Dim settings As String = My.Settings.DirSPP2 & "\" & SPP2MYSQL & "\SPP-Database.ini"
                    ' Создаём информацию о процессе
                    Dim startInfo = New ProcessStartInfo("cmd.exe") With {
                        .Arguments = "/C " & exefile & " --defaults-file=" & settings & " --standalone --console",
                        .CreateNoWindow = True,
                        .RedirectStandardInput = True,
                        .RedirectStandardOutput = True,
                        .RedirectStandardError = True,
                        .UseShellExecute = False,
                        .WindowStyle = ProcessWindowStyle.Hidden,
                        .WorkingDirectory = My.Settings.DirSPP2 & "\" & SPP2MYSQL
                    }
                    _mysqlProcess = New Process()

                    Try
                        _mysqlProcess.StartInfo = startInfo
                        ' Запускаем
                        If _mysqlProcess.Start() Then
                            GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P043_ServerStarted, "MySQL"), CONSOLE))
                            AddHandler _mysqlProcess.OutputDataReceived, AddressOf MySqlOutputDataReceived
                            AddHandler _mysqlProcess.ErrorDataReceived, AddressOf MySqlErrorDataReceived
                            AddHandler _mysqlProcess.Exited, AddressOf MySqlExited
                            _mysqlProcess.BeginOutputReadLine()
                            _mysqlProcess.BeginErrorReadLine()
                        Else
                            _mysqlProcess = Nothing
                        End If
                    Catch ex As Exception
                        ' MySQL выдал исключение
                        GV.Log.WriteException(ex)
                        UpdateMySQLConsole("[EConsole] " & String.Format(My.Resources.E019_StopException, "MySQL"), ECONSOLE)
                    End Try

                Else
                    ' Сервер уже запущен
                    GV.Log.WriteWarning(UpdateMySQLConsole(String.Format(My.Resources.P041_AlreadyStarted, "MySQL"), WCONSOLE))
                End If

            End If

        End If

    End Sub

    ''' <summary>
    ''' Выход из mysqld.exe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MySqlExited(ByVal sender As Object, ByVal e As EventArgs)
        RemoveHandler _mysqlProcess.OutputDataReceived, AddressOf MySqlOutputDataReceived
        RemoveHandler _mysqlProcess.ErrorDataReceived, AddressOf MySqlErrorDataReceived
        RemoveHandler _mysqlProcess.Exited, AddressOf MySqlExited
    End Sub

    ''' <summary>
    ''' Останавливает сервер MySQL
    ''' </summary>
    Friend Sub ShutdownMySQL(p As EProcess, a As EAction)

        If Not _MySqlLOCKED Then

            Do While CheckProcess(EProcess.world)
                Threading.Thread.Sleep(500)
            Loop

            ' Проверяем наличие процесса MySQL
            If CheckProcess(EProcess.mysqld) Then

                ' Остановка MySQL
                GV.SPP2Launcher.UpdateMessageStatusStrip(String.Format(My.Resources.P044_ServerStop, "MySQL"))
                GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P044_ServerStop, "MySQL"), CONSOLE))

                ' Задержка для нормальной отработки
                Threading.Thread.Sleep(500)

                If My.Settings.UseIntMySQL Then
                    Dim login, pass, port As String
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            login = My.Settings.MySqlClassicIntUserName
                            pass = My.Settings.MySqlClassicIntPassword
                            port = My.Settings.MySqlClassicIntPort
                        Case GV.EModule.Tbc.ToString
                            login = My.Settings.MySqlClassicIntUserName
                            pass = My.Settings.MySqlClassicIntPassword
                            port = My.Settings.MySqlClassicIntPort
                        Case GV.EModule.Wotlk.ToString
                            login = My.Settings.MySqlClassicIntUserName
                            pass = My.Settings.MySqlClassicIntPassword
                            port = My.Settings.MySqlClassicIntPort
                        Case Else
                            ' Неизвестный модуль
                            GV.Log.WriteWarning(String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType))
                            Exit Sub
                    End Select

                    ' Создаём информацию для процесса
                    Dim startInfo = New ProcessStartInfo With {
                    .CreateNoWindow = True,
                    .UseShellExecute = False,
                    .FileName = My.Settings.DirSPP2 & "\" & SPP2MYSQL & "\bin\mysqladmin.exe",
                    .Arguments = "-u " & login & " -p" & pass & " --port=" & port & " shutdown",
                    .WindowStyle = ProcessWindowStyle.Hidden
                }

                    ' Выполняем
                    Try
                        ' Блокируем меню серверов
                        LockedServersMenu()
                        Using exeProcess = Process.Start(startInfo)
                            exeProcess.WaitForExit()
                        End Using
                    Catch ex As Exception
                        ' Исключение при остановке MySQL
                        UpdateMainMenu(False)
                        GV.Log.WriteException(ex)
                        GV.Log.WriteError(UpdateMySQLConsole(String.Format(My.Resources.E019_StopException, "MySQL"), ECONSOLE))
                    End Try

                    ' Создаём поток для ожидания остановки MySQL
                    Dim wpe = New Threading.Thread(Sub() WaitProcessEnd(p, a)) With {
                    .CurrentCulture = GV.CI,
                    .CurrentUICulture = GV.CI,
                    .IsBackground = True
                }
                    wpe.Start()

                End If

            Else

                UpdateMessageStatusStrip("")

                ' Если на выход - то вот так
                If a = EAction.NeedExit Then
                    EnableClosing = True
                    If Me.InvokeRequired Then
                        Me.Invoke(Sub()
                                      Close()
                                  End Sub)
                    Else
                        Close()
                    End If

                Else a = EAction.UpdateMainMenu
                    GV.SPP2Launcher.UpdateMainMenu(False)
                End If
            End If

        End If

    End Sub

    ''' <summary>
    ''' Проверка соединения с сервером MySQL
    ''' Отсюда стартует автоматом Apache
    ''' Остюда стартует при _NeedServerStart и серверы WoW
    ''' </summary>
    Friend Sub CheckMySQL()
        Dim host, port As String
        If My.Settings.UseIntMySQL Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        End If

        If host = "" Or port = "" Then Exit Sub
        Dim tcpClient = New Net.Sockets.TcpClient
        Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)

        Try

            If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                tcpClient.Close()
                _MysqlON = False
            Else
                _MysqlON = True
                tcpClient.EndConnect(ac)
                tcpClient.Close()
            End If

        Catch ex As Exception
            GV.Log.WriteException(ex)
            _MysqlON = False

        Finally

            ac.AsyncWaitHandle.Close()
            ChangeIcons(ECaller.mysql)

            If CheckProcess(EProcess.mysqld) Then

                ' Проверить на флаг остановки
                If Not NeedServerStop Then

                    ' Автозапуск сервера Apache кроме как если не ручная остановка 
                    If _MysqlON And My.Settings.UseIntApache And My.Settings.ApacheAutostart And Not _apacheSTOP And Not CheckProcess(EProcess.apache) Then
                        ChangeApacheMenu(False, True, True)
                        StartApache()
                    End If

                    ' Поступил запрос на запуск серверов WoW
                    If _NeedServerStart And _MysqlON Then

                        ' Автозапуск сервера Apache если конкретная команда запуска серверов
                        If My.Settings.UseIntApache And My.Settings.ApacheAutostart And Not CheckProcess(EProcess.apache) Then
                            ChangeApacheMenu(False, True, True)
                            StartApache()
                        End If

                        If My.Settings.UseAutoBackupDatabase Then AutoBackups()

                        ' Надо изменить пункты меню
                        ChangeServersMenu(False, False, False)

                        ' Надо изменить меню MySQL - ВСЁ ЗАПРЕЩЕНО
                        ChangeMySqlMenu(False, False, False)

                        ' Запускаем World через 1 сек.
                        TimerStartWorld.Change(1000, 1000)
                        GV.Log.WriteInfo(String.Format(My.Resources.P021_TimerSetted, "world", "1000"))

                        ' Включаем флаг запуска Realmd
                        NeedRealmdStart = True

                        ' Выключаем флаг ручного запуска сервера
                        _NeedServerStart = False

                    ElseIf My.Settings.UseIntMySQL And Not _MysqlON Then

                        ' Надо изменить меню MySQL - ВСЁ РАЗРЕШЕНО
                        ChangeMySqlMenu(True, True, True)

                    End If

                Else

                    ' Поступил сигнал остановки - у нас ВНУТРЕННЕЕ MySQL?
                    If My.Settings.UseIntMySQL Then

                        ' Надо изменить меню MySQL
                        ChangeMySqlMenu(True, True, True)

                    End If
                End If

            Else

                ' Запуск если надо
                If _NeedServerStart And _MysqlON Then
                    Dim a = 1
                Else
                    ' MySQL молчит!
                    ' Надо изменить меню MySQL
                    If My.Settings.UseIntMySQL Then ChangeMySqlMenu(True, True, True)
                End If
            End If

        End Try

        ' Автоподсказки
        If CheckProcess(EProcess.mysqld) AndAlso HintCollection.Count = 0 Then
            Me.Invoke(Sub()
                          MySqlDB.MANGOS.COMMAND.SELECT_COMMAND(HintCollection)
                          TextBox_Command.AutoCompleteSource = AutoCompleteSource.CustomSource
                          TextBox_Command.AutoCompleteCustomSource = HintCollection
                          TextBox_Command.AutoCompleteMode = If(My.Settings.UseCommandAutoHints, AutoCompleteMode.SuggestAppend, AutoCompleteMode.None)
                      End Sub)
        End If

    End Sub

    ''' <summary>
    ''' Изменение ВСЕХ пунктов меню MySQL
    ''' </summary>
    ''' <param name="start">Доступ к меню запуска.</param>
    ''' <param name="restart">Доступ к меню перезапуска.</param>
    ''' <param name="stop">Доступ меню остановки.</param>
    Friend Sub ChangeMySqlMenu(start As Boolean, restart As Boolean, [stop] As Boolean)
        If Not IsNothing(Me) Then
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              ChangeMySqlMenu(start, restart, [stop])
                          End Sub)
            Else
                TSMI_MySqlStart.Enabled = start
                TSMI_MySqlRestart.Enabled = restart
                TSMI_MySqlStop.Enabled = [stop]
            End If
        End If
    End Sub

#End Region

#Region " === СЕРВЕР APACHE === "

    ''' <summary>
    ''' МЕНЮ - ЗАПУСК Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheStart_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheStart.Click
        If Not _ApacheLOCKED Then
            If Not CheckProcess(EProcess.apache) And _MysqlON Then
                _apacheSTOP = False
                ' Изменяем меню
                ChangeApacheMenu(False, True, True)
                ' Запускаем Apache
                StartApache()
            Else
                GV.Log.WriteError(OutMySqlConsole(String.Format(My.Resources.P051_NeedMySQL, "MySQL"), ECONSOLE))
            End If
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - ПЕРЕЗАПУСК Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheRestart_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheRestart.Click
        If Not _ApacheLOCKED Then
            _apacheSTOP = False
            ShutdownApache()
            If _MysqlON Then
                If My.Settings.ApacheAutostart Then
                    ' Меняем меню
                    ChangeApacheMenu(False, False, True)
                    ' Ничего не делаем, MySQL сам поднимет Apache
                Else
                    ' Меняем меню
                    ChangeApacheMenu(False, True, True)
                    ' Запускаем Apache
                    StartApache()
                End If
            Else
                ' Без MySQL Apache не фиг делать - сайт не подымется
                ChangeApacheMenu(False, False, False)
            End If
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheStop_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheStop.Click
        _apacheSTOP = True
        If _ApacheLOCKED Then
            ' Это остановка изолированного сервера
            _ApacheLOCKED = False
            RichTextBox_ConsoleMySQL.Clear()
            ChangeApacheMenu(True, True, True)
            ShutdownApache()
            UpdateMainMenu(False)
        Else
            ChangeApacheMenu(True, True, True)
            ShutdownApache()
            'UpdateMainMenu(False)
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - НАСТРОЙКА Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheSettings_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheSettings.Click
        Dim fApacheSettings As New ApacheSettings
        fApacheSettings.ShowDialog()
    End Sub

    ''' <summary>
    ''' Запускает сервер Apache.
    ''' </summary>
    Friend Sub StartApache()
        If Not _ApacheLOCKED And Not _MySqlLOCKED And Not _ServersLOCKED And Not _NeedExitLauncher Then
            If Not CheckProcess(EProcess.apache) Then
                GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P042_ServerStart, "Apache"), CONSOLE))
                ' Разбираемся с настройками Apache
                If My.Settings.UseIntApache Then
                    Dim listen As String = ""
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            listen = If(My.Settings.ApacheClassicIntHost = "0.0.0.0",
                                        "Listen " & My.Settings.ApacheClassicIntPort,
                                        "Listen " & My.Settings.ApacheClassicIntHost & ":" & My.Settings.ApacheClassicIntPort)
                        Case GV.EModule.Tbc.ToString
                            listen = If(My.Settings.ApacheTbcIntHost = "0.0.0.0",
                                        "Listen " & My.Settings.ApacheTbcIntPort,
                                        "Listen " & My.Settings.ApacheTbcIntHost & ":" & My.Settings.ApacheTbcIntPort)
                        Case GV.EModule.Wotlk.ToString
                            listen = If(My.Settings.ApacheWotlkIntHost = "0.0.0.0",
                                        "Listen " & My.Settings.ApacheWotlkIntPort,
                                        "Listen " & My.Settings.ApacheWotlkIntHost & ":" & My.Settings.ApacheWotlkIntPort)
                    End Select
                    If listen <> "" Then
                        ' Устанавливаем в httpd.conf текущие настройки
                        Dim lines() As String = System.IO.File.ReadAllLines(My.Settings.DirSPP2 & "\" & SPP2APACHE & "\conf\httpd.conf")
                        For count = 0 To lines.Length - 1
                            If lines(count).StartsWith("Listen ") Then
                                lines(count) = listen
                                System.IO.File.WriteAllLines(My.Settings.DirSPP2 & "\" & SPP2APACHE & "\conf\httpd.conf", lines)
                                Exit For
                            End If
                        Next
                    End If
                End If
                ' Создаём информацию о процессе
                Dim startInfo = New ProcessStartInfo() With {
                    .FileName = "CMD.exe",
                    .WorkingDirectory = My.Settings.DirSPP2 & "\" & SPP2APACHE & "\",
                    .Arguments = "/C " & My.Settings.DirSPP2 & "\" & SPP2APACHE & "\bin\spp-httpd.exe",
                    .CreateNoWindow = True,
                    .UseShellExecute = False,
                    .WindowStyle = ProcessWindowStyle.Normal
                }

                Dim p As New Process
                Try
                    p.StartInfo = startInfo
                    ' Запускаем
                    If p.Start() Then
                        GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P043_ServerStarted, "Apache"), CONSOLE))
                    Else
                        ' Не удалось запустить сервер
                        GV.Log.WriteError(UpdateMySQLConsole(String.Format(My.Resources.P049_NotStarted, "Apache"), ECONSOLE))
                    End If
                Catch ex As Exception
                    ' Apache выдал исключение
                    GV.Log.WriteException(ex)
                    GV.Log.WriteError(UpdateMySQLConsole(String.Format(My.Resources.E018_StartException, "Apache"), ECONSOLE))
                End Try
            Else
                GV.Log.WriteWarning(UpdateMySQLConsole(String.Format(My.Resources.P041_AlreadyStarted, "Apache"), WCONSOLE))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Выключает встренный сервер Apache.
    ''' </summary>
    Friend Sub ShutdownApache()
        If Not _ApacheLOCKED Then
            GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P044_ServerStop, "Apache"), CONSOLE))
            Try
                ' Сначала так
                Dim id = GetApachePid()
                ' Если есть - убиваем головной процесс
                If id > 0 Then Process.GetProcessById(id).Kill()
                ' Затем проверяем наличие процесса/процессов и если что ТОЖЕ УБИВАЕМ!!!
                CheckProcess(EProcess.apache, True)
                TSSL_Apache.GetCurrentParent().Invoke(Sub()
                                                          TSSL_Apache.Image = My.Resources.red_ball
                                                      End Sub)
                GV.Log.WriteInfo(UpdateMySQLConsole(String.Format(My.Resources.P045_ServerStopped, "Apache"), CONSOLE))
                'If Not _NeedExitLauncher Then UpdateMainMenu(False)
            Catch ex As Exception
                ' Исключение при остановке Apache
                GV.Log.WriteException(ex)
                GV.Log.WriteError(UpdateMySQLConsole(String.Format(My.Resources.E019_StopException, "Apache"), ECONSOLE))
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Возвращает PID локального процесса Apache
    ''' </summary>
    ''' <returns></returns>
    Friend Function GetApachePid() As Integer
        Try
            If IO.File.Exists(My.Settings.DirSPP2 & "\" & SPP2APACHE & "\logs\httpd.pid") Then
                Dim line() = IO.File.ReadAllLines(My.Settings.DirSPP2 & "\" & SPP2APACHE & "\logs\httpd.pid", System.Text.Encoding.Default)
                Return CInt(line(0))
            Else
                Return 0
            End If
        Catch ex As Exception
            GV.Log.WriteException(ex)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Проверка доступности Web сервера.
    ''' </summary>
    Friend Sub CheckHttp()
        Dim host, port As String
        If My.Settings.UseIntApache Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.ApacheClassicIntHost
                    port = My.Settings.ApacheClassicIntPort
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.ApacheClassicIntHost
                    port = My.Settings.ApacheClassicIntPort
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.ApacheClassicIntHost
                    port = My.Settings.ApacheClassicIntPort
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.ApacheClassicExtHost
                    port = My.Settings.ApacheClassicExtPort
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.ApacheClassicExtHost
                    port = My.Settings.ApacheClassicExtPort
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.ApacheClassicExtHost
                    port = My.Settings.ApacheClassicExtPort
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        End If
        If host = "" Or port = "" Then Exit Sub
        If host = "0.0.0.0" Then host = "127.0.0.1"
        Dim tcpClient = New Net.Sockets.TcpClient
        Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)
        Try
            If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(0.5), False) Then
                tcpClient.Close()
                ' Красный
                ChangeApacheStatusIcon(False)
            Else
                tcpClient.EndConnect(ac)
                tcpClient.Close()
                ' Зелёный
                ChangeApacheStatusIcon(True)
            End If
        Catch ex As Exception
            ' Красный
            ChangeApacheStatusIcon(False)
            GV.Log.WriteException(ex)
        Finally
            ac.AsyncWaitHandle.Close()
        End Try

    End Sub

    ''' <summary>
    ''' Изменение пунктов меню Apache.
    ''' </summary>
    ''' <param name="start">Доступ к меню запуска.</param>
    ''' <param name="restart">Доступ к меню перезапуска.</param>
    ''' <param name="[stop]">Доступ к меню остановки.</param>
    Friend Sub ChangeApacheMenu(start As Boolean, restart As Boolean, [stop] As Boolean)
        Try
            If Not IsNothing(Me) Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  ChangeApacheMenu(start, restart, [stop])
                              End Sub)
                Else
                    TSMI_ApacheStart.Enabled = start
                    TSMI_ApacheRestart.Enabled = restart
                    TSMI_ApacheStop.Enabled = [stop]
                End If
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Если status = True - зелёный...
    ''' </summary>
    ''' <param name="status"></param>
    Friend Sub ChangeApacheStatusIcon(status As Boolean)
        If Not IsNothing(Me) Then
            If Me.Visible Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  ChangeApacheStatusIcon(status)
                              End Sub)
                Else
                    If status Then
                        TSSL_Apache.Image = My.Resources.green_ball
                    Else
                        TSSL_Apache.Image = My.Resources.red_ball
                    End If
                End If
            Else
                ' Других иконок для Apache нет.
            End If
        End If
    End Sub

#End Region

#Region " === СЕРВЕР WORLD === "

    ''' <summary>
    ''' Запускает сервер мира...
    ''' </summary>
    Friend Sub StartWorld(obj As Object)
        SyncLock lockWorld
            If Not _WorldON Then
                If CheckProcess(EProcess.mysqld) And Not _NeedExitLauncher And Not NeedServerStop And IsNothing(_WorldProcess) Then

                    Do
                        If Not IsBackupStarted Then Exit Do
                        Threading.Thread.Sleep(500)
                    Loop

                    GV.Log.WriteInfo(String.Format(My.Resources.P042_ServerStart, "World"))

                    ' Исключаем повторный запуск World
                    _WorldON = True

                    ServerIsStarting = True

                    If ServerWowAutostart Then
                        ' Пишем - контроль включён
                        GV.SPP2Launcher.UpdateWorldConsole(My.Resources.P019_ControlEnabled, CONSOLE)
                    Else
                        ' Пишем - контроль отключен
                        GV.SPP2Launcher.UpdateWorldConsole(My.Resources.P030_ControlDisabled, CONSOLE)
                    End If

                    Dim login As String = ""
                    Dim world As String = ""
                    Dim characters As String = ""
                    Dim logs As String = ""
                    Dim playerbots As String = ""
                    If My.Settings.UseIntMySQL Then
                        Select Case My.Settings.LastLoadedServerType
                            Case GV.EModule.Classic.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlClassicIntHost & ";" &
                                    My.Settings.MySqlClassicIntPort & ";" &
                                    My.Settings.MySqlClassicIntUserName & ";" &
                                    My.Settings.MySqlClassicIntPassword & ";"
                                login &= base & My.Settings.MySqlClassicIntRealmd & Chr(34)
                                world &= base & My.Settings.MySqlClassicIntMangos & Chr(34)
                                characters &= base & My.Settings.MySqlClassicIntCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlClassicIntLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlClassicIntPlayerbots & Chr(34)
                            Case GV.EModule.Tbc.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlClassicIntHost & ";" &
                                    My.Settings.MySqlTbcIntPort & ";" &
                                    My.Settings.MySqlTbcIntUserName & ";" &
                                    My.Settings.MySqlTbcIntPassword & ";"
                                login &= base & My.Settings.MySqlTbcIntRealmd & Chr(34)
                                world &= base & My.Settings.MySqlTbcIntMangos & Chr(34)
                                characters &= base & My.Settings.MySqlTbcIntCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlTbcIntLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlTbcIntPlayerbots & Chr(34)
                            Case GV.EModule.Wotlk.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlWotlkIntHost & ";" &
                                    My.Settings.MySqlWotlkIntPort & ";" &
                                    My.Settings.MySqlWotlkIntUserName & ";" &
                                    My.Settings.MySqlWotlkIntPassword & ";"
                                login &= base & My.Settings.MySqlWotlkIntRealmd & Chr(34)
                                world &= base & My.Settings.MySqlWotlkIntMangos & Chr(34)
                                characters &= base & My.Settings.MySqlWotlkIntCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlWotlkIntLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlWotlkIntPlayerbots & Chr(34)
                            Case Else
                                ' Неизвестный модуль
                                GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                                Exit Sub
                        End Select
                    Else
                        Select Case My.Settings.LastLoadedServerType
                            Case GV.EModule.Classic.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlClassicExtHost & ";" &
                                    My.Settings.MySqlClassicExtPort & ";" &
                                    My.Settings.MySqlClassicExtUserName & ";" &
                                    My.Settings.MySqlClassicExtPassword & ";"
                                login &= base & My.Settings.MySqlClassicExtRealmd & Chr(34)
                                world &= base & My.Settings.MySqlClassicExtMangos & Chr(34)
                                characters &= base & My.Settings.MySqlClassicExtCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlClassicExtLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlClassicExtPlayerbots & Chr(34)
                            Case GV.EModule.Tbc.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlTbcExtHost & ";" &
                                    My.Settings.MySqlTbcExtPort & ";" &
                                    My.Settings.MySqlTbcExtUserName & ";" &
                                    My.Settings.MySqlTbcExtPassword & ";"
                                login &= base & My.Settings.MySqlTbcExtRealmd & Chr(34)
                                world &= base & My.Settings.MySqlTbcExtMangos & Chr(34)
                                characters &= base & My.Settings.MySqlTbcExtCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlTbcExtLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlTbcExtPlayerbots & Chr(34)
                            Case GV.EModule.Wotlk.ToString
                                Dim base As String = Chr(34) & My.Settings.MySqlWotlkExtHost & ";" &
                                    My.Settings.MySqlWotlkExtPort & ";" &
                                    My.Settings.MySqlWotlkExtUserName & ";" &
                                    My.Settings.MySqlWotlkExtPassword & ";"
                                login &= base & My.Settings.MySqlWotlkExtRealmd & Chr(34)
                                world &= base & My.Settings.MySqlWotlkExtMangos & Chr(34)
                                characters &= base & My.Settings.MySqlWotlkExtCharacters & Chr(34)
                                logs &= base & My.Settings.MySqlWotlkExtLogs & Chr(34)
                                playerbots &= base & My.Settings.MySqlWotlkExtPlayerbots & Chr(34)
                            Case Else
                                ' Неизвестный модуль
                                GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                                Exit Sub
                        End Select
                    End If

                    ' Правим файл конфигурации World
                    _IniWorld.Write("MangosdConf", "LoginDatabaseInfo", login)
                    _IniWorld.Write("MangosdConf", "WorldDatabaseInfo", world)
                    _IniWorld.Write("MangosdConf", "CharacterDatabaseInfo", characters)
                    _IniWorld.Write("MangosdConf", "LogsDatabaseInfo", logs)
                    _IniWorld.Write("MangosdConf", "PlayerbotDatabaseInfo", playerbots)

                    ' Создаём информацию о процессе
                    Dim startInfo = New ProcessStartInfo(My.Settings.CurrentFileWorld) With {
                        .CreateNoWindow = True,
                        .RedirectStandardInput = True,
                        .RedirectStandardOutput = True,
                        .RedirectStandardError = True,
                        .UseShellExecute = False,
                        .WindowStyle = ProcessWindowStyle.Normal,
                        .WorkingDirectory = My.Settings.CurrentServerSettings
                    }

                    _WorldProcess = New Process()
                    Try
                        _WorldProcess.StartInfo = startInfo
                        BP.WasLaunched(GV.EProcess.World)
                        ' Запускаем
                        If _WorldProcess.Start() Then
                            GV.Log.WriteInfo(String.Format(My.Resources.P043_ServerStarted, "World"))
                            AddHandler _WorldProcess.OutputDataReceived, AddressOf WorldOutputDataReceived
                            AddHandler _WorldProcess.ErrorDataReceived, AddressOf WorldErrorDataReceived
                            AddHandler _WorldProcess.Exited, AddressOf WorldExited
                            _WorldProcess.BeginOutputReadLine()
                            _WorldProcess.BeginErrorReadLine()
                        Else
                            _WorldProcess = Nothing
                        End If
                    Catch ex As Exception
                        ' World выдал исключение
                        _WorldON = False
                        Try
                            _WorldProcess.Dispose()
                        Catch
                        End Try
                        GV.Log.WriteException(ex)
                        MessageBox.Show(My.Resources.E014_WorldException & vbLf & ex.Message,
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Else
                    'TimerStartWorld.Change(2000, 2000)
                    'GV.Log.WriteWarning(My.Resources.P035_ThereIsProblems)
                End If
            Else
                GV.Log.WriteInfo(String.Format(My.Resources.P041_AlreadyStarted, "World"))
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Выход из world.exe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldExited(ByVal sender As Object, ByVal e As EventArgs)
        GV.Log.WriteInfo(String.Format(My.Resources.P045_ServerStopped, "World"))
        RemoveHandler _WorldProcess.OutputDataReceived, AddressOf WorldOutputDataReceived
        RemoveHandler _WorldProcess.ErrorDataReceived, AddressOf WorldErrorDataReceived
        RemoveHandler _WorldProcess.Exited, AddressOf RealmdExited
    End Sub

    ''' <summary>
    ''' Останавливает сервер World.
    ''' </summary>
    ''' <param name="otherServers">Выключить так же и все прочие сервера.</param>
    Friend Sub ShutdownWorld(otherServers As Boolean)
        Dim processes = GetAllProcesses()
        Dim pc = processes.FindAll(Function(p) p.ProcessName = "mangosd")
        Dim id = 0

        ' Если серверы заблокированы но надо выйти из приложения - не гасим серверы WoW!
        If Not (_ServersLOCKED And _NeedExitLauncher) Then
            ' Идём глядеть
            If pc.Count > 0 Then
                For Each process In pc
                    Try
                        If process.MainModule.FileName = My.Settings.CurrentFileWorld Then
                            id = process.Id
                            UpdateWorldConsole(vbCrLf & My.Resources.P020_NeedServerStop & vbCrLf, CONSOLE)
                            If _WorldON Then
                                ' Сервер нас слышит, поэтому отправляем saveall и server shutdown  согласно требованиям разработчиков
                                SendCommandToWorld(".saveall")
                                ' Делаем паузу
                                Threading.Thread.Sleep(5000)
                                SendCommandToWorld("server shutdown 1")
                            Else
                                ' Сервер не готов нас слушать, поэтому удаляем хандлеры и киллим процесс world
                                If Not IsNothing(_WorldProcess) Then WorldExited(Me, Nothing)
                                Try
                                    process.Kill()
                                    _WorldON = False
                                    _WorldProcess = Nothing
                                Catch
                                End Try
                            End If
                        End If
                    Catch
                        ' Нет доступа.
                    End Try
                Next
            Else
                _WorldON = False
                ' Удалям хандлеры, если таковые были
                If Not IsNothing(_WorldProcess) Then WorldExited(Me, Nothing)
                _WorldProcess = Nothing
            End If

        End If

        ' Завершаем остановку сервера World в потоке
        Dim sw As New Threading.Thread(Sub() StoppingWorld(id, otherServers)) With {
                .CurrentUICulture = GV.CI,
                .CurrentCulture = GV.CI,
                .IsBackground = True
            }
        sw.Start()

    End Sub

    ''' <summary>
    ''' Проверяет доступность сервера World.
    ''' </summary>
    Friend Sub CheckWorld()
        Try
            Dim host = _IniWorld.ReadString("MangosdConf", "BindIP", "127.0.0.1")
            If host = "0.0.0.0" Then host = "127.0.0.1"
            Dim port = _IniWorld.ReadString("MangosdConf", "WorldServerPort", "8085")
            Dim tcpClient = New Net.Sockets.TcpClient
            Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)

            Try

                If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                    tcpClient.Close()
                    _WorldON = False
                Else
                    tcpClient.EndConnect(ac)
                    tcpClient.Close()
                    _WorldON = True
                End If

            Catch ex As Exception
                _WorldON = False
                GV.Log.WriteException(ex)

            Finally
                ChangeIcons(ECaller.world)
                ac.AsyncWaitHandle.Close()
                If WorldON And NeedRealmdStart Then StartRealmd(Nothing)
            End Try

        Catch ex As Exception
            GV.Log.WriteException(ex)
            MessageBox.Show(ex.Message,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If WorldON Then
                ' Устанавливаем новый уровень фильтрации сообщений консоли World
                _CurrentWorldConsoleFilter = My.Settings.ConsolePostMessageFilter
            Else
                ' Устанавливаем начальный уровень фильтрации сообщений консоли World
                _CurrentWorldConsoleFilter = My.Settings.ConsoleMessageFilter

                If Not IsNothing(_WorldProcess) And Not _NeedServerStart And Not _NeedExitLauncher And Not NeedServerStop And Not ServerIsStarting Then

                    If Not IsNothing(_WorldProcess) Then WorldExited(Me, Nothing)
                    Try
                        _WorldProcess.Kill()
                        _WorldON = False
                        _WorldProcess = Nothing
                        _WorldProcess.Dispose()
                    Catch
                    End Try
                    'ShutdownWorld(False)

                    WorldStartTime = 0
                    GV.SPP2Launcher.UpdateMessageStatusStrip("")
                    ' Сервер рухнул
                    Dim msg = String.Format(My.Resources.P045_ServerStopped, "world",
                                            If(GV.SPP2Launcher.ServerWowAutostart, My.Resources.P055_RestartIs, ""))
                    GV.Log.WriteError(msg)
                    GV.SPP2Launcher.UpdateWorldConsole(msg, Color.Red)
                    ' Устанавливаем перезапуск (если автостарт или Ручной) через 10 секунд
                    TimerStartWorld.Change(10000, 10000)
                    'If ServerWowAutostart Then
                    '    ' Запускаем поток
                    '    Dim sq As New Threading.Thread(AddressOf StartWorld) With {
                    '        .CurrentCulture = GV.CI,
                    '        .CurrentUICulture = GV.CI,
                    '        .IsBackground = True
                    '    }
                    '    sq.Start()
                    'End If
                    If ServerWowAutostart Then ServerIsStarting = True
                End If
            End If
        End Try
    End Sub

#End Region

#Region " === СЕРВЕР REALMD === "

    ''' <summary>
    ''' Запускает сервер авторизации.
    ''' </summary>
    Friend Sub StartRealmd(ob As Object)
        SyncLock lockRealmd
            NeedRealmdStart = False
            If Not _RealmdON Then
                If CheckProcess(EProcess.mysqld) And Not NeedServerStop And Not _NeedExitLauncher And IsNothing(_RealmdProcess) Then
                    GV.Log.WriteInfo(String.Format(My.Resources.P042_ServerStart, "Realm"))

                    ' Исключаем повторный запуск Realmd
                    _RealmdON = True

                    If ServerWowAutostart Then
                        ' Пишем - контроль включён
                        GV.SPP2Launcher.UpdateRealmdConsole(My.Resources.P019_ControlEnabled, CONSOLE)
                    Else
                        ' Пишем - контроль отключен
                        GV.SPP2Launcher.UpdateRealmdConsole(My.Resources.P030_ControlDisabled, CONSOLE)
                    End If

                    Dim value As String = ""
                    If My.Settings.UseIntMySQL Then
                        Select Case My.Settings.LastLoadedServerType
                            Case GV.EModule.Classic.ToString
                                value = Chr(34) & My.Settings.MySqlClassicIntHost & ";" &
                                        My.Settings.MySqlClassicIntPort & ";" &
                                        My.Settings.MySqlClassicIntUserName & ";" &
                                        My.Settings.MySqlClassicIntPassword & ";" &
                                        My.Settings.MySqlClassicIntRealmd & Chr(34)
                            Case GV.EModule.Tbc.ToString
                                value = Chr(34) & My.Settings.MySqlTbcIntHost & ";" &
                                        My.Settings.MySqlTbcIntPort & ";" &
                                        My.Settings.MySqlTbcIntUserName & ";" &
                                        My.Settings.MySqlTbcIntPassword & ";" &
                                        My.Settings.MySqlTbcIntRealmd & Chr(34)
                            Case GV.EModule.Wotlk.ToString
                                value = Chr(34) & My.Settings.MySqlWotlkIntHost & ";" &
                                        My.Settings.MySqlWotlkIntPort & ";" &
                                        My.Settings.MySqlWotlkIntUserName & ";" &
                                        My.Settings.MySqlWotlkIntPassword & ";" &
                                        My.Settings.MySqlWotlkIntRealmd & Chr(34)
                            Case Else
                                ' Неизвестный модуль
                                GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                                Exit Sub
                        End Select
                    Else
                        Select Case My.Settings.LastLoadedServerType
                            Case GV.EModule.Classic.ToString
                                value = Chr(34) & My.Settings.MySqlClassicExtHost & ";" &
                                        My.Settings.MySqlClassicExtPort & ";" &
                                        My.Settings.MySqlClassicExtUserName & ";" &
                                        My.Settings.MySqlClassicExtPassword & ";" &
                                        My.Settings.MySqlClassicExtRealmd & Chr(34)
                            Case GV.EModule.Tbc.ToString
                                value = Chr(34) & My.Settings.MySqlTbcExtHost & ";" &
                                        My.Settings.MySqlTbcExtPort & ";" &
                                        My.Settings.MySqlTbcExtUserName & ";" &
                                        My.Settings.MySqlTbcExtPassword & ";" &
                                        My.Settings.MySqlTbcExtRealmd & Chr(34)
                            Case GV.EModule.Wotlk.ToString
                                value = Chr(34) & My.Settings.MySqlWotlkExtHost & ";" &
                                        My.Settings.MySqlWotlkExtPort & ";" &
                                        My.Settings.MySqlWotlkExtUserName & ";" &
                                        My.Settings.MySqlWotlkExtPassword & ";" &
                                        My.Settings.MySqlWotlkExtRealmd & Chr(34)
                            Case Else
                                ' Неизвестный модуль
                                GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
                                Exit Sub
                        End Select
                    End If

                    ' Правим файл конфигурации Realmd
                    _IniRealmd.Write("RealmdConf", "LoginDatabaseInfo", value)

                    ' Создаём информацию о процессе
                    Dim startInfo = New ProcessStartInfo(My.Settings.CurrentFileRealmd) With {
                            .CreateNoWindow = True,
                            .RedirectStandardInput = True,
                            .RedirectStandardOutput = True,
                            .RedirectStandardError = True,
                            .UseShellExecute = False,
                            .WindowStyle = ProcessWindowStyle.Normal,
                            .WorkingDirectory = My.Settings.CurrentServerSettings
                        }

                    _RealmdProcess = New Process()

                    Try
                        _RealmdProcess.StartInfo = startInfo
                        BP.WasLaunched(GV.EProcess.Realmd)
                        ' Запускаем
                        If _RealmdProcess.Start() Then
                            GV.Log.WriteInfo(String.Format(My.Resources.P043_ServerStarted, "Realm"))
                            AddHandler _RealmdProcess.OutputDataReceived, AddressOf RealmdOutputDataReceived
                            AddHandler _RealmdProcess.ErrorDataReceived, AddressOf RealmdErrorDataReceived
                            AddHandler _RealmdProcess.Exited, AddressOf RealmdExited
                            _RealmdProcess.BeginOutputReadLine()
                            _RealmdProcess.BeginErrorReadLine()
                        Else
                            _RealmdProcess = Nothing
                        End If
                    Catch ex As Exception
                        ' Realmd выдал исключение
                        _RealmdON = False
                        Try
                            _RealmdProcess.Dispose()
                        Catch
                        End Try
                        GV.Log.WriteException(ex)
                        MessageBox.Show(My.Resources.E012_RealmdException & vbLf & ex.Message,
                                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            Else
                GV.Log.WriteInfo(String.Format(My.Resources.P041_AlreadyStarted, "Realmd"))
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Выход из realmd.exe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RealmdExited(ByVal sender As Object, ByVal e As EventArgs)
        GV.Log.WriteInfo(String.Format(My.Resources.P045_ServerStopped, "Realm"))
        RemoveHandler _RealmdProcess.OutputDataReceived, AddressOf RealmdOutputDataReceived
        RemoveHandler _RealmdProcess.ErrorDataReceived, AddressOf RealmdErrorDataReceived
        RemoveHandler _RealmdProcess.Exited, AddressOf RealmdExited
    End Sub

    ''' <summary>
    ''' Останавливает сервер Realmd.
    ''' </summary>
    Friend Sub ShutdownRealmd()
        If Not _ServersLOCKED Then
            CheckProcess(EProcess.realmd, True)
            _RealmdON = False
            ' Удаляем хандлеры, если таковые были
            If Not IsNothing(_RealmdProcess) Then RealmdExited(Me, Nothing)
            _RealmdProcess = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Проверка доступности сервера Realmd.
    ''' </summary>
    Friend Sub CheckRealmd()
        Try
            ' Сначала проверяем наличие процесса
            Dim host = _IniRealmd.ReadString("RealmdConf", "BindIP", "127.0.0.1")
            If host = "0.0.0.0" Then host = "127.0.0.1"
            Dim port = _IniRealmd.ReadString("RealmdConf", "RealmServerPort", "3724")
            Dim tcpClient = New Net.Sockets.TcpClient
            Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)

            Try

                If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                    tcpClient.Close()
                    _RealmdON = False
                Else
                    tcpClient.EndConnect(ac)
                    tcpClient.Close()
                    _RealmdON = True
                End If

            Catch ex As Exception
                _RealmdON = False
                GV.Log.WriteException(ex)

            Finally
                ChangeIcons(ECaller.realmd)
                ac.AsyncWaitHandle.Close()
            End Try

        Catch ex As Exception
            GV.Log.WriteException(ex)
            MessageBox.Show(ex.Message,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If RealmdON And WorldON And My.Settings.WowClientPath <> "" And My.Settings.ClientAutoStart And Not CheckProcess(EProcess.wow) Then
                Threading.Thread.Sleep(200)
                StartWowClient()
            End If
        End Try
    End Sub

#End Region

#Region " === ЯРЛЫК И МЕНЮ В ТРЕЕ === "

    ''' <summary>
    ''' МЕНЮ - ОТКРЫТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_OpenLauncher_Click(sender As Object, e As EventArgs) Handles TSMI_OpenLauncher.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
    End Sub

    ''' <summary>
    ''' МЕНЮ - ЗАКРЫТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_CloseLauncher_Click(sender As Object, e As EventArgs) Handles TSMI_CloseLauncher.Click
        ' Выводим предупреждение
        Dim str = If(_MySqlLOCKED Or _ApacheLOCKED Or _ServersLOCKED, My.Resources.P050_Exit2, My.Resources.P050_Exit1)
        Dim result = MessageBox.Show(str, My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            ShutdownAll(True)
        End If
    End Sub

    ''' <summary>
    ''' ДВОЙНОЙ КЛИК МЫШКОЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon_SPP2.MouseDoubleClick
        Me.Show()
        Me.TopMost = True
        Me.WindowState = FormWindowState.Normal
        Me.TopMost = False
        Me.StartPosition = FormStartPosition.Manual
        Me.Size = My.Settings.AppSize
    End Sub

#End Region

#Region " === МЕНЮ СЕРВЕР === "

    ''' <summary>
    ''' МЕНЮ - ЗАПУСК СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerStart_Click(sender As Object, e As EventArgs) Handles TSMI_ServerStart.Click
        StartWorldRealmd()
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА СЕРВЕРА WoW
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerStop_Click(sender As Object, e As EventArgs) Handles TSMI_ServerStop.Click
        _firstRealmdStart = True
        _isLastCommandStart = False
        _NeedServerStop = True
        _NeedServerStart = False
        _NeedExitLauncher = False
        _ServersLOCKED = False
        ShutdownWorld(False)
    End Sub

    ''' <summary>
    ''' Изменяет автозапуск сервера WoW.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_WowAutoStart_Click(sender As Object, e As EventArgs) Handles TSMI_WowAutoStart.Click

        TSMI_WowAutoStart.Checked = Not TSMI_WowAutoStart.Checked
        _ServerWowAutostart = TSMI_WowAutoStart.Checked

        Select Case My.Settings.LastLoadedServerType
            Case GV.EModule.Classic.ToString
                My.Settings.ServerClassicAutostart = ServerWowAutostart
            Case GV.EModule.Tbc.ToString
                My.Settings.ServerTbcAutostart = ServerWowAutostart
            Case GV.EModule.Wotlk.ToString
                My.Settings.ServerWotlkAutostart = ServerWowAutostart
            Case Else
                ' Неизвестный модуль
                GV.Log.WriteInfo(My.Resources.E008_UnknownModule)
        End Select

        My.Settings.Save()

        If Not MySqlLOCKED And Not ApacheLOCKED And Not ServersLOCKED And ServerWowAutostart Then
            StartWorldRealmd()
        Else
            OutWorldConsole(My.Resources.P030_ControlDisabled, CONSOLE)
            OutRealmdConsole(My.Resources.P030_ControlDisabled, CONSOLE)
        End If
    End Sub

    ''' <summary>
    ''' Запускает серверы по необходимости.
    ''' </summary>
    Private Sub StartWorldRealmd()
        If Not CheckProcess(EProcess.realmd) And Not CheckProcess(EProcess.world) Then

            ' Не запущен ни один из серверов
            If My.Settings.UseIntMySQL And Not CheckProcess(EProcess.mysqld) Then TimerStartMySQL.Change(1000, 1000)

            ' Изменяем пункты меню - ЗАПРЕЩАЕМ
            ChangeServersMenu(False, False, False)

            ' То же с MySQL
            ChangeMySqlMenu(False, False, False)

            _isLastCommandStart = True
            _NeedServerStop = False
            _NeedServerStart = True
            RichTextBox_ConsoleRealmd.Clear()
            RichTextBox_ConsoleWorld.Clear()

            ' Ожидание сервера World
            OutRealmdConsole(My.Resources.P067_WorldWait, CONSOLE)

        Else

            If CheckProcess(EProcess.realmd) And CheckProcess(EProcess.world) Then

                If ServerWowAutostart Then
                    OutWorldConsole(My.Resources.P019_ControlEnabled, CONSOLE)
                    OutRealmdConsole(My.Resources.P019_ControlEnabled, CONSOLE)
                Else
                    OutWorldConsole(My.Resources.P030_ControlDisabled, CONSOLE)
                    OutRealmdConsole(My.Resources.P030_ControlDisabled, CONSOLE)
                End If

            Else

                ' Надо запустить какой то конкретный сервер
                If Not CheckProcess(EProcess.realmd) Then

                    ' Изменяем пункты меню - ЗАПРЕЩАЕМ
                    ChangeServersMenu(False, False, False)

                    ' То же с MySQL
                    ChangeMySqlMenu(False, False, False)

                    _isLastCommandStart = True
                    RichTextBox_ConsoleRealmd.Clear()

                    StartRealmd(Nothing)

                End If

                If Not CheckProcess(EProcess.world) Then

                    ' Делаем бэкап, если надо
                    If My.Settings.UseAutoBackupDatabase Then AutoBackups()

                    ' Изменяем пункты меню - ЗАПРЕЩАЕМ
                    ChangeServersMenu(False, False, False)

                    ' То же с MySQL
                    ChangeMySqlMenu(False, False, False)

                    _isLastCommandStart = True
                    RichTextBox_ConsoleWorld.Clear()

                    ' Запускаем поток
                    Dim sq As New Threading.Thread(AddressOf StartWorld) With {
                        .CurrentCulture = GV.CI,
                        .CurrentUICulture = GV.CI,
                        .IsBackground = True
                    }
                    sq.Start()

                End If

            End If

        End If
    End Sub

    ''' <summary>
    ''' Глобальная остановка всего и вся.
    ''' </summary>
    ''' <param name="shutdown">Завершить работу лаунчера после остановки серверов.</param>
    Friend Sub ShutdownAll(shutdown As Boolean)
        TabControl1.SelectedIndex = 0
        GV.Log.WriteInfo(UpdateMySQLConsole(My.Resources.P040_CommandShutdown, CONSOLE))
        Thread.Sleep(500)
        _NeedExitLauncher = shutdown
        ' Запускаем остановку всех серверов 
        ShutdownWorld(True)
    End Sub

#End Region

#Region " === КОНСОЛЬ === "

    ''' <summary>
    ''' ПРИ НАЖАТИИ КЛАВИШ В КОМАНДНОЙ СТРОКЕ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TextBox_Command_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Command.KeyDown
        If Not Me.TextBox_Command.IsHandleCreated Then Return

        Select Case e.KeyData

            Case Keys.Enter
                e.Handled = True
                e.SuppressKeyPress = True
                If TabControl1.SelectedTab.Name = "TabPage_MySQL" Then
                    If Not IsNothing(_mysqlProcess) Then _mysqlProcess.StandardInput.WriteLine(Me.TextBox_Command.Text)
                ElseIf TabControl1.SelectedTab.Name = "TabPage_Realmd" Then
                    If Not IsNothing(_RealmdProcess) Then _RealmdProcess.StandardInput.WriteLine(Me.TextBox_Command.Text)
                ElseIf TabControl1.SelectedTab.Name = "TabPage_World" Then
                    If Me.TextBox_Command.Text <> "" Then
                        SendCommandToWorld(Me.TextBox_Command.Text)
                    End If
                End If
                Me.TextBox_Command.Text = ""

            Case Keys.Up
                If My.Settings.UseConsoleBuffer Then
                    Dim text = ConsoleCommandBuffer.GetUp
                    If Not IsNothing(text) OrElse text <> "" Then
                        TextBox_Command.Text = text
                        TextBox_Command.Select(text.Length, text.Length)
                    End If
                End If

            Case Keys.Down
                If My.Settings.UseConsoleBuffer Then
                    Dim text = ConsoleCommandBuffer.GetDown
                    If Not IsNothing(text) OrElse text <> "" Then
                        TextBox_Command.Text = text
                        TextBox_Command.Select(text.Length, text.Length)
                    End If
                End If

            Case Keys.Escape
                Me.TextBox_Command.Text = ""

        End Select
    End Sub

    ''' <summary>
    ''' Отправляет команду в сервер World.
    ''' </summary>
    ''' <param name="text"></param>
    Friend Sub SendCommandToWorld(text As String)

        ' Если буфер разрешён, добавляем текст в буфер
        If My.Settings.UseConsoleBuffer Then ConsoleCommandBuffer.Add(text)

        Dim t = String.Format(My.Resources.P012_ConsoleCommand, text)
        GV.Log.WriteInfo(t)
        UpdateWorldConsole(t, CONSOLE)
        If Not IsNothing(_WorldProcess) Then
            _WorldProcess.StandardInput.WriteLine(text)
            If Not _WorldON Then
                UpdateWorldConsole(My.Resources.P037_WorldNotStarted, CONSOLE)
            End If
        Else
            UpdateWorldConsole(My.Resources.P037_WorldNotStarted, CONSOLE)
        End If
    End Sub

    ''' <summary>
    ''' Вывод сообщения в консоль сервера World с учётом Invoke.
    ''' </summary>
    ''' <param name="text">Текст для вывода.</param>
    Friend Function UpdateWorldConsole(text As String, color As Drawing.Color) As String
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            If Me.RichTextBox_ConsoleWorld.InvokeRequired Then
                Me.RichTextBox_ConsoleWorld.Invoke(Sub()
                                                       OutWorldConsole(text, color)
                                                   End Sub)
            Else
                OutWorldConsole(text, color)
            End If
        End If
        Return text
    End Function

    ''' <summary>
    ''' Прямой вывод в консоль World.
    ''' </summary>
    ''' <param name="text"></param>
    Private Sub OutWorldConsole(text As String, color As Drawing.Color)
        Select Case RichTextBox_ConsoleWorld.Lines.Count
            Case 0
                RichTextBox_ConsoleWorld.SelectionColor = color
                RichTextBox_ConsoleWorld.AppendText(text)
                RichTextBox_ConsoleWorld.ScrollToCaret()
            Case 500
                ' Не более 500 строк в окне
                Dim str = RichTextBox_ConsoleWorld.Lines(0)
                RichTextBox_ConsoleWorld.Select(0, str.Length + 1)
                RichTextBox_ConsoleWorld.ReadOnly = False
                RichTextBox_ConsoleWorld.SelectedText = String.Empty
                RichTextBox_ConsoleWorld.ReadOnly = True
                RichTextBox_ConsoleWorld.AppendText(vbCrLf)
                RichTextBox_ConsoleWorld.SelectionColor = color
                RichTextBox_ConsoleWorld.AppendText(text)
                RichTextBox_ConsoleWorld.ScrollToCaret()
            Case Else
                RichTextBox_ConsoleWorld.AppendText(vbCrLf)
                RichTextBox_ConsoleWorld.SelectionColor = color
                RichTextBox_ConsoleWorld.AppendText(text)
                RichTextBox_ConsoleWorld.ScrollToCaret()
        End Select
        RichTextBox_ConsoleWorld.Select(RichTextBox_ConsoleWorld.GetFirstCharIndexOfCurrentLine(), 0)
    End Sub

    ''' <summary>
    ''' Вывод сообщения в консоль сервера MySQL с учётом Invoke.
    ''' </summary>
    ''' <param name="text"></param>
    Friend Function UpdateMySQLConsole(ByVal text As String, ByVal ink As Drawing.Color) As String
        Dim msg As String = ""
        If Not IsNothing(text) And Me.Visible = True Then
            If ink.A = 0 And ink.R = 0 And ink.G = 0 And ink.B = 0 Then
                If text.Contains("Got an error reading communication packets") Then Return text
                If text.Contains("[Warning]") Then
                    msg = Mid(text, 31)
                    ink = Color.Red
                ElseIf text.Contains("[ERROR]") Then
                    msg = Mid(text, 31)
                    ink = Color.Red
                ElseIf text.Contains("[Note]") Then
                    msg = Mid(text, 31)
                    ink = Color.DarkOrange
                Else
                    msg = text
                    ink = Color.YellowGreen
                End If
            Else
                msg = text
            End If
            If Me.RichTextBox_ConsoleMySQL.InvokeRequired Then
                Me.RichTextBox_ConsoleMySQL.Invoke(Sub()
                                                       OutMySqlConsole(msg, ink)
                                                   End Sub)
            Else
                OutMySqlConsole(msg, ink)
            End If
        End If
        Return text
    End Function

    ''' <summary>
    ''' Вывод в консоль MySQL только из главного потока!
    ''' </summary>
    ''' <param name="text">Текст для вывода.</param>
    ''' <param name="ink">Цвет текста.</param>
    Friend Function OutMySqlConsole(text As String, ink As Drawing.Color) As String
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            Select Case RichTextBox_ConsoleMySQL.Lines.Count
                Case 0
                    RichTextBox_ConsoleMySQL.ReadOnly = False
                    RichTextBox_ConsoleMySQL.SelectedText = String.Empty
                    RichTextBox_ConsoleMySQL.SelectionColor = ink
                    RichTextBox_ConsoleMySQL.ReadOnly = True

                    RichTextBox_ConsoleMySQL.AppendText(text)
                    RichTextBox_ConsoleMySQL.ScrollToCaret()
                Case 500
                    ' Не более 500 строк в окне
                    Dim str = RichTextBox_ConsoleMySQL.Lines(0)
                    RichTextBox_ConsoleMySQL.Select(0, str.Length + 1)
                    RichTextBox_ConsoleMySQL.ReadOnly = False
                    RichTextBox_ConsoleMySQL.SelectedText = String.Empty
                    RichTextBox_ConsoleMySQL.ReadOnly = True
                    RichTextBox_ConsoleMySQL.AppendText(vbCrLf)
                    RichTextBox_ConsoleMySQL.SelectionColor = ink
                    RichTextBox_ConsoleMySQL.AppendText(text)
                    RichTextBox_ConsoleMySQL.ScrollToCaret()
                Case Else
                    RichTextBox_ConsoleMySQL.AppendText(vbCrLf)
                    RichTextBox_ConsoleMySQL.SelectionColor = ink
                    RichTextBox_ConsoleMySQL.AppendText(text)
                    RichTextBox_ConsoleMySQL.ScrollToCaret()
            End Select

        End If
        Return text
    End Function

    ''' <summary>
    ''' Вывод сообщения в консоль сервера Realmd.
    ''' </summary>
    ''' <param name="text">Текст для вывода.</param>
    Friend Sub UpdateRealmdConsole(text As String, color As Drawing.Color)
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            If Me.RichTextBox_ConsoleRealmd.InvokeRequired Then
                Me.RichTextBox_ConsoleRealmd.Invoke(Sub()
                                                        OutRealmdConsole(text, color)
                                                    End Sub)
            Else
                OutRealmdConsole(text, color)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Прямой вывод в консоль Realmd
    ''' </summary>
    ''' <param name="text"></param>
    Private Sub OutRealmdConsole(text As String, color As Drawing.Color)
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            Select Case RichTextBox_ConsoleRealmd.Lines.Count
                Case 0
                    RichTextBox_ConsoleRealmd.SelectionColor = color
                    RichTextBox_ConsoleRealmd.AppendText(text)
                    RichTextBox_ConsoleRealmd.ScrollToCaret()
                Case 500
                    ' Не более 500 строк в окне
                    Dim str = RichTextBox_ConsoleRealmd.Lines(0)
                    RichTextBox_ConsoleRealmd.Select(0, str.Length + 1)
                    RichTextBox_ConsoleRealmd.ReadOnly = False
                    RichTextBox_ConsoleRealmd.SelectedText = String.Empty
                    RichTextBox_ConsoleRealmd.ReadOnly = True
                    RichTextBox_ConsoleRealmd.AppendText(vbCrLf)
                    RichTextBox_ConsoleRealmd.SelectionColor = color
                    RichTextBox_ConsoleRealmd.AppendText(text)
                    RichTextBox_ConsoleRealmd.ScrollToCaret()
                Case Else
                    RichTextBox_ConsoleRealmd.AppendText(vbCrLf)
                    RichTextBox_ConsoleRealmd.SelectionColor = color
                    RichTextBox_ConsoleRealmd.AppendText(text)
                    RichTextBox_ConsoleRealmd.ScrollToCaret()
            End Select
            RichTextBox_ConsoleRealmd.Select(RichTextBox_ConsoleRealmd.GetFirstCharIndexOfCurrentLine(), 0)
            If text.Contains(My.Resources.E015_RealmdCrashed) Then
                ' Сервер Realmd рухнул. Удаляем хандлеры
                If Not IsNothing(_RealmdProcess) Then RealmdExited(Me, Nothing)
                RealmdProcess = Nothing
                _RealmdON = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' КОНТЕКСТНОЕ МЕНЮ - КОПИРОВАТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Copy_Click(sender As Object, e As EventArgs) Handles TSMI_Copy.Click
        Select Case TabControl1.SelectedTab.Name
            Case "TabPage_MySQL"
                If RichTextBox_ConsoleMySQL.SelectionLength > 0 Then
                    My.Computer.Clipboard.SetText(RichTextBox_ConsoleMySQL.SelectedText)
                Else
                    My.Computer.Clipboard.Clear()
                End If
            Case "TabPage_Realmd"
                If RichTextBox_ConsoleRealmd.SelectionLength > 0 Then
                    My.Computer.Clipboard.SetText(RichTextBox_ConsoleRealmd.SelectedText)
                Else
                    My.Computer.Clipboard.Clear()
                End If
            Case "TabPage_World"
                If RichTextBox_ConsoleWorld.SelectionLength > 0 Then
                    My.Computer.Clipboard.SetText(RichTextBox_ConsoleWorld.SelectedText)
                Else
                    My.Computer.Clipboard.Clear()
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Меняет задний фон консоли.
    ''' </summary>
    Friend Sub SetConsoleTheme()
        If My.Settings.ConsoleTheme = "Black Theme" Then
            RichTextBox_ConsoleMySQL.BackColor = Color.Black
            RichTextBox_ConsoleRealmd.BackColor = Color.Black
            RichTextBox_ConsoleWorld.BackColor = Color.Black
        Else
            RichTextBox_ConsoleMySQL.BackColor = Color.White
            RichTextBox_ConsoleRealmd.BackColor = Color.White
            RichTextBox_ConsoleWorld.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' Изменяет шрифт консоли.
    ''' </summary>
    Friend Sub ChangeFont()
        My.Settings.Save()

        ' Получает шрифт с текущими настройками.
        Dim fnt = GetCurrentFont()

        ' MySQL
        RichTextBox_ConsoleMySQL.SuspendLayout()
        For x = 0 To RichTextBox_ConsoleMySQL.Lines().Count
            Dim istart = RichTextBox_ConsoleMySQL.GetFirstCharIndexFromLine(x)
            Dim iend = RichTextBox_ConsoleMySQL.GetFirstCharIndexFromLine(x + 1) - 1
            If istart >= 0 Then
                RichTextBox_ConsoleMySQL.Select(istart, iend - istart)
                RichTextBox_ConsoleMySQL.SelectionFont = fnt
            End If
        Next
        RichTextBox_ConsoleMySQL.ResumeLayout()

        ' Realmd
        RichTextBox_ConsoleRealmd.SuspendLayout()
        For x = 0 To RichTextBox_ConsoleRealmd.Lines().Count
            Dim istart = RichTextBox_ConsoleRealmd.GetFirstCharIndexFromLine(x)
            Dim iend = RichTextBox_ConsoleRealmd.GetFirstCharIndexFromLine(x + 1) - 1
            If istart >= 0 Then
                RichTextBox_ConsoleRealmd.Select(istart, iend - istart)
                RichTextBox_ConsoleRealmd.SelectionFont = fnt
            End If
        Next
        RichTextBox_ConsoleRealmd.ResumeLayout()

        ' World
        RichTextBox_ConsoleWorld.SuspendLayout()
        Dim x3 = 0
        For Each line In RichTextBox_ConsoleWorld.Lines()
            Dim istart = RichTextBox_ConsoleWorld.GetFirstCharIndexFromLine(x3)
            Dim iend = istart + line.Length
            'Dim iend = RichTextBox_ConsoleWorld.GetFirstCharIndexFromLine(x + 1) - 1
            If istart >= 0 Then
                RichTextBox_ConsoleWorld.Select(istart, iend - istart)
                RichTextBox_ConsoleWorld.SelectionFont = fnt
            End If
            x3 += 1
        Next
        RichTextBox_ConsoleWorld.ResumeLayout()

        RichTextBox_ConsoleMySQL.Font = fnt
        RichTextBox_ConsoleRealmd.Font = fnt
        RichTextBox_ConsoleWorld.Font = fnt

    End Sub

#End Region

#Region " === ПРОЧИЕ ОПЕРАЦИИ === "

    ''' <summary>
    ''' Изменяет иконки приложения и трея в зависимости от состояния серверов.
    ''' </summary>
    ''' <param name="caller"></param>
    Friend Sub ChangeIcons(caller As ECaller)

        Try

            If Not IsNothing(Me) Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  ChangeIcons(caller)
                              End Sub)
                Else

                    ' Проверка для пунктов меню
                    If Not _isBackupStarted Then
                        If CheckProcess(EProcess.world) Or CheckProcess(EProcess.realmd) Then
                            TSMI_Reset.Enabled = False
                            TSMI_ServerSwitcher.Enabled = False
                        Else
                            TSMI_Reset.Enabled = True
                            TSMI_ServerSwitcher.Enabled = True
                        End If
                    End If

                    Select Case caller

                        Case ECaller.mysql

                            If Not _WorldON Then

                                If _MysqlON Then

                                    ' Запрещаем доступ к повторному запуску MySQL
                                    TSMI_MySqlStart.Enabled = False

                                    ' MySQL запущен
                                    If Me.Visible Then
                                        ' Основное окно открыто
                                        TSSL_MySQL.Image = My.Resources.green_ball
                                        If _isLastCommandStart Or ServerWowAutostart Then
                                            Me.Icon = My.Resources.cmangos_red
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                        Else
                                            Me.Icon = My.Resources.wow
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                        End If
                                    Else
                                        ' Меняем иконку в трее, коли свёрнуты
                                        If _isLastCommandStart Or ServerWowAutostart Then
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                        Else
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                        End If
                                    End If

                                Else

                                    ' MySQL не запущен
                                    If Me.Visible Then
                                        ' Основное окно октрыто
                                        TSSL_MySQL.Image = My.Resources.red_ball
                                        If _isLastCommandStart Or ServerWowAutostart Then
                                            Me.Icon = My.Resources.cmangos_red
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                        Else
                                            Me.Icon = My.Resources.wow
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                        End If
                                    Else
                                        ' Меняем иконку в трее, коли свёрнуты
                                        If _isLastCommandStart Or ServerWowAutostart Then
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                        Else
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                        End If
                                    End If

                                End If

                            End If

                        Case ECaller.realmd

                            If _RealmdON Then

                                ' Realmd запущен
                                TSMI_Reset.Enabled = False
                                TSMI_ServerSwitcher.Enabled = False

                                If Me.Visible Then
                                    ' Основное окно открыто
                                    TSSL_Realm.Image = My.Resources.green_ball
                                    If _isLastCommandStart Or ServerWowAutostart Then
                                        If Not _WorldON Then
                                            Me.Icon = My.Resources.cmangos_realmd_started
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                                        End If
                                    End If
                                Else
                                    ' Меняем иконку в трее, коли свёрнуты
                                    If _isLastCommandStart Or ServerWowAutostart Then
                                        If Not _WorldON Then
                                            Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                                        End If
                                    End If
                                End If

                            Else

                                ' Realmd не запущен
                                If WorldON And Not CheckProcess(EProcess.realmd) Then
                                    TSMI_ServerStart.Enabled = True
                                End If

                                ChangeRealmdCheck = 1.1
                                If Me.Visible Then
                                    ' Основное окно открыто
                                    TSSL_Realm.Image = My.Resources.red_ball
                                    If _isLastCommandStart Or ServerWowAutostart Then
                                        Me.Icon = My.Resources.cmangos_realmd_started
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                                    Else
                                        Me.Icon = My.Resources.wow
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                    End If
                                Else
                                    ' Меняем иконку в трее, коли свёрнуты
                                    If _isLastCommandStart Or ServerWowAutostart Then
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                                    Else
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                    End If
                                End If

                            End If


                        Case ECaller.world

                            If WorldON Then

                                ' World запущен
                                If Me.Visible Then

                                    TSMI_Reset.Enabled = False
                                    TSMI_ServerSwitcher.Enabled = False
                                    ServerIsStarting = False

                                    ' Основное окно открыто
                                    TSSL_World.Image = My.Resources.green_ball
                                    If _RealmdON Then
                                        Select Case My.Settings.LastLoadedServerType
                                            Case GV.EModule.Classic.ToString
                                                Me.Icon = My.Resources.cmangos_classic
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_classic
                                            Case GV.EModule.Tbc.ToString
                                                Me.Icon = My.Resources.cmangos_tbc
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_tbc
                                            Case GV.EModule.Wotlk.ToString
                                                Me.Icon = My.Resources.cmangos_wotlk
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_wotlk
                                        End Select
                                    Else
                                        Me.Icon = My.Resources.cmangos_orange
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                    End If
                                Else
                                    ' Меняем иконку в трее, коли свёрнуты
                                    If _RealmdON Then
                                        Select Case My.Settings.LastLoadedServerType
                                            Case GV.EModule.Classic.ToString
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_classic
                                            Case GV.EModule.Tbc.ToString
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_tbc
                                            Case GV.EModule.Wotlk.ToString
                                                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_wotlk
                                        End Select
                                    Else
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                    End If
                                End If

                            Else

                                ' World не запущен
                                If Not _isBackupStarted Then
                                    If Not CheckProcess(EProcess.world) Then
                                        TSMI_ServerStart.Enabled = True
                                    Else
                                        TSMI_ServerStart.Enabled = False
                                    End If
                                End If

                                TSSL_World.Image = My.Resources.red_ball

                                If _isLastCommandStart Or ServerWowAutostart Then
                                    If Me.Visible Then
                                        ' Основное окно открыто
                                        Me.Icon = My.Resources.cmangos_orange
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                    Else
                                        ' Меняем иконку в трее, коли свёрнуты
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                    End If

                                Else
                                    If Me.Visible Then
                                        ' Основное окно открыто
                                        Me.Icon = My.Resources.wow
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                    Else
                                        ' Меняем иконку в трее, коли свёрнуты
                                        Me.NotifyIcon_SPP2.Icon = My.Resources.wow
                                    End If
                                End If

                            End If

                            If WorldON And RealmdON Then TSMI_ServerStart.Enabled = False

                    End Select
                End If
            End If

        Catch
        End Try

    End Sub

    ''' <summary>
    ''' Обновление информации в StatusStrip и управление меню СТАРТ/СТОП.
    ''' </summary>
    Friend Sub OutInfoPlayers()
        If Not IsNothing(Me) Then

            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              OutInfoPlayers()
                          End Sub)

            Else

                Dim online, total As String
                online = MySqlDB.CHARACTERS.ACCOUNT.SELECT_ONLINE_CHARS()
                total = MySqlDB.CHARACTERS.ACCOUNT.SELECT_TOTAL_CHARS()
                TSSL_Count.Text = online
                TSSL_Count.ToolTipText = String.Format(My.Resources.P011_AllChars, total)

            End If
        End If
    End Sub

    Private Sub ShutdownMainMenu_Click(sender As Object, e As EventArgs) Handles TSMI_Shutdown.Click
        ' Выводим предупреждение
        Dim str = My.Resources.P050_Exit1
        Dim result = MessageBox.Show(str, My.Resources.P016_WarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            ShutdownAll(True)
        End If
    End Sub

#End Region

End Class
