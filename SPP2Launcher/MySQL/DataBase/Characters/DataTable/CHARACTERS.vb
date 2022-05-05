
Imports MySql.Data.MySqlClient
Imports DevCake.WoW.SPP2Launcher.MySqlProvider

Namespace MySqlDataBases.CHARACTERS

    ''' <summary>
    ''' Таблица Caracters базы Characters
    ''' </summary>
    Public Class CHARACTERS

        ''' <summary>
        ''' Возвращает общее количество чаров в базе данных.
        ''' </summary>
        ''' <returns></returns>
        Shared Function SELECT_TOTAL_CHARS() As String
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
        End Function

        ''' <summary>
        ''' Возвращает количество чаров в онлайн.
        ''' </summary>
        ''' <returns></returns>
        Shared Function SELECT_ONLINE_CHARS() As String
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
        End Function

    End Class

End Namespace
