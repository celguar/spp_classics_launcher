
Imports System.Text
Imports System.Threading

Public Class Launcher

#Region " === ДРУЖЕСТВЕННЫЕ СВОЙСТВА === "

    ''' <summary>
    ''' Текущий запущенный сервер.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property CurrentRunningServer As String = ""

    ''' <summary>
    ''' Флаг - идёт общая остановка всех серверов.
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property IsShutdown As Boolean

    ''' <summary>
    ''' Флаг разрешения закрытия лаунчера.
    ''' </summary>
    Friend Property EnableClosing As Boolean

#End Region

#Region " === ПРИВАТНЫЕ ПОЛЯ === "

    ''' <summary>
    ''' Процесс MySQL
    ''' </summary>
    Private _mysqlProcess As Process

    ''' <summary>
    ''' Процесс Realmd
    ''' </summary>
    Private _realmdProcess As Process

    ''' <summary>
    ''' Процесс World
    ''' </summary>
    Private _worldProcess As Process

    ''' <summary>
    ''' Флаг работы MySQL
    ''' </summary>
    Private _mysqlON As Boolean

    ''' <summary>
    ''' Файл конфигурации сервера Realmd.
    ''' </summary>
    Private _iniRealmd As IniFiles

    ''' <summary>
    ''' Флаг работы Realmd
    ''' </summary>
    Private _realmdON As Boolean

    ''' <summary>
    ''' Файл конфигурации сервера World.
    ''' </summary>
    Private _iniWorld As IniFiles

    ''' <summary>
    ''' Флаг работы World
    ''' </summary>
    Private _worldON As Boolean

    ''' <summary>
    ''' Флаг запрета запуска Apache
    ''' </summary>
    Private _apacheSTOP As Boolean

    ''' <summary>
    ''' Флаг ручного запуска сервера
    ''' </summary>
    Private _needServerStart As Boolean

    ''' <summary>
    ''' Флаг остановки сервера
    ''' </summary>
    Private _needServerStop As Boolean

#End Region

#Region " === КОНСТРУКТОР ИНИЦИАЛИЗАЦИИ === "

    Sub New()
        InitializeComponent()
        ' Восстановим размеры и положение окна
        Me.StartPosition = FormStartPosition.Manual
        Me.Size = My.Settings.AppSize
        ' Если всего один модуль, то прячем смену типа сервера
        If GV.Modules.Count = 1 Then TSMI_ServerSwitcher.Visible = False
        ' Настраиваем таймеры
        TimerCheckMySQL = New Threading.Timer(AddressOf TimerTik_CheckMySQL)
        TimerCheckMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckApache = New Threading.Timer(AddressOf TimerTik_CheckApache)
        TimerCheckApache.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckWorld = New Threading.Timer(AddressOf TimerTik_CheckWorld)
        TimerCheckWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckRealmd = New Threading.Timer(AddressOf TimerTik_CheckRealmd)
        TimerCheckRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartMySQL = New Threading.Timer(AddressOf TimerTik_StartMySQL)
        TimerStartMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartApache = New Threading.Timer(AddressOf TimerTik_StartApache)
        TimerStartApache.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartWorld = New Threading.Timer(AddressOf TimerTik_StartWorld)
        TimerStartWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartRealmd = New Threading.Timer(AddressOf TimerTik_StartRealmd)
        TimerStartRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        ' Загружаем шрифт
        LoadFont()
    End Sub

#End Region

#Region " === СТАНДАРТЫЕ ОПЕРАЦИИ === "

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Гасим сервера, которых не должно быть априоре...
        ShutdownAll(False)
        UpdateSettings()
        If My.Settings.AppLocation().X < 0 Or My.Settings.AppLocation().Y < 0 Then
            ' Исправляем ошибку, если сервер был прихлопнут в свёрнутом состоянии
            My.Settings.AppLocation() = New Point(0, 0)
        End If
        Me.Location = My.Settings.AppLocation
        ' Включаем таймеры проверки серверов
        TimerCheckMySQL.Change(2000, 2000)
        TimerCheckApache.Change(2000, 2000)
        TimerCheckWorld.Change(2000, 2000)
        TimerCheckRealmd.Change(2000, 2000)
        ' Если автозапуск MySQL сервера
        If My.Settings.UseIntMySQL And My.Settings.MySqlAutostart Then
            TimerStartMySQL.Change(500, 500)
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ЗАКРЫТИИ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If GV.FirstStart Then
            ' Это был первый запуск
            My.Settings.AppSize = GV.SPP2Launcher.Size
            My.Settings.AppLocation = Me.Location
            My.Settings.Save()
        Else
            If CurrentRunningServer <> "" Or EnableClosing = False Then
                Me.WindowState = FormWindowState.Minimized
                e.Cancel = True
            Else
                My.Settings.AppSize = Me.Size
                My.Settings.AppLocation = Me.Location
                My.Settings.Save()
                GV.Log.WriteInfo(My.Resources.P005_Exiting)
            End If
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
                    openFileDialog.InitialDirectory = "c:\"
                    openFileDialog.Filter = My.Resources.P003_SetWowClientPath
                    openFileDialog.FilterIndex = 1
                    openFileDialog.RestoreDirectory = True

                    If openFileDialog.ShowDialog() = DialogResult.OK Then
                        My.Settings.WowClientPath = openFileDialog.FileName
                        My.Settings.Save()
                    End If
                End Using
            End If
            If My.Settings.WowClientPath <> "" Then
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
        My.Settings.Locale = "en-GB"
        TryRestart()
    End Sub

    ''' <summary>
    ''' РУССКИЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Russian_Click(sender As Object, e As EventArgs) Handles TSMI_Russian.Click
        My.Settings.Locale = "ru-RU"
        TryRestart()
    End Sub

    ''' <summary>
    ''' Попытка перезагрузки лайнчера.
    ''' </summary>
    Private Sub TryRestart()
        My.Settings.Save()
        If CurrentRunningServer <> "" Or Not IsNothing(_mysqlProcess) Then
            ' Перезагрузка отменяется
            MessageBox.Show(My.Resources.P006_NeedReboot,
                            My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            _EnableClosing = True
            Application.Restart()
        End If
    End Sub

#End Region

#Region " === ЛАУНЧЕР === "

    ''' <summary>
    ''' МЕНЮ - НАСТРОЙКИ ПРИЛЖЕНИЯ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_Launcher_Click_1(sender As Object, e As EventArgs) Handles TSMI_Launcher.Click
        Dim fLauncherSettings As New LauncherSettings
        fLauncherSettings.ShowDialog()
    End Sub

    ''' <summary>
    ''' Обновление параметров для вывода на экран.
    ''' </summary>
    Friend Sub UpdateSettings()
        ' Если это первый запуск - предупреждаем
        If GV.FirstStart Then
            MessageBox.Show(My.Resources.P012_FirstStart,
                            My.Resources.P023_InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' Выключаем все меню запуска/перезапуска серверов
            TSMI_MySqlStart.Visible = False
            TSMI_MySqlRestart.Visible = False
            TSMI_MySqlStop.Visible = False
            TSMI_MySQL1.Visible = False
            TSMI_ApacheStart.Visible = False
            TSMI_ApacheRestart.Visible = False
            TSMI_ApacheStop.Visible = False
            TSMI_Apache1.Visible = False
            TSMI_ServerStart.Visible = False
            TSMI_ServerStop.Visible = False
            TSMI_Server2.Visible = False
            TSMI_RunWow.Enabled = False
        Else
            ' Обустраиваем меню MySQL
            If My.Settings.UseIntMySQL Then
                TSMI_MySqlStart.Enabled = True
                TSMI_MySqlRestart.Enabled = True
                TSMI_MySqlStop.Enabled = True
            Else
                TSMI_MySqlStart.Enabled = False
                TSMI_MySqlRestart.Enabled = False
                TSMI_MySqlStop.Enabled = False
            End If
            ' Обустраиваем меню Apache
            If My.Settings.UseIntApache Then
                TSMI_ApacheStart.Enabled = True
                TSMI_ApacheRestart.Enabled = True
                TSMI_ApacheStop.Enabled = True
            Else
                TSMI_ApacheStart.Enabled = False
                TSMI_ApacheRestart.Enabled = False
                TSMI_ApacheStop.Enabled = False
            End If
        End If
        ' Заголовок приложения
        Dim srv As String = ""
        Dim autostart As Boolean
        Select Case My.Settings.LastLoadedServerType
            Case GV.EModule.Classic.ToString
                srv = "Classic"
                _iniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\realmd.conf")
                _iniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\mangosd.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\vanilla\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\vanilla\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\vanilla\"
                autostart = My.Settings.ServerClassicAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_classic_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))
            Case GV.EModule.Tbc.ToString
                srv = "The Burning Crusade"
                _iniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\realmd.conf")
                _iniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\mangosd.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\tbc\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\tbc\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\tbc\"
                autostart = My.Settings.ServerTbcAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_tbc_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))
            Case GV.EModule.Wotlk.ToString
                srv = "Wrath of the Lich King"
                _iniRealmd = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\realmd.conf")
                _iniWorld = New IniFiles(My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\mangosd.conf")
                My.Settings.CurrentFileRealmd = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\wotlk\Bin64\realmd.exe"
                My.Settings.CurrentFileWorld = My.Settings.DirSPP2 & "\" & SPP2CMANGOS & "\wotlk\Bin64\mangosd.exe"
                My.Settings.CurrentServerSettings = My.Settings.DirSPP2 & "\" & SPP2SETTINGS & "\wotlk\"
                autostart = My.Settings.ServerWotlkAutostart
                TSMI_OpenLauncher.Image = My.Resources.cmangos_wotlk_core
                GV.Log.WriteInfo(String.Format(My.Resources.P026_SettingsApplied, srv))
            Case Else
                Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                GV.Log.WriteError(str)
                MessageBox.Show(str,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Select
        My.Settings.Save()

        Text = My.Resources.P010_LauncherCaption & " : " & srv

        ' Настраиваем консоли
        Dim bc As Color = If(My.Settings.ConsoleTheme = "Black", Drawing.Color.Black, Drawing.Color.SeaShell)
        'Dim fnt As New Font("Segoe UI", My.Settings.CurrentConsoleFont, FontStyle.Bold)
        Dim fnt = New Font(F.Families(0), My.Settings.ConsoleFontSize, My.Settings.ConsoleFontStyle)

        ' Realmd
        Dim ink() = _iniRealmd.ReadString("RealmdConf", "LogColors").Split(" "c)
        'My.Settings.RealmdConsoleForeColor = Drawing.Color.FromArgb(CInt(ink(3)), CInt(ink(2)), CInt(ink(1)), CInt(ink(0)))
        RichTextBox_ConsoleRealmd.ForeColor = My.Settings.RealmdConsoleForeColor
        RichTextBox_ConsoleRealmd.BackColor = bc
        RichTextBox_ConsoleRealmd.Font = fnt

        ' World
        ink = _iniWorld.ReadString("MangosdConf", "LogColors").Split(" "c)
        'My.Settings.WorldConsoleForeColor = Drawing.Color.FromArgb(CInt(ink(0)), CInt(ink(1)), CInt(ink(2)), CInt(ink(3)))
        RichTextBox_ConsoleWorld.ForeColor = My.Settings.WorldConsoleForeColor
        RichTextBox_ConsoleWorld.BackColor = bc
        RichTextBox_ConsoleWorld.Font = fnt

        ' MySQL
        'fnt = LoadFont()
        RichTextBox_ConsoleMySQL.ForeColor = Color.Green
        RichTextBox_ConsoleMySQL.BackColor = bc
        RichTextBox_ConsoleMySQL.Font = fnt

        ' Разбираемся с пунктом меню смены типа сервера
        If autostart Then
            ' Прячем меню смены сервера
            TSMI_ServerSwitcher.Visible = False
            TSMI_SepSrv1.Visible = False
        Else
            ' Отображаем меню смены сервера
            TSMI_ServerSwitcher.Visible = True
            TSMI_SepSrv1.Visible = True
        End If
        ' Устанавливаем тему консоли.
        SetConsoleTheme()
    End Sub

#End Region

#Region " === РЕЗЕРВНОЕ СОХРАНЕНИЕ === "

    ''' <summary>
    ''' Выполняет автосохранение.
    ''' </summary>
    Private Sub AutoSave()

    End Sub

#End Region

#Region " === СМЕНИТЬ ТИП СЕРВЕРА === "

    ''' <summary>
    ''' СМЕНА ТИПА СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerSwitcher_Click(sender As Object, e As EventArgs) Handles TSMI_ServerSwitcher.Click
        If CurrentRunningServer <> "" Or Not IsNothing(_mysqlProcess) Then
            ' Запущен один из процессов игнорировать которые нельзя
            MessageBox.Show(My.Resources.P006_NeedReboot,
                            My.Resources.P023_InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' Останавливаем ВСЕ сервера
            ShutdownAll(True)
            Me.Hide()
            _EnableClosing = True
            NotifyIcon_SPP2.Visible = False
            My.Settings.NextLoadServerType = GV.EModule.Restart.ToString
            Dim fServerSelector As New ServerSelector()
            fServerSelector.ShowDialog()
        Else
            ' Процессов нет - запускаем окно выбора типа сервера
            ShutdownAll(True)
            NotifyIcon_SPP2.Visible = False
            My.Settings.NextLoadServerType = ""
            Dim fServerSelector As New ServerSelector()
            fServerSelector.ShowDialog()
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
        TimerStartMySQL.Change(500, 500)
    End Sub

    ''' <summary>
    ''' МЕНЮ - ПЕРЕЗАПУСК MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlRestart_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlRestart.Click
        ShutdownMySQL(GetAllProcesses)
        TimerStartMySQL.Change(1000, 1000)
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА MySQL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_MySqlStop_Click(sender As Object, e As EventArgs) Handles TSMI_MySqlStop.Click
        ShutdownMySQL(GetAllProcesses)
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
        If My.Settings.UseIntMySQL AndAlso Not _mysqlON AndAlso IsNothing(_mysqlProcess) Then
            GV.Log.WriteAll(My.Resources.SQL002_Start)
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
                    GV.Log.WriteAll(My.Resources.SQL003_Started)
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
                MessageBox.Show(My.Resources.E009_MySqlException & vbCrLf & ex.Message,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Выход из mysqld.exe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MySqlExited(ByVal sender As Object, ByVal e As EventArgs)
        GV.Log.WriteAll(My.Resources.SQL004_Stopped)
        RemoveHandler _mysqlProcess.OutputDataReceived, AddressOf MySqlOutputDataReceived
        RemoveHandler _mysqlProcess.ErrorDataReceived, AddressOf MySqlErrorDataReceived
        RemoveHandler _mysqlProcess.Exited, AddressOf MySqlExited
    End Sub

    ''' <summary>
    ''' Останавливает сервер MySQL
    ''' </summary>
    Friend Sub ShutdownMySQL(processes As List(Of Process))
        Dim pc = processes.FindAll(Function(p) p.ProcessName = "mysqld")
        If pc.Count = 0 Then Exit Sub
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
                    MessageBox.Show(String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType),
                                    My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                Using exeProcess = Process.Start(startInfo)
                    GV.Log.WriteAll(My.Resources.SQL001_Shutdown)
                    exeProcess.WaitForExit()
                End Using
                _mysqlON = False
                _mysqlProcess = Nothing
                TSSL_MySQL.Image = My.Resources.red_ball
            Catch ex As Exception
                GV.Log.WriteException(ex)
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Проверка соединения с сервером MySQL
    ''' </summary>
    Friend Sub CheckMySQL(obj As Object)
        Dim host, port As String
        Dim autostart As Boolean
        If My.Settings.UseIntMySQL Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                    autostart = My.Settings.ServerClassicAutostart
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                    autostart = My.Settings.ServerTbcAutostart
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                    autostart = My.Settings.ServerWotlkAutostart
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteAll(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                    autostart = My.Settings.ServerClassicAutostart
                Case GV.EModule.Tbc.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                    autostart = My.Settings.ServerTbcAutostart
                Case GV.EModule.Wotlk.ToString
                    host = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                    autostart = My.Settings.ServerWotlkAutostart
                Case Else
                    ' Неизвестный модуль
                    GV.Log.WriteAll(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        End If
        If host = "" Or port = "" Then Exit Sub
        Dim tcpClient = New Net.Sockets.TcpClient
        Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)
        Try
            If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                tcpClient.Close()
                If Me.Visible Then
                    TSSL_MySQL.GetCurrentParent().Invoke(Sub()
                                                             TSSL_MySQL.Image = My.Resources.red_ball
                                                             Me.Icon = My.Resources.cmangos_red
                                                             Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                                         End Sub)
                Else
                    ' Меняем иконку в трее, коли свёрнуты
                    Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                End If
                _mysqlON = False
            Else
                tcpClient.EndConnect(ac)
                tcpClient.Close()
                If Me.Visible Then
                    TSSL_MySQL.GetCurrentParent().Invoke(Sub()
                                                             TSSL_MySQL.Image = My.Resources.green_ball
                                                             If Not _realmdON Then
                                                                 Me.Icon = My.Resources.cmangos_orange
                                                                 Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                                             End If
                                                         End Sub)
                Else
                    ' Меняем иконку в трее, коли свёрнуты
                    Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                End If
                _mysqlON = True
            End If
        Catch ex As Exception
            If Me.Visible Then
                TSSL_MySQL.GetCurrentParent().Invoke(Sub()
                                                         TSSL_MySQL.Image = My.Resources.red_ball
                                                         Me.Icon = My.Resources.cmangos_red
                                                         Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                                     End Sub)
            Else
                ' Меняем иконку в трее, коли свёрнуты
                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
            End If
            _mysqlON = False
            GV.Log.WriteException(ex)
        Finally
            ' Если установлен автозапуск сервера Apache
            If _mysqlON Then
                If My.Settings.UseIntApache AndAlso My.Settings.ApacheAutostart AndAlso Not _apacheSTOP AndAlso GetApachePid() = 0 Then
                    ' Включаем Apache через 1,5 сек.
                    TimerStartApache.Change(1500, 1500)
                End If
            Else
                If My.Settings.UseIntApache Then
                    ' Выключаем Apache
                    ShutdownApache()
                End If
            End If
            ' Если установлен автозапуск сервера
            If _mysqlON AndAlso Not _needServerStop AndAlso (autostart OrElse _needServerStart) Then
                _needServerStart = False
                ' Запускаем World через 2 сек.
                TimerStartRealmd.Change(2000, 2000)
            End If
        End Try
        ac.AsyncWaitHandle.Close()
    End Sub

#End Region

#Region " === СЕРВЕР APACHE === "

    ''' <summary>
    ''' МЕНЮ - ЗАПУСК Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheStart_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheStart.Click
        If _mysqlON Then
            _apacheSTOP = False
            If My.Settings.ApacheAutostart Then
                ' Ничего не делаем, CheckMySQL сам поднимет Apache
            Else
                ' Через 0,5 сек запускаем Apache
                TimerStartApache.Change(500, 500)
            End If
        Else
            ' Без MySQL Apache не фиг делать - сайт не подымется
        End If
    End Sub

    ''' <summary>
    ''' МЕНЮ - ПЕРЕЗАПУСК Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheRestart_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheRestart.Click
        RestartApache()
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА Apache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ApacheStop_Click(sender As Object, e As EventArgs) Handles TSMI_ApacheStop.Click
        _apacheSTOP = True
        ShutdownApache()
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
    ''' <param name="obj"></param>
    Friend Sub StartApache(obj As Object)
        If My.Settings.UseIntApache Or GetApachePid() = 0 Then
            GV.Log.WriteAll(My.Resources.Apache002_Start)
            ' Разбираемся с настройками Apache
            If My.Settings.UseIntApache Then
                Dim listen As String = ""
                Select Case My.Settings.LastLoadedServerType
                    Case GV.EModule.Classic.ToString
                        listen = If(My.Settings.ApacheClassicIntHost = "ANY",
                                    "Listen " & My.Settings.ApacheClassicIntPort,
                                    "Listen " & My.Settings.ApacheClassicIntHost & ":" & My.Settings.ApacheClassicIntPort)
                    Case GV.EModule.Tbc.ToString
                        listen = If(My.Settings.ApacheTbcIntHost = "ANY",
                                    "Listen " & My.Settings.ApacheTbcIntPort,
                                    "Listen " & My.Settings.ApacheTbcIntHost & ":" & My.Settings.ApacheTbcIntPort)
                    Case GV.EModule.Wotlk.ToString
                        listen = If(My.Settings.ApacheWotlkIntHost = "ANY",
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
                    GV.Log.WriteAll(My.Resources.Apache003_Started)
                Else
                    GV.Log.WriteAll(My.Resources.Apache005_NotStarted)
                End If
            Catch ex As Exception
                ' Apache выдал исключение
                GV.Log.WriteException(ex)
                GV.Log.WriteAll(My.Resources.Apache005_NotStarted)
                MessageBox.Show(My.Resources.E010_ApacheException & vbLf & ex.Message,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Перезагружает локальный сервер Apache.
    ''' </summary>
    Friend Sub RestartApache()
        _apacheSTOP = False
        ShutdownApache()
        If _mysqlON Then
            If My.Settings.ApacheAutostart Then
                ' Ничего не делаем, MySQL сам поднимет Apache
            Else
                ' Через 0,5 сек запускаем Apache
                TimerStartApache.Change(500, 500)
            End If
        Else
            ' Без MySQL Apache не фиг делать - сайт не подымется
        End If
    End Sub

    ''' <summary>
    ''' Выключает сервер Apache.
    ''' </summary>
    Friend Sub ShutdownApache()
        Dim pid = GetApachePid()
        If pid > 0 Then
            Dim p As Process
            Try
                p = Process.GetProcessById(pid)
                p.Kill()
                GV.Log.WriteAll(My.Resources.Apache004_Stopped)
                IO.File.Delete(My.Settings.DirSPP2 & "\" & SPP2APACHE & "\logs\httpd.pid")
            Catch
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
    Public Sub CheckApache(obj As Object)
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
                    GV.Log.WriteAll(My.Resources.E008_UnknownModule)
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
                    GV.Log.WriteAll(My.Resources.E008_UnknownModule)
                    Exit Sub
            End Select
        End If
        If host = "" Or port = "" Then Exit Sub
        If host = "ANY" Then host = "127.0.0.1"
        Dim tcpClient = New Net.Sockets.TcpClient
        Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)
        Try
            If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                tcpClient.Close()
                If Me.Visible Then
                    TSSL_Apache.GetCurrentParent().Invoke(Sub()
                                                              TSSL_Apache.Image = My.Resources.red_ball
                                                          End Sub)
                End If
            Else
                tcpClient.EndConnect(ac)
                tcpClient.Close()
                If Me.Visible Then
                    TSSL_Apache.GetCurrentParent().Invoke(Sub()
                                                              TSSL_Apache.Image = My.Resources.green_ball
                                                          End Sub)
                End If
            End If
        Catch ex As Exception
            If Me.Visible Then
                TSSL_Apache.GetCurrentParent().Invoke(Sub()
                                                          TSSL_Apache.Image = My.Resources.red_ball
                                                      End Sub)
            End If
            GV.Log.WriteException(ex)
        End Try
        ac.AsyncWaitHandle.Close()
    End Sub

#End Region

#Region " === СЕРВЕР WORLD === "

    ''' <summary>
    ''' Проверяет доступность сервера World.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub CheckWorld(obj As Object)

    End Sub

    ''' <summary>
    ''' Останавливает сервер World.
    ''' </summary>
    Friend Sub ShutdownWorld(processes As List(Of Process))

    End Sub

#End Region

#Region " === СЕРВЕР REALMD === "

    ''' <summary>
    ''' Запуск сервера Realmd.
    ''' </summary>
    Friend Sub StartRealmd(ob As Object)
        If _mysqlON AndAlso Not _needServerStop AndAlso Not _realmdON AndAlso IsNothing(_realmdProcess) Then
            GV.Log.WriteAll(My.Resources.P030_RealmdStart)

            ' Исключаем повторный запуск Realmd
            _realmdON = True

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
                        GV.Log.WriteAll(My.Resources.E008_UnknownModule)
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
                        GV.Log.WriteAll(My.Resources.E008_UnknownModule)
                        Exit Sub
                End Select
            End If

            ' Правим файл конфигурации Realmd
            _iniRealmd.Write("RealmdConf", "LoginDatabaseInfo", value)

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
            _realmdProcess = New Process()
            Try
                _realmdProcess.StartInfo = startInfo
                ' Запускаем
                If _realmdProcess.Start() Then
                    GV.Log.WriteAll(My.Resources.P031_RealmdStarted)
                    AddHandler _realmdProcess.OutputDataReceived, AddressOf RealmdOutputDataReceived
                    AddHandler _realmdProcess.ErrorDataReceived, AddressOf RealmdErrorDataReceived
                    AddHandler _realmdProcess.Exited, AddressOf RealmdExited
                    _realmdProcess.BeginOutputReadLine()
                    _realmdProcess.BeginErrorReadLine()
                Else
                    _realmdProcess = Nothing
                End If
            Catch ex As Exception
                ' Realmd выдал исключение
                _realmdON = False
                Try
                    _realmdProcess.Dispose()
                Catch
                End Try
                GV.Log.WriteException(ex)
                MessageBox.Show(My.Resources.E012_RealmdException & vbLf & ex.Message,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Выход из realmd.exe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RealmdExited(ByVal sender As Object, ByVal e As EventArgs)
        GV.Log.WriteAll(My.Resources.P032_RealmdStopped)
        RemoveHandler _realmdProcess.OutputDataReceived, AddressOf RealmdOutputDataReceived
        RemoveHandler _realmdProcess.ErrorDataReceived, AddressOf RealmdErrorDataReceived
        RemoveHandler _realmdProcess.Exited, AddressOf RealmdExited
    End Sub

    ''' <summary>
    ''' Останавливает сервер Realmd.
    ''' </summary>
    Friend Sub ShutdownRealmd(listpc As List(Of Process))
        If Not IsNothing(_realmdProcess) Then RealmdExited(Me, Nothing)
        Dim pc = listpc.FindAll(Function(p) p.ProcessName = "realmd")
        For Each process In pc
            Try
                If process.MainModule.FileName = My.Settings.CurrentFileRealmd Then
                    Try
                        process.Kill()
                        Thread.Sleep(100)
                        _realmdON = False
                        _realmdProcess = Nothing
                        TSSL_Realm.GetCurrentParent.Invoke(Sub()
                                                               TSSL_Realm.Image = My.Resources.red_ball
                                                           End Sub)
                    Catch
                    End Try
                End If
            Catch
                ' Нет доступа.
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Проверка доступности сервера Realmd.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub CheckRealmd(obj As Object)
        Dim host = _iniRealmd.ReadString("RealmdConf", "BindIP", "127.0.0.1")
        If host = "0.0.0.0" Then host = "127.0.0.1"
        Dim port = _iniRealmd.ReadString("RealmdConf", "RealmServerPort", "3724")
        Dim tcpClient = New Net.Sockets.TcpClient
        Dim ac = tcpClient.BeginConnect(host, CInt(port), Nothing, Nothing)
        Try
            If Not ac.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), False) Then
                tcpClient.Close()
                _realmdON = False
                If Me.Visible Then

                    TSSL_Realm.GetCurrentParent().Invoke(Sub()
                                                             TSSL_Realm.Image = My.Resources.red_ball
                                                             Me.Icon = My.Resources.cmangos_red
                                                             Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                                                         End Sub)
                Else
                    ' Меняем иконку в трее, коли свёрнуты
                    Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
                End If
            Else
                tcpClient.EndConnect(ac)
                tcpClient.Close()
                _realmdON = True
                If Me.Visible Then
                    TSSL_Realm.GetCurrentParent().Invoke(Sub()
                                                             TSSL_Realm.Image = My.Resources.green_ball
                                                             Me.Icon = My.Resources.cmangos_realmd_started
                                                             Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                                                         End Sub)
                Else
                    ' Меняем иконку в трее, коли свёрнуты
                    Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_realmd_started
                End If
            End If
        Catch ex As Exception
            If Me.Visible Then
                _realmdON = False
                TSSL_Realm.GetCurrentParent().Invoke(Sub()
                                                         TSSL_Realm.Image = My.Resources.red_ball
                                                         Me.Icon = My.Resources.cmangos_orange
                                                         Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_orange
                                                     End Sub)
            Else
                ' Меняем иконку в трее, коли свёрнуты
                Me.NotifyIcon_SPP2.Icon = My.Resources.cmangos_red
            End If
            GV.Log.WriteException(ex)
        End Try
        ac.AsyncWaitHandle.Close()
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
        NotifyIcon_SPP2.Visible = False
        _EnableClosing = True
        If CurrentRunningServer <> "" Then AutoSave()
        ShutdownAll(True)
        Application.Exit()
    End Sub

    ''' <summary>
    ''' ДВОЙНОЙ КЛИК МЫШКОЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon_SPP2.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ РАЗМЕРА ОКНА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Launcher_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            ' Эта штука переводит приложение в фоновый режим
            ' А ОНО НАДО?
            Me.Hide()
        End If
    End Sub

#End Region

#Region " === СЕРВЕР === "

    ''' <summary>
    ''' МЕНЮ - НАСТРОЙКИ СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerSettings_Click(sender As Object, e As EventArgs) Handles TSMI_ServerSettings.Click
        Dim fServerSettings = New ServerSettings
        fServerSettings.ShowDialog()
    End Sub

    ''' <summary>
    ''' МЕНЮ - ЗАПУСК СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerStart_Click(sender As Object, e As EventArgs) Handles TSMI_ServerStart.Click
        _needServerStart = True
        _needServerStop = False
        ServerStart()
    End Sub

    ''' <summary>
    ''' МЕНЮ - ОСТАНОВКА СЕРВЕРА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMI_ServerStop_Click(sender As Object, e As EventArgs) Handles TSMI_ServerStop.Click
        _needServerStart = False
        _needServerStop = True
        ServerStop()
    End Sub

    ''' <summary>
    ''' Запускает сервер WOW.
    ''' </summary>
    Friend Sub ServerStart()
        _IsShutdown = False
        StartMySQL(Nothing)
        _needServerStart = True
    End Sub

    ''' <summary>
    ''' Останавливает сервер WOW.
    ''' </summary>
    Friend Sub ServerStop()
        _IsShutdown = False
        Dim processes = GetAllProcesses()
        ShutdownRealmd(processes)
        If My.Settings.UseIntMySQL AndAlso Not My.Settings.MySqlAutostart Then ShutdownMySQL(processes)
    End Sub

    ''' <summary>
    ''' Глобальная остановка всего и вся.
    ''' </summary>
    Friend Sub ShutdownAll(shutdown As Boolean)
        _IsShutdown = shutdown
        Dim processes = GetAllProcesses()
        ShutdownWorld(processes)
        ShutdownRealmd(processes)
        ShutdownApache()
        ShutdownMySQL(processes)
        StoppingCheckTimers()
    End Sub

#End Region

#Region " === КОНСОЛЬ === "

    ''' <summary>
    ''' Вывод сообщения в консоль сервера MySQL.
    ''' </summary>
    ''' <param name="text"></param>
    Friend Sub UpdateMySQLConsole(ByVal text As String)
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            If Not text.Contains("Got an error reading communication packets") Then
                Dim ink As New Drawing.Color
                If text.Contains("[Warning]") Then
                    text = Mid(text, 31)
                    ink = Color.OrangeRed
                ElseIf text.Contains("[ERROR]") Then
                    text = Mid(text, 31)
                    ink = Color.Red
                ElseIf text.Contains("[Note]") Then
                    text = Mid(text, 31)
                    ink = Color.DarkOrange
                Else
                    ink = Color.Green
                End If
                Me.RichTextBox_ConsoleMySQL.Invoke(Sub()
                                                       RichTextBox_ConsoleMySQL.SelectionColor = ink
                                                       Select Case RichTextBox_ConsoleMySQL.Lines.Count
                                                           Case 0
                                                               RichTextBox_ConsoleMySQL.AppendText(text)
                                                           Case 500
                                                               ' Не более 500 строк в окне
                                                               Dim str = RichTextBox_ConsoleMySQL.Lines(0)
                                                               RichTextBox_ConsoleMySQL.Select(0, str.Length + 1)
                                                               RichTextBox_ConsoleMySQL.ReadOnly = False
                                                               RichTextBox_ConsoleMySQL.SelectedText = String.Empty
                                                               RichTextBox_ConsoleMySQL.ReadOnly = True
                                                               RichTextBox_ConsoleMySQL.AppendText(vbCrLf & text)
                                                               RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                           Case Else
                                                               RichTextBox_ConsoleMySQL.AppendText(vbCrLf & text)
                                                               RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                       End Select
                                                   End Sub)
            End If
        End If
    End Sub

    ''' <summary>
    ''' При перезагрузке в консоль текста без потери цвета.
    ''' </summary>
    ''' <param name="text">Текст для вывода.</param>
    ''' <param name="ink">Цвет текста.</param>
    Friend Sub UpdateMySQLConsole(text As String, ink As Color)
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            Me.RichTextBox_ConsoleMySQL.Invoke(Sub()
                                                   RichTextBox_ConsoleMySQL.SelectionColor = ink
                                                   Select Case RichTextBox_ConsoleMySQL.Lines.Count
                                                       Case 0
                                                           RichTextBox_ConsoleMySQL.AppendText(text)
                                                       Case 500
                                                           ' Не более 500 строк в окне
                                                           Dim str = RichTextBox_ConsoleMySQL.Lines(0)
                                                           RichTextBox_ConsoleMySQL.Select(0, str.Length + 1)
                                                           RichTextBox_ConsoleMySQL.ReadOnly = False
                                                           RichTextBox_ConsoleMySQL.SelectedText = String.Empty
                                                           RichTextBox_ConsoleMySQL.ReadOnly = True
                                                           RichTextBox_ConsoleMySQL.AppendText(vbCrLf & text)
                                                           RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                       Case Else
                                                           RichTextBox_ConsoleMySQL.AppendText(vbCrLf & text)
                                                           RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                   End Select
                                               End Sub)
        End If
    End Sub

    ''' <summary>
    ''' Вывод сообщения в консоль сервера Realmd.
    ''' </summary>
    ''' <param name="text">Текст для вывода.</param>
    Friend Sub UpdateRealmdConsole(ByVal text As String)
        If Not IsNothing(text) AndAlso Me.Visible = True Then
            Me.RichTextBox_ConsoleMySQL.Invoke(Sub()
                                                   RichTextBox_ConsoleRealmd.SelectionColor = My.Settings.RealmdConsoleForeColor
                                                   Select Case RichTextBox_ConsoleRealmd.Lines.Count
                                                       Case 0
                                                           RichTextBox_ConsoleRealmd.AppendText(text)
                                                       Case 500
                                                           ' Не более 500 строк в окне
                                                           Dim str = RichTextBox_ConsoleRealmd.Lines(0)
                                                           RichTextBox_ConsoleRealmd.Select(0, str.Length + 1)
                                                           RichTextBox_ConsoleRealmd.ReadOnly = False
                                                           RichTextBox_ConsoleRealmd.SelectedText = String.Empty
                                                           RichTextBox_ConsoleRealmd.ReadOnly = True
                                                           RichTextBox_ConsoleRealmd.AppendText(vbCrLf & text)
                                                           RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                       Case Else
                                                           RichTextBox_ConsoleRealmd.AppendText(vbCrLf & text)
                                                           RichTextBox_ConsoleMySQL.ScrollToCaret()
                                                   End Select
                                               End Sub)
        End If
    End Sub

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
                    If Not IsNothing(_realmdProcess) Then _realmdProcess.StandardInput.WriteLine(Me.TextBox_Command.Text)
                End If
                Me.TextBox_Command.Text = ""
            Case Keys.Escape
                Me.TextBox_Command.Text = ""
        End Select
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

        Dim fnt = New Font(F.Families(0), My.Settings.ConsoleFontSize, My.Settings.ConsoleFontStyle)

        ' MySQL
        RichTextBox_ConsoleMySQL.SuspendLayout()
        Dim text = RegularExpressions.Regex.Split(RichTextBox_ConsoleMySQL.Text, "(\r\n|\r|\n)", RegularExpressions.RegexOptions.ExplicitCapture)
        RichTextBox_ConsoleMySQL.Text = ""
        RichTextBox_ConsoleMySQL.Font = fnt 'New Font("Calibri", My.Settings.CurrentConsoleFont, FontStyle.Bold)
        For Each line In text
            Dim ink As Color
            If line.Contains("[Warning]") Then
                ink = Color.OrangeRed
            ElseIf line.Contains("[ERROR]") Then
                ink = Color.Red
            ElseIf line.Contains("[Note]") Then
                ink = Color.DarkOrange
            Else
                ink = Color.Green
            End If
            UpdateMySQLConsole(line, ink)
        Next
        RichTextBox_ConsoleMySQL.ResumeLayout()

        ' Realmd
        RichTextBox_ConsoleRealmd.SuspendLayout()
        Dim txt = RichTextBox_ConsoleRealmd.Text
        RichTextBox_ConsoleRealmd.Text = ""
        RichTextBox_ConsoleRealmd.Font = fnt 'New Font("Segoe UI", My.Settings.CurrentConsoleFont, FontStyle.Bold)
        RichTextBox_ConsoleRealmd.Text = txt
        RichTextBox_ConsoleRealmd.ResumeLayout()

        ' World
        RichTextBox_ConsoleWorld.SuspendLayout()
        txt = RichTextBox_ConsoleWorld.Text
        RichTextBox_ConsoleWorld.Text = ""
        RichTextBox_ConsoleWorld.Font = fnt 'New Font("Segoe UI", My.Settings.CurrentConsoleFont, FontStyle.Bold)
        RichTextBox_ConsoleWorld.Text = txt
        RichTextBox_ConsoleWorld.ResumeLayout()

    End Sub

#End Region

#Region " === TABCONTROL === "

    ''' <summary>
    ''' Окрашивает ярлыки вкладок в те же цвета, что и фон вкладки.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TabControl1_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles TabControl1.DrawItem

        Dim g As Graphics = e.Graphics
        Dim tp As TabPage = TabControl1.TabPages(e.Index)
        Dim br As Brush
        Dim r As New RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2)
        Dim strTitle As String = tp.Text
        Dim sf As New StringFormat With {.Alignment = StringAlignment.Center}

        If TabControl1.SelectedIndex = e.Index Then
            ' Активная вкладка
            br = New SolidBrush(Color.White)
            g.FillRectangle(br, e.Bounds)
            br = New SolidBrush(tp.ForeColor)
            g.DrawString(strTitle, TabControl1.Font, br, r, sf)
        Else
            ' Прочие вкладки
            br = New SolidBrush(tp.BackColor)
            g.FillRectangle(br, e.Bounds)
            br = New SolidBrush(tp.ForeColor)
            g.DrawString(strTitle, TabControl1.Font, br, r, sf)
        End If

    End Sub

#End Region

End Class
