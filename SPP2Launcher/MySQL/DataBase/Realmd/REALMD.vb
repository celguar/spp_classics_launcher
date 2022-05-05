
Imports MySql.Data.MySqlClient
Imports DevCake.WoW.SPP2Launcher.MySqlProvider

Namespace MySqlDataBases

    ''' <summary>
    ''' Команда BackUp для базы данных Realmd.
    ''' </summary>
    Public Class Backup

        ''' <summary>
        ''' Сохраняет базу данных Realmd.
        ''' </summary>
        Shared Function REALMD() As String
            Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                Using sqlComm As New MySqlCommand
                    Try
                        GV.Log.WriteSQL(String.Format(My.Resources.P022_BackupStart, "REALMD"))
                        sqlConn.Open()
                        Dim mb As New MySqlBackup(sqlComm)
                        Dim file = String.Format("{0}_{1}-{2}-{3}--{4}-{5}.{6}", "C:\TEMP\realmd", Year(Now), Month(Now), Today, Hour(Now), Minute(Now), "sql")
                        mb.ExportToFile(file)
                        GV.Log.WriteSQL(My.Resources.P024_Success)
                        Return ""
                    Catch ex As Exception
                        GV.Log.WriteSQLException(ex)
                        Return My.Resources.E017_SeeLog
                    End Try
                End Using
            End Using
        End Function

    End Class

End Namespace
