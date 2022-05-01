
''' <summary>
''' Класс контролируемых процессов.
''' </summary>
Public Class ProcessController

    ''' <summary>
    ''' Список контролируемых процессов.
    ''' </summary>
    Public Processes As List(Of BaseProcess)

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    Sub New()
        Processes = New List(Of BaseProcess)
    End Sub

    ''' <summary>
    ''' Добавляет новый процесс для контроля.
    ''' </summary>
    ''' <param name="name"></param>
    Sub AddProcess(name As GV.EProcess)
        Processes.Add(New BaseProcess(name))
    End Sub

    ''' <summary>
    ''' Процесс был запущен.
    ''' </summary>
    ''' <param name="name"></param>
    Sub ProcessStarted(name As GV.EProcess)
        Dim n As String = ""
        Select Case name
            Case GV.EProcess.Realmd
                n = "realmd"
                Processes.Find(Function(p) p.Name = n).WasLaunched = True
            Case GV.EProcess.World
                n = "mangosd"
                Processes.Find(Function(p) p.Name = n).WasLaunched = True
        End Select
    End Sub

End Class

''' <summary>
''' Базовый элемент класса контроля процессов.
''' </summary>
Public Class BaseProcess

    Public Name As String
    Public WasLaunched As Boolean

    ''' <summary>
    ''' Конструктор иницилизации.
    ''' </summary>
    ''' <param name="name">Имя процесса.</param>
    Sub New(name As GV.EProcess)
        Select Case name
            Case GV.EProcess.Realmd
                Me.Name = "realmd"
            Case GV.EProcess.World
                Me.Name = "mangosd"
        End Select
    End Sub

End Class
