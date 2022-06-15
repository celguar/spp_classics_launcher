
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text

Module SPP2Helper

    ''' <summary>
    ''' Строка ожидаемая для вывода в консоль World
    ''' </summary>
    Friend WaitWorldMessage As String = ""

    ''' <summary>
    ''' Команда полностью выполнена
    ''' </summary>
    Friend CommandSuccessfull As Boolean

    ''' <summary>
    ''' Интервал проверки состояния сервера Realmd.
    ''' </summary>
    Friend ChangeRealmdCheck As Double = 2.3

    ''' <summary>
    ''' Семейство шрифтов для консоли.
    ''' </summary>
    Friend F As Drawing.Text.PrivateFontCollection

    ''' <summary>
    ''' Поток контролирующий состояние процессов.
    ''' </summary>
    Friend PC As Threading.Thread

    ''' <summary>
    ''' Список базовых контролируемых процессов.
    ''' </summary>
    Friend BP As ProcessController

    ''' <summary>
    ''' Время безотказной работы.
    ''' </summary>
    Friend WorldStartTime As Double

    ''' <summary>
    ''' Коллекция подсказок для командной строки консоли.
    ''' </summary>
    Friend HintCollection As New AutoCompleteStringCollection()

    ''' <summary>
    ''' Идёт процесс Backup.
    ''' </summary>
    Friend BackupProcess As Boolean

#Region " === ПЕРЕЧИСЛЕНИЯ И СТРУКТУРЫ === "

    ''' <summary>
    ''' Процесс вызвавший процедуру.
    ''' </summary>
    Friend Enum ECaller

        mysql

        realmd

        world

    End Enum

    Friend Enum EProcess As Byte

        mysqld

        apache

        realmd

        world

        wow

    End Enum

    Friend Enum EAction

        [Nothing]

        Start

        NeedExit

        UpdateMainMenu

    End Enum

    Friend Enum ECommand

        AccountCreate

        AccountSetPassword

        SetGmLevel

        SetAddon

    End Enum

    Public Structure ConsoleCommand

        Public Command As String

        Public Message As ECommand

        Public MyAction As Action(Of Boolean)

        Sub New(command As String, message As ECommand, action As Action(Of Boolean))
            Me.Command = command
            Me.Message = message
            Me.MyAction = action
        End Sub

    End Structure

#End Region

#Region " === КОНСТАНТЫ === "

    ''' <summary>
    ''' Цвет вывода в консоль обычных сообщений.
    ''' </summary>
    Friend CONSOLE As Color = Color.YellowGreen

    ''' <summary>
    ''' Цвет вывода в консоль предупреждений.
    ''' </summary>
    Friend WCONSOLE As Color = Color.Yellow

    ''' <summary>
    ''' Цвет вывода в консоль сообщений с ошибкой.
    ''' </summary>
    Friend ECONSOLE As Color = Color.OrangeRed

    ''' <summary>
    ''' Цвет вывода в консоль диалоговых вопросов.
    ''' </summary>
    Friend QCONSOLE As Color = Color.DarkCyan

    ''' <summary>
    ''' Корневой каталог проекта Single Player Project 2.
    ''' </summary>
    Friend Const SPP2GLOBAL As String = "SPP_Server"

    ''' <summary>
    ''' Каталог расположения модулей серверов Single Player Project 2.
    ''' </summary>
    Friend Const SPP2MODULES As String = "SPP_Server\Modules"

    ''' <summary>
    ''' Каталог расположения Web сервера.
    ''' </summary>
    Friend Const SPP2APACHE As String = "SPP_Server\Server\Tools\Apache24"

    ''' <summary>
    ''' Каталог расположения сервера MySQL.
    ''' </summary>
    Friend Const SPP2MYSQL As String = "SPP_Server\Server\Database"

    ''' <summary>
    ''' Каталог расположения CMaNGOS.
    ''' </summary>
    Friend Const SPP2CMANGOS As String = "SPP_Server\Server\Binaries"

    ''' <summary>
    ''' Каталог расположения настроек конфигурации модулей серверов.
    ''' </summary>
    Friend Const SPP2SETTINGS As String = "SPP_Server\Settings"

    ''' <summary>
    ''' Каталог расположения резервных копий баз данных.
    ''' </summary>
    Friend Const SPP2SAVES As String = "SPP_Server\Saves"

#End Region

#Region " === ТАЙМЕРЫ === "

    ''' <summary>
    ''' Таймер запуска сервера MySQL.
    ''' </summary>
    Friend TimerStartMySQL As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера Realmd.
    ''' </summary>
    Friend TimerStartRealmd As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера World.
    ''' </summary>
    Friend TimerStartWorld As Threading.Timer

#End Region

#Region " === РАБОТА ТАЙМЕРОВ === "

    ''' <summary>
    ''' Время запустить сервер MySQL.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartMySQL(obj As Object)
        Threading.Thread.CurrentThread.CurrentUICulture = GV.CI
        Threading.Thread.CurrentThread.CurrentCulture = GV.CI
        ' Выключаем таймер
        TimerStartMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "MySQL"))
        If GV.SPP2Launcher.NeedExitLauncher Then Exit Sub
        ' Запускаем MySQL
        GV.SPP2Launcher.StartMySQL(obj)
    End Sub

    ''' <summary>
    ''' Время запустить сервер World.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartWorld(obj As Object)
        Threading.Thread.CurrentThread.CurrentUICulture = GV.CI
        Threading.Thread.CurrentThread.CurrentCulture = GV.CI
        ' Выключаем таймер
        TimerStartWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "World"))
        If GV.SPP2Launcher.NeedExitLauncher Then Exit Sub
        ' Запускаем World
        GV.SPP2Launcher.StartWorld(obj)
    End Sub

    ''' <summary>
    ''' Время запустить Realmd.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartRealmd(obj As Object)
        Threading.Thread.CurrentThread.CurrentUICulture = GV.CI
        Threading.Thread.CurrentThread.CurrentCulture = GV.CI
        ' Выключаем таймер
        TimerStartRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "Realmd"))
        If GV.SPP2Launcher.NeedExitLauncher Then Exit Sub
        ' Запускаем Realmd
        GV.SPP2Launcher.StartRealmd(obj)
    End Sub

#End Region

#Region " === ЗАГРУЗКА ШРИФТОВ === "

    ''' <summary>
    ''' Загружает шрифт из файла, предварительно выгрузив на диск...
    ''' </summary>
    ''' <returns></returns>
    Friend Function LoadFont() As Font
        Dim cat = SPP2SettingsProvider.SettingsFolder
        If Not IO.File.Exists(cat & "\" & "notomono-regular.ttf") Then
            IO.File.WriteAllBytes(cat & "\" & "notomono-regular.ttf", My.Resources.notomono_regular)
        End If
        F = New System.Drawing.Text.PrivateFontCollection()
        F.AddFontFile(cat & "\" & "notomono-regular.ttf")
        Return New Font(F.Families(0), My.Settings.ConsoleFontSize)
    End Function

    ''' <summary>
    ''' Загружает шрифт из ресурсов. Использовать только с GDI+ и только ttf!
    ''' </summary>
    ''' <returns></returns>
    Friend Function GetFont() As Font
        F = New Drawing.Text.PrivateFontCollection
        Dim fnt As Font
        Dim buffer() As Byte
        buffer = My.Resources.notomono_regular
        Dim ip As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(Byte)) * buffer.Length)
        Marshal.Copy(buffer, 0, ip, buffer.Length)
        Try
            F.AddMemoryFont(ip, buffer.Length) ' если тип шрифта отличается от ttf будет исключение
            'fnt = New Font(F.Families(0), My.Settings.ConsoleFontSize, FontStyle.Bold, GraphicsUnit.Pixel)
            Return New Font(F.Families(0), My.Settings.ConsoleFontSize)
        Catch ex As Exception
            'fnt = New Font("Segoe UI", My.Settings.ConsoleFontSize, FontStyle.Bold, GraphicsUnit.Pixel) ' подставляем системный если не удалось загрузить из Resources
            Return New Font("Consolas", My.Settings.ConsoleFontSize)
        End Try
        Marshal.FreeHGlobal(ip)
        Return fnt
    End Function

    ''' <summary>
    ''' Возвращает шрифт с текущими настройками.
    ''' </summary>
    ''' <returns></returns>
    Friend Function GetCurrentFont() As Font
        If F.Families.Count > 0 Then
            Return New Font(F.Families(0), My.Settings.ConsoleFontSize, My.Settings.ConsoleFontStyle)
        Else
            Return New Font("Consolas", My.Settings.ConsoleFontSize, My.Settings.ConsoleFontStyle)
        End If
    End Function

#End Region

#Region " === РАЗНОЕ === "

    ''' <summary>
    ''' Создаёт новый кортеж сообщения об ошибке.
    ''' </summary>
    ''' <returns></returns>
    Friend Function NewExitCode() As Tuple(Of Integer, String)
        Return New Tuple(Of Integer, String)(0, "OK")
    End Function

    ''' <summary>
    ''' Функция возвращает список имеющихся адресов IpV4 на данном компьютере.
    ''' </summary>
    ''' <param name="addANY">Флаг добавления в список значение ANY.</param>
    ''' <returns>Список IpV4 адаптеров в системе.</returns>
    Friend Function GetLocalIpAddresses(addANY As Boolean) As List(Of String)
        Dim strHostName As String
        Dim alladdresses() As Net.IPAddress
        Dim addresses As New List(Of String)

        strHostName = Net.Dns.GetHostName()
        alladdresses = Net.Dns.GetHostAddresses(strHostName)

        ' Ищем адреса IpV4
        For Each address As Net.IPAddress In alladdresses
            ' Собираем найденные в список
            If address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                addresses.Add(address.ToString)
            End If
        Next

        ' Добавляем loopback
        addresses.Add(Net.IPAddress.Loopback.ToString)
        If addANY Then addresses.Add("0.0.0.0")

        Return addresses
    End Function

    ''' <summary>
    ''' Возвращает список доступных процессов.
    ''' </summary>
    Friend Function GetAllProcesses() As List(Of Process)
        Dim processlist() As Process = Process.GetProcesses()
        Dim retlist As New List(Of Process)
        For Each process As Process In processlist
            Try
                'If String.Compare(process.MainModule.FileName, path, StringComparison.OrdinalIgnoreCase) = 0 Then
                retlist.Add(process)
                'End If
            Catch ex As Exception
            End Try
        Next process
        Return retlist
    End Function

    ''' <summary>
    ''' Поиск процесса и выполнение действия. Возвращает True если процесс найден и его месторасположение соответствует требуемому.
    ''' </summary>
    ''' <param name="p">Искомый процесс.</param>
    ''' <param name="kill">Убить процесс. Данный параметр действителен только в отношении Apaсhe и Realmd
    ''' По умолчанию - False</param>
    ''' <returns></returns>
    Friend Function CheckProcess(p As EProcess, Optional ByVal kill As Boolean = False) As Boolean
        Dim pm As String = ""
        Dim path As String = ""

        Select Case p

            Case EProcess.mysqld
                pm = "mysqld"
                path = My.Settings.DirSPP2 & "\" & SPP2MYSQL & "\bin\mysqld.exe"

            Case EProcess.apache
                pm = "spp-httpd"
                path = My.Settings.DirSPP2 & "\" & SPP2APACHE & "\bin\spp-httpd.exe"

            Case EProcess.realmd
                pm = "realmd"
                path = My.Settings.CurrentFileRealmd

            Case EProcess.world
                pm = "mangosd"
                path = My.Settings.CurrentFileWorld

            Case EProcess.wow
                pm = "Wow"
                path = My.Settings.WowClientPath

        End Select

        Dim processes = Process.GetProcessesByName(pm)
        For Each pr In processes
            Try
                If Not kill Then
                    If pr.MainModule.FileName = path Then Return True
                Else
                    ' Убиваем только Apache или Realmd
                    If p = EProcess.apache Or p = EProcess.realmd Then pr.Kill()
                End If
            Catch ex As Exception
                GV.Log.WriteException(ex)
                GV.Log.WriteError(String.Format(My.Resources.E019_StopException, pr))
            End Try
        Next
        Return False
    End Function

    ''' <summary>
    ''' Удаляет "лишние" бэкапы.
    ''' </summary>
    ''' <param name="autosave">Флаг проверки - autosave или ручные сохранения.</param>
    ''' <param name="fileName"></param>
    Friend Sub RemoveOldBackups(autosave As Boolean, fileName As String)
        If Not My.Settings.UseSqlBackupProjectFolder Then
            Try
                Dim path = IO.Path.GetDirectoryName(fileName)
                Dim name = IO.Path.GetFileName(fileName).Split("_"c)(0)
                Dim sfiles As New List(Of SortedFiles)
                ' Получаем список
                Dim files = IO.Directory.GetFiles(path, name & "*", IO.SearchOption.TopDirectoryOnly)
                Dim count = If(autosave, My.Settings.AutosaveBackupCount + 1, My.Settings.ManualBackupCount)
                If files.Count > count Then
                    ' Заполняем список
                    For Each file In files
                        sfiles.Add(New SortedFiles(IO.File.GetLastWriteTime(file), file))
                    Next
                    ' Сортируем по дате
                    sfiles.OrderBy(Function(x) x.Created).ToList
                    Do
                        Try
                            IO.File.Delete(sfiles(0).FileName)
                            sfiles.RemoveAt(0)
                            If sfiles.Count = count Then Exit Do
                        Catch ex As Exception
                            GV.Log.WriteException(ex)
                            Exit Do
                        End Try
                    Loop
                End If
            Catch ex As Exception
                GV.Log.WriteException(ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Один файл.
    ''' </summary>
    Private Class SortedFiles
        Public Created As Date
        Public FileName As String

        Sub New(created As Date, fileName As String)
            Me.Created = created
            Me.FileName = fileName
        End Sub

    End Class

#End Region

#Region " === ОТПРАВКА СООБЩЕНИЙ В КОНСОЛЬ === "

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateMySQLConsole(e.Data, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateMySQLConsole(e.Data, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateRealmdConsole(e.Data, My.Settings.RealmdConsoleForeColor)
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateRealmdConsole(e.Data, Color.Red)
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль World сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then

            If GV.SPP2Launcher.CurrentWorldConsoleFilter = 0 Then
                GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
            ElseIf My.Settings.ConsoleMessageFilter = 1 Then
                If Not e.Data.Contains("ERROR") Then
                    GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
                End If
            Else
                If e.Data.Contains("__") Or
                    e.Data.Contains("Starting") Or
                    e.Data.Contains("Initializing") Or
                    e.Data.Contains("initialized") Or
                    e.Data.Contains("Loading") Or
                    e.Data.Contains("Loaded") Or
                    e.Data.Contains("SQL") Or
                    e.Data.Contains("SERVER STARTUP TIME") Then
                    GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
                End If
            End If

            ' Эта строка говорит о том, что сервер полностью запустился.
            If e.Data.Contains("SERVER STARTUP TIME") Then
                WorldStartTime = Date.Now.ToOADate()
            End If

            ' Эта строка говорит о том, что сервер готов к смерти
            If e.Data.Contains("mangos>Halting process") Or
                e.Data.Contains(My.Resources.E016_WorldCrashed) Or
                e.Data.Contains("mangos>mangos>") Then
                ' Сервер World остановился. Удаляем хандлеры
                If Not IsNothing(GV.SPP2Launcher.WorldProcess) Then GV.SPP2Launcher.WorldExited(Nothing, Nothing)
                GV.SPP2Launcher.WorldProcess = Nothing
                GV.SPP2Launcher.WorldON = False
                CheckProcess(EProcess.world, True)
                ' Если постфильтр > 0 тогда выводим и этот текст
                If My.Settings.ConsolePostMessageFilter > 0 Then GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
            End If

            ' Ожидаемая строка выполнения команды сервером
            If WaitWorldMessage.Length > 0 And e.Data.Contains(WaitWorldMessage) Then
                CommandSuccessfull = True
                ' Если постфильтр > 0 тогда выводим и этот текст
                If My.Settings.ConsolePostMessageFilter > 0 Then GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
            End If

        End If

    End Sub

    ' ПРИМЕР ИСПОЛЬЗОВАНИЯ WaitSuccessfull
    ' Отправляем команду на создание аккаунта
    'Dim cmd = String.Format(".account create {0} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim)
    'Dim cm = New ConsoleCommand(cmd, ECommand.AccountCreate, AddressOf AccountCreated)
    'Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
    't.Start(cm)

    ''' <summary>
    ''' Ожидание выполнения команды. При успешном выполнении возвращает True.
    ''' False следует считать как timeout.
    ''' </summary>
    ''' <param name="obj">Структура вида ConsoleCommand.</param>
    Friend Sub WaitSuccessfull(obj As Object)
        Try
            Dim cm = CType(obj, ConsoleCommand)
            Dim t = Now
            CommandSuccessfull = False
            Dim result As Boolean

            Select Case cm.Message

                Case ECommand.AccountCreate
                    WaitWorldMessage = "Account created:"

                Case ECommand.AccountSetPassword
                    WaitWorldMessage = "The password was changed"

                Case ECommand.SetGmLevel
                    WaitWorldMessage = "You change security level"

                Case ECommand.SetAddon
                    WaitWorldMessage = "has been granted"

            End Select

            ' Отправляем команду в консоль
            GV.SPP2Launcher.SendCommandToWorld(cm.Command)
            GV.SPP2Launcher.UpdateWorldConsole(My.Resources.P068_WaitExecution, CONSOLE)

            Do
                If Not GV.SPP2Launcher.WorldON Then Exit Do
                If Now > t.AddSeconds(60) Then Exit Do
                If CommandSuccessfull Then
                    result = True
                    Exit Do
                End If
                Threading.Thread.Sleep(500)
            Loop

            WaitWorldMessage = String.Empty
            CommandSuccessfull = False

            If Not result Then
                ' Таймаут
                GV.SPP2Launcher.UpdateWorldConsole(My.Resources.P069_Timeout, ECONSOLE)
            End If

            ' Пеерходим в нужную процедуру
            cm.MyAction(result)

        Catch ex As Exception
            GV.Log.WriteException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Отправляет в консоль World сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            If My.Settings.ConsoleMessageFilter = 0 Then
                GV.SPP2Launcher.UpdateWorldConsole(e.Data, Color.Red)
            ElseIf My.Settings.ConsoleMessageFilter = 2 Then
                If e.Data.Contains("SQL") Then
                    GV.SPP2Launcher.UpdateWorldConsole(e.Data, Color.Red)
                End If
            End If
        End If
    End Sub

#End Region

#Region " === ПОТОКИ === "

    ''' <summary>
    ''' Выводит в консоль World информацию о разработчиках.
    ''' </summary>
    Friend Sub PreStart()
        Try
            ' Выводим информацию по разработчикам
            If IO.File.Exists(My.Settings.DirSPP2 & "\" & SPP2GLOBAL & "\credits.txt") Then
                Dim str = IO.File.ReadAllText(My.Settings.DirSPP2 & "\" & SPP2GLOBAL & "\credits.txt")
                Dim s() = System.Text.RegularExpressions.Regex.Split(str, "(\r\n|\r|\n)",
                          RegularExpressions.RegexOptions.ExplicitCapture)
                For Each line As String In s
                    GV.SPP2Launcher.UpdateWorldConsole(line, My.Settings.WorldConsoleForeColor)
                    Threading.Thread.Sleep(50)
                Next
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Поиск процесса, ожидание его завершения и выполнение следующего действия.
    ''' Возвращает False если постдействие не выполнено и True если все действия выполнены успешно.
    ''' </summary>
    ''' <param name="p">Искомый процесс.</param>
    ''' <param name="a1">Выполняемое действие по завершению процесса.</param>
    ''' <param name="outMessage">Выводить информацию в логи и консоль? По умолчанию - True.</param>
    Friend Sub WaitProcessEnd(p As EProcess, a1 As EAction, Optional ByVal outMessage As Boolean = True)
        Dim str As String = ""
        Dim timer As Threading.Timer = Nothing
        Dim path As String = ""

        Select Case p

            Case EProcess.mysqld
                path = My.Settings.DirSPP2 & "\" & SPP2MYSQL & "\bin\mysqld.exe"
                str = String.Format(My.Resources.P045_ServerStopped, "MySQL")
                timer = TimerStartMySQL

            Case EProcess.apache
                Exit Sub

            Case EProcess.realmd
                path = My.Settings.CurrentFileRealmd
                str = String.Format(My.Resources.P045_ServerStopped, "Realmd")
                timer = TimerStartRealmd

            Case EProcess.world
                path = My.Settings.CurrentFileWorld
                str = String.Format(My.Resources.P045_ServerStopped, "World")
                timer = TimerStartWorld

        End Select

        Dim processes = Process.GetProcessesByName(p.ToString)
        If processes.Count > 0 Then
            For Each pr In processes
                Try
                    If pr.MainModule.FileName = path Then

                        Do While Not pr.HasExited
                            ' Процесс найден, ждём его завершения
                            Threading.Thread.Sleep(1000)
                        Loop
                        ' Процесс завершен
                        If outMessage Then GV.Log.WriteInfo(GV.SPP2Launcher.UpdateMySQLConsole(str, CONSOLE))
                        GV.SPP2Launcher.UpdateMessageStatusStrip("")
                        Exit For
                    End If
                Catch ex As Exception
                    ' Нет доступа
                End Try
            Next
        End If

        ' Разбирем постзадачу
        Select Case a1

            Case EAction.Nothing
                ' Ничего не выполнять

            Case EAction.Start
                ' Запускаем таймер необходимой постзадачи
                If Not IsNothing(timer) Then timer.Change(2000, 2000)

            Case EAction.UpdateMainMenu
                ' Обновление текущих настроек
                GV.SPP2Launcher.Invoke(Sub()
                                           GV.SPP2Launcher.UpdateMainMenu(False)
                                       End Sub)

            Case EAction.NeedExit
                ' Это по сути выход из приложения
                ' Дожидаемся остановки всех серверов
                Do
                    If Not CheckProcess(EProcess.mysqld) AndAlso Not GV.SPP2Launcher.RealmdON Then Exit Do
                    Threading.Thread.Sleep(100)
                Loop

                ' Закрываем приложение
                GV.SPP2Launcher.EnableClosing = True
                GV.SPP2Launcher.NotifyIcon_SPP2.Visible = False
                If GV.NeedRestart Or My.Settings.FirstStart Then
                    If Not GV.ResetSettings Then
                        My.Settings.LastLoadedServerType = GV.EModule.Restart.ToString
                        My.Settings.Save()
                    Else
                        IO.File.Delete(SPP2SettingsProvider.SettingsFile)
                    End If
                End If
                GV.SPP2Launcher.NotifyIcon_SPP2.Visible = False
                Try
                    Application.Exit()
                Catch
                End Try

        End Select

    End Sub

    ''' <summary>
    ''' Тик на каждую секунду.
    ''' </summary>
    Friend Sub EverySecond()
        Do
            Threading.Thread.Sleep(1000)
            If WorldStartTime > 0 Then
                GV.SPP2Launcher.UpdateMessageStatusStrip(String.Format("Uptime: {0:dd\.hh\:mm\:ss} ", Date.Now - Date.FromOADate(WorldStartTime)))
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Выводит в строку состояния просьбу ожидания завершения процесса mangosd
    ''' Следует отметить, что на деле контролится вывод сообщения "mangos>Halting process..."
    ''' в процедуре "OutWorldConsole(text As String)
    ''' </summary>
    ''' <param name="processID">Идентификатор процесса, который следует завалить.</param>
    ''' <param name="otherServers">Вырубить так же и прочие серверы.</param>
    Friend Sub StoppingWorld(processID As Integer, otherServers As Boolean)
        WorldStartTime = 0

        ' Очищаем контроллер автозапуска серверов WoW
        BP.ProcessesAreStopped()

        If processID > 0 Then
            Do
                ' Пишем в строку состояния - Идёт остановка серверов
                GV.Log.WriteInfo(GV.SPP2Launcher.UpdateMessageStatusStrip(String.Format(My.Resources.P044_ServerStop, "World...")))

                Threading.Thread.Sleep(500)
                If Not IsNothing(GV.SPP2Launcher.WorldProcess) Then
                    ' Процесс ещё продолжается - Дождитесь окончания...
                    'GV.Log.WriteInfo(GV.SPP2Launcher.UpdateMessageStatusStrip(My.Resources.P039_WaitEnd))
                Else
                    ' Процесс завершился
                    GV.Log.WriteInfo(GV.SPP2Launcher.UpdateMessageStatusStrip(String.Format(My.Resources.P045_ServerStopped, "World")))
                    GV.SPP2Launcher.ChangeIcons(ECaller.world)
                    Try
                        Dim pc = Process.GetProcessById(processID)
                        pc.Kill()
                    Catch
                    End Try
                    Exit Do
                End If
                Threading.Thread.Sleep(500)
            Loop
        End If

        ' Гасим Realmd
        If Not GV.SPP2Launcher.ServersLOCKED Then GV.SPP2Launcher.ShutdownRealmd()
        GV.SPP2Launcher.ChangeIcons(ECaller.realmd)
        GV.SPP2Launcher.UpdateMessageStatusStrip("")

        ' Если необходимо, выполняем BackUp
        If My.Settings.UseAutoBackupDatabase And GV.SPP2Launcher.NeedExitLauncher And Not GV.SPP2Launcher.ServersLOCKED Then
            ' Идёт бэкап БД
            GV.SPP2Launcher.AutoBackups()
            Do
                Threading.Thread.Sleep(500)
                If Not GV.SPP2Launcher.IsBackupStarted Then Exit Do
            Loop
        End If

        ' Гасим любые проявления запуска таймеров
        TimerStartMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerStartWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        'PC.Abort()

        GV.SPP2Launcher.UpdateMessageStatusStrip("")

        Threading.Thread.Sleep(2000)

        If GV.SPP2Launcher.NeedExitLauncher Or otherServers Then

            ' Дождитесь окончания
            GV.SPP2Launcher.UpdateMessageStatusStrip(My.Resources.P039_WaitEnd)

            ' Требуется погасить и шелупонь
            If Not GV.SPP2Launcher.ApacheLOCKED Then
                CheckProcess(EProcess.apache, True)
                ' Ожидание
                Do
                    If (Not CheckProcess(EProcess.apache)) Then Exit Do
                    Threading.Thread.Sleep(500)
                Loop
            End If

            If GV.SPP2Launcher.NeedExitLauncher Then
                ' Проверяем блокировку сервера MySQL
                If Not GV.SPP2Launcher.MySqlLOCKED Then
                    ' Контроль за MySql с последующим выходом из приложения
                    GV.SPP2Launcher.ShutdownMySQL(EProcess.mysqld, EAction.NeedExit)
                Else
                    GV.SPP2Launcher.EnableClosing = True
                    GV.SPP2Launcher.NotifyIcon_SPP2.Visible = False
                    If Not GV.ResetSettings Then
                        Application.Exit()
                    Else
                        IO.File.Delete(SPP2SettingsProvider.SettingsFile)
                        Application.Exit()
                    End If
                End If
            Else
                ' Проверяем блокировку сервера MySQL
                If Not GV.SPP2Launcher.MySqlLOCKED Then
                    ' Контроль за MySql с последующим обновлением параметров меню
                    GV.SPP2Launcher.ShutdownMySQL(EProcess.mysqld, EAction.UpdateMainMenu)
                End If
            End If

        Else
            GV.SPP2Launcher.UpdateMessageStatusStrip("")
        End If

    End Sub

    ''' <summary>
    ''' Поток контролирующий наличие процессов серверов WoW после их запуска.
    ''' </summary>
    Friend Sub Controller()

        Dim _checkMySQL As Date = Date.Now
        Dim _checkHttp As Date = Date.Now
        Dim _checkRealmd As Date = Date.Now
        Dim _checkWorld As Date = Date.Now
        Dim _updateStatus As Date = Date.Now

        ' Ожидаем завершения стартового потока
        Threading.Thread.Sleep(1000)
        Do While GV.SPP2Launcher.StartThreadCompleted = True
            Threading.Thread.Sleep(50)
        Loop

        ' Убивает информацию о стартовом потоке
        GV.SPP2Launcher.StartThreadCompleted = Nothing

        Do
            ' Пауза в пол секунды
            Threading.Thread.Sleep(500)

            If GV.SPP2Launcher.NeedExitLauncher Then Exit Do

            ' Чекаем MySQL каждые 2 секунды
            If Date.Now - _checkMySQL > TimeSpan.FromSeconds(2) Then
                _checkMySQL = Date.Now
                GV.SPP2Launcher.CheckMySQL()
                GC.Collect()
            End If

            ' Чекаем Http каждые 1 секунды
            If Date.Now - _checkHttp > TimeSpan.FromSeconds(1) Then
                _checkHttp = Date.Now
                GV.SPP2Launcher.CheckHttp()
            End If

            ' Чекаем Realmd согласно ChangeRealmdCheck
            If Date.Now - _checkRealmd > TimeSpan.FromSeconds(ChangeRealmdCheck) Then
                _checkRealmd = Date.Now
                GV.SPP2Launcher.CheckRealmd()
            End If

            ' Чекаем World каждые 1.7 секунды
            If Date.Now - _checkWorld > TimeSpan.FromSeconds(1.7) Then
                _checkWorld = Date.Now
                GV.SPP2Launcher.CheckWorld()
            End If

            ' Обновляем инфо в строке состояния каждые 1 сек.
            If Date.Now - _updateStatus > TimeSpan.FromSeconds(1) Then
                _updateStatus = Date.Now
                GV.SPP2Launcher.OutInfoPlayers()
            End If

            ' Проверка наличия процессов серверов
            Dim lp = GetAllProcesses()
            For Each control In BP.Processes

                If GV.SPP2Launcher.NeedExitLauncher Then Exit Do

                If control.WasLaunched Then
                    ' Да, этот процесс уже был однажды запущен
                    Dim processes = lp.FindAll(Function(p) p.ProcessName = control.Name)

                    Dim ok = False
                    For Each existing In processes

                        If GV.SPP2Launcher.NeedExitLauncher Then Exit Do

                        Try
                            Dim path As String = ""

                            Select Case control.Name

                                Case "realmd"
                                    path = My.Settings.CurrentFileRealmd

                                Case "mangosd"
                                    path = My.Settings.CurrentFileWorld

                            End Select

                            If existing.MainModule.FileName = path Then
                                ' всё в порядке, процесс идёт
                                ok = True
                                Exit For
                            End If
                        Catch ex As Exception
                            ' Нет доступа к процессу
                        End Try
                    Next
                    If Not ok Then
                        ' Этот процесс отсутствует
                        Select Case control.Name

                            Case "realmd"
                                GV.SPP2Launcher.RealmdProcess = Nothing
                                control.WasLaunched = False
                                control.CrashCount += 1
                                If Not GV.SPP2Launcher.NeedServerStop Then
                                    ' Сервер рухнул
                                    Dim msg = String.Format(My.Resources.E015_RealmdCrashed, control.CrashCount,
                                                            If(GV.SPP2Launcher.ServerWowAutostart, My.Resources.P055_RestartIs, ""))
                                    GV.Log.WriteError(msg)
                                    GV.SPP2Launcher.UpdateRealmdConsole(msg, Color.Red)
                                    ' Устанавливаем перезапуск (если автостарт или Ручной запуск) через 10 секунд
                                    If GV.SPP2Launcher.ServerWowAutostart Then TimerStartRealmd.Change(10000, 10000)
                                End If

                            Case "mangosd"
                                GV.SPP2Launcher.WorldProcess = Nothing
                                control.WasLaunched = False
                                control.CrashCount += 1
                                If Not GV.SPP2Launcher.NeedServerStop Then
                                    WorldStartTime = 0
                                    GV.SPP2Launcher.UpdateMessageStatusStrip("")
                                    ' Сервер рухнул
                                    Dim msg = String.Format(My.Resources.E016_WorldCrashed, control.CrashCount,
                                                            If(GV.SPP2Launcher.ServerWowAutostart, My.Resources.P055_RestartIs, ""))
                                    GV.Log.WriteError(msg)
                                    GV.SPP2Launcher.UpdateWorldConsole(msg, Color.Red)
                                    ' Устанавливаем перезапуск (если автостарт или Ручной) через 10 секунд
                                    If GV.SPP2Launcher.ServerWowAutostart Then TimerStartWorld.Change(10000, 10000)
                                End If

                        End Select
                    End If
                End If
            Next
        Loop
    End Sub

#End Region

End Module
