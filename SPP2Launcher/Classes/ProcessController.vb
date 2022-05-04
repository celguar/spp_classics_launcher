
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
    ''' Попытка запуска процесса.
    ''' </summary>
    ''' <param name="name"></param>
    Sub WasLaunched(name As GV.EProcess)
        Dim n As String = ""
        Dim pc As BaseProcess
        Select Case name
            Case GV.EProcess.Realmd
                n = "realmd"
                pc = Processes.Find(Function(p) p.Name = n)
            Case GV.EProcess.World
                n = "mangosd"
                Processes.Find(Function(p) p.Name = n).WasLaunched = True
        End Select
    End Sub

    ''' <summary>
    ''' Все процессы остановлены.
    ''' </summary>
    Sub ProcessesAreStopped()
        For Each p In Processes
            p.WasLaunched = False
        Next
    End Sub

    ''' <summary>
    ''' Процесс упал.
    ''' </summary>
    ''' <param name="name"></param>
    Sub ProcessCrashed(name As GV.EProcess)
        Dim n As String = ""
        Select Case name
            Case GV.EProcess.Realmd
                n = "realmd"
                Processes.Find(Function(p) p.Name = n).CrashCount = +1
            Case GV.EProcess.World
                n = "mangosd"
                Processes.Find(Function(p) p.Name = n).CrashCount += 1
        End Select
    End Sub

End Class

''' <summary>
''' Базовый элемент класса контроля процессов.
''' </summary>
Public Class BaseProcess

    ''' <summary>
    ''' Имя процесса
    ''' </summary>
    Public Name As String

    ''' <summary>
    ''' Была ли попытка запуска?
    ''' </summary>
    Public WasLaunched As Boolean

    ''' <summary>
    ''' Количество падений процесса.
    ''' </summary>
    Public CrashCount As Integer

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
