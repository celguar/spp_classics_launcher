﻿
Public Class GV

#Region " === ДРУЖЕСТВЕННЫЕ ПОЛЯ И СВОЙСТВА === "

    ''' <summary>
    ''' Событие для Spash.
    ''' </summary>
    ''' <param name="code">Код события.</param>
    ''' <param name="message">Сообщение.</param>
    Friend Shared Event OutMessage(ByVal code As ECodeSend, ByVal message As String)

    ''' <summary>
    ''' Настройки локализации.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared Property CI As Globalization.CultureInfo

    ''' <summary>
    ''' Список найденных модулей.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared Property Modules As New List(Of SPP2Module)

    ''' <summary>
    ''' Код ошибки инициализации.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property ErrorCode As ECode = ECode.OK

    ''' <summary>
    ''' Текущая форма лаунчера.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared Property SPP2Launcher As Launcher

    ''' <summary>
    ''' Журнал событий.
    ''' </summary>
    Friend Shared Log As EasyLog

    ''' <summary>
    ''' Требуется перезагрузка лаунчера.
    ''' </summary>
    Friend Shared NeedRestart As Boolean

    ''' <summary>
    ''' Сброс настроек.
    ''' </summary>
    Friend Shared ResetSettings As Boolean

    ''' <summary>
    ''' Клиент MySQL.
    ''' </summary>
    Friend Shared SQL As MySqlProvider

#End Region

#Region " === ПЕРЕЧИСЛЕНИЯ === "

    Public Enum EProcess

        MySQL

        Apache

        Realmd

        World

    End Enum

    Public Enum ECode As Byte

        ''' <summary>
        ''' ОК
        ''' </summary>
        OK

        ''' <summary>
        ''' Ошибка загрузки настроек лаунчера.
        ''' </summary>
        ErrorLauncherConfig

        ''' <summary>
        ''' Другой лаунчер уже запущен.
        ''' </summary>
        ErrorLauncherAlready

        ''' <summary>
        ''' Не найден базовый каталог проекта.
        ''' </summary>
        ErrorBaseCat

        ''' <summary>
        ''' Не найден каталог исполняемых файлов CMaNGOS.
        ''' </summary>
        ErrorMangoCat

        ''' <summary>
        ''' Не найден каталог MySQL
        ''' </summary>
        ErrorMySqlCat

        ''' <summary>
        ''' Не найден каталог Apache
        ''' </summary>
        ErrorApacheCat

        ''' <summary>
        ''' Не найден каталог модулей проекта.
        ''' </summary>
        ErrorModulesCat

        ''' <summary>
        ''' Не найдено ни одного модуля сервера WoW.
        ''' </summary>
        ErrorNoModules

    End Enum

    ''' <summary>
    ''' Коды событий отправляемые подписчикам.
    ''' </summary>
    Public Enum ECodeSend As Byte

        ''' <summary>
        ''' Ошибка.
        ''' </summary>
        [Error]

        ''' <summary>
        ''' Добавлен новый модуль.
        ''' </summary>
        AddModule

    End Enum

    ''' <summary>
    ''' Известные лаунчеру модули.
    ''' </summary>
    Public Enum EModule As Byte

        Classic

        Tbc

        Wotlk

        Restart

    End Enum

#End Region

#Region " === ПЕРВИЧНАЯ ИНИЦИАЛИЗАЦИЯ === "

    ''' <summary>
    ''' Инициализация базовых параметров лаунчера.
    ''' </summary>
    Public Sub BaseInit()

        Try
            ' Включаем локальные настройки
            SPP2SettingsProvider.ApplyProvider(Application.StartupPath & "\SPP2.cfg", True, My.MySettings.Default)
            ' Всякий раз проводим Upgrade на случай, если файл разрушен.
            My.Settings.Upgrade()
        Catch ex As Exception
            ' Тут уже и не знаю что делать!
            _ErrorCode = ECode.ErrorLauncherConfig
            Exit Sub
        End Try

        If My.Settings.FirstStart Then
            ' Устанавливаем локаль из системы
            My.Settings.Locale = My.Computer.Info.InstalledUICulture.Name
            My.Settings.Save()
        End If

        ' Устанавливаем локаль из настроек
        CI = New Globalization.CultureInfo(My.Settings.Locale)
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(GV.CI.Name)
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(GV.CI.Name)
        My.Application.ChangeCulture(GV.CI.Name)
        My.Application.ChangeUICulture(GV.CI.Name)

        ' Инициализация журнала событий
        Dim _err = NewExitCode()
        Dim cat = SPP2SettingsProvider.SettingsFolder
        If Not IO.Directory.Exists(cat & "\Logs") Then IO.Directory.CreateDirectory(cat & "\Logs")
        Log = New EasyLog(logLevel:=CType(My.Settings.LogLevel, EasyLog.ESeverity),
                          sqlLogLevel:=CType(My.Settings.SqlLogLevel, EasyLog.ESqlSeverity),
                          _err:=_err,
                          logDir:=cat & "\Logs",
                          language:=CI.Name)
        Log.WriteInfo(My.Resources.P001_BaseInit)

        If IO.Directory.Exists(Application.StartupPath & "\SPP_Server") Then
            My.Settings.DirSPP2 = Application.StartupPath
            My.Settings.FirstStart = False
        Else
            Dim parent = IO.Path.GetDirectoryName(Application.StartupPath & ".txt")
            If IO.Directory.Exists(parent & "\SPP_Server") Then
                My.Settings.DirSPP2 = parent
                My.Settings.FirstStart = False
            End If
        End If

    End Sub

#End Region

#Region " === ИНИЦИАЛИЗАЦИЯ МОДУЛЕЙ === "

    ''' <summary>
    ''' Инициализация основных настроек.
    ''' </summary>
    Public Sub FullInit()
        Dim cat As String

        ' Проверяем наличие другого запущенного лаунчера
        ' Следует отметить, что фоновые процессы не будут пойманы.
        Dim listpc = GetAllProcesses()
        Dim pc = listpc.FindAll(Function(p) p.ProcessName = Process.GetCurrentProcess().ProcessName)
        If pc.Count > 1 Then
            ' Пока никак не реагируем
            _ErrorCode = ECode.ErrorLauncherAlready
        End If

        ' Проверяем наличие в корне каталога SPP_Server 
        cat = SPP2GLOBAL
        If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then

            ' Проверяем наличие исполняемых файлов CMaNGOS
            cat = SPP2CMANGOS
            If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then

                ' Проверяем наличие MySQL
                cat = SPP2MYSQL
                If My.Settings.UseIntMySQL AndAlso Not IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then
                    _ErrorCode = ECode.ErrorMySqlCat
                    RaiseEvent OutMessage(ECodeSend.Error, My.Resources.E005_MySqlNotFound)
                    My.Settings.UseIntMySQL = False
                    Exit Sub
                End If

                cat = SPP2APACHE
                If My.Settings.UseIntApache AndAlso Not IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then
                    _ErrorCode = ECode.ErrorApacheCat
                    RaiseEvent OutMessage(ECodeSend.Error, My.Resources.E005_MySqlNotFound)
                    My.Settings.UseIntApache = False
                    Exit Sub
                End If

                ' Проверяем наличие в SPP_Server каталога Modules
                cat = SPP2MODULES
                If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then

                    ' Проверяем наличие модуля Classic
                    cat = SPP2MODULES & "\vanilla\maps"
                    If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then
                        Modules.Add(New SPP2Module(EModule.Classic))
                        RaiseEvent OutMessage(ECodeSend.AddModule, EModule.Classic.ToString)
                    End If

                    ' Проверяем наличие модуля Tbc
                    cat = SPP2MODULES & "\tbc\maps"
                    If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then
                        Modules.Add(New SPP2Module(EModule.Tbc))
                        RaiseEvent OutMessage(ECodeSend.AddModule, EModule.Tbc.ToString)
                    End If

                    ' Проверяем наличие модуля Wotlk
                    cat = SPP2MODULES & "\wotlk\maps"
                    If IO.Directory.Exists(My.Settings.DirSPP2 & "\" & cat) Then
                        Modules.Add(New SPP2Module(EModule.Wotlk))
                        RaiseEvent OutMessage(ECodeSend.AddModule, EModule.Wotlk.ToString)
                    End If

                    ' Не найдено ни одного модуля сервера WoW
                    If Modules.Count = 0 Then
                        _ErrorCode = ECode.ErrorNoModules
                        RaiseEvent OutMessage(ECodeSend.Error, My.Resources.E004_ModulesNotFound)
                    End If

                Else
                    _ErrorCode = ECode.ErrorModulesCat
                    RaiseEvent OutMessage(ECodeSend.Error, String.Format(My.Resources.E001_DirNotFound, cat))
                End If

            Else
                _ErrorCode = ECode.ErrorMangoCat
                RaiseEvent OutMessage(ECodeSend.Error, My.Resources.E007_MangoNotFound)
            End If

        Else
            _ErrorCode = ECode.ErrorBaseCat
            RaiseEvent OutMessage(ECodeSend.Error, String.Format(My.Resources.E001_DirNotFound, cat))
        End If
    End Sub

#End Region

End Class
