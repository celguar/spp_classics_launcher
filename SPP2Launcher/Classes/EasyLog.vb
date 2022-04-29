
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Xml

Public Class EasyLog

#Region " === ПЕРЕЧИСЛЕНИЯ === "

    ''' <summary>
    ''' Уровень серёзности логирования
    ''' </summary>
    Public Enum Severity

        ''' <summary>
        ''' Регистрировать все события и выше
        ''' </summary>
        All

        ''' <summary>
        ''' Регистрировать информационные сообщения и выше
        ''' </summary>
        Info

        ''' <summary>
        ''' Регистрировать SQL команды и выше
        ''' </summary>
        SQL

        ''' <summary>
        ''' Регистрировать предупреждения и выше
        ''' </summary>
        Warning

        ''' <summary>
        ''' Регистрировать ошибки и выше
        ''' </summary>
        [Error]

        ''' <summary>
        ''' Регистрировать только исключения
        ''' </summary>
        Exception

    End Enum

#End Region

#Region " === ПЕРЕМЕННЫЕ === "

    ''' <summary>
    ''' Каталог для вывода журналов сообщений. По умолчанию это текущий рабочий каталог приложения.
    ''' </summary>
    Private _logDir As New DirectoryInfo(Directory.GetCurrentDirectory())

    ''' <summary>
    ''' Префикс для использования в имени файла. По умолчанию префикс отсутствует.
    ''' </summary>
    Private _prefix As String

    ''' <summary>
    ''' Формат даты для использования в имени файла. По умолчанию равно «yyyy_MM_dd»
    ''' </summary>
    Private _dateFormat As String

    ''' <summary>
    ''' Суффикс для использования в имени файла. По умолчанию суффикс отсутствует.
    ''' </summary>
    Private _suffix As String

    ''' <summary>
    ''' Расширение используемое в имени журнала событий. По умолчанию равно "xlog".
    ''' </summary>
    Private _extension As String = Nothing

    ''' <summary>
    ''' Управляет форматом записи журнала событий xml или txt.
    ''' </summary>
    Private _writeText As Boolean

    ''' <summary>
    ''' Уровень регистрируемых событий. По умолчанию равно Severity.Info
    ''' </summary>
    Private _logLevel As Severity = Severity.Info

    ''' <summary>
    ''' Фоновая задача для записи записей журнала на диск.
    ''' </summary>
    Private _backgroundTask As Task

    ''' <summary>
    ''' Синхронизация для ядра фоновой задачи.
    ''' </summary>
    Private ReadOnly _backgroundTaskSyncRoot As New Object()

    ''' <summary>
    ''' Синхронизация для ядра файла журнала событий.
    ''' </summary>
    Private ReadOnly _logFileSyncRoot As New Object()

    ''' <summary>
    ''' Строка-разделитель атрибутов в сообщении для текстового файла журнала событий.
    ''' </summary>
    Private _textSeparator As String = " | "

    ''' <summary>
    ''' Очередь записи журнала для блокировки при одновременном обращении из потоков.
    ''' </summary>
    Private ReadOnly _logEntryQueue As New Queue(Of XElement)()

    ''' <summary>
    ''' Флаг постановки новых записей в очередь. True - добавление в очередь запрещено.
    ''' </summary>
    Private _stopWriteMessages As Boolean = False

    ''' <summary>
    ''' Флаг остановки фоновой задачи обработки сообщений. True - фоновая задача остановлена.
    ''' </summary>
    Private _stopLoggingRequested As Boolean = False

    ''' <summary>
    ''' Последнее исключение, которое произошло в фоновом режиме при попытке записи в файл журнала событий.
    ''' </summary>
    Private _lastExceptionInBackgroundTask As Exception = Nothing

#End Region

#Region " === ПУБЛИЧНЫЕ СВОЙСТВА === "

    ''' <summary>
    ''' Каталог для журнала событий. По умолчанию это текущий рабочий каталог приложения.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LogDir As String
        Get
            Return _logDir.FullName
        End Get
    End Property

    ''' <summary>
    ''' Максимальный размер файла.
    ''' </summary>
    ''' <returns></returns>
    Public Property MaxLogSize As Integer

    ''' <summary>
    ''' Формат записей в журнале событий. По умолчанию равен True, что определяет ведение расширенной детализации событий, с такими атрибутами как Severity, Source и т.д.
    ''' Значение False упрощает записи исключая выше описанные атрибуты, уменьшая таким образом общий размер файла журнала событий.
    ''' </summary>
    ''' <returns></returns>
    Public Property ExtendedLog As Boolean = True

    ''' <summary>
    ''' Префикс для использования в имени файла журнала событий. По умолчанию префикс не назначен.
    ''' </summary>
    ''' <returns></returns>
    Public Property Prefix As String
        Get
            Return If(_prefix, String.Empty)
        End Get
        Set(ByVal value As String)
            _prefix = value
        End Set
    End Property

    ''' <summary>
    ''' Суффикс для использования в имени файла журнала событий. По умолчанию суффикс не назначен.
    ''' </summary>
    ''' <returns></returns>
    Public Property Suffix As String
        Get
            Return If(_suffix, String.Empty)
        End Get
        Set(ByVal value As String)
            _suffix = value
        End Set
    End Property

    ''' <summary>
    ''' Расширение для использования в имени файла журнала событий. По умолчанию это "log".
    ''' </summary>
    ''' <returns></returns>
    Public Property Extension As String
        Get
            Return If(_extension, "xlog")
        End Get
        Set(ByVal value As String)
            _extension = value
        End Set
    End Property

    ''' <summary>
    ''' Формат даты для использования в имени файла журнала событий.
    ''' По умолчанию установлено значение "yyyy_MM_dd" (например, 2013_04_21), что приводит к ежедневному изменению файла журнала.
    ''' Установите в Nothing, чтобы вернуться к значению по умолчанию.
    ''' Установите например в "yyyy_MM_dd_HH" для изменения файла журнала каждый час.
    ''' </summary>
    ''' <returns></returns>
    Public Property DateFormat As String
        Get
            Return If(_dateFormat, "yyyy_MM_dd")
        End Get
        Set(ByVal value As String)
            _dateFormat = value
        End Set
    End Property

    ''' <summary>
    ''' Уровень фиксирования событий согласно установленного уровня. Записывает события согласно установленного в параметре уровня и выше.
    ''' Например параметр Severity.Info предполагает журналирование абсолютно всех событий, а параметр Severity.Error события ошибок и исключений.
    ''' Другими словами параметр отсекает от записи в файл журнала события с уровнем ниже указанной серьезности.
    ''' </summary>
    ''' <returns></returns>
    Public Property LogLevel As Severity
        Get
            Return _logLevel
        End Get
        Set(ByVal value As Severity)
            _logLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Управляет форматом записи в журнал событий. По умолчанию установлено значение False, таким образом запись будет вестись в формате xml файла.
    ''' </summary>
    ''' <returns></returns>
    Public Property WriteText As Boolean
        Get
            Return _writeText
        End Get
        Set(value As Boolean)
            If IsNothing(_extension) Then
                _extension = If(value = True, "log", "xlog")
            End If
            _writeText = value
        End Set
    End Property

    ''' <summary>
    ''' Если параметр "WriteText" имеет значение True, то данный параметр будет разделять атрибуты полей в текстовом файле.
    ''' По умолчанию равен " | ".
    ''' </summary>
    ''' <returns></returns>
    Public Property TextSeparator As String
        Get
            Return _textSeparator
        End Get
        Set(ByVal value As String)
            _textSeparator = If(value, String.Empty)
        End Set
    End Property

    ''' <summary>
    ''' Текущее имя файла журнала событий.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FileName As String
        Get
            Return GetFileName(DateTime.Now)
        End Get
    End Property

    ''' <summary>
    ''' Последнее исключение, которое произошло в фоновом режиме при попытке записи в файл журнала событий.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LastExceptionInBackgroundTask As Exception
        Get
            Return _lastExceptionInBackgroundTask
        End Get
    End Property

    ''' <summary>
    ''' Количество сообщений ожидающих записи в файл журнала событий. По достижении 1000 можно предположить, что существует
    ''' системная проблема записи сообщений в файл и имеет смысл проверить параметр LastExceptionInBackgroundTask.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NumberOfLogEntriesWaitingToBeWrittenToFile As Integer
        Get
            Return _logEntryQueue.Count
        End Get
    End Property

    ''' <summary>
    ''' Возвращает состояние фоновой задачи записи сообщений в файл.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property StateBackgroundTask As Boolean
        Get
            Return _backgroundTask IsNot Nothing
        End Get
    End Property

#End Region

#Region " === КОНСТРУКТОРЫ ИНИЦИАЛИЗАЦИИ === "

    ''' <summary>
    ''' Статический конструктор инициализации.
    ''' </summary>
    Sub New()
        ' Подключение к основному процессу вплоть до его завершения
        AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf CurrentDomainProcessExit
        ' Устанавливаем локализацию
        SetLanguage(Nothing)
        ' Запускаем фоновый процесс
        StartBackgroundTask()
    End Sub

    ''' <summary>
    ''' Инициализация параметров журнала событий.
    ''' По итогу инициализации имя файла будет выглядеть как LogDir + Prefix + dateTime.ToString(DateFormat) + Suffix + Extension
    ''' </summary>
    ''' <param name="_err">Возвращает исключение если таковое имело место быть, иначе Nothing.</param>
    ''' <param name="logDir">Необязательный параметр: Директория сохранения журналов событий. Если она отсутствует, то будет создана.</param>
    ''' <param name="prefix">Необязательный параметр: Префикс для имени файла журнала событий. По умолчанию без префикса.</param>
    ''' <param name="suffix">Необязательный параметр: Суффикс для имени файла журнала событий. По умолчанию без суффикса.</param>
    ''' <param name="extension">Необязательный параметр: Расширение для имени файла журнала событий.По умолчанию установлен "log".</param>
    ''' <param name="dateFormat">Необязательный параметр: Формат даты для имени файла журнала событий. По умолчанию установлено значение "yyyy_MM_dd".</param>
    ''' <param name="logLevel">Необязательный параметр: Уровень серёзности фиксируемых событий. По умолчанию установлен Severity.Info - все сообщения.</param>
    ''' <param name="startExplicitly">Необязательный параметр: Указывает на необходимость явного включения логирования командой StartLogging.
    ''' Параметр будет учтён только при запуске из данного конструктора инициализации. По умолчанию False.</param>
    ''' <param name="check">Необязательный параметр: Флаг выполнения проверки параметров перед запуском фонового процесса методом тестовой записи в журнал событий.</param>
    ''' <param name="writeAsText">Необязательный параметр: Флаг сохранения журнала событий в формате txt. По умолчанию True, что соответствует записи в текстовом формате.</param>
    ''' <param name="textSeparator">Необязательный параметр: Строка разделитель атрибутов при сохранении журнала событий в формате txt. По умолчанию " | ".</param>
    ''' <param name="cleanAtStartup">Необязательный параметр: При инициализации удаляет старый файл журнала событий. По умолчанию True.
    ''' Параметр будет учтён только при запуске из данного конструктора инициализации. По умолчанию False.</param>
    ''' <param name="extendedLog">Необязательный параметр: Формат хранения записей событий. По умолчанию False, то есть без расширенных атрибутов.</param>
    ''' <param name="maxLogSize">Необязательный параметр: Максимальный размер файла журнала событий. При превышении создаётся новый. По умолчанию = 10000 байт.</param>
    ''' <param name="language">Необязательный параметр: Язык локализации сообщений используемых в EasyLog. Системные сообщения не изменяются!</param>
    Public Sub New(ByRef _err As Tuple(Of Integer, String),
                   Optional ByVal logDir As String = Nothing,
                   Optional ByVal prefix As String = Nothing,
                   Optional ByVal suffix As String = Nothing,
                   Optional ByVal extension As String = Nothing,
                   Optional ByVal dateFormat As String = Nothing,
                   Optional ByVal logLevel? As Severity = Nothing,
                   Optional ByVal startExplicitly? As Boolean = Nothing,
                   Optional ByVal check As Boolean = True,
                   Optional ByVal writeAsText? As Boolean = True,
                   Optional ByVal textSeparator As String = Nothing,
                   Optional ByVal cleanAtStartup? As Boolean = True,
                   Optional ByVal extendedLog As Boolean = False,
                   Optional ByVal maxLogSize As Integer = 10000,
                   Optional ByVal language As String = "ru-RU")

        Try

            Me.MaxLogSize = maxLogSize

            ' Формат записи в журнал сообщений
            Me.ExtendedLog = extendedLog

            ' Сохраняем формат хранения журнала сообщений
            If writeAsText IsNot Nothing Then Me.WriteText = writeAsText.Value

            ' Сохраняем разделитель атрибутов
            If textSeparator IsNot Nothing Then Me.TextSeparator = textSeparator

            ' Устанавливаем уровень логирования
            If logLevel IsNot Nothing Then Me.LogLevel = logLevel.Value

            ' Сохраняем расширение
            If extension IsNot Nothing Then Me.Extension = extension

            ' Сохраняем суффикс имени
            If suffix IsNot Nothing Then Me.Suffix = suffix

            ' Сохраняем формат даты
            If dateFormat IsNot Nothing Then Me.DateFormat = dateFormat

            ' Сохраняем префикс имени
            If prefix IsNot Nothing Then Me.Prefix = prefix

            ' Сохраняем тип запуска
            If startExplicitly IsNot Nothing Then _stopWriteMessages = startExplicitly.Value

            ' Создаём указанную директорию
            If logDir IsNot Nothing Then SetLogDir(logDir, True)

            ' Удаляем старые файлы
            If cleanAtStartup IsNot Nothing AndAlso cleanAtStartup.Value Then
                'File.Delete(GetFileName(DateTime.Now))
                For Each file In IO.Directory.GetFiles(Me.LogDir, Me.Prefix & "*" & Me.Suffix & "." & Me.Extension)
                    IO.File.Delete(file)
                Next
            End If

            ' Устанавливаем локализацию
            SetLanguage(language)

            ' Проверка работоспособности указанных параметров
            If check Then
                Dim ex = CheckOptions()
                If Not IsNothing(ex) Then Throw New Exception(ex.Message)
            End If

            ' Запускаем фоновый процесс
            StartBackgroundTask()

            WriteTestMessage(_lng.log0009 & " " & logLevel)

        Catch ex As Exception
            _err = New Tuple(Of Integer, String)(1, ex.Message)
        End Try

    End Sub

#End Region

#Region " === ПУБЛИЧНЫЕ МЕТОДЫ === "

    ''' <summary>
    ''' Устанавливает новый каталог для сохранения журналов событий.
    ''' </summary>
    ''' <param name="logDir">Каталог для сохранения журналов событий. При передаче пустой строки используется текущий рабочий каталог.</param>
    ''' <param name="createIfNotExisting">Флаг создания каталога. По умолчанию False, т.е. каталог создаваться не будет.</param>
    ''' <returns>Возвращает исключение, если таковое имело место быть.</returns>
    Public Function SetLogDir(ByVal logDir As String, Optional ByVal createIfNotExisting As Boolean = False) As Exception
        If String.IsNullOrEmpty(logDir) Then
            logDir = Directory.GetCurrentDirectory()
        End If

        Try
            _logDir = New DirectoryInfo(logDir)

            If Not _logDir.Exists Then
                If createIfNotExisting Then
                    _logDir.Create()
                Else
                    Throw New DirectoryNotFoundException(String.Format(_lng.log0001 & _lng.log0100, _logDir.FullName))
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Return ex
        End Try
    End Function

    ''' <summary>
    ''' Проверка возможности записи в файл.
    ''' </summary>
    ''' <param name="message">Тестовое сообщение для записи в файл.</param>
    Public Function CheckOptions(Optional ByVal message As String = "") As Exception
        If message = "" Then message = _lng.log0103
        ' Попытка прямой записи в файл для проверки возможности
        Return WriteTestMessage(message)
    End Function

    ''' <summary>
    ''' Запускает фоновый процесс отвечающий за обработку событий в очереди.
    ''' </summary>
    Public Sub StartBackgroundTask()
        ' Проверка на случай, если задача уже и так выполняется
        If _backgroundTask IsNot Nothing OrElse _stopLoggingRequested Then
            Return
        End If

        ' Сброс флага остановки
        _stopLoggingRequested = False

        SyncLock _backgroundTaskSyncRoot
            If _backgroundTask IsNot Nothing Then
                Return
            End If

            ' Сброс последнего исключения
            _lastExceptionInBackgroundTask = Nothing

            ' Создание и запуск задачи
            _backgroundTask = New Task(AddressOf WriteLogEntriesToFile, TaskCreationOptions.LongRunning)
            _backgroundTask.Start()
        End SyncLock
    End Sub

    ''' <summary>
    ''' Возобновляет запись в журнал событий после команды StopLogging
    ''' </summary>
    Public Sub StartLogging()
        _stopWriteMessages = False
    End Sub

    ''' <summary>
    ''' Прекращает запись в журнал событий включая прекращение постановки задач в очередь.
    ''' </summary>
    Public Sub StopLogging()
        _stopWriteMessages = True
    End Sub

    ''' <summary>
    ''' Записывает в журнал событий абсолютно все сообщения. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="message">Текст сообщения для записи в журнал событий.</param>
    ''' <param name="useBackgroundTask">Необязательный параметр. Определяет использование фоновой задачи для записи сообщений на диск.
    ''' По умолчанию = True. Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    Public Function WriteAll(ByVal message As String, Optional ByVal useBackgroundTask As Boolean = True) As Exception
        Return WriteMessage(message, Severity.All, useBackgroundTask)
    End Function

    ''' <summary>
    ''' Записывает в журнал событий информационные сообщения и выше. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="message">Текст сообщения для записи в журнал событий.</param>
    ''' <param name="useBackgroundTask">Необязательный параметр. Определяет использование фоновой задачи для записи сообщений на диск.
    ''' По умолчанию = True. Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    Public Function WriteInfo(ByVal message As String, Optional ByVal useBackgroundTask As Boolean = True) As Exception
        Return WriteMessage(message, Severity.Info, useBackgroundTask)
    End Function

    ''' <summary>
    ''' Записывает в журнал имена запросов/ответов и выше. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="useBackgroundTask"></param>
    ''' <returns></returns>
    Public Function WriteSQL(ByVal message As String, Optional ByVal useBackgroundTask As Boolean = True) As Exception
        Return WriteMessage(message, Severity.SQL, useBackgroundTask)
    End Function

    ''' <summary>
    ''' Записывает в журнал событий сообщение с уровнем серьёзности ПРЕДУПРЕЖДЕНИЕ. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="message">Текст сообщения для записи в журнал событий.</param>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    Public Function WriteWarning(ByVal message As String, Optional ByVal useBackgroundTask As Boolean = True) As Exception
        Return WriteMessage(message, Severity.Warning, useBackgroundTask)
    End Function

    ''' <summary>
    ''' Записывает в журнал событий сообщение с уровнем серьёзности ОШИБКА. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    Public Function WriteError(ByVal message As String, Optional ByVal useBackgroundTask As Boolean = True) As Exception
        Return WriteMessage(message, Severity.Error, useBackgroundTask)
    End Function

    ''' <summary>
    ''' Записывает в журнал событий ИСКЛЮЧЕНИЕ. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="ex">Исключение для записи в журнал событий.</param>
    ''' <param name="message">Поясняющее сообщение для записи в журнал событий.</param>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    ''' <param name="framesToSkip">Сколько кадров пропустить при обнаружении вызывающего метода "GetCaller".
    ''' Это полезно, когда вызовы журнала для "EasyLog" помещаются в приложение. По умолчанию = 0.</param>
    Public Function WriteException(ByVal ex As Exception,
                                   Optional ByVal message As String = Nothing,
                                   Optional ByVal useBackgroundTask As Boolean = True,
                                   Optional ByVal framesToSkip As Integer = 0) As Exception
        If ex Is Nothing Then Return Nothing
        If Not IsNothing(message) Then WriteMessage(message, Severity.Exception, useBackgroundTask)
        Return WriteXElement(GetExceptionXElement(ex), True, Severity.Exception, useBackgroundTask, framesToSkip)
    End Function

    ''' <summary>
    ''' Записывает в журнал событий ИСКЛЮЧЕНИЕ. В случае ошибки возвращает Exception.
    ''' </summary>
    ''' <param name="message">Поясняющее сообщение для записи в журнал событий.</param>
    ''' <param name="ex">Исключение для записи в журнал событий.</param>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    ''' <param name="framesToSkip">Сколько кадров пропустить при обнаружении вызывающего метода "GetCaller".
    ''' Это полезно, когда вызовы журнала для "EasyLog" помещаются в приложение. По умолчанию = 0.</param>
    ''' <returns></returns>
    Public Function WriteException(ByVal message As String,
                                   Optional ByVal ex As Exception = Nothing,
                                   Optional ByVal useBackgroundTask As Boolean = True,
                                   Optional ByVal framesToSkip As Integer = 0) As Exception
        If message Is Nothing AndAlso ex Is Nothing Then Return Nothing
        If Not IsNothing(message) Then WriteMessage(message, Severity.Exception, useBackgroundTask)
        If Not IsNothing(ex) Then
            Return WriteXElement(GetExceptionXElement(ex), True, Severity.Exception, useBackgroundTask, framesToSkip)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Записывает в директорию LogDir в файл fileName содержимое байтового буфера.
    ''' </summary>
    ''' <param name="buff">Буфер для записи в файл.</param>
    ''' <param name="fileName">Имя файла для записи.</param>
    ''' <returns>Возвращает исключение.</returns>
    Public Function WriteBytes(ByVal buff() As Byte, ByVal fileName As String) As Exception
        Try
            If Not Directory.Exists(LogDir & "\XML") Then Directory.CreateDirectory(LogDir & "\XML")
            Dim fs As FileStream = File.Create(LogDir & "\XML\" & fileName)
            fs.Write(buff, 0, buff.Length - 1)
            fs.Close()
            fs.Dispose()
            Return Nothing
        Catch ex As Exception
            Return ex
        End Try
    End Function

    ''' <summary>
    ''' Получает строку XML с подробной информацией об исключении.
    ''' </summary>
    ''' <remarks>
    ''' Рекурсивно добавляет элементы для внутренних исключений. Для самого внутреннего исключения добавляется трассировка стека.
    ''' Распознаёт тэги для "Exception.Data", "SqlException", "COMException", "AggregateException".
    ''' </remarks>
    ''' <param name="ex">Исключение возникшее при разборе XML строки.</param>
    ''' <returns>Возвращает строку с информацией из исключения.</returns>
    Public Function GetExceptionAsXmlString(ByVal ex As Exception) As String
        Dim xElement As XElement = GetExceptionXElement(ex)
        Return If(xElement Is Nothing, String.Empty, xElement.ToString())
    End Function

    ''' <summary>
    ''' Получает XElement для исключения
    ''' </summary>
    ''' <remarks>
    ''' Рекурсивно добавляет элементы для внутренних исключений. Для самого внутреннего исключения добавляется трассировка стека.
    ''' Распознаёт тэги для "Exception.Data", "SqlException", "COMException", "AggregateException".
    ''' </remarks>
    ''' <param name="ex">Исключение возникшее при получении XElement.</param>
    ''' <returns>Возвращает строку с информацией из исключения.</returns>
    Public Function GetExceptionXElement(ByVal ex As Exception) As XElement
        If ex Is Nothing Then Return Nothing

        Dim xElement = New XElement("Exception")
        xElement.Add(New XAttribute("Type", ex.GetType().FullName))

        Dim s = If(IsNothing(ex.TargetSite) OrElse IsNothing(ex.TargetSite.DeclaringType),
            If(IsNothing(ex.Source), "PLUGIN", ex.Source), String.Format("{0}.{1}", ex.TargetSite.DeclaringType.FullName, ex.TargetSite.Name))
        xElement.Add(New XAttribute("Source", s))

        xElement.Add(New XElement("Message", ex.Message))

        If ex.Data.Count > 0 Then
            Dim xDataElement = New XElement("Data")

            For Each de As DictionaryEntry In ex.Data
                xDataElement.Add(New XElement("Entry", New XAttribute("Key", de.Key), New XAttribute("Value", If(de.Value, String.Empty))))
            Next de

            xElement.Add(xDataElement)
        End If

        If TypeOf ex Is SqlException Then
            Dim sqlEx = CType(ex, SqlException)
            Dim xSqlElement = New XElement("SqlException")
            xSqlElement.Add(New XAttribute("ErrorNumber", sqlEx.Number))

            If Not String.IsNullOrEmpty(sqlEx.Server) Then
                xSqlElement.Add(New XAttribute("ServerName", sqlEx.Server))
            End If

            If Not String.IsNullOrEmpty(sqlEx.Procedure) Then
                xSqlElement.Add(New XAttribute("Procedure", sqlEx.Procedure))
            End If

            xElement.Add(xSqlElement)
        End If

        If TypeOf ex Is COMException Then
            Dim comEx = CType(ex, COMException)
            Dim xComElement = New XElement("ComException")
            Try
                xComElement.Add(New XAttribute("ErrorCode", String.Format("0x{0:X8}", CUInt(comEx.ErrorCode))))
            Catch
            Finally
                xComElement.Add(New XAttribute("ErrorCode", String.Format("0x{0:X8}", CInt(comEx.ErrorCode))))
            End Try
            xElement.Add(xComElement)
        End If

        If TypeOf ex Is AggregateException Then
            Dim xAggElement = New XElement("AggregateException")
            For Each innerEx As Exception In (CType(ex, AggregateException)).InnerExceptions
                xAggElement.Add(GetExceptionXElement(innerEx))
            Next innerEx
            xElement.Add(xAggElement)
        End If

        xElement.Add(If(ex.InnerException Is Nothing, New XElement("StackTrace", ex.StackTrace), GetExceptionXElement(ex.InnerException)))

        Return xElement
    End Function

    ''' <summary>
    ''' Получает имя файла журнала для указанной даты.
    ''' </summary>
    ''' <param name="dateTime">Дата файла журнала событий для получения.</param>
    ''' <returns>Имя файла журнала событий для указанной даты.</returns>
    Public Function GetFileName(ByVal dateTime As DateTime) As String
        Return String.Format("{0}\{1}{2}{3}.{4}", LogDir, Prefix, dateTime.ToString(DateFormat), Suffix, Extension)
    End Function

    ''' <summary>
    ''' Проверка наличия файла журнала событий на указанную дату.
    ''' </summary>
    ''' <param name="dateTime">Дата файла журнала событий для проверки.</param>
    ''' <returns>При наличии файла за указанную дату возвращает True, иначе False.</returns>
    Public Function LogFileExists(ByVal dateTime As DateTime) As Boolean
        Return File.Exists(GetFileName(dateTime))
    End Function

    ''' <summary>
    ''' Получить текущий файл журнала событий в виде документа XML.
    ''' </summary>
    ''' <remarks>
    ''' При отсутствии текущего файла журнала событий не выдает исключений.
    ''' </remarks>
    ''' <returns>Возвращает текущий файл журнала событий как документ XML или Nothing, если таковой не существует.</returns>
    Public Function GetLogFileAsXml() As XDocument
        Return GetLogFileAsXml(DateTime.Now)
    End Function

    ''' <summary>
    ''' Получить файл журнала событий за указанную дату в виде XML документа.
    ''' </summary>
    ''' <remarks>
    ''' При отсутствии файла журнала событий не выдает исключений.
    ''' </remarks>
    ''' <param name="dateTime">Дата и время файла журнала. Используйте DateTime.Now, чтобы получить текущий файл журнала.</param>
    ''' <returns>Возвращает файл журнала как документ XML или Nothing, если таковой не существует.</returns>
    Public Function GetLogFileAsXml(ByVal dateTime As DateTime) As XDocument
        Dim fileName_ As String = GetFileName(dateTime)
        If Not File.Exists(fileName_) Then Return Nothing
        FlushMessage()
        Dim sb = New StringBuilder()
        sb.AppendLine("<?xml version=""1.0"" encoding=""utf-8""?>")
        sb.AppendLine("<LogEntries>")
        sb.AppendLine(File.ReadAllText(fileName_))
        sb.AppendLine("</LogEntries>")
        Return XDocument.Parse(sb.ToString())
    End Function

    ''' <summary>
    ''' Получить текущий файл журнала событий в виде текстового документа
    ''' </summary>
    ''' <remarks>
    ''' При отсутствии текущего файла журнала событий не выдаёт исключений.
    ''' </remarks>
    ''' <returns>Возвращает текущий файл журнала событий в виде текстового документа или Nothing, если он не существует.</returns>
    Public Function GetLogFileAsText() As String
        Return GetLogFileAsText(DateTime.Now)
    End Function

    ''' <summary>
    ''' Получить файл журнала событий за указанную дату в виде текстового документа.
    ''' </summary>
    ''' <remarks>
    ''' При отсутствии файла журнала событий не выдает исключений.
    ''' </remarks>
    ''' <param name="dateTime">Дата и время файла журнала. Используйте DateTime.Now, чтобы получить текущий файл журнала.</param>
    ''' <returns>Возвращает текущий файл журнала событий в виде текстового документа или Nothing, если он не существует.</returns>
    Public Function GetLogFileAsText(ByVal dateTime As DateTime) As String
        Dim fileName As String = GetFileName(dateTime)
        If Not File.Exists(fileName) Then Return Nothing
        FlushMessage()
        Return File.ReadAllText(fileName)
    End Function

    ''' <summary>
    ''' Открывает текущий файл журнала событий в подходящей программе.
    ''' </summary>
    ''' <remarks>
    ''' Открывает программу по умолчанию для отображения текстовых или XML-файлов и отображает запрошенный файл, если он существует. Иначе ничего не делает.
    ''' Если "WriteText" имеет значение False, временный XML-файл создается и сохраняется во временном пути пользователя всякий раз, когда вызывается данный метод.
    ''' Поэтому не используйте метод черезмерно.
    ''' </remarks>
    Public Sub ShowLogFile()
        ShowLogFile(DateTime.Now)
    End Sub

    ''' <summary>
    ''' Открывает файл журнала за прошедшую дату в подходящей программе.
    ''' </summary>
    ''' <remarks>
    ''' Открывает программу по умолчанию для отображения текстовых или XML-файлов и отображает запрошенный файл, если он существует. Иначе ничего не делает.
    ''' Если "WriteText" имеет значение False, временный XML-файл создается и сохраняется во временном пути пользователя всякий раз, когда вызывается данный метод.
    ''' Поэтому не используйте метод черезмерно.
    ''' </remarks>
    ''' <param name="dateTime">Дата и время для отображения файла журнала.</param>
    Public Sub ShowLogFile(ByVal dateTime As DateTime)
        Dim fileName As String

        If WriteText Then
            FlushMessage()
            fileName = GetFileName(dateTime)
        Else
            fileName = String.Format("{0}Log_{1}.xml", Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmssffff"))
            Dim logFileXml As XDocument = GetLogFileAsXml(dateTime)
            If logFileXml IsNot Nothing Then
                logFileXml.Save(fileName)
            End If
        End If

        If Not File.Exists(fileName) Then Return

        ' Запуск приложения связанного с открытием файла
        Process.Start(fileName)

        ' Техническое ожидание открытия приложения
        Thread.Sleep(2000)
    End Sub

    ''' <summary>
    ''' Ожидание процесса записи на диск всех сообщений из очереди.
    ''' </summary>
    Public Sub FlushMessage()
        ' Есть ли ещё элементы ожидающие запись на диск?
        Do While NumberOfLogEntriesWaitingToBeWrittenToFile > 0
            ' Запоминаем количество записей в очереди
            Dim lastNumber As Integer = NumberOfLogEntriesWaitingToBeWrittenToFile
            ' Даём время для выполнения задания записи
            Thread.Sleep(222)
            ' Количество записей осталось прежним? Тогда выходим из цикла, чтобы не зависнуть навечно...
            If lastNumber = NumberOfLogEntriesWaitingToBeWrittenToFile Then Exit Do
        Loop
    End Sub

    ''' <summary>
    ''' Очистка очереди записей журнала фоновых задач. Удаляет все сообщения журнала, ожидающие записи в "FileName" с помощью фоновой задачи.
    ''' </summary>
    Public Sub ClearQueue()
        SyncLock _logEntryQueue
            _logEntryQueue.Clear()
        End SyncLock
    End Sub

#End Region

#Region " === ПРИВАТНЫЕ МЕТОДЫ === "

    ''' <summary>
    ''' Основной процесс завершается.
    ''' </summary>
    ''' <remarks>
    ''' Это своего рода статический деструктор, используемый для сброса очереди записей журнала.
    ''' </remarks>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CurrentDomainProcessExit(ByVal sender As Object, ByVal e As EventArgs)
        ' Прекращаем постановку задач в очередь
        _stopWriteMessages = True

        ' Если не была запущена фоновая задача - нечего ждать
        If _backgroundTask Is Nothing Then Return

        ' Запись событий из очереди в файл
        FlushMessage()

        ' Самое время остановить фоновую задачу регистрации сообщений
        _stopLoggingRequested = True

        SyncLock _backgroundTaskSyncRoot
            If _backgroundTask Is Nothing Then
                Return
            End If

            ' Ждём секунду и устанавливаем задаче значение Nothing
            _backgroundTask.Wait(1000)
            _backgroundTask = Nothing
        End SyncLock
    End Sub

    ''' <summary>
    ''' Постановка нового сообщения в очередь для обработки.
    ''' </summary>
    ''' <param name="logEntry">Сообщение в формате XElement для постановки в очередь.</param>
    Private Sub Enqueue(ByVal logEntry As XElement)
        ' Если флаг выставлен то выходим
        If _stopWriteMessages Then Return

        SyncLock _logEntryQueue
            ' Прекращение постановки в очередь, если та переполнена
            If _logEntryQueue.Count < 10000 Then
                _logEntryQueue.Enqueue(logEntry)
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Получить следующую запись из очереди, но не удалять из таковой.
    ''' </summary>
    ''' <returns>Возвращает следующий элемент из очереди или Nothing, если очередь пуста.</returns>
    Private Function Peek() As XElement
        SyncLock _logEntryQueue
            Return If(_logEntryQueue.Count = 0, Nothing, _logEntryQueue.Peek())
        End SyncLock
    End Function

    ''' <summary>
    ''' Получение следующей записи из очереди с одновременным удалением.
    ''' </summary>
    Private Sub Dequeue()
        SyncLock _logEntryQueue
            If _logEntryQueue.Count > 0 Then
                _logEntryQueue.Dequeue()
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Записать сообщения в файл на диске.
    ''' Поток каждые 100 миллисекунд просматривает новые элементы в очереди.
    ''' </summary>
    Private Sub WriteLogEntriesToFile()
        Do While Not _stopLoggingRequested
            ' Получаем следующую запись журнала из очереди
            Dim xmlEntry As XElement = Peek()
            If xmlEntry Is Nothing Then
                ' Если очередь пуста, сделаем паузу и глянем позже
                Thread.Sleep(100)
                Continue Do
            End If

            ' Попытка десять раз записать запись в файл журнала. Между попытками ожидание, потому как файл может временно быть заблокирован другим приложением.
            ' Если после десяти попыток ничего не получилось, удаляем запись из очереди, то есть проще говоря - запись потеряна.
            ' Это необходимо для того, чтобы очередь не была переполнена и у нас не возникла ситуация с нехваткой памяти.
            For i As Integer = 0 To 9
                ' Собственно запись в файл журнала
                Dim ex As Exception = WriteLogEntryToFile(xmlEntry)
                WriteOwnExceptionToEventLog(ex)
                _lastExceptionInBackgroundTask = ex

                ' Или мы выполнили задачу, или уже срать, когда очередь переполнена
                If _lastExceptionInBackgroundTask Is Nothing OrElse NumberOfLogEntriesWaitingToBeWrittenToFile > 1000 Then
                    Exit For
                End If

                ' Ожидание
                Thread.Sleep(100)
            Next i

            Dequeue()
        Loop
    End Sub

    ''' <summary>
    ''' Отправляет исключения происходящие при попытке сохранить события в файл в стандартный журнал Windows.
    ''' При возникновении исключения в момент записи события в файл, бывает трудно понять суть проблемы.
    ''' Именно поэтому исключения связанные с данной проблемой отправляются в системный журнал Windows.
    ''' Событие записывается как ошибка в журнал приложения под источником "EasyLog".
    ''' </summary>
    ''' <param name="ex">Исключение для записи в журнал событий.</param>
    Private Sub WriteOwnExceptionToEventLog(ByVal ex As Exception)
        ' Фильтруем незначащие дабы не загромождать системный журнал
        If ex Is Nothing OrElse (_lastExceptionInBackgroundTask IsNot Nothing AndAlso ex.Message = _lastExceptionInBackgroundTask.Message) Then
            Return
        End If

        Try
            Const source As String = "EasyLog"
            Const logName As String = "Application"
            Dim message As String

            Try
                Dim xElement As XElement = GetExceptionXElement(ex)
                message = xElement.ToString()
            Catch
                message = ex.Message
            End Try

            If Not EventLog.SourceExists(source) Then
                EventLog.CreateEventSource(source, logName)
            End If

            EventLog.WriteEntry(source, message, EventLogEntryType.Error, 0)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Попытка записи одной строки в файл. Метод как из фонового потока так и напрямую из основного потока.
    ''' Блокировка соответственно, чтобы избежать нескольких потоков одновременно доступ к файлу.
    ''' Когда блокировка не может быть получена в течение пяти секунд, "xmlEntry" не записывается в файл и возвращается исключение.
    ''' </summary>
    ''' <param name="xmlEntry">XElement для записи в файл журнала событий.</param>
    Private Function WriteLogEntryToFile(ByVal xmlEntry As XElement) As Exception
        If xmlEntry Is Nothing Then Return Nothing

        Const secondsToWaitForFile As Integer = 5

        If Monitor.TryEnter(_logFileSyncRoot, New TimeSpan(0, 0, 0, secondsToWaitForFile)) Then
            Try
                If File.Exists(FileName) AndAlso New IO.FileInfo(FileName).Length + Len(ConvertXmlToPlainText(xmlEntry)) > MaxLogSize Then
                    Try
                        File.Copy(FileName, GetNewFileName, True)
                        File.Delete(FileName)
                    Catch
                    End Try
                End If
                ' Используем файловый поток, чтобы иметь возможность явно указать FileShare.None
                Using fileStream = New FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.None)
                    Using streamWriter = New StreamWriter(fileStream)
                        If WriteText Then
                            ' Записываем простой текст
                            streamWriter.WriteLine(ConvertXmlToPlainText(xmlEntry))
                        Else
                            ' Записываем XML
                            streamWriter.WriteLine(xmlEntry)
                        End If
                    End Using
                End Using
                Return Nothing
            Catch ex As Exception
                Try
                    ex.Data("Filename") = FileName
                Catch
                End Try

                Try
                    Dim user As WindowsIdentity = WindowsIdentity.GetCurrent()
                    ex.Data("Username") = If(user Is Nothing, _lng.log0101, user.Name)
                Catch
                End Try
                Return ex
            Finally
                Monitor.Exit(_logFileSyncRoot)
            End Try
        End If

        Try
            Return New Exception(String.Format(_lng.log0102, FileName, secondsToWaitForFile))
        Catch ex As Exception
            Return ex
        End Try
    End Function

    ''' <summary>
    ''' Преобразует "xmlEntry" в простой текст для записи в файл.
    ''' </summary>
    ''' <param name="xmlEntry">XML запись для преобразования.</param>
    ''' <returns></returns>
    Private Function ConvertXmlToPlainText(ByVal xmlEntry As XElement) As String
        Dim sb = New StringBuilder()

        For Each element In xmlEntry.DescendantsAndSelf()
            ' Если имеет атрибут
            If element.HasAttributes Then
                For Each attribute In element.Attributes()
                    If sb.Length > 0 Then
                        sb.Append(TextSeparator)
                    End If
                    ' Проверяем тип записи
                    If ExtendedLog Then
                        sb.Append(attribute.Name).Append(" = ").Append(attribute.Value)
                    Else
                        sb.Append(attribute.Value)
                    End If
                Next attribute
            Else
                ' иначе
                If sb.Length > 0 Then
                    sb.Append(TextSeparator)
                End If
                ' Удаляем новые строки, чтобы получить все в одну строку
                Dim value As String = element.Value.Replace(vbCrLf, " ")
                ' Проверяем тип записи
                If ExtendedLog Then
                    sb.Append(element.Name).Append(" = ").Append(value)
                Else
                    sb.Append(value)
                End If
            End If
        Next element

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Обнаруживает метод, который вызывал метод журнала событий.
    ''' </summary>
    ''' <remarks>
    ''' Метод перемещается вверх по кадрам в трассировке стека, пока не будет достигнут первый метод за пределами "EasyLog".
    ''' </remarks>
    ''' <param name="framesToSkip">Сколько кадров пропустить при обнаружении вызывающего метода.</param>
    ''' <returns>Возвращает класс и метод, который вызывал исключение.</returns>
    Private Function GetCaller(Optional ByVal framesToSkip As Integer = 0) As String
        Dim result As String = String.Empty

        Dim i As Integer = 1

        Do
            ' Идём вверх по стеку...
            Dim stackFrame = New StackFrame(i)
            i += 1
            Dim methodBase As MethodBase = stackFrame.GetMethod()
            If methodBase Is Nothing Then
                Exit Do
            End If

            ' Здесь мы в конце - формально мы никогда не должны заходить так далеко 
            Dim declaringType As Type = methodBase.DeclaringType
            If declaringType Is Nothing Then
                Exit Do
            End If

            ' Получаем имя класса и метод текущего фрейма стека
            result = String.Format("{0}.{1}", declaringType.FullName, methodBase.Name)

            ' Здесь мы находимся у первого метода вне класса EasyLog.
            framesToSkip -= 1
            If declaringType IsNot GetType(EasyLog) AndAlso methodBase.Name <> ".ctor" AndAlso methodBase.Name <> ".cctor" AndAlso framesToSkip < 0 Then
                Exit Do
            End If
        Loop

        Return result
    End Function

    ''' <summary>
    ''' Тестовая часть
    ''' </summary>
    ''' <param name="message"></param>
    ''' <returns></returns>
    Private Function WriteTestMessage(ByVal message As String) As Exception
        If String.IsNullOrEmpty(message) Then Return Nothing
        Return WriteXElement(New XElement("Message", message), False, 0, False, 0)
    End Function

    ''' <summary>
    ''' Записывает сообщение в журнал событий.
    ''' </summary>
    ''' <param name="message">Текст сообщения для записи в журнал событий.</param>
    ''' <param name="severity">Уровень серъёзности произошедшего события.</param>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    ''' <param name="framesToSkip">Сколько кадров пропустить при обнаружении вызывающего метода "GetCaller".
    ''' Это полезно, когда вызовы журнала для "EasyLog" помещаются в приложение. По умолчанию = 0.</param>
    Private Function WriteMessage(ByVal message As String,
                                  Optional ByVal severity As Severity = Severity.Info,
                                  Optional ByVal useBackgroundTask As Boolean = True,
                                  Optional ByVal framesToSkip As Integer = 0) As Exception
        If String.IsNullOrEmpty(message) Then Return Nothing
        Return WriteXElement(New XElement("Message", message), True, severity, useBackgroundTask, framesToSkip)
    End Function

    ''' <summary>
    ''' Записывает XElement в журнал событий и при ошибке возвращает Exception.
    ''' </summary>
    ''' <param name="xElement">xElement для записи в журнал событий.</param>
    ''' <param name="severity">Уровень серъёзности произошедшего события.</param>
    ''' <param name="useBackgroundTask">Использовать ли фоновую задачу (поток) для записи сообщений на диск. По умолчанию = True.
    ''' Это намного быстрее, чем запись непосредственно на диск в основном потоке.</param>
    ''' <param name="framesToSkip">Сколько кадров пропустить при обнаружении вызывающего метода "GetCaller".
    ''' Это полезно, когда вызовы журнала для "EasyLog" помещаются в приложение. По умолчанию = 0.</param>
    Private Function WriteXElement(ByVal xElement As XElement,
                              Optional ByVal useSeverity As Boolean = True,
                              Optional ByVal severity As Severity = Severity.Info,
                              Optional ByVal useBackgroundTask As Boolean = True,
                              Optional ByVal framesToSkip As Integer = 0) As Exception

        If useSeverity Then
            ' Фильтровать записи ниже установленного уровня серъёзности и согласно параметру
            If xElement Is Nothing OrElse severity < LogLevel OrElse _stopWriteMessages Then Return Nothing
        End If

        Try

            Dim logEntry = New XElement("LogEntry")

            ' Собираем элемент сообщения для записи
            If ExtendedLog Then
                logEntry.Add(New XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")))
                logEntry.Add(New XAttribute("Severity", severity))
                logEntry.Add(New XAttribute("Source", GetCaller(framesToSkip)))
                logEntry.Add(New XAttribute("ThreadId", Thread.CurrentThread.ManagedThreadId))
                logEntry.Add(xElement)
            Else
                logEntry.Add(New XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")))
                logEntry.Add(xElement)
            End If

            If useBackgroundTask Then
                ' Поставить запись в журнал для записи в файл с помощью фоновой задачи
                Enqueue(logEntry)
            Else
                ' Прямая запись в файл. Это синхронизируется между потоками в методе
                Return WriteLogEntryToFile(logEntry)
            End If

            Return Nothing

        Catch ex As Exception
            Return ex
        End Try

    End Function

    ''' <summary>
    ''' Получает имя файла журнала для указанной даты.
    ''' </summary>
    ''' <returns>Имя файла журнала событий для указанной даты.</returns>
    Private Function GetNewFileName() As String
        Static num As Integer
        num += 1
        Return String.Format("{0}\{1}{2}{3}.{4}.{5}", LogDir, Prefix, Now.ToString(DateFormat), Suffix, num.ToString, Extension)
    End Function

#End Region

#Region " === ЛОКАЛИЗАЦИЯ === "

    ''' <summary>
    ''' Идентификатор совместимости для локализации класса EasyLog
    ''' </summary>
    Public Const LocalID As String = "8f6d227b-c461-419e-88ae-11ccabebfffe"

    ''' <summary>
    ''' Структура содержащая текущую локализацию.
    ''' </summary>
    Private _lng As TLanguage

#Region " === БАЗОВАЯ СТРУКТУРА СТРОК ЛОКАЛИЗАЦИИ === "

    ''' <summary>
    ''' Структура содержащая строки локализации класса EasyLog.
    ''' </summary>
    Private Structure TLanguage

        ''' <summary>
        ''' Короткое обозначение языка локализации.
        ''' </summary>
        Public Language As String

        ''' <summary>
        ''' Автор локализации.
        ''' </summary>
        Public Author As String

        ''' <summary>
        ''' ОК
        ''' </summary>
        Public log0000 As String

        ''' <summary>
        ''' Ошибка:
        ''' </summary>
        Public log0001 As String

        ''' <summary>
        ''' Ошибка загрузки файла локализации.
        ''' </summary>
        Public log0002 As String

        ''' <summary>
        ''' Файл локализации '{0}' не существует.
        ''' </summary>
        Public log0003 As String

        ''' <summary>
        ''' Указанный файл '{0}' не предназначен для локализации EasyLog.
        ''' </summary>
        Public log0004 As String

        ''' <summary>
        ''' В файле локализации не найдено поле идентификатора. Загрузка отменена.
        ''' </summary>
        Public log0005 As String

        ''' <summary>
        ''' В файле локализации отсутствует ключ {0}.
        ''' </summary>
        Public log0006 As String

        ''' <summary>
        ''' Локализация выполнена. Текущий язык:
        ''' </summary>
        Public log0007 As String

        ''' <summary>
        ''' Локализация успешно сохранена в файл:
        ''' </summary>
        Public log0008 As String

        ''' <summary>
        ''' Уровень журнала регистраций:
        ''' </summary>
        Public log0009 As String


        ''' <summary>
        ''' Каталог '{0}' не существует!
        ''' </summary>
        Public log0100 As String

        ''' <summary>
        ''' неизвестный
        ''' </summary>
        Public log0101 As String

        ''' <summary>
        ''' Не удалось сохранить в файл '{0}', потому как он был заблокирован более {1} сек.
        ''' </summary>
        Public log0102 As String

        ''' <summary>
        ''' ===| ПЕРВАЯ ЗАПИСЬ ПЕРЕД НАЧАЛОМ НОВОГО ЖУРНАЛА СОБЫТИЙ |===
        ''' </summary>
        Public log0103 As String

        ''' <summary>
        ''' Ошибка инициализации EasyLog.
        ''' </summary>
        Public log0104 As String

    End Structure

#End Region

    ''' <summary>
    ''' Возвращает текущую локализацию.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CurrentLanguage As String
        Get
            Return _lng.Language
        End Get
    End Property

    ''' <summary>
    ''' Автор локализации.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LocalizationAuthor As String
        Get
            Return _lng.Author
        End Get
    End Property

    ''' <summary>
    ''' Возвращает локализованную коллекцию строковых ключей и их значений.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LocalizedPairs As Dictionary(Of String, String)
        Get
            Dim _lng As New Dictionary(Of String, String)

            Return _lng
        End Get
    End Property

    ''' <summary>
    ''' Устанавливает локализацию для класса EasyLog из встроенных в класс (RU, EN).
    ''' </summary>
    ''' <param name="language">Доступная локализация из коробки. В данном случае RU или EN.</param>
    Public Sub SetLanguage(ByVal language As String)
        If IsNothing(language) Then language = "ru-RU"
        Select Case UCase(language)
            Case "RU-RU"
                ' Локализация локализации :)
                _lng.Language = "ru-RU"
                _lng.Author = "© 2020 RafStudio inc."
                _lng.log0000 = "ОK."
                _lng.log0001 = "Ошибка: "
                _lng.log0002 = "Ошибка загрузки файла локализации EasyLog."
                _lng.log0003 = "Файл локализации '{0}' не существует."
                _lng.log0004 = "Указанный файл '{0}' не предназначен для локализации EasyLog."
                _lng.log0005 = "В файле локализации не найдено поле идентификатора. Загрузка отменена."
                _lng.log0006 = "В файле локализации отсутствует ключ '{0}'. Загрузка отменена."
                _lng.log0007 = "Локализация модуля EasyLog выполнена. Текущий язык: "
                _lng.log0008 = "Локализация модуля EasyLog успешно сохранена в файл: "
                _lng.log0009 = "Уровень журнала регистраций: "
                ' Далее локализация непосредственно EasyLog
                _lng.log0100 = "Каталог '{0}' не существует!"
                _lng.log0101 = "неизвестный"
                _lng.log0102 = "Не удалось сохранить в файл '{0}', потому как он был заблокирован более {1} сек."
                _lng.log0103 = "===| ПЕРВАЯ ЗАПИСЬ ПЕРЕД НАЧАЛОМ ВЕДЕНИЯ ЖУРНАЛА СОБЫТИЙ |==="
                _lng.log0104 = "Ошибка инициализации EasyLog."

            Case Else
                ' Локализация локализации :)
                _lng.Language = "en-GB"
                _lng.Author = "© 2020 RafStudio inc."
                _lng.log0000 = "ОK."
                _lng.log0001 = "Error: "
                _lng.log0002 = "Error loading EasyLog localization file."
                _lng.log0003 = "The localization file '{0}' does not exist."
                _lng.log0004 = "The specified file '{0}' is not intended for localization of EasyLog."
                _lng.log0005 = "No identifier field was found in the localization file. Download canceled."
                _lng.log0006 = "The key '{0}' is missing in the localization file. Download canceled."
                _lng.log0007 = "Localization completed. Current language: "
                _lng.log0008 = "The localization was successfully saved to a file: "
                _lng.log0009 = "Log level: "
                ' Далее локализация непосредственно EasyLog
                _lng.log0100 = "Directory '{0}' does not exist!"
                _lng.log0101 = "unknown"
                _lng.log0102 = "Could not write to file '{0}', because it was blocked by another thread for more than {1} sec."
                _lng.log0103 = "===| FIRST ENTRY BEFORE BEGINNING EVENT LOG |==="
                _lng.log0104 = "EasyLog initialization error."

        End Select

        ' Выдаём в журнал сообщение о успешной смене локализации
        WriteInfo(_lng.log0007 & _lng.Language)

    End Sub

    ''' <summary>
    ''' Загружает и устанавливает файл локализации для класса EasyLog.
    ''' </summary>
    ''' <param name="fileName">Имя загружаемого файла локализации. Следует понимать - имя расширения метод подставит сам в зависимости от параметра textFormat.
    ''' Это будет либо InilanguageFile с расширением .ilf либо XmlLanguageFile с расширением .xlf - другого не дано, учитывая LocalID.</param>
    ''' <param name="textFormat">Формат загружаемого файла. False - загрузить файл локализации в формате xml.</param>
    ''' <param name="ignoreMissingKeys">Позволяет игнорировать отсутствующие в файле строки локализации.</param>
    ''' <returns></returns>
    Public Function LoadLanguage(ByVal fileName As String,
                                 ByVal textFormat As Boolean,
                                 Optional ByVal ignoreMissingKeys As Boolean = True) As Exception
        Try
            If String.IsNullOrEmpty(fileName) Then Throw New Exception(String.Format(_lng.log0003, "''"))

            ' Суть расширений файла проста - IniLanguageFile или XmlLanguageFile
            Dim file = IO.Path.GetFileNameWithoutExtension(fileName)
            If textFormat Then file &= ".ilf" Else file &= ".xlf"

            If Not IO.File.Exists(file) Then Throw New Exception(String.Format(_lng.log0003, file))

            Dim tStruct As ValueType = New TLanguage

            If textFormat Then

                ' Грузим текстовый файл локализации
                Dim idOK As Boolean = False
                Dim lines() As String = IO.File.ReadAllLines(file)
                Dim dlang As New Dictionary(Of String, String)

                ' Обрабатываем файл
                For Each line In lines
                    Dim name As String
                    Dim value As String
                    Dim Equals As Integer = InStr(line, "=", CompareMethod.Text)
                    If Equals > 0 Then
                        name = Left(line, Equals - 1)
                        value = Right(line, Len(line) - Equals)
                        Select Case UCase(name)
                            Case "LOCALID"
                                If UCase(value) <> UCase(LocalID) Then Throw New Exception(String.Format(_lng.log0004, fileName))
                                idOK = True
                            Case Else
                                dlang.Add(name, value)
                        End Select
                    End If
                Next

                If Not idOK Then Throw New Exception(_lng.log0005)

                ' Проверяем наличие обязательных ключей локализации из загруженного файла
                For Each field In _lng.GetType.GetFields()
                    Dim value As String = Nothing
                    dlang.TryGetValue(field.Name, value)
                    ' Если какой-то ключ не найден - ошибка
                    If IsNothing(value) Then
                        If Not ignoreMissingKeys Then Throw New Exception(String.Format(_lng.log0006, field.Name))
                    Else
                        field.SetValue(tStruct, value)
                    End If
                Next

            Else

                ' Грузим xml файл локализации
                Dim XDOC As New XmlDocument
                XDOC.Load(file)

                If Not IsNothing(XDOC) Then
                    If XDOC.DocumentElement.GetAttribute("LocalID") = "8f6d227b-c461-419e-88ae-11ccabebfffe" Then
                        Dim XNode As XmlNode
                        ' Проверяем наличие обязательных ключей локализации из загруженного файла
                        For Each field In _lng.GetType.GetFields()
                            XNode = XDOC.SelectSingleNode("CtrlDoc/Params/Param/" & field.Name)
                            ' Если какой-то ключ не найден - ошибка
                            If IsNothing(XNode) Then
                                If Not ignoreMissingKeys Then Throw New Exception(String.Format(_lng.log0006, field.Name))
                            Else
                                field.SetValue(tStruct, XNode.InnerText)
                            End If
                        Next
                    Else
                        Throw New Exception(String.Format(_lng.log0004, fileName))
                    End If
                End If

            End If

            ' Заменяем локализацию
            _lng = CType(tStruct, TLanguage)

            ' Выдаём в журнал сообщение о смене локализации
            WriteInfo(_lng.log0007 & _lng.Language)

            Return Nothing

        Catch ex As Exception
            WriteWarning(ex.Message)
            Return ex
        End Try
    End Function

    ''' <summary>
    ''' Сохраняет текущую локализацию класса EasyLog в файл указанного формата.
    ''' </summary>
    ''' <param name="fileName">Имя сохраняемого файла локализации. Следует понимать - имя расширения метод подставит сам в зависимости от параметра textFormat.</param>
    ''' <param name="description">Описание файла локализации.</param>
    ''' <param name="textFormat">Формат сохраняемого файла. False - сохранить файл локализации в формате xml.
    ''' Исходя из этого параметра мы получаем имя сохраняемого файла с расширением или ilf (IniLanguageFile) или xlf (XmlLanguageFile)</param>
    ''' <returns></returns>
    Public Function SaveCurrentLanguage(ByVal fileName As String,
                                        ByVal description As String,
                                        ByVal textFormat As Boolean) As Exception

        ' Суть расширений файла проста - IniLanguageFile или XmlLanguageFile
        Dim file = IO.Path.GetFileNameWithoutExtension(fileName)
        If textFormat Then file &= ".ilf" Else file &= ".xlf"

        Try
            If textFormat Then

                Dim sb As New StringBuilder
                sb.Append(CChar("#"), description.Length + 4)
                sb.AppendLine()
                sb.AppendLine("# " & description & " #")
                sb.Append(CChar("#"), description.Length + 4)
                sb.AppendLine()
                sb.AppendLine()
                sb.AppendLine("LocalID=" & LocalID)
                For Each field In _lng.GetType.GetFields()
                    sb.AppendLine(field.Name & "=" & field.GetValue(_lng).ToString)
                Next


                IO.File.WriteAllText(file, sb.ToString)

            Else
                Dim XDOC As New XDocument With {
                .Declaration = New XDeclaration("1.0", "utg-8", "True")
            }
                Dim CtrlDoc = New XElement("CtrlDoc")
                CtrlDoc.Add(New XAttribute("Version", "1.0"))
                CtrlDoc.Add(New XAttribute("Product", "EasyLog"))
                CtrlDoc.Add(New XAttribute("LocalID", LocalID))
                CtrlDoc.Add(New XAttribute("Description", description))
                Dim Params As New XElement("Params")
                Dim Param As XElement = Nothing
                For Each field In _lng.GetType.GetFields()
                    Param = New XElement("Param")
                    Param.Add(New XElement(field.Name, field.GetValue(_lng)))
                    Params.Add(Param)
                Next
                CtrlDoc.Add(Params)
                XDOC.Add(CtrlDoc)
                XDOC.Save(file)
            End If

            ' Выдаём в журнал сообщение об успешном сохранении локализации
            WriteInfo(_lng.log0008 & file)

            Return Nothing

        Catch ex As Exception
            WriteWarning(ex.Message)
            Return ex
        End Try
    End Function

#End Region

End Class

