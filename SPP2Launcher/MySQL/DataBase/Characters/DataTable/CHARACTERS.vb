
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDB.CHARACTERS

    ''' <summary>
    ''' Таблица Caracters базы Characters
    ''' </summary>
    Public Class ACCOUNT

        ''' <summary>
        ''' Возвращает общее количество чаров в базе данных.
        ''' </summary>
        ''' <returns></returns>
        Shared Function SELECT_TOTAL_CHARS() As String
            If CheckProcess(EProcess.mysqld) Then
                Dim count As String = ""
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbCharacters))
                    Using sqlComm As New MySqlCommand
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT COUNT(*) FROM characters"
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            count = sqlComm.ExecuteScalar().ToString
                            GV.Log.WriteSQL("All Chars = " & count)
                            Return count
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return "err"
                        End Try
                    End Using
                End Using
            Else
                Return "N/A"
            End If
        End Function

        ''' <summary>
        ''' Возвращает количество чаров в онлайн.
        ''' </summary>
        ''' <returns></returns>
        Shared Function SELECT_ONLINE_CHARS() As String
            If CheckProcess(EProcess.mysqld) And GV.SPP2Launcher.WorldON Then
                Dim count As String = ""
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbCharacters))
                    Using sqlComm As New MySqlCommand
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT SUM(online) FROM characters"
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            count = sqlComm.ExecuteScalar().ToString
                            GV.Log.WriteSQL("Online Chars = " & count)
                            Return count
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return "err"
                        End Try
                    End Using
                End Using
            Else
                Return "N/A"
            End If
        End Function

    End Class

End Namespace
