
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDB.PLAYERBOTS

    Public Enum ECommandRemoveBots

        ''' <summary>
        ''' Удалить исключая связанных с реальными игроками или гильдиями.
        ''' </summary>
        Exclude = 1

        ''' <summary>
        ''' Удалить всех без исключения.
        ''' </summary>
        All = 2

    End Enum

    Public Class AI_PLAYERBOT_RANDOM_BOTS

        ''' <summary>
        ''' Добавляет в БД команду удаления ботов.
        ''' </summary>
        ''' <param name="command">Тип команды удаления ботов - всех или исключая связанных с игроками и реальными гильдиями..</param>
        ''' <returns></returns>
        Shared Function INSERT_COMMAND_RESET(command As ECommandRemoveBots) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbPlayerbots))
                    Using sqlComm As New MySqlCommand
                        Try
                            Dim cmd As String = ""
                            cmd &= "INSERT INTO ai_playerbot_random_bots "
                            cmd &= "(owner, bot, time, event, value) "
                            cmd &= "VALUES ('0', '0', '0', 'bot_delete', @Command)"
                            With sqlComm
                                .Connection = sqlConn
                                .CommandType = CommandType.Text
                                .CommandText = cmd
                                .Parameters.AddWithValue("@Command", command)
                            End With
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Command inserted {0}.", "bot_delete = " & command))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

        ''' <summary>
        ''' Удаляет из БД команду удаления ботов.
        ''' </summary>
        ''' <returns></returns>
        Shared Function DELETE_COMMAND_RESET() As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Dim accounts As New List(Of String)
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbPlayerbots))
                    Using sqlComm As New MySqlCommand
                        Dim cmd As String = ""
                        cmd &= "DELETE FROM ai_playerbot_random_bots "
                        cmd &= "WHERE event = 'bot_delete'"
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = cmd
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Command {0} removed.", "bot_delete"))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

    End Class

End Namespace