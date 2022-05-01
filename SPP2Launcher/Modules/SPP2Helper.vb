
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
    ''' Таймер проверки доступности сервера MySQL.
    ''' </summary>
    Friend TimerCheckMySQL As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера MySQL.
    ''' </summary>
    Friend TimerStartMySQL As Threading.Timer

    ''' <summary>
    ''' Таймер проверки доступности сервера Http.
    ''' </summary>
    Friend TimerCheckApache As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера Apache.
    ''' </summary>
    Friend TimerStartApache As Threading.Timer

    ''' <summary>
    ''' Таймер проверки доступности сервера Realmd.
    ''' </summary>
    Friend TimerCheckRealmd As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера Realmd.
    ''' </summary>
    Friend TimerStartRealmd As Threading.Timer

    ''' <summary>
    ''' Таймер проверки доступности сервера World.
    ''' </summary>
    Friend TimerCheckWorld As Threading.Timer

    ''' <summary>
    ''' Таймер запуска сервера World.
    ''' </summary>
    Friend TimerStartWorld As Threading.Timer

#End Region

#Region " === РАБОТА ТАЙМЕРОВ === "

    ''' <summary>
    ''' Останавливает таймеры проверки состояния серверов.
    ''' </summary>
    Friend Sub StoppingCheckTimers()
        TimerCheckMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckApache.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        TimerCheckRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
    End Sub

    ''' <summary>
    ''' Время проверить наличие подключения к серверу MySQL.
    ''' </summary>
    Friend Sub TimerTik_CheckMySQL(obj As Object)
        GV.SPP2Launcher.CheckMySQL(obj)
    End Sub

    ''' <summary>
    ''' Время запустить сервер MySQL.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartMySQL(obj As Object)
        ' Выключаем таймер
        TimerStartMySQL.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "MySQL"))
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "MySQL"))
        ' Запускаем MySQL
        GV.SPP2Launcher.StartMySQL(obj)
    End Sub

    ''' <summary>
    ''' Время проверить наличие подключения к Web серверу.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_CheckApache(obj As Object)
        GV.SPP2Launcher.CheckApache(obj)
    End Sub

    ''' <summary>
    ''' Время запустить сервер Apache.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartApache(obj As Object)
        ' Выключаем таймер
        TimerStartApache.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "Apache"))
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "Apache"))
        ' Запускаем Apache
        GV.SPP2Launcher.StartApache(obj)
    End Sub

    ''' <summary>
    ''' Время проверить наличие подключения к серверу World.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_CheckWorld(obj As Object)
        GV.SPP2Launcher.CheckWorld(obj)
    End Sub

    ''' <summary>
    ''' Время запустить сервер World.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartWorld(obj As Object)
        ' Выключаем таймер
        TimerStartWorld.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "World"))
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
        GV.Log.WriteInfo(String.Format(My.Resources.P034_LaunchAttempt, "World"))
        ' Запускаем World
        GV.SPP2Launcher.StartWorld(obj)
    End Sub

    ''' <summary>
    ''' Время проверить наличие подключения к серверу Realmd.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_CheckRealmd(obj As Object)
        GV.SPP2Launcher.CheckRealmd(obj)
    End Sub

    ''' <summary>
    ''' Время запустить Realmd.
    ''' </summary>
    ''' <param name="obj"></param>
    Friend Sub TimerTik_StartRealmd(obj As Object)
        ' Выключаем таймер
        TimerStartRealmd.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        GV.Log.WriteInfo(String.Format(My.Resources.P033_TimerTriggered, "Realmd"))
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
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
        Try
            If Not IO.File.Exists(Application.StartupPath & "\" & "notomono-regular.ttf") Then
                IO.File.WriteAllBytes(Application.StartupPath & "\" & "notomono-regular.ttf", My.Resources.notomono_regular)
            End If
            F = New System.Drawing.Text.PrivateFontCollection()
            F.AddFontFile(Application.StartupPath & "\" & "notomono-regular.ttf")
            Return New Font(F.Families(0), My.Settings.ConsoleFontSize)
        Catch
            ' В случае проблем возвращаем хоть что-то...
            Return New Font("Consolas", My.Settings.ConsoleFontSize)
        End Try
    End Function

    ''' <summary>
    ''' Загружает шрифт из ресурсов.
    ''' Только ttf!!!
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

#End Region

#Region " === ОТПРАВКА СООБЩЕНИЙ В КОНСОЛЬ === "

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        Threading.Thread.Sleep(100)
        GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateRealmdConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateRealmdConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль World сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateWorldConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль World сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub WorldErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateWorldConsole(e.Data)
    End Sub

#End Region

#Region " === ПОТОКИ === "

    Friend Sub PreStart()

        ' Добавляем bинформацию по разработчикам
        If IO.File.Exists(My.Settings.DirSPP2 & "\" & SPP2GLOBAL & "\credits.txt") Then
            Dim str = IO.File.ReadAllText(My.Settings.DirSPP2 & "\" & SPP2GLOBAL & "\credits.txt")
            Dim s() = System.Text.RegularExpressions.Regex.Split(str, "(\r\n|\r|\n)",
                      RegularExpressions.RegexOptions.ExplicitCapture)
            For Each line As String In s
                GV.SPP2Launcher.UpdateWorldConsole(line)
                Threading.Thread.Sleep(300)
            Next
        End If

        ' Включаем таймеры проверки серверов
        TimerCheckMySQL.Change(2000, 2000)
        TimerCheckApache.Change(2000, 2000)
        TimerCheckWorld.Change(2000, 2000)
        TimerCheckRealmd.Change(2000, 2000)
        TimerCheckWorld.Change(2000, 2000)

        ' Если автозапуск MySQL сервера
        If My.Settings.UseIntMySQL And My.Settings.MySqlAutostart Then
            TimerStartMySQL.Change(500, 500)
        End If

    End Sub

    ''' <summary>
    ''' Поток контролирующий наличие процессов серверов WoW после их запуска.
    ''' </summary>
    Friend Sub Controller()

        ' Ожидаем завершения стартового потока
        Threading.Thread.Sleep(1000)
        Do While GV.SPP2Launcher.StartThreadCompleted = True
            Threading.Thread.Sleep(50)
        Loop

        GV.SPP2Launcher.StartThreadCompleted = Nothing
        GV.SPP2Launcher.UpdateRealmdConsole(My.Resources.P019_ControlEnabled & vbCrLf)
        GV.SPP2Launcher.UpdateWorldConsole(vbCrLf & My.Resources.P019_ControlEnabled & vbCrLf)

        ' Автостарт серверов WoW
        Select Case My.Settings.LastLoadedServerType
            Case GV.EModule.Classic.ToString
                TimerStartRealmd.Change(2000, 2000)
                TimerStartWorld.Change(1000, 1000)
            Case GV.EModule.Tbc.ToString
                TimerStartRealmd.Change(2000, 2000)
                TimerStartWorld.Change(1000, 1000)
            Case GV.EModule.Wotlk.ToString
                TimerStartRealmd.Change(2000, 2000)
                TimerStartWorld.Change(1000, 1000)
        End Select

        Do
            Threading.Thread.Sleep(500)
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
                                If Not GV.SPP2Launcher.NeedServerStop Then
                                    ' Сервер рухнул
                                    GV.SPP2Launcher.UpdateRealmdConsole(vbCrLf & My.Resources.E015_RealmdCrashed & vbCrLf)
                                    ' Устанавливаем перезапуск (если автостарт) через 10 секунд
                                    If GV.SPP2Launcher.NeedServerStart Then TimerStartRealmd.Change(10000, 10000)
                                End If

                            Case "mangosd"
                                GV.SPP2Launcher.WorldProcess = Nothing
                                control.WasLaunched = False
                                If Not GV.SPP2Launcher.NeedServerStop Then
                                    ' Сервер рухнул
                                    GV.SPP2Launcher.UpdateWorldConsole(vbCrLf & My.Resources.E016_WorldCrashed & vbCrLf)
                                    ' Устанавливаем перезапуск (если автостарт) через 10 секунд
                                    If GV.SPP2Launcher.NeedServerStart Then TimerStartWorld.Change(10000, 10000)
                                End If

                        End Select
                    End If
                End If
            Next
        Loop
    End Sub

#End Region

End Module
