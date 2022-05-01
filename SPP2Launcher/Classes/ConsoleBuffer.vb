
Public Class ConsoleBuffer

    ''' <summary>
    ''' Список последних команд.
    ''' </summary>
    Public LastCommand As List(Of String)

    ''' <summary>
    ''' Расположение в списке команд.
    ''' </summary>
    Private Point As Integer

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    Sub New()
        LastCommand = New List(Of String) From {""}
    End Sub

    ''' <summary>
    ''' Добавляет команду в список.
    ''' </summary>
    ''' <param name="text"></param>
    Public Sub Add(text As String)
        If LastCommand.Count > 100 Then LastCommand.Remove(LastCommand.Item(0))
        LastCommand.Add(text)
    End Sub

    ''' <summary>
    ''' Получает команду вверх по стеку.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetUp() As String
        If LastCommand.Count = 0 Then Return Nothing
        Point -= 1
        If Point < 0 Then Point = LastCommand.Count - 1
        Return LastCommand.Item(Point)
    End Function

    ''' <summary>
    ''' Получает команду вниз по стеку.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDown() As String
        If LastCommand.Count = 0 Then Return Nothing
        Point += 1
        If Point > LastCommand.Count - 1 Then Point = 0
        Return LastCommand.Item(Point)
    End Function

End Class
