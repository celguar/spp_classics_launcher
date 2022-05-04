
Imports MySql.Data.MySqlClient
Imports DevCake.WoW.SPP2Launcher.MySqlProvider

Namespace MySqlTables

    ''' <summary>
    ''' Таблица Command базы данных Mangos.
    ''' </summary>
    Public Class COMMAND

        ''' <summary>
        ''' Возвращает список строк автоподсказок.
        ''' </summary>
        Shared Sub SELECT_COMMAND(ByRef source As AutoCompleteStringCollection)
            Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbWorld))
                Using sqlComm As New MySqlCommand
                    With sqlComm
                        .Connection = sqlConn
                        .CommandType = CommandType.Text
                        .CommandText = "SELECT name FROM command"
                    End With
                    Try
                        GV.Log.WriteSQL(sqlComm.CommandText)
                        sqlConn.Open()
                        Dim reader = sqlComm.ExecuteReader()
                        While reader.Read()
                            source.Add(reader("name").ToString)
                        End While
                        GV.Log.WriteSQL("All Hints = " & source.Count)
                    Catch ex As Exception
                        GV.Log.WriteSQLException(ex)
                        source = New AutoCompleteStringCollection
                    End Try
                End Using
            End Using
        End Sub

    End Class

End Namespace
