
Imports MySql.Data.MySqlClient

Public Class MySqlClient

    Private ReadOnly conn As MySqlConnection

#Region " === КОНСТРУКТОРЫ ИНИЦИАЛИЗАЦИИ === "

    Public Sub New(ByVal hostname As String, ByVal database As String, ByVal username As String, ByVal password As String)
        conn = New MySqlConnection("host=" & hostname & ";database=" & database & ";username=" & username & ";password=" & password & ";")
    End Sub

    Public Sub New(ByVal hostname As String, ByVal database As String, ByVal username As String, ByVal password As String, ByVal portNumber As Integer)
        conn = New MySqlConnection("host=" & hostname & ";database=" & database & ";username=" & username & ";password=" & password & ";port=" & portNumber & ";")
    End Sub

    Public Sub New(ByVal hostname As String, ByVal database As String, ByVal username As String, ByVal password As String, ByVal portNumber As Integer, ByVal connectionTimeout As Integer)
        conn = New MySqlConnection("host=" & hostname & ";database=" & database & ";username=" & username & ";password=" & password & ";port=" & portNumber & ";Connection Timeout=" & connectionTimeout & ";")
    End Sub

#End Region

#Region " === ОТКРЫТИЕ И ЗАКРЫТИЕ СОЕДИНЕНИЯ === "

    ''' <summary>
    ''' Открывает соединение. При ошибке возвращает True.
    ''' </summary>
    ''' <returns></returns>
    Private Function Open() As Boolean
        Try
            conn.Open()
            Return False
        Catch
            Return True
        End Try
    End Function

    ''' <summary>
    ''' Закрывает соединение. При ошибке возвращает True.
    ''' </summary>
    ''' <returns></returns>
    Private Function Close() As Boolean
        Try
            conn.Close()
            Return False
        Catch
            Return True
        End Try
    End Function

#End Region

    Public Sub Insert(ByVal table As String, ByVal column As String, ByVal value As String)
        Dim query As String = "INSERT INTO " & table & " (" & column & ") VALUES (" & value & ")"
        Try
            If Me.Open() Then
                Dim cmd As New MySqlCommand(query, conn)
                cmd.ExecuteNonQuery()
                Me.Close()
            End If
        Catch
        End Try
    End Sub

    Public Sub Delete(ByVal table As String, ByVal WHERE As String)
        Dim query As String = "DELETE FROM " & table & " WHERE " & WHERE & ""

        If Me.Open() Then
            Try
                Dim cmd As New MySqlCommand(query, conn)
                cmd.ExecuteNonQuery()
                Me.Close()
            Catch
                Me.Close()
            End Try
        End If
    End Sub

    Public Function [Select](ByVal table As String, ByVal WHERE As String) As Dictionary(Of String, String)
        Dim query As String = "SELECT * FROM " & table & " WHERE " & WHERE & ""

        Dim selectResult As New Dictionary(Of String, String)()

        If Me.Open() Then
            Dim cmd As New MySqlCommand(query, conn)
            Dim dataReader As MySqlDataReader = cmd.ExecuteReader()

            Try
                Do While dataReader.Read()
                    For i As Integer = 0 To dataReader.FieldCount - 1
                        selectResult.Add(dataReader.GetName(i), dataReader.GetValue(i).ToString())
                    Next i
                Loop
                dataReader.Close()
            Catch
            End Try
            Me.Close()

            Return selectResult
        End If
        Return selectResult
    End Function

    Public Function Count(ByVal table As String) As Integer
        Dim query As String = "SELECT Count(*) FROM " & table & ""
        Dim _count As Integer = -1
        If Me.Open() Then
            Try
                Dim cmd As New MySqlCommand(query, conn)
                _count = Integer.Parse(cmd.ExecuteScalar().ToString)
                Me.Close()
            Catch
                Me.Close()
            End Try
            Return _count
        End If
        Return _count
    End Function

End Class
