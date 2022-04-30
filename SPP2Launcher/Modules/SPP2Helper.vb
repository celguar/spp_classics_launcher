
Imports System.Runtime.InteropServices

Module SPP2Helper

    ''' <summary>
    ''' Семейство шрифтов для консоли.
    ''' </summary>
    Friend F As Drawing.Text.PrivateFontCollection

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

    ''' <summary>
    ''' Создаёт новый кортеж сообщения об ошибке.
    ''' </summary>
    ''' <returns></returns>
    Friend Function NewExitCode() As Tuple(Of Integer, String)
        Return New Tuple(Of Integer, String)(0, "OK")
    End Function

    ''' <summary>
    ''' Загружает шрифт из файла.
    ''' </summary>
    ''' <returns></returns>
    Friend Function LoadFont() As Font
        F = New System.Drawing.Text.PrivateFontCollection()
        F.AddFontFile("C:\Users\RafStudio\Downloads\notomono-regular.ttf")
        Return New Font(F.Families(0), My.Settings.ConsoleFontSize)
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
            fnt = New Font(F.Families(0), My.Settings.ConsoleFontSize, FontStyle.Bold, GraphicsUnit.Pixel)
        Catch ex As Exception
            fnt = New Font("Segoe UI", My.Settings.ConsoleFontSize, FontStyle.Bold, GraphicsUnit.Pixel) ' подставляем системный если не удалось загрузить из Resources
        End Try
        Marshal.FreeHGlobal(ip)
        Return fnt
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

#Region " === ОТПРАВКА СООБЩЕНИЙ В КОНСОЛЬ === "

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlOutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        Threading.Thread.Sleep(100)
        GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль MySQL сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub MySqlErrorDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateMySQLConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdOutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateRealmdConsole(e.Data)
    End Sub

    ''' <summary>
    ''' Отправляет в консоль Realmd сообщение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Friend Sub RealmdErrorDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        GV.SPP2Launcher.UpdateRealmdConsole(e.Data)
    End Sub

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
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
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
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
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
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
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
        If GV.SPP2Launcher.IsShutdown Then Exit Sub
        ' Запускаем Realmd
        GV.SPP2Launcher.StartRealmd(obj)
    End Sub

#End Region

End Module
