
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text

Module SPP2Helper

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

#Region " === КОНСТАНТЫ === "

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

#End Region

#Region " === ТАЙМЕРЫ === "

    ''' <summary>
    ''' Таймер запуска сервера MySQL.
    ''' </summary>
    Friend TimerStartMySQL As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера Apache.
    ''' </summary>
    Friend TimerStartApache As Threading.Timer

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
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "MySQL"))
        ' Запускаем MySQL
        GV.SPP2Launcher.StartMySQL(obj)
    End Sub

    ''' <summary>
    ''' Время запустить сервер Apache.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartApache(obj As Object)
        Threading.Thread.CurrentThread.CurrentUICulture = GV.CI
        Threading.Thread.CurrentThread.CurrentCulture = GV.CI
        ' Выключаем таймер
        TimerStartApache.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "Apache"))
        If GV.SPP2Launcher.NeedExitLauncher Then Exit Sub
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "Apache"))
        ' Запускаем Apache
        GV.SPP2Launcher.StartApache(obj)
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
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "World"))
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
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "Realmd"))
        ' Запускаем Realmd
        GV.SPP2Launcher.StartRealmd(obj)
    End Sub

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
    ''' <returns>Список IpV4 адаптеров в системе.</returns>
    Friend Function GetLocalIpAddresses() As List(Of String)
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
        addresses.Add("ANY")

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

#End Region

#Region " === ЗАГРУЗКА ШРИФТОВ === "

    ''' <summary>
    ''' Загружает шрифт из файла, предварительно выгрузив на диск...
    ''' </summary>
    ''' <returns></returns>
    Friend Function LoadFont() As Font
        Dim program = Application.StartupPath
        Try
            If Not IO.File.Exists(program & "\" & "notomono-regular.ttf") Then
                IO.File.WriteAllBytes(program & "\" & "notomono-regular.ttf", My.Resources.notomono_regular)
            End If
        Catch
            ' Указанный каталог недоступен - сохраняем параметры в LocalApplicationData
            program = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\" & Assembly.GetExecutingAssembly.FullName.Split(","c)(0)
            If Not IO.Directory.Exists(program) Then IO.Directory.CreateDirectory(program)
            IO.File.WriteAllBytes(program & "\" & "notomono-regular.ttf", My.Resources.notomono_regular)
        End Try
        F = New System.Drawing.Text.PrivateFontCollection()
        F.AddFontFile(program & "\" & "notomono-regular.ttf")
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

#Region " === ОТПРАВКА СООБЩЕНИЙ В КОНСОЛЬ === "

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
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
            If My.Settings.ConsoleMessageFilter < 1 Then
                GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
            Else
                If Not e.Data.Contains("ERROR") Then
                    GV.SPP2Launcher.UpdateWorldConsole(e.Data, My.Settings.WorldConsoleForeColor)
                End If
            End If
            If e.Data.Contains("SERVER STARTUP TIME") Then
                WorldStartTime = Date.Now.ToOADate()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Отправляет в консоль World сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not IsNothing(e.Data) Then
            If My.Settings.ConsoleMessageFilter < 1 Then
                GV.SPP2Launcher.UpdateWorldConsole(e.Data, Color.Red)
            End If
        End If
    End Sub

#End Region

#Region " === ПОТОКИ === "

    ''' <summary>
    ''' Тик на каждую секунду.
    ''' </summary>
    Friend Sub EverySecond()
        Do
            Threading.Thread.Sleep(1000)
            If WorldStartTime > 0 Then
                GV.SPP2Launcher.OutMessageStatusStrip(String.Format("Uptime: {0:dd\.hh\:mm\:ss} ", Date.Now - Date.FromOADate(WorldStartTime)))
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Выводит в консоль World информацию о разработчиках.
    ''' </summary>
    Friend Sub PreStart()

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
        If processID > 0 Then
            Do
                ' Пишем в лог - Идёт остановка серверов
                GV.Log.WriteInfo(My.Resources.P038_StoppingWorld)
                GV.SPP2Launcher.OutMessageStatusStrip(My.Resources.P038_StoppingWorld)
                Threading.Thread.Sleep(1000)
                If Not IsNothing(GV.SPP2Launcher.WorldProcess) Then
                    ' Процесс ещё продолжается - Дождитесь окончания...
                    GV.Log.WriteInfo(My.Resources.P039_WaitEnd)
                    GV.SPP2Launcher.OutMessageStatusStrip(My.Resources.P039_WaitEnd)
                Else
                    ' Процесс завершился
                    GV.Log.WriteInfo("Shutdown is OK!")
                    GV.SPP2Launcher.WorldProcess = Nothing
                    Try
                        Dim pc = Process.GetProcessById(processID)
                        pc.Kill()
                    Catch
                    End Try
                    Exit Do
                End If
            Loop
        End If

        GV.SPP2Launcher.OutMessageStatusStrip("")
        WorldStartTime = Nothing

        ' Гасим Realmd
        GV.SPP2Launcher.ShutdownRealmd()

        ' Очищаем контроллер автозапуска серверов WoW
        BP.ProcessesAreStopped()

        ' Если идёт процесс BackUp то ждём завершения
        Do While BackupProcess
            Threading.Thread.Sleep(200)
        Loop

        If GV.SPP2Launcher.NeedExitLauncher Or otherServers Then
            ' Надо погасить и прочие серверы
            GV.SPP2Launcher.ShutdownApache()
            GV.SPP2Launcher.ShutdownMySQL()
        End If

        ' Проверяем требование о выxоде из приложения
        If GV.SPP2Launcher.NeedExitLauncher Then
            ' Дожидаемся остановки всех серверов
            Do
                If Not GV.SPP2Launcher.MySqlON AndAlso Not GV.SPP2Launcher.RealmdON Then Exit Do
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
                Application.Exit()
            Else
                Application.Exit()
            End If

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

            ' Чекаем MySQL каждые 2 секунды
            If Date.Now - _checkMySQL > TimeSpan.FromSeconds(2) Then
                _checkMySQL = Date.Now
                GV.SPP2Launcher.CheckMySQL()
                GC.Collect()
            End If

            ' Чекаем Http каждые 2 секунды
            If Date.Now - _checkHttp > TimeSpan.FromSeconds(2) Then
                _checkHttp = Date.Now
                GV.SPP2Launcher.CheckHttp()
            End If

            ' Чекаем Realmd каждые 2 секунды
            If Date.Now - _checkRealmd > TimeSpan.FromSeconds(2) Then
                _checkRealmd = Date.Now
                GV.SPP2Launcher.CheckRealmd()
            End If

            ' Чекаем World каждые 2 секунды
            If Date.Now - _checkWorld > TimeSpan.FromSeconds(2) Then
                _checkWorld = Date.Now
                GV.SPP2Launcher.CheckWorld()
            End If

            ' Обновляем инфо в строке состояния каждые 2 сек.
            If Date.Now - _updateStatus > TimeSpan.FromSeconds(2) Then
                _updateStatus = Date.Now
                GV.SPP2Launcher.OutInfoStatusStrip()
            End If

            ' Проверка наличия процессов серверов
            Dim lp = GetAllProcesses()
            For Each control In BP.Processes
                If control.WasLaunched Then
                    ' Да, этот процесс уже был однажды запущен
                    Dim processes = lp.FindAll(Function(p) p.ProcessName = control.Name)

                    Dim ok = False
                    For Each existing In processes
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
                                    Dim msg = String.Format(My.Resources.E015_RealmdCrashed, control.CrashCount, "10")
                                    GV.Log.WriteError(msg)
                                    GV.SPP2Launcher.UpdateRealmdConsole(vbCrLf & msg & vbCrLf, Color.Red)
                                    ' Устанавливаем перезапуск (если автостарт или Ручной запуск) через 10 секунд
                                    If GV.SPP2Launcher.ServerWowAutostart Then TimerStartRealmd.Change(10000, 10000)
                                End If

                            Case "mangosd"
                                GV.SPP2Launcher.WorldProcess = Nothing
                                control.WasLaunched = False
                                control.CrashCount += 1
                                If Not GV.SPP2Launcher.NeedServerStop Then
                                    WorldStartTime = 0
                                    GV.SPP2Launcher.OutMessageStatusStrip("")
                                    ' Сервер рухнул
                                    Dim msg = String.Format(My.Resources.E016_WorldCrashed, control.CrashCount, "10")
                                    GV.Log.WriteError(msg)
                                    GV.SPP2Launcher.UpdateWorldConsole(vbCrLf & msg & vbCrLf, Color.Red)
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
